namespace Theta.XSPOC.Apex.Api.Core.Models
{
    /// <summary>
    /// Represents the type of values stored in the memory cache.
    /// </summary>
    public enum MemoryCacheValueType
    {

        /// <summary>
        /// Represents the group status controller column formatter.
        /// </summary>
        GroupStatusControllerColumnFormatter = 0,

        /// <summary>
        /// Represents the group status system parameter gauge off hour.
        /// </summary>
        GroupStatusSystemParameterGaugeOffHour = 1,

        /// <summary>
        /// Represents the group status string Id column formatter.
        /// </summary>
        GroupStatusStringIdColumnFormatter = 2,

        /// <summary>
        /// Represents the group status camera alarm column formatter.
        /// </summary>
        GroupStatusCameraAlarmColumnFormatter = 3,

        /// <summary>
        /// Represents the system parameter next gen significant digits.
        /// </summary>
        SystemParameterNextGenSignificantDigits = 4,

    }
}
