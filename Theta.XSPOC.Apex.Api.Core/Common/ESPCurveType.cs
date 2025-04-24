using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents an ESP curve type.
    /// </summary>
    public class ESPCurveType : EnhancedEnumBase, ICurveType
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
        /// Gets the Well Performance Curve
        /// </summary>
        public static ESPCurveType WellPerformanceCurve { get; } = CreateValue(10, Text.FromString("Well Performance Curve"),
            UnitCategory.FluidRate, UnitCategory.Head);

        /// <summary>
        /// Gets the Pump Curve
        /// </summary>
        public static ESPCurveType PumpCurve { get; } = CreateValue(9, Text.FromString("Pump Curve"),
            UnitCategory.FluidRate, UnitCategory.Head);

        /// <summary>
        /// Gets the Power Curve
        /// </summary>
        public static ESPCurveType PowerCurve { get; } = CreateValue(11, Text.FromString("Power Curve"),
            UnitCategory.FluidRate, UnitCategory.Power);

        /// <summary>
        /// Gets the Efficiency Curve
        /// </summary>
        public static ESPCurveType EfficiencyCurve { get; } = CreateValue(12, Text.FromString("Efficiency Curve"),
            UnitCategory.FluidRate, UnitCategory.None);

        /// <summary>
        /// Gets the Recommended Range Curve - Left
        /// </summary>
        public static ESPCurveType RecommendedRangeLeft { get; } = CreateValue(13,
            Text.FromString("Recommended Range Curve - Left"),
            UnitCategory.FluidRate, UnitCategory.Head);

        /// <summary>
        /// Gets the Recommended Range Curve - Top
        /// </summary>
        public static ESPCurveType RecommendedRangeTop { get; } = CreateValue(14,
            Text.FromString("Recommended Range Curve - Top"),
            UnitCategory.FluidRate, UnitCategory.Head);

        /// <summary>
        /// Gets the Recommended Range Curve - Right
        /// </summary>
        public static ESPCurveType RecommendedRangeRight { get; } = CreateValue(15,
            Text.FromString("Recommended Range Curve - Right"),
            UnitCategory.FluidRate, UnitCategory.Head);

        /// <summary>
        /// Gets the Recommended Range Curve - Bottom
        /// </summary>
        public static ESPCurveType RecommendedRangeBottom { get; } = CreateValue(16,
            Text.FromString("Recommended Range Curve - Bottom"),
            UnitCategory.FluidRate, UnitCategory.Head);

        /// <summary>
        /// Gets the tornado curve.
        /// </summary>
        /// <value>
        /// The tornado curve.
        /// </value>
        public static ESPCurveType TornadoCurve { get; } = CreateValue(28, Text.FromString("Tornado Curve"),
            UnitCategory.FluidRate, UnitCategory.Head);

        #endregion

        #region Constructors

        /// <summary>
        /// Necessary for implementing the EnhancedEnumBase
        /// </summary>
        protected ESPCurveType(int key, Text name)
            : base(key, name)
        {
        }

        /// <summary>
        /// Initializes a new ESPCurveType with a specified key and name as well as unit categories.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        /// <param name="xAxisUnitCategory">The unit category that this curve's x axis is associated with.</param>
        /// <param name="yAxisUnitCategory">the unit category that this curve's y axis is associated with.</param>
        private ESPCurveType(int key, Text name, UnitCategory xAxisUnitCategory, UnitCategory yAxisUnitCategory)
            : base(key, name)
        {
            XAxisUnitCategory = xAxisUnitCategory;
            XAxisUnitCategory = yAxisUnitCategory;
        }

        #endregion

        private static ESPCurveType CreateValue(int key, string name, UnitCategory xAxisUnitCategory,
            UnitCategory yAxisUnitCategory)
        {
            var value = new ESPCurveType(key, new Text(name), xAxisUnitCategory, yAxisUnitCategory);

            Register(value);

            return value;
        }

    }
}
