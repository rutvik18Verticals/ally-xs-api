using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Enumerates the industry applications (or capability) in the oil and gas industry.
    /// </summary>
    public class IndustryApplication : EnhancedEnumBase
    {

        #region Fields

        /// <summary>
        /// Gets a value indicating whether the application is a form of artificial lift.
        /// </summary>
        public bool IsArtificialLift { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Necessary for implementing the EnhancedEnumBase
        /// </summary>
        protected IndustryApplication(int key, Text name)
            : base(key, name)
        {
        }

        #endregion

        #region Static Properties

        /// <summary>
        /// Indicates that no known applications are supported.
        /// </summary>
        public static IndustryApplication None { get; } = CreateValue(0, "None", false);

        /// <summary>
        /// Indicates the support of Rod Artificial Lift application functionality.
        /// </summary>
        public static IndustryApplication RodArtificialLift { get; } =
            CreateValue(3, "Rod Artificial Lift", true);

        /// <summary>
        /// Indicates the support of Electric-Submersible Pump (ESP) Artificial Lift application functionality.
        /// </summary>
        public static IndustryApplication ESPArtificialLift { get; } = CreateValue(4, "ESP Artificial Lift", true);

        /// <summary>
        /// Indicates the support of Progressive-Cavity Pump (PCP) Artificial Lift application functionality.
        /// </summary>
        public static IndustryApplication PCPArtificialLift { get; } = CreateValue(5, "PCP Artificial Lift", true);

        /// <summary>
        /// Indicates the support of Plunger Artificial Lift application functionality.
        /// </summary>
        public static IndustryApplication PlungerArtificialLift { get; } =
            CreateValue(6, "Plunger Artificial Lift", true);

        /// <summary>
        /// Indicates the support of Gas Artificial Lift application functionality.
        /// </summary>
        public static IndustryApplication GasArtificialLift { get; } = CreateValue(7, "Gas Artificial Lift", true);

        /// <summary>
        /// Indicates the support of Gas Flow Meter application functionality.
        /// </summary>
        public static IndustryApplication GasFlowMeter { get; } = CreateValue(8, "Gas Flow Meter", false);

        /// <summary>
        /// Indicates the support of Liquid Flow Meter application functionality.
        /// </summary>
        public static IndustryApplication LiquidFlowMeter { get; } = CreateValue(9, "Liquid Flow Meter", false);

        /// <summary>
        /// Indicates the support of Tank application functionality.
        /// </summary>
        public static IndustryApplication Tank { get; } = CreateValue(10, "Tank", false);

        /// <summary>
        /// Indicates the support of Proportional-Integral-Derivative (PID) application functionality.
        /// </summary>
        public static IndustryApplication PID { get; } = CreateValue(11, "PID", false);

        /// <summary>
        /// Indicates the support of Valve application functionality.
        /// </summary>
        public static IndustryApplication Valve { get; } = CreateValue(12, "Valve", false);

        /// <summary>
        /// Indicates the support of Injection application functionality.
        /// </summary>
        public static IndustryApplication Injection { get; } = CreateValue(13, "Injection", false);

        /// <summary>
        /// Indicates the support of Pump application functionality.
        /// </summary>
        public static IndustryApplication Pump { get; } = CreateValue(14, "Pump", false);

        /// <summary>
        /// Indicates the support of Jet Pump Artificial Lift application functionality.
        /// </summary>
        public static IndustryApplication JetPumpArtificialLift { get; } =
            CreateValue(15, "Jet Pump Artificial Lift", true);

        /// <summary>
        /// Indicates the support of Chemical Injection application functionality.
        /// </summary>
        public static IndustryApplication ChemicalInjection { get; } = CreateValue(17, "Chemical Injection", false);

        /// <summary>
        /// Indicates the support of Plunger-Assisted Gas Artificial Lift application functionality.
        /// </summary>
        public static IndustryApplication PlungerAssistedGasArtificialLift { get; } =
            CreateValue(16, "Plunger-Assisted Gas Artificial Lift", true);

        /// <summary>
        /// Indicates the support of Facility application functionality.
        /// </summary>
        public static IndustryApplication Facility { get; } = CreateValue(18, "Facility", false);

        /// <summary>
        /// Gets the default value.
        /// </summary>
        public static IndustryApplication Default { get; } = None;

        #endregion

        #region Static Methods

        private static IndustryApplication CreateValue(int key, string name, bool isArtificialLift)
        {
            var value = new IndustryApplication(key, new Text(name))
            {
                IsArtificialLift = isArtificialLift,
            };

            Register(value);

            return value;
        }

        #endregion

    }
}
