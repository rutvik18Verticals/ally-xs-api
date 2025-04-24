using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Contracts.Responses.Values;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// Describes a trend data response that needs to be send out.
    /// </summary>
    public class DataHistoryDefaultTrendsResponse : ResponseBase
    {

        /// <summary>
        /// Gets or sets the <seealso cref="List{DataHistoryTrends}"/> values.
        /// </summary>
        public List<DataHistoryDefaultTrends> Values { get; set; }

    }
}