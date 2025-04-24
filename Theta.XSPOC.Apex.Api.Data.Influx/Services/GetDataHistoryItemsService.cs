using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data.Influx.Services
{
    /// <summary>
    /// This implementation to read the data stored in the InfluxDB.
    /// </summary>
    public class GetDataHistoryItemsService : IGetDataHistoryItemsService
    {

        #region Private Fields

        private readonly IDataHistoryTrendData _influxDataStore;

        #endregion

        #region Protected Fields

        /// <summary>
        /// The <seealso cref="IConfiguration"/> configurations.
        /// </summary>
        protected IConfiguration AppConfig { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs an instance of the <seealso cref="GetDataHistoryItemsService"/>.
        /// </summary>
        /// <param name="appConfig">The <seealso cref="IConfiguration"/>.</param>
        /// <param name="influxDataStore">The <seealso cref="IDataHistoryTrendData"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="appConfig"/> is null.
        /// </exception>
        public GetDataHistoryItemsService(IConfiguration appConfig, IDataHistoryTrendData influxDataStore)
        {
            AppConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            _influxDataStore = influxDataStore ?? throw new ArgumentNullException(nameof(influxDataStore));
        }

        #endregion

        #region IGetDataHistoryItemsService Implementation

        /// <summary>
        /// Gets the downtime data from InfluxDB stores based on the provided filters, start date, and end date.
        /// </summary>
        /// <param name="filterData">The list of downtime filters.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>The <seealso cref="IList{DowntimeByWellsInfluxModel}"/></returns>
        public async Task<IList<DowntimeByWellsInfluxModel>> GetDowntimeAsync(IList<DowntimeFiltersInflux> filterData,
            string startDate, string endDate)
        {
            var result = await _influxDataStore.GetDowntimeAsync(filterData, startDate, endDate);

            return result.ToList() ?? new List<DowntimeByWellsInfluxModel>();
        }

        /// <summary>
        /// Gets the data from InfluxDB stores based on available parameters. 
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="customerId">The customer guid.</param>
        /// <param name="pocType">The poc type.</param>
        /// <param name="addresses">The register addresses.</param>
        /// <param name="paramStandardType">The param standard type values.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>The <seealso cref="IList{DataPointModel}"/></returns>
        public async Task<IList<DataPointModel>> GetDataHistoryItems(Guid assetId, Guid customerId, string pocType,
            List<string> addresses, List<string> paramStandardType, string startDate, string endDate)
        {
            var data = await _influxDataStore.GetTrendData(assetId, customerId, pocType, addresses,
                paramStandardType, startDate, endDate);

            return data ?? null;
        }

        /// <summary>
        /// Method to get the historical trends data for the <paramref name="assetId"/> and <paramref name="customerId"/>
        /// and retirns the value at <paramref name="channelIds"/> for the dates between <paramref name="startDate"/> 
        /// and <paramref name="endDate"/>.  
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="customerId">The customer guid.</param>
        /// <param name="pocType">The poc type.</param>
        /// <param name="channelIds">The channeal id of addresses.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>The <seealso cref="IList{DataPointModel}"/></returns>
        public async Task<IList<DataPointModel>> GetDataHistoryTrendData(Guid assetId, Guid customerId, string pocType,
           List<string> channelIds, string startDate, string endDate)
        {
            var data = await _influxDataStore.GetTrendData(assetId, customerId, pocType, channelIds, startDate, endDate);

            return data ?? null;
        }

        /// <summary>
        /// Method to get the historical trends data for the <paramref name="assetIds"/> and <paramref name="customerId"/>
        /// and returns the value at <paramref name="channelIds"/> for the dates between <paramref name="startDate"/> 
        /// and <paramref name="endDate"/>.  
        /// </summary>
        /// <param name="assetIds">The asset guid.</param>
        /// <param name="customerId">The customer guid.</param>        
        /// <param name="channelIds">The channeal id of addresses.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="downsampleType">The down sample Type.</param>
        /// <param name="downsampleWindowSize">The down sample Window Size</param>
        /// <param name="pageNum">The page number</param>
        /// <param name="pageSize">The page size</param>
        /// <returns>The <seealso cref="IList{DataPointModel}"/></returns>
        public async Task<IList<DataPointModel>> GetAllyTimeSeriesDataHistoryTrendData(List<Guid> assetIds, Guid customerId, 
           List<string> channelIds, string startDate, string endDate, string downsampleType, string downsampleWindowSize
            , int pageNum, int pageSize)
        {
            var data = await _influxDataStore.GetAllyTimeSeriesTrendData(assetIds, customerId,
                                                 channelIds, startDate, endDate, downsampleType, downsampleWindowSize, pageNum, pageSize);

            return data ?? null;
        }

        /// <summary>
        /// Method to get the historical trends data for the <paramref name="assetIds"/> and <paramref name="customerId"/>
        /// and returns the value at <paramref name="channelIds"/> for the dates between <paramref name="startDate"/> 
        /// and <paramref name="endDate"/>.  
        /// </summary>
        /// <param name="assetIds">The asset guid.</param>
        /// <param name="customerId">The customer guid.</param>        
        /// <param name="channelIds">The channeal id of addresses.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="downsampleType">The down sample Type.</param>
        /// <param name="downsampleWindowSize">The down sample Window Size</param>
        /// <param name="pageNum">The page number</param>
        /// <param name="pageSize">The page size</param>
        /// <param name="inputs">Asset details.</param>
        /// <returns>The <seealso cref="IList{DataPointModel}"/></returns>
        public async Task<IList<TimeSeriesData>> GetTimeSeriesResponseAsync (List<Guid> assetIds, Guid customerId,
           List<string> channelIds, string startDate, string endDate, string downsampleType, string downsampleWindowSize
            , int pageNum, int pageSize, List<TimeSeriesInputModel> inputs)
        {
            var data = await _influxDataStore.GetTimeSeriesResponse(assetIds, customerId,
                                                 channelIds, startDate, endDate, downsampleType, downsampleWindowSize, pageNum, pageSize, inputs);

            return data ?? null;
        }

        /// <summary>
        /// Gets the downtime data from InfluxDB stores based on the provided filters, start date, and end date.
        /// </summary>
        /// <param name="filterData">The list of downtime filters.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>The <seealso cref="IList{DowntimeByWellsInfluxModel}"/></returns>
        public async Task<IList<DowntimeByWellsInfluxModel>> GetDowntimeAsync(IList<DowntimeFiltersWithChannelIdInflux> filterData,
            string startDate, string endDate)
        {
            var result = await _influxDataStore.GetDowntimeAsync(filterData, startDate, endDate);

            return result.ToList() ?? new List<DowntimeByWellsInfluxModel>();

        }

        /// <summary>
        /// Method to get the Current scan data for the <paramref name="assetId"/>
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <returns>The <seealso cref="IList{CurrentRawScanDataInfluxModel}"/></returns>
        public async Task<IList<CurrentRawScanDataInfluxModel>> GetCurrentRawScanData(Guid assetId)
        {
            var data = await _influxDataStore.GetCurrentRawScanData(assetId);

            return data ?? null;
        }

        #endregion

    }
}
