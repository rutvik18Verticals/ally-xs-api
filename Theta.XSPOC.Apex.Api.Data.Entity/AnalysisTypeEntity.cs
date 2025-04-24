using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// Represents the AnalysisTypeEntity.
    /// </summary>
    [Table("tblAnalysisTypes")]
    [Index("Name", Name = "UX_tblAnalysisTypes_Name", IsUnique = true)]
    public class AnalysisTypeEntity
    {

        /// <summary>
        /// Gets and set the the Id.
        /// </summary>
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets and set the the Name.
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets and set the the PhraseId.
        /// </summary>
        [Column("PhraseID")]
        public int? PhraseId { get; set; }

    }
}
