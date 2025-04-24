using System.Collections.Generic;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Alarms
{
    /// <summary>
    /// AlarmEvents mongo document
    /// </summary>
    public class AlarmEvents : DocumentBase
    {
        /// <summary>
        /// Gets or sets the legacy Id of the Alarms.
        /// </summary>
        public IDictionary<string, string> LegacyId { get; set; }

        /// <summary>
        /// Gets or sets AlarmType
        /// </summary>
        public int AlarmType { get; set; }

        /// <summary>
        /// Gets or sets AlarmType
        /// </summary>
        public int AlarmID { get; set; }

        /// <summary>
        /// Gets or sets EventDateTime
        /// </summary>
        public DateTime EventDateTime { get; set; }

        /// <summary>
        /// Gets or sets EventType
        /// </summary>
        public int EventType { get; set; }

        /// <summary>
        /// Gets or sets Value
        /// </summary>
        public double? Value { get; set; }

        /// <summary>
        /// Gets or sets PictureID
        /// </summary>
        public int? PictureId { get; set; }

        /// <summary>
        /// Gets or sets AcknowledgedDateTime
        /// </summary>
        public DateTime? AcknowledgedDateTime { get; set; }

        /// <summary>
        /// Gets or sets AcknowledgedUserName
        /// </summary>
        public string AcknowledgedUserName { get; set; }
    }
}
