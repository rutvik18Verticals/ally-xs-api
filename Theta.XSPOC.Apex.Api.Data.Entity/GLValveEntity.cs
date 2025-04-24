using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The GL Valves database table.
    /// </summary>
    [Table("tblGLValves")]
    public class GLValveEntity
    {

        /// <summary>
        /// Gets or sets the GL valves id.
        /// </summary>
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the diameter.
        /// </summary>
        public float Diameter { get; set; }

        /// <summary>
        /// Gets or sets the bellow area.
        /// </summary>
        public float BellowsArea { get; set; }

        /// <summary>
        /// Gets or sets the port size.
        /// </summary>
        public float PortSize { get; set; }

        /// <summary>
        /// Gets or sets the port area.
        /// </summary>
        public float PortArea { get; set; }

        /// <summary>
        /// Gets or sets the port-to-bellows-area ratio.
        /// </summary>
        public float PortToBellowsAreaRatio { get; set; }

        /// <summary>
        /// Gets or sets the production pressure effect factor.
        /// </summary>
        public float ProductionPressureEffectFactor { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [MaxLength(50)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer id.
        /// </summary>
        [Column("ManufacturerID")]
        public int? ManufacturerId { get; set; }

        /// <summary>
        /// Gets or sets the 1 - Ap / Ab [ aka 1 - PortToBellowsAreaRatio ] value.
        /// </summary>
        public float? OneMinusR { get; set; }

    }
}
