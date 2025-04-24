using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses.Values
{
    /// <summary>
    /// Represents the response values for group status downtime by well.
    /// </summary>
    public class GroupStatusDowntimeByWellValue
    {

        /// <summary>
        /// Gets or sets the list of assets.
        /// </summary>
        public IList<GroupStatusKPIValue> Assets { get; set; }

        /// <summary>
        /// Gets or sets the list of assets grouped by duration.
        /// </summary>
        public IList<GroupStatusKPIValue> GroupByDuration { get; set; }

    }
}
