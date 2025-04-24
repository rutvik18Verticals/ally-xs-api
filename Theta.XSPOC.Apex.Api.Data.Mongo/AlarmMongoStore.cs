using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Alarms;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using Theta.XSPOC.Apex.Api.Core.Logging;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Alarms;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Enums;
using Theta.XSPOC.Apex.Api.Data.Influx.Services;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Camera;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Customers;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// Implementation of mongo operations related to Alarm.
    /// </summary>
    public class AlarmMongoStore : MongoOperations, IAlarmStore
    {
        #region Private Constants

        private const string COLLECTION_NAME_ASSET = "Asset";
        private const string COLLECTION_NAME_MASTERVARIABLES = "MasterVariables";
        private const string COLLECTION_NAME_ALARM_CONFIGURATION = "AlarmConfiguration";
        private const string COLLECTION_NAME_LOOKUP = "Lookup";
        private const string COLLECTION_NAME_NOTIFICATION = "Notification";
        private const string COLLECTION_NAME_CAMERA = "Cameras";
        private const string COLLECTION_NAME_CAMERA_ALARMS = "CameraAlarms";
        private const string COLLECTION_NAME_ALARMS_EVENTS = "AlarmEvents";
        private const string COLLECTION_NAME_CUSTOMER = "Customers";

        #endregion

        #region Private Fields

        private readonly IMongoDatabase _database;
        private readonly IThetaLoggerFactory _loggerFactory;
        private readonly IDataHistoryTrendData _influxDataStore;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="AlarmMongoStore"/> using the provided 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        /// <param name="influxDataStore">The Influx databse.</param>
        public AlarmMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory, IDataHistoryTrendData influxDataStore)
            : base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _influxDataStore = influxDataStore ?? throw new ArgumentNullException(nameof(influxDataStore));
        }
        #endregion

        #region IAlarmConfigRepository Implementation
        /// <summary>
        /// Gets the list of alarm configuration for the provided <paramref name="assetId"/>
        /// </summary>
        /// <param name="assetId">The asset id used to get the alarm configuration data.</param>
        /// <param name="customerId">The customer id.</param>
        /// <returns>
        /// A <seealso cref="IList{AlarmConfig}"/> that contains the alarm configuration data for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then and empty list is returned. If
        /// the <paramref name="assetId"/> is the default GUID then null is returned.
        /// </returns>
        public async Task<IList<AlarmData>> GetAlarmConfigurationAsync(Guid assetId, Guid customerId)
        {
            var correlationId = Guid.NewGuid().ToString();

            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(AlarmMongoStore)} {nameof(GetAlarmConfigurationAsync)}", correlationId);

            // await Task.Yield();

            if (assetId == Guid.Empty)
            {
                return new List<AlarmData>();
            }

            // Set Filter for Get Asset document.
            var assetfilter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
               .Where(x => (x.LegacyId.ContainsKey("AssetGUID") && x.LegacyId["AssetGUID"] != null &&
                          (x.LegacyId["AssetGUID"].ToUpper() == assetId.ToString().ToUpper())));

            // Get Asset Document
            var assetData = Find<MongoAssetCollection.Asset>(COLLECTION_NAME_ASSET, assetfilter, correlationId);

            // In case asset not found.
            if (assetData == null || assetData.Count == 0)
            {
                logger.Write(Level.Info, "Missing node");
                logger.Write(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetAlarmConfigurationAsync)}");

                return new List<AlarmData>();
            }

            // Get NodeId from Asset.
            var nodeId = assetData.FirstOrDefault()?.LegacyId["NodeId"];

            // Get POCType ID from Asset.
            var poctypesId = assetData.FirstOrDefault()?.POCType.LegacyId["POCTypesId"];

            // Set Filter for Get RTU AlarmConfiguration documents.
            var alarmConfigRtuFilter = new FilterDefinitionBuilder<AlarmConfiguration>()
                    .Where(x => x.AlarmType == AlarmTypesEnum.RTUAlarm.ToString() && x.POCType.LegacyId["POCTypesId"] == poctypesId
                    && x.Enabled == true);

            // Get AlarmConfiguration Documents
            var rtuAlarmConfigData = Find<AlarmConfiguration>(COLLECTION_NAME_ALARM_CONFIGURATION, alarmConfigRtuFilter, correlationId);

            // In case AlarmConfiguration not found.
            if (rtuAlarmConfigData == null || rtuAlarmConfigData.Count == 0)
            {
                logger.WriteCId(Level.Info, "Missing RTU Alarm Config", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetAlarmConfigurationAsync)}", correlationId);

                return new List<AlarmData>();
            }

            // Get RTU Alarm type Address
            var rtuAlarmAddress = rtuAlarmConfigData.Select(x => int.Parse(x.LegacyId["Register"])).Distinct().ToArray();

            // Set Filter for Get MasterVariable documents master variables (Parameters).
            var rtuAlarmMVFilter = new FilterDefinitionBuilder<Parameters>()
                    .Where(x => x.ParameterType == ParameterTypes.Param.ToString() && (x.POCType.LegacyId["POCTypesId"] == poctypesId || x.POCType.LegacyId["POCTypesId"] == "99")
                     && rtuAlarmAddress.Contains(x.Address));

            // Get MasterVariables (Facility) Documents
            var rtuAlarmMVData = Find<Parameters>(COLLECTION_NAME_MASTERVARIABLES, rtuAlarmMVFilter, correlationId);

            // In case MasterVariables not found.
            if (rtuAlarmMVData == null || rtuAlarmMVData.Count == 0)
            {
                logger.WriteCId(Level.Info, "Missing RTU Alarm tag (MasterVariables)", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetAlarmConfigurationAsync)}", correlationId);

                return new List<AlarmData>();
            }

            // Get RTU ChannleIds and their data from Influx.
            var rtuChannelIds = rtuAlarmMVData.Select(x => x.ChannelId).Distinct().ToList();
            var latestTrendDataRecords = await _influxDataStore.GetLatestTrendData(assetId, customerId, poctypesId, rtuChannelIds);
            var trendDataResultSet = CreateTrendDataResponseFromInflux(latestTrendDataRecords, rtuChannelIds);

            // Get paramStandardTypesIds from master variables facility.
            var statesIds = rtuAlarmMVData.Where(x => x.State != null).Select(x => x.State.ToString()).Distinct().ToArray();

            // Set Filter for Param Standard Types from Lookup document.
            var statesLookupFilter = new FilterDefinitionBuilder<Lookup>()
                .Where(x => x.LookupType == LookupTypes.States.ToString()
                && statesIds.Contains(x.LegacyId["StatesId"]));

            // Get Param Standard Types data (Lookup).
            var lookupStatesData = Find<Lookup>(COLLECTION_NAME_LOOKUP, statesLookupFilter, correlationId);

            // In case MasterVariables not found.
            if (lookupStatesData == null || lookupStatesData.Count == 0)
            {
                lookupStatesData = new List<Lookup>();
                logger.WriteCId(Level.Info, "Missing lookup States Data (Lookup)", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetAlarmConfigurationAsync)}", correlationId);
            }

            var resultSet = (from a in rtuAlarmConfigData
                             join m in rtuAlarmMVData on a.Register equals int.Parse(m.LegacyId["Address"])
                             join td in trendDataResultSet on m.ChannelId equals td.TrendName
                             join lookupLeft in lookupStatesData on new { m.State, td.Y } equals new { State = GetDictionaryValue(lookupLeft?.LegacyId, "StatesId"), Y = decimal.Parse(lookupLeft?.LegacyId["Value"]) } into lookupStatesDataTemp
                             from l in lookupStatesDataTemp.DefaultIfEmpty()
                             select new AlarmData()
                             {
                                 AlarmType = "RTU",
                                 AlarmRegister = a.Register.ToString(),
                                 AlarmBit = a.Bit ?? 0,
                                 AlarmDescription = a.Description,
                                 AlarmPriority = a.Priority ?? 0,
                                 AlarmNormalState = ((RTU)a.Document)?.NormalState == true
                                                                                    ? 1
                                                                                    : 0,
                                 CurrentValue = (float)td.Y,
                                 StateText = ((States)l?.LookupDocument).Text
                             }).Distinct().ToList();

            return resultSet;
        }

        /// <summary>
        /// Gets the list of host alarms for the provided <paramref name="assetId"/>
        /// </summary>
        /// <param name="assetId">The asset id used to get the alarm configuration data.</param>
        /// <param name="customerId">The customer id.</param>
        /// <returns>
        /// A <seealso cref="IList{AlarmConfig}"/> that contains the host alarm data for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then and empty list is returned. If
        /// the <paramref name="assetId"/> is the default GUID then null is returned.
        /// </returns>
        public async Task<IList<AlarmData>> GetHostAlarmsAsync(Guid assetId, Guid customerId)
        {
            try
            {
                var correlationId = Guid.NewGuid().ToString();

                var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
                logger.WriteCId(Level.Trace, $"Starting {nameof(AlarmMongoStore)} {nameof(GetHostAlarmsAsync)}", correlationId);

                await Task.Yield();

                if (assetId == Guid.Empty)
                {
                    return new List<AlarmData>();
                }

                // Set Filter for Get Asset document.
                var assetfilter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
                   .Where(x => (x.LegacyId.ContainsKey("AssetGUID") && x.LegacyId["AssetGUID"] != null &&
                              (x.LegacyId["AssetGUID"].ToUpper() == assetId.ToString().ToUpper())));

                // Get Asset Document
                var assetData = Find<MongoAssetCollection.Asset>(COLLECTION_NAME_ASSET, assetfilter, correlationId);

                // In case asset not found.
                if (assetData == null || assetData.Count == 0)
                {
                    logger.Write(Level.Info, "Missing node");
                    logger.Write(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetHostAlarmsAsync)}");

                    return new List<AlarmData>();
                }

                // Get NodeId from Asset.
                var nodeId = assetData.FirstOrDefault()?.LegacyId["NodeId"];

                // Get POCType ID from Asset.
                var poctypesId = assetData.FirstOrDefault()?.POCType.LegacyId["POCTypesId"];

                // Set Filter for Get AlarmConfiguration documents (also fetch xdiagoutputs) (HostAlarm).
                var alarmConfigHostFilter = new FilterDefinitionBuilder<AlarmConfiguration>()
                        .Where(x => x.AlarmType == AlarmTypesEnum.HostAlarm.ToString()
                        && (x.LegacyId.ContainsKey("NodeId") && x.LegacyId["NodeId"] != null && (x.LegacyId["NodeId"] == nodeId))
                        && x.Enabled == true);

                // Get AlarmConfiguration Documents
                var hostAlarmConfigDataIncludingXdiag = Find<AlarmConfiguration>(COLLECTION_NAME_ALARM_CONFIGURATION, alarmConfigHostFilter, correlationId);

                // In case AlarmConfiguration not found.
                if (hostAlarmConfigDataIncludingXdiag == null || hostAlarmConfigDataIncludingXdiag.Count == 0)
                {
                    logger.WriteCId(Level.Info, "Missing Host Alarm Config", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetHostAlarmsAsync)}", correlationId);

                    return new List<AlarmData>();
                }

                // host alarm without xdiag
                var hostAlarmConfigData = hostAlarmConfigDataIncludingXdiag.Where(h => h.LegacyId.ContainsKey("Address") && h.LegacyId["Address"] != null).ToList();

                // host alarm without xdiag
                var hostAlarmConfigDataXdiagOnly = hostAlarmConfigDataIncludingXdiag.Where(h => h.LegacyId.ContainsKey("Address") && h.LegacyId["Address"] == null).ToList();

                // Get Host Alarm type Address (int)
                var hostAlarmConfigAddressInt = hostAlarmConfigData.Select(x => int.Parse(x.LegacyId["Address"])).Distinct().ToArray();

                // Phrases Data variable.
                IList<Lookup> phraseLookupData = null;

                // Set Filter for Get MasterVariable documents master variables (Parameters).
                var hostTypeMVFilter = new FilterDefinitionBuilder<Parameters>()
                        .Where(x => x.ParameterType == ParameterTypes.Param.ToString() && (x.POCType.LegacyId["POCTypesId"] == poctypesId || x.POCType.LegacyId["POCTypesId"] == "99")
                         && hostAlarmConfigAddressInt.Contains(x.Address));

                // Get MasterVariables (Param) Documents
                var hostTypeAlarmMVData = Find<Parameters>(COLLECTION_NAME_MASTERVARIABLES, hostTypeMVFilter, correlationId);

                // In case MasterVariables not found.
                if (hostTypeAlarmMVData == null || hostTypeAlarmMVData.Count == 0)
                {
                    logger.WriteCId(Level.Info, "Missing Host Alarm tag (MasterVariables)", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetHostAlarmsAsync)}", correlationId);
                    // return null;
                }
                else
                {
                    // TODO: PhraseId missing in MasterVariable Collection, should not get from UnitType.
                    // Get Phrase IDs from MasterVariable collection.
                    var phraseIds = hostTypeAlarmMVData.Select(x => ((UnitTypes)x.UnitType.LookupDocument).PhraseId).Distinct().ToList();

                    // Set Filter for Get Phrases from Lookup document.
                    var phrasesLookupFilter = new FilterDefinitionBuilder<Lookup>()
                        .Where(x => x.LookupType == LookupTypes.LocalePhrases.ToString()
                        && phraseIds.Contains(((LocalePhrases)x.LookupDocument).PhraseId));

                    // Get Phrases data (Lookup).
                    phraseLookupData = Find<Lookup>(COLLECTION_NAME_LOOKUP, phrasesLookupFilter, correlationId);

                    // In case Phrases (Lookup) not found.
                    if (phraseLookupData == null || phraseLookupData.Count == 0)
                    {
                        logger.WriteCId(Level.Info, "Missing Phrases lookup data", correlationId);
                        logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetHostAlarmsAsync)}", correlationId);
                    }
                }

                // Get Host Alarm type Address (string)
                var hostAlarmConfigAddress = hostAlarmConfigData.Select(x => x.LegacyId["Address"]).Distinct().ToArray();

                // Set Filter for Get AlarmConfiguration documents (for FacilityTagAlarm).
                var alarmConfigFacilityTagFilter = new FilterDefinitionBuilder<AlarmConfiguration>()
                        .Where(x => x.AlarmType == AlarmTypesEnum.FacilityTagAlarm.ToString()
                        && (x.LegacyId.ContainsKey("Address") && x.LegacyId["Address"] != null && hostAlarmConfigAddress.Contains(x.LegacyId["Address"]))
                        && ((((FacilityTagAlarm)x.Document).GroupNodeId == nodeId)
                            ||
                            (x.LegacyId.ContainsKey("NodeId") && x.LegacyId["NodeId"] != null && (x.LegacyId["NodeId"] == nodeId)))
                        && (x.Bit == 0));

                // Get AlarmConfiguration (Facility) Documents
                var facilityTagAlarmConfigData = Find<AlarmConfiguration>(COLLECTION_NAME_ALARM_CONFIGURATION, alarmConfigFacilityTagFilter, correlationId);

                // In case MasterVariables not found.
                if (facilityTagAlarmConfigData == null || facilityTagAlarmConfigData.Count == 0)
                {
                    logger.WriteCId(Level.Info, "Missing Facility Alarm tag (MasterVariables)", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetHostAlarmsAsync)}", correlationId);
                }

                // Get Facility Alarm type Ids
                var hostAlarmConfigIds = hostAlarmConfigDataIncludingXdiag.Select(x => x.Id).Distinct().ToArray();

                // Set Filter for Get Notification documents.
                var notificationAlarmFilter = new FilterDefinitionBuilder<Notification>()
                        .Where(x => (x.LegacyId.ContainsKey("NodeId") && x.LegacyId["NodeId"] != null && (x.LegacyId["NodeId"] == nodeId))
                        && hostAlarmConfigIds.Contains(x.AlaramId));

                // Get AlarmConfiguration (Facility) Documents
                var notificationAlarmData = Find<Notification>(COLLECTION_NAME_NOTIFICATION, notificationAlarmFilter, correlationId);

                // In case MasterVariables not found.
                if (notificationAlarmData == null || notificationAlarmData.Count == 0)
                {
                    logger.WriteCId(Level.Info, "Missing Notification Alarm Data", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetHostAlarmsAsync)}", correlationId);
                }

                // TODO: PhraseId missing in MasterVariables, should not get from ((UnitTypes)hostMVParamData?.UnitType?.LookupDocument)?.PhraseId).
                var parameterHostAlarms = (from h in hostAlarmConfigData
                                           let hostMVParamData = hostTypeAlarmMVData?.FirstOrDefault(r => (r.Address == int.Parse(h.LegacyId["Address"])) && (r.POCType.LegacyId["POCTypesId"] == poctypesId || r.POCType.LegacyId["POCTypesId"] == "99")) //.FirstOrDefault()
                                           let phraseData = phraseLookupData?.FirstOrDefault(p => ((LocalePhrases)p.LookupDocument).PhraseId == ((UnitTypes)hostMVParamData?.UnitType?.LookupDocument)?.PhraseId)
                                           let facd = facilityTagAlarmConfigData?.FirstOrDefault(f => f.LegacyId["Address"] == h.LegacyId["Address"] && (((FacilityTagAlarm)f.Document).GroupNodeId == nodeId || f.LegacyId["NodeId"] == nodeId) && f.Bit == 0)
                                           let notData = notificationAlarmData?.FirstOrDefault(n => n.AlaramId == h.Id)
                                           select new HostAlarmEntry()
                                           {
                                               Id = int.Parse(h.LegacyId["ID"]),
                                               IsEnabled = h.Enabled ?? false,
                                               SourceId = facd != null ? int.Parse(facd.LegacyId["Address"]) : 
                                                   (h.LegacyId["Address"] != null ? int.Parse(h.LegacyId["Address"]) : ((HostAlarm)h.Document).XDiagOutputsId),
                                               IsParameter = true,
                                               Description = (facd != null && facd.Description != null) ? facd.Description :
                                                  (phraseData != null ? ((LocalePhrases)phraseData.LookupDocument).English : hostMVParamData?.Description),
                                               AlarmType = int.Parse(h.LegacyId["AlarmType"]),
                                               AlarmState = ((HostAlarm)h.Document).AlarmState,
                                               LoLimit = h.LoLimit.HasValue ? (float)h.LoLimit : null,
                                               LoLoLimit = ((HostAlarm)h.Document).LoLoLimit.HasValue ? (float)((HostAlarm)h.Document).LoLoLimit : null,
                                               HiLimit = h.HiLimit.HasValue ? (float)h.HiLimit : null,
                                               HiHiLimit = ((HostAlarm)h.Document).HiHiLimit.HasValue ?  (float)((HostAlarm)h.Document).HiHiLimit : null,
                                               NumberDays = ((HostAlarm)h.Document).NumDays.HasValue ? (float)((HostAlarm)h.Document).NumDays : null,
                                               ExactValue = ((HostAlarm)h.Document).ExactValue.HasValue ? (float)((HostAlarm)h.Document).ExactValue : null,
                                               ValueChange = ((HostAlarm)h.Document).ValueChange.HasValue ? (float)((HostAlarm)h.Document).ValueChange : null,
                                               PercentChange = ((HostAlarm)h.Document).PercentChange,
                                               SpanLimit = ((HostAlarm)h.Document).MinToMaxLimit,
                                               IgnoreZeroAddress = ((HostAlarm)h.Document).IgnoreZeroAddress,
                                               AlarmAction = h.AlarmAction,
                                               IgnoreValue = ((HostAlarm)h.Document).IgnoreValue.HasValue ? (float)((HostAlarm)h.Document).IgnoreValue : null,
                                               EmailOnAlarm = 0,
                                               EmailOnLimit = 0,
                                               EmailOnClear = 0,
                                               PageOnAlarm = 0,
                                               PageOnLimit = 0,
                                               PageOnClear = 0,
                                               Priority = h.Priority,
                                               IsPushEnabled = notData?.PushEnabled
                                           }).Distinct().ToList();

                // Get Host alarms 
                List<HostAlarmEntry> xdiagHostAlarms = new List<HostAlarmEntry>();

                if(hostAlarmConfigDataXdiagOnly.Count > 0)
                {
                    // Get xDiagoutputs Facility Alarm type.
                    var xdiagoutputIds = hostAlarmConfigDataXdiagOnly.Select(x => ((HostAlarm)x.Document).XDiagOutputsId).Distinct().ToArray();

                    // xDiagoutput Data variable.
                    IList<Lookup> xDiagOutputLookupData = null;

                    // Set Filter for Get xDiagoutputs from Lookup document.
                    var xDiagoutputLookupFilter = new FilterDefinitionBuilder<Lookup>()
                        .Where(x => x.LookupType == LookupTypes.XDiagOutputs.ToString()
                        && xdiagoutputIds.Contains(((XDiagOutputs)x.LookupDocument).XDiagOutputsId));

                    // Get xDiagoutput data (Lookup).
                    xDiagOutputLookupData = Find<Lookup>(COLLECTION_NAME_LOOKUP, xDiagoutputLookupFilter, correlationId);

                    // In case Xdiagoutput (Lookup) not found.
                    if (xDiagOutputLookupData == null || xDiagOutputLookupData.Count == 0)
                    {
                        logger.WriteCId(Level.Info, "Missing Xdiagoutput lookup data", correlationId);
                        logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetHostAlarmsAsync)}", correlationId);
                    }

                    xdiagHostAlarms = (from h in hostAlarmConfigDataXdiagOnly
                                       let xdiagData = xDiagOutputLookupData?.FirstOrDefault(p => ((XDiagOutputs)p.LookupDocument).XDiagOutputsId == ((HostAlarm)h.Document).XDiagOutputsId)
                                       let notData = notificationAlarmData?.FirstOrDefault(n => n.AlaramId == h.Id)
                                       select new HostAlarmEntry()
                                       {
                                           Id = int.Parse(h.LegacyId["ID"]),
                                           IsEnabled = h.Enabled ?? false,
                                           SourceId = h.LegacyId["Address"] != null ? int.Parse(h.LegacyId["Address"]) : ((XDiagOutputs)xdiagData.LookupDocument).XDiagOutputsId,
                                           IsParameter = false,
                                           Description = ((XDiagOutputs)xdiagData.LookupDocument).Name,
                                           AlarmType = int.Parse(h.LegacyId["AlarmType"]),
                                           AlarmState = ((HostAlarm)h.Document).AlarmState,
                                           LoLimit = h.LoLimit.HasValue ? (float)h.LoLimit : null,
                                           LoLoLimit = ((HostAlarm)h.Document).LoLoLimit.HasValue ? (float)((HostAlarm)h.Document).LoLoLimit : null,
                                           HiLimit = h.HiLimit.HasValue ? (float)h.HiLimit : null,
                                           HiHiLimit = ((HostAlarm)h.Document).HiHiLimit.HasValue ? (float)((HostAlarm)h.Document).HiHiLimit : null,
                                           NumberDays = ((HostAlarm)h.Document).NumDays.HasValue ? (float)((HostAlarm)h.Document).NumDays : null,
                                           ExactValue = ((HostAlarm)h.Document).ExactValue.HasValue ? (float)((HostAlarm)h.Document).ExactValue : null,
                                           ValueChange = ((HostAlarm)h.Document).ValueChange.HasValue ? (float)((HostAlarm)h.Document).ValueChange : null,
                                           PercentChange = ((HostAlarm)h.Document).PercentChange,
                                           SpanLimit = ((HostAlarm)h.Document).MinToMaxLimit,
                                           IgnoreZeroAddress = ((HostAlarm)h.Document).IgnoreZeroAddress,
                                           AlarmAction = h.AlarmAction,
                                           IgnoreValue = ((HostAlarm)h.Document).IgnoreValue.HasValue ? (float)((HostAlarm)h.Document).IgnoreValue : null,
                                           EmailOnAlarm = 0,
                                           EmailOnLimit = 0,
                                           EmailOnClear = 0,
                                           PageOnAlarm = 0,
                                           PageOnLimit = 0,
                                           PageOnClear = 0,
                                           Priority = h.Priority,
                                           IsPushEnabled = notData?.PushEnabled
                                       }).ToList();
                }

                var result = parameterHostAlarms.Union(xdiagHostAlarms).OrderByDescending(m => m.Description)
                       .ThenByDescending(m => m.SourceId);

                // This is required to do a ToList before the Select because the columns in the DB are of a different
                // data type and the select before the ToList will cause entity framework to not be able to 
                // generate the SQL.
                return result.ToList().Select(Map).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new List<AlarmData>();
            }
        }

        /// <summary>
        /// Gets the list of facility tag alarms for the provided <paramref name="assetId"/>
        /// </summary>
        /// <param name="assetId">The asset id used to get the alarm configuration data.</param>
        /// <param name="customerId">The customer id.</param>
        /// <returns>
        /// A <seealso cref="IList{AlarmConfig}"/> that contains the facility tag alarms data for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then and empty list is returned. If
        /// the <paramref name="assetId"/> is the default GUID then null is returned. 
        /// </returns>
        public async Task<IList<AlarmData>> GetFacilityTagAlarmsAsync(Guid assetId, Guid customerId)
        {
            var correlationId = Guid.NewGuid().ToString();

            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(AlarmMongoStore)} {nameof(GetFacilityTagAlarmsAsync)}", correlationId);

            await Task.Yield();

            if (assetId == Guid.Empty)
            {
                return new List<AlarmData>();
            }
            
            // Set Filter for Get Asset document.
            var assetfilter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
               .Where(x => (x.LegacyId.ContainsKey("AssetGUID") && x.LegacyId["AssetGUID"] != null &&
                          (x.LegacyId["AssetGUID"].ToUpper() == assetId.ToString().ToUpper())));

            // Get Asset Document
            var assetData = Find<MongoAssetCollection.Asset>(COLLECTION_NAME_ASSET, assetfilter, correlationId);

            // In case asset not found.
            if (assetData == null || assetData.Count == 0)
            {
                logger.WriteCId(Level.Info, "Missing node", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetFacilityTagAlarmsAsync)}", correlationId);

                return new List<AlarmData>();
            }

            // Get NodeId from Asset.
            var nodeId = assetData.FirstOrDefault()?.LegacyId["NodeId"];

            // Get POCType ID from Asset.
            var poctypesId = assetData.FirstOrDefault()?.POCType.LegacyId["POCTypesId"];

            // Get NodeId from Asset.
            var assetObjId = assetData.FirstOrDefault()?.Id;

            // Set Filter for Get AlarmConfiguration documents.
            var alarmConfigFacilityFilter = new FilterDefinitionBuilder<AlarmConfiguration>()
                    .Where(x => x.AlarmType == AlarmTypesEnum.FacilityTagAlarm.ToString() 
                    && ((((FacilityTagAlarm)x.Document).GroupNodeId == null && x.LegacyId["NodeId"] == nodeId) || ((FacilityTagAlarm)x.Document).GroupNodeId == nodeId)
                    && ((FacilityTagAlarm)x.Document).AlarmState > 0);

            // Get AlarmConfiguration Documents
            var facilityAlarmConfigData = Find<AlarmConfiguration>(COLLECTION_NAME_ALARM_CONFIGURATION, alarmConfigFacilityFilter, correlationId);

            // In case AlarmConfiguration not found.
            if (facilityAlarmConfigData == null || facilityAlarmConfigData.Count == 0)
            {
                logger.WriteCId(Level.Info, "Missing Facility Alarm Config", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetFacilityTagAlarmsAsync)}", correlationId);

                return new List<AlarmData>();
            }

            // Get Facility Alarm type Address
            var facilityAlarmAddress = facilityAlarmConfigData.Select(x => int.Parse(x.LegacyId["Address"])).Distinct().ToArray();

            // Set Filter for Get MasterVariable documents master variables (Parameters).
            var facilityTagAlarmFilter = new FilterDefinitionBuilder<Parameters>()
                    .Where(x => x.ParameterType == ParameterTypes.Facility.ToString() && (x.POCType.LegacyId["POCTypesId"] == poctypesId || x.POCType.LegacyId["POCTypesId"] == "99")
                     && facilityAlarmAddress.Contains(x.Address)
                     && ((FacilityTagDetails)x.ParameterDocument).AssetId == assetObjId);

            // Get MasterVariables (Facility) Documents
            var facilityTagAlarmData = Find<Parameters>(COLLECTION_NAME_MASTERVARIABLES, facilityTagAlarmFilter, correlationId);

            // In case MasterVariables not found.
            if (facilityTagAlarmData == null || facilityTagAlarmData.Count == 0)
            {
                logger.WriteCId(Level.Info, "Missing Facility Alarm tag (MasterVariables)", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetFacilityTagAlarmsAsync)}", correlationId);

                return new List<AlarmData>();
            }

            // Facility Tag Alarms result. 
            var result = facilityTagAlarmData.Select(m => MapFacilityTagAlarm(m.Description, GetAlarmState(facilityAlarmConfigData, m.Address)));
            return result.ToList();
        }

        /// <summary>
        /// Gets the list of camera alarms for the provided <paramref name="assetId"/>
        /// </summary>
        /// <param name="assetId">The asset id used to get the alarm configuration data.</param>
        /// <param name="customerId">The customer id.</param>
        /// <returns>
        /// A <seealso cref="IList{AlarmConfig}"/> that contains the camera alarms data for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then and empty list is returned. If
        /// the <paramref name="assetId"/> is the default GUID then null is returned.
        /// </returns>
        public async Task<IList<AlarmData>> GetCameraAlarmsAsync(Guid assetId, Guid customerId)
        {
            var correlationId = Guid.NewGuid().ToString();

            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(AlarmMongoStore)} {nameof(GetCameraAlarmsAsync)}", correlationId);

            await Task.Yield();

            if (assetId == Guid.Empty)
            {
                return new List<AlarmData>();
            }

            // Set Filter for Get Asset document.
            var assetfilter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
               .Where(x => (x.LegacyId.ContainsKey("AssetGUID") && x.LegacyId["AssetGUID"] != null &&
                          (x.LegacyId["AssetGUID"].ToUpper() == assetId.ToString().ToUpper())));

            // Get Asset Document
            var assetData = Find<MongoAssetCollection.Asset>(COLLECTION_NAME_ASSET, assetfilter, correlationId);

            // In case asset not found.
            if (assetData == null || assetData.Count == 0)
            {
                logger.WriteCId(Level.Info, "Missing node", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetCameraAlarmsAsync)}", correlationId);

                return new List<AlarmData>();
            }

            // Get NodeId from Asset.
            var nodeId = assetData.FirstOrDefault()?.LegacyId["NodeId"];

            // Get POCType ID from Asset.
            var poctypesId = assetData.FirstOrDefault()?.POCType.LegacyId["POCTypesId"];

            //// Get NodeId from Asset.
            //var assetObjId = assetData.FirstOrDefault()?.Id;

            // Set Filter for Get Camera document.
            var camerafilter = new FilterDefinitionBuilder<Camera>()
               .Where(x => (x.LegacyId["NodeId"] == nodeId 
                            && (x.CameraType.LookupType == CameraTypesEnum.CameraTypes.ToString())));

            // Get Asset Document
            var cameras = Find<Camera>(COLLECTION_NAME_CAMERA, camerafilter, correlationId);

            // In case camera not found.
            if (cameras == null || cameras.Count == 0)
            {
                logger.WriteCId(Level.Info, "Missing Camera Data", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetCameraAlarmsAsync)}", correlationId);

                return new List<AlarmData>();
            }

            // Set Filter for Get Camera document.
            var cameraAlarmfilter = new FilterDefinitionBuilder<CameraAlarms>()
               .Where(x => (x.Enabled == 1));

            // Get Asset Document
            var cameraAlarms = Find<CameraAlarms>(COLLECTION_NAME_CAMERA_ALARMS, cameraAlarmfilter, correlationId);

            // In case asset not found.
            if (cameraAlarms == null || cameraAlarms.Count == 0)
            {
                logger.WriteCId(Level.Info, "Missing Camera Alarms Data", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetCameraAlarmsAsync)}", correlationId);

                return new List<AlarmData>();
            }

            // Set Filter for Get Camera document.
            var alarmEventsfilter = new FilterDefinitionBuilder<AlarmEvents>()
               .Where(x => (x.AlarmType == 1));

            // Get Asset Document
            var alarmEventsData = Find<AlarmEvents>(COLLECTION_NAME_ALARMS_EVENTS, alarmEventsfilter, correlationId);
            List<AlarmEvents> alarmEventsGrouped = new List<AlarmEvents>();

            // In case asset not found.
            if (alarmEventsData == null || alarmEventsData.Count == 0)
            {
                logger.WriteCId(Level.Info, "Missing Alarms Events Data", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetCameraAlarmsAsync)}", correlationId);
            }
            else
            {
                var groupedAlarms = alarmEventsData
                .GroupBy(e => e.AlarmID)
                .Select(g => new
                {
                    AlarmID = g.Key,
                    MaxEventDateTime = g.Max(e => e.EventDateTime)
                })
                .ToList();

                alarmEventsGrouped = alarmEventsData
                    .Join(
                        groupedAlarms,
                        alarm => new { alarm.AlarmID, EventDate = alarm.EventDateTime },
                        group => new { group.AlarmID, EventDate = group.MaxEventDateTime },
                        (alarm, group) => alarm // Select matching alarms
                    )
                    .Where(alarm => alarm.AcknowledgedDateTime == null)
                    .ToList();
            }

            // Get AlarmType from Camera Alarm
            var alarmTypesArray = cameraAlarms.Select(x => x.AlarmType.ToString()).Distinct().ToArray();

            // Set Filter for Get Phrases from Lookup document.
            var cameraAlarmTypeLookupFilter = new FilterDefinitionBuilder<Lookup>()
                .Where(x => x.LookupType == LookupTypes.CameraAlarmTypes.ToString()
                && alarmTypesArray.Contains(x.LegacyId["CameraAlarmTypesId"]));

            // Get Phrases data (Lookup).
            var cameraAlarmTypeLookupData = Find<Lookup>(COLLECTION_NAME_LOOKUP, cameraAlarmTypeLookupFilter, correlationId);
            List<int?> phraseIds = new List<int?>();

            // In case Phrases (Lookup) not found.
            if (cameraAlarmTypeLookupData == null || cameraAlarmTypeLookupData.Count == 0)
            {
                logger.WriteCId(Level.Info, "Missing Camera Alarm Type lookup data", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetCameraAlarmsAsync)}", correlationId);
            }
            else
            {
                // Get Phrase IDs from Lookup collection.
                phraseIds = cameraAlarmTypeLookupData.Select(x => ((CameraAlarmTypes)x.LookupDocument).PhraseId).Distinct().ToList();
            }

            // Set Filter for Get Phrases from Lookup document.
            var phrasesLookupFilter = new FilterDefinitionBuilder<Lookup>()
                .Where(x => x.LookupType == LookupTypes.LocalePhrases.ToString()
                && phraseIds.Contains(((LocalePhrases)x.LookupDocument).PhraseId));

            // Get Phrases data (Lookup).
            var phraseLookupData = Find<Lookup>(COLLECTION_NAME_LOOKUP, phrasesLookupFilter, correlationId);

            // In case Phrases (Lookup) not found.
            if (phraseLookupData == null || phraseLookupData.Count == 0)
            {
                logger.WriteCId(Level.Info, "Missing Phrases lookup data", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetCameraAlarmsAsync)}", correlationId);
            }

            var result = (from c in cameras
                          join ca in cameraAlarms on int.Parse(c.LegacyId["CameraId"]) equals ca.CameraID
                          join aeg in alarmEventsGrouped on int.Parse(ca.LegacyId["CameraAlarmID"]) equals aeg?.AlarmID
                          let cameraAlarmType = cameraAlarmTypeLookupData?.FirstOrDefault(cat => ca.AlarmType == int.Parse(cat.LegacyId["CameraAlarmTypesId"]))
                          let phraseData = phraseLookupData?.FirstOrDefault(p => ((LocalePhrases)p.LookupDocument).PhraseId == ((CameraAlarmTypes)cameraAlarmType?.LookupDocument)?.PhraseId)
                          select MapCameraAlarm(c.Name, ((CameraAlarmTypes)cameraAlarmType?.LookupDocument).Name, ((LocalePhrases)phraseData?.LookupDocument)?.English)).ToList();

            return result;
        }

        /// <summary>
        /// Gets the facility tag header and details <paramref name="assetId"/>
        /// </summary>
        /// <param name="assetId">The asset id used to get the facility configuration data.</param>
        /// <returns>The <seealso cref="FacilityDataModel"/> data.</returns>
        public async Task<FacilityDataModel> GetFacilityHeaderAndDetails(Guid assetId)
        {
            var correlationId = Guid.NewGuid().ToString();

            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(AlarmMongoStore)} {nameof(GetFacilityHeaderAndDetails)}", correlationId);

            await Task.Yield();

            if (assetId == Guid.Empty)
            {
                return new FacilityDataModel();
            }

            var responseData = new FacilityDataModel();

            // Set Filter for Get Asset document.
            var assetfilter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
               .Where(x => (x.LegacyId.ContainsKey("AssetGUID") && x.LegacyId["AssetGUID"] != null &&
                          (x.LegacyId["AssetGUID"].ToUpper() == assetId.ToString().ToUpper())));

            // Get Asset Document
            var assetData = Find<MongoAssetCollection.Asset>(COLLECTION_NAME_ASSET, assetfilter, correlationId);

            // In case asset not found.
            if (assetData == null || assetData.Count == 0)
            {
                logger.WriteCId(Level.Info, "Missing node", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetFacilityHeaderAndDetails)}", correlationId);

                return responseData;
            }

            // Get NodeId from Asset.
            var nodeId = assetData.FirstOrDefault()?.LegacyId["NodeId"];

            // Get POCType ID from Asset.
            var poctypesId = assetData.FirstOrDefault()?.POCType.LegacyId["POCTypesId"];

            // Get NodeId from Asset.
            var assetObjId = assetData.FirstOrDefault()?.Id;

            // Get NodeId from Asset.
            var customerObjId = assetData.FirstOrDefault()?.CustomerId;

            // Set Filter for Get Customer document.
            var customerfilter = new FilterDefinitionBuilder<Customer>()
               .Where(x => (x.Id == customerObjId));

            // Get Customer Document
            var customerData = Find<Customer>(COLLECTION_NAME_CUSTOMER, customerfilter, correlationId);
            
            // In case customer not found.
            if (customerData == null || customerData.Count == 0)
            {
                logger.WriteCId(Level.Info, "Customer not found", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetFacilityHeaderAndDetails)}", correlationId);
            }

            // Set Filter for Get MasterVariables documents for Facility.
            var facilityTagMVFilter = new FilterDefinitionBuilder<Parameters>()
                    .Where(x => x.ParameterType == ParameterTypes.Facility.ToString()
                    && (((FacilityTagDetails)x.ParameterDocument).AssetId == assetObjId));

            // Get MasterVariables (Facility) Documents
            var facilityTagMVData = Find<Parameters>(COLLECTION_NAME_MASTERVARIABLES, facilityTagMVFilter, correlationId);
            int facilityTagCount = 0;
            // In case MasterVariables not found.
            if (facilityTagMVData == null || facilityTagMVData.Count == 0)
            {
                logger.WriteCId(Level.Info, "Missing Facility tag (MasterVariables)", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetFacilityHeaderAndDetails)}", correlationId);

                return responseData;
            }
            else
            {
                // Get Facility Tag count of Enabled only.
                facilityTagCount = facilityTagMVData.Count(f => f.Enabled == true);
            }

            // Set Filter for Get AlarmConfiguration documents for Facility.
            var facilityAlarmConfigFilter = new FilterDefinitionBuilder<AlarmConfiguration>()
                    .Where(x => x.AlarmType == AlarmTypesEnum.FacilityTagAlarm.ToString()
                    && x.Enabled == true
                    && (((FacilityTagAlarm)x.Document).GroupNodeId == nodeId || x.LegacyId["NodeId"] == nodeId)); // && ((FacilityTagAlarm)x.Document).AlarmState != 0

            // Get AlarmConfiguration Documents
            var facilityAlarmConfigData = Find<AlarmConfiguration>(COLLECTION_NAME_ALARM_CONFIGURATION, facilityAlarmConfigFilter, correlationId);
            int facilityTagAlarmCount = 0;
            // In case AlarmConfiguration not found.
            if (facilityAlarmConfigData == null || facilityAlarmConfigData.Count == 0)
            {
                facilityAlarmConfigData = new List<AlarmConfiguration>();
                logger.WriteCId(Level.Info, "Missing Facility Alarm Config", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetFacilityHeaderAndDetails)}", correlationId);
            }
            else
            {
                facilityTagAlarmCount = facilityAlarmConfigData.Count(x => ((FacilityTagAlarm)x.Document).AlarmState != 0);
            }

            // Set Filter for Get AlarmConfiguration documents for HostAlarm.
            var hostAlarmConfigFilter = new FilterDefinitionBuilder<AlarmConfiguration>()
                    .Where(x => x.AlarmType == AlarmTypesEnum.HostAlarm.ToString()
                    && x.LegacyId["NodeId"] == nodeId
                    && x.Enabled == true
                    && ((HostAlarm)x.Document).AlarmState > 0);

            // Get AlarmConfiguration Documents
            var hostAlarmConfigData = Find<AlarmConfiguration>(COLLECTION_NAME_ALARM_CONFIGURATION, hostAlarmConfigFilter, correlationId);
            int hostAlarmCount = 0;

            // In case AlarmConfiguration not found.
            if (hostAlarmConfigData == null || hostAlarmConfigData.Count == 0)
            {
                logger.WriteCId(Level.Info, "Missing Host Alarm Config", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetFacilityHeaderAndDetails)}", correlationId);
            }
            else
            {
                hostAlarmCount = hostAlarmConfigData.Count;
            }

            // Table 1: Node data
            var facilityTagsData = assetData
                .OrderBy(headers => headers.AssetConfig.DisplayName)
                .Select(headers => new FacilityDataModel
                {
                    NodeId = headers.LegacyId["NodeId"],
                    Facility = headers.AssetConfig.DisplayName,
                    CommStatus = headers.AssetConfig.CommunicationStatus,
                    Indicator = "",
                    Enabled = headers.AssetConfig.IsEnabled,
                    NodeHostAlarm = "",
                    NodeHostAlarmState = 0,
                    HostAlarmBackColor = "",
                    HostAlarmForeColor = "",
                    TagCount = facilityTagCount,
                    AlarmCount = facilityTagAlarmCount + hostAlarmCount,
                    Comment = headers.AssetConfig.Comment
                }).AsEnumerable();

            if (facilityTagsData != null && facilityTagsData.Any())
            {
                responseData = facilityTagsData.FirstOrDefault();
                responseData.TagGroups = new List<FacilityTagGroupModel>();
            }

            // Get FacilityTagGroupID from master variables facility.
            var FacilityTagGroupIds = facilityTagMVData.Where(x => ((FacilityTagDetails)x.ParameterDocument).FacilityTagGroupID != null).Select(x => ((FacilityTagDetails)x.ParameterDocument).FacilityTagGroupID.ToString()).Distinct().ToArray();

            // Set Filter for Get facility tag group from Lookup document.
            var facilityTagGroupLookupFilter = new FilterDefinitionBuilder<Lookup>()
                .Where(x => x.LookupType == LookupTypes.FacilityTagGroups.ToString()
                && FacilityTagGroupIds.Contains(x.LegacyId["FacilityTagGroupsId"]));

            // Get facility tag group data (Lookup).
            var facilityTagGroupLookupData = Find<Lookup>(COLLECTION_NAME_LOOKUP, facilityTagGroupLookupFilter, correlationId);

            // In case facility tag group (Lookup) not found.
            if (facilityTagGroupLookupData == null || facilityTagGroupLookupData.Count == 0)
            {
                logger.WriteCId(Level.Info, "Missing Camera Alarm Type lookup data", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetCameraAlarmsAsync)}", correlationId);
            }

            // Table 2: Tag Group Information
            var tagGroup1 = (from ft in facilityTagMVData
                             join facAlarm in facilityAlarmConfigData on new { Address = ft.LegacyId["Address"], Bit = ft.LegacyId["Bit"], AssetIdObj = ((FacilityTagDetails)ft.ParameterDocument).AssetId } equals new { Address = facAlarm?.LegacyId["Address"], Bit = facAlarm?.LegacyId["Bit"], AssetIdObj = ((FacilityTagAlarm)facAlarm?.Document).NodeId }
                             join asst in assetData on ((FacilityTagDetails)ft.ParameterDocument).AssetId equals asst.Id
                             let facilityGroupData = facilityTagGroupLookupData?.FirstOrDefault(f => int.Parse(f.LegacyId["FacilityTagGroupsId"]) == ((FacilityTagDetails)ft.ParameterDocument).FacilityTagGroupID)
                             where ((FacilityTagDetails)ft.ParameterDocument).FacilityTagGroupID != null
                             group new { ft, facAlarm, facilityGroupData, asst } by new { ((FacilityTagGroups)facilityGroupData?.LookupDocument).DisplayOrder, ((FacilityTagDetails)ft.ParameterDocument).FacilityTagGroupID, ((FacilityTagGroups)facilityGroupData.LookupDocument).Name, NodeId = asst.LegacyId["NodeId"] }
                             into grp
                             select new FacilityTagGroupModel
                             {
                                 DisplayOrder = grp.Key.DisplayOrder.ToString(),
                                 Id = grp.Key.FacilityTagGroupID.ToString(),
                                 TagGroupName = grp.Key.Name,
                                 TagCount = grp.Sum(x => x.ft.Enabled ? 1 : 0).ToString(),
                                 AlarmCount = grp.Sum(x => ((x.facAlarm?.Enabled ?? false) && (((FacilityTagAlarm)x.facAlarm?.Document).AlarmState == 1 || ((FacilityTagAlarm)x.facAlarm?.Document).AlarmState == 2)) ? 1 : 0).ToString(),
                                 NodeId = grp.Key.NodeId,
                                 TagGroupNodeId = grp.Key.Name + grp.Key.NodeId,
                                 GroupHostAlarm = "",
                                 HostAlarmBackColor = "",
                                 HostAlarmForeColor = "",
                                 FacilityTags = new List<FacilityTagsModel>()
                             }).AsEnumerable();

            var tagGroup2 = (from ft in facilityTagMVData
                             join facAlarm in facilityAlarmConfigData on new { Address = ft.LegacyId["Address"], Bit = ft.LegacyId["Bit"], AssetIdObj = ((FacilityTagDetails)ft.ParameterDocument).AssetId } equals new { Address = facAlarm?.LegacyId["Address"], Bit = facAlarm?.LegacyId["Bit"], AssetIdObj = ((FacilityTagAlarm)facAlarm?.Document).NodeId }
                             join asst in assetData on ((FacilityTagDetails)ft.ParameterDocument).AssetId equals asst.Id
                             where ft.Enabled && ((FacilityTagDetails)ft.ParameterDocument).FacilityTagGroupID == null
                             group new { ft, facAlarm } by asst.LegacyId["NodeId"] into g
                             select new FacilityTagGroupModel
                             {
                                 DisplayOrder = "",
                                 Id = "",
                                 TagGroupName = "",
                                 TagCount = g.Sum(x => x.ft.Enabled ? 1 : 0).ToString(),
                                 AlarmCount = g.Sum(x => ((x.facAlarm?.Enabled ?? false) && (((FacilityTagAlarm)x.facAlarm?.Document).AlarmState == 1 || ((FacilityTagAlarm)x.facAlarm?.Document).AlarmState == 2)) ? 1 : 0).ToString(),
                                 NodeId = g.Key,
                                 TagGroupNodeId = g.Key,
                                 GroupHostAlarm = "",
                                 HostAlarmBackColor = "",
                                 HostAlarmForeColor = "",
                                 FacilityTags = new List<FacilityTagsModel>()
                             }).AsEnumerable();

            var tagGroups = tagGroup1.Union(tagGroup2)
                   .Select(group => new FacilityTagGroupModel
                   {
                       NodeId = group.NodeId,
                       TagGroupName = group.TagGroupName,
                       AlarmCount = group.AlarmCount,
                       DisplayOrder = group.DisplayOrder,
                       TagCount = group.TagCount,
                       HostAlarmBackColor = group.HostAlarmBackColor,
                       HostAlarmForeColor = group.HostAlarmForeColor,
                       Id = group.Id,
                       GroupHostAlarm = group.GroupHostAlarm,
                       TagGroupNodeId = group.TagGroupNodeId,
                       FacilityTags = new List<FacilityTagsModel>(),
                   });
            responseData.TagGroups = tagGroups.ToList();

            // Get paramStandardTypesIds from master variables facility.
            var paramStandardTypesIds = facilityTagMVData.Where(x => x.ParamStandardType != null && x.ParamStandardType.LegacyId["ParamStandardTypesId"] != null).Select(x => x.ParamStandardType?.LegacyId["ParamStandardTypesId"]).Distinct().ToArray();

            // Set Filter for Param Standard Types from Lookup document.
            var paramStandardTypesLookupFilter = new FilterDefinitionBuilder<Lookup>()
                .Where(x => x.LookupType == LookupTypes.ParamStandardTypes.ToString()
                && paramStandardTypesIds.Contains(x.LegacyId["ParamStandardTypesId"]));

            // Get Param Standard Types data (Lookup).
            var paramStandardTypesLookupData = Find<Lookup>(COLLECTION_NAME_LOOKUP, paramStandardTypesLookupFilter, correlationId);

            // In case Param Standard Types (Lookup) not found.
            if (paramStandardTypesLookupData == null || paramStandardTypesLookupData.Count == 0)
            {
                logger.WriteCId(Level.Info, "Missing Param Standard Types lookup data", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetCameraAlarmsAsync)}", correlationId);
            }

            // Get paramStandardTypesIds from master variables facility.
            var statesIds = facilityTagMVData.Where(x => x.State != null).Select(x => x.State.ToString()).Distinct().ToArray();

            // Set Filter for Param Standard Types from Lookup document.
            var statesLookupFilter = new FilterDefinitionBuilder<Lookup>()
                .Where(x => x.LookupType == LookupTypes.States.ToString()
                && statesIds.Contains(x.LegacyId["StatesId"]));

            // Get Param Standard Types data (Lookup).
            var statesLookupData = Find<Lookup>(COLLECTION_NAME_LOOKUP, statesLookupFilter, correlationId);

            // In case Param Standard Types (Lookup) not found.
            if (statesLookupData == null || statesLookupData.Count == 0)
            {
                statesLookupData = new List<Lookup>();
                logger.WriteCId(Level.Info, "Missing states lookup data", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AlarmMongoStore)} {nameof(GetCameraAlarmsAsync)}", correlationId);
            }

            // Get Facility tag ChannleIds and their data from Influx.
            var facilityChannelIds = facilityTagMVData.Select(x => x.ChannelId).Distinct().ToList();
            var customerGuidString = customerData.FirstOrDefault()?.LegacyId["CustomerGUID"];
            var customerGuid = Guid.Empty;
            if (!string.IsNullOrWhiteSpace(customerGuidString))
            {
                customerGuid = new Guid(customerGuidString);
            }

            var latestTrendDataRecords = await _influxDataStore.GetLatestTrendData(assetId, customerGuid, poctypesId, facilityChannelIds);
            var trendDataResultSet = CreateTrendDataResponseFromInflux(latestTrendDataRecords, facilityChannelIds);

            // Table 3: Tags in Tag Groups
            var facilityTags = (from ft in facilityTagMVData
                                join asst in assetData on ((FacilityTagDetails)ft.ParameterDocument).AssetId equals asst.Id
                                join facAlarm in facilityAlarmConfigData on new { Address = ft.LegacyId["Address"], Bit = ft.LegacyId["Bit"], AssetIdObj = ((FacilityTagDetails)ft.ParameterDocument).AssetId } equals new { Address = facAlarm?.LegacyId["Address"], Bit = facAlarm?.LegacyId["Bit"], AssetIdObj = ((FacilityTagAlarm)facAlarm?.Document).NodeId }
                                join st in statesLookupData on new { stateId = ((States)((FacilityTagAlarm)facAlarm?.Document)?.StateID?.LookupDocument)?.StatesId, value = ((States)((FacilityTagAlarm)facAlarm?.Document)?.StateID?.LookupDocument)?.Value } equals
                                new { stateId = (st == null ? (int?)null : int.Parse(st?.LegacyId["StatesId"])), value = (st == null ? (int?)null : int.Parse(st?.LegacyId["Value"])) }
                                into std
                                from st in std.DefaultIfEmpty()
                                join td in trendDataResultSet on ft.ChannelId equals td.TrendName into ltd
                                from td in ltd.DefaultIfEmpty()
                                let facilityGroupData = facilityTagGroupLookupData?.FirstOrDefault(f => int.Parse(f.LegacyId["FacilityTagGroupsId"]) == ((FacilityTagDetails)ft.ParameterDocument).FacilityTagGroupID)
                                let paramStandardTypesData = paramStandardTypesLookupData?.FirstOrDefault(f => int.Parse(f.LegacyId["ParamStandardTypesId"]) == ((ParamStandardTypes)ft.ParamStandardType.LookupDocument).ParamStandardType)
                                where ft.Enabled
                                select new
                                {
                                    NodeId = asst.LegacyId["NodeId"],
                                    TagGroupNodeID = (((FacilityTagDetails)ft.ParameterDocument)?.FacilityTagGroupID == null ? "" : ((FacilityTagDetails)ft.ParameterDocument)?.FacilityTagGroupID) + asst.LegacyId["NodeId"],
                                    ft.Description,
                                    Value = td?.Y.ToString(),
                                    FloatValue = Convert.ToDouble(td?.Y),
                                    State = st != null ? ((States)st?.LookupDocument)?.Text : td?.Y.ToString(),
                                    Units = ((FacilityTagDetails)ft.ParameterDocument)?.EngUnits,
                                    UnitType = (paramStandardTypesData != null ? ((ParamStandardTypes)paramStandardTypesData?.LookupDocument)?.UnitTypeId : ((UnitTypes)ft.UnitType?.LookupDocument)?.UnitTypesId),
                                    UnitTypePhrase = "",
                                    HostAlarm = (((FacilityTagAlarm)facAlarm?.Document)?.AlarmState == 1 ? ((FacilityTagAlarm)facAlarm?.Document)?.AlarmTextHi :
                                        (((FacilityTagAlarm)facAlarm?.Document)?.AlarmState == 2 ? ((FacilityTagAlarm)facAlarm?.Document)?.AlarmTextLo : ((FacilityTagAlarm)facAlarm?.Document)?.AlarmTextClear)),
                                    LastUpdate = td?.X,
                                    WriteAddress = ft.Address,
                                    HostAlarmBackColor = "",
                                    HostAlarmForeColor = "",
                                    Address = string.IsNullOrEmpty(ft.Tag) ? ft.Address.ToString() : ft.Tag.ToString(),
                                    ((FacilityTagAlarm)facAlarm?.Document)?.AlarmTextHi,
                                    ((FacilityTagAlarm)facAlarm?.Document)?.AlarmTextLo,
                                    ((FacilityTagAlarm)facAlarm?.Document)?.AlarmTextClear,
                                    AlarmState = (int)((FacilityTagAlarm)facAlarm?.Document)?.AlarmState,
                                    ft.Enabled,
                                    ((FacilityTagDetails)ft.ParameterDocument)?.Writeable,
                                    ((DataTypes)ft.DataType.LookupDocument)?.DataTypeId,
                                    ft.Name,
                                    StateId = ((States)((FacilityTagAlarm)facAlarm?.Document)?.StateID?.LookupDocument)?.StatesId,
                                    BackColor = ((((States)((FacilityTagAlarm)facAlarm?.Document)?.StateID?.LookupDocument) != null) ? ((States)((FacilityTagAlarm)facAlarm?.Document)?.StateID?.LookupDocument)?.BackColor : 0),
                                    ForeColor = ((((States)((FacilityTagAlarm)facAlarm?.Document)?.StateID?.LookupDocument) != null) ? ((States)((FacilityTagAlarm)facAlarm?.Document)?.StateID?.LookupDocument)?.ForeColor : 0),
                                    FacilityTagGroupId = ((FacilityTagDetails)ft.ParameterDocument)?.FacilityTagGroupID,
                                    ((FacilityTagDetails)ft.ParameterDocument)?.DisplayOrder,
                                    DisplayOrderIsNull = (((FacilityTagDetails)ft.ParameterDocument)?.DisplayOrder == null ? 1 : 0)
                                    // NOT MAPPED in Mongo And Not Required.
                                    //ft.EngLo,
                                    //ft.EngHi,
                                    //ft.RawLo,
                                    //ft.RawHi,
                                }).AsEnumerable();

            foreach (var group in responseData.TagGroups)
            {
                var tags = new List<FacilityTagsModel>();
                foreach (var tag in facilityTags)
                {
                    if (tag != null)
                    {
                        if (group.NodeId == tag.NodeId
                            && group.Id == tag.FacilityTagGroupId.ToString())
                        {
                            tags.Add(new FacilityTagsModel
                            {
                                NodeId = tag.NodeId,
                                Description = tag.Description,
                                Address = tag.Address.ToString(),
                                StringValue = tag.Value,
                                StateId = tag.StateId,
                                EngUnits = tag.Units,
                                UnitType = (short)tag.UnitType,
                                AlarmState = tag.AlarmState,
                                HostAlarm = tag.HostAlarm,
                                UpdateDate = tag.LastUpdate
                            });
                        }
                    }
                }

                group.FacilityTags = tags.OrderBy(t => t.Description).ToList();
            }

            return responseData;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Get Poc type from asset item
        /// </summary>
        /// <param name="assetObj"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        private short GetPocType(MongoAssetCollection.Asset assetObj, IThetaLogger logger)
        {
            try
            {
                POCTypes obj = (POCTypes)assetObj.POCType.LookupDocument;
                if (obj != null)
                {
                    return (short)obj.POCType;
                }
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, "An error occurred while getting PocType.", ex, "correlationId");
            }

            return 0;
        }

        private static AlarmData MapFacilityTagAlarm(string description, int? alarmState)
        {
            var alarmStateString = (alarmState) switch
            {
                1 => "-Hi",
                2 => "-Lo",
                _ => string.Empty
            };

            return new AlarmData()
            {
                AlarmDescription = $"{description} {alarmStateString}",
                AlarmType = "Facility Tag",
            };
        }

        /// <summary>
        /// Get alarm state from AlarmConfiguration.
        /// </summary>
        /// <param name="facilityAlarmConfigData"></param>
        /// <param name="address"></param>
        /// <returns></returns>
        private static int? GetAlarmState(IList<AlarmConfiguration> facilityAlarmConfigData, int address)
        {
            var alarmTag = facilityAlarmConfigData.FirstOrDefault(s => int.Parse(s.LegacyId["Address"]) == address);
            if (alarmTag != null)
            {
                return ((FacilityTagAlarm)alarmTag?.Document).AlarmState;
            }

            return null;
        }

        private List<DataPoint> CreateTrendDataResponseFromInflux(IList<DataPointModel> trendData, List<string> channelIds)
        {
            var responseData = new List<DataPoint>();
            if (trendData != null && trendData.Count > 0)
            {
                if (trendData.Any(a => a.ColumnValues != null))
                {
                    foreach (var channel in channelIds)
                    {
                        var data = trendData
                            .Where(a => a.ColumnValues != null &&
                                                           a.ColumnValues[channel]?.ToString() != null)
                            .Select(x => new DataPoint()
                            {
                                X = x.Time,
                                Y = decimal.Parse(x.ColumnValues[channel]),
                                TrendName = channel
                            }).ToList();
                        responseData.AddRange(data);
                    }
                }
                else
                {
                    // TODO: fill trend name
                    responseData = trendData
                    .Where(a => a.Value != null)
                    .Select(x => new DataPoint()
                    {
                        X = x.Time,
                        Y = decimal.Parse(x.Value.ToString()),
                        TrendName = x.TrendName
                    }).ToList();

                }
            }

            return responseData.OrderBy(a => a.X).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionaryData"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private int? GetDictionaryValue(IDictionary<string, string> dictionaryData, string key)
        {
            if (dictionaryData == null)
            {
                return null;
            }
                
            if (dictionaryData.TryGetValue(key, out var idStr) && int.TryParse(idStr, out var parsedId))
            {
                return parsedId;
            }
            return -1; // Default value if parsing fails or key is not found
        }

        private static AlarmData MapCameraAlarm(string cameraName, string cameraTypeName, string localName)
        {
            return new AlarmData()
            {
                AlarmDescription = $"{cameraName} - {localName ?? cameraTypeName}",
                AlarmType = "Camera",
            };
        }
        #endregion

        #region Private Data Structures

        private class HostAlarmEntry
        {

            /// <summary>
            /// Gets or set the id.
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Gets or set is enabled flag.
            /// </summary>
            public bool IsEnabled { get; set; }

            /// <summary>
            /// Gets or set source id.
            /// </summary>
            public int? SourceId { get; set; }

            /// <summary>
            /// Gets or set is parameter flag.
            /// </summary>
            public bool IsParameter { get; set; }

            /// <summary>
            /// Gets or set the description.
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// Gets or set alarm type.
            /// </summary>
            public int? AlarmType { get; set; }

            /// <summary>
            /// Gets or set alarm state.
            /// </summary>
            public int? AlarmState { get; set; }

            /// <summary>
            /// Gets or set the low limit.
            /// </summary>
            public float? LoLimit { get; set; }

            /// <summary>
            /// Gets or set the low low limit.
            /// </summary>
            public float? LoLoLimit { get; set; }

            /// <summary>
            /// Gets or set the high limit.
            /// </summary>
            public float? HiLimit { get; set; }

            /// <summary>
            /// Gets or set the high high limit.
            /// </summary>
            public float? HiHiLimit { get; set; }

            /// <summary>
            /// Gets or set the number days.
            /// </summary>
            public float? NumberDays { get; set; }

            /// <summary>
            /// Gets or set the exact value.
            /// </summary>
            public float? ExactValue { get; set; }

            /// <summary>
            /// Gets or set the value change.
            /// </summary>
            public float? ValueChange { get; set; }

            /// <summary>
            /// Gets or set the percent change.
            /// </summary>
            public int? PercentChange { get; set; }

            /// <summary>
            /// Gets or set the span limit.
            /// </summary>
            public int? SpanLimit { get; set; }

            /// <summary>
            /// Gets or set the ignore zero address.
            /// </summary>
            public int? IgnoreZeroAddress { get; set; }

            /// <summary>
            /// Gets or set the alarm action.
            /// </summary>
            public int? AlarmAction { get; set; }

            /// <summary>
            /// Gets or set the ignore value.
            /// </summary>
            public float? IgnoreValue { get; set; }

            /// <summary>
            /// Gets or set the email on alarm value.
            /// </summary>
            public int EmailOnAlarm { get; set; }

            /// <summary>
            /// Gets or set the email on limit value.
            /// </summary>
            public int EmailOnLimit { get; set; }

            /// <summary>
            /// Gets or set the email on clear value.
            /// </summary>
            public int EmailOnClear { get; set; }

            /// <summary>
            /// Gets or set the page on alarm value.
            /// </summary>
            public int PageOnAlarm { get; set; }

            /// <summary>
            /// Gets or set the page on limit value.
            /// </summary>
            public int PageOnLimit { get; set; }

            /// <summary>
            /// Gets or set the page on clear value.
            /// </summary>
            public int PageOnClear { get; set; }

            /// <summary>
            /// Gets or set the priority.
            /// </summary>
            public int? Priority { get; set; }

            /// <summary>
            /// Gets or set the is push enabled flag.
            /// </summary>
            public bool? IsPushEnabled { get; set; }

        }

        private AlarmData Map(HostAlarmEntry hostAlarmEntry)
        {
            if (hostAlarmEntry == null)
            {
                return null;
            }

            return new AlarmData()
            {
                AlarmState = hostAlarmEntry.AlarmState,
                AlarmDescription = hostAlarmEntry.Description,
                AlarmRegister = hostAlarmEntry.SourceId.ToString(),
                AlarmPriority = hostAlarmEntry.Priority ?? 0,
                AlarmType = "Host",
            };
        }
        #endregion
    }
}
