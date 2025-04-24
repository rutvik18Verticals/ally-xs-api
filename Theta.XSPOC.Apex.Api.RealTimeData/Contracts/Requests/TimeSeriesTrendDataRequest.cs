namespace Theta.XSPOC.Apex.Api.RealTimeData.Contracts.Requests
{
    /// <summary>
    ///
    /// </summary>

    public class TimeSeriesRequest
    {
        #region Properties

        /// <summary>
        /// Gets or sets the customer id.
        /// </summary>
        /// <example>1111d1d1-1ad1-1111-1f1a-11b1111e1111</example>        
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the asset id.
        /// </summary>
        /// <example>["3333d3d3-3ad1-3333-3f3a-33b3333e3333", "4444d4d4-4ad4-4444-446a-44b4444e4444"]</example>   
        public string[] AssetIds { get; set; }

        /// <summary>
        /// Gets or sets the Tags.
        /// </summary>
        /// <example>[271, 267, 268]</example>
        public int[] Tags { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <example>2024-10-30T00:00:00Z</example>
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <example>2024-10-31T00:00:00Z</example>
        public string EndDate { get; set; }

        /// <summary>
        /// Gets or sets the Sampling.
        /// </summary>
        /// <example>first</example>
        public string DownSampleType { get; set; }

        /// <summary>
        /// Gets or sets the Sampling.
        /// </summary>
        /// <example>5m</example>
        public string DownSampleWindowSize { get; set; }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        /// <example>1</example>
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public int PageSize { get; set; }

        #endregion
    }
}