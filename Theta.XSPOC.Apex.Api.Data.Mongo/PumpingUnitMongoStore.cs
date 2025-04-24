using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset.RodPump;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using Applications = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Enums.Applications;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// Represents a Mongo store for PumpingUnit entities.
    /// </summary>
    public class PumpingUnitMongoStore : MongoOperations, IPumpingUnit
    {

        #region Private Constants

        private const string LOOKUP_COLLECTION = "Lookup";
        private const string ASSET_COLLECTION = "Asset";

        #endregion

        #region Private Fields

        private readonly IMongoDatabase _database;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="PumpingUnitMongoStore"/>. 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public PumpingUnitMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory)
            : base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IPumpingUnit Implementation

        /// <summary>
        /// Gets the unit names for the specified node Ids.
        /// </summary>
        /// <param name="nodeIds">The list of node Ids.</param>
        /// <param name="correlationId"></param>
        /// <returns>The list of PumpingUnitForUnitNameModel objects.</returns>
        public IList<PumpingUnitForUnitNameModel> GetUnitNames(IList<string> nodeIds, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(PumpingUnitMongoStore)} {nameof(GetUnitNames)}", correlationId);

            var pumpingUnits = new List<PumpingUnitForUnitNameModel>();

            try
            {
                var stringNodeId = nodeIds.Select(x => x.ToString()).ToArray();
                var assetFilter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
                    .Where(x => (x.LegacyId.ContainsKey("NodeId") && x.LegacyId["NodeId"] != null &&
                                 stringNodeId.Contains(x.LegacyId["NodeId"]) && x.ArtificialLiftType == Applications.RodLift.ToString()));

                var assetData = Find<MongoAssetCollection.Asset>(ASSET_COLLECTION, assetFilter, correlationId);
                var assetDataList = assetData?.ToList();

                var assetPumpingUnits = assetDataList?.Select(a => ((RodPumpDetail)a.AssetDetails)?.PumpingUnit)?.ToList();
                if (assetPumpingUnits != null && assetPumpingUnits.Count > 0)
                {
                    var pumpingUnitIds = assetPumpingUnits.Where(x => x != null && x.LookupDocument != null
                    && x.LookupType == LookupTypes.PumpingUnit.ToString())
                        .Select(x => ((PumpingUnit)x.LookupDocument)?.UnitId.ToString())
                        .Distinct().ToArray();
                    var lookupFilter = new FilterDefinitionBuilder<Lookup>()
                       .Where(x => x.LookupType == LookupTypes.PumpingUnit.ToString()
                       && pumpingUnitIds.Contains(x.LegacyId["UnitId"]));

                    var lookupPumpingUnits = Find<Lookup>(LOOKUP_COLLECTION, lookupFilter, correlationId);
                    if (lookupPumpingUnits != null && lookupPumpingUnits.Count > 0)
                    {
                        pumpingUnits = lookupPumpingUnits.Select(a => new PumpingUnitForUnitNameModel
                        {
                            APIDesignation = ((PumpingUnit)a.LookupDocument).APIDesignation?.ToString(),
                            UnitId = ((PumpingUnit)a.LookupDocument).UnitId.ToString()
                        }).ToList();
                    }

                    var lookupCustomPUFilter = new FilterDefinitionBuilder<Lookup>()
                       .Where(x => x.LookupType == LookupTypes.PUCustom.ToString()
                       && pumpingUnitIds.Contains(x.LegacyId["UnitId"]));

                    var lookupCustomPU = Find<Lookup>(LOOKUP_COLLECTION, lookupCustomPUFilter, correlationId);

                    if ((lookupPumpingUnits == null && lookupCustomPU == null)
                        || (lookupPumpingUnits.Count == 0 && lookupCustomPU.Count == 0))
                    {
                        logger.WriteCId(Level.Info, "Missing lookup data", correlationId);
                        logger.WriteCId(Level.Trace, $"Finished {nameof(PumpingUnitMongoStore)} {nameof(GetUnitNames)}", correlationId);

                        return pumpingUnits;
                    }

                    if (lookupPumpingUnits != null && lookupCustomPU.Count > 0)
                    {
                        pumpingUnits.AddRange(lookupCustomPU.Select(a => new PumpingUnitForUnitNameModel
                        {
                            APIDesignation = ((PUCustom)a.LookupDocument).APIDesignation?.ToString(),
                            UnitId = ((PUCustom)a.LookupDocument).UnitId.ToString()
                        }).ToList());
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(PumpingUnitMongoStore)} {nameof(GetUnitNames)}", correlationId);

            return pumpingUnits;
        }

        #endregion

    }
}
