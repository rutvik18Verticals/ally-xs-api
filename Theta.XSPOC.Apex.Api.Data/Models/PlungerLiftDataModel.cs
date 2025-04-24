namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the plunger lift data model.
    /// </summary>
    public class PlungerLiftDataModel
    {

        /// <summary>
        /// Gets or Sets the current mode.
        /// </summary>
        public string CurrentMode { get; set; }

        /// <summary>
        /// Gets or Sets the Casing Pressure.
        /// </summary>
        public string CasingPressure { get; set; }

        /// <summary>
        /// Gets or Sets the Tubing Pressure.
        /// </summary>
        public string TubingPressure { get; set; }

        /// <summary>
        /// Gets or Sets the Line Pressure.
        /// </summary>
        public string LinePressure { get; set; }

        /// <summary>
        /// Gets or Sets the Flow Rate.
        /// </summary>
        public string FlowRate { get; set; }

        /// <summary>
        /// Gets or Sets the Solar Voltage.
        /// </summary>
        public string SolarVoltage { get; set; }

        /// <summary>
        /// Gets or Sets the Battery Voltage.
        /// </summary>
        public string BatteryVoltage { get; set; }

        /// <summary>
        /// Gets or Sets the Countdown Hours.
        /// </summary>
        public string CountdownHours { get; set; }

        /// <summary>
        /// Gets or Sets the Countdown Seconds.
        /// </summary>
        public string CountdownSeconds { get; set; }

        /// <summary>
        /// Gets or Sets the Countdown Hours Min.
        /// </summary>
        public string CountdownHoursMin { get; set; }

        /// <summary>
        /// Gets or Sets the Calculated Critical Flow.
        /// </summary>
        public string CalculatedCriticalFlow { get; set; }

        /// <summary>
        /// Gets or Sets the Surface Casing Pressure.
        /// </summary>
        public string SurfaceCasingPressure { get; set; }

        /// <summary>
        /// Gets or Sets the Gas Temperature.
        /// </summary>
        public string GasTemperature { get; set; }

        /// <summary>
        /// Gets or Sets the Efm Lifetime Accumulation.
        /// </summary>
        public string EfmLifetimeAccumulation { get; set; }

        /// <summary>
        /// Gets or Sets the Gas Today Accumulation.
        /// </summary>
        public string GasTodayAccumulation { get; set; }

        /// <summary>
        /// Gets or Sets the Gas Yesterday Accumulation.
        /// </summary>
        public string GasYesterdayAccumulation { get; set; }

        /// <summary>
        /// Gets or Sets the Oil Today Accumulation.
        /// </summary>
        public string OilTodayAccumulation { get; set; }

        /// <summary>
        /// Gets or Sets the of Oil Yesterday Accumulation.
        /// </summary>
        public string OilYesterdayAccumulation { get; set; }

        /// <summary>
        /// Gets or Sets the of Water Today Accumulation.
        /// </summary>
        public string WaterTodayAccumulation { get; set; }

        /// <summary>
        /// Gets or Sets the Water Yesterday Accumulation.
        /// </summary>
        public string WaterYesterdayAccumulation { get; set; }

        /// <summary>
        /// Gets or Sets the Controller Firmware Version.
        /// </summary>
        public string ControllerFirmwareVersion { get; set; }

        /// <summary>
        /// Gets or Sets the Controller Firmware Revision.
        /// </summary>
        public string ControllerFirmwareRevision { get; set; }

        /// <summary>
        /// Gets or Sets the Current Load Factor.
        /// </summary>
        public string CurrentLoadFactor { get; set; }

    }
}
