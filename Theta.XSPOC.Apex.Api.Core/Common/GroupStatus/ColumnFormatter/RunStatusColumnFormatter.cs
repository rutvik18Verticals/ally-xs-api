using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter
{
    /// <summary>
    /// Represents a column formatter for Run Status Column values.
    /// </summary>
    public class RunStatusColumnFormatter : ColumnFormatterBase, IColumnFormatter
    {

        /// <summary>
        /// Gets the responsibilities of the column formatter.
        /// </summary>
        public IList<string> Responsibilities => new[]
        {
            "RUN STATUS",
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

            var enabled = IsEnabled(dr);

            var runStatusKey = GetKey(dr, "tblNodeMaster.RunStatus");
            var runStatus = dr[runStatusKey].ToString();
            string value;
            if (runStatus != string.Empty && enabled)
            {
                value = runStatus;
            }
            else
            {
                value = string.Empty;
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
            if (dr == null)
            {
                throw new ArgumentNullException(nameof(dr));
            }

            if (column == null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            var enabled = IsEnabled(dr);
            var runStatusKey = GetKey(dr, "tblNodeMaster.RunStatus");
            var runStatus = dr[runStatusKey].ToString();

            GetColorsFromStatus(enabled, runStatus, column);
        }

        #region private method

        /// <summary>
        /// Performs formatting on the specified row column.
        /// </summary>
        private void GetColorsFromStatus(bool enabled, string runStatus, RowColumnModel column)
        {
            if (column == null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            if (runStatus == null)
            {
                throw new ArgumentNullException(nameof(runStatus));
            }

            string[] badStatus =
            {
                "Stopped", "Sutdown", "Unable to Run", "Cannot Stop", "Stop Malf"
            };
            string[] runningStatus =
            {
                "Running", "Idle"
            };

            if (enabled == false)
            {
                return;
            }

            if (runStatus == string.Empty)
            {
                column.BackColor = ConvertToHex(Color.Yellow);
                column.ForeColor = ConvertToHex(Color.Black);

                // Our UI requires this to show borders for "empty" cells.
                if (column.Value == string.Empty)
                {
                    column.Value = " ";
                }
            }
            else if (badStatus.Contains(runStatus)
                     || runStatus.IndexOf("Shutdown") > -1
                     || runStatus.IndexOf("Loss") > -1
                     || runStatus.IndexOf(":") > -1)
            {
                column.BackColor = ConvertToHex(Color.Red);
                column.ForeColor = ConvertToHex(Color.White);
            }
            else if (runningStatus.Contains(runStatus) || (runStatus.IndexOf("-") > -1))
            {

            }
            else
            {
                column.BackColor = ConvertToHex(Color.Yellow);
                column.ForeColor = ConvertToHex(Color.Black);
            }
        }

        #endregion

    }

}
