using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Common.WorkflowModels;
using Theta.XSPOC.Apex.Api.WellControl.Contracts.V2;

namespace Theta.XSPOC.Apex.Api.WellControl.Data.Services.DbStoreManagers
{
    /// <summary>
    /// A management layer that is responsible for coordinating one or more DbStore implementations to persist a data
    /// update request, received by the integration layer, to storage.
    /// </summary>
    public interface IDbStoreManager
    {

        /// <summary>
        /// Describes the responsibility a Db Store Manager has. This is a direct representation of the PayloadType
        /// that is received as a <seealso cref="DataUpdateEvent"/> by the integration layer.
        /// </summary>
        public string Responsibility { get; }

        /// <summary>
        /// Asynchronously updates the store.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>A <seealso cref="DbStoreResult"/> describing the outcome of the operation.</returns>
        public Task<DbStoreResult> UpdateAsync(string payload, string correlationId);

    }
}
