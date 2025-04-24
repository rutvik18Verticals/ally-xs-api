using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{

    /// <summary>
    /// The group membership cache table.
    /// </summary>
    [Table("tblGroupMembershipCache")]
    public class GroupMembershipCacheEntity
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        [Column("NodeID", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the group name.
        /// </summary>
        [Column("GroupName", TypeName = "nvarchar")]
        [MaxLength(50)]
        public string GroupName { get; set; }

    }
}
