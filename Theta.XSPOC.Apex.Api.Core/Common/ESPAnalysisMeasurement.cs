using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the types of measurements used in ESP Analysis.
    /// </summary>
    public class ESPAnalysisMeasurement : EnhancedEnumBase
    {

        #region Properties

        /// <summary>
        /// Gets the unit category associated with this measurement.
        /// </summary>
        public UnitCategory UnitCategory { get; }

        #endregion

        #region Static Properties

        /// <summary>
        /// Specifies the type of measurement is 'Number Of Stages'.
        /// </summary>
        public static ESPAnalysisMeasurement NumberOfStages { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Vertical Pump Depth'.
        /// </summary>
        public static ESPAnalysisMeasurement VerticalPumpDepth { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Measured Pump Depth'.
        /// </summary>
        public static ESPAnalysisMeasurement MeasuredPumpDepth { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Oil Rate'.
        /// </summary>
        public static ESPAnalysisMeasurement OilRate { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Water Rate'.
        /// </summary>
        public static ESPAnalysisMeasurement WaterRate { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Gas Rate'.
        /// </summary>
        public static ESPAnalysisMeasurement GasRate { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Pump Intake Pressure'.
        /// </summary>
        public static ESPAnalysisMeasurement PumpIntakePressure { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Gross Rate'.
        /// </summary>
        public static ESPAnalysisMeasurement GrossRate { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Fluid Level Above Pump'.
        /// </summary>
        public static ESPAnalysisMeasurement FluidLevelAbovePump { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Tubing Pressure'.
        /// </summary>
        public static ESPAnalysisMeasurement TubingPressure { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Casing Pressure'.
        /// </summary>
        public static ESPAnalysisMeasurement CasingPressure { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Frequency'.
        /// </summary>
        public static ESPAnalysisMeasurement Frequency { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Productivity Index'.
        /// </summary>
        public static ESPAnalysisMeasurement ProductivityIndex { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Pressure Across Pump'.
        /// </summary>
        public static ESPAnalysisMeasurement PressureAcrossPump { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Pump Discharge Pressure'.
        /// </summary>
        public static ESPAnalysisMeasurement PumpDischargePressure { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Head Across Pump'.
        /// </summary>
        public static ESPAnalysisMeasurement HeadAcrossPump { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Frictional Loss In Tubing'.
        /// </summary>
        public static ESPAnalysisMeasurement FrictionalLossInTubing { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Pump Efficiency'.
        /// </summary>
        public static ESPAnalysisMeasurement PumpEfficiency { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Calculated Fluid Level Above Pump'.
        /// </summary>
        public static ESPAnalysisMeasurement CalculatedFluidLevelAbovePump { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Fluid Specific Gravity'.
        /// </summary>
        public static ESPAnalysisMeasurement FluidSpecificGravity { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Pump Static Pressure'.
        /// </summary>
        public static ESPAnalysisMeasurement PumpStaticPressure { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Rate at Bubble-Point'.
        /// </summary>
        public static ESPAnalysisMeasurement RateAtBubblePoint { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Rate at Max Oil'.
        /// </summary>
        public static ESPAnalysisMeasurement RateAtMaxOil { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Rate at Max Liquid'.
        /// </summary>
        public static ESPAnalysisMeasurement RateAtMaxLiquid { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'IPR Slope'.
        /// </summary>
        public static ESPAnalysisMeasurement IPRSlope { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Water Cut'.
        /// </summary>
        public static ESPAnalysisMeasurement WaterCut { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Flowing Bottomhole Pressure'.
        /// </summary>
        public static ESPAnalysisMeasurement FlowingBHP { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Gas Oil Ratio At Pump'.
        /// </summary>
        public static ESPAnalysisMeasurement GasOilRatioAtPump { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Formation Volume Factor'.
        /// </summary>
        public static ESPAnalysisMeasurement FormationVolumeFactor { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Gas Compressibility Factor'.
        /// </summary>
        public static ESPAnalysisMeasurement GasCompressibilityFactor { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Gas Volume Factor'.
        /// </summary>
        public static ESPAnalysisMeasurement GasVolumeFactor { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Producing Gas Oil Ratio'.
        /// </summary>
        public static ESPAnalysisMeasurement ProducingGasOilRatio { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Gas In Solution'.
        /// </summary>
        public static ESPAnalysisMeasurement GasInSolution { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Free Gas At Pump'.
        /// </summary>
        public static ESPAnalysisMeasurement FreeGasAtPump { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Oil Volume At Pump'.
        /// </summary>
        public static ESPAnalysisMeasurement OilVolumeAtPump { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Gas Volume At Pump'.
        /// </summary>
        public static ESPAnalysisMeasurement GasVolumeAtPump { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Total Volume At Pump'.
        /// </summary>
        public static ESPAnalysisMeasurement TotalVolumeAtPump { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Free Gas'.
        /// </summary>
        public static ESPAnalysisMeasurement FreeGas { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Composite Tubing Specific Gravity'.
        /// </summary>
        public static ESPAnalysisMeasurement CompositeTubingSpecificGravity { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Gas Density'.
        /// </summary>
        public static ESPAnalysisMeasurement GasDensity { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Liquid Density'.
        /// </summary>
        public static ESPAnalysisMeasurement LiquidDensity { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Annular Separation Efficiency'.
        /// </summary>
        public static ESPAnalysisMeasurement AnnularSeparationEfficiency { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Tubing Gas'.
        /// </summary>
        public static ESPAnalysisMeasurement TubingGas { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Tubing Gas Oil Ratio'.
        /// </summary>
        public static ESPAnalysisMeasurement TubingGasOilRatio { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Head Relative To Recommended Range'.
        /// </summary>
        public static ESPAnalysisMeasurement HeadRelativeToRecommendedRange { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Flow  Relative To Recommended Range'.
        /// </summary>
        public static ESPAnalysisMeasurement FlowRelativeToRecommendedRange { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Design Score'.
        /// </summary>
        public static ESPAnalysisMeasurement DesignScore { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Head Relative To Well Performance Curve'.
        /// </summary>
        public static ESPAnalysisMeasurement HeadRelativeToWellPerformanceCurve { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Head Relative To Pump Curve'.
        /// </summary>
        public static ESPAnalysisMeasurement HeadRelativeToPumpCurve { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Pump Degradation'.
        /// </summary>
        public static ESPAnalysisMeasurement PumpDegradation { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Max Potential Production Rate'.
        /// </summary>
        public static ESPAnalysisMeasurement MaxPotentialProductionRate { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Max Potential Frequency'.
        /// </summary>
        public static ESPAnalysisMeasurement MaxPotentialFrequency { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Production Increase Percentage'.
        /// </summary>
        public static ESPAnalysisMeasurement ProductionIncreasePercentage { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Turpin Parameter'.
        /// </summary>
        public static ESPAnalysisMeasurement TurpinParameter { get; private set; }

        #endregion

        #region Constructors

        static ESPAnalysisMeasurement()
        {
            //names have no spaces in order to match column names in the database
            NumberOfStages = CreateValue(1, "Number Of Stages", UnitCategory.None);
            VerticalPumpDepth = CreateValue(2, "Vertical Pump Depth", UnitCategory.LongLength);
            MeasuredPumpDepth = CreateValue(3, "Measured Pump Depth", UnitCategory.LongLength);
            OilRate = CreateValue(4, "Oil Rate", UnitCategory.FluidRate);
            WaterRate = CreateValue(5, "Water Rate", UnitCategory.FluidRate);
            GasRate = CreateValue(6, "Gas Rate", UnitCategory.GasRate);
            PumpIntakePressure = CreateValue(7, "Pump Intake Pressure", UnitCategory.Pressure);
            GrossRate = CreateValue(8, "Gross Rate", UnitCategory.FluidRate);
            FluidLevelAbovePump = CreateValue(9, "Fluid Level Above Pump", UnitCategory.LongLength);
            TubingPressure = CreateValue(10, "Tubing Pressure", UnitCategory.Pressure);
            CasingPressure = CreateValue(11, "Casing Pressure", UnitCategory.Pressure);
            Frequency = CreateValue(12, "Frequency", UnitCategory.Frequency);
            ProductivityIndex = CreateValue(13, "Productivity Index", UnitCategory.None);
            PressureAcrossPump = CreateValue(14, "Pressure Across Pump", UnitCategory.Pressure);
            PumpDischargePressure = CreateValue(15, "Pump Discharge Pressure", UnitCategory.Pressure);
            HeadAcrossPump = CreateValue(16, "Head Across Pump", UnitCategory.Head);
            FrictionalLossInTubing = CreateValue(17, "Frictional Loss In Tubing", UnitCategory.Pressure);
            PumpEfficiency = CreateValue(18, "Pump Efficiency", UnitCategory.None);
            CalculatedFluidLevelAbovePump = CreateValue(19, "Calculated Fluid Level Above Pump", UnitCategory.LongLength);
            FluidSpecificGravity = CreateValue(20, "Fluid Specific Gravity", UnitCategory.RelativeDensity);
            PumpStaticPressure = CreateValue(21, "Pump Static Pressure", UnitCategory.Pressure);
            RateAtBubblePoint = CreateValue(22, "Qb", UnitCategory.FluidRate);
            RateAtMaxOil = CreateValue(23, "Qomax", UnitCategory.FluidRate);
            RateAtMaxLiquid = CreateValue(24, "Qlmax", UnitCategory.FluidRate);
            IPRSlope = CreateValue(25, "IPR Slope", UnitCategory.None);
            WaterCut = CreateValue(26, "Water Cut", UnitCategory.None);
            FlowingBHP = CreateValue(27, "Flowing Bottomhole Pressure", UnitCategory.Pressure);
            GasOilRatioAtPump = CreateValue(28, "Gas Oil Ratio At Pump", UnitCategory.GasVolume);
            FormationVolumeFactor = CreateValue(29, "Formation Volume Factor", UnitCategory.None);
            GasCompressibilityFactor = CreateValue(30, "Gas Compressibility Factor", UnitCategory.None);
            GasVolumeFactor = CreateValue(31, "Gas Volume Factor", UnitCategory.FluidVolume);
            ProducingGasOilRatio = CreateValue(32, "Producing Gas Oil Ratio", UnitCategory.GasVolume);
            GasInSolution = CreateValue(33, "Gas In Solution", UnitCategory.GasVolume);
            FreeGasAtPump = CreateValue(34, "Free Gas At Pump", UnitCategory.GasVolume);
            OilVolumeAtPump = CreateValue(35, "Oil Volume At Pump", UnitCategory.FluidVolume);
            GasVolumeAtPump = CreateValue(36, "Gas Volume At Pump", UnitCategory.FluidVolume);
            TotalVolumeAtPump = CreateValue(37, "Total Volume At Pump", UnitCategory.FluidVolume);
            FreeGas = CreateValue(38, "Free Gas", UnitCategory.None);
            CompositeTubingSpecificGravity = CreateValue(39, "Composite Tubing Specific Gravity", UnitCategory.RelativeDensity);
            GasDensity = CreateValue(40, "Gas Density", UnitCategory.Density);
            LiquidDensity = CreateValue(41, "Liquid Density", UnitCategory.Density);
            AnnularSeparationEfficiency = CreateValue(42, "Annular Separation Efficiency", UnitCategory.None);
            TubingGas = CreateValue(43, "Tubing Gas", UnitCategory.GasVolume);
            TubingGasOilRatio = CreateValue(44, "Tubing Gas Oil Ratio", UnitCategory.GasVolume);
            HeadRelativeToRecommendedRange = CreateValue(45, "Head Relative To Recommended Range", UnitCategory.None);
            FlowRelativeToRecommendedRange = CreateValue(46, "Flow  Relative To Recommended Range", UnitCategory.None);
            DesignScore = CreateValue(47, "Design Score", UnitCategory.None);
            HeadRelativeToWellPerformanceCurve = CreateValue(48, "Head Relative To Well Performance Curve", UnitCategory.None);
            HeadRelativeToPumpCurve = CreateValue(49, "Head Relative To Pump Curve", UnitCategory.None);
            PumpDegradation = CreateValue(50, "Pump Degradation", UnitCategory.None);
            MaxPotentialProductionRate = CreateValue(51, "Max Potential Production Rate", UnitCategory.FluidVolume);
            MaxPotentialFrequency = CreateValue(52, "Max Potential Frequency", UnitCategory.Frequency);
            ProductionIncreasePercentage = CreateValue(53, "Production Increase Percentage", UnitCategory.None);
            ProductionIncreasePercentage = CreateValue(54, "Turpin Parameter", UnitCategory.None);
        }

        /// <summary>
        /// Initializes a new AnalysisMeasurement with a specified key and name.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="name">The name</param>
        protected ESPAnalysisMeasurement(int key, Text name)
            : base(key, name)
        {

        }

        /// <summary>
        /// Initializes a new AnalysisMeasurement with a specified key, name and unit category.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        /// <param name="unitCategory">The unit category associated with the measurement.</param>
        protected ESPAnalysisMeasurement(int key, Text name, UnitCategory unitCategory)
            : base(key, name)
        {
            UnitCategory = unitCategory;
        }

        #endregion

        #region Static Methods

        private static ESPAnalysisMeasurement CreateValue(int key, string name, UnitCategory unitCategory)
        {
            ESPAnalysisMeasurement value = new ESPAnalysisMeasurement(key, new Text(name), unitCategory);

            Register(value);

            return value;
        }

        #endregion
    }
}
