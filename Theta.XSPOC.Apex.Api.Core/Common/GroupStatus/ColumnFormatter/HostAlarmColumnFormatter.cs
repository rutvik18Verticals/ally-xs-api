using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter
{
    /// <summary>
    /// Represents a column formatter for host alarm values.
    /// </summary>
    public class HostAlarmColumnFormatter : ColumnFormatterBase, IColumnFormatter
    {

        #region Private Fields

        private readonly IHostAlarm _hostAlarmStore;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="HostAlarmColumnFormatter"/> class.
        /// </summary>
        /// <param name="hostAlarmStore">The host alarm store.</param>
        /// <exception cref="ArgumentNullException"><paramref name="hostAlarmStore"/> is null.</exception>
        public HostAlarmColumnFormatter(IHostAlarm hostAlarmStore)
        {
            _hostAlarmStore = hostAlarmStore ?? throw new ArgumentNullException(nameof(hostAlarmStore));
        }

        /// <summary>
        /// Gets the responsibilities of the column formatter.
        /// </summary>
        public IList<string> Responsibilities => new[]
        {
            "HOSTALARMS",
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

            var wellKey = GetKey(dr, "Well");

            if (cache is IList<HostAlarmForGroupStatus> result)
            {
                result = result.Where(x => x.NodeId == dr[wellKey].ToString()).ToList();
            }
            else
            {
                result = _hostAlarmStore.GetAllGroupStatusHostAlarms(new List<string>()
                {
                    dr[wellKey].ToString(),
                }, correlationId);
            }

            var enabled = IsEnabled(dr);
            var value = GetHostAlarmDescription(result.FirstOrDefault());

            if (string.IsNullOrEmpty(value))
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

            var wellKey = GetKey(dr, "Well");

            if (cache is IList<HostAlarmForGroupStatus> result)
            {
                result = result.Where(x => x.NodeId == dr[wellKey].ToString()).ToList();
            }
            else
            {
                result = _hostAlarmStore.GetAllGroupStatusHostAlarms(new List<string>()
                {
                    dr[wellKey].ToString(),
                }, correlationId);
            }

            var value = result.FirstOrDefault()?.AlarmState;

            if (!value.HasValue)
            {
                return;
            }

            string backColor = null;
            string foreColor = null;

            HostAlarmColorSelection(value.Value, ref backColor, ref foreColor);

            column.BackColor = backColor;
            column.ForeColor = foreColor;
        }

        #region Private Methods

        private string GetHostAlarmDescription(HostAlarmForGroupStatus hostAlarm)
        {
            return (hostAlarm?.AlarmState) switch
            {
                1 => $"{hostAlarm?.AlarmDescription}-Hi",
                2 => $"{hostAlarm?.AlarmDescription}-HiHi",
                3 => $"{hostAlarm?.AlarmDescription}-Lo",
                4 => $"{hostAlarm?.AlarmDescription}-LoLo",
                5 when hostAlarm?.AlarmType == 4 => $"{hostAlarm?.AlarmDescription}-ROC: +{hostAlarm?.PercentChange}%",
                5 when hostAlarm?.AlarmType == 5 => $"{hostAlarm?.AlarmDescription}-ROC: -{hostAlarm?.PercentChange}%",
                6 => $"{hostAlarm?.AlarmDescription}-MaxSpan: {hostAlarm?.MinToMaxLimit}",
                7 => $"{hostAlarm?.AlarmDescription}-NearPumpoff",
                8 => $"{hostAlarm?.AlarmDescription}-Val Chg: {(hostAlarm?.AlarmType == 9 ? "-" : "+")}{hostAlarm?.ValueChange}",
                10 => $"{hostAlarm?.AlarmDescription}-MinSpan: {hostAlarm?.MinToMaxLimit}",
                _ => hostAlarm?.AlarmState >= 101 && hostAlarm?.AlarmState <= 200
                    ? $"{hostAlarm?.AlarmDescription} (Acknowledged)"
                    : hostAlarm?.AlarmDescription,
            };
        }

        private void HostAlarmColorSelection(int state, ref string backColor, ref string foreColor)
        {
            switch (state)
            {
                case 0:
                    // Use Default Colors in UI

                    break;
                case 1:
                case 3:
                    backColor = ConvertToHex(Color.Pink);
                    foreColor = ConvertToHex(Color.Black);

                    break;
                case var _ when (state >= 2 && state <= 100):
                    backColor = ConvertToHex(Color.Red);
                    foreColor = ConvertToHex(Color.White);

                    break;
                case var _ when (state >= 101 && state <= 200):
                    backColor = ConvertToHex(Color.Yellow);
                    foreColor = ConvertToHex(Color.Black);

                    break;
                case var _ when (state >= 201 && state <= 300):
                    backColor = ConvertToHex(Color.Cyan);
                    foreColor = ConvertToHex(Color.Black);

                    break;
                default:
                    backColor = ConvertToHex(Color.Red);
                    foreColor = ConvertToHex(Color.White);

                    break;
            }
        }

        #endregion

    }
}
