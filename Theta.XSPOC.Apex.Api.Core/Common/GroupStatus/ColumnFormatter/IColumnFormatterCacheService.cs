using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter
{
    /// <summary>
    /// Represents a cache service for column formatters.
    /// </summary>
    public interface IColumnFormatterCacheService
    {

        /// <summary>
        /// Retrieves the data for a given name and node list.
        /// </summary>
        /// <param name="name">The name of the data.</param>
        /// <param name="nodeList">The list of nodes.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The retrieved data.</returns>
        object GetData(string name, IList<string> nodeList, string correlationId);

    }
}
