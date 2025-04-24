using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{

    /// <summary>
    /// Represents the output for group status downtime by well.
    /// </summary>
    public class GroupStatusDowntimeByWellOutput : CoreOutputBase
    {

        /// <summary>
        /// Gets or sets the list of asset values.
        /// </summary>
        public IList<GroupStatusKPIValues> Assets { get; set; }

        /// <summary>
        /// Gets or sets the list of assets group by duration values.
        /// </summary>
        public IList<GroupStatusKPIValues> GroupByDuration { get; set; }

    }
}
