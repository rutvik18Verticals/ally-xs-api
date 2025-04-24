using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the core layer's output for group and asset data.
    /// </summary>
    public class GroupAndAssetDataOutput : CoreOutputBase
    {

        /// <summary>
        /// Gets or sets the list of values.
        /// </summary>
        public GroupAndAssetModel Values { get; set; }

    }
}
