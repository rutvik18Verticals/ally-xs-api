namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the correlations MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class Correlations : LookupBase
    {

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Phrase Id.
        /// </summary>
        public int PhraseId { get; set; }

    }
}
