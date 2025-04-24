namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Defines the alarm data.
    /// </summary>
    public record AlarmData
    {

        /// <summary>
        /// Gets or sets the alarm state.
        /// </summary>
        public int? AlarmState { get; set; }

        /// <summary>
        /// Gets or sets the configuration value.
        /// </summary>
        public float CurrentValue { get; set; }

        /// <summary>
        /// Gets or sets the alarm register.
        /// </summary>
        public string AlarmRegister { get; set; }

        /// <summary>
        /// Gets or sets the alarm bit.
        /// </summary>
        public int AlarmBit { get; set; }

        /// <summary>
        /// Gets or sets the alarm normal state.
        /// </summary>
        public int AlarmNormalState { get; set; }

        /// <summary>
        /// Gets or sets the alarm description.
        /// </summary>
        public string AlarmDescription { get; set; }

        /// <summary>
        /// Gets or sets the alarm priority.
        /// </summary>
        public int AlarmPriority { get; set; }

        /// <summary>
        /// Gets or sets the state text.
        /// </summary>
        public string StateText { get; set; }

        /// <summary>
        /// Gets or sets the alarm type.
        /// </summary>
        public string AlarmType { get; set; }

    }
}