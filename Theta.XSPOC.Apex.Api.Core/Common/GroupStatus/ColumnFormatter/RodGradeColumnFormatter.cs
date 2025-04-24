using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter
{
    /// <summary>
    /// Represents a column formatter for rod grade.
    /// </summary>
    public class RodGradeColumnFormatter : ColumnFormatterBase, IColumnFormatter
    {

        #region Private Fields

        private readonly IRod _rodStore;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RodGradeColumnFormatter"/> class.
        /// </summary>
        /// <param name="rodStore">The rod store.</param>
        /// <exception cref="ArgumentNullException"><paramref name="rodStore"/> is null.</exception>
        public RodGradeColumnFormatter(IRod rodStore)
        {
            _rodStore = rodStore ?? throw new ArgumentNullException(nameof(rodStore));
        }

        #endregion

        /// <summary>
        /// Gets the responsibilities of the column formatter.
        /// </summary>
        public IList<string> Responsibilities => new[]
        {
            "ROD GRADE"
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
            var value = GetRods(dr[wellKey].ToString(), cache, correlationId);

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
        }

        #region Private Methods

        private string GetRods(string nodeId, object cache, string correlationId)
        {
            if (cache is IList<RodForGroupStatusModel> result)
            {
                result = result.Where(x => x.NodeId == nodeId).ToList();
            }
            else
            {
                result = _rodStore.GetRodForGroupStatus(new List<string>()
                {
                    nodeId
                }, correlationId);
            }

            return result?.Count > 0 ? string.Join(",", result.Select(x => x.Name)) : string.Empty;
        }

        #endregion

    }
}
