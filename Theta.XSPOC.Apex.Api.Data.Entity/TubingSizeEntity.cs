using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The TubingSizes table.
    /// </summary>
    [Table("tblTubingSizes")]
    public class TubingSizeEntity
    {

        /// <summary>
        /// Gets or sets the tubing size id.
        /// </summary>
        [Key]
        [Column("TubingSize")]
        public int TubingSize { get; set; }

        /// <summary>
        /// Gets or sets the tubing name.
        /// </summary>
        [Required]
        [Column("TubingName", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string TubingName { get; set; }

        /// <summary>
        /// Gets or sets the tubing outer diameter.
        /// </summary>
        [Required]
        [Column("TubingOD")]
        public float TubingOd { get; set; }

        /// <summary>
        /// Gets or sets the tubing inner diameter.
        /// </summary>
        [Required]
        [Column("TubingID")]
        public float TubingId { get; set; }

        /// <summary>
        /// Gets or sets the tubing weight.
        /// </summary>
        [Required]
        [Column("TubingWeight")]
        public float TubingWeight { get; set; }

    }
}
