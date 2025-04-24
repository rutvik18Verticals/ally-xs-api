using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The pumping unit manufacturer table, which contains information like the pumping unit type.
    /// </summary>
    [Table("tblPumpingUnitManufacturers")]
    public class PumpingUnitManufacturerEntity
    {

        /// <summary>
        /// Gets or sets the primary key for the table.
        /// </summary>
        [Column("ID")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer abbreviation.
        /// </summary>
        [Column("Abbrev", TypeName = "nvarchar")]
        [MaxLength(255)]
        public string ManufacturerAbbreviation { get; set; }

        /// <summary>
        /// Gets or sets the unit type id.
        /// </summary>
        [Column("UnitTypeID")]
        public int UnitTypeId { get; set; }

        /// <summary>
        /// Gets or sets the required rotation.
        /// </summary>
        [Column("RequiredRotation")]
        public int RequiredRotation { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer.
        /// </summary>
        [Column("Manuf", TypeName = "nvarchar")]
        [MaxLength(255)]
        public string Manuf { get; set; }

    }
}
