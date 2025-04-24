using Theta.XSPOC.Apex.Api.Core.Models.Outputs;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Interface for get group and asset.
    /// </summary>
    public interface IGroupAndAssetService
    {

        /// <summary>
        /// Processes the provided group and asset request and generates group and asset based on that data.
        /// </summary>
        /// <param name="userName">The user name to get the group and asset data.</param>
        /// <param name="correlationId">The correlation GUID.</param>
        /// <param name="isNewArchitecture">Boolean specifying fetch new architecture wells.</param>
        /// <returns>The list of <seealso cref="GroupAndAssetData"/></returns>
        GroupAndAssetDataOutput GetGroupAndAssetData(string userName, string correlationId, bool isNewArchitecture);

    }
}
