namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// A class representing the cost type lookup.
    /// </summary>
    public class CostType : LookupBase
    {

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the phrase id.
        /// </summary>
        public int? PhraseId { get; set; }

    }
}
