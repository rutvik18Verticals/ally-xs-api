using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The current raw scan data table.
    /// </summary>
    [Table("tblCurrRawScanData")]
    public class CurrentRawScanDataEntity
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        [Column("NodeID", TypeName = "nvarchar")]
        [MaxLength(50)]
        [Required]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the register address.
        /// </summary>
        [Column("Address")]
        public int Address { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [Column("Value")]
        public float? Value { get; set; }

        /// <summary>
        /// Gets or sets the string value.
        /// </summary>
        [Column("StringValue", TypeName = "nvarchar")]
        public string StringValue { get; set; }

        /// <summary>
        /// Gets or sets the date and time updated.
        /// </summary>
        [Column("DateTimeUpdated")]
        public DateTime? DateTimeUpdated { get; set; }

    }
}
