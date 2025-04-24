using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Contracts.Responses;
using Theta.XSPOC.Apex.Api.Contracts.Responses.Values;
using Theta.XSPOC.Apex.Api.Core.Common;
using DataPoint = Theta.XSPOC.Apex.Api.Contracts.Responses.Values.DataPoint;

namespace Theta.XSPOC.Apex.Api.Contracts.Mappers
{
    /// <summary>
    /// Maps <seealso cref="Core.Models.Outputs.DataHistoryListOutput"/> to DataHistoryResponse object.
    /// </summary>
    public static class DataHistoryMapper
    {

        /// <summary>
        /// Maps the <seealso cref="Core.Models.Outputs.DataHistoryListOutput"/> core model to
        /// <seealso cref="DataHistoryResponse"/> object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="coreModel">The <seealso cref="Core.Models.Outputs.DataHistoryListOutput"/> object.</param>
        /// <returns>The <seealso cref="DataHistoryResponse"/></returns>
        public static DataHistoryResponse Map(string correlationId, Core.Models.Outputs.DataHistoryListOutput coreModel)
        {
            if (coreModel == null || coreModel.Values == null || coreModel.Values.Count == 0)
            {
                return null;
            }

            var responseValues = new List<DataHistoryListItem>();

            foreach (var value in coreModel.Values.AsEnumerable())
            {
                responseValues.Add(new DataHistoryListItem
                {
                    Id = value.Id,
                    Name = value.Name,
                    TypeId = value.TypeId,
                    Items = value.Items,
                });
            }

            return new DataHistoryResponse()
            {
                DateCreated = DateTime.UtcNow,
                Id = correlationId,
                Values = responseValues
            };
        }

        /// <summary>
        /// Maps the <seealso cref="Core.Models.Outputs.DataHistoryTrendItemsOutput"/> core model to
        /// <seealso cref="List{DataHistoryItemsResponse}"/> object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="coreModel">The <seealso cref="Core.Models.Outputs.DataHistoryTrendItemsOutput"/> object.</param>
        /// <returns>The mapped <seealso cref="List{DataHistoryItemsResponse}"/> object.</returns>
        public static DataHistoryTrendResponse Map(string correlationId, Core.Models.Outputs.DataHistoryTrendItemsOutput coreModel)
        {
            if (coreModel == null || coreModel.Values == null)
            {
                return null;
            }

            var responseValues = new List<DataHistoryItemsResponse>();

            var groupedResponse = coreModel.Values
                .GroupBy(x => x.TrendName, (trendName, values)
                => new DataHistoryItemsResponse()
                {
                    TrendName = trendName,
                    Values = values.Select(v => new DataPoint()
                    {
                        X = v.X,
                        Y = v.Y,
                        Note = v.Note,
                    }).ToList(),
                });

            foreach (var group in groupedResponse)
            {
                responseValues.Add(new DataHistoryItemsResponse()
                {
                    TrendName = group.TrendName.ToString(),
                    Values = group.Values.Select(v => new DataPoint()
                    {
                        X = v.X,
                        Y = v.Y,
                        Note = v.Note,
                    }).ToList()
                });
            }

            return new DataHistoryTrendResponse()
            {
                DateCreated = DateTime.Now,
                Id = correlationId,
                TrendsData = responseValues
            };
        }

