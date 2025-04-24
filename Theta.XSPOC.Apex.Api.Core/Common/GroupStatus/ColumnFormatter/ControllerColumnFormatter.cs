using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter
{
    /// <summary>
    /// Represents a column formatter for the Controller column.
    /// </summary>
    public class ControllerColumnFormatter : ColumnFormatterBase, IColumnFormatter
    {

        private readonly IPocType _pocTypeStore;

        /// <summary>
        /// Gets the responsibilities of the column formatter.
        /// </summary>
        public IList<string> Responsibilities { get; } = new string[]
        {
            "CONTROLLER", "POCTYPE", "POC TYPE",
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerColumnFormatter"/> class.
        /// </summary>
        /// <param name="pocTypeStore">The instance of <see cref="IPocType"/> used for retrieving POC type information.</param>
        /// <exception cref="ArgumentNullException"><paramref name="pocTypeStore"/> is null.</exception>
        public ControllerColumnFormatter(IPocType pocTypeStore)
        {
            _pocTypeStore = pocTypeStore ?? throw new ArgumentNullException(nameof(pocTypeStore));
        }

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

            dr["Controller"] = string.Empty;

            var key =
                dr.FirstOrDefault(x => x.Key.Equals("tblNodeMaster.POCTYPE", StringComparison.OrdinalIgnoreCase)).Key;

            var pocType = dr[key];

            if (pocType == null || !int.TryParse(pocType.ToString(), out var pocTypeInt))
            {
                return;
            }

            if (pocTypeInt > 0)
            {
                if (cache is IList<PocTypeModel> result)
                {
                    column.Value = result.FirstOrDefault(x => x.PocType == pocTypeInt)?.Description;
                }
                else
                {
                    column.Value = _pocTypeStore.Get(pocTypeInt, correlationId)?.Description;
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
            string correlationId, object cache = null)
        {
            if (dr == null)
            {
                throw new ArgumentNullException(nameof(dr));
            }

            if (column == null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            // Use Default Colors in UI
        }

    }
}
