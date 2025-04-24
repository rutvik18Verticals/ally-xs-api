using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the inputs for an ESP analysis result
    /// </summary>
    public class ESPAnalysisInput : AnalysisInputBase
    {

        #region Properties

        /// <summary>
        /// Gets or sets the list of pump configurations
        /// </summary>
        public IList<PumpConfiguration> PumpConfigs { get; set; }

        /// <summary>
        /// Gets or sets the number of stages
        /// </summary>
        public int? NumberOfStages { get; set; }

        /// <summary>
        /// Gets or sets the vertical pump depth
        /// </summary>
        public float? VerticalPumpDepth { get; set; }

        /// <summary>
        /// Gets or sets the measured depth
        /// </summary>
        public float? MeasuredPumpDepth { get; set; }

        /// <summary>
        /// Gets or sets the pump intake pressure
        /// </summary>
        public float? PumpIntakePressure { get; set; }

        /// <summary>
        /// Gets or sets the fluid level above pump
        /// </summary>
        public float? FluidLevelAbovePump { get; set; }

        /// <summary>
        /// Gets or sets the tubing pressure
        /// </summary>
        public float? TubingPressure { get; set; }

        /// <summary>
        /// Gets or sets the frequency
        /// </summary>
        public float? Frequency { get; set; }

        /// <summary>
        /// Gets or sets the oil API gravity.
        /// </summary>
        public float? OilAPI { get; set; }

        /// <summary>
        /// Gets or sets the gas handling inputs
        /// </summary>
        public GasHandlingAnalysisInput GasHandlingInputs { get; set; }

        /// <summary>
        /// Gets or sets the discharge gauge pressure.
        /// </summary>
        /// <value>
        /// The discharge gauge pressure.
        /// </value>
        public float? DischargeGaugePressure { get; set; }

        /// <summary>
        /// Gets or sets the discharge gauge pressure depth.
        /// </summary>
        /// <value>
        /// The discharge gauge pressure depth.
        /// </value>
        public float? DischargeGaugeDepth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the discharge gauge in analysis.
        /// </summary>
        /// <value>
        ///   <c>true</c> to use the discharge gauge in analysis; otherwise, <c>false</c>.
        /// </value>
        public bool UseDischargeGaugeInAnalysis { get; set; }

        #endregion

    }
}
