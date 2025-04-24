using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The PCSFDatalogConfigurations database table.
    /// </summary>
    [Table("tblPCSFDatalogConfigurations")]
    public class PCSFDatalogConfigurationEntity
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        [Column("NodeID")]
        [MaxLength(50)]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the datalog number.
        /// </summary>
        public int DatalogNumber { get; set; }

        /// <summary>
        /// Gets or sets the value determining scheduled scan is enabled or not.
        /// </summary>
        public bool? ScheduledScanEnabled { get; set; }

        /// <summary>
        /// Gets or sets the value determining on demand scan enabled is enabled or not.
        /// </summary>
        public bool? OnDemandScanEnabled { get; set; }

        /// <summary>
        /// Gets or sets the last saved index.
        /// </summary>
        public int? LastSavedIndex { get; set; }

        /// <summary>
        /// Gets or sets the last saved date time.
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime? LastSavedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the datalog name.
        /// </summary>
        [MaxLength(20)]
        public string DatalogName { get; set; }

        /// <summary>
        /// Gets or sets the name 1.
        /// </summary>
        [MaxLength(20)]
        public string Name1 { get; set; }

        /// <summary>
        /// Gets or sets the name 2.
        /// </summary>
        [MaxLength(20)]
        public string Name2 { get; set; }

        /// <summary>
        /// Gets or sets the name 3.
        /// </summary>
        [MaxLength(20)]
        public string Name3 { get; set; }

        /// <summary>
        /// Gets or sets the name 4.
        /// </summary>
        [MaxLength(20)]
        public string Name4 { get; set; }

        /// <summary>
        /// Gets or sets the name 5.
        /// </summary>
        [MaxLength(20)]
        public string Name5 { get; set; }

        /// <summary>
        /// Gets or sets the name 6.
        /// </summary>
        [MaxLength(20)]
        public string Name6 { get; set; }

        /// <summary>
        /// Gets or sets the name 7.
        /// </summary>
        [MaxLength(20)]
        public string Name7 { get; set; }

        /// <summary>
        /// Gets or sets the name 8.
        /// </summary>
        [MaxLength(20)]
        public string Name8 { get; set; }

        /// <summary>
        /// Gets or sets the name 9.
        /// </summary>
        [MaxLength(20)]
        public string Name9 { get; set; }

        /// <summary>
        /// Gets or sets the name 10.
        /// </summary>
        [MaxLength(20)]
        public string Name10 { get; set; }

        /// <summary>
        /// Gets or sets the name 11.
        /// </summary>
        [MaxLength(20)]
        public string Name11 { get; set; }

        /// <summary>
        /// Gets or sets the name 12.
        /// </summary>
        [MaxLength(20)]
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
        [Column("Field2IsWellID")]
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
