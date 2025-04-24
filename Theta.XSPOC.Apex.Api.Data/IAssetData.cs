using System;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// Defines operations related to asset data.
    /// </summary>
    public interface IAssetData
    {

        /// <summary>
        /// Gets the well's enabled status.
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="correlationId"></param>
        /// <returns>The well's enabled status.</returns>
        bool? GetWellEnabledStatus(Guid assetId, string correlationId);

    }
}
