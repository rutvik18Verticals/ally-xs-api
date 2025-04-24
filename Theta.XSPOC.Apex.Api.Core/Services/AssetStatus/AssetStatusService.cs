using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Mappers;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs.AssetStatus;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Alarms;
using Theta.XSPOC.Apex.Api.Data.Asset;
using Theta.XSPOC.Apex.Api.Data.HistoricalData;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.Asset;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Data;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using Theta.XSPOC.Apex.Kernel.Quantity;
using Theta.XSPOC.Apex.Kernel.UnitConversion;
using Theta.XSPOC.Apex.Kernel.UnitConversion.Models;
using Theta.XSPOC.Apex.Kernel.Utilities;
using MathUtility = Theta.XSPOC.Apex.Api.Core.Common.MathUtility;
using UnitCategory = Theta.XSPOC.Apex.Kernel.UnitConversion.Models.UnitCategory;

namespace Theta.XSPOC.Apex.Api.Core.Services.AssetStatus
{
    /// <summary>
    /// This is the asset status service that will gather all the required data for the asset status screen.
    /// </summary>
    public class AssetStatusService : IAssetStatusService
    {

        #region Private Fields

        private readonly IAssetStore _assetRepository;
        private readonly IHistoricalStore _historicalRepository;
        private readonly IAlarmStore _alarmRepository;
        private readonly IException _exceptionRepository;
        private readonly IUserDefaultStore _userDefaultRepository;
        private readonly ISystemSettingStore _systemSettingStore;
        private readonly ILocalePhrases _phraseStore;
        private readonly IUnitConversion _unitConversion;
        private readonly IThetaLoggerFactory _loggerFactory;
        private readonly ICommonService _commonService;
        private readonly IConfiguration _configuration;
        private readonly IDateTimeConverter _dateTimeConverter;

        private IList<RodStringData> rodStrings;
        private ESPMotorInformationModel espMotorInformation;
        private ESPPumpInformationModel espPumpInformation;
        private GLAnalysisInformationModel glAnalysisInformation;
        private GasLiftInformationModel gasLiftInformation;
        private GasFlowMeterInformationModel gasFlowMeterInformation;
        private ChemicalInjectionInformationModel chemicalInjectionInformation;
        private PlungerLiftDataModel plungerLiftDataModel;

        private int SignificantDigitCount = 3;

        private const string SYSTEM_PARAMETER_KEY = "NextGen.SignificantDigits";
        private const string PROTOCOL_MODBUS = "i";
        private const string PROTOCOL_MODBUS_TCP = "m";
        private const string PROTOCOL_MODBUS_ETHERNET = "e";
        private const string PROTOCOL_MODBUS_OPC = "o";

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="AssetStatusService"/> using the provided
        /// <paramref name="assetRepository"/> and <paramref name="historicalRepository"/>. 
        /// </summary>
        /// <param name="assetRepository">The asset repository.</param>
        /// <param name="historicalRepository">The historical repository.</param>
        /// <param name="alarmRepository">The alarm repository.</param>
        /// <param name="exceptionRepository">The exception repository.</param>
        /// <param name="userDefaultRepository">The user default repository.</param>
        /// <param name="systemSettingStore">The system setting store.</param>
        /// <param name="phraseStore">The phrase store.</param>
        /// <param name="unitConversion">The unit conversion library.</param>
        /// <param name="loggerFactory">        
        /// The <seealso cref="IThetaLoggerFactory"/> service.</param>
        /// <param name="commonService">The common service.</param>
        /// <param name="configuration">The app configuration.</param>
        /// <param name="dateTimeConverter">The <seealso cref="IDateTimeConverter"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// when <paramref name="assetRepository"/> is null
        /// or
        /// when <paramref name="historicalRepository"/> is null
        /// or
        /// when <paramref name="alarmRepository"/> is null
        /// or
        /// when <paramref name="exceptionRepository"/> is null
        /// or
        /// when <paramref name="userDefaultRepository"/> is null
        /// or
        /// when <paramref name="systemSettingStore"/> is null
        /// or
        /// when <paramref name="phraseStore"/> is null
        /// or
        /// when <paramref name="unitConversion"/> is null
        /// or
        /// when <paramref name="loggerFactory"/>is null.
        /// or
        /// when <paramref name="commonService"/> is null.
        /// </exception>
        public AssetStatusService(IAssetStore assetRepository, IHistoricalStore historicalRepository,
            IAlarmStore alarmRepository, IException exceptionRepository,
            IUserDefaultStore userDefaultRepository, ISystemSettingStore systemSettingStore,
            ILocalePhrases phraseStore, IUnitConversion unitConversion, IThetaLoggerFactory loggerFactory, ICommonService commonService, IConfiguration configuration,
            IDateTimeConverter dateTimeConverter)
        {
            _assetRepository = assetRepository ?? throw new ArgumentNullException(nameof(assetRepository));
            _historicalRepository =
                historicalRepository ?? throw new ArgumentNullException(nameof(historicalRepository));
            _alarmRepository =
                alarmRepository ?? throw new ArgumentNullException(nameof(alarmRepository));
            _exceptionRepository = exceptionRepository ?? throw new ArgumentNullException(nameof(exceptionRepository));
            _userDefaultRepository =
                userDefaultRepository ?? throw new ArgumentNullException(nameof(userDefaultRepository));
            _systemSettingStore = systemSettingStore ?? throw new ArgumentNullException(nameof(systemSettingStore));
            _phraseStore = phraseStore ?? throw new ArgumentNullException(nameof(phraseStore));
            _unitConversion = unitConversion ?? throw new ArgumentNullException(nameof(unitConversion));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _commonService = commonService ?? throw new ArgumentNullException(nameof(commonService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _dateTimeConverter = dateTimeConverter ?? throw new ArgumentNullException(nameof(dateTimeConverter));
        }

        #endregion

        #region IAssetStatusService Implementation

        /// <summary>
        /// Gets the rod lift asset status data.
        /// </summary>
        /// <param name="requestWithCorrelationId">
        /// The <seealso cref="AssetStatusInput"/> that provides the input values with the correlation id.
        /// </param>
        /// <returns>
        /// The <seealso cref="RodLiftAssetStatusData"/> with the correlation id. If the asset id or the customer id
        /// is the default GUID then null is returned for the value with the correlation id.
        /// If the user id is null or empty null is returned with the correlation id.
        /// </returns>
        public async Task<WithCorrelationId<RodLiftAssetStatusDataOutput>> GetAssetStatusDataAsync(
            WithCorrelationId<AssetStatusInput> requestWithCorrelationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.GroupAndAsset);
            logger.WriteCId(Level.Trace, $"Starting {nameof(AssetStatusService)} {nameof(GetAssetStatusDataAsync)}", requestWithCorrelationId?.CorrelationId);

            if (requestWithCorrelationId == null)
            {
                var message = $"{nameof(requestWithCorrelationId)} is null, cannot get asset status.";
                logger.Write(Level.Error, message);

                return null;
            }

            var correlationId = requestWithCorrelationId.CorrelationId;

            var assetStatusInput = requestWithCorrelationId.Value;

            var userId = assetStatusInput.UserId;

            if (string.IsNullOrWhiteSpace(assetStatusInput?.UserId))
            {
                var message = $"user id is null, cannot get asset status.";
                logger.WriteCId(Level.Info, message, correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AssetStatusService)} {nameof(GetAssetStatusDataAsync)}", requestWithCorrelationId.CorrelationId);

                return new WithCorrelationId<RodLiftAssetStatusDataOutput>(correlationId, null);
            }

            var assetId = assetStatusInput.AssetId;
            var userDefault = await _userDefaultRepository.GetDefaultItemByGroup(userId, "measurements", correlationId);

            IDictionary<string, string> userDefaultStrings = new Dictionary<string, string>();

            if (userDefault.Keys.Any() == false)
            {
                userDefaultStrings = LoadDefaults();
            }
            else
            {
                foreach (var key in userDefault.Keys)
                {
                    userDefaultStrings[key] = userDefault[key]?.Value;
                }
            }

            IDictionary<int, string> phrases = _phraseStore.GetAll(requestWithCorrelationId.CorrelationId, GetPhraseIds());

            IDictionary<int, string> phraseStrings = new Dictionary<int, string>();

            foreach (var key in phrases.Keys)
            {
                phraseStrings[key] = phrases[key];
            }

            if (int.TryParse(_commonService.GetSystemParameter(SYSTEM_PARAMETER_KEY, $"{MemoryCacheValueType.SystemParameterNextGenSignificantDigits}", correlationId), out var SignificantDigitCountSystemParameter))
            {
                SignificantDigitCount = SignificantDigitCountSystemParameter;
            }

            var coreData = await _assetRepository.GetAssetStatusDataAsync(assetId);
            if (coreData == null)
            {
                var message = $"{nameof(coreData)} is null, cannot get asset status.";
                logger.WriteCId(Level.Info, message, correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AssetStatusService)} {nameof(GetAssetStatusDataAsync)}", requestWithCorrelationId.CorrelationId);

                return new WithCorrelationId<RodLiftAssetStatusDataOutput>(correlationId, null);
            }

            var customerId = coreData.CustomerGUID ?? Guid.Empty;
            var registerList = await _historicalRepository.GetAssetStatusRegisterDataAsync(assetId, customerId);
            var alarmConfigList = await GetAlarms(assetId, customerId);
            var exceptionList = await _exceptionRepository.GetAssetStatusExceptionsAsync(assetId, customerId, correlationId);

