using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The FailureSubcomponents database table.
    /// </summary>
    [Table("tblFailureSubcomponents")]
    public class FailureSubComponentEntity
    {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the component id.
        /// </summary>
        [Column("ComponentID")]
        public int? ComponentId { get; set; }

        /// <summary>
        /// Gets or sets the phrase id.
        /// </summary>
        [Column("PhraseID")]
        public int? PhraseId { get; set; }

    }
}