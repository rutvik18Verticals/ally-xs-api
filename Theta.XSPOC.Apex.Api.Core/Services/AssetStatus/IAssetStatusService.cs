using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs.AssetStatus;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services.AssetStatus
{
    /// <summary>
    /// This is the asset status service interface that defines methods that will gather all the required data for
    /// the asset status screen.
    /// </summary>
    public interface IAssetStatusService
    {

        /// <summary>
        /// Gets the rod lift asset status data.
        /// </summary>
        /// <param name="requestWithCorrelationId">
        /// The <seealso cref="AssetStatusInput"/> that provides the input values with the correlation id.
        /// </param>
        /// <returns>The <seealso cref="RodLiftAssetStatusDataOutput"/> with the correlation id.</returns>
        Task<WithCorrelationId<RodLiftAssetStatusDataOutput>> GetAssetStatusDataAsync(
            WithCorrelationId<AssetStatusInput> requestWithCorrelationId);

    }
}
