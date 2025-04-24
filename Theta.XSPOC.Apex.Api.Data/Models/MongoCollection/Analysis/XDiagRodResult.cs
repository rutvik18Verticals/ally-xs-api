namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Analysis
{
    /// <summary>
    /// Represents a XDiagRodResult
    /// </summary>
    public class XDiagRodResult
    {
        /// <summary>
        /// Gets or sets the Rod Num.
        /// </summary>
        public int RodNum { get; set; }

        /// <summary>
        /// Gets or sets the Grade.
        /// </summary>
        public string Grade { get; set; }

        /// <summary>
        /// Gets or sets the Length.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets the Diameter.
        /// </summary>
        public double? Diameter { get; set; }

        /// <summary>
        /// Gets or sets the Loading.
        /// </summary>
        public double? Loading { get; set; }

        /// <summary>
        /// Gets or sets the BottomMinStress.
        /// </summary>
        public double? BottomMinStress { get; set; }

        /// <summary>
        /// Gets or sets the TopMinStress.
        /// </summary>
        public double? TopMinStress { get; set; }

        /// <summary>
        /// Gets or sets the TopMaxStress.
        /// </summary>
        public double? TopMaxStress { get; set; }

        /// <summary>
        /// Gets or sets the RodGuide.
        /// </summary>
        public Lookup.Lookup RodGuide { get; set; }

        /// <summary>
        /// Gets or sets the DragFrictionCoefficient.
        /// </summary>
        public double? DragFrictionCoefficient { get; set; }

        /// <summary>
        /// Gets or sets the GuideCountPerRod.
        /// </summary>
        public double? GuideCountPerRod { get; set; }

    }
}
