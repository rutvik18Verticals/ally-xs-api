using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represent the class for data history trend output response.
    /// </summary>
    public class DataHistoryTrendOutput : CoreOutputBase
    {
        /// <summary>
        /// Gets or sets the list of <seealso cref="GraphViewTrendsData"/> values.
        /// </summary>
        public List<GraphViewTrendsData> Values { get; set; }
    }
}
