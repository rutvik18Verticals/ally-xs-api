using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represent the gl curve type.
    /// </summary>
    public class GLCurveType : EnhancedEnumBase, ICurveType
    {

        #region Properties

        /// <summary>
        /// Gets the unit category that this curve's x axis is measured in.
        /// </summary>
        public UnitCategory XAxisUnitCategory { get; }

        /// <summary>
        /// Gets the unit category that this curve's y axis is measured in.
        /// </summary>
        public UnitCategory YAxisUnitCategory { get; }

        #endregion

        #region Static Properties

        /// <summary>
        /// Gets the Production Performance Curve
        /// </summary>
        public static GLCurveType ProductionPerformanceCurve { get; } = CreateValue(1,
            Text.FromString("Injection Performance Curve"), UnitCategory.GasRate, UnitCategory.FluidRate);

        /// <summary>
        /// Gets the Pressure Performance Curve
        /// </summary>
        public static GLCurveType PressurePerformanceCurve { get; } = CreateValue(2,
            Text.FromString("Pressure Performance Curve"), UnitCategory.GasRate, UnitCategory.Pressure);

        /// <summary>
        /// Gets the Gas Injection Curve
        /// </summary>
        public static GLCurveType GasInjectionCurve { get; } = CreateValue(4, Text.FromString("Gas Injection Curve"),
            UnitCategory.Pressure, UnitCategory.LongLength);

        /// <summary>
        /// Gets the Gas ReservoirFluidCurve
        /// </summary>
        public static GLCurveType ReservoirFluidCurve { get; } = CreateValue(5, Text.FromString("Reservoir Fluid Curve"),
            UnitCategory.Pressure, UnitCategory.LongLength);

        /// <summary>
        /// Gets the Gas KillFluidCurve
        /// </summary>
        public static GLCurveType KillFluidCurve { get; } = CreateValue(6, Text.FromString("Kill Fluid Curve"),
            UnitCategory.Pressure, UnitCategory.LongLength);

        /// <summary>
        /// Gets the Temperature Curve
        /// </summary>
        public static GLCurveType TemperatureCurve { get; } = CreateValue(7, Text.FromString("Temperature Curve"),
            UnitCategory.Temperature, UnitCategory.LongLength);

        /// <summary>
        /// Gets the Production Fluid Curve
        /// </summary>
        public static GLCurveType ProductionFluidCurve { get; } = CreateValue(8, Text.FromString("Production Fluid Curve"),
            UnitCategory.Pressure, UnitCategory.LongLength);

        /// <summary>
        /// Gets the Flowing Bottomhole Pressure Performance Curve
        /// </summary>
        public static GLCurveType FlowingBottomholePressurePerformanceCurve { get; } = CreateValue(27,
            "Flowing Bottomhole Pressure Performance Curve", UnitCategory.GasRate, UnitCategory.Pressure);

        /// <summary>
        /// Gets the Injection Rate For Critical Velocity Curve
        /// </summary>
        public static GLCurveType InjectionRateForCriticalVelocityCurve { get; } = CreateValue(29,
            "Injection Rate For Critical Velocity Curve",
            UnitCategory.GasRate, UnitCategory.LongLength);

        #endregion

        #region Constructors

        /// <summary>
        /// Necessary for implementing the EnhancedEnumBase
        /// </summary>
        protected GLCurveType(int key, Text name)
            : base(key, name)
        {
        }

        /// <summary>
        /// Initializes a new GLCurveType with a specified key and name as well as unit categories.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        /// <param name="xAxisUnitCategory">The unit category that this curve's x axis is associated with.</param>
        /// <param name="yAxisUnitCategory">the unit category that this curve's y axis is associated with.</param>
        private GLCurveType(int key, Text name, UnitCategory xAxisUnitCategory, UnitCategory yAxisUnitCategory)
            : base(key, name)
        {
            XAxisUnitCategory = xAxisUnitCategory;
            YAxisUnitCategory = yAxisUnitCategory;
        }

        #endregion

        private static GLCurveType CreateValue(int key, string name, UnitCategory xAxisUnitCategory,
            UnitCategory yAxisUnitCategory)
        {
            var value = new GLCurveType(key, new Text(name), xAxisUnitCategory, yAxisUnitCategory);

            Register(value);

            return value;
        }

    }
}
