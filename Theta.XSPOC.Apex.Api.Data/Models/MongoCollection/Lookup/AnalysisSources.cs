namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{

    /// <summary>
    /// This class defines the analysis sources MongoDB sub document for the <seealso cref="LookupBase"/>.
    /// </summary>
    public class AnalysisSources : LookupBase
    {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int AnalysisSourcesId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the phrase id.
        /// </summary>
        public int? PhraseId { get; set; }

    }
}
