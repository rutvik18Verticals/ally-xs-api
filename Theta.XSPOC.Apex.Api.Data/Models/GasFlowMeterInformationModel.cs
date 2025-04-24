namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the gas flow meter information model.
    /// </summary>
    public class GasFlowMeterInformationModel
    {

        /// <summary>
        /// Gets or sets the pulse count.
        /// </summary>
        public string PulseCount { get; set; }

        /// <summary>
        /// Gets or sets the flowing temperature.
        /// </summary>
        public string FlowingTemperature { get; set; }

        /// <summary>
        /// Gets or sets the accumulated volume.
        /// </summary>
        public string AccumulatedVolume { get; set; }

        /// <summary>
        /// Gets or sets the todays volume.
        /// </summary>
        public string TodaysVolume { get; set; }

        /// <summary>
        /// Gets or sets the yesterdays volume.
        /// </summary>
        public string YesterdaysVolume { get; set; }

        /// <summary>
        /// Gets or sets the differential pressure.
        /// </summary>
        public string DifferentialPressure { get; set; }

        /// <summary>
        /// Gets or sets the charger.
        /// </summary>
        public string Charger { get; set; }

        /// <summary>
        /// Gets or sets the battery voltage.
        /// </summary>
        public string BatteryVoltage { get; set; }

        /// <summary>
        /// Gets or sets the meter id.
        /// </summary>
        public string MeterId { get; set; }

        /// <summary>
        /// Gets or sets the todays mass.
        /// </summary>
        public string TodaysMass { get; set; }

        /// <summary>
        /// Gets or sets the yesterdays mass.
        /// </summary>
        public string YesterdaysMass { get; set; }

        /// <summary>
        /// Gets or sets the accumulated mass.
        /// </summary>
        public string AccumulatedMass { get; set; }

        /// <summary>
        /// Gets or sets the energy rate.
        /// </summary>
        public string EnergyRate { get; set; }

        /// <summary>
        /// Gets or sets the todays energy.
        /// </summary>
        public string TodaysEnergy { get; set; }

        /// <summary>
        /// Gets or sets the yesterdays energy.
        /// </summary>
        public string YesterdaysEnergy { get; set; }

        /// <summary>
        /// Gets or sets the Last calculated period volume.
        /// </summary>
        public string LastCalculatedPeriodVolume { get; set; }

        /// <summary>
        /// Gets or sets the line pressure.
        /// </summary>
        public string LinePressure { get; set; }

        /// <summary>
        /// Gets or sets the current mode.
        /// </summary>
        public string CurrentMode { get; set; }

        /// <summary>
        /// Gets or sets the count down hours.
        /// </summary>
        public string CountdownHours { get; set; }

        /// <summary>
        /// Gets or sets the count down seconds.
        /// </summary>
        public string CountdownSeconds { get; set; }

        /// <summary>
        /// Gets or sets the solar voltage.
        /// </summary>
        public string SolarVoltage { get; set; }

    }
}
