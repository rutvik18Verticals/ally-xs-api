namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represent the analysis diagnostics.
    /// </summary>
    public class AnalysisDiagnostics
    {

        /// <summary>
        /// Gets or sets the %flow relative to recommended range
        /// </summary>
        public double? FlowRelativeToRecommendedRange { get; set; }

        /// <summary>
        /// Gets or sets the %head relative to recommended range
        /// </summary>
        public double? HeadRelativeToRecommendedRange { get; set; }

        /// <summary>
        /// Gets or sets the pump degradation.
        /// </summary>
        public double? PumpDegradation { get; set; }

        /// <summary>
        /// Gets or sets the head relative to pump curve
        /// </summary>
        public double? HeadRelativeToPumpCurve { get; set; }

        /// <summary>
        /// Gets or sets the head relative to well performance curve
        /// </summary>
        public double? HeadRelativeToWellPerformanceCurve { get; set; }

        /// <summary>
        /// Gets or sets the design score out of 100
        /// </summary>
        public float? DesignScore { get; set; }

        /// <summary>
        /// Gets or sets the production increase percentage
        /// </summary>
        public double? ProductionIncreasePercentage { get; set; }

        /// <summary>
        /// Gets or sets the maximum potential production rate.
        /// </summary>
        public double? MaxPotentialProductionRate { get; set; }

        /// <summary>
        /// Gets or sets the maximum potential frequency.
        /// </summary>
        public double? MaxPotentialFrequency { get; set; }

    }
}
