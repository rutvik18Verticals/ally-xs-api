namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the rod size to MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class RodSize : LookupBase
    {

        /// <summary>
        /// Gets or sets the unique identifier for the rod size.
        /// </summary>
        public int RodSizeId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the associated rod size group (nullable).
        /// </summary>
        public int? RodSizeGroupId { get; set; }

        /// <summary>
        /// Gets or sets the display name of the rod size (nullable).
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the diameter of the rod size (nullable).
        /// </summary>
        public float? Diameter { get; set; }

        /// <summary>
        /// Gets or sets the weight of the rod size (nullable).
        /// </summary>
        public float? RodWeight { get; set; }

        /// <summary>
        /// Gets or sets sinker bar (nullable).
        /// </summary>
        public bool? SinkerBar { get; set; }

    }
}
