using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The System Parameter table.
    /// </summary>
    [Table("tblSystemParameters")]
    public class SystemParametersEntity
    {

        /// <summary>
        /// Gets or sets the parameter.
        /// </summary>
        [Column("Parameter")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MaxLength(50)]
        public string Parameter { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [Column("Value", TypeName = "ntext")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the comment associated with the parameter entry.
        /// </summary>
        [Column("Comment")]
        [MaxLength(100)]
        public string Comment { get; set; }

    }
}
