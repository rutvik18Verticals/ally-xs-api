using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Alarms
{
    /// <summary>
    /// A AlarmConfiguration class with properties
    /// </summary>
    public class AlarmConfiguration : DocumentBase
    {
        /// <summary>
        /// Gets or sets the legacy Id of the Alarms.
        /// </summary>
        public IDictionary<string, string> LegacyId { get; set; }

        /// <summary>
        /// Gets or sets the poc type.
        /// </summary>
        public Lookup.Lookup POCType { get; set; }

        /// <summary>
        /// Gets or sets the bit position in the register.
        /// </summary>
        public int? Bit { get; set; }

        /// <summary>
        /// Gets or sets the register identifier.
        /// </summary>
        public int? Register { get; set; }

        /// <summary>
        /// Gets or sets whether the alarm is enabled.
        /// </summary>
        public bool? Enabled { get; set; }

        /// <summary>
        /// Gets or sets the low limit for triggering the alarm.
        /// </summary>
        public double? LoLimit { get; set; }

        /// <summary>
        /// Gets or sets the high limit for triggering the alarm.
        /// </summary>
        public double? HiLimit { get; set; }

        /// <summary>
        /// Gets or sets the action to take when the alarm is triggered.
        /// </summary>
        public int? AlarmAction { get; set; }

        /// <summary>
        /// Gets or sets the priority of the alarm.
        /// </summary>
        public int? Priority { get; set; }

        /// <summary>
        /// Gets or sets the description of the alarm.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of the alarm.
        /// </summary>
        public string AlarmType { get; set; }

        /// <summary>
        /// Gets or sets the lookup document that is based on lookup type.
        /// </summary>
        public AlarmDocumentBase Document { get; set; }
    }

}
