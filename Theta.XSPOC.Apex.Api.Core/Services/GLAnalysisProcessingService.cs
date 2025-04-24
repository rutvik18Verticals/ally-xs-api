using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Theta.XSPOC.Apex.Api.Common.Converters;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Mappers;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using Theta.XSPOC.Apex.Kernel.Quantity;
using Theta.XSPOC.Apex.Kernel.Quantity.Measures;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Implementation of IGLAnalysisProcessingService.
    /// </summary>
    public class GLAnalysisProcessingService : IGLAnalysisProcessingService
    {

        #region Private Dependencies

        private readonly IGLAnalysis _glAnalysisService;
        private readonly IAnalysisCurve _analysisCurves;
        private readonly IAnalysisCurveSets _analysisCurvesSet;
        private readonly IThetaLoggerFactory _loggerFactory;
        private readonly ILocalePhrases _localePhrases;
        private readonly IWellAnalysisCorrelation _correlationService;
        private static IManufacturer _manufacturerService;
        private readonly IGLAnalysisGetCurveCoordinate _gLAnalysisGetCurveCoordinate;
        private readonly ICommonService _commonService;

        private const string NULL_TEXT = "-";
        private const double FLOWRATE_AXIS_MAX_MULTIPLIER = 1.2;

        private IDictionary<int, string> phrases;

        private enum PhraseIDs
        {

            TestDate = 102, // Test Date
            TubingPressure = 264, // Tubing Pressure
            CasingPressure = 265, // Casing Pressure
            WaterSpecificGravity = 267, // Water Specific Gravity
            OilRate = 532, // Oil Rate
            Injection = 1134, // Injection
            WaterRate = 1250, // Water Rate
            GasRate = 1251, // Gas Rate
            Surface = 3115, // Surface
            FlowingBottomholePressure = 4088, // Flowing Bottomhole Pressure
            Theselectedwelltestwasprocessedsuccessfully = 4816, // The selected well test was processed successfully.
            BottomholeTemperature = 4831, // Bottomhole Temperature
            Theselectedwelltesthasnotbeenprocessed = 5560, // The selected well test has not been processed.
            SpecificGravityofOil = 5584, // Specific Gravity of Oil
            ProductionRate = 5613, // Production Rate
            TubingInnerDiameter = 5646, // Tubing Inner Diameter
            InjectionRate = 5758, // Injection Rate
            FormationGasOilRatio = 5765, // Formation Gas Oil Ratio
            WellheadTemperature = 5766, // Wellhead Temperature
            ReservoirPressure = 5767, // Reservoir Pressure
            TestFlowCorrelationDate = 5769, // Flow Correlation
            MaximumProductionRate = 5771, // Maximum Production Rate
            InjectionRateForMaxProductionRate = 5773, // Injection Rate For Max Production Rate
            OptimalProductionRate = 5774, // Optimal Production Rate
            InjectionRateForOptimalProdRate = 5776, // Injection Rate For Optimal Prod. Rate
            Orifice = 5957, // Orifice
            Injecting = 5991, // Injecting
            NotInjecting = 5992, // Not Injecting
            TesFBHPatInjectionDepthtDate = 6078, // FBHP at Injection Depth
            InjectionRateforCriticalFlowrate = 6099, // Injection Rate for Critical Flowrate:
            TubingCriticalVelocityCorrelation = 6103, // Tubing Critical Velocity Correlation
            PinjForMaximumProductionRate = 6444, // P@inj For Maximum Production Rate
            PinjForOptimalProductionRate = 6445, // P@inj For Optimal Production Rate
            Lastran = 6467, // last ran
            Inclinj = 6683, // incl inj
            Exclinj = 6684, // excl inj
            CalculatedMeasuredInjectionDepth = 6845, // Calculated Measured Injection Depth
            CalculatedVerticalInjectionDepth = 6846, // Calculated Vertical Injection Depth
            ProducedGasSpecificGravity = 7215, // Produced Gas Specific Gravity
            InjectedGasSpecificGravity = 7216, // Injected Gas Specific Gravity
            Watercut = 100199, // Water cut
            VerticalWellDepth = 5798, // Vertical Well Depth
            VerticalInjectionDepth = 5760, // Vertical Injection Depth
            WellboreDiagramMissingRequirements = 6002, // is missing from analysis result.
                                                       // Wellbore diagram cannot be shown.
            WellboreDiagramMismatchedValues = 6001, // Please check the well configuration.
                                                    // Well depth / mid-perf depth, injection depth and the
                                                    // configurated valve or orifice depths do not match.
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="GLAnalysisProcessingService"/>.
        /// </summary>
        /// <param name="glAnalysisService">The <seealso cref="IGLAnalysis"/> service.</param>
        /// <param name="loggerFactory">The <seealso cref="IThetaLoggerFactory"/> service.</param>
        /// <param name="localePhrases">The <seealso cref="ILocalePhrases"/> service.</param>
        /// <param name="wellAnalysisCorrelation">The <seealso cref="IWellAnalysisCorrelation"/> service.</param>
        /// <param name="manufacturer">The <seealso cref="IManufacturer"/> service.</param>
        /// <param name="analysisCurves">The <seealso cref="IAnalysisCurve"/> service.</param>
        /// <param name="analysisCurvesSet">The <seealso cref="IAnalysisCurveSets"/> service.</param>
        /// <param name="gLAnalysisGetCurveCoordinate">The <seealso cref="IGLAnalysisGetCurveCoordinate"/> service.</param>
        /// <param name="commonService"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="glAnalysisService"/> is null
        /// <paramref name="analysisCurves"/> is null
        /// or
        /// <paramref name="loggerFactory"/> is null
        /// or
        /// <paramref name="localePhrases"/> is null
        /// or
        /// <paramref name="wellAnalysisCorrelation"/> is null
        /// </exception>
        public GLAnalysisProcessingService(IGLAnalysis glAnalysisService, IThetaLoggerFactory loggerFactory,
             ILocalePhrases localePhrases, IWellAnalysisCorrelation wellAnalysisCorrelation, IManufacturer manufacturer,
             IAnalysisCurve analysisCurves, IAnalysisCurveSets analysisCurvesSet, IGLAnalysisGetCurveCoordinate gLAnalysisGetCurveCoordinate, ICommonService commonService)
        {
            _glAnalysisService = glAnalysisService ?? throw new ArgumentNullException(nameof(glAnalysisService));
            _analysisCurves = analysisCurves ?? throw new ArgumentNullException(nameof(analysisCurves));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _localePhrases = localePhrases ?? throw new ArgumentNullException(nameof(localePhrases));
            _correlationService = wellAnalysisCorrelation ?? throw new ArgumentNullException(nameof(wellAnalysisCorrelation));
            _manufacturerService = manufacturer ?? throw new ArgumentNullException(nameof(manufacturer));
            _gLAnalysisGetCurveCoordinate = gLAnalysisGetCurveCoordinate ?? throw new ArgumentNullException(nameof(gLAnalysisGetCurveCoordinate));
            _analysisCurvesSet = analysisCurvesSet ?? throw new ArgumentNullException(nameof(analysisCurvesSet));
            _commonService = commonService ?? throw new ArgumentNullException(nameof(commonService));
        }

        #endregion

        #region IGLAnalysisProcessingService Implementation

        /// <summary>
        /// Provids the GLAnalysis survey date.
        /// </summary>
        /// <param name="requestWithCorrelationId">The <seealso cref="Common.GLAnalysisInput"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="NotificationTypesDataOutput"/>.</returns>
        public GLAnalysisSurveyDataOutput GetGLAnalysisSurveyDate(
            WithCorrelationId<GLSurveyAnalysisInput> requestWithCorrelationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.GLAnalysis);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GLAnalysisProcessingService)} {nameof(GetGLAnalysisSurveyDate)}", requestWithCorrelationId?.CorrelationId);

            GLAnalysisSurveyDataOutput gLAnalysisDataOutput = new GLAnalysisSurveyDataOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            if (requestWithCorrelationId == null)
            {
                var message = $"{nameof(requestWithCorrelationId)} is null, cannot get GLAnalysis survey Date.";
                logger.Write(Level.Info, message);
                gLAnalysisDataOutput.Result.Status = false;
                gLAnalysisDataOutput.Result.Value = message;

                return gLAnalysisDataOutput;
            }

            if (requestWithCorrelationId?.Value == null)
            {
                var message = $"{nameof(requestWithCorrelationId)} is null, cannot get GLAnalysis survey date.";
                logger.WriteCId(Level.Info, message, requestWithCorrelationId?.CorrelationId);
                gLAnalysisDataOutput.Result = new MethodResult<string>(false, message);

                return gLAnalysisDataOutput;
            }

            var correlationId = requestWithCorrelationId?.CorrelationId;

            var glAnalysisRequest = requestWithCorrelationId.Value;

            if (glAnalysisRequest.Guid.Equals(null))
            {
                var message = $"Either {nameof(glAnalysisRequest.Guid)}" +
                    $" should be provided to get survey date.";
                logger.WriteCId(Level.Info, message, correlationId);
                gLAnalysisDataOutput.Result = new MethodResult<string>(false, message);

                return gLAnalysisDataOutput;
            }

            if (!glAnalysisRequest.Guid.Equals(null))
            {
                var resultData = _glAnalysisService.GetGLAnalysisSurveyDate(glAnalysisRequest.Guid,
                    IndustryApplication.GasArtificialLift.Key, SurveyCurveType.TemperatureCurve.Key,
                    SurveyCurveType.PressureCurve.Key, correlationId);

                gLAnalysisDataOutput.Values = new List<GLAnalysisData>();

                foreach (var data in resultData)
                {
                    gLAnalysisDataOutput.Values.Add(GLAnalysisDataMapper.MapToDomainObject(data));
                }
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(GLAnalysisProcessingService)} {nameof(GetGLAnalysisSurveyDate)}", requestWithCorrelationId?.CorrelationId);

            return gLAnalysisDataOutput;
        }

        /// <summary>
        /// Processes the provided gl analysis request and generates response based on that data.
        /// </summary>
        /// <param name="data">The <seealso cref="WithCorrelationId{GLAnalysisInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="GLAnalysisDataOutput"/></returns>
        public GLAnalysisDataOutput GetGLAnalysisResults(WithCorrelationId<Models.Inputs.GLAnalysisInput> data)
        {
            var logger = _loggerFactory.Create(LoggingModel.GLAnalysis);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GLAnalysisProcessingService)} {nameof(GetGLAnalysisResults)}", data?.CorrelationId);

            GLAnalysisDataOutput response = new GLAnalysisDataOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            if (data == null)
            {
                var message = $"{nameof(data)} is null, cannot get gas lift analysis results.";
                logger.Write(Level.Info, message);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            if (data?.Value == null)
            {
                var message = $"{nameof(data)} is null, cannot get gas lift analysis results.";
                logger.WriteCId(Level.Info, message, data?.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            var correlationId = data?.CorrelationId;
            var request = data.Value;

            if (string.IsNullOrEmpty(request.TestDate) ||
                request.AssetId == Guid.Empty || request.AnalysisTypeId == null)
            {
                var message = $"{nameof(request.TestDate)} and {nameof(request.AssetId)}" +
                    $" should be provided to get gas lift analysis results.";
                logger.WriteCId(Level.Info, message, correlationId);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            if (request.AnalysisTypeId.Value == AnalysisType.Sensitivity.Key && request.AnalysisResultId.HasValue == false)
            {
                var message = $"{nameof(request.AnalysisResultId)} " +
                    " should be provided to get gas lift analysis results for sensitivity analysis.";
                logger.WriteCId(Level.Info, message, correlationId);
                response.Result.Status = false;
                response.Result.Value = message;
                return response;
            }
            GLAnalysisResponse glAnalysisData = null;
            if (request.AnalysisTypeId.Value == AnalysisType.WellTest.Key)
            {
                glAnalysisData = _glAnalysisService.GetGLAnalysisData(request.AssetId, request.TestDate,
                    EnhancedEnumBase.GetValue<AnalysisType>((int)request.AnalysisTypeId).Key, correlationId);
            }
            else if (request.AnalysisTypeId.Value == AnalysisType.Sensitivity.Key)
            {
                glAnalysisData = _glAnalysisService.GetGLSensitivityAnalysisData(request.AssetId, request.TestDate,
                    request.AnalysisResultId.Value, correlationId);
            }

            if (glAnalysisData == null || glAnalysisData.NodeMasterData == null || glAnalysisData.WellDetail == null
                || glAnalysisData.AnalysisResultEntity == null || glAnalysisData.ValveStatusEntities == null
                || glAnalysisData.WellValveEntities == null)
            {
                var message = (glAnalysisData?.NodeMasterData == null) ? $"{nameof(glAnalysisData.NodeMasterData)} is null"
                    : (glAnalysisData?.WellDetail == null) ? $"{nameof(glAnalysisData.WellDetail)} is null"
                    : (glAnalysisData?.AnalysisResultEntity == null) ? $"{nameof(glAnalysisData.AnalysisResultEntity)} is null"
                    : (glAnalysisData?.ValveStatusEntities == null) ? $"{nameof(glAnalysisData.ValveStatusEntities)} is null"
                    : (glAnalysisData?.WellValveEntities == null) ? $"{nameof(glAnalysisData.WellValveEntities)} is null"
                    : $"{nameof(glAnalysisData)} is null";
                response.Result.Status = false;
                response.Result.Value = $"{message}, cannot get gas lift analysis results.";
                logger.WriteCId(Level.Info, $"{message}, cannot get gas lift analysis results.", correlationId);
            }
            else
            {
                var analysisResult = GetGLAnalysisResponseData(glAnalysisData, correlationId);
                var wellboreData = GetGLAnalysisWellboreData(analysisResult, request.AssetId, correlationId);
                var wellboreViewData = GetGLAnalysisWellboreViewData(wellboreData, analysisResult.FlowControlDeviceStatuses.ValveStatuses, analysisResult.Inputs.UseTVD, correlationId);

                var glAnalysisValues = CreateResponse(analysisResult, glAnalysisData.TestDate,
                    glAnalysisData.WellDetail, glAnalysisData.GasRatePhrase, wellboreViewData, correlationId);

                response.Values = glAnalysisValues;
                response.Result.Status = true;
                response.Result.Value = string.Empty;
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(GLAnalysisProcessingService)} {nameof(GetGLAnalysisResults)}", data?.CorrelationId);

            return response;
        }

        /// <summary>
        /// Provids the GLAnalysis survey Curve Coordinates.
        /// </summary>
        /// <param name="requestWithCorrelationId">The <seealso cref="GLAnalysisCurveCoordinateInput"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="GLAnalysisCurveCoordinateDataOutput"/>.</returns>
        public GLAnalysisCurveCoordinateDataOutput GetGLAnalysisSurveyCurveCoordinate(
            WithCorrelationId<GLAnalysisCurveCoordinateInput>
                requestWithCorrelationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.GLAnalysis);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GLAnalysisProcessingService)} {nameof(GetGLAnalysisSurveyCurveCoordinate)}",
                requestWithCorrelationId?.CorrelationId);

            GLAnalysisCurveCoordinateDataOutput gLAnalysisDataOutput = new GLAnalysisCurveCoordinateDataOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            if (requestWithCorrelationId == null)
            {
                var message = $"{nameof(requestWithCorrelationId)} is null, cannot get GL Analysis CurveCoordinate Data.";
                logger.Write(Level.Info, message);
                gLAnalysisDataOutput.Result.Status = false;
                gLAnalysisDataOutput.Result.Value = message;

                return gLAnalysisDataOutput;
            }

            if (requestWithCorrelationId?.Value == null)
            {
                var message = $"{nameof(requestWithCorrelationId)} is null, cannot get GL Analysis CurveCoordinate Data.";
                logger.WriteCId(Level.Info, message, requestWithCorrelationId?.CorrelationId);
                gLAnalysisDataOutput.Result = new MethodResult<string>(false, message);

                return gLAnalysisDataOutput;
            }

            var correlationId = requestWithCorrelationId?.CorrelationId;

            var glAnalysisRequest = requestWithCorrelationId.Value;

            if (glAnalysisRequest.Guid.Equals(null))
            {
                var message = $"Either {nameof(glAnalysisRequest.Guid)}" +
                    $" should be provided to get survey date.";
                logger.WriteCId(Level.Info, message, correlationId);
                gLAnalysisDataOutput.Result = new MethodResult<string>(false, message);

                return gLAnalysisDataOutput;
            }

            if (!glAnalysisRequest.Guid.Equals(null))
            {
                var resultSurveyData = _glAnalysisService.GetGLAnalysisCurveCoordinatesSurveyDate(glAnalysisRequest.Guid,
                    glAnalysisRequest.SurveyDate, correlationId);

                gLAnalysisDataOutput.Values = new List<GLAnalysisCurveCoordinateData>();
                if (resultSurveyData != null)
                {
                    var result = RetrieveSurveyCurvesRange(resultSurveyData, correlationId);

                    foreach (var data in result)
                    {
                        gLAnalysisDataOutput.Values.Add(GLAnalysisDataMapper.MapToCurveCoordinateDomainObject(data));
                    }
                }
            }

            var values = gLAnalysisDataOutput.Values;
            RoundCoordinatesToSignificantDigits(ref values, correlationId);

            logger.WriteCId(Level.Trace, $"Finished {nameof(GLAnalysisProcessingService)} {nameof(GetGLAnalysisSurveyCurveCoordinate)}",
               requestWithCorrelationId?.CorrelationId);

            return gLAnalysisDataOutput;
        }

        /// <summary>
        /// Processes the provided Analysis Curve Coordinate request and generates response based on that data.
        /// </summary>
        /// <param name="data">The <seealso cref="WithCorrelationId{GLAnalysisCurveCoordinateInput}"/> to act on, annotated with a correlation id.</param>
        /// <returns>The <seealso cref="GLAnalysisCurveCoordinateDataOutput"/></returns>
        public GLAnalysisCurveCoordinateDataOutput GetGLAnalysisCurveCoordinateResults(
            WithCorrelationId<GLAnalysisCurveCoordinateInput> data)
        {
            var logger = _loggerFactory.Create(LoggingModel.GLAnalysis);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GLAnalysisProcessingService)} {nameof(GetGLAnalysisCurveCoordinateResults)}",
                data?.CorrelationId);

            if (data == null)
            {
                var message = $"{nameof(data)} is null, cannot get gas lift analysis curve coordinate results.";
                logger.Write(Level.Info, message);

                return new GLAnalysisCurveCoordinateDataOutput()
                {
                    Result = new MethodResult<string>(false, message),
                };
            }

            var phraseId = Enum.GetValues<PhraseIDs>().Cast<int>().ToArray();

            phrases = _localePhrases.GetAll(data.CorrelationId, phraseId);

            GLAnalysisCurveCoordinateDataOutput response = new GLAnalysisCurveCoordinateDataOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            #region Validations

            if (data?.Value == null)
            {
                var message = $"{nameof(data)} is null, cannot get gas lift analysis curve coordinate results.";
                logger.WriteCId(Level.Info, message, data?.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            var correlationId = data?.CorrelationId;
            var request = data.Value;

            if (string.IsNullOrEmpty(request.AssetId.ToString()) || request.AssetId == Guid.Empty)
            {
                var message = $"{nameof(request.AssetId)} and {nameof(request.AssetId)}" +
                    $" should be provided to get gas lift analysis curve coordinate results.";
                logger.WriteCId(Level.Info, message, correlationId);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            if (string.IsNullOrEmpty(request.AnalysisResultId.ToString()))
            {
                var message = $"{nameof(request.AnalysisResultId)} and {nameof(request.AnalysisResultId)}" +
                    $" should be provided to get gas lift analysis curve coordinate results.";
                logger.WriteCId(Level.Info, message, correlationId);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            if (string.IsNullOrEmpty(request.AnalysisTypeId.ToString()))
            {
                var message = $"{nameof(request.AnalysisTypeId)} and {nameof(request.AnalysisTypeId)}" +
                    $" should be provided to get gas lift analysis curve coordinate results.";
                logger.WriteCId(Level.Info, message, correlationId);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            if (string.IsNullOrEmpty(request.TestDate.ToString()))
            {
                var message = $"{nameof(request.TestDate)} and {nameof(request.TestDate)}" +
                    $" should be provided to get gas lift analysis curve coordinate results.";
                logger.WriteCId(Level.Info, message, correlationId);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            #endregion

            IList<AnalysisResultCurvesModel> analysisResultCurve =
                _gLAnalysisGetCurveCoordinate.GetAnalysisResultCurve(data.Value.AnalysisResultId,
                    IndustryApplication.GasArtificialLift.Key, data.CorrelationId);

            if (analysisResultCurve == null)
            {
                response.Result.Status = false;
                response.Result.Value = "gas lift analysis results is null.";
                return response;
            }
            else
            {
                response.Values = GetGLAnalysisCurveCoordinateResponseResults(analysisResultCurve);

                var iprCurveCoordinate = GetIPRCurveCoordinates(data);
                if (response.Values != null)
                {
                    double min = 0;
                    double max = 0;

                    foreach (var item in response.Values)
                    {
                        GetMinAndMax(item.CoordinatesOutput, ref min, ref max);
                    }

                    if (iprCurveCoordinate?.CoordinatesOutput?.Count > 0)
                    {
                        response.Values.Add(iprCurveCoordinate);
                    }

                    AddFBHPCurves(data, ref response);
                    AddStaticFluidCurve(data, ref response);

                    var analysisResult =
                        _glAnalysisService.GetGLAnalysisData(data.Value.AssetId,
                        data.Value.TestDate, data.Value.AnalysisTypeId, correlationId);

                    if (analysisResult == null || analysisResult.NodeMasterData == null || analysisResult.WellDetail == null
                        || analysisResult.AnalysisResultEntity == null || analysisResult.ValveStatusEntities == null
                        || analysisResult.WellValveEntities == null)
                    {
                        response.Result.Status = false;
                        response.Result.Value = $"cannot get gas lift analysis results.";
                        logger.WriteCId(Level.Info, $"cannot get gas lift analysis results.", correlationId);
                        return response;
                    }
                    else
                    {
                        var analysisData = GetGLAnalysisResponseData(analysisResult, correlationId);

                        AddColemanCriticalFlowRate(max, analysisData, ref response);
                        AddThornhillCraverValve(max, analysisData, ref response);
                        AddOperatingPoint(analysisData, ref response);
                    }
                }

                var values = response.Values;
                RoundCoordinatesToSignificantDigits(ref values, correlationId);

                response.Result.Status = true;
                response.Result.Value = string.Empty;
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(GLAnalysisProcessingService)} {nameof(GetGLAnalysisCurveCoordinateResults)}",
               data?.CorrelationId);

            return response;
        }

        #endregion

        #region Private Methods

        private GLAnalysisResult GetGLAnalysisResponseData(GLAnalysisResponse glAnalysisData, string correlationId)
        {
            var resultEntity = glAnalysisData.AnalysisResultEntity;

            var input = new Common.GLAnalysisInput
            {
                GasInjectionRate = resultEntity.InjectedGasRate.HasValue ? resultEntity.InjectedGasRate : null,
                GasInjectionDepth = resultEntity.GasInjectionDepth.HasValue ? resultEntity.GasInjectionDepth : null,
                VerticalWellDepth = resultEntity.VerticalWellDepth.HasValue ? resultEntity.VerticalWellDepth : null,
                MeasuredWellDepth = resultEntity.MeasuredWellDepth.HasValue ? resultEntity.MeasuredWellDepth : null,
                WellheadTemperature = resultEntity.WellheadTemperature.HasValue ? resultEntity.WellheadTemperature : null,
                BottomholeTemperature =
                    resultEntity.BottomholeTemperature.HasValue ? resultEntity.BottomholeTemperature : null,
                ReservoirPressure = resultEntity.ReservoirPressure.HasValue ? resultEntity.ReservoirPressure : null,
                BubblepointPressure = resultEntity.BubblepointPressure.HasValue ? resultEntity.BubblepointPressure : null,
                FormationGasOilRatio = resultEntity.FormationGor,
                OilRate = resultEntity.OilRate.HasValue ? resultEntity.OilRate : null,
                WaterRate = resultEntity.WaterRate.HasValue ? resultEntity.WaterRate : null,
                GasRate = resultEntity.GasRate.HasValue ? resultEntity.GasRate : null,
                WellheadPressure = resultEntity.WellheadPressure.HasValue ? resultEntity.WellheadPressure : null,
                CasingPressure = resultEntity.CasingPressure.HasValue ? resultEntity.CasingPressure : null,
                OilSpecificGravity = resultEntity.OilSpecificGravity ?? null,
                WaterSpecificGravity = resultEntity.WaterSpecificGravity ?? null,
                GasSpecificGravity = resultEntity.GasSpecificGravity.HasValue ? resultEntity.GasSpecificGravity : null,
                PercentCO2 = resultEntity.PercentCO2.HasValue ? resultEntity.PercentCO2 : 0,
                PercentN2 = resultEntity.PercentN2.HasValue ? (float)resultEntity.PercentN2 : 0,
                PercentH2S = resultEntity.PercentH2S.HasValue ? (float)resultEntity.PercentH2S : 0,
                TubingInnerDiameter = resultEntity.TubingId ?? null,
                CasingInnerDiameter = resultEntity.CasingId ?? null,
                TubingOuterDiameter = resultEntity.TubingOD ?? null,
                EstimateInjectionDepth = resultEntity.EstimateInjectionDepth.HasValue
                    && (bool)resultEntity.EstimateInjectionDepth,
                UseDownholeGaugeForAnalysis = resultEntity.UseDownholeGageInAnalysis.HasValue
                    && (bool)resultEntity.UseDownholeGageInAnalysis,
                DownholeGaugeDepth = resultEntity.DownholeGageDepth.HasValue ? resultEntity.DownholeGageDepth : null,
                DownholeGaugePressure = resultEntity.DownholeGagePressure.HasValue ? resultEntity.DownholeGagePressure : null,
                UseTVD = resultEntity.UseTVD,
            };

            input.MultiphaseFlowCorrelation =
                new AnalysisCorrelation(_correlationService.GetAnalysisCorrelation(resultEntity.FlowCorrelationId, correlationId));
            input.OilFormationVolumeFactorCorrelation =
                new AnalysisCorrelation(
                    _correlationService.GetAnalysisCorrelation(resultEntity.OilFormationVolumeFactorCorrelationId, correlationId));
            input.OilViscosityCorrelation =
                new AnalysisCorrelation(_correlationService.GetAnalysisCorrelation(resultEntity.OilViscosityCorrelationId, correlationId));
            input.SolutionGasOilRatioCorrelation =
                new AnalysisCorrelation(
                    _correlationService.GetAnalysisCorrelation(resultEntity.SolutionGasOilRatioCorrelationId, correlationId));
            input.ZFactorCorrelation =
                new AnalysisCorrelation(_correlationService.GetAnalysisCorrelation(resultEntity.ZfactorCorrelationId, correlationId));
            input.TubingCriticalVelocityCorrelation =
                new AnalysisCorrelation(
                    _correlationService.GetAnalysisCorrelation(resultEntity.TubingCriticalVelocityCorrelationId, correlationId));

            input.AnalysisType = EnhancedEnumBase.GetValue<AnalysisType>(resultEntity.AnalysisType);

            var output = new GLAnalysisOutput
            {
                ProductivityIndex = resultEntity.ProductivityIndex,
                FlowingBHP = resultEntity.FlowingBhp.HasValue ? resultEntity.FlowingBhp : null,
                FlowingBHPAtInjectionDepth = resultEntity.FlowingBHPAtInjectionDepth.HasValue
                    ? resultEntity.FlowingBHPAtInjectionDepth
                    : null,
                IPRSlope = resultEntity.IPRSlope,
                RateAtBubblePoint = resultEntity.RateAtBubblePoint ?? null,
                RateAtMaxLiquid = resultEntity.RateAtMaxLiquid ?? null,
                RateAtMaxOil = resultEntity.RateAtMaxOil ?? null,
                WaterCut = resultEntity.WaterCut ?? null,
                KillFluidLevel = resultEntity.KillFluidLevel.HasValue ? resultEntity.KillFluidLevel : null,
                ReservoirFluidLevel = resultEntity.ReservoirFluidLevel.HasValue ? resultEntity.ReservoirFluidLevel : null,
                GLRForMaxLiquidRate = resultEntity.GLRForMaxLiquidRate,
                GLRForOptimumLiquidRate = resultEntity.GlrforOptimumLiquidRate,
                InjectedGLR = resultEntity.InjectedGLR,
                GrossRate = resultEntity.GrossRate ?? null,
                InjectionRateForMaxLiquidRate = resultEntity.InjectionRateForMaxLiquidRate.HasValue
                    ? resultEntity.InjectionRateForMaxLiquidRate
                    : null,
                InjectionRateForOptimumLiquidRate = resultEntity.InjectionRateForOptimumLiquidRate.HasValue
                    ? resultEntity.InjectionRateForOptimumLiquidRate
                    : null,
                MaxLiquidRate = resultEntity.MaxLiquidRate.HasValue ? resultEntity.MaxLiquidRate : null,
                MinimumFBHP = resultEntity.MinimumFbhp.HasValue ? resultEntity.MinimumFbhp : null,
                OptimumLiquidRate = resultEntity.OptimumLiquidRate.HasValue ? resultEntity.OptimumLiquidRate : null,
                FBHPForOptimumLiquidRate = resultEntity.FbhpforOptimumLiquidRate.HasValue
                    ? resultEntity.FbhpforOptimumLiquidRate
                    : null,
                ValveCriticalVelocity =
                    resultEntity.ValveCriticalVelocity.HasValue ? resultEntity.ValveCriticalVelocity : null,
                TubingCriticalVelocity =
                    resultEntity.TubingCriticalVelocity.HasValue ? resultEntity.TubingCriticalVelocity : null,
                InjectionRateForTubingCriticalVelocity = resultEntity.InjectionRateForTubingCriticalVelocity.HasValue
                    ? resultEntity.InjectionRateForTubingCriticalVelocity
                    : null,
                AdjustedAnalysisToDownholeGaugeReading = resultEntity.AdjustedAnalysisToDownholeGaugeReading.HasValue
                    && (bool)resultEntity.AdjustedAnalysisToDownholeGaugeReading,
            };

            output.ValveInjectionDepthEstimateResultData = new ValveInjectionDepthEstimateResult()
            {
                MeasuredInjectionDepthFromValveAnalysis = resultEntity.MeasuredInjectionDepthFromAnalysis,
                VerticalInjectionDepthFromValveAnalysis = resultEntity.VerticalInjectionDepthFromAnalysis,
            };

            var source = new AnalysisSource
            {
                TubingPressureSource = resultEntity.TubingPressureSource ?? null,
                CasingPressureSource = resultEntity.CasingPressureSource ?? null,
                WellHeadTemperatureSource = resultEntity.WellHeadTemperatureSource ?? null,
                BottomholeTemperatureSource = resultEntity.BottomholeTemperatureSource ?? null,
                OilSpecificGravitySource = resultEntity.OilSpecificGravitySource ?? null,
                WaterSpecificGravitySource = resultEntity.WaterSpecificGravitySource ?? null,
                GasSpecificGravitySource = resultEntity.GasSpecificGravitySource ?? null,
                InjectedGasRateSource = resultEntity.InjectedGasRateSource ?? null,
                DownholeGaugePressureSource = resultEntity.DownholeGagePressureSource ?? null,
                OilRateSource = resultEntity.OilRateSource ?? null,
                WaterRateSource = resultEntity.WaterRateSource ?? null,
                GasRateSource = resultEntity.GasRateSource ?? null,
                MultiphaseFlowCorrelationSource = resultEntity.MultiphaseFlowCorrelationSource ?? null,
            };

            List<ValveStatus> valveStatusList = new List<ValveStatus>();
            for (var i = 0; i < glAnalysisData.WellValveEntities.Count; i++)
            {
                valveStatusList.Add(MapToDomain(glAnalysisData.WellValveEntities[i], glAnalysisData.ValveStatusEntities[i], correlationId));
            }

            OrificeStatus orificeStatus = new OrificeStatus();
            if (glAnalysisData.WellOrificeStatus != null)
            {
                orificeStatus = MapToDomain(glAnalysisData.WellOrificeStatus, correlationId);
            }

            var flowControlDeviceStatuses = new FlowControlDeviceStatuses
            {
                ValveStatuses = valveStatusList,
                InjectingGasBelowTubing = resultEntity.IsInjectingBelowTubing != null
                    && (bool)resultEntity.IsInjectingBelowTubing,
                OrificeStatus = orificeStatus,
            };

            var resultMessageLocalized = string.Empty;

            if (resultEntity.ResultMessageTemplate != null)
            {
                resultMessageLocalized = resultEntity.ResultMessageTemplate;
            }

            var model = new GLAnalysisResult(resultEntity.Id)
            {
                ProcessedDateTime = resultEntity.ProcessedDate,
                Inputs = input,
                Outputs = output,
                ResultMessage = resultEntity.ResultMessage,
                ResultMessageLocalized = resultMessageLocalized,
                IsSuccess = resultEntity.Success,
                Sources = source,
                FlowControlDeviceStatuses = flowControlDeviceStatuses,
                IsProcessed = resultEntity.IsProcessed,
            };

            return model;
        }

        private static ValveStatus MapToDomain(GLWellValveModel wellValve, GLValveStatusModel valveStatus, string correlationId)
        {
            var model = new ValveStatus();

            if (valveStatus != null)
            {
                model.SetId(valveStatus.Id);
                model.GasRate = valveStatus.GasRate == null ? null : valveStatus.GasRate;
                model.IsInjectingGas = valveStatus.IsInjectingGas;
                model.State = EnhancedEnumBase.GetValue<ValveState>(valveStatus.ValveState);
                model.InjectionPressureAtDepth =
                    valveStatus.InjectionPressureAtDepth == null ? null : valveStatus.InjectionPressureAtDepth;
                model.TubingCriticalVelocityAtDepth = valveStatus.TubingCriticalVelocityAtDepth == null
                    ? null
                    : valveStatus.TubingCriticalVelocityAtDepth;
                model.InjectionRateForTubingCriticalVelocity =
                    valveStatus.InjectionRateForTubingCriticalVelocity == null
                        ? null
                        : valveStatus.InjectionRateForTubingCriticalVelocity;
                model.ClosingPressureAtDepth =
                    valveStatus.ClosingPressureAtDepth == null ? null : valveStatus.ClosingPressureAtDepth;
                model.OpeningPressureAtDepth = valveStatus.OpeningPressureAtDepth == null
                    ? null
                    : valveStatus.OpeningPressureAtDepth;
                model.Depth = valveStatus.Depth == null ? null : valveStatus.Depth;

                if (wellValve != null)
                {
                    model.FlowControlDevice = new WellValve(wellValve.Id)
                    {
                        TPRO = wellValve.TestRackOpeningPressure == null ? null : wellValve.TestRackOpeningPressure,
                        ClosingPressureAtDepth = wellValve.ClosingPressureAtDepth == null
                            ? null
                            : wellValve.ClosingPressureAtDepth,
                        OpeningPressureAtDepth = wellValve.OpeningPressureAtDepth == null
                            ? null
                            : wellValve.OpeningPressureAtDepth,
                        VerticalDepth = wellValve.VerticalDepth,
                        MeasuredDepth = wellValve.MeasuredDepth,
                    };

                    if (wellValve.ValveId != null)
                    {
                        var manufacturer = _manufacturerService.Get((int)wellValve.ManufacturerId, correlationId);
                        var glmanufacturer = new Manufacturer();
                        if (manufacturer != null)
                        {
                            glmanufacturer = new Manufacturer(manufacturer.ManufacturerID);
                        }

                        model.FlowControlDevice.Valve = new Valve(wellValve.ValveId)
                        {
                            BellowsArea = wellValve.BellowsArea,
                            Description = wellValve.Description,
                            Diameter = wellValve.Diameter,
                            Manufacturer = glmanufacturer,
                            OneMinusR = wellValve.OneMinusR,
                            PortArea = wellValve.PortArea,
                            PortSize = wellValve.PortSize,
                            PortToBellowsAreaRatio = wellValve.PortToBellowsAreaRatio,
                            ProductionPressureEffectFactor = wellValve.ProductionPressureEffectFactor,
                        }; //model.FlowControlDevice.Valve = new Valve(valves.ID)
                    } //if (valves != null)
                } //if (wellValves != null)
            } //if (valveStatus != null)

            return model;
        }

        private static OrificeStatus MapToDomain(GLWellOrificeStatusModel orifice, string correlationId)
        {
            var model = new OrificeStatus();

            if (orifice != null)
            {
                model.InjectionPressureAtDepth =
                    orifice.InjectionPressureAtDepth == null ? null : orifice.InjectionPressureAtDepth;
                model.IsInjectingGas = orifice.IsInjectingGas;
                model.State = EnhancedEnumBase.GetValue<OrificeState>(orifice.OrificeState);
                model.TubingCriticalVelocityAtDepth = orifice.TubingCriticalVelocityAtDepth == null
                    ? null
                    : orifice.TubingCriticalVelocityAtDepth;
                model.InjectionRateForTubingCriticalVelocity = orifice.InjectionRateForTubingCriticalVelocity == null
                    ? null
                    : orifice.InjectionRateForTubingCriticalVelocity;
                model.Depth = orifice.Depth == null ? null : orifice.Depth;
                if (orifice != null)
                {
                    var manufacturer = _manufacturerService.Get(orifice.ManufacturerId, correlationId);
                    var glmanufacturer = new Manufacturer();
                    if (manufacturer != null)
                    {
                        glmanufacturer = new Manufacturer(manufacturer.ManufacturerID);
                    }

                    model.SetId(orifice.NodeId);
                    model.FlowControlDevice = new Orifice
                    {
                        Manufacturer = glmanufacturer,
                        MeasuredDepth = orifice.MeasuredDepth == null
                            ? null
                            : orifice.MeasuredDepth,
                        VerticalDepth = orifice.VerticalDepth,
                        PortSize = orifice.PortSize,
                    }; //model.FlowControlDevice = new Orifice
                } //if (orifice != null)
            } //if (status != null)

            return model;
        }

        private GLAnalysisValues CreateResponse(GLAnalysisResult result, DateTime testDate,
            GLWellDetailModel glWellDetail, string gasRatePhrase, GLAnalysisWellboreViewData wellboreViewData, string correlationId)
        {
            var digits = _commonService.GetSystemParameterNextGenSignificantDigits(correlationId);

            var phraseid = Enum.GetValues<PhraseIDs>().Cast<int>().ToArray();

            phrases = _localePhrases.GetAll(correlationId, phraseid);

            var responseValues = new GLAnalysisValues()
            {
                Inputs = new List<AnalysisInputOutput>(),
                Outputs = new List<AnalysisInputOutput>(),
                Valves = new List<FlowControlDeviceAnalysisValues>(),
                WellboreViewData = wellboreViewData
            };

            responseValues.Inputs.Add(new AnalysisInputOutput()
            {
                Id = "TestDate",
                DisplayValue = testDate.ToString(),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.TestDate]),
                Value = testDate.ToString(),
            });

            responseValues.Inputs.Add(new AnalysisInputOutput()
            {
                Id = "OilRate",
                DisplayValue = FormatValue(result.Inputs.OilRate, NULL_TEXT, digits, LiquidFlowRate.BarrelPerDay.Symbol),
                Value = result.Inputs.OilRate.HasValue
                    ? LiquidFlowRate.FromBarrelsPerDay((double)result.Inputs.OilRate.Value).Amount : null,
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(_localePhrases.Get(532, correlationId)),
                SourceId = result.Sources.OilRateSource,
                MeasurementAbbreviation = LiquidFlowRate.BarrelPerDay.Symbol,
            });

            responseValues.Inputs.Add(new AnalysisInputOutput()
            {
                Id = "WaterRate",
                DisplayValue = FormatValue(result.Inputs.WaterRate, NULL_TEXT, digits, LiquidFlowRate.BarrelPerDay.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.WaterRate]),
                Value = result.Inputs.WaterRate.HasValue
                    ? LiquidFlowRate.FromBarrelsPerDay((double)result.Inputs.WaterRate.Value).Amount : null,
                MeasurementAbbreviation = LiquidFlowRate.BarrelPerDay.Symbol,
                SourceId = result.Sources.WaterRateSource,
            });

            responseValues.Inputs.Add(new AnalysisInputOutput()
            {
                Id = "GasRate",
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(GetGasRatePhrase(gasRatePhrase)),
                DisplayValue = FormatValue(result.Inputs.GasRate, NULL_TEXT, digits, GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol),
                Value = result.Inputs.GasRate.HasValue
                    ? GasFlowRate.FromThousandStandardCubicFeetPerDay((double)result.Inputs.GasRate.Value).Amount : null,
                MeasurementAbbreviation = GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol,
                SourceId = result.Sources.GasRateSource,
            });

            responseValues.Inputs.Add(new AnalysisInputOutput()
            {
                Id = "FormationGasOilRatio",
                DisplayValue = FormatValue(result.Inputs.FormationGasOilRatio, NULL_TEXT, digits, GasFlowRate.StandardCubicMeterPerDay.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(_localePhrases.Get(5765, correlationId)),
                Value = result.Inputs.FormationGasOilRatio.HasValue
                    ? $"{result.Inputs.FormationGasOilRatio.Value}"
                    : null,
                MeasurementAbbreviation = GasFlowRate.StandardCubicMeterPerDay.Symbol,
            });

            responseValues.Inputs.Add(new AnalysisInputOutput()
            {
                Id = "TubingInnerDiameter",
                DisplayValue = FormatValue(result.Inputs.TubingInnerDiameter, NULL_TEXT, digits, Length.Inch.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.TubingInnerDiameter]),
                Value = result.Inputs.TubingInnerDiameter.HasValue
                    ? Length.FromInches((double)result.Inputs.TubingInnerDiameter.Value).Amount : null,
                MeasurementAbbreviation = Length.Inch.Symbol,
            });

            responseValues.Inputs.Add(new AnalysisInputOutput()
            {
                Id = "CasingPressure",
                DisplayValue = FormatValue(result.Inputs.CasingPressure, NULL_TEXT, digits, Pressure.PoundPerSquareInch.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.CasingPressure]),
                Value = result.Inputs.CasingPressure.HasValue
                    ? Pressure.FromPoundsPerSquareInch((double)result.Inputs.CasingPressure.Value).Amount : null,
                MeasurementAbbreviation = Pressure.PoundPerSquareInch.Symbol,
                SourceId = result.Sources.CasingPressureSource
            });

            responseValues.Inputs.Add(new AnalysisInputOutput()
            {
                Id = "TubingPressure",
                DisplayValue = FormatValue(result.Inputs.WellheadPressure, NULL_TEXT, digits, Pressure.PoundPerSquareInch.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.TubingPressure]),
                Value = result.Inputs.WellheadPressure.HasValue
                    ? Pressure.FromPoundsPerSquareInch((double)result.Inputs.WellheadPressure.Value).Amount : null,
                MeasurementAbbreviation = Pressure.PoundPerSquareInch.Symbol,
                SourceId = result.Sources.TubingPressureSource
            });

            responseValues.Inputs.Add(new AnalysisInputOutput()
            {
                Id = "WellheadTemperature",
                DisplayValue = FormatValue(result.Inputs.WellheadTemperature, NULL_TEXT, digits, Temperature.DegreeFahrenheit.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.WellheadTemperature]),
                Value = result.Inputs.WellheadTemperature.HasValue
                    ? Temperature.FromDegreesFahrenheit((double)result.Inputs.WellheadTemperature.Value).Amount : null,
                MeasurementAbbreviation = Temperature.DegreeFahrenheit.Symbol,
                SourceId = result.Sources.WellHeadTemperatureSource,
            });

            responseValues.Inputs.Add(new AnalysisInputOutput()
            {
                Id = "ReservoirPressure",
                DisplayValue = FormatValue(result.Inputs.ReservoirPressure, NULL_TEXT, digits, Pressure.PoundPerSquareInch.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.ReservoirPressure]),
                Value = result.Inputs.ReservoirPressure.HasValue
                    ? Pressure.FromPoundsPerSquareInch((double)result.Inputs.ReservoirPressure.Value).Amount : null,
                MeasurementAbbreviation = Pressure.PoundPerSquareInch.Symbol,
            });

            responseValues.Inputs.Add(new AnalysisInputOutput()
            {
                Id = "BottomholeTemperature",
                DisplayValue = FormatValue(result.Inputs.BottomholeTemperature, NULL_TEXT, digits, Temperature.DegreeFahrenheit.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.BottomholeTemperature]),
                Value = result.Inputs.BottomholeTemperature.HasValue
                    ? Temperature.FromDegreesFahrenheit((double)result.Inputs.BottomholeTemperature.Value).Amount : null,
                MeasurementAbbreviation = Temperature.DegreeFahrenheit.Symbol,
                SourceId = result.Sources.BottomholeTemperatureSource,
            });

            responseValues.Inputs.Add(new AnalysisInputOutput()
            {
                Id = "BubblepointPressure",
                DisplayValue = FormatValue(result.Inputs.BubblepointPressure, NULL_TEXT, digits, Pressure.PoundPerSquareInch.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.BottomholeTemperature]),
                Value = result.Inputs.BubblepointPressure.HasValue
                    ? Pressure.FromPoundsPerSquareInch((double)result.Inputs.BubblepointPressure.Value).Amount : null,
                MeasurementAbbreviation = Pressure.PoundPerSquareInch.Symbol,
            });

            if (result.Inputs.OilSpecificGravity.HasValue)
            {
                var oilApi = 141.500f / result.Inputs.OilSpecificGravity.Value - 131.500f;

                responseValues.Inputs.Add(new AnalysisInputOutput()
                {
                    Id = "OilSpecificGravity",
                    DisplayValue = FormatValue(oilApi, string.Empty, digits, OilRelativeDensity.APIGravity.Symbol),
                    SourceId = result.Sources.OilSpecificGravitySource,
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(_localePhrases.Get(5584, correlationId)),
                    Value = OilRelativeDensity.FromDegreesAPI(oilApi).Amount,
                    MeasurementAbbreviation = OilRelativeDensity.APIGravity.Symbol,
                });
            }
            else
            {
                responseValues.Inputs.Add(new AnalysisInputOutput()
                {
                    Id = "OilSpecificGravity",
                    DisplayValue = NULL_TEXT,
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.SpecificGravityofOil]),
                });
            }

            responseValues.Inputs.Add(new AnalysisInputOutput()
            {
                Id = "WaterSpecificGravity",
                DisplayValue = FormatValue(result.Inputs.WaterSpecificGravity, NULL_TEXT, digits, string.Empty),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(_localePhrases.Get(267, correlationId)),
                Value = result.Inputs.WaterSpecificGravity ?? null,
                SourceId = result.Sources.WaterSpecificGravitySource,
            });

            responseValues.Inputs.Add(new AnalysisInputOutput()
            {
                Id = "ProducedGasSpecificGravity",
                DisplayValue = FormatValue(result.Inputs.GasSpecificGravity, NULL_TEXT, digits, string.Empty),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.ProducedGasSpecificGravity]),
                Value = result.Inputs.GasSpecificGravity ?? null
            });

            if (glWellDetail.InjectedGasSpecificGravity.HasValue)
            {
                responseValues.Inputs.Add(new AnalysisInputOutput()
                {
                    Id = "InjectedGasSpecificGravity",
                    DisplayValue = FormatValue(glWellDetail.InjectedGasSpecificGravity, string.Empty, digits, string.Empty),
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.InjectedGasSpecificGravity]),
                    Value = glWellDetail.InjectedGasSpecificGravity ?? null
                });
            }
            else
            {
                responseValues.Inputs.Add(new AnalysisInputOutput()
                {
                    Id = "InjectedGasSpecificGravity",
                    DisplayValue = NULL_TEXT,
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.InjectedGasSpecificGravity]),
                });
            }

            responseValues.Inputs.Add(new AnalysisInputOutput()
            {
                Id = "FlowCorrelation",
                DisplayValue = result.Inputs.MultiphaseFlowCorrelation.Correlation.Name,
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.TestFlowCorrelationDate]),
                Value = result.Inputs.MultiphaseFlowCorrelation.Correlation.Name,
                SourceId = result.Sources.MultiphaseFlowCorrelationSource,
            });

            // Outputs
            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "WaterCut",
                DisplayValue = FormatValue(result.Outputs.WaterCut, NULL_TEXT, digits, Fraction.Percentage.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.Watercut]),
                Value = result.Outputs.WaterCut.HasValue
                    ? Fraction.FromPercentage((double)result.Outputs.WaterCut.Value).Amount : null,
                MeasurementAbbreviation = Fraction.Percentage.Symbol,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "ProductionRate",
                DisplayValue = FormatValue(result.Outputs.GrossRate, NULL_TEXT, digits, LiquidFlowRate.BarrelPerDay.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.ProductionRate]),
                Value = result.Outputs.GrossRate.HasValue
                    ? LiquidFlowRate.FromBarrelsPerDay((double)result.Outputs.GrossRate.Value).Amount : null,
                MeasurementAbbreviation = LiquidFlowRate.BarrelPerDay.Symbol,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "InjectionRate",
                DisplayValue = FormatValue(result.Inputs.GasInjectionRate, NULL_TEXT, digits, GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.InjectionRate]),
                Value = result.Inputs.GasInjectionRate.HasValue
                    ? GasFlowRate.FromThousandStandardCubicFeetPerDay((double)result.Inputs.GasInjectionRate.Value).Amount : null,
                MeasurementAbbreviation = GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol,
                SourceId = result.Sources.InjectedGasRateSource,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "CalculatedVerticalInjectionDepth",
                DisplayValue = FormatValue(result.Outputs.ValveInjectionDepthEstimateResultData.VerticalInjectionDepthFromValveAnalysis, NULL_TEXT, digits, Length.Foot.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.CalculatedVerticalInjectionDepth]),
                Value = result.Outputs.ValveInjectionDepthEstimateResultData.VerticalInjectionDepthFromValveAnalysis ?? null,
                MeasurementAbbreviation = Length.Foot.Symbol,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "CalculatedMeasuredInjectionDepth",
                DisplayValue = FormatValue(result.Outputs.ValveInjectionDepthEstimateResultData.MeasuredInjectionDepthFromValveAnalysis, NULL_TEXT, digits, Length.Foot.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.CalculatedMeasuredInjectionDepth]),
                Value = result.Outputs.ValveInjectionDepthEstimateResultData.MeasuredInjectionDepthFromValveAnalysis ?? null,
                MeasurementAbbreviation = Length.Foot.Symbol,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "FlowingBottomholePressure",
                DisplayValue = FormatValue(result.Outputs.FlowingBHP, NULL_TEXT, digits, Pressure.PoundPerSquareInch.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.FlowingBottomholePressure]),
                Value = result.Outputs.FlowingBHP.HasValue
                    ? Pressure.FromPoundsPerSquareInch((double)result.Outputs.FlowingBHP.Value).Amount : null,
                MeasurementAbbreviation = Pressure.PoundPerSquareInch.Symbol,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "MaximumProductionRate",
                DisplayValue = FormatValue(result.Outputs.MaxLiquidRate, NULL_TEXT, digits, LiquidFlowRate.BarrelPerDay.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.MaximumProductionRate]),
                Value = result.Outputs.MaxLiquidRate.HasValue
                    ? LiquidFlowRate.FromBarrelsPerDay((double)result.Outputs.MaxLiquidRate.Value).Amount : null,
                MeasurementAbbreviation = LiquidFlowRate.BarrelPerDay.Symbol,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "InjectionRateForMaximumProductionRate",
                DisplayValue = FormatValue(result.Outputs.InjectionRateForMaxLiquidRate, NULL_TEXT, digits, GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.InjectionRateForMaxProductionRate]),
                Value = result.Outputs.InjectionRateForMaxLiquidRate.HasValue
                    ? GasFlowRate.FromThousandStandardCubicFeetPerDay((double)result.Outputs.InjectionRateForMaxLiquidRate.Value).Amount : null,
                MeasurementAbbreviation = GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "PressureAtInjectionForMaximumProductionRate",
                DisplayValue = FormatValue(result.Outputs.MinimumFBHP, NULL_TEXT, digits, Pressure.PoundPerSquareInch.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.PinjForMaximumProductionRate]),
                Value = result.Outputs.MinimumFBHP.HasValue
                    ? Pressure.FromPoundsPerSquareInch((double)result.Outputs.MinimumFBHP.Value).Amount : null,
                MeasurementAbbreviation = Pressure.PoundPerSquareInch.Symbol,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "OptimalProductionRate",
                DisplayValue = FormatValue(result.Outputs.OptimumLiquidRate, NULL_TEXT, digits, LiquidFlowRate.BarrelPerDay.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.OptimalProductionRate]),
                Value = result.Outputs.OptimumLiquidRate.HasValue
                    ? LiquidFlowRate.FromBarrelsPerDay((double)result.Outputs.OptimumLiquidRate.Value).Amount : null,
                MeasurementAbbreviation = LiquidFlowRate.BarrelPerDay.Symbol,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "InjectionRateForOptimalProductionRate",
                DisplayValue = FormatValue(result.Outputs.InjectionRateForOptimumLiquidRate, NULL_TEXT, digits, GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.InjectionRateForOptimalProdRate]),
                Value = result.Outputs.InjectionRateForOptimumLiquidRate.HasValue
                    ? GasFlowRate
                        .FromThousandStandardCubicFeetPerDay((double)result.Outputs.InjectionRateForOptimumLiquidRate.Value).Amount
                        : null,
                MeasurementAbbreviation = GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "PressureAtInjectionForOptimalProductionRate",
                DisplayValue = FormatValue(result.Outputs.FBHPForOptimumLiquidRate, NULL_TEXT, digits, Pressure.PoundPerSquareInch.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.PinjForOptimalProductionRate]),
                Value = result.Outputs.FBHPForOptimumLiquidRate.HasValue
                    ? Pressure.FromPoundsPerSquareInch((double)result.Outputs.FBHPForOptimumLiquidRate.Value).Amount : null,
                MeasurementAbbreviation = Pressure.PoundPerSquareInch.Symbol,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "FlowingBottomholePressureAtInjectionDepth",
                DisplayValue = FormatValue(result.Outputs.FlowingBHPAtInjectionDepth, NULL_TEXT, digits, Pressure.PoundPerSquareInch.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.TesFBHPatInjectionDepthtDate]),
                Value = result.Outputs.FlowingBHPAtInjectionDepth.HasValue
                    ? Pressure.FromPoundsPerSquareInch((double)result.Outputs.FlowingBHPAtInjectionDepth.Value).Amount : null,
                MeasurementAbbreviation = Pressure.PoundPerSquareInch.Symbol,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "ModThornhillCraverValveCV",
                DisplayValue = FormatValue(result.Outputs.ValveCriticalVelocity, NULL_TEXT, digits, GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(_localePhrases.Get(6078, correlationId)),
                Value = result.Outputs.ValveCriticalVelocity.HasValue
                    ? GasFlowRate.FromThousandStandardCubicFeetPerDay((double)result.Outputs.ValveCriticalVelocity.Value).Amount
                    : null,
                MeasurementAbbreviation = GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "ColemanCriticalFlowRateSurface",
                DisplayValue = FormatValue(result.Outputs.TubingCriticalVelocity, NULL_TEXT, digits, GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.TubingCriticalVelocityCorrelation] + " " + phrases[(int)PhraseIDs.Surface]),
                Value = result.Outputs.TubingCriticalVelocity.HasValue
                    ? GasFlowRate.FromThousandStandardCubicFeetPerDay((double)result.Outputs.TubingCriticalVelocity.Value).Amount : null,
                MeasurementAbbreviation = GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol,
            });

            var injectionRateForCriticalFlowRateSurface = GetCriticalVelocityAtInjectionDepth(result, false);
            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "ColemanCriticalFlowRateInjection",
                DisplayValue = FormatValue(injectionRateForCriticalFlowRateSurface, NULL_TEXT, digits, GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.TubingCriticalVelocityCorrelation] + " " + phrases[(int)PhraseIDs.Injection]),
                Value = injectionRateForCriticalFlowRateSurface.HasValue
                    ? GasFlowRate.FromThousandStandardCubicFeetPerDay((double)injectionRateForCriticalFlowRateSurface.Value).Amount : null,
                MeasurementAbbreviation = GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "InjectionRateForCriticalFlowRateSurface",
                DisplayValue = FormatValue(result.Outputs.InjectionRateForTubingCriticalVelocity, NULL_TEXT, digits, GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.InjectionRateforCriticalFlowrate] + ": " + phrases[(int)PhraseIDs.Surface]),
                Value = result.Outputs.InjectionRateForTubingCriticalVelocity.HasValue
                    ? GasFlowRate
                        .FromThousandStandardCubicFeetPerDay((double)result.Outputs.InjectionRateForTubingCriticalVelocity.Value).Amount
                        : null,
                MeasurementAbbreviation = GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol,
            });

            var injectionRateForCriticalFlowRateInjection = GetCriticalVelocityAtInjectionDepth(result);
            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "InjectionRateForCriticalFlowRateInjection",
                DisplayValue = FormatValue(injectionRateForCriticalFlowRateInjection, NULL_TEXT, digits, GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.InjectionRateforCriticalFlowrate] + ": " + phrases[(int)PhraseIDs.Injection]),
                Value = injectionRateForCriticalFlowRateInjection.HasValue
                    ? GasFlowRate.FromThousandStandardCubicFeetPerDay((double)injectionRateForCriticalFlowRateInjection.Value).Amount
                    : null,
                MeasurementAbbreviation = GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol,
            });

            string processedDateTimeText;
            string resultMessage;
            if (result == null)
            {
                resultMessage = phrases[(int)PhraseIDs.Theselectedwelltesthasnotbeenprocessed];
                processedDateTimeText = string.Empty;
            }
            else if (string.IsNullOrEmpty(result.ResultMessageLocalized) && result.IsSuccess)
            {
                resultMessage = phrases[(int)PhraseIDs.Theselectedwelltestwasprocessedsuccessfully];
                processedDateTimeText = string.Format("({0} {1})", phrases[(int)PhraseIDs.Lastran],
                    result.ProcessedDateTime.ToString());
            }
            else if (result.Inputs?.AnalysisType == AnalysisType.Sensitivity &&
                     result.ProcessedDateTime.Equals(System.Data.SqlTypes.SqlDateTime.MinValue.Value) &&
                     string.IsNullOrWhiteSpace(result.ResultMessageLocalized))
            {
                resultMessage = result.ResultMessageLocalized;
                processedDateTimeText = string.Empty;
            }
            else
            {
                resultMessage = !string.IsNullOrEmpty(result.ResultMessageLocalized)
                    ? Interpolate(result.Outputs,
                        result.ResultMessageLocalized, correlationId)
                    : result.ResultMessageLocalized;

                processedDateTimeText = string.Format("({0} {1})", phrases[(int)PhraseIDs.Lastran],
                    result.ProcessedDateTime.ToString());
            }

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "ProcessedDateTimeText",
                DisplayValue = processedDateTimeText,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "XDiagAnalysis",
                DisplayValue = resultMessage,
            });

            if (result?.FlowControlDeviceStatuses != null)
            {
                if (result.FlowControlDeviceStatuses.ValveStatuses != null)
                {
                    responseValues.Valves =
                        MapValveStatusesToValues(result.FlowControlDeviceStatuses.ValveStatuses, glWellDetail, correlationId);
                }

                if (result.FlowControlDeviceStatuses.OrificeStatus != null
                    && !string.IsNullOrEmpty(result.FlowControlDeviceStatuses.OrificeStatus?.Id?.ToString()))
                {
                    responseValues.Valves.Add(
                        MapToFlowControlDeviceAnalysisValues(result.FlowControlDeviceStatuses.OrificeStatus, correlationId));
                }

                var count = responseValues.Valves.Count;
                for (var i = 0; i < count; i++)
                {
                    responseValues.Valves[i].Position = count - i;
                }
            }

            return responseValues;
        }

        private string Interpolate(GLAnalysisOutput analysisOutput, string phrase, string correlationId)
        {
            if (analysisOutput == null)
            {
                var analysisOutputParam = nameof(analysisOutput);
                throw new ArgumentNullException($"{analysisOutputParam} cannot be null");
            }

            if (phrase == null)
            {
                var phraseParam = nameof(phrase);
                throw new ArgumentNullException($"{phraseParam} cannot be null");
            }

            var digits = _commonService.GetSystemParameterNextGenSignificantDigits(correlationId);

            var result = phrase;

            foreach (var placeholder in EnhancedEnumBase.GetValues<GLAnalysisPhrasePlaceholder>())
            {
                if (placeholder == GLAnalysisPhrasePlaceholder.VerticalInjectionDepthFromValveAnalysis)
                {
                    var value = analysisOutput.ValveInjectionDepthEstimateResultData?.VerticalInjectionDepthFromValveAnalysis;
                    if (value.HasValue)
                    {
                        var convertedValue = MathUtility.RoundToSignificantDigits(value.Value, digits);
                        result = result.Replace(placeholder.Name, convertedValue.ToString());
                    } // value.HasValue is true
                } // placeholder is VerticalInjectionDepthFromValveAnalysis
                else if (placeholder == GLAnalysisPhrasePlaceholder.OptimumLiquidRate)
                {
                    var value = analysisOutput.OptimumLiquidRate;
                    if (value != null)
                    {
                        var convertedValue = MathUtility.RoundToSignificantDigits(value.Value, digits);
                        var unitPhrase = GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol;
                        result = result.Replace(placeholder.Name, $"{convertedValue} {unitPhrase}");
                    } // value is not null
                } // placeholder is OptimumLiquidRate
                else if (placeholder == GLAnalysisPhrasePlaceholder.InjectionRateForOptimumLiquidRate)
                {
                    var value = analysisOutput.InjectionRateForOptimumLiquidRate;
                    if (value != null)
                    {
                        var convertedValue = GasFlowRate.FromThousandStandardCubicFeetPerDay(
                            MathUtility.RoundToSignificantDigits(value.Value, digits)).ToString();
                        var unitPhrase = GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol;
                        result = result.Replace(placeholder.Name, $"{convertedValue} {unitPhrase}");
                    } // value is not null
                } // placeholder is InjectionRateForOptimumLiquidRate
            }

            return result;
        }

        private IList<FlowControlDeviceAnalysisValues> MapValveStatusesToValues(IList<ValveStatus> valveStatuses,
            GLWellDetailModel glWellDetail, string correlationId)
        {
            ApplyUnstableValveDisplayLogic(valveStatuses);

            return valveStatuses
                .Select(valve => MapToFlowControlDeviceAnalysisValues(valve, glWellDetail, correlationId))
                .OrderBy(valve => valve.Depth).ToList();
        }

        private void ApplyUnstableValveDisplayLogic(IList<ValveStatus> deviceStatuses)
        {
            // Prior to creating a view model, we clean up the statuses for display.

            //1. With the exception of the logic cases below, we do not display Injecting for unstable valves,
            //   though we consider them to be injecting in our calculations. So in preparation, set all the
            //   unstable valves to be not injecting.
            foreach (var device in deviceStatuses.Where(s => s.State == ValveState.Unstable))
            {
                device.IsInjectingGas = false;
            }

            var closedValveCount = deviceStatuses.Count(s => s.State == ValveState.Closed);
            var openNotInjectingCount =
                deviceStatuses.Count(s => s.State == ValveState.Open && s.IsInjectingGas == false);
            var unstableValveCount = deviceStatuses.Count(s => s.State == ValveState.Unstable);

            // 2. In a case where none of the valves in a well are injecting but there is more than one unstable valve, 
            //    the deepest of these valves should have its status be set to Open - Injecting. 
            //    The remaining unstable valves will still be in Unstable state.

            if (closedValveCount + openNotInjectingCount + unstableValveCount == deviceStatuses.Count &&
                unstableValveCount > 1)
            {
                var deepestUnstableValve = deviceStatuses
                    .OrderByDescending(s => s.FlowControlDevice.VerticalDepth)
                    .FirstOrDefault(s => s.State == ValveState.Unstable);

                if (deepestUnstableValve != null)
                {
                    deepestUnstableValve.State = ValveState.Open;
                    deepestUnstableValve.IsInjectingGas = true;
                }
            }

            // 3. In a case where the only non-closed valve in a well is an Unstable valve, that valve should 
            //    have its status set to Unstable - Injecting.
            if (closedValveCount + openNotInjectingCount + unstableValveCount == deviceStatuses.Count &&
                unstableValveCount == 1)
            {
                var onlyUnstableValve = deviceStatuses.First(s => s.State == ValveState.Unstable);
                onlyUnstableValve.IsInjectingGas = true;
            }
        }

        private FlowControlDeviceAnalysisValues MapToFlowControlDeviceAnalysisValues(OrificeStatus orificeStatus, string correlationId)
        {
            var digits = _commonService.GetSystemParameterNextGenSignificantDigits(correlationId);

            var viewModel = new FlowControlDeviceAnalysisValues();

            viewModel.ClosingPressureAtDepth = string.Empty;
            viewModel.OpeningPressureAtDepth = string.Empty;
            viewModel.InjectionPressureAtDepth = orificeStatus?.InjectionPressureAtDepth != null
                ? Pressure.FromPoundsPerSquareInch(MathUtility.RoundToSignificantDigits(
                    (double)orificeStatus.InjectionPressureAtDepth.Value, digits)).ToString()
                : string.Empty;
            viewModel.TubingCriticalVelocityAtDepth = GetTubingCriticalVelocityAtValveDepthMessage(
                orificeStatus?.TubingCriticalVelocityAtDepth, orificeStatus?.InjectionRateForTubingCriticalVelocity, correlationId);
            viewModel.Depth = orificeStatus?.Depth != null
                ? $"{MathUtility.RoundToSignificantDigits(orificeStatus.Depth.Value, digits)}"
                : string.Empty;
            if (orificeStatus?.State != null && orificeStatus?.IsInjectingGas != null)
            {
                viewModel.Status = orificeStatus.State.Name;
                if (orificeStatus.IsInjectingGas.Value)
                {
                    viewModel.Status += " - " + phrases[(int)PhraseIDs.Injecting];
                }
                else
                {
                    viewModel.Status += " - " + phrases[(int)PhraseIDs.NotInjecting];
                }
            }

            viewModel.Description = phrases[(int)PhraseIDs.Orifice];

            return viewModel;
        }

        private FlowControlDeviceAnalysisValues MapToFlowControlDeviceAnalysisValues(ValveStatus valveStatus,
            GLWellDetailModel glWellDetail, string correlationId)
        {
            var digits = _commonService.GetSystemParameterNextGenSignificantDigits(correlationId);

            var viewModel = new FlowControlDeviceAnalysisValues();

            var wellValve = valveStatus?.FlowControlDevice;

            var openingPressureAtDepth = glWellDetail.ValveConfigurationOption != ValveConfigurationOption.UseDesignData.Key
                ? valveStatus?.OpeningPressureAtDepth
                : wellValve?.OpeningPressureAtDepth;
            var closingPressureAtDepth = glWellDetail.ValveConfigurationOption != ValveConfigurationOption.UseDesignData.Key
                ? valveStatus?.ClosingPressureAtDepth
                : wellValve?.ClosingPressureAtDepth;

            viewModel.ClosingPressureAtDepth = closingPressureAtDepth != null
                ? Pressure.FromPoundsPerSquareInch(MathUtility.RoundToSignificantDigits(
                    (double)closingPressureAtDepth.Value, digits)).ToString()
                : string.Empty;
            viewModel.OpeningPressureAtDepth = openingPressureAtDepth != null
                ? Pressure.FromPoundsPerSquareInch(MathUtility.RoundToSignificantDigits(
                    (double)openingPressureAtDepth.Value, digits)).ToString()
                : string.Empty;
            viewModel.InjectionPressureAtDepth = valveStatus?.InjectionPressureAtDepth != null
                ? Pressure.FromPoundsPerSquareInch(MathUtility.RoundToSignificantDigits(
                    (double)valveStatus.InjectionPressureAtDepth.Value, digits)).ToString()
                : string.Empty;
            viewModel.TubingCriticalVelocityAtDepth = GetTubingCriticalVelocityAtValveDepthMessage(
                valveStatus?.TubingCriticalVelocityAtDepth, valveStatus?.InjectionRateForTubingCriticalVelocity, correlationId);
            viewModel.Depth = (valveStatus?.Depth != null)
                ? $"{MathUtility.RoundToSignificantDigits(valveStatus.Depth.Value, digits)}"
                : string.Empty;

            if (valveStatus?.State != null)
            {
                viewModel.IsOpen = valveStatus.State == ValveState.Open;
                viewModel.IsInjecting = valveStatus.IsInjectingGas ?? false;
                viewModel.Status = valveStatus.State.Name;
                if (valveStatus.State == ValveState.Open && valveStatus.IsInjectingGas.HasValue)
                {
                    if (valveStatus.IsInjectingGas.Value)
                    {
                        //viewModel.Color = _defaultColors.OpenInjectingValveColor;
                        viewModel.Status += " - " + phrases[(int)PhraseIDs.Injecting];
                    }
                    else
                    {
                        //viewModel.Color = _defaultColors.OpenNotInjectingValveColor;
                        viewModel.Status += " - " + phrases[(int)PhraseIDs.NotInjecting];
                    }
                }
                else if (valveStatus.State == ValveState.Unstable)
                {
                    //viewModel.Color = _defaultColors.UnstableValveColor;

                    if (valveStatus.IsInjectingGas.Value)
                    {
                        viewModel.Status += " - " + phrases[(int)PhraseIDs.Injecting];
                    }
                }
                else
                {
                    //viewModel.Color = _defaultColors.DefaultValveColor;
                }
            }

            viewModel.Description = wellValve.Valve.Description;

            return viewModel;
        }

        private string GetTubingCriticalVelocityAtValveDepthMessage(float? tubingCriticalVelocity,
            float? injectionRateForTubingCriticalVelocity, string correlationId)
        {
            var digits = _commonService.GetSystemParameterNextGenSignificantDigits(correlationId);

            var message = "";

            if (tubingCriticalVelocity != null)
            {
                message += MathUtility.RoundToSignificantDigits(tubingCriticalVelocity.Value, digits);
            }

            if (injectionRateForTubingCriticalVelocity != null)
            {
                message += $" ({MathUtility.RoundToSignificantDigits(injectionRateForTubingCriticalVelocity.Value, digits)})";
            }

            if (message.Length != 0)
            {
                message += " " + "mscf/d";
            }

            return message;
        }

        private string GetGasRatePhrase(string gasInTest)
        {
            var stringTemplate = "{0} ({1})";

            if (string.IsNullOrWhiteSpace(gasInTest) == false && int.TryParse(gasInTest, out var gasInTextInt) &&
                Convert.ToBoolean(gasInTextInt))
            {
                return string.Format(stringTemplate, phrases[(int)PhraseIDs.GasRate],
                    phrases[(int)PhraseIDs.Inclinj]);
            }

            return string.Format(stringTemplate, phrases[(int)PhraseIDs.GasRate],
                phrases[(int)PhraseIDs.Exclinj]);
        }

        /// <summary>
        /// Processes the provided Get GLAnalysis Curve Coordinate Response Results and generates response based on that data.
        /// </summary>
        /// <param name="analysisResultCurve">Analysis Result Curve</param>
        /// <returns>The <seealso cref="IList{GLAnalysisCurveCoordinateData}"/></returns>
        private IList<GLAnalysisCurveCoordinateData> GetGLAnalysisCurveCoordinateResponseResults
             (IList<AnalysisResultCurvesModel> analysisResultCurve)
        {
            IList<GLAnalysisCurveCoordinateData> listGLAnalysisCurveCoordinateData = new List<GLAnalysisCurveCoordinateData>();

            foreach (var data in analysisResultCurve)
            {
                GLAnalysisCurveCoordinateData gLAnalysisCurveCoordinateData = new GLAnalysisCurveCoordinateData();
                gLAnalysisCurveCoordinateData.Id = data.AnalysisResultCurveID;
                gLAnalysisCurveCoordinateData.CurveTypeId = data.CurveTypesID;
                gLAnalysisCurveCoordinateData.Name = string.Empty;
                gLAnalysisCurveCoordinateData.DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(data.Name);
                gLAnalysisCurveCoordinateData.CoordinatesOutput = data
                    .Coordinates
                    .Select(x => new CoordinatesData<float>
                    {
                        X = x.X,
                        Y = x.Y,
                    }).ToList();
                listGLAnalysisCurveCoordinateData.Add(gLAnalysisCurveCoordinateData);
            }

            return listGLAnalysisCurveCoordinateData;
        }

        private IList<SurveyData> RetrieveSurveyCurvesRange(IList<GlAnalysisSurveyCurveCoordinateDataModel> entities, string correlationId)
        {
            var analysisCurves = new List<SurveyData>();

            int analysisResultId;
            var application = IndustryApplication.GasArtificialLift;

            if (entities != null)
            {
                foreach (var entity in entities)
                {
                    analysisResultId = entity.Id;
                    var analysisResultCurves = _analysisCurves.Fetch(analysisResultId, application.Key, new List<int>()
                    {
                        SurveyCurveType.PressureCurve.Key,
                        SurveyCurveType.TemperatureCurve.Key,
                    }, correlationId);

                    if (analysisResultCurves != null)
                    {
                        foreach (var analysisResultCurve in analysisResultCurves)
                        {
                            var curve = CreateAnalysisCurve(analysisResultCurve.Id, analysisResultCurve.CurveTypeId, application);
                            var curveCoordinates = _gLAnalysisGetCurveCoordinate.FetchCurveCoordinates((int)curve.Id, correlationId);

                            var curveCoordinatesData = new List<CurveCoordinate>();

                            foreach (var curveCoordinate in curveCoordinates)
                            {
                                var newCoordinate = new CurveCoordinate(curveCoordinate.Id);

                                newCoordinate.Coordinate = new Coordinate<double, double>(curveCoordinate.X, curveCoordinate.Y);

                                curveCoordinatesData.Add(newCoordinate);
                            }

                            curve.Curve = curveCoordinatesData;

                            analysisCurves.Add(MapToSurveyCurveCoordinatesDomain(entity, curve));
                        }// foreach analysisResultCurve
                    }// if analysisResultCurves is not null
                }// foreach entity
            }// if entities is null

            return analysisCurves;
        }

        private AnalysisCurve CreateAnalysisCurve(int curveId, int curveTypeId, IndustryApplication industryApplication)
        {
            var curveIsIPR = false;
            var curveIsSurveyCurve = false;

            if (EnhancedEnumBase.IsDefined<IPRCurveType>(curveTypeId))
            {
                curveIsIPR = true;
            }

            if (EnhancedEnumBase.IsDefined<SurveyCurveType>(curveTypeId))
            {
                curveIsSurveyCurve = true;
            }

            if (curveIsIPR)
            {
                return new IPRAnalysisCurve(curveId, EnhancedEnumBase.GetValue<IPRCurveType>(curveTypeId));
            }
            else if (curveIsSurveyCurve)
            {
                return new SurveyAnalysisCurve(curveId, EnhancedEnumBase.GetValue<SurveyCurveType>(curveTypeId));
            }
            else if (industryApplication == IndustryApplication.ESPArtificialLift)
            {
                return new ESPAnalysisCurve(curveId, EnhancedEnumBase.GetValue<ESPCurveType>(curveTypeId));
            }
            else if (industryApplication == IndustryApplication.GasArtificialLift
                     || industryApplication == IndustryApplication.PlungerAssistedGasArtificialLift)
            {
                return new GLAnalysisCurve(curveId, EnhancedEnumBase.GetValue<GLCurveType>(curveTypeId));
            }

            return null;
        }

        private SurveyData MapToSurveyCurveCoordinatesDomain(GlAnalysisSurveyCurveCoordinateDataModel entity, AnalysisCurve curve)
        {
            var model = new SurveyData(entity.Id)
            {
                SurveyDate = entity.SurveyDate,
                SurveyCurve = curve,
            };

            return model;
        }

        /// <summary>
        /// Processes the provided request and generates the ipr curve co-ordinate response based on that data.
        /// </summary>
        /// <param name="data">The <seealso cref="GLAnalysisCurveCoordinateInput"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The response <seealso cref="CurveCoordinateDataOutput"/> object.</returns>
        private GLAnalysisCurveCoordinateData GetIPRCurveCoordinates(WithCorrelationId<GLAnalysisCurveCoordinateInput> data)
        {
            var logger = _loggerFactory.Create(LoggingModel.GLAnalysis);

            GLAnalysisCurveCoordinateData response;

            var request = data.Value;

            var glIPRCurveCoordinates = _gLAnalysisGetCurveCoordinate.GetGLAnalysisIPRCurveCoordinate(request.AssetId, request.TestDate, data.CorrelationId);

            if (glIPRCurveCoordinates == null || glIPRCurveCoordinates.AnalysisResultCurvesEntities == null
                || glIPRCurveCoordinates.IPRAnalysisResultEntity == null || glIPRCurveCoordinates.NodeMasterData == null)
            {
                logger.WriteCId(Level.Info, $"{glIPRCurveCoordinates}, " +
                    $"cannot get gas lift analysis ipr curve coordinates.", data.CorrelationId);

                return null;
            }
            else
            {
                response = GetIPRCurveCoordinatesReponseData(glIPRCurveCoordinates, data.CorrelationId);
            }

            return response;
        }

        private GLAnalysisCurveCoordinateData GetIPRCurveCoordinatesReponseData(IPRAnalysisCurveCoordinateModel iprAnalysisCurveCoordinateModel, string correlationId)
        {
            var curveCoordinateData = new GLAnalysisCurveCoordinateData();

            var iprAnalysisResultModel = iprAnalysisCurveCoordinateModel.IPRAnalysisResultEntity;

            var analysisResultCurves = iprAnalysisCurveCoordinateModel.AnalysisResultCurvesEntities.ToList();

            var application = iprAnalysisCurveCoordinateModel.NodeMasterData.ApplicationId.HasValue ? EnhancedEnumBase.GetValue<IndustryApplication>
                (iprAnalysisCurveCoordinateModel.NodeMasterData.ApplicationId.Value) : IndustryApplication.None;

            if (analysisResultCurves != null)
            {
                var analysisCurves = new List<AnalysisCurve>();
                foreach (var analysisResultCurve in analysisResultCurves)
                {
                    var curve = CreateAnalysisCurve(analysisResultCurve.AnalysisResultCurveID, analysisResultCurve.CurveTypesID, application);

                    var curveCoordinate = _gLAnalysisGetCurveCoordinate.FetchCurveCoordinates((int)curve.Id, correlationId);

                    var coordinates = new List<CurveCoordinate>();
                    foreach (var coordinate in curveCoordinate)
                    {
                        var newCoordinate = new CurveCoordinate(coordinate.Id);
                        newCoordinate.Coordinate = new Coordinate<double, double>(coordinate.X, coordinate.Y);

                        coordinates.Add(newCoordinate);
                    }
                    curve.Curve = coordinates;

                    analysisCurves.Add(curve);
                } // foreach (var analysisResultCurve in analysisResultCurves)

                var analysisCurve = analysisCurves.FirstOrDefault(c => (EnhancedEnumBase)c.CurveType == IPRCurveType.GetCurveTypeFor(application));

                var responseValues = new GLAnalysisCurveCoordinateData();
                if (analysisCurve != null)
                {
                    curveCoordinateData = new GLAnalysisCurveCoordinateData()
                    {
                        Id = iprAnalysisResultModel.Id,
                        Name = "IPRCurve",
                        CurveTypeId = analysisCurve.GetCurveTypeId(),
                        DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(EnhancedEnumBase.GetValue<IPRCurveType>(analysisCurve.GetCurveTypeId())),
                        CoordinatesOutput = analysisCurve.GetCurveCoordinateList().Select(c => new CoordinatesData<float>()
                        {
                            X = (float)c.XValue,
                            Y = (float)c.YValue,
                        }).ToList(),
                    };
                } // if (analysisCurve != null)
            } //if (analysisResultCurves != null)

            return curveCoordinateData;
        }

        private void AddStaticFluidCurve(WithCorrelationId<GLAnalysisCurveCoordinateInput> data, ref GLAnalysisCurveCoordinateDataOutput response)
        {
            var staticFluidCurves = GetStaticFluidCurve(data);

            if (staticFluidCurves?.ReservoirFluidCurve?.Count > 0)
            {
                response.Values.Add(new GLAnalysisCurveCoordinateData()
                {
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(GLCurveType.ReservoirFluidCurve.Name),
                    CoordinatesOutput = staticFluidCurves.ReservoirFluidCurve
                        .Select(x => new CoordinatesData<float>
                        {
                            X = (float)x.XValue,
                            Y = (float)x.YValue,
                        }).ToList(),
                    CurveTypeId = GLCurveType.ReservoirFluidCurve.Key,
                    Id = GLCurveType.ReservoirFluidCurve.Key,
                    Name = "ReservoirFluidCurve"
                });
            }

            if (staticFluidCurves?.KillFluidCurve?.Count > 0)
            {
                response.Values.Add(new GLAnalysisCurveCoordinateData()
                {
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(GLCurveType.KillFluidCurve.Name),
                    CoordinatesOutput = staticFluidCurves.KillFluidCurve
                        .Select(x => new CoordinatesData<float>
                        {
                            X = (float)x.XValue,
                            Y = (float)x.YValue,
                        }).ToList(),
                    CurveTypeId = GLCurveType.KillFluidCurve.Key,
                    Id = GLCurveType.KillFluidCurve.Key,
                    Name = "KillFluidCurve"
                });
            }
        }

        private StaticFluidCurves GetStaticFluidCurve(WithCorrelationId<GLAnalysisCurveCoordinateInput> input)
        {
            var curves = new StaticFluidCurves();
            var result = _gLAnalysisGetCurveCoordinate.GetDataForStaticFluidCurve(input.Value.AssetId, input.CorrelationId);

            if (result == null || result.Perforations.Count == 0)
            {
                return curves;
            }

            var killFluidCurve = new List<Coordinate<double, double>>();
            var reservoirFluidCurve = new List<Coordinate<double, double>>();
            var wellDepth = CalculatePerforationDepth(result.Perforations)?.Amount ?? result.ProductionDepth;

            if (result.ReservoirPressure != null && wellDepth != null)
            {
                var bottomholeCoordinate = new Coordinate<double, double>(result.ReservoirPressure.Value, wellDepth.Value);
                killFluidCurve.Add(bottomholeCoordinate);
                reservoirFluidCurve.Add(bottomholeCoordinate);
            }

            if (result.KillFluidLevel != null)
            {
                var fluidLevelCoordinate = new Coordinate<double, double>(0, result.KillFluidLevel.Value);
                killFluidCurve.Add(fluidLevelCoordinate);
                curves.KillFluidCurve = killFluidCurve;
            }

            if (result.ReservoirFluidLevel != null)
            {
                var fluidLevelCoordinate = new Coordinate<double, double>(0, result.ReservoirFluidLevel.Value);
                reservoirFluidCurve.Add(fluidLevelCoordinate);
                curves.ReservoirFluidCurve = reservoirFluidCurve;
            }

            return curves;
        }

        private Quantity<Length> CalculatePerforationDepth(IList<PerforationModel> perforations)
        {
            var result = perforations.Average(x => x.TopDepth + (0.5 * x.Length));

            return Length.FromFeet(result);
        }

        private class StaticFluidCurves
        {

            /// <summary>
            /// Gets and sets the KillFluidCurve.
            /// </summary>
            public IList<Coordinate<double, double>> KillFluidCurve { get; set; }

            /// <summary>
            /// Gets and sets the KillFluidCurve.
            /// </summary>
            public IList<Coordinate<double, double>> ReservoirFluidCurve { get; set; }

        }

        private void AddFBHPCurves(WithCorrelationId<GLAnalysisCurveCoordinateInput> data,
            ref GLAnalysisCurveCoordinateDataOutput response)
        {
            var curveSet = new AnalysisCurveSet()
            {
                AnalysisResultId = data.Value.AnalysisResultId,
                AnalysisResultSource = AnalysisResultSource.GasLift,
                CurveSetType = CurveSetType.FBHP,
                Curves = new List<AnalysisCurveSetMemberBase>(),
            };
            var glrCurveSet = _analysisCurvesSet.GetAnalysisCurvesSet(data.Value.AnalysisResultId, AnalysisResultSource.GasLift.Key, CurveSetType.FBHP.Key, data.CorrelationId);
            if (glrCurveSet != null)
            {
                var entityHashset = glrCurveSet.ToHashSet();
                curveSet.CurveSetId = entityHashset.Select(x => x.AnalysisCurveSetDataModels.CurveSetId).FirstOrDefault();
                var curveSetMemberIds = entityHashset.Where(x => x.AnalysisCurveSetMemberModels != null)
                    .Select(x => x.AnalysisCurveSetMemberModels?.CurveSetMemberId).Distinct();
                foreach (var curveSetMemberId in curveSetMemberIds)
                {
                    var curveSetMember = AnalysisCurveSetMemberUtility.GetAnalysisCurveSetMember(CurveSetType.FBHP);
                    curveSetMember.CurveSetMemberId = curveSetMemberId;
                    var curves = entityHashset.Where(x => x.AnalysisCurveSetMemberModels.CurveSetMemberId == curveSetMemberId
                            && x.CurveSetCoordinatesModels != null).OrderBy(x => x.CurveSetCoordinatesModels.X)
                        .Select(x => x.CurveSetCoordinatesModels);
                    var newCurves = new List<CurveCoordinate>();
                    foreach (var coordinate in curves)
                    {
                        var newCurve = new CurveCoordinate(coordinate.Id);
                        newCurve.Coordinate = new Coordinate<double, double>(coordinate.X,
                            coordinate.Y);
                        newCurves.Add(newCurve);
                    }
                    curveSetMember.Curve = newCurves;
                    curveSet.Curves.Add(curveSetMember);
                }
                if (curveSet != null)
                {
                    var curveSetMemberId = curveSet.Curves
                        .Where(x => x.CurveSetMemberId.HasValue)
                        .Select(x => x.CurveSetMemberId.Value).ToList();
                    var annotationEntities = _analysisCurvesSet.GetGLRCurveSetAnnotations(curveSetMemberId, data.CorrelationId);
                    MapToDomain(annotationEntities, curveSet);
                }
                foreach (var curve in curveSet.Curves)
                {
                    var value = new GLAnalysisCurveCoordinateData()
                    {
                        Id = curve.CurveSetMemberId.Value,
                        CurveTypeId = curveSet.CurveSetType.Key,
                        Name = curveSet.CurveSetType.Name,
                        DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower($"{curveSet.CurveSetType.Name} {(curve.AnnotationData as GasLiquidRatioCurveAnnotation)?.GasLiquidRatio}"),
                        CoordinatesOutput = curve.GetCurveCoordinateList()
                            .Select(x => new CoordinatesData<float>()
                            {
                                X = (float)x.XValue,
                                Y = (float)x.YValue,
                            }).ToList(),
                    };
                    response.Values.Add(value);
                }
            }
        }
        private void MapToDomain(IList<GasLiquidRatioCurveAnnotationModel> annotationEntities, AnalysisCurveSet curveSet)
        {
            if (annotationEntities == null || annotationEntities.Count == 0 || curveSet == null)
            {
                return;
            }
            foreach (var entity in annotationEntities)
            {
                var annotation = new GasLiquidRatioCurveAnnotation()
                {
                    GasLiquidRatio = entity.GasLiquidRatio,
                    IsPrimaryCurve = entity.IsPrimaryCurve,
                };
                var curve = curveSet.Curves.FirstOrDefault(c => c.CurveSetMemberId == entity.CurveSetMemberId);
                if (curve != null)
                {
                    curve.AnnotationData = annotation;
                }
            }
        }

        private void AddOperatingPoint(GLAnalysisResult analysisData, ref GLAnalysisCurveCoordinateDataOutput response)
        {
            if (GenerateFBHPCoordinate(analysisData, out var fbhpCoordinate) == false || fbhpCoordinate.HasValue == false)
            {
                return;
            }

            response.Values.Add(new GLAnalysisCurveCoordinateData()
            {
                Id = -1,
                CurveTypeId = -3,
                DisplayName = "Pressure operating point",
                Name = "PressureOperatingPoint",
                CoordinatesOutput = new List<CoordinatesData<float>>()
                {
                    new CoordinatesData<float>()
                    {
                        X = (float)fbhpCoordinate.Value.XValue,
                        Y = (float)fbhpCoordinate.Value.YValue,
                    }
                }
            });

            if (GenerateFlowRateCoordinate(analysisData, out var flowRateCoordinate) == false ||
                flowRateCoordinate.HasValue == false)
            {
                return;
            }

            response.Values.Add(new GLAnalysisCurveCoordinateData()
            {
                Id = -1,
                CurveTypeId = -2,
                DisplayName = "Production operating point",
                Name = "ProductionOperatingPoint",
                CoordinatesOutput = new List<CoordinatesData<float>>()
                {
                    new CoordinatesData<float>()
                    {
                        X = (float)flowRateCoordinate.Value.XValue,
                        Y = (float)flowRateCoordinate.Value.YValue,
                    }
                }
            });
        }

        private void AddColemanCriticalFlowRate(double max, GLAnalysisResult analysisData, ref GLAnalysisCurveCoordinateDataOutput response)
        {
            var tubingCriticalVelocityCurve = GetCriticalVelocityCurve(
                analysisData.Outputs.InjectionRateForTubingCriticalVelocity, max);

            if (tubingCriticalVelocityCurve?.Count > 0)
            {
                response.Values.Add(new GLAnalysisCurveCoordinateData()
                {
                    Id = -1,
                    CurveTypeId = -1,
                    DisplayName = "Coleman crit flowrate at surface",
                    Name = "TubingCriticalVelocityCurve",
                    CoordinatesOutput = tubingCriticalVelocityCurve.Select(x => new CoordinatesData<float>()
                    {
                        X = (float)x.XValue,
                        Y = (float)x.YValue,
                    }).ToList(),
                });
            }

            var tubingCriticalVelocityAtInjectionDepthCurve = GetCriticalVelocityCurve(
                GetCriticalVelocityAtInjectionDepth(analysisData), max);

            if (tubingCriticalVelocityAtInjectionDepthCurve?.Count > 0)
            {
                response.Values.Add(new GLAnalysisCurveCoordinateData()
                {
                    Id = -1,
                    CurveTypeId = -4,
                    DisplayName = "Coleman crit flowrate at inj. depth",
                    Name = "TubingCriticalVelocityCurveAtInjectionDepth",
                    CoordinatesOutput = tubingCriticalVelocityAtInjectionDepthCurve.Select(x => new CoordinatesData<float>()
                    {
                        X = (float)x.XValue,
                        Y = (float)x.YValue,
                    }).ToList(),
                });
            }
        }

        private void AddThornhillCraverValve(double max, GLAnalysisResult analysisData, ref GLAnalysisCurveCoordinateDataOutput response)
        {
            var valveCriticalVelocityCurve = GetCriticalVelocityCurve(
                analysisData?.Outputs?.ValveCriticalVelocity, max);

            if (valveCriticalVelocityCurve?.Count > 0)
            {
                response.Values.Add(new GLAnalysisCurveCoordinateData()
                {
                    Id = -1,
                    CurveTypeId = -5,
                    DisplayName = "Mod* thornhill-craver valve CV",
                    Name = "ValveCriticalVelocityCurve",
                    CoordinatesOutput = valveCriticalVelocityCurve.Select(x => new CoordinatesData<float>()
                    {
                        X = (float)x.XValue,
                        Y = (float)x.YValue,
                    }).ToList(),
                });
            }
        }

        private void GetMinAndMax(IList<CoordinatesData<float>> curve, ref double min, ref double max,
    bool x = false)
        {
            if (curve != null)
            {
                foreach (var coord in curve)
                {
                    min = x ? Math.Min(min, coord.X) : Math.Min(min, coord.Y);
                    max = x ? Math.Max(max, coord.X) : Math.Max(max, coord.Y);
                }
            }
        }

        private IList<Coordinate<double, double>> GetCriticalVelocityCurve(float? criticalVelocity,
            double productionCurveMax)
        {
            var criticalVelocityCurve = new List<Coordinate<double, double>>();
            const double minCurveXValue = 0.01; //Necessary minimum value to make sure line displays on chart

            if (criticalVelocity != null)
            {
                double injectionRateX = criticalVelocity.Value;
                injectionRateX = injectionRateX <= minCurveXValue ? minCurveXValue : injectionRateX;

                var productionRateY = productionCurveMax * FLOWRATE_AXIS_MAX_MULTIPLIER;

                var bottomCoordinate = new Coordinate<double, double>(x: injectionRateX, y: 0);
                var topCoordinate = new Coordinate<double, double>(x: injectionRateX, y: productionRateY);

                criticalVelocityCurve.Add(bottomCoordinate);
                criticalVelocityCurve.Add(topCoordinate);
            }

            return criticalVelocityCurve;
        }

        private bool GenerateFlowRateCoordinate(GLAnalysisResult analysis, out Coordinate<double, double>? coordinate)
        {
            coordinate = null;
            IList<string> requirements = new List<string>();

            if (analysis?.Outputs?.GrossRate == null)
            {
                requirements.Add(String.Format(phrases[(int)PhraseIDs.ProductionRate],
                    String.Empty, phrases[(int)PhraseIDs.WaterRate]));
            }

            if ((analysis?.Inputs?.GasInjectionRate == null))
            {
                requirements.Add(phrases[(int)PhraseIDs.InjectionRate]);
            }

            if (requirements.Count != 0 || (analysis?.Outputs?.GrossRate) == null || (analysis.Inputs?.GasInjectionRate) == null)
            {
                return false;
            }

            var grossRate = analysis.Outputs.GrossRate.Value;

            var gasInjectionRate = analysis.Inputs.GasInjectionRate.Value;
            coordinate = new Coordinate<double, double>(gasInjectionRate, grossRate);

            return true;
        }

        private bool GenerateFBHPCoordinate(GLAnalysisResult analysis, out Coordinate<double, double>? coordinate)
        {
            coordinate = null;
            IList<string> requirements = new List<string>();

            if (analysis?.Outputs?.FlowingBHPAtInjectionDepth == null)
            {
                requirements.Add(String.Format(phrases[(int)PhraseIDs.TesFBHPatInjectionDepthtDate]));
            }

            if ((analysis?.Inputs?.GasInjectionRate == null))
            {
                requirements.Add(phrases[(int)PhraseIDs.InjectionRate]);
            }

            if (requirements.Count != 0 || (analysis?.Outputs?.FlowingBHPAtInjectionDepth) == null || (analysis?.Inputs?.GasInjectionRate) == null)
            {
                return false;
            }

            var flowingBHP = analysis.Outputs.FlowingBHPAtInjectionDepth.Value;

            var gasInjectionRate = analysis.Inputs.GasInjectionRate.Value;

            coordinate = new Coordinate<double, double>(gasInjectionRate, flowingBHP);

            return true;
        }

        private float? GetCriticalVelocityAtInjectionDepth(GLAnalysisResult analysis, bool getInjectionRateForCV = true)
        {
            float? result = null;
            ValveStatus injectingValveStatus = null;
            OrificeStatus injectingOrificeStatus = null;

            if (analysis?.FlowControlDeviceStatuses?.ValveStatuses != null)
            {
                injectingValveStatus = analysis.FlowControlDeviceStatuses.ValveStatuses
                    .OrderByDescending(s => s.FlowControlDevice.VerticalDepth)
                    .FirstOrDefault(s => s.IsInjectingGas == true);
            }

            if (analysis?.FlowControlDeviceStatuses?.OrificeStatus != null)
            {
                if (analysis.FlowControlDeviceStatuses.OrificeStatus.IsInjectingGas == true)
                {
                    injectingOrificeStatus = analysis.FlowControlDeviceStatuses.OrificeStatus;
                }
            }

            if (getInjectionRateForCV)
            {
                result = injectingOrificeStatus?.InjectionRateForTubingCriticalVelocity ??
                    injectingValveStatus?.InjectionRateForTubingCriticalVelocity;
            }
            else
            {
                result = injectingOrificeStatus?.TubingCriticalVelocityAtDepth ??
                    injectingValveStatus?.TubingCriticalVelocityAtDepth;
            }

            return result;
        }

        private GLAnalysisWellboreData GetGLAnalysisWellboreData(GLAnalysisResult analysisResult, Guid assetId, string correlationId)
        {
            var wellboreData = _glAnalysisService.GetWellboreData(assetId, correlationId);

            if (wellboreData == null)
            {
                return null;
            }

            var wellDepth = wellboreData?.Perforations.Count > 0 ? CalculatePerforationDepth(wellboreData.Perforations) : wellboreData?.ProductionDepth == null ?
                null : Length.FromFeet((double)wellboreData.ProductionDepth);

            var packerDepth = wellboreData?.PackerDepth == null ? null : Length.FromFeet((double)wellboreData.PackerDepth);

            var sumOfTubingLengths = wellboreData?.Tubings.Sum(x => x.Length);
            var tubingDepth = sumOfTubingLengths == null ? null : Length.FromFeet((double)sumOfTubingLengths);

            var gasInjectionDepth = analysisResult?.Outputs?.ValveInjectionDepthEstimateResultData?.VerticalInjectionDepthFromValveAnalysis == null ?
                null : Length.FromFeet(analysisResult.Outputs.ValveInjectionDepthEstimateResultData.VerticalInjectionDepthFromValveAnalysis.Value);

            gasInjectionDepth ??= (analysisResult.Inputs.GasInjectionDepth == null ? null : Length.FromFeet((double)analysisResult.Inputs.GasInjectionDepth));

            var hasPacker = wellboreData?.HasPacker ?? false;

            // TODO: convert all fields using user configuration

            var glAnalysisWellboreData = new GLAnalysisWellboreData
            {
                WellDepth = wellDepth,
                PackerDepth = packerDepth,
                GasInjectionDepth = gasInjectionDepth,
                HasPacker = hasPacker,
                TubingDepth = tubingDepth,
            };

            return glAnalysisWellboreData;
        }

        private GLAnalysisWellboreViewData GetGLAnalysisWellboreViewData(GLAnalysisWellboreData wellboreData, IList<ValveStatus> flowControlDevices, bool useTVD, string correlationId)
        {
            var phraseIds = Enum.GetValues<PhraseIDs>().Cast<int>().ToArray();
            phrases = _localePhrases.GetAll(correlationId, phraseIds);

            const float DEFAULT_CASING_HEIGHT = 10;
            const float DEFAULT_TOP_HEIGHT = 160;
            const float DEFAULT_BOTTOM_HEIGHT = 110;

            var errorMessage = string.Empty;

            if (wellboreData == null)
            {
                errorMessage = "Wellbore ";
                errorMessage += phrases[(int)PhraseIDs.WellboreDiagramMissingRequirements];
                return new GLAnalysisWellboreViewData
                {
                    ErrorMessage = errorMessage,
                };
            }

            if (wellboreData.WellDepth == null || wellboreData.GasInjectionDepth == null)
            {
                errorMessage = "";

                if (wellboreData.WellDepth == null)
                {
                    errorMessage += phrases[(int)PhraseIDs.VerticalWellDepth];
                }

                if (wellboreData.GasInjectionDepth == null)
                {
                    errorMessage += errorMessage.Length > 0 ? " & " : "";
                    errorMessage += phrases[(int)PhraseIDs.VerticalInjectionDepth];
                }

                errorMessage += phrases[(int)PhraseIDs.WellboreDiagramMissingRequirements];

                return new GLAnalysisWellboreViewData
                {
                    ErrorMessage = errorMessage,
                };
            }

            var topHeight = DEFAULT_TOP_HEIGHT;
            var bottomHeight = DEFAULT_BOTTOM_HEIGHT;
            var casingHeight = DEFAULT_CASING_HEIGHT;
            var wellboreScreenHeight = topHeight + bottomHeight + casingHeight;
            var wellboreHeightToScreenRatio = wellboreScreenHeight / wellboreData.WellDepth.Amount;
            var injectionScreenPoint = wellboreData.GasInjectionDepth.Amount * wellboreHeightToScreenRatio;
            var maxDeviceDepth = flowControlDevices.Max(x => x.FlowControlDevice.GetDepth(useTVD));

            if (maxDeviceDepth != null)
            {
                var deviceIsTooDeep = maxDeviceDepth.Value / wellboreData.WellDepth.Amount > (topHeight + bottomHeight) / wellboreScreenHeight;
                var injectionIsTooDeep = wellboreData.GasInjectionDepth.Amount / wellboreData.WellDepth.Amount > (topHeight + bottomHeight) / wellboreScreenHeight;
                var deviceCasingHeight = wellboreScreenHeight - maxDeviceDepth.Value * wellboreHeightToScreenRatio;
                var injectionCasingHeight = wellboreScreenHeight - wellboreData.GasInjectionDepth.Amount * wellboreHeightToScreenRatio;

                if (deviceIsTooDeep && injectionIsTooDeep)
                {
                    casingHeight = deviceCasingHeight < injectionCasingHeight ? (float)deviceCasingHeight : (float)injectionCasingHeight;
                }
                else if (deviceIsTooDeep)
                {
                    casingHeight = (float)deviceCasingHeight;
                }
                else if (injectionIsTooDeep)
                {
                    casingHeight = (float)injectionCasingHeight;
                }
            }

            if (casingHeight <= 0)
            {
                errorMessage = phrases[(int)PhraseIDs.WellboreDiagramMismatchedValues];
                return new GLAnalysisWellboreViewData
                {
                    ErrorMessage = errorMessage,
                };
            }

            var casingFluidColumn = casingHeight;
            var topFluidColumn = (float)injectionScreenPoint;

            if (wellboreScreenHeight - topFluidColumn - casingHeight < 0)
            {
                errorMessage = phrases[(int)PhraseIDs.WellboreDiagramMismatchedValues];
                return new GLAnalysisWellboreViewData
                {
                    ErrorMessage = errorMessage,
                };
            }

            var bottomFluidColumn = (float)(wellboreScreenHeight - injectionScreenPoint - casingHeight);

            return new GLAnalysisWellboreViewData
            {
                HasPacker = wellboreData.HasPacker,
                WellDepth = wellboreData.WellDepth?.Amount == null ? null : (float)wellboreData.WellDepth.Amount,
                PackerDepth = wellboreData.PackerDepth?.Amount == null ? null : (float)wellboreData.PackerDepth.Amount,
                InjectionDepth = wellboreData.GasInjectionDepth?.Amount == null ? null : (float)wellboreData.GasInjectionDepth.Amount,
                TubingDepth = wellboreData.TubingDepth?.Amount == null ? null : (float)wellboreData.TubingDepth.Amount,
                CasingFluidColumn = casingFluidColumn,
                TopFluidColumn = topFluidColumn,
                BottomFluidColumn = bottomFluidColumn,
                ErrorMessage = errorMessage,
            };
        }

        private string FormatValue(double? value, string emptyText, int significantDigits, string measurementUnit)
        {
            if (value.HasValue == false)
            {
                return emptyText;
            }

            if (string.IsNullOrWhiteSpace(measurementUnit))
            {
                return $"{MathUtility.RoundToSignificantDigits(value, significantDigits)}";
            }

            return $"{MathUtility.RoundToSignificantDigits(value, significantDigits)} {measurementUnit}";
        }

        private void RoundCoordinatesToSignificantDigits(ref IList<GLAnalysisCurveCoordinateData> values, string correlationId)
        {
            if (values == null)
            {
                return;
            }

            var digits = _commonService.GetSystemParameterNextGenSignificantDigits(correlationId);

            foreach (var curve in values)
            {
                if (curve.CoordinatesOutput == null)
                {
                    continue;
                }

                foreach (var item in curve.CoordinatesOutput)
                {
                    if (double.TryParse(item.Y.ToString(), out var yDouble))
                    {
                        var yDoubleRounded = MathUtility.RoundToSignificantDigits(yDouble, digits);

                        if (float.TryParse(yDoubleRounded.ToString(), out var yFloatRounded))
                        {
                            item.Y = yFloatRounded;
                        }
                    }

                    if (double.TryParse(item.X.ToString(), out var xDouble))
                    {
                        var xDoubleRounded = MathUtility.RoundToSignificantDigits(xDouble, digits);

                        if (float.TryParse(xDoubleRounded.ToString(), out var xFloatRounded))
                        {
                            item.X = xFloatRounded;
                        }
                    }
                }
            }
        }

        #endregion

    }
}
