using System;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;

namespace Theta.XSPOC.Apex.Api.WellControl.Services
{
    /// <summary>
    /// Interface for processing set point requests.
    /// </summary>
    public interface ISetpointGroupProcessingService
    {

        /// <summary>
        /// Get setpoint groups and setpoint registers
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="SetpointsGroupOutput"/> data.</returns>
        public SetpointsGroupOutput GetSetpointGroups(Guid assetId, string correlationId);

    }
}
