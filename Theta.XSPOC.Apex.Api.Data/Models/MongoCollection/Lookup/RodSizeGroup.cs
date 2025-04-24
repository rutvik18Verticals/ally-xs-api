namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the rod size group to MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class RodSizeGroup : LookupBase
    {

        /// <summary>
        /// Gets or sets the unique identifier for the rod size group.
        /// </summary>
        public int RodSizeGroupId { get; set; }

        /// <summary>
        /// Gets or sets the name of the rod size group (nullable).
        /// </summary>
        public string Name { get; set; }

    }
}
