using InfluxDB.Client.Writes;
using MathNet.Numerics;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Common.WorkflowModels;
using Theta.XSPOC.Apex.Api.Data.Entity.Logging;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using Theta.XSPOC.Apex.Kernel.Quantity.Measures;
using Theta.XSPOC.Apex.Kernel.Utilities;

namespace Theta.XSPOC.Apex.Api.Data.Influx.Services
{
    /// <summary>
    /// This implementation represents fetching trend data from OSS Influx Data
    /// </summary>
    public class OSSInfluxDataStore : IDataHistoryTrendData
    {

        #region Private Fields

        private readonly IOSSInfluxClientFactory _influxDbClient;
        private readonly IDateTimeConverter _dateTimeConverter;
        private string _bucketName;
        private string _org;
        private string _measurement;
        private string _pageSize;

        #endregion

        #region Constants

        private const string BUCKET_NAME = "xspoc";
        private const string ORGANIZATION = "ChampionX";
        private const string MEASUREMENT = "XSPOCData";
        private const double DEFAULT_WINDOW_SIZE = 750;
        private const string DEFAULT_PAGE_SIZE = "50000";

        #endregion

        #region Protected Fields

        /// <summary>
        /// The <seealso cref="IConfiguration"/> configurations.
        /// </summary>
        protected IConfiguration AppConfig { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="OSSInfluxDataStore"/> using the provided configuration.
        /// </summary>
        /// <param name="ossInfluxDbClient">The <seealso cref="IOSSInfluxClientFactory"/>.</param>       
        /// <param name="appConfig">The <seealso cref="IConfiguration"/>.</param>  
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ossInfluxDbClient"/> is null.
        /// <param name="dateTimeConverter">The <seealso cref="IDateTimeConverter"/>.</param>  
        /// </exception> 
        public OSSInfluxDataStore(IOSSInfluxClientFactory ossInfluxDbClient, IConfiguration appConfig, IDateTimeConverter dateTimeConverter)
        {
            _influxDbClient = ossInfluxDbClient ?? throw new ArgumentNullException(nameof(ossInfluxDbClient));
            _dateTimeConverter = dateTimeConverter ?? throw new ArgumentNullException(nameof(dateTimeConverter));
            AppConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            SetConfigurationOnInit();
        }

        #endregion

        #region IDataHistoryTrendData Implementation

        /// <summary>
        /// Method to read the trend data from influx data store with <paramref name="channelIds"/>. 
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="customerId">The customer guid.</param>
        /// <param name="pocType">The poc type.</param>
        /// <param name="channelIds">The list of channel ids.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>The <seealso cref="IList{DataPointModel}"/></returns>
        public async Task<IList<DataPointModel>> GetTrendData(Guid assetId, Guid customerId, string pocType,
           List<string> channelIds, string startDate, string endDate)
        {
            var responseDataPoints = new List<DataPointModel>();

            if (assetId == Guid.Empty || customerId == Guid.Empty ||
                string.IsNullOrEmpty(pocType) || channelIds == null || channelIds.Count == 0)
            {
                return null;
            }

            DbStoreResult result;

            try
            {
                using (var client = _influxDbClient.Create())
                {
                    var startDateTime = DateTime.Parse(startDate);
                    var endDateTime = DateTime.Parse(endDate);

                    var timeRangeInSeconds = (endDateTime - startDateTime).TotalSeconds;
                    var windowSize = timeRangeInSeconds / DEFAULT_WINDOW_SIZE;

                    var queryBuilder = new StringBuilder();
                    queryBuilder.Append($"from(bucket: \"{_bucketName}\")");
                    queryBuilder.Append($" |> range(start: {startDate}Z, stop: {endDate}Z)");
                    queryBuilder.Append($" |> filter(fn: (r) => r[\"_measurement\"] == \"{_measurement}\")");
                    queryBuilder.Append($" |> filter(fn: (r) => r[\"AssetID\"] == \"{assetId}\")");
                    if (customerId != Guid.Empty)
                    {
                        queryBuilder.Append($" |> filter(fn: (r) => r[\"CustomerID\"] == \"{customerId}\")");
                    }
                    if (pocType != string.Empty)
                    {
                        queryBuilder.Append($" |> filter(fn: (r) => r[\"POCType\"] == \"{pocType}\")");
                    }

                    if (channelIds != null && channelIds.Count > 0)
                    {
                        if (channelIds.Count > 1)
                        {
                            var multiAddrQuery = new StringBuilder();
                            foreach (var channel in channelIds)
                            {
                                if (!string.IsNullOrEmpty(channel))
                                {
                                    multiAddrQuery.Append($"r[\"_field\"] == \"{channel}\" or ");
                                }
                            }
                            var mQuery = multiAddrQuery.ToString()[..^4];
                            queryBuilder.Append($" |> filter(fn: (r) => {mQuery})");
                        }
                        else
                        {
                            queryBuilder.Append($" |> filter(fn: (r) => r[\"_field\"] == \"{channelIds[0]}\")");
                        }
                    }

                    var integerWindowSize = windowSize < 1 ? 1 : windowSize.Round(0);

                    queryBuilder.Append($" |> aggregateWindow(every: {integerWindowSize}s, fn: mean)");
                    queryBuilder.Append($"|> keep(columns: [\"_time\", \"_value\", \"_field\"])");

                    var query = queryBuilder.ToString();

                    var rawResult = await client.GetQueryApi().QueryRawAsync(query, new InfluxDB.Client.Api.Domain.Dialect(), _org);

                    var lines = rawResult.Split('\n');
                    foreach (var line in lines.Skip(1)) // Skipping header line
                    {
                        var values = line.Split(',');
                        if (values.Length == 6)
                        {
                            var tryDateTime = DateTime.TryParse(values[3], null, DateTimeStyles.AdjustToUniversal, out var dateResult);
                            if (tryDateTime == false)
                            {
                                continue;
                            }

                            responseDataPoints.Add(new DataPointModel
                            {
                                Time = dateResult,
                                Value = double.TryParse(values[4], out var value) ? value : null,
                                TrendName = values[5]
                            });
                        }
                    } // foreach line in lines
                } // using client
            } // try block
            catch (InfluxDB.Client.Core.Exceptions.RequestTimeoutException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.LikelyRecoverable,
                };
            } // catch block of RequestTimeoutException
            catch (InfluxDB.Client.Core.Exceptions.TooManyRequestsException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.LikelyRecoverable,
                };
            } // catch block of TooManyRequestsException
            catch (InfluxDB.Client.Core.Exceptions.InternalServerErrorException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of InternalServerErrorException
            catch (InfluxDB.Client.Core.Exceptions.BadRequestException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of BadRequestException
            catch (InfluxDB.Client.Core.Exceptions.InfluxException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of InfluxException
            catch (Exception ex)
            {
                _ = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block

            return responseDataPoints;
        }

        /// <summary>
        /// Method to read the trend data from influx data store with <paramref name="channelIds"/>. 
        /// </summary>
        /// <param name="assetIds">The asset guid.</param>
        /// <param name="customerId">The customer guid.</param>      
        /// <param name="channelIds">The list of channel ids.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="downsampleType">The down sample Type.</param>
        /// <param name="downsampleWindowSize">The down sample Window Size.</param>
        /// <param name="pageNum">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The <seealso cref="IList{DataPointModel}"/></returns>
        public async Task<IList<DataPointModel>> GetAllyTimeSeriesTrendData(List<Guid> assetIds, Guid customerId,
           List<string> channelIds, string startDate, string endDate, string downsampleType, string downsampleWindowSize,
           int pageNum, int pageSize)
        {
            var responseDataPoints = new List<DataPointModel>();
            int pageLimit = pageSize != 0 ? pageSize : Convert.ToInt32(_pageSize);

            if (assetIds == null || assetIds.Count == 0 || assetIds[0] == Guid.Empty || customerId == Guid.Empty
                || channelIds == null || channelIds.Count == 0)
            {
                return null;
            }

            DbStoreResult result;

            try
            {
                using (var client = _influxDbClient.Create())
                {
                    var startDateTime = DateTime.Parse(startDate);
                    var endDateTime = DateTime.Parse(endDate);

                    var queryBuilder = new StringBuilder();
                    queryBuilder.Append($"from(bucket: \"{_bucketName}\")");
                    queryBuilder.Append($" |> range(start: {startDate}, stop: {endDate})");
                    queryBuilder.Append($" |> filter(fn: (r) => r[\"_measurement\"] == \"{_measurement}\")");

                    foreach (var item in assetIds)
                    {
                        if (item == assetIds.First())
                        {
                            queryBuilder.Append($" |> filter(fn: (r) => r[\"AssetID\"] == \"{item}\"");
                        }
                        else
                        {
                            queryBuilder.Append($" or r[\"AssetID\"] == \"{item}\"");
                        }

                        if (item == assetIds.Last())
                        {
                            queryBuilder.Append(')');
                        }
                    }

                    if (customerId != Guid.Empty)
                    {
                        queryBuilder.Append($" |> filter(fn: (r) => r[\"CustomerID\"] == \"{customerId}\")");
                    }

                    if (channelIds != null && channelIds.Count > 0)
                    {
                        if (channelIds.Count > 1)
                        {
                            var multiAddrQuery = new StringBuilder();
                            foreach (var channel in channelIds)
                            {
                                if (!string.IsNullOrEmpty(channel))
                                {
                                    multiAddrQuery.Append($"r[\"_field\"] == \"{channel}\" or ");
                                }
                            }
                            var mQuery = multiAddrQuery.ToString()[..^4];
                            queryBuilder.Append($" |> filter(fn: (r) => {mQuery})");
                        }
                        else
                        {
                            queryBuilder.Append($" |> filter(fn: (r) => r[\"_field\"] == \"{channelIds[0]}\")");
                        }
                    }

                    queryBuilder.Append($" |> aggregateWindow(every: " + downsampleWindowSize + ", fn: " + downsampleType + ", createEmpty:false)");

                    //Get Total Count
                    var querytotalCount = new StringBuilder();
                    querytotalCount.Append(queryBuilder);
                    querytotalCount.Append($"|> group()");
                    querytotalCount.Append($"|> count(column: \"_value\")");

                    if (pageNum > 0)
                    {
                        int offset = (pageNum - 1) * pageLimit + 1;
                        queryBuilder.Append($"|> group()");
                        queryBuilder.Append($"|> limit(n: " + pageLimit + ", offset:" + offset + ")");
                    }

                    queryBuilder.Append($" |> keep(columns: [\"_time\", \"_value\", \"_field\",\"AssetID\",\"POCType\"])");
                    queryBuilder.Append($" |> yield(name: \"first\")");

                    var query = queryBuilder.ToString();

                    var rawResult = await client.GetQueryApi().QueryRawAsync(query, new InfluxDB.Client.Api.Domain.Dialect(), _org);

                    var lines = rawResult.Split('\n');
                    foreach (var line in lines.Skip(1)) // Skipping header line
                    {
                        var values = line.Split(',');
                        if (values.Length == 8)
                        {
                            responseDataPoints.Add(new DataPointModel
                            {
                                AssetId = values[5],
                                TimeOfTimeSeries = values[3],//dateResult,
                                ValueOfTimeSeries = double.TryParse(values[4], out var value) ? value : 0,
                                POCTypeId = values[6],
                                ChannelId = values[7]
                            });
                        }

                    } // foreach line in lines

                    if (responseDataPoints.Count > 1)
                    {
                        var queryTotalCnt = querytotalCount.ToString();
                        var rawTotalCount = await client.GetQueryApi().QueryRawAsync(queryTotalCnt, new InfluxDB.Client.Api.Domain.Dialect(), _org);
                        var totalLines = rawTotalCount.Split('\n');
                        foreach (var line in totalLines.Skip(1)) // Skipping header line
                        {
                            var values = line.Split(',');
                            if (values.Length == 4)
                            {
                                responseDataPoints[0].TotalCount = Convert.ToInt32(values[3]);
                                int pages = Math.DivRem(responseDataPoints[0].TotalCount, pageLimit, out var remainder);
                                if (remainder != 0)
                                {
                                    responseDataPoints[0].TotalPages = (pages + 1);
                                }
                                else
                                {
                                    responseDataPoints[0].TotalPages = pages;
                                }
                            }
                        } // foreach line in lines
                    }

                } // using client
            } // try block
            catch (InfluxDB.Client.Core.Exceptions.RequestTimeoutException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.LikelyRecoverable,
                };
            } // catch block of RequestTimeoutException
            catch (InfluxDB.Client.Core.Exceptions.TooManyRequestsException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.LikelyRecoverable,
                };
            } // catch block of TooManyRequestsException
            catch (InfluxDB.Client.Core.Exceptions.InternalServerErrorException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of InternalServerErrorException
            catch (InfluxDB.Client.Core.Exceptions.BadRequestException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of BadRequestException
            catch (InfluxDB.Client.Core.Exceptions.InfluxException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of InfluxException
            catch (Exception ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };

                Console.WriteLine($"Error: {result.Message}");
            } // catch block

            return responseDataPoints;
        }

        /// <summary>
        /// Method to read the trend data from influx data store with <paramref name="channelIds"/>. 
        /// </summary>
        /// <param name="assetIds">The asset guid.</param>
        /// <param name="customerId">The customer guid.</param>      
        /// <param name="channelIds">The list of channel ids.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="downsampleType">The down sample Type.</param>
        /// <param name="downsampleWindowSize">The down sample Window Size.</param>
        /// <param name="pageNum">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="inputs">Asset details.</param>
        /// <returns>The <seealso cref="IList{DataPointModel}"/></returns>
        public async Task<IList<TimeSeriesData>> GetTimeSeriesResponse(List<Guid> assetIds, Guid customerId,
          List<string> channelIds, string startDate, string endDate, string downsampleType, string downsampleWindowSize,
           int pageNum, int pageSize, List<TimeSeriesInputModel> inputs)
        {
            var responseDataPoints = new List<TimeSeriesData>();
            int pageLimit = pageSize != 0 ? pageSize : Convert.ToInt32(_pageSize);

            if (assetIds == null || assetIds.Count == 0 || assetIds[0] == Guid.Empty || customerId == Guid.Empty
                || channelIds == null || channelIds.Count == 0 || inputs == null || inputs.Count == 0)
            {
                return null;
            }

            DbStoreResult result;

            try
            {
                using (var client = _influxDbClient.Create())
                {
                    var startDateTime = DateTime.Parse(startDate);
                    var endDateTime = DateTime.Parse(endDate);

                    var queryBuilder = new StringBuilder();
                    queryBuilder.Append($"from(bucket: \"{_bucketName}\")");
                    queryBuilder.Append($" |> range(start: {startDate}, stop: {endDate})");
                    queryBuilder.Append($" |> filter(fn: (r) => r[\"_measurement\"] == \"{_measurement}\")");

                    if (customerId != Guid.Empty)
                    {
                        queryBuilder.Append($" |> filter(fn: (r) => r[\"CustomerID\"] == \"{customerId}\")");
                    }

                    foreach (var item in assetIds)
                    {
                        if (item == assetIds.First())
                        {
                            queryBuilder.Append($" |> filter(fn: (r) => r[\"AssetID\"] == \"{item}\"");
                        }
                        else
                        {
                            queryBuilder.Append($" or r[\"AssetID\"] == \"{item}\"");
                        }

                        if (item == assetIds.Last())
                        {
                            queryBuilder.Append(')');
                        }
                    }

                    if (channelIds != null && channelIds.Count > 0)
                    {
                        if (channelIds.Count > 1)
                        {
                            var multiAddrQuery = new StringBuilder();
                            foreach (var channel in channelIds)
                            {
                                if (!string.IsNullOrEmpty(channel))
                                {
                                    multiAddrQuery.Append($"r[\"_field\"] == \"{channel}\" or ");
                                }
                            }
                            var mQuery = multiAddrQuery.ToString()[..^4];
                            queryBuilder.Append($" |> filter(fn: (r) => {mQuery})");
                        }
                        else
                        {
                            queryBuilder.Append($" |> filter(fn: (r) => r[\"_field\"] == \"{channelIds[0]}\")");
                        }
                    }

                    queryBuilder.Append($" |> aggregateWindow(every: " + downsampleWindowSize + ", fn: " + downsampleType + ", createEmpty:false)");
                    queryBuilder.Append($"|> group(columns: [\"AssetID\",\"POCType\"])");
                    queryBuilder.Append($"|> pivot(rowKey: [\"_time\"], columnKey: [\"_field\"], valueColumn: \"_value\")");
                    queryBuilder.Append($"|> group()");
                    queryBuilder.Append($"|> sort(columns: [\"_time\",\"AssetID\"])");

                    //Get Total Count
                    var querytotalCount = new StringBuilder();
                    querytotalCount.Append(queryBuilder);
                    querytotalCount.Append($"|> count(column: \"AssetID\")");

                    if (pageNum > 0)
                    {
                        int offset = (pageNum - 1) * pageLimit;

                        queryBuilder.Append($"|> limit(n: " + pageLimit + ", offset:" + offset + ")");
                    }

                    //queryBuilder.Append($" |> keep(columns: [\"_time\", \"_value\", \"_field\",\"AssetID\",\"POCType\"])");
                    queryBuilder.Append($" |> yield(name: \"first\")");

                    var query = queryBuilder.ToString();

                    var rawResult = await client.GetQueryApi().QueryRawAsync(query, new InfluxDB.Client.Api.Domain.Dialect(), _org);

                    var dataLines = rawResult.Split('\n').ToList();
                    dataLines.Remove(string.Empty);
                    if (dataLines.Any())
                    {
                        var columns = dataLines[0].Split(",").ToList();

                        foreach (var line in dataLines.Skip(1))
                        {
                            var record = line.Trim().Split(",").ToList();
                            var resultSet = new TimeSeriesData { ChannelIds = new List<string>(), Values = new List<double?>() };
                            foreach (var column in columns)
                            {
                                if (column == string.Empty || column == "result" || column == "table")
                                {
                                    continue;
                                }

                                if (column == "AssetID")
                                {
                                    resultSet.AssetId = record[columns.IndexOf(column)];
                                    continue;
                                }
                                if (column == "POCType")
                                {
                                    resultSet.POCTypeId = record[columns.IndexOf(column)];
                                    continue;
                                }
                                if (column == "_time")
                                {
                                    if (DateTime.TryParse(record[columns.IndexOf(column)], null, DateTimeStyles.AdjustToUniversal, out var dateResult))
                                    {
                                        resultSet.Timestamp = dateResult.ToString("yyyy-MM-ddTHH:mm:ssZ");
                                    }
                                    continue;
                                }
                                if (double.TryParse(record[columns.IndexOf(column)], out var value))
                                {
                                    var item = inputs.Where(x => x.AssetId.ToString() == resultSet.AssetId.ToString()
                                              && x.ChannelIds.Contains(column)).ToList();
                                    if (item != null)
                                    {
                                        if (item.Count > 0)
                                        {
                                            resultSet.ChannelIds.Add(column);
                                            resultSet.Values.Add(value);
                                        }
                                    }

                                }
                                else
                                {
                                    var item = inputs.Where(x => x.AssetId.ToString() == resultSet.AssetId.ToString()
                                              && x.ChannelIds.Contains(column)).ToList();
                                    if (item != null)
                                    {
                                        if (item.Count > 0)
                                        {
                                            resultSet.ChannelIds.Add(column);
                                            resultSet.Values.Add(null);
                                        }
                                    }
                                }
                            }
                            responseDataPoints.Add(resultSet);

                        }

                        if (responseDataPoints.Count > 0)
                        {
                            var queryTotalCnt = querytotalCount.ToString();
                            var rawTotalCount = await client.GetQueryApi().QueryRawAsync(queryTotalCnt, new InfluxDB.Client.Api.Domain.Dialect(), _org);
                            var totalLines = rawTotalCount.Split('\n');
                            foreach (var line in totalLines.Skip(1)) // Skipping header line
                            {
                                var values = line.Split(',');
                                if (values.Length == 4)
                                {
                                    responseDataPoints[0].TotalCount = Convert.ToInt32(values[3]);
                                    int pages = Math.DivRem(responseDataPoints[0].TotalCount, pageLimit, out var remainder);
                                    if (remainder != 0)
                                    {
                                        responseDataPoints[0].TotalPages = (pages + 1);
                                    }
                                    else
                                    {
                                        responseDataPoints[0].TotalPages = pages;
                                    }
                                }
                            } // foreach line in lines
                        }
                    }
                } // using client
            } // try block
            catch (InfluxDB.Client.Core.Exceptions.RequestTimeoutException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.LikelyRecoverable,
                };
            } // catch block of RequestTimeoutException
            catch (InfluxDB.Client.Core.Exceptions.TooManyRequestsException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.LikelyRecoverable,
                };
            } // catch block of TooManyRequestsException
            catch (InfluxDB.Client.Core.Exceptions.InternalServerErrorException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of InternalServerErrorException
            catch (InfluxDB.Client.Core.Exceptions.BadRequestException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of BadRequestException
            catch (InfluxDB.Client.Core.Exceptions.InfluxException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of InfluxException
            catch (Exception ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };

                Console.WriteLine($"Error: {result.Message}");
            } // catch block

            return responseDataPoints;
        }

        /// <summary>
        /// Method to read the trend data from influx data store with <paramref name="addresses"/>
        /// and <paramref name="paramStandardType"/>. 
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="customerId">The customer guid.</param>
        /// <param name="pocType">The poc type.</param>
        /// <param name="addresses">The register addresses.</param>
        /// <param name="paramStandardType">The param standard type values.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>The <seealso cref="IList{DataPointModel}"/></returns>
        public async Task<IList<DataPointModel>> GetTrendData(Guid assetId, Guid customerId, string pocType, List<string> addresses,
            List<string> paramStandardType, string startDate, string endDate)
        {
            var responseDataPoints = new List<DataPointModel>();
            var result = new DbStoreResult()
            {
                Message = string.Empty,
                KindOfError = ErrorType.None,
            };

            if (assetId == Guid.Empty || customerId == Guid.Empty ||
                string.IsNullOrEmpty(pocType))
            {
                return null;
            }

            List<PointData> trendData = new List<PointData>();
            try
            {
                using (var client = _influxDbClient.Create())
                {
                    var queryBuilder = new StringBuilder();
                    queryBuilder.Append($"from(bucket: \"{_bucketName}\")");
                    queryBuilder.Append($" |> range(start: {startDate}Z, stop: {endDate}Z)");
                    queryBuilder.Append($" |> filter(fn: (r) => r[\"AssetID\"] == \"{assetId}\")");
                    if (customerId != Guid.Empty)
                    {
                        queryBuilder.Append($" |> filter(fn: (r) => r[\"CustomerID\"] == \"{customerId}\")");
                    }
                    queryBuilder.Append($" |> filter(fn: (r) => r[\"_measurement\"] == \"POCType{pocType}\" or r[\"_measurement\"] == \"POCType{pocType}\")");

                    if (addresses != null && addresses.Count > 0)
                    {
                        if (addresses.Count > 1)
                        {
                            var multiAddrQuery = new StringBuilder();
                            foreach (var addr in addresses)
                            {
                                if (!string.IsNullOrEmpty(addr))
                                {
                                    multiAddrQuery.Append($"r[\"Address\"] == \"C{addr}\" or ");
                                }
                            }
                            var mQuery = multiAddrQuery.ToString()[..^4];
                            queryBuilder.Append($" |> filter(fn: (r) => {mQuery})");
                        }
                        else
                        {
                            queryBuilder.Append($" |> filter(fn: (r) => r[\"Address\"] == \"C{addresses[0]}\")");
                        }
                    }
                    if (paramStandardType != null && paramStandardType.Count > 0)
                    {
                        if (paramStandardType.Count > 1)
                        {
                            var multiparamStdTypeQuery = new StringBuilder();
                            foreach (var value in paramStandardType)
                            {
                                if (!string.IsNullOrEmpty(value))
                                {
                                    multiparamStdTypeQuery.Append($"r[\"ParamStandardType\"] == \"C{value}\" or ");
                                }
                            }
                            var mQuery = multiparamStdTypeQuery.ToString()[..^4];
                            queryBuilder.Append($" |> filter(fn: (r) => {mQuery})");
                        }
                        else
                        {
                            queryBuilder.Append($" |> filter(fn: (r) => r[\"ParamStandardType\"] == \"{paramStandardType[0]}\")");
                        }
                    }

                    queryBuilder.Append($" |> filter(fn: (r) => r[\"_field\"] == \"Value\")");
                    var query = queryBuilder.ToString();

                    var tables = await client.GetQueryApi().QueryAsync(query, _org);

                    foreach (var record in tables.SelectMany(table => table.Records))
                    {
                        responseDataPoints.Add(new DataPointModel
                        {
                            Time = DateTime.Parse(record.GetTime().Value.ToString(), null, DateTimeStyles.AdjustToUniversal),
                            Value = record.GetValue(),
                            TrendName = record.GetValueByKey("Address").ToString().Replace("C", ""),
                        });
                    }
                }
            } // try block
            catch (InfluxDB.Client.Core.Exceptions.RequestTimeoutException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.LikelyRecoverable,
                };
            } // catch block of RequestTimeoutException
            catch (InfluxDB.Client.Core.Exceptions.TooManyRequestsException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.LikelyRecoverable,
                };
            } // catch block of TooManyRequestsException
            catch (InfluxDB.Client.Core.Exceptions.InternalServerErrorException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of InternalServerErrorException
            catch (InfluxDB.Client.Core.Exceptions.BadRequestException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of BadRequestException
            catch (InfluxDB.Client.Core.Exceptions.InfluxException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of InfluxException
            catch (Exception ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block

            return responseDataPoints;
        }

        /// <summary>
        /// Gets the downtime data from InfluxDB stores based on the provided filters, start date, and end date.
        /// </summary>
        /// <param name="filterData">The list of downtime filters.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>The <seealso cref="IList{DowntimeByWellsInfluxModel}"/></returns>
        public async Task<IList<DowntimeByWellsInfluxModel>> GetDowntimeAsync(IList<DowntimeFiltersInflux> filterData,
            string startDate, string endDate)
        {
            if (filterData == null)
            {
                throw new ArgumentNullException(nameof(filterData));
            }

            if (string.IsNullOrWhiteSpace(startDate))
            {
                throw new ArgumentNullException(nameof(startDate));
            }

            if (string.IsNullOrWhiteSpace(endDate))
            {
                throw new ArgumentNullException(nameof(endDate));
            }

            if (filterData.Count == 0)
            {
                return new List<DowntimeByWellsInfluxModel>();
            }

            var dbStoreResult = new DbStoreResult()
            {
                Message = string.Empty,
                KindOfError = ErrorType.None,
            };

            var result = new List<DowntimeByWellsInfluxModel>();

            try
            {
                using (var client = _influxDbClient.Create())
                {
                    var count = 0;

                    var queryBuilder = new StringBuilder();

                    foreach (var measurementData in filterData)
                    {
                        var assetIds = measurementData.AssetIds.Where(x => x != Guid.Empty).ToList();
                        var customerIds = measurementData.CustomerIds.Where(x => x != Guid.Empty).ToList();

                        if (assetIds.Count == 0 || customerIds.Count == 0)
                        {
                            continue;
                        }

                        if (count > 0)
                        {
                            queryBuilder.Append(',');
                        }

                        queryBuilder.Append(GenerateMeasurementQuery(measurementData.POCType, measurementData.ParamStandardType,
                            assetIds,
                            customerIds, startDate, endDate));

                        count++;
                    }

                    if (count > 1)
                    {
                        // if there are multiple measurements, union them.
                        queryBuilder.Insert(0, "union(tables: [");
                        queryBuilder.Append("])");
                    }
                    var query = queryBuilder.ToString();
                    var tables = await client.GetQueryApi().QueryAsync(query, _org);

                    foreach (var record in tables.SelectMany(x => x.Records))
                    {
                        result.Add(new DowntimeByWellsInfluxModel
                        {
                            Date = DateTime.Parse(record.GetTime().Value.ToString(), null, DateTimeStyles.AdjustToUniversal),
                            Value = float.TryParse(record.GetValue()?.ToString(), out var valueFloat) ? valueFloat : 0,
                            Id = record.GetValueByKey("AssetID").ToString(),
                            ParamStandardType = record.GetValueByKey("ParamStandardType").ToString(),
                        });
                    }
                }
            }
            catch (InfluxDB.Client.Core.Exceptions.RequestTimeoutException ex)
            {
                dbStoreResult = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.LikelyRecoverable,
                };
            } // catch block of RequestTimeoutException
            catch (InfluxDB.Client.Core.Exceptions.TooManyRequestsException ex)
            {
                dbStoreResult = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.LikelyRecoverable,
                };
            } // catch block of TooManyRequestsException
            catch (InfluxDB.Client.Core.Exceptions.InternalServerErrorException ex)
            {
                dbStoreResult = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of InternalServerErrorException
            catch (InfluxDB.Client.Core.Exceptions.BadRequestException ex)
            {
                dbStoreResult = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of BadRequestException
            catch (InfluxDB.Client.Core.Exceptions.InfluxException ex)
            {
                dbStoreResult = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of InfluxException
            catch (Exception ex)
            {
                dbStoreResult = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block

            return result;
        }

        /// <summary>
        /// Gets the downtime data from InfluxDB stores based on the provided filters, start date, and end date.
        /// </summary>
        /// <param name="filterData">The list of downtime filters.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>The <seealso cref="IList{DowntimeByWellsInfluxModel}"/></returns>
        public async Task<IList<DowntimeByWellsInfluxModel>> GetDowntimeAsync(IList<DowntimeFiltersWithChannelIdInflux> filterData,
            string startDate, string endDate)
        {
            if (filterData == null)
            {
                throw new ArgumentNullException(nameof(filterData));
            }

            if (string.IsNullOrWhiteSpace(startDate))
            {
                throw new ArgumentNullException(nameof(startDate));
            }

            if (string.IsNullOrWhiteSpace(endDate))
            {
                throw new ArgumentNullException(nameof(endDate));
            }

            if (filterData.Count == 0)
            {
                return new List<DowntimeByWellsInfluxModel>();
            }

            var dbStoreResult = new DbStoreResult()
            {
                Message = string.Empty,
                KindOfError = ErrorType.None,
            };

            var result = new List<DowntimeByWellsInfluxModel>();

            try
            {
                using (var client = _influxDbClient.Create())
                {
                    foreach (var measurementData in filterData)
                    {
                        var queryBuilder = new StringBuilder();
                        var assetIds = measurementData.AssetIds.Where(x => x != Guid.Empty).ToList();
                        var customerIds = measurementData.CustomerIds.Where(x => x != Guid.Empty).ToList();

                        if (assetIds.Count == 0 || customerIds.Count == 0)
                        {
                            continue;
                        }

                        queryBuilder.Append(GenerateMeasurementQueryWithChannelId(measurementData.ChannelIds,
                            assetIds, customerIds, startDate, endDate));

                        var query = queryBuilder.ToString();
                        var tables = await client.GetQueryApi().QueryAsync(query, _org);

                        foreach (var record in tables.SelectMany(x => x.Records))
                        {
                            result.Add(new DowntimeByWellsInfluxModel
                            {
                                Date = DateTime.Parse(record.GetTime().ToString()),
                                Value = float.TryParse(record.GetValue()?.ToString(), out var valueFloat) ? valueFloat : 0,
                                Id = record.GetValueByKey("AssetID").ToString(),
                                ParamStandardType = record.GetValueByKey("ParamStandardType").ToString(),
                            });
                        }
                    }
                }
            }
            catch (InfluxDB.Client.Core.Exceptions.RequestTimeoutException ex)
            {
                dbStoreResult = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.LikelyRecoverable,
                };
            } // catch block of RequestTimeoutException
            catch (InfluxDB.Client.Core.Exceptions.TooManyRequestsException ex)
            {
                dbStoreResult = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.LikelyRecoverable,
                };
            } // catch block of TooManyRequestsException
            catch (InfluxDB.Client.Core.Exceptions.InternalServerErrorException ex)
            {
                dbStoreResult = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of InternalServerErrorException
            catch (InfluxDB.Client.Core.Exceptions.BadRequestException ex)
            {
                dbStoreResult = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of BadRequestException
            catch (InfluxDB.Client.Core.Exceptions.InfluxException ex)
            {
                dbStoreResult = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of InfluxException
            catch (Exception ex)
            {
                dbStoreResult = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block

            return result;
        }

        /// <summary>
        /// Method to fetch the letest trend data from influx data store.
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="customerId"></param>
        /// <param name="pocType"></param>
        /// <param name="channelIds"></param>
        /// <returns></returns>
        public async Task<IList<DataPointModel>> GetLatestTrendData(Guid assetId, Guid customerId, string pocType,
            List<string> channelIds)
        {
            var responseDataPoints = new List<DataPointModel>();

            if (assetId == Guid.Empty || customerId == Guid.Empty ||
                string.IsNullOrEmpty(pocType) || channelIds == null || channelIds.Count == 0)
            {
                return null;
            }

            DbStoreResult result;

            try
            {
                using (var client = _influxDbClient.Create())
                {
                    var queryBuilder = new StringBuilder();
                    queryBuilder.Append($"from(bucket: \"{_bucketName}\")");
                    queryBuilder.Append($" |> range(start: 0)");
                    queryBuilder.Append($" |> filter(fn: (r) => r[\"_measurement\"] == \"{_measurement}\")");
                    queryBuilder.Append($" |> filter(fn: (r) => r[\"AssetID\"] == \"{assetId}\")");
                    if (customerId != Guid.Empty)
                    {
                        queryBuilder.Append($" |> filter(fn: (r) => r[\"CustomerID\"] == \"{customerId}\")");
                    }
                    if (pocType != string.Empty)
                    {
                        queryBuilder.Append($" |> filter(fn: (r) => r[\"POCType\"] == \"{pocType}\")");
                    }

                    if (channelIds != null && channelIds.Count > 0)
                    {
                        if (channelIds.Count > 1)
                        {
                            var multiAddrQuery = new StringBuilder();
                            foreach (var channel in channelIds)
                            {
                                if (!string.IsNullOrEmpty(channel))
                                {
                                    multiAddrQuery.Append($"r[\"_field\"] == \"{channel}\" or ");
                                }
                            }
                            var mQuery = multiAddrQuery.ToString()[..^4];
                            queryBuilder.Append($" |> filter(fn: (r) => {mQuery})");
                        }
                        else
                        {
                            queryBuilder.Append($" |> filter(fn: (r) => r[\"_field\"] == \"{channelIds[0]}\")");
                        }
                    }

                    queryBuilder.Append($"|> keep(columns: [\"_time\", \"_field\", \"_value\"])");
                    queryBuilder.Append($"|> sort(columns: [\"_time\"], desc: true)");
                    queryBuilder.Append($"|> limit(n:1)");

                    var query = queryBuilder.ToString();

                    var tables = await client.GetQueryApi().QueryAsync(query, _org);

                    foreach (var record in tables.SelectMany(table => table.Records))
                    {
                        responseDataPoints.Add(new DataPointModel
                        {
                            Time = DateTime.Parse(record.GetTime().Value.ToString(), null, DateTimeStyles.AdjustToUniversal),
                            Value = record.GetValue(),
                            TrendName = record.GetField()
                        });
                    }
                }
            } // try block
            catch (InfluxDB.Client.Core.Exceptions.RequestTimeoutException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.LikelyRecoverable,
                };
            } // catch block of RequestTimeoutException
            catch (InfluxDB.Client.Core.Exceptions.TooManyRequestsException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.LikelyRecoverable,
                };
            } // catch block of TooManyRequestsException
            catch (InfluxDB.Client.Core.Exceptions.InternalServerErrorException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of InternalServerErrorException
            catch (InfluxDB.Client.Core.Exceptions.BadRequestException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of BadRequestException
            catch (InfluxDB.Client.Core.Exceptions.InfluxException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of InfluxException
            catch (Exception ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block

            return responseDataPoints;
        }

        /// <summary>
        /// Gets the downtime data from InfluxDB stores based on the assetId, start date, and end date.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="channelId">The channel id.</param>
        /// <returns>The influx data for downtime PST in the form of <seealso cref="IList{DataPointModel}"/>.</returns>
        public async Task<IList<DataPointModel>> GetDowntime(Guid assetId, DateTime startDate, DateTime endDate, string channelId)
        {
            var responseDataPoints = new List<DataPointModel>();

            DbStoreResult result;

            try
            {
                using (var client = _influxDbClient.Create())
                {
                    var queryBuilder = new StringBuilder();
                    queryBuilder.Append($"from(bucket: \"{_bucketName}\")");
                    queryBuilder.Append($" |> range(start: {startDate.ToString("yyyy-MM-ddTHH:mm:ssZ")}, stop: {endDate.ToString("yyyy-MM-ddTHH:mm:ssZ")})");
                    queryBuilder.Append($" |> filter(fn: (r) => r[\"_measurement\"] == \"{_measurement}\")");
                    queryBuilder.Append($" |> filter(fn: (r) => r[\"AssetID\"] == \"{assetId}\")");
                    queryBuilder.Append($" |> filter(fn: (r) => r[\"_field\"] == \"{channelId}\")");
                    queryBuilder.Append($" |> sort(columns: [\"_time\"])");
                    queryBuilder.Append($" |> keep(columns: [\"_time\", \"_field\", \"_value\"])");

                    var query = queryBuilder.ToString();

                    var tables = await client.GetQueryApi().QueryAsync(query, _org);

                    foreach (var record in tables.SelectMany(table => table.Records))
                    {
                        responseDataPoints.Add(new DataPointModel
                        {
                            Time = DateTime.Parse(record.GetTime().Value.ToString(), null, DateTimeStyles.AdjustToUniversal),
                            Value = record.GetValue(),
                            TrendName = record.GetField()
                        });
                    }

                    return responseDataPoints;
                }
            } // try block
            catch (InfluxDB.Client.Core.Exceptions.RequestTimeoutException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.LikelyRecoverable,
                };
            } // catch block of RequestTimeoutException
            catch (InfluxDB.Client.Core.Exceptions.TooManyRequestsException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.LikelyRecoverable,
                };
            } // catch block of TooManyRequestsException
            catch (InfluxDB.Client.Core.Exceptions.InternalServerErrorException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of InternalServerErrorException
            catch (InfluxDB.Client.Core.Exceptions.BadRequestException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of BadRequestException
            catch (InfluxDB.Client.Core.Exceptions.InfluxException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of InfluxException
            catch (Exception ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block

            return null;
        }

        #endregion

        #region IDataHistoryTrendData Implementation

        /// <summary>
        /// Method to fetch the GetCurrentRawScanData from influx data store />. 
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <returns>The <seealso cref="IList{CurrentRawScanDataInfluxModel}"/></returns>
        public async Task<IList<CurrentRawScanDataInfluxModel>> GetCurrentRawScanData(Guid assetId)
        {
            if (assetId == Guid.Empty)
            {
                return null;
            }

            var response = new List<CurrentRawScanDataInfluxModel>();
            DbStoreResult result = new DbStoreResult();

            try
            {
                using (var client = _influxDbClient.Create())
                {
                    var queryBuilder = new StringBuilder();
                    queryBuilder.Append($"from(bucket: \"{_bucketName}\") ");
                    queryBuilder.Append($"|> range(start: -1y) ");
                    queryBuilder.Append($"|> filter(fn: (r) => r[\"_measurement\"] == \"{_measurement}\" and r[\"AssetID\"] == \"{assetId}\") ");
                    queryBuilder.Append($"|> aggregateWindow(every: inf, fn: last, column: \"_value\") ");
                    queryBuilder.Append($"|> group(columns: [\"ChannelId\"]) ");
                    queryBuilder.Append($"|> keep(columns: [\"_time\", \"_value\", \"POCType\", \"_field\", \"AssetID\"])");

                    string query = queryBuilder.ToString();
                    var tables = await client.GetQueryApi().QueryAsync(query, _org);

                    foreach (var record in tables.SelectMany(table => table.Records))
                    {
                        var currentRawScanDataInflux = new CurrentRawScanDataInfluxModel
                        {
                            DateTimeUpdated = DateTime.Parse(record.GetTime().Value.ToString(), null, DateTimeStyles.AdjustToUniversal),
                            Value = record.Values["_value"] is double value ? (float)value : default,
                            POCType = record.Values["POCType"]?.ToString(),
                            ChannelId = record.Values["_field"]?.ToString()
                        };

                        response.Add(currentRawScanDataInflux);
                    }
                }
            }
            catch (InfluxDB.Client.Core.Exceptions.RequestTimeoutException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.LikelyRecoverable,
                };
            }
            catch (InfluxDB.Client.Core.Exceptions.TooManyRequestsException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.LikelyRecoverable,
                };
            }
            catch (InfluxDB.Client.Core.Exceptions.InternalServerErrorException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            }
            catch (InfluxDB.Client.Core.Exceptions.BadRequestException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            }
            catch (InfluxDB.Client.Core.Exceptions.InfluxException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            }
            catch (Exception ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            }

            return response;
        }

        /// <summary>
        /// Method to fetch the graph data from influx data store.
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="wellName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="pocType"></param>
        /// <param name="parameters"></param>
        /// <param name="channelIds"></param>
        /// <param name="listOfTrends"></param>
        /// <param name="aggregate"></param>
        /// <param name="aggregateMethod"></param>
        /// <param name="nodeTimeZoneOffset"></param>
        /// <param name="honorDaylighSaving"></param>
        /// <returns></returns>
        public async Task<IList<DataPointsModelDto>> GetInfluxDataAssetTrends(Guid assetId, string wellName, DateTime startDate, DateTime endDate, int pocType, List<Parameters> parameters, List<string> channelIds, List<DefaultParameters> listOfTrends, string aggregate, string aggregateMethod, float nodeTimeZoneOffset, bool honorDaylighSaving)
        {
            var influxData = await QueryInfluxData(assetId, pocType, channelIds, startDate, endDate, aggregate, aggregateMethod);
            var lastRecord = await QueryInfluxDataLastOneRecord(assetId, channelIds, startDate, endDate, pocType.ToString());

            if (influxData == null || !influxData.Any())
            {
                return null;
            }

            if (lastRecord != null && lastRecord.Any())
            {
                var dt = lastRecord[1]?.Split(',')?.ToList();
                dt.RemoveRange(3, 2);
                var str = String.Join(",", dt);
                influxData.Insert(1, str);
            }

            var dataPointDictionery = new Dictionary<string, List<DataPointModelDto>>();
            var thresholdValuesDictionery = new Dictionary<string, List<DataPointModelDto>>();

            var columns = influxData[0].Split(",").ToList();

            for (int i = 0; i < columns.Count; i++)
            {
                bool isThresholdValue = false;
                string unitOfMeasure = string.Empty;
                var influxValues = new List<DataPointModelDto>();

                var parameterDetails = parameters.FirstOrDefault(x => x.ChannelId == columns[i]);
                if (parameterDetails != null)
                {
                    string parameterLegacyId = parameterDetails.ParamStandardType.LegacyId.FirstOrDefault().Value;
                    var paramName = listOfTrends.FirstOrDefault(x => x.Pst == parameterLegacyId)?.Name;

                    if (string.IsNullOrEmpty(paramName))
                    {
                        paramName = parameterLegacyId;
                        isThresholdValue = true;
                    }

                    var facilityTagResult = parameters.FirstOrDefault(x => x.ParameterType == "Facility" && x.Name.Split('|').Contains(wellName) && x.ChannelId == columns[i]);
                    if (facilityTagResult != null)
                    {
                        unitOfMeasure = facilityTagResult.UnitOfMeasure;
                    }
                    else
                    {
                        unitOfMeasure = parameterDetails.UnitOfMeasure;
                    }

                    var unit = GetUnitOfMeasureShortForm(unitOfMeasure);

                    foreach (var record in influxData.Skip(1))
                    {
                        var item = record.Split(',').ToList();
                        DateTime.TryParse(item[3], null, DateTimeStyles.AdjustToUniversal, out var dateResult);
                        var dt = _dateTimeConverter.ConvertToApplicationServerTimeFromUTC(dateResult, string.Empty, LoggingModel.SqlDateTime);
                        var dtResponse = _dateTimeConverter.GetTimeZoneAdjustedTime(nodeTimeZoneOffset, honorDaylighSaving, dt, string.Empty, LoggingModel.SqlDateTime);
                        var dtResponseAlly = GetTimeZoneAdjustedTime(nodeTimeZoneOffset, honorDaylighSaving, dt, string.Empty, LoggingModel.SqlDateTime);
                        influxValues.Add(new DataPointModelDto
                        {
                            Name = paramName,
                            Date = dtResponse,
                            Value = !string.IsNullOrEmpty(item[i])?MathUtility.RoundToSignificantDigits(Convert.ToDouble(item[i]), 3).ToString(): Convert.ToString(item[i]),
                            UOM = unitOfMeasure,
                            Short_UOM = unit,
                            ParamTypeId = parameterLegacyId
                        });
                    }

                    if (isThresholdValue)
                    {
                        if (thresholdValuesDictionery.TryGetValue(paramName, out var value))
                        {
                            value.AddRange(influxValues);
                        }
                        else
                        {
                            thresholdValuesDictionery.Add(paramName, influxValues);
                        }
                    }
                    else
                    {
                        if (dataPointDictionery.TryGetValue(paramName, out var value))
                        {
                            value.AddRange(influxValues);
                        }
                        else
                        {
                            dataPointDictionery.Add(paramName, influxValues);
                        }
                    }
                }
            }

            return GetDataPoint(dataPointDictionery, thresholdValuesDictionery, listOfTrends);
        }

        /// <summary>
        /// Method to fetch the GetCurrentRawScanData from influx data store />. 
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="customerId">The customer guid.</param>
        /// <returns>The <seealso cref="IList{DataPointModel}"/></returns>
        public async Task<IList<DataPointModel>> GetCurrentRawScanData(Guid assetId, Guid customerId)
        {
            if (assetId == Guid.Empty)
            {
                return null;
            }

            var response = new List<DataPointModel>();
            DbStoreResult result = new DbStoreResult();

            try
            {
                using (var client = _influxDbClient.Create())
                {
                    var queryBuilder = new StringBuilder();
                    queryBuilder.Append($"from(bucket: \"{_bucketName}\")");
                    queryBuilder.Append($"|> range(start: -1y) "); // filter data from last 1 year only.
                    queryBuilder.Append($" |> filter(fn: (r) => r[\"_measurement\"] == \"{_measurement}\")");
                    queryBuilder.Append($" |> filter(fn: (r) => r[\"AssetID\"] == \"{assetId}\")");

                    if (customerId != Guid.Empty)
                    {
                        queryBuilder.Append($" |> filter(fn: (r) => r[\"CustomerID\"] == \"{customerId}\")");
                    }

                    queryBuilder.Append($"|> keep(columns: [\"_time\", \"_field\", \"_value\", \"AssetID\", \"CustomerID\", \"POCType\"])");
                    queryBuilder.Append($"|> sort(columns: [\"_time\"], desc: true)");
                    queryBuilder.Append($"|> limit(n:1)");

                    string query = queryBuilder.ToString();
                    var tables = await client.GetQueryApi().QueryAsync(query, _org);

                    foreach (var record in tables.SelectMany(table => table.Records))
                    {
                        var currentRawScanDataInflux = new DataPointModel
                        {
                            Time = DateTime.Parse(record.GetTime().Value.ToString(), null, DateTimeStyles.AdjustToUniversal),
                            Value = record.Values["_value"] is double value ? (float)value : default,
                            POCTypeId = record.Values["POCType"]?.ToString(),
                            TrendName = record.Values["_field"]?.ToString() // TrendName is ChannelId.
                        };

                        response.Add(currentRawScanDataInflux);
                    }
                }
            }
            catch (InfluxDB.Client.Core.Exceptions.RequestTimeoutException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.LikelyRecoverable,
                };
            }
            catch (InfluxDB.Client.Core.Exceptions.TooManyRequestsException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.LikelyRecoverable,
                };
            }
            catch (InfluxDB.Client.Core.Exceptions.InternalServerErrorException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            }
            catch (InfluxDB.Client.Core.Exceptions.BadRequestException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            }
            catch (InfluxDB.Client.Core.Exceptions.InfluxException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            }
            catch (Exception ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            }

            return response;
        }
        #endregion

        #region Private Method

        /// <summary>
        /// Set any additional configuration that needs to be done during the initialize process
        /// such as the Deployment Mode, Bucket, Org, EndPoint, User Password Or Token and Bucket Retention.
        /// </summary>
        private void SetConfigurationOnInit()
        {
            _bucketName = AppConfig.GetSection("AppSettings:BucketName").Value ??
                    BUCKET_NAME;

            _org = AppConfig.GetSection("AppSettings:Org").Value ??
                   ORGANIZATION;

            _measurement = AppConfig.GetSection("AppSettings:MeasurementName").Value ??
                    MEASUREMENT;

            _pageSize = AppConfig.GetSection("AppSettings:PageSize").Value ?? DEFAULT_PAGE_SIZE;
        }

        private string GenerateMeasurementQuery(string pocType, IList<string> paramStandardTypes, IList<Guid> assetIds,
            IList<Guid> customerIds, string startDate, string endDate)
        {
            var queryBuilder = new StringBuilder();

            queryBuilder.Append($"from(bucket: \"{_bucketName}\")");
            queryBuilder.Append($" |> range(start: {startDate}Z, stop: {endDate}Z)");
            queryBuilder.Append($" |> filter(fn: (r) => r[\"_measurement\"] == \"POCType{pocType}\")");

            foreach (var item in assetIds)
            {
                if (item == assetIds.First())
                {
                    queryBuilder.Append($" |> filter(fn: (r) => r[\"AssetID\"] == \"{item}\"");
                }
                else
                {
                    queryBuilder.Append($" or r[\"AssetID\"] == \"{item}\"");
                }

                if (item == assetIds.Last())
                {
                    queryBuilder.Append(')');
                }
            }

            foreach (var item in customerIds)
            {
                if (item == customerIds.First())
                {
                    queryBuilder.Append($" |> filter(fn: (r) => r[\"CustomerID\"] == \"{item}\"");
                }
                else
                {
                    queryBuilder.Append($" or r[\"CustomerID\"] == \"{item}\"");
                }

                if (item == customerIds.Last())
                {
                    queryBuilder.Append(')');
                }
            }

            foreach (var item in paramStandardTypes)
            {
                if (item == paramStandardTypes.First())
                {
                    queryBuilder.Append($" |> filter(fn: (r) => r[\"ParamStandardType\"] == \"{item}\"");
                }
                else
                {
                    queryBuilder.Append($" or r[\"ParamStandardType\"] == \"{item}\"");
                }

                if (item == paramStandardTypes.Last())
                {
                    queryBuilder.Append(')');
                }
            }

            queryBuilder.Append(" |> filter(fn: (r) => r[\"_field\"] == \"Value\")");

            return queryBuilder.ToString();
        }

        private string GenerateMeasurementQueryWithChannelId(IList<string> channelId, IList<Guid> assetIds,
            IList<Guid> customerIds, string startDate, string endDate)
        {
            var queryBuilder = new StringBuilder();

            queryBuilder.Append($"from(bucket: \"{_bucketName}\")");
            queryBuilder.Append($" |> range(start: {startDate}Z, stop: {endDate}Z)");
            queryBuilder.Append($" |> filter(fn: (r) => r[\"_measurement\"] == \"{_measurement}\")");

            foreach (var item in assetIds)
            {
                if (item == assetIds.First())
                {
                    queryBuilder.Append($" |> filter(fn: (r) => r[\"AssetID\"] == \"{item}\"");
                }
                else
                {
                    queryBuilder.Append($" or r[\"AssetID\"] == \"{item}\"");
                }

                if (item == assetIds.Last())
                {
                    queryBuilder.Append(')');
                }
            }

            foreach (var item in customerIds)
            {
                if (item == customerIds.First())
                {
                    queryBuilder.Append($" |> filter(fn: (r) => r[\"CustomerID\"] == \"{item}\"");
                }
                else
                {
                    queryBuilder.Append($" or r[\"CustomerID\"] == \"{item}\"");
                }

                if (item == customerIds.Last())
                {
                    queryBuilder.Append(')');
                }
            }

            foreach (var item in channelId)
            {
                if (item == channelId.First())
                {
                    queryBuilder.Append($" |> filter(fn: (r) => r[\"_field\"] == \"{item}\"");
                }
                else
                {
                    queryBuilder.Append($" or r[\"_field\"] == \"{item}\"");
                }

                if (item == channelId.Last())
                {
                    queryBuilder.Append(')');
                }
            }

            return queryBuilder.ToString();
        }

        private async Task<IList<string>> QueryInfluxData(Guid assetId, int pocType, List<string> channelIds, DateTime startDate, DateTime endDate, string aggregate, string aggregateMethod)
        {
            DbStoreResult result;
            try
            {
                using (var client = _influxDbClient.Create())
                {

                    var queryBuilder = new StringBuilder();
                    queryBuilder.Append($"from(bucket: \"{_bucketName}\")");
                    queryBuilder.Append($" |> range(start: {startDate.ToString("yyyy-MM-ddTHH:mm:ssZ")}, stop: {endDate.ToString("yyyy-MM-ddTHH:mm:ssZ")})");
                    queryBuilder.Append($" |> filter(fn: (r) => r[\"_measurement\"] == \"{_measurement}\")");
                    queryBuilder.Append($" |> filter(fn: (r) => r[\"AssetID\"] == \"{assetId}\")");
                    queryBuilder.Append($" |> filter(fn: (r) => r[\"POCType\"] == \"{pocType}\")");

                    if (channelIds != null && channelIds.Count > 0)
                    {
                        if (channelIds.Count > 1)
                        {
                            var multiAddrQuery = new StringBuilder();
                            foreach (var channel in channelIds)
                            {
                                if (!string.IsNullOrEmpty(channel))
                                {
                                    multiAddrQuery.Append($"r[\"_field\"] == \"{channel}\" or ");
                                }
                            }
                            var mQuery = multiAddrQuery.ToString()[..^4];
                            queryBuilder.Append($" |> filter(fn: (r) => {mQuery})");
                        }
                        else
                        {
                            queryBuilder.Append($" |> filter(fn: (r) => r[\"_field\"] == \"{channelIds[0]}\")");
                        }
                    }

                    queryBuilder.Append($" |> aggregateWindow(every: " + aggregate + ", fn: " + aggregateMethod + ", createEmpty:false)");
                    queryBuilder.Append($"|> group(columns: [\"AssetID\",\"POCType\"])");
                    queryBuilder.Append($"|> pivot(rowKey: [\"_time\"], columnKey: [\"_field\"], valueColumn: \"_value\")");
                    queryBuilder.Append($"|> group()");
                    queryBuilder.Append($"|> sort(columns: [\"_time\",\"AssetID\"])");
                    queryBuilder.Append($" |> yield(name: \"first\")");

                    var query = queryBuilder.ToString();
                    var rawResult = await client.GetQueryApi().QueryRawAsync(query, new InfluxDB.Client.Api.Domain.Dialect(), _org);

                    var dataLines = rawResult.Split('\n').ToList();
                    dataLines.Remove(string.Empty);

                    return dataLines;
                } // using client
            } // try block
            catch (InfluxDB.Client.Core.Exceptions.RequestTimeoutException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.LikelyRecoverable,
                };
            } // catch block of RequestTimeoutException
            catch (InfluxDB.Client.Core.Exceptions.TooManyRequestsException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.LikelyRecoverable,
                };
            } // catch block of TooManyRequestsException
            catch (InfluxDB.Client.Core.Exceptions.InternalServerErrorException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of InternalServerErrorException
            catch (InfluxDB.Client.Core.Exceptions.BadRequestException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of BadRequestException
            catch (InfluxDB.Client.Core.Exceptions.InfluxException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of InfluxException
            catch (Exception ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };

                Console.WriteLine($"Error: {result.Message}");
            } // catch block

            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="channelIds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="pocType"></param>
        /// <returns></returns>
        private async Task<IList<string>> QueryInfluxDataLastOneRecord(Guid assetId, List<string> channelIds, DateTime startDate, DateTime endDate, string pocType)
        {
            DbStoreResult result;
            try
            {
                using (var client = _influxDbClient.Create())
                {
                    var queryBuilder = new StringBuilder();
                    queryBuilder.Append($"from(bucket: \"{_bucketName}\")");
                    queryBuilder.Append($" |> range(start: {startDate.ToString("yyyy-MM-ddTHH:mm:ssZ")}, stop: {endDate.ToString("yyyy-MM-ddTHH:mm:ssZ")})");
                    queryBuilder.Append($" |> filter(fn: (r) => r[\"_measurement\"] == \"{_measurement}\")");
                    queryBuilder.Append($" |> filter(fn: (r) => r[\"AssetID\"] == \"{assetId}\")");
                    queryBuilder.Append($" |> filter(fn: (r) => r[\"POCType\"] == \"{pocType}\")");

                    if (channelIds != null && channelIds.Count > 0)
                    {
                        if (channelIds.Count > 1)
                        {
                            var multiAddrQuery = new StringBuilder();
                            foreach (var channel in channelIds)
                            {
                                if (!string.IsNullOrEmpty(channel))
                                {
                                    multiAddrQuery.Append($"r[\"_field\"] == \"{channel}\" or ");
                                }
                            }
                            var mQuery = multiAddrQuery.ToString()[..^4];
                            queryBuilder.Append($" |> filter(fn: (r) => {mQuery})");
                        }
                        else
                        {
                            queryBuilder.Append($" |> filter(fn: (r) => r[\"_field\"] == \"{channelIds[0]}\")");
                        }
                    }

                    queryBuilder.Append($"|> group(columns: [\"AssetID\",\"POCType\"])");
                    queryBuilder.Append($"|> pivot(rowKey: [\"_time\"], columnKey: [\"_field\"], valueColumn: \"_value\")");
                    queryBuilder.Append($"|> group()");
                    queryBuilder.Append($"|> sort(columns: [\"_time\"], desc: true)");
                    queryBuilder.Append($"|> limit(n:1)");
                    queryBuilder.Append($" |> yield(name: \"first\")");

                    var query = queryBuilder.ToString();
                    var rawResult = await client.GetQueryApi().QueryRawAsync(query, new InfluxDB.Client.Api.Domain.Dialect(), _org);
                    var dataLines = rawResult.Split('\n').ToList();
                    dataLines.Remove(string.Empty);

                    return dataLines;
                } // using client
            } // try block
            catch (InfluxDB.Client.Core.Exceptions.RequestTimeoutException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.LikelyRecoverable,
                };
            } // catch block of RequestTimeoutException
            catch (InfluxDB.Client.Core.Exceptions.TooManyRequestsException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.LikelyRecoverable,
                };
            } // catch block of TooManyRequestsException
            catch (InfluxDB.Client.Core.Exceptions.InternalServerErrorException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of InternalServerErrorException
            catch (InfluxDB.Client.Core.Exceptions.BadRequestException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of BadRequestException
            catch (InfluxDB.Client.Core.Exceptions.InfluxException ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };
            } // catch block of InfluxException
            catch (Exception ex)
            {
                result = new DbStoreResult()
                {
                    Message = ex.Message,
                    KindOfError = ErrorType.NotRecoverable,
                };

                Console.WriteLine($"Error: {result.Message}");
            } // catch block

            return null;
        }
        private List<DataPointsModelDto> GetDataPoint(Dictionary<string, List<DataPointModelDto>> keyValuePairs, Dictionary<string, List<DataPointModelDto>> thresholdValues, List<DefaultParameters> listOfTrends)
        {
            List<DataPointsModelDto> list = new List<DataPointsModelDto>();
            foreach (var item in keyValuePairs)
            {
                var values = item.Value.Where(x => !string.IsNullOrEmpty(x.Value) && !x.Value.ToLower().Contains('e')).DistinctBy(x => x.Date);
                var selectedTrend = listOfTrends.Where(a => a.Name == item.Key)?.FirstOrDefault();
                thresholdValues.TryGetValue(selectedTrend?.HighParamType, out var maxValues);
                thresholdValues.TryGetValue(selectedTrend?.LowParamType, out var minValues);

                DataPointsModelDto dataPointsModelDto = new DataPointsModelDto();
                if (values.ToList().Any())
                {
                    dataPointsModelDto.TrendName = values.Select(x => x.Name).FirstOrDefault();
                    dataPointsModelDto.ParamTypeId = values.Select(x => x.ParamTypeId).FirstOrDefault();
                    dataPointsModelDto.Address = values.Select(x => x.Address).FirstOrDefault();
                    dataPointsModelDto.Displayorder = Convert.ToInt32(listOfTrends.FirstOrDefault(x => x.Pst == dataPointsModelDto.ParamTypeId).DisplayOrder);
                    dataPointsModelDto.UnitOfMeasure = values.Select(x => x.UOM).FirstOrDefault();
                    dataPointsModelDto.Short_UnitOfMeasure = values.Select(x => x.Short_UOM).FirstOrDefault();
                    dataPointsModelDto.Date = values.Select(x => x.Date).OrderByDescending(p => p.Date).FirstOrDefault();
                    dataPointsModelDto.Value = values.OrderByDescending(s => s.Date).Select(x => x.Value).FirstOrDefault();
                    dataPointsModelDto.Min = values.Any(x => !string.IsNullOrEmpty(x.Value)) ? values.Min(x => x.Value) : "0";
                    dataPointsModelDto.Max = values.Any(x => !string.IsNullOrEmpty(x.Value)) ? values.Max(x => x.Value) : "0";
                    dataPointsModelDto.Median = values.Any(x => !string.IsNullOrEmpty(x.Value)) ? Math.Round(values.Average(x => Convert.ToDecimal(x.Value)), 3).ToString() : "0";                    
                    dataPointsModelDto.DataPoints = values.ToList();
                    dataPointsModelDto.MinThresholdValues = minValues?.Where(minValues => !string.IsNullOrEmpty(minValues.Value)).ToList();
                    dataPointsModelDto.MaxThresholdValues = maxValues?.Where(maxValues => !string.IsNullOrEmpty(maxValues.Value)).ToList();
                }
                else
                {
                    dataPointsModelDto.TrendName = item.Value.Select(x => x.Name).FirstOrDefault();
                    dataPointsModelDto.ParamTypeId = item.Value.Select(x => x.ParamTypeId).FirstOrDefault();
                    dataPointsModelDto.Address = item.Value.Select(x => x.Address).FirstOrDefault();
                    dataPointsModelDto.Displayorder = Convert.ToInt32(listOfTrends.FirstOrDefault(x => x.Pst == dataPointsModelDto.ParamTypeId).DisplayOrder);
                    dataPointsModelDto.UnitOfMeasure = item.Value.Select(x => x.UOM).FirstOrDefault();
                    dataPointsModelDto.Short_UnitOfMeasure = item.Value.Select(x => x.Short_UOM).FirstOrDefault();
                    dataPointsModelDto.Date = item.Value.Select(x => x.Date).OrderByDescending(p => p.Date).FirstOrDefault();                   
                }

                list.Add(dataPointsModelDto);
            }

            var paraTypesFromDB = list.Select(x => x.TrendName).ToList();
            var paraTypesNotInDB = listOfTrends.ExceptBy(paraTypesFromDB, e => e.Name);

            foreach (var item in paraTypesNotInDB)
            {
                DataPointsModelDto dataPointsModelDto = new DataPointsModelDto();
                dataPointsModelDto.TrendName = item.Name;
                dataPointsModelDto.Displayorder = Convert.ToInt32(item.DisplayOrder);

                list.Add(dataPointsModelDto);
            }

            return list.OrderBy(x => x.Displayorder).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unitOfMeasure"></param>
        /// <returns></returns>
        private static string GetUnitOfMeasureShortForm(string unitOfMeasure)
        {
            var unit = (unitOfMeasure?.ToLower()) switch
            {
                "pressure" => Pressure.PoundPerSquareInch.Symbol,
                "frequency" => Frequency.Hertz.Symbol,
                "temperature" => Temperature.DegreeFahrenheit.Symbol,
                "length" => Length.Foot.Symbol,
                "speed" => Speed.FootPerMinute.Symbol,
                "rotationalspeed" => RotationalSpeed.Hertz.Symbol,
                string tempUOM when tempUOM.Contains("voltage") => Voltage.Volt.Symbol,
                string tempUOM when tempUOM.Contains("current") => Current.Ampere.Symbol,
                "acceleration from gravity" => "g",
                //case "inulation Resistance":
                //    unit = Resi.Ampere.Symbol;
                //    break;
                //case "vibration":
                //    unit = Vi.Ampere.Symbol;
                //    break;
                _ => unitOfMeasure,
            };
            return unit;
        }

        #endregion

        private DateTime GetTimeZoneAdjustedTime<TLogger>(float nodeTimeZoneOffset, bool honorDaylighSaving, DateTime scanTime,
            string correlationId, LoggingModelBase<TLogger> loggingModel) where TLogger : Enum
        {
            //var logger = _loggerFactory.Create(loggingModel);

            var timezoneInfo = _dateTimeConverter.GetTimeZoneInfo(correlationId, loggingModel);

            // If time zone is currently in daylight saving, set daylightSaving flag to true
            bool daylightSaving = false;
            if (timezoneInfo.IsDaylightSavingTime(scanTime))
            {
                //logger.WriteCId(Level.Debug, "Time Zone is in daylight savings.", correlationId);

                daylightSaving = true;
            }

            // If tzoffset=0, then do not apply any dst corrections - just adjust based on server
            TimeSpan daylightSavingBias = new TimeSpan(0);
            if (daylightSaving && nodeTimeZoneOffset != 0 && honorDaylighSaving)
            {
                var dt = DateTime.UtcNow;

                daylightSavingBias = timezoneInfo.BaseUtcOffset - timezoneInfo.GetUtcOffset(dt);

                //logger.WriteCId(Level.Debug, $"Adding daylight savings bias {daylightSavingBias}.", correlationId);
            }

            int offSetMinutes = (int)(nodeTimeZoneOffset * 60);
            TimeSpan timeZoneOffset = new TimeSpan(0, offSetMinutes, 0);

            var result = scanTime + timeZoneOffset + daylightSavingBias;

            //logger.WriteCId(Level.Debug,
              //  $"Adjusting time to original scan time {scanTime}, time zone offset {timeZoneOffset} with daylight" +
               // $" saving bias {daylightSavingBias}.", correlationId);

            //logger.WriteCId(Level.Debug, $"New adjusted time {result}.", correlationId);

            return result;
        }

    }
}