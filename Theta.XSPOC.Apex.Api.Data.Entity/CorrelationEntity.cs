using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// Represents the tblCorrelations.
    /// </summary>
    [Table("tblCorrelations")]
    public class CorrelationEntity
    {

        /// <summary>
        /// Gets or sets the curve set member ID.
        /// </summary>
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        [StringLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the PhraseID.
        /// </summary>
        [Column("PhraseID")]
        public int PhraseId { get; set; }

    }
}
