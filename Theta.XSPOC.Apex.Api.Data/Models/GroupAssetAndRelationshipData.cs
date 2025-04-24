using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class representing a collection of groups, assets, and their relationships.
    /// </summary>
    public class GroupAssetAndRelationshipData
    {

        /// <summary>
        /// Gets or sets the groups.
        /// </summary>
        public IList<NodeTreeModel> Groups { get; set; }

        /// <summary>
        /// Gets or sets the assets.
        /// </summary>
        public IList<NodeTreeModel> Assets { get; set; }

        /// <summary>
        /// Gets or sets the node master data.
        /// </summary>
        public IDictionary<string, NodeMasterModel> NodeMasterData { get; set; }

        /// <summary>
        /// Gets or sets the asset memberships.
        /// </summary>
        public IDictionary<string, List<string>> AssetMemberships { get; set; }

    }
}
