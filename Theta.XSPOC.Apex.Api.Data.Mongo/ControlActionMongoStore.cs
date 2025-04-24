using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using MongoLookupCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// Represents a Mongo store for PocType entities.
    /// </summary>
    public class ControlActionMongoStore : MongoOperations, IControlAction
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
        /// Constructs a new <seealso cref="ControlActionMongoStore"/> using the provided 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public ControlActionMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory)
            : base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IControlAction Implementation

        /// <summary>
        /// Gets the supported control actions for the well represented by the provided <paramref name="assetGUID"/>.
        /// </summary>
        /// <param name="assetGUID">The asset guid.</param>
        /// <param name="correlationId"></param>
        /// <returns>The supported control actions for the well represented by the provided <paramref name="assetGUID"/>.</returns>
        public IList<ControlAction> GetControlActions(Guid assetGUID, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(ControlActionMongoStore)} {nameof(GetControlActions)}", correlationId);

            var filter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
               .Where(x => (x.LegacyId.ContainsKey("AssetGUID") && x.LegacyId["AssetGUID"] != null &&
                          (x.LegacyId["AssetGUID"].ToUpper() == assetGUID.ToString().ToUpper())));

            var assetData = Find<MongoAssetCollection.Asset>(ASSET_COLLECTION, filter, correlationId);

            IList<ControlAction> result = new List<ControlAction>();

            try
            {
                var asset = assetData.FirstOrDefault();
                var pocType = int.Parse(asset?.POCType?.LegacyId["POCTypesId"]);

                var filterPocTypeActions = new FilterDefinitionBuilder<MongoLookupCollection.Lookup>()
                   .Where(x => x.LookupType == LookupTypes.POCTypeAction.ToString()
                   && ((POCTypeAction)x.LookupDocument).POCType == pocType);

                var lookupPocTypeActions = Find<MongoLookupCollection.Lookup>(LOOKUP_COLLECTION, filterPocTypeActions, correlationId);

                if (lookupPocTypeActions != null || lookupPocTypeActions.Count > 0)
                {
                    var controlActionsIds = lookupPocTypeActions.Select(x => ((POCTypeAction)x.LookupDocument).ControlActionId).ToList();

                    var filterControlActions = new FilterDefinitionBuilder<MongoLookupCollection.Lookup>()
                        .Where(x => x.LookupType == LookupTypes.ControlActions.ToString()
                              && controlActionsIds.Contains(((ControlActions)x.LookupDocument).ControlActionId));

                    var lookupControlActions = Find<MongoLookupCollection.Lookup>(LOOKUP_COLLECTION, filterControlActions, correlationId);

                    if (lookupControlActions == null || lookupControlActions.Count == 0)
                    {
                        logger.WriteCId(Level.Info, "Missing lookup data", correlationId);
                        logger.WriteCId(Level.Trace, $"Finished {nameof(ControlActionMongoStore)} {nameof(GetControlActions)}", correlationId);

                        return result;
                    }
                    else
                    {
                        result = lookupControlActions.Select(x => new ControlAction
                        {
                            Id = ((ControlActions)x.LookupDocument).ControlActionId,
                            Name = ((ControlActions)x.LookupDocument).Description
                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(ControlActionMongoStore)} {nameof(GetControlActions)}", correlationId);

            return result;
        }

        #endregion

    }
}
