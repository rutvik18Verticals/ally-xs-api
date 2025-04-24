using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Models.Inputs
{
    /// <summary>
    /// Represent the class for data history Time Series trends input data.
    /// </summary>
    public class TimeSeriesInput
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
        public List<string> Tags { get; set; }

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

        /// <summary>
        /// Gets or sets the value indicating chart is grouped/overlay.
        /// </summary>
        public bool IsOverlay { get; set; }

        #endregion

    }
}