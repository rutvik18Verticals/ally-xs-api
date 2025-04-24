using Theta.XSPOC.Apex.Api.Core.Common.Interface;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the WellValve object with a unique identifier.
    /// </summary>
    public class WellValve : IdentityBase, IFlowControlDevice
    {

        #region Properties

        /// <summary>
        /// Gets or sets the valve
        /// </summary>
        public Valve Valve { get; set; }

        /// <summary>
        /// The valve's vertical depth from the surface.
        /// </summary>
        public float? VerticalDepth { get; set; } // Length

        /// <summary>
        /// The valve's measured depth in the well.
        /// </summary>
        public float? MeasuredDepth { get; set; } // Length

        /// <summary>
        /// The valve's test-rack opening pressure.
        /// </summary>
        public float? TPRO { get; set; } // Pressure

        /// <summary>
        /// Gets or sets the closing pressure at valve depth for this specific valve
        /// </summary>
        public float? ClosingPressureAtDepth { get; set; } // Pressure

        /// <summary>
        /// Gets or sets the opening pressure at valve depth for this specific valve
        /// </summary>
        public float? OpeningPressureAtDepth { get; set; } // Pressure

        /// <summary>
        /// Gets or sets the closing pressure at surface for this specific valve
        /// </summary>
        public float? ClosingPressureAtSurface { get; set; } // Pressure

        /// <summary>
        /// Gets or sets the opening pressure at surface for this specific valve
        /// </summary>
        public float? OpeningPressureAtSurface { get; set; } // Pressure

        /// <summary>
        /// Gets or sets the true vertical depth from the surface.
        /// </summary>
        public float? TrueVerticalDepth { get; set; } // Length

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new WellValve with a default ID
        /// </summary>
        public WellValve()
        {
        }

        /// <summary>
        /// Initializes a new WellValve with a specified ID
        /// </summary>
        /// <param name="id">The ID</param>
        public WellValve(object id)
            : base(id)
        {
        }

        /// <summary>
        /// Gets the well valve depth based on if true vertical depth is true.
        /// </summary>
        /// <param name="useTVD">Represents if using true vertical depth is enabled.</param>
        /// <returns>If useTVD is true, returns current true vertical depth. 
        /// If useTVD is false, returns vertical depth unless it is null, then returns measured depth.</returns>
        public float? GetDepth(bool useTVD) // Length
        {
            if (useTVD)
            {
                return TrueVerticalDepth;
            }

            return VerticalDepth ?? MeasuredDepth;
        }

        #endregion

    }
}
