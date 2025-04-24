using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity.RodLift
{
    /// <summary>
    /// The rod string sizes table for rod lift.
    /// </summary>
    [Table("tblRodSizes")]
    public class RodStringSizeEntity
    {

        /// <summary>
        /// Gets or sets the rod size id.
        /// </summary>
        [Column("RodSizeID")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? RodSizeId { get; set; }

        /// <summary>
        /// Gets or sets the rod size group id.
        /// </summary>
        [Column("RodSizeGroupID")]
        public int? RodSizeGroupId { get; set; }

        /// <summary>
        /// Gets or sets the grade of the rod string.
        /// </summary>
        [Column("DisplayName", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the diameter of the rod string.
        /// </summary>
        [Column("Diameter")]
        public float? Diameter { get; set; }

        /// <summary>
        /// Gets or sets the rod weight.
        /// </summary>
        [Column("RodWeight")]
        public float? RodWeight { get; set; }

        /// <summary>
        /// Gets or sets the has sinker bar flag.
        /// </summary>
        [Column("SinkerBar")]
        public bool? HasSinkerBar { get; set; }

    }
}