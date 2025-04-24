using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface that represents host alarm.
    /// </summary>
    public interface IHostAlarm
    {

        /// <summary>
        /// Get the IHostAlarm limits for data history.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="addresses">The addresses.</param>
        /// <param name="correlationId"></param>
        /// <returns>The List of <seealso cref="HostAlarmForDataHistoryModel"/>.</returns>
        IList<HostAlarmForDataHistoryModel> GetLimitsForDataHistory(Guid assetId, int[] addresses, string correlationId);

        /// <summary>
        /// Get all host alarms for group status.
        /// </summary>
        /// <param name="nodeIds">The list of node ids.</param>
        /// <param name="correlationId"></param>
        /// <returns>The List of <seealso cref="HostAlarmForGroupStatus"/>.</returns>
        IList<HostAlarmForGroupStatus> GetAllGroupStatusHostAlarms(IList<string> nodeIds, string correlationId);

    }
}