            SortByDescendingPriority(ref alarmConfigList);
            SortByDescendingPriority(ref exceptionList);

            var diagramType = GetDiagramType(coreData);

            var paramStandardData = await _historicalRepository.GetParamStandardDataAsync(assetId, customerId);
            await GetAssetStatusRegisterByApplicationAsync(coreData, paramStandardData, diagramType);

            coreData = coreData != null ? CovertUnits(coreData, phraseStrings, userDefaultStrings) : null;
            rodStrings = rodStrings == null ? null : CovertUnits(rodStrings, phraseStrings, userDefaultStrings);
            registerList = registerList == null ? null : ConvertUnits(registerList, phraseStrings, userDefaultStrings, correlationId);
            paramStandardData = paramStandardData == null ? null : ConvertUnits(paramStandardData, phraseStrings, userDefaultStrings, correlationId);
            glAnalysisInformation = glAnalysisInformation != null
                ? ConvertUnits(glAnalysisInformation, phraseStrings, userDefaultStrings) : null;

            var facilityTagData = await GetFacilityData(assetId, diagramType, correlationId);

            var PIDData = GetPIDData(diagramType, paramStandardData, coreData.NodeId, correlationId);

            var valveControlData = GetValveControlData(diagramType, coreData.NodeId, correlationId);

            var parseResult = int.TryParse(_configuration.GetSection("AppSettings:AssetStatusRefreshInterval").Value, out var refreshInterval);

            if (parseResult == false)
            {
                refreshInterval = 15;
            }

            var smartenLiveUrl = "";

            if (coreData.PocType == (int)DeviceType.RPC_Spirit_SMARTEN || coreData.PocType == (int)DeviceType.ESP_Dover_Smarten_IAM)
            {
                var isParseNodeAddressSuccessful = TryParseNodeAddress(coreData.NodeAddress, out Protocol? protocol, 
                    out string hostname, out string opcTypeName, out ushort? port, out string rtuAddress, 
                    out int? offset);

                if (string.IsNullOrEmpty(hostname))
                {
                    hostname = await _assetRepository.GetIpHostNameByPortId(coreData.PortId);
                }

                if (hostname != null)
                {
                    var nodeMasterApiPort = coreData.ApiPort ?? 8080;
                    smartenLiveUrl = $"https://{hostname}:{nodeMasterApiPort}/Login.html";
                }
            }
            //Lastgoodscan
            if (coreData.LastGoodScan != null)
            {
                DateTime lastGoodscanValue = coreData.LastGoodScan.Value;
                lastGoodscanValue = _dateTimeConverter.ConvertToApplicationServerTimeFromUTC(lastGoodscanValue, correlationId, LoggingModel.GroupAndAsset);
                coreData.LastGoodScan = _dateTimeConverter.GetTimeZoneAdjustedTime(coreData.TzOffset, coreData.TzDaylight, lastGoodscanValue,
                                                                                 correlationId,LoggingModel.GroupAndAsset);
            }

            var rodLiftAssetStatusData = new RodLiftAssetStatusData()
            {
                UserDefaults = userDefault,
                CoreData = coreData,
                RegisterData = registerList,
                AlarmData = alarmConfigList,
                ExceptionData = exceptionList,
                ParamStandardData = paramStandardData,
                RodStrings = rodStrings,
                DiagramType = (int)diagramType,
                ESPMotorInformation = espMotorInformation ?? null,
                ESPPumpInformation = espPumpInformation ?? null,
                GLAnalysisData = glAnalysisInformation ?? null,
                ChemicalInjectionInformation = chemicalInjectionInformation ?? null,
                PlungerLiftData = plungerLiftDataModel ?? null,
                FacilityTagData = facilityTagData ?? null,
                PIDDataModel = PIDData,
                ValveControlData = valveControlData,
                RefreshInterval = refreshInterval,
                SmartenLiveUrl = smartenLiveUrl,
            };

            logger.WriteCId(Level.Trace, $"Finished {nameof(AssetStatusService)} {nameof(GetAssetStatusDataAsync)}", requestWithCorrelationId.CorrelationId);

            return new WithCorrelationId<RodLiftAssetStatusDataOutput>(correlationId,
                AssetStatusMapper.Map(rodLiftAssetStatusData, SignificantDigitCount));
        }

        #endregion

        #region Private Methods

        private int[] GetPhraseIds()
        {
            return new int[]
            {
                284, // lbs
                289, // in
                555, // m3
                556, // °C
                557, // kPa
                558, // m
                559, // cm
                725, // bpd
                726, // mscfd
                1643, // ft
                1644, // psi
                1645, // F
                2968, // HP
                3137, // kW
                3140, // bbl
                4030, // MCF
                5998, // qt
                5999, // qt/d
                6497, // Mm3
                20181, // kg
                104795, // g/cm³
                104797, // API °
            };
        }

        private enum ControlModePhraseId
        {
            Manual = 20150,
            StepOpen = 4107,
            StepClosed = 4108,
            RampOpen = 4109,
            RampClosed = 4110,
            AutoDP = 4111,
            AutoFlowRate = 4112,
            AutoSP = 4113,
            AutoNominations = 4114,
            AutoDPWithShutIn = 4115,
            AutoFlowRateWithShutIn = 4116,
            AutoSPWithShutIn = 4117,
            AutoNominationsWithShutIn = 4118,
            InvalidControlMode = 4119
        }

        private int[] GetControlModePhraseIds()
        {
            return new int[]
            {
                (int)ControlModePhraseId.Manual, // Manual
                (int)ControlModePhraseId.StepOpen, // Step Open
                (int)ControlModePhraseId.StepClosed, // Step Closed
                (int)ControlModePhraseId.RampOpen, // Ramp Open
                (int)ControlModePhraseId.RampClosed, // Ramp Closed
                (int)ControlModePhraseId.AutoDP, // Auto DP
                (int)ControlModePhraseId.AutoFlowRate, // Auto Flow Rate
                (int)ControlModePhraseId.AutoSP, // Auto SP
                (int)ControlModePhraseId.AutoNominations, // Auto Nominations
                (int)ControlModePhraseId.AutoDPWithShutIn, // Auto DP with shut-in
                (int)ControlModePhraseId.AutoFlowRateWithShutIn, // Auto Flow Rate with shut-in
                (int)ControlModePhraseId.AutoSPWithShutIn, // Auto SP with shut-in
                (int)ControlModePhraseId.AutoNominationsWithShutIn, // Auto Nominations with shut-in
                (int)ControlModePhraseId.InvalidControlMode, // Invalid Control Mode
            };
        }

        private static void SortByDescendingPriority(ref IList<AlarmData> data)
        {
            if (data is not List<AlarmData> orderedData)
            {
                return;
            }

            orderedData.Sort((a, b) => a.AlarmPriority.CompareTo(b.AlarmPriority));
            orderedData.Reverse();

            data = orderedData;
        }

        private static void SortByDescendingPriority(ref IList<ExceptionData> data)
        {
            if (data is not List<ExceptionData> orderedData)
            {
                return;
            }

            orderedData.Sort((a, b) => a.Priority.CompareTo(b.Priority));
            orderedData.Reverse();

            data = orderedData;
        }

