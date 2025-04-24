using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The Sensitivity Analysis IPR table.
    /// </summary>
    [Table("tblSensitivityAnalysisIPR")]
    public partial class SensitivityAnalysisIPREntity
    {

        /// <summary>
        /// Gets or sets the sensitivity analysis id.
        /// </summary>
        [Column("SensitivityAnalysisID")]
        public int SensitivityAnalysisId { get; set; }

        /// <summary>
        /// Gets or sets the ipr analysis result id
        /// </summary>
        [Column("IPRAnalysisResultID")]
        public int IPRAnalysisResultId { get; set; }

    }
}
