using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface for setpoint group related operations.
    /// </summary>
    public interface ISetpointGroup
    {

        /// <summary>
        /// Get setpoint groups and setpoint registers
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{SetpointGroupModel}"/> data.</returns>
        public IList<SetpointGroupModel> GetSetPointGroupData(Guid assetId, string correlationId);

    }
}