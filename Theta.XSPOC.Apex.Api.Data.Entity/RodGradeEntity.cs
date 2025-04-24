using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// Represents a rod grade entity.
    /// </summary>
    [Table("tblRodGrades")]
    public partial class RodGradeEntity
    {

        /// <summary>
        /// Gets or sets the Id of the rod grade.
        /// </summary>
        [Key]
        [Column("RodGradeID")]
        public int RodGradeId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the rod size group.
        /// </summary>
        [Column("RodSizeGroupID")]
        public int? RodSizeGroupId { get; set; }

        /// <summary>
        /// Gets or sets the name of the rod grade.
        /// </summary>
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the display order of the rod grade.
        /// </summary>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the elasticity of the rod grade.
        /// </summary>
        public float? Elasticity { get; set; }

        /// <summary>
        /// Gets or sets the Id of the rod material.
        /// </summary>
        [Column("RodMatlID")]
        public int? RodMatlId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the stress method.
        /// </summary>
        [Column("StressMethodID")]
        public int? StressMethodId { get; set; }

        /// <summary>
        /// Gets or sets the tensile strength of the rod grade.
        /// </summary>
        public float? TensileStrength { get; set; }

    }
}
