using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Models;
using Theta.XSPOC.Apex.Api.Core.Services;

namespace Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter
{
    /// <summary>
    /// Represents a column formatter for Percent RT Column values.
    /// </summary>
    public class PercentRTColumnFormatter : ColumnFormatterBase, IColumnFormatter
    {

        #region Private Fields

        private readonly ICommonService _commonService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PercentRTColumnFormatter"/> class.
        /// </summary>
        /// <param name="commonService">The common service.</param>
        /// <exception cref="ArgumentNullException"><paramref name="commonService"/> is null.</exception>
        public PercentRTColumnFormatter(ICommonService commonService)
        {
            _commonService = commonService ??
                throw new ArgumentNullException(nameof(commonService));
        }

        #endregion

        /// <summary>
        /// Gets the responsibilities of the column formatter.
        /// </summary>
        public IList<string> Responsibilities => new[]
        {
            "%RT",
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
            bool performCalculation = true, object cache = null)
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
            bool gaugeOffHourIsSet = false;

            var lastGoodScanTimeKey = GetKey(dr, "tblNodeMaster.LastGoodScanTime");
            var lastGoodScanTime = string.IsNullOrWhiteSpace(lastGoodScanTimeKey) ? string.Empty : dr[lastGoodScanTimeKey].ToString();

            var todayRuntimeKey = GetKey(dr, "tblNodeMaster.TodayRuntime");
            var todayRuntime = string.IsNullOrWhiteSpace(todayRuntimeKey) ? string.Empty : dr[todayRuntimeKey].ToString();

            var pocTypeKey = GetKey(dr, "tblNodeMaster.POCType");
            var pocType = string.IsNullOrWhiteSpace(pocTypeKey) ? string.Empty : dr[pocTypeKey].ToString();

            float value;
            string gaugeOffHour = "0";

            if (string.IsNullOrWhiteSpace(lastGoodScanTime) == false && string.IsNullOrWhiteSpace(todayRuntime) == false &&
                enabled)
            {
                var lastGoodScanHours = Convert.ToDateTime(lastGoodScanTime).Hour +
                    Convert.ToDateTime(lastGoodScanTime).Minute / 60f;

                if (!gaugeOffHourIsSet)
                {
                    gaugeOffHour = _commonService.GetSystemParameter("GAUGE_OFF_HOUR",
                        $"{MemoryCacheValueType.GroupStatusSystemParameterGaugeOffHour}", correlationId);

                    gaugeOffHour = string.IsNullOrWhiteSpace(gaugeOffHour) ? "0" : gaugeOffHour;
                }

                var hoursSinceGO = lastGoodScanHours - Convert.ToInt16(gaugeOffHour);

                if (hoursSinceGO < 0)
                {
                    hoursSinceGO += 24;
                }

                if (hoursSinceGO != 0)
                {
                    value = float.Parse(todayRuntime) * 100.0f / hoursSinceGO + 0.01f;
                }
                else
                {
                    value = 0;
                }

                if (value > 100)
                {
                    value = 100;
                }

                if (pocType != null)
                {
                    switch (pocType)
                    {
                        case "3":
                        case "4":
                        case "5":
                        case "9":
                            value = float.Parse(todayRuntime) * 100.0f / 24.0f;
                            break;
                        default:
                            break;
                    }
                }

                column.Value = Convert.ToString(Math.Round(value, 0));
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
            string correlationId, object cache = null)
        {
        }

    }
}
