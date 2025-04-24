using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the types of measurements used in GL Analysis.
    /// </summary>
    public class GLAnalysisMeasurement : EnhancedEnumBase
    {

        #region Properties

        /// <summary>
        /// Gets the unit category associated with this measurement.
        /// </summary>
        public UnitCategory UnitCategory { get; }

        #endregion

        #region Static Properties

        /// <summary>
        /// Specifies the type of measurement is 'Gas Injection Depth'.
        /// </summary>
        public static GLAnalysisMeasurement GasInjectionDepth { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Vertical Well Depth'.
        /// </summary>
        public static GLAnalysisMeasurement VerticalWellDepth { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Measured Well Depth'.
        /// </summary>
        public static GLAnalysisMeasurement MeasuredWellDepth { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Oil Rate'.
        /// </summary>
        public static GLAnalysisMeasurement OilRate { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Water Rate'.
        /// </summary>
        public static GLAnalysisMeasurement WaterRate { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Gas Rate'.
        /// </summary>
        public static GLAnalysisMeasurement GasRate { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Wellhead Pressure'.
        /// </summary>
        public static GLAnalysisMeasurement WellheadPressure { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Casing Pressure'.
        /// </summary>
        public static GLAnalysisMeasurement CasingPressure { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is Oil Specific Gravity'.
        /// </summary>
        public static GLAnalysisMeasurement OilSpecificGravity { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Water Specific Gravity'.
        /// </summary>
        public static GLAnalysisMeasurement WaterSpecificGravity { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Gas Specific Gravity'.
        /// </summary>
        public static GLAnalysisMeasurement GasSpecificGravity { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Tubing Inner Diameter'.
        /// </summary>
        public static GLAnalysisMeasurement TubingInnerDiameter { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Tubing Outer Diameter'.
        /// </summary>
        public static GLAnalysisMeasurement TubingOuterDiameter { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Wellhead Temperature'.
        /// </summary>
        public static GLAnalysisMeasurement WellheadTemperature { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Bottom Hole Temperature'.
        /// </summary>
        public static GLAnalysisMeasurement BottomHoleTemperature { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Percent H2S'.
        /// </summary>
        public static GLAnalysisMeasurement PercentH2S { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Percent N2'.
        /// </summary>
        public static GLAnalysisMeasurement PercentN2 { get; private set; }

        /// <summary>
        /// Specifies the type of measurement is 'Percent CO2'.
        /// </summary>
        public static GLAnalysisMeasurement PercentCO2 { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Productivity Index'.
        /// </summary>
        public static GLAnalysisMeasurement ProductivityIndex { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Rate at Bubble-Point'.
        /// </summary>
        public static GLAnalysisMeasurement RateAtBubblePoint { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Rate at Max Oil'.
        /// </summary>
        public static GLAnalysisMeasurement RateAtMaxOil { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Rate at Max Liquid'.
        /// </summary>
        public static GLAnalysisMeasurement RateAtMaxLiquid { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'IPR Slope'.
        /// </summary>
        public static GLAnalysisMeasurement IPRSlope { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Water Cut'.
        /// </summary>
        public static GLAnalysisMeasurement WaterCut { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Flowing Bottomhole Pressure'.
        /// </summary>
        public static GLAnalysisMeasurement FlowingBHP { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Injected GLR'.
        /// </summary>
        public static GLAnalysisMeasurement InjectedGLR { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Total GLR'.
        /// </summary>
        public static GLAnalysisMeasurement TotalGLR { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Gross Rate'.
        /// </summary>
        public static GLAnalysisMeasurement GrossRate { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Formation GOR'.
        /// </summary>
        public static GLAnalysisMeasurement FormationGOR { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Injected Gas Rate'.
        /// </summary>
        public static GLAnalysisMeasurement InjectedGasRate { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Max Liquid Rate'.
        /// </summary>
        public static GLAnalysisMeasurement MaxLiquidRate { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Injection Rate for Max Liquid Rate'.
        /// </summary>
        public static GLAnalysisMeasurement InjectionRateForMaxLiquidRate { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'GLR for Max Liquid Rate'.
        /// </summary>
        public static GLAnalysisMeasurement GLRForMaxLiquidRate { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Optimum Liquid Rate'.
        /// </summary>
        public static GLAnalysisMeasurement OptimumLiquidRate { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Injection Rate for Optimum Liquid Rate'.
        /// </summary>
        public static GLAnalysisMeasurement InjectionRateForOptimumLiquidRate { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'GLR for Optimum Liquid Rate'.
        /// </summary>
        public static GLAnalysisMeasurement GLRForOptimumLiquidRate { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Minimum Flowing Bottomhole Pressure'.
        /// </summary>
        public static GLAnalysisMeasurement MinimumFlowingBHP { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Valve Critical Velocity'.
        /// </summary>
        public static GLAnalysisMeasurement ValveCriticalVelocity { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'Tubing Critical Velocity'.
        /// </summary>
        public static GLAnalysisMeasurement TubingCriticalVelocity { get; private set; }

        /// <summary>
        /// Specifies that the type of measurement is 'First Injecting Valve Depth'.
        /// </summary>
        public static GLAnalysisMeasurement FirstInjectingValveDepth { get; private set; }

        #endregion

        #region Constructors

        static GLAnalysisMeasurement()
        {
            //names have no spaces in order to match column names in the database
            GasInjectionDepth = CreateValue(1, "Gas Injection Depth", UnitCategory.LongLength);
            VerticalWellDepth = CreateValue(2, "Vertical Well Depth", UnitCategory.LongLength);
            MeasuredWellDepth = CreateValue(45, "Measured Well Depth", UnitCategory.LongLength);
            OilRate = CreateValue(3, "Oil Rate", UnitCategory.FluidRate);
            WaterRate = CreateValue(4, "Water Rate", UnitCategory.FluidRate);
            GasRate = CreateValue(5, "Gas Rate", UnitCategory.FluidRate);
            WellheadPressure = CreateValue(6, "Tubing Pressure", UnitCategory.Pressure);
            CasingPressure = CreateValue(7, "Casing Pressure", UnitCategory.Pressure);
            OilSpecificGravity = CreateValue(8, "Oil API", UnitCategory.OilRelativeDensity);
            WaterSpecificGravity = CreateValue(9, "Water Specific Gravity", UnitCategory.RelativeDensity);
            GasSpecificGravity = CreateValue(11, "Gas Specific Gravity", UnitCategory.RelativeDensity);
            TubingInnerDiameter = CreateValue(12, "Tubing Inner Diameter", UnitCategory.ShortLength);
            TubingOuterDiameter = CreateValue(13, "Tubing Outer Diameter", UnitCategory.ShortLength);
            WellheadTemperature = CreateValue(14, "Wellhead Temperature", UnitCategory.Temperature);
            BottomHoleTemperature = CreateValue(15, "Bottom Hole Temperature", UnitCategory.Temperature);
            PercentH2S = CreateValue(16, "Percent H2S", UnitCategory.None);
            PercentN2 = CreateValue(17, "Percent N2", UnitCategory.None);
            PercentCO2 = CreateValue(18, "Percent CO2", UnitCategory.None);
            ProductivityIndex = CreateValue(19, "Productivity Index", UnitCategory.None);
            RateAtBubblePoint = CreateValue(20, "Qb", UnitCategory.FluidRate);
            RateAtMaxOil = CreateValue(21, "Qomax", UnitCategory.FluidRate);
            RateAtMaxLiquid = CreateValue(22, "Qlmax", UnitCategory.FluidRate);
            IPRSlope = CreateValue(23, "IPR Slope", UnitCategory.None);
            WaterCut = CreateValue(24, "Water Cut", UnitCategory.None);
            FlowingBHP = CreateValue(25, "Flowing Bottomhole Pressure", UnitCategory.Pressure);
            InjectedGLR = CreateValue(26, "Injected GLR", UnitCategory.None);
            InjectedGasRate = CreateValue(27, "Injected Gas Rate", UnitCategory.GasRate);
            MaxLiquidRate = CreateValue(28, "Max Liquid Rate", UnitCategory.FluidRate);
            InjectionRateForMaxLiquidRate = CreateValue(29, "Injection Rate for Max Ql", UnitCategory.GasRate);
            GLRForMaxLiquidRate = CreateValue(30, "GLR For Max Ql", UnitCategory.None);
            OptimumLiquidRate = CreateValue(31, "Optimum Ql", UnitCategory.FluidRate);
            InjectionRateForOptimumLiquidRate = CreateValue(32, "Injection Rate for Optimum Ql", UnitCategory.GasRate);
            GLRForOptimumLiquidRate = CreateValue(33, "GLR For Optimum Ql", UnitCategory.None);
            MinimumFlowingBHP = CreateValue(34, "Minimum Flowing Bottomhole Pressure", UnitCategory.Pressure);
            ValveCriticalVelocity = CreateValue(35, "Valve Critical Velocity", UnitCategory.GasVolume);
            TubingCriticalVelocity = CreateValue(36, "Tubing Critical Velocity", UnitCategory.GasVolume);
            FormationGOR = CreateValue(37, "Formation GOR", UnitCategory.GasRate);
            TotalGLR = CreateValue(38, "Total GLR", UnitCategory.GasRate);
            GrossRate = CreateValue(39, "Total GLR", UnitCategory.GasRate);
            FirstInjectingValveDepth = CreateValue(40, "First Injecting Valve Depth", UnitCategory.LongLength);
        }

        /// <summary>
        /// Initializes a new GLAnalysisMeasurement with a specified key and name.
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="name">The name</param>
        protected GLAnalysisMeasurement(int key, Text name)
            : base(key, name)
        {

        }

        /// <summary>
        /// Initializes a new GLAnalysisMeasurement with a specified key, name and unit category.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        /// <param name="unitCategory">The unit category associated with the measurement.</param>
        protected GLAnalysisMeasurement(int key, Text name, UnitCategory unitCategory)
            : base(key, name)
        {
            UnitCategory = unitCategory;
        }

        #endregion

        #region Static Methods

        private static GLAnalysisMeasurement CreateValue(int key, string name, UnitCategory unitCategory)
        {
            GLAnalysisMeasurement value = new GLAnalysisMeasurement(key, new Text(name), unitCategory);

            Register(value);

            return value;
        }

        #endregion
    }
}
