using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// Represents the interface for retrieving rods.
    /// </summary>
    public interface IRod
    {

        /// <summary>
        /// Retrieves the rods for a group status.
        /// </summary>
        /// <param name="nodeIds">The list of node Ids.</param>
        /// <param name="correlationId"></param>
        /// <returns>The list of rods for the group status.</returns>
        public IList<RodForGroupStatusModel> GetRodForGroupStatus(IList<string> nodeIds, string correlationId);

    }
}
