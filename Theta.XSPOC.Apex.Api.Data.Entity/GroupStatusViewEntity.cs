using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{

    /// <summary>
    /// The group status view  table.
    /// </summary>
    [Table("tblGroupStatusViews")]
    public class GroupStatusViewEntity
    {

        /// <summary>
        /// Gets or sets the view id.
        /// </summary>
        [Column("ViewID")]
        public int ViewId { get; set; }

        /// <summary>
        /// Gets or sets the view name.
        /// </summary>
        [Column("ViewName", TypeName = "nvarchar")]
        [MaxLength(50)]
        [Unicode(false)]
        public string ViewName { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        [Column("UserID")]
        [MaxLength(50)]
        [Unicode(false)]
        public string UserId { get; set; }

        /// <summary>
        /// Get and set the FilterId.
        /// </summary>
        [Column("FilterID")]
        public int? FilterId { get; set; }

    }
}
