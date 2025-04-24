namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents the vwAggregateCameraAlarmStatus.
    /// </summary>
    public class VwAggregateCameraAlarmStatusModel
    {

        /// <summary>
        /// Get and set the NodeId.
        /// </summary>
        public string NodeId { get; set; } = null!;

        /// <summary>
        /// Get and set the AlarmCount.
        /// </summary>
        public int AlarmCount { get; set; }

    }
}
