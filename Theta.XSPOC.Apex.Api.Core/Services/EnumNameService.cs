using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;
using UnitCategory = Theta.XSPOC.Apex.Api.Core.Common.UnitCategory;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Initializes names for enum types.
    /// </summary>
    public class EnumNameService : IEnumNameService
    {

        #region Private Enumerations

        private enum EnumTable
        {

            tblApplications,
            tblCorrelations,
            tblCorrelationTypes,
            tblAnalysisTypes,
            tblAnalysisCurveSetTypes,
            tblUnitTypes,

        }

        private enum PhraseId
        {

            ConnectionFailed = 4638,
            CRCError = 4639,
            Failure = 151,
            None = 361,
            OK = 2988,
            Timeout = 1394,
            Unsupported = 4640,
            NumberOfStages = 2953,
            VerticalPumpDepth = 2954,
            MeasuredPumpDepth = 2955,
            OilRate = 532,
            WaterRate = 1250,
            GasRate = 1251,
            PumpIntakePressure = 2789,
            GrossRate = 533,
            FluidLevelAbovePump = 5086,
            WellheadPressure = 4867,
            CasingPressure = 265,
            Frequency = 1414,
            ProductivityIndex = 4603,
            PressureAcrossPump = 5106,
            PumpDischargePressure = 2790,
            HeadAcrossPump = 5556,
            FrictionalLossInTubing = 5099,
            PumpEfficiency = 1164,
            CalculatedFluidLevelAbovePump = 5557,
            FluidSpecificGravity = 5558,
            PumpStaticPressure = 5097,
            RateAtBubblePoint = 4804,
            RateAtMaxOil = 4089,
            RateAtMaxLiquid = 4805,
            IPRSlope = 4806,
            WaterCut = 108,
            GasOilRatioAtPump = 4832,
            FormationVolumeFactor = 4833,
            GasCompressibilityFactor = 4834,
            GasVolumeFactor = 4835,
            ProducingGOR = 4839,
            GasInSolution = 4840,
            FreeGasAtPump = 4841,
            OilVolumeAtPump = 4842,
            GasVolumeAtPump = 4843,
            TotalVolumeAtPump = 5593,
            FreeGas = 5594,
            CompositeTubingSpecificGravity = 5596,
            GasDensity = 5597,
            LiquidDensity = 5598,
            AnnularSeparationEfficiency = 5599,
            TubingGas = 5600,
            TubingGOR = 5601,
            HeadRelativeToRecommendedRange = 6061,
            FlowRelativeToRecommendedRange = 6062,
            DesignScore = 6063,
            HeadRelativeToWellPerformance = 6064,
            HeadRelativeToPumpCurve = 6065,
            PumpDegradation = 5985,
            MaxPotentialProductionRate = 6066,
            MaxPotentialFrequency = 6067,
            ProductionIncreasePercentage = 6068,
            Modbus = 4768,
            ModbusTCP = 4907,
            ModbusEthernet = 4908,
            OPC = 4909,
            PercentCO2 = 5209,
            PercentH2S = 5223,
            PercentN2 = 5210,
            GasSpecificGravity = 4918,
            OilAPIGravity = 268,
            WaterSpecificGravity = 267,
            BottomholeTemperature = 4831,
            MinimumFlowingBHP = 5770,
            GLRForOptimumLiquidRate = 5775,
            InjectionRateForOptimumLiquidRate = 5776,
            OptimumLiquidRate = 5774,
            GLRForMaxLiquidRate = 5772,
            InjectionRateForMaxLiquidRate = 5773,
            InjectedGasRate = 5758,
            InjectedGLR = 5791,
            FlowingBottomHolePressure = 5100,
            WellheadTemperature = 5766,
            TubingOuterDiameter = 493,
            TubingInnerDiameter = 5081,
            VerticalWellDepth = 5798,
            MeasuredWellDepth = 5799,
            GasInjectionDepth = 5873,
            ValveStateOpen = 5987,
            ValveStateClosed = 5988,
            ValveStateUnstable = 5989,
            OrificeStateOpen = 5993,
            ValveStatusUnknown = 5994,
            ValveCriticalVelocity = 6049,
            TubingCriticalVelocity = 6047,
            MaxLiquidRate = 5771,
            Discrete = 732,
            Analog = 6164, //currently set to "User Limit"
            ParameterOption = 6159,
            Span = 6160,
            RateOfChange = 6161,
            SPC = 3745,
            NearPumpOff = 6162,
            NoData = 6346,
            Communication = 49,
            Camera = 6037,
            ValueChange = 6163,
            LessThanLimit = 6292,
            GreaterThanLimit = 6291,
            High = 1102,
            Low = 1103,
            VeryHigh = 4920,
            VeryLow = 4921,
            BasicInput = 6365, // Basic Input
            UseDesignData = 6366, // Use Design Data
            QuickInput = 6369, // Quick Input
            Integer = 6506, // Integer
            Real = 6507, // Real
            Boolean = 6508, // Boolean
            String = 6509, // String
            SurfaceCardAnalysis = 6510, // Surface Card Analysis
            TaggingPercentConsideredUpstroke = 6511, // Tagging Percent Considered - Upstroke
            TaggingPercentConsideredDownstroke = 6512, // Tagging Percent Considered - Downstroke
            TaggingPercentChangeUpstrokeThreshold = 6513, // Tagging Percent Change Upstroke Threshold
            TaggingPercentChangeDownstrokeThreshold = 6514, // Tagging Percent Change Downstroke Threshold
            TaggingSlopeUpstrokeThreshold = 6515, // Tagging Slope Upstroke Threshold
            TaggingSlopeDownstrokeThreshold = 6516, // Tagging Slope Downstroke Threshold
            ShallowFrictionPercentConsideredBottom = 6517, // Shallow Friction Percent Considered - Bottom
            ShallowFrictionPercentConsideredTop = 6518, // Shallow Friction Percent Considered - Top
            ShallowFrictionBottomThreshold = 6519, // Shallow Friction Bottom Threshold
            ShallowFrictionTopThreshold = 6520, // Shallow Friction Top Threshold
            BadCardRoughnessThreshold = 6521, // Bad Card Roughness Threshold
            BrokenShaft = 6627, // Broken Shaft 
            WaterCutIncrease = 6628, // Water Cut Increase   
            IncreasedInflow = 6629, // Increased Inflow    
            PressureIncrease = 6630, // Pressure Increase    
            TemperatureIncrease = 6631, // Temperature Increase 
            BlockedPumpIntake = 6632, // Blocked Pump Intake  
            ReducedInflow = 6633, // Reduced Inflow  
            WearingStages = 6634, // Wearing Stages  
            HoleInTubing = 6635, // Hole In Tubing 
            BlockedPumpStages = 6636, // Blocked Pump Stages  
            GasSlugging = 6637, // Gas Slugging 
            GasIncrease = 6638, // Gas Increase 
            BackpressureIncrease = 6639, // Backpressure Increase
            BackpressureDecrease = 6640, // Backpressure Decrease
            Cycling = 6641, // Cycling 
            HighGasIngestion = 6642, // High Gas Ingestion   
            HighDrawdown = 6643, // High Drawdown 
            LongShutdown = 6644, // Long Shutdown 
            BadPIPSensor = 6645, // Bad PIP Sensor 
            BadTempSensor = 6646, // Bad Temp Sensor 
            All = 425, // All
            Unacknowledged = 6796, // Unacknowledged
            Active = 6797, // Active
            Suppressed = 6798, // Suppressed
            ExactValue = 6785, // Exact Value

        }

        #endregion

        private readonly ILocalePhrases _localePhrases;

        private readonly IEnumEntity _enumEntityModel;

        private IDictionary<int, Text> _phrases;

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="EnumNameService"/>.
        /// </summary>
        /// <param name="localePhrases">The <seealso cref="ILocalePhrases"/> service.</param>
        /// <param name="enumEntityModel">The <seealso cref="IEnumEntity"/> service.</param>
        /// <exception cref="ArgumentNullException">
        /// or
        /// </exception>
        public EnumNameService(ILocalePhrases localePhrases, IEnumEntity enumEntityModel)
        {
            _localePhrases = localePhrases ?? throw new ArgumentNullException(nameof(localePhrases));
            _enumEntityModel = enumEntityModel ?? throw new ArgumentNullException(nameof(enumEntityModel));
        }

        #endregion

        #region IEnumNameService Members

        /// <summary>
        /// Initializes names for enum types.
        /// </summary>
        public void Initialize()
        {
            InitializeEnhancedEnum();

            _phrases = _localePhrases.GetAll(string.Empty, Enum.GetValues(typeof(PhraseId)).Cast<int>().ToArray())
                .ToDictionary(x => x.Key, x => Text.FromString(x.Value));

            EnhancedEnumBase.SetUnsupportedName(_phrases[(int)PhraseId.Unsupported]);

            InitializeCurveTypes();
            InitializeFlowControlDeviceStates();
            InitializeValveConfigurationOptions();

            IList<EnumEntityModel> entities = _enumEntityModel.GetApplicationEntities();

            entities = entities.Concat(_enumEntityModel.GetCorrelationEntities()).ToList();
            entities = entities.Concat(_enumEntityModel.GetCorrelationTypeEntities()).ToList();
            entities = entities.Concat(_enumEntityModel.GetAnalysisTypeEntities()).ToList();
            entities = entities.Concat(_enumEntityModel.GetAnalysisCurveSetTypes()).ToList();
            entities = entities.Concat(_enumEntityModel.GetUnitCategories()).ToList();

            if (entities.Any())
            {
                int[] phraseIds = entities.Select(e => e.PhraseId)
                    .Distinct()
                    .ToArray();
                IDictionary<int, Text> phrases =
                    _localePhrases.GetAll(string.Empty, phraseIds).ToDictionary(x => x.Key, x => Text.FromString(x.Value));

                foreach (EnumEntityModel entity in entities)
                {
                    Text phrase = phrases[entity.PhraseId];

                    switch ((EnumTable)entity.Table)
                    {
                        case EnumTable.tblApplications:
                            EnhancedEnumBase.SetName<IndustryApplication>(entity.Id, phrase);
                            break;
                        case EnumTable.tblCorrelations:
                            EnhancedEnumBase.SetName<Correlation>(entity.Id, phrase);
                            break;
                        case EnumTable.tblCorrelationTypes:
                            EnhancedEnumBase.SetName<CorrelationType>(entity.Id, phrase);
                            break;
                        case EnumTable.tblAnalysisTypes:
                            EnhancedEnumBase.SetName<AnalysisType>(entity.Id, phrase);
                            break;
                        case EnumTable.tblAnalysisCurveSetTypes:
                            EnhancedEnumBase.SetName<CurveSetType>(entity.Id, phrase);
                            break;
                        case EnumTable.tblUnitTypes:
                            EnhancedEnumBase.SetName<UnitCategory>(entity.Id, phrase);
                            break;
                        default:
                            break;
                    } //switch entity.Table
                } //foreach entity
            } //if cachedEntities.Any()

            _ = entities.ToList(); //force execution
        }

        private void InitializeValveConfigurationOptions()
        {
            IDictionary<int, int> phraseIdsByOutputId;

            phraseIdsByOutputId = _enumEntityModel.GetGLValveConfigurationOption();

            var phrases = _localePhrases.GetAll(string.Empty, phraseIdsByOutputId.Values.ToArray());

            foreach (var pair in phraseIdsByOutputId)
            {
                EnhancedEnumBase.SetName<ValveConfigurationOption>(pair.Key, Text.FromString(phrases[pair.Value]));
            }
        }

        private void InitializeFlowControlDeviceStates()
        {
            IDictionary<int, int> phraseIdsByOutputId;

            phraseIdsByOutputId = _enumEntityModel.GetGLFlowControlDeviceState();

            var phrases = _localePhrases.GetAll(string.Empty, phraseIdsByOutputId.Values.ToArray());

            foreach (var pair in phraseIdsByOutputId)
            {
                if (EnhancedEnumBase.GetValues<ValveState>().Any(e => e.Key == pair.Key))
                {
                    EnhancedEnumBase.SetName<ValveState>(pair.Key, Text.FromString(phrases[pair.Value]));
                }

                if (EnhancedEnumBase.GetValues<OrificeState>().Any(e => e.Key == pair.Key))
                {
                    EnhancedEnumBase.SetName<OrificeState>(pair.Key, Text.FromString(phrases[pair.Value]));
                }
            }
        }

        private void InitializeCurveTypes()
        {
            InitializeCurveType<GLCurveType>(IndustryApplication.GasArtificialLift);
            InitializeCurveType<ESPCurveType>(IndustryApplication.ESPArtificialLift);
            InitializeCurveType<IPRCurveType>(IndustryApplication.GasArtificialLift);
            InitializeCurveType<IPRCurveType>(IndustryApplication.ESPArtificialLift);
            InitializeCurveType<IPRCurveType>(IndustryApplication.RodArtificialLift);
            InitializeCurveType<IPRCurveType>(IndustryApplication.PCPArtificialLift);
            InitializeCurveType<IPRCurveType>(IndustryApplication.PlungerArtificialLift);
            InitializeCurveType<IPRCurveType>(IndustryApplication.JetPumpArtificialLift);
            InitializeCurveType<IPRCurveType>(IndustryApplication.PlungerAssistedGasArtificialLift);
        }

        private void InitializeCurveType<T>(IndustryApplication application) where T : EnhancedEnumBase, ICurveType
        {
            IDictionary<int, int> phraseIdsByOutputId;

            phraseIdsByOutputId = _enumEntityModel.GetCurveTypes(application.Key);

            var phrases = _localePhrases.GetAll(string.Empty, phraseIdsByOutputId.Values.ToArray());

            foreach (var pair in phraseIdsByOutputId)
            {
                EnhancedEnumBase.SetName<T>(pair.Key, Text.FromString(phrases[pair.Value]));
            }
        }

        /// <summary>
        /// There is an issue with the EnhancedEnum creating the dictionary and populating the name. This "hack" resolves it.
        /// Not going to spend more time researching because there is a good chance that we will stop using EnhancedEnumBase.
        /// The EnhancedEnumBase is created Application-wide; however, the phrases, need to come from a user's settings calling the API. 
        /// </summary>
        private void InitializeEnhancedEnum()
        {
            _ = GLCurveType.ProductionPerformanceCurve;
            _ = IPRCurveType.ESPIPRCurve;
            _ = SurveyCurveType.TemperatureCurve;
            _ = ValveState.Open;
            _ = AnalysisResultSource.GasLift;
            _ = AnalysisType.WellTest;
            _ = ComparisonOperator.Equal;
            _ = Correlation.HagedornAndBrown;
            _ = CorrelationType.MultiphaseFlow;
            _ = CurveSetType.Tornado;
            _ = ESPAnalysisPhrasePlaceholder.CalculatedFluidLevelAbovePump;
            _ = ESPCurveType.PumpCurve;
            _ = IndustryApplication.None;
            _ = UnitCategory.None;
            _ = GLAnalysisPhrasePlaceholder.VerticalInjectionDepthFromValveAnalysis;
        }

        #endregion

    }
}
