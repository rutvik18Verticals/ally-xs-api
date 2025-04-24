namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents the vw30DayRuntimeAverage.
    /// </summary>
    public class Vw30DayRuntimeAverageModel
    {

        /// <summary>
        /// Get and set the NodeId.
        /// </summary>
        public string NodeId { get; set; } = null!;

        /// <summary>
        /// Get and set the RtPct30Day.
        /// </summary>
        public double? RtPct30Day { get; set; }

    }
}
