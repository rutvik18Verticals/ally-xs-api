using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{

    /// <summary>
    /// Represents the interface for retrieving and manipulating PocType data.
    /// </summary>
    public interface IPocType
    {

        /// <summary>
        /// Retrieves all PocType models.
        /// </summary>
        /// <param name="correlationId">The correlation Id.</param>
        /// <returns>A list of PocType models.</returns>
        IList<PocTypeModel> GetAll(string correlationId);

        /// <summary>
        /// Retrieves a specific PocType model by its Id.
        /// </summary>
        /// <param name="pocType">The Id of the PocType.</param>
        /// <param name="correlationId">The correlation Id.</param>
        /// <returns>The PocType model.</returns>
        PocTypeModel Get(int pocType, string correlationId);

    }
}
