using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity.Models
{
    /// <summary>
    /// Represents the vwAggregateCameraAlarmStatus.
    /// </summary>
    [Keyless]
    public partial class VwAggregateCameraAlarmStatus
    {

        /// <summary>
        /// Get and set the NodeId.
        /// </summary>
        [Column("NodeID")]
        [MaxLength(50)]
        public string NodeId { get; set; } = null!;

        /// <summary>
        /// Get and set the AlarmCount.
        /// </summary>
        public int AlarmCount { get; set; }

    }
}
