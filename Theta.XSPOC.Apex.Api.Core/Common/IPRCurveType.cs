using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// This is the implementation that represents the configuration of a ipr curve type.
    /// </summary>
    public class IPRCurveType : EnhancedEnumBase, ICurveType
    {

        #region Static Properties

        /// <summary>
        /// Gets the IPR Curve for ESP
        /// </summary>
        public static IPRCurveType ESPIPRCurve { get; private set; }

        /// <summary>
        /// Gets the IPR Curve for Gas Lift
        /// </summary>
        public static IPRCurveType GasLiftIPRCurve { get; private set; }

        /// <summary>
        /// Gets the IPR Curve for Rod Pump
        /// </summary>
        public static IPRCurveType RodPumpIPRCurve { get; private set; }

        /// <summary>
        /// Gets the IPR Curve for PCP
        /// </summary>
        public static IPRCurveType PCPIPRCurve { get; private set; }

        /// <summary>
        /// Gets the IPR Curve for Plunger Lift
        /// </summary>
        public static IPRCurveType PlungerLiftIPRCurve { get; private set; }

        /// <summary>
        /// Gets the IPR Curve for Jet Pump
        /// </summary>
        public static IPRCurveType JetPumpIPRCurve { get; private set; }

        /// <summary>
        /// Gets the IPR Curve for Plunger Assisted Gas Lift
        /// </summary>
        public static IPRCurveType PlungerAssistedGasLiftIPRCurve { get; private set; }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="UnitCategory"/> that this curve's x axis is measured in.
        /// </summary>
        public UnitCategory XAxisUnitCategory => UnitCategory.FluidRate;

        /// <summary>
        /// Gets the <see cref="UnitCategory"/> that this curve's y axis is measured in.
        /// </summary>
        public UnitCategory YAxisUnitCategory => UnitCategory.Pressure;

        /// <summary>
        /// Gets the <see cref="IndustryApplication"/> this curve is associated with.
        /// </summary>
        public IndustryApplication IndustryApplication { get; }

        #endregion

        #region Constructors

        static IPRCurveType()
        {
            ESPIPRCurve = CreateValue(18, Text.FromString("IPR curve for ESP"),
                IndustryApplication.ESPArtificialLift);
            GasLiftIPRCurve = CreateValue(19, Text.FromString("IPR curve for Gas Lift"),
                IndustryApplication.GasArtificialLift);
            RodPumpIPRCurve = CreateValue(20, Text.FromString("IPR curve for Rod Pump"),
                IndustryApplication.RodArtificialLift);
            PCPIPRCurve = CreateValue(21, Text.FromString("IPR curve for PCP"),
                IndustryApplication.PCPArtificialLift);
            PlungerLiftIPRCurve = CreateValue(22, Text.FromString("IPR curve for Plunger Lift"),
                IndustryApplication.PlungerArtificialLift);
            JetPumpIPRCurve = CreateValue(23, Text.FromString("IPR curve for Jet Pump"),
                IndustryApplication.JetPumpArtificialLift);
            PlungerAssistedGasLiftIPRCurve = CreateValue(24,
                Text.FromString("IPR curve for Plunger Assisted Gas Lift"),
                IndustryApplication.PlungerAssistedGasArtificialLift);
        }

        /// <summary>
        /// Initializes a new <see cref="IPRCurveType"/> with a specified key and name as well as industry application.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        /// <param name="industryApplication">The <see cref="IndustryApplication"/> that this curve is associated with.
        /// </param>
        private IPRCurveType(int key, Text name, IndustryApplication industryApplication)
            : base(key, name)
        {
            IndustryApplication = industryApplication;
        }

        /// <summary>
        /// Constructs a new IPR Curve Type
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        public IPRCurveType(int key, Text name) : base(key, name)
        {
        }

        #endregion

        #region Private Methods

        private static IPRCurveType CreateValue(int key, string name, IndustryApplication industryApplication)
        {
            var value = new IPRCurveType(key, new Text(name), industryApplication);

            Register(value);

            return value;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the IPR curve for the given IndustryApplication
        /// </summary>
        /// <param name="application">The Industry Application</param>
        /// <returns>An IPRCurveType for the application if defined, else null</returns>
        public static IPRCurveType GetCurveTypeFor(IndustryApplication application)
        {
            if (application == IndustryApplication.ESPArtificialLift)
            {
                return ESPIPRCurve;
            }
            else if (application == IndustryApplication.GasArtificialLift)
            {
                return GasLiftIPRCurve;
            }
            else if (application == IndustryApplication.JetPumpArtificialLift)
            {
                return JetPumpIPRCurve;
            }
            else if (application == IndustryApplication.PCPArtificialLift)
            {
                return PCPIPRCurve;
            }
            else if (application == IndustryApplication.PlungerArtificialLift)
            {
                return PlungerLiftIPRCurve;
            }
            else if (application == IndustryApplication.PlungerAssistedGasArtificialLift)
            {
                return PlungerAssistedGasLiftIPRCurve;
            }
            else if (application == IndustryApplication.RodArtificialLift)
            {
                return RodPumpIPRCurve;
            }
            else
            {
                return null;
            }
        }

        #endregion

    }
}
