using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents an exception model.
    /// </summary>
    public class ExceptionModel
    {

        /// <summary>
        /// Gets or sets the node Id.
        /// </summary>
        [Column("NodeID")]
        [MaxLength(50)]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the exception group name.
        /// </summary>
        [MaxLength(50)]
        public string ExceptionGroupName { get; set; }

        /// <summary>
        /// Gets or sets the priority of the exception.
        /// </summary>
        public int? Priority { get; set; }

    }
}
