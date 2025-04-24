using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Alarms
{
    /// <summary>
    /// Represents the properties of a HostAlarm. Used as a sub-document type in Alarm Configuration.
    /// </summary>
    public class HostAlarm : AlarmDocumentBase
    {
        /// <summary>
        /// Gets or sets the Alarm Type
        /// </summary>
        public int? HostAlarmTypeId { get; set; }

        /// <summary>
        /// Gets or sets the asset identifier.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string AssetId { get; set; }

        /// <summary>
        /// Gets or sets the state of the alarm.
        /// </summary>
        public int? AlarmState { get; set; }

        /// <summary>
        /// Gets or sets the average value over 30 days.
        /// </summary>
        public double? Average30D { get; set; }

        /// <summary>
        /// Gets or sets the current cycles value.
        /// </summary>
        public int? CyclesValue { get; set; }

        /// <summary>
        /// Gets or sets the limit of cycles.
        /// </summary>
        public int? CyclesLimit { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the notification was issued.
        /// </summary>
        public DateTime? DateTimeNotificationIssued { get; set; }

        /// <summary>
        /// Gets or sets the discrete normal state.
        /// </summary>
        public int? DiscreteNormalState { get; set; }

        /// <summary>
        /// Gets or sets the exact value of the metric.
        /// </summary>
        public double? ExactValue { get; set; }

        /// <summary>
        /// Gets or sets the high-high limit.
        /// </summary>
        public double? HiHiLimit { get; set; }

        /// <summary>
        /// Gets or sets whether to ignore zero addresses.
        /// </summary>
        public int? IgnoreZeroAddress { get; set; }

        /// <summary>
        /// Gets or sets the value to ignore.
        /// </summary>
        public double? IgnoreValue { get; set; }

        /// <summary>
        /// Gets or sets the low-low limit.
        /// </summary>
        public double? LoLoLimit { get; set; }

        /// <summary>
        /// Gets or sets the date of the last state change.
        /// </summary>
        public DateTime? LastStateChange { get; set; }

        /// <summary>
        /// Gets or sets the minimum to maximum limit.
        /// </summary>
        public int? MinToMaxLimit { get; set; }

        /// <summary>
        /// Gets or sets the number of days.
        /// </summary>
        public double? NumDays { get; set; }

        /// <summary>
        /// Gets or sets the percentage change.
        /// </summary>
        public int? PercentChange { get; set; }

        /// <summary>
        /// Gets or sets the minutes for persistent notification.
        /// </summary>
        public int? PersistentNotificationMinutes { get; set; }

        /// <summary>
        /// Gets or sets the standard deviation.
        /// </summary>
        public double? StandardDev { get; set; }

        /// <summary>
        /// Gets or sets the change in value.
        /// </summary>
        public double? ValueChange { get; set; }

        /// <summary>
        /// Gets or sets the XDiagOutputsId.
        /// </summary>
        public int? XDiagOutputsId { get; set; }
    }

}
