using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Influx.Services;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset.Asset;
using MongoParameters = Theta.XSPOC.Apex.Kernel.Mongo.Models.Parameter.Parameters;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// </summary>
    public class CurrentRawScansMongoInflux : MongoOperations, ICurrentRawScansStore
    {
        private const string COLLECTION_NAME_ASSET = "Asset";
        private const string COLLECTION_NAME_MASTERVARIABLES = "MasterVariables";

        private readonly IMongoDatabase _database;
        private readonly IGetDataHistoryItemsService _getDataHistoryItemsService;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Constructs a new <seealso cref="ParameterDataTypeMongoStore"/> using the provided 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        /// <param name="getDataHistoryItemsService">The service to fetch data for DataHistory using Influx.</param>
        /// <param name="memoryCache">The memory cache.</param>
        public CurrentRawScansMongoInflux(IMongoDatabase database, IThetaLoggerFactory loggerFactory, IGetDataHistoryItemsService getDataHistoryItemsService, IMemoryCache memoryCache) :
            base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _getDataHistoryItemsService = getDataHistoryItemsService ?? throw new ArgumentNullException(nameof(getDataHistoryItemsService));
            _cache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        /// <summary>
        /// Gets the list of alarm configuration for the provided <paramref name="assetId"/>
        /// </summary>
        /// <param name="assetId">The asset id used to get the alarm configuration data.</param>
        /// <param name="customerId">The asset id used to get the alarm configuration data.</param>
        /// <param name="nodeId">The asset id used to get the alarm configuration data.</param>
        /// <returns>
        /// A <seealso cref="IList{AlarmConfig}"/> that contains the alarm configuration data for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then and empty list is returned. If
        /// <paramref name="customerId"/>
        /// </returns>
        public async Task<IList<CurrentRawScanDataInflux>> GetCurrentRawScanDataFromInflux(Guid assetId, Guid customerId, string nodeId)
        {

            var parameterCollection = _database.GetCollection<MongoParameters>(COLLECTION_NAME_MASTERVARIABLES);
            var assetsCollection = _database.GetCollection<MongoAssetCollection>(COLLECTION_NAME_ASSET);

            var builderParam = Builders<MongoParameters>.Filter;
            var filterParam = builderParam.Empty;

            var assetData = await assetsCollection.Find(x => x.LegacyId["AssetGUID"] == assetId.ToString()).FirstOrDefaultAsync();

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

            var mvData = parameterCollection.Find(mvFilterParam).ToList();
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
            List<DataPointModel> data = (await _getDataHistoryItemsService.GetCurrentRawScanData(assetId, customerId)).ToList();

            MapAddressToCurrentData(data, mvData, nodeId);

            var result = new List<CurrentRawScanDataInflux>();

            return result;
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

        /// <summary>
        /// Method to get the Current scan data for the <paramref />
        /// </summary>
        /// <param name="dataPoints">The dataPoints.</param>
        /// <param name="mvData">The mvData.</param>
        /// <param name="nodeId">The nodeId.</param>
        /// <returns>The <seealso cref="IList{CurrentRawScanDataInfluxModel}"/></returns>
        private IList<CurrentRawScanDataInflux> MapAddressToCurrentData(IList<DataPointModel> dataPoints, IList<MongoParameters> mvData, string nodeId)
        {
            var addressedMappedCurrentData = (from m in mvData
                                              join t in dataPoints on m.ChannelId equals t.ChannelId
                                              select new CurrentRawScanDataInflux
                                              {
                                                  NodeId = nodeId,
                                                  Address = m.Address,
                                                  Value = ConvertToFloat(t.Value)
                                              }).ToList();
            return addressedMappedCurrentData;
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
    }
}
