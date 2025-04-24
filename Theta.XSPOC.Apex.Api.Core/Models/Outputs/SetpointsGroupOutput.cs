using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Core.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the core layer's output for set point groups data sent out as response.
    /// </summary>
    public class SetpointsGroupOutput : CoreOutputBase
    {

        /// <summary>
        /// Gets or sets the list of <seealso cref="DataHistoryListItem"/> values.
        /// </summary>
        public IList<SetPointGroupsData> Values { get; set; }

    }
}
