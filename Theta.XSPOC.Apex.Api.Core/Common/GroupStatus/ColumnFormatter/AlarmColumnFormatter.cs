using System;
using System.Collections.Generic;
using System.Drawing;

namespace Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter
{
    /// <summary>
    /// Represents a column formatter for alarm.
    /// </summary>
    public class AlarmColumnFormatter : ColumnFormatterBase, IColumnFormatter
    {

        /// <summary>
        /// Gets the responsibilities of the column formatter.
        /// </summary>
        public IList<string> Responsibilities => new[]
        {
            "ALARMS"
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
            var highPriAlarmKey = GetKey(dr, "tblNodeMaster.HighPriAlarm");
            string value = string.Empty;

            if (highPriAlarmKey != null && dr[highPriAlarmKey] != null)
            {
                if (dr[highPriAlarmKey].ToString()?.Length > 2)
                {
                    value = dr[highPriAlarmKey].ToString();
                }
                else if (enabled)
                {
                    value = "OK";
                }
            }
            else if (highPriAlarmKey != null && dr[highPriAlarmKey] == null)
            {
                if (enabled)
                {
                    value = "OK";
                }
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

            var enabled = IsEnabled(dr);
            var highPriAlarmKey = GetKey(dr, "tblNodeMaster.HighPriAlarm");
            var value = string.Empty;

            if (highPriAlarmKey != null && dr[highPriAlarmKey] != null)
            {
                if (dr[highPriAlarmKey].ToString()?.Length > 2)
                {
                    value = dr[highPriAlarmKey].ToString();
                }
                else if (enabled)
                {
                    value = "OK";
                }
            }

            if (value?.Length > 2)
            {
                column.BackColor = ConvertToHex(Color.Red);
                column.ForeColor = ConvertToHex(Color.White);
            }
        }

    }
}
