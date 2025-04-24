using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The GroupStatusColumns table.
    /// </summary>
    [Table("tblGroupStatusColumns")]
    [Index("ColumnName", "SourceId", Name = "IX_tblGroupStatusColumns", IsUnique = true)]
    public class GroupStatusColumnEntity
    {

        /// <summary>
        /// Get and set the ColumnId.
        /// </summary>
        [Key]
        [Column("ColumnID")]
        public int ColumnId { get; set; }

        /// <summary>
        /// Get and set the ColumnName.
        /// </summary>
        [MaxLength(100)]
        public string ColumnName { get; set; }

        /// <summary>
        /// Get and set the Alias.
        /// </summary>
        [MaxLength(100)]
        public string Alias { get; set; }

        /// <summary>
        /// Get and set the SourceId.
        /// </summary>
        [Column("SourceID")]
        public int SourceId { get; set; }

        /// <summary>
        /// Get and set the Align.
        /// </summary>
        public int? Align { get; set; }

        /// <summary>
        /// Get and set the Visible.
        /// </summary>
        public int? Visible { get; set; }

        /// <summary>
        /// Get and set the Measure.
        /// </summary>
        [MaxLength(50)]
        [Unicode(false)]
        public string Measure { get; set; }

        /// <summary>
        /// Get and set the Formula.
        /// </summary>
        public string Formula { get; set; }

    }
}
