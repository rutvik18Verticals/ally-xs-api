using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{

    /// <summary>
    /// The GL Well Orifice database table.
    /// </summary>
    [Table("tblGLWellOrifice")]
    public class GLWellOrificeEntity
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        [Key]
        [Column("NodeID")]
        [MaxLength(50)]
        public string NodeId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the manufacturer id.
        /// </summary>
        [Column("ManufacturerID")]
        public int ManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the measured depth.
        /// </summary>
        public float? MeasuredDepth { get; set; }

        /// <summary>
        /// Gets or sets the vertical depth.
        /// </summary>
        public float VerticalDepth { get; set; }

        /// <summary>
        /// Gets or sets the port size.
        /// </summary>
        public float PortSize { get; set; }

        /// <summary>
        /// Gets or sets the true vertical depth.
        /// </summary>
        public float? TrueVerticalDepth { get; set; }

    }
}
