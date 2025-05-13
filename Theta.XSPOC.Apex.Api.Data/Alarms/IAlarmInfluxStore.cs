using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data.Alarms
{
    /// <summary>
    /// The interface that defines the methods for the alarm store.
    /// </summary>
    public interface IAlarmInfluxStore
    {
        /// <summary>
        /// Gets the list of alarm configuration for the provided <paramref name="assetId"/>
        /// </summary>
        /// <param name="assetId">The asset id used to get the alarm configuration data.</param>
        /// <param name="customerId">The asset id used to get the alarm configuration data.</param>
        /// <param name="nodeId">The nodeId used to get the alarm configuration data.</param>
        /// <returns>
        /// A <seealso cref="IList{AlarmConfig}"/> that contains the alarm configuration data for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then and empty list is returned.
        /// </returns>
        Task<IList<CurrentRawScanDataInflux>> GetCurrentRawScanDataFromInflux(Guid assetId, Guid customerId, string nodeId);

    }
} 
