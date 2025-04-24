using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter
{
    /// <summary>
    /// Represents a conditional column formatter that applies formatting based on specified conditions.
    /// </summary>
    public class ConditionalColumnFormatter : ColumnFormatterBase, IColumnFormatter
    {

        /// <summary>
        /// Gets the responsibilities of the conditional column formatter.
        /// </summary>
        public IList<string> Responsibilities => new List<string>()
        {
            "CONDITIONAL"
        };

        /// <summary>
        /// Calculates the value for the specified row and column.
        /// </summary>
        /// <param name="dr">The data row.</param>
        /// <param name="column">The column model.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="performCalculation">A flag indicating whether to perform the calculation.</param>
        /// <param name="cache"></param>
        public void CalculateValue(IDictionary<string, object> dr, RowColumnModel column, string correlationId,
            bool performCalculation = true,
            object cache = null)
        {
        }

        /// <summary>
        /// Performs formatting for the specified row, column, and group status column.
        /// </summary>
        /// <param name="dr">The data row.</param>
        /// <param name="column">The column model.</param>
        /// <param name="groupStatusColumn">The group status column.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="cache">The cached data.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="dr"/> is null OR
        /// <paramref name="column"/> is null OR
        /// <paramref name="groupStatusColumn"/> is null.</exception>
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

            string dbValue = column.Value;

            foreach (var conditionalFormat in groupStatusColumn.ConditionalFormats)
            {
                bool conditionMet = false;
                int backColor = conditionalFormat.BackColor;
                int foreColor = conditionalFormat.ForeColor;
                if (float.TryParse(dbValue, out var value))
                {
                    if (conditionalFormat.Operator == ComparisonOperator.Equal)
                    {
                        conditionMet = (value == conditionalFormat.Value);
                    }
                    else if (conditionalFormat.Operator == ComparisonOperator.NotEqual)
                    {
                        conditionMet = (value != conditionalFormat.Value);
                    }
                    else if (conditionalFormat.Operator == ComparisonOperator.GreaterThan)
                    {
                        conditionMet = (value > conditionalFormat.Value);
                    }
                    else if (conditionalFormat.Operator == ComparisonOperator.GreaterThanOrEqual)
                    {
                        conditionMet = (value >= conditionalFormat.Value);
                    }
                    else if (conditionalFormat.Operator == ComparisonOperator.LessThan)
                    {
                        conditionMet = (value < conditionalFormat.Value);
                    }
                    else if (conditionalFormat.Operator == ComparisonOperator.LessThanOrEqual)
                    {
                        conditionMet = (value <= conditionalFormat.Value);
                    }
                    else if (conditionalFormat.Operator == ComparisonOperator.Between)
                    {
                        conditionMet = (value >= conditionalFormat.MinValue && value <= conditionalFormat.MaxValue);
                    }
                    else if (conditionalFormat.Operator == ComparisonOperator.NotBetween)
                    {
                        conditionMet = (value < conditionalFormat.MinValue || value > conditionalFormat.MaxValue);
                    }
                    else if (conditionalFormat.Operator == ComparisonOperator.StringEqual)
                    {
                        conditionMet = (dbValue.ToLower() == conditionalFormat.StringValue.ToLower());
                    }
                    else if (conditionalFormat.Operator == ComparisonOperator.Contains)
                    {
                        conditionMet = (dbValue.ToLower().Contains(conditionalFormat.StringValue.ToLower()));
                    }
                    else if (conditionalFormat.Operator == ComparisonOperator.StartsWith)
                    {
                        conditionMet = (dbValue.ToLower().StartsWith(conditionalFormat.StringValue.ToLower()));
                    }
                    else if (conditionalFormat.Operator == ComparisonOperator.EndsWith)
                    {
                        conditionMet = (dbValue.ToLower().EndsWith(conditionalFormat.StringValue.ToLower()));
                    }
                    else if (conditionalFormat.Operator == ComparisonOperator.DoesNotEqual)
                    {
                        conditionMet = (dbValue.ToLower() != conditionalFormat.StringValue.ToLower());
                    }
                    else if (conditionalFormat.Operator == ComparisonOperator.DoesNotContain)
                    {
                        conditionMet = (!dbValue.ToLower().Contains(conditionalFormat.StringValue.ToLower()));
                    }
                    else if (conditionalFormat.Operator == ComparisonOperator.DoesNotStartWith)
                    {
                        conditionMet = (!dbValue.ToLower().StartsWith(conditionalFormat.StringValue.ToLower()));
                    }
                    else if (conditionalFormat.Operator == ComparisonOperator.DoesNotEndWith)
                    {
                        conditionMet = (!dbValue.ToLower().EndsWith(conditionalFormat.StringValue.ToLower()));
                    }
                }
                else
                {
                    if (conditionalFormat.Operator == ComparisonOperator.StringEqual)
                    {
                        conditionMet = (dbValue.ToLower() == conditionalFormat.StringValue.ToLower());
                    }
                    else if (conditionalFormat.Operator == ComparisonOperator.Contains)
                    {
                        conditionMet = (dbValue.ToLower().Contains(conditionalFormat.StringValue.ToLower()));
                    }
                    else if (conditionalFormat.Operator == ComparisonOperator.StartsWith)
                    {
                        conditionMet = (dbValue.ToLower().StartsWith(conditionalFormat.StringValue.ToLower()));
                    }
                    else if (conditionalFormat.Operator == ComparisonOperator.EndsWith)
                    {
                        conditionMet = (dbValue.ToLower().EndsWith(conditionalFormat.StringValue.ToLower()));
                    }
                    else if (conditionalFormat.Operator == ComparisonOperator.DoesNotEqual)
                    {
                        conditionMet = (dbValue.ToLower() != conditionalFormat.StringValue.ToLower());
                    }
                    else if (conditionalFormat.Operator == ComparisonOperator.DoesNotContain)
                    {
                        conditionMet = (!dbValue.ToLower().Contains(conditionalFormat.StringValue.ToLower()));
                    }
                    else if (conditionalFormat.Operator == ComparisonOperator.DoesNotStartWith)
                    {
                        conditionMet = (!dbValue.ToLower().StartsWith(conditionalFormat.StringValue.ToLower()));
                    }
                    else if (conditionalFormat.Operator == ComparisonOperator.DoesNotEndWith)
                    {
                        conditionMet = (!dbValue.ToLower().EndsWith(conditionalFormat.StringValue.ToLower()));
                    }
                }

                if (!conditionMet)
                {
                    continue;
                }

                column.BackColor = ConvertToHex(backColor);
                column.ForeColor = ConvertToHex(foreColor);

                break;
            }
        }

    }
}
