namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// THe WellStatistics table.
    /// </summary>
    public class WellStatisticModel
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        public string NodeId { get; set; } = null!;

        /// <summary>
        /// Gets or sets the AverageCycles.
        /// </summary>
        public float? AverageCycles { get; set; }

        /// <summary>
        /// Gets or sets the AlarmStateCycles.
        /// </summary>
        public float? AlarmStateCycles { get; set; }

        /// <summary>
        /// Gets or sets the AverageRuntime.
        /// </summary>
        public float? AverageRuntime { get; set; }

        /// <summary>
        /// Gets or sets the AlarmStateRuntime.
        /// </summary>
        public float? AlarmStateRuntime { get; set; }

        /// <summary>
        /// Gets or sets the AverageFillage.
        /// </summary>
        public float? AverageFillage { get; set; }

        /// <summary>
        /// Gets or sets the AlarmStateFillage.
        /// </summary>
        public float? AlarmStateFillage { get; set; }

    }
}
