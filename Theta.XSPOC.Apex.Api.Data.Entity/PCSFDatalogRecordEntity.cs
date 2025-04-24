using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The PCSFDatalogRecords database table.
    /// </summary>
    [Table("tblPCSFDatalogRecords")]
    public class PCSFDatalogRecordEntity
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
        /// Gets or sets the log date time.
        /// </summary>
        public DateTime LogDateTime { get; set; }

        /// <summary>
        /// Gets or sets the record index.
        /// </summary>
        public int? RecordIndex { get; set; }

        /// <summary>
        /// Gets or sets the value 2.
        /// </summary>
        [MaxLength(20)]
        public float? Value2 { get; set; }

        /// <summary>
        /// Gets or sets the value 3.
        /// </summary>
        [MaxLength(20)]
        public float? Value3 { get; set; }

        /// <summary>
        /// Gets or sets the value 4.
        /// </summary>
        [MaxLength(20)]
        public float? Value4 { get; set; }

        /// <summary>
        /// Gets or sets the value 5.
        /// </summary>
        [MaxLength(20)]
        public float? Value5 { get; set; }

        /// <summary>
        /// Gets or sets the value 6.
        /// </summary>
        [MaxLength(20)]
        public float? Value6 { get; set; }

        /// <summary>
        /// Gets or sets the value 7.
        /// </summary>
        [MaxLength(20)]
        public float? Value7 { get; set; }

        /// <summary>
        /// Gets or sets the value 8.
        /// </summary>
        [MaxLength(20)]
        public float? Value8 { get; set; }

        /// <summary>
        /// Gets or sets the value 9.
        /// </summary>
        [MaxLength(20)]
        public float? Value9 { get; set; }

        /// <summary>
        /// Gets or sets the value 10.
        /// </summary>
        [MaxLength(20)]
        public float? Value10 { get; set; }

        /// <summary>
        /// Gets or sets the value 11.
        /// </summary>
        [MaxLength(20)]
        public float? Value11 { get; set; }

        /// <summary>
        /// Gets or sets the value 12.
        /// </summary>
        [MaxLength(20)]
        public float? Value12 { get; set; }

    }
}
