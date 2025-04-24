using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The Data History table.
    /// </summary>
    [Table("tblDataHistory")]
    public class DataHistoryEntity
    {

        /// <summary>
        /// Gets or sets Date. 
        /// </summary>
        [Column("Date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets NodeID.
        /// </summary>
        [Column("NodeID")]
        public string NodeID { get; set; }

        /// <summary>
        /// Gets or sets Address.
        /// </summary>
        [Column("Address")]
        public int Address { get; set; }

        /// <summary>
        /// Gets or sets Values.
        /// </summary>
        [Column("Value")]
        public float Value { get; set; }

    }
}
