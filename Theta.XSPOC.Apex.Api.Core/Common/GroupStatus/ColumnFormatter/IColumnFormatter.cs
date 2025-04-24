using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter
{
    /// <summary>
    /// Represents a column formatter for performing formatting operations on a row column.
    /// </summary>
    public interface IColumnFormatter
    {

        /// <summary>
        /// Gets the responsibilities of the column formatter.
        /// </summary>
        public IList<string> Responsibilities { get; }

        /// <summary>
        /// Calculates the value for the specified row column.
        /// </summary>
        /// <param name="dr">The data row containing the column value.</param>
        /// <param name="column">The row column model.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="performCalculation">Indicates whether to perform the calculation.</param>
        /// <param name="cache">The cached data.</param>
        void CalculateValue(IDictionary<string, object> dr, RowColumnModel column, string correlationId, bool performCalculation = true,
            object cache = null);

        /// <summary>
        /// Performs formatting on the specified row column.
        /// </summary>
        /// <param name="dr">The data row containing the column value.</param>
        /// <param name="column">The row column model.</param>
        /// <param name="groupStatusColumn">The <see cref="GroupStatusColumns"/>.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="cache">The cached data.</param>
        void PerformFormat(IDictionary<string, object> dr, RowColumnModel column, GroupStatusColumns groupStatusColumn,
            string correlationId, object cache = null);

    }
}
