using System;
using System.Collections.Generic;
using System.Drawing;

namespace Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter
{
    /// <summary>
    /// Represents a column formatter for parameter.
    /// </summary>
    public class ParameterColumnFormatter : ColumnFormatterBase, IColumnFormatter
    {

        /// <summary>
        /// Gets the responsibilities of the column formatter.
        /// </summary>
        public IList<string> Responsibilities => new[]
        {
            "PARAMETER"
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

            if (groupStatusColumn == null)
            {
                throw new ArgumentNullException(nameof(groupStatusColumn));
            }

            if (groupStatusColumn.FieldHeading == "Description")
            {
                var desBackColorKey = GetKey(dr, "BackColor");
                var desBackColor = dr[desBackColorKey].ToString();
                var desForeColorKey = GetKey(dr, "ForeColor");
                var desForeColor = dr[desForeColorKey].ToString();

                if (!string.IsNullOrEmpty(desBackColor))
                {
                    column.BackColor = ConvertToHex(Color.Yellow);
                }

                if (!string.IsNullOrEmpty(desForeColor))
                {
                    var color = Color.FromName(desForeColor);

                    if (color.IsKnownColor)
                    {
                        column.ForeColor = ConvertToHex(color);
                    } // if (color.IsKnownColor)
                } // if (!string.IsNullOrEmpty(desForeColor))
            } // if (groupStatusColumn.FieldHeading == "Description")
            else
            {
                string columnName = GetKey(dr, groupStatusColumn.FieldHeading);
                var backColorKey = GetKey(dr, $"{columnName}.BackColor");
                var backColor = dr[backColorKey].ToString();
                var foreColorKey = GetKey(dr, key: columnName + ".ForeColor");
                var foreColor = dr[foreColorKey].ToString();

                if (!string.IsNullOrEmpty(backColor))
                {
                    var color = Color.FromName(backColor);

                    if (color.IsKnownColor)
                    {
                        column.BackColor = ConvertToHex(color);
                    }
                }

                if (!string.IsNullOrEmpty(foreColor))
                {
                    var color = Color.FromName(foreColor);

                    if (color.IsKnownColor)
                    {
                        column.ForeColor = ConvertToHex(color);
                    } // if (color.IsKnownColor)
                } // if (!string.IsNullOrEmpty(foreColor))
            } // else
        }

    }
}
