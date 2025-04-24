namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the Facility Tag Groups MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class FacilityTagGroups : LookupBase
    {
        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the DisplayOrder
        /// </summary>
        public int? DisplayOrder { get; set; }
    }
}
