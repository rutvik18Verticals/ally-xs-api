namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Analysis
{
    /// <summary>
    /// A class that represents an ESP Analysis Result.
    /// </summary>
    public class IPRWellConfig
    {
        /// <summary>
        /// Gets or sets the AnalysisCorrelation.
        /// </summary>
        public Lookup.Lookup AnalysisCorrelation { get; set; }

        /// <summary>
        /// Gets or sets the FBHPAnalysisSource.
        /// </summary>
        public Lookup.Lookup FBHPAnalysisSource { get; set; }

        /// <summary>
        /// Gets or sets the GrossRateAnalysisSource.
        /// </summary>
        public Lookup.Lookup GrossRateAnalysisSource { get; set; }

        /// <summary>
        /// Gets or sets the WaterCutAnalysisSource.
        /// </summary>
        public Lookup.Lookup WaterCutAnalysisSource { get; set; }
    }
}
