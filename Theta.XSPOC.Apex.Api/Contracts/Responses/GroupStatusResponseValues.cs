using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// Represents the response values for group status.
    /// </summary>
    public class GroupStatusResponseValues
    {

        /// <summary>
        /// Gets or sets the columns of the group status.
        /// </summary>
        public IList<GroupStatusColumn> Columns { get; set; } = new List<GroupStatusColumn>();

        /// <summary>
        /// Gets or sets the rows of the group status.
        /// </summary>
        public IList<IList<GroupStatusRowColumn>> Rows { get; set; } = new List<IList<GroupStatusRowColumn>>();

    }
}
