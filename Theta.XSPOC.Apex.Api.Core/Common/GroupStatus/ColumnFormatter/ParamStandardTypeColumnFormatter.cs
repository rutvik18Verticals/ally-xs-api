using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter
{
    /// <summary>
    /// Represents a column formatter for alarm.
    /// </summary>
    public class ParamStandardTypeColumnFormatter : ColumnFormatterBase, IColumnFormatter
    {

        /// <summary>
        /// Gets the responsibilities of the column formatter.
        /// </summary>
        public IList<string> Responsibilities => new[]
        {
            "PARAMSTANDARD"
        };

        /// <summary>
        /// Calculates the value for the specified row column.
        /// </summary>
        /// <param name="dr">The data row containing the column value.</param>
        /// <param name="column">The row column model.</param>
        /// <param name="correlationId"></param>
        /// <param name="performCalculation">Indicates whether to perform the calculation.</param>
        /// <param name="cache"></param>
        /// <exception cref="ArgumentNullException"><paramref name="dr"/> is null OR
        /// <paramref name="column"/> is null.</exception>
        public void CalculateValue(IDictionary<string, object> dr, RowColumnModel column,
            string correlationId,
            bool performCalculation = true,
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

            if (!string.IsNullOrWhiteSpace(column.Value))
            {
                return;
            }

            if (column.ValueType == typeof(DateTime))
            {
                column.Value = "01/01/1970 12:00:00 AM";
            }
            else if (column.ValueType == typeof(double) || column.ValueType == typeof(float) || column.ValueType == typeof(int))
            {
                column.Value = "0";
            }
            else
            {
                column.Value = string.Empty;
            }
        }

        /// <summary>
        /// Performs formatting on the specified row column.
        /// </summary>
        /// <param name="dr">The data row containing the column value.</param>
        /// <param name="column">The row column model.</param>
        /// <param name="groupStatusColumn">The group status column.</param>
        /// <param name="correlationId"></param>
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
                throw new ArgumentNullException(nameof(groupStatusColumn));
            }

            string columnPrefix = GetKey(dr, groupStatusColumn.FieldHeading);

            var backColorKey = GetKey(dr, columnPrefix + ".BackColor");
            var backColor = dr[backColorKey].ToString();
            var foreColorKey = GetKey(dr, columnPrefix + ".ForeColor");
            var foreColor = dr[foreColorKey].ToString();

            if (backColorKey != null && backColor != null)
            {
                column.BackColor = backColor;
            }

            if (foreColorKey != null && foreColor != null)
            {
                column.ForeColor = foreColor;
            }

            if (column.BackColor == "")
            {
                // Use Default Colors in UI
            }

            if (column.ForeColor == "")
            {
                // Use Default Colors in UI
            }
        }

    }
}
