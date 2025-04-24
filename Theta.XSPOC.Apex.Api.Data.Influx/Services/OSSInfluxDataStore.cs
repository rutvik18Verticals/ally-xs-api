using InfluxDB.Client.Writes;
using MathNet.Numerics;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Common.WorkflowModels;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data.Influx.Services
{
    /// <summary>
    /// This implementation represents fetching trend data from OSS Influx Data
    /// </summary>
    public class OSSInfluxDataStore : IDataHistoryTrendData
    {

        #region Private Fields

        private readonly IOSSInfluxClientFactory _influxDbClient;
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
        /// </exception> 
        public OSSInfluxDataStore(IOSSInfluxClientFactory ossInfluxDbClient, IConfiguration appConfig)
        {
            _influxDbClient = ossInfluxDbClient ?? throw new ArgumentNullException(nameof(ossInfluxDbClient));
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
            int pageLimit= pageSize != 0? pageSize:Convert.ToInt32(_pageSize);
            
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
                   
                    queryBuilder.Append($" |> aggregateWindow(every: "+ downsampleWindowSize + ", fn: "+downsampleType+", createEmpty:false)");

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

        #endregion

    }
}