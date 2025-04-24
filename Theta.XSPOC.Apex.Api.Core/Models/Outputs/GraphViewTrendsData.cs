using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represent the class for data history trends axis data output response.
    /// </summary>
    public class GraphViewTrendsData
    {

        /// <summary>
        /// Gets or sets the Axis Label.
        /// </summary>
        public string AxisLabel { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the chart index.
        /// </summary>
        public int AxisIndex { get; set; }

        /// <summary>
        /// Gets or sets the chart item id.
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// Gets or sets the list of <seealso cref="DataPoint"/> values.
        /// </summary>
        public List<DataPoint> AxisValues { get; set; } = new List<DataPoint>();

        /// <summary>
        /// Gets or sets the address of the trend.
        /// </summary>
        public int? Address { get; set; }

    }
}
