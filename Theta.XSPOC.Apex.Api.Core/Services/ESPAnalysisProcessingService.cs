using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using Theta.XSPOC.Apex.Api.Common.Calculators.ESP;
using Theta.XSPOC.Apex.Api.Common.Calculators.Well;
using Theta.XSPOC.Apex.Api.Common.Converters;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using Theta.XSPOC.Apex.Kernel.Quantity;
using Theta.XSPOC.Apex.Kernel.Quantity.Measures;
using ESPAnalysisInput = Theta.XSPOC.Apex.Api.Core.Common.ESPAnalysisInput;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Implementation of IESPAnalysisProcessingService interface.
    /// </summary>
    public class ESPAnalysisProcessingService : IESPAnalysisProcessingService
    {

        #region Private Members

        private readonly IESPAnalysis _espAnalysisService;
        private readonly IThetaLoggerFactory _loggerFactory;
        private readonly ILocalePhrases _localePhrases;
        private readonly IESPPump _espPumpData;
        private readonly INodeMaster _nodeMaster;
        private readonly IAnalysisCurve _analysisCurves;
        private readonly ICurveCoordinate _curveCoordinate;
        private readonly IAnalysisCurveSets _analysisCurveSet;
        private readonly IESPTornadoCurveSetAnnotation _analysisCurveSetAnnotatio;
        private readonly IGasAwareESPCalculator _espCalculator;
        private readonly IGasAwareWellCalculator _wellCalculator;
        private readonly ICommonService _commonService;

        private const string NULL_TEXT = "-";

        private enum PhraseIDs
        {

            TestDate = 102, // Test Date
            PumpIntakePressure = 258, // Pump Intake Pressure
            TubingPressure = 264, // Tubing Pressure
            CasingPressure = 265, // Casing Pressure
            OilRate = 532, // Oil Rate
            GrossRate = 533, // Gross Rate
            Pump = 928, // Pump
            PumpEfficiency = 1164, // Pump Efficiency
            WaterRate = 1250, // Water Rate
            GasRate = 1251, // Gas Rate
            Frequency = 1414, // Frequency
            FormationVolumeFactor = 2459, // Formation Volume Factor
            SolutionGOR = 2460, // Solution GOR
            OilAPI = 2461, // Oil API
            PumpDischargePressure = 2790, // Pump Discharge Pressure
            NumberOfStages = 2953, // Number Of Stages
            PumpDepthVertical = 2954, // Pump Depth (Vertical)
            PumpDepthMeasured = 2955, // Pump Depth (Measured)
            ProductivityIndex = 4603, // Productivity Index
            Theselectedwelltestwasprocessedsuccessfully = 4816, // The selected well test was processed successfully.
            SpecificGravityofGas = 4830, // Specific Gravity of Gas
            BottomholeTemperature = 4831, // Bottomhole Temperature
            GasCompressibilityFactor = 4834, // Gas Compressibility Factor
            GasVolumeFactor = 4835, // Gas Volume Factor
            ProducingGasOilRatio = 4839, // Producing Gas Oil Ratio
            GasinSolution = 4840, // Gas in Solution
            FreeGasatPump = 4841, // Free Gas at Pump
            OilVolumeatPump = 4842, // Oil Volume at Pump
            GasVolumeatPump = 4843, // Gas Volume at Pump
            TurpinParameter = 4845, // Turpin Parameter
            CompositeTubingSpecificGravity = 4846, // Composite Tubing Specific Gravity
            GasDensity = 4847, // Gas Density
            LiquidDensity = 4849, // Liquid Density
            TubingGas = 4850, // Tubing Gas
            FluidLevelAbovePump = 5086, // Fluid Level Above Pump
            Ptr = 5099, // ΔPtr
            PressureAcrossPump = 5106, // Pressure Across Pump
            GasSeparatorEfficiency = 5579, // Gas Separator Efficiency
            CasingInnerDiameter = 5580, // Casing Inner Diameter
            TubingOuterDiameter = 5581, // Tubing Outer Diameter
            SpecificGravityofOil = 5584, // Specific Gravity of Oil
            TotalVolumeatPump = 5593, // Total Volume at Pump
            FreeGas = 5594, // Free Gas
            AnnularSeparation = 5599, // Annular Separation
            CasingValveState = 5633, // Casing Valve State
            FlowingBHP = 5644, // Flowing BHP
            PumpDegradation = 5985, // Pump Degradation
            MaxRunningFrequency = 6818, // Max Running Frequency
            MotorLoad = 6823, // Motor Load
            Manufacturer = 666, // Manufacturer
            MeasuringUnitSCFPerB = 5642, // Measuring Unit scf/b
            MeasuringUnitBPerMSCF = 5636, // Measuring Unit b/mscf
            MeasuringUnitLBPerCF = 5576, // Measuring Unit lb/cf
            MidPerfDepth = 6481, // Mid-Perforation Depth
        }

        private enum PressureProfilePhrasesIds
        {
            PumpIntakePressure = 258, // Pump Intake Pressure
            NoAnalysisAvailable = 2760, // No analysis available,
            SelectedWellTestWasNotProcessedSuccessfully = 5561, // The selected well test was not processed successfully.
            MissingData = 6483, // Missing data
            PumpDischargePressure = 2790, //Pump Discharge Pressure
            PumpStaticPressure = 5097, //Pump Static Pressure
            PressureAcrossPump = 5106, //Pressure Across Pump
            FrictionalPressureLossInTubing = 5099, //Frictional Loss in Tubing
            CasingPressure = 265, // Casing Pressure
            TubingPressure = 264, // Tubing Pressure
            GasHandling = 5577, // Gas Handling
            WaterRate = 1250, // Water Rate
            OilRate = 532, // Oil Rate
            WaterSpecificGravity = 5617, //Water Specific Gravity
            VerticalPumpDepth = 2954, // Vertical Pump Depth
            CalculatedFluidLevelAbovePump = 5557, // Calculated Fluid Level Above Pump
            MidPerfDepth = 6481, // Mid-Perforation Depth
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="ESPAnalysisProcessingService"/>.
        /// </summary>
        /// <param name="espAnalysisService">
        ///     The <seealso cref="IESPAnalysis"/> service.</param>
        /// <param name="loggerFactory">
        ///     The <seealso cref="IThetaLoggerFactory"/> service.</param>
        /// <param name="localePhrases">
        ///     The <seealso cref="ILocalePhrases"/> service.</param>
        /// <param name="espPumpData">
        ///     The <seealso cref="IESPPump"/> service.</param>
        /// <param name="nodeMaster">
        ///     The <seealso cref="INodeMaster"/> service.</param>
        /// <param name="analysisCurves">
        ///     The <seealso cref="IAnalysisCurve"/> service.</param>
        /// <param name="curveCoordinate">
        ///     The <seealso cref="ICurveCoordinate"/> service.</param>
        /// <param name="analysisCurveSet">
        ///     The <seealso cref="IAnalysisCurveSets"/> service.</param>
        /// <param name="analysisCurveSetAnnotatio">
        ///     The <seealso cref="IESPTornadoCurveSetAnnotation"/> service.</param>
        /// <param name="espCalculator">
        ///     The <seealso cref="IGasAwareESPCalculator"/>.
        /// </param>
        /// <param name="wellCalculator">
        ///     The <seealso cref="IGasAwareWellCalculator"/>.
        /// </param>
        /// <param name="commonService"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="espAnalysisService"/> is null
        /// or
        /// <paramref name="loggerFactory"/> is null
        /// or
        /// <paramref name="localePhrases"/> is null
        /// or
        /// <paramref name="espPumpData"/> is null
        /// or
        /// <paramref name="nodeMaster"/> is null
        /// or
        /// <paramref name="analysisCurves"/> is null
        /// or
        /// <paramref name="curveCoordinate"/> is null
        /// or
        /// <paramref name="analysisCurveSet"/> is null
        /// or
        /// <paramref name="analysisCurveSetAnnotatio"/> is null
        /// or
        /// <paramref name="espCalculator"/> is null
        /// or
        /// <paramref name="wellCalculator"/> is null.
        /// </exception>
        public ESPAnalysisProcessingService(IESPAnalysis espAnalysisService, IThetaLoggerFactory loggerFactory,
            ILocalePhrases localePhrases, IESPPump espPumpData, INodeMaster nodeMaster,
            IAnalysisCurve analysisCurves, ICurveCoordinate curveCoordinate,
            IAnalysisCurveSets analysisCurveSet, IESPTornadoCurveSetAnnotation analysisCurveSetAnnotatio,
            IGasAwareESPCalculator espCalculator, IGasAwareWellCalculator wellCalculator, ICommonService commonService)
        {
            _espAnalysisService = espAnalysisService ?? throw new ArgumentNullException(nameof(espAnalysisService));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _localePhrases = localePhrases ?? throw new ArgumentNullException(nameof(localePhrases));
            _espPumpData = espPumpData ?? throw new ArgumentNullException(nameof(espPumpData));
            _nodeMaster = nodeMaster ?? throw new ArgumentNullException(nameof(nodeMaster));
            _analysisCurves = analysisCurves ?? throw new ArgumentNullException(nameof(analysisCurves));
            _curveCoordinate = curveCoordinate ?? throw new ArgumentNullException(nameof(curveCoordinate));
            _analysisCurveSet = analysisCurveSet ?? throw new ArgumentNullException(nameof(analysisCurveSet));
            _analysisCurveSetAnnotatio =
                analysisCurveSetAnnotatio ?? throw new ArgumentNullException(nameof(analysisCurveSetAnnotatio));
            _espCalculator = espCalculator ?? throw new ArgumentNullException(nameof(espCalculator));
            _wellCalculator = wellCalculator ?? throw new ArgumentNullException(nameof(wellCalculator));
            _commonService = commonService ?? throw new ArgumentNullException(nameof(commonService));
        }

        #endregion

        #region IESPAnalysisProcessingService Implementation

        /// <summary>
        /// Processes the provided esp analysis request and generates response based on that data.
        /// </summary>
        /// <param name="data">The <seealso cref="WithCorrelationId{ESPAnalysisInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="ESPAnalysisDataOutput"/></returns>
        public ESPAnalysisDataOutput GetESPAnalysisResults(WithCorrelationId<Models.Inputs.ESPAnalysisInput> data)
        {
            var logger = _loggerFactory.Create(LoggingModel.ESPAnalysis);
            logger.WriteCId(Level.Trace, $"Starting {nameof(ESPAnalysisProcessingService)} {nameof(GetESPAnalysisResults)}", data?.CorrelationId);

            ESPAnalysisDataOutput response = new ESPAnalysisDataOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            if (data == null)
            {
                var message = $"{nameof(data)} is null, cannot get esp analysis results.";
                logger.Write(Level.Info, message);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            if (data?.Value == null)
            {
                var message = $"{nameof(data)} is null, cannot get esp analysis results.";
                logger.WriteCId(Level.Info, message, data?.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            var correlationId = data?.CorrelationId;
            var request = data.Value;

            if (string.IsNullOrEmpty(request.TestDate) ||
                request.AssetId == Guid.Empty)
            {
                var message = $"{nameof(request.TestDate)} and {nameof(request.AssetId)}" +
                    $" should be provided to get esp analysis results.";
                logger.WriteCId(Level.Info, message, correlationId);
                response.Result.Status = false;
                response.Result.Value = message;

                return response;
            }

            var espAnalysisData = _espAnalysisService.GetESPAnalysisData(request.AssetId,
                request.TestDate, correlationId);

            if (espAnalysisData == null || espAnalysisData.AnalysisResultEntity == null
                || espAnalysisData.WellPumpEntities == null || espAnalysisData.NodeMasterData == null)
            {
                var message = (espAnalysisData?.AnalysisResultEntity == null)
                    ? $"{nameof(espAnalysisData.AnalysisResultEntity)} is null"
                    : (espAnalysisData?.WellPumpEntities == null)
                        ? $"{nameof(espAnalysisData.WellPumpEntities)} is null"
                        : (espAnalysisData?.NodeMasterData == null)
                            ? $"{nameof(espAnalysisData.NodeMasterData)} is null"
                            : $"{nameof(espAnalysisData)} is null";

                response.Result.Status = false;
                response.Result.Value = $"{message}, cannot get esp analysis results.";
                logger.WriteCId(Level.Info, $"{message}, cannot get esp analysis results.", correlationId);
            }
            else
            {
                var espAnalysisValues = GetESPAnalysisReponseData(espAnalysisData, correlationId);

                if (espAnalysisValues != null)
                {
                    var responseValues = CreateResponse(espAnalysisValues, espAnalysisData.TestDate, correlationId);

                    response.Values = responseValues;
                    response.Result.Status = true;
                    response.Result.Value = string.Empty;

                    response.PressureProfile = GetPressureProfile(request.AssetId, espAnalysisData.TestDate, correlationId);
                }
                else
                {
                    var message = $"{nameof(espAnalysisValues)} is null, cannot get esp analysis results.";
                    logger.WriteCId(Level.Error, message, correlationId);
                    response.Result.Status = false;
                    response.Result.Value = message;
                }
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(ESPAnalysisProcessingService)} {nameof(GetESPAnalysisResults)}", data?.CorrelationId);

            return response;
        }

        /// <summary>
        /// Processes the provided curve coordinate request and generates curve coordinate based on that data.
        /// </summary>
        /// <param name="data">The <seealso cref="CurveCoordinatesInput"/> to act on, annotated
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="CurveCoordinateDataOutput"/>.</returns>
        public CurveCoordinateDataOutput GetCurveCoordinate(WithCorrelationId<CurveCoordinatesInput> data)
        {
            var logger = _loggerFactory.Create(LoggingModel.ESPAnalysis);
            logger.WriteCId(Level.Trace, $"Starting {nameof(ESPAnalysisProcessingService)} {nameof(GetCurveCoordinate)}", data?.CorrelationId);

            if (data == null)
            {
                logger.Write(Level.Info, $"{nameof(data)} is null, cannot get curve coordinates.");

                return null;
            }

            if (data.Value == null)
            {
                logger.WriteCId(Level.Info, $"{nameof(data)} is null, cannot get curve coordinates.",
                    data?.CorrelationId);

                return null;
            }

            var correlationId = data?.CorrelationId;
            var request = data.Value;

            if (request.AssetId == Guid.Empty)
            {
                logger.WriteCId(Level.Info,
                    $"{nameof(request)} should be provided to get curve coordinates.",
                    correlationId);
                return null;
            }

            if (string.IsNullOrEmpty(request.TestDate))
            {
                logger.WriteCId(Level.Info,
                    $"{nameof(request)} should be provided to get curve coordinates.",
                    correlationId);
                return null;
            }

            string nodeId = _nodeMaster.GetNodeIdByAsset(request.AssetId, correlationId);
            var testDate = DateTime.Parse(request.TestDate);

            var resultEntity = _espAnalysisService.GetESPAnalysisResult(nodeId, testDate, correlationId);

            AnalysisResult model = null;
            var analysisCurves = new List<AnalysisCurve>();

            int analysisResultId;
            var application = IndustryApplication.ESPArtificialLift;

            if (resultEntity != null)
            {
                analysisResultId = resultEntity.Id;

                model = new AnalysisResult(analysisResultId)
                {
                    Outputs = new AnalysisOutput(),
                };

                var analysisResultCurves = _analysisCurves.GetAnalysisResultCurves(analysisResultId, application.Key, correlationId);

                if (analysisResultCurves != null)
                {
                    foreach (var analysisResultCurve in analysisResultCurves)
                    {
                        var curve = CreateAnalysisCurve(analysisResultCurve.Id, analysisResultCurve.CurveTypeId, application);
                        var curveCoordinates = _curveCoordinate.GetCurvesCoordinates((int)curve.Id, correlationId);

                        var curveCoordinatesData = new List<CurveCoordinate>();

                        foreach (var curveCoordinate in curveCoordinates)
                        {
                            var newCoordinate = new CurveCoordinate(curveCoordinate.Id);

                            newCoordinate.Coordinate = new Coordinate<double, double>(curveCoordinate.X, curveCoordinate.Y);

                            curveCoordinatesData.Add(newCoordinate);
                        }

                        curve.Curve = curveCoordinatesData;

                        analysisCurves.Add(curve);
                    }

                    foreach (var curve in analysisCurves)
                    {
                        var analysisCurve = curve as ESPAnalysisCurve;
                        if (analysisCurve != null)
                        {
                            model.Outputs.SetAnalysisCurve((ESPAnalysisCurve)curve);
                        }
                    }

                    var sourceData = AnalysisResultSource.ESP;
                    var curveSetData = CurveSetType.Tornado;

                    var curveSet = new AnalysisCurveSet()
                    {
                        AnalysisResultId = analysisResultId,
                        AnalysisResultSource = sourceData,
                        CurveSetType = curveSetData,
                        Curves = new List<AnalysisCurveSetMemberBase>(),
                    };

                    var tornadoCurveSet = _analysisCurveSet.GetAnalysisCurvesSet(analysisResultId, sourceData.Key,
                        curveSetData.Key, correlationId);

                    var entityHashset = tornadoCurveSet.ToHashSet();

                    curveSet.CurveSetId = entityHashset.Select(x => x.AnalysisCurveSetDataModels.CurveSetId).FirstOrDefault();
                    var curveSetMemberIds = entityHashset.Where(x => x.AnalysisCurveSetMemberModels != null)
                        .Select(x => x.AnalysisCurveSetMemberModels?.CurveSetMemberId).Distinct();

                    foreach (var curveSetMemberId in curveSetMemberIds)
                    {
                        var curveSetMember = AnalysisCurveSetMemberUtility.GetAnalysisCurveSetMember(curveSetData);
                        curveSetMember.CurveSetMemberId = curveSetMemberId;
                        var curves = entityHashset.Where(x => x.AnalysisCurveSetMemberModels.CurveSetMemberId == curveSetMemberId
                                && x.CurveSetCoordinatesModels != null).OrderBy(x => x.CurveSetCoordinatesModels.X)
                            .Select(x => x.CurveSetCoordinatesModels);

                        var newCurves = new List<CurveCoordinate>();

                        if (curves == null)
                        {
                            curveSetMember.Curve = newCurves;
                            curveSet.Curves.Add(curveSetMember);

                            continue;
                        }

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
                        var curveSetMemberId = curveSet.Curves.Select(c => c.CurveSetMemberId).ToList();
                        var annotationEntities = _analysisCurveSetAnnotatio.GetESPTornadoCurveSetAnnotations(correlationId)
                            .Where(e => curveSetMemberId.Contains(e.CurveSetMemberId)).ToList();

                        MapToDomain(annotationEntities, curveSet);
                    }

                    model.Outputs.TornadoCurves = curveSet;
                }
            }

            var result = GetESPAnalysisCurveResponse(model, resultEntity, correlationId);

            logger.WriteCId(Level.Trace, $"Finished {nameof(ESPAnalysisProcessingService)} {nameof(GetCurveCoordinate)}", data?.CorrelationId);

            return result;
        }

        /// <summary>
        /// Gets the pressure profile data.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="testDate">The test date.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <see cref="ESPPressureProfileData"/>.</returns>
        public ESPPressureProfileData GetPressureProfile(Guid assetId, DateTime testDate, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.ESPAnalysis);
            logger.WriteCId(Level.Trace, $"Starting {nameof(ESPAnalysisProcessingService)} {nameof(GetPressureProfile)}", correlationId);

            var digits = _commonService.GetSystemParameterNextGenSignificantDigits(correlationId);

            if (assetId == Guid.Empty)
            {
                logger.WriteCId(Level.Info,
                    $"{nameof(assetId)} should be provided to get pressure profile data.",
                    correlationId);

                return null;
            }

            var pressureProfileData = _espAnalysisService.GetESPPressureProfileData(assetId, testDate, correlationId);

            var phraseIds = Enum.GetValues<PressureProfilePhrasesIds>().Cast<int>().ToArray();
            var phrases = _localePhrases.GetAll(correlationId, phraseIds);

            // Todo: Update missing requirements method.
            GetMissingRequirementsForPressureProfile(
                pressureProfileData, phrases, out var missingRequirements, out var missingRequirementsLocalized,
                out var perforationDepth);

            if (missingRequirements.Count != 0)
            {
                string requirements = String.Join(Environment.NewLine, missingRequirements.ToArray());
                string requirementsLocalized = string.Join(Environment.NewLine,
                    missingRequirementsLocalized.ToArray());

                string message = String.Format("The pressure profile could not be generated because the " +
                    "following data is missing: {0}{1}", Environment.NewLine, requirements);

                logger.WriteCId(Level.Info, message, correlationId);

                return new ESPPressureProfileData()
                {
                    IsValid = false,
                    ErrorMessage = string.Format(
                        "{0} {1}:{2}{3}",
                        phrases[(int)PressureProfilePhrasesIds.SelectedWellTestWasNotProcessedSuccessfully],
                        phrases[(int)PressureProfilePhrasesIds.MissingData],
                        Environment.NewLine,
                        requirementsLocalized),
                };
            }

            var result = new ESPPressureProfileData()
            {
                IsValid = true
            };

            // Todo: Everywhere "ConvertTo()" is used, use the user's unit configuration settings instead.
            // Except: water specific gravity, specific gravity of oil.
            if (pressureProfileData.PumpIntakePressure.HasValue)
            {
                result.PumpIntakePressure = Pressure
                    .FromPoundsPerSquareInch(
                        MathUtility.RoundToSignificantDigits(pressureProfileData.PumpIntakePressure.Value, digits));
            }

            result.UseDischargeGaugeInAnalysis = pressureProfileData.UseDischargeGaugeInAnalysis;

            if (pressureProfileData.PumpDischargePressure.HasValue)
            {
                result.PumpDischargePressure = Pressure
                    .FromPoundsPerSquareInch(
                        MathUtility.RoundToSignificantDigits(pressureProfileData.PumpDischargePressure.Value, digits));
            }

            if (pressureProfileData.PumpStaticPressure.HasValue)
            {
                result.PumpStaticPressure = Pressure
                    .FromPoundsPerSquareInch(
                        MathUtility.RoundToSignificantDigits(pressureProfileData.PumpStaticPressure.Value, digits));
            }

            if (pressureProfileData.PressureAcrossPump.HasValue)
            {
                result.PumpPressureDelta = Pressure
                    .FromPoundsPerSquareInch(
                        MathUtility.RoundToSignificantDigits(pressureProfileData.PressureAcrossPump.Value, digits));
            }

            if (pressureProfileData.FrictionalPressureDrop.HasValue)
            {
                result.PumpFrictionDelta = Pressure
                    .FromPoundsPerSquareInch(
                        MathUtility.RoundToSignificantDigits(pressureProfileData.FrictionalPressureDrop.Value, digits));
            }

            if (pressureProfileData.CasingPressure.HasValue)
            {
                result.CasingPressure = Pressure
                    .FromPoundsPerSquareInch(
                        MathUtility.RoundToSignificantDigits(pressureProfileData.CasingPressure.Value, digits));
            }

            if (pressureProfileData.TubingPressure.HasValue)
            {
                result.TubingPressure = Pressure
                    .FromPoundsPerSquareInch(
                        MathUtility.RoundToSignificantDigits(pressureProfileData.TubingPressure.Value, digits));
            }

            if (pressureProfileData.FlowingBottomholePressure.HasValue)
            {
                result.FlowingBottomholePressure = Pressure
                    .FromPoundsPerSquareInch(
                        MathUtility.RoundToSignificantDigits(pressureProfileData.FlowingBottomholePressure.Value, digits));
            }

            // Calculate flowing bottomhole pressure gradient if composite specific gravity exists.
            if (pressureProfileData.CompositeTubingSpecificGravity.HasValue)
            {
                var compositeTubingSpecificGravity = RelativeDensity
                    .FromSpecificGravity(
                        MathUtility.RoundToSignificantDigits(pressureProfileData.CompositeTubingSpecificGravity.Value, digits));

                var flowingBottomholePressureGradient = _espCalculator
                    .CalculateFlowingBHPGradient(compositeTubingSpecificGravity);

                result.FlowingBottomholePressureGradient =
                    MathUtility.RoundToSignificantDigits(flowingBottomholePressureGradient, digits).ToString();
            }

            // Calculate water cut if water rate and oil rate exist.
            Quantity<Fraction> waterCut = null;
            if (pressureProfileData.WaterRate.HasValue && pressureProfileData.OilRate.HasValue)
            {
                var waterRate = LiquidVolume
                    .FromBarrels(
                        MathUtility.RoundToSignificantDigits(pressureProfileData.WaterRate.Value, digits));

                var oilRate = LiquidVolume
                    .FromBarrels(
                        MathUtility.RoundToSignificantDigits(pressureProfileData.OilRate.Value, digits));

                var grossRate = oilRate + waterRate;

                if (grossRate.Amount != 0)
                {
                    waterCut = _wellCalculator.CalculateWaterCut(waterRate, oilRate);
                }
            }

            // Calculate static gradient if water specific gravity, specific gravity of oil, and water cut exist.
            if (pressureProfileData.IsGasHandlingEnabled &&
                pressureProfileData.WaterSpecificGravity.HasValue &&
                pressureProfileData.SpecificGravityOfOil.HasValue &&
                waterCut != null)
            {
                var waterSpecificGravity = RelativeDensity
                    .FromSpecificGravity(
                        MathUtility.RoundToSignificantDigits(pressureProfileData.WaterSpecificGravity.Value, digits));

                var specificGravityOfOil = OilRelativeDensity
                    .FromSpecificGravity(
                        MathUtility.RoundToSignificantDigits(pressureProfileData.SpecificGravityOfOil.Value, digits));

                var staticGradient = _wellCalculator.CalculateStaticGradient(
                    waterSpecificGravity, specificGravityOfOil, waterCut);

                result.StaticGradient = MathUtility.RoundToSignificantDigits(staticGradient, digits).ToString();
            }

            if (pressureProfileData.DischargeGaugePressure.HasValue)
            {
                result.DischargeGaugePressure = Pressure
                    .FromPoundsPerSquareInch(MathUtility.RoundToSignificantDigits(pressureProfileData.DischargeGaugePressure.Value, digits));
            }

            if (pressureProfileData.DischargeGaugeDepth.HasValue)
            {
                result.DischargeGaugeDepth = Length.FromFeet(pressureProfileData.DischargeGaugeDepth.Value);
            }

            // Calculate fluid level.
            if (pressureProfileData.VerticalPumpDepth.HasValue &&
                pressureProfileData.CalculatedFluidLevelAbovePump.HasValue)
            {
                var verticalPumpDepth = Length
                .FromFeet(
                        MathUtility.RoundToSignificantDigits(pressureProfileData.VerticalPumpDepth.Value, digits));

                result.VerticalPumpDepth = verticalPumpDepth;

                var calculatedFluidLevelAbovePump = Length
                    .FromFeet(
                        MathUtility.RoundToSignificantDigits(pressureProfileData.CalculatedFluidLevelAbovePump.Value, digits));

                var fluidLevel = Length.FromFeet(MathUtility.RoundToSignificantDigits(_wellCalculator
                    .CalculateFluidLevelFromSurface(verticalPumpDepth, calculatedFluidLevelAbovePump).Amount, digits));

                result.FluidLevel = fluidLevel;
            }

            if (perforationDepth != null)
            {
                result.PerforationDepth = Length.FromFeet(
                    MathUtility.RoundToSignificantDigits(perforationDepth.Amount, digits));
            }

            result.PressureGradientUnits = Pressure.PoundPerSquareInch.Symbol + "/" + Length.Foot.Symbol;
            logger.WriteCId(Level.Trace, $"Finished {nameof(ESPAnalysisProcessingService)} {nameof(GetPressureProfile)}", correlationId);

            return result;
        }

        #endregion

        #region Private Methods

        #region Pressure Profile

        private void GetMissingRequirementsForPressureProfile(
            ESPPressureProfileModel data,
            IDictionary<int, string> phrases,
            out HashSet<string> missingRequirements,
            out HashSet<string> missingRequirementsLocalized,
            out Quantity<Length> perforationDepth)
        {
            missingRequirements = new HashSet<string>();
            missingRequirementsLocalized = new HashSet<string>();
            perforationDepth = null;

            if (data == null)
            {
                missingRequirements.Add("Analysis must be completed");
                missingRequirementsLocalized.Add(phrases[(int)PressureProfilePhrasesIds.NoAnalysisAvailable]);

                return;
            }

            if (!data.PumpIntakePressure.HasValue)
            {
                missingRequirements.Add("Pump Intake Pressure");
                missingRequirementsLocalized.Add(phrases[(int)PressureProfilePhrasesIds.PumpIntakePressure]);
            }

            if (!data.PumpDischargePressure.HasValue)
            {
                missingRequirements.Add("Pump Discharge Pressure");
                missingRequirementsLocalized.Add(phrases[(int)PressureProfilePhrasesIds.PumpDischargePressure]);
            }

            if (!data.PumpStaticPressure.HasValue)
            {
                missingRequirements.Add("Pump Static Pressure");
                missingRequirementsLocalized.Add(phrases[(int)PressureProfilePhrasesIds.PumpStaticPressure]);
            }

            if (!data.PressureAcrossPump.HasValue)
            {
                missingRequirements.Add("Pressure Across Pump");
                missingRequirementsLocalized.Add(phrases[(int)PressureProfilePhrasesIds.PressureAcrossPump]);
            }

            if (!data.FrictionalPressureDrop.HasValue)
            {
                missingRequirements.Add("Frictional Pressure Loss in Tubing");
                missingRequirementsLocalized.Add(
                    phrases[(int)PressureProfilePhrasesIds.FrictionalPressureLossInTubing]);
            }

            if (!data.CasingPressure.HasValue)
            {
                missingRequirements.Add("Casing Pressure");
                missingRequirementsLocalized.Add(phrases[(int)PressureProfilePhrasesIds.CasingPressure]);
            }

            if (!data.TubingPressure.HasValue)
            {
                missingRequirements.Add("Tubing Pressure");
                missingRequirementsLocalized.Add(phrases[(int)PressureProfilePhrasesIds.TubingPressure]);
            }

            if (!data.FlowingBottomholePressure.HasValue)
            {
                missingRequirements.Add("Flowing Bottomhole Pressure");
            }

            if (!data.WaterSpecificGravity.HasValue)
            {
                missingRequirements.Add("Water Specific Gravity");
                missingRequirementsLocalized.Add(phrases[(int)PressureProfilePhrasesIds.WaterSpecificGravity]);
            }

            if (!data.VerticalPumpDepth.HasValue)
            {
                missingRequirements.Add("Vertical Pump Depth");
                missingRequirementsLocalized.Add(phrases[(int)PressureProfilePhrasesIds.VerticalPumpDepth]);
            }

            if (!data.CalculatedFluidLevelAbovePump.HasValue)
            {
                missingRequirements.Add("Calculated Fluid Level Above Pump");
                missingRequirementsLocalized.Add(phrases[(int)PressureProfilePhrasesIds.CalculatedFluidLevelAbovePump]);
            }

            if (data.Perforations != null && data.Perforations.Count > 0)
            {
                perforationDepth = CalculatePerforationDepth(data.Perforations);
            }
            else if (data.ProductionDepth.HasValue)
            {
                perforationDepth = Length.FromFeet(data.ProductionDepth.Value);
            }

            if (perforationDepth == null)
            {
                missingRequirements.Add("Mid-Perforation Depth");
                missingRequirementsLocalized.Add(phrases[(int)PressureProfilePhrasesIds.MidPerfDepth]);
            }

            if (data.IsGasHandlingEnabled)
            {
                if (!data.WaterRate.HasValue)
                {
                    missingRequirements.Add("Gas Handling: Water Rate");
                    missingRequirementsLocalized.Add(
                        string.Format(
                            "{0}: {1}",
                            phrases[(int)PressureProfilePhrasesIds.GasHandling],
                            phrases[(int)PressureProfilePhrasesIds.WaterRate]));
                }

                if (!data.OilRate.HasValue)
                {
                    missingRequirements.Add("Gas Handling: Oil Rate");
                    missingRequirementsLocalized.Add(
                        string.Format(
                            "{0}: {1}",
                            phrases[(int)PressureProfilePhrasesIds.GasHandling],
                            phrases[(int)PressureProfilePhrasesIds.OilRate]));
                }
            }
        }

        private Quantity<Length> CalculatePerforationDepth(IList<ESPPerforation> perforations)
        {
            return Length.FromFeet(perforations.Average(p => p.TopDepth + (0.5 * p.Length)));
        }

        #endregion

        private AnalysisResult GetESPAnalysisReponseData(ESPAnalysisResponse espAnalysisData, string correlationId)
        {
            AnalysisResult result = new AnalysisResult();

            if (espAnalysisData.AnalysisResultEntity != null)
            {
                result = MapToDomain(espAnalysisData.AnalysisResultEntity, espAnalysisData.WellPumpEntities, correlationId);
            }

            return result;
        }

        private AnalysisResult MapToDomain(ESPAnalysisResultModel ànalysisResults, IList<ESPWellPumpModel> pumpEntities, string correlationId)
        {
            var pumpConfigs = new List<PumpConfiguration>();
            PumpConfiguration pumpConfig;
            foreach (var pumpEntity in pumpEntities.OrderBy(e => e.OrderNumber))
            {
                var pump = _espPumpData.GetESPPumpData(pumpEntity.ESPPumpId, correlationId);

                if (pump != null)
                {
                    pumpConfig = new PumpConfiguration()
                    {
                        Pump = GetPumpData(pump),
                        NumberOfStages = pumpEntity.NumberOfStages,
                    };

                    pumpConfigs.Add(pumpConfig);
                }
            }

            var input = new ESPAnalysisInput
            {
                VerticalPumpDepth = ànalysisResults.VerticalPumpDepth,
                MeasuredPumpDepth = ànalysisResults.MeasuredPumpDepth,
                OilRate = ànalysisResults.OilRate,
                WaterRate = ànalysisResults.WaterRate,
                GasRate = ànalysisResults.GasRate,
                PumpIntakePressure = ànalysisResults.PumpIntakePressure,
                GrossRate = ànalysisResults.GrossRate,
                FluidLevelAbovePump = ànalysisResults.FluidLevelAbovePump,
                TubingPressure = ànalysisResults.TubingPressure,
                CasingPressure = ànalysisResults.CasingPressure,
                Frequency = ànalysisResults.Frequency,
                OilAPI = ànalysisResults.OilApi,
                WaterSpecificGravity = ànalysisResults.WaterSpecificGravity,
                TubingInnerDiameter = ànalysisResults.TubingId,
                PumpConfigs = pumpConfigs,
                DischargeGaugePressure = ànalysisResults.DischargeGaugePressure,
                DischargeGaugeDepth = ànalysisResults.DischargeGageDepth,
                UseDischargeGaugeInAnalysis = ànalysisResults.UseDischargeGageInAnalysis,
                UseTVD = ànalysisResults.UseTVD,
            };

            //Gas Handling inputs
            var gasInputs = new GasHandlingAnalysisInput()
            {
                GasRelativeDensity = ànalysisResults.SpecificGravityOfGas,
                BottomholeTemperature = ànalysisResults.BottomholeTemperature,
                WellheadTemperature = ànalysisResults.WellHeadTemperature,
                GasSeparatorEfficiency = ànalysisResults.GasSeparatorEfficiency,
                CasingInnerDiameter = ànalysisResults.CasingId,
                TubingOuterDiameter = ànalysisResults.TubingOd,
                IsCasingValveClosed = ànalysisResults.CasingValveClosed,
            };

            input.GasHandlingInputs = gasInputs;

            var output = new AnalysisOutput
            {
                ProductivityIndex = ànalysisResults.ProductivityIndex,
                FlowingBHP = ànalysisResults.FlowingBhp,
                PressureAcrossPump = ànalysisResults.PressureAcrossPump,
                PumpDischargePressure = ànalysisResults.PumpDischargePressure,
                FrictionalLossInTubing = ànalysisResults.FrictionalLossInTubing,
                PumpEfficiency = ànalysisResults.PumpEfficiency,
                CalculatedFluidLevelAbovePump = ànalysisResults.CalculatedFluidLevelAbovePump,
                FluidSpecificGravity = ànalysisResults.FluidSpecificGravity,
                HeadAcrossPump = ànalysisResults.HeadAcrossPump,
                PumpStaticPressure = ànalysisResults.PumpStaticPressure,
                IPRSlope = ànalysisResults.Iprslope,
                RateAtBubblePoint = ànalysisResults.RateAtBubblePoint,
                RateAtMaxLiquid = ànalysisResults.RateAtMaxLiquid,
                RateAtMaxOil = ànalysisResults.RateAtMaxOil,
                WaterCut = ànalysisResults.WaterCut,
                MaxRunningFrequency = ànalysisResults.MaxRunningFrequency,
                MotorLoadPercentage = ànalysisResults.MotorLoadPercentage,
            };

            var gasOutputs = new GasHandlingAnalysisOutput()
            {
                AnnularSeparationEfficiency = ànalysisResults.AnnularSeparationEfficiency,
                CompositeTubingSpecificGravity = ànalysisResults.CompositeTubingSpecificGravity,
                FormationVolumeFactor = ànalysisResults.FormationVolumeFactor,
                FreeGas = ànalysisResults.FreeGas,
                FreeGasAtPump = ànalysisResults.FreeGasAtPump,
                GasCompressibilityFactor = ànalysisResults.GasCompressibilityFactor,
                GasDensity = ànalysisResults.GasDensity,
                GasInSolution = ànalysisResults.GasInSolution,
                GasOilRatioAtPump = ànalysisResults.GasOilRatioAtPump,
                GasVolumeAtPump = ànalysisResults.GasVolumeAtPump,
                GasFormationVolumeFactor = ànalysisResults.GasVolumeFactor,
                LiquidDensity = ànalysisResults.LiquidDensity,
                OilVolumeAtPump = ànalysisResults.OilVolumeAtPump,
                ProducingGasOilRatio = ànalysisResults.ProducingGor,
                OilSpecificGravity = ànalysisResults.SpecificGravityOfOil,
                TotalVolumeAtPump = ànalysisResults.TotalVolumeAtPump,
                TubingGas = ànalysisResults.TubingGas,
                TubingGasOilRatio = ànalysisResults.TubingGor,
                TurpinParameter = ànalysisResults.TurpinParameter,
            };

            var diagnostics = new AnalysisDiagnostics()
            {
                HeadRelativeToPumpCurve = ànalysisResults.HeadRelativeToPumpCurve,
                HeadRelativeToWellPerformanceCurve = ànalysisResults.HeadRelativeToWellPerformance,
                HeadRelativeToRecommendedRange = ànalysisResults.HeadRelativeToRecommendedRange,
                FlowRelativeToRecommendedRange = ànalysisResults.FlowRelativeToRecommendedRange,
                DesignScore = ànalysisResults.DesignScore,
                ProductionIncreasePercentage = ànalysisResults.ProductionIncreasePercentage,
                PumpDegradation = ànalysisResults.PumpDegradation,
                MaxPotentialProductionRate = ànalysisResults.MaxPotentialProductionRate,
                MaxPotentialFrequency = ànalysisResults.MaxPotentialFrequency,
            };

            output.GasHandlingOutputs = gasOutputs;
            output.Diagnostics = diagnostics;

            var source = new AnalysisSource
            {
                PumpIntakePressureSource = ànalysisResults.PumpIntakePressureSource,
                FluidLevelAbovePumpSource = ànalysisResults.FluidLevelAbovePumpSource,
                TubingPressureSource = ànalysisResults.TubingPressureSource,
                CasingPressureSource = ànalysisResults.CasingPressureSource,
                FrequencySource = ànalysisResults.FrequencySource,
                WellHeadTemperatureSource = ànalysisResults.WellHeadTemperatureSource,
                BottomholeTemperatureSource = ànalysisResults.BottomholeTemperatureSource,
                OilSpecificGravitySource = ànalysisResults.OilSpecificGravitySource,
                WaterSpecificGravitySource = ànalysisResults.WaterSpecificGravitySource,
                GasSpecificGravitySource = ànalysisResults.GasSpecificGravitySource,
                OilRateSource = ànalysisResults.OilRateSource,
                WaterRateSource = ànalysisResults.WaterRateSource,
                GasRateSource = ànalysisResults.GasRateSource,
                DischargeGaugePressureSource = ànalysisResults.DischargeGagePressureSource,
            };

            var resultMessageLocalized = string.Empty;
            if (ànalysisResults.ResultMessageTemplate != null)
            {
                resultMessageLocalized = ESPAnalysisPhraseInterpolator.Interpolate(
                    ànalysisResults.ResultMessageTemplate, input, output);
            }

            var model = new AnalysisResult(ànalysisResults.Id)
            {
                ProcessedDateTime = ànalysisResults.ProcessedDate,
                Inputs = input,
                Outputs = output,
                ResultMessage = ànalysisResults.ResultMessage,
                ResultMessageLocalized = resultMessageLocalized,
                IsSuccess = ànalysisResults.Success,
                Sources = source,
                IsGasHandlingEnabled = ànalysisResults.EnableGasHandling,
            };

            return model;
        }

        private ESPAnalysisValues CreateResponse(AnalysisResult result, DateTime testDate, string correlationId)
        {
            IDictionary<int, string> phrases;
            var phraseid = Enum.GetValues<PhraseIDs>().Cast<int>().ToArray();
            phrases = _localePhrases.GetAll(correlationId, phraseid);

            var digits = _commonService.GetSystemParameterNextGenSignificantDigits(correlationId);

            var responseValues = new ESPAnalysisValues()
            {
                Inputs = new List<AnalysisInputOutput>(),
                Outputs = new List<AnalysisInputOutput>(),
                GasHandlingInputs = new List<AnalysisInputOutput>(),
                GasHandlingOutputs = new List<AnalysisInputOutput>(),
            };

            if (result.Inputs.PumpConfigs.Count > 1)
            {
                var pumpConfigCounter = 0;

                foreach (var pumpConfig in result.Inputs.PumpConfigs)
                {
                    pumpConfigCounter++;

                    responseValues.Inputs.Add(new AnalysisInputOutput()
                    {
                        Id = "Manufacturer",
                        DisplayValue =
                            pumpConfig.Pump.Manufacturer == null
                                ? NULL_TEXT
                                : pumpConfig.Pump.Manufacturer?.Manufacturer ?? NULL_TEXT,
                        DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.Manufacturer]),
                        Value = pumpConfig.Pump.Manufacturer?.Manufacturer,
                    });

                    responseValues.Inputs.Add(new AnalysisInputOutput()
                    {
                        Id = "PumpName",
                        DisplayValue = pumpConfig.Pump.Name ?? NULL_TEXT,
                        DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.Pump]),
                        Value = pumpConfig.Pump.Name,
                    });

                    responseValues.Inputs.Add(new AnalysisInputOutput()
                    {
                        Id = "NumberOfStages",
                        DisplayValue = pumpConfig.NumberOfStages.ToString() ?? NULL_TEXT,
                        DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.NumberOfStages]),
                        Value = pumpConfig.NumberOfStages,
                    });
                }
            }
            else
            {
                responseValues.Inputs.Add(new AnalysisInputOutput()
                {
                    Id = "PumpName",
                    DisplayValue = result.Inputs.PumpConfigs.FirstOrDefault()?.Pump?.Name ?? NULL_TEXT,
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.Pump]),
                    Value = result.Inputs.PumpConfigs.FirstOrDefault()?.Pump?.Name,
                });

                responseValues.Inputs.Add(new AnalysisInputOutput()
                {
                    Id = "NumberOfStages",
                    DisplayValue = result.Inputs.PumpConfigs.FirstOrDefault()?.NumberOfStages?.ToString() ?? NULL_TEXT,
                    DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.NumberOfStages]),
                    Value = result.Inputs.PumpConfigs.FirstOrDefault()?.NumberOfStages?.ToString(),
                });
            }

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
                DisplayValue = FormatValue(result.Inputs?.OilRate, NULL_TEXT, digits, LiquidFlowRate.BarrelPerDay.Symbol),
                SourceId = result.Sources.OilRateSource,
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.OilRate]),
                Value = result.Inputs?.OilRate != null
                    ? LiquidFlowRate.FromBarrelsPerDay((double)result.Inputs?.OilRate).Amount : null,
                MeasurementAbbreviation = LiquidFlowRate.BarrelPerDay.Symbol,
            });

            responseValues.Inputs.Add(new AnalysisInputOutput()
            {
                Id = "WaterRate",
                DisplayValue = FormatValue(result.Inputs?.WaterRate, NULL_TEXT, digits, LiquidFlowRate.BarrelPerDay.Symbol),
                SourceId = result.Sources.WaterRateSource,
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.WaterRate]),
                Value = result.Inputs?.WaterRate != null
                    ? LiquidFlowRate.FromBarrelsPerDay((double)result.Inputs?.WaterRate.Value).Amount : null,
                MeasurementAbbreviation = LiquidFlowRate.BarrelPerDay.Symbol,
            });

            responseValues.Inputs.Add(new AnalysisInputOutput()
            {
                Id = "GrossRate",
                DisplayValue = FormatValue(result.Inputs?.GrossRate, NULL_TEXT, digits, LiquidFlowRate.BarrelPerDay.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.GrossRate]),
                Value = result.Inputs?.GrossRate != null
                    ? LiquidFlowRate.FromBarrelsPerDay(result.Inputs.GrossRate.Value).Amount : null,
                MeasurementAbbreviation = LiquidFlowRate.BarrelPerDay.Symbol,
            });

            responseValues.Inputs.Add(new AnalysisInputOutput()
            {
                Id = "GasRate",
                DisplayValue = FormatValue(result.Inputs?.GasRate, NULL_TEXT, digits, GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol),
                SourceId = result.Sources.GasRateSource,
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.GasRate]),
                Value = result.Inputs?.GasRate != null
                    ? GasFlowRate.FromThousandStandardCubicFeetPerDay(result.Inputs.GasRate.Value).Amount : null,
                MeasurementAbbreviation = GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol,
            });

            responseValues.Inputs.Add(new AnalysisInputOutput()
            {
                Id = "PumpIntakePressure",
                DisplayValue = FormatValue(result.Inputs?.PumpIntakePressure, NULL_TEXT, digits, Pressure.PoundPerSquareInch.Symbol),
                SourceId = result.Sources.PumpIntakePressureSource,
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.PumpIntakePressure]),
                Value = result.Inputs?.PumpIntakePressure != null
                    ? Pressure.FromPoundsPerSquareInch(result.Inputs.PumpIntakePressure.Value).Amount : null,
                MeasurementAbbreviation = Pressure.PoundPerSquareInch.Symbol,
            });

            responseValues.Inputs.Add(new AnalysisInputOutput()
            {
                Id = "FluidLevelAbovePump",
                DisplayValue = FormatValue(result.Inputs?.FluidLevelAbovePump, NULL_TEXT, digits, Length.Foot.Symbol),
                SourceId = result.Sources.FluidLevelAbovePumpSource,
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.FluidLevelAbovePump]),
                Value = result.Inputs?.FluidLevelAbovePump != null
                    ? Length.FromFeet(result.Inputs.FluidLevelAbovePump.Value).Amount : null,
                MeasurementAbbreviation = Length.Foot.Symbol,
            });

            responseValues.Inputs.Add(new AnalysisInputOutput()
            {
                Id = "TubingPressure",
                DisplayValue = FormatValue(result.Inputs?.TubingPressure, NULL_TEXT, digits, Pressure.PoundPerSquareInch.Symbol),
                SourceId = result.Sources.TubingPressureSource,
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.TubingPressure]),
                Value = result.Inputs?.TubingPressure != null
                    ? Pressure.FromPoundsPerSquareInch(result.Inputs.TubingPressure.Value).Amount : null,
                MeasurementAbbreviation = Pressure.PoundPerSquareInch.Symbol,
            });

            responseValues.Inputs.Add(new AnalysisInputOutput()
            {
                Id = "CasingPressure",
                DisplayValue = FormatValue(result.Inputs?.CasingPressure, NULL_TEXT, digits, Pressure.PoundPerSquareInch.Symbol),
                SourceId = result.Sources.CasingPressureSource,
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.CasingPressure]),
                Value = result.Inputs?.CasingPressure != null
                    ? Pressure.FromPoundsPerSquareInch(result.Inputs.CasingPressure.Value).Amount : null,
                MeasurementAbbreviation = Pressure.PoundPerSquareInch.Symbol,
            });

            responseValues.Inputs.Add(new AnalysisInputOutput()
            {
                Id = "Frequency",
                DisplayValue = FormatValue(result.Inputs?.Frequency, NULL_TEXT, digits, Frequency.Hertz.Symbol),
                SourceId = result.Sources.FrequencySource,
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.Frequency]),
                Value = result.Inputs?.Frequency != null
                    ? Frequency.FromHertz(result.Inputs.Frequency.Value).Amount : null,
                MeasurementAbbreviation = Frequency.Hertz.Symbol,
            });

            responseValues.Inputs.Add(new AnalysisInputOutput()
            {
                Id = "VerticalPumpDepth",
                DisplayValue = FormatValue(result.Inputs?.VerticalPumpDepth, NULL_TEXT, digits, Length.Foot.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.PumpDepthVertical]),
                Value = result.Inputs?.VerticalPumpDepth != null
                    ? Length.FromFeet(result.Inputs.VerticalPumpDepth.Value).Amount : null,
                MeasurementAbbreviation = Length.Foot.Symbol,
            });

            responseValues.Inputs.Add(new AnalysisInputOutput()
            {
                Id = "MeasuredPumpDepth",
                DisplayValue = FormatValue(result.Inputs?.MeasuredPumpDepth, NULL_TEXT, digits, Length.Foot.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.PumpDepthMeasured]),
                Value = result.Inputs?.MeasuredPumpDepth != null
                    ? Length.FromFeet(result.Inputs.MeasuredPumpDepth.Value).Amount : null,
                MeasurementAbbreviation = Length.Foot.Symbol,
            });

            // Output

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "ProductivityIndex",
                DisplayValue = FormatValue(result.Outputs?.ProductivityIndex, NULL_TEXT, digits, string.Empty),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.ProductivityIndex]),
                Value = result.Outputs?.ProductivityIndex != null ? result.Outputs.ProductivityIndex.Value : null,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "PressureAcrossPumpLength",
                DisplayValue = FormatValue(result.Outputs?.HeadAcrossPump, NULL_TEXT, digits, Length.Foot.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.PressureAcrossPump]),
                Value = result.Outputs?.HeadAcrossPump != null
                    ? Length.FromFeet(result.Outputs.HeadAcrossPump.Value).Amount : null,
                MeasurementAbbreviation = Length.Foot.Symbol,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "PressureAcrossPumpPressure",
                DisplayValue = FormatValue(result.Outputs?.PressureAcrossPump, NULL_TEXT, digits, Pressure.PoundPerSquareInch.Symbol),
                DisplayName = NULL_TEXT,
                Value = result.Outputs?.PressureAcrossPump != null
                    ? Pressure.FromPoundsPerSquareInch(result.Outputs.PressureAcrossPump.Value).Amount : null,
                MeasurementAbbreviation = Pressure.PoundPerSquareInch.Symbol,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "PumpDischargePressure",
                DisplayValue = FormatValue(result.Outputs?.PumpDischargePressure, NULL_TEXT, digits, Pressure.PoundPerSquareInch.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.PumpDischargePressure]),
                Value = result.Outputs?.PumpDischargePressure != null
                    ? Pressure.FromPoundsPerSquareInch(result.Outputs.PumpDischargePressure.Value).Amount : null,
                MeasurementAbbreviation = Pressure.PoundPerSquareInch.Symbol,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "FrictionalLossInTubing",
                DisplayValue = FormatValue(result.Outputs?.FrictionalLossInTubing, NULL_TEXT, digits, Pressure.PoundPerSquareInch.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.Ptr]),
                Value = result.Outputs?.FrictionalLossInTubing != null
                    ? Pressure.FromPoundsPerSquareInch(result.Outputs.FrictionalLossInTubing.Value).Amount : null,
                MeasurementAbbreviation = Pressure.PoundPerSquareInch.Symbol,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "PumpEfficiency",
                DisplayValue = FormatValue(result.Outputs?.PumpEfficiency, NULL_TEXT, digits, Fraction.Percentage.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.PumpEfficiency]),
                Value = result.Outputs?.PumpEfficiency != null
                    ? Fraction.FromPercentage(result.Outputs.PumpEfficiency.Value).Amount : null,
                MeasurementAbbreviation = Fraction.Percentage.Symbol,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "CalculatedFluidLevelAbovePump",
                DisplayValue = FormatValue(result.Outputs?.CalculatedFluidLevelAbovePump, NULL_TEXT, digits, Length.Foot.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.FluidLevelAbovePump]),
                Value = result.Outputs?.CalculatedFluidLevelAbovePump != null
                    ? Length.FromFeet(result.Outputs.CalculatedFluidLevelAbovePump.Value).Amount : null,
                MeasurementAbbreviation = Length.Foot.Symbol,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "FlowingBottomholePressure",
                DisplayValue = FormatValue(result.Outputs?.FlowingBHP, NULL_TEXT, digits, Pressure.PoundPerSquareInch.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.FlowingBHP]),
                Value = result.Outputs?.FlowingBHP != null
                    ? Pressure.FromPoundsPerSquareInch(result.Outputs.FlowingBHP.Value).Amount : null,
                MeasurementAbbreviation = Pressure.PoundPerSquareInch.Symbol,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "PumpDegradation",
                DisplayValue = FormatValue(result.Outputs?.Diagnostics?.PumpDegradation == null ? null : result.Outputs.Diagnostics.PumpDegradation * 100, NULL_TEXT, digits, Fraction.Percentage.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.PumpDegradation]),
                Value = result.Outputs?.Diagnostics?.PumpDegradation != null
                    ? result.Outputs?.Diagnostics?.PumpDegradation * 100
                    : null,
                MeasurementAbbreviation = Fraction.Percentage.Symbol,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "MaximumRunningFrequency",
                DisplayValue = FormatValue(result.Outputs?.MaxRunningFrequency, NULL_TEXT, digits, Frequency.Hertz.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.MaxRunningFrequency]),
                Value = result.Outputs?.MaxRunningFrequency != null ? result.Outputs?.MaxRunningFrequency.Value : null,
                MeasurementAbbreviation = Frequency.Hertz.Symbol,
            });

            responseValues.Outputs.Add(new AnalysisInputOutput()
            {
                Id = "MotorLoadPercentage",
                DisplayValue = FormatValue(result.Outputs?.MotorLoadPercentage, NULL_TEXT, digits, Fraction.Percentage.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.MotorLoad]),
                Value = result.Outputs?.MotorLoadPercentage != null ? result.Outputs.MotorLoadPercentage.Value : null,
                MeasurementAbbreviation = Fraction.Percentage.Symbol,
            });

            string casingValveState;

            if (result.Inputs?.GasHandlingInputs?.IsCasingValveClosed == null)
            {
                casingValveState = NULL_TEXT;
            }
            else
            {
                casingValveState = result.Inputs.GasHandlingInputs.IsCasingValveClosed == true ? "Closed" : "Open";
            }

            responseValues.GasHandlingInputs.Add(new AnalysisInputOutput()
            {
                Id = "CasingValveState",
                DisplayValue = casingValveState,
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.CasingValveState]),
                Value = casingValveState
            });

            responseValues.GasHandlingInputs.Add(new AnalysisInputOutput()
            {
                Id = "SpecificGravityGas",
                DisplayValue = result.Inputs?.GasHandlingInputs != null &&
                    result.Inputs?.GasHandlingInputs?.GasRelativeDensity != null
                        ? $"{MathUtility.RoundToSignificantDigits(result.Inputs.GasHandlingInputs.GasRelativeDensity.Value, digits)}"
                        : NULL_TEXT,
                SourceId = result.Sources.GasSpecificGravitySource,
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.SpecificGravityofGas]),
                Value = result.Inputs?.GasHandlingInputs != null && result.Inputs?.GasHandlingInputs?.GasRelativeDensity != null
                    ? result.Inputs.GasHandlingInputs?.GasRelativeDensity.Value
                    : null,
            });

            responseValues.GasHandlingInputs.Add(new AnalysisInputOutput()
            {
                Id = "BottomholeTemperature",
                DisplayValue = FormatValue(result.Inputs?.GasHandlingInputs?.BottomholeTemperature, NULL_TEXT, digits, Temperature.DegreeFahrenheit.Symbol),
                SourceId = result.Sources.BottomholeTemperatureSource,
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.BottomholeTemperature]),
                Value = result.Inputs?.GasHandlingInputs?.BottomholeTemperature != null
                    ? Temperature.FromDegreesFahrenheit(result.Inputs.GasHandlingInputs.BottomholeTemperature.Value)
                        .Amount : null,
                MeasurementAbbreviation = Temperature.DegreeFahrenheit.Symbol,
            });

            responseValues.GasHandlingInputs.Add(new AnalysisInputOutput()
            {
                Id = "GasSeparatorEfficiency",
                DisplayValue = FormatValue(result.Inputs?.GasHandlingInputs?.GasSeparatorEfficiency, NULL_TEXT, digits, Fraction.Percentage.Symbol),
                SourceId = 2,
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.GasSeparatorEfficiency]),
                Value = result.Inputs?.GasHandlingInputs?.GasSeparatorEfficiency != null
                    ? Fraction.FromPercentage((double)result.Inputs.GasHandlingInputs.GasSeparatorEfficiency).Amount
                    : null,
                MeasurementAbbreviation = Fraction.Percentage.Symbol,
            });

            responseValues.GasHandlingInputs.Add(new AnalysisInputOutput()
            {
                Id = "OilAPI",
                DisplayValue = FormatValue(result.Inputs?.OilAPI, NULL_TEXT, digits, OilRelativeDensity.APIGravity.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.OilAPI]),
                Value = result.Inputs?.OilAPI != null
                    ? OilRelativeDensity.FromDegreesAPI(result.Inputs.OilAPI.Value).Amount : null,
                MeasurementAbbreviation = OilRelativeDensity.APIGravity.Symbol,
            });

            responseValues.GasHandlingInputs.Add(new AnalysisInputOutput()
            {
                Id = "CasingInnerDiameter",
                DisplayValue = FormatValue(result.Inputs?.GasHandlingInputs?.CasingInnerDiameter, NULL_TEXT, digits, Length.Inch.Symbol),
                SourceId = 2,
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.CasingInnerDiameter]),
                Value = result.Inputs?.GasHandlingInputs != null && result.Inputs.GasHandlingInputs.CasingInnerDiameter != null
                    ? Length.FromInches(result.Inputs.GasHandlingInputs.CasingInnerDiameter.Value).Amount : null,
                MeasurementAbbreviation = Length.Inch.Symbol,
            });

            responseValues.GasHandlingInputs.Add(new AnalysisInputOutput()
            {
                Id = "TubingOuterDiameter",
                DisplayValue = FormatValue(result.Inputs?.GasHandlingInputs?.TubingOuterDiameter, NULL_TEXT, digits, Length.Inch.Symbol),
                SourceId = 2,
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.TubingOuterDiameter]),
                MeasurementAbbreviation = Length.Inch.Symbol,
                Value = result.Inputs?.GasHandlingInputs?.TubingOuterDiameter != null
                    ? Length.FromInches(result.Inputs.GasHandlingInputs.TubingOuterDiameter.Value).Amount : null,
            });

            responseValues.GasHandlingOutputs.Add(new AnalysisInputOutput()
            {
                Id = "SolutionGasOilRatio",
                DisplayValue = FormatValue(result.Outputs?.GasHandlingOutputs?.GasOilRatioAtPump, NULL_TEXT, digits, phrases[(int)PhraseIDs.MeasuringUnitSCFPerB]),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.SolutionGOR]),
                MeasurementAbbreviation = phrases[(int)PhraseIDs.MeasuringUnitSCFPerB],
                Value = result.Outputs?.GasHandlingOutputs?.GasOilRatioAtPump != null
                    ? result.Outputs.GasHandlingOutputs.GasOilRatioAtPump.Value : null,
            });

            responseValues.GasHandlingOutputs.Add(new AnalysisInputOutput()
            {
                Id = "SpecificGravityOil",
                DisplayValue = FormatValue(result.Outputs?.GasHandlingOutputs?.OilSpecificGravity, NULL_TEXT, digits, string.Empty),
                SourceId = result.Sources.OilSpecificGravitySource,
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.SpecificGravityofOil]),
                Value = result.Outputs?.GasHandlingOutputs?.OilSpecificGravity != null
                    ? result.Outputs.GasHandlingOutputs.OilSpecificGravity.Value : null,
            });

            responseValues.GasHandlingOutputs.Add(new AnalysisInputOutput()
            {
                Id = "FormationVolumeFactor",
                DisplayValue = FormatValue(result.Outputs?.GasHandlingOutputs?.FormationVolumeFactor, NULL_TEXT, digits, string.Empty),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.FormationVolumeFactor]),
                Value = result.Outputs?.GasHandlingOutputs?.FormationVolumeFactor != null
                    ? result.Outputs.GasHandlingOutputs.FormationVolumeFactor.Value : null,
            });

            responseValues.GasHandlingOutputs.Add(new AnalysisInputOutput()
            {
                Id = "GasCompressibilityFactor",
                DisplayValue = FormatValue(result.Outputs?.GasHandlingOutputs?.GasCompressibilityFactor, NULL_TEXT, digits, string.Empty),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.GasCompressibilityFactor]),
                Value = result.Outputs?.GasHandlingOutputs?.GasCompressibilityFactor != null
                    ? result.Outputs.GasHandlingOutputs.GasCompressibilityFactor.Value : null,
            });

            responseValues.GasHandlingOutputs.Add(new AnalysisInputOutput()
            {
                Id = "GasVolumeFactor",
                DisplayValue = FormatValue(result.Outputs?.GasHandlingOutputs?.GasFormationVolumeFactor, NULL_TEXT, digits, phrases[(int)PhraseIDs.MeasuringUnitBPerMSCF]),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.GasVolumeFactor]),
                MeasurementAbbreviation = phrases[(int)PhraseIDs.MeasuringUnitBPerMSCF],
                Value = result.Outputs?.GasHandlingOutputs?.GasFormationVolumeFactor != null
                    ? result.Outputs.GasHandlingOutputs.GasFormationVolumeFactor.Value : null,
            });

            responseValues.GasHandlingOutputs.Add(new AnalysisInputOutput()
            {
                Id = "ProducingGasOilRatio",
                DisplayValue = FormatValue(result.Outputs?.GasHandlingOutputs?.ProducingGasOilRatio, NULL_TEXT, digits, phrases[(int)PhraseIDs.MeasuringUnitSCFPerB]),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.ProducingGasOilRatio]),
                MeasurementAbbreviation = phrases[(int)PhraseIDs.MeasuringUnitSCFPerB],
                Value = result.Outputs?.GasHandlingOutputs?.ProducingGasOilRatio != null
                    ? result.Outputs.GasHandlingOutputs.ProducingGasOilRatio.Value : null,
            });

            responseValues.GasHandlingOutputs.Add(new AnalysisInputOutput()
            {
                Id = "GasInSolution",
                DisplayValue = FormatValue(result.Outputs?.GasHandlingOutputs?.GasInSolution, NULL_TEXT, digits, GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.GasinSolution]),
                Value = result.Outputs?.GasHandlingOutputs?.GasInSolution != null
                    ? GasFlowRate.FromThousandStandardCubicFeetPerDay(result.Outputs.GasHandlingOutputs.GasInSolution.Value)
                        .Amount : null,
                MeasurementAbbreviation = GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol,
            });

            responseValues.GasHandlingOutputs.Add(new AnalysisInputOutput()
            {
                Id = "FreeGasAtPump",
                DisplayValue = FormatValue(result.Outputs?.GasHandlingOutputs?.FreeGasAtPump, NULL_TEXT, digits, GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.FreeGasatPump]),
                Value = result.Outputs?.GasHandlingOutputs?.FreeGasAtPump != null
                    ? GasFlowRate.FromThousandStandardCubicFeetPerDay(result.Outputs.GasHandlingOutputs.FreeGasAtPump.Value)
                        .Amount : null,
                MeasurementAbbreviation = GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol,
            });

            responseValues.GasHandlingOutputs.Add(new AnalysisInputOutput()
            {
                Id = "OilVolumeAtPump",
                DisplayValue = FormatValue(result.Outputs?.GasHandlingOutputs?.OilVolumeAtPump, NULL_TEXT, digits, LiquidFlowRate.BarrelPerDay.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.OilVolumeatPump]),
                Value = result.Outputs?.GasHandlingOutputs?.OilVolumeAtPump != null
                    ? LiquidFlowRate.FromBarrelsPerDay(result.Outputs.GasHandlingOutputs.OilVolumeAtPump.Value).Amount : null,
                MeasurementAbbreviation = LiquidFlowRate.BarrelPerDay.Symbol,
            });

            responseValues.GasHandlingOutputs.Add(new AnalysisInputOutput()
            {
                Id = "GasVolumeAtPump",
                DisplayValue = FormatValue(result.Outputs?.GasHandlingOutputs?.GasVolumeAtPump, NULL_TEXT, digits, LiquidFlowRate.BarrelPerDay.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.GasVolumeatPump]),
                Value = result.Outputs?.GasHandlingOutputs?.GasVolumeAtPump != null
                    ? LiquidFlowRate.FromBarrelsPerDay(result.Outputs.GasHandlingOutputs.GasVolumeAtPump.Value).Amount : null,
                MeasurementAbbreviation = LiquidFlowRate.BarrelPerDay.Symbol,
            });

            responseValues.GasHandlingOutputs.Add(new AnalysisInputOutput()
            {
                Id = "TotalVolumeAtPump",
                DisplayValue = FormatValue(result.Outputs?.GasHandlingOutputs?.TotalVolumeAtPump, NULL_TEXT, digits, LiquidFlowRate.BarrelPerDay.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.TotalVolumeatPump]),
                MeasurementAbbreviation = LiquidFlowRate.BarrelPerDay.Symbol,
                Value = result.Outputs?.GasHandlingOutputs?.TotalVolumeAtPump != null
                    ? LiquidFlowRate.FromBarrelsPerDay(result.Outputs.GasHandlingOutputs.TotalVolumeAtPump.Value).Amount : null,
            });

            responseValues.GasHandlingOutputs.Add(new AnalysisInputOutput()
            {
                Id = "FreeGas",
                DisplayValue = FormatValue(result.Outputs?.GasHandlingOutputs?.FreeGas, NULL_TEXT, digits, Fraction.Percentage.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.FreeGas]),
                MeasurementAbbreviation = Fraction.Percentage.Symbol,
                Value = result.Outputs?.GasHandlingOutputs?.FreeGas != null
                    ? Fraction.FromPercentage(result.Outputs.GasHandlingOutputs.FreeGas.Value).Amount : null,
            });

            responseValues.GasHandlingOutputs.Add(new AnalysisInputOutput()
            {
                Id = "TurpinParameter",
                DisplayValue = FormatValue(result.Outputs?.GasHandlingOutputs?.TurpinParameter, NULL_TEXT, digits, string.Empty),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.TurpinParameter]),
            });

            responseValues.GasHandlingOutputs.Add(new AnalysisInputOutput()
            {
                Id = "CompositeTubingSpecificGravity",
                DisplayValue = FormatValue(result.Outputs?.GasHandlingOutputs?.CompositeTubingSpecificGravity, NULL_TEXT, digits, string.Empty),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.CompositeTubingSpecificGravity]),
            });

            responseValues.GasHandlingOutputs.Add(new AnalysisInputOutput()
            {
                Id = "GasDensity",
                DisplayValue = FormatValue(result.Outputs?.GasHandlingOutputs?.GasDensity, NULL_TEXT, digits, phrases[(int)PhraseIDs.MeasuringUnitLBPerCF]),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.GasDensity]),
                MeasurementAbbreviation = phrases[(int)PhraseIDs.MeasuringUnitLBPerCF],
            });

            responseValues.GasHandlingOutputs.Add(new AnalysisInputOutput()
            {
                Id = "LiquidDensity",
                DisplayValue = FormatValue(result.Outputs?.GasHandlingOutputs?.LiquidDensity, NULL_TEXT, digits, phrases[(int)PhraseIDs.MeasuringUnitLBPerCF]),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.LiquidDensity]),
                MeasurementAbbreviation = phrases[(int)PhraseIDs.MeasuringUnitLBPerCF],
            });

            responseValues.GasHandlingOutputs.Add(new AnalysisInputOutput()
            {
                Id = "AnnularSeparationEfficiency",
                DisplayValue = FormatValue(result.Outputs?.GasHandlingOutputs?.AnnularSeparationEfficiency, NULL_TEXT, digits, Fraction.Percentage.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.AnnularSeparation]),
                Value = result.Outputs?.GasHandlingOutputs?.AnnularSeparationEfficiency != null
                    ? Fraction.FromPercentage(result.Outputs.GasHandlingOutputs.AnnularSeparationEfficiency.Value)
                    .Amount : null,
                MeasurementAbbreviation = Fraction.Percentage.Symbol,
            });

            responseValues.GasHandlingOutputs.Add(new AnalysisInputOutput()
            {
                Id = "TubingGas",
                DisplayValue = FormatValue(result.Outputs?.GasHandlingOutputs?.TubingGas, NULL_TEXT, digits, GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower(phrases[(int)PhraseIDs.TubingGas]),
                Value = result.Outputs?.GasHandlingOutputs?.TubingGas != null
                    ? GasFlowRate.FromThousandStandardCubicFeetPerDay(result.Outputs.GasHandlingOutputs.TubingGas.Value)
                        .Amount : null,
                MeasurementAbbreviation = GasFlowRate.ThousandStandardCubicFeetPerDay.Symbol,
            });

            responseValues.GasHandlingOutputs.Add(new AnalysisInputOutput()
            {
                Id = "TubingGasOilRatio",
                DisplayValue = FormatValue(result.Outputs?.GasHandlingOutputs?.TubingGasOilRatio, NULL_TEXT, digits, phrases[(int)PhraseIDs.MeasuringUnitSCFPerB]),
                DisplayName = PhraseConverter.ConvertFirstToUpperRestToLower("Tubing Gas Oil Ratio"),
                MeasurementAbbreviation = phrases[(int)PhraseIDs.MeasuringUnitSCFPerB],
            });

            if (string.IsNullOrWhiteSpace(result.ResultMessageLocalized) && result.IsSuccess)
            {
                responseValues.Outputs.Add(new AnalysisInputOutput()
                {
                    Id = "XDIAGAnalysis",
                    DisplayValue = phrases[(int)PhraseIDs.Theselectedwelltestwasprocessedsuccessfully],
                });
            }
            else
            {
                responseValues.Outputs.Add(new AnalysisInputOutput()
                {
                    Id = "XDIAGAnalysis",
                    DisplayValue = result.ResultMessageLocalized,
                    Value = result.ResultMessageLocalized,
                });
            }

            responseValues.IsGasHandlingEnabled = result.IsGasHandlingEnabled && result.Inputs?.GasRate > 0;

            return responseValues;
        }

        private static Pump GetPumpData(ESPPumpDataModel pumpDataModel)
        {
            var id = pumpDataModel.ESPPumpId;

            var pump = new Pump(id)
            {
                MaxDailyVolume = pumpDataModel.MaxBPD,
                MinDailyVolume = pumpDataModel.MinBPD,
                Name = pumpDataModel.Series != null && pumpDataModel.PumpModel != null
                    ? string.Format("{0} {1}", pumpDataModel.Series, pumpDataModel.PumpModel)
                    : pumpDataModel.Pump,
                Series = pumpDataModel.Series,
                PumpModel = pumpDataModel.PumpModel,
                Manufacturer = pumpDataModel.Manufacturer,
                IsCustom = pumpDataModel.ESPPumpId > 10000,
            };

            if (pumpDataModel.BEPBPD.HasValue)
            {
                pump.BEPDailyVolume = pumpDataModel.BEPBPD.Value;
            }

            if (pumpDataModel.MinCasingSize.HasValue)
            {
                pump.MinCasingDiameter = pumpDataModel.MinCasingSize.Value;
            }

            if (pumpDataModel.HousingPressureLimit.HasValue)
            {
                pump.HousingPressureLimit = pumpDataModel.HousingPressureLimit.Value;
            }

            if (pumpDataModel.UseCoefficients.Value)
            {
                var source = new PumpCoefficients();

                if (pump.IsCustom)
                {
                    if (pumpDataModel.HeadIntercept.HasValue && pumpDataModel.Head1Coef.HasValue &&
                        pumpDataModel.Head2Coef.HasValue &&
                        pumpDataModel.Head3Coef.HasValue && pumpDataModel.Head4Coef.HasValue && pumpDataModel.Head5Coef.HasValue)
                    {
                        source.PressureCoefficients = new Coefficients(
                            IdGenerator.GenerateIdForPumpCoefficients(id, PumpCoefficientSource.Pressure))
                        {
                            Intercept = pumpDataModel.HeadIntercept.Value,
                            FirstOrder = pumpDataModel.Head1Coef.Value,
                            SecondOrder = pumpDataModel.Head2Coef.Value,
                            ThirdOrder = pumpDataModel.Head3Coef.Value,
                            FourthOrder = pumpDataModel.Head4Coef.Value,
                            FifthOrder = pumpDataModel.Head5Coef ?? 0,
                            SixthOrder = pumpDataModel.Head6Coef ?? 0,
                            SeventhOrder = pumpDataModel.Head7Coef ?? 0,
                            EighthOrder = pumpDataModel.Head8Coef ?? 0,
                            NinthOrder = pumpDataModel.Head9Coef ?? 0,
                        };
                    }

                    if (pumpDataModel.HPIntercept.HasValue && pumpDataModel.HP1Coef.HasValue && pumpDataModel.HP2Coef.HasValue &&
                        pumpDataModel.HP3Coef.HasValue && pumpDataModel.HP4Coef.HasValue && pumpDataModel.HP5Coef.HasValue)
                    {
                        source.PowerCoefficients = new Coefficients(
                            IdGenerator.GenerateIdForPumpCoefficients(id, PumpCoefficientSource.Power))
                        {
                            Intercept = pumpDataModel.HPIntercept.Value,
                            FirstOrder = pumpDataModel.HP1Coef.Value,
                            SecondOrder = pumpDataModel.HP2Coef.Value,
                            ThirdOrder = pumpDataModel.HP3Coef.Value,
                            FourthOrder = pumpDataModel.HP4Coef.Value,
                            FifthOrder = pumpDataModel.HP5Coef.Value,
                            SixthOrder = pumpDataModel.HP6Coef ?? 0,
                            SeventhOrder = pumpDataModel.HP7Coef ?? 0,
                            EighthOrder = pumpDataModel.HP8Coef ?? 0,
                            NinthOrder = pumpDataModel.HP9Coef ?? 0,
                        };
                    }

                    if (pumpDataModel.EfficiencyIntercept.HasValue && pumpDataModel.Eff1Coef.HasValue &&
                        pumpDataModel.Eff2Coef.HasValue &&
                        pumpDataModel.Eff3Coef.HasValue && pumpDataModel.Eff4Coef.HasValue && pumpDataModel.Eff5Coef.HasValue)
                    {
                        source.EfficiencyCoefficients = new Coefficients(
                            IdGenerator.GenerateIdForPumpCoefficients(id, PumpCoefficientSource.Efficiency))
                        {
                            Intercept = pumpDataModel.EfficiencyIntercept.Value,
                            FirstOrder = pumpDataModel.Eff1Coef.Value,
                            SecondOrder = pumpDataModel.Eff2Coef.Value,
                            ThirdOrder = pumpDataModel.Eff3Coef.Value,
                            FourthOrder = pumpDataModel.Eff4Coef.Value,
                            FifthOrder = pumpDataModel.Eff5Coef.Value,
                            SixthOrder = pumpDataModel.Eff6Coef ?? 0,
                            SeventhOrder = pumpDataModel.Eff7Coef ?? 0,
                            EighthOrder = pumpDataModel.Eff8Coef ?? 0,
                            NinthOrder = pumpDataModel.Eff9Coef ?? 0,
                        };
                    }
                }
                else
                {
                    var storedCoefficients = pumpDataModel.Data == null
                        ? null
                        : Deserialize(EncryptionUtility.Decrypt(pumpDataModel.Data));

                    Coefficients coefficients;
                    if (storedCoefficients != null
                        && storedCoefficients.TryGetValue("PressureCoefficients", out var pressureCoefficients)
                        && pressureCoefficients != null)
                    {
                        coefficients = pressureCoefficients;

                        source.PressureCoefficients = new Coefficients(
                            IdGenerator.GenerateIdForPumpCoefficients(id, PumpCoefficientSource.Pressure))
                        {
                            Intercept = coefficients.Intercept,
                            FirstOrder = coefficients.FirstOrder,
                            SecondOrder = coefficients.SecondOrder,
                            ThirdOrder = coefficients.ThirdOrder,
                            FourthOrder = coefficients.FourthOrder,
                            FifthOrder = coefficients.FifthOrder,
                            SixthOrder = coefficients.SixthOrder,
                            SeventhOrder = coefficients.SeventhOrder,
                            EighthOrder = coefficients.EighthOrder,
                            NinthOrder = coefficients.NinthOrder,
                        };
                    }

                    if (storedCoefficients != null
                        && storedCoefficients.TryGetValue("PowerCoefficients", out var powerCoefficients)
                        && powerCoefficients != null)
                    {
                        coefficients = powerCoefficients;

                        source.PowerCoefficients = new Coefficients(
                            IdGenerator.GenerateIdForPumpCoefficients(id, PumpCoefficientSource.Power))
                        {
                            Intercept = coefficients.Intercept,
                            FirstOrder = coefficients.FirstOrder,
                            SecondOrder = coefficients.SecondOrder,
                            ThirdOrder = coefficients.ThirdOrder,
                            FourthOrder = coefficients.FourthOrder,
                            FifthOrder = coefficients.FifthOrder,
                            SixthOrder = coefficients.SixthOrder,
                            SeventhOrder = coefficients.SeventhOrder,
                            EighthOrder = coefficients.EighthOrder,
                            NinthOrder = coefficients.NinthOrder,
                        };
                    }

                    if (storedCoefficients != null
                        && storedCoefficients.TryGetValue("EfficiencyCoefficients", out var efficiencyCoefficients)
                        && efficiencyCoefficients != null)
                    {
                        coefficients = efficiencyCoefficients;

                        source.EfficiencyCoefficients = new Coefficients(
                            IdGenerator.GenerateIdForPumpCoefficients(id, PumpCoefficientSource.Efficiency))
                        {
                            Intercept = coefficients.Intercept,
                            FirstOrder = coefficients.FirstOrder,
                            SecondOrder = coefficients.SecondOrder,
                            ThirdOrder = coefficients.ThirdOrder,
                            FourthOrder = coefficients.FourthOrder,
                            FifthOrder = coefficients.FifthOrder,
                            SixthOrder = coefficients.SixthOrder,
                            SeventhOrder = coefficients.SeventhOrder,
                            EighthOrder = coefficients.EighthOrder,
                            NinthOrder = coefficients.NinthOrder,
                        };
                    }
                }

                pump.PerformanceSource = source;
            }
            else if (pumpDataModel.CurvePoints != null)
            {
                var source = new PumpCurvePointList();

                foreach (var curvePoint in pumpDataModel.CurvePoints)
                {
                    var point = new PumpCurvePoint(curvePoint.GenerateId())
                    {
                        FlowRate = curvePoint.FlowRate,
                        Head = curvePoint.HeadFeetPerStage,
                        Power = curvePoint.PowerInHP,
                        EfficiencyPercent = (float?)curvePoint.Efficiency,
                    };

                    source.Add(point);
                }

                pump.PerformanceSource = source;
            }

            return pump;
        }

        private static IDictionary<string, Coefficients> Deserialize(string json)
        {
            var data = new Dictionary<string, Coefficients>();
            if (json == "")
            {
                return data;
            }

            using (var memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                var obj = Activator.CreateInstance(data.GetType());
                var serializer = new DataContractJsonSerializer(obj.GetType());
                return (Dictionary<string, Coefficients>)serializer.ReadObject(memoryStream);
            }
        }

        private CurveCoordinateDataOutput GetESPAnalysisCurveResponse(AnalysisResult result,
            ESPAnalysisResultModel analysisResult, string correlationId)
        {
            var digits = _commonService.GetSystemParameterNextGenSignificantDigits(correlationId);

            var responseValues = new List<CurveCoordinateValues>();

            if (result.Outputs.AnalysisCurves != null)
            {
                foreach (var curve in result.Outputs.AnalysisCurves)
                {
                    responseValues.Add(new CurveCoordinateValues()
                    {
                        Id = (int)curve.Id,
                        CurveTypeId = curve.GetCurveTypeId(),
                        DisplayName = EnhancedEnumBase.GetValue<ESPCurveType>(curve.GetCurveTypeId()),
                        Coordinates = curve.GetCurveCoordinateList().Select(x => new CoordinatesData<double>()
                        {
                            X = MathUtility.RoundToSignificantDigits(x.XValue, digits),
                            Y = MathUtility.RoundToSignificantDigits(x.YValue, digits),
                        }).ToList(),
                    });
                }
            }

            if (result.Outputs.EfficiencyAnalysisCurve != null)
            {
                responseValues.Add(new CurveCoordinateValues()
                {
                    Id = (int)result.Outputs.EfficiencyAnalysisCurve.Id,
                    CurveTypeId = result.Outputs.EfficiencyAnalysisCurve.GetCurveTypeId(),
                    DisplayName =
                        EnhancedEnumBase.GetValue<ESPCurveType>(result.Outputs.EfficiencyAnalysisCurve.GetCurveTypeId()),
                    Coordinates = result.Outputs.EfficiencyAnalysisCurve.GetCurveCoordinateList().Select(x =>
                        new CoordinatesData<double>()
                        {
                            X = MathUtility.RoundToSignificantDigits(x.XValue, digits),
                            Y = MathUtility.RoundToSignificantDigits(x.YValue, digits),
                        }).ToList(),
                });
            }

            if (result.Outputs.PowerAnalysisCurve != null)
            {
                responseValues.Add(new CurveCoordinateValues()
                {
                    Id = (int)result.Outputs.PowerAnalysisCurve.Id,
                    CurveTypeId = result.Outputs.PowerAnalysisCurve.GetCurveTypeId(),
                    DisplayName = EnhancedEnumBase.GetValue<ESPCurveType>(result.Outputs.PowerAnalysisCurve.GetCurveTypeId()),
                    Coordinates = result.Outputs.PowerAnalysisCurve.GetCurveCoordinateList()
                        .Select(x => new CoordinatesData<double>()
                        {
                            X = MathUtility.RoundToSignificantDigits(x.XValue, digits),
                            Y = MathUtility.RoundToSignificantDigits(x.YValue, digits),
                        }).ToList(),
                });
            }

            if (result.Outputs.PumpAnalysisCurve != null)
            {
                responseValues.Add(new CurveCoordinateValues()
                {
                    Id = (int)result.Outputs.PumpAnalysisCurve.Id,
                    CurveTypeId = result.Outputs.PumpAnalysisCurve.GetCurveTypeId(),
                    DisplayName = EnhancedEnumBase.GetValue<ESPCurveType>(result.Outputs.PumpAnalysisCurve.GetCurveTypeId()),
                    Coordinates = result.Outputs.PumpAnalysisCurve.GetCurveCoordinateList()
                        .Select(x => new CoordinatesData<double>()
                        {
                            X = MathUtility.RoundToSignificantDigits(x.XValue, digits),
                            Y = MathUtility.RoundToSignificantDigits(x.YValue, digits),
                        }).ToList(),
                });
            }

            if (result.Outputs.RecommendedRangeBottomAnalysisCurve != null)
            {
                responseValues.Add(new CurveCoordinateValues()
                {
                    Id = (int)result.Outputs.RecommendedRangeBottomAnalysisCurve.Id,
                    CurveTypeId = result.Outputs.RecommendedRangeBottomAnalysisCurve.GetCurveTypeId(),
                    DisplayName =
                        EnhancedEnumBase.GetValue<ESPCurveType>(
                            result.Outputs.RecommendedRangeBottomAnalysisCurve.GetCurveTypeId()),
                    Coordinates = result.Outputs.RecommendedRangeBottomAnalysisCurve.GetCurveCoordinateList()
                        .Select(x => new CoordinatesData<double>()
                        {
                            X = MathUtility.RoundToSignificantDigits(x.XValue, digits),
                            Y = MathUtility.RoundToSignificantDigits(x.YValue, digits),
                        }).ToList(),
                });
            }

            if (result.Outputs.RecommendedRangeLeftAnalysisCurve != null)
            {
                responseValues.Add(new CurveCoordinateValues()
                {
                    Id = (int)result.Outputs.RecommendedRangeLeftAnalysisCurve.Id,
                    CurveTypeId = result.Outputs.RecommendedRangeLeftAnalysisCurve.GetCurveTypeId(),
                    DisplayName =
                        EnhancedEnumBase.GetValue<ESPCurveType>(result.Outputs.RecommendedRangeLeftAnalysisCurve
                            .GetCurveTypeId()),
                    Coordinates = result.Outputs.RecommendedRangeLeftAnalysisCurve.GetCurveCoordinateList().Select(x =>
                        new CoordinatesData<double>()
                        {
                            X = MathUtility.RoundToSignificantDigits(x.XValue, digits),
                            Y = MathUtility.RoundToSignificantDigits(x.YValue, digits),
                        }).ToList(),
                });
            }

            if (result.Outputs.RecommendedRangeRightAnalysisCurve != null)
            {
                responseValues.Add(new CurveCoordinateValues()
                {
                    Id = (int)result.Outputs.RecommendedRangeRightAnalysisCurve.Id,
                    CurveTypeId = result.Outputs.RecommendedRangeRightAnalysisCurve.GetCurveTypeId(),
                    DisplayName =
                        EnhancedEnumBase.GetValue<ESPCurveType>(
                            result.Outputs.RecommendedRangeRightAnalysisCurve.GetCurveTypeId()),
                    Coordinates = result.Outputs.RecommendedRangeRightAnalysisCurve.GetCurveCoordinateList().Select(x =>
                        new CoordinatesData<double>()
                        {
                            X = MathUtility.RoundToSignificantDigits(x.XValue, digits),
                            Y = MathUtility.RoundToSignificantDigits(x.YValue, digits),
                        }).ToList(),
                });
            }

            if (result.Outputs.RecommendedRangeTopAnalysisCurve != null)
            {
                responseValues.Add(new CurveCoordinateValues()
                {
                    Id = (int)result.Outputs.RecommendedRangeTopAnalysisCurve.Id,
                    CurveTypeId = result.Outputs.RecommendedRangeTopAnalysisCurve.GetCurveTypeId(),
                    DisplayName =
                        EnhancedEnumBase.GetValue<ESPCurveType>(result.Outputs.RecommendedRangeTopAnalysisCurve.GetCurveTypeId()),
                    Coordinates = result.Outputs.RecommendedRangeTopAnalysisCurve.GetCurveCoordinateList().Select(x =>
                        new CoordinatesData<double>()
                        {
                            X = MathUtility.RoundToSignificantDigits(x.XValue, digits),
                            Y = MathUtility.RoundToSignificantDigits(x.YValue, digits),
                        }).ToList(),
                });
            }

            if (result.Outputs.WellPerformanceAnalysisCurve != null)
            {
                responseValues.Add(new CurveCoordinateValues()
                {
                    Id = (int)result.Outputs.WellPerformanceAnalysisCurve.Id,
                    CurveTypeId = result.Outputs.WellPerformanceAnalysisCurve.GetCurveTypeId(),
                    DisplayName =
                        EnhancedEnumBase.GetValue<ESPCurveType>(result.Outputs.WellPerformanceAnalysisCurve.GetCurveTypeId()),
                    Coordinates = result.Outputs.WellPerformanceAnalysisCurve.GetCurveCoordinateList().Select(x =>
                        new CoordinatesData<double>()
                        {
                            X = MathUtility.RoundToSignificantDigits(x.XValue, digits),
                            Y = MathUtility.RoundToSignificantDigits(x.YValue, digits),
                        }).ToList(),
                });
            }

            if (result.Outputs.TornadoCurves != null)
            {
                foreach (var curve in result.Outputs.TornadoCurves.Curves)
                {
                    var value = new CurveCoordinateValues()
                    {
                        Id = (int)curve.CurveSetMemberId,
                        CurveTypeId = result.Outputs.TornadoCurves.CurveSetType.Key,
                        Name = result.Outputs.TornadoCurves.CurveSetType.Name,
                        DisplayName = $"{(curve.AnnotationData as TornadoCurveAnnotation)?.Frequency}",
                        Coordinates = curve.GetCurveCoordinateList()
                            .Select(x => new CoordinatesData<double>()
                            {
                                X = MathUtility.RoundToSignificantDigits(x.XValue, digits),
                                Y = MathUtility.RoundToSignificantDigits(x.YValue, digits),
                            }).ToList(),
                    };

                    responseValues.Add(value);
                }
            }

            if ((analysisResult.TotalVolumeAtPump != null || analysisResult.GrossRate != null) && analysisResult.HeadAcrossPump != null)
            {
                var head = MathUtility.RoundToSignificantDigits(analysisResult.HeadAcrossPump.Value, digits);

                double grossRate = analysisResult.TotalVolumeAtPump != null
                    ? analysisResult.TotalVolumeAtPump.Value
                    : analysisResult.GrossRate.Value;
                grossRate = MathUtility.RoundToSignificantDigits(grossRate, digits);

                responseValues.Add(new CurveCoordinateValues()
                {
                    Id = -6,
                    CurveTypeId = -6,
                    Name = "OperatingPoint",
                    DisplayName = "Operating Point",
                    Coordinates = new List<CoordinatesData<double>>()
                    {
                        new CoordinatesData<double>()
                        {
                            X = grossRate,
                            Y = head,
                        },
                    },
                });
            }

            var response = new CurveCoordinateDataOutput()
            {
                Values = responseValues,
                Result = new MethodResult<string>(true, "Curve Coordinate Data"),
            };

            return response;
        }

        private AnalysisCurve CreateAnalysisCurve(int curveId, int curveTypeId, IndustryApplication industryApplication)
        {
            const int IPR_CURVE_FOR_ESP = 18;
            const int IPR_CURVE_FOR_GAS_LIFT = 19;
            const int IPR_CURVE_FOR_ROD_PUMP = 20;
            const int IPR_CURVE_FOR_PCP = 21;
            const int IPR_CURVE_FOR_PLUNGER_LIFT = 22;
            const int IPR_CURVE_FOR_JET_PUMP = 23;
            const int IPR_CURVE_FOR_PLUNGER_ASSISTED_GAS_LIFT = 24;

            const int TEMPERATURE_SURVEY_CURVE = 25;
            const int PRESSURE_SURVEY_CURVE = 26;

            var curveIsIPR = false;
            var curveIsSurveyCurve = false;

            if (curveTypeId == IPR_CURVE_FOR_ESP ||
                curveTypeId == IPR_CURVE_FOR_GAS_LIFT ||
                curveTypeId == IPR_CURVE_FOR_ROD_PUMP ||
                curveTypeId == IPR_CURVE_FOR_PCP ||
                curveTypeId == IPR_CURVE_FOR_PLUNGER_LIFT ||
                curveTypeId == IPR_CURVE_FOR_JET_PUMP ||
                curveTypeId == IPR_CURVE_FOR_PLUNGER_ASSISTED_GAS_LIFT)
            {
                curveIsIPR = true;
            }

            if (curveTypeId == TEMPERATURE_SURVEY_CURVE ||
                curveTypeId == PRESSURE_SURVEY_CURVE)
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

        private void MapToDomain(IList<ESPTornadoCurveSetAnnotationModel> annotationEntities, AnalysisCurveSet curveSet)
        {
            if (annotationEntities == null)
            {
                return;
            }

            foreach (var entity in annotationEntities)
            {
                var annotation = new TornadoCurveAnnotation()
                {
                    Frequency = entity.Frequency,
                };

                var curve = curveSet.Curves.FirstOrDefault(c => c.CurveSetMemberId == entity.CurveSetMemberId);
                if (curve != null)
                {
                    curve.AnnotationData = annotation;
                }
            }
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

        #endregion

    }
}
