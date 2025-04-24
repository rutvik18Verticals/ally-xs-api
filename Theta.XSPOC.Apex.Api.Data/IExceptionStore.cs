using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface that defines the methods for the exception store.
    /// </summary>
    public interface IExceptionStore
    {

        /// <summary>
        /// This method will retrieve the exceptions for the provided <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset id used to retrieve the data.</param>
        /// <param name="customerId">The customer id used to verify customer has access to the data.</param>
        /// <returns>A <seealso cref="IList{ExceptionData}"/> that represents the most recent historical data
        /// for the defined registers for the provided <paramref name="assetId"/>.</returns>
        Task<IList<ExceptionData>> GetAssetStatusExceptionsAsync(Guid assetId, Guid customerId);

    }
}
