namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// A Class to represent Downhole Separator.
    /// </summary>
    public class DownholeSeparator : LookupBase
    {

        /// <summary>
        /// Gets or sets the manufacturer of the downhole separator.
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the name of the downhole separator.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the phrase id.
        /// </summary>
        public int? PhraseId { get; set; }

    }
}