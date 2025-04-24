using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// Represents the tblGLCurveSetAnnotations.
    /// </summary>
    [Table("tblGLCurveSetAnnotations")]
    public class GLCurveSetAnnotationEntity
    {
        /// <summary>
        /// Gets and sets the CurveSetMemberId.
        /// </summary>
        [Key]
        public int CurveSetMemberId { get; set; }

        /// <summary>
        /// Gets and sets the AssociatedGasLiquidRatio.
        /// </summary>
        public double AssociatedGasLiquidRatio { get; set; }

        /// <summary>
        /// Gets and sets the IsPrimary.
        /// </summary>
        public bool IsPrimary { get; set; }

    }
}
