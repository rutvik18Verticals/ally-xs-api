using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter
{
    /// <summary>
    /// Represents a column formatter for exception values.
    /// </summary>
    public class ExceptionColumnFormatter : ColumnFormatterBase, IColumnFormatter
    {

        private readonly IException _exceptionStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionColumnFormatter"/> class.
        /// </summary>
        /// <param name="exceptionStore">The exception store.</param>
        /// <exception cref="ArgumentNullException"><paramref name="exceptionStore"/> is null.</exception>
        public ExceptionColumnFormatter(IException exceptionStore)
        {
            _exceptionStore = exceptionStore ?? throw new ArgumentNullException(nameof(exceptionStore));
        }

        /// <summary>
        /// Gets the responsibilities of the column formatter.
        /// </summary>
        public IList<string> Responsibilities => new[]
        {
            "EXCEPTIONGROUPNAME"
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

            var wellKey = GetKey(dr, "Well");

            if (cache is IList<ExceptionModel> result)
            {
                result = result.Where(x => x.NodeId == dr[wellKey].ToString()).ToList();
            }
            else
            {
                result = _exceptionStore.GetExceptions(new List<string>
                {
                    dr[wellKey].ToString()
                }, correlationId);
            }

            string value = null;

            if (result != null)
            {
                value = result.FirstOrDefault()?.ExceptionGroupName;
            }

            var enabled = IsEnabled(dr);

            if (!string.IsNullOrEmpty(value) && value.Length <= 2)
            {
                value = enabled ? "-" : "";
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

            var wellKey = GetKey(dr, "Well");

            if (cache is IList<ExceptionModel> result)
            {
                result = result.Where(x => x.NodeId == dr[wellKey].ToString()).ToList();
            }
            else
            {
                result = _exceptionStore.GetExceptions(new List<string>
                {
                    dr[wellKey].ToString()
                }, correlationId);
            }

            // Use Default Colors in UI

            if (result?.FirstOrDefault()?.Priority == null)
            {
                return;
            }

            if (result?.FirstOrDefault()?.Priority > 100)
            {
                column.BackColor = ConvertToHex(Color.Red);
                column.ForeColor = ConvertToHex(Color.White);
            }
            else
            {
                column.BackColor = ConvertToHex(Color.Yellow);
                column.ForeColor = ConvertToHex(Color.Black);
            }
        }

    }
}
