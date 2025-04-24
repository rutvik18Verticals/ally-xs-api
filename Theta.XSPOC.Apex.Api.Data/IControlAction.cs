using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// Defines operations related to control actions.
    /// </summary>
    public interface IControlAction
    {

        /// <summary>
        /// Gets the supported control actions for the well represented by the provided <paramref name="assetGUID"/>.
        /// </summary>
        /// <param name="assetGUID">The asset guid.</param>
        /// <param name="correlationId"></param>
        /// <returns>The supported control actions for the well represented by the provided <paramref name="assetGUID"/>.</returns>
        IList<ControlAction> GetControlActions(Guid assetGUID, string correlationId);

    }
}
