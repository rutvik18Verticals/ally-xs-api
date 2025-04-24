namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset.RodPump
{
    /// <summary>
    /// A class representing a rod from a rod pump.
    /// </summary>
    public class Rod
    {

        /// <summary>
        /// Gets or sets the location of the rod downhole.
        /// </summary>
        public short RodNumber { get; set; }

        /// <summary>
        /// Gets or sets the length of the rod.
        /// </summary>
        public int? Length { get; set; }

        /// <summary>
        /// Gets or sets the drag friction coefficient of the rod.
        /// </summary>
        public double? DragFrictionCoefficient { get; set; }

        /// <summary>
        /// Gets or sets the guide count per rod.
        /// </summary>
        public double? GuideCountPerRod { get; set; }

        /// <summary>
        /// Gets or sets the diameter of the rod.
        /// </summary>
        public double? Diameter { get; set; }

        /// <summary>
        /// Gets or sets the material of the rod.
        /// </summary>
        public Lookup.Lookup Material { get; set; }

        /// <summary>
        /// GEts or sets the grade of the rod.
        /// </summary>
        public Lookup.Lookup RodGrade { get; set; }

        /// <summary>
        /// Gets or sets the size group of the rod.
        /// </summary>
        public Lookup.Lookup RodSizeGroup { get; set; }

        /// <summary>
        /// Gets or sets the size of the rod.
        /// </summary>
        public Lookup.Lookup RodSize { get; set; }

        /// <summary>
        /// Gets or sets the guide of the rod.
        /// </summary>
        public Lookup.Lookup RodGuide { get; set; }

    }
}
