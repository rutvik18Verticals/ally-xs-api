namespace Theta.XSPOC.Apex.Api.WellControl.Integration.Models
{
    /// <summary>
    /// Represents the different collection modes that are supported when trending data after scan.
    /// </summary>
    public enum CollectionMode
    {

        /// <summary>
        /// Collection Mode is not supported.
        /// </summary>
        Unsupported = 0,

        /// <summary>
        /// Intended to have on record per day, with date stamp (no time) at time of insert.
        /// </summary>
        OnceADay = 1,

        /// <summary>
        /// Intended to have on record per day, with date stamp (no time) at time of insert, but the date stamp is the
        /// previous day, better for daily summarized data after gauge-off.
        /// </summary>
        OnceADayGaugeOff = 2,

        /// <summary>
        /// Insert every scan, with date and time stamp.
        /// </summary>
        TrendOnEveryScan = 3,

        /// <summary>
        /// Trend every scan only if well is not idle or shutdown.
        /// </summary>
        TrendOnlyIfWellNotIdleOrShutdown = 4,

        /// <summary>
        /// Trends whenever GetDailyData is run, with date and time stamp at time that it is run.
        /// </summary>
        TrendWhenDailyDataRuns = 5,

    }
}
