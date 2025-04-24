using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The GL Manufacturers database table.
    /// </summary>
    [Table("tblGLManufacturers")]
    public class GLManufacturerEntity
    {

        /// <summary>
        /// Gets or sets the manufacturer id.
        /// </summary>
        [Key]
        [Column("ManufacturerID")]
        public int ManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer name.
        /// </summary>
        [MaxLength(100)]
        public string Manufacturer { get; set; } = null!;

    }
}
