namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Contains all the data needed to run gas handling diagnostics on an ESP well
    /// </summary>
    public class GasHandlingAnalysisInput
    {

        /// <summary>
        /// Gets or sets a value indicating whether the casing valve is closed.
        /// </summary>
        public bool? IsCasingValveClosed { get; set; }

        /// <summary>
        /// Gets or sets the gas relative density.
        /// </summary>
        public double? GasRelativeDensity { get; set; }

        /// <summary>
        /// Gets or sets the wellhead temperature.
        /// </summary>
        public double? WellheadTemperature { get; set; }

        /// <summary>
        /// Gets or sets the bottomhole temperature.
        /// </summary>
        public double? BottomholeTemperature { get; set; }

        /// <summary>
        /// Gets or sets the gas separator efficiency.
        /// </summary>
        public double? GasSeparatorEfficiency { get; set; }

        /// <summary>
        /// Gets or sets the inner diameter of the casing.
        /// </summary>
        public double? CasingInnerDiameter { get; set; }

        /// <summary>
        /// Gets or sets the outer diameter of the tubing.
        /// </summary>
        public double? TubingOuterDiameter { get; set; }

    }
}
