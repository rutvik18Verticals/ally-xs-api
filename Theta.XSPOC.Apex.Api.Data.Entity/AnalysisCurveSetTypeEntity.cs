using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// Represtents the tblAnalysisCurveSetTypes.
    /// </summary>
    [Table("tblAnalysisCurveSetTypes")]
    public partial class AnalysisCurveSetTypeEntity
    {

        /// <summary>
        /// Gets and sets the ID.
        /// </summary>
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets and sets the Name.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets the PhraseId.
        /// </summary>
        [Column("PhraseID")]
        public int PhraseId { get; set; }

    }
}
