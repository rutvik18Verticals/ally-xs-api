using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The Graph view settings database table.
    /// </summary>
    [Table("tblGraphViews")]
    public class GraphViewsEntity
    {

        /// <summary>
        /// Gets or sets the view id.
        /// </summary>
        [Key]
        [Column("ViewID")]
        public int ViewId { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        [Column("UserID")]
        public string UserId { get; set; }

        /// r<summary>
        /// Gets or sets the view name.
        /// </summary>
        [Column("ViewName")]
        public string ViewName { get; set; }

    }
}
