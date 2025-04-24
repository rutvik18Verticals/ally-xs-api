using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The SavedParameters table.
    /// </summary>
    [Table("tblSavedParameters")]
    public class SavedParameterEntity
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        [Column("NodeID")]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        [Column("Address")]
        public int Address { get; set; }

        /// <summary>
        /// Gets or sets the backup value.
        /// </summary>
        [Column("Value")]
        public float? Value { get; set; }

        /// <summary>
        /// Gets or sets the backup date.
        /// </summary>
        [Column("BackupDate")]
        public DateTime? BackupDate { get; set; }

    }
}
