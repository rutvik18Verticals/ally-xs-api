using Theta.XSPOC.Apex.Api.Contracts.Responses.Values;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// Describes a trend data response that needs to be send out.
    /// </summary>
    public class DataHistoryTrendsResponse : ResponseBase
    {

        /// <summary>
        /// Gets or sets the <seealso cref="DataHistoryTrends"/> values.
        /// </summary>
        public DataHistoryTrends Values { get; set; }

    }
}