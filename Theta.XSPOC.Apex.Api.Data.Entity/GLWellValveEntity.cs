using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The GL Well Valves database table.
    /// </summary>
    [Table("tblGLWellValves")]
    public class GLWellValveEntity
    {

        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        [Column("NodeID")]
        [MaxLength(50)]
        public string NodeId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the gl valve id.
        /// </summary>
        [Column("GLValveID")]
        public int GLValveId { get; set; }

        /// <summary>
        /// Gets or sets the vertical depth.
        /// </summary>
        public float VerticalDepth { get; set; }

        /// <summary>
        /// Gets or sets the test rack opening pressure.
        /// </summary>
        public float? TestRackOpeningPressure { get; set; }

        /// <summary>
        /// Gets or sets the closing pressure at depth.
        /// </summary>
        public float? ClosingPressureAtDepth { get; set; }

        /// <summary>
        /// Gets or sets the measured depth.
        /// </summary>
        public float? MeasuredDepth { get; set; }

        /// <summary>
        /// Gets or sets the opening pressure at depth.
        /// </summary>
        public float? OpeningPressureAtDepth { get; set; }

        /// <summary>
        /// Gets or sets the opening pressure at surface.
        /// </summary>
        public float? OpeningPressureAtSurface { get; set; }

        /// <summary>
        /// Gets or sets the closing pressure at surface.
        /// </summary>
        public float? ClosingPressureAtSurface { get; set; }

        /// <summary>
        /// Gets or sets the true vertical depth.
        /// </summary>
        public float? TrueVerticalDepth { get; set; }

    }
}
