using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The analysis curves set table.
    /// </summary>
    [Table("tblAnalysisCurveSet")]
    public class AnalysisCurveSetEntity
    {

        /// <summary>
        /// Gets or sets analysis result source id.
        /// </summary>
        public int AnalysisResultSourceId { get; set; }

        /// <summary>
        /// Gets or sets analysis result id.
        /// </summary>
        public int AnalysisResultId { get; set; }

        /// <summary>
        /// Gets or sets curve set id.
        /// </summary>
        [Key]
        public int CurveSetId { get; set; }

        /// <summary>
        /// Gets or sets curve set type id.
        /// </summary>
        public int CurveSetTypeId { get; set; }

    }
}
