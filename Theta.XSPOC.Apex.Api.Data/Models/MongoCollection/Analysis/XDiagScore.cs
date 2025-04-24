namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Analysis
{
    /// <summary>
    /// A class that represents XDiag Scores.
    /// </summary>
    public class XDiagScores
    {
        /// <summary>
        /// Gets or sets the total score.
        /// </summary>
        public double? TotalScore { get; set; }

        /// <summary>
        /// Gets or sets the total summary.
        /// </summary>
        public string TotalSummary { get; set; }

        /// <summary>
        /// Gets or sets the gearbox score.
        /// </summary>
        public double? GearboxScore { get; set; }

        /// <summary>
        /// Gets or sets the gearbox summary.
        /// </summary>
        public string GearboxSummary { get; set; }

        /// <summary>
        /// Gets or sets the rod loading score.
        /// </summary>
        public double? RodLoadingScore { get; set; }

        /// <summary>
        /// Gets or sets the rod loading summary.
        /// </summary>
        public string RodLoadingSummary { get; set; }

        /// <summary>
        /// Gets or sets the structure loading score.
        /// </summary>
        public double? StructureLoadingScore { get; set; }

        /// <summary>
        /// Gets or sets the structure loading summary.
        /// </summary>
        public string StructureLoadingSummary { get; set; }

        /// <summary>
        /// Gets or sets the system efficiency score.
        /// </summary>
        public double? SystemEfficiencyScore { get; set; }

        /// <summary>
        /// Gets or sets the system efficiency summary.
        /// </summary>
        public string SystemEfficiencySummary { get; set; }

        /// <summary>
        /// Gets or sets the bottom min stress score.
        /// </summary>
        public double? BottomMinStressScore { get; set; }

        /// <summary>
        /// Gets or sets the bottom min stress summary.
        /// </summary>
        public string BottomMinStressSummary { get; set; }

        /// <summary>
        /// Gets or sets the MPRL score.
        /// </summary>
        public double? MPRLScore { get; set; }

        /// <summary>
        /// Gets or sets the MPRL summary.
        /// </summary>
        public string MPRLSummary { get; set; }

        /// <summary>
        /// Gets or sets the total summary locale.
        /// </summary>
        public string TotalSummaryLocale { get; set; }

        /// <summary>
        /// Gets or sets the gearbox summary locale.
        /// </summary>
        public string GearboxSummaryLocale { get; set; }

        /// <summary>
        /// Gets or sets the rod loading summary locale.
        /// </summary>
        public string RodLoadingSummaryLocale { get; set; }

        /// <summary>
        /// Gets or sets the structure loading summary locale.
        /// </summary>
        public string StructureLoadingSummaryLocale { get; set; }

        /// <summary>
        /// Gets or sets the system efficiency summary locale.
        /// </summary>
        public string SystemEfficiencySummaryLocale { get; set; }

        /// <summary>
        /// Gets or sets the bottom min stress summary locale.
        /// </summary>
        public string BottomMinStressSummaryLocale { get; set; }

        /// <summary>
        /// Gets or sets the MPRL summary locale.
        /// </summary>
        public string MPRLSummaryLocale { get; set; }
    }
}
