using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses.Values
{
    /// <summary>
    /// Represents a response data history alarm limits model.
    /// </summary>
    public class DataHistoryTrends
    {

        /// <summary>
        /// Gets or sets the list of <seealso cref="DataPoint"/> values.
        /// </summary>
        public List<TrendsData> Chart1 { get; set; }

        /// <summary>
        /// Gets or sets the list of <seealso cref="DataPoint"/> values.
        /// </summary>
        public List<TrendsData> Chart2 { get; set; }

        /// <summary>
        /// Gets or sets the list of <seealso cref="DataPoint"/> values.
        /// </summary>
        public List<TrendsData> Chart3 { get; set; }

        /// <summary>
        /// Gets or sets the list of <seealso cref="DataPoint"/> values.
        /// </summary>
        public List<TrendsData> Chart4 { get; set; }

    }
}
