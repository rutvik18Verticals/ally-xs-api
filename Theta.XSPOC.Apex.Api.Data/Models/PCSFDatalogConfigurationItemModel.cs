using System;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the pcsf datalog configuration model.
    /// </summary>
    public class PCSFDatalogConfigurationItemModel
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the datalog number.
        /// </summary>
        public int DatalogNumber { get; set; }

        /// <summary>
        /// Gets or sets the scheduled scan enabled or not.
        /// </summary>
        public bool? ScheduledScanEnabled { get; set; }

        /// <summary>
        /// Gets or sets the on demand scan enabled or not.
        /// </summary>
        public bool? OnDemandScanEnabled { get; set; }

        /// <summary>
        /// Gets or sets the last saved index.
        /// </summary>
        public int? LastSavedIndex { get; set; }

        /// <summary>
        /// Gets or sets the last saved date time.
        /// </summary>
        public DateTime? LastSavedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the datalog name.
        /// </summary>
        public string DatalogName { get; set; }

        /// <summary>
        /// Gets or sets the name 1.
        /// </summary>
        public string Name1 { get; set; }

        /// <summary>
        /// Gets or sets the name 2.
        /// </summary>
        public string Name2 { get; set; }

        /// <summary>
        /// Gets or sets the name 3.
        /// </summary>
        public string Name3 { get; set; }

        /// <summary>
        /// Gets or sets the name 4.
        /// </summary>
        public string Name4 { get; set; }

        /// <summary>
        /// Gets or sets the name 5.
        /// </summary>
        public string Name5 { get; set; }

        /// <summary>
        /// Gets or sets the name 6.
        /// </summary>
        public string Name6 { get; set; }

        /// <summary>
        /// Gets or sets the name 7.
        /// </summary>
        public string Name7 { get; set; }

        /// <summary>
        /// Gets or sets the name 8.
        /// </summary>
        public string Name8 { get; set; }

        /// <summary>
        /// Gets or sets the name 9.
        /// </summary>
        public string Name9 { get; set; }

        /// <summary>
        /// Gets or sets the name 10.
        /// </summary>
        public string Name10 { get; set; }

        /// <summary>
        /// Gets or sets the name 11.
        /// </summary>
        public string Name11 { get; set; }

        /// <summary>
        /// Gets or sets the name 12.
        /// </summary>
        public string Name12 { get; set; }

        /// <summary>
        /// Gets or sets the current transaction counter.
        /// </summary>
        public int CurrentTransactionCounter { get; set; }

        /// <summary>
        /// Gets or sets the index of newest record.
        /// </summary>
        public int IndexOfNewestRecord { get; set; }

        /// <summary>
        /// Gets or sets the index of oldest record.
        /// </summary>
        public int IndexOfOldestRecord { get; set; }

        /// <summary>
        /// Gets or sets the number of records.
        /// </summary>
        public int NumberOfRecords { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of records.
        /// </summary>
        public int MaximumNumberOfRecords { get; set; }

        /// <summary>
        /// Gets or sets the value indicating field 2 is well id.
        /// </summary>
        public bool Field2IsWellId { get; set; }

        /// <summary>
        /// Gets or sets the number of fields.
        /// </summary>
        public int NumberOfFields { get; set; }

        /// <summary>
        /// Gets or sets the interval minutes.
        /// </summary>
        public int IntervalMinutes { get; set; }

    }
}
