namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the gl well orifice status model.
    /// </summary>
    public class GLWellOrificeStatusModel
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer id.
        /// </summary>
        public int ManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the measured depth.
        /// </summary>
        public float? MeasuredDepth { get; set; }

        /// <summary>
        /// Gets or sets the vertical depth.
        /// </summary>
        public float VerticalDepth { get; set; }

        /// <summary>
        /// Gets or sets the port size.
        /// </summary>
        public float PortSize { get; set; }

        /// <summary>
        /// Gets or sets the true vertical depth.
        /// </summary>
        public float? TrueVerticalDepth { get; set; }

        /// <summary>
        /// Gets or sets the orifice state enum value.
        /// </summary>
        public int OrificeState { get; set; }

        /// <summary>
        /// Gets or sets the value indicating injecting gas is enabled or not.
        /// </summary>
        public bool? IsInjectingGas { get; set; }

        /// <summary>
        /// Gets or sets the gas lift analysis result id. 
        /// </summary>
        public int GLAnalysisResultId { get; set; }

        /// <summary>
        /// Gets or sets the injection rate for tubing critical velocity.
        /// </summary>
        public float? InjectionRateForTubingCriticalVelocity { get; set; }

        /// <summary>
        /// Gets or sets the tubing critical velocity at depth.
        /// </summary>
        public float? TubingCriticalVelocityAtDepth { get; set; }

        /// <summary>
        /// Gets or sets the injection pressure at depth.
        /// </summary>
        public float? InjectionPressureAtDepth { get; set; }

        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        public float? Depth { get; set; }

    }
}
