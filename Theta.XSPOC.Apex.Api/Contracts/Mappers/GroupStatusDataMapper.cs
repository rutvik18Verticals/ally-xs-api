using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Common.Converters;
using Theta.XSPOC.Apex.Api.Contracts.Responses;
using Theta.XSPOC.Apex.Api.Contracts.Responses.Values;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using GroupStatusColumn = Theta.XSPOC.Apex.Api.Contracts.Responses.GroupStatusColumn;
using GroupStatusRowColumn = Theta.XSPOC.Apex.Api.Contracts.Responses.GroupStatusRowColumn;

namespace Theta.XSPOC.Apex.Api.Contracts.Mappers
{
    /// <summary>
    /// Mapper for Group Status.
    /// </summary>
    public static class GroupStatusDataMapper
    {
        /// <summary>
        /// Maps the <seealso cref="GroupStatusOutput"/> core model to
        /// <seealso cref="GroupStatusResponse"/> object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="output">The <seealso cref="GroupStatusOutput"/> object</param>
        /// <param name="digits"></param>
        /// <returns>The <seealso cref="GroupStatusResponse"/></returns>
        public static GroupStatusResponse Map(string correlationId, GroupStatusOutput output, int digits)
        {
            var columns = new List<GroupStatusColumn>();
            var rows = new List<IList<GroupStatusRowColumn>>();

            if (output?.Values != null)
            {
                columns.AddRange(output.Values.Columns.Select(item => new GroupStatusColumn
                {
                    Name = PhraseConverter.ConvertFirstToUpperRestToLower(item.Name),
                    Id = item.Id
                }));

                if (output.Values.Rows.Count > 0)
                {
                    rows.AddRange(output.Values.Rows.Select(row => row.Columns.Select(column => new GroupStatusRowColumn
                    {
                        ColumnId = column.ColumnId,
                        Value = float.TryParse(column.Value, out var value) ? MathUtility.RoundToSignificantDigits(value, digits).ToString() : column.Value,
                        BackColor = column.BackColor,
                        ForeColor = column.ForeColor
                    }).ToList()));
                }
            }

            var response = new GroupStatusResponse
            {
                Values = new GroupStatusResponseValues()
                {
                    Rows = rows,
                    Columns = columns
                },
                DateCreated = DateTime.UtcNow,
                Id = correlationId
            };

            return response;
        }

        /// <summary>
        /// Maps the <seealso cref="GroupStatusWidgetOutput"/> core model to
        /// <seealso cref="GroupStatusWidgetResponse"/> object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="output">The <seealso cref="GroupStatusWidgetOutput"/> object</param>
        /// <param name="digits"></param>
        /// <returns>The <seealso cref="GroupStatusWidgetResponse"/></returns>
        public static GroupStatusWidgetResponse MapGroupClassifications(string correlationId, GroupStatusWidgetOutput output, int digits)
        {
            if (output == null)
            {
                return null;
            }
            var response = new GroupStatusWidgetResponse();

            if (output.ClassificationValues != null)
            {
                response.ClassificationValues = output.ClassificationValues.Select(x => new GroupStatusClassificationResponseValues
                {
                    Id = x.Id,
                    Name = PhraseConverter.ConvertFirstToUpperRestToLower(x.Name),
                    Hours = MathUtility.RoundToSignificantDigits(x.Hours, digits),
                    Percent = MathUtility.RoundToSignificantDigits(x.Percent, digits),
                    Count = MathUtility.RoundToSignificantDigits(x.Count, digits),
                    Priority = x.Priority,
                }).OrderBy(x => x.Priority).ToList();
                response.AssetCount = output.AssetCount;
            }
            else
            {
                response.ClassificationValues = new List<GroupStatusClassificationResponseValues>();
            }

            response.Id = correlationId;
            response.DateCreated = DateTime.UtcNow;

            return response;
        }

        /// <summary>
        /// Maps the <seealso cref="GroupStatusDowntimeByWellOutput"/> core model to
        /// <seealso cref="GroupStatusDowntimeByWellResponse"/> object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="output">The <seealso cref="GroupStatusKPIOutput"/> object</param>
        /// <param name="digits">The significant digits used for rounding.</param>
        /// <returns>The <seealso cref="GroupStatusDowntimeByWellResponse"/></returns>
        public static GroupStatusDowntimeByWellResponse Map(string correlationId, GroupStatusDowntimeByWellOutput output, int digits)
        {
            if (output?.Assets == null)
            {
                return null;
            }

            var response = new GroupStatusDowntimeByWellResponse()
            {
                Values = new GroupStatusDowntimeByWellValue()
                {
                    Assets = output.Assets.Select(x => new GroupStatusKPIValue
                    {
                        Id = x.Id.ToString(),
                        Name = x.Name,
                        Percent = MathUtility.RoundToSignificantDigits(x.Percent, digits),
                        Count = MathUtility.RoundToSignificantDigits(x.Count, digits),
                    }).ToList(),
                    GroupByDuration = output.GroupByDuration.Select(x => new GroupStatusKPIValue
                    {
                        Id = x.Id.ToString(),
                        Name = x.Name,
                        Percent = MathUtility.RoundToSignificantDigits(x.Percent, digits),
                        Count = MathUtility.RoundToSignificantDigits(x.Count, digits),
                    }).ToList()
                },
                Id = correlationId,
                DateCreated = DateTime.UtcNow,
            };

            return response;
        }

        /// <summary>
        /// Maps the <seealso cref="GroupStatusKPIOutput"/> core model to
        /// <seealso cref="GroupStatusKPIResponse"/> object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="output">The <seealso cref="GroupStatusKPIOutput"/> object</param>
        /// <param name="digits"></param>
        /// <returns>The <seealso cref="GroupStatusKPIResponse"/></returns>
        public static GroupStatusKPIResponse Map(string correlationId, GroupStatusKPIOutput output, int digits)
        {
            if (output?.Values == null)
            {
                return null;
            }

            var response = new GroupStatusKPIResponse
            {
                Values = output.Values.Select(x => new GroupStatusKPIValue
                {
                    Id = x.Id.ToString(),
                    Name = PhraseConverter.ConvertFirstToUpperRestToLower(x.Name),
                    Percent = MathUtility.RoundToSignificantDigits(x.Percent, digits),
                    Count = MathUtility.RoundToSignificantDigits(x.Count, digits),
                }).ToList(),
                Id = correlationId,
                DateCreated = DateTime.UtcNow
            };

            return response;
        }

    }
}
