using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data.HistoricalData
{
    /// <summary>
    /// This is the interface that defines the methods for the historical store.
    /// </summary>
    public interface IHistoricalStore
    {

        /// <summary>
        /// This method will retrieve the most recent historical data for the defined registers for the provided
        /// <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset id used to retrieve the data.</param>
        /// <param name="customerId">The customer id used to verify customer has access to the data.</param>
        /// <returns>
        /// A <seealso cref="IList{RegisterData}"/> that represents the most recent historical data
        /// for the defined registers for the provided <paramref name="assetId"/>.
        /// </returns>
        Task<IList<RegisterData>> GetAssetStatusRegisterDataAsync(Guid assetId, Guid customerId);

        /// <summary>
        /// This method will retrieve the most recent param standard data for the provided <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset id used to retrieve the data.</param>
        /// <param name="customerId">The customer id used to verify customer has access to the data.</param>
        /// <returns>
        /// A <seealso cref="IList{ParamStandardData}"/> that represents the most recent historical data
        /// for the param standard data for the provided <paramref name="assetId"/>.
        /// </returns>
        Task<IList<ParamStandardData>> GetParamStandardDataAsync(Guid assetId, Guid customerId);

    }
}
