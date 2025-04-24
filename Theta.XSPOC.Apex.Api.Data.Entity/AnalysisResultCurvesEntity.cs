using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The analysis result curves table.
    /// </summary>
    [Table("tblAnalysisResultCurves")]
    public class AnalysisResultCurvesEntity
    {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Column("ID")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets analysis result id.
        /// </summary>
        [Column("AnalysisResultID")]
        public int AnalysisResultId { get; set; }

        /// <summary>
        /// Gets or sets curve type id.
        /// </summary>
        [Column("CurveTypeID")]
        public int CurveTypeId { get; set; }

    }
}
