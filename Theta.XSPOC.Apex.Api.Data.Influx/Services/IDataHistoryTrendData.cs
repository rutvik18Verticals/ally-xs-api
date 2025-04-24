using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data.Influx.Services
{
    /// <summary>
    /// This is the interface for fetching data history trend data from influx.
    /// </summary>
    public interface IDataHistoryTrendData
    {

        /// <summary>
        /// Method to read the trend data from influx data store with <paramref name="channelIds"/>. 
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="customerId">The customer guid.</param>
        /// <param name="pocType">The poc type.</param>
        /// <param name="channelIds">The list of channel ids.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>The <seealso cref="IList{DataPointModel}"/></returns>
        Task<IList<DataPointModel>> GetTrendData(Guid assetId, Guid customerId, string pocType,
           List<string> channelIds, string startDate, string endDate);

        /// <summary>
        /// Method to read the trend data from influx data store with <paramref name="channelIds"/>. 
        /// </summary>
        /// <param name="assetIds">The list of asset guid.</param>
        /// <param name="customerId">The customer guid.</param>        
        /// <param name="channelIds">The list of channel ids.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="downsampleType">The down sample Type.</param>
        /// <param name="downsampleWindowSize">The down sample WindowSize.</param>
        /// <param name="pageNum">The page number.</param> 
        /// <param name="pageSize">The page size.</param> 
        /// <returns>The <seealso cref="IList{DataPointModel}"/></returns>
        Task<IList<DataPointModel>> GetAllyTimeSeriesTrendData(List<Guid> assetIds, Guid customerId,
           List<string> channelIds, string startDate, string endDate,string downsampleType, string downsampleWindowSize
            , int pageNum, int pageSize);

        /// <summary>
        /// Method to read the trend data from influx data store with <paramref name="channelIds"/>. 
        /// </summary>
        /// <param name="assetIds">The list of asset guid.</param>
        /// <param name="customerId">The customer guid.</param>        
        /// <param name="channelIds">The list of channel ids.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="downsampleType">The down sample Type.</param>
        /// <param name="downsampleWindowSize">The down sample WindowSize.</param>
        /// <param name="pageNum">The page number.</param> 
        /// <param name="pageSize">The page size.</param> 
        /// <param name="inputs">Asset details.</param>
        /// <returns>The <seealso cref="IList{DataPointModel}"/></returns>
        Task<IList<TimeSeriesData>> GetTimeSeriesResponse(List<Guid> assetIds, Guid customerId,
           List<string> channelIds, string startDate, string endDate, string downsampleType, string downsampleWindowSize
            , int pageNum, int pageSize, List<TimeSeriesInputModel> inputs);
        
        /// <summary>
        /// Method to read the trend data from influx data store with <paramref name="address"/>
        /// and <paramref name="paramStandardType"/>. 
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="customerId">The customer guid.</param>
        /// <param name="pocType">The poc type.</param>
        /// <param name="address">The register addresses.</param>
        /// <param name="paramStandardType">The param standard type values.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>The <seealso cref="IList{DataPointModel}"/></returns>
        Task<IList<DataPointModel>> GetTrendData(Guid assetId, Guid customerId, string pocType,
           List<string> address, List<string> paramStandardType, string startDate, string endDate);

        /// <summary>
        /// Gets the downtime data from InfluxDB stores based on the provided filters, start date, and end date.
        /// </summary>
        /// <param name="filterData">The list of downtime filters.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>The <seealso cref="IList{DowntimeByWellsInfluxModel}"/></returns>
        public Task<IList<DowntimeByWellsInfluxModel>> GetDowntimeAsync(
            IList<DowntimeFiltersInflux> filterData, string startDate, string endDate);

        /// <summary>
        /// Gets the downtime data from InfluxDB stores based on the provided filters, start date, and end date.
        /// </summary>
        /// <param name="filterData">The list of downtime filters.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>        
        /// <returns>The <seealso cref="IList{DowntimeByWellsInfluxModel}"/></returns>
        public Task<IList<DowntimeByWellsInfluxModel>> GetDowntimeAsync(
            IList<DowntimeFiltersWithChannelIdInflux> filterData, string startDate, string endDate);

        /// <summary>
        /// Method to fetch the letest trend data from influx data store.
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="customerId">The customer guid.</param>
        /// <param name="pocType">The poc type.</param>
        /// <param name="channelIds">The list of channel ids.</param>
        /// <returns></returns>
        public Task<IList<DataPointModel>> GetLatestTrendData(Guid assetId, Guid customerId, string pocType,
            List<string> channelIds);

        /// <summary>
        /// Method to read the current scan data from influx data store with <paramref name="assetId"/>
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <returns>The <seealso cref="IList{CurrentRawScanDataInfluxModel}"/></returns>
        Task<IList<CurrentRawScanDataInfluxModel>> GetCurrentRawScanData(Guid assetId);

    }
}
