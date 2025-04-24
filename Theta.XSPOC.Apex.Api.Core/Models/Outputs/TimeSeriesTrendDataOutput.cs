using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Core.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represent the class for data history trend output response.
    /// </summary>
    public class TimeSeriesTrendDataOutput : CoreOutputBase
    {
        /// <summary>
        /// Gets or sets the list of <seealso cref="TimeSeriesTrendData"/> values.
        /// </summary>
        public List<AllyTimeSeriesTrendDataPoint> Values { get; set; }
    }

    /// <summary>
    /// Represent the class for data history trend output response.
    /// </summary>
    public class TimeSeriesOutput : CoreOutputBase
    {
        /// <summary>
        /// Gets or sets the list of <seealso cref="TimeSeriesTrendData"/> values.
        /// </summary>
        public List<TimeSeriesDataPoint> Values { get; set; }
    }
}
