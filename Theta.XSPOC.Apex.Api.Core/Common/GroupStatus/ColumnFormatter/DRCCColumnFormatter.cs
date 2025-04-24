using System;
using System.Collections.Generic;
using System.Drawing;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;

namespace Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter
{
    /// <summary>
    /// Represents a column formatter for DRCC Column values.
    /// </summary>
    public class DRCCColumnFormatter : ColumnFormatterBase, IColumnFormatter
    {

        #region Private Fields

        private readonly IDateTimeConverter _dateTimeConverter;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the <seealso cref="DRCCColumnFormatter"/>.
        /// </summary>
        /// <param name="dateTimeConverter">The <seealso cref="IDateTimeConverter"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dateTimeConverter"/> is null.
        /// </exception>
        public DRCCColumnFormatter(IDateTimeConverter dateTimeConverter)
        {
            _dateTimeConverter = dateTimeConverter ?? throw new ArgumentNullException(nameof(dateTimeConverter));
        }

        #endregion

        /// <summary>
        /// Gets the responsibilities of the column formatter.
        /// </summary>
        public IList<string> Responsibilities => new[]
        {
            "DRC",
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

            var downReasonCodeKey = GetKey(dr, "tblWellDetails.DownReasonCode");
            var downReasonCode = dr[downReasonCodeKey].ToString();
            var value = string.Empty;

            if (downReasonCode != null)
            {
                if (downReasonCode.Length > 1)
                {
                    value = downReasonCode;
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
            var todayRunTimeKey = GetKey(dr, "tblNodeMaster.TodayRunTime");
            var todayRunTime = Convert.ToInt32(dr[todayRunTimeKey]);
            var commStatusKey = GetKey(dr, "tblNodeMaster.CommStatus");
            var commStatus = dr[commStatusKey].ToString();
            var runStatusKey = GetKey(dr, "tblNodeMaster.RunStatus");
            var runStatus = dr[runStatusKey].ToString();
            var lastGoodScanTimeKey = GetKey(dr, "tblNodeMaster.LastGoodScanTime");
            var lastGoodScanTime = _dateTimeConverter.ConvertFromApplicationServerTimeToUTC
                (Convert.ToDateTime(dr[lastGoodScanTimeKey]), "", LoggingModel.GroupStatus);

            if (column.Value.Length > 1)
            {
                if (todayRunTime > 0 && enabled)
                {
                    column.BackColor = ConvertToHex(Color.Red);
                    column.ForeColor = ConvertToHex(Color.White);
                }
            }
            else
            {
                if (runStatus?.ToLower() == "shutdown"
                    || (todayRunTime < 0.2 && commStatus?.ToLower() == "ok")
                    || (commStatus?.ToLower() == "timeout" && (DateTime.UtcNow - lastGoodScanTime).TotalHours > 1))
                {
                    column.BackColor = ConvertToHex(Color.Red);
                    column.ForeColor = ConvertToHex(Color.White);

                    // Our UI requires this to show borders for "empty" cells.
                    if (column.Value == string.Empty)
                    {
                        column.Value = " ";
                    }
                }
            }
        }

    }

}
