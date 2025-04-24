using System;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Kernel.Integration;

namespace Theta.XSPOC.Apex.Api.WellControl.Services
{
    /// <summary>
    /// This is the interface for Well Enable/Disable.
    /// </summary>
    public interface IWellEnableDisableService
    {

        /// <summary>
        /// Pass asset guid, enabled, dataCollection and disableCode to service.
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="enabled">The Enabled.</param>
        /// <param name="dataCollection">The data Collection.</param>
        /// <param name="disableCode">The disable Code.</param>
        /// <param name="socketId">The socket id.</param>
        /// <returns>Task<seealso cref="ConsumerBaseAction" />></returns>
        public Task<ConsumerBaseAction> WellEnableDisableAsync(Guid assetId, string enabled, string dataCollection, string disableCode, string socketId);

    }
}
