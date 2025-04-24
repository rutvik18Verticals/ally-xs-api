namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents the class for host alarm for data history.
    /// </summary>
    public class HostAlarmForDataHistoryModel
    {

        /// <summary>
        /// Gets or sets Address.
        /// </summary>
        public int Address { get; set; }

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

    }
}
