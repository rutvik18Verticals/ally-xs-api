using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter
{
    /// <summary>
    /// Represents a column formatter for TIS Column values.
    /// </summary>
    public class TISColumnFormatter : ColumnFormatterBase, IColumnFormatter
    {

        /// <summary>
        /// Gets the responsibilities of the column formatter.
        /// </summary>
        public IList<string> Responsibilities => new[]
        {
            "TIS",
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

            var TIS = GetKey(dr, "TIS");
            var timeInState = dr[TIS].ToString();
            string value;
            if (Convert.IsDBNull(timeInState) || !String.IsNullOrEmpty(timeInState))
            {
                value = CalculateTIS(timeInState);
            }

            else
            {
                value = "";
            }

            column.Value = value;
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
            // No formatting logic provided in the original VB.NET code
        }

        /// <summary>
        /// Performs caluculate TIS.
        /// </summary>
        /// <param name="time">The data row containing the column value.</param>
        /// <exception cref="ArgumentNullException"><paramref name="time"/> is null OR
        /// <paramref name="time"/> is null.</exception>
        private static string CalculateTIS(string time)
        {
            int mins = Convert.ToInt32(time);
            TimeSpan t = new TimeSpan(0, mins, 0);
            string timeFormat;

            if (mins < 60)
            {
                timeFormat = "m";
            }
            else if (mins >= 60 && mins < 1440)
            {
                time = t.TotalHours.ToString("N1");
                timeFormat = "h";
            }
            else
            {
                time = t.TotalDays.ToString("N1");
                timeFormat = "d";
            }

            return time + " " + timeFormat;
        }

    }
}
