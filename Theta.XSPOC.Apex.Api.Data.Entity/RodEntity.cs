using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// Represents a rod entity.
    /// </summary>
    [Table("tblRods")]
    public partial class RodEntity
    {

        /// <summary>
        /// Gets or sets the node Id.
        /// </summary>
        [Column("NodeID")]
        [MaxLength(50)]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the rod number.
        /// </summary>
        public short RodNum { get; set; }

        /// <summary>
        /// Gets or sets the grade of the rod.
        /// </summary>
        [MaxLength(50)]
        [Unicode(false)]
        public string Grade { get; set; }

        /// <summary>
        /// Gets or sets the length of the rod.
        /// </summary>
        public int? Length { get; set; }

        /// <summary>
        /// Gets or sets the diameter of the rod.
        /// </summary>
        public double? Diameter { get; set; }

        /// <summary>
        /// Gets or sets the ID of the rod grade.
        /// </summary>
        [Column("RodGradeID")]
        public int? RodGradeId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the rod size.
        /// </summary>
        [Column("RodSizeID")]
        public int? RodSizeId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the rod guide.
        /// </summary>
        [Column("RodGuideID")]
        public short? RodGuideId { get; set; }

        /// <summary>
        /// Gets or sets the drag friction coefficient of the rod.
        /// </summary>
        public float? DragFrictionCoefficient { get; set; }

        /// <summary>
        /// Gets or sets the guide count per rod.
        /// </summary>
        public int? GuideCountPerRod { get; set; }

    }
}