        private async Task<IList<AlarmData>> GetAlarms(Guid assetId, Guid customerId)
        {
            var alarmConfigResult = new List<AlarmData>();
            var alarmConfigList = await _alarmRepository.GetAlarmConfigurationAsync(assetId, customerId);
            var hostAlarms = await _alarmRepository.GetHostAlarmsAsync(assetId, customerId);
            var facilityTagAlarms = await _alarmRepository.GetFacilityTagAlarmsAsync(assetId, customerId);
            var cameraAlarms = await _alarmRepository.GetCameraAlarmsAsync(assetId, customerId);
            foreach (var alarmConfig in alarmConfigList)
            {
                if (int.TryParse(alarmConfig.CurrentValue.ToString(), out var currValue))
                {
                    try
                    {
                        if (BitFunctions.ExamineBit(currValue, alarmConfig.AlarmBit) !=
                            (alarmConfig.AlarmNormalState == 0))
                        {
                            continue;
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                if (string.IsNullOrWhiteSpace(alarmConfig.StateText) == false)
                {
                    alarmConfig.AlarmDescription = alarmConfig.StateText;
                }

                alarmConfigResult.Add(alarmConfig);
            }

            foreach (var hostAlarm in hostAlarms)
            {
                if (hostAlarm == null)
                {
                    continue;
                }

                var alarmStateValue = hostAlarm.AlarmState ?? 0;

                var alarmStateString = (alarmStateValue % 100) switch
                {
                    0 => "-Clear",
                    1 => "-Hi",
                    2 => "-Hi-Hi",
                    3 => "-Lo",
                    4 => "-Lo-Lo",
                    5 => "-ROC",
                    6 => "-MaxSpan",
                    7 => "-PO",
                    8 => "-VC",
                    10 => "-MinSpan",
                    _ => string.Empty
                };

                if (alarmStateString == "-Clear")
                {
                    continue;
                }

                if (alarmStateValue >= 101 && alarmStateValue < 200)
                {
                    alarmStateString += " (Acknowledged)";
                }

                hostAlarm.AlarmDescription += alarmStateString;

                alarmConfigResult.Add(hostAlarm);
            }

            alarmConfigResult.AddRange(facilityTagAlarms);
            alarmConfigResult.AddRange(cameraAlarms);

            return alarmConfigResult;
        }

        private async Task<FacilityDataModel> GetFacilityData(Guid assetId, DiagramType diagramType, string correlationId)
        {
            if (diagramType == DiagramType.Facility || diagramType == DiagramType.RegisterView)
            {
                var facilityData = await _alarmRepository.GetFacilityHeaderAndDetails(assetId);

                var measurement = new MeasurementUtility(_phraseStore);
                var abbrMeasurements = measurement.GetAbbreviatedMeasurements(correlationId);

                facilityData.TagGroups.ForEach(tagGroup =>
                {
                    tagGroup.FacilityTags.ForEach(tag =>
                    {
                        if (abbrMeasurements.TryGetValue(tag.UnitType, out var abbrMeasurement))
                        {
                            tag.EngUnits = abbrMeasurement;
                        }
                        else
                        {
                            tag.EngUnits = string.Empty;
                        }
                    });
                });

                return facilityData;
            }
            else
            {
                return null;
            }
        }

        private string GetparamStandardData(int property, IList<ParamStandardData> paramStandardData)
        {
            var data = paramStandardData.
                    Where(x => x.ParamStandardType == property)
                    .Select(x => x.Address).SingleOrDefault();
            return data == 0 ? "" : data.ToString();
        }

        private PIDDataModel GetPIDData(DiagramType diagramType, IList<ParamStandardData> paramStandardData, string nodeId, string correlationId)
        {
            if (diagramType != DiagramType.PID)
            {
                return null;
            }

            var pIDDataModel = new PIDDataModel();

            pIDDataModel.PrimaryProcessVariable = GetparamStandardData(StandardMeasurement.PrimaryProcessVariable.Key, paramStandardData);
            pIDDataModel.PrimarySetpoint = GetparamStandardData(StandardMeasurement.PrimarySetPoint.Key, paramStandardData);
            pIDDataModel.PrimaryDeadband = GetparamStandardData(StandardMeasurement.PrimaryDeadband.Key, paramStandardData);
            pIDDataModel.PrimaryProportionalGain = GetparamStandardData(StandardMeasurement.PrimaryProportionalGain.Key, paramStandardData);
            pIDDataModel.PrimaryIntegral = GetparamStandardData(StandardMeasurement.PrimaryIntegral.Key, paramStandardData);
            pIDDataModel.PrimaryDerivative = GetparamStandardData(StandardMeasurement.PrimaryDerivative.Key, paramStandardData);
            pIDDataModel.PrimaryScaleFactor = GetparamStandardData(StandardMeasurement.PrimaryScaleFactor.Key, paramStandardData);
            pIDDataModel.PrimaryOutput = GetparamStandardData(StandardMeasurement.PrimaryOutput.Key, paramStandardData);
            pIDDataModel.OverrideProcessVariable = GetparamStandardData(StandardMeasurement.OverrideProcessVariable.Key, paramStandardData);
            pIDDataModel.OverrideSetpoint = GetparamStandardData(StandardMeasurement.OverrideSetPoint.Key, paramStandardData);
            pIDDataModel.OverrideDeadband = GetparamStandardData(StandardMeasurement.OverrideDeadband.Key, paramStandardData);
            pIDDataModel.OverrideProportionalGain = GetparamStandardData(StandardMeasurement.OverrideProportionalGain.Key, paramStandardData);
            pIDDataModel.OverrideIntegral = GetparamStandardData(StandardMeasurement.OverrideIntegral.Key, paramStandardData);
            pIDDataModel.OverrideDerivative = GetparamStandardData(StandardMeasurement.OverrideDerivative.Key, paramStandardData);
            pIDDataModel.OverrideScaleFactor = GetparamStandardData(StandardMeasurement.OverrideScaleFactor.Key, paramStandardData);
            pIDDataModel.OverrideOutput = GetparamStandardData(StandardMeasurement.OverrideOutput.Key, paramStandardData);

            const int CONTROLLER_MODE_ADDRESS = 7;

            var registerList = new List<int>
            {
                CONTROLLER_MODE_ADDRESS,
            };

            var scanData = _assetRepository.GetCurrRawScanDataItems(nodeId, registerList);

            if (scanData != null)
            {
                foreach (var data in scanData)
                {
                    switch (int.Parse(data.Key))
                    {
                        case CONTROLLER_MODE_ADDRESS:
                            pIDDataModel.ControllerModeValue = GetControlMode(data.Value.ToString(), correlationId);
                            break;
                        default:
                            break;
                    } // switch
                } // foreach scanData
            } // if scanData is not null

            return pIDDataModel;
        }

        private string GetControlMode(string modeValue, string correlationId)
        {
            IDictionary<int, string> phraseStrings = _phraseStore.GetAll(correlationId, GetControlModePhraseIds());
            string mode = string.Empty;

            if (!string.IsNullOrEmpty(modeValue) && int.TryParse(modeValue, out int modeInt))
            {
                mode = (ControllerMode)modeInt switch
                {
                    ControllerMode.Manual => phraseStrings[(int)ControlModePhraseId.Manual],
                    ControllerMode.StepOpen => phraseStrings[(int)ControlModePhraseId.StepOpen],
                    ControllerMode.StepClosed => phraseStrings[(int)ControlModePhraseId.StepClosed],
                    ControllerMode.RampOpen => phraseStrings[(int)ControlModePhraseId.RampOpen],
                    ControllerMode.RampClosed => phraseStrings[(int)ControlModePhraseId.RampClosed],
                    ControllerMode.AutoDP => phraseStrings[(int)ControlModePhraseId.AutoDP],
                    ControllerMode.AutoFlowRate => phraseStrings[(int)ControlModePhraseId.AutoFlowRate],
                    ControllerMode.AutoSP => phraseStrings[(int)ControlModePhraseId.AutoSP],
                    ControllerMode.AutoNominations => phraseStrings[(int)ControlModePhraseId.AutoNominations],
                    ControllerMode.AutoDPWithShutIn => phraseStrings[(int)ControlModePhraseId.AutoDPWithShutIn],
                    ControllerMode.AutoFlowRateWithShutIn => phraseStrings[(int)ControlModePhraseId.AutoFlowRateWithShutIn],
                    ControllerMode.AutoSPWithShutIn => phraseStrings[(int)ControlModePhraseId.AutoSPWithShutIn],
                    ControllerMode.AutoNominationsWithShutIn => phraseStrings[(int)ControlModePhraseId.AutoNominationsWithShutIn],
                    ControllerMode.InvalidControlMode => phraseStrings[(int)ControlModePhraseId.InvalidControlMode],
                    _ => modeValue,
                };
            }

            return mode;
        }

        private ValveControlDataModel GetValveControlData(DiagramType diagramType, string nodeId, string correlationId)
        {
            const int CONTROLLER_MODE_ADDRESS = 7;
            const int VALUE_DP = 219;
            const int VALUE_SP = 218;
            const int VALUE_FLOW = 233;
            const int SETPOINT_DP = 210;
            const int SETPOINT_SP = 203;
            const int SETPOINT_FLOW = 214;
            const int HI_LIMIT_DP = 208;
            const int HI_LIMIT_SP = 201;
            const int HI_LIMIT_FLOW = 212;
            const int LO_LIMIT_DP = 209;
            const int LO_LIMIT_SP = 202;
            const int LO_LIMIT_FLOW = 213;
            const int DEADBAND_DP = 211;
            const int DEADBAND_SP = 204;
            const int DEADBAND_FLOW = 215;
            const int GAIN_DP = 107;
            const int GAIN_SP = 108;
            const int GAIN_FLOW = 109;
            const int SHUT_IN_LEFT = 2;
            const int SP_OVERRIDE = 206;
            const string STATION_ID = "TfReg:0.0.4";
            const string LOCATION = "TfReg:0.0.5";

            var registerList = new List<int>
            {
                CONTROLLER_MODE_ADDRESS,
                VALUE_DP,
                VALUE_SP,
                VALUE_FLOW,
                SETPOINT_DP,
                SETPOINT_SP,
                SETPOINT_FLOW,
                HI_LIMIT_DP,
                HI_LIMIT_SP,
                HI_LIMIT_FLOW,
                LO_LIMIT_DP,
                LO_LIMIT_SP,
                LO_LIMIT_FLOW,
                DEADBAND_DP,
                DEADBAND_SP,
                DEADBAND_FLOW,
                GAIN_DP,
                GAIN_SP,
                GAIN_FLOW,
                SHUT_IN_LEFT,
                SP_OVERRIDE
            };

            var scanData = _assetRepository.GetCurrRawScanDataItems(nodeId, registerList);

            var valveControlData = new ValveControlDataModel();

            if (diagramType != DiagramType.ValveControl)
            {
                return null;
            }

            if (scanData != null)
            {
                foreach (var data in scanData)
                {
                    switch (int.Parse(data.Key))
                    {
                        case CONTROLLER_MODE_ADDRESS:
                            valveControlData.ControllerModeValue = GetControlMode(data.Value.ToString(), correlationId);
                            break;
                        case VALUE_DP:
                            valveControlData.ValueDP = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case VALUE_SP:
                            valveControlData.ValueSP = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case VALUE_FLOW:
                            valveControlData.ValueFlowRate = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case SETPOINT_DP:
                            valveControlData.SetpointDP = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case SETPOINT_SP:
                            valveControlData.SetpointSP = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case SETPOINT_FLOW:
                            valveControlData.SetpointFlowRate = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case HI_LIMIT_DP:
                            valveControlData.HiLimitDP = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case HI_LIMIT_SP:
                            valveControlData.HiLimitSP = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case HI_LIMIT_FLOW:
                            valveControlData.HiLimitFlowRate = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case LO_LIMIT_DP:
                            valveControlData.LoLimitDP = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case LO_LIMIT_SP:
                            valveControlData.LoLimitSP = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case LO_LIMIT_FLOW:
                            valveControlData.LoLimitFlowRate = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case DEADBAND_DP:
                            valveControlData.DeadBandDP = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case DEADBAND_SP:
                            valveControlData.DeadBandSP = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case DEADBAND_FLOW:
                            valveControlData.DeadBandFlowRate = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case GAIN_DP:
                            valveControlData.GainDP = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case GAIN_SP:
                            valveControlData.GainSP = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case GAIN_FLOW:
                            valveControlData.GainFlowRate = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case SHUT_IN_LEFT:
                            valveControlData.ShutInLeftValue = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case SP_OVERRIDE:
                            valveControlData.SPOverrideValue = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        default:
                            break;
                    }
                }
            }

            var scanDataItemsStringValueList = new List<string>
            {
                 STATION_ID,
                 LOCATION
                };
            var scanDataStringValueList = _assetRepository.GetCurrRawScanDataItemsStringValue(nodeId, scanDataItemsStringValueList);

            if (scanDataStringValueList != null)
            {
                foreach (var data in scanDataStringValueList)
                {
                    switch (data.Key)
                    {
                        case STATION_ID:
                            valveControlData.StationId = data.Value;
                            break;
                        case LOCATION:
                            valveControlData.Location = data.Value;
                            break;
                        default:
                            break;
                    }
                }
            }

            return valveControlData;
        }

        private RodLiftAssetStatusCoreData CovertUnits(RodLiftAssetStatusCoreData coreData,
            IDictionary<int, string> phrases, IDictionary<string, string> userDefault)
        {
            var result = coreData;

            var convertedValue = result.GrossRate == null
                ? null
                : _unitConversion.Convert(((Quantity)result?.GrossRate)?.Amount, UnitCategory.FluidRate,
                phrases, userDefault);

            result.GrossRate = result.GrossRate == null
                ? null
                : _unitConversion.CreateUnitObject((int)UnitCategory.FluidRate, convertedValue.Value);

            convertedValue = result.StrokeLength == null ? null
                : _unitConversion.Convert(((Quantity)result.StrokeLength).Amount, UnitCategory.ShortLength,
                phrases, userDefault);
            result.StrokeLength = result.StrokeLength == null ? null
                : _unitConversion.CreateUnitObject((int)UnitCategory.ShortLength, convertedValue.Value);
            result.StrokeLengthUnitString = convertedValue == null ? string.Empty : convertedValue.Units;

            convertedValue = result.TubingPressure == null ? null
                : _unitConversion.Convert(((Quantity)result.TubingPressure).Amount, UnitCategory.Pressure,
                phrases, userDefault);
            result.TubingPressure = result.TubingPressure == null ? null
                : _unitConversion.CreateUnitObject((int)UnitCategory.Pressure, convertedValue.Value);
            result.TubingPressureUnitString = convertedValue == null ? string.Empty : convertedValue.Units;

            convertedValue = result.GasRate == null ? null
                : _unitConversion.Convert(((Quantity)result.GasRate)?.Amount, UnitCategory.GasRate,
                phrases, userDefault);
            result.GasRate = result.GasRate == null ? null
                : _unitConversion.CreateUnitObject((int)UnitCategory.GasRate, convertedValue.Value);

            convertedValue = result.CasingPressure == null
                ? null
                : _unitConversion.Convert(((Quantity)result.CasingPressure).Amount, UnitCategory.Pressure,
                phrases, userDefault);
            result.CasingPressure = result.CasingPressure == null
                ? null
                : _unitConversion.CreateUnitObject((int)UnitCategory.Pressure, convertedValue.Value);
            result.CasingPressureUnitString = convertedValue == null ? string.Empty : convertedValue.Units;

            convertedValue = result.FluidLevel == null ? null
                : _unitConversion.Convert(((Quantity)result.FluidLevel).Amount, UnitCategory.LongLength,
                phrases, userDefault);
            result.FluidLevel = result.FluidLevel == null
                ? null
                : _unitConversion.CreateUnitObject((int)UnitCategory.LongLength, convertedValue.Value);
            result.FluidLevelUnitString = convertedValue == null ? string.Empty : convertedValue.Units;

            convertedValue = result.PlungerDiameter == null ? null
                : _unitConversion.Convert(((Quantity)result.PlungerDiameter).Amount,
                UnitCategory.ShortLength,
                phrases, userDefault);
            result.PlungerDiameter = result.PlungerDiameter == null
                ? null
                : _unitConversion.CreateUnitObject((int)UnitCategory.ShortLength, convertedValue.Value);
            result.PlungerDiameterUnitString = convertedValue == null ? string.Empty : convertedValue.Units;

            convertedValue = result.PumpDepth == null ? null
                : _unitConversion.Convert(((Quantity)result.PumpDepth).Amount, UnitCategory.LongLength,
                phrases, userDefault);
            result.PumpDepth = result.PumpDepth == null ? null
                : _unitConversion.CreateUnitObject((int)UnitCategory.LongLength, convertedValue.Value);
            result.PumpDepthUnitString = convertedValue == null ? string.Empty : convertedValue.Units;

            return result;
        }

        private IList<RodStringData> CovertUnits(IList<RodStringData> rodStringDataList,
            IDictionary<int, string> phrases, IDictionary<string, string> userDefault)
        {
            var result = new List<RodStringData>();

            foreach (var rodStringData in rodStringDataList)
            {
                if (rodStringData.Length is not Quantity rodStringLength)
                {
                    continue;
                }

                var convertedValue = _unitConversion.Convert(rodStringLength.Amount, UnitCategory.LongLength,
                    phrases, userDefault);

                rodStringData.Length =
                    _unitConversion.CreateUnitObject((int)UnitCategory.LongLength, convertedValue.Value);
                rodStringData.UnitString = convertedValue.Units;

                result.Add(rodStringData);
            }

            return result;
        }

        private IList<ParamStandardData> ConvertUnits(IList<ParamStandardData> paramStandardList,
            IDictionary<int, string> phrases, IDictionary<string, string> userDefaults, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.GroupAndAsset);

            var result = new List<ParamStandardData>();

            int scale;
            foreach (var item in paramStandardList)
            {
                if (item.Value is Quantity quantityValue)
                {
                    scale = quantityValue.Amount.ToString().Contains('.')
                        ? quantityValue.Amount.ToString().Split(".")[1].Length
                        : 0;

                    if (scale > 15)
                    {
                        logger.WriteCId(Level.Debug,
                            $"ParamStandardType {item.ParamStandardType} Address {item.Address} Scale is greater than 15, will set to 15",
                            correlationId);

                        scale = 15;
                    }

                    var convertedValue = _unitConversion.Convert(quantityValue.Amount, (UnitCategory)item.UnitTypeId,
                        phrases, userDefaults, ConversionAction.MetricValue, scale);
                    item.Value = _unitConversion.CreateUnitObject(item.UnitTypeId, convertedValue.Value);
                    item.Units = convertedValue.Units;

                    result.Add(item);
                }
                else
                {
                    result.Add(item);
                }
            }

            return result;
        }

        private IList<RegisterData> ConvertUnits(IList<RegisterData> registerList, IDictionary<int, string> phrases,
            IDictionary<string, string> userDefaults, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.GroupAndAsset);

            var result = new List<RegisterData>();

            int scale;
            foreach (var item in registerList)
            {
                if (item.Value is Quantity quantityValue)
                {
                    scale = quantityValue.Amount.ToString().Contains('.')
                        ? quantityValue.Amount.ToString().Split(".")[1].Length
                        : 0;

                    if (scale > 15)
                    {
                        logger.WriteCId(Level.Debug,
                            $"Address {item.Address} Scale is greater than 15, will set to 15",
                            correlationId);

                        scale = 15;
                    }
                    if (item.UnitType != null)
                    {
                        var convertedValue = _unitConversion.Convert(quantityValue.Amount, (UnitCategory)item.UnitType,
                            phrases, userDefaults, ConversionAction.MetricValue, scale);
                        item.Value = _unitConversion.CreateUnitObject((int)item.UnitType, convertedValue.Value);
                        item.Units = convertedValue.Units;
                    }

                    result.Add(item);
                }
                else
                {
                    result.Add(item);
                }
            }

            return result;
        }

        private GLAnalysisInformationModel ConvertUnits(GLAnalysisInformationModel glAnalysisInformationData, IDictionary<int, string> phrases,
            IDictionary<string, string> userDefaults)
        {
            if (glAnalysisInformationData != null)
            {
                var convertedValue = glAnalysisInformationData.BottomholeTemperature == null
                ? null
                : _unitConversion.Convert(glAnalysisInformationData.BottomholeTemperature, UnitCategory.Temperature,
                phrases, userDefaults);
                glAnalysisInformationData.BottomholeTemperature = convertedValue == null ? null :
                    MathUtility.RoundToSignificantDigits(convertedValue.Value, SignificantDigitCount).ToString();

                convertedValue = glAnalysisInformationData.GasInjectionRate == null
                ? null
                : _unitConversion.Convert(glAnalysisInformationData.GasInjectionRate, UnitCategory.GasRate,
                phrases, userDefaults);
                glAnalysisInformationData.GasInjectionRate = convertedValue == null ? null :
                    MathUtility.RoundToSignificantDigits(convertedValue.Value, SignificantDigitCount).ToString();

                convertedValue = glAnalysisInformationData.ReservoirPressure == null
                ? null
                : _unitConversion.Convert(glAnalysisInformationData.ReservoirPressure, UnitCategory.Pressure,
                phrases, userDefaults);
                glAnalysisInformationData.ReservoirPressure = convertedValue == null ? null :
                    MathUtility.RoundToSignificantDigits(convertedValue.Value, SignificantDigitCount).ToString();

                convertedValue = glAnalysisInformationData.WellheadTemperature == null
                ? null
                : _unitConversion.Convert(glAnalysisInformationData.WellheadTemperature, UnitCategory.Temperature,
                phrases, userDefaults);
                glAnalysisInformationData.WellheadTemperature = convertedValue == null ? null :
                    MathUtility.RoundToSignificantDigits(convertedValue.Value, SignificantDigitCount).ToString();

                convertedValue = glAnalysisInformationData.ValveDepth == null
                ? null
                : _unitConversion.Convert(glAnalysisInformationData.ValveDepth, UnitCategory.ShortLength,
                phrases, userDefaults);
                glAnalysisInformationData.ValveDepth = convertedValue == null ? null :
                    MathUtility.RoundToSignificantDigits(convertedValue.Value, SignificantDigitCount).ToString();

                convertedValue = glAnalysisInformationData.FlowingBottomholePressure == null
                ? null
                : _unitConversion.Convert(glAnalysisInformationData.FlowingBottomholePressure, UnitCategory.Pressure,
                phrases, userDefaults);
                glAnalysisInformationData.BottomholeTemperature = convertedValue == null ? null :
                    MathUtility.RoundToSignificantDigits(convertedValue.Value, SignificantDigitCount).ToString();

                convertedValue = glAnalysisInformationData.InjectionRateForTubingCriticalVelocity == null
                ? null
                : _unitConversion.Convert(glAnalysisInformationData.InjectionRateForTubingCriticalVelocity, UnitCategory.ShortLength,
                phrases, userDefaults);
                glAnalysisInformationData.InjectionRateForTubingCriticalVelocity = convertedValue == null ? null :
                    MathUtility.RoundToSignificantDigits(convertedValue.Value, SignificantDigitCount).ToString();
            }

            return glAnalysisInformationData;
        }

        private IDictionary<string, string> LoadDefaults()
        {
            if (_systemSettingStore.TryGetSetting<string>("DefaultUnits_FluidRate", out var defaultFluidRate) == false)
            {
                defaultFluidRate = string.Empty;
            }

            if (_systemSettingStore.TryGetSetting<string>("DefaultUnits_Pressure", out var defaultPressure) == false)
            {
                defaultPressure = string.Empty;
            }

            if (_systemSettingStore.TryGetSetting<string>("DefaultUnits_Weight", out var defaultWeight) == false)
            {
                defaultWeight = string.Empty;
            }

            if (_systemSettingStore.TryGetSetting<string>("DefaultUnits_Temperature", out var defaultTemperature) ==
                false)
            {
                defaultTemperature = string.Empty;
            }

            if (_systemSettingStore.TryGetSetting<string>("DefaultUnits_Length", out var defaultLength) == false)
            {
                defaultLength = string.Empty;
            }

            if (_systemSettingStore.TryGetSetting<string>("DefaultUnits_Power", out var defaultPower) == false)
            {
                defaultPower = string.Empty;
            }

            if (_systemSettingStore.TryGetSetting<string>("DefaultUnits_GasRate", out var defaultGasRate) == false)
            {
                defaultGasRate = string.Empty;
            }

            if (_systemSettingStore.TryGetSetting<string>("DefaultUnits_ShortLength", out var defaultShortLength) ==
                false)
            {
                defaultShortLength = string.Empty;
            }

            if (_systemSettingStore.TryGetSetting<string>("DefaultUnits_OilGravity", out var defaultOilGravity) ==
                false)
            {
                defaultOilGravity = string.Empty;
            }

            return new SortedDictionary<string, string>()
            {
                {
                    "fluid rates",
                    defaultFluidRate
                },
                {
                    "pressure",
                    defaultPressure
                },
                {
                    "weight",
                    defaultWeight
                },
                {
                    "temperature",
                    defaultTemperature
                },
                {
                    "length",
                    defaultLength
                },
                {
                    "power",
                    defaultPower
                },
                {
                    "gas rate",
                    defaultGasRate
                },
                {
                    "smlength",
                    defaultShortLength
                },
                {
                    "gravity",
                    defaultOilGravity
                },
            };
        }

        private DiagramType GetDiagramType(RodLiftAssetStatusCoreData coreData)
        {
            IndustryApplication appln = EnhancedEnumBase.GetValue<IndustryApplication>(coreData.ApplicationId == null ?
                0 : (int)coreData.ApplicationId);

            DiagramType selectedDiagram;
            if (appln == IndustryApplication.RodArtificialLift)
            {
                selectedDiagram = GetRodArtificialLiftDiagram(coreData);
            }
            else if (appln == IndustryApplication.ESPArtificialLift)
            {
                selectedDiagram = DiagramType.Esp;
            }
            else if (appln == IndustryApplication.PCPArtificialLift)
            {
                selectedDiagram = DiagramType.Pcp;
            }
            else if (appln == IndustryApplication.PlungerArtificialLift)
            {
                selectedDiagram = GetPlungerArtificialLiftDiagram((short)coreData.PocType);
            }
            else if (appln == IndustryApplication.GasArtificialLift)
            {
                selectedDiagram = GetGasArtificialLiftDiagram((short)coreData.PocType);
            }
            else if (appln == IndustryApplication.GasFlowMeter)
            {
                selectedDiagram = GetGasFlowMeterDiagram((short)coreData.PocType);
            }
            else if (appln == IndustryApplication.LiquidFlowMeter)
            {
                selectedDiagram = GetLiquidFlowMeterDiagram((short)coreData.PocType);
            }
            else if (appln == IndustryApplication.Tank)
            {
                selectedDiagram = DiagramType.TotalFlowTank;
            }
            else if (appln == IndustryApplication.PID)
            {
                selectedDiagram = DiagramType.PID;
            }
            else if (appln == IndustryApplication.Valve)
            {
                selectedDiagram = DiagramType.ValveControl;
            }
            else if (appln == IndustryApplication.Injection)
            {
                selectedDiagram = DiagramType.Injection;
            }
            else if (appln == IndustryApplication.Pump)
            {
                selectedDiagram = DiagramType.RegisterView;
            }
            else if (appln == IndustryApplication.JetPumpArtificialLift)
            {
                selectedDiagram = DiagramType.JetPump;
            }
            else if (appln == IndustryApplication.PlungerAssistedGasArtificialLift)
            {
                selectedDiagram = GetPlungerAssistedGasArtificialLiftDiagram((short)coreData.PocType);
            }
            else if (appln == IndustryApplication.ChemicalInjection)
            {
                selectedDiagram = GetChemicalInjectionPumpDiagram((short)coreData.PocType);
            }
            else if (appln == IndustryApplication.Facility)
            {
                selectedDiagram = DiagramType.Facility;
            }
            else if (appln == IndustryApplication.None)
            {
                selectedDiagram = DiagramType.RegisterView;
            }
            else
            {
                selectedDiagram = DiagramType.RegisterView;
            }

            return selectedDiagram;
        }

        private DiagramType GetChemicalInjectionPumpDiagram(short pocType)
        {
            if (pocType == (short)DeviceType.Wellmark_DigiUltra)
            {
                return DiagramType.ChemicalInjectionPumpDigiUltra;
            }

            return DiagramType.ChemicalInjectionPump;
        }

        private DiagramType GetPlungerAssistedGasArtificialLiftDiagram(short pocType)
        {
            //bool _changeToPlungerLift = false;
            // If this is not an autolift controller then use the PlungerLift logic, 
            // the gaslift part is only for GasLiftAnalysis features
            // A GasLift controller would be set as that Application ID without the PlungerLift Features.
            if (pocType != (short)DeviceType.PCS_Ferguson_AutoLift_Well)
            {
                return GetPlungerArtificialLiftDiagram(pocType);
            }

            bool autoLiftWellWithGasLiftOnly = GetAutoLiftWellWithGasLiftOnly();

            if (autoLiftWellWithGasLiftOnly)
            {
                return DiagramType.GasLiftManager;
            }

            return DiagramType.GasLiftManager;
        }

        private async Task GetAssetStatusRegisterByApplicationAsync(RodLiftAssetStatusCoreData coreData, IList<ParamStandardData> paramStandardData,
            DiagramType diagramType)
        {
            IndustryApplication appln = EnhancedEnumBase.GetValue<IndustryApplication>(coreData.ApplicationId == null ?
                0 : (int)coreData.ApplicationId);
            if (appln == IndustryApplication.RodArtificialLift)
            {
                rodStrings = await _assetRepository.GetRodStringAsync(coreData.AssetGUID);
            }
            else if (appln == IndustryApplication.ESPArtificialLift)
            {
                espMotorInformation = await _assetRepository.GetESPMotorInformation(coreData.AssetGUID);
                espPumpInformation = await _assetRepository.GetESPPumpInformation(coreData.AssetGUID);
            }
            else if (appln == IndustryApplication.GasArtificialLift)
            {
                glAnalysisInformation = await _assetRepository.GetGasLiftAnalysisInformation(coreData.AssetGUID);

                GetGasFlowMeterInformation(paramStandardData);
            }
            else if (appln == IndustryApplication.ChemicalInjection)
            {
                if (diagramType != DiagramType.ChemicalInjectionPumpDigiUltra)
                {
                    chemicalInjectionInformation = await _assetRepository.GetChemicalInjectionInformation(coreData.AssetGUID);
                }
            }
            else if (appln == IndustryApplication.PlungerArtificialLift)
            {
                if (coreData.PocType == (int)DeviceType.TF_Plunger_Control_Well)
                {
                    plungerLiftDataModel = PopulateTotalFlowPlungerLiftForm(coreData.NodeId);
                }
                else if (coreData.PocType == (int)DeviceType.PCS_Ferguson_8000_Single_Well)
                {
                    plungerLiftDataModel = PopulatePCSFSWCForm(coreData.NodeId);
                }
                else
                {
                    plungerLiftDataModel = PopulatePCSFMWMForm(coreData.NodeId);
                }
            }
            else if (appln == IndustryApplication.PCPArtificialLift)
            {
                rodStrings = await _assetRepository.GetRodStringAsync(coreData.AssetGUID);
            }
        }

        private void GetGasFlowMeterInformation(IList<ParamStandardData> paramStandardData)
        {
            gasFlowMeterInformation = new GasFlowMeterInformationModel();
            gasLiftInformation = new GasLiftInformationModel();

            foreach (var item in paramStandardData)
            {
                double? roundedValue = null;

                if (item.Value != null && item.Decimals != null)
                {
                    if (item.Value is Quantity quantityValue)
                    {
                        roundedValue = Math.Round((double)quantityValue.Amount, (int)item.Decimals, MidpointRounding.AwayFromZero);
                    }
                    else if (item.Value is AnalogValue analogValue)
                    {
                        roundedValue = Math.Round((double)analogValue.Amount, (int)item.Decimals, MidpointRounding.AwayFromZero);
                    }
                }
                else if (item.Value != null && item.Decimals == null)
                {
                    if (item.Value is Quantity quantityValue)
                    {
                        roundedValue = MathUtility.RoundToSignificantDigits(quantityValue.Amount, SignificantDigitCount);
                    }
                    else if (item.Value is AnalogValue analogValue)
                    {
                        roundedValue = MathUtility.RoundToSignificantDigits(analogValue.Amount, SignificantDigitCount);
                    }
                }

                switch (item.ParamStandardType)
                {
                    case 69: // Battery Voltage
                        gasFlowMeterInformation.BatteryVoltage = roundedValue.ToString();
                        break;
                    case 70: // Differential Pressure
                        gasFlowMeterInformation.DifferentialPressure = roundedValue.ToString();
                        break;
                    case 71: // Flowing Temperature
                        gasFlowMeterInformation.FlowingTemperature = roundedValue.ToString();
                        break;
                    case 72: // Yesterday's Contract Volume
                        gasFlowMeterInformation.YesterdaysVolume = roundedValue.ToString();
                        break;
                    case 73: // Today's Contract Volume
                        gasFlowMeterInformation.TodaysVolume = roundedValue.ToString();
                        break;
                    case 129: // Today's Mass
                        gasFlowMeterInformation.TodaysMass = roundedValue.ToString();
                        break;
                    case 130: // Yesterday's Mass
                        gasFlowMeterInformation.YesterdaysMass = roundedValue.ToString();
                        break;
                    case 131: // Accumulated Mass
                        gasFlowMeterInformation.AccumulatedMass = roundedValue.ToString();
                        break;
                    case 132: // Pulse Count
                        gasFlowMeterInformation.PulseCount = roundedValue.ToString();
                        break;
                    case 137: // Charger
                        gasFlowMeterInformation.Charger = roundedValue.ToString();
                        break;
                    case 146: // Accumulated Volume
                        gasFlowMeterInformation.AccumulatedVolume = roundedValue.ToString();
                        break;
                    case 172: // Meter ID
                        gasFlowMeterInformation.MeterId = roundedValue.ToString();
                        break;
                    case 174: // Energy Rate
                        gasFlowMeterInformation.EnergyRate = roundedValue.ToString();
                        break;
                    case 175: // Today's Energy
                        gasFlowMeterInformation.TodaysEnergy = roundedValue.ToString();
                        break;
                    case 176: // Yesterday's Energy
                        gasFlowMeterInformation.YesterdaysEnergy = roundedValue.ToString();
                        break;
                    case 177: // Last Calculated Period Volume
                        gasFlowMeterInformation.LastCalculatedPeriodVolume = roundedValue.ToString();
                        break;
                    case 185: // Flowline Pressure
                        gasFlowMeterInformation.LinePressure = roundedValue.ToString();
                        break;
                    case 191: // Injection Gas Rate
                        gasLiftInformation.GasInjectionRate = roundedValue.ToString();
                        break;
                    case 197: // Yesterdays Gas Volume
                        gasLiftInformation.YesterdayGasInjectionVolume = roundedValue.ToString();
                        break;
                    case 241: // Current Mode
                        gasFlowMeterInformation.CurrentMode = roundedValue.ToString();
                        break;
                    case 242: // Countdown Hours
                        gasFlowMeterInformation.CountdownHours = roundedValue.ToString();
                        break;
                    case 243: // Countdown Seconds
                        gasFlowMeterInformation.CountdownSeconds = roundedValue.ToString();
                        break;
                    case 244: // Solar Voltage
                        gasFlowMeterInformation.SolarVoltage = roundedValue.ToString();
                        break;
                    default:
                        break;
                }
            }
        }

        private PlungerLiftDataModel PopulateTotalFlowPlungerLiftForm(string nodeId)
        {
            const int CURRENT_MODE_REGISTER = 302049;
            const int CASING_PRESSURE_REGISTER = 5005;
            const int TUBING_PRESSURE_REGISTER_TF = 5006;
            const int LINE_PRESSURE_REGISTER_TF = 5007;
            const int FLOW_RATE_REGISTER_TF = 5003;
            const int GAS_TEMPERATURE_REGISTER_TF = 5002;
            const int EFM_LIFETIME_ACCUMULATION_REGISTER = 302078;
            const int GAS_TODAY_ACCUMULATION_REGISTER = 302080;
            const int GAS_YESTERDAY_ACCUMULATION_REGISTER = 302082;
            const int OIL_TODAY_ACCUMULATION_REGISTER = 302084;
            const int OIL_YESTERDAY_ACCUMULATION_REGISTER = 302086;
            const int WATER_TODAY_ACCUMULATION_REGISTER = 302088;
            const int WATER_YESTERDAY_ACCUMULATION_REGISTER = 302090;

            var registerList = new List<int>
            {
                CURRENT_MODE_REGISTER,
                CASING_PRESSURE_REGISTER,
                TUBING_PRESSURE_REGISTER_TF,
                LINE_PRESSURE_REGISTER_TF,
                FLOW_RATE_REGISTER_TF,
                GAS_TEMPERATURE_REGISTER_TF,
                GAS_TODAY_ACCUMULATION_REGISTER,
                GAS_YESTERDAY_ACCUMULATION_REGISTER,
                OIL_TODAY_ACCUMULATION_REGISTER,
                OIL_YESTERDAY_ACCUMULATION_REGISTER,
                WATER_TODAY_ACCUMULATION_REGISTER,
                WATER_YESTERDAY_ACCUMULATION_REGISTER,
                EFM_LIFETIME_ACCUMULATION_REGISTER
            };

            var plungerLiftData = new PlungerLiftDataModel();
            var scanData = _assetRepository.GetCurrRawScanDataItems(nodeId, registerList);

            if (scanData != null)
            {
                foreach (var data in scanData)
                {
                    switch (int.Parse(data.Key))
                    {
                        case CURRENT_MODE_REGISTER:
                            plungerLiftData.CurrentMode = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case CASING_PRESSURE_REGISTER:
                            plungerLiftData.CasingPressure = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case TUBING_PRESSURE_REGISTER_TF:
                            plungerLiftData.TubingPressure = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case LINE_PRESSURE_REGISTER_TF:
                            plungerLiftData.LinePressure = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case FLOW_RATE_REGISTER_TF:
                            plungerLiftData.FlowRate = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case GAS_TEMPERATURE_REGISTER_TF:
                            plungerLiftData.GasTemperature = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case GAS_TODAY_ACCUMULATION_REGISTER:
                            plungerLiftData.EfmLifetimeAccumulation = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case GAS_YESTERDAY_ACCUMULATION_REGISTER:
                            plungerLiftData.GasTodayAccumulation = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case OIL_TODAY_ACCUMULATION_REGISTER:
                            plungerLiftData.GasYesterdayAccumulation = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case OIL_YESTERDAY_ACCUMULATION_REGISTER:
                            plungerLiftData.OilTodayAccumulation = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case WATER_TODAY_ACCUMULATION_REGISTER:
                            plungerLiftData.OilYesterdayAccumulation = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case WATER_YESTERDAY_ACCUMULATION_REGISTER:
                            plungerLiftData.WaterTodayAccumulation = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case EFM_LIFETIME_ACCUMULATION_REGISTER:
                            plungerLiftData.WaterYesterdayAccumulation = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        default:
                            break;
                    }
                }
            }

            return plungerLiftData;

        }

        private PlungerLiftDataModel PopulatePCSFSWCForm(string nodeId)
        {
            const int CURRENT_MODE_REGISTER = 300001;
            const int CASING_PRESSURE_REGISTER = 300002;
            const int TUBING_PRESSURE_REGISTER = 300003;
            const int LINE_PRESSURE_REGISTER = 300004;
            const int FLOW_RATE_REGISTER = 300005;
            const int SOLAR_VOLTAGE_REGISTER = 300006;
            const int BATTERY_VOLTAGE_REGISTER = 300007;
            const int COUNTDOWN_HOURS_REGISTER = 300008;
            const int COUNTDOWN_SECONDS_REGISTER = 300009;
            const int CALCULATED_CRITICAL_FLOW_REGISTER = 300013;
            const int SURFACE_CASING_PRESSURE_REGISTER = 300015;
            const int GAS_TEMPERATURE_REGISTER = 300016;
            const int GAS_TODAY_ACCUMULATION_REGISTER = 400098;
            const int GAS_YESTERDAY_ACCUMULATION_REGISTER = 400099;
            const int WATER_TODAY_ACCUMULATION_REGISTER = 300046;
            const int WATER_YESTERDAY_ACCUMULATION_REGISTER = 300046;
            const int CONTROLLER_FIRMWARE_VERSION_REGISTER = 400211;
            const int CONTROLLER_FIRMWARE_REVISION_REGISTER = 400212;

            List<int> registerList = new List<int>()
            {
                CURRENT_MODE_REGISTER,
                CASING_PRESSURE_REGISTER,
                TUBING_PRESSURE_REGISTER,
                LINE_PRESSURE_REGISTER,
                FLOW_RATE_REGISTER,
                COUNTDOWN_HOURS_REGISTER,
                COUNTDOWN_SECONDS_REGISTER,
                BATTERY_VOLTAGE_REGISTER,
                SOLAR_VOLTAGE_REGISTER,
                CONTROLLER_FIRMWARE_VERSION_REGISTER,
                CONTROLLER_FIRMWARE_REVISION_REGISTER,
                CALCULATED_CRITICAL_FLOW_REGISTER,
                SURFACE_CASING_PRESSURE_REGISTER,
                GAS_TEMPERATURE_REGISTER,
                GAS_TODAY_ACCUMULATION_REGISTER,
                GAS_YESTERDAY_ACCUMULATION_REGISTER,
                WATER_TODAY_ACCUMULATION_REGISTER,
                WATER_YESTERDAY_ACCUMULATION_REGISTER
            };

            var plungerLiftData = new PlungerLiftDataModel();
            var scanData = _assetRepository.GetCurrRawScanDataItems(nodeId, registerList);

            if (scanData != null)
            {
                foreach (var data in scanData)
                {
                    switch (int.Parse(data.Key))
                    {
                        case CURRENT_MODE_REGISTER:
                            plungerLiftData.CurrentMode = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case CASING_PRESSURE_REGISTER:
                            plungerLiftData.CasingPressure = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case TUBING_PRESSURE_REGISTER:
                            plungerLiftData.TubingPressure = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case LINE_PRESSURE_REGISTER:
                            plungerLiftData.LinePressure = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case FLOW_RATE_REGISTER:
                            plungerLiftData.FlowRate = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case COUNTDOWN_HOURS_REGISTER:
                            plungerLiftData.CountdownHours = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case COUNTDOWN_SECONDS_REGISTER:
                            plungerLiftData.CountdownSeconds = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case BATTERY_VOLTAGE_REGISTER:
                            plungerLiftData.BatteryVoltage = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case SOLAR_VOLTAGE_REGISTER:
                            plungerLiftData.SolarVoltage = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case CONTROLLER_FIRMWARE_VERSION_REGISTER:
                            plungerLiftData.ControllerFirmwareVersion = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case CONTROLLER_FIRMWARE_REVISION_REGISTER:
                            plungerLiftData.ControllerFirmwareRevision = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case CALCULATED_CRITICAL_FLOW_REGISTER:
                            plungerLiftData.WaterTodayAccumulation = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case SURFACE_CASING_PRESSURE_REGISTER:
                            plungerLiftData.SurfaceCasingPressure = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case GAS_TEMPERATURE_REGISTER:
                            plungerLiftData.GasTemperature = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case GAS_TODAY_ACCUMULATION_REGISTER:
                            plungerLiftData.GasTodayAccumulation = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case GAS_YESTERDAY_ACCUMULATION_REGISTER:
                            plungerLiftData.GasYesterdayAccumulation = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case WATER_TODAY_ACCUMULATION_REGISTER:
                            plungerLiftData.WaterTodayAccumulation = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            plungerLiftData.WaterYesterdayAccumulation = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        default:
                            break;
                    }
                }
            }

            return plungerLiftData;

        }

        private PlungerLiftDataModel PopulatePCSFMWMForm(string nodeId)
        {
            const int CURRENT_MODE_REGISTER = 302049;
            const int CASING_PRESSURE_REGISTER = 302050;
            const int TUBING_PRESSURE_REGISTER = 302051;
            const int LINE_PRESSURE_REGISTER = 302052;
            const int FLOW_RATE_REGISTER = 302053;
            const int SOLAR_VOLTAGE_REGISTER = 302054;
            const int BATTERY_VOLTAGE_REGISTER = 302055;
            const int COUNTDOWN_HOURS_REGISTER = 302056;
            const int COUNTDOWN_SECONDS_REGISTER = 302057;
            const int CALCULATED_CRITICAL_FLOW_REGISTER = 302064;
            const int SURFACE_CASING_PRESSURE_REGISTER = 302076;
            const int GAS_TEMPERATURE_REGISTER = 302077;
            const int EFM_LIFETIME_ACCUMULATION_REGISTER = 302078;
            const int GAS_TODAY_ACCUMULATION_REGISTER = 302080;
            const int GAS_YESTERDAY_ACCUMULATION_REGISTER = 302082;
            const int OIL_TODAY_ACCUMULATION_REGISTER = 302086;
            const int OIL_YESTERDAY_ACCUMULATION_REGISTER = 302088;
            const int WATER_TODAY_ACCUMULATION_REGISTER = 302092;
            const int WATER_YESTERDAY_ACCUMULATION_REGISTER = 302094;
            const int CONTROLLER_FIRMWARE_VERSION_REGISTER = 402051;
            const int CONTROLLER_FIRMWARE_REVISION_REGISTER = 402052;

            List<int> registerList = new List<int>()
            {
                CURRENT_MODE_REGISTER,
                CASING_PRESSURE_REGISTER,
                TUBING_PRESSURE_REGISTER,
                LINE_PRESSURE_REGISTER,
                FLOW_RATE_REGISTER,
                COUNTDOWN_HOURS_REGISTER,
                COUNTDOWN_SECONDS_REGISTER,
                BATTERY_VOLTAGE_REGISTER,
                SOLAR_VOLTAGE_REGISTER,
                CONTROLLER_FIRMWARE_VERSION_REGISTER,
                CONTROLLER_FIRMWARE_REVISION_REGISTER,
                CALCULATED_CRITICAL_FLOW_REGISTER,
                SURFACE_CASING_PRESSURE_REGISTER,
                GAS_TEMPERATURE_REGISTER,
                GAS_TODAY_ACCUMULATION_REGISTER,
                GAS_YESTERDAY_ACCUMULATION_REGISTER,
                OIL_TODAY_ACCUMULATION_REGISTER,
                OIL_YESTERDAY_ACCUMULATION_REGISTER,
                WATER_TODAY_ACCUMULATION_REGISTER,
                WATER_YESTERDAY_ACCUMULATION_REGISTER,
                EFM_LIFETIME_ACCUMULATION_REGISTER
            };

            var plungerLiftData = new PlungerLiftDataModel();
            var scanData = _assetRepository.GetCurrRawScanDataItems(nodeId, registerList);

            if (scanData != null)
            {
                foreach (var data in scanData)
                {
                    switch (int.Parse(data.Key))
                    {
                        case CURRENT_MODE_REGISTER:
                            plungerLiftData.CurrentMode = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case CASING_PRESSURE_REGISTER:
                            plungerLiftData.CasingPressure = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case TUBING_PRESSURE_REGISTER:
                            plungerLiftData.TubingPressure = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case LINE_PRESSURE_REGISTER:
                            plungerLiftData.LinePressure = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case FLOW_RATE_REGISTER:
                            plungerLiftData.FlowRate = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case COUNTDOWN_HOURS_REGISTER:
                            plungerLiftData.CountdownHours = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case COUNTDOWN_SECONDS_REGISTER:
                            plungerLiftData.CountdownSeconds = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case BATTERY_VOLTAGE_REGISTER:
                            plungerLiftData.BatteryVoltage = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case SOLAR_VOLTAGE_REGISTER:
                            plungerLiftData.SolarVoltage = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case CONTROLLER_FIRMWARE_VERSION_REGISTER:
                            plungerLiftData.ControllerFirmwareVersion = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case CONTROLLER_FIRMWARE_REVISION_REGISTER:
                            plungerLiftData.ControllerFirmwareRevision = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case CALCULATED_CRITICAL_FLOW_REGISTER:
                            plungerLiftData.WaterTodayAccumulation = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case SURFACE_CASING_PRESSURE_REGISTER:
                            plungerLiftData.SurfaceCasingPressure = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case GAS_TEMPERATURE_REGISTER:
                            plungerLiftData.GasTemperature = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case GAS_TODAY_ACCUMULATION_REGISTER:
                            plungerLiftData.GasTodayAccumulation = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case GAS_YESTERDAY_ACCUMULATION_REGISTER:
                            plungerLiftData.GasYesterdayAccumulation = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case WATER_TODAY_ACCUMULATION_REGISTER:
                            plungerLiftData.WaterTodayAccumulation = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case WATER_YESTERDAY_ACCUMULATION_REGISTER:
                            plungerLiftData.WaterYesterdayAccumulation = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case OIL_TODAY_ACCUMULATION_REGISTER:
                            plungerLiftData.OilTodayAccumulation = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case OIL_YESTERDAY_ACCUMULATION_REGISTER:
                            plungerLiftData.OilYesterdayAccumulation = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        case EFM_LIFETIME_ACCUMULATION_REGISTER:
                            plungerLiftData.EfmLifetimeAccumulation = MathUtility.RoundToSignificantDigits(data.Value, SignificantDigitCount).ToString();
                            break;
                        default:
                            break;
                    }
                }
            }

            return plungerLiftData;

        }

        private bool GetAutoLiftWellWithGasLiftOnly()
        {
            return false;
        }

        private DiagramType GetLiquidFlowMeterDiagram(short pocType)
        {
            if (pocType == (short)DeviceType.TF_Pulse_Accumulator)
            {
                return DiagramType.PulseAccumulator;
            }

            return DiagramType.LiquidFlowMeter;
        }

        private DiagramType GetGasFlowMeterDiagram(short pocType)
        {
            if (pocType == (short)DeviceType.TF_Pulse_Accumulator)
            {
                return DiagramType.PulseAccumulator;
            }

            return pocType switch
            {
                (short)DeviceType.TF_Orifice_Gas_Meter or
                (short)DeviceType.TF_Coriolis_Gas_Meter or
                (short)DeviceType.TF_Turbine_Gas_Meter or
                (short)DeviceType.TF_VCone_Gas_Meter or
                (short)DeviceType.EFM_FisherROC_GasOrifice or
                (short)DeviceType.EFM_FisherROC_GasTurbine => DiagramType.GasFlowMonitor,
                _ => DiagramType.Efm,
            };
        }

        private DiagramType GetGasArtificialLiftDiagram(short pocType)
        {
            if (pocType == (short)DeviceType.PCS_Ferguson_Gas_Lift)
            {
                return DiagramType.GasLift;
            }
            else if (pocType == (short)DeviceType.TF_GasLift_Well
                  || pocType == (short)DeviceType.PCS_Ferguson_GLM_Well
                  || pocType == (short)DeviceType.PCS_Ferguson_AutoLift_Well)
            {
                return DiagramType.GasLiftManager;
            }

            return DiagramType.GasLift;
        }

        private DiagramType GetPlungerArtificialLiftDiagram(short pocType)
        {
            if (pocType == (short)DeviceType.PCS_Ferguson_4000_Single_Well)
            {
                return DiagramType.PlungerLift;
            }
            else if (pocType == (short)DeviceType.TF_Plunger_Control_Well
                  || pocType == (short)DeviceType.PCS_Ferguson_MWM_Well
                  || pocType == (short)DeviceType.PCS_Ferguson_AutoLift_Well
                  || pocType == (short)DeviceType.PCS_Ferguson_8000_Single_Well)
            {
                return DiagramType.MultiWellPlungerLift;
            }

            return DiagramType.PlungerLift;
        }

        private DiagramType GetRodArtificialLiftDiagram(RodLiftAssetStatusCoreData coreData)
        {
            var hasPumpingUnitTypeId = int.TryParse(coreData.PumpingUnitTypeId, out int pumpingUnitTypeId);
            var manufacturerID = coreData.PumpingUnitManufacturer;

            if (hasPumpingUnitTypeId)
            {
                switch ((PumpingUnitType)pumpingUnitTypeId)
                {
                    case PumpingUnitType.Conventional:
                        return DiagramType.Conventional;
                    case PumpingUnitType.Enhanced:
                        return DiagramType.ReverseMark;
                    case PumpingUnitType.Grooves:
                        return DiagramType.Conventional;
                    case PumpingUnitType.LowProfile:
                        return DiagramType.LowProfile;
                    case PumpingUnitType.MarkII:
                        return DiagramType.Mark2;
                    case PumpingUnitType.AirBalanced:
                        return DiagramType.AirBalanced;
                    case PumpingUnitType.LongStroke:
                        return DiagramType.Rotaflex;
                    case PumpingUnitType.BeamBalanced:
                        return DiagramType.Conventional;
                    case PumpingUnitType.BeltLowProfile:
                        return DiagramType.Conventional;
                    case PumpingUnitType.Hydraulic:
                        if (manufacturerID == "HNC")
                        {
                            return DiagramType.NOVCoreLiftController;
                        }
                        else if (manufacturerID == "HSS")
                        {
                            return DiagramType.TundraSSi;
                        }
                        else
                        {
                            return DiagramType.HydraulicStrokingUnit;
                        }
                    case PumpingUnitType.CombinationCrankBeam:
                        return DiagramType.Conventional;
                    case PumpingUnitType.LinearElectric:
                        return DiagramType.UnicoLRP;
                    default:
                        break;
                }
            }

            return DiagramType.Conventional;
        }

        private bool TryParseNodeAddress(string address,
            out Protocol? protocol, out string hostname, out string opcTypeName,
            out ushort? port, out string rtuAddress, out int? offset)
        {
            const string KEY_PROTOCOL = "protocol";
            const string KEY_HOST = "host";
            const string KEY_OPC = "opc";
            const string KEY_PORT = "port";
            const string KEY_RTU_A = "rtuA";
            const string KEY_RTU_B = "rtuB";
            const string KEY_OFFSET = "offset";
            const string DELIMITERS = @"\|\+\*";
            const string FORMAT = @"(((?<{0}>[a-zA-Z])" +
                                        @"(?<{1}>[^{7}]*)" +
                                        @"(\*(?<{2}>[^{7}]*))?" +
                                        @"(\|(?<{3}>[^{7}]*))?" +
                                        @"\|(?<{4}>[^{7}]*)?)" +
                                    @"|(?<{5}>[^{7}]*))" +
                                    @"(\+(?<{6}>[^{7}]*))?";

            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            //set defaults
            protocol = null;
            hostname = null;
            opcTypeName = null;
            port = null;
            rtuAddress = null;
            offset = null;

            string pattern = String.Format(FORMAT,
                                           KEY_PROTOCOL,
                                           KEY_HOST,
                                           KEY_OPC,
                                           KEY_PORT,
                                           KEY_RTU_A,
                                           KEY_RTU_B,
                                           KEY_OFFSET,
                                           DELIMITERS);
            Match match = Regex.Match(address, pattern);
            bool success = match.Success;

            if (success)
            {
                Group group = match.Groups[KEY_RTU_B];
                if (group.Success)
                {
                    rtuAddress = group.Value;
                }
                else
                {
                    group = match.Groups[KEY_PROTOCOL];
                    if (group.Success)
                    {
                        switch (group.Value)
                        {
                            case PROTOCOL_MODBUS:
                                protocol = Protocol.Modbus;
                                break;
                            case PROTOCOL_MODBUS_TCP:
                                protocol = Protocol.ModbusTCP;
                                break;
                            case PROTOCOL_MODBUS_ETHERNET:
                                protocol = Protocol.ModbusEthernet;
                                break;
                            case PROTOCOL_MODBUS_OPC:
                                protocol = Protocol.OPC;
                                break;
                            default:
                                success = false;
                                break;
                        }
                    }

                    group = match.Groups[KEY_HOST];
                    if (group.Success)
                    {
                        hostname = group.Value;
                    }

                    group = match.Groups[KEY_OPC];
                    if (group.Success)
                    {
                        opcTypeName = group.Value;
                    }

                    group = match.Groups[KEY_PORT];
                    if (group.Success)
                    {
                        if (ushort.TryParse(group.Value, out ushort temp))
                        {
                            port = temp;
                        }
                        else
                        {
                            success = false;
                        }
                    }

                    group = match.Groups[KEY_RTU_A];
                    if (group.Success)
                    {
                        rtuAddress = group.Value;
                    }
                }

                group = match.Groups[KEY_OFFSET];
                if (group.Success)
                {
                    if (int.TryParse(group.Value, out int temp))
                    {
                        offset = temp;
                    }
                    else
                    {
                        success = false;
                    }
                }
            }

            return success;
        }

        #endregion

    }
}