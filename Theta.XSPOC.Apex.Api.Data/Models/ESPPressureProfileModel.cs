using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents the raw data needed to generate an esp pressure profile.
    /// </summary>
    public class ESPPressureProfileModel
    {

        /// <summary>
        /// Gets or sets the pump intake pressure.
        /// </summary>
        public float? PumpIntakePressure { get; set; }

        /// <summary>
        /// Gets or sets the pump discharge pressure.
        /// </summary>
        public float? PumpDischargePressure { get; set; }

        /// <summary>
        /// Gets or sets the pump static pressure.
        /// </summary>
        public float? PumpStaticPressure { get; set; }

        /// <summary>
        /// Gets or sets the pressure across pump.
        /// </summary>
        public float? PressureAcrossPump { get; set; }

        /// <summary>
        /// Gets or sets the frictional pressure drop.
        /// </summary>
        public float? FrictionalPressureDrop { get; set; }

        /// <summary>
        /// Gets or sets the casing pressure.
        /// </summary>
        public float? CasingPressure { get; set; }

        /// <summary>
        /// Gets or sets the tubing pressure.
        /// </summary>
        public float? TubingPressure { get; set; }

        /// <summary>
        /// Gets or sets the flowing bottomhole pressure.
        /// </summary>
        public float? FlowingBottomholePressure { get; set; }

        /// <summary>
        /// Gets or sets the composite tubing specific gravity.
        /// </summary>
        public float? CompositeTubingSpecificGravity { get; set; }

        /// <summary>
        /// Gets or sets the water rate.
        /// </summary>
        public float? WaterRate { get; set; }

        /// <summary>
        /// Gets or sets the oil rate.
        /// </summary>
        public float? OilRate { get; set; }

        /// <summary>
        /// Gets or sets the water specific gravity.
        /// </summary>
        public float? WaterSpecificGravity { get; set; }

        /// <summary>
        /// Gets or sets the is gas handling enabled boolean.
        /// </summary>
        public bool IsGasHandlingEnabled { get; set; }

        /// <summary>
        /// Gets or sets the specific gravity of oil.
        /// </summary>
        public float? SpecificGravityOfOil { get; set; }

        /// <summary>
        /// Gets or sets the use discharge gauge in analysis boolean value.
        /// </summary>
        public bool UseDischargeGaugeInAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the discharge gauge pressure.
        /// </summary>
        public float? DischargeGaugePressure { get; set; }

        /// <summary>
        /// Gets or sets the discharge gauge depth.
        /// </summary>
        public float? DischargeGaugeDepth { get; set; }

        /// <summary>
        /// Gets or sets the vertical pump depth.
        /// </summary>
        public float? VerticalPumpDepth { get; set; }

        /// <summary>
        /// Gets or sets the calculated fluid level above pump.
        /// </summary>
        public float? CalculatedFluidLevelAbovePump { get; set; }

        /// <summary>
        /// Gets or sets the perforations.
        /// </summary>
        public IList<ESPPerforation> Perforations { get; set; }

        /// <summary>
        /// Gets or sets the production depth.
        /// </summary>
        public float? ProductionDepth { get; set; }

    }
}
