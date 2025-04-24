namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{

    /// <summary>
    /// This class defines the failure sub components MongoDB sub document for the <seealso cref="LookupBase"/>.
    /// </summary>
    public class FailureSubcomponents : LookupBase
    {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int FailureSubcomponentsId { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the component id.
        /// </summary>
        public int? ComponentId { get; set; }

        /// <summary>
        /// Gets or sets the Phrase Id.
        /// </summary>
        public int? PhraseId { get; set; }

    }
}
