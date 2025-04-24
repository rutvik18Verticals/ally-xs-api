namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the sensitivity analysis custom input type to MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class SensitivityAnalysisCustomInputType : LookupBase
    {

        /// <summary>
        /// Gets or sets the unique identifier for the sensitivity analysis custom input type.
        /// </summary>
        public int SensitivityAnalysisCustomInputTypeId { get; set; }

        /// <summary>
        /// Gets or sets the name of the sensitivity analysis custom input type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the associated phrase (nullable).
        /// </summary>
        public int? PhraseId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the optional annotation phrase (nullable).
        /// </summary>
        public int? OptionalAnnotationPhraseId { get; set; }

    }
}
