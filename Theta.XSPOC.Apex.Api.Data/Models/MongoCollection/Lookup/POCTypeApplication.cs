namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the POC Type application to MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class POCTypeApplication : LookupBase
    {

        /// <summary>
        /// Gets or sets the POCType.
        /// </summary>
        public int POCType { get; set; }

        /// <summary>
        /// Gets or sets the ApplicationId.
        /// </summary>
        public int ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this application is the default for the specified POCType.
        /// </summary>
        public bool IsDefault { get; set; }

    }
}
