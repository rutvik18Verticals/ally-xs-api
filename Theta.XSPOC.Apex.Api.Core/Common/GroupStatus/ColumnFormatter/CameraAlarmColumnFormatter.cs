using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Drawing;
using Theta.XSPOC.Apex.Api.Core.Models;
using Theta.XSPOC.Apex.Api.Data;

namespace Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter
{
    /// <summary>
    /// Represents a column formatter for camera alarm.
    /// </summary>
    public class CameraAlarmColumnFormatter : ColumnFormatterBase, IColumnFormatter
    {

        #region Private Enums

        private enum PhraseIds
        {

            Alarm = 149, // Alarm
            OK = 611, // OK

        }

        #endregion

        #region Private Fields

        private readonly ILocalePhrases _phraseStore;
        private readonly IMemoryCache _memoryCache;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraAlarmColumnFormatter"/> class.
        /// </summary>
        /// <param name="phraseStore">The phrase store.</param>
        /// <param name="memoryCache"></param>
        /// <exception cref="ArgumentNullException"><paramref name="phraseStore"/> is null.</exception>
        public CameraAlarmColumnFormatter(ILocalePhrases phraseStore, IMemoryCache memoryCache)
        {
            _phraseStore = phraseStore ?? throw new ArgumentNullException(nameof(phraseStore));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        #endregion

        /// <summary>
        /// Gets the responsibilities of the column formatter.
        /// </summary>
        public IList<string> Responsibilities => new[]
        {
            "CAMERAALARMS"
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

            var alarmKey = GetKey(dr, "CameraAlarms");
            var value = GetAlarmCount(dr[alarmKey]);

            if (value > 0)
            {
                if (_memoryCache.TryGetValue($"{MemoryCacheValueType.GroupStatusCameraAlarmColumnFormatter}::{PhraseIds.Alarm}",
                        out var cacheValue))
                {
                    column.Value = cacheValue?.ToString();
                }
                else
                {
                    column.Value = _phraseStore.Get((int)PhraseIds.Alarm, correlationId);
                    _memoryCache.Set($"{MemoryCacheValueType.GroupStatusCameraAlarmColumnFormatter}::{PhraseIds.Alarm}",
                        column.Value);
                }
            }
            else
            {
                if (_memoryCache.TryGetValue($"{MemoryCacheValueType.GroupStatusCameraAlarmColumnFormatter}::{PhraseIds.OK}",
                        out var cacheValue))
                {
                    column.Value = cacheValue?.ToString();
                }
                else
                {
                    column.Value = _phraseStore.Get((int)PhraseIds.OK, correlationId);
                    _memoryCache.Set($"{MemoryCacheValueType.GroupStatusCameraAlarmColumnFormatter}::{PhraseIds.OK}",
                        column.Value);
                }
            }
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
            if (column == null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            string phrase;

            if (_memoryCache.TryGetValue($"{MemoryCacheValueType.GroupStatusCameraAlarmColumnFormatter}::{PhraseIds.Alarm}",
                    out var cacheValue))
            {
                phrase = cacheValue?.ToString();
            }
            else
            {
                phrase = _phraseStore.Get((int)PhraseIds.Alarm, correlationId);
                _memoryCache.Set($"{MemoryCacheValueType.GroupStatusCameraAlarmColumnFormatter}::{PhraseIds.Alarm}",
                    phrase, TimeSpan.FromHours(24));
            }

            if (column.Value != phrase)
            {
                return;
            }

            column.BackColor = ConvertToHex(Color.Red);
            column.ForeColor = ConvertToHex(Color.White);
        }

        #region Private Methods

        private int GetAlarmCount(object value)
        {
            int result = 0;

            if (value != DBNull.Value)
            {
                result = Convert.ToInt32(value);
            }

            return result;
        }

        #endregion

    }
}
