using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents a row in the group status.
    /// </summary>
    public class GroupStatusRow
    {
        /// <summary>
        /// Gets or sets the columns in the group status row.
        /// </summary>
        public IList<GroupStatusRowColumn> Columns { get; set; } = new List<GroupStatusRowColumn>();
    }
}
