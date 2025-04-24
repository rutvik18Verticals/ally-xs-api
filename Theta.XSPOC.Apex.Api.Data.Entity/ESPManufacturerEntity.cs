using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The esp manufacturers table.
    /// </summary>
    [Table("tblESPManufacturers")]
    public partial class ESPManufacturerEntity
    {

        /// <summary>
        /// Gets or sets the Manufacturer Id.
        /// </summary>
        [Key]
        [Column("ManufacturerID")]
        public int ManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the Manufacturer.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Manufacturer { get; set; }

    }
}
