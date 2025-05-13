using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public interface ICurrentRawScansStore
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
        /// <summary>
        /// Gets the list of current raw scan data for the provided <paramref name="assetId"/>
        /// </summary>
        /// <param name="assetId">The asset id used to get the current raw scan data.</param>
        /// <param name="customerId">The customer id used to verify customer has access to the data.</param>
        /// <param name="nodeId">The nodeId used to get the current raw scan data.</param>
        /// <returns>
        /// A <seealso cref="IList{CurrentRawScanDataInflux}"/> that contains the current raw scan data for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then and empty list is returned.
        /// </returns>
        Task<IList<CurrentRawScanDataInflux>> GetCurrentRawScanDataFromInflux(Guid assetId, Guid customerId, string nodeId);
    }
}
