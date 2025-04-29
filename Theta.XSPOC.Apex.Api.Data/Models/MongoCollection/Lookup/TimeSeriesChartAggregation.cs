namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup
{
    /// <summary>
    /// This class defines the aggregation values for timeseries charts MongoDB sub document for the <seealso cref="Lookup.LookupDocument"/>.
    /// </summary>
    public class TimeSeriesChartAggregation : LookupBase
    {

        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the interval in minutes.
        /// </summary>
        public int Minutes { get; set; }

        /// <summary>
        /// Gets or sets the Aggregate value.
        /// </summary>
        public string Aggregate { get; set; }

    }
}