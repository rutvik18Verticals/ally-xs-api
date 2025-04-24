using System;
using System.Collections.Generic;
using System.Drawing;

namespace Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter
{
    /// <summary>
    /// Represents a column formatter for percentage comm values.
    /// </summary>
    public class PercentCommColumnFormatter : ColumnFormatterBase, IColumnFormatter
    {

        /// <summary>
        /// Gets the responsibilities of the column formatter.
        /// </summary>
        public IList<string> Responsibilities => new[]
        {
            "%COM", "TBLNODEMASTER.COMMSUCCESS",
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

            if (TryGetValue(dr, out var value))
            {
                column.Value = value;
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

            // Use Default Colors in UI

            if (TryGetValue(dr, out var value) == false)
            {
                return;
            }

            var enabled = IsEnabled(dr);

            if (enabled && long.TryParse(value, out var valueLong) && valueLong < 75)
            {
                column.BackColor = ConvertToHex(Color.Red);
                column.ForeColor = ConvertToHex(Color.White);
            }
        }

        private bool TryGetValue(IDictionary<string, object> dr, out string value)
        {
            var commSuccessKey = GetKey(dr, "tblNodeMaster.CommSuccess");
            var commAttemptKey = GetKey(dr, "tblNodeMaster.CommAttempt");

            value = string.Empty;

            if (dr[commSuccessKey] == null)
            {
                return false;
            }

            if (dr[commAttemptKey] != null)
            {
                if (dr[commAttemptKey].Equals(0))
                {
                    value = "0";
                }
                else if (dr[commSuccessKey].Equals(0))
                {
                    value = "0";
                }
                else
                {
                    var calculatedValue = float.Parse(dr[commSuccessKey].ToString()) /
                        float.Parse(dr[commAttemptKey].ToString()) * 100;

                    value = float.IsNaN(calculatedValue) ? "0" : Math.Round(calculatedValue).ToString();
                }
            }

            return true;
        }

    }
}
