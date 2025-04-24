using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{

    /// <summary>
    /// Represents a response containing group status classification data.
    /// </summary>
    public class GroupStatusWidgetResponse : ResponseBase
    {

        /// <summary>
        /// Gets or sets the classification widget values.
        /// </summary>
        public List<GroupStatusClassificationResponseValues> ClassificationValues { get; set; }

        /// <summary>
        /// Gets or sets the count of unique assets.
        /// </summary>
        public int AssetCount { get; set; }

    }
}
