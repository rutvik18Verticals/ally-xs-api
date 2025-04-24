namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the rod guide to MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class RodGuide : LookupBase
    {

        /// <summary>
        /// Gets or sets the unique identifier for the rod guide.
        /// </summary>
        public short RodGuideId { get; set; }

        /// <summary>
        /// Gets or sets the name of the rod guide.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the associated phrase (nullable).
        /// </summary>
        public int? PhraseId { get; set; }

        /// <summary>
        /// Gets or sets the default drag friction coefficient (nullable).
        /// </summary>
        public float? DefaultDragFrictionCoefficient { get; set; }

        /// <summary>
        /// Gets or sets the default side load (nullable).
        /// </summary>
        public float? DefaultSideLoad { get; set; }

    }
}
