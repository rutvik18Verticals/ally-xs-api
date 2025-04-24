using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The Data History Archive table.
    /// </summary>
    [Table("tblDataHistoryArchive")]
    public class DataHistoryArchiveEntity
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
        public int Value { get; set; }

    }
}
