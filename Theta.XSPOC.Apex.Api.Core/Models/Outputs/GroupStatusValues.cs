using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the group status values.
    /// </summary>
    public class GroupStatusValues
    {

        /// <summary>
        /// Gets or sets the list of columns in the group status values.
        /// </summary>
        public IList<GroupStatusColumn> Columns { get; set; } = new List<GroupStatusColumn>();

        /// <summary>
        /// Gets or sets the list of rows in the group status values.
        /// </summary>
        public IList<GroupStatusRow> Rows { get; set; } = new List<GroupStatusRow>();

    }
}
