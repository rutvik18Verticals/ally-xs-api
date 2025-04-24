using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// Represents the tblCorrelationTypes.
    /// </summary>
    [Table("tblCorrelationTypes")]
    public class CorrelationTypeEntity
    {

        /// <summary>
        /// Get and set the Id.
        /// </summary>
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Get and set the Name.
        /// </summary>
        [StringLength(60)]
        public string Name { get; set; }

        /// <summary>
        /// Get and set the PhraseId.
        /// </summary>
        [Column("PhraseID")]
        public int PhraseId { get; set; }

    }
}
