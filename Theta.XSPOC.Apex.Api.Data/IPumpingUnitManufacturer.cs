using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// Represents an interface for retrieving pumping unit manufacturers.
    /// </summary>
    public interface IPumpingUnitManufacturer
    {

        /// <summary>
        /// Retrieves a list of pumping unit manufacturers for the specified node IDs.
        /// </summary>
        /// <param name="nodeIds">The list of node IDs.</param>
        /// <param name="correlationId"></param>
        /// <returns>A list of pumping unit manufacturers.</returns>
        IList<PumpingUnitManufacturerForGroupStatus> GetManufacturers(IList<string> nodeIds, string correlationId);

    }
}
