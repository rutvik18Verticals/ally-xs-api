namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    ///  Represents the input source for the analysis result.
    /// </summary>
    public class AnalysisSource
    {

        #region Properties

        /// <summary>
        /// Gets or sets the pump intake pressure source
        /// </summary>
        public int? PumpIntakePressureSource { get; set; }

        /// <summary>
        /// Gets or sets the fluid level above pump source
        /// </summary>
        public int? FluidLevelAbovePumpSource { get; set; }

        /// <summary>
        /// Gets or sets the tubing pressure source
        /// </summary>
        public int? TubingPressureSource { get; set; }

        /// <summary>
        /// Gets or sets the casing pressure source
        /// </summary>
        public int? CasingPressureSource { get; set; }

        /// <summary>
        /// Gets or sets the frequency source
        /// </summary>
        public int? FrequencySource { get; set; }

        /// <summary>
        /// Gets or sets the wellhead temperature source
        /// </summary>
        public int? WellHeadTemperatureSource { get; set; }

        /// <summary>
        /// Gets or sets the bottomhole temperature source
        /// </summary>
        public int? BottomholeTemperatureSource { get; set; }

        /// <summary>
        /// Gets or sets the oil specific gravity source
        /// </summary>
        public int? OilSpecificGravitySource { get; set; }

        /// <summary>
        /// Gets or sets the water specific gravity source
        /// </summary>
        public int? WaterSpecificGravitySource { get; set; }

        /// <summary>
        /// Gets or sets the gas specific gravity source
        /// </summary>
        public int? GasSpecificGravitySource { get; set; }

        /// <summary>
        /// Gets or sets the oil rate source.
        /// </summary>
        /// <value>
        /// The oil rate source.
        /// </value>
        public int? OilRateSource { get; set; }

        /// <summary>
        /// Gets or sets the water rate source.
        /// </summary>
        /// <value>
        /// The water rate source.
        /// </value>
        public int? WaterRateSource { get; set; }

        /// <summary>
        /// Gets or sets the gas rate source.
        /// </summary>
        /// <value>
        /// The gas rate source.
        /// </value>
        public int? GasRateSource { get; set; }

        /// <summary>
        /// Gets or sets the discharge gauge pressure source.
        /// </summary>
        /// <value>
        /// The discharge gauge pressure source.
        /// </value>
        public int? DischargeGaugePressureSource { get; set; }

        ///GL Analysis Properties
        /// <summary>
        /// Gets or sets the multiphase flow correlation source
        /// </summary>
        public int? MultiphaseFlowCorrelationSource { get; set; }

        /// <summary>
        /// Gets or sets the injected gas rate source
        /// </summary>
        public int? InjectedGasRateSource { get; set; }

        /// <summary>
        /// Gets or sets the downhole gauge pressure source
        /// </summary>
        public int? DownholeGaugePressureSource { get; set; }

        #endregion

    }
}
