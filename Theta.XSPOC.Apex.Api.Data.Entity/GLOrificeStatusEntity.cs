using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The GL Orifice Status database table.
    /// </summary>
    [Table("tblGLOrificeStatus")]
    public class GLOrificeStatusEntity
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        [Column("NodeID")]
        [MaxLength(50)]
        public string NodeId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the orifice state enum value.
        /// </summary>
        public int OrificeState { get; set; }

        /// <summary>
        /// Gets or sets the value indicating injecting gas is enabled or not.
        /// </summary>
        public bool? IsInjectingGas { get; set; }

        /// <summary>
        /// Gets or sets the gas lift analysis result id. 
        /// </summary>
        [Column("GLAnalysisResultID")]
        public int GLAnalysisResultId { get; set; }

        /// <summary>
        /// Gets or sets the injection rate for tubing critical velocity.
        /// </summary>
        public float? InjectionRateForTubingCriticalVelocity { get; set; }

        /// <summary>
        /// Gets or sets the tubing critical velocity at depth.
        /// </summary>
        public float? TubingCriticalVelocityAtDepth { get; set; }

        /// <summary>
        /// Gets or sets the injection pressure at depth.
        /// </summary>
        public float? InjectionPressureAtDepth { get; set; }

        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        public float? Depth { get; set; }

    }
}
