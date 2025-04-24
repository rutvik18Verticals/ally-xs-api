namespace Theta.XSPOC.Apex.Api.Contracts.Responses.Values
{
    /// <summary>
    /// Represents a response data history alarm limits model.
    /// </summary>
    public class DataHistoryAlarmLimitsValues
    {

        /// <summary>
        /// The address.
        /// </summary>
        public int Address { get; set; }

        /// <summary>
        /// The lolimit.
        /// </summary>
        public float? LoLimit { get; set; }

        /// <summary>
        /// The lololimit.
        /// </summary>
        public float? LoLoLimit { get; set; }

        /// <summary>
        /// The hilimit.
        /// </summary>
        public float? HiLimit { get; set; }

        /// <summary>
        /// The hihilimit.
        /// </summary>
        public float? HiHiLimit { get; set; }

    }
}
