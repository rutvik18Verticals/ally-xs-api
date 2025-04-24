using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter
{
    /// <summary>
    /// Represents a column formatter for pumping unit manufacturer values.
    /// </summary>
    public class PumpingUnitManufacturerColumnFormatter : ColumnFormatterBase, IColumnFormatter
    {

        #region Private Fields

        private readonly IPumpingUnitManufacturer _pumpingUnitManufacturerStore;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PumpingUnitManufacturerColumnFormatter"/> class.
        /// </summary>
        /// <param name="pumpingUnitManufacturerStore">The pumping unit manufacturer store.</param>
        /// <exception cref="ArgumentNullException"><paramref name="pumpingUnitManufacturerStore"/> is null.</exception>
        public PumpingUnitManufacturerColumnFormatter(IPumpingUnitManufacturer pumpingUnitManufacturerStore)
        {
            _pumpingUnitManufacturerStore = pumpingUnitManufacturerStore ??
                throw new ArgumentNullException(nameof(pumpingUnitManufacturerStore));
        }

        #endregion

        /// <summary>
        /// Gets the responsibilities of the column formatter.
        /// </summary>
        public IList<string> Responsibilities => new List<string>()
        {
            "PUMPING UNIT MANUFACTURER",
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

            var wellKey = GetKey(dr, "Well");

            IList<PumpingUnitManufacturerForGroupStatus> result = cache as IList<PumpingUnitManufacturerForGroupStatus> ??
                _pumpingUnitManufacturerStore.GetManufacturers(new List<string>()
                {
                    dr[wellKey].ToString(),
                }, correlationId);

            var pumpingUnitManufacturerKey = GetKey(dr, "Pumping Unit Manufacturer");
            var unitId = dr[pumpingUnitManufacturerKey].ToString();

            var value = result?.FirstOrDefault(x => x.UnitId == unitId);

            column.Value = value?.Manufacturer ?? string.Empty;
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
            if (column == null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            // Use Default Colors in UI
        }

    }
}
