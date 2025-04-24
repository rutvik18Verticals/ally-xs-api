using Microsoft.AspNetCore.Http;
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
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

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
        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the <seealso cref="RealTimeDataProcessingService"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public RealTimeDataProcessingService(IThetaLoggerFactory loggerFactory, IAllyTimeSeriesNodeMaster allyNodeMasterStore, 
            IParameterMongoStore parameterStore, IGetDataHistoryItemsService dataHistoryInfluxStore)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _allyNodeMasterStore = allyNodeMasterStore ?? throw new ArgumentNullException(nameof(allyNodeMasterStore));
            _parameterStore = parameterStore ?? throw new ArgumentNullException(nameof(parameterStore));
            _dataHistoryInfluxStore = dataHistoryInfluxStore ?? throw new ArgumentNullException(nameof(dataHistoryInfluxStore));
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

            var custData = await _allyNodeMasterStore.ValidateCustomerAsync(request.UserAccount,request.TokenKey,request.TokenValue, input.CorrelationId);

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
 
        #endregion

        #region Private Methods

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

        #endregion
    }
}
