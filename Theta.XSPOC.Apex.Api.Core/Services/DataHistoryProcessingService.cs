using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Influx.Services;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using Theta.XSPOC.Apex.Kernel.Utilities;
using MathUtility = Theta.XSPOC.Apex.Api.Core.Common.MathUtility;
using UnitCategory = Theta.XSPOC.Apex.Api.Core.Common.UnitCategory;
using Microsoft.Extensions.Configuration;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Implementation of IDataHistoryProcessingService interface.
    /// </summary>
    public class DataHistoryProcessingService : IDataHistoryProcessingService
    {

        #region Private Members

        private readonly IDataHistorySQLStore _dataHistorySQLStore;
        private readonly IThetaLoggerFactory _loggerFactory;
        private readonly ILocalePhrases _localePhrases;
        private readonly IHostAlarm _hostAlarmSQLStore;
        private readonly IGetDataHistoryItemsService _dataHistoryInfluxStore;
        private readonly IPortConfigurationStore _portConfigurationStore;
        private IDictionary<int, string> _phrases;
        private readonly IParameterMongoStore _parameterStore;
        private readonly IConfiguration _configuration;

        private int itemIndex;
        private IList<MeasurementTrendItemModel> measurementTrendData;
        private IList<ControllerTrendItemModel> controllerTrendData;
        private readonly ICommonService _commonService;

        private enum LocalePhraseIDs
        {
            OperationalScore = 6340, // Operational Score         
            DownProductionBOE = 6385, // Down Production BOE    
            LatestProductionBOE = 6383, // Production Latest BOE
            PeakProductionBOE = 6384, // Production Peak BOE   
            CasingPressure = 265, // Casing Pressure   
            TubingPressure = 264, // Tubing Pressure
            InjectionPressure = 20140, // Injection Pressure   
            DifferentialPressure = 20141, // Differential Pressure
            LinePressure = 20142, // Line Pressure
            LineTemperature = 20143, // Line Temperature
            UnitTemperature = 20144, //Unit Temperature
            BoardTemperature = 20145, //Board Temperature
            Battery = 20147, // Battery Voltage
            FCU_DifferentialPressure = 20195, // Weight
            FCU_LinePressure = 20196, // FCU Line Pressure
            FCU_LineTemperature = 20197, //FCU Line Temperature
            FCU_Rate = 20198, // FCU Rate
            ProductionTemp = 4160, // Production Temp.
            FlowRate = 2994, // Flow Rate
            Group = 298, // Group
            Parameters = 655, // Parameters
            Analysis = 440, // Analysis
            RodStressAnalysis = 599, // Rod Stress Analysis
            WellTest = 1352, // Well Test
            Failure = 151, //Failure
            Component = 6475, // Component
            Subcomponent = 6476, // Sub Component
            Meter = 1101, // Meter
            GasLift = 3969, // Gas Lift
            PlungerLift = 20148, // Plunger Lift
            Events = 441, // Events
            InclInj = 6683, // InclInj
            ExclInj = 6684, // ExclInj
            Volume = 513, // Volume
            AccumVolume = 1111, // Accum. Volume
            TargetRate = 1077, // Target Rate
            InstantRate = 1109, // Instant Rate
            Pressure = 754, // Pressure
            TargetPressure = 1074, // Target Pressure
            ValvePosition = 1113, // Valve Position
            Custom = 4188, // Custom ,
            Duration = 689, //Duration
            GasRate = 1251, //Gas Rate
            OilRate = 532, //Oil Rate 
            WaterRate = 1250, //Water Rate
            TotalFluid = 1252, //Total Fluid
            FluidAbovePump = 7022, //Fluid Above Pump
            BottomMinStress = 544, //Bottom Min Stress
            TopMinStress = 545, //Top Min Stress
            TopMaxStress = 546, //Top Max Stress
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="DataHistoryProcessingService"/>.
        /// </summary>
        /// <param name="dataHistorySQLStore">
        /// The <seealso cref="IDataHistorySQLStore"/> service.</param>
        /// <param name="loggerFactory">
        /// The <seealso cref="IThetaLoggerFactory"/> service.</param>
        /// <param name="localePhrases">
        /// The <seealso cref="ILocalePhrases"/> service.</param>
        /// <param name="hostAlarmSQLStore"> The <seealso cref="IHostAlarm"/> service.</param>
        /// <param name="dataHistoryInfluxStore">
        /// The <seealso cref="IGetDataHistoryItemsService"/> service.</param>
        /// <param name="parameterStore">
        /// The <seealso cref="IParameterMongoStore"/> service.</param>        
        /// <param name="portConfigurationStore">The port configuration store.</param>
        /// <param name="commonService">The <see cref="ICommonService"/>.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dataHistorySQLStore"/> is null OR
        /// <paramref name="loggerFactory"/> is null OR
        /// <paramref name="localePhrases"/> is null OR
        /// <paramref name="hostAlarmSQLStore"/> is null OR
        /// <paramref name="dataHistoryInfluxStore"/> is null OR
        /// <paramref name="portConfigurationStore"/> is null OR
        /// <paramref name="parameterStore"/> is null.        
        /// </exception>
        public DataHistoryProcessingService(IDataHistorySQLStore dataHistorySQLStore, IThetaLoggerFactory loggerFactory,
            ILocalePhrases localePhrases, IHostAlarm hostAlarmSQLStore, IGetDataHistoryItemsService dataHistoryInfluxStore,
            IPortConfigurationStore portConfigurationStore, IParameterMongoStore parameterStore, ICommonService commonService, IConfiguration configuration)
        {
            _dataHistorySQLStore = dataHistorySQLStore ?? throw new ArgumentNullException(nameof(dataHistorySQLStore));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _localePhrases = localePhrases ?? throw new ArgumentNullException(nameof(localePhrases));
            _hostAlarmSQLStore = hostAlarmSQLStore ?? throw new ArgumentNullException(nameof(hostAlarmSQLStore));
            _dataHistoryInfluxStore = dataHistoryInfluxStore ?? throw new ArgumentNullException(nameof(dataHistoryInfluxStore));
            _portConfigurationStore = portConfigurationStore ?? throw new ArgumentNullException(nameof(portConfigurationStore));
            _parameterStore = parameterStore ?? throw new ArgumentNullException(nameof(parameterStore));
            _commonService = commonService ?? throw new ArgumentNullException(nameof(commonService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        #endregion

        #region IDataHistoryProcessingService Implementation

        /// <summary>
        /// Processes the Data History Trend Data.
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="correlationId"></param>
        /// <returns>The output response domain object <seealso cref="DataHistoryTrendsListOutput"/>.</returns>
        public DataHistoryTrendsListOutput GetDataHistoryTrendData(string assetId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.TrendData);
            logger.WriteCId(Level.Trace, $"Starting {nameof(DataHistoryProcessingService)} {nameof(GetDataHistoryTrendData)}", correlationId);
            var response = new DataHistoryTrendsListOutput();

            if (string.IsNullOrEmpty(assetId))
            {
                var message = $"{nameof(assetId)} is empty, cannot get trend data results.";
                logger.Write(Level.Info, message);

                return null;
            }

            var dataHistoryItems = _dataHistorySQLStore.GetDataHistoryTrends(assetId, correlationId);

            if (dataHistoryItems != null)
            {
                var localePhraseIds = Enum.GetValues<LocalePhraseIDs>().Cast<int>().ToArray();
                _phrases = _localePhrases.GetAll(correlationId, localePhraseIds);
                response = GetDataHistoryTrendsReponseData(dataHistoryItems);
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryProcessingService)} {nameof(GetDataHistoryTrendData)}", correlationId);

            return response;
        }

        /// <summary>
        /// Processes the Data History Trend Data.
        /// </summary>
        /// <param name="inputData">The <seealso cref="WithCorrelationId{DataHistoryTrendInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The output response domain object <seealso cref="DataHistoryListOutput"/>.</returns>
        public DataHistoryListOutput GetDataHistoryListData(WithCorrelationId<DataHistoryTrendInput> inputData)
        {
            var logger = _loggerFactory.Create(LoggingModel.TrendData);
            logger.WriteCId(Level.Trace, $"Starting {nameof(DataHistoryProcessingService)} {nameof(GetDataHistoryListData)}", inputData?.CorrelationId);
            var response = new DataHistoryListOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            if (inputData == null)
            {
                var message = $"{nameof(inputData)} is null, cannot get data history trend.";
                logger.WriteCId(Level.Info, message, inputData?.CorrelationId);

                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            if (inputData?.Value == null)
            {
                var message = $"{nameof(inputData)} is null, cannot get data history trend.";
                logger.WriteCId(Level.Info, message, inputData?.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            var correlationId = inputData?.CorrelationId;
            var request = inputData.Value;

            if (string.IsNullOrEmpty(request.GroupName) ||
                request.AssetId == Guid.Empty)
            {
                var message = $"{nameof(request.GroupName)} and {nameof(request.AssetId)}" +
                    $" should be provided to get data history trend.";
                logger.WriteCId(Level.Info, message, correlationId);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            var dataHistoryList = _dataHistorySQLStore.GetDataHistoryList(request.AssetId, correlationId);
            if (dataHistoryList != null)
            {
                var localePhraseIds = Enum.GetValues<LocalePhraseIDs>().Cast<int>().ToArray();
                _phrases = _localePhrases.GetAll(correlationId, localePhraseIds);

                response = GetDataHistoryReponseData(dataHistoryList, request.GroupName, correlationId);
                response.Result = new MethodResult<string>(true, string.Empty);
            }
            else
            {
                response.Result.Status = false;
                response.Result.Value = $"cannot trend data history.";
                logger.WriteCId(Level.Info, $"cannot trend data history.", correlationId);
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryProcessingService)} {nameof(GetDataHistoryListData)}", correlationId);

            return response;
        }

        /// <summary>
        /// Gets the Data History Trend Data Items.
        /// </summary>
        /// <param name="inputData">The <seealso cref="WithCorrelationId{DataHistoryTrendInput}"/> to act on, annotated 
        /// with a correlation id.</param>
       // <returns>The <seealso cref="DataHistoryTrendItemsOutput"/> with <seealso cref="List{DataPoint}"/>.</returns>
        public async Task<DataHistoryTrendItemsOutput> GetDataHistoryTrendDataItemsAsync(
            WithCorrelationId<DataHistoryTrendInput> inputData)
        {
            var logger = _loggerFactory.Create(LoggingModel.TrendData);
            logger.WriteCId(Level.Trace, $"Starting {nameof(DataHistoryProcessingService)} {nameof(GetDataHistoryTrendData)}", inputData?.CorrelationId);

            var response = new DataHistoryTrendItemsOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            if (inputData == null)
            {
                var message = $"{nameof(inputData)} is null, cannot get data history trend data items.";
                logger.Write(Level.Info, message);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            if (inputData?.Value == null)
            {
                var message = $"{nameof(inputData)} is null, cannot get data history trend data items.";
                logger.WriteCId(Level.Info, message, inputData?.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            var correlationId = inputData?.CorrelationId;
            var request = inputData.Value;

            var localePhraseIds = Enum.GetValues<LocalePhraseIDs>().Cast<int>().ToArray();
            _phrases = _localePhrases.GetAll(correlationId, localePhraseIds);

            var asset = _portConfigurationStore.GetNode(request.AssetId, inputData.CorrelationId);
            IList<DataPoint> dataPoints = new List<DataPoint>();

            if (asset == null)
            {
                var message = $"{nameof(asset)} is null, cannot get data history trend data items.";
                logger.WriteCId(Level.Info, message, correlationId);

                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            var selectedTypes = request.TypeId.Split(",");
            TreeNodeType treeNodeTypeId;
            var isLegacyWell = await _portConfigurationStore.IsLegacyWellAsync((int)asset?.PortId, inputData.CorrelationId);

            itemIndex = 0;

            measurementTrendData = _dataHistorySQLStore.GetMeasurementTrendItems(asset.NodeId, correlationId);

            controllerTrendData = _dataHistorySQLStore.GetControllerTrendItems(asset.NodeId, asset.PocType, correlationId);

            foreach (var type in selectedTypes)
            {
                treeNodeTypeId = (TreeNodeType)int.Parse(type);

                if (isLegacyWell ||
                    treeNodeTypeId != TreeNodeType.CommonTrend)
                {
                    if (request.AssetId == Guid.Empty || string.IsNullOrEmpty(type) ||
                        string.IsNullOrEmpty(request.ItemId) || string.IsNullOrEmpty(request.StartDate) ||
                        string.IsNullOrEmpty(request.StartDate))
                    {
                        var message = $"{nameof(request.AssetId)}, {nameof(type)}, {nameof(request.ItemId)}," +
                            $" {nameof(request.StartDate)},{nameof(request.StartDate)}" +
                            $" should be provided to get data history trend data items.";
                        logger.WriteCId(Level.Info, message, correlationId);
                        response.Result.Status = false;
                        response.Result.Value = message;
                        return response;
                    }

                    var dataHistoryItems = _dataHistorySQLStore.GetDataHistoryTrendDataItems(request.AssetId.ToString(), correlationId);

                    if (dataHistoryItems == null || dataHistoryItems.NodeMasterData == null)
                    {
                        var message = $"Invalid data. Cannot get data history trend data items.";
                        logger.WriteCId(Level.Info, message, correlationId);
                        response.Result.Status = false;
                        response.Result.Value = message;
                        return response;
                    }

                    dataPoints.AddRange(CreateDataHistoryItemsResponse(dataHistoryItems, request, type, inputData.CorrelationId));
                }
                else
                {
                    if (request.AssetId == Guid.Empty || string.IsNullOrEmpty(type) ||
                        string.IsNullOrEmpty(request.ItemId) || string.IsNullOrEmpty(request.StartDate) ||
                        string.IsNullOrEmpty(request.StartDate))
                    {
                        var message = $"{nameof(inputData)} is null, cannot get data history trend data items.";
                        logger.WriteCId(Level.Info, message, inputData?.CorrelationId);
                        response.Result.Status = false;
                        response.Result.Value = message;
                        return response;
                    }
                    request.CustomerId = asset.CompanyGuid != null ? (Guid)asset.CompanyGuid : Guid.Empty;
                    request.POCType = asset.PocType.ToString();

                    GetAddressAndParamStandardType(type, request.ItemId,
                        out var address, out var paramStdType);

                    var startDate = DateTime.Parse(request.StartDate, null, DateTimeStyles.AssumeLocal).ToUniversalTime();
                    var endDate = DateTime.Parse(request.EndDate, null, DateTimeStyles.AssumeLocal).ToUniversalTime();
                    if (address.Count == 0 && paramStdType.Count == 0)
                    {
                        response.Result.Status = false;
                        response.Result.Value = $"Cannot get data history trend data items for the selected {request.AssetId}";
                    }
                    else
                    {
                        var dataHistoryItems = await _dataHistoryInfluxStore.GetDataHistoryItems(request.AssetId, request.CustomerId,
                        request.POCType, address, paramStdType,
                        startDate.ToString("yyyy-MM-ddTHH:mm:ss"), endDate.ToString("yyyy-MM-ddTHH:mm:ss"));

                        if (dataHistoryItems != null)
                        {
                            response.Result.Status = true;
                            response.Result.Value = string.Empty;

                            dataPoints.AddRange(CreateDataHistoryItemsResponseFromInflux(dataHistoryItems, request.ItemId, address));
                        }
                        else
                        {
                            response.Result.Status = false;
                            response.Result.Value = $"Cannot get data history trend data items for the selected {request.AssetId}";
                        }
                    }
                }
                itemIndex++;
            }

            if (dataPoints != null)
            {
                response.Values = dataPoints.ToList();
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryProcessingService)} {nameof(GetDataHistoryTrendDataItemsAsync)}", correlationId);

            return response;
        }

        /// <summary>
        /// Gets the Data History Alarm Limits.
        /// </summary>
        /// <param name="input">The <seealso cref="WithCorrelationId{DataHistoryAlarmLimitsInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="DataHistoryAlarmLimitsOutput"/>.</returns>
        public DataHistoryAlarmLimitsOutput GetAlarmLimits(WithCorrelationId<DataHistoryAlarmLimitsInput> input)
        {
            var logger = _loggerFactory.Create(LoggingModel.TrendData);
            logger.WriteCId(Level.Trace, $"Starting {nameof(DataHistoryProcessingService)} {nameof(GetAlarmLimits)}", input?.CorrelationId);
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            var values = new List<DataHistoryAlarmLimitsValues>();
            var addressesAsInt = Array.ConvertAll(input.Value.Addresses, int.Parse);
            var result = _hostAlarmSQLStore.GetLimitsForDataHistory(input.Value.AssetId, addressesAsInt, input?.CorrelationId);
            foreach (var item in result)
            {
                if (item.AlarmType <= 0)
                {
                    continue;
                }
                var addressAlarmLimits = new DataHistoryAlarmLimitsValues()
                {
                    Address = item.Address,
                };
                if (item.LoLimit.HasValue)
                {
                    addressAlarmLimits.LoLimit = (float?)Math.Round(item.LoLimit.Value, 3);
                }
                if (item.LoLoLimit.HasValue && item.AlarmType == 2)
                {
                    addressAlarmLimits.LoLoLimit = (float?)Math.Round(item.LoLoLimit.Value, 3);
                }
                if (item.HiLimit.HasValue)
                {
                    addressAlarmLimits.HiLimit = (float?)Math.Round(item.HiLimit.Value, 3);
                }
                if (item.HiHiLimit.HasValue && item.AlarmType == 2)
                {
                    addressAlarmLimits.HiHiLimit = (float?)Math.Round(item.HiHiLimit.Value, 3);
                }
                values.Add(addressAlarmLimits);
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryProcessingService)} {nameof(GetAlarmLimits)}", input?.CorrelationId);

            return new DataHistoryAlarmLimitsOutput()
            {
                Result = new MethodResult<string>(true, string.Empty),
                Values = values,
            };
        }

        /// <summary>
        /// Processes the Data History Trend Data Items.
        /// </summary>
        /// <param name="inputData">The <seealso cref="WithCorrelationId{DataHistoryTrendInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="DataHistoryTrendOutput"/>.</returns>
        public async Task<DataHistoryTrendOutput> GetDefaultTrendDataItemsAsync(WithCorrelationId<DataHistoryTrendInput> inputData)
        {
            var logger = _loggerFactory.Create(LoggingModel.TrendData);
            logger.WriteCId(Level.Trace, $"Starting {nameof(DataHistoryProcessingService)} {nameof(GetDefaultTrendDataItemsAsync)}", inputData?.CorrelationId);

            var response = new DataHistoryTrendOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            if (inputData == null)
            {
                var message = $"{nameof(inputData)} is null, cannot get data history trend data items.";
                logger.Write(Level.Error, message);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            if (inputData?.Value == null)
            {
                var message = $"{nameof(inputData)} is null, cannot get data history trend data items.";
                logger.WriteCId(Level.Error, message, inputData?.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            var request = inputData.Value;

            var localePhraseIds = Enum.GetValues<LocalePhraseIDs>().Cast<int>().ToArray();
            _phrases = _localePhrases.GetAll(inputData.CorrelationId, localePhraseIds);

            var asset = _portConfigurationStore.GetNode(request.AssetId, inputData.CorrelationId);

            if (asset == null)
            {
                var message = $"{nameof(asset)} is null, cannot get data history trend data items.";
                logger.Write(Level.Error, message);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            var isLegacyWell = await _portConfigurationStore.IsLegacyWellAsync((int)asset?.PortId, inputData.CorrelationId);

            if (!string.IsNullOrEmpty(request.ViewId))
            {
                request.AssetId = asset.AssetGuid;
                request.CustomerId = asset.CompanyGuid != null ? (Guid)asset.CompanyGuid : Guid.Empty;
                request.POCType = asset.PocType.ToString();
                var defaultTrends = _dataHistorySQLStore.GetDefaultTrendsData(request.ViewId, inputData?.CorrelationId);
                GraphViewSettingsModel defaultTrendSettings = new GraphViewSettingsModel();

                if (string.IsNullOrEmpty(request.StartDate) && string.IsNullOrEmpty(request.EndDate))
                {
                    defaultTrendSettings = _dataHistorySQLStore.GetDefaultTrendViewSettings(request.ViewId, inputData?.CorrelationId);
                }
                else
                {
                    defaultTrendSettings = new GraphViewSettingsModel
                    {
                        ViewId = int.Parse(request.ViewId),
                        StartDate = request.StartDate,
                        EndDate = request.EndDate
                    };
                }

                if (defaultTrends != null && defaultTrends.Count > 0)
                {
                    var values = await GetDefaultTrendDataPointAsync(defaultTrends, defaultTrendSettings, isLegacyWell, request, inputData.CorrelationId);
                    RoundYValueToSignificantDigits(ref values, inputData.CorrelationId);

                    response.Values = values;
                    response.Result = new MethodResult<string>(true, string.Empty);
                }
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryProcessingService)} {nameof(GetAlarmLimits)}", inputData?.CorrelationId);

            return response;
        }

        private void RoundYValueToSignificantDigits(ref List<GraphViewTrendsData> values, string correlationId)
        {
            if (values == null)
            {
                return;
            }

            var digits = _commonService.GetSystemParameterNextGenSignificantDigits(correlationId);

            foreach (var trendData in values)
            {
                if (trendData.AxisValues == null)
                {
                    continue;
                }

                foreach (var item in trendData.AxisValues)
                {
                    if (!double.TryParse(item.Y.ToString(), out var yDouble))
                    {
                        continue;
                    }

                    var yDoubleRounded = MathUtility.RoundToSignificantDigits(yDouble, digits);

                    if (decimal.TryParse(yDoubleRounded.ToString(), out var yDecimalRounded))
                    {
                        item.Y = yDecimalRounded;
                    }

                }
            }
        }

        /// <summary>
        /// Processes the Data History Trend Data Items.
        /// </summary>
        /// <param name="inputData">The <seealso cref="WithCorrelationId{DataHistoryTrendInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="DataHistoryDefaultTrendOutput"/>.</returns>
        public DataHistoryDefaultTrendOutput GetDefaultTrendsViews(WithCorrelationId<DataHistoryTrendInput> inputData)
        {
            var logger = _loggerFactory.Create(LoggingModel.TrendData);
            logger.WriteCId(Level.Trace, $"Starting {nameof(DataHistoryProcessingService)} {nameof(GetDefaultTrendsViews)}", inputData?.CorrelationId);

            var response = new DataHistoryDefaultTrendOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            if (inputData == null)
            {
                var message = $"{nameof(inputData)} is null, cannot get default trends views.";
                logger.Write(Level.Error, message);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            if (inputData?.Value == null)
            {
                var message = $"{nameof(inputData)} is null, cannot get default trends views.";
                logger.WriteCId(Level.Error, message, inputData?.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            var request = inputData.Value;

            var defaultTrendView = _dataHistorySQLStore.GetDefaultTrendViews(request.UserId, inputData?.CorrelationId);
            if (defaultTrendView != null)
            {
                var values = defaultTrendView
                    .Select(a => new DataHistoryDefaultTrendData
                    {
                        ViewId = a.ViewId,
                        ViewName = a.ViewName,
                        StartDate = a.StartDate,
                        EndDate = a.EndDate,
                        IsSelected = a.IsSelected,
                        IsGlobal = a.IsGlobal,
                    }).ToList();

                if (values.Count > 0 && !values.Any(a => a.IsSelected))
                {
                    values.First().IsSelected = true;
                }

                response.Values = values;
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryProcessingService)} {nameof(GetDefaultTrendsViews)}", inputData?.CorrelationId);

            return response;
        }

        /// <summary>
        /// Processes the Data History Trend Data Items.
        /// </summary>
        /// <param name="input">The <seealso cref="WithCorrelationId{TrendIDataInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="DataHistoryListOutput"/>.</returns>
        public async Task<DataHistoryTrendOutput> GetTrendDataAsync(WithCorrelationId<TrendIDataInput> input)
        {
            var logger = _loggerFactory.Create(LoggingModel.TrendData);
            logger.WriteCId(Level.Trace, $"Starting {nameof(DataHistoryProcessingService)} {nameof(GetTrendDataAsync)}", input?.CorrelationId);

            var response = new DataHistoryTrendOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            if (input == null)
            {
                var message = $"{nameof(input)} is null, cannot get data history trend data items.";
                logger.Write(Level.Error, message);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            if (input?.Value == null)
            {
                var message = $"{nameof(input)} is null, cannot get data history trend data items.";
                logger.WriteCId(Level.Error, message, input?.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            var request = input.Value;

            var localePhraseIds = Enum.GetValues<LocalePhraseIDs>().Cast<int>().ToArray();
            _phrases = _localePhrases.GetAll(input.CorrelationId, localePhraseIds);

            var asset = _portConfigurationStore.GetNode(request.AssetId, input.CorrelationId);

            if (asset == null)
            {
                var message = $"{nameof(asset)} is null, cannot get data history trend data items.";
                logger.Write(Level.Error, message);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            var isLegacyWell = await _portConfigurationStore.IsLegacyWellAsync((int)asset?.PortId, input.CorrelationId);

            request.AssetId = asset.AssetGuid;
            request.CustomerId = asset.CompanyGuid != null ? (Guid)asset.CompanyGuid : Guid.Empty;
            request.POCType = asset.PocType.ToString();

            var dataHistoryItems = _dataHistorySQLStore.GetDataHistoryTrendDataItems(request.AssetId.ToString(),
                input.CorrelationId);

            if (dataHistoryItems == null || dataHistoryItems.NodeMasterData == null)
            {
                var message = $"Invalid data. Cannot get data history trend data items.";
                logger.WriteCId(Level.Error, message, input.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;
                return response;
            }

            measurementTrendData = _dataHistorySQLStore.GetMeasurementTrendItems(asset.NodeId, input.CorrelationId);

            controllerTrendData = _dataHistorySQLStore.GetControllerTrendItems(asset.NodeId, asset.PocType, input.CorrelationId);

            var outputResponse = new List<GraphViewTrendsData>();
            if (!request.IsOverlay)
            {
                var chartTypes = new List<string>() { request.Chart1Type, request.Chart2Type, request.Chart3Type, request.Chart4Type };
                var chartItemIds = new List<string>() { request.Chart1ItemId, request.Chart2ItemId, request.Chart3ItemId, request.Chart4ItemId };

                outputResponse.AddRange(await GetTrendDataByAxesAsync(
                    input.CorrelationId, dataHistoryItems, request, isLegacyWell, chartTypes, chartItemIds));
            }
            else
            {
                outputResponse.AddRange(await GetTrendDataByAxisAsync(dataHistoryItems, request, request.Chart1Type,
                    request.Chart1ItemId, isLegacyWell, 0, input.CorrelationId));
            }

            RoundYValueToSignificantDigits(ref outputResponse, input.CorrelationId);

            response.Values = outputResponse;
            response.Result = new MethodResult<string>(true, string.Empty);
            logger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryProcessingService)} {nameof(GetTrendDataAsync)}", input?.CorrelationId);

            return response;
        }

        #endregion

        #region Private Methods

        private DataHistoryTrendsListOutput GetDataHistoryTrendsReponseData(DataHistoryItemModel dataHistoryItems)
        {
            var response = new DataHistoryTrendsListOutput();

            var espAnalysisTrendResult = ESPAnalysisTrendData.GetItems();
            if (espAnalysisTrendResult == null)
            {
                return null;
            }
            response.ESPAnalysisTrendDataItems = espAnalysisTrendResult;

            var glAnalysisTrendResult = GLAnalysisTrendData.GetItems();
            if (glAnalysisTrendResult == null)
            {
                return null;
            }
            response.GLAnalysisTrendDataItems = glAnalysisTrendResult;

            var operationalScoreTrendData = OperationalScoreTrendData.GetItems(_phrases);
            if (operationalScoreTrendData == null)
            {
                return null;
            }
            response.OperationalScoreTrendDataItems = operationalScoreTrendData;

            var productionStatisticsTrendData = ProductionStatisticsTrendData.GetItems(_phrases);
            if (productionStatisticsTrendData == null)
            {
                return null;
            }
            response.ProductionStatisticsTrendDataItems = productionStatisticsTrendData;

            if (dataHistoryItems.FailureComponentTrendData == null || dataHistoryItems.FailureSubComponentTrendData == null
                || dataHistoryItems.MeterTrendData == null || dataHistoryItems.PCSFDatalogConfiguration == null
                || dataHistoryItems.EventsTrendData == null)
            {
                return null;
            }
            response.MeterColumnDataItems = dataHistoryItems.MeterTrendData;
            response.FailureComponentTrendData = dataHistoryItems.FailureComponentTrendData;
            response.FailureSubComponentTrendData = dataHistoryItems.FailureSubComponentTrendData;
            response.EventsTrendDataItems = dataHistoryItems.EventsTrendData;

            var plungerLiftTrend = PlungerLiftTrendData.GetItems(dataHistoryItems.NodeMasterData.NodeId,
                dataHistoryItems.NodeMasterData.PocType, _phrases);
            if (plungerLiftTrend == null)
            {
                return null;
            }
            response.PlungerLiftTrendDataItems = plungerLiftTrend;

            var pcsfDatalogConfigurationTrendData = new Dictionary<int, PCSFDatalogConfiguration>();
            foreach (var item in dataHistoryItems.PCSFDatalogConfiguration)
            {
                pcsfDatalogConfigurationTrendData.Add(item.DatalogNumber, new PCSFDatalogConfiguration(item));
            }
            response.PCSFDatalogConfigurationData = pcsfDatalogConfigurationTrendData;

            return response;
        }

        /// <summary>
        /// Gets the esp analysis and pcsf datalog data history items trend items i.e <seealso cref="IList{DataPoint}"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="measurement">The measurement parameter.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{DataPoint}"/>.</returns>
        private IList<DataPoint> GetESPAnalysisDataHistoryTrendItems(string nodeId, int measurement,
            DateTime startDate, DateTime endDate, string correlationId)
        {
            IList<DataPoint> data = new List<DataPoint>();

            var espAnalysisResult = _dataHistorySQLStore.SearchESPAnalysisResult(nodeId, startDate, endDate, correlationId);

            if (espAnalysisResult != null && espAnalysisResult.Any())
            {
                var parameterRecords = GetESPAnalysisTrendDataParameterRecord(espAnalysisResult,
                    EnhancedEnumBase.GetValue<ESPAnalysisMeasurement>(measurement));

                foreach (var record in parameterRecords)
                {
                    if (record.Value != null)
                    {
                        data.Add(new DataPoint(record.RecordDateTime.Value, (decimal)record.Value,
                            string.Empty, EnhancedEnumBase.GetValue<ESPAnalysisMeasurement>(measurement)));
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// Gets the esp analysis and pcsf datalog data history items trend items i.e <seealso cref="IList{DataPoint}"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="measurement">The measurement parameter.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{DataPoint}"/>.</returns>
        private IList<DataPoint> GetGLAnalysisDataHistoryTrendItems(string nodeId, int measurement,
            DateTime startDate, DateTime endDate, string correlationId)
        {
            IList<DataPoint> data = new List<DataPoint>();

            var glAnalysisResult = _dataHistorySQLStore.SearchGLAnalysisResult(nodeId, startDate, endDate, correlationId);

            if (glAnalysisResult != null && glAnalysisResult.Any())
            {
                var parameterRecords = GetGLAnalysisTrendDataParameterRecord(glAnalysisResult,
                    EnhancedEnumBase.GetValue<GLAnalysisMeasurement>(measurement), correlationId);

                foreach (var record in parameterRecords)
                {
                    if (record.Value != null)
                    {
                        data.Add(new DataPoint(record.RecordDateTime.Value, (decimal)record.Value,
                            string.Empty, EnhancedEnumBase.GetValue<GLAnalysisMeasurement>(measurement)));
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// Gets the pcsf datalog data history items trend items i.e <seealso cref="IList{DataPoint}"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="datalogNumber">The node id.</param>
        /// <param name="fieldNumber">The node id.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{DataPoint}"/>.</returns>
        private IList<DataPoint> GetPCSFDatalogRecordTrendItems(string nodeId, int datalogNumber, int fieldNumber,
            DateTime startDate, DateTime endDate, string correlationId)
        {
            IList<DataPoint> data = new List<DataPoint>();

            var pcsfDatalogRecordItems = _dataHistorySQLStore.GetPCSFDatalogRecordItems(nodeId, datalogNumber,
                startDate, endDate, correlationId);

            SortedDictionary<DateTime, PCSFDatalogRecord> pcsfDatalogRecords = new SortedDictionary<DateTime, PCSFDatalogRecord>();

            foreach (var dataItem in pcsfDatalogRecordItems)
            {
                var record = new PCSFDatalogRecord(dataItem);
                pcsfDatalogRecords.Add(record.LogDateTime, record);
            }

            foreach (KeyValuePair<DateTime, PCSFDatalogRecord> dataItem in pcsfDatalogRecords)
            {
                GetDataValueForField(dataItem, data, fieldNumber);
            }

            return data;
        }

        private IList<ParameterRecord> GetESPAnalysisTrendDataParameterRecord(IEnumerable<ESPAnalysisResultModel> data, ESPAnalysisMeasurement measurement)
        {
            var results = new List<ParameterRecord>();

            IEnumerable<KeyValuePair<DateTime, float?>> columns;

            if (measurement == ESPAnalysisMeasurement.NumberOfStages)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.NumberOfStages));
            }
            else if (measurement == ESPAnalysisMeasurement.VerticalPumpDepth)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.VerticalPumpDepth));
            }
            else if (measurement == ESPAnalysisMeasurement.MeasuredPumpDepth)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.MeasuredPumpDepth));
            }
            else if (measurement == ESPAnalysisMeasurement.OilRate)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.OilRate));
            }
            else if (measurement == ESPAnalysisMeasurement.WaterRate)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.WaterRate));
            }
            else if (measurement == ESPAnalysisMeasurement.GasRate)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.GasRate));
            }
            else if (measurement == ESPAnalysisMeasurement.PumpIntakePressure)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.PumpIntakePressure));
            }
            else if (measurement == ESPAnalysisMeasurement.GrossRate)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.GrossRate));
            }
            else if (measurement == ESPAnalysisMeasurement.FluidLevelAbovePump)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.FluidLevelAbovePump));
            }
            else if (measurement == ESPAnalysisMeasurement.TubingPressure)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.TubingPressure));
            }
            else if (measurement == ESPAnalysisMeasurement.CasingPressure)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.CasingPressure));
            }
            else if (measurement == ESPAnalysisMeasurement.Frequency)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.Frequency));
            }
            else if (measurement == ESPAnalysisMeasurement.ProductivityIndex)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.ProductivityIndex));
            }
            else if (measurement == ESPAnalysisMeasurement.PressureAcrossPump)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.PressureAcrossPump));
            }
            else if (measurement == ESPAnalysisMeasurement.PumpDischargePressure)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.PumpDischargePressure));
            }
            else if (measurement == ESPAnalysisMeasurement.HeadAcrossPump)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.HeadAcrossPump));
            }
            else if (measurement == ESPAnalysisMeasurement.FrictionalLossInTubing)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.FrictionalLossInTubing));
            }
            else if (measurement == ESPAnalysisMeasurement.PumpEfficiency)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.PumpEfficiency));
            }
            else if (measurement == ESPAnalysisMeasurement.CalculatedFluidLevelAbovePump)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.CalculatedFluidLevelAbovePump));
            }
            else if (measurement == ESPAnalysisMeasurement.FluidSpecificGravity)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.FluidSpecificGravity));
            }
            else if (measurement == ESPAnalysisMeasurement.PumpStaticPressure)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.PumpStaticPressure));
            }
            else if (measurement == ESPAnalysisMeasurement.RateAtBubblePoint)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.RateAtBubblePoint));
            }
            else if (measurement == ESPAnalysisMeasurement.RateAtMaxLiquid)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.RateAtMaxLiquid));
            }
            else if (measurement == ESPAnalysisMeasurement.RateAtMaxOil)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.RateAtMaxOil));
            }
            else if (measurement == ESPAnalysisMeasurement.IPRSlope)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.Iprslope));
            }
            else if (measurement == ESPAnalysisMeasurement.WaterCut)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.WaterCut));
            }
            else if (measurement == ESPAnalysisMeasurement.GasOilRatioAtPump)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.GasOilRatioAtPump));
            }
            else if (measurement == ESPAnalysisMeasurement.FormationVolumeFactor)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.FormationVolumeFactor));
            }
            else if (measurement == ESPAnalysisMeasurement.GasCompressibilityFactor)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.GasCompressibilityFactor));
            }
            else if (measurement == ESPAnalysisMeasurement.GasVolumeFactor)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.GasVolumeFactor));
            }
            else if (measurement == ESPAnalysisMeasurement.ProducingGasOilRatio)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.ProducingGor));
            }
            else if (measurement == ESPAnalysisMeasurement.GasInSolution)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.GasInSolution));
            }
            else if (measurement == ESPAnalysisMeasurement.FreeGasAtPump)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.FreeGasAtPump));
            }
            else if (measurement == ESPAnalysisMeasurement.OilVolumeAtPump)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.OilVolumeAtPump));
            }
            else if (measurement == ESPAnalysisMeasurement.GasVolumeAtPump)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.GasVolumeAtPump));
            }
            else if (measurement == ESPAnalysisMeasurement.TotalVolumeAtPump)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.TotalVolumeAtPump));
            }
            else if (measurement == ESPAnalysisMeasurement.FreeGas)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.FreeGas));
            }
            else if (measurement == ESPAnalysisMeasurement.CompositeTubingSpecificGravity)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.CompositeTubingSpecificGravity));
            }
            else if (measurement == ESPAnalysisMeasurement.GasDensity)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.GasDensity));
            }
            else if (measurement == ESPAnalysisMeasurement.LiquidDensity)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.LiquidDensity));
            }
            else if (measurement == ESPAnalysisMeasurement.AnnularSeparationEfficiency)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.AnnularSeparationEfficiency));
            }
            else if (measurement == ESPAnalysisMeasurement.TubingGas)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.TubingGas));
            }
            else if (measurement == ESPAnalysisMeasurement.TubingGasOilRatio)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.TubingGor));
            }
            else if (measurement == ESPAnalysisMeasurement.FlowingBHP)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.FlowingBhp));
            }
            else if (measurement == ESPAnalysisMeasurement.HeadRelativeToRecommendedRange)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.HeadRelativeToRecommendedRange));
            }
            else if (measurement == ESPAnalysisMeasurement.FlowRelativeToRecommendedRange)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.FlowRelativeToRecommendedRange));
            }
            else if (measurement == ESPAnalysisMeasurement.DesignScore)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.DesignScore));
            }
            else if (measurement == ESPAnalysisMeasurement.HeadRelativeToWellPerformanceCurve)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.HeadRelativeToWellPerformance));
            }
            else if (measurement == ESPAnalysisMeasurement.HeadRelativeToPumpCurve)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.HeadRelativeToPumpCurve));
            }
            else if (measurement == ESPAnalysisMeasurement.PumpDegradation)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.PumpDegradation));
            }
            else if (measurement == ESPAnalysisMeasurement.MaxPotentialProductionRate)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.MaxPotentialProductionRate));
            }
            else if (measurement == ESPAnalysisMeasurement.MaxPotentialFrequency)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.MaxPotentialFrequency));
            }
            else if (measurement == ESPAnalysisMeasurement.ProductionIncreasePercentage)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.ProductionIncreasePercentage));
            }
            else if (measurement == ESPAnalysisMeasurement.TurpinParameter)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.TurpinParameter));
            }
            else
            {
                throw new ArgumentException("Invalid measurement. Unable to query data from ESP Analysis Results.");
            }

            foreach (var record in columns)
            {
                results.Add(new ParameterRecord()
                {
                    RecordDateTime = record.Key,
                    Value = record.Value,
                });
            }

            return results;
        }

        private IList<ParameterRecord> GetGLAnalysisTrendDataParameterRecord(IEnumerable<GLAnalysisResultModel> data,
            GLAnalysisMeasurement measurement, string correlationId)
        {
            var results = new List<ParameterRecord>();

            IEnumerable<KeyValuePair<DateTime, float?>> columns;

            if (measurement == GLAnalysisMeasurement.GasInjectionDepth)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.GasInjectionDepth));
            }
            else if (measurement == GLAnalysisMeasurement.VerticalWellDepth)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.VerticalWellDepth));
            }
            else if (measurement == GLAnalysisMeasurement.MeasuredWellDepth)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.MeasuredWellDepth));
            }
            else if (measurement == GLAnalysisMeasurement.OilRate)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.OilRate));
            }
            else if (measurement == GLAnalysisMeasurement.WaterRate)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.WaterRate));
            }
            else if (measurement == GLAnalysisMeasurement.GasRate)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.GasRate));
            }
            else if (measurement == GLAnalysisMeasurement.WellheadPressure)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.WellheadPressure));
            }
            else if (measurement == GLAnalysisMeasurement.CasingPressure)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.CasingPressure));
            }
            else if (measurement == GLAnalysisMeasurement.OilSpecificGravity)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.OilSpecificGravity));
            }
            else if (measurement == GLAnalysisMeasurement.WaterSpecificGravity)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.WaterSpecificGravity));
            }
            else if (measurement == GLAnalysisMeasurement.GasSpecificGravity)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.GasSpecificGravity));
            }
            else if (measurement == GLAnalysisMeasurement.TubingInnerDiameter)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.TubingId));
            }
            else if (measurement == GLAnalysisMeasurement.TubingOuterDiameter)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.TubingOD));
            }
            else if (measurement == GLAnalysisMeasurement.WellheadTemperature)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.WellheadTemperature));
            }
            else if (measurement == GLAnalysisMeasurement.PercentH2S)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.PercentH2S));
            }
            else if (measurement == GLAnalysisMeasurement.PercentN2)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.PercentN2));
            }
            else if (measurement == GLAnalysisMeasurement.PercentCO2)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.PercentCO2));
            }
            else if (measurement == GLAnalysisMeasurement.ProductivityIndex)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.ProductivityIndex));
            }
            else if (measurement == GLAnalysisMeasurement.RateAtBubblePoint)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.RateAtBubblePoint));
            }
            else if (measurement == GLAnalysisMeasurement.RateAtMaxLiquid)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.RateAtMaxLiquid));
            }
            else if (measurement == GLAnalysisMeasurement.RateAtMaxOil)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.RateAtMaxOil));
            }
            else if (measurement == GLAnalysisMeasurement.IPRSlope)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.IPRSlope));
            }
            else if (measurement == GLAnalysisMeasurement.WaterCut)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.WaterCut));
            }
            else if (measurement == GLAnalysisMeasurement.FlowingBHP)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.FlowingBhp));
            }
            else if (measurement == GLAnalysisMeasurement.InjectedGLR)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.InjectedGLR));
            }
            else if (measurement == GLAnalysisMeasurement.InjectedGasRate)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.InjectedGasRate));
            }
            else if (measurement == GLAnalysisMeasurement.MaxLiquidRate)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.MaxLiquidRate));
            }
            else if (measurement == GLAnalysisMeasurement.InjectionRateForMaxLiquidRate)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.InjectionRateForMaxLiquidRate));
            }
            else if (measurement == GLAnalysisMeasurement.GLRForMaxLiquidRate)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.GLRForMaxLiquidRate));
            }
            else if (measurement == GLAnalysisMeasurement.OptimumLiquidRate)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.OptimumLiquidRate));
            }
            else if (measurement == GLAnalysisMeasurement.InjectionRateForOptimumLiquidRate)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.InjectionRateForOptimumLiquidRate));
            }
            else if (measurement == GLAnalysisMeasurement.GLRForOptimumLiquidRate)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.GlrforOptimumLiquidRate));
            }
            else if (measurement == GLAnalysisMeasurement.MinimumFlowingBHP)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.MinimumFbhp));
            }
            else if (measurement == GLAnalysisMeasurement.TubingCriticalVelocity)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.TubingCriticalVelocity));
            }
            else if (measurement == GLAnalysisMeasurement.ValveCriticalVelocity)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.ValveCriticalVelocity));
            }
            else if (measurement == GLAnalysisMeasurement.FormationGOR)
            {
                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, a.FormationGor));
            }
            else if (measurement == GLAnalysisMeasurement.TotalGLR)
            {
                var includeInjectedGasInWellTestSetting = _dataHistorySQLStore.GetGLIncludeInjGasInTestValue(correlationId);

                bool includeInjectedGasInWellTestGasRate = !string.IsNullOrEmpty(includeInjectedGasInWellTestSetting)
                    && Convert.ToBoolean(includeInjectedGasInWellTestSetting);

                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate,
                CalculateTotalGLR(includeInjectedGasInWellTestGasRate, a.GasRate, a.InjectedGasRate,
                  a.GrossRate)));
            }
            else if (measurement == GLAnalysisMeasurement.FirstInjectingValveDepth)
            {
                var value = _dataHistorySQLStore.GetFirstInjectingFlowControlDeviceDepth(data.Select(a => a.Id).FirstOrDefault(), correlationId);

                columns = data.Select(a => new KeyValuePair<DateTime, float?>(a.TestDate, value));
            }
            else
            {
                throw new ArgumentException("Invalid measurement. Unable to query data from GL Analysis Results.");
            }

            foreach (var record in columns)
            {
                results.Add(new ParameterRecord()
                {
                    RecordDateTime = record.Key,
                    Value = record.Value,
                });
            }

            return results;
        }

        private DataHistoryListOutput GetDataHistoryReponseData(DataHistoryModel dataHistoryList, string assetGroupName, string correlationId)
        {
            var response = new DataHistoryListOutput();

            IList<DataHistoryListItem> values = new List<DataHistoryListItem>();
            var asset = dataHistoryList.NodeMasterData;

            if (dataHistoryList.GroupTrendData != null)
            {
                var groupDataTrend = new DataHistoryListItem()
                {
                    Id = assetGroupName,
                    Name = $"{_phrases[(int)LocalePhraseIDs.Group]}: {assetGroupName}",
                    TypeId = (int)TreeNodeType.GroupDataRoot,
                    Items = dataHistoryList.GroupTrendData.Select(x => new DataHistoryListItem()
                    {
                        Id = x.Description,
                        Name = x.Description,
                        TypeId = (int)TreeNodeType.GroupDataTrend,
                    }).OrderBy(a => a.Id).ToList(),
                };

                values.Add(groupDataTrend);
            }

            var commonTrend = new DataHistoryListItem()
            {
                Id = _phrases[(int)LocalePhraseIDs.Parameters],
                Name = _phrases[(int)LocalePhraseIDs.Parameters],
                TypeId = (int)TreeNodeType.CommonRoot,
                Items = new List<DataHistoryListItem>(),
            };
            foreach (var item in GetMeasurementTrendDataItems(dataHistoryList.MeasurementTrendData))
            {
                commonTrend.Items.Add(new DataHistoryListItem()
                {
                    Name = item.Name == "Gas Rate"
                                ? GetGasRatePhrase(item.Description, asset.ApplicationId, dataHistoryList.GLIncludeInjGasInTest)
                                : item.Description,
                    TypeId = (int)TreeNodeType.CommonTrend,
                    Id = item.PriorityAddress.ToString(),
                });
            }

            foreach (var item in GetControllerTrendDataItems(dataHistoryList.ControllerTrendData))
            {
                commonTrend.Items.Add(new DataHistoryListItem()
                {
                    Name = item.Name == "Gas Rate"
                        ? GetGasRatePhrase(item.Description, asset.ApplicationId, dataHistoryList.GLIncludeInjGasInTest)
                        : item.Description,
                    TypeId = (int)TreeNodeType.CommonTrend,
                    Id = item.Address.ToString(),
                });
            }

            var commonTrendData = new List<TrendData>();
            commonTrendData.AddRange(GetProductionStatisticsTrendDataItems());
            commonTrendData.AddRange(GetOperationalScoreTrendDataItems());
            commonTrend.Items.AddRange(commonTrendData.Select(x => new DataHistoryListItem()
            {
                Name = x.Name == "Gas Rate"
                    ? GetGasRatePhrase(x.Description, asset.ApplicationId, dataHistoryList.GLIncludeInjGasInTest)
                    : x.Description,
                TypeId = (int)TreeNodeType.CommonTrend,
                Id = x.Name == "Gas Rate"
                    ? GetGasRatePhrase(x.Description, asset.ApplicationId, dataHistoryList.GLIncludeInjGasInTest)
                    : x.Description,
            }).ToList());
            commonTrend.Items = commonTrend.Items.OrderBy(x => x.Name).ToList();
            values.Add(commonTrend);

            if (asset.ApplicationId == 4) // ESP
            {
                var espTrendData = new List<TrendData>();
                espTrendData.AddRange(ESPAnalysisTrendData.GetItems());

                var espTrend = new DataHistoryListItem()
                {
                    Id = _phrases[(int)LocalePhraseIDs.Analysis],
                    Name = _phrases[(int)LocalePhraseIDs.Analysis],
                    TypeId = (int)TreeNodeType.AnalysisRoot,
                    Items = espTrendData.Select(x => new DataHistoryListItem()
                    {
                        Name = x.Description,
                        Id = x.Description,
                        TypeId = (int)TreeNodeType.AnalysisTrend,
                    }).OrderBy(a => a.Id).ToList(),
                };

                values.Add(espTrend);
            }
            else if (asset.ApplicationId == 7) // GL
            {
                var glTrendData = new List<TrendData>();
                glTrendData.AddRange(GLAnalysisTrendData.GetItems());

                var glTrend = new DataHistoryListItem()
                {
                    Id = _phrases[(int)LocalePhraseIDs.Analysis],
                    Name = _phrases[(int)LocalePhraseIDs.Analysis],
                    TypeId = (int)TreeNodeType.AnalysisRoot,
                    Items = glTrendData.Select(x => new DataHistoryListItem()
                    {
                        Name = x.Description,
                        Id = x.Description,
                        TypeId = (int)TreeNodeType.AnalysisTrend,
                    }).OrderBy(a => a.Id).ToList(),
                };

                values.Add(glTrend);
            }
            else if (asset.ApplicationId == 3) // RL
            {
                var rlTrendData = new List<TrendData>();
                rlTrendData.AddRange(AnalysisTrendData.GetItems(dataHistoryList.AnalysisTrendData));

                var rlTrend = new DataHistoryListItem()
                {
                    Id = _phrases[(int)LocalePhraseIDs.Analysis],
                    Name = _phrases[(int)LocalePhraseIDs.Analysis],
                    TypeId = (int)TreeNodeType.AnalysisRoot,
                    Items = rlTrendData.Select(x => new DataHistoryListItem()
                    {
                        Name = x.Description,
                        Id = x.Description,
                        TypeId = (int)TreeNodeType.AnalysisTrend,
                    }).OrderBy(a => a.Id).ToList(),
                };

                values.Add(rlTrend);

                var rodStressTrendData = new List<TrendData>();
                rodStressTrendData.AddRange(RodStressTrendData.GetItems(dataHistoryList.RodStressTrendData, _phrases));

                var rodStress = new DataHistoryListItem()
                {
                    Id = _phrases[(int)LocalePhraseIDs.RodStressAnalysis],
                    Name = _phrases[(int)LocalePhraseIDs.RodStressAnalysis],
                    TypeId = (int)TreeNodeType.RodStressRoot,
                    Items = rodStressTrendData.Select(x => new DataHistoryListItem()
                    {
                        Name = x.Description,
                        Id = x.Description,
                        TypeId = (int)TreeNodeType.RodStressTrend,
                    }).OrderBy(a => a.Id).ToList(),
                };

                values.Add(rodStress);
            }

            if (HasWellTestOrFailureTrend(asset.ApplicationId))
            {
                var wellTestTrendData = new List<TrendData>();
                wellTestTrendData.AddRange(GetWellTestTrendDataItems());

                var wellTestTrend = new DataHistoryListItem()
                {
                    Id = _phrases[(int)LocalePhraseIDs.WellTest],
                    Name = _phrases[(int)LocalePhraseIDs.WellTest],
                    TypeId = (int)TreeNodeType.WellTestRoot,
                    Items = wellTestTrendData.Select(x => new DataHistoryListItem()
                    {
                        Name = x.Name == "Gas Rate"
                            ? GetGasRatePhrase(x.Description, asset.ApplicationId, dataHistoryList.GLIncludeInjGasInTest)
                            : x.Description,
                        TypeId = (int)TreeNodeType.WellTestTrend,
                        Id = x.Name == "Gas Rate"
                            ? GetGasRatePhrase(x.Description, asset.ApplicationId, dataHistoryList.GLIncludeInjGasInTest)
                            : x.Description,
                    }).OrderBy(a => a.Id).ToList(),
                };

                values.Add(wellTestTrend);

                var failureComponentTrend = new DataHistoryListItem()
                {
                    Id = $"{_phrases[(int)LocalePhraseIDs.Failure]}: {_phrases[(int)LocalePhraseIDs.Component]}",
                    Name = $"{_phrases[(int)LocalePhraseIDs.Failure]}: {_phrases[(int)LocalePhraseIDs.Component]}",
                    TypeId = (int)TreeNodeType.FailureComponentRoot,
                    Items = dataHistoryList.FailureComponentTrendData
                    .Where(a => a.Application == asset.ApplicationId)
                    .Select(x => new DataHistoryListItem()
                    {
                        Name = x.Name,
                        Id = x.Id.ToString(),
                        TypeId = (int)TreeNodeType.FailureComponentTrend,
                    }).OrderBy(a => a.Name).ToList(),
                };

                values.Add(failureComponentTrend);

                var failureComponentIds = failureComponentTrend.Items.Select(a => a.Id).ToArray();

                var failureSubComponentTrend = new DataHistoryListItem()
                {
                    Id = $"{_phrases[(int)LocalePhraseIDs.Failure]}: {_phrases[(int)LocalePhraseIDs.Subcomponent]}",
                    Name = $"{_phrases[(int)LocalePhraseIDs.Failure]}: {_phrases[(int)LocalePhraseIDs.Subcomponent]}",
                    TypeId = (int)TreeNodeType.FailureSubcomponentRoot,
                    Items = dataHistoryList.FailureSubComponentTrendData
                    .Where(a => failureComponentIds.Contains(a.ComponentId.ToString()))
                    .Select(x => new DataHistoryListItem()
                    {
                        Name = x.Name,
                        Id = x.Id.ToString(),
                        TypeId = (int)TreeNodeType.FailureSubcomponentTrend,
                    }).OrderBy(a => a.Name).ToList(),
                };

                values.Add(failureSubComponentTrend);
            }

            if (IsMeter(asset.PocType))
            {
                var meterTrendData = GetMeterTrendData(dataHistoryList.MeterTrendData, asset.PocType);

                var meterTrend = new DataHistoryListItem()
                {
                    Id = _phrases[(int)LocalePhraseIDs.Meter],
                    Name = _phrases[(int)LocalePhraseIDs.Meter],
                    TypeId = (int)TreeNodeType.MeterRoot,
                    Items = meterTrendData.Select(x => new DataHistoryListItem()
                    {
                        Name = x.Description,
                        Id = x.Description,
                        TypeId = (int)TreeNodeType.MeterTrend,
                    }).OrderBy(a => a.Id).ToList(),
                };

                values.Add(meterTrend);
            }

            if (IsPCSFSingleWellController(asset.PocType))
            {
                var plungerLiftTrendData = PlungerLiftTrendData.GetItems(asset.NodeId, asset.PocType, _phrases);

                var plungerLiftTrend = new DataHistoryListItem()
                {
                    Id = asset.PocType == (int)DeviceType.PCS_Ferguson_Gas_Lift
                        ? _phrases[(int)LocalePhraseIDs.GasLift]
                        : _phrases[(int)LocalePhraseIDs.PlungerLift],
                    Name = asset.PocType == (int)DeviceType.PCS_Ferguson_Gas_Lift
                        ? _phrases[(int)LocalePhraseIDs.GasLift]
                        : _phrases[(int)LocalePhraseIDs.PlungerLift],
                    TypeId = (int)TreeNodeType.PlungerLiftRoot,
                    Items = plungerLiftTrendData.Select(x => new DataHistoryListItem()
                    {
                        Id = x.Description,
                        Name = x.Description,
                        TypeId = (int)TreeNodeType.PlungerLiftTrend,
                    }).OrderBy(a => a.Id).ToList(),
                };

                values.Add(plungerLiftTrend);
            }

            if (IsPCSFEnronArchiveController(asset.PocType))
            {
                var pcsfDatalogConfigurationTrendData = new Dictionary<int, PCSFDatalogConfiguration>();
                foreach (var item in dataHistoryList.PCSFDatalogConfiguration)
                {
                    pcsfDatalogConfigurationTrendData.Add(item.DatalogNumber, new PCSFDatalogConfiguration(item));
                }

                foreach (var item in pcsfDatalogConfigurationTrendData)
                {
                    AddOneDataLogToTreeView(asset, item, ref values, correlationId);
                }
            }

            var eventTrendData = GetEventTrendDataItems(dataHistoryList.EventsTrendData);

            var eventTrend = new DataHistoryListItem()
            {
                Id = _phrases[(int)LocalePhraseIDs.Events],
                Name = _phrases[(int)LocalePhraseIDs.Events],
                TypeId = (int)TreeNodeType.EventRoot,
                Items = eventTrendData.Select(x => new DataHistoryListItem()
                {
                    Id = x.Description,
                    Name = x.Description,
                    TypeId = (int)TreeNodeType.EventTrend,
                }).OrderBy(a => a.Id).ToList()
            };

            values.Add(eventTrend);

            response.Values = values;

            return response;
        }

        private IEnumerable<DataPoint> CreateDataHistoryItemsResponse(DataHistoryItemModel dataHistoryItems,
            DataHistoryTrendInput request, string type, string correlationId)
        {
            List<DataPoint> dataPoints = new List<DataPoint>();

            var asset = dataHistoryItems.NodeMasterData;
            var treeNodeTypeId = (TreeNodeType)int.Parse(type);
            var startDate = DateTime.Parse(request.StartDate);
            var endDate = DateTime.Parse(request.EndDate);
            var selectedItems = request.ItemId.Split(",");
            var itemId = selectedItems[itemIndex];

            if (treeNodeTypeId == TreeNodeType.CommonTrend)
            {
                // since the common trend is made of different TrendData, we need to figure out where it came from.

                // check measurement
                if (measurementTrendData != null)
                {
                    var measurement = GetMeasumentTrendItem(measurementTrendData, itemId, asset,
                        dataHistoryItems);

                    if (measurement != null)
                    {
                        var result = _dataHistorySQLStore.GetMeasurementTrendData(asset.NodeId,
                            (int)measurement.ParamStandardType, startDate, endDate, correlationId);
                        if (result != null)
                        {
                            var points = result
                                .Select(x => new DataPoint()
                                {
                                    X = x.Date,
                                    Y = (decimal)x.Value,
                                    TrendName = itemId
                                });
                            dataPoints.AddRange(points);
                        }
                    }
                }

                // check controller
                if (controllerTrendData != null)
                {
                    var controller = GetControllerTrendItem(controllerTrendData, itemId, asset,
                        dataHistoryItems);

                    if (controller != null)
                    {
                        var result = _dataHistorySQLStore.GetControllerTrendData(asset.NodeId, controller.Address,
                            startDate, endDate, correlationId);
                        if (result != null)
                        {
                            var points = result.Select(x => new DataPoint()
                            {
                                X = x.Date,
                                Y = (decimal)x.Value,
                                TrendName = itemId
                            });

                            dataPoints.AddRange(points);
                        }
                    }
                }

                // check production statistics
                var productionStatisticsTrendData = ProductionStatisticsTrendData.GetItems(_phrases);
                if (productionStatisticsTrendData != null)
                {
                    var productionStatistic = productionStatisticsTrendData.FirstOrDefault(x => (x.Name == "Gas Rate"
                    ? GetGasRatePhrase(x.Description, asset.ApplicationId,
                                        dataHistoryItems.GLIncludeInjGasInTest) : x.Description) == itemId);

                    if (productionStatistic != null)
                    {
                        var result = _dataHistorySQLStore.GetProductionStatisticsTrendData(asset.NodeId, startDate,
                            endDate, productionStatistic.Name, correlationId);
                        if (result != null)
                        {
                            var points = result
                                .Where(a => a.Value != null)
                                .Select(x => new DataPoint()
                                {
                                    X = x.ProcessedDate != null ? (DateTime)x.ProcessedDate : DateTime.MinValue,
                                    Y = (decimal)x.Value,
                                    TrendName = itemId
                                });

                            dataPoints.AddRange(points);
                        }
                    }
                }

                // check operational score
                var operationalScoreTrendData = OperationalScoreTrendData.GetItems(_phrases);
                if (operationalScoreTrendData != null)
                {
                    var operationScore = operationalScoreTrendData.FirstOrDefault(x => (x.Name == "Gas Rate"
                    ? GetGasRatePhrase(x.Description, asset.ApplicationId,
                                        dataHistoryItems.GLIncludeInjGasInTest) : x.Description) == itemId);

                    if (operationScore != null)
                    {
                        var result = _dataHistorySQLStore.GetOperationalScoreTrendData(operationScore.Name, asset.NodeId,
                            startDate, endDate, correlationId);
                        if (result != null)
                        {
                            var points = result
                                .Where(a => a.Value != null)
                                .Select(x => new DataPoint()
                                {
                                    X = x.Date,
                                    Y = (decimal)x.Value,
                                    TrendName = itemId
                                });

                            dataPoints.AddRange(points);
                        }
                    }
                }
            }
            else if (treeNodeTypeId == TreeNodeType.GroupDataTrend)
            {
                // since the group trend needs additional data (group parameter id) other than what is provided to the endpoint,
                // we'll need to run GetItems() and find the data.
                var groupDataTrendData = _dataHistorySQLStore.GetGroupTrendData(correlationId).FirstOrDefault(x => x.Description == itemId);
                if (groupDataTrendData != null)
                {
                    var result = _dataHistorySQLStore.GetGroupTrendData(startDate, endDate, groupDataTrendData.Id,
                        request.GroupName, correlationId);
                    if (result != null)
                    {
                        var points = result.Select(x => new DataPoint()
                        {
                            X = x.Date.Value,
                            Y = (decimal)x.Value,
                            TrendName = itemId
                        });
                        dataPoints.AddRange(points);
                    }
                }
            }
            else if (treeNodeTypeId == TreeNodeType.AnalysisTrend)
            {
                if (asset.ApplicationId == IndustryApplication.RodArtificialLift.Key)
                {
                    var analysisTrendData = _dataHistorySQLStore.GetAnalysisTrendItems(correlationId);

                    if (analysisTrendData != null)
                    {
                        var analysisTrend = analysisTrendData.FirstOrDefault(x => x == itemId);
                        if (analysisTrend != null)
                        {
                            var result = _dataHistorySQLStore.GetAnalysisTrendData(asset.NodeId,
                                startDate, endDate, analysisTrend, correlationId);

                            if (result != null && result.Any(a => a.Value != null))
                            {
                                var points = result
                                    .Where(a => a.Value != null)
                                    .Select(x => new DataPoint()
                                    {
                                        X = x.Date,
                                        Y = (decimal)x.Value,
                                        TrendName = itemId
                                    });
                                dataPoints.AddRange(points);
                            }
                        }
                    }
                }
                else if (asset.ApplicationId == IndustryApplication.ESPArtificialLift.Key)
                {
                    var espAnalysisMeasurement = EnhancedEnumBase.Parse<ESPAnalysisMeasurement>(Regex.Replace(itemId, @"\s+", ""));

                    if (espAnalysisMeasurement != null)
                    {
                        dataPoints.AddRange(GetESPAnalysisDataHistoryTrendItems(asset.NodeId, espAnalysisMeasurement.Key,
                            startDate, endDate, correlationId));
                    }
                }
                else if (asset.ApplicationId == IndustryApplication.GasArtificialLift.Key)
                {
                    var glAnalysisMeasurement = EnhancedEnumBase.Parse<GLAnalysisMeasurement>(Regex.Replace(itemId, @"\s+", ""));

                    if (glAnalysisMeasurement != null)
                    {
                        dataPoints.AddRange(GetGLAnalysisDataHistoryTrendItems(asset.NodeId,
                            glAnalysisMeasurement.Key, startDate, endDate, correlationId));
                    }
                }
            }
            else if (treeNodeTypeId == TreeNodeType.EventTrend)
            {
                var eventTrendDataItem = dataHistoryItems.EventsTrendData.FirstOrDefault(x => x.Name == itemId);
                if (eventTrendDataItem != null)
                {
                    var eventTrendData = _dataHistorySQLStore.GetEventTrendData(asset.NodeId,
                        eventTrendDataItem.EventTypeId, startDate, endDate, correlationId);
                    if (eventTrendData != null)
                    {
                        var points = eventTrendData.Select(x => new DataPoint()
                        {
                            X = x.Date,
                            Y = 1,
                            Note = IsUserCreatedEvent(x.EventTypeId) ? x.Note + " - " + x.UserId : x.Note,
                            TrendName = itemId
                        });

                        dataPoints.AddRange(points);
                    }
                }
            }
            else if (treeNodeTypeId == TreeNodeType.RodStressTrend)
            {
                var rodStressTrendDataItemsModel = _dataHistorySQLStore.GetRodStressTrendItems(asset.NodeId, correlationId);

                if (rodStressTrendDataItemsModel != null)
                {
                    var rodStressTrendDataItems = RodStressTrendData.GetItems(rodStressTrendDataItemsModel, _phrases);

                    var rodStressTrendDataItem = rodStressTrendDataItems.FirstOrDefault(x => x.Description == itemId);
                    if (rodStressTrendDataItem != null)
                    {
                        var rodStressTrendData = _dataHistorySQLStore.GetRodStressTrendData(rodStressTrendDataItem.StressColumn,
                            asset.NodeId, (int)rodStressTrendDataItem.RodNumber, rodStressTrendDataItem.Grade,
                            (float)rodStressTrendDataItem.Diameter, startDate, endDate, correlationId);

                        if (rodStressTrendData != null)
                        {
                            var points = rodStressTrendData
                                .Where(a => a.StressColumn != null)
                                .Select(x => new DataPoint()
                                {
                                    X = x.Date,
                                    Y = (decimal)x.StressColumn,
                                    TrendName = itemId
                                });

                            dataPoints.AddRange(points);
                        }// if (rodStressTrendData != null)
                    }//if (rodStressTrendDataItem != null)
                }//if (rodStressTrendDataItemsModel != null)
            }
            else if (treeNodeTypeId == TreeNodeType.WellTestTrend)
            {
                var wellTestTrendDataItems = GetWellTestTrendDataItems();

                if (wellTestTrendDataItems != null)
                {
                    var wellTestTrendDataItem = wellTestTrendDataItems.FirstOrDefault(x => x.Description == itemId || x.Name == itemId);
                    if (wellTestTrendDataItem != null)
                    {
                        var wellTestTrendData = _dataHistorySQLStore.GetWellTestTrendData(asset.NodeId, wellTestTrendDataItem.Description,
                            startDate, endDate, true, correlationId);

                        if (wellTestTrendData != null)
                        {
                            var points = wellTestTrendData
                                .Where(a => a.Value != null)
                                .Select(x => new DataPoint()
                                {
                                    X = x.TestDate,
                                    Y = (decimal)x.Value,
                                    TrendName = itemId
                                });

                            dataPoints.AddRange(points);
                        }// if (wellTestTrendData != null)
                    }//if (wellTestTrendDataItem != null)
                }//if (wellTestTrendDataItems != null)
            }
            else if (treeNodeTypeId == TreeNodeType.MeterTrend)
            {
                var meterTrendDataItems = dataHistoryItems.MeterTrendData;
                if (meterTrendDataItems != null)
                {
                    var meterTrendDataItem = meterTrendDataItems.FirstOrDefault(x => x.Name == itemId);
                    if (meterTrendDataItem != null)
                    {
                        var meterTrendData = _dataHistorySQLStore.GetMeterHistoryTrendData(asset.NodeId, startDate, endDate,
                            meterTrendDataItem.Name, correlationId);

                        if (meterTrendData != null)
                        {
                            var points = meterTrendData
                                .Where(a => a.Value != null)
                                .Select(x => new DataPoint()
                                {
                                    X = x.Date,
                                    Y = (decimal)x.Value,
                                    TrendName = itemId
                                });
                        }// if (meterTrendData != null)
                    }//if (meterTrendDataItem != null)
                }//if (meterTrendDataItems != null)
            }
            else if (treeNodeTypeId == TreeNodeType.FailureComponentTrend)
            {
                var failureComponentDataItems = dataHistoryItems.FailureComponentTrendData;
                if (failureComponentDataItems != null)
                {
                    var failureComponentDataItem = failureComponentDataItems.FirstOrDefault(x => x.Name == itemId);
                    if (failureComponentDataItem != null)
                    {
                        var failureComponentData = _dataHistorySQLStore.GetFailureComponentItems(asset.NodeId, startDate, endDate,
                            failureComponentDataItem.ComponentId, null, correlationId);

                        if (failureComponentData != null)
                        {
                            IList<DataPoint> failureDataPoints = new List<DataPoint>();
                            foreach (var failure in failureComponentData)
                            {
                                if (failure.ComponentId != null && failure.ComponentId == failureComponentDataItem.Id)
                                {
                                    failureDataPoints.Add(new DataPoint(failure.Date, 0M, failureComponentDataItem.Name, itemId));
                                }
                            }
                            dataPoints.AddRange(failureDataPoints);
                        }// if (failureComponentData != null)
                    }//if (failureComponentData != null)
                }//if (failureComponentDataItems != null)
            }
            else if (treeNodeTypeId == TreeNodeType.FailureSubcomponentTrend)
            {
                var failureSubComponentDataItems = dataHistoryItems.FailureSubComponentTrendData;
                if (failureSubComponentDataItems != null)
                {
                    var failureSubComponentDataItem = failureSubComponentDataItems.FirstOrDefault(x => x.Name == itemId);
                    if (failureSubComponentDataItem != null)
                    {
                        var failureComponentData = _dataHistorySQLStore.GetFailureComponentItems(asset.NodeId, startDate, endDate,
                            null, failureSubComponentDataItem.Id, correlationId);

                        if (failureComponentData != null)
                        {
                            IList<DataPoint> failureDataPoints = new List<DataPoint>();
                            foreach (var failure in failureComponentData)
                            {
                                if (failure.SubcomponentId != null && failure.SubcomponentId == failureSubComponentDataItem.Id)
                                {
                                    failureDataPoints.Add(new DataPoint(failure.Date, 0M, failureSubComponentDataItem.Name, itemId));
                                }
                            }
                            dataPoints.AddRange(failureDataPoints);
                        }// if (failureComponentData != null)
                    }//if (failureComponentData != null)
                }//if (failureComponentDataItems != null)
            }
            else if (treeNodeTypeId == TreeNodeType.PlungerLiftTrend)
            {
                var plungerLiftTrendDataItems = PlungerLiftTrendData.GetItems(dataHistoryItems.NodeMasterData.NodeId,
                    dataHistoryItems.NodeMasterData.PocType, _phrases);
                if (plungerLiftTrendDataItems != null)
                {
                    var plungerLiftTrendDataItem = plungerLiftTrendDataItems.FirstOrDefault(x => x.Name == Regex.Replace(itemId, @"\s+", ""));
                    if (plungerLiftTrendDataItem != null)
                    {
                        var plungerLiftTrendData = _dataHistorySQLStore.GetPlungerLiftTrendData(asset.NodeId,
                            startDate, endDate, plungerLiftTrendDataItem.Name, correlationId);

                        if (plungerLiftTrendData != null)
                        {
                            var points = plungerLiftTrendData
                                .Where(a => a.Value != null)
                                .Select(x => new DataPoint()
                                {
                                    X = x.Date,
                                    Y = (decimal)x.Value,
                                    TrendName = itemId
                                });

                            dataPoints.AddRange(points);
                        }// if (failureComponentData != null)
                    }//if (failureComponentData != null)
                }//if (failureComponentDataItems != null)
            }
            else if (treeNodeTypeId == TreeNodeType.EnronArchiveTrend)
            {
                GetDatalogandFieldNumber(itemId, out var datalogNumber, out var fieldNumber);
                if (datalogNumber != -1 && fieldNumber != -1)
                {
                    GetPCSFDatalogRecordTrendItems(asset.NodeId, datalogNumber, fieldNumber, startDate, endDate, correlationId);
                }
            }

            return dataPoints.OrderBy(x => x.TrendName).ThenBy(x => x.X).ToList();
        }

        private IList<EventTrendData> GetEventTrendDataItems(IList<EventTrendItem> eventsTrendData)
        {
            IList<EventTrendData> eventTrendDataItems = new List<EventTrendData>();
            foreach (var item in eventsTrendData)
            {
                eventTrendDataItems.Add(new EventTrendData(item.Name, item.EventTypeId));
            }
            return eventTrendDataItems;
        }

        private IList<MeterTrendData> GetMeterTrendData(IList<MeterColumnItemModel> meterTrendData, short pocType)
        {
            if ((41 <= pocType && 49 >= pocType) ||
                (541 <= pocType && 549 >= pocType))
            {
                // gas meter
                return MeterTrendData.GetItems(meterTrendData, pocType, 1, _phrases);
            }

            if ((81 <= pocType && 89 >= pocType) ||
                (581 <= pocType && 589 >= pocType))
            {
                // injection meter
                return MeterTrendData.GetItems(meterTrendData, pocType, 2, _phrases);
            }

            if ((51 <= pocType && 59 >= pocType) ||
                (551 <= pocType && 559 >= pocType))
            {
                return MeterTrendData.GetItems(meterTrendData, pocType, 3, _phrases);
            }

            return new List<MeterTrendData>();
        }

        private IEnumerable<TrendData> GetWellTestTrendDataItems()
        {
            return new List<WellTestTrendData>()
            {
                new("Duration", _phrases[(int)LocalePhraseIDs.Duration]),
                new("GasRate", _phrases[(int)LocalePhraseIDs.GasRate], UnitCategory.GasRate),
                new("OilRate", _phrases[(int)LocalePhraseIDs.OilRate], UnitCategory.FluidRate),
                new("WaterRate", _phrases[(int)LocalePhraseIDs.WaterRate], UnitCategory.FluidRate),
                new("TotalFluid", _phrases[(int)LocalePhraseIDs.TotalFluid], UnitCategory.FluidRate),
                new("FluidAbovePump", _phrases[(int)LocalePhraseIDs.FluidAbovePump], UnitCategory.LongLength)
            }.ToArray();
        }

        private OperationalScoreTrendData[] GetOperationalScoreTrendDataItems()
        {
            return new List<OperationalScoreTrendData>
                {
                    new("OperationalScore", _phrases[(int)LocalePhraseIDs.OperationalScore])
                }.ToArray();
        }

        private ProductionStatisticsTrendData[] GetProductionStatisticsTrendDataItems()
        {
            return new List<ProductionStatisticsTrendData>()
                {
                    new("DownProductionBOE", _phrases[(int)LocalePhraseIDs.DownProductionBOE]),
                    new("LatestProductionBOE", _phrases[(int)LocalePhraseIDs.LatestProductionBOE]),
                    new("PeakProductionBOE", _phrases[(int)LocalePhraseIDs.PeakProductionBOE]),
                }.OrderBy(a => a.Description).ToArray();
        }

        private ControllerTrendData[] GetControllerTrendDataItems(IList<ControllerTrendItemModel> controllerTrendData)
        {
            IList<ControllerTrendData> controllerTrendDataList = new List<ControllerTrendData>();

            if (controllerTrendData != null && controllerTrendData.Count > 0)
            {
                foreach (var item in controllerTrendData)
                {
                    controllerTrendDataList.Add(new ControllerTrendData(item));
                }
            }
            return controllerTrendDataList.OrderBy(a => a.Description).ToArray();
        }

        private MeasurementTrendData[] GetMeasurementTrendDataItems(IList<MeasurementTrendItemModel> lstMeasurementTrendData)
        {
            var measurementTrendDataList = new List<MeasurementTrendData>();
            if (lstMeasurementTrendData != null && lstMeasurementTrendData.Count > 0)
            {
                foreach (var item in lstMeasurementTrendData)
                {
                    measurementTrendDataList.Add(new MeasurementTrendData(item));
                }
            }
            return measurementTrendDataList.OrderBy(a => a.Description).ToArray();
        }

        private string GetGasRatePhrase(string gasRatePhrase, int? applicationId, string gLIncludeInjGasInTest)
        {
            if (applicationId == null || applicationId != IndustryApplication.GasArtificialLift.Key)
            {
                return gasRatePhrase;
            }

            var stringTemplate = "{0} ({1})";

            if (string.IsNullOrWhiteSpace(gLIncludeInjGasInTest) == false && int.TryParse(gLIncludeInjGasInTest, out var gasInTextInt)
                && Convert.ToBoolean(gasInTextInt))
            {
                return string.Format(stringTemplate, _phrases[(int)LocalePhraseIDs.GasRate],
                    _phrases[(int)LocalePhraseIDs.InclInj]);
            }

            return string.Format(stringTemplate, _phrases[(int)LocalePhraseIDs.GasRate],
                _phrases[(int)LocalePhraseIDs.ExclInj]);
        }

        private bool HasWellTestOrFailureTrend(int? applicationId)
        {
            if (applicationId.HasValue)
            {
                return (3 <= applicationId && 7 >= applicationId) || (15 <= applicationId && 16 >= applicationId);
            }

            return false;
        }

        private bool IsMeter(int pocType)
        {
            return (41 <= pocType && 49 >= pocType) ||
                (81 <= pocType && 89 >= pocType) ||
                (541 <= pocType && 549 >= pocType) ||
                (581 <= pocType && 589 >= pocType) ||
                (51 <= pocType && 59 >= pocType) ||
                (551 <= pocType && 559 >= pocType);
        }

        /// <summary>
        /// Determines whether given deviceType is a PCSF 4000 Single Well Controller.
        /// This includes both the Single Well Plunger Lift (POCType = 77)
        /// and the Single Well Gas Lift (POCType = 220)
        /// This device has a special "PlungerLift History" table.
        /// </summary>
        /// <param name="pocType">POCType of the device.</param>
        private bool IsPCSFSingleWellController(int pocType)
        {
            if (pocType == (int)DeviceType.PCS_Ferguson_Gas_Lift)
            {
                return true;
            }

            return pocType == (int)DeviceType.PCS_Ferguson_4000_Single_Well;
        }

        private void AddOneDataLogToTreeView(NodeMasterModel asset, KeyValuePair<int, PCSFDatalogConfiguration> item,
            ref IList<DataHistoryListItem> trendListToDisplay, string correlationId)
        {
            if (item.Value.DatalogName == string.Empty)
            {
                return;
            }

            if (item.Value.ScheduledScanEnabled == false && item.Value.OnDemandScanEnabled == false)
            {
                return;
            }

            var datalogName = $"{item.Value.DatalogName} ({item.Value.DatalogNumber})";

            var pcsfEnronArchiveTrendData = GetPCSFEnronArchiveTrendDataItems(asset, item.Value.DatalogNumber, correlationId);

            if (pcsfEnronArchiveTrendData == null || pcsfEnronArchiveTrendData.Length == 0)
            {
                return;
            }

            string text;

            if (asset.PocType == (int)DeviceType.PCS_Ferguson_GLM_Well ||
                asset.PocType == (int)DeviceType.PCS_Ferguson_GLM_Master)
            {
                text = $"PCSF GLM: {datalogName}";
            }
            else if (asset.PocType == (int)DeviceType.PCS_Ferguson_AutoLift_Well ||
                     asset.PocType == (int)DeviceType.PCS_Ferguson_AutoLift_Master)
            {
                text = $"PCSF ALM: {datalogName}";
            }
            else if (asset.PocType == (int)DeviceType.PCS_Ferguson_MWM_Well ||
                     asset.PocType == (int)DeviceType.PCS_Ferguson_MWM_Master)
            {
                text = $"PCSF MWM: {datalogName}";
            }
            else if (asset.PocType == (int)DeviceType.PCS_Ferguson_8000_Single_Well)
            {
                text = $"PCSF SWC: {datalogName}";
            }
            else
            {
                text = $"PCSF Datalog: {datalogName}";
            }

            var pcsfEnronArchiveTrend = new DataHistoryListItem()
            {
                Id = text,
                Name = text,
                TypeId = (int)TreeNodeType.EnronArchiveRoot,
                Items = pcsfEnronArchiveTrendData.Select(x => new DataHistoryListItem()
                {
                    Id = x.Description,
                    Name = x.Description,
                    TypeId = (int)TreeNodeType.EnronArchiveTrend,
                }).OrderBy(a => a.Id).ToList()
            };

            trendListToDisplay.Add(pcsfEnronArchiveTrend);
        }

        private bool IsPCSFEnronArchiveController(int pocType)
        {
            return pocType switch
            {
                (int)DeviceType.PCS_Ferguson_AutoLift_Well => true,
                (int)DeviceType.PCS_Ferguson_AutoLift_Master => true,
                (int)DeviceType.PCS_Ferguson_GLM_Master => true,
                (int)DeviceType.PCS_Ferguson_GLM_Well => true,
                (int)DeviceType.PCS_Ferguson_MWM_Well => true,
                (int)DeviceType.PCS_Ferguson_MWM_Master => true,
                (int)DeviceType.PCS_Ferguson_8000_Single_Well => true,
                _ => false
            };
        }

        private PCSFEnronArchiveTrendData[] GetPCSFEnronArchiveTrendDataItems(NodeMasterModel node, int datalogNumber, string correlationId)
        {
            var returnArray = new List<PCSFEnronArchiveTrendData>();
            if (!node.Enabled)
            {
                return returnArray.ToArray();
            }

            string controllerNodeId;
            if (IsPCSF8000SWCWell(node.PocType))
            {
                controllerNodeId = node.NodeId;
            }
            else
            {
                controllerNodeId = GetMasterNodeId(node.NodeId, node.Node, node.PocType, node.PortId.Value, correlationId);
            }

            var datalog = _dataHistorySQLStore.GetPCSFDatalogConfigurationData(controllerNodeId, datalogNumber, correlationId);

            if (datalog == null)
            {
                return returnArray.ToArray();
            }

            var wellNames = new Dictionary<int, string>();
            wellNames.Add(0, node.NodeId);

            var returnValue = _dataHistorySQLStore.FindMostRecentPCSFDatalogRecord(wellNames, datalog.DatalogNumber, correlationId);

            if (returnValue != null)
            {
                for (var fieldNumber = 2; fieldNumber <= 12; fieldNumber++)
                {
                    AddItemToReturnArray(ref returnArray, datalog, fieldNumber, node.NodeId);
                }
            }

            return returnArray.ToArray();
        }

        private string GetMasterNodeId(string nodeId, string node, int pocType, int portId, string correlationId)
        {
            if (string.IsNullOrEmpty(nodeId) || string.IsNullOrEmpty(node))
            {
                return string.Empty;
            }

            int addressType = 0;
            string tcpAddresssString = string.Empty;
            string remotePortString = string.Empty;
            string deviceAddressString = string.Empty;
            int addressOffset = 0;

#pragma warning disable CA1062 // Validate arguments of public methods
            ProcessAddress(ref node, ref addressType, ref tcpAddresssString, ref remotePortString, ref deviceAddressString,
                ref addressOffset);
#pragma warning restore CA1062 // Validate arguments of public methods

            if (pocType == (int)DeviceType.PCS_Ferguson_GLM_Well || pocType == (int)DeviceType.PCS_Ferguson_MWM_Well ||
                pocType == (int)DeviceType.PCS_Ferguson_AutoLift_Well)
            {
                return _dataHistorySQLStore.GetMasterNodeId(node, pocType, portId, correlationId);
            }

            if (pocType == (int)DeviceType.PCS_Ferguson_MWM_Master || pocType == (int)DeviceType.PCS_Ferguson_GLM_Master ||
                pocType == (int)DeviceType.PCS_Ferguson_AutoLift_Master)
            {
                return nodeId;
            }

            if (IsFisherROCSlavePOCType(pocType))
            {
                return _dataHistorySQLStore.GetMasterNodeId(node, pocType, portId, correlationId);
            }

            if (pocType == (int)DeviceType.EFM_FisherROC_Master || pocType == (int)DeviceType.EFM_Totalflow_Master)
            {
                return nodeId;
            }

            return IsTotalflowSlavePOCType(pocType) ? _dataHistorySQLStore.GetMasterNodeId(node, pocType, portId, correlationId) : string.Empty;
        }

        private static bool IsPCSF8000SWCWell(int pocType)
        {
            if (pocType == (int)DeviceType.PCS_Ferguson_8000_Single_Well)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Logic for determining if a POCType is handled by the Fisher ROC Master Controller Comm Device.
        /// </summary>
        /// <param name="pocType">Input pocType to check.</param>
        /// <returns>True if the input POCType is for a EFM Fisher ROC Slave Controller.</returns>
        public static bool IsFisherROCSlavePOCType(int pocType)
        {
            return pocType switch
            {
                (int)DeviceType.EFM_FisherROC_GasOrifice => true,
                (int)DeviceType.EFM_FisherROC_GasTurbine => true,
                (int)DeviceType.EFM_FisherROC_LiquidTurbine => true,
                _ => pocType >= (int)DeviceType.EFM_FisherROC_SlavePOCType_First &&
                                        pocType <= (int)DeviceType.EFM_FisherROC_SlavePOCType_Last,
            };
        }

        /// <summary>
        /// Logic for determining if a POCType is handled by the Totalflow Master Controller Comm Device.
        /// </summary>
        /// <param name="pocType">Input pocType to check.</param>
        /// <returns>True if the input POCType is for a EFM Totalflow Slave Controller.</returns>
        public static bool IsTotalflowSlavePOCType(int pocType)
        {
            return pocType switch
            {
                (int)DeviceType.TF_LevelMaster_Tank => true,
                (int)DeviceType.TF_GasLift_Well => true,
                (int)DeviceType.TF_Orifice_Gas_Meter => true,
                (int)DeviceType.TF_Plunger_Control_Well => true,
                (int)DeviceType.TF_VCone_Gas_Meter => true,
                (int)DeviceType.TF_Coriolis_Gas_Meter => true,
                (int)DeviceType.TF_PID_Controller => true,
                (int)DeviceType.TF_Liquid_Tube => true,
                (int)DeviceType.TF_Turbine_Gas_Meter => true,
                _ => pocType >= (int)DeviceType.TF_SlavePOCType_First & pocType <= (int)DeviceType.TF_SlavePOCType_Last,
            };
        }

        private static void AddItemToReturnArray(ref List<PCSFEnronArchiveTrendData> returnArray,
                PCSFDatalogConfigurationItemModel datalog, int fieldNumber, string nodeId)
        {
            switch (fieldNumber)
            {
                case 2:
                    if (datalog.Name2 != string.Empty && datalog.Name2 != "Well-ID")
                    {
                        var item = new PCSFEnronArchiveTrendData(nodeId,
                            datalog.Name2 + " (" + datalog.DatalogNumber + ":" + fieldNumber + ")",
                            datalog.Name2 + " (" + datalog.DatalogNumber + ":" + fieldNumber + ")", 0);
                        returnArray.Add(item);
                    }

                    break;
                case 3:
                    if (datalog.Name3 != string.Empty)
                    {
                        var item = new PCSFEnronArchiveTrendData(nodeId,
                            datalog.Name3 + " (" + datalog.DatalogNumber + ":" + fieldNumber + ")",
                            datalog.Name3 + " (" + datalog.DatalogNumber + ":" + fieldNumber + ")", 0);
                        returnArray.Add(item);
                    }

                    break;
                case 4:
                    if (datalog.Name4 != string.Empty)
                    {
                        var item = new PCSFEnronArchiveTrendData(nodeId,
                            datalog.Name4 + " (" + datalog.DatalogNumber + ":" + fieldNumber + ")",
                            datalog.Name4 + " (" + datalog.DatalogNumber + ":" + fieldNumber + ")", 0);
                        returnArray.Add(item);
                    }

                    break;
                case 5:
                    if (datalog.Name5 != string.Empty)
                    {
                        var item = new PCSFEnronArchiveTrendData(nodeId,
                            datalog.Name5 + " (" + datalog.DatalogNumber + ":" + fieldNumber + ")",
                            datalog.Name5 + " (" + datalog.DatalogNumber + ":" + fieldNumber + ")", 0);
                        returnArray.Add(item);
                    }

                    break;
                case 6:
                    if (datalog.Name6 != string.Empty)
                    {
                        var item = new PCSFEnronArchiveTrendData(nodeId,
                            datalog.Name6 + " (" + datalog.DatalogNumber + ":" + fieldNumber + ")",
                            datalog.Name6 + " (" + datalog.DatalogNumber + ":" + fieldNumber + ")", 0);
                        returnArray.Add(item);
                    }

                    break;
                case 7:
                    if (datalog.Name7 != string.Empty)
                    {
                        var item = new PCSFEnronArchiveTrendData(nodeId,
                            datalog.Name7 + " (" + datalog.DatalogNumber + ":" + fieldNumber + ")",
                            datalog.Name7 + " (" + datalog.DatalogNumber + ":" + fieldNumber + ")", 0);
                        returnArray.Add(item);
                    }

                    break;
                case 8:
                    if (datalog.Name8 != string.Empty)
                    {
                        var item = new PCSFEnronArchiveTrendData(nodeId,
                            datalog.Name8 + " (" + datalog.DatalogNumber + ":" + fieldNumber + ")",
                            datalog.Name8 + " (" + datalog.DatalogNumber + ":" + fieldNumber + ")", 0);
                        returnArray.Add(item);
                    }

                    break;
                case 9:
                    if (datalog.Name9 != string.Empty)
                    {
                        var item = new PCSFEnronArchiveTrendData(nodeId,
                            datalog.Name9 + " (" + datalog.DatalogNumber + ":" + fieldNumber + ")",
                            datalog.Name9 + " (" + datalog.DatalogNumber + ":" + fieldNumber + ")", 0);
                        returnArray.Add(item);
                    }

                    break;
                case 10:
                    if (datalog.Name10 != string.Empty)
                    {
                        var item = new PCSFEnronArchiveTrendData(nodeId,
                            datalog.Name10 + " (" + datalog.DatalogNumber + ":" + fieldNumber + ")",
                            datalog.Name10 + " (" + datalog.DatalogNumber + ":" + fieldNumber + ")", 0);
                        returnArray.Add(item);
                    }

                    break;
                case 11:
                    if (datalog.Name11 != string.Empty)
                    {
                        var item = new PCSFEnronArchiveTrendData(nodeId,
                            datalog.Name11 + " (" + datalog.DatalogNumber + ":" + fieldNumber + ")",
                            datalog.Name11 + " (" + datalog.DatalogNumber + ":" + fieldNumber + ")", 0);
                        returnArray.Add(item);
                    }

                    break;
                case 12:
                    if (datalog.Name12 != string.Empty)
                    {
                        var item = new PCSFEnronArchiveTrendData(nodeId,
                            datalog.Name12 + " (" + datalog.DatalogNumber + ":" + fieldNumber + ")",
                            datalog.Name12 + " (" + datalog.DatalogNumber + ":" + fieldNumber + ")", 0);
                        returnArray.Add(item);
                    }

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Handles parsing the Node field from NodeMaster Db data.
        /// </summary>
        /// <param name="address">This is the initial Node field but is changed by removing the offset (in/out).</param>
        /// <param name="addressType">defined below (output).</param>
        /// <param name="tcpAddr">TCPIP address string (output).</param>
        /// <param name="remotePort">TCPIP port number (output).</param>
        /// <param name="deviceAddress">Modbus address of device (output).</param>
        /// <param name="offset">modbuss address offset for this NodeID (output).</param>
        private void ProcessAddress(ref string address, ref int addressType, ref string tcpAddr, ref string remotePort,
            ref string deviceAddress, ref int offset)
        {
            deviceAddress = string.Empty;
            if (address.Contains('+'))
            {
                var bParsed = int.TryParse(address.AsSpan(address.IndexOf('+') + 1), out offset);

                if (bParsed == false)
                {
                    offset = 0;
                }

                address = address[..address.IndexOf("+")];
            }

            if (address.StartsWith("O", true, null) & address.IndexOf("|") > 0)
            {
                ref string local1 = ref address;
                ref int local2 = ref addressType;
                int num = 4;
                ref int local3 = ref num;
                string str = SetAddressTypeAndSubstring(ref local1, ref local2, ref local3);
                tcpAddr = str[..str.IndexOf("|")];
                remotePort = str[(tcpAddr.Length + 1)..];
            }
            else if (address.Contains('.'))
            {
                string str1;
                if (address.StartsWith("I", true, null))
                {
                    ref string local4 = ref address;
                    ref int local5 = ref addressType;
                    int num = 3;
                    ref int local6 = ref num;
                    str1 = SetAddressTypeAndSubstring(ref local4, ref local5, ref local6);
                }
                else if (address.StartsWith("E", true, null))
                {
                    ref string local7 = ref address;
                    ref int local8 = ref addressType;
                    int num = 6;
                    ref int local9 = ref num;
                    str1 = SetAddressTypeAndSubstring(ref local7, ref local8, ref local9);
                }
                else if (address.StartsWith("A", true, null))
                {
                    ref string local10 = ref address;
                    ref int local11 = ref addressType;
                    int num = 1;
                    ref int local12 = ref num;
                    str1 = SetAddressTypeAndSubstring(ref local10, ref local11, ref local12);
                }
                else if (address.StartsWith("S", true, null))
                {
                    ref string local13 = ref address;
                    ref int local14 = ref addressType;
                    int num = 2;
                    ref int local15 = ref num;
                    str1 = SetAddressTypeAndSubstring(ref local13, ref local14, ref local15);
                }
                else if (address.StartsWith("V", true, null))
                {
                    ref string local16 = ref address;
                    ref int local17 = ref addressType;
                    int num = 5;
                    ref int local18 = ref num;
                    str1 = SetAddressTypeAndSubstring(ref local16, ref local17, ref local18);
                }
                else if (address.StartsWith("M", true, null))
                {
                    ref string local19 = ref address;
                    ref int local20 = ref addressType;
                    int num = 8;
                    ref int local21 = ref num;
                    str1 = SetAddressTypeAndSubstring(ref local19, ref local20, ref local21);
                }
                else
                {
                    str1 = address;
                }

                tcpAddr = str1[..str1.IndexOf("|")];
                string str2 = str1[(tcpAddr.Length + 1)..];
                remotePort = str2[..str2.IndexOf("|")];
                deviceAddress = str2[(remotePort.Length + 1)..];
            }
            else if (address.StartsWith("V", true, null))
            {
                ref string local22 = ref address;
                ref int local23 = ref addressType;
                int num = 5;
                ref int local24 = ref num;
                string str3 = SetAddressTypeAndSubstring(ref local22, ref local23, ref local24);
                tcpAddr = str3[..str3.IndexOf("|")];
                string str4 = str3[(tcpAddr.Length + 1)..];
                remotePort = str4[..str4.IndexOf("|")];
                deviceAddress = str4[(remotePort.Length + 1)..];
            }
            else if (address.StartsWith("D", true, null))
            {
                ref string local25 = ref address;
                ref int local26 = ref addressType;
                int num = 7;
                ref int local27 = ref num;
                string str = SetAddressTypeAndSubstring(ref local25, ref local26, ref local27);
                tcpAddr = str[..str.IndexOf("|")];
                remotePort = str[(tcpAddr.Length + 1)..];
            }
            else
            {
                deviceAddress = address;
            }
        }

        private string SetAddressTypeAndSubstring(ref string address, ref int addressType, ref int addressTypeValue)
        {
            addressType = addressTypeValue;
            return address[1..];
        }

        private static void GetDataValueForField(KeyValuePair<DateTime, PCSFDatalogRecord> dataItem, IList<DataPoint> data, int fieldNumber)
        {
            switch (fieldNumber)
            {
                case 2:
                    data.Add(new DataPoint(dataItem.Key, ((decimal)dataItem.Value?.Value2)));
                    break;
                case 3:
                    data.Add(new DataPoint(dataItem.Key, ((decimal)dataItem.Value?.Value3)));
                    break;
                case 4:
                    data.Add(new DataPoint(dataItem.Key, ((decimal)dataItem.Value?.Value4)));
                    break;
                case 5:
                    data.Add(new DataPoint(dataItem.Key, ((decimal)dataItem.Value?.Value5)));
                    break;
                case 6:
                    data.Add(new DataPoint(dataItem.Key, ((decimal)dataItem.Value?.Value6)));
                    break;
                case 7:
                    data.Add(new DataPoint(dataItem.Key, ((decimal)dataItem.Value?.Value7)));
                    break;
                case 8:
                    data.Add(new DataPoint(dataItem.Key, ((decimal)dataItem.Value?.Value8)));
                    break;
                case 9:
                    data.Add(new DataPoint(dataItem.Key, ((decimal)dataItem.Value?.Value9)));
                    break;
                case 10:
                    data.Add(new DataPoint(dataItem.Key, ((decimal)dataItem.Value?.Value10)));
                    break;
                case 11:
                    data.Add(new DataPoint(dataItem.Key, ((decimal)dataItem.Value?.Value11)));
                    break;
                case 12:
                    data.Add(new DataPoint(dataItem.Key, ((decimal)dataItem.Value?.Value12)));
                    break;
                default:
                    break;
            }
        }

        private bool IsUserCreatedEvent(int eventTypeId)
        {
            return (eventTypeId == 1 ||
                eventTypeId == 2 ||
                eventTypeId == 6 ||
                eventTypeId == 7 ||
                eventTypeId == 21 ||
                eventTypeId == 8 ||
                eventTypeId == 11 ||
                eventTypeId == 16 ||
                eventTypeId == 18 ||
                eventTypeId == 19 ||
                eventTypeId == 26);
        }

        private void GetDatalogandFieldNumber(string itemId, out int datalogNumber, out int fieldNumber)
        {
            datalogNumber = -1;
            fieldNumber = -1;
            int startPos = itemId.IndexOf('(');
            int endPos = itemId.IndexOf(')');
            if (startPos >= 0 && endPos >= 0)
            {
                string text = itemId.Substring(startPos + 1, endPos - 1);
                if (!string.IsNullOrEmpty(text))
                {
                    var textArr = text.Trim().Split(':');
                    if (textArr?.Length > 0)
                    {
                        var dParsed = int.TryParse(textArr[0], out datalogNumber);
                        if (!dParsed)
                        {
                            datalogNumber = -1;
                        }
                        var fParsed = int.TryParse(textArr[1], out fieldNumber);
                        if (!fParsed)
                        {
                            fieldNumber = -1;
                        }
                    }
                }
            }
        }

        private float? CalculateTotalGLR(bool includeInjectedGasInWellTestgasRate, float? gasRate, float? injectedgasRate, float? grossRate)
        {
            float? totalGLR;
            if (grossRate == 0)
            {
                totalGLR = 0;
            }
            else
            {
                if (includeInjectedGasInWellTestgasRate)
                {
                    totalGLR = (gasRate / grossRate * 1000);
                }
                else
                {
                    totalGLR = ((gasRate + injectedgasRate) / grossRate * 1000);
                }
            }
            return totalGLR;
        }

        private List<DataPoint> CreateDataHistoryItemsResponseFromInflux(IList<DataPointModel> dataHistoryItems,
            string requestItemId, List<string> channelIds)
        {
            var responseData = new List<DataPoint>();
            if (dataHistoryItems != null && dataHistoryItems.Count > 0)
            {
                var selectedItems = requestItemId.Split(",");
                var itemId = selectedItems[itemIndex];
                if (dataHistoryItems.Any(a => a.ColumnValues != null))
                {
                    foreach (var channel in channelIds)
                    {
                        var data = dataHistoryItems
                            .Where(a => a.ColumnValues != null &&
                                                           a.ColumnValues[channel]?.ToString() != null)
                            .Select(x => new DataPoint()
                            {
                                X = x.Time,
                                Y = decimal.Parse(x.ColumnValues[channel]),
                                TrendName = itemId
                            }).ToList();
                        responseData.AddRange(data);
                    }
                }
                else
                {
                    responseData = dataHistoryItems
                    .Where(a => a.Value != null)
                    .Select(x => new DataPoint()
                    {
                        X = x.Time,
                        Y = decimal.Parse(x.Value.ToString()),
                        TrendName = itemId
                    }).ToList();

                }
            }

            return responseData.OrderBy(a => a.X).ToList();
        }

        private void GetAddressAndParamStandardType(string typeId, string itemId,
            out List<string> addresses, out List<string> paramStdTypes)
        {
            addresses = new List<string>();
            paramStdTypes = new List<string>();
            var treeNodeTypeId = (TreeNodeType)int.Parse(typeId);

            if (treeNodeTypeId == TreeNodeType.CommonTrend)
            {
                var selectedItems = itemId.Split(',');

                var item = selectedItems[itemIndex];

                var measurement = measurementTrendData.FirstOrDefault(x => x.Name.Equals(item.Trim(),
                   StringComparison.OrdinalIgnoreCase) || x.Description.Equals(item.Trim(),
                   StringComparison.OrdinalIgnoreCase));

                measurement ??= measurementTrendData.FirstOrDefault(x => x.Description.Contains(item.Trim(), StringComparison.OrdinalIgnoreCase) || x.Name.Contains(item.Trim(), StringComparison.OrdinalIgnoreCase));

                if (measurement != null)
                {
                    if (measurement.Address != null)
                    {
                        addresses.Add(measurement?.Address.ToString());
                    }
                    if (measurement.ParameterType != null)
                    {
                        paramStdTypes.Add(measurement?.ParameterType);
                    }
                }

                if (controllerTrendData != null)
                {
                    var controller = controllerTrendData.FirstOrDefault(x => x.Description.Contains(item.Trim(),
                    StringComparison.OrdinalIgnoreCase) || x.Name.Contains(item.Trim(),
                    StringComparison.OrdinalIgnoreCase));

                    if (controller != null)
                    {
                        addresses.Add(controller?.Address.ToString());
                    }
                }
            }
        }

        private void GetAddressAndParamStdTypeFromParameterDocument(int pocType, string typeId, string itemId,
          out List<string> channelIds, string correlationId, out int? address)
        {
            channelIds = new List<string>();
            var treeNodeTypeId = (TreeNodeType)int.Parse(typeId);
            address = null;

            if (treeNodeTypeId == TreeNodeType.CommonTrend)
            {
                var selectedItems = itemId.Split(',');

                var item = selectedItems.Length == 1 ? selectedItems[0] : selectedItems[itemIndex];

                var measurement = measurementTrendData.FirstOrDefault(x => x.Name.Equals(item.Trim(),
                   StringComparison.OrdinalIgnoreCase) || x.Description.Equals(item.Trim(),
                   StringComparison.OrdinalIgnoreCase));

                measurement ??= measurementTrendData.FirstOrDefault(x => x.Description.Contains(item.Trim(), StringComparison.OrdinalIgnoreCase) || x.Name.Contains(item.Trim(), StringComparison.OrdinalIgnoreCase));

                if (measurement != null)
                {
                    if (measurement.Address != null)
                    {
                        address = measurement.Address;

                        var parameterModels = _parameterStore.GetParameterData(pocType.ToString(),
                            (int)measurement.Address, correlationId);

                        if (parameterModels != null && parameterModels?.Count > 0)
                        {
                            channelIds.AddRange(parameterModels.Select(x => x.ChannelId).ToList());
                        }
                    }
                    if (measurement.ParameterType != null)
                    {
                        var parameterModel = _parameterStore.GetParameterByParamStdType(pocType.ToString(),
                           (int)measurement.ParamStandardType, correlationId);

                        if (parameterModel?.ChannelId != null)
                        {
                            channelIds.Add(parameterModel.ChannelId);
                        }
                    }
                    return;
                }

                if (controllerTrendData != null)
                {
                    var controller = controllerTrendData.FirstOrDefault(x => x.Description.Contains(item.Trim(),
                    StringComparison.OrdinalIgnoreCase) || x.Name.Contains(item.Trim(),
                    StringComparison.OrdinalIgnoreCase));

                    if (controller != null)
                    {
                        var parameterModels = _parameterStore.GetParameterData(pocType.ToString(),
                            (int)controller?.Address, correlationId);
                        if (parameterModels != null && parameterModels?.Count > 0)
                        {
                            channelIds.AddRange(parameterModels.Select(x => x.ChannelId).ToList());
                        }
                    }
                }
            }
        }

        private async Task<List<GraphViewTrendsData>> GetDefaultTrendDataPointAsync(List<GraphViewTrendsModel> defaultTrends,
            GraphViewSettingsModel defaultTrendSettings, bool isLegacyWell, DataHistoryTrendInput request, string correlationId)
        {
            var outputResponse = new List<GraphViewTrendsData>();

            var dataHistoryItems = _dataHistorySQLStore.GetDataHistoryTrendDataItems(request.AssetId.ToString(), correlationId);
            var asset = dataHistoryItems.NodeMasterData;

            request.StartDate = defaultTrendSettings.StartDate;
            request.EndDate = defaultTrendSettings.EndDate;

            var measurementTrendData = _dataHistorySQLStore.GetMeasurementTrendItems(asset.NodeId, correlationId);
            var controllerTrendData = _dataHistorySQLStore.GetControllerTrendItems(asset.NodeId, asset.PocType, correlationId);

            var trendChannelMapping = new Dictionary<GraphViewTrendsModel, List<string>>();

            foreach (var trend in defaultTrends)
            {
                var treeNodeType = GetTreeNodeType(trend.Source);

                if (isLegacyWell || treeNodeType != TreeNodeType.CommonTrend)
                {
                    continue;
                }

                itemIndex = 0;
                var channelIds = new List<string>();
                if (trend.Source == 11)
                {
                    var parameterModel = _parameterStore.GetParameterByParamStdType(request.POCType, trend.Address, correlationId);
                    if (parameterModel?.ChannelId != null)
                    {
                        channelIds.Add(parameterModel.ChannelId);
                    }
                }
                else
                {
                    var parameterModels = _parameterStore.GetParameterData(trend.PocType == 0 ? request.POCType.ToString() : trend.PocType.ToString(),
                        trend.Address, correlationId);
                    if (parameterModels != null && parameterModels?.Count > 0)
                    {
                        channelIds.AddRange(parameterModels.Select(x => x.ChannelId).ToList());
                    }
                }

                if (channelIds.Any())
                {
                    trendChannelMapping.Add(trend, channelIds);
                }
            }

            var allChannelIds = trendChannelMapping.Values.SelectMany(x => x).Distinct().ToList();
            if (allChannelIds.Any())
            {
                var startDate = ConvertWellTimeToUtc(correlationId, asset.Tzoffset, asset.Tzdaylight, request.StartDate).ToString("yyyy-MM-ddTHH:mm:ss");
                var endDate = ConvertWellTimeToUtc(correlationId, asset.Tzoffset, asset.Tzdaylight, request.EndDate).ToString("yyyy-MM-ddTHH:mm:ss");

                var dataHistoryInfluxStore = await _dataHistoryInfluxStore.GetDataHistoryTrendData(request.AssetId,
                    request.CustomerId, request.POCType, allChannelIds, startDate, endDate);

                foreach (var keyValuePair in trendChannelMapping)
                {
                    var trend = keyValuePair.Key;
                    var channelIds = keyValuePair.Value;
                    var trendData = dataHistoryInfluxStore.Where(x => channelIds.Contains(x.TrendName)).ToList();

                    ConvertDataPointDatesFromUtcToWellTime(trendData, asset.Tzoffset, asset.Tzdaylight, correlationId);

                    var treeNodeType = GetTreeNodeType(trend.Source);

                    var graphViewTrendsData = new GraphViewTrendsData
                    {
                        AxisLabel = trend.ColumnName,
                        AxisIndex = request.IsOverlay ? 0 : trend.Axis,
                        ItemId = (int)treeNodeType,
                        Address = trend.Address,
                    };

                    var dataPoints = CreateDataHistoryItemsResponseFromInflux(trendData, trend.ColumnName, channelIds);
                    if (dataPoints != null)
                    {
                        graphViewTrendsData.AxisValues = dataPoints.ToList();
                    }

                    outputResponse.Add(graphViewTrendsData);
                }
            }

            foreach (var trend in defaultTrends)
            {
                var treeNodeType = GetTreeNodeType(trend.Source);

                if (isLegacyWell || treeNodeType != TreeNodeType.CommonTrend)
                {
                    request.ItemId = trend.ColumnName;
                    var graphViewTrendsData = new GraphViewTrendsData
                    {
                        AxisLabel = trend.ColumnName,
                        AxisIndex = request.IsOverlay ? 0 : trend.Axis,
                        Address = trend.Address,
                    };

                    var dataPoints = CreateDataHistoryItemsResponse(dataHistoryItems, request, ((int)treeNodeType).ToString(), correlationId);
                    if (dataPoints != null)
                    {
                        graphViewTrendsData.AxisValues = dataPoints.ToList();
                        graphViewTrendsData.ItemId = (int)treeNodeType;
                    }

                    outputResponse.Add(graphViewTrendsData);
                }
            }

            return outputResponse;
        }

        private TreeNodeType GetTreeNodeType(int source)
        {
            DataHistoryTrendType dataHistoryTrendType = (DataHistoryTrendType)source;
            TreeNodeType treeNodeType = TreeNodeType.CommonTrend;
            switch (dataHistoryTrendType)
            {
                case DataHistoryTrendType.MeasurementTrend:
                case DataHistoryTrendType.ControllerTrend:
                case DataHistoryTrendType.ProductionStatisticsTrend:
                case DataHistoryTrendType.OperationalScoreTrend:
                    {
                        treeNodeType = TreeNodeType.CommonTrend;
                        break;
                    }
                case DataHistoryTrendType.EventTrend:
                    {
                        treeNodeType = TreeNodeType.EventTrend;
                        break;
                    }
                case DataHistoryTrendType.WellTestTrend:
                    {
                        treeNodeType = TreeNodeType.WellTestTrend;
                        break;
                    }
                case DataHistoryTrendType.FailureComponentTrend:
                    {
                        treeNodeType = TreeNodeType.FailureComponentTrend;
                        break;
                    }
                case DataHistoryTrendType.PlungerLiftTrend:
                    {
                        treeNodeType = TreeNodeType.PlungerLiftTrend;
                        break;
                    }
                case DataHistoryTrendType.GroupDataTrend:
                    {
                        treeNodeType = TreeNodeType.GroupDataTrend;
                        break;
                    }
                case DataHistoryTrendType.EnronArchiveTrend:
                    {
                        treeNodeType = TreeNodeType.EnronArchiveTrend;
                        break;
                    }
                case DataHistoryTrendType.AnalysisTrend:
                case DataHistoryTrendType.ALAnalysisTrend:
                case DataHistoryTrendType.GLAnalysisTrend:
                    {
                        treeNodeType = TreeNodeType.AnalysisTrend;
                        break;
                    }
                case DataHistoryTrendType.FailureSubcomponentTrend:
                    {
                        treeNodeType = TreeNodeType.FailureSubcomponentTrend;
                        break;
                    }
                case DataHistoryTrendType.MeterTrend:
                    {
                        treeNodeType = TreeNodeType.MeterTrend;
                        break;
                    }
                default:
                    break;
            }

            return treeNodeType;
        }

        private async Task<List<GraphViewTrendsData>> GetTrendDataByAxesAsync(string correlationId, DataHistoryItemModel dataHistoryItems, TrendIDataInput request, bool isLegacyWell,
            IList<string> chartTypes, IList<string> chartItemIds)
        {
            List<GraphViewTrendsData> outputResponse = new List<GraphViewTrendsData>();

            var trendChannelMapping = new Dictionary<GraphViewTrendsData, List<string>>();

            // Create channel id mappings for each chart
            int chartCount = chartTypes.Count;
            itemIndex = 0;
            for (int chartIndex = 0; chartIndex < chartCount; chartIndex++)
            {
                var selectedTypes = chartTypes[chartIndex].Split(',');
                var selectedChartItemId = chartItemIds[chartIndex].Split(',');
                TreeNodeType treeNodeTypeId;

                foreach (var type in selectedTypes)
                {
                    if (string.IsNullOrEmpty(type))
                    {
                        continue;
                    }

                    treeNodeTypeId = (TreeNodeType)int.Parse(type);

                    if (isLegacyWell || treeNodeTypeId != TreeNodeType.CommonTrend)
                    {
                        continue;
                    }

                    var graphViewTrendsData = new GraphViewTrendsData();
                    graphViewTrendsData.AxisLabel = selectedChartItemId.Length == 1 ? selectedChartItemId[0] : selectedChartItemId[itemIndex];
                    graphViewTrendsData.AxisIndex = chartIndex;
                    graphViewTrendsData.ItemId = (int)treeNodeTypeId;

                    GetAddressAndParamStdTypeFromParameterDocument(int.Parse(request.POCType), type, chartItemIds[chartIndex],
                        out var channelIds, correlationId, out var address);

                    graphViewTrendsData.Address = address;

                    trendChannelMapping.Add(graphViewTrendsData, channelIds);

                    itemIndex++;
                }
            }

            var allChannelIds = trendChannelMapping.Values.SelectMany(x => x).Distinct().ToList();

            // Get data from influx
            var startDate = ConvertWellTimeToUtc(correlationId, dataHistoryItems.NodeMasterData.Tzoffset, dataHistoryItems.NodeMasterData.Tzdaylight, request.StartDate).ToString("yyyy-MM-ddTHH:mm:ss");
            var endDate = ConvertWellTimeToUtc(correlationId, dataHistoryItems.NodeMasterData.Tzoffset, dataHistoryItems.NodeMasterData.Tzdaylight, request.EndDate).ToString("yyyy-MM-ddTHH:mm:ss");

            var dataHistoryInfluxStore = await _dataHistoryInfluxStore.GetDataHistoryTrendData(request.AssetId, request.CustomerId,
                request.POCType, allChannelIds, startDate, endDate);

            itemIndex = 0;
            foreach (var keyValuePair in trendChannelMapping)
            {
                var trend = keyValuePair.Key;
                var channelIds = keyValuePair.Value;

                if (dataHistoryInfluxStore != null)
                {
                    var trendData = dataHistoryInfluxStore.Where(x => channelIds.Contains(x.TrendName)).ToList();

                    ConvertDataPointDatesFromUtcToWellTime(trendData, dataHistoryItems.NodeMasterData.Tzoffset, dataHistoryItems.NodeMasterData.Tzdaylight, correlationId);

                    var dataPoints = CreateDataHistoryItemsResponseFromInflux(trendData, trend.AxisLabel, channelIds);
                    if (dataPoints != null)
                    {
                        trend.AxisValues = dataPoints.ToList();
                    }
                }

                outputResponse.Add(trend);
            }

            // Get data from legacy store
            for (int chartIndex = 0; chartIndex < chartCount; chartIndex++)
            {
                var selectedTypes = chartTypes[chartIndex].Split(',');
                var selectedChartItemId = chartItemIds[chartIndex].Split(',');
                TreeNodeType treeNodeTypeId;

                foreach (var type in selectedTypes)
                {
                    if (string.IsNullOrEmpty(type))
                    {
                        continue;
                    }

                    treeNodeTypeId = (TreeNodeType)int.Parse(type);

                    var graphViewTrendsData = new GraphViewTrendsData();
                    graphViewTrendsData.AxisLabel = selectedChartItemId[itemIndex];
                    graphViewTrendsData.AxisIndex = chartIndex;

                    if (isLegacyWell || treeNodeTypeId != TreeNodeType.CommonTrend)
                    {
                        var inputRequest = new DataHistoryTrendInput();
                        inputRequest.AssetId = request.AssetId;
                        inputRequest.CustomerId = request.CustomerId;
                        inputRequest.POCType = request.POCType;
                        inputRequest.StartDate = request.StartDate;
                        inputRequest.EndDate = request.EndDate;
                        inputRequest.ItemId = chartItemIds[chartIndex];
                        inputRequest.TypeId = chartTypes[chartIndex];
                        inputRequest.GroupName = request.GroupName;

                        var dataPoints = CreateDataHistoryItemsResponse(dataHistoryItems, inputRequest, ((int)treeNodeTypeId).ToString(), correlationId);

                        if (dataPoints != null)
                        {
                            graphViewTrendsData.AxisValues = dataPoints.ToList();
                            graphViewTrendsData.ItemId = (int)treeNodeTypeId;
                        }

                        outputResponse.Add(graphViewTrendsData);
                    }
                }
            }

            return outputResponse;
        }

        private async Task<List<GraphViewTrendsData>> GetTrendDataByAxisAsync(DataHistoryItemModel dataHistoryItems, TrendIDataInput request,
            string chartType, string chartItemId, bool isLegacyWell, int chartIndex, string correlationId)
        {
            List<GraphViewTrendsData> outputResponse = new List<GraphViewTrendsData>();
            var selectedTypes = chartType.Split(',');
            var selectedChartItemId = chartItemId.Split(',');
            TreeNodeType treeNodeTypeId;
            itemIndex = 0;

            foreach (var type in selectedTypes)
            {
                if (string.IsNullOrEmpty(type))
                {
                    continue;
                }

                treeNodeTypeId = (TreeNodeType)int.Parse(type);

                var graphViewTrendsData = new GraphViewTrendsData();
                graphViewTrendsData.AxisLabel = selectedChartItemId[itemIndex];
                graphViewTrendsData.AxisIndex = chartIndex;

                if (isLegacyWell || treeNodeTypeId != TreeNodeType.CommonTrend)
                {
                    var inputRequest = new DataHistoryTrendInput();
                    inputRequest.AssetId = request.AssetId;
                    inputRequest.CustomerId = request.CustomerId;
                    inputRequest.POCType = request.POCType;
                    inputRequest.StartDate = request.StartDate;
                    inputRequest.EndDate = request.EndDate;
                    inputRequest.ItemId = chartItemId;
                    inputRequest.TypeId = chartType;
                    inputRequest.GroupName = request.GroupName;

                    GetAddressAndParamStdTypeFromParameterDocument(int.Parse(request.POCType), type, chartItemId,
                        out var _, correlationId, out var address);

                    var dataPoints = CreateDataHistoryItemsResponse(dataHistoryItems, inputRequest, ((int)treeNodeTypeId).ToString(), correlationId);

                    if (dataPoints != null)
                    {
                        graphViewTrendsData.AxisValues = dataPoints.ToList();
                        graphViewTrendsData.ItemId = (int)treeNodeTypeId;
                        graphViewTrendsData.Address = address;
                    }
                }
                else
                {
                    GetAddressAndParamStdTypeFromParameterDocument(int.Parse(request.POCType), type, chartItemId,
                        out var channelIds, correlationId, out var address);
                    var startDate = ConvertWellTimeToUtc(correlationId, dataHistoryItems.NodeMasterData.Tzoffset, dataHistoryItems.NodeMasterData.Tzdaylight, request.StartDate).ToString("yyyy-MM-ddTHH:mm:ss");
                    var endDate = ConvertWellTimeToUtc(correlationId, dataHistoryItems.NodeMasterData.Tzoffset, dataHistoryItems.NodeMasterData.Tzdaylight, request.EndDate).ToString("yyyy-MM-ddTHH:mm:ss");

                    var dataHistoryInfluxStore = await _dataHistoryInfluxStore.GetDataHistoryTrendData(request.AssetId, request.CustomerId,
                                    request.POCType, channelIds, startDate, endDate);

                    if (dataHistoryInfluxStore != null)
                    {
                        var trendData = dataHistoryInfluxStore.ToList();

                        ConvertDataPointDatesFromUtcToWellTime(trendData, dataHistoryItems.NodeMasterData.Tzoffset, dataHistoryItems.NodeMasterData.Tzdaylight, correlationId);

                        var dataPoints = CreateDataHistoryItemsResponseFromInflux(dataHistoryInfluxStore, chartItemId, channelIds);
                        if (dataPoints != null)
                        {
                            graphViewTrendsData.AxisValues = dataPoints.ToList();
                            graphViewTrendsData.ItemId = (int)treeNodeTypeId;
                            graphViewTrendsData.Address = address;
                        }
                    }
                }

                itemIndex++;

                outputResponse.Add(graphViewTrendsData);
            }

            return outputResponse;
        }

        private MeasurementTrendItemModel GetMeasumentTrendItem(IList<MeasurementTrendItemModel> measurementTrendData, string itemId, NodeMasterModel asset, DataHistoryItemModel dataHistoryItems)
        {
            var measurement = measurementTrendData.FirstOrDefault((x => (x.Name == "Gas Rate"
                                        ? GetGasRatePhrase(x.Description, asset.ApplicationId,
                                        dataHistoryItems.GLIncludeInjGasInTest) : x.Description).Contains(itemId, StringComparison.OrdinalIgnoreCase) ||
                                        (x.Name == "Gas Rate" ? GetGasRatePhrase(x.Name, asset.ApplicationId,
                                        dataHistoryItems.GLIncludeInjGasInTest) : x.Name).Contains(itemId, StringComparison.OrdinalIgnoreCase)));

            return measurement;

        }

        private ControllerTrendItemModel GetControllerTrendItem(IList<ControllerTrendItemModel> controllerTrendData, string itemId, NodeMasterModel asset, DataHistoryItemModel dataHistoryItems)
        {
            var data = controllerTrendData.FirstOrDefault((x => (x.Name == "Gas Rate"
                                        ? GetGasRatePhrase(x.Description, asset.ApplicationId,
                                        dataHistoryItems.GLIncludeInjGasInTest) : x.Description).Contains(itemId, StringComparison.OrdinalIgnoreCase) ||
                                        (x.Name == "Gas Rate" ? GetGasRatePhrase(x.Name, asset.ApplicationId,
                                        dataHistoryItems.GLIncludeInjGasInTest) : x.Name).Contains(itemId, StringComparison.OrdinalIgnoreCase)));

            return data;

        }

        private DateTime ConvertWellTimeToUtc(string correlationId, float nodeTimeZoneOffset, bool honorDaylightSaving, string dateTime)
        {
            var timezoneInfo = GetTimeZoneInfo(correlationId, LoggingModel.TrendData);
            var offset = -1 * (timezoneInfo.BaseUtcOffset.TotalHours + nodeTimeZoneOffset);
            var utcDateTime = DateTime.Parse(dateTime, null).AddHours(offset);

            var daylightSaving = false;
            if (timezoneInfo.IsDaylightSavingTime(utcDateTime))
            {
                daylightSaving = true;
            }

            // If tzoffset=0, then do not apply any dst corrections - just adjust based on server
            TimeSpan daylightSavingBias = new TimeSpan(0);
            if (daylightSaving && nodeTimeZoneOffset != 0 && honorDaylightSaving)
            {
                var dt = DateTime.UtcNow;

                daylightSavingBias = timezoneInfo.BaseUtcOffset - timezoneInfo.GetUtcOffset(dt);
            }

            utcDateTime = utcDateTime.Add(daylightSavingBias);

            return utcDateTime;
        }

        private void ConvertDataPointDatesFromUtcToWellTime(List<DataPointModel> dataPoints, float nodeTimeZoneOffset, bool honorDaylightSaving, string correlationId)
        {
            foreach (var dataPoint in dataPoints)
            {
                var applicationServerTime = ConvertToApplicationServerTimeFromUTC(dataPoint.Time, correlationId, LoggingModel.TrendData);
                var wellTime = GetTimeZoneAdjustedTime(nodeTimeZoneOffset, honorDaylightSaving, applicationServerTime, correlationId, LoggingModel.TrendData);

                dataPoint.Time = wellTime;
            }
        }

        private TimeZoneInfo GetTimeZoneInfo<TLogger>(string correlationId,
            LoggingModelBase<TLogger> loggingModel) where TLogger : Enum
        {
            var logger = _loggerFactory.Create(loggingModel);

            var applicationTimeZone = _configuration.GetSection("TimeZoneBehavior:ApplicationTimeZone").Value;

            try
            {
                if (applicationTimeZone != null)
                {
                    var result = TimeZoneInfo.FindSystemTimeZoneById(applicationTimeZone);

                    logger.WriteCId(Level.Debug, $"Application time zone is set, using {result.DisplayName} time zone.",
                        correlationId);

                    return result;
                }

                logger.WriteCId(Level.Debug, "Application time zone is not set, using UTC time zone.", correlationId);

                return TimeZoneInfo.Utc;
            }
            catch (TimeZoneNotFoundException)
            {
                logger.WriteCId(Level.Error, "Time Zone Not found, using UTC time zone.", correlationId);

                return TimeZoneInfo.Utc;
            }
            catch (InvalidTimeZoneException)
            {
                logger.WriteCId(Level.Error, "Time Zone invalid, using UTC time zone.", correlationId);

                return TimeZoneInfo.Utc;
            }
            catch (Exception e)
            {
                logger.WriteCId(Level.Error, "Unknown error, using UTC time zone.", e, correlationId);

                return TimeZoneInfo.Utc;
            }
        }

        private DateTime ConvertToApplicationServerTimeFromUTC<TLogger>(DateTime utcDateTime, string correlationId,
            LoggingModelBase<TLogger> loggingModel) where TLogger : Enum
        {
            var logger = _loggerFactory.Create(loggingModel);

            var applicationTimeZone = _configuration.GetSection("TimeZoneBehavior:ApplicationTimeZone").Value;

            if (applicationTimeZone == null)
            {
                logger.WriteCId(Level.Debug, "Application time zone is not set, using UTC time zone.",
                    correlationId);

                return utcDateTime;
            }

            var timeZone = GetTimeZoneInfo(correlationId, loggingModel);
            var timeZoneTicks = timeZone.GetUtcOffset(DateTime.UtcNow).Ticks;

            var result = utcDateTime.AddTicks(timeZoneTicks);

            logger.WriteCId(Level.Debug,
                $"Adjusting from UTC: {utcDateTime} to Time Zone {timeZone.DisplayName}: {result}", correlationId);

            return result;
        }

        private DateTime GetTimeZoneAdjustedTime<TLogger>(float nodeTimeZoneOffset, bool honorDaylighSaving, DateTime scanTime,
            string correlationId, LoggingModelBase<TLogger> loggingModel) where TLogger : Enum
        {
            var logger = _loggerFactory.Create(loggingModel);

            var timezoneInfo = GetTimeZoneInfo(correlationId, loggingModel);

            // If time zone is currently in daylight saving, set daylightSaving flag to true
            bool daylightSaving = false;
            if (timezoneInfo.IsDaylightSavingTime(scanTime))
            {
                logger.WriteCId(Level.Debug, "Time Zone is in daylight savings.", correlationId);

                daylightSaving = true;
            }

            // If tzoffset=0, then do not apply any dst corrections - just adjust based on server
            TimeSpan daylightSavingBias = new TimeSpan(0);
            if (daylightSaving && nodeTimeZoneOffset != 0 && honorDaylighSaving)
            {
                var dt = DateTime.UtcNow;

                daylightSavingBias = timezoneInfo.BaseUtcOffset - timezoneInfo.GetUtcOffset(dt);

                logger.WriteCId(Level.Debug, $"Adding daylight savings bias {daylightSavingBias}.", correlationId);
            }

            int offSetMinutes = (int)(nodeTimeZoneOffset * 60);
            TimeSpan timeZoneOffset = new TimeSpan(0, offSetMinutes, 0);

            var result = scanTime + timeZoneOffset + daylightSavingBias;

            logger.WriteCId(Level.Debug,
                $"Adjusting time to original scan time {scanTime}, time zone offset {timeZoneOffset} with daylight" +
                $" saving bias {daylightSavingBias}.", correlationId);

            logger.WriteCId(Level.Debug, $"New adjusted time {result}.", correlationId);

            return result;
        }

        #endregion

    }
}