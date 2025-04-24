using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// Represents a group status table entity.
    /// </summary>
    [Table("tblGroupStatusTables")]
    public partial class GroupStatusTableEntity
    {

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets the Id of the table.
        /// </summary>
        [Key]
        [Column("TableID")]
        public int TableId { get; set; }

    }
}
