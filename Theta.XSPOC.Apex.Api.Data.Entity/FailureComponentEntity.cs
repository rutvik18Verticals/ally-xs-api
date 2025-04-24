using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The FailureComponent database table.
    /// </summary>
    [Table("tblFailureComponents")]
    public class FailureComponentEntity
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
        /// Gets or sets the application id.
        /// </summary>
        [Column("ApplicationID")]
        public int? ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the phrase id.
        /// </summary>
        [Column("PhraseID")]
        public int? PhraseId { get; set; }

    }
}
