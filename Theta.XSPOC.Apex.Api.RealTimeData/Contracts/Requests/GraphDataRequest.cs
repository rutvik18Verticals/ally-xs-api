namespace Theta.XSPOC.Apex.Api.RealTimeData.Contracts.Requests
{
    /// <summary>
    /// ESP Graph Data Request
    /// </summary>
    public class GraphDataRequest
    {
        /// <summary>
        /// Gets and Sets Value of AssetId
        /// </summary>

        public string AssetId { get; set; }
        /// <summary>
        /// Gets and Sets Value of StartDate
        /// </summary>

        public string StartDate { get; set; }
        /// <summary>
        /// Gets and Sets Value of EndDate
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// Gets and Sets Value of Aggregate
        /// </summary>
        public string? Aggregate { get; set; }

        /// <summary>
        /// Gets and Sets Value of AggregateMethod
        /// </summary>
        public string? AggregateMethod { get; set; }

        /// <summary>
        /// Gets and Sets Value of ReteriveMixMax
        /// </summary>
        public bool ReteriveMixMax { get; set; }
    }
}
