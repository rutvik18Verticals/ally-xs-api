using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The System GroupStandardColumn table.
    /// </summary>
    [Table("tblGroupStandardColumns")]
    public class GroupStandardColumnEntity
    {

        /// <summary>
        /// Get and set the Id.
        /// </summary>
        [Column("ID")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Get and set the Name.
        /// </summary>
        [Column("Name")]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Get and set the Definition.
        /// </summary>
        [Column("Definition")]
        [MaxLength(1998)]
        public string Definition { get; set; }

    }
}
