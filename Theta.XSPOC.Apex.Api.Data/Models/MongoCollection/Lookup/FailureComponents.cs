namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{

    /// <summary>
    /// This class defines the failure components MongoDB sub document for the <seealso cref="LookupBase"/>.
    /// </summary>
    public class FailureComponents : LookupBase
    {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int FailureComponentsId { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the application id.
        /// </summary>
        public int? ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the Phrase Id.
        /// </summary>
        public int? PhraseId { get; set; }

    }
}
