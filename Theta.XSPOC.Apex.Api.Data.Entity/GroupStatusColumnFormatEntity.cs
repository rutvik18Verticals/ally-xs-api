using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The System GroupStatusColumnFormat table.
    /// </summary>
    [Table("tblGroupStatusColumnFormats")]
    public class GroupStatusColumnFormatEntity
    {

        /// <summary>
        /// Get and set the FormatId.
        /// </summary>
        [Key]
        [Column("FormatID")]
        public int FormatId { get; set; }

        /// <summary>
        /// Get and set the DataType.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string DataType { get; set; }

        /// <summary>
        /// Get and set the FormatMask.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string FormatMask { get; set; }

        /// <summary>
        /// Get and set the Locked.
        /// </summary>
        public bool? Locked { get; set; }

    }
}
