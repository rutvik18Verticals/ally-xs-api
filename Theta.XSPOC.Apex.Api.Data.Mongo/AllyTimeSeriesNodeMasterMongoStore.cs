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
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Customers;
using ParameterMongo = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter;

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

        private const string COLLECTION_DEFAULT_PARAMETERS_NAME = "DefaultParameters";

        private const string COLLECTION_MASTERVARIABLES_NAME = "MasterVariables";
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

        #region IAllyTimeSeriesNodeMaster Implementation
        
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
                        ApplicationId = GetArtificialLiftType(asset.ArtificialLiftType),
                        MethodofProduction = asset.ArtificialLiftType,
                        Field = asset.Field,
                        Area = asset.Area,
                        BusinessUnit = asset.BusinessUnit,
                        TimeZoneId = asset.AssetConfig.TimeZoneId,
                        TimeZoneOffset = asset.AssetConfig.TimeZoneOffset,
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
        public async Task<IList<NodeMasterModel>> ValidateCustomerAsync(string userAccount, string tokenKey, string tokenValue, string correlationId)
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

        /// <summary>
        /// Get the list of all parameter standard type.
        /// </summary>
        /// <returns>The <seealso cref="IList{ParamStandardData}"/>.</returns>
        public async Task<IList<DefaultParameters>> GetAllDefaultParametersAsync(string liftType, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersMongoStore)}" + $" {nameof(GetAllDefaultParametersAsync)}", correlationId);

            await Task.Yield();

            var filter = new FilterDefinitionBuilder<DefaultParameters>()
              .Where(x => (x.Selected == true && x.LiftType == liftType));

            var defaultPara = Find<DefaultParameters>(COLLECTION_DEFAULT_PARAMETERS_NAME, filter, correlationId);

            if (defaultPara != null)
            {
                logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersMongoStore)} " + $"{nameof(GetAllDefaultParametersAsync)}", correlationId);
                return defaultPara;
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersMongoStore)} " + $"{nameof(GetAllDefaultParametersAsync)}", correlationId);

            return null;
        }

        /// <summary>
        /// Get the list of all parameter standard type.
        /// </summary>
        /// <returns>The <seealso cref="IList{TimeSeriesChartAggregation}"/>.</returns>
        public async Task<IList<TimeSeriesChartAggregation>> GetTimeSeriesChartAggregationAsync(string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersMongoStore)}" + $" {nameof(GetTimeSeriesChartAggregationAsync)}", correlationId);

            await Task.Yield();

            var filter = new FilterDefinitionBuilder<MongoLookupCollection.Lookup>()
               .Where(x => (x.LookupType == "TimeSeriesChartAggregation"));

            var lookupData = Find<MongoLookupCollection.Lookup>(COLLECTION_LOOKUP_NAME, filter, correlationId);

            if (lookupData != null)
            {
                if (lookupData.Count > 0)
                {
                    var result = lookupData.Select(item => new TimeSeriesChartAggregation
                    {
                        Name = ((TimeSeriesChartAggregation)item.LookupDocument).Name,
                        Minutes = ((TimeSeriesChartAggregation)item.LookupDocument).Minutes,
                        Aggregate = ((TimeSeriesChartAggregation)item.LookupDocument).Aggregate,
                    });

                    logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersMongoStore)} " + $"{nameof(GetTimeSeriesChartAggregationAsync)}", correlationId);

                    return result.ToList();
                }
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersMongoStore)} " + $"{nameof(GetTimeSeriesChartAggregationAsync)}", correlationId);

            return new List<TimeSeriesChartAggregation>();
        }

        /// <summary>
        /// Get Asset Mongo Collection.
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        public async Task<MongoAssetCollection.Asset> GetAssetAsync(Guid assetId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(AllyTimeSeriesNodeMasterMongoStore)}.{nameof(GetAssetAsync)}", correlationId);
            
            await Task.Yield();

            try
            {
                var assetGuidString = assetId.ToString();

                var filter = Builders<MongoAssetCollection.Asset>.Filter.And(
                    Builders<MongoAssetCollection.Asset>.Filter.Exists($"LegacyId.AssetGUID"),
                    Builders<MongoAssetCollection.Asset>.Filter.Ne(x => x.LegacyId["AssetGUID"], null),
                    Builders<MongoAssetCollection.Asset>.Filter.Regex("LegacyId.AssetGUID", new BsonRegularExpression($"^{assetGuidString}$", "i"))
                );

                var assetData = Find<MongoAssetCollection.Asset>(COLLECTION_ASSET_NAME, filter, correlationId);

                if (assetData == null || !assetData.Any())
                {
                    logger.WriteCId(Level.Warn, $"Asset not found for AssetGUID: {assetId}", correlationId);
                    return null;
                }

                var asset = assetData.FirstOrDefault();
                logger.WriteCId(Level.Trace, $"Finished {nameof(AllyTimeSeriesNodeMasterMongoStore)}.{nameof(GetAssetAsync)}", correlationId);
                return asset;
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, $"Error in {nameof(AllyTimeSeriesNodeMasterMongoStore)}.{nameof(GetAssetAsync)}: {ex.Message}", correlationId);
                return null;
            }
        }

        /// <summary>
        /// Get Asset Mongo Collection.
        /// </summary>
        /// <param name="customerObjectId"></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        public async Task<MongoCustomerCollection.Customer> GetCustomerAsync(string customerObjectId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(AllyTimeSeriesNodeMasterMongoStore)}.{nameof(GetCustomerAsync)}", correlationId);

            await Task.Yield();

            try
            {
                // Get Customer
                var customerBuilder = Builders<Customer>.Filter;
                var filterCustomer = customerBuilder.Eq(x => x.Id, customerObjectId);

                var customerData = Find<MongoCustomerCollection.Customer>(COLLECTION_CUSTOMER_NAME, filterCustomer, correlationId);

                // In case customer not found.
                if (customerData == null || customerData.Count == 0)
                {
                    logger.Write(Level.Info, "Missing customer");
                    logger.Write(Level.Trace, $"Finished {nameof(AllyTimeSeriesNodeMasterMongoStore)} {nameof(GetCustomerAsync)}");

                    return null;
                }

                logger.WriteCId(Level.Trace, $"Finished {nameof(AllyTimeSeriesNodeMasterMongoStore)} " + $"{nameof(GetCustomerAsync)}", correlationId);

                return customerData.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, $"Error in {nameof(AllyTimeSeriesNodeMasterMongoStore)}.{nameof(GetCustomerAsync)}: {ex.Message}", correlationId);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterKeys"></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        public async Task<IDictionary<(int POCType, string ChannelId), ParameterMongo.Parameters>> GetParametersBulk(List<(int POCType, string ChannelId)> parameterKeys, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(AllyTimeSeriesNodeMasterMongoStore)}.{nameof(GetParametersBulk)}", correlationId);
            
            await Task.Yield();

            try
            {
                // Build an Or filter for all POCType and ChannelId pairs
                var filters = parameterKeys.Select(key =>
                    Builders<ParameterMongo.Parameters>.Filter.And(
                        Builders<ParameterMongo.Parameters>.Filter.Eq("POCType.LookupDocument.POCType", key.POCType),
                        Builders<ParameterMongo.Parameters>.Filter.Eq("ChannelId", key.ChannelId)
                    )
                );
                var filter = Builders<ParameterMongo.Parameters>.Filter.Or(filters);

                var parameterDocs = Find<ParameterMongo.Parameters>(COLLECTION_MASTERVARIABLES_NAME, filter, correlationId);

                if (parameterDocs == null || !parameterDocs.Any())
                {
                    logger.WriteCId(Level.Warn, "No parameters found for provided keys.", correlationId);
                    return new Dictionary<(int POCType, string ChannelId), ParameterMongo.Parameters>();
                }

                // Map results to a dictionary for quick lookup
                var parameterDict = parameterDocs
                    .GroupBy(param => (((POCTypes)param.POCType.LookupDocument).POCType, param.ChannelId))
                    .ToDictionary(
                        group => group.Key,
                        group => group.First()
                    );

                logger.WriteCId(Level.Trace, $"Finished {nameof(AllyTimeSeriesNodeMasterMongoStore)}.{nameof(GetParametersBulk)}", correlationId);
                return parameterDict;
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, $"Error in {nameof(AllyTimeSeriesNodeMasterMongoStore)}.{nameof(GetParametersBulk)}: {ex.Message}", correlationId);
                return new Dictionary<(int POCType, string ChannelId), ParameterMongo.Parameters>();
            }
        }
        #endregion

        #region Private Helping Methods
        /// <summary>
        /// Gets the node master data based on asset guid.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns></returns>
        public async Task<IList<NodeMasterModel>> GetAssetDetails(Guid assetId, string correlationId)
        {
            await Task.Yield();
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(NodeMastersMongoStore)} " +
                $"{nameof(GetAssetDetails)}", correlationId);

            var filter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
                .Where(x => (x.LegacyId.ContainsKey("AssetGUID") && x.LegacyId["AssetGUID"] != null &&
                           (x.LegacyId["AssetGUID"].ToUpper() == assetId.ToString().ToUpper())));

            var assetData = Find<MongoAssetCollection.Asset>(COLLECTION_ASSET_NAME, filter, correlationId);

            logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersMongoStore)} " +
                $"{nameof(GetAssetDetails)}", correlationId);

            if (assetData != null)
            {
                if (assetData.Count > 0)
                {
                    var result = assetData.Select(asset => new NodeMasterModel
                    {
                        AssetGuid = Guid.Parse(asset.LegacyId["AssetGUID"]),
                        NodeId = asset.Name,
                        PocType = (short)(asset.POCType != null ? (asset.POCType.LegacyId != null ? short.Parse(asset.POCType.LegacyId["POCTypesId"]) : 0) : 0),
                        ApplicationId = GetArtificialLiftType(asset.ArtificialLiftType),
                        Tzoffset = (float)asset.AssetConfig.TimeZoneOffset,
                        Tzdaylight = asset.AssetConfig.HonorDayLightSavings
                    });

                    logger.WriteCId(Level.Trace, $"Finished {nameof(NodeMastersMongoStore)} " + $"{nameof(GetByAssetIdsForAllyTimeSeriesAsync)}", correlationId);

                    return result.Where(x => x.AssetGuid == assetId).ToList();
                }
            }

            return null;
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

        private static int GetBitPriority(IDictionary<string, string> legacyId)
        {
            if (legacyId.TryGetValue("Bit", out var bitValue))
            {
                return bitValue switch
                {
                    "1" => 2, // Highest priority
                    "0" => 1, // Second priority
                    _ => 0    // Lowest priority for "", null, or other values
                };
            }
            return 0; // Default for missing "Bit" key
        }
        #endregion
    }
}