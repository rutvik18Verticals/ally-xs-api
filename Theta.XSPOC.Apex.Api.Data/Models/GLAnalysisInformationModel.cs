namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the esp pump model.
    /// </summary>
    public class GLAnalysisInformationModel
    {

        /// <summary>
        /// Gets or set the bottomhole temperature.
        /// </summary>
        public string BottomholeTemperature { get; set; }

        /// <summary>
        /// Gets or set the flowing bottomhole temperature.
        /// </summary>
        public string FlowingBottomholePressure { get; set; }

        /// <summary>
        /// Gets or set the gas injection rate.
        /// </summary>
        public string GasInjectionRate { get; set; }

        /// <summary>
        /// Gets or set the well head temperature.
        /// </summary>
        public string WellheadTemperature { get; set; }

        /// <summary>
        /// Gets or set the reservoir pressure.
        /// </summary>
        public string ReservoirPressure { get; set; }

        /// <summary>
        /// Gets or set the injection rate for tubing critical velocity.
        /// </summary>
        public string InjectionRateForTubingCriticalVelocity { get; set; }

        /// <summary>
        /// Gets or set the valve depth.
        /// </summary>
        public string ValveDepth { get; set; }

    }
}
