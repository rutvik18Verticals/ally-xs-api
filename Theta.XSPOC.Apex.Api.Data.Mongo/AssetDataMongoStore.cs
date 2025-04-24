using MongoDB.Driver;
using System;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// Implementation of mongo operations related to node master data.
    /// </summary>
    public class AssetDataMongoStore : MongoOperations, IAssetData
    {

        #region Private Constants

        private const string COLLECTION_NAME = "Asset";

        #endregion

        #region Private Fields

        private readonly IMongoDatabase _database;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="AssetDataMongoStore"/> using the provided 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public AssetDataMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory)
            : base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }
        #endregion

        #region IAssetData Implementation

        /// <summary>
        /// Gets the well's enabled status.
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="correlationId"></param>
        /// <returns>The well's enabled status.</returns>
        public bool? GetWellEnabledStatus(Guid assetId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(AssetDataMongoStore)}" +
                $" {nameof(GetWellEnabledStatus)}", correlationId);

            var filter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
               .Where(x => (x.LegacyId.ContainsKey("AssetGUID") && x.LegacyId["AssetGUID"] != null &&
                          (x.LegacyId["AssetGUID"].ToUpper() == assetId.ToString().ToUpper())));

            var assetData = Find<MongoAssetCollection.Asset>(COLLECTION_NAME, filter, correlationId);

            if (assetData == null)
            {
                logger.WriteCId(Level.Info, "Missing node", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AssetDataMongoStore)} {nameof(GetWellEnabledStatus)}", correlationId);

                return null;
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(AssetDataMongoStore)} " +
                $"{nameof(GetWellEnabledStatus)}", correlationId);

            return Map(assetData?.FirstOrDefault())?.Enabled;

        }

        #endregion

        #region Private Methods

        private NodeMasterModel Map(MongoAssetCollection.Asset assetData)
        {
            return new NodeMasterModel
            {
                AssetGuid = Guid.Parse(assetData.LegacyId["AssetGUID"]),
                NodeId = assetData.Name,
                Enabled = assetData.AssetConfig.IsEnabled
            };
        }

        #endregion

    }
}
