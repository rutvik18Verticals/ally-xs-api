using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the SurveyCurveType.
    /// </summary>
    public class SurveyCurveType : EnhancedEnumBase, ICurveType
    {

        #region Properties

        /// <summary>
        /// Gets the unit category that this curve's x axis is measured in.
        /// </summary>
        public UnitCategory XAxisUnitCategory { get; private set; }

        /// <summary>
        /// Gets the unit category that this curve's y axis is measured in.
        /// </summary>
        public UnitCategory YAxisUnitCategory { get; private set; }

        #endregion

        #region Static Properties

        /// <summary>
        /// Gets the Pressure Curve
        /// </summary>
        public static SurveyCurveType PressureCurve { get; private set; } = CreateValue(26,
            Text.FromString("Pressure Survey Curve"),
            UnitCategory.Pressure, UnitCategory.LongLength);

        /// <summary>
        /// Gets the Pump Curve
        /// </summary>
        public static SurveyCurveType TemperatureCurve { get; private set; } = CreateValue(25,
            Text.FromString("Temperature Survey Curve"),
            UnitCategory.Temperature, UnitCategory.LongLength);

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for SurveyCurveType.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="name"></param>
        protected SurveyCurveType(int key, Text name) : base(key, name)
        {
        }

        #endregion

        private static SurveyCurveType CreateValue(int key, string name, UnitCategory xAxisUnitCategory,
            UnitCategory yAxisUnitCategory)
        {
            var value = new SurveyCurveType(key, new Text(name))
            {
                XAxisUnitCategory = xAxisUnitCategory,
                YAxisUnitCategory = yAxisUnitCategory,
            };

            Register(value);

            return value;
        }

    }
}
