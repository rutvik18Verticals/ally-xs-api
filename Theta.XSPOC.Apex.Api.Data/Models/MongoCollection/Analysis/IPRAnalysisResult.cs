using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Analysis
{
    /// <summary>
    /// A class That represents an IPR analysis result.
    /// </summary>
    public class IPRAnalysisResult : AnalysisDocumentBase
    {

        /// <summary>
        /// Gets or sets the static bottomhole pressure.
        /// </summary>
        public double? StaticBottomholePressure { get; set; }

        /// <summary>
        /// Gets or sets the bubblepoint pressure.
        /// </summary>
        public double? BubblepointPressure { get; set; }

        /// <summary>
        /// Gets or sets the flowing bottomhole pressure.
        /// </summary>
        public double? FlowingBottomholePressure { get; set; }

        /// <summary>
        /// Gets or sets the gross rate.
        /// </summary>
        public double? GrossRate { get; set; }

        /// <summary>
        /// Gets or sets the water rate.
        /// </summary>
        public double? WaterRate { get; set; }

        /// <summary>
        /// Gets or sets the water cut.
        /// </summary>
        public double? WaterCut { get; set; }

        /// <summary>
        /// Gets or sets the IRP slope.
        /// </summary>
        public double? IPRSlope { get; set; }

        /// <summary>
        /// Gets or sets the productivity index.
        /// </summary>
        public double? ProductivityIndex { get; set; }

        /// <summary>
        /// Gets or sets the rate at bubble point.
        /// </summary>
        public double? RateAtBubblePoint { get; set; }

        /// <summary>
        /// Gets or sets the rate at max oil.
        /// </summary>
        public double? RateAtMaxOil { get; set; }

        /// <summary>
        /// Gets or sets the rate at max liquid.
        /// </summary>
        public double? RateAtMaxLiquid { get; set; }

        /// <summary>
        /// Gets or sets the analysis correlation.
        /// </summary>
        public int? AnalysisCorrelationID { get; set; }

        /// <summary>
        /// Gets or sets if the FBHP (flowing bottom hole pressure) calculation used defaults.
        /// </summary>
        public bool FBHPCalculationUsedDefaults { get; set; }

        /// <summary>
        /// Gets or sets the FBHP (flowing bottomhole pressure) calculation result.
        /// </summary>
        public string FBHPCalculationResult { get; set; }

        /// <summary>
        /// Gets or sets the SBHP (static bottomhole pressure) calculation result.
        /// </summary>
        public string SBHPCalculationResult { get; set; }

        /// <summary>
        /// Gets or sets the bubblepoint pressure source.
        /// </summary>
        public Lookup.Lookup BubblepointPressureSource { get; set; }

        /// <summary>
        /// Gets or sets if the FBHP (flowing bottomhole pressure) calculation measured depth is used.
        /// </summary>
        public bool? FBHPCalculationUsedMeasuredDepth { get; set; }

        /// <summary>
        /// Gets or sets the IPR calculation errors.
        /// </summary>
        public IList<Lookup.Lookup> IPRCalculationErrors { get; set; }

        /// <summary>
        /// Gets or sets the IPRWellConfig
        /// </summary>
        public IPRWellConfig IPRWellConfig { get; set; }

        /// <summary>
        /// Gets or sets the Curve
        /// </summary>
        public IList<Curve> Curve { get; set; }

    }
}
