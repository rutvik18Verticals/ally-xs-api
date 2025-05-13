using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;
using MongoCustomerCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Customers;
using ParameterMongo = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter;

namespace Theta.XSPOC.Apex.Api.Data
{   
    /// <summary>
    /// This is the interface that represents node master.
    /// </summary>
    public interface IAllyTimeSeriesNodeMaster
    {
        /// <summary>
        /// Retrieves the node data based on asset ids.
        /// </summary>
        /// <param name="assetId">The asset ids.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The list of <seealso cref="NodeMasterModel"/>.</returns>
        Task<IList<NodeMasterModel>> GetByAssetIdsForAllyTimeSeriesAsync(IList<Guid> assetId, string correlationId);

        /// <summary>
        /// Retrieves the node data based on customer id.
        /// </summary>
        /// <param name="customerIds">The customer id.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The list of <seealso cref="NodeMasterModel"/>.</returns>
        Task<IList<NodeMasterModel>> GetAssetsByCustomerIdAsync(List<string> customerIds, string correlationId);

        /// <summary>
        /// Retrieves parameter standard type data.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The list of <seealso cref="NodeMasterModel"/>.</returns>
        Task<IList<ParamStandardData>> GetAllParameterStandardTypesAsync(string correlationId);

        /// <summary>
        /// Validates if customer token is valid.
        /// </summary>
        /// <param name="customerId">The customer id.</param>
        /// <param name="tokenKey">The API token.</param>
        /// <param name="tokenValue">The API token.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="NodeMasterModel"/>.</returns>
        public Task<IList<NodeMasterModel>> ValidateCustomerAsync(string customerId, string tokenKey, string tokenValue, string correlationId);
        /// <summary>
        /// Gets all default parameter collection values
        /// </summary>
        /// <param name="liftType">THe lift type.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="DefaultParameters"/>.</returns>
        public Task<IList<DefaultParameters>> GetAllDefaultParametersAsync(string liftType, string correlationId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        public Task<IList<TimeSeriesChartAggregation>> GetTimeSeriesChartAggregationAsync(string correlationId);

        /// <summary>
        /// Get Asset Mongo Collection.
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        public Task<MongoAssetCollection.Asset> GetAssetAsync(Guid assetId, string correlationId);

        /// <summary>
        /// Get Customer Mongo Collection.
        /// </summary>
        /// <param name="customerObjectId"></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        public Task<MongoCustomerCollection.Customer> GetCustomerAsync(string customerObjectId, string correlationId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterKeys"></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        public Task<IDictionary<(int POCType, string ChannelId), ParameterMongo.Parameters>> GetParametersBulk(List<(int POCType, string ChannelId)> parameterKeys, string correlationId);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        public Task<IList<NodeMasterModel>> GetAssetDetails(Guid assetId, string correlationId);
    }
}
