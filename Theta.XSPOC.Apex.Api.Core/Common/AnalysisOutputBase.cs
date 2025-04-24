namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Base class for all analysis related outputs.
    /// </summary>
    public abstract class AnalysisOutputBase
    {

        /// <summary>
        /// Gets or sets the productivity index
        /// </summary>
        public double? ProductivityIndex { get; set; }

        /// <summary>
        /// Gets or sets the rate at bubble-point
        /// </summary>
        public double? RateAtBubblePoint { get; set; }

        /// <summary>
        /// Gets or sets the rate at max oil
        /// </summary>
        public double? RateAtMaxOil { get; set; }

        /// <summary>
        /// Gets or sets the rate at max liquid
        /// </summary>
        public double? RateAtMaxLiquid { get; set; }

        /// <summary>
        /// Gets or sets the slope of the IPR curve
        /// </summary>
        public double? IPRSlope { get; set; }

        /// <summary>
        /// Gets or sets the water cut
        /// </summary>
        public double? WaterCut { get; set; }

        /// <summary>
        /// Gets or sets the flowing bottomhole pressure
        /// </summary>
        public double? FlowingBHP { get; set; }

    }
}
