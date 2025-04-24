using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// the table tblPerforations.
    /// </summary>
    [Table("tblPerforations")]
    public partial class PerforationEntity
    {

        /// <summary>
        /// Gets and sets the NodeID.
        /// </summary>
        [Column("NodeID")]
        [MaxLength(50)]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets and sets the Depth.
        /// </summary>
        public float Depth { get; set; }

        /// <summary>
        /// Gets and sets the Interval.
        /// </summary>
        public float Interval { get; set; }

        /// <summary>
        /// Gets and sets the Diameter.
        /// </summary>
        public float? Diameter { get; set; }

        /// <summary>
        /// Gets and sets the HolesPerFt.
        /// </summary>
        public short? HolesPerFt { get; set; }

    }
}