using Microsoft.Extensions.Configuration;
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
using Theta.XSPOC.Apex.Kernel.Quantity.Measures;
using Theta.XSPOC.Apex.Kernel.Utilities;

namespace Theta.XSPOC.Apex.Api.Data.Influx.Services
{
    /// <summary>
    /// This implementation represents fetching trend data from Enterprise Influx Data
    /// </summary>
    public class EnterpriseInfluxDataStore : IDataHistoryTrendData
    {

        #region Private Fields

        private readonly IEnterpriseInfluxClientFactory _influxDbClient;
        private readonly IDateTimeConverter _dateTimeConverter;
        private string _bucketName;
        private string _measurement;
        private string _pageSize;

        #endregion

        #region Constants

        private const string BUCKET_NAME = "xspoc";
        private const string MEASUREMENT = "XSPOCData";
        //private const double DEFAULT_WINDOW_SIZE = 750;
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
        /// Constructs a new <seealso cref="EnterpriseInfluxDataStore"/> using the provided configuration.
        /// </summary>
        /// <param name="enterpriseInfluxDbClient">The <seealso cref="IEnterpriseInfluxClientFactory"/>.</param>       
        /// <param name="appConfig">The <seealso cref="IConfiguration"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="enterpriseInfluxDbClient"/> is null.
        /// <param name="dateTimeConverter">The <seealso cref="IDateTimeConverter"/>.</param>   
        /// </exception> 
        public EnterpriseInfluxDataStore(IEnterpriseInfluxClientFactory enterpriseInfluxDbClient, IConfiguration appConfig, IDateTimeConverter dateTimeConverter)
        {
            _influxDbClient = enterpriseInfluxDbClient ?? throw new ArgumentNullException(nameof(enterpriseInfluxDbClient));
            _dateTimeConverter = dateTimeConverter ?? throw new ArgumentNullException(nameof(dateTimeConverter));
            AppConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
            SetConfigurationOnInit();
        }

        #endregion

        #region IDataHistoryTrendData Implementation

        /// <summary>
        /// Method to fetch the trend data from influx data store with <paramref name="channelIds"/>. 
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

            var sDate = DateTime.Parse(startDate).ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ");
            var eDate = DateTime.Parse(endDate).ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ");

