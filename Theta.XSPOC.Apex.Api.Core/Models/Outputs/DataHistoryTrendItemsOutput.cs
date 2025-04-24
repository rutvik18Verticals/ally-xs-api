using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Core.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represent the class for data history list output response.
    /// </summary>
    public class DataHistoryTrendItemsOutput : CoreOutputBase
    {

        /// <summary>
        /// Gets or sets the list of <seealso cref="DataPoint"/> values.
        /// </summary>
        public IList<DataPoint> Values { get; set; }

    }
}
