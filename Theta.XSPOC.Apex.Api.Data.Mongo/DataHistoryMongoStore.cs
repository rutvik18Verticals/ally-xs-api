
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup;
using Theta.XSPOC.Apex.Api.Data.Influx.Services;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Customers;
using Theta.XSPOC.Apex.Kernel.Logging;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset.Asset;
using MongoParameters = Theta.XSPOC.Apex.Kernel.Mongo.Models.Parameter.Parameters;
namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// Mongo Implementation of the IDataHistorySQLStore interface's methods which has DataHistory Table in use. 
    /// </summary>
    public class DataHistoryMongoStore : MongoOperations, IDataHistoryMongoStore
    {
        #region Private Constants

        private const string COLLECTION_ASSET_NAME = "Asset";
        private const string COLLECTION_CUSTOMER_NAME = "Customers";
        private const string COLLECTION_MASTERVARIABLES_NAME = "MasterVariables";
        #endregion

        #region Private Fields

        private readonly IMongoDatabase _database;
        private readonly IGetDataHistoryItemsService _getDataHistoryItemsService;
        private readonly IMemoryCache _cache;

        #endregion

        /// <summary>
        /// Constructs a new <seealso cref="ParameterDataTypeMongoStore"/> using the provided 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        /// <param name="getDataHistoryItemsService">The service to fetch data for DataHistory using Influx.</param>
        /// <param name="memoryCache">The memory cache.</param>

        public DataHistoryMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory, IGetDataHistoryItemsService getDataHistoryItemsService, IMemoryCache memoryCache) :
            base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _getDataHistoryItemsService = getDataHistoryItemsService ?? throw new ArgumentNullException(nameof(getDataHistoryItemsService));
            _cache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        /// <summary>
        /// Gets the <seealso cref="IList{ControllerTrendDataModel}"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="address">The address.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{ControllerTrendDataModel}"/>.</returns>
        public async Task<IList<ControllerTrendDataModel>> GetControllerTrendData(string nodeId, int address, DateTime startDate, DateTime endDate, string correlationId)
        {
            float maxDecimal = 9999999999999999999999999999f;
            var assetsCollection = _database.GetCollection<MongoAssetCollection>(COLLECTION_ASSET_NAME);
            var customerCollection = _database.GetCollection<Customer>(COLLECTION_CUSTOMER_NAME);

            // Fetch asset data
            var assetData = await assetsCollection.Find(x => x.LegacyId["NodeId"] == nodeId).FirstOrDefaultAsync();
            string poctype = assetData?.POCType.LegacyId["POCTypesId"];

            // Get NodeId from Asset
            Guid assetGUID = Guid.TryParse(assetData?.LegacyId?["AssetGUID"], out var parsedGuid)
                ? parsedGuid
                : Guid.Empty;

            if (assetGUID == Guid.Empty)
            {
                return Enumerable.Empty<ControllerTrendDataModel>().ToList();
            }

            // Get Asset Id from Asset.
            var assetObjId = assetData?.Id;

            // Get Asset Id from Asset.
            var customerObjId = assetData?.CustomerId;

            // Get Customer
            var customerBuilder = Builders<Customer>.Filter;
            var filterCustomer = customerBuilder.Eq(x => x.Id, customerObjId);
            var customerData = customerCollection.Find(filterCustomer).FirstOrDefault();

            // Get customerGUID from Customer.
            Guid customerGUID = Guid.TryParse(customerData?.LegacyId?["CustomerGUID"], out var parsedCustomerGuid)
                ? parsedCustomerGuid
                : Guid.Empty;

            // Fetch data history from Influx
            IList<DataPointModel> dataHistories = await _getDataHistoryItemsService.GetDataHistoryItems(
                assetGUID, customerGUID, poctype, new List<string> { address.ToString() }, 
                null, startDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss"), endDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss"));

            // Join parameters with data history
            var joinedResult = (from dh in dataHistories
                                orderby dh.Time
                                select new ControllerTrendDataModel
                                {
                                    Date = dh.Time,
                                    Value = ConvertToFloat(dh.Value) is float v ? (v > maxDecimal ? maxDecimal : v) : 0f
                                }).ToList();

            return joinedResult;
        }

        /// <summary>
        /// Get the controller trend item by node id and poc type.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="pocType">The poc type.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="ControllerTrendItemModel"/>.</returns>
        public async Task<IList<DataPointModel>> GetControllerTrendItems(string nodeId, int pocType, string correlationId)
        {
            var parametersCollection = _database.GetCollection<MongoParameters>(COLLECTION_MASTERVARIABLES_NAME);
            var assetsCollection = _database.GetCollection<MongoAssetCollection>(COLLECTION_ASSET_NAME);
            var customerCollection = _database.GetCollection<Customer>(COLLECTION_CUSTOMER_NAME);

            var builderParam = Builders<MongoParameters>.Filter;
            var filterParam = builderParam.Empty;

            var assetData = await assetsCollection.Find(x => x.LegacyId["NodeId"] == nodeId).FirstOrDefaultAsync();
            string poctypeStr = pocType.ToString();
            // Get NodeId from Asset
            Guid assetGUID = Guid.TryParse(assetData?.LegacyId?["AssetGUID"], out var parsedGuid)
                ? parsedGuid
                : Guid.Empty;

            if (assetGUID == Guid.Empty)
            {
                return Enumerable.Empty<DataPointModel>().ToList();
            }

            // Get Asset Id from Asset.
            var assetObjId = assetData?.Id;

            // Get Asset Id from Asset.
            var customerObjId = assetData?.CustomerId;

            // Get Customer
            var customerBuilder = Builders<Customer>.Filter;
            var filterCustomer = customerBuilder.Eq(x => x.Id, customerObjId);
            var customerData = customerCollection.Find(filterCustomer).FirstOrDefault();

            // Get customerGUID from Customer.
            Guid customerGUID = Guid.TryParse(customerData?.LegacyId?["CustomerGUID"], out var parsedCustomerGuid)
                ? parsedCustomerGuid
                : Guid.Empty;

            var mvBuilderParam = Builders<MongoParameters>.Filter;
            var mvFilterParam = mvBuilderParam.Empty;

            // Get POCType ID from Asset.
            string poctypesId = assetData?.POCType.LegacyId["POCTypesId"];

            if (string.IsNullOrWhiteSpace(poctypesId))
            {
                mvFilterParam &= mvBuilderParam.Eq(x => x.LegacyId["POCType"], "99");
            }
            else
            {
                // add poc type filter (also add poctype 99)
                mvFilterParam &= mvBuilderParam.Or(
                    mvBuilderParam.Eq(x => x.LegacyId["POCType"], poctypesId),
                    mvBuilderParam.Eq(x => x.LegacyId["POCType"], "99")
                );
            }

            var mvData = parametersCollection.Find(mvFilterParam).ToList();
            if (mvData == null)
            {
                mvData = new List<MongoParameters> { };
            }
            else
            {
                // get distinct data by channelId.
                mvData = mvData
                    .OrderByDescending(x => GetBitPriority(x.LegacyId))
                    .DistinctBy(x => x.ChannelId)
                    .ToList();
            }
            var mvChannelIds = mvData.Select(x => x.ChannelId).Distinct().ToList();

            var dataHistoryItems = await _getDataHistoryItemsService.GetDataHistoryTrendData(assetGUID
                , customerGUID, poctypeStr,
                mvChannelIds, DateTime.MinValue.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss"), DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss"));

            return dataHistoryItems;
        }

        /// <summary>
        /// Gets the downtime by wells.
        /// </summary>
        /// <param name="nodeIds">The node ids.</param>
        /// <param name="numberOfDays">The number of days.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <see cref="DowntimeByWellsModel"/>.</returns>
        public async Task<DowntimeByWellsModel> GetDowntime(IList<string> nodeIds, int numberOfDays, string correlationId)
        {
            const int pstRunTime = 179;
            const int pstIdleTime = 180;
            const int pstCycles = 181;
            const int pstFrequency = 2;
            const int pstGasInjectionRate = 191;

            const int applicationRodPump = 3;
            const int applicationESP = 4;
            const int applicationGL = 7;

            var startDate = DateTime.UtcNow.Date.AddDays(-numberOfDays);
            var endDate = DateTime.UtcNow;

            var assetsCollection = _database.GetCollection<MongoAssetCollection>(COLLECTION_ASSET_NAME);
            var customerCollection = _database.GetCollection<Customer>(COLLECTION_CUSTOMER_NAME); 
            var parametersCollection = _database.GetCollection<MongoParameters>(COLLECTION_MASTERVARIABLES_NAME);

            // Fetch asset data for the given node IDs
            var assetFilter = Builders<MongoAssetCollection>.Filter.In(a => a.LegacyId["NodeId"], nodeIds);
            var assets = await assetsCollection.Find(assetFilter).ToListAsync();

            var customerObjIds = assets.Select(a => a.CustomerId).Distinct().ToList();
            var customerBuilder = Builders<Customer>.Filter;
            var filterCustomer = customerBuilder.In(x => x.Id ,customerObjIds);
            var customers = await customerCollection.Find(filterCustomer).ToListAsync();

            if (!assets.Any())
            {
                return new DowntimeByWellsModel
                {
                    RodPump = new List<DowntimeByWellsRodPumpModel>(),
                    ESP = new List<DowntimeByWellsValueModel>(),
                    GL = new List<DowntimeByWellsValueModel>()
                };
            }

            // Extract asset GUIDs
            var assetGuids = assets
                .Select(a => Guid.TryParse(a.LegacyId?["AssetGUID"], out var guid) ? guid : Guid.Empty)
            .Where(g => g != Guid.Empty)
            .ToList();

            var customerGUIDs = customers
                .Select(c => Guid.TryParse(c.LegacyId?["CustomerGUID"], out var guid) ? guid : Guid.Empty)
                .Where(g => g != Guid.Empty)
                .ToList();

            if (!assetGuids.Any())
            {
                return new DowntimeByWellsModel
                {
                    RodPump = new List<DowntimeByWellsRodPumpModel>(),
                    ESP = new List<DowntimeByWellsValueModel>(),
                    GL = new List<DowntimeByWellsValueModel>()
                };
            }

            // Fetch parameters for the given node IDs
            var parameterFilter = Builders<MongoParameters>.Filter.And(
               Builders<MongoParameters>.Filter.Eq(p => p.Enabled, true),
               Builders<MongoParameters>.Filter.In(p => p.ParamStandardType.LegacyId["ParamStandardTypesId"],
             new[] { pstRunTime.ToString(), pstIdleTime.ToString(), pstCycles.ToString(), pstFrequency.ToString(), pstGasInjectionRate.ToString() }),
               Builders<MongoParameters>.Filter.In(p => p.Address, new[] { pstRunTime, pstIdleTime, pstCycles, pstFrequency, pstGasInjectionRate }) // Add Address filter
        );

            var parameters = await parametersCollection.Find(parameterFilter).ToListAsync();

            if (!parameters.Any())
            {
                return new DowntimeByWellsModel
                {
                    RodPump = new List<DowntimeByWellsRodPumpModel>(),
                    ESP = new List<DowntimeByWellsValueModel>(),
                    GL = new List<DowntimeByWellsValueModel>()
                };
            }

            // Prepare downtime filters
            var downtimeFilters = parameters.Select(p => new DowntimeFiltersInflux
            {
                AssetIds = assetGuids,
                CustomerIds = customerGUIDs,
                ParamStandardType = new List<string> { p.ParamStandardType.LegacyId["ParamStandardTypesId"] }
            }).ToList();

            // Fetch downtime data using the GetDowntimeAsync method
            var downtimeData = await _getDataHistoryItemsService.GetDowntimeAsync(downtimeFilters, startDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss"), endDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss"));

            if (!downtimeData.Any())
            {
                return new DowntimeByWellsModel
                {
                    RodPump = new List<DowntimeByWellsRodPumpModel>(),
                    ESP = new List<DowntimeByWellsValueModel>(),
                    GL = new List<DowntimeByWellsValueModel>()
                };
            }

            var rodPumpResult = downtimeData
                .Where(d => d.ParamStandardType == pstRunTime.ToString() || d.ParamStandardType == pstIdleTime.ToString() || d.ParamStandardType == pstCycles.ToString())
                .GroupBy(d => new { d.Id, d.Date })
                .Select(g => new DowntimeByWellsRodPumpModel
                {
                    Id = g.Key.Id.ToString(),
                    Date = g.Key.Date,
                    Runtime = g.Where(d => d.ParamStandardType == pstRunTime.ToString()).Sum(d => d.Value),
                    IdleTime = g.Where(d => d.ParamStandardType == pstIdleTime.ToString()).Sum(d => d.Value),
                    Cycles = g.Where(d => d.ParamStandardType == pstCycles.ToString()).Sum(d => d.Value)
                })
                .ToList();

            var espResult = downtimeData
                .Where(d => d.ParamStandardType == pstFrequency.ToString())
                .Select(d => new DowntimeByWellsValueModel
                {
                    Id = d.Id.ToString(),
                    Date = d.Date,
                    Value = d.Value
                })
                .ToList();

            // Process downtime data for GL
            var glResult = downtimeData
                .Where(d => d.ParamStandardType == pstGasInjectionRate.ToString())
                .Select(d => new DowntimeByWellsValueModel
                {
                    Id = d.Id.ToString(),
                    Date = d.Date,
                    Value = d.Value
                })
                .ToList();

            foreach (var asset in assets)
            {
                var pocType = asset.POCType?.LegacyId?["POCTypesId"];
                Guid assetGuid = Guid.TryParse(asset.LegacyId?["AssetGUID"], out var ag) ? ag : Guid.Empty;

                if (assetGuid == Guid.Empty || string.IsNullOrEmpty(pocType))
                {
                    continue;
                }
                int appId = Convert.ToInt32(asset.LegacyId?["AssetGUID"]);

                if (appId == applicationRodPump)
                {
                    // Fetch data for Rod Pump
                    var grouped = downtimeData.GroupBy(x => new { x.Id, x.Date });

                    foreach (var g in grouped)
                    {
                        var runtime = g.FirstOrDefault(x => x.Id == pstRunTime.ToString())?.Value ?? 0;
                        var idle = g.FirstOrDefault(x => x.Id == pstIdleTime.ToString())?.Value ?? 0;
                        var cycles = g.FirstOrDefault(x => x.Id == pstCycles.ToString())?.Value ?? 0;

                        if (runtime > 0)
                        {
                            rodPumpResult.Add(new DowntimeByWellsRodPumpModel
                            {
                                Id = g.Key.Id.ToString(),
                                Runtime = (float)runtime,
                                IdleTime = (float)idle,
                                Cycles = (float)cycles,
                                Date = g.Key.Date
                            });
                        }
                    }
                }
                else if (appId == applicationESP)
                {
                    // Fetch data for ESP
                   
                    espResult.AddRange(downtimeData.Select(d => new DowntimeByWellsValueModel
                    {
                        Id = d.Id.ToString(),
                        Value = (float)d.Value,
                        Date = d.Date
                    }));
                }
                else if (appId == applicationGL)
                {
                    // Fetch data for GL
                    
                    glResult.AddRange(downtimeData.Select(d => new DowntimeByWellsValueModel
                    {
                        Id = d.Id.ToString(),
                        Value = (float)d.Value,
                        Date = d.Date
                    }));
                }
            }

            // Combine results into the final model
            return new DowntimeByWellsModel
            {
                RodPump = rodPumpResult,
                ESP = espResult,
                GL = glResult
            };
        }

        /// <summary>
        /// Gets the <seealso cref="IList{MeasurementTrendDataModel}"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="correlationId"></param>
        /// <param name="paramStandardType"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>The <seealso cref="IList{MeasurementTrendDataModel}"/>.</returns>
        public async Task<IList<DataPointModel>> GetMeasurementTrendData(string nodeId, int paramStandardType, DateTime startDate, DateTime endDate, string correlationId)
        {
            var parametersCollection = _database.GetCollection<MongoParameters>(COLLECTION_MASTERVARIABLES_NAME);
            var assetsCollection = _database.GetCollection<MongoAssetCollection>(COLLECTION_ASSET_NAME);
            var customerCollection = _database.GetCollection<Customer>(COLLECTION_CUSTOMER_NAME);
            var builderParam = Builders<MongoParameters>.Filter;
            var filterParam = builderParam.Empty;

            var assetData = await assetsCollection.Find(x => x.LegacyId["NodeId"] == nodeId).FirstOrDefaultAsync();
            string poctype = assetData?.POCType.LegacyId["POCTypesId"];
            // Get Asset Id from Asset.
            var assetObjId = assetData?.Id;

            // Get NodeId from Asset.
            Guid assetGUID = Guid.TryParse(assetData?.LegacyId?["AssetGUID"], out var parsedGuid)
                     ? parsedGuid
                     : Guid.Empty;

            if (assetGUID == Guid.Empty)
            {
                return Enumerable.Empty<DataPointModel>().ToList();
            }
            #region Fetch CustomerGUID
            // Get Asset Id from Asset.
            var customerObjId = assetData?.CustomerId;

            // Get Customer
            var customerBuilder = Builders<Customer>.Filter;
            var filterCustomer = customerBuilder.Eq(x => x.Id, customerObjId);
            var customerData = customerCollection.Find(filterCustomer).FirstOrDefault();

            // Get customerGUID from Customer.
            Guid customerGUID = Guid.TryParse(customerData?.LegacyId?["CustomerGUID"], out var parsedCustomerGuid)
                ? parsedCustomerGuid
                : Guid.Empty;

            #endregion
            IList<DataPointModel> dataHistories = await _getDataHistoryItemsService.GetDataHistoryItems(assetGUID, customerGUID, poctype, null, 
                new List<string> { paramStandardType.ToString() }, startDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss"), endDate.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss"));

            return dataHistories;
        }

        /// <summary>
        /// Gets the <seealso cref="IList{MeasurementTrendItemModel}"/> from MongoDB.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <returns>The <seealso cref="IList{MeasurementTrendDataModel}"/>.</returns>
        public async Task<IList<DataPointModel>> GetMeasurementTrendItems(string nodeId)
        {
            #region Mongo Collections
            var parametersCollection = _database.GetCollection<MongoParameters>(COLLECTION_MASTERVARIABLES_NAME);
            var assetCollection = _database.GetCollection<MongoAssetCollection>(COLLECTION_ASSET_NAME);
            var customerCollection = _database.GetCollection<Customer>(COLLECTION_CUSTOMER_NAME);
            #endregion

            IList<DataPointModel> dataModels = new List<DataPointModel>();
            string cacheKey = string.Empty;

            #region Fetch AssetGUID
            var asset = await assetCollection.Find(x => x.LegacyId["NodeId"] == nodeId).FirstOrDefaultAsync();
            
            if (asset == null)
            {
                return dataModels;
            }
           
            Guid assetGUID = Guid.TryParse(asset?.LegacyId?["AssetGUID"], out var parsedGuid)
                 ? parsedGuid
                 : Guid.Empty;

            // Get Asset Id from Asset.
            var assetObjId = asset?.Id;

            #endregion

            #region Fetch CustomerGUID
            // Get Asset Id from Asset.
            var customerObjId = asset?.CustomerId;

            // Get Customer
            var customerBuilder = Builders<Customer>.Filter;
            var filterCustomer = customerBuilder.Eq(x => x.Id, customerObjId);
            var customerData = customerCollection.Find(filterCustomer).FirstOrDefault();

            // Get customerGUID from Customer.
            Guid customerGUID = Guid.TryParse(customerData?.LegacyId?["CustomerGUID"], out var parsedCustomerGuid)
                ? parsedCustomerGuid
                : Guid.Empty;

            #endregion

            #region FetchPamaters

            string poctypeStr = asset?.POCType.LegacyId["POCTypesId"];
           

            if (!short.TryParse(poctypeStr, out var poctype))
            {
                poctype = 99; // or handle this scenario appropriately
            }

            var nodePocTypes = new List<short> { poctype };
            var nodePocTypeCase = poctype == 17 ? 8 : 0;
            var poctype99Filter = Builders<MongoParameters>.Filter.And(
                //Builders<MongoParameters>.Filter.Eq(p => p.Enabled, true),
               Builders<MongoParameters>.Filter.Ne(p => p.ParamStandardType, null),
               (poctype == 99)
                    ? Builders<MongoParameters>.Filter.Empty
                    : Builders<MongoParameters>.Filter.Eq(p => p.LegacyId["POCType"], poctypeStr)
                );
           
            // Fetch Parameters (excluding Facility tags)
            var parameters = await parametersCollection
                .Find(poctype99Filter)
                .ToListAsync();

            #endregion
            
            var filterParam = Builders<MongoParameters>.Filter.And(
                Builders<MongoParameters>.Filter.Eq(p => p.ParameterType, "Facility")
               );

            var builderParam = Builders<MongoParameters>.Filter;
            var filterParamForMv = builderParam.Empty;

            if (string.IsNullOrWhiteSpace(poctypeStr))
            {
                filterParamForMv &= builderParam.Eq(x => x.LegacyId["POCType"], "99");
            }
            else
            {
                // add poc type filter (also add poctype 99)
                filterParam &= builderParam.Or(
                    builderParam.Eq(x => x.LegacyId["POCType"], poctypeStr),
                    builderParam.Eq(x => x.LegacyId["POCType"], "99")
                );
            }

            if (parameters == null)
            {
                parameters = new List<MongoParameters> { };
            }
            else
            {
                // get distinct data by channelId.
                parameters = parameters
                    .OrderByDescending(x => GetBitPriority(x.LegacyId))
                    .DistinctBy(x => x.ChannelId)
                    .ToList();
            }
            var mvChannelIds = parameters.Select(x => x.ChannelId).Distinct().ToList();

            #region Fetch DataHistory From Influx

            // Get distinct addresses from DataHistory
            var resultDataHistory = await _getDataHistoryItemsService.GetDataHistoryTrendData(
                assetGUID, customerGUID, poctypeStr,mvChannelIds, DateTime.MinValue.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss"), DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss"));
            #endregion

            return resultDataHistory;
        }

        /// <summary>
        /// Get Asset Mongo Collection.
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public async Task<MongoAssetCollection> GetAssetAsync(string nodeId)
        {
                var assetsCollection = _database.GetCollection<MongoAssetCollection>(COLLECTION_ASSET_NAME);
                var assetData = await assetsCollection.Find(x => x.LegacyId["NodeId"] == nodeId).FirstOrDefaultAsync();
                if (assetData == null)
                {
                    return null;
                }
                return assetData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelIds"></param>
        /// <param name="nodeId"></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        public async Task<IDictionary<(int Address, string ChannelId), MongoParameters>> GetParametersBulk(List<string> channelIds,string nodeId, string correlationId)
        {
            await Task.Yield();

            try
            {
                var assetCollection = _database.GetCollection<MongoAssetCollection>(COLLECTION_ASSET_NAME);
                var parameterCollection = _database.GetCollection<MongoParameters>(COLLECTION_MASTERVARIABLES_NAME);
                var asset = await assetCollection.Find(x => x.LegacyId["NodeId"] == nodeId).FirstOrDefaultAsync();
                string poctypeStr = asset?.POCType.LegacyId["POCTypesId"];
                if (!short.TryParse(poctypeStr, out var poctype))
                {
                    poctype = 99; // or handle this scenario appropriately
                }
                // Build an Or filter for all POCType and ChannelId pairs
                var filters = 
                    Builders<MongoParameters>.Filter.And(
                        Builders<MongoParameters>.Filter.Eq("LegacyId.POCType", poctype),
                        Builders<MongoParameters>.Filter.In("ChannelId", channelIds)
                );
                
                var filter = Builders<MongoParameters>.Filter.Or(filters);

                var parameterDocs = await parameterCollection.Find<MongoParameters>(filter).ToListAsync();

                if (parameterDocs == null || !parameterDocs.Any())
                {
                    return new Dictionary<(int POCType, string ChannelId), MongoParameters>();
                }

                // Map results to a dictionary for quick lookup
                var parameterDict = parameterDocs
                    .GroupBy(param => (param.Address, param.ChannelId))
                    .ToDictionary(
                        group => group.Key,
                        group => group.First()
                    );

                return parameterDict;
            }
            catch (Exception)
            {
                return new Dictionary<(int Address, string ChannelId), MongoParameters>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelIds"></param>
        /// <param name="pocType    "></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        public async Task<IDictionary<(int Address, string ChannelId), MongoParameters>> GetParametersBulk(List<string> channelIds, int pocType, string correlationId)
        {
            await Task.Yield();

            try
            {
                var parameterCollection = _database.GetCollection<MongoParameters>(COLLECTION_MASTERVARIABLES_NAME);

                // Build an Or filter for all POCType and ChannelId pairs
                var filters =
                     Builders<MongoParameters>.Filter.And(
                         Builders<MongoParameters>.Filter.Eq("LegacyId.POCType", pocType.ToString()),
                         Builders<MongoParameters>.Filter.In("ChannelId", channelIds)
                 );

                var filter = Builders<MongoParameters>.Filter.Or(filters);

                var parameterDocs = await parameterCollection.Find<MongoParameters>(filter).ToListAsync();

                if (parameterDocs == null || !parameterDocs.Any())
                {
                    return new Dictionary<(int POCType, string ChannelId), MongoParameters>();
                }

                // Map results to a dictionary for quick lookup
                var parameterDict = parameterDocs
                    .GroupBy(param => (param.Address, param.ChannelId))
                    .ToDictionary(
                        group => group.Key,
                        group => group.First()
                    );

                return parameterDict;
            }
            catch (Exception)
            {
                return new Dictionary<(int Address, string ChannelId), MongoParameters>();
            }
        }

        private float? ConvertToFloat(object value)
        {
            if (value == null)
            {
                return null;
            }

            if (value is float f)
            {
                return f;
            }

            if (value is double d)
            {
                return (float)d;
            }

            if (float.TryParse(value.ToString(), out var result))
            {
                return result;
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
    }
}
