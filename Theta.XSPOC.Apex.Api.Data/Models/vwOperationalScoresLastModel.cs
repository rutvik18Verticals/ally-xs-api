using System.ComponentModel.DataAnnotations;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents the vwOperationalScoresLast.
    /// </summary>
    public class VwOperationalScoresLastModel
    {

        /// <summary>
        /// Get and set the NodeId.
        /// </summary>
        [MaxLength(50)]
        public string NodeId { get; set; } = null!;

        /// <summary>
        /// Get and set the OperationalScore.
        /// </summary>
        public float? OperationalScore { get; set; }

    }
}
