namespace Theta.XSPOC.Apex.Api.Data.Entity.Models
{
    /// <summary>
    /// Represents the CommonWorkingSet.
    /// </summary>
    public class CommonWorkingSet
    {

        /// <summary>
        /// Get and set the nodemaster.
        /// </summary>
        public NodeMasterEntity NodeMaster { get; set; }

        /// <summary>
        /// Get and set the WellStatistic.
        /// </summary>
        public WellStatisticEntity WellStatistic { get; set; }

        /// <summary>
        /// Get and set the WellDetail.
        /// </summary>
        public WellDetailsEntity WellDetail { get; set; }

        /// <summary>
        /// Get and set the String.
        /// </summary>
        public StringsEntity String { get; set; }

        /// <summary>
        /// Get and set the vwAggregateCameraAlarmStatus.
        /// </summary>
        public VwAggregateCameraAlarmStatus VwAggregateCameraAlarmStatus { get; set; }

        /// <summary>
        /// Get and set the vwOperationalScoresLast.
        /// </summary>
        public VwOperationalScoresLast VwOperationalScoresLast { get; set; }

        /// <summary>
        /// Get and set the vw30DayRuntimeAverage.
        /// </summary>
        public Vw30DayRuntimeAverage Vw30DayRuntimeAverage { get; set; }

    }
}
