namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the wellbore view data.
    /// </summary>
    public class GLAnalysisWellboreViewData
    {

        /// <summary>
        /// Gets or sets the well depth.
        /// </summary>
        public float? WellDepth { get; set; }

        /// <summary>
        /// Gets or sets the packer depth.
        /// </summary>
        public float? PackerDepth { get; set; }

        /// <summary>
        /// Gets or sets the injection depth.
        /// </summary>
        public float? InjectionDepth { get; set; }

        /// <summary>
        /// Gets or sets the has packer.
        /// </summary>
        public bool HasPacker { get; set; }

        /// <summary>
        /// Gets or sets the tubing depth.
        /// </summary>
        public float? TubingDepth { get; set; }

        /// <summary>
        /// Gets or sets the top fluid column.
        /// </summary>
        public float? TopFluidColumn { get; set; }

        /// <summary>
        /// Gets or sets the bottom fluid column.
        /// </summary>
        public float? BottomFluidColumn { get; set; }

        /// <summary>
        /// Gets or sets the casing fluid column.
        /// </summary>
        public float? CasingFluidColumn { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string ErrorMessage { get; set; }

    }
}