            DbStoreResult result;
            try
            {
                var client = _influxDbClient.Create();
                var queryBuilder = new StringBuilder();

                if (channelIds != null && channelIds.Count > 0)
                {
                    queryBuilder.Append($"SELECT time, AssetID, CustomerID, POCType, ");
                    if (channelIds.Count > 1)
                    {
                        var multiAddrQuery = new StringBuilder();
                        foreach (var channel in channelIds)
                        {
                            if (!string.IsNullOrEmpty(channel))
                            {
                                multiAddrQuery.Append($"{channel}, ");
                            }
                        }
                        var mQuery = multiAddrQuery.ToString()[..^2];
                        queryBuilder.Append(mQuery);
                    }
                    else
                    {
                        queryBuilder.Append($"{channelIds[0]}");
                    }

                    queryBuilder.Append($" FROM \"{_measurement}\" WHERE ");
                    queryBuilder.Append($"\"AssetID\" = \'{assetId}\' ");

                    if (customerId != Guid.Empty)
                    {
                        queryBuilder.Append($"and \"CustomerID\" = '{customerId}' ");
                    }

                    if (!string.IsNullOrEmpty(pocType))
                    {
                        queryBuilder.Append($" and \"POCType\" = '{pocType}' ");
                    }

                    queryBuilder.Append($" and time >= \'{sDate}\' and time <= \'{eDate}\'");
                }

                var query = queryBuilder.ToString();

                var data = await client.Client.QueryAsync(query, _bucketName);
                if (data != null && data.Any())
                {
                    var dataPoints = new List<DataPointModel>();

                    var records = data.FirstOrDefault();
                    var columns = records.Columns;
                    foreach (var record in records.Values)
                    {
                        var dataPoint = new DataPointModel
                        {
                            Time = DateTime.Parse(record[0].ToString(), null, DateTimeStyles.AdjustToUniversal),
                        };
                        var columnValues = new Dictionary<string, string>();
                        foreach (var column in columns)
                        {
                            columnValues.Add(column.ToString(), record[columns.IndexOf(column)]?.ToString());
                        }
                        dataPoint.ColumnValues = columnValues;
                        dataPoints.Add(dataPoint);
                    }

                    foreach (var channel in channelIds)
                    {
                        foreach (var dp in dataPoints)
                        {
                            if (dp.ColumnValues.TryGetValue(channel, out var value))
                            {
                                responseDataPoints.Add(new DataPointModel
                                {
                                    Time = dp.Time,
                                    Value = value,
                                    TrendName = channel
                                });
                            }
                        }
                    }

                    result = new DbStoreResult()
                    {
                        Message = string.Empty,
                        KindOfError = ErrorType.None,
                    };
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
            finally
            {
                // Dispose the client
                _influxDbClient.Dispose();
            }

            return responseDataPoints;
        }

        /// <summary>
        /// Method to fetch the trend data from influx data store with <paramref name="channelIds"/>. 
        /// </summary>
        /// <param name="assetIds">The asset guid.</param>
        /// <param name="customerId">The customer guid.</param>        
        /// <param name="channelIds">The list of channel ids.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="downsampleType">The down sample type.</param>
        /// <param name="downsampleWindowSize">The down sample window size.</param>
        /// <param name="pageNum">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The <seealso cref="IList{DataPointModel}"/></returns>
        public async Task<IList<DataPointModel>> GetAllyTimeSeriesTrendData(List<Guid> assetIds, Guid customerId,
           List<string> channelIds, string startDate, string endDate, string downsampleType, string downsampleWindowSize,
            int pageNum, int pageSize)
        {
            assetIds = new List<Guid>();
            assetIds.Add(Guid.Parse("1338bed4-6e21-404f-a2d1-bd9c44836bfb"));
            assetIds.Add(Guid.Parse("1d7fe3dc-5a03-48c7-be8b-aa2296ab2338"));
            customerId = Guid.Parse("21571790-68c1-4f16-86fc-f9e06bce9312");
            channelIds = new List<string>();
            channelIds.Add("C116");
            channelIds.Add("C132");
            channelIds.Add("C343");
            //startDate = "2023-12-31T18:30:00Z";
            //endDate = "2024-12-31T22:30:00Z";

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
                var client = _influxDbClient.Create();
                var queryBuilder = new StringBuilder();

                if (channelIds != null && channelIds.Count > 0)
                {
                    queryBuilder.Append($"SELECT time, ");
                    if (channelIds.Count > 1)
                    {
                        var multiAddrQuery = new StringBuilder();
                        foreach (var channel in channelIds)
                        {
                            if (!string.IsNullOrEmpty(channel))
                            {
                                multiAddrQuery.Append($"{downsampleType}({channel}) AS {channel}, ");
                            }
                        }
                        var mQuery = multiAddrQuery.ToString()[..^2];
                        queryBuilder.Append(mQuery);
                    }
                    else
                    {
                        queryBuilder.Append($"{channelIds[0]}");
                    }

                    queryBuilder.Append($" FROM \"{_measurement}\" WHERE ");
                    //queryBuilder.Append($"\"AssetID\" = \'{assetId}\' ");

                    foreach (var assetId in assetIds)
                    {
                        if (assetId == assetIds.First())
                        {
                            queryBuilder.Append($" (\"AssetID\" = '{assetId}' ");
                        }
                        else
                        {
                            queryBuilder.Append($" OR \"AssetID\" = '{assetId}' ");
                        }

                        if (assetId == assetIds.Last())
                        {
                            queryBuilder.Append(')');
                        }
                    }

                    if (customerId != Guid.Empty)
                    {
                        queryBuilder.Append($"and \"CustomerID\" = '{customerId}' ");
                    }

                    queryBuilder.Append($" and time >= \'{startDate}\' and time <= \'{endDate}\'");

                    queryBuilder.Append($"GROUP BY AssetID,POCType, time({downsampleWindowSize})");

                    if (pageNum > 0)
                    {
                        int offset = (pageNum - 1) * pageLimit + 1;

                        queryBuilder.Append($"limit {pageLimit} offset {offset}");
                    }

                }

                var query = queryBuilder.ToString();

                var dataSet = await client.Client.QueryAsync(query, _bucketName);
                if (dataSet != null && dataSet.Any())
                {
                    foreach (var data in dataSet)
                    {
                        var assetID = data.Tags["AssetID"].ToString();
                        var pocTypeID = data.Tags["POCType"].ToString();

                        foreach (var item in data.Values)
                        {
                            var tryDateTime = DateTime.TryParse(item[0]?.ToString(), null, DateTimeStyles.AdjustToUniversal, out var dateResult);
                            if (tryDateTime == false)
                            {
                                continue;
                            }

                            for (int i = 1; i < item.Count; i++)
                            {
                                responseDataPoints.Add(new DataPointModel
                                {
                                    AssetId = assetID,
                                    POCTypeId = pocTypeID,
                                    TimeOfTimeSeries = dateResult.ToString("yyyy-MM-ddTHH:mm:ssZ"),//DateTime.Parse(recordTime).ToUniversalTime().ToString(),//DateTime.TryParse(recordTime,.ToString("yyyy-MM-ddTHH:mm:ssZ"),                                                                      
                                    ValueOfTimeSeries = double.TryParse(item[i]?.ToString(), out var value) ? value : 0,
                                    ChannelId = data.Columns[i]
                                });
                            }
                            //var columnValues = new Dictionary<string, string>();
                            //foreach (var column in columns)
                            //{
                            //    columnValues.Add(column.ToString(), record[columns.IndexOf(column)]?.ToString());
                            //}
                            //dataPoint.ColumnValues = columnValues;
                            //responseDataPoints.Add(dataPoint);
                        }
                    }
                    result = new DbStoreResult()
                    {
                        Message = string.Empty,
                        KindOfError = ErrorType.None,
                    };
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
            finally
            {
                // Dispose the client
                _influxDbClient.Dispose();
            }

            return responseDataPoints;
        }

        /// <summary>
        /// Method to fetch the trend data from influx data store with <paramref name="channelIds"/>. 
        /// </summary>
        /// <param name="assetIds">The asset guid.</param>
        /// <param name="customerId">The customer guid.</param>        
        /// <param name="channelIds">The list of channel ids.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="downsampleType">The down sample type.</param>
        /// <param name="downsampleWindowSize">The down sample window size.</param>
        /// <param name="pageNum">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="inputs">Asset details.</param>
        /// <returns>The <seealso cref="IList{DataPointModel}"/></returns>
        public async Task<IList<TimeSeriesData>> GetTimeSeriesResponse(List<Guid> assetIds, Guid customerId,
       List<string> channelIds, string startDate, string endDate, string downsampleType, string downsampleWindowSize,
        int pageNum, int pageSize, List<TimeSeriesInputModel> inputs)
        {
            var responseTrendData = new List<TimeSeriesData>();
            int pageLimit = pageSize != 0 ? pageSize : Convert.ToInt32(_pageSize);

            if (assetIds == null || assetIds.Count == 0 || assetIds[0] == Guid.Empty || customerId == Guid.Empty
                || channelIds == null || channelIds.Count == 0 || inputs == null || inputs.Count == 0)
            {
                return null;
            }

            DbStoreResult result;
            try
            {
                var client = _influxDbClient.Create();
                var countQueryBuilder = new StringBuilder();
                var dataQueryBuilder = new StringBuilder();
                var queryBuilder = new StringBuilder();

                if (channelIds != null && channelIds.Count > 0)
                {
                    countQueryBuilder.Append($"SELECT count(AssetID) FROM ( ");
                    dataQueryBuilder.Append($"SELECT * FROM ( ");

                    queryBuilder.Append($"SELECT ");
                    if (channelIds.Count > 0)
                    {
                        var multiAddrQuery = new StringBuilder();
                        foreach (var channel in channelIds)
                        {
                            if (!string.IsNullOrEmpty(channel))
                            {
                                multiAddrQuery.Append($"{downsampleType}({channel}) AS {channel}, ");
                            }
                        }
                        var mQuery = multiAddrQuery.ToString()[..^2];
                        queryBuilder.Append(mQuery);
                    }

                    queryBuilder.Append($" FROM \"{_measurement}\" WHERE ");
                    //queryBuilder.Append($"\"AssetID\" = \'{assetId}\' ");

                    foreach (var assetId in assetIds)
                    {
                        if (assetId == assetIds.First())
                        {
                            queryBuilder.Append($" (\"AssetID\" = '{assetId}' ");
                        }
                        else
                        {
                            queryBuilder.Append($" OR \"AssetID\" = '{assetId}' ");
                        }

                        if (assetId == assetIds.Last())
                        {
                            queryBuilder.Append(')');
                        }
                    }

                    if (customerId != Guid.Empty)
                    {
                        queryBuilder.Append($"and \"CustomerID\" = '{customerId}' ");
                    }

                    queryBuilder.Append($" and time >= \'{startDate}\' and time <= \'{endDate}\'");

                    queryBuilder.Append($"GROUP BY AssetID,POCType, time({downsampleWindowSize}) )");

                    dataQueryBuilder.Append(queryBuilder);

                    dataQueryBuilder.Append($"order by time ");

                    if (pageNum > 0)
                    {
                        int offset = (pageNum - 1) * pageLimit;

                        dataQueryBuilder.Append($" limit {pageLimit} offset {offset}");
                    }

                    //Total Count query
                    countQueryBuilder.Append(queryBuilder);
                }

                var query = dataQueryBuilder.ToString();

                var dataSet = await client.Client.QueryAsync(query, _bucketName);
                if (dataSet != null && dataSet.Any())
                {
                    foreach (var data in dataSet)
                    {
                        var columns = data.Columns;
                        var records = data.Values;

                        foreach (var record in records)
                        {
                            var resultSet = new TimeSeriesData { ChannelIds = new List<string>(), Values = new List<double?>() };
                            foreach (var column in columns)
                            {
                                if (column == "AssetID")
                                {
                                    resultSet.AssetId = record[columns.IndexOf(column)]?.ToString();
                                    continue;
                                }
                                if (column == "POCType")
                                {
                                    resultSet.POCTypeId = record[columns.IndexOf(column)]?.ToString();
                                    continue;
                                }
                                if (column == "time")
                                {
                                    if (DateTime.TryParse(record[columns.IndexOf(column)]?.ToString(), null, DateTimeStyles.AdjustToUniversal, out var dateResult))
                                    {
                                        resultSet.Timestamp = dateResult.ToString("yyyy-MM-ddTHH:mm:ssZ");
                                    }
                                    continue;
                                }

                                if (double.TryParse(record[data.Columns.IndexOf(column)]?.ToString(), out var value))
                                {
                                    var item = inputs.Where(x => x.AssetId.ToString() == resultSet.AssetId.ToString()
                                              && x.ChannelIds.Contains(column)).ToList();
                                    if (item != null)
                                    {
                                        if (item.Count > 0)
                                        {
                                            resultSet.ChannelIds.Add(column.ToString());
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
                                            resultSet.ChannelIds.Add(column.ToString());
                                            resultSet.Values.Add(null);
                                        }
                                    }
                                }
                            }
                            responseTrendData.Add(resultSet);
                        }
                    }

                    if (responseTrendData.Count > 1)
                    {
                        var totalCntquery = countQueryBuilder.ToString();

                        var totalCntdataSet = await client.Client.QueryAsync(totalCntquery, _bucketName);
                        if (totalCntdataSet != null && totalCntdataSet.Any())
                        {
                            foreach (var data in totalCntdataSet)
                            {
                                if (Int32.TryParse(data.Values[0][1]?.ToString(), out var cnt1))
                                {
                                    responseTrendData[0].TotalCount = cnt1;
                                    int pages = Math.DivRem(responseTrendData[0].TotalCount, pageLimit, out var remainder);
                                    if (remainder != 0)
                                    {
                                        responseTrendData[0].TotalPages = (pages + 1);
                                    }
                                    else
                                    {
                                        responseTrendData[0].TotalPages = pages;
                                    }
                                }
                            }

                        }
                    }
                }

                result = new DbStoreResult()
                {
                    Message = string.Empty,
                    KindOfError = ErrorType.None,
                };
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
            finally
            {
                // Dispose the client
                _influxDbClient.Dispose();
            }

            return responseTrendData;
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

            if (assetId == Guid.Empty || customerId == Guid.Empty ||
                string.IsNullOrEmpty(pocType) || ((addresses == null || addresses.Count == 0) &&
                (paramStandardType == null || paramStandardType.Count == 0)))
            {
                return null;
            }

            var sDate = DateTime.Parse(startDate).ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ");
            var eDate = DateTime.Parse(endDate).ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ");

            DbStoreResult result;
            try
            {
                var client = _influxDbClient.Create();
                var queryBuilder = new StringBuilder();
                /* 
                startDate = new DateTime(2022, 9, 1).ToString(); //to be removed
                endDate = new DateTime(2022, 12, 30).ToString(); //to be removed
                 SELECT  time, AssetID, CustomerID, POCType, C10101, C10102, C10103, C10104, C10105 FROM "XSPOCData" WHERE "AssetID" = '000112d0-95c0-47a7-af8a-4448906939a9' 
                    AND "CustomerID" = '870d0c60-e132-4f74-945b-94403a802924'  --AND "POCType" = '8' 
                    AND time >= '2023-04-01T00:00:00.000000000Z' AND time <= '2023-04-09T00:00:00.000000000Z'
                 
                assetId = new Guid("2728b065-8dbe-4bec-9c87-d179baad6362"); //to be removed
                customerId = new Guid("ae5595a0-3b82-4f42-ac26-347feebab28d"); //to be removed
                pocType = "8"; //to be removed
                */
                if (addresses != null && addresses.Count > 0)
                {
                    queryBuilder.Append($"SELECT time, AssetID, CustomerID, POCType, ");
                    if (addresses.Count > 1)
                    {
                        var multiAddrQuery = new StringBuilder();
                        foreach (var addr in addresses)
                        {
                            if (!string.IsNullOrEmpty(addr))
                            {
                                multiAddrQuery.Append($"C{addr}, ");
                            }
                        }
                        var mQuery = multiAddrQuery.ToString()[..^2];
                        queryBuilder.Append(mQuery);
                    }
                    else
                    {
                        queryBuilder.Append($"{addresses[0]}");
                    }

                    queryBuilder.Append($" FROM \"{_bucketName}\" WHERE ");
                    queryBuilder.Append($"\"AssetID\" = \'{assetId}\' ");

                    if (customerId != Guid.Empty)
                    {
                        queryBuilder.Append($"and \"CustomerID\" = '{customerId}' ");
                    }

                    if (!string.IsNullOrEmpty(pocType))
                    {
                        queryBuilder.Append($" and \"POCType\" = '{pocType}' ");
                    }

                    queryBuilder.Append($" and time >= \'{sDate}\' and time <= \'{eDate}\'");
                }

                var query = queryBuilder.ToString();

                var data = await client.Client.QueryAsync(query, _bucketName);
                if (data != null && data.Any())
                {
                    var records = data.FirstOrDefault();
                    var columns = records.Columns;
                    foreach (var record in records.Values)
                    {
                        var dataPoint = new DataPointModel
                        {
                            Time = DateTime.Parse(record[0].ToString(), null, DateTimeStyles.AdjustToUniversal),
                        };
                        var columnValues = new Dictionary<string, string>();
                        foreach (var column in columns)
                        {
                            columnValues.Add(column.ToString(), record[columns.IndexOf(column)]?.ToString());
                        }
                        dataPoint.ColumnValues = columnValues;
                        responseDataPoints.Add(dataPoint);
                    }

                    result = new DbStoreResult()
                    {
                        Message = string.Empty,
                        KindOfError = ErrorType.None,
                    };
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
            finally
            {
                // Dispose the client
                _influxDbClient.Dispose();
            }

            return responseDataPoints;
        }

        /// <summary>
        /// Gets the downtime data from InfluxDB stores based on the provided filters, start date, and end date.
        /// </summary>
        /// <param name="filterData">The list of downtime filters.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns>The <seealso cref="IList{DowntimeByWellsInfluxModel}"/></returns>
        public Task<IList<DowntimeByWellsInfluxModel>> GetDowntimeAsync(IList<DowntimeFiltersInflux> filterData,
            string startDate, string endDate)
        {
            throw new NotImplementedException();
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
                var client = _influxDbClient.Create();
                var queryBuilder = new StringBuilder();

                foreach (var measurementData in filterData)
                {
                    var assetIds = measurementData.AssetIds.Where(x => x != Guid.Empty).ToList();
                    var customerIds = measurementData.CustomerIds.Where(x => x != Guid.Empty).ToList();

                    if (assetIds.Count == 0 || customerIds.Count == 0)
                    {
                        continue;
                    }

                    queryBuilder.Append(GenerateInfluxQLMeasurementQuery(measurementData.ChannelIds,
                        assetIds, customerIds, startDate, endDate));

                    var query = queryBuilder.ToString();

                    var data = await client.Client.QueryAsync(queryBuilder.ToString(), _bucketName);
                    if (data != null && data.Any())
                    {
                        var records = data.FirstOrDefault();
                        foreach (var record in records.Values)
                        {
                            result.Add(new DowntimeByWellsInfluxModel
                            {
                                Date = DateTime.Parse(record[0].ToString(), null, DateTimeStyles.AdjustToUniversal),
                                Value = float.TryParse(record[2]?.ToString(), out var valueFloat) ? valueFloat : 0,
                                Id = record[1]?.ToString(),
                                //ParamStandardType = record.GetValueByKey("ParamStandardType").ToString(),
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
                var client = _influxDbClient.Create();
                var queryBuilder = new StringBuilder();

                if (channelIds != null && channelIds.Count > 0)
                {
                    queryBuilder.Append($"SELECT time, AssetID, CustomerID, POCType, ");
                    if (channelIds.Count > 1)
                    {
                        var multiAddrQuery = new StringBuilder();
                        foreach (var channel in channelIds)
                        {
                            if (!string.IsNullOrEmpty(channel))
                            {
                                multiAddrQuery.Append($"{channel}, ");
                            }
                        }
                        var mQuery = multiAddrQuery.ToString()[..^2];
                        queryBuilder.Append(mQuery);
                    }
                    else
                    {
                        queryBuilder.Append($"{channelIds[0]}");
                    }

                    queryBuilder.Append($" FROM \"{_measurement}\" WHERE ");
                    queryBuilder.Append($"\"AssetID\" = \'{assetId}\' ");

                    if (customerId != Guid.Empty)
                    {
                        queryBuilder.Append($"and \"CustomerID\" = '{customerId}' ");
                    }

                    if (!string.IsNullOrEmpty(pocType))
                    {
                        queryBuilder.Append($" and \"POCType\" = '{pocType}' ");
                    }

                    queryBuilder.Append(" order by time desc limit 1");
                }

                var query = queryBuilder.ToString();

                var data = await client.Client.QueryAsync(query, _bucketName);
                if (data != null && data.Any())
                {
                    var records = data.FirstOrDefault();
                    var columns = records.Columns;
                    foreach (var record in records.Values)
                    {
                        var dataPoint = new DataPointModel
                        {
                            Time = DateTime.Parse(record[0].ToString(), null, DateTimeStyles.AdjustToUniversal),
                        };
                        var columnValues = new Dictionary<string, string>();
                        foreach (var column in columns)
                        {
                            columnValues.Add(column.ToString(), record[columns.IndexOf(column)]?.ToString());
                        }
                        dataPoint.ColumnValues = columnValues;
                        responseDataPoints.Add(dataPoint);
                    }

                    result = new DbStoreResult()
                    {
                        Message = string.Empty,
                        KindOfError = ErrorType.None,
                    };
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
            finally
            {
                // Dispose the client
                _influxDbClient.Dispose();
            }

            return responseDataPoints;
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
            var lastRecord = await QueryInfluxDataLastOneRecord(new List<string> { assetId.ToString() }, channelIds, startDate, endDate, new List<string> { pocType.ToString() });

            if (influxData == null || !influxData.Any())
            {
                return null;
            }
            if (lastRecord?.FirstOrDefault()?.Values?.FirstOrDefault()?.Any() == true)
            {
                influxData.FirstOrDefault().Values.Insert(0, lastRecord.FirstOrDefault().Values.FirstOrDefault());
            }

            var dataPointDictionery = new Dictionary<string, List<DataPointModelDto>>();
            var thresholdValuesDictionery = new Dictionary<string, List<DataPointModelDto>>();

            for (int i = 0; i < influxData.FirstOrDefault().Columns.Count; i++)
            {
                bool isThresholdValue = false;
                string unitOfMeasure = string.Empty;
                var influxValues = new List<DataPointModelDto>();
                //var parameterDetails = parameters.Where(x => x.ChannelId == influxData.FirstOrDefault().Columns[i]).FirstOrDefault();
                var parameterDetails = parameters.FirstOrDefault(x => x.ChannelId == influxData.FirstOrDefault().Columns[i]);
                if (parameterDetails != null)
                {
                    string parameterLegacyId = parameterDetails.ParamStandardType.LegacyId.FirstOrDefault().Value;
                    var paramName = listOfTrends.FirstOrDefault(x => x.Pst == parameterLegacyId)?.Name;

                    if (string.IsNullOrEmpty(paramName))
                    {
                        paramName = parameterLegacyId;
                        isThresholdValue = true;
                    }

                    //var facilityTagResult = parameters.Where(x => x.ParameterType == "Facility" && x.Name.Split('|').Contains(wellName) && x.ChannelId == influxData.FirstOrDefault().Columns[i]).FirstOrDefault();

                    var facilityTagResult = parameters.FirstOrDefault(x => x.ParameterType == "Facility" && x.Name.Split('|').Contains(wellName) && x.ChannelId == influxData.FirstOrDefault().Columns[i]);
                    if (facilityTagResult != null)
                    {
                        unitOfMeasure = facilityTagResult.UnitOfMeasure;
                    }
                    else
                    {
                        unitOfMeasure = parameterDetails.UnitOfMeasure;
                    }

                    var unit = GetUnitOfMeasureShortForm(unitOfMeasure);

                    foreach (var record in influxData.SelectMany(d => d.Values).Select((value, index) => (value, index)))
                    {
                        var dt = _dateTimeConverter.ConvertToApplicationServerTimeFromUTC(DateTime.Parse(record.value[0].ToString()), string.Empty, LoggingModel.SqlDateTime);
                        var dtResponse = _dateTimeConverter.GetTimeZoneAdjustedTime(nodeTimeZoneOffset, honorDaylighSaving, dt, string.Empty, LoggingModel.SqlDateTime);
                        influxValues.Add(new DataPointModelDto
                        {
                            Name = paramName,
                            Date = dtResponse,
                            Value = !string.IsNullOrEmpty(record.value[i].ToString()) ? 
                            MathUtility.RoundToSignificantDigits(Convert.ToDouble(record.value[i]), 3).ToString() : Convert.ToString(record.value[i]),//Convert.ToString(record.value[i]),
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
        /// Gets the downtime data from InfluxDB stores based on the assetId, start date, and end date.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="channelId">The channel id.</param>
        /// <returns>The influx data for downtime PST in the form of <seealso cref="IList{DataPointModel}"/>.</returns>
        public async Task<IList<DataPointModel>> GetDowntime(Guid assetId, DateTime startDate, DateTime endDate, string channelId)
        {
            DbStoreResult result;

            try
            {
                var queryBuilder = new StringBuilder();
                queryBuilder.Append($"SELECT time, {channelId}");
                queryBuilder.Append($" FROM {MEASUREMENT} WHERE ");
                queryBuilder.Append($"\"AssetID\" = \'{assetId}\' ");
                var startDateFormat = "yyyy-MM-ddTHH:mm:ss.fffffffZ";
                var endDateFormat = endDate.TimeOfDay.TotalMilliseconds > 0 ? startDateFormat : "yyyy-MM-ddT23:59:59.fffffffZ";

                var sDate = DateTime.Parse(startDate.ToString()).ToString(startDateFormat);
                var eDate = DateTime.Parse(endDate.ToString()).ToString(endDateFormat);
                queryBuilder.Append($" and time >= \'{sDate}\' and time <= \'{eDate}\'");
                queryBuilder.Append($" order by time ");

                //Step 3: Execute Influx Query
                var client = _influxDbClient.Create();
                var query = queryBuilder.ToString();
                result = new DbStoreResult()
                {
                    Message = string.Empty,
                    KindOfError = ErrorType.None,
                };

                var response = new List<DataPointModel>();

                var responseData = await client.Client.QueryAsync(query, _bucketName);

                foreach (var record in responseData.SelectMany(d => d.Values).Select((value, index) => (value, index)))
                {
                    response.Add(new DataPointModel
                    {
                        Time = DateTime.Parse(record.value[0].ToString(), null, DateTimeStyles.AdjustToUniversal),
                        Value = Convert.ToString(record.value[1]),
                        ChannelId = channelId,
                    });
                }

                return response;

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
            finally
            {
                // Dispose the client
                _influxDbClient.Dispose();
            }

            return null;
        }

        #endregion

        # region GetCurrentRawScanData Implementation

        /// <summary>
        /// Method to fetch the GetCurrentRawScanData from influx data store />. 
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <returns>The <seealso cref="IList{DataPointModel}"/></returns>
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
                var client = _influxDbClient.Create();
                var queryBuilder = new StringBuilder();

                // Query to select the latest value and time per ChannelId for a specific AssetID
                queryBuilder.Append($"SELECT LAST(\"_value\") AS value, LAST(\"_time\") AS time, \"POCType\", \"_field\", \"AssetID\" ");
                queryBuilder.Append($"FROM \"{_measurement}\" ");
                queryBuilder.Append($"WHERE \"AssetID\" = '{assetId}' ");
                queryBuilder.Append($"AND time > now() - 1y ");
                queryBuilder.Append($"GROUP BY \"ChannelId\" ");
                queryBuilder.Append($"ORDER BY time DESC");

                string query = queryBuilder.ToString();

                var data = await client.Client.QueryAsync(query, _bucketName);

                if (data != null && data.Any())
                {
                    foreach (var records in data)
                    {
                        var record = records.Values;
                        var currentRawScanDataInflux = new CurrentRawScanDataInfluxModel
                        {
                            Value = float.TryParse(record[0]?.ToString(), out var valueData) ? valueData : default,
                            DateTimeUpdated = DateTime.Parse(record[1]?.ToString() ?? DateTime.UtcNow.ToString(), null, DateTimeStyles.AdjustToUniversal),
                            POCType = record[2]?.ToString(),
                            ChannelId = record[3]?.ToString()
                        };
                        response.Add(currentRawScanDataInflux);
                    }
                }
                else
                {
                    result.Message = "No data found for the specified AssetID";
                    result.KindOfError = ErrorType.LikelyRecoverable;
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
            finally
            {
                _influxDbClient.Dispose();
            }

            return response;
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
                var client = _influxDbClient.Create();
                var queryBuilder = new StringBuilder();

                // Query to select the latest value and time per ChannelId for a specific AssetID
                queryBuilder.Append($"SELECT * ");
                queryBuilder.Append($"FROM \"{_measurement}\" ");
                queryBuilder.Append($"WHERE \"AssetID\" = '{assetId}' ");
                queryBuilder.Append($"and time > now() - 1y "); // filter data from last 1 year only.

                if (customerId != Guid.Empty)
                {
                    queryBuilder.Append($"and \"CustomerID\" = '{customerId}' ");
                }
                queryBuilder.Append($"group by \"AssetID\" ");
                queryBuilder.Append(" order by time desc limit 1");

                string query = queryBuilder.ToString();

                var data = await client.Client.QueryAsync(query, _bucketName);
                if (data != null && data.Any())
                {
                    var records = data.FirstOrDefault();
                    var columns = records.Columns;
                    foreach (var record in records.Values)
                    {
                        var dataPoint = new DataPointModel
                        {
                            Time = DateTime.Parse(record[0].ToString(), null, DateTimeStyles.AdjustToUniversal),
                        };
                        var columnValues = new Dictionary<string, string>();
                        foreach (var column in columns)
                        {
                            columnValues.Add(column.ToString(), record[columns.IndexOf(column)]?.ToString());
                        }
                        dataPoint.ColumnValues = columnValues;
                        response.Add(dataPoint);
                    }
                }
                else
                {
                    result.Message = "No data found for the specified AssetID";
                    result.KindOfError = ErrorType.LikelyRecoverable;
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
            finally
            {
                _influxDbClient.Dispose();
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
            _measurement = AppConfig.GetSection("AppSettings:MeasurementName").Value ??
                    MEASUREMENT;
            _pageSize = AppConfig.GetSection("AppSettings:PageSize").Value ?? DEFAULT_PAGE_SIZE;
        }

        private string GenerateInfluxQLMeasurementQuery(IList<string> channelIds, IList<Guid> assetIds,
            IList<Guid> customerIds, string startDate, string endDate)
        {
            var queryBuilder = new StringBuilder();

            var sDate = DateTime.Parse(startDate).ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ");
            var eDate = DateTime.Parse(endDate).ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ");

            if (channelIds != null && channelIds.Count > 0)
            {
                queryBuilder.Append($"SELECT time, AssetID,");
                if (channelIds.Count > 1)
                {
                    var multiAddrQuery = new StringBuilder();
                    foreach (var channel in channelIds)
                    {
                        if (!string.IsNullOrEmpty(channel))
                        {
                            multiAddrQuery.Append($"{channel}, ");
                        }
                    }
                    var mQuery = multiAddrQuery.ToString()[..^2];
                    queryBuilder.Append(mQuery);
                }
                else
                {
                    queryBuilder.Append($"{channelIds[0]}");
                }
                queryBuilder.Append($" FROM \"XSPOCData\" WHERE ");
                foreach (var assetId in assetIds)
                {
                    if (assetId == assetIds.First())
                    {
                        queryBuilder.Append($" (\"AssetID\" = '{assetId}' ");
                    }
                    else
                    {
                        queryBuilder.Append($" OR \"AssetID\" = '{assetId}' ");
                    }

                    if (assetId == assetIds.Last())
                    {
                        queryBuilder.Append(')');
                    }
                }

                foreach (var customerId in customerIds)
                {
                    if (customerId == customerIds.First())
                    {
                        queryBuilder.Append($" AND (\"CustomerID\" = '{customerId}' ");
                    }
                    else
                    {
                        queryBuilder.Append($" OR \"CustomerID\" = '{customerId}' ");
                    }

                    if (customerId == customerIds.Last())
                    {
                        queryBuilder.Append(')');
                    }
                }

                queryBuilder.Append($" AND time >= \'{sDate}\' AND time <= \'{eDate}\'");

            }

            return queryBuilder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="pocType"></param>
        /// <param name="channelIds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="aggregate"></param>
        /// <param name="aggregateMethod"></param>
        /// <returns></returns>
        private async Task<IEnumerable<InfluxData.Net.InfluxDb.Models.Responses.Serie>> QueryInfluxData(Guid assetId, int pocType, List<string> channelIds, DateTime? startDate, DateTime? endDate, string aggregate, string aggregateMethod)
        {
            DbStoreResult result;
            try
            {

                var useAggregate = !string.IsNullOrEmpty(aggregate);
                var channelIdStr = useAggregate ? string.Join(", ", channelIds.Distinct().Select(c => $"{aggregateMethod}({c}) AS {c}").ToList()) : string.Join(", ", channelIds.Distinct());

                var queryBuilder = new StringBuilder();
                queryBuilder.Append($"SELECT time, {channelIdStr}");
                queryBuilder.Append($" FROM \"XSPOCData\" WHERE ");
                queryBuilder.Append($"\"AssetID\" = \'{assetId}\' ");

                if (!string.IsNullOrEmpty(pocType.ToString()))
                {
                    queryBuilder.Append($" and \"POCType\" = '{pocType}' ");
                }

                if (startDate != null && endDate != null)
                {
                    var startDateFormat = "yyyy-MM-ddTHH:mm:ss.fffffffZ";
                    var endDateFormat = endDate.Value.TimeOfDay.TotalMilliseconds > 0 ? startDateFormat : "yyyy-MM-ddT23:59:59.fffffffZ";

                    var sDate = DateTime.Parse(startDate.ToString()).ToString(startDateFormat);
                    var eDate = DateTime.Parse(endDate.ToString()).ToString(endDateFormat);
                    queryBuilder.Append($" and time >= \'{sDate}\' and time <= \'{eDate}\'");
                }
                if (useAggregate)
                {
                    queryBuilder.Append($" GROUP BY time({aggregate})");
                }
                queryBuilder.Append($" order by time desc ");

                if (startDate == null || endDate == null)
                {
                    queryBuilder.Append($" Limit 1");
                }

                //Step 3: Execute Influx Query
                var client = _influxDbClient.Create();
                var query = queryBuilder.ToString();
                result = new DbStoreResult()
                {
                    Message = string.Empty,
                    KindOfError = ErrorType.None,
                };
                return await client.Client.QueryAsync(query, _bucketName);

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
            finally
            {
                // Dispose the client
                _influxDbClient.Dispose();
            }

            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetIds"></param>
        /// <param name="channelIds"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="pocTypes"></param>
        /// <returns></returns>
        private async Task<IEnumerable<InfluxData.Net.InfluxDb.Models.Responses.Serie>> QueryInfluxDataLastOneRecord(List<string> assetIds, List<string> channelIds, DateTime? startDate, DateTime? endDate, List<string> pocTypes = null)
        {
            DbStoreResult result;
            try
            {
                var queryBuilder = new StringBuilder();
                var channelIdsCsv = string.Join(", ", channelIds.Distinct());
                var assetIdsCsv = string.Join("|", assetIds.Distinct());
                var pocTypesCsv = string.Join(", ", pocTypes.Distinct());

                queryBuilder.Append($"SELECT time, {channelIdsCsv}");

                queryBuilder.Append($" FROM \"XSPOCData\" WHERE ");
                queryBuilder.Append($"\"AssetID\" =~ /" + assetIdsCsv + "/");

                if (pocTypes.Any())
                {
                    queryBuilder.Append($" and  \"POCType\" =~ /" + pocTypesCsv + "/");
                }

                if (startDate != null && endDate != null)
                {
                    var startDateFormat = "yyyy-MM-ddTHH:mm:ss.fffffffZ";
                    var endDateFormat = endDate.Value.TimeOfDay.TotalMilliseconds > 0 ? startDateFormat : "yyyy-MM-ddT23:59:59.fffffffZ";

                    var sDate = DateTime.Parse(startDate.ToString()).ToString(startDateFormat);
                    var eDate = DateTime.Parse(endDate.ToString()).ToString(endDateFormat);
                    queryBuilder.Append($" and time >= \'{sDate}\' and time <= \'{eDate}\'");
                }

                queryBuilder.Append($" group by \"AssetID\" ");
                queryBuilder.Append($" order by time desc Limit 1");

                //Step 3: Execute Influx Query

                var client = _influxDbClient.Create();
                var query = queryBuilder.ToString();

                result = new DbStoreResult()
                {
                    Message = string.Empty,
                    KindOfError = ErrorType.None,
                };

                return await client.Client.QueryAsync(query, _bucketName);
            }
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
            finally
            {
                // Dispose the client
                _influxDbClient.Dispose();
            }

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
    }
}
