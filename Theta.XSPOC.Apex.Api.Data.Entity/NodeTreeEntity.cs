using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// Represents the node tree table.
    /// </summary>
    [Table("tblNodeTree")]
    public class NodeTreeEntity
    {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the node.
        /// </summary>
        [MaxLength(50)]
        [Column("Node")]
        public string Node { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        [Column("Type")]
        public Int16 Type { get; set; }

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        [MaxLength(50)]
        [Column("Parent")]
        public string Parent { get; set; }

        /// <summary>
        /// Gets or sets the number of descendants.
        /// </summary>
        [Column("NumDescendants")]
        public int NumDescendants { get; set; }

    }
}