using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data.Alarms
{
    /// <summary>
    /// The interface that defines the methods for the alarm store.
    /// </summary>
    public interface IAlarmStore
    {
        /// <summary>
        /// Gets the list of alarm configuration for the provided <paramref name="assetId"/>
        /// </summary>
        /// <param name="assetId">The asset id used to get the alarm configuration data.</param>
        /// <param name="customerId">The customer id.</param>
        /// <returns>
        /// A <seealso cref="IList{AlarmConfig}"/> that contains the alarm configuration data for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then and empty list is returned.
        /// </returns>
        Task<IList<AlarmData>> GetAlarmConfigurationAsync(Guid assetId, Guid customerId);

        /// <summary>
        /// Gets the list of host alarms for the provided <paramref name="assetId"/>
        /// </summary>
        /// <param name="assetId">The asset id used to get the alarm configuration data.</param>
        /// <param name="customerId">The customer id.</param>
        /// <returns>
        /// A <seealso cref="IList{AlarmConfig}"/> that contains the host alarm data for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then and empty list is returned.
        /// </returns>
        Task<IList<AlarmData>> GetHostAlarmsAsync(Guid assetId, Guid customerId);

        /// <summary>
        /// Gets the list of facility tag alarms for the provided <paramref name="assetId"/>
        /// </summary>
        /// <param name="assetId">The asset id used to get the alarm configuration data.</param>
        /// <param name="customerId">The customer id.</param>
        /// <returns>
        /// A <seealso cref="IList{AlarmConfig}"/> that contains the facility tag alarms data for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then and empty list is returned.
        /// </returns>
        Task<IList<AlarmData>> GetFacilityTagAlarmsAsync(Guid assetId, Guid customerId);

        /// <summary>
        /// Gets the list of camera alarms for the provided <paramref name="assetId"/>
        /// </summary>
        /// <param name="assetId">The asset id used to get the alarm configuration data.</param>
        /// <param name="customerId">The customer id.</param>
        /// <returns>
        /// A <seealso cref="IList{AlarmConfig}"/> that contains the camera alarms data for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then and empty list is returned.
        /// </returns>
        Task<IList<AlarmData>> GetCameraAlarmsAsync(Guid assetId, Guid customerId);

        /// <summary>
        /// Gets the facility tag header and details <paramref name="assetId"/>
        /// </summary>
        /// <param name="assetId">The asset id used to get the facility configuration data.</param>
        /// <returns>The <seealso cref="FacilityDataModel"/> data.</returns>
        Task<FacilityDataModel> GetFacilityHeaderAndDetails(Guid assetId);
    }
}
