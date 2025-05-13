using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter;

namespace Theta.XSPOC.Apex.Api.Data.Influx.Services
{
    /// <summary>
    /// This interface defines the read operations to get data history items from the data store.
    /// </summary>
    public interface IGetDataHistoryItemsService
    {

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
        public Task<IList<DataPointModel>> GetDataHistoryItems(Guid assetId, Guid customerId, string pocType,
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
        /// Method to read the trend data from influx data store with <paramref name="channelIds"/>. 
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="customerId">The customer guid.</param>
        /// <param name="pocType">The poc type.</param>
        /// <param name="channelIds">The list of channel ids.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>The <seealso cref="IList{DataPointModel}"/></returns>
        public Task<IList<DataPointModel>> GetDataHistoryTrendData(Guid assetId, Guid customerId, string pocType,
            List<string> channelIds, string startDate, string endDate);

        /// <summary>
        /// Method to read the trend data from influx data store with <paramref name="channelIds"/>. 
        /// </summary>
        /// <param name="assetIds">The asset guid.</param>
        /// <param name="customerId">The customer guid.</param>
        /// <param name="channelIds">The list of channel ids.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="downsampleType">The down sample Type.</param>
        /// <param name="downsampleWindowSize">The down sample WindowSize.</param>
        /// <param name="pageNum">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The <seealso cref="IList{DataPointModel}"/></returns>
        public Task<IList<DataPointModel>> GetAllyTimeSeriesDataHistoryTrendData(List<Guid> assetIds, Guid customerId,
             List<string> channelIds, string startDate, string endDate, string downsampleType, string downsampleWindowSize, 
             int pageNum, int pageSize);

        /// <summary>
        /// Method to read the trend data from influx data store with <paramref name="channelIds"/>. 
        /// </summary>
        /// <param name="assetIds">The asset guid.</param>
        /// <param name="customerId">The customer guid.</param>
        /// <param name="channelIds">The list of channel ids.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="downsampleType">The down sample Type.</param>
        /// <param name="downsampleWindowSize">The down sample WindowSize.</param>
        /// <param name="pageNum">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="inputs">Asset details.</param>
        /// <returns>The <seealso cref="IList{AllyTrendData}"/></returns>
        public Task<IList<TimeSeriesData>> GetTimeSeriesResponseAsync(List<Guid> assetIds, Guid customerId,
             List<string> channelIds, string startDate, string endDate, string downsampleType, string downsampleWindowSize,
             int pageNum, int pageSize, List<TimeSeriesInputModel> inputs);

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
        /// Method to read the Current scan data from influx data store />. 
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <returns>The <seealso cref="IList{CurrentRawScanDataInfluxModel}"/></returns>
        public Task<IList<CurrentRawScanDataInfluxModel>> GetCurrentRawScanData(Guid assetId);

        /// <summary>
        /// Gets the graph data from InfluxDB stores based on the provided filters.
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="wellName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="pocType"></param>
        /// <param name="parameters"></param>
        /// <param name="channelIds"></param>
        /// <param name="listOfTrends"></param>
        /// <param name="aggregate"></param>
        /// <param name="aggregateMethod"></param>
        /// <param name="nodeTimeZoneOffset"></param>
        /// <param name="honorDaylighSaving"></param>
        /// <returns></returns>
        public Task<IList<DataPointsModelDto>> GetInfluxDataAssetTrends(Guid assetId, string wellName, DateTime startDate, DateTime endDate, int pocType, List<Parameters> parameters, List<string> channelIds, List<DefaultParameters> listOfTrends, string aggregate, string aggregateMethod, float nodeTimeZoneOffset, bool honorDaylighSaving);

        /// <summary>
        /// Method to read the Current scan data from influx data store />. 
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="customerId">The customer guid.</param>
        /// <returns></returns>
        public Task<IList<DataPointModel>> GetCurrentRawScanData(Guid assetId, Guid customerId);

        /// <summary>
        /// Gets the downtime data from InfluxDB stores based on the assetId, start date, and end date.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="channelId">The Channel Id.</param>
        /// <returns>The influx data for downtime PST in the form of <seealso cref="IList{DataPointModel}"/>.</returns>
        public Task<IList<DataPointModel>> GetDowntime(Guid assetId, DateTime startDate, DateTime endDate, string channelId);
    }
}
