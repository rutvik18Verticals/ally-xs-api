namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the inputs common to all forms of Artifical Lift analysis
    /// </summary>
    public abstract class AnalysisInputBase
    {

        /// <summary>
        /// Gets or sets the oil rate
        /// </summary>
        public float? OilRate { get; set; }

        /// <summary>
        /// Gets or sets the water rate
        /// </summary>
        public float? WaterRate { get; set; }

        /// <summary>
        /// Gets or sets the gas rate
        /// </summary>
        public float? GasRate { get; set; }

        /// <summary>
        /// Gets or sets the gross rate
        /// </summary>
        public float? GrossRate { get; set; }

        /// <summary>
        /// Gets or sets the water specific gravity
        /// </summary>
        public float? WaterSpecificGravity { get; set; }

        /// <summary>
        /// Gets or sets the bottomhole test pressure
        /// </summary>
        public float? BottomholeTestPressure { get; set; }

        /// <summary>
        /// Gets or sets the tubing inner diameter
        /// </summary>
        public float? TubingInnerDiameter { get; set; }

        /// <summary>
        /// Gets or sets the casing pressure.
        /// </summary>
        public float? CasingPressure { get; set; }

        /// <summary>
        /// Gets or sets the multiphase flow correlation
        /// </summary>
        public AnalysisCorrelation MultiphaseFlowCorrelation { get; set; }

        /// <summary>
        /// Gets or sets the analysis type
        /// </summary>
        public AnalysisType AnalysisType { get; set; } = AnalysisType.WellTest;

        /// <summary>
        /// Gets or sets the use true vertical depth option.
        /// </summary>
        public bool UseTVD { get; set; }

    }
}
