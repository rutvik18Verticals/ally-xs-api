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
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// Represents a mongo store implementation for PumpingUnitManufacturer entities.
    /// </summary>
    public class PumpingUnitManufacturerMongoStore : MongoOperations, IPumpingUnitManufacturer
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
        /// Constructs a new <seealso cref="PumpingUnitManufacturerMongoStore"/> using the provided.
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public PumpingUnitManufacturerMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory)
            : base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IPumpingUnitManufacturer Implementation

        /// <summary>
        /// Retrieves a list of pumping unit manufacturers for the specified node IDs.
        /// </summary>
        /// <param name="nodeIds">The list of node IDs.</param>
        /// <param name="correlationId"></param>
        /// <returns>A list of pumping unit manufacturers <seealso cref="IList{PumpingUnitManufacturerForGroupStatus}"/>.</returns>
        public IList<PumpingUnitManufacturerForGroupStatus> GetManufacturers(IList<string> nodeIds, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(PumpingUnitManufacturerMongoStore)} {nameof(GetManufacturers)}", correlationId);

            var pumpingUnitManufacturers = new List<PumpingUnitManufacturerForGroupStatus>();

            try
            {
                var stringNodeId = nodeIds.Select(x => x.ToString()).ToArray();

                var assetFilter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
                    .Where(x => (x.LegacyId.ContainsKey("NodeId") && x.LegacyId["NodeId"] != null &&
                                 stringNodeId.Contains(x.LegacyId["NodeId"]) && x.ArtificialLiftType == ""));

                var assetData = Find<MongoAssetCollection.Asset>(ASSET_COLLECTION, assetFilter, correlationId);

                var assetDataList = assetData.ToList();

                var pumpingUnits = assetDataList.Where(x => x.AssetDetails != null && ((RodPumpDetail)x.AssetDetails).PumpingUnit != null)
                    .Select(a => ((RodPumpDetail)a.AssetDetails).PumpingUnit).ToList();

                if (pumpingUnits.Count > 0)
                {
                    var pumpingUnitIds = pumpingUnits.Select(x => ((PumpingUnit)x.LookupDocument)?.UnitId.ToString())
                        .Distinct().ToArray();

                    var lookupFilter = new FilterDefinitionBuilder<Lookup>()
                       .Where(x => x.LookupType == LookupTypes.PumpingUnitManufacturer.ToString()
                       && pumpingUnitIds.Contains(x.LegacyId["UnitId"]));

                    var lookupPumpingUnitManufacturer = Find<Lookup>(LOOKUP_COLLECTION, lookupFilter, correlationId);

                    if (lookupPumpingUnitManufacturer != null && lookupPumpingUnitManufacturer.Count > 0)
                    {
                        pumpingUnitManufacturers = lookupPumpingUnitManufacturer
                            .Where(x => x != null && x.LookupDocument != null)
                            .Select(a => new PumpingUnitManufacturerForGroupStatus
                            {
                                UnitId = ((PumpingUnitManufacturer)a.LookupDocument).UnitTypeId.ToString(),
                                Manufacturer = ((PumpingUnitManufacturer)a.LookupDocument).Manuf.ToString()
                            }).ToList();
                    }

                    var lookupCustomPUFilter = new FilterDefinitionBuilder<Lookup>()
                       .Where(x => x.LookupType == LookupTypes.PUCustom.ToString()
                       && pumpingUnitIds.Contains(x.LegacyId["UnitId"]));

                    var lookupCustomPUManufacturer = Find<Lookup>(LOOKUP_COLLECTION, lookupCustomPUFilter, correlationId);

                    if (lookupPumpingUnitManufacturer == null && lookupCustomPUManufacturer == null)
                    {
                        logger.WriteCId(Level.Info, "Missing lookup data", correlationId);
                        logger.WriteCId(Level.Trace, $"Finished {nameof(PumpingUnitManufacturerMongoStore)} {nameof(GetManufacturers)}", correlationId);

                        return pumpingUnitManufacturers;
                    }

                    if (lookupCustomPUManufacturer != null && lookupCustomPUManufacturer.Count > 0)
                    {
                        pumpingUnitManufacturers.AddRange(lookupCustomPUManufacturer.Select(a => new PumpingUnitManufacturerForGroupStatus
                        {
                            UnitId = ((PUCustom)a.LookupDocument).UnitId.ToString(),
                            Manufacturer = ((PUCustom)a.LookupDocument).Manufacturer.ToString()
                        }).ToList());
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(PumpingUnitManufacturerMongoStore)} {nameof(GetManufacturers)}", correlationId);

            return pumpingUnitManufacturers;
        }

        #endregion

    }
}
