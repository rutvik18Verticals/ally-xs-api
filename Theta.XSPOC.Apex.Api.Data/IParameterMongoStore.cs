using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface that represents groups and assets.
    /// </summary>
    public interface IParameterMongoStore
    {

        /// <summary>
        /// Gets the <seealso cref="Parameters"/> for the specified <paramref name="pocType"/> and
        /// <paramref name="address"/>.
        /// </summary>
        /// <param name="pocType">The poc type.</param>
        /// <param name="address">The register address.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>A <see cref="List{Parameters}"/> object containing channel id.</returns>
        List<Parameters> GetParameterData(string pocType, int address, string correlationId);

        /// <summary>
        /// Gets the <seealso cref="Parameters"/> for the specified <paramref name="pocType"/> and
        /// <paramref name="paramStdType"/>.
        /// </summary>
        /// <param name="pocType">The poc type.</param>
        /// <param name="paramStdType">The param standard type.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>A <see cref="Parameters"/> object containing channel id.</returns>
        Parameters GetParameterByParamStdType(string pocType, int paramStdType, string correlationId);

        /// <summary>
        /// Gets the <seealso cref="Parameters"/> for the specified <paramref name="pocType"/> and
        /// <paramref name="paramStdTypes"/>.
        /// </summary>
        /// <param name="pocType">The poc type.</param>
        /// <param name="paramStdTypes">The list of param standard type.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>A <see cref="List{Parameters}"/> object containing channel id.</returns>
        List<Parameters> GetParameterByParamStdType(string pocType, List<string> paramStdTypes,
            string correlationId);

        /// <summary>
        /// Gets the <seealso cref="Parameters"/> for the specified <paramref name="pocTypes"/> and
        /// <paramref name="paramStdTypes"/>.
        /// </summary>
        /// <param name="pocTypes">The poc type.</param>
        /// <param name="paramStdTypes">The list of param standard type.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>A <see cref="List{Parameters}"/> object containing channel id.</returns>
        public List<Parameters> GetParameterByParamStdType(List<string> pocTypes, List<string> paramStdTypes,
            string correlationId);

    }
}
