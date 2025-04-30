
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup;
using Theta.XSPOC.Apex.Api.Data.Influx.Services;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter;
using Theta.XSPOC.Apex.Kernel.Logging;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset.Asset;
using Parameters = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter.Parameters;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// Mongo Implementation of the IDataHistorySQLStore interface's methods which has DataHistory Table in use. 
    /// </summary>
    public class DataHistoryMongoStore : MongoOperations, IDataHistoryMongoStore
    {
        #region Private Fields

        private readonly IMongoDatabase _database;
        private readonly IThetaLoggerFactory _loggerFactory;
        private readonly IGetDataHistoryItemsService _getDataHistoryItemsService;

        #endregion

        /// <summary>
        /// Constructs a new <seealso cref="ParameterDataTypeMongoStore"/> using the provided 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        /// <param name="getDataHistoryItemsService">The service to fetch data for DataHistory using Influx.</param>
        public DataHistoryMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory, IGetDataHistoryItemsService getDataHistoryItemsService) :
            base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _getDataHistoryItemsService = getDataHistoryItemsService ?? throw new ArgumentNullException(nameof(getDataHistoryItemsService));
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
            var parametersCollection = _database.GetCollection<Parameters>("Parameters");
            var assetsCollection = _database.GetCollection<MongoAssetCollection>("Asset");

            // Fetch asset data
            var assetData = await assetsCollection.Find(x => x.LegacyId["NodeId"] == nodeId).FirstOrDefaultAsync();
            string poctype = assetData?.POCType.LegacyId["POCTypesId"];

            // Filter for parameters
            var parameterFilter = Builders<Parameters>.Filter.And(
                Builders<Parameters>.Filter.Eq(p => p.LegacyId["NodeId"], nodeId),
                Builders<Parameters>.Filter.Eq(p => p.Address, address),
                Builders<Parameters>.Filter.Eq(p => p.Enabled, true)
            );

            var parameters = await parametersCollection.Find(parameterFilter).ToListAsync();
            var parameterList = parameters.Select(p => new
            {
                p.Address,
                p.ChannelId
            }).ToList();

            // Get NodeId from Asset
            Guid assetGUID = Guid.TryParse(assetData?.LegacyId?["AssetGUID"], out var parsedGuid)
                ? parsedGuid
                : Guid.Empty;

            if (assetGUID == Guid.Empty)
            {
                return Enumerable.Empty<ControllerTrendDataModel>().ToList();
            }

            // Fetch data history from Influx
            IList<DataPointModel> dataHistories = await _getDataHistoryItemsService.GetDataHistoryItems(
                assetGUID, Guid.Empty, poctype, new List<string> { address.ToString() }, null, startDate.ToString(), endDate.ToString());

            // Join parameters with data history
            var joinedResult = (from p in parameterList
                                join dh in dataHistories on p.ChannelId equals dh.ChannelId
                                orderby dh.Time
                                select new ControllerTrendDataModel
                                {
                                    Date = dh.Time,
                                    Value = ConvertToFloat(dh.Value) is float v ? (v > maxDecimal ? maxDecimal : v) : 0f
                                }).Distinct().ToList();

            return joinedResult;
        }

        /// <summary>
        /// Get the controller trend item by node id and poc type.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="pocType">The poc type.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="ControllerTrendItemModel"/>.</returns>
        public IList<ControllerTrendItemModel> GetControllerTrendItems(string nodeId, int pocType, string correlationId)
        {
            var parametersCollection = _database.GetCollection<Parameters>("Parameters");
            var assetsCollection = _database.GetCollection<MongoAssetCollection>("Asset");

            // Fetch asset data
            var assetData = assetsCollection.Find(x => x.LegacyId["NodeId"] == nodeId).FirstOrDefault();
            if (assetData == null)
            {
                return Enumerable.Empty<ControllerTrendItemModel>().ToList();
            }

            string poctype = assetData.POCType.LegacyId["POCTypesId"];

            // Filter for facility tags
            var facilityTagFilter = Builders<Parameters>.Filter.And(
                Builders<Parameters>.Filter.Eq(p => p.ParameterType, "Facility"),
                Builders<Parameters>.Filter.Eq(p => p.Enabled, true),
                Builders<Parameters>.Filter.Eq(p => p.LegacyId["NodeId"], nodeId),
                Builders<Parameters>.Filter.Ne(p => p.ParamStandardType, null)
            );

            var facilityTags = parametersCollection.Find(facilityTagFilter).ToList();
            var facilityTagAddresses = facilityTags.Select(ft => ft.Address).ToList();
            bool facilityTagsExist = facilityTagAddresses.Any();

            // Filter for parameters
            var parameterFilter = Builders<Parameters>.Filter.And(
                Builders<Parameters>.Filter.Eq(p => p.ParamStandardType, null),
                Builders<Parameters>.Filter.Or(
                    Builders<Parameters>.Filter.Eq(p => p.LegacyId["POCType"], poctype),
                    Builders<Parameters>.Filter.Eq(p => p.LegacyId["POCType"], "99")
                )
            );

            var parameters = parametersCollection.Find(parameterFilter).ToList();

            // Map parameters to ControllerTrendItemModel
            var parameterItems = parameters.Select(p => new ControllerTrendItemModel
            {
                Name = p.Description,
                Description = p.Description,
                Address = p.Address,
                UnitType = int.TryParse(p.UnitType?.Id, out var unitTypeId) ? unitTypeId : 0, // Safely parse the string to an int
                FacilityTag = 0,
                Tag = null
            }).ToList();

            // Map facility tags to ControllerTrendItemModel
            var facilityTagItems = facilityTags.Select(ft => new ControllerTrendItemModel
            {
                Name = ft.Description,
                Description = ft.Description ?? string.Empty,
                Address = ft.Address,
                UnitType = int.TryParse(ft.UnitType?.Id, out var unitTypeId) ? unitTypeId : 0, // Safely parse the string to an int
                FacilityTag = 1,
                Tag = null
            }).ToList();

            // Combine parameters and facility tags
            var combinedItems = parameterItems.Concat(facilityTagItems).ToList();

            // Fetch data history from Influx
            var dataHistoryAddresses = _getDataHistoryItemsService.GetDataHistoryItems(
                Guid.Parse(assetData.LegacyId["AssetGUID"]),
                Guid.Empty,
                poctype,
                combinedItems.Select(item => item.Address.ToString()).ToList(),
                null,
                null,
                null
            ).Result.Select(dh => int.Parse(dh.ChannelId)).ToList();

            // Filter combined items by data history addresses
            var filteredItems = combinedItems.Where(item => dataHistoryAddresses.Contains(item.Address)).ToList();

            return filteredItems;
        }

        /// <summary>
        /// Gets the downtime by wells.
        /// </summary>
        /// <param name="nodeIds">The node ids.</param>
        /// <param name="numberOfDays">The number of days.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <see cref="DowntimeByWellsModel"/>.</returns>
        public DowntimeByWellsModel GetDowntime(IList<string> nodeIds, int numberOfDays, string correlationId)
        {
            throw new NotImplementedException();
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
        public async Task<IList<MeasurementTrendDataModel>> GetMeasurementTrendData(string nodeId, int paramStandardType, DateTime startDate, DateTime endDate, string correlationId)
        {
            float maxDecimal = 9999999999999999999999999999f;
            var parametersCollection = _database.GetCollection<Parameters>("Parameters");
            var assetsCollection = _database.GetCollection<MongoAssetCollection>("Asset");
            var builderParam = Builders<Parameters>.Filter;
            var filterParam = builderParam.Empty;

            var assetData = await assetsCollection.Find(x => x.LegacyId["NodeId"] == nodeId).FirstOrDefaultAsync();
            string poctype = assetData?.POCType.LegacyId["POCTypesId"];

            filterParam = Builders<Parameters>.Filter.And(
                 Builders<Parameters>.Filter.Eq(p => p.ParameterType, "Facility"),
                 Builders<Parameters>.Filter.Eq(p => p.Enabled, true),
                 Builders<Parameters>.Filter.Eq(p => p.LegacyId["NodeId"], nodeId),
                 Builders<Parameters>.Filter.Ne(p => p.ParamStandardType, null),
                 Builders<Parameters>.Filter.Eq(p => p.ParamStandardType.LegacyId["ParamStandardTypesId"], paramStandardType.ToString())
                );

            var facilityTags = await parametersCollection.Find(filterParam).ToListAsync();
            var facilityTagAddresses = facilityTags.Select(ft => ft.Address).ToList();

            bool facilityTagsExist = facilityTagAddresses.Any();

            var parameterFilter =
               Builders<Parameters>.Filter.And(
               Builders<Parameters>.Filter.Eq(p => p.ParamStandardType.LegacyId["ParamStandardTypesId"], paramStandardType.ToString()),

               Builders<Parameters>.Filter.Or(
               Builders<Parameters>.Filter.Eq(p => p.LegacyId["POCType"], poctype),
               Builders<Parameters>.Filter.Eq(p => p.LegacyId["POCType"], "99"),

               Builders<Parameters>.Filter.And(
               Builders<Parameters>.Filter.Eq(p => p.LegacyId["POCType"], "8"),
               Builders<Parameters>.Filter.Where(p => p.LegacyId["POCType"] == "17")
               )));

            var parameters = await parametersCollection.Find(parameterFilter).ToListAsync();
            var parametersList = parameters
                .Select(p => new
                {
                    p.Address,
                    p.ChannelId,
                    IsManual = (p.Address > 4000 && p.Address <= 5000) ? 1 : 0
                })
                .ToList();

            // Get NodeId from Asset.
            Guid assetGUID = Guid.TryParse(assetData?.LegacyId?["AssetGUID"], out var parsedGuid)
                     ? parsedGuid
                     : Guid.Empty;

            if (assetGUID != Guid.Empty)
            {
                return Enumerable.Empty<MeasurementTrendDataModel>().ToList();
            }

            IList<DataPointModel> dataHistories = await _getDataHistoryItemsService.GetDataHistoryItems(assetGUID, Guid.Empty, poctype, null, null, startDate.ToString(), endDate.ToString());

            var poctype99Filter = Builders<Parameters>.Filter.And(
                Builders<Parameters>.Filter.Eq(p => p.LegacyId["POCType"], "99"),
                Builders<Parameters>.Filter.Eq(p => p.ParamStandardType.LegacyId["ParamStandardTypesId"], paramStandardType.ToString())
            );

            var poctype99Parameters = await parametersCollection.Find(poctype99Filter).ToListAsync();
            var poctype99Addresses = poctype99Parameters.Select(p => p.Address).ToList();

            var addressList = parametersList
            .Concat(facilityTags.Select(ft => new
            {
                ft.Address,
                ft.ChannelId,
                IsManual = 0
            }))
            .ToList();

            var joinedResult = (from d in addressList
                                join dh in dataHistories on d.ChannelId equals dh.ChannelId
                                where !facilityTagsExist ||
                                      (facilityTagAddresses.Contains(d.Address) || poctype99Addresses.Contains(d.Address))
                                orderby dh.Time
                                select new MeasurementTrendDataModel
                                {
                                    Address = d.Address,
                                    Date = dh.Time,
                                    Value = ConvertToFloat(dh.Value) is float v ? (v > maxDecimal ? maxDecimal : v) : 0f,
                                    IsManual = d.IsManual == 1
                                })
                    .Distinct()
                    .ToList();

            return joinedResult;
        }

        /// <summary>
        /// Gets the <seealso cref="IList{MeasurementTrendItemModel}"/> from MongoDB.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="IList{MeasurementTrendItemModel}"/>.</returns>
        public async Task<IList<MeasurementTrendItemModel>> GetMeasurementTrendItems(string nodeId, string correlationId)
        {
            var parametersCollection = _database.GetCollection<Parameters>("Parameters");
            var assetsCollection = _database.GetCollection<MongoAssetCollection>("Asset");

            // Fetch asset data
            var assetData = await assetsCollection.Find(x => x.LegacyId["NodeId"] == nodeId).FirstOrDefaultAsync();
            if (assetData == null)
            {
                return Enumerable.Empty<MeasurementTrendItemModel>().ToList();
            }

            string pocType = assetData.POCType.LegacyId["POCTypesId"];

            // Filter for facility tags
            var facilityTagFilter = Builders<Parameters>.Filter.And(
                Builders<Parameters>.Filter.Eq(p => p.ParameterType, "Facility"),
                Builders<Parameters>.Filter.Eq(p => p.Enabled, true),
                Builders<Parameters>.Filter.Eq(p => p.LegacyId["NodeId"], nodeId),
                Builders<Parameters>.Filter.Ne(p => p.ParamStandardType, null)
            );

            var facilityTags = await parametersCollection.Find(facilityTagFilter).ToListAsync();
            var facilityTagAddresses = facilityTags.Select(ft => ft.Address).ToList();
            bool facilityTagsExist = facilityTagAddresses.Any();

            // Filter for parameters
            var parameterFilter = Builders<Parameters>.Filter.And(
                Builders<Parameters>.Filter.Ne(p => p.ParamStandardType, null),
                Builders<Parameters>.Filter.Or(
                    Builders<Parameters>.Filter.Eq(p => p.LegacyId["POCType"], pocType),
                    Builders<Parameters>.Filter.Eq(p => p.LegacyId["POCType"], "99"),
                    Builders<Parameters>.Filter.And(
                        Builders<Parameters>.Filter.Eq(p => p.LegacyId["POCType"], "8"),
                        Builders<Parameters>.Filter.Where(p => p.LegacyId["POCType"] == "17")
                    )
                )
            );

            var parameters = await parametersCollection.Find(parameterFilter).ToListAsync();

            // Map parameters and facility tags to MeasurementTrendItemModel
            var parameterItems = parameters.Select(p => new MeasurementTrendItemModel
            {
                ParamStandardType = p.ParamStandardType?.LegacyId["ParamStandardTypesId"] != null
                                    ? int.Parse(p.ParamStandardType.LegacyId["ParamStandardTypesId"])
                                    : null,
                Address = p.Address,
                Description = p.Description,
                PhraseID = (p.ParameterDocument as ParameterDetails)?.PhraseId,
                ParameterType = p.LegacyId["POCType"] == "99" ? "1" : "2"
            }).ToList();

            var facilityTagItems = facilityTags.Select(ft => new MeasurementTrendItemModel
            {
                ParamStandardType = ft.ParamStandardType?.LegacyId["ParamStandardTypesId"] != null
                                    ? int.Parse(ft.ParamStandardType.LegacyId["ParamStandardTypesId"])
                                    : null,
                Address = ft.Address,
                Description = ft.Description,
                PhraseID = null,
                ParameterType = "2"
            }).ToList();

            // Combine parameters and facility tags
            var combinedItems = parameterItems.Concat(facilityTagItems).ToList();

            // Fetch ParamStandardTypes for additional details
            var paramStandardTypesCollection = _database.GetCollection<ParamStandardTypes>("ParamStandardTypes");
            var paramStandardTypes = await paramStandardTypesCollection.Find(Builders<ParamStandardTypes>.Filter.Empty).ToListAsync();

            // Join combined items with ParamStandardTypes
            var resultData = (from item in combinedItems
                              join pst in paramStandardTypes on item.ParamStandardType equals pst.ParamStandardType
                              select new MeasurementTrendItemModel
                              {
                                  ParamStandardType = pst.ParamStandardType,
                                  Name = pst.Description,
                                  UnitTypeID = pst.UnitTypeId,
                                  Address = item.Address,
                                  Description = item.Description
                              }).ToList();

            // Group and order the result
            var measurementTrendItems = resultData.GroupBy(d => d.ParamStandardType)
                .SelectMany(g => g.Select((item, index) => new MeasurementTrendItemModel
                {
                    Name = item.Name,
                    ParamStandardType = item.ParamStandardType,
                    UnitTypeID = item.UnitTypeID,
                    Address = item.Address,
                    Description = item.Description
                }))
                .Distinct()
                .OrderBy(x => x.Name)
                .ThenBy(x => x.ParamStandardType)
                .ThenBy(x => x.Address)
                .ToList();

            return measurementTrendItems;
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
