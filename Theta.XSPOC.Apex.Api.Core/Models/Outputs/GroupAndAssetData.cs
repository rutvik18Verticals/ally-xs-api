using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the class for group and asset data.
    /// </summary>
    public class GroupAndAssetData
    {

        /// <summary>
        /// Gets or sets the asset group name.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the list of children groups.
        /// </summary>
        public IList<GroupAndAssetData> ChildGroups { get; set; }

        /// <summary>
        /// Gets or sets the list of assets.
        /// </summary>
        public IList<AssetData> Assets { get; set; }

    }
}
