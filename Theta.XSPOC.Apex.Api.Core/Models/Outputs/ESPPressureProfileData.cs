using Theta.XSPOC.Apex.Kernel.Quantity;
using Theta.XSPOC.Apex.Kernel.Quantity.Measures;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents esp pressure profile data.
    /// </summary>
    public class ESPPressureProfileData
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
        /// Gets or sets the pump intake pressure.
        /// </summary>
        public Quantity<Pressure> PumpIntakePressure { get; set; }

        /// <summary>
        /// Gets or sets the pump discharge.
        /// </summary>
        public Quantity<Pressure> PumpDischargePressure { get; set; }

        /// <summary>
        /// Gets or sets the pump static pressure.
        /// </summary>
        public Quantity<Pressure> PumpStaticPressure { get; set; }

        /// <summary>
        /// Gets or sets the pump pressure delta.
        /// </summary>
        public Quantity<Pressure> PumpPressureDelta { get; set; }

        /// <summary>
        /// Gets or sets the pump friction delta.
        /// </summary>
        public Quantity<Pressure> PumpFrictionDelta { get; set; }

        /// <summary>
        /// Gets or sets the casing pressure.
        /// </summary>
        public Quantity<Pressure> CasingPressure { get; set; }

        /// <summary>
        /// Gets or sets the tubing pressure.
        /// </summary>
        public Quantity<Pressure> TubingPressure { get; set; }

        /// <summary>
        /// Gets or sets the flowing bottomhole pressure.
        /// </summary>
        public Quantity<Pressure> FlowingBottomholePressure { get; set; }

        /// <summary>
        /// Gets or sets the pressure gradient units.
        /// </summary>
        public string PressureGradientUnits { get; set; }

        /// <summary>
        /// Gets or sets the flowing bottomhole pressure gradient.
        /// </summary>
        public string FlowingBottomholePressureGradient { get; set; }

        /// <summary>
        /// Gets or sets the static gradient.
        /// </summary>
        public string StaticGradient { get; set; }

        /// <summary>
        /// Gets or sets the use discharge gauge in analysis.
        /// </summary>
        public bool UseDischargeGaugeInAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the discharge gauge pressure.
        /// </summary>
        public Quantity<Pressure> DischargeGaugePressure { get; set; }

        /// <summary>
        /// Gets or sets the discharge gauge depth.
        /// </summary>
        public Quantity<Length> DischargeGaugeDepth { get; set; }

        /// <summary>
        /// Gets or sets the fluid level.
        /// </summary>
        public Quantity<Length> FluidLevel { get; set; }

        /// <summary>
        /// Gets or sets the vertical pump depth.
        /// </summary>
        public Quantity<Length> VerticalPumpDepth { get; set; }

        /// <summary>
        /// Gets or sets the perforation depth.
        /// </summary>
        public Quantity<Length> PerforationDepth { get; set; }

    }
}
