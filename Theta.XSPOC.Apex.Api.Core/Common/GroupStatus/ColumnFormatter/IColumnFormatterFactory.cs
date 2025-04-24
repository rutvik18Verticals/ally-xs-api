using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter
{
    /// <summary>
    /// Represents a factory for creating column formatters.
    /// </summary>
    public interface IColumnFormatterFactory
    {

        /// <summary>
        /// Creates a column formatter based on the source Id, name, and conditional formats.
        /// </summary>
        /// <param name="sourceId">The source ID.</param>
        /// <param name="name">The name of the column formatter.</param>
        /// <param name="conditionalFormats">The list of conditional formats.</param>
        /// <returns>The created column formatter.</returns>
        IColumnFormatter Create(int sourceId, string name, IList<ConditionalFormat> conditionalFormats);

        /// <summary>
        /// Creates a column formatter based on the source Id and name.
        /// </summary>
        /// <param name="sourceId">The source ID.</param>
        /// <param name="name">The name of the column formatter.</param>
        /// <returns>The created column formatter.</returns>
        IColumnFormatter Create(int sourceId, string name);

    }
}
