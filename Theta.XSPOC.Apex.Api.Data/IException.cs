using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// Represents an interface for retrieving exceptions.
    /// </summary>
    public interface IException
    {

        /// <summary>
        /// Retrieves a list of exceptions based on the provided node Ids.
        /// </summary>
        /// <param name="nodeIds">The list of node Ids.</param>
        /// <param name="correlationId"></param>
        /// <returns>A list of exception models.</returns>
        IList<ExceptionModel> GetExceptions(IList<string> nodeIds, string correlationId);

        /// <summary>
        /// This method will retrieve the exceptions for the provided <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset id used to retrieve the data.</param>
        /// <param name="customerId">The customer id used to verify customer has access to the data.</param>
        /// <param name="correlationId"></param>
        /// <returns>A <seealso cref="IList{ExceptionData}"/> that represents the most recent historical data
        /// for the defined registers for the provided <paramref name="assetId"/>.</returns>
        Task<IList<ExceptionData>> GetAssetStatusExceptionsAsync(Guid assetId, Guid customerId, string correlationId);

    }
}
