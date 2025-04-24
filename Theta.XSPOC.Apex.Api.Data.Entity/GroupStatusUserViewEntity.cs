using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The group status user view .
    /// </summary>
    [Table("tblGroupStatusUserViews")]
    public class GroupStatusUserViewEntity
    {

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        [Column("UserID", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the view id.
        /// </summary>
        [Column("ViewID")]
        public int ViewId { get; set; }

        /// <summary>
        /// Gets or sets the group name.
        /// </summary>
        [Column("GroupName", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string GroupName { get; set; }

    }
}
