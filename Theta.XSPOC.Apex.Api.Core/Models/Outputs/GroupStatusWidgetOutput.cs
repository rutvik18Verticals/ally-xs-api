using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the output of a group status classification and alarm widget response object.
    /// </summary>
    public class GroupStatusWidgetOutput : CoreOutputBase
    {

        /// <summary>
        /// Gets or sets the group status values.
        /// </summary>
        public List<GroupStatusClassification> ClassificationValues { get; set; }

        /// <summary>
        /// Gets or sets the count of unique assets.
        /// </summary>
        public int AssetCount { get; set; }

    }
}
