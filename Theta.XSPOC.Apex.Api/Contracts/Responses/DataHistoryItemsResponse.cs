using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Contracts.Responses.Values;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// Describes a trend data response that needs to be send out.
    /// </summary>
    public class DataHistoryItemsResponse
    {
        /// <summary>
        /// Gets or sets the TrendName.
        /// </summary>
        public string TrendName { get; set; }

        /// <summary>
        /// Gets or sets the list of values.
        /// </summary>
        public IList<DataPoint> Values { get; set; }

    }

    /// <summary>
    /// Describes a trend data response that needs to be send out.
    /// </summary>
    public class DataHistoryTrendResponse : ResponseBase
    {
        /// <summary>
        /// Gets or sets the List of Trend Data.
        /// </summary>
        public List<DataHistoryItemsResponse> TrendsData { get; set; }

    }
}