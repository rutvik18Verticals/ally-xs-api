using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity.Models
{
    /// <summary>
    /// Represents the vw30DayRuntimeAverage.
    /// </summary>
    [Keyless]
    public partial class Vw30DayRuntimeAverage
    {

        /// <summary>
        /// Get and set the NodeId.
        /// </summary>
        [MaxLength(50)]
        public string NodeId { get; set; } = null!;

        /// <summary>
        /// Get and set the RtPct30Day.
        /// </summary>
        [Column("RT_Pct30Day")]
        public double? RtPct30Day { get; set; }

    }
}
