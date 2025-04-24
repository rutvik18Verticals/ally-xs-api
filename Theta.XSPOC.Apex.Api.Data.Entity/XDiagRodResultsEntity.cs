using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The xdiag rod result table.
    /// </summary>
    [Table("tblXDiagRodResults")]
    public class XDiagRodResultsEntity
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        [Column("NodeID")]
        [MaxLength(50)]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        [Column("Date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the rod number.
        /// </summary>
        [Column("RodNum")]
        public short? RodNum { get; set; }

        /// <summary>
        /// Gets or sets the grade.
        /// </summary>
        [Column("Grade")]
        public string Grade { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        [Column("Length")]
        public int? Length { get; set; }

        /// <summary>
        /// Gets or sets the diameter.
        /// </summary>
        [Column("Diameter")]
        public double? Diameter { get; set; }

        /// <summary>
        /// Gets or sets the loading.
        /// </summary>
        [Column("Loading")]
        public float? Loading { get; set; }

        /// <summary>
        /// Gets or sets the bottom min stress.
        /// </summary>
        [Column("BottomMinStress")]
        public float? BottomMinStress { get; set; }

        /// <summary>
        /// Gets or sets the top min stress.
        /// </summary>
        [Column("TopMinStress")]
        public float? TopMinStress { get; set; }

        /// <summary>
        /// Gets or sets the top max stress.
        /// </summary>
        [Column("TopMaxStress")]
        public float? TopMaxStress { get; set; }

        /// <summary>
        /// Gets or sets the rod guide id.
        /// </summary>
        [Column("RodGuideID")]
        public int? RodGuideID { get; set; }

        /// <summary>
        /// Gets or sets the drag friction coefficient.
        /// </summary>
        [Column("DragFrictionCoefficient")]
        public float? DragFrictionCoefficient { get; set; }

        /// <summary>
        /// Gets or sets the guide count per rod.
        /// </summary>
        [Column("GuideCountPerRod")]
        public int? GuideCountPerRod { get; set; }

    }
}
