using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;
using MongoCustomerCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Customers;
using MongoLookupCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using MongoCustomerAccessDetailsCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.CustomerAccessDetails;
using Enums = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Enums;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using MongoDB.Bson;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// Implementation of mongo operations related to node master data.
    /// </summary>
    public class AllyTimeSeriesNodeMasterMongoStore : MongoOperations, IAllyTimeSeriesNodeMaster
    {
        #region Private Constants

        private const string COLLECTION_ASSET_NAME = "Asset";

        private const string COLLECTION_CUSTOMER_NAME = "Customers";

        private const string COLLECTION_CUSTOMER_ACCESS_DEATILS_NAME = "CustomerAccessDetails";

        private const string COLLECTION_LOOKUP_NAME = "Lookup";

        #endregion

        #region Private Fields

        private readonly IMongoDatabase _database;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="AllyTimeSeriesNodeMasterMongoStore"/> using the provided 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public AllyTimeSeriesNodeMasterMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory)
            : base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion
        /// <summary>
        /// Get the node id from node master by asset id for Ally timeseries.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="IList{NodeMasterModel}"/>.</returns>
        public async Task<IList<NodeMasterModel>> GetByAssetIdsForAllyTimeSeriesAsync(IList<Guid> assetId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersMongoStore)}" + $" {nameof(GetByAssetIdsForAllyTimeSeriesAsync)}", correlationId);

            await Task.Yield();
            
            var stringAssetId = assetId.Select(x => x.ToString().ToLower()).ToArray();

            var filters = stringAssetId.Select(id =>
                Builders<MongoAssetCollection.Asset>.Filter.Regex("LegacyId.AssetGUID",
                    new BsonRegularExpression(id, "i"))
            );

            var filter = Builders<MongoAssetCollection.Asset>.Filter.And(
                Builders<MongoAssetCollection.Asset>.Filter.Exists("LegacyId.AssetGUID"),
                Builders<MongoAssetCollection.Asset>.Filter.Ne("LegacyId.AssetGUID", BsonNull.Value),
                Builders<MongoAssetCollection.Asset>.Filter.Or(filters)
            );

            var assetData = Find<MongoAssetCollection.Asset>(COLLECTION_ASSET_NAME, filter, correlationId);

            if (assetData != null)
            {
                if (assetData.Count > 0)
                {
                    var result = assetData.Select(asset => new NodeMasterModel
                    {
                        AssetGuid = Guid.Parse(asset.LegacyId["AssetGUID"]),
                        NodeId = asset.Name,
                        PocType = (short)(asset.POCType != null ? (asset.POCType.LegacyId != null ? short.Parse(asset.POCType.LegacyId["POCTypesId"]) : 0) : 0),
                        ApplicationId = GetArtificialLiftType(asset.ArtificialLiftType)
                    });

                    logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersMongoStore)} " + $"{nameof(GetByAssetIdsForAllyTimeSeriesAsync)}", correlationId);

                    return result.Where(x => assetId.Contains(x.AssetGuid)).ToList();
                }
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersMongoStore)} " + $"{nameof(GetByAssetIdsForAllyTimeSeriesAsync)}", correlationId);

            return new List<NodeMasterModel>();
        }

        /// <summary>
        /// Get the assets by customer ID.
        /// </summary>
        /// <param name="customerIds">The customer id.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="IList{NodeMasterModel}"/>.</returns>
        public async Task<IList<NodeMasterModel>> GetAssetsByCustomerIdAsync(List<string> customerIds, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersMongoStore)}" + $" {nameof(GetAssetsByCustomerIdAsync)}", correlationId);

            await Task.Yield();
            
            var stringCustIds = customerIds.Select(x => x.ToString().ToLower()).ToArray();

            var filters = stringCustIds.Select(id =>
                Builders<MongoCustomerCollection.Customer>.Filter.Regex("LegacyId.CustomerGUID",
                    new BsonRegularExpression(id, "i"))
            );

            var filterCust = Builders<MongoCustomerCollection.Customer>.Filter.And(
                Builders<MongoCustomerCollection.Customer>.Filter.Exists("LegacyId.CustomerGUID"),
                Builders<MongoCustomerCollection.Customer>.Filter.Ne("LegacyId.CustomerGUID", BsonNull.Value),
                Builders<MongoCustomerCollection.Customer>.Filter.Or(filters)
            );

            var custData = Find<MongoCustomerCollection.Customer>(COLLECTION_CUSTOMER_NAME, filterCust, correlationId);

            if (custData != null)
            {
                if (custData.Count > 0)
                {
                    var ids = custData.Select(x => x.Id).ToList();

                    var filter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
                    .Where(x => (ids.Contains(x.CustomerId) && x.LegacyId.ContainsKey("AssetGUID") && x.LegacyId["AssetGUID"] != null &&
                                 x.AssetConfig != null));

                    var assetData = Find<MongoAssetCollection.Asset>(COLLECTION_ASSET_NAME, filter, correlationId);

                    if (assetData != null)
                    {
                        if (assetData.Count > 0)
                        {
                            var result = assetData.Select(asset => new NodeMasterModel
                            {
                                CustomerId = custData.Where(x => x.Id == asset.CustomerId).Select(x => x.LegacyId["CustomerGUID"]).FirstOrDefault()?.ToString(),
                                AssetGuid = Guid.Parse(asset.LegacyId["AssetGUID"]),
                                NodeId = asset.Name,
                                PocType = (short)(asset.POCType != null ? (asset.POCType.LegacyId != null ? short.Parse(asset.POCType.LegacyId["POCTypesId"]) : 0) : 0),
                                ApplicationId = GetArtificialLiftType(asset.ArtificialLiftType),
                                Enabled = asset.AssetConfig != null && asset.AssetConfig.IsEnabled
                            });

                            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersMongoStore)} " + $"{nameof(GetAssetsByCustomerIdAsync)}", correlationId);

                            return result.ToList();
                        }
                    }
                }
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersMongoStore)} " + $"{nameof(GetAssetsByCustomerIdAsync)}", correlationId);

            return new List<NodeMasterModel>();
        }

        /// <summary>
        /// Get the list of all parameter standard type.
        /// </summary>
        /// <returns>The <seealso cref="IList{ParamStandardData}"/>.</returns>
        public async Task<IList<ParamStandardData>> GetAllParameterStandardTypesAsync(string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersMongoStore)}" + $" {nameof(GetAllParameterStandardTypesAsync)}", correlationId);

            await Task.Yield();

            var filter = new FilterDefinitionBuilder<MongoLookupCollection.Lookup>()
               .Where(x => (x.LookupType == "ParamStandardTypes"));

            var lookupData = Find<MongoLookupCollection.Lookup>(COLLECTION_LOOKUP_NAME, filter, correlationId);

            if (lookupData != null)
            {
                if (lookupData.Count > 0)
                {
                    var result = lookupData.Select(item => new ParamStandardData
                    {
                        ParamStandardType = ((ParamStandardTypes)item.LookupDocument).ParamStandardType,
                        StringValue = ((ParamStandardTypes)item.LookupDocument).Description
                    });

                    logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersMongoStore)} " + $"{nameof(GetAllParameterStandardTypesAsync)}", correlationId);

                    return result.ToList();
                }
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersMongoStore)} " + $"{nameof(GetAllParameterStandardTypesAsync)}", correlationId);

            return new List<ParamStandardData>();
        }

        /// <summary>
        /// Checks if customer token is valid.
        /// </summary>
        /// <param name="userAccount">The user account details.</param>
        /// <param name="tokenKey">The API token Key.</param>
        /// <param name="tokenValue">The API token Key.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="IList{NodeMasterModel}"/>.</returns>
        public async Task<IList<NodeMasterModel>> ValidateCustomerAsync(string userAccount, string tokenKey,string tokenValue, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersMongoStore)}" + $" {nameof(ValidateCustomerAsync)}", correlationId);

            await Task.Yield();

            var filterCust = new FilterDefinitionBuilder<MongoCustomerAccessDetailsCollection.CustomerAccessDetails>()
               .Where(x => (x.UserAccount.ToLower() == userAccount.ToLower()));

            var custData = Find<MongoCustomerAccessDetailsCollection.CustomerAccessDetails>(COLLECTION_CUSTOMER_ACCESS_DEATILS_NAME, filterCust, correlationId);

            if (custData != null)
            {
                if (custData.Count > 0)
                {
                    var result = custData.Select(item => new NodeMasterModel
                    {
                        UserAccount = item.UserAccount,
                        CustomerId = item.CustomerGUID,
                        IsValid = item.APITokenKey == tokenKey && item.APITokenValue == tokenValue && item.ExpirationDate >= DateTime.UtcNow,
                    });

                    logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersMongoStore)} " + $"{nameof(ValidateCustomerAsync)}", correlationId);

                    return result.ToList();
                }
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersMongoStore)} " + $"{nameof(GetAssetsByCustomerIdAsync)}", correlationId);

            return new List<NodeMasterModel>();
        }

        private int? GetArtificialLiftType(string artificialLiftType)
        {
            if (artificialLiftType == null)
            {
                return null;
            }
            else
            {
                if (Enum.TryParse(typeof(Enums.Applications), artificialLiftType, out var result))
                {
                    return ((int?)(Enums.Applications)result);
                }
            }

            return null;
        }
    }
}