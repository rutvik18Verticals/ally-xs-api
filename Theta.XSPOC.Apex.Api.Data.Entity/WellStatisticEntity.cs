using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// THe WellStatistics table.
    /// </summary>
    [Table("tblWellStatistics")]
    public class WellStatisticEntity
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        [Column("NodeID", TypeName = "nvarchar")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MaxLength(100)]
        public string NodeId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the AverageCycles.
        /// </summary>
        [Column("AverageCycles")]
        public float? AverageCycles { get; set; }

        /// <summary>
        /// Gets or sets the AlarmStateCycles.
        /// </summary>
        [Column("AlarmStateCycles")]
        public float? AlarmStateCycles { get; set; }

        /// <summary>
        /// Gets or sets the AverageRuntime.
        /// </summary>
        [Column("AverageRuntime")]
        public float? AverageRuntime { get; set; }

        /// <summary>
        /// Gets or sets the AlarmStateRuntime.
        /// </summary>
        [Column("AlarmStateRuntime")]
        public float? AlarmStateRuntime { get; set; }

        /// <summary>
        /// Gets or sets the AverageFillage.
        /// </summary>
        [Column("AverageFillage")]
        public float? AverageFillage { get; set; }

        /// <summary>
        /// Gets or sets the AlarmStateFillage.
        /// </summary>
        [Column("AlarmStateFillage")]
        public float? AlarmStateFillage { get; set; }

    }
}
