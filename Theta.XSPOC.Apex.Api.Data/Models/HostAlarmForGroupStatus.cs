using System;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents the status of a host alarm for a group.
    /// </summary>
    public class HostAlarmForGroupStatus
    {

        /// <summary>
        /// Gets or sets Address.
        /// </summary>
        public int? Address { get; set; }

        /// <summary>
        /// Gets or sets LoLimit.
        /// </summary>
        public float? LoLimit { get; set; }

        /// <summary>
        /// Gets or sets LoLoLimit.
        /// </summary>
        public float? LoLoLimit { get; set; }

        /// <summary>
        /// Gets or sets HiLimit.
        /// </summary>
        public float? HiLimit { get; set; }

        /// <summary>
        /// Gets or sets HiHiLimit.
        /// </summary>
        public float? HiHiLimit { get; set; }

        /// <summary>
        /// Gets or sets AlarmType.
        /// </summary>
        public int AlarmType { get; set; }

        /// <summary>
        /// Gets or sets NodeId.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets AlarmState.
        /// </summary>
        public int? AlarmState { get; set; }

        /// <summary>
        /// Gets or sets alarm description.
        /// </summary>
        public string AlarmDescription { get; set; }

        /// <summary>
        /// Gets or sets ValueChange.
        /// </summary>
        public float? ValueChange { get; set; }

        /// <summary>
        /// Gets or sets PercentChange.
        /// </summary>
        public int? PercentChange { get; set; }

        /// <summary>
        /// Gets or sets MinToMaxLimit.
        /// </summary>
        public int? MinToMaxLimit { get; set; }

        /// <summary>
        /// Gets or sets LastStateChange.
        /// </summary>
        public DateTime? LastStateChange { get; set; }

        /// <summary>
        /// Gets or sets PushEnabled.
        /// </summary>
        public bool PushEnabled { get; set; }

        /// <summary>
        /// Gets or sets Priority.
        /// </summary>
        public int? Priority { get; set; }

        /// <summary>
        /// Gets or sets AssetId.
        /// </summary>
        public string AssetId { get; set; }

    }
}
