using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The group parameters database table.
    /// </summary>
    [Table("tblGroupParameters")]
    public partial class GroupParameterEntity
    {

        /// <summary>
        /// Gets or sets the group parameter id.
        /// </summary>
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the function type.
        /// </summary>
        public short FunctionType { get; set; }

        /// <summary>
        /// Gets or sets the phrase id.
        /// </summary>
        [Column("PhraseID")]
        public int? PhraseId { get; set; }

    }
}
