namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the rod material to MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class RodMaterial : LookupBase
    {

        /// <summary>
        /// Gets or sets the unique identifier for the rod material.
        /// </summary>
        public int RodMatlId { get; set; }

        /// <summary>
        /// Gets or sets the name of the rod material (nullable).
        /// </summary>
        public string Name { get; set; }

    }
}
