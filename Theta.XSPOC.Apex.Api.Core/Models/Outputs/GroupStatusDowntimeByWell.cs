using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the group status downtime by well values.
    /// </summary>
    public class GroupStatusDowntimeByWell
    {

        /// <summary>
        /// Gets or sets the list of assets.
        /// </summary>
        public IList<GroupStatusKPIValues> Assets { get; set; }

        /// <summary>
        /// Gets or sets the list of assets grouped by duration.
        /// </summary>
        public IList<GroupStatusKPIValues> GroupByDuration { get; set; }

    }
}
