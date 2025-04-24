using Theta.XSPOC.Apex.Api.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the output of a group status operation.
    /// </summary>
    public class GroupStatusOutput : CoreOutputBase
    {

        /// <summary>
        /// Gets or sets the group status values.
        /// </summary>
        public GroupStatusValues Values { get; set; }

    }
}
