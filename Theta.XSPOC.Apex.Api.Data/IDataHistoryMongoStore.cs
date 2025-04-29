
using System.Collections.Generic;
using System;
using Theta.XSPOC.Apex.Api.Data.Models;
using System.Threading.Tasks;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// Mongo Implementation of the IDataHistorySQLStore interface's methods which has DataHistory Table in use.
    /// </summary>
    public interface IDataHistoryMongoStore
    {
        /// <summary>
        /// Gets the <seealso cref="IList{MeasurementTrendDataModel}"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="correlationId"></param>
        /// <param name="paramStandardType"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns>The <seealso cref="IList{MeasurementTrendDataModel}"/>.</returns>
        Task<IList<MeasurementTrendDataModel>> GetMeasurementTrendData(string nodeId,
            int paramStandardType, DateTime startDate, DateTime endDate, string correlationId);

        /// <summary>
        /// Gets the <seealso cref="IList{MeasurementTrendDataModel}"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{MeasurementTrendDataModel}"/>.</returns>
        IList<MeasurementTrendItemModel> GetMeasurementTrendItems(string nodeId, string correlationId);

        /// <summary>
        /// Get the controller trend item by node id and poc type.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="pocType">The poc type.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="ControllerTrendItemModel"/>.</returns>
        IList<ControllerTrendItemModel> GetControllerTrendItems(string nodeId, int pocType, string correlationId);

        /// <summary>
        /// Gets the <seealso cref="IList{ControllerTrendDataModel}"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="address">The address.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{ControllerTrendDataModel}"/>.</returns>
        Task<IList<ControllerTrendDataModel>> GetControllerTrendData(string nodeId,
            int address, DateTime startDate, DateTime endDate, string correlationId);

        /// <summary>
        /// Gets the downtime by wells.
        /// </summary>
        /// <param name="nodeIds">The node ids.</param>
        /// <param name="numberOfDays">The number of days.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <see cref="DowntimeByWellsModel"/>.</returns>
        DowntimeByWellsModel GetDowntime(IList<string> nodeIds, int numberOfDays, string correlationId);
    }
}
