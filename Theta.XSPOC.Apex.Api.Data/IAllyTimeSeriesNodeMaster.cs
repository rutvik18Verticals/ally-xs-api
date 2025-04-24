using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{      /// <summary>
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
    }
}
