using System;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// A service which handles well control operations.
    /// </summary>
    public interface IWellControlService
    {

        /// <summary>
        /// Gets the supported control actions for the well represented 
        /// by the provided <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="correlationId"></param>
        /// <returns>The supported control actions for the well represented 
        /// by the provided <paramref name="assetId"/>.</returns>
        GetWellControlActionsOutput GetWellControlActions(Guid assetId, string correlationId);

    }
}
