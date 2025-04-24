using System;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// The Facility Tags table.
    /// </summary>
    public class FacilityTagsDataOutput
    {

        /// <summary>
        /// Get and set the Address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Get and set the Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Get and set the value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Get and set the Units.
        /// </summary>
        public string Units { get; set; }

        /// <summary>
        /// Get and set the last update date.
        /// </summary>
        public DateTime? LastUpdated { get; set; }

        /// <summary>
        /// Get and set the Alarm.
        /// </summary>
        public PropertyValueContract Alarm { get; set; }

        /// <summary>
        /// Get and set the AlarmTextHi.
        /// </summary>
        public string HostAlarm { get; set; }

    }
}
