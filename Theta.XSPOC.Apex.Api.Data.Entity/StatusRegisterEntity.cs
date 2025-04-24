using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The status registers table.
    /// </summary>
    [Table("tblStatusRegisters")]
    public class StatusRegisterEntity
    {

        /// <summary>
        /// Gets or sets the poc type column.
        /// </summary>
        [Column("POCType")]
        public int PocType { get; set; }

        /// <summary>
        /// Gets or sets the address column.
        /// </summary>
        [Column("Address")]
        public int RegisterAddress { get; set; }

        /// <summary>
        /// Gets or sets the bit column.
        /// </summary>
        [Column("Bit")]
        public int Bit { get; set; }

        /// <summary>
        /// Gets or sets the order column.
        /// </summary>
        [Column("Order")]
        public int? Order { get; set; }

        /// <summary>
        /// Gets or sets the format column.
        /// </summary>
        [Column("Format", TypeName = "varchar")]
        [MaxLength(50)]
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the units column.
        /// </summary>
        [Column("Units", TypeName = "varchar")]
        [MaxLength(20)]
        public string Units { get; set; }

        /// <summary>
        /// Gets or sets the locked column.
        /// </summary>
        [Column("Locked")]
        public bool? Locked { get; set; }

    }
}