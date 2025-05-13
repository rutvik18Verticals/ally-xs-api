using MathNet.Numerics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Influx.Services;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using Enums = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Enums;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Service that handles processing of real time trend data.
    /// </summary>
    public class RealTimeDataProcessingService : IRealTimeDataProcessingService
    {

        #region Private Members

        private readonly IThetaLoggerFactory _loggerFactory;
        private readonly IAllyTimeSeriesNodeMaster _allyNodeMasterStore;
        private readonly IParameterMongoStore _parameterStore;
        private readonly IGetDataHistoryItemsService _dataHistoryInfluxStore;
        private readonly IDateTimeConverter _dateTimeConverter;
        private readonly IConfiguration _configuration;

        private const string XSPOC_DEFAULT_AGGREGATION = "5m";
        private const string XSPOC_DEFAULT_AGGREGATION_METHOD = "last";
        private readonly int ESPWellDowntimePST;    // = 2; //frequency. ESP well is said to be down if the frequency(PST 279) is 0.
        private readonly int GLWellDowntimeRate;    // = 191; //Ingestion Gas Rate. GL well is said to be down if the Ingestion Gas Rate(191) is 0.

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the <seealso cref="RealTimeDataProcessingService"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public RealTimeDataProcessingService(IThetaLoggerFactory loggerFactory, IAllyTimeSeriesNodeMaster allyNodeMasterStore,
            IParameterMongoStore parameterStore, IGetDataHistoryItemsService dataHistoryInfluxStore, IDateTimeConverter dateTimeConverter,
            IConfiguration configuration)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _allyNodeMasterStore = allyNodeMasterStore ?? throw new ArgumentNullException(nameof(allyNodeMasterStore));
            _parameterStore = parameterStore ?? throw new ArgumentNullException(nameof(parameterStore));
            _dataHistoryInfluxStore = dataHistoryInfluxStore ?? throw new ArgumentNullException(nameof(dataHistoryInfluxStore));
            _dateTimeConverter = dateTimeConverter ?? throw new ArgumentNullException(nameof(dateTimeConverter));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            ESPWellDowntimePST = _configuration.GetSection("AppSettings:ESPWellDowntimePSTParamId") == null ? 2 : int.Parse(_configuration.GetSection("AppSettings:ESPWellDowntimePSTParamId").Value);
            GLWellDowntimeRate = _configuration.GetSection("AppSettings:GLWellDowntimeRateParamId") == null ? 191 : int.Parse(_configuration.GetSection("AppSettings:GLWellDowntimeRateParamId").Value);
        }

        #endregion

        #region IRealTimeDataProcessingService Implementation

        /// <summary>
        /// Processes the Data History Trend Data Items.
        /// </summary>
        /// <param name="input">The <seealso cref="WithCorrelationId{TrendIDataInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="DataHistoryListOutput"/>.</returns>
        public async Task<TimeSeriesOutput> GetAllyTimeSeriesTrendDataAsync(WithCorrelationId<TimeSeriesInput> input)
        {
            var logger = _loggerFactory.Create(LoggingModel.TrendData);

            logger.WriteCId(Level.Trace, $"Starting {nameof(RealTimeDataProcessingService)} {nameof(GetAllyTimeSeriesTrendDataAsync)}", input?.CorrelationId);

            var response = new TimeSeriesOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            if (input == null)
            {
                var message = $"{nameof(input)} is null, cannot get time series trend data items.";
                logger.Write(Level.Error, message);
                response.Result.Status = false;
                response.Result.Value = message;
                response.ErrorCode = "400";

                return response;
            }

            if (input?.Value == null)
            {
                var message = $"{nameof(input)} is null, cannot get time series trend data items.";
                logger.WriteCId(Level.Error, message, input?.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;
                response.ErrorCode = "400";

                return response;
            }

            var request = input.Value;

            Guid bucket = Guid.Empty;
            var assetGUIDs = request.AssetIds.Where(x => Guid.TryParse(x, out bucket)).Select(x => bucket).ToList();

            if (assetGUIDs == null || assetGUIDs.Count == 0)
            {
                var message = $"{nameof(request.AssetIds)} is null or contains all invalid guids, cannot get time series trend data items.";
                logger.WriteCId(Level.Error, message, input?.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;
                response.ErrorCode = "400";

                return response;
            }

            DateTime startTime = DateTime.Now;

            var assets = await _allyNodeMasterStore.GetByAssetIdsForAllyTimeSeriesAsync(assetGUIDs, input.CorrelationId);

            DateTime endTime = DateTime.Now;

            logger.WriteCId(Level.Trace, $"Time taken by GetByAssetIdsForAllyTimeSeriesAsync:{(endTime - startTime).TotalSeconds} sec", input?.CorrelationId);

            if (assets == null || assets.Count == 0)
            {
                var message = $"{nameof(assets)} is invalid, cannot get time series trend data items.";
                logger.Write(Level.Error, message);
                response.Result.Status = false;
                response.Result.Value = message;
                response.ErrorCode = "400";

                return response;
            }

            var pocTypes = assets.Select(x => x.PocType.ToString()).ToList();
            startTime = DateTime.Now;
            var parameter = _parameterStore.GetParameterByParamStdType(pocTypes, request.Tags, input.CorrelationId);
            endTime = DateTime.Now;
            logger.WriteCId(Level.Trace, $"Time taken by GetParameterByParamStdType:{(endTime - startTime).TotalSeconds} sec", input?.CorrelationId);

            if (parameter == null || parameter.Count == 0)
            {
                var message = $"Invalid data. Cannot get time series trend data items.";
                logger.WriteCId(Level.Error, message, input.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;
                response.ErrorCode = "400";
                return response;
            }

            var startDate = request.StartDate;
            var endDate = request.EndDate;
            var lstGUIDs = assets.Select(x => x.AssetGuid).ToList();
            var lstChannelIds = parameter.Select(x => x.ChannelId).ToList();
            var allyTimeSeriesData = new List<TimeSeriesDataPoint>();

            startTime = DateTime.Now;
            logger.WriteCId(Level.Trace, $"Starting {nameof(IGetDataHistoryItemsService)} GetAllyTimeSeriesDataHistoryTrendData from Influx", input?.CorrelationId);

            var dataHistoryInfluxStore = await _dataHistoryInfluxStore.GetAllyTimeSeriesDataHistoryTrendData(lstGUIDs,
                                     Guid.Parse(request.CustomerId), lstChannelIds, startDate, endDate, request.DownSampleType
                                     , request.DownSampleWindowSize, request.Page, request.PageSize);

            logger.WriteCId(Level.Trace, $"Ending {nameof(IGetDataHistoryItemsService)} GetAllyTimeSeriesDataHistoryTrendData from Influx", input?.CorrelationId);
            endTime = DateTime.Now;
            logger.WriteCId(Level.Trace, $"Time taken by GetAllyTimeSeriesDataHistoryTrendData:{(endTime - startTime).TotalSeconds} sec", input?.CorrelationId);

            if (dataHistoryInfluxStore == null || dataHistoryInfluxStore.Count == 0)
            {
                var message = $"Data not found.";
                logger.WriteCId(Level.Error, message, input.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;
                response.ErrorCode = "404";
                return response;
            }

            if (dataHistoryInfluxStore != null)
            {
                if (dataHistoryInfluxStore.Count > 0)
                {
                    allyTimeSeriesData = dataHistoryInfluxStore.Select(x => new TimeSeriesDataPoint()
                    {
                        AssetID = x.AssetId,
                        TagId = GetTagID(x.POCTypeId, x.ChannelId, parameter),
                        Value = x.ValueOfTimeSeries,
                        Timestamp = x.TimeOfTimeSeries //x.Time.ToString("yyyy-MM-ddTHH:mm:ssZ")
                    }).ToList();

                    if (dataHistoryInfluxStore[0] != null)
                    {
                        if (allyTimeSeriesData.Count > 1)
                        {
                            allyTimeSeriesData[0].TotalRecords = dataHistoryInfluxStore[0].TotalCount;
                            allyTimeSeriesData[0].TotalPages = dataHistoryInfluxStore[0].TotalPages;
                        }
                    }
                }
                response.Values = allyTimeSeriesData;
                response.Result = new MethodResult<string>(true, string.Empty);
                logger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataProcessingService)} {nameof(GetAllyTimeSeriesTrendDataAsync)}", input?.CorrelationId);

            }
            return response;
        }

        /// <summary>
        /// Processes the Data History Trend Data Items.
        /// </summary>
        /// <param name="input">The <seealso cref="WithCorrelationId{TrendIDataInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="DataHistoryListOutput"/>.</returns>
        public async Task<TimeSeriesOutput> GetTimeSeriesDataAsync(WithCorrelationId<TimeSeriesInput> input)
        {
            var logger = _loggerFactory.Create(LoggingModel.TrendData);

            logger.WriteCId(Level.Trace, $"Starting {nameof(RealTimeDataProcessingService)} {nameof(GetTimeSeriesDataAsync)}", input?.CorrelationId);

            var response = new TimeSeriesOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            if (input == null)
            {
                var message = $"{nameof(input)} is null, cannot get time series trend data items.";
                logger.Write(Level.Error, message);
                response.Result.Status = false;
                response.Result.Value = message;
                response.ErrorCode = StatusCodes.Status400BadRequest.ToString();

                return response;
            }

            if (input?.Value == null)
            {
                var message = $"{nameof(input)} is null, cannot get time series trend data items.";
                logger.WriteCId(Level.Error, message, input?.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;
                response.ErrorCode = StatusCodes.Status400BadRequest.ToString();

                return response;
            }

            var request = input.Value;

            Guid bucket = Guid.Empty;
            var assetGUIDs = request.AssetIds.Where(x => Guid.TryParse(x, out bucket)).Select(x => bucket).ToList();

            if (assetGUIDs == null || assetGUIDs.Count == 0)
            {
                var message = $"{nameof(request.AssetIds)} is null or contains all invalid guids, cannot get time series trend data items.";
                logger.WriteCId(Level.Error, message, input?.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;
                response.ErrorCode = StatusCodes.Status400BadRequest.ToString();

                return response;
            }

            DateTime startTime = DateTime.Now;

            var assets = await _allyNodeMasterStore.GetByAssetIdsForAllyTimeSeriesAsync(assetGUIDs, input.CorrelationId);

            DateTime endTime = DateTime.Now;

            logger.WriteCId(Level.Trace, $"Time taken by GetByAssetIdsForAllyTimeSeriesAsync:{(endTime - startTime).TotalSeconds} sec", input?.CorrelationId);

            if (assets == null || assets.Count == 0)
            {
                var message = $"{nameof(assets)} is invalid, cannot get time series trend data items.";
                logger.Write(Level.Error, message);
                response.Result.Status = false;
                response.Result.Value = message;
                response.ErrorCode = StatusCodes.Status400BadRequest.ToString();

                return response;
            }

            var pocTypes = assets.Select(x => x.PocType.ToString()).ToList();
            startTime = DateTime.Now;
            var parameter = _parameterStore.GetParameterByParamStdType(pocTypes, request.Tags, input.CorrelationId);
            endTime = DateTime.Now;
            logger.WriteCId(Level.Trace, $"Time taken by GetParameterByParamStdType:{(endTime - startTime).TotalSeconds} sec", input?.CorrelationId);

            if (parameter == null || parameter.Count == 0)
            {
                var message = $"Invalid data. Cannot get time series trend data items.";
                logger.WriteCId(Level.Error, message, input.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;
                response.ErrorCode = StatusCodes.Status400BadRequest.ToString();
                return response;
            }

            var inputs = new List<TimeSeriesInputModel>();

            inputs = assets.Select(x => new TimeSeriesInputModel
            {
                AssetId = x.AssetGuid,
                POCTypeId = x.PocType.ToString(),
                ChannelIds = parameter.Where(y => (y.POCType?.LegacyId["POCTypesId"]?.ToLower() == x.PocType.ToString()) ||
                (y.LegacyId["POCType"]?.ToString() == "99")
                )?.Select(z => z.ChannelId).ToList()
            }).ToList();

            var startDate = request.StartDate;
            var endDate = request.EndDate;
            var lstGUIDs = assets.Select(x => x.AssetGuid).ToList();
            //var lstChannelIds = parameter.Select(x => x.ChannelId).ToList();
            var channelIds = parameter.Select(x => x.ChannelId).ToList().Distinct();
            List<string> lstChannelIds = new List<string>();
            foreach (var channelId in channelIds)
            {
                lstChannelIds.Add(channelId);
            }
            var allyTimeSeriesData = new List<TimeSeriesDataPoint>();

            startTime = DateTime.Now;
            logger.WriteCId(Level.Trace, $"Starting {nameof(IGetDataHistoryItemsService)} GetAllyTimeSeriesDataHistoryTrendData from Influx", input?.CorrelationId);

            var dataHistoryInfluxStore = await _dataHistoryInfluxStore.GetTimeSeriesResponseAsync(lstGUIDs,
                                     Guid.Parse(request.CustomerId), lstChannelIds, startDate, endDate, request.DownSampleType
                                     , request.DownSampleWindowSize, request.Page, request.PageSize, inputs);

            logger.WriteCId(Level.Trace, $"Ending {nameof(IGetDataHistoryItemsService)} GetAllyTimeSeriesDataHistoryTrendData from Influx", input?.CorrelationId);
            endTime = DateTime.Now;
            logger.WriteCId(Level.Trace, $"Time taken by GetAllyTimeSeriesDataHistoryTrendData:{(endTime - startTime).TotalSeconds} sec", input?.CorrelationId);

            if (dataHistoryInfluxStore == null || dataHistoryInfluxStore.Count == 0)
            {
                var message = $"Data not found.";
                logger.WriteCId(Level.Error, message, input.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;
                response.ErrorCode = StatusCodes.Status404NotFound.ToString();
                return response;
            }

            if (dataHistoryInfluxStore != null)
            {
                if (dataHistoryInfluxStore.Count > 0)
                {
                    StringBuilder error = new StringBuilder();
                    string str = string.Empty;
                    foreach (var item in dataHistoryInfluxStore)
                    {
                        var timeSeriesDataPoint = new TimeSeriesDataPoint
                        {
                            AssetID = item.AssetId,
                            Values = item.Values,
                            Timestamp = item.Timestamp //x.Time.ToString("yyyy-MM-ddTHH:mm:ssZ")}; 
                        };

                        var TagIds = GetListOfTagIDs(item.AssetId, item.POCTypeId, item.ChannelIds, parameter, out str);
                        if (!str.IsNullOrEmpty())
                        {
                            error.AppendLine(str);
                        }
                        timeSeriesDataPoint.TagIds = TagIds;

                        allyTimeSeriesData.Add(timeSeriesDataPoint);
                    }

                    if (error.Length > 0)
                    {
                        logger.WriteCId(Level.Trace, $"ZeroTagids:" + error.ToString(), input?.CorrelationId);
                    }

                    if (dataHistoryInfluxStore[0] != null)
                    {
                        if (allyTimeSeriesData.Count > 1)
                        {
                            allyTimeSeriesData[0].TotalRecords = dataHistoryInfluxStore[0].TotalCount;
                            allyTimeSeriesData[0].TotalPages = dataHistoryInfluxStore[0].TotalPages;
                        }
                    }
                }
                response.Values = allyTimeSeriesData;
                response.Result = new MethodResult<string>(true, string.Empty);
                logger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataProcessingService)} {nameof(GetTimeSeriesDataAsync)}", input?.CorrelationId);

            }
            return response;
        }

        /// <summary>
        /// Fetch asset details by customer Ids.
        /// </summary>
        /// <param name="input">The <seealso cref="WithCorrelationId{AssetByCustomerInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="AssetDetailsOutput"/>.</returns>
        public async Task<AssetDetailsOutput> GetAssetsByCustomerIdAsync(WithCorrelationId<AssetByCustomerInput> input)
        {
            var logger = _loggerFactory.Create(LoggingModel.TrendData);

            logger.WriteCId(Level.Trace, $"Starting {nameof(RealTimeDataProcessingService)} {nameof(GetAssetsByCustomerIdAsync)}", input?.CorrelationId);

            var response = new AssetDetailsOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            var assetDetailsData = new List<AssetDetails>();

            if (input == null)
            {
                var message = $"{nameof(input)} is null, cannot get assets by customer id.";
                logger.Write(Level.Error, message);
                response.Result.Status = false;
                response.Result.Value = message;
                response.ErrorCode = "400";

                return response;
            }

            if (input?.Value == null)
            {
                var message = $"{nameof(input)} is null, cannot get assets by customer id.";
                logger.WriteCId(Level.Error, message, input?.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;
                response.ErrorCode = "400";

                return response;
            }

            var request = input.Value;

            DateTime startTime = DateTime.Now;

            var assets = await _allyNodeMasterStore.GetAssetsByCustomerIdAsync(request.CustomerIds, input.CorrelationId);

            DateTime endTime = DateTime.Now;

            logger.WriteCId(Level.Trace, $"Time taken by GetAssetsByCustomerIdAsync:{(endTime - startTime).TotalSeconds} sec", input?.CorrelationId);

            if (assets == null || assets.Count == 0)
            {
                var message = $"Data not found.";
                logger.WriteCId(Level.Error, message, input.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;
                response.ErrorCode = "404";
                return response;
            }

            if (assets != null)
            {
                if (assets.Count > 0)
                {

                    foreach (var item in assets)
                    {
                        var assetDetails = new AssetDetails
                        {
                            CustomerId = item.CustomerId,
                            AssetID = item.AssetGuid.ToString(),
                            Name = item.NodeId,
                            PocType = item.PocType,
                            ApplicationId = item.ApplicationId,
                            IsEnabled = item.Enabled,
                        };

                        assetDetailsData.Add(assetDetails);
                    }

                }
                response.Values = assetDetailsData;
                response.Result = new MethodResult<string>(true, string.Empty);
                logger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataProcessingService)} {nameof(GetAssetsByCustomerIdAsync)}", input?.CorrelationId);

            }
            return response;
        }

        /// <summary>
        /// Processes the request to get all parameter standard types.
        /// </summary>
        /// <param name="input">The <seealso cref="WithCorrelationId{input}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="ParameterStandardTypeOutput"/>.</returns>
        public async Task<ParameterStandardTypeOutput> GetAllParameterStandardTypesAsync(WithCorrelationId<string> input)
        {
            var logger = _loggerFactory.Create(LoggingModel.TrendData);

            logger.WriteCId(Level.Trace, $"Starting {nameof(RealTimeDataProcessingService)} {nameof(GetAllParameterStandardTypesAsync)}", input?.CorrelationId);

            var response = new ParameterStandardTypeOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            var paramStandardTypeDetailsData = new List<ParamStandardTypeDetails>();

            DateTime startTime = DateTime.Now;

            var standardParams = await _allyNodeMasterStore.GetAllParameterStandardTypesAsync(input.CorrelationId);

            DateTime endTime = DateTime.Now;

            logger.WriteCId(Level.Trace, $"Time taken by GetAllParameterStandardTypesAsync:{(endTime - startTime).TotalSeconds} sec", input?.CorrelationId);

            if (standardParams == null || standardParams.Count == 0)
            {
                var message = $"Data not found.";
                logger.WriteCId(Level.Error, message, input.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;
                response.ErrorCode = "404";
                return response;
            }

            if (standardParams != null)
            {
                if (standardParams.Count > 0)
                {

                    foreach (var item in standardParams)
                    {
                        var assetDetails = new ParamStandardTypeDetails
                        {
                            Id = item.ParamStandardType,
                            Description = item.StringValue
                        };

                        paramStandardTypeDetailsData.Add(assetDetails);
                    }

                }
                response.Values = paramStandardTypeDetailsData;
                response.Result = new MethodResult<string>(true, string.Empty);
                logger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataProcessingService)} {nameof(GetAllParameterStandardTypesAsync)}", input?.CorrelationId);

            }
            return response;
        }

        /// <summary>
        /// Validate customer by user account.
        /// </summary>
        /// <param name="input">The <seealso cref="WithCorrelationId{ValidateCustomerInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="ValidateCustomerOutput"/>.</returns>
        public async Task<ValidateCustomerOutput> ValidateCustomerAsync(WithCorrelationId<ValidateCustomerInput> input)
        {
            var logger = _loggerFactory.Create(LoggingModel.TrendData);

            logger.WriteCId(Level.Trace, $"Starting {nameof(RealTimeDataProcessingService)} {nameof(ValidateCustomerAsync)}", input?.CorrelationId);

            var response = new ValidateCustomerOutput()
            {
                Result = new MethodResult<string>(true, string.Empty)
            };

            var validateCustData = new List<ValidateCustomer>();

            if (input == null)
            {
                var message = $"{nameof(input)} is null, cannot validate customer id.";
                logger.Write(Level.Error, message);
                response.Result.Status = false;
                response.Result.Value = message;
                response.ErrorCode = "400";

                return response;
            }

            if (input?.Value == null)
            {
                var message = $"{nameof(input)} is null, cannot validate customer id.";
                logger.WriteCId(Level.Error, message, input?.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;
                response.ErrorCode = "400";

                return response;
            }

            var request = input.Value;

            DateTime startTime = DateTime.Now;

            var custData = await _allyNodeMasterStore.ValidateCustomerAsync(request.UserAccount, request.TokenKey, request.TokenValue, input.CorrelationId);

            DateTime endTime = DateTime.Now;

            logger.WriteCId(Level.Trace, $"Time taken by ValidateCustomerAsync:{(endTime - startTime).TotalSeconds} sec", input?.CorrelationId);

            if (custData == null || custData.Count == 0)
            {
                var message = $"Data not found.";
                logger.WriteCId(Level.Error, message, input.CorrelationId);
                response.Result.Status = false;
                response.Result.Value = message;
                response.ErrorCode = "404";
                return response;
            }

            if (custData != null)
            {
                if (custData.Count > 0)
                {

                    foreach (var item in custData)
                    {
                        var validateCust = new ValidateCustomer
                        {
                            UserAccount = item.UserAccount,
                            IsValid = item.IsValid,
                        };

                        validateCustData.Add(validateCust);
                    }

                }
                response.Values = validateCustData;
                response.Result = new MethodResult<string>(true, string.Empty);
                logger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataProcessingService)} {nameof(ValidateCustomerAsync)}", input?.CorrelationId);

            }
            return response;
        }

        /// <summary>
        /// Processes the time series graph data
        /// </summary>
        /// <param name="input">The <seealso cref="WithCorrelationId{GraphDataInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="DataPointsModelDto"/>.</returns>
        public async Task<IList<DataPointsModelDto>> GetAssetTrendsGraphData(WithCorrelationId<GraphDataInput> input)
        {
            var logger = _loggerFactory.Create(LoggingModel.TrendData);
            if (input == null)
            {
                var message = $"{nameof(input)} is null, cannot get time series trend data items.";
                logger.Write(Level.Error, message);
                return null;
            }

            if (input?.Value == null)
            {
                var message = $"{nameof(input)} is null, cannot get time series trend data items.";
                logger.WriteCId(Level.Error, message, input?.CorrelationId);
                return null;
            }
            var request = input.Value;

            DateTime startTime = DateTime.Now;
            var assets = await _allyNodeMasterStore.GetAssetDetails(request.AssetId, input.CorrelationId);
            DateTime endTime = DateTime.Now;

            logger.WriteCId(Level.Trace, $"Time taken by GetByAssetIdsForAllyTimeSeriesAsync:{(endTime - startTime).TotalSeconds} sec", input?.CorrelationId);

            var filteredParameters = new List<Parameters>();

            if (assets == null || assets.Count == 0)
            {
                var message = $"{nameof(assets)} is invalid, cannot get graph trend data items.";
                logger.Write(Level.Error, message);
                return null;
            }
            var pocTypes = assets.Select(x => x.PocType.ToString()).ToList();
            var liftType = assets.Select(x => x.ApplicationId.ToString()).FirstOrDefault();
            if (!string.IsNullOrEmpty(liftType))
            {
                liftType = liftType == "4" ? "ESP" : liftType == "3" ? "Rod" : "GL";
            }
            startTime = DateTime.Now;
            var listOfTrends = await _allyNodeMasterStore.GetAllDefaultParametersAsync(liftType, input?.CorrelationId);
            endTime = DateTime.Now;
            logger.WriteCId(Level.Trace, $"Time taken by GetAllDefaultParametersAsync:{(endTime - startTime).TotalSeconds} sec", input?.CorrelationId);

            if (listOfTrends == null || listOfTrends.Count == 0)
            {
                var message = $"{nameof(assets)} is invalid, cannot get graph trend data items.";
                logger.Write(Level.Error, message);
                return null;
            }

            var listOfParamType = listOfTrends.Where(p=>p.Pst != string.Empty)?.Select(x=>x.Pst).ToList();

            if (request.ReteriveMixMax)
            {
                listOfParamType.AddRange(listOfTrends.Where(p => p.HighParamType != string.Empty)?.Select(d => d.HighParamType).ToList());
                listOfParamType.AddRange(listOfTrends.Where(p => p.LowParamType != string.Empty)?.Select(d => d.LowParamType).ToList());
            }

            startTime = DateTime.Now;
            var parameters = _parameterStore.GetParameterByParamStdType(pocTypes[0], listOfParamType.Where(d => !string.IsNullOrEmpty(d)).ToList()?.Distinct()?.ToList(), input?.CorrelationId);
            endTime = DateTime.Now;
            logger.WriteCId(Level.Trace, $"Time taken by GetParameterByParamStdType:{(endTime - startTime).TotalSeconds} sec", input?.CorrelationId);

            var paramTags = parameters.Where(x => x.ParameterType == "Param").ToList();
            var facilityTags = parameters.Where(x => x.ParameterType == "Facility" && x.Name.Contains(assets[0].NodeId)).ToList();

            if (facilityTags == null || facilityTags.Count <= 0)
            {
                filteredParameters = paramTags;
            }
            else
            {
                filteredParameters = paramTags.Where(x => !facilityTags.Select(i => i.Address).Contains(x.Address)).ToList();
                filteredParameters = filteredParameters.Union(facilityTags).ToList();
            }

            //Step 2 : Get ChannelIds
            List<string> channelIds = filteredParameters.Select(x => x.ChannelId).Distinct().ToList();

            if (channelIds.Count <= 0)
            {
                return null;
            }

            // var appStartDt = _dateTimeConverter.ConvertToApplicationServerTimeFromUTC(request.StartDate, input?.CorrelationId, LoggingModel.TrendData);
            //var appEndDt = _dateTimeConverter.ConvertToApplicationServerTimeFromUTC(request.EndDate, input?.CorrelationId, LoggingModel.TrendData);

            var offSetStartDt = ConvertWellTimeToUtc(input?.CorrelationId, assets[0].Tzoffset, assets[0].Tzdaylight, request.StartDate.ToString());
            var offSetEndDt = ConvertWellTimeToUtc(input?.CorrelationId, assets[0].Tzoffset, assets[0].Tzdaylight, request.EndDate.ToString());

            var offSetStartDt1 = _dateTimeConverter.GetTimeZoneAdjustedTime(assets[0].Tzoffset, assets[0].Tzdaylight, request.StartDate,
            input?.CorrelationId, LoggingModel.TrendData);

           var offSetEndDt1 = _dateTimeConverter.GetTimeZoneAdjustedTime(assets[0].Tzoffset, assets[0].Tzdaylight, request.EndDate,
            input?.CorrelationId, LoggingModel.TrendData);

            if (string.IsNullOrEmpty(request.Aggregate))
            {
                var aggregation = await GetAggregate(offSetStartDt, offSetEndDt, input?.CorrelationId );
                request.Aggregate = aggregation?.Aggregate;               
            }
            if (string.IsNullOrEmpty(request.AggregateMethod))
            {
                request.AggregateMethod = XSPOC_DEFAULT_AGGREGATION_METHOD;
            }

            startTime = DateTime.Now;
            var result = await _dataHistoryInfluxStore.GetInfluxDataAssetTrends(assets[0].AssetGuid, assets[0].NodeId, offSetStartDt, offSetEndDt, assets[0].PocType, filteredParameters, channelIds, listOfTrends.ToList(), request.Aggregate, request.AggregateMethod, assets[0].Tzoffset, assets[0].Tzdaylight);
            endTime = DateTime.Now;
            logger.WriteCId(Level.Trace, $"Time taken by GetInfluxDataAssetTrends:{(endTime - startTime).TotalSeconds} sec", input?.CorrelationId);
            return result;
        }

        /// <summary>
        /// Get the downtime and last 5 shutdowns for the asset for a period of days
        /// </summary>
        /// <param name="input">The <seealso cref="WithCorrelationId{GraphDataInput}"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The downtime and last 5 shutdown details.</returns>
        public async Task<WellDowntimeDataOutput> GetDowntime(WithCorrelationId<GraphDataInput> input)
        {
            var logger = _loggerFactory.Create(LoggingModel.TrendData);

            if (input == null)
            {
                var message = $"{nameof(input)} is null, cannot get downtime for the asset.";
                logger.Write(Level.Error, message);

                return null;
            }

            if (input?.Value == null)
            {
                var message = $"{nameof(input)} is null,  cannot get downtime for the asset.";
                logger.WriteCId(Level.Error, message, input?.CorrelationId);

                return null;
            }

            var request = input.Value;

            //Get Asset and POCType
            var assets = await _allyNodeMasterStore.GetByAssetIdsForAllyTimeSeriesAsync(new List<Guid>() { request.AssetId },
                input.CorrelationId);

            if (assets == null || assets.Count == 0)
            {
                var message = $"{nameof(assets)} is invalid, cannot get downtime for the asset.";
                logger.Write(Level.Error, message);

                return null;
            }

            var pocTypes = assets.Select(x => x.PocType.ToString()).ToList();
            var applicationId = assets.Select(x=>x.ApplicationId).FirstOrDefault();
            if(!applicationId.HasValue || applicationId.Value == 0)
            {
                var message = $"{nameof(assets)} is invalid, cannot get downtime for the asset.";
                logger.Write(Level.Error, message);
                return null;
            }
            var assetType = (Enums.Applications)applicationId;
            var paramStdType = 0;
            if (assetType == Enums.Applications.ESP)
            {
                paramStdType = ESPWellDowntimePST;
            }
            else if(assetType == Enums.Applications.GasLift)
            {
                paramStdType = GLWellDowntimeRate;
            }
            else
            {
                var message = $"{nameof(assets)} is invalid, cannot get downtime for this asset type.";
                logger.Write(Level.Error, message);
                return null;
            }

            //Get Parameter data
            var parameters = _parameterStore.GetParameterByParamStdType(pocTypes?[0], paramStdType, input?.CorrelationId);

            if (parameters == null)
            {
                var message = $"{nameof(assets)} is invalid, cannot get downtime for the asset.";
                logger.Write(Level.Error, message);

                return null;
            }

            //Get ChannelIds
            List<string> channelIds = new List<string> { parameters.ChannelId };

            if (channelIds.Count <= 0)
            {
                return null;
            }

            //Get Asset Timezone to convert dates to utc dates to fetch the downtime data
            TimeZoneInfo timeZone;
            if (string.IsNullOrWhiteSpace(assets.FirstOrDefault().TimeZoneId))
            {
                string applicationTimeZone = _configuration.GetSection("TimeZoneBehavior:ApplicationTimeZone").Value;
                timeZone = TimeZoneInfo.FindSystemTimeZoneById(applicationTimeZone);
            }
            else
            {
                timeZone = TimeZoneInfo.FindSystemTimeZoneById(assets.FirstOrDefault().TimeZoneId);
            }
            
            request.StartDate = request.StartDate.AddMinutes(-timeZone.GetUtcOffset(request.StartDate).TotalMinutes);
            request.EndDate = request.EndDate.AddMinutes(-timeZone.GetUtcOffset(request.EndDate).TotalMinutes);

            //get downtime data from influx between the dates
            var responseData = _dataHistoryInfluxStore.GetDowntime(request.AssetId, request.StartDate,
                request.EndDate, channelIds[0]);

            if (responseData == null || responseData?.Result == null)
            {
                var message = $"Data not found.";
                logger.WriteCId(Level.Error, message, input.CorrelationId);

                return new WellDowntimeDataOutput
                {
                    ErrorCode = "204",
                    Result = new MethodResult<string>(false, message)
                };
            }

            //Get Downtime data and last 5 shutdowns
            return GetDowntimeDataAndLast5Shutdowns(assets, responseData.Result);
        }

        #endregion

        #region Private Methods

        private async Task<TimeSeriesChartAggregation> GetAggregate(DateTime from, DateTime to, string correlationId)
        {
            var minutes = (to - from).TotalMinutes;

            var aggregations = await _allyNodeMasterStore.GetTimeSeriesChartAggregationAsync(correlationId);

            if (aggregations != null && aggregations.Any())
            {
                aggregations = aggregations.OrderBy(x => x.Minutes).ToList();
                foreach (var aggregate in aggregations)
                {
                    if (minutes <= aggregate.Minutes)
                    {
                        return aggregate;
                    }
                }
                return aggregations.Last();
            }

            return new TimeSeriesChartAggregation { Aggregate = XSPOC_DEFAULT_AGGREGATION };
        }

        private List<int> GetListOfTagIDs(string assetid, string pocTypeId, List<string> channelIds, List<Parameters> parameter, out string error)
        {
            error = string.Empty;
            List<int> tagIds = new List<int>();
            if (channelIds != null && channelIds.Count > 0)
            {
                foreach (var channelId in channelIds)
                {
                    int tagId = GetTagID(pocTypeId, channelId, parameter);
                    tagIds.Add(tagId);

                    if (tagId == 0)
                    {
                        error = "tagid zero: AssetID:" + assetid + " pocTypeId: " + pocTypeId + " channelId: " + channelId;
                    }

                }
            }
            return tagIds;
        }

        private int GetTagID(string pocTypeId, string channelId, List<Parameters> parameter)
        {
            int tagID = 0;
            var tagIds = parameter.Where(x => (x.ChannelId == channelId
            && (x.POCType?.LegacyId["POCTypesId"]?.ToString() == pocTypeId ||
               (x.LegacyId["POCType"]?.ToString() == "99"))))
                 .Select(x => x.ParamStandardType.LegacyId["ParamStandardTypesId"]).ToList();
            if (tagIds != null)
            {
                if (tagIds.Count > 0)
                {
                    if (Int32.TryParse(tagIds[0], out tagID))
                    {
                        return tagID;
                    }
                }
            }
            return tagID;
        }

        private DateTime ConvertWellTimeToUtc(string correlationId, float nodeTimeZoneOffset, bool honorDaylightSaving, string dateTime)
        {
            var timezoneInfo = _dateTimeConverter.GetTimeZoneInfo(correlationId, LoggingModel.TrendData);
            var offset = -1 * (timezoneInfo.BaseUtcOffset.TotalHours + nodeTimeZoneOffset);
            var utcDateTime = DateTime.Parse(dateTime, null).AddHours(offset);

            var daylightSaving = false;
            if (timezoneInfo.IsDaylightSavingTime(utcDateTime))
            {
                daylightSaving = true;
            }

            // If tzoffset=0, then do not apply any dst corrections - just adjust based on server
            TimeSpan daylightSavingBias = new TimeSpan(0);
            if (daylightSaving && nodeTimeZoneOffset != 0 && honorDaylightSaving)
            {
                var dt = DateTime.UtcNow;

                daylightSavingBias = timezoneInfo.BaseUtcOffset - timezoneInfo.GetUtcOffset(dt);
            }

            utcDateTime = utcDateTime.Add(daylightSavingBias);

            return utcDateTime;
        }

        private WellDowntimeDataOutput GetDowntimeDataAndLast5Shutdowns(IList<NodeMasterModel> assets,
            IList<DataPointModel> responseData)
        {
            var asset = assets.FirstOrDefault();
            var downTimeOutputData = new WellDowntimeDataOutput
            {
                AssetGuid = asset.AssetGuid.ToString(),
                Well = asset.NodeId,
                WellFieldAreaBusinessUnitName = asset.BusinessUnit,
                WellFieldAreaName = asset.Area,
                WellFieldName = asset.Field,
                WellMethodOfProduction = asset.MethodofProduction,
                WellTimezone = asset.TimeZoneId,
            };

            var downTimeData = new List<WellDowntimeData>();
            var last5Shutdowns = new List<WellDowntimeData>();

            bool waitingForRecovery = false;            
            DateTime downtime = DateTime.MinValue;

            //Get Asset Timezone to convert downtime utc dates to asset/application timezone
            TimeZoneInfo timeZone;
            if (string.IsNullOrWhiteSpace(asset.TimeZoneId))
            {
                string applicationTimeZone = _configuration.GetSection("TimeZoneBehavior:ApplicationTimeZone").Value;
                timeZone = TimeZoneInfo.FindSystemTimeZoneById(applicationTimeZone);
            }
            else
            {
                timeZone = TimeZoneInfo.FindSystemTimeZoneById(asset.TimeZoneId);
            }
            
            responseData ??= new List<DataPointModel>();
            int index = 0;
            foreach (var point in responseData)
            {
                var hasValue = float.TryParse(point.Value.ToString(), out var value);
                if (!hasValue)
                {
                    continue;
                }
                if (value == 0 && !waitingForRecovery)
                {
                    // Value dropped to 0
                    waitingForRecovery = true;
                    downtime = point.Time;
                    
                    //downtime started
                    downTimeData.Add(new WellDowntimeData
                    {
                        Date = downtime,
                        DateMilliseconds = (long)(downtime - DateTime.UnixEpoch).TotalMilliseconds,
                        DateLocal = TimeZoneInfo.ConvertTimeFromUtc(downtime, timeZone),
                        DateLocalMilliseconds = (long)(TimeZoneInfo.ConvertTimeFromUtc(downtime, timeZone) - DateTime.UnixEpoch).TotalMilliseconds,
                        DateLocalString = TimeZoneInfo.ConvertTimeFromUtc(downtime, timeZone).ToString("dd-MMM-yyyy hh:mm")
                    });
                }
                else if (value > 0 && waitingForRecovery)
                {
                    // Value recovered (> 0)
                    //downtime ended
                    //update end time of last downtime
                    downTimeData[index].EndDate = point.Time;
                    downTimeData[index].EndDateMilliseconds = (long)(point.Time - DateTime.UnixEpoch).TotalMilliseconds;
                    downTimeData[index].EndDateLocal = TimeZoneInfo.ConvertTimeFromUtc(point.Time, timeZone);
                    downTimeData[index].EndDateLocalMilliseconds = (long)(TimeZoneInfo.ConvertTimeFromUtc(point.Time, timeZone) - DateTime.UnixEpoch).TotalMilliseconds;
                    downTimeData[index].EndDateLocalString = TimeZoneInfo.ConvertTimeFromUtc(point.Time, timeZone).ToString("dd-MMM-yyyy hh:mm");
                    downTimeData[index].Hours = (point.Time - downtime).TotalHours.Round(2);

                    waitingForRecovery = false;
                    index++;
                }
            }
            if (downTimeData != null && downTimeData.Count > 0)
            {
                //if downtime started but didnot end within the selected period, then update end date as last point in the response
                if (waitingForRecovery)
                {
                    downTimeData.Last().EndDate = responseData.Last().Time;
                    downTimeData.Last().EndDateMilliseconds = (long)(responseData.Last().Time - DateTime.UnixEpoch).TotalMilliseconds;
                    downTimeData.Last().EndDateLocal = TimeZoneInfo.ConvertTimeFromUtc(responseData.Last().Time, timeZone);
                    downTimeData.Last().EndDateLocalMilliseconds = (long)(TimeZoneInfo.ConvertTimeFromUtc(responseData.Last().Time, timeZone) - DateTime.UnixEpoch).TotalMilliseconds;
                    downTimeData.Last().EndDateLocalString = TimeZoneInfo.ConvertTimeFromUtc(responseData.Last().Time, timeZone).ToString("dd-MMM-yyyy hh:mm");
                    downTimeData.Last().Hours = (responseData.Last().Time - downtime).TotalHours.Round(2);
                }
                last5Shutdowns = downTimeData.OrderByDescending(x => x.Date).Take(5).ToList();
            }

            downTimeOutputData.DowntimeData = downTimeData ?? new List<WellDowntimeData>();
            downTimeOutputData.Last5Shutdowns = last5Shutdowns ?? new List<WellDowntimeData>();

            downTimeOutputData.Result = new MethodResult<string>(true, "Request successfully completed!");

            return downTimeOutputData;
        }

        #endregion

    }
}
