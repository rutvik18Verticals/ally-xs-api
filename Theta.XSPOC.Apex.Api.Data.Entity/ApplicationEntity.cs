using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// Represents the Applications.
    /// </summary>
    [Table("tblApplications")]
    public class ApplicationEntity
    {

        /// <summary>
        /// Gets and sets the Id.
        /// </summary>
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets and sets the Name.
        /// </summary>
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets the PhraseId.
        /// </summary>
        [Column("PhraseID")]
        public int? PhraseId { get; set; }

    }
}
