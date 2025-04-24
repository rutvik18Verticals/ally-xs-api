
using System;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The IPR Analysis Result data model.
    /// </summary>
    public class IPRAnalysisResultModel
    {

        /// <summary>
        /// Gets or sets the auto generated id of the table.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets node id.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets test date.
        /// </summary>
        public DateTime TestDate { get; set; }

        /// <summary>
        /// Gets or sets static bottom hole pressure.
        /// </summary>
        public float? StaticBottomholePressure { get; set; }

        /// <summary>
        /// Gets or sets bubble point pressure.
        /// </summary>
        public float? BubblepointPressure { get; set; }

        /// <summary>
        /// Gets or sets flowing bottom hole pressure.
        /// </summary>
        public float? FlowingBottomholePressure { get; set; }

        /// <summary>
        /// Gets or sets gross rate.
        /// </summary>
        public float? GrossRate { get; set; }

        /// <summary>
        /// Gets or sets water rate.
        /// </summary>
        public float? WaterRate { get; set; }

        /// <summary>
        /// Gets or sets water cut rate.
        /// </summary>
        public float? WaterCut { get; set; }

        /// <summary>
        /// Gets or sets the ipr slope.
        /// </summary>
        public float? IPRSlope { get; set; }

        /// <summary>
        /// Gets or sets the productivity index.
        /// </summary>
        public float? ProductivityIndex { get; set; }

        /// <summary>
        /// Gets or sets rate at bubble point.
        /// </summary>
        public float? RateAtBubblePoint { get; set; }

        /// <summary>
        /// Gets or sets rate at max oil.
        /// </summary>
        public float? RateAtMaxOil { get; set; }

        /// <summary>
        /// Gets or sets rate at max liquid.
        /// </summary>
        public float? RateAtMaxLiquid { get; set; }

        /// <summary>
        /// Gets or sets analysis correlation id.
        /// </summary>
        public int? AnalysisCorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the fbhp calculation used defaults.
        /// </summary>
        public bool FBHPCalculationUsedDefaults { get; set; }

        /// <summary>
        /// Gets or sets the fbhp calculation result.
        /// </summary>
        public string FBHPCalculationResult { get; set; }

        /// <summary>
        /// Gets or sets the sbhp calculation result.
        /// </summary>
        public string SBHPCalculationResult { get; set; }

        /// <summary>
        /// Gets or sets the bubble point pressure source.
        /// </summary>
        public int? BubblepointPressureSource { get; set; }

        /// <summary>
        /// Gets or sets the fbhp calculation used measured depth.
        /// </summary>
        public bool? FBHPCalculationUsedMeasuredDepth { get; set; }

    }
}
