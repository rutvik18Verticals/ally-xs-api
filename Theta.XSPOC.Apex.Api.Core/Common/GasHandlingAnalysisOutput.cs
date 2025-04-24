namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Contains all the data obtained from diagnostics on an ESP well
    /// </summary>
    public class GasHandlingAnalysisOutput
    {

        /// <summary>
        /// Gets or sets the gas/oil ratio at the pump.
        /// </summary>
        public double? GasOilRatioAtPump { get; set; }

        /// <summary>
        /// Gets or sets the relative density of the oil in specific gravity.
        /// </summary>
        public double? OilSpecificGravity { get; set; }

        /// <summary>
        /// Gets or sets the formation volume factor.
        /// </summary>
        public double? FormationVolumeFactor { get; set; }

        /// <summary>
        /// Gets or sets the gas compressibility factor.
        /// </summary>
        public double? GasCompressibilityFactor { get; set; }

        /// <summary>
        /// Gets or sets the gas formation volume factor.
        /// </summary>
        public double? GasFormationVolumeFactor { get; set; }

        /// <summary>
        /// Gets or sets the producing gas/oil ratio.
        /// </summary>
        public double? ProducingGasOilRatio { get; set; }

        /// <summary>
        /// Gets or sets the gas in solution.
        /// </summary>
        public double? GasInSolution { get; set; }

        /// <summary>
        /// Gets or sets the free (not dissolved) gas at the pump.
        /// </summary>
        public double? FreeGasAtPump { get; set; }

        /// <summary>
        /// Get or sets the oil volume at the pump.
        /// </summary>
        public double? OilVolumeAtPump { get; set; }

        /// <summary>
        /// Gets or sets the total (free + dissolved) gas volume at the pump.
        /// </summary>
        public double? GasVolumeAtPump { get; set; }

        /// <summary>
        /// Gets or sets the total volume at the pump.
        /// </summary>
        public double? TotalVolumeAtPump { get; set; }

        /// <summary>
        /// Gets or sets the percentage of free gas.
        /// </summary>
        public double? FreeGas { get; set; }

        /// <summary>
        /// Gets or sets the turpin parameter.
        /// </summary>
        public double? TurpinParameter { get; set; }

        /// <summary>
        /// Gets or sets the composite tubing specific gravity.
        /// </summary>
        public double? CompositeTubingSpecificGravity { get; set; }

        /// <summary>
        /// Gets or sets the gas density.
        /// </summary>
        public double? GasDensity { get; set; }

        /// <summary>
        /// Gets or sets the liquid density.
        /// </summary>
        public double? LiquidDensity { get; set; }

        /// <summary>
        /// Gets or sets the annular separation efficiency.
        /// </summary>
        public double? AnnularSeparationEfficiency { get; set; }

        /// <summary>
        /// Gets or sets the gas in the tubing.
        /// </summary>
        public double? TubingGas { get; set; }

        /// <summary>
        /// Gets or sets the gas/oil ratio in the tubing.
        /// </summary>
        public double? TubingGasOilRatio { get; set; }

    }
}
