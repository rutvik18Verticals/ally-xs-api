namespace Theta.XSPOC.Apex.Api.Contracts.Requests
{
    /// <summary>
    /// 
    /// </summary>

    public class TimeSeriesTrendDataRequest
    {
        #region Properties

        /// <summary>
        /// Gets or sets the customer id.
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the asset id.
        /// </summary>
        public string[] AssetIds { get; set; }

        /// <summary>
        /// Gets or sets the Tags.
        /// </summary>
        public int[] Tags { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// Gets or sets the Sampling.
        /// </summary>
        public string DownSampleType { get; set; }

        /// <summary>
        /// Gets or sets the Sampling.
        /// </summary>
        public string DownSampleWindowSize { get; set; }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        public int PageSize { get; set; }

        #endregion
    }
}