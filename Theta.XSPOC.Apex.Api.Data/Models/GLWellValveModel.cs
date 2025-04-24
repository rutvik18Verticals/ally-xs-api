namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the gl well valve model.
    /// </summary>
    public class GLWellValveModel
    {

        /// <summary>
        /// Get or sets the gl well valve id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Get or sets the node id.
        /// </summary>
        public string NodeId { get; set; } = null!;

        /// <summary>
        /// Get or sets the gl valve id.
        /// </summary>
        public int GLValveId { get; set; }

        /// <summary>
        /// Get or sets the vertical depth.
        /// </summary>
        public float VerticalDepth { get; set; }

        /// <summary>
        /// Get or sets the the test rack opening pressure.
        /// </summary>
        public float? TestRackOpeningPressure { get; set; }

        /// <summary>
        /// Get or sets the closing pressure at depth.
        /// </summary>
        public float? ClosingPressureAtDepth { get; set; }

        /// <summary>
        /// Get or sets the the measured depth.
        /// </summary>
        public float? MeasuredDepth { get; set; }

        /// <summary>
        /// Get or sets the  the opening pressure at depth.
        /// </summary>
        public float? OpeningPressureAtDepth { get; set; }

        /// <summary>
        /// Get or sets the opening pressure at surface.
        /// </summary>
        public float? OpeningPressureAtSurface { get; set; }

        /// <summary>
        /// Get or sets the closing pressure at surface.
        /// </summary>
        public float? ClosingPressureAtSurface { get; set; }

        /// <summary>
        /// Get or sets the true vertical depth.
        /// </summary>
        public float? TrueVerticalDepth { get; set; }

        /// <summary>
        /// Gets or sets the gl valves id.
        /// </summary>
        public float? ValveId { get; set; }

        /// <summary>
        /// Get or sets the diameter.
        /// </summary>
        public float Diameter { get; set; }

        /// <summary>
        /// Get or sets the bellow area.
        /// </summary>
        public float BellowsArea { get; set; }

        /// <summary>
        /// Get or sets the  port size.
        /// </summary>
        public float PortSize { get; set; }

        /// <summary>
        /// Get or sets the  port area.
        /// </summary>
        public float PortArea { get; set; }

        /// <summary>
        /// Get or sets the port-to-bellows-area ratio.
        /// </summary>
        public float PortToBellowsAreaRatio { get; set; }

        /// <summary>
        /// Get or sets the production pressure effect factor.
        /// </summary>
        public float ProductionPressureEffectFactor { get; set; }

        /// <summary>
        /// Get or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Get or sets the manufacturer id.
        /// </summary>
        public int? ManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the 1 - Ap / Ab [ aka 1 - PortToBellowsAreaRatio ] value.
        /// </summary>
        public float? OneMinusR { get; set; }

    }
}
