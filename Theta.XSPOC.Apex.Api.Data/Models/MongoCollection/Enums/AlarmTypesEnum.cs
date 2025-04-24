namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Enums
{
    /// <summary>
    /// Defines different types of alarms.
    /// </summary>
    public enum AlarmTypesEnum
    {

        /// <summary>
        /// Represents a camera related alarm.
        /// </summary>
        CameraAlarm = 1,

        /// <summary>
        /// Represents an alarm related to facility tags.
        /// </summary>
        FacilityTagAlarm = 2,

        /// <summary>
        /// Represents an alarm from the host system.
        /// </summary>
        HostAlarm = 3,

        /// <summary>
        /// Represents an alarm from an RTU (Remote Terminal Unit).
        /// </summary>
        RTUAlarm = 4

    }
}
