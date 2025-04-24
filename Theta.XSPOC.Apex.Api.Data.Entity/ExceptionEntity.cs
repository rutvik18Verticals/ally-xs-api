using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// Represents an exception entity.
    /// </summary>
    [Table("tblExceptions")]
    public partial class ExceptionEntity
    {

        /// <summary>
        /// Gets or sets the node ID.
        /// </summary>
        [Column("NodeID")]
        [MaxLength(50)]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the exception group name.
        /// </summary>
        [Column("ExceptionGroupName", TypeName = "nvarchar")]
        [MaxLength(50)]
        [Unicode(false)]
        public string ExceptionGroupName { get; set; }

        /// <summary>
        /// Gets or sets the priority of the exception.
        /// </summary>
        [Column("Priority")]
        public int? Priority { get; set; }

    }
}
