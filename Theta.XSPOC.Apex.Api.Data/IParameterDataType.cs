using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface that represents the IParameterDataType.
    /// </summary>
    public interface IParameterDataType
    {

        /// <summary>
        /// Gets the data types.
        /// </summary>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="DataTypesModel"/>.</returns>
        public IList<DataTypesModel> GetItems(string correlationId);

        /// <summary>
        /// Gets the data types for each of the provided addresses for a given <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="addresses">The addresses.</param>
        /// <param name="correlationId"></param>
        /// <returns>A dictionary with keys as addresses and values as data types.</returns>
        IDictionary<int, short?> GetParametersDataTypes(Guid assetId, IList<int> addresses, string correlationId);

    }
}
