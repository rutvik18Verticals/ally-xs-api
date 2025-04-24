namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The GL Valves status data model.
    /// </summary>
    public class GLValveStatusModel
    {

        /// <summary>
        /// Gets or sets the GL valves status id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the GL well valve id.
        /// </summary>
        public int GlwellValveId { get; set; }

        /// <summary>
        /// Gets or sets the valve percent open.
        /// </summary>
        public float PercentOpen { get; set; }

        /// <summary>
        /// Gets or sets the gas rate.
        /// </summary>
        public float? GasRate { get; set; }

        /// <summary>
        /// Gets or sets the gl analysis result id.
        /// </summary>
        public int GLAnalysisResultId { get; set; }

        /// <summary>
        /// Gets or sets the injection pressure at depth.
        /// </summary>
        public float? InjectionPressureAtDepth { get; set; }

        /// <summary>
        /// Gets or sets the valve state.
        /// </summary>
        public int ValveState { get; set; }

        /// <summary>
        /// Gets or sets the valve indicating is injecting gas set.
        /// </summary>
        public bool? IsInjectingGas { get; set; }

        /// <summary>
        /// Gets or sets the injection rate for tubing critical velocity.
        /// </summary>
        public float? InjectionRateForTubingCriticalVelocity { get; set; }

        /// <summary>
        /// Gets or sets the tubing critical velocity at depth.
        /// </summary>
        public float? TubingCriticalVelocityAtDepth { get; set; }

        /// <summary>
        /// Gets or sets the opening pressure at depth.
        /// </summary>
        public float? OpeningPressureAtDepth { get; set; }

        /// <summary>
        /// Gets or sets the closing pressure at depth.
        /// </summary>
        public float? ClosingPressureAtDepth { get; set; }

        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        public float? Depth { get; set; }

    }
}
