namespace Theta.XSPOC.Apex.Api.Contracts.Responses.Values
{
    /// <summary>
    /// Represents esp pressure profile data.
    /// </summary>
    public class ESPPressureProfile
    {

        /// <summary>
        /// Gets or sets is valid.
        /// Indicates whether all requirements for valid esp pressure profile data were met.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// If applicable, this message explains why the esp pressure profile data is not valid.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the casing pressure.
        /// </summary>
        public ValueWithUnit<double> CasingPressure { get; set; }

        /// <summary>
        /// Gets or sets the tubing pressure.
        /// </summary>
        public ValueWithUnit<double> TubingPressure { get; set; }

        /// <summary>
        /// Gets or sets the pump intake pressure.
        /// </summary>
        public ValueWithUnit<double> PumpIntakePressure { get; set; }

        /// <summary>
        /// Gets or sets the used discharge gauge in analysis boolean value.
        /// </summary>
        public bool UsedDischargeGaugeInAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the pump discharge pressure.
        /// </summary>
        public ValueWithUnit<double> PumpDischargePressure { get; set; }

        /// <summary>
        /// Gets or sets the discharge gauge depth.
        /// </summary>
        public ValueWithUnit<double> DischargeGaugeDepth { get; set; }

        /// <summary>
        /// Gets or sets the pump static pressure.
        /// </summary>
        public ValueWithUnit<double> PumpStaticPressure { get; set; }

        /// <summary>
        /// Gets or sets the fluid level.
        /// </summary>
        public ValueWithUnit<double> FluidLevel { get; set; }

        /// <summary>
        /// Gets or sets the vertical pump depth.
        /// </summary>
        public ValueWithUnit<double> VerticalPumpDepth { get; set; }

        /// <summary>
        /// Gets or sets the perforation depth.
        /// </summary>
        public ValueWithUnit<double> PerforationDepth { get; set; }

        /// <summary>
        /// Gets or sets the pump friction delta.
        /// </summary>
        public ValueWithUnit<double> PumpFrictionDelta { get; set; }

        /// <summary>
        /// Gets or sets the pump pressure delta.
        /// </summary>
        public ValueWithUnit<double> PumpPressureDelta { get; set; }

        /// <summary>
        /// Gets or sets the flowing bhp.
        /// </summary>
        public ValueWithUnit<double> FlowingBHP { get; set; }

        /// <summary>
        /// Gets or sets the flowing bhp gradient.
        /// </summary>
        public string FlowingBHPGradient { get; set; }

        /// <summary>
        /// Gets or sets the static gradient.
        /// </summary>
        public string StaticGradient { get; set; }

        /// <summary>
        /// Gets or sets the pressure gradient units.
        /// </summary>
        public string PressureGradientUnits { get; set; }

        /// <summary>
        /// Gets or sets the discharge gauge pressure.
        /// </summary>
        public ValueWithUnit<double> DischargeGaugePressure { get; set; }

    }
}
