using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Core.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represent the class for data history trend output response.
    /// </summary>
    public class DataHistoryDefaultTrendOutput : CoreOutputBase
    {
        /// <summary>
        /// Gets or sets the list of <seealso cref="GraphViewTrendsData"/> values.
        /// </summary>
        public List<DataHistoryDefaultTrendData> Values { get; set; }

    }
}