        /// <summary>
        /// Maps the <seealso cref="Core.Models.Outputs.DataHistoryTrendOutput"/> core model to
        /// <seealso cref="DataHistoryResponse"/> object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="coreModel">The <seealso cref="Core.Models.Outputs.DataHistoryTrendOutput"/> object.</param>
        /// <returns>The <seealso cref="DataHistoryResponse"/></returns>
        public static DataHistoryTrendsResponse Map(string correlationId, Core.Models.Outputs.DataHistoryTrendOutput coreModel)
        {
            if (coreModel == null)
            {
                return null;
            }

            DataHistoryTrends responseValues = new DataHistoryTrends()
            {
                Chart1 = new List<TrendsData>(),
                Chart2 = new List<TrendsData>(),
                Chart3 = new List<TrendsData>(),
                Chart4 = new List<TrendsData>(),
            };

            if (coreModel.Values != null)
            {
                responseValues = new DataHistoryTrends
                {
                    Chart1 = MapGraphData(coreModel.Values.Where(x => x.AxisIndex == 0).ToList()),
                    Chart2 = MapGraphData(coreModel.Values.Where(x => x.AxisIndex == 1).ToList()),
                    Chart3 = MapGraphData(coreModel.Values.Where(x => x.AxisIndex == 2).ToList()),
                    Chart4 = MapGraphData(coreModel.Values.Where(x => x.AxisIndex == 3).ToList()),
                };
            }

            return new DataHistoryTrendsResponse()
            {
                DateCreated = DateTime.UtcNow,
                Id = correlationId,
                Values = responseValues
            };
        }

        /// <summary>
        /// Maps the <seealso cref="Core.Models.Outputs.DataHistoryDefaultTrendOutput"/> core model to
        /// <seealso cref="DataHistoryResponse"/> object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="coreModel">The <seealso cref="Core.Models.Outputs.DataHistoryDefaultTrendOutput"/> object.</param>
        /// <returns>The <seealso cref="DataHistoryDefaultTrendsResponse"/></returns>
        public static DataHistoryDefaultTrendsResponse Map(string correlationId,
            Core.Models.Outputs.DataHistoryDefaultTrendOutput coreModel)
        {
            if (coreModel == null || coreModel.Values == null)
            {
                return null;
            }

            var responseValues = coreModel.Values
                .Select(x => new DataHistoryDefaultTrends
                {
                    ViewId = x.ViewId,
                    ViewName = x.ViewName,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    IsSelected = x.IsSelected,
                    IsGlobal = x.IsGlobal,
                }).ToList();

            return new DataHistoryDefaultTrendsResponse()
            {
                DateCreated = DateTime.UtcNow,
                Id = correlationId,
                Values = responseValues
            };
        }

        private static List<TrendsData> MapGraphData(List<Core.Models.Outputs.GraphViewTrendsData> chartData)
        {
            var trendData = new List<TrendsData>();
            foreach (var cData in chartData)
            {
                trendData.Add(new TrendsData()
                {
                    AxisLabel = cData.AxisLabel,
                    ItemId = cData.ItemId,
                    AxisValues = cData.AxisValues.Select(a => new DataPoint
                    {
                        X = a.X,
                        Y = a.Y,
                        Note = a.Note,
                    }).ToList(),
                    Address = cData.Address,
                });
            }

            return trendData;
        }

        /// <summary>
        /// Maps the <seealso cref="Core.Models.Outputs.TimeSeriesOutput"/> core model to
        /// <seealso cref="TimeSeriesTrendDataResponse"/> object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="coreModel">The <seealso cref="Core.Models.Outputs.TimeSeriesTrendDataOutput"/> object.</param>
        /// <returns>The <seealso cref="DataHistoryDefaultTrendsResponse"/></returns>
        public static AllyTimeSeriesTrendDataResponse Map(string correlationId,
            Core.Models.Outputs.TimeSeriesOutput coreModel)
        {
            if (coreModel == null || coreModel.Values == null || coreModel.Values.Count == 0)
            {
                return null;
            }

            var responseValues = coreModel.Values
                .Select(x => new AllyTimeSeriesTrendDataResponseValues
                {
                    AssetId = x.AssetID,                   
                    TagId = x.TagId,                   
                    Value = x.Value,                   
                    Timestamp = x.Timestamp,
                }).ToList();

            return new AllyTimeSeriesTrendDataResponse()
            {
                TotalRecords = coreModel.Values[0].TotalRecords,
                TotalPages = coreModel.Values[0].TotalPages,
                DateCreated = DateTime.Parse(DateTime.UtcNow.ToString()).ToUniversalTime(),
                Id = correlationId,
                Values = responseValues
            };
        }
    }
}
