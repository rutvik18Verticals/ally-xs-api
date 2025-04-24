using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Models;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter
{
    /// <summary>
    /// Represents a column formatter for the pumping unit column.
    /// </summary>
    public class StringIdColumnFormatter : ColumnFormatterBase, IColumnFormatter
    {

        #region Private Fields

        private readonly IStringIdStore _stringIdStore;
        private readonly IMemoryCache _memoryCache;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StringIdColumnFormatter"/> class.
        /// </summary>
        /// <param name="stringIdStore">The string Id Store.</param>
        /// <param name="memoryCache"></param>
        /// <exception cref="ArgumentNullException"><paramref name="stringIdStore"/> is null.</exception>
        public StringIdColumnFormatter(IStringIdStore stringIdStore, IMemoryCache memoryCache)
        {
            _stringIdStore = stringIdStore ?? throw new ArgumentNullException(nameof(stringIdStore));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        #endregion

        /// <summary>
        /// Gets the responsibilities of the column formatter.
        /// </summary>
        public IList<string> Responsibilities => new[]
        {
            "STRINGID",
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

            var stringIdIntKey = GetKey(dr, "StringIDInt");
            var stringIdInt = dr[stringIdIntKey].ToString();

            IList<StringIdModel> result = new List<StringIdModel>();

            if (_memoryCache.TryGetValue($"{MemoryCacheValueType.GroupStatusStringIdColumnFormatter}", out var cacheValue))
            {
                result = cacheValue as List<StringIdModel>;
            }
            else
            {
                result = _stringIdStore.GetStringId(correlationId);

                _memoryCache.Set($"{MemoryCacheValueType.GroupStatusStringIdColumnFormatter}", result, TimeSpan.FromHours(24));
            }

            if (stringIdInt != null && result != null)
            {
                column.Value = result.FirstOrDefault(x => x.StringId.ToString() == stringIdInt)?.StringName ?? string.Empty;
            }
            else
            {
                column.Value = "";
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
        }

    }
}
