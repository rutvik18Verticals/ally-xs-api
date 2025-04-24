using System;
using System.Collections.Generic;
using System.Drawing;

namespace Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter
{
    /// <summary>
    /// Represents a column formatter for Percent Fill Column values.
    /// </summary>
    public class PercentFillColumnFormatter : ColumnFormatterBase, IColumnFormatter
    {

        /// <summary>
        /// Gets the responsibilities of the column formatter.
        /// </summary>
        public IList<string> Responsibilities => new[]
        {
            "%FILL",
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

            var PumpFillage = GetKey(dr, "tblNodeMaster.PumpFillage");
            var pumpFillage = dr[PumpFillage].ToString();
            var WellOpType = GetKey(dr, "tblNodeMaster.WellOpType");
            var wellOpType = dr[WellOpType].ToString();
            var value = "0";
            if (enabled)
            {
                if (!string.IsNullOrEmpty(wellOpType))
                {
                    if (wellOpType == "0" || wellOpType == "3")
                    {
                        value = "0";
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(pumpFillage))
                        {
                            value = pumpFillage;
                        }
                    }
                }
                else
                {
                    value = "0";
                }
            }
            else
            {
                value = "0";
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
            var AlarmStateFillage = GetKey(dr, "tblWellStatistics.AlarmStateFillage");
            var alarmStateFillage = dr[AlarmStateFillage].ToString();
            if (alarmStateFillage != null || enabled)
            {
                switch (alarmStateFillage)
                {
                    case "1":
                        column.BackColor = ConvertToHex(Color.Red);
                        column.ForeColor = ConvertToHex(Color.White);
                        break;
                    case "2":
                        column.BackColor = ConvertToHex(Color.PaleVioletRed);
                        column.ForeColor = ConvertToHex(Color.Black);
                        break;
                    case "3":
                        column.BackColor = ConvertToHex(Color.Yellow);
                        column.ForeColor = ConvertToHex(Color.Black);
                        break;
                    default:
                        break;
                }
            }
        }

    }

}
