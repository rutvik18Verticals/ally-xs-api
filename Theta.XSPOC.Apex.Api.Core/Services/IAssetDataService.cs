using System;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Service which handles processing of asset data.
    /// </summary>
    public interface IAssetDataService
    {

        /// <summary>
        /// Gets the enabled status of a asset by its <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <returns>The well's enabled status data.</returns>
        WellEnabledStatusOutput GetEnabledStatus(WithCorrelationId<Guid> assetId);

    }
}
