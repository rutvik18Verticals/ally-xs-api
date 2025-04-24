
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Exceptions;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using MongoAsset = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{

    /// <summary>
    /// Implementation of mongo operations related to Exceptions data.
    /// </summary>
    public class ExceptionMongoStore : MongoOperations, IException
    {
        #region Private Constants

        private const string COLLECTION_NAME_EXCEPTIONS = "Exceptions";
        private const string COLLECTION_NAME_ASSET = "Asset";

        #endregion

        #region Private Fields

        private readonly IMongoDatabase _database;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="ExceptionMongoStore"/> using the provided 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public ExceptionMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory)
            : base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IExceptionRepository Implementation

        /// <summary>
        /// Retrieves a list of exceptions based on the provided node Ids.
        /// </summary>
        /// <param name="nodeIds">The list of node Ids.</param>
        /// <param name="correlationId"></param>
        /// <returns>A list of exception models.</returns>
        public IList<ExceptionModel> GetExceptions(IList<string> nodeIds, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(ExceptionMongoStore)} {nameof(GetExceptions)}", correlationId);

            if (nodeIds == null)
            {
                logger.WriteCId(Level.Info, "Missing node ids", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(ExceptionMongoStore)} {nameof(GetExceptions)}", correlationId);
                throw new ArgumentNullException(nameof(nodeIds));
            }

            List<MongoAsset.Asset> assets = (List<MongoAsset.Asset>)GetAssets(nodeIds, correlationId);

            if (assets == null)
            {
                logger.WriteCId(Level.Info, "Asset not found for specified nodeIds", correlationId);
                return null;
            }

            var assetIds = assets.Select(a => a.Id).ToList();
            var assetIdMap = assets.ToDictionary(asset => asset.Id, asset => asset.Name);

            var filter = Builders<Exceptions>.Filter.In("AssetId", assetIds);

            var wellTestDocs = Find<Exceptions>(COLLECTION_NAME_EXCEPTIONS, filter, correlationId);

            var exceptions = wellTestDocs
                .ToList()
                .GroupBy(e => e.AssetId)
                .Select(g => g
                    .OrderByDescending(e => e.Priority)
                    .ThenBy(e => e.ExceptionGroupName)
                    .FirstOrDefault())
                .Select(e => new ExceptionModel
                {
                    NodeId = assetIdMap[e.AssetId],
                    ExceptionGroupName = e.ExceptionGroupName,
                    Priority = e.Priority,
                })
                .ToList();

            return exceptions;
        }

        /// <summary>
        /// This method will retrieve the exceptions for the provided <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset id used to retrieve the data.</param>
        /// <param name="customerId">The customer id used to verify customer has access to the data.</param>
        /// <param name="correlationId"></param>
        /// <returns>A <seealso cref="IList{ExceptionData}"/> that represents the most recent historical data
        /// for the defined registers for the provided <paramref name="assetId"/>.</returns>
        public async Task<IList<ExceptionData>> GetAssetStatusExceptionsAsync(Guid assetId, Guid customerId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(ExceptionMongoStore)}.{nameof(GetAssetStatusExceptionsAsync)}", correlationId);

            if (assetId == Guid.Empty)
            {
                logger.WriteCId(Level.Info, "Missing asset id", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(ExceptionMongoStore)}.{nameof(GetAssetStatusExceptionsAsync)}", correlationId);
                return null;
            }

            try
            {
                var asset = await Task.Run(() => 
                    GetAsset(assetId, correlationId));
                if (asset == null)
                {
                    logger.WriteCId(Level.Warn, $"Asset not found for AssetGuid: {assetId}", correlationId);
                    return new List<ExceptionData>();
                }

                var exceptions = await Task.Run(() => 
                    Find<Exceptions>(COLLECTION_NAME_EXCEPTIONS, Builders<Exceptions>.Filter.Eq("AssetId", asset.Id), correlationId));
                if (exceptions == null || !exceptions.Any())
                {
                    logger.WriteCId(Level.Warn, $"No exceptions data found for AssetId: {asset.Id}", correlationId);
                    return new List<ExceptionData>();
                }

                var exceptionDataList = exceptions.Select(e => new ExceptionData
                {
                    Description = e.ExceptionGroupName,
                    Priority = e.Priority ?? 0
                }).ToList();

                logger.WriteCId(Level.Trace, $"Finished {nameof(ExceptionMongoStore)}.{nameof(GetAssetStatusExceptionsAsync)}", correlationId);
                return exceptionDataList;
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, $"Error in {nameof(ExceptionMongoStore)}.{nameof(GetAssetStatusExceptionsAsync)}: {ex.Message}", correlationId);
                return new List<ExceptionData>();
            }
        }

        #endregion

        #region Private mongo fetch methods

        private IList<MongoAsset.Asset> GetAssets(IList<string> nodeIds, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(ExceptionMongoStore)}.{nameof(GetAssets)}", correlationId);

            try
            {
                var filter = Builders<MongoAsset.Asset>.Filter.In("Name", nodeIds);

                /*
                Node Check for case sensitive and change logic if required
                var filters = nodeIds.Select(nodeId =>
                     Builders<MongoAsset.Asset>.Filter.Regex("Name", new BsonRegularExpression($"^{nodeId}$", "i"))
                 );
                var filter = Builders<MongoAsset.Asset>.Filter.Or(filters);
                */

                var assetData = Find<MongoAsset.Asset>(COLLECTION_NAME_ASSET, filter, correlationId);

                if (assetData == null || !assetData.Any())
                {
                    logger.WriteCId(Level.Warn, $"No Assets found", correlationId);
                    return null;
                }

                logger.WriteCId(Level.Trace, $"Finished {nameof(ExceptionMongoStore)}.{nameof(GetAssets)}", correlationId);
                return assetData.ToList();

            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, $"Error in {nameof(ExceptionMongoStore)}.{nameof(GetAssets)}: {ex.Message}", correlationId);
                return null;
            }
        }

        private MongoAsset.Asset GetAsset(Guid assetId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(ExceptionMongoStore)}.{nameof(GetAsset)}", correlationId);

            try
            {
                var assetGuidString = assetId.ToString();

                var filter = Builders<MongoAsset.Asset>.Filter.And(
                    Builders<MongoAsset.Asset>.Filter.Exists($"LegacyId.AssetGUID"),
                    Builders<MongoAsset.Asset>.Filter.Ne(x => x.LegacyId["AssetGUID"], null),
                    Builders<MongoAsset.Asset>.Filter.Regex("LegacyId.AssetGUID", new BsonRegularExpression($"^{assetGuidString}$", "i"))
                );

                var assetData = Find<MongoAsset.Asset>(COLLECTION_NAME_ASSET, filter, correlationId);

                if (assetData == null || !assetData.Any())
                {
                    logger.WriteCId(Level.Warn, $"Asset not found for AssetGUID: {assetId}", correlationId);
                    return null;
                }

                var asset = assetData.FirstOrDefault();
                logger.WriteCId(Level.Trace, $"Finished {nameof(ExceptionMongoStore)}.{nameof(GetAsset)}", correlationId);
                return asset;
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, $"Error in {nameof(ExceptionMongoStore)}.{nameof(GetAsset)}: {ex.Message}", correlationId);
                return null;
            }
        }

        #endregion
    }
}
