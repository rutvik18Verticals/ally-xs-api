using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The analysis correlations table.
    /// </summary>
    [Table("tblAnalysisCorrelations")]
    public class AnalysisCorrelationEntity
    {

        /// <summary>
        /// Gets or sets the correlation id. 
        /// </summary>
        [Column("CorrelationID")]
        public int CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the correlation type id.
        /// </summary>
        [Column("CorrelationTypeID")]
        public int CorrelationTypeId { get; set; }

        /// <summary>
        /// Gets or sets the analysis correlations id.
        /// </summary>
        [Key]
        [Column("ID")]
        public int Id { get; set; }

    }
}
