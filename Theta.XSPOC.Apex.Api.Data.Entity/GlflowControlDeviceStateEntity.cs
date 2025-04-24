using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// Represents the tblGLFlowControlDeviceStates.
    /// </summary>
    [Table("tblGLFlowControlDeviceStates")]
    public class GlflowControlDeviceStateEntity
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
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Gets and sets the PhraseId.
        /// </summary>
        [Column("PhraseID")]
        public int PhraseId { get; set; }

    }
}
