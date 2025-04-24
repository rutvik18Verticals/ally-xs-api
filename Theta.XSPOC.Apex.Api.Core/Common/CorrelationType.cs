using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// The types of correlations supported in XSPOC
    /// </summary>
    public class CorrelationType : EnhancedEnumBase
    {

        #region Static Properties

        /// <summary>
        /// Gets the Multi Phase Flow Correlation
        /// </summary>
        public static CorrelationType MultiphaseFlow { get; } =
            CreateValue(1, Text.FromString("Multiphase Flow Correlation"));

        /// <summary>
        /// Gets the Oil Viscosity Correlation
        /// </summary>
        public static CorrelationType OilViscosity { get; } =
            CreateValue(2, Text.FromString("Oil Viscosity Correlation"));

        /// <summary>
        /// Gets the Oil Viscosity Correlation
        /// </summary>
        public static CorrelationType OilFormationVolumeFactor { get; } =
            CreateValue(3, Text.FromString("Oil Formation Volume Factor Correlation"));

        /// <summary>
        /// Gets the Oil Viscosity Correlation
        /// </summary>
        public static CorrelationType SolutionGasOilRatio { get; } =
            CreateValue(4, Text.FromString("Solution Gas Oil Ratio Correlation"));

        /// <summary>
        /// Gets the Z-Factor Correlation
        /// </summary>
        public static CorrelationType ZFactor { get; } =
            CreateValue(5, Text.FromString("Z-Factor Correlation"));

        /// <summary>
        /// Gets the IPR Correlation
        /// </summary>
        public static CorrelationType IPRCorrelation { get; } =
            CreateValue(6, Text.FromString("IPR Correlation"));

        /// <summary>
        /// Gets the Tubing Critical Velocity Correlation
        /// </summary>
        public static CorrelationType TubingCriticalVelocity { get; } =
            CreateValue(7, Text.FromString("Tubing Critical Velocity Correlation"));

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new CorrelationType with a specified key and name.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        public CorrelationType(int key, Text name) : base(key, name)
        {
        }

        #endregion

        private static CorrelationType CreateValue(int key, string name)
        {
            var value = new CorrelationType(key, new Text(name));

            Register(value);

            return value;
        }

    }
}
