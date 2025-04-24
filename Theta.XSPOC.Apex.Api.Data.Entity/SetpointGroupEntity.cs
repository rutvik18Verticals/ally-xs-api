using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The SetpointGroup table.
    /// </summary>
    [Table("tblSetpointGroups")]
    public class SetpointGroupEntity
    {

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [Key]
        [Column("SetpointGroup")]
        public int SetpointGroupId { get; set; }

        /// <summary>
        /// Gets or sets the setpoint group name.
        /// </summary>
        [Column("DisplayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the display order of setpoint group.
        /// </summary>
        [Column("DisplayOrder")]
        public int? DisplayOrder { get; set; }

    }
}
