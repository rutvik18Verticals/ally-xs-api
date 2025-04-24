using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity.XDIAG
{
    /// <summary>
    /// The xdiag outputs table.
    /// </summary>
    [Table("tblXDiagOutputs")]
    public class XDIAGOutputEntity
    {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Column("ID")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Column("Name")]
        public string Name { get; set; }

    }
}