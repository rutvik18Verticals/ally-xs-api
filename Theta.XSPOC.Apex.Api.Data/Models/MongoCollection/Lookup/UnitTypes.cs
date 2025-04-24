namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the POC types MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class UnitTypes : LookupBase
    {

        /// <summary>
        /// Gets or sets the Unit types Id.
        /// </summary>
        public int UnitTypesId { get; set; }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Phrase Id.
        /// </summary>
        public int? PhraseId { get; set; }

    }
}
