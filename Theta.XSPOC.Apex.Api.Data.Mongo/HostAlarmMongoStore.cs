using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Alarms;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Enums;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// Represents a Mongo store for Host Alarms entities.
    /// </summary>
    public class HostAlarmMongoStore : MongoOperations, IHostAlarm
    {

        #region Private Constants

        private const string ALARMS_COLLECTION = "AlarmConfiguration";
        private const string ASSET_COLLECTION = "Asset";
        private const string PARAMETER_COLLECTION = "MasterVariables";

        #endregion

        #region Private Fields

        private readonly IMongoDatabase _database;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="HostAlarmMongoStore"/> using the provided 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public HostAlarmMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory)
            : base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IHostAlarm Implementation

        /// <summary>
        /// Get the IHostAlarm limits for data history.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="addresses">The addresses.</param>
        /// <param name="correlationId"></param>
        /// <returns>The List of <seealso cref="HostAlarmForDataHistoryModel"/>.</returns>
        public IList<HostAlarmForDataHistoryModel> GetLimitsForDataHistory(Guid assetId, int[] addresses, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(HostAlarmMongoStore)} {nameof(GetLimitsForDataHistory)}", correlationId);

            var responseData = new List<HostAlarmForDataHistoryModel>();
            try
            {
                var assetFilter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
                   .Where(x => (x.LegacyId.ContainsKey("AssetGUID") && x.LegacyId["AssetGUID"] != null &&
                              (x.LegacyId["AssetGUID"].ToLower() == assetId.ToString().ToLower())));

                var assetData = Find<MongoAssetCollection.Asset>(ASSET_COLLECTION, assetFilter, correlationId);

                if (assetData == null || assetData.Count == 0)
                {
                    logger.WriteCId(Level.Info, "Missing node", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(HostAlarmMongoStore)} {nameof(GetLimitsForDataHistory)}", correlationId);

                    return null;
                }
                else
                {
                    var nodeId = assetData.FirstOrDefault()?.LegacyId["NodeId"];

                    var hostAlarmFilter = new FilterDefinitionBuilder<AlarmConfiguration>()
                        .Where(x => x.AlarmType == AlarmTypesEnum.HostAlarm.ToString()
                        && (x.LegacyId.ContainsKey("NodeId") && x.LegacyId["NodeId"] != null &&
                        (x.LegacyId["NodeId"] == nodeId)) && !string.IsNullOrEmpty(x.LegacyId["Address"])
                        && int.Parse(x.LegacyId["AlarmType"]) <= 3 && addresses.Contains(int.Parse(x.LegacyId["Address"])));

                    var hostAlarmData = Find<AlarmConfiguration>(ALARMS_COLLECTION, hostAlarmFilter, correlationId);

                    if (hostAlarmData == null || hostAlarmData.Count == 0)
                    {
                        logger.WriteCId(Level.Info, "Missing host alarm data", correlationId);
                        logger.WriteCId(Level.Trace, $"Finished {nameof(HostAlarmMongoStore)} {nameof(GetLimitsForDataHistory)}", correlationId);

                        return null;
                    }
                    else
                    {
                        var hostAlarms = hostAlarmData
                        .Select(a => new
                        {
                            Address = int.Parse(a.LegacyId["Address"]),
                            AlarmType = int.Parse(a.LegacyId["AlarmType"]),
                            HiHiLimit = (float?)((HostAlarm)a.Document).HiHiLimit,
                            HiLimit = (float?)a.HiLimit,
                            LoLimit = (float?)a.LoLimit,
                            LoLoLimit = (float?)((HostAlarm)a.Document).LoLoLimit,
                        }).ToList();

                        responseData = hostAlarms.GroupBy(x => x.Address)
                          .Select(x => x.OrderByDescending(o => o.AlarmType)
                          .FirstOrDefault()).ToList()
                          .Select(x => new HostAlarmForDataHistoryModel()
                          {
                              Address = x.Address,
                              AlarmType = x.AlarmType,
                              HiHiLimit = x.HiHiLimit,
                              HiLimit = x.HiLimit,
                              LoLimit = x.LoLimit,
                              LoLoLimit = x.LoLoLimit,
                          }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(HostAlarmMongoStore)} {nameof(GetLimitsForDataHistory)}", correlationId);

            return responseData;
        }

        /// <summary>
        /// Get all host alarms for group status.
        /// </summary>
        /// <param name="nodeIds">The list of node ids.</param>
        /// <param name="correlationId"></param>
        /// <returns>The List of <seealso cref="HostAlarmForGroupStatus"/>.</returns>
        public IList<HostAlarmForGroupStatus> GetAllGroupStatusHostAlarms(IList<string> nodeIds, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(HostAlarmMongoStore)} {nameof(GetAllGroupStatusHostAlarms)}", correlationId);

            var hostAlarms = new List<HostAlarmForGroupStatus>();

            var hostAlarmFilter = new FilterDefinitionBuilder<AlarmConfiguration>()
                .Where(x => x.AlarmType == AlarmTypesEnum.HostAlarm.ToString() && x.Enabled == true
                && (x.LegacyId.ContainsKey("NodeId") && x.LegacyId["NodeId"] != null &&
                (nodeIds.Contains(x.LegacyId["NodeId"]))) && ((HostAlarm)x.Document).AlarmState > 0);

            var hostAlarmData = Find<AlarmConfiguration>(ALARMS_COLLECTION, hostAlarmFilter, correlationId);

            if (hostAlarmData == null || hostAlarmData.Count == 0)
            {
                logger.WriteCId(Level.Info, "Missing host alarm data", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(HostAlarmMongoStore)} {nameof(GetAllGroupStatusHostAlarms)}", correlationId);

                return null;
            }
            else
            {
                var nodeIdWithHostAlarm = hostAlarmData.Select(x => x.LegacyId["NodeId"]).ToArray();

                var nodeMasterFilter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
                    .Where(x => (x.LegacyId.ContainsKey("NodeId") && x.LegacyId["NodeId"] != null &&
                                       (nodeIdWithHostAlarm.Contains(x.LegacyId["NodeId"]))));

                var nodeMasterData = Find<MongoAssetCollection.Asset>(ASSET_COLLECTION, nodeMasterFilter, correlationId);

                var nodeObjId = nodeMasterData.Select(x => x.Id).ToArray();

                var hostAlarmAddress = hostAlarmData.Select(x => int.Parse(x.LegacyId["Address"])).Distinct().ToArray();

                var poctypes = nodeMasterData.Select(x => x.POCType.LegacyId["POCTypesId"]).Distinct().ToArray();

                var parameters = new FilterDefinitionBuilder<Parameters>()
                    .Where(x => x.ParameterType == "Parameter" && hostAlarmAddress.Contains(x.Address)
                    && (poctypes.Contains(x.POCType.LegacyId["POCTypesId"]) || x.POCType.LegacyId["POCTypesId"] == "99"));

                var parameterData = Find<Parameters>(PARAMETER_COLLECTION, parameters, correlationId);

                var facilityTags = new FilterDefinitionBuilder<Parameters>()
                    .Where(x => x.ParameterType == "FacilityTag" && hostAlarmAddress.Contains(x.Address)
                    && nodeObjId.Contains(((FacilityTagDetails)x.ParameterDocument).TrendToAsset));

                var facilityTagsData = Find<Parameters>(PARAMETER_COLLECTION, parameters, correlationId);

                hostAlarms = hostAlarmData.Select(x => new HostAlarmForGroupStatus()
                {
                    NodeId = x.LegacyId["NodeId"],
                    Address = int.Parse(x.LegacyId["Address"]),
                    AlarmState = ((HostAlarm)x.Document).AlarmState,
                    AlarmType = ((HostAlarm)x.Document).HostAlarmTypeId != null ? (int)((HostAlarm)x.Document).HostAlarmTypeId : 0,
                    Priority = x.Priority,
                    ValueChange = (float?)((HostAlarm)x.Document).ValueChange,
                    PercentChange = ((HostAlarm)x.Document).PercentChange,
                    LoLimit = (float?)x.LoLimit,
                    HiLimit = (float?)x.HiLimit,
                    LoLoLimit = (float?)((HostAlarm)x.Document).LoLoLimit,
                    HiHiLimit = (float?)((HostAlarm)x.Document).HiHiLimit,
                    MinToMaxLimit = ((HostAlarm)x.Document).MinToMaxLimit,
                    LastStateChange = ((HostAlarm)x.Document).LastStateChange,
                    AlarmDescription = string.Empty,
                    AssetId = ((HostAlarm)x.Document).AssetId,
                }).ToList();

                foreach (var hostAlarm in hostAlarms)
                {
                    Parameters parameter = new Parameters();
                    var poctype = nodeMasterData.FirstOrDefault(x => x.LegacyId["NodeId"] == hostAlarm.NodeId)?.POCType.LegacyId["POCTypesId"];
                    if (poctype != null)
                    {
                        parameter = parameterData.FirstOrDefault(x => x.Address == hostAlarm.Address && x.LegacyId["POCTypesId"] == poctype);
                    }

                    var facilityTag = facilityTagsData.FirstOrDefault(x => x.Address == hostAlarm.Address
                    && ((FacilityTagDetails)x.ParameterDocument).AssetId == hostAlarm.AssetId);

                    hostAlarm.AlarmDescription = facilityTag?.Description ?? parameter?.Description;
                }
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(HostAlarmMongoStore)} {nameof(GetAllGroupStatusHostAlarms)}", correlationId);

            return hostAlarms.ToList();
        }

        #endregion

    }
}
