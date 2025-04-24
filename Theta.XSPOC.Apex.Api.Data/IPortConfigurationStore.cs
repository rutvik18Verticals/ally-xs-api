using System;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// Represents operations that can be performed on the system's port configurations.
    /// </summary>
    public interface IPortConfigurationStore
    {

        /// <summary>
        /// Determines if the well is running comms on the legacy system.
        /// </summary>
        /// <param name="portId">The port id.</param>
        /// <param name="correlationId"></param>
        /// <returns>True if the well is running on the legacy system, false otherwise.</returns>
        public Task<bool> IsLegacyWellAsync(int portId, string correlationId);

        /// <summary>
        /// Gets the NodeMaster data based on asset guid.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="NodeMasterModel"/></returns>
        public NodeMasterModel GetNode(Guid assetId, string correlationId);

    }
}
