namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines therod grade to MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class RodGrade : LookupBase
    {

        /// <summary>
        /// Gets or sets the unique identifier for the rod grade.
        /// </summary>
        public int RodGradeId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the associated rod size group (nullable).
        /// </summary>
        public int? RodSizeGroupId { get; set; }

        /// <summary>
        /// Gets or sets the name of the rod grade (nullable).
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the display order of the rod grade (nullable).
        /// </summary>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the elasticity value of the rod grade (nullable).
        /// </summary>
        public float? Elasticity { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the associated rod material (nullable).
        /// </summary>
        public int? RodMatlId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the stress method (nullable).
        /// </summary>
        public int? StressMethodId { get; set; }

        /// <summary>
        /// Gets or sets the tensile strength value of the rod grade (nullable).
        /// </summary>
        public float? TensileStrength { get; set; }

    }
}
