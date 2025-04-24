using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter
{
    /// <summary>
    /// Represents a column formatter for the "COMM" column.
    /// </summary>
    public class CommColumnFormatter : ColumnFormatterBase, IColumnFormatter
    {

        /// <summary>
        /// Gets the responsibilities of the column formatter.
        /// </summary>
        public IList<string> Responsibilities => new[]
        {
            "COMM",
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
            if (dr == null)
            {
                throw new ArgumentNullException(nameof(dr));
            }

            if (column == null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            var enabled = IsEnabled(dr);

            var key =
                dr.FirstOrDefault(x => x.Key.Equals("tblNodeMaster.CommStatus", StringComparison.OrdinalIgnoreCase)).Key;

            string value;

            if (enabled == false || dr[key] == null)
            {
                dr[key] = string.Empty;
                value = string.Empty;
            }
            else
            {
                value = dr[key].ToString();
            }

            column.Value = value;
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

            var key = GetKey(dr, "Comm");

            var value = dr[key] == null ? string.Empty : dr[key].ToString();

            if (value == string.Empty || value == "OK")
            {
                // Use Default Colors in UI
            }
            else
            {
                column.BackColor = ConvertToHex(Color.Red);
                column.ForeColor = ConvertToHex(Color.White);
            }
        }

    }
}
