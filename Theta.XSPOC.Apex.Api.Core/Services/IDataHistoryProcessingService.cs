using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Interface for processing DataHistoryProcessingService.
    /// </summary>
    public interface IDataHistoryProcessingService
    {

        /// <summary>
        /// Processes the Data History Trend Data.
        /// </summary>
        /// <param name="assetId">The asset guid id.</param>
        /// <param name="correlationId"></param>
        /// <returns> <seealso cref="DataHistoryTrendsListOutput"/>.</returns>
        DataHistoryTrendsListOutput GetDataHistoryTrendData(string assetId, string correlationId);

        /// <summary>
        /// Processes the Data History Trend Data.
        /// </summary>
        /// <param name="inputData">The <seealso cref="WithCorrelationId{DataHistoryTrendInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="DataHistoryListOutput"/>.</returns>
        DataHistoryListOutput GetDataHistoryListData(WithCorrelationId<DataHistoryTrendInput> inputData);

        /// <summary>
        /// Processes the Data History Trend Data Items.
        /// </summary>
        /// <param name="inputData">The <seealso cref="WithCorrelationId{DataHistoryTrendInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="DataHistoryListOutput"/>.</returns>
        Task<DataHistoryTrendItemsOutput> GetDataHistoryTrendDataItemsAsync(WithCorrelationId<DataHistoryTrendInput> inputData);

        /// <summary>
        /// Processes the Data History Alarm Limits.
        /// </summary>
        /// <param name="input">The <seealso cref="WithCorrelationId{DataHistoryAlarmLimitsInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="DataHistoryAlarmLimitsOutput"/>.</returns>
        DataHistoryAlarmLimitsOutput GetAlarmLimits(WithCorrelationId<DataHistoryAlarmLimitsInput> input);

        /// <summary>
        /// Processes the Data History Trend Data Items.
        /// </summary>
        /// <param name="input">The <seealso cref="WithCorrelationId{DataHistoryTrendInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="DataHistoryListOutput"/>.</returns>
        Task<DataHistoryTrendOutput> GetDefaultTrendDataItemsAsync(WithCorrelationId<DataHistoryTrendInput> input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input">The <seealso cref="WithCorrelationId{DataHistoryTrendInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="DataHistoryDefaultTrendOutput"/>.</returns>
        DataHistoryDefaultTrendOutput GetDefaultTrendsViews(WithCorrelationId<DataHistoryTrendInput> input);

        /// <summary>
        /// Processes the Data History Trend Data Items.
        /// </summary>
        /// <param name="input">The <seealso cref="WithCorrelationId{TrendIDataInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="DataHistoryListOutput"/>.</returns>
        Task<DataHistoryTrendOutput> GetTrendDataAsync(WithCorrelationId<TrendIDataInput> input);
    }
}
