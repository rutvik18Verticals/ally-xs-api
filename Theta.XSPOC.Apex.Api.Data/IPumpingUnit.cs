using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// Represents an interface for a pumping unit.
    /// </summary>
    public interface IPumpingUnit
    {

        /// <summary>
        /// Gets the unit names for the specified node Ids.
        /// </summary>
        /// <param name="nodeIds">The list of node Ids.</param>
        /// <param name="correlationId"></param>
        /// <returns>The list of pumping unit names.</returns>
        IList<PumpingUnitForUnitNameModel> GetUnitNames(IList<string> nodeIds, string correlationId);

    }
}
