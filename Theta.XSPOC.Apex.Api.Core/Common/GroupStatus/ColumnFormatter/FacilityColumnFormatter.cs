using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter
{
    /// <summary>
    /// Represents a column formatter for Percent Fill Column values.
    /// </summary>
    public class FacilityColumnFormatter : ColumnFormatterBase, IColumnFormatter
    {

        /// <summary>
        /// Gets the responsibilities of the column formatter.
        /// </summary>
        public IList<string> Responsibilities => new[]
        {
            "FACILITY"
        };

        /// <summary>
        /// Calculates the value for the specified row column.
        /// </summary>
        /// <param name="dr">The data row containing the column value.</param>
        /// <param name="column">The row column model.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="performCalculation">Indicates whether to perform the calculation.</param>
        /// <param name="cache"></param>
        /// <exception cref="ArgumentNullException"><paramref name="dr"/> is null OR
        /// <paramref name="column"/> is null.</exception>
        public void CalculateValue(IDictionary<string, object> dr, RowColumnModel column, string correlationId,
            bool performCalculation = true,
            object cache = null)
        {
        }

        /// <summary>
        /// Performs formatting on the specified row column.
        /// </summary>
        /// <param name="dr">The data row containing the column value.</param>
        /// <param name="column">The row column model.</param>
        /// <param name="groupStatusColumn">The group status column.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="cache">The cached data.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dr"/> is null OR
        /// <paramref name="column"/> is null.</exception>
        public void PerformFormat(IDictionary<string, object> dr, RowColumnModel column, GroupStatusColumns groupStatusColumn,
            string correlationId,
            object cache = null)
        {
            if (dr == null)
            {
                throw new ArgumentNullException(nameof(dr));
            }

            if (column == null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            if (groupStatusColumn == null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            string columnPrefix = GetKey(dr, groupStatusColumn.FieldHeading);

            var backColorKey = GetKey(dr, columnPrefix.Remove(0, 16) + ".BackColor");
            var backColor = dr[backColorKey].ToString();
            var foreColorKey = GetKey(dr, columnPrefix.Remove(0, 16) + ".ForeColor");
            var foreColor = dr[foreColorKey].ToString();

            if (backColor == "")
            {
                // Use Default Colors in UI
            }

            if (foreColor == "")
            {
                // Use Default Colors in UI
            }
        }

    }
}
