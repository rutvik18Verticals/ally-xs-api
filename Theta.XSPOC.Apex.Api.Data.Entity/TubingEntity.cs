using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The Tubings table.
    /// </summary>
    [Table("tblTubings")]
    public class TubingEntity
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        [Required]
        [Column("NodeID", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the order number.
        /// </summary>
        [Required]
        [Column("OrderNum")]
        public short OrderNum { get; set; }

        /// <summary>
        /// Gets or sets the tubing size id.
        /// </summary>
        [Required]
        [Column("TubingSizeID")]
        public int TubingSizeId { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        [Required]
        [Column("Length")]
        public int Length { get; set; }

    }
}
