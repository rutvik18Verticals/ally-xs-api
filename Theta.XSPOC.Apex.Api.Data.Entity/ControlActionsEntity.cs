using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The control actions database table.
    /// </summary>
    [Table("tblControlActions")]
    public class ControlActionsEntity
    {

        /// <summary>
        /// Gets or sets the control action id.
        /// </summary>
        [Key]
        [Column("ControlActionID")]
        public int ControlActionId { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [Column("Description")]
        [MaxLength(50)]
        public string Description { get; set; }

    }
}
