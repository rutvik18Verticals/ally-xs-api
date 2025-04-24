using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface that represents groups and assets.
    /// </summary>
    public interface IGroupAndAsset
    {

        /// <summary>
        /// Gets all groups, assets, and their relationships.
        /// </summary>
        /// <param name="groupName">The group name default parameter.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>A <see cref="GroupAssetAndRelationshipData"/> object containing groups, assets, and
        /// their relationships.</returns>
        GroupAndAssetModel GetGroupAssetAndRelationshipData(string correlationId, string groupName = "");

    }
}
