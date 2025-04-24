namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the chemical injection data model.
    /// </summary>
    public class ChemicalInjectionInformationModel
    {

        /// <summary>
        /// Gets or set the accumulated cycle volume.
        /// </summary>
        public string AccumulatedCycleVolume { get; set; }

        /// <summary>
        /// Gets or set the current battery volts.
        /// </summary>
        public string CurrentBatteryVolts { get; set; }

        /// <summary>
        /// Gets or set the current cycle.
        /// </summary>
        public string CurrentCycle { get; set; }

        /// <summary>
        /// Gets or set the current run mode.
        /// </summary>
        public string CurrentRunMode { get; set; }

        /// <summary>
        /// Gets or set the current tank level.
        /// </summary>
        public string CurrentTankLevel { get; set; }

        /// <summary>
        /// Gets or set the cycle frequency.
        /// </summary>
        public string CycleFrequency { get; set; }

        /// <summary>
        /// Gets or set the cycle time.
        /// </summary>
        public string CycleTime { get; set; }

        /// <summary>
        /// Gets or set the daily volume.
        /// </summary>
        public string DailyVolume { get; set; }

        /// <summary>
        /// Gets or set the injection rate.
        /// </summary>
        public string InjectionRate { get; set; }

        /// <summary>
        /// Gets or set the is pump on.
        /// </summary>
        public bool IsPumpOn { get; set; }

        /// <summary>
        /// Gets or set the pump status.
        /// </summary>
        public string PumpStatus { get; set; }

        /// <summary>
        /// Gets or set the target cycle volume.
        /// </summary>
        public string TargetCycleVolume { get; set; }

        /// <summary>
        /// Gets or set the todays volume.
        /// </summary>
        public string TodaysVolume { get; set; }

    }
}
