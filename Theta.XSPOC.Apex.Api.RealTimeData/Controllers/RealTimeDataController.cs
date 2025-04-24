using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System;
using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using Theta.XSPOC.Apex.Api.RealTimeData.Contracts.Requests;
using Theta.XSPOC.Apex.Api.RealTimeData.Contracts.Mappers;
using System.Text.RegularExpressions;
using Theta.XSPOC.Apex.Api.RealTimeData.Contracts.Responses;

namespace Theta.XSPOC.Apex.Api.RealTimeData.Controllers
{
    /// <summary>
    /// Handles Real Time Data requests.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]     
    public class RealTimeDataController : BaseController
    {

        #region BaseController Members

        /// <summary>
        /// Gets or sets the controller logger.
        /// </summary>
        protected override IThetaLogger ControllerLogger => LoggerFactory.Create(LoggingModel.TrendData);

        /// <summary>
        /// The <seealso cref="IConfiguration"/> configurations.
        /// </summary>
        protected IConfiguration AppConfig { get; }

        #endregion

        #region Private Fields

        private readonly IRealTimeDataProcessingService _service;
        private const string DEFAULT_MAX_ASSETID = "50";
        private const string DEFAULT_MAX_TAGID = "50";
        private const string DEFAULT_MAX_PAGE_SIZE = "50000";
        private const string DEFAULT_ASSET_CNT_5TO50_SAMPTYPE_30M_LMT_DWNSAMPWINDSIZE = "15m";
        private const string DEFAULT_ASSET_CNT_ABOVE50_SAMPTYPE_30M_LMT_DWNSAMPWINDSIZE = "30m";
        private const string downSampleTypeRegEx = "^(first)$"; //"^(first|last|Min)$"
        private const string downsampleWindowSizeRegExNum = "^\\d+$";
        private const string downsampleWindowSizeRegExChar = "^(s|m|h|d)$";
        private const string downsampleWindowSizeLimitAllowedRegExChar = "^(h|d)$";

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the <seealso cref="RealTimeDataController"/>.
        /// </summary>
        /// <param name="loggerFactory">The <seealso cref="IThetaLoggerFactory"/>.</param>
        /// <paramref name="loggerFactory"/> is null.
        public RealTimeDataController(IThetaLoggerFactory loggerFactory,
            IRealTimeDataProcessingService realTimeDataProcessingService, IConfiguration appConfig) : base(loggerFactory)
        {
            _service = realTimeDataProcessingService ?? throw new ArgumentNullException(nameof(realTimeDataProcessingService));
            AppConfig = appConfig ?? throw new ArgumentNullException(nameof(appConfig));
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Handles requests for fetching real time series data.
        /// </summary>
        /// <param name="request">The filters contains the customerId,assetIds,tags,startdate,enddate,downsampleType,downsampleWindowSize,page.</param>        
        [ProducesResponseType(typeof(TimeSeriesDataResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string),StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string),StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string),StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string),StatusCodes.Status501NotImplemented)]
        [HttpPost("ParameterData", Name = "ParameterData")]
        public async Task<IActionResult> GetTimeSeriesDataAsync([FromBody] TimeSeriesRequest request)
        {
            try
            {
                GetOrCreateCorrelationId(out var correlationId);

                ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(RealTimeDataController)} {nameof(GetTimeSeriesDataAsync)}", correlationId);

                if (request == null)
                {
                    ControllerLogger.WriteCId(Level.Info, "Invalid Defaults", correlationId);
                    ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(GetTimeSeriesDataAsync)}", correlationId);

                    return BadRequest("Invalid Defaults");
                }

                Guid custID = Guid.Empty;

                if (request.CustomerId.IsNullOrEmpty() || !Guid.TryParse(request.CustomerId, out custID))
                {
                    ControllerLogger.WriteCId(Level.Info, "Invalid Defaults", correlationId);
                    ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(GetTimeSeriesDataAsync)}", correlationId);

                    return BadRequest("CustomerID is required in valid format");
                }

                if (request.AssetIds == null || request.AssetIds.Length == 0)
                {
                    ControllerLogger.WriteCId(Level.Info, "Invalid Defaults", correlationId);
                    ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(GetTimeSeriesDataAsync)}", correlationId);

                    return BadRequest("AssetId is required.");
                }

                Guid assetID = Guid.Empty;
                var assetGUIDs = request.AssetIds.Where(x => Guid.TryParse(x, out assetID)).Select(x => assetID).ToList();
                if (assetGUIDs == null || assetGUIDs.Count == 0)
                {
                    ControllerLogger.WriteCId(Level.Info, "Invalid Defaults", correlationId);
                    ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(GetTimeSeriesDataAsync)}", correlationId);

                    return BadRequest("Atleast one assetId is required in valid format");
                }

                var maxAssetIdsLimit = AppConfig.GetSection("AppSettings:MaxNoOfAssetIds").Value ?? DEFAULT_MAX_ASSETID;

                if (assetGUIDs.Count > Convert.ToInt32(maxAssetIdsLimit))
                {
                    ControllerLogger.WriteCId(Level.Info, "Invalid Defaults", correlationId);
                    ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(GetTimeSeriesDataAsync)}", correlationId);

                    return BadRequest("Provided AssetIds should not exceed max limit");
                }

                if (request.Tags == null || request.Tags.Length == 0 || request.Tags.Where(x => x > 0).ToList()?.Count == 0)
                {
                    ControllerLogger.WriteCId(Level.Info, "Invalid Defaults", correlationId);
                    ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(GetTimeSeriesDataAsync)}", correlationId);
                    return BadRequest("Tags is required.");
                }

                var maxTagIdLimit = AppConfig.GetSection("AppSettings:MaxNoOfTagIds").Value ?? DEFAULT_MAX_TAGID;
                if (request.Tags.Length > Convert.ToInt32(maxTagIdLimit))
                {
                    ControllerLogger.WriteCId(Level.Info, "Invalid Defaults", correlationId);
                    ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(GetTimeSeriesDataAsync)}", correlationId);

                    return BadRequest("Provided TagIds should not exceed max limit");
                }

                if (request.StartDate.IsNullOrEmpty() || !DateTime.TryParseExact(request.StartDate, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime stDate))
                {
                    ControllerLogger.WriteCId(Level.Info, "Invalid Defaults", correlationId);
                    ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(GetTimeSeriesDataAsync)}", correlationId);

                    return BadRequest("Start Date is not provided or provided format is incorrect.");
                }

                if (request.EndDate.IsNullOrEmpty() || !DateTime.TryParseExact(request.EndDate, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime endDate))
                {
                    ControllerLogger.WriteCId(Level.Info, "Invalid Defaults", correlationId);
                    ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(GetTimeSeriesDataAsync)}", correlationId);

                    return BadRequest("End Date is not provided or provided format is incorrect.");
                }

                if (stDate > endDate)
                {
                    ControllerLogger.WriteCId(Level.Info, "Invalid Defaults", correlationId);
                    ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(GetTimeSeriesDataAsync)}", correlationId);

                    return BadRequest("Start date cannot be greater than end date.");
                }

                if (endDate > DateTime.UtcNow)
                {
                    ControllerLogger.WriteCId(Level.Info, "Invalid Defaults", correlationId);
                    ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(GetTimeSeriesDataAsync)}", correlationId);

                    return BadRequest("End date cannot be a future date.");
                }

                if (request.DownSampleType.IsNullOrEmpty())
                {
                    ControllerLogger.WriteCId(Level.Info, "Invalid Defaults", correlationId);
                    ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(GetTimeSeriesDataAsync)}", correlationId);

                    return BadRequest("DownSampleType is required.");
                }

                if (!Regex.IsMatch(request.DownSampleType.Trim().ToLower(), downSampleTypeRegEx))
                {
                    ControllerLogger.WriteCId(Level.Info, "Invalid Defaults", correlationId);
                    ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(GetTimeSeriesDataAsync)}", correlationId);

                    return BadRequest("DownSampleType should be provided in valid format.");
                }

                if (request.DownSampleWindowSize.IsNullOrEmpty())
                {
                    ControllerLogger.WriteCId(Level.Info, "Invalid Defaults", correlationId);
                    ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(GetTimeSeriesDataAsync)}", correlationId);

                    return BadRequest("DownSampleWindowSize is required.");
                }

                var chars = request.DownSampleWindowSize.Trim().ToLower()[^1..];
                var numbers = request.DownSampleWindowSize.Trim().ToLower()[..^1];

                if (!Regex.IsMatch(numbers, downsampleWindowSizeRegExNum) || !Regex.IsMatch(chars, downsampleWindowSizeRegExChar))
                {
                    ControllerLogger.WriteCId(Level.Info, "Invalid Defaults", correlationId);
                    ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(GetTimeSeriesDataAsync)}", correlationId);

                    return BadRequest("DownSampleWindowSize should be provided in valid format.");
                }            

                if (request.Page == 0)
                {
                    ControllerLogger.WriteCId(Level.Info, "Invalid Defaults", correlationId);
                    ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(GetTimeSeriesDataAsync)}", correlationId);

                    return BadRequest("Page is required.");
                }

                var downSampleWindowSize = request.DownSampleWindowSize;
                var assetCnt5To50SampType30mLmtDwnSampWindSize = AppConfig.GetSection("AppSettings:AssetCnt5To50SampType30mLmtDwnSampWindSize").Value ?? DEFAULT_ASSET_CNT_5TO50_SAMPTYPE_30M_LMT_DWNSAMPWINDSIZE;
                var assetCntAbove50SampType30mLmtDwnSampWindSize = AppConfig.GetSection("AppSettings:AssetCntAbove50SampType30mLmtDwnSampWindSize").Value ?? DEFAULT_ASSET_CNT_5TO50_SAMPTYPE_30M_LMT_DWNSAMPWINDSIZE;
                var duration = (endDate - stDate).TotalMinutes;

                if (request.AssetIds.Length > 5 && request.AssetIds.Length <= 50 && duration > 30)
                {
                    if (!Regex.IsMatch(chars.ToLower(),downsampleWindowSizeLimitAllowedRegExChar))
                    {
                        if (chars.ToLower() != "m" || (chars.ToLower() == "m" && Convert.ToInt32(numbers) <= 15))
                        {
                            downSampleWindowSize = assetCnt5To50SampType30mLmtDwnSampWindSize;
                        }
                    }                    
                }
                else
                {
                    if (request.AssetIds.Length > 50 && duration > 30)
                    {
                        if (!Regex.IsMatch(chars.ToLower(), downsampleWindowSizeLimitAllowedRegExChar))                            
                        {
                            if (chars.ToLower() != "m" || (chars.ToLower() == "m" && Convert.ToInt32(numbers) <= 30))
                            {
                                downSampleWindowSize = assetCntAbove50SampType30mLmtDwnSampWindSize;
                            }
                        }
                    }
                }

                string pageSize = request.PageSize > 0 ? request.PageSize.ToString() : AppConfig.GetSection("AppSettings:PageSize").Value ?? DEFAULT_MAX_PAGE_SIZE;

                var input = new TimeSeriesInput()
                {
                    CustomerId = request.CustomerId,
                    AssetIds = request.AssetIds,
                    Tags = request.Tags.Select(x => x.ToString()).ToList(),
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    DownSampleType = request.DownSampleType,
                    DownSampleWindowSize = downSampleWindowSize,
                    Page = request.Page,
                    PageSize = Convert.ToInt32(pageSize),
                };

                var requestWithCorrelationId = new WithCorrelationId<TimeSeriesInput>(
                correlationId, input);

                var serviceResult = await _service.GetTimeSeriesDataAsync(requestWithCorrelationId);

                if (!ValidateServiceResult(correlationId, out var defaultInvalidStatusCodeResult, serviceResult))
                {
                    ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(GetTimeSeriesDataAsync)}", correlationId);

                    if (serviceResult.ErrorCode == StatusCodes.Status400BadRequest.ToString())
                    {
                        return BadRequest(serviceResult.Result.Value);
                    }
                    if (serviceResult.ErrorCode == StatusCodes.Status404NotFound.ToString())
                    {
                        return NotFound(StatusCodesUtility.GetStatusCodeMessage(StatusCodes.Status404NotFound));
                    }

                    return defaultInvalidStatusCodeResult;
                }

                var response = TimeSeriesMapper.MapTimeSeriesData(requestWithCorrelationId.CorrelationId, serviceResult);

                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(GetTimeSeriesDataAsync)}", correlationId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                ControllerLogger.Write(Level.Fatal, $"Error: {nameof(RealTimeDataController)} {nameof(GetTimeSeriesDataAsync)} {ex.Message}",ex);
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodesUtility.GetStatusCodeMessage(StatusCodes.Status500InternalServerError));
            }

        }

        /// <summary>
        /// Handles requests for fetching assetIds by CustomerIds.
        /// </summary>
        /// <param name="inputReq">The filters contains comma separated customer Ids</param>             
        [ProducesResponseType(typeof(AssetDetailsDataResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), StatusCodes.Status501NotImplemented)]
        [HttpGet("Assets", Name = "Assets")]
        public async Task<IActionResult> GetAssetsByCustomerIdAsync([FromQuery] AssetsByCustomerIdRequest inputReq)
        {
            GetOrCreateCorrelationId(out var correlationId);

            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(RealTimeDataController)} {nameof(GetAssetsByCustomerIdAsync)}", correlationId);

           
            if (inputReq == null || inputReq.CustomerIds == null ||inputReq.CustomerIds.Split(",").Length == 0)
            {
                ControllerLogger.WriteCId(Level.Info, "Invalid Defaults", correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(GetAssetsByCustomerIdAsync)}", correlationId);

                return BadRequest("CustomerIDs is required");
            }

            var custlst = inputReq.CustomerIds.Split(",").ToList();

            Guid custID = Guid.Empty;
            var custGUIDs = custlst.Where(x => Guid.TryParse(x, out custID)).Select(x => custID.ToString()).ToList();
            if (custGUIDs == null || custGUIDs.Count == 0)
            {
                ControllerLogger.WriteCId(Level.Info, "Invalid Defaults", correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(GetAssetsByCustomerIdAsync)}", correlationId);

                return BadRequest("CustomerIDs is required in valid format");
            }

            var input = new AssetByCustomerInput()
            {
                CustomerIds = custGUIDs
            };

            var requestWithCorrelationId = new WithCorrelationId<AssetByCustomerInput>(
            correlationId, input);

            var serviceResult = await _service.GetAssetsByCustomerIdAsync(requestWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out var defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(GetAssetsByCustomerIdAsync)}", correlationId);

                if (serviceResult.ErrorCode == "400")
                {
                    return BadRequest(serviceResult.Result.Value);
                }
                if (serviceResult.ErrorCode == "404")
                {
                    return NotFound(serviceResult.Result.Value);
                }

                return defaultInvalidStatusCodeResult;
            }

            var response = TimeSeriesMapper.MapAssetDetails(requestWithCorrelationId.CorrelationId, serviceResult);

            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(GetAssetsByCustomerIdAsync)}", correlationId);

            return Ok(response);

        }

        /// <summary>
        /// Handles requests for fetching all parameter standard types.
        /// </summary>               
        [ProducesResponseType(typeof(ParameterStandardTypeDataResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), StatusCodes.Status501NotImplemented)]
        [HttpGet("Parameters", Name = "Parameters")]
        public async Task<IActionResult> GetAllParameterStandardTypesAsync()
        {
            GetOrCreateCorrelationId(out var correlationId);

            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(RealTimeDataController)} {nameof(GetAllParameterStandardTypesAsync)}", correlationId);

            var requestWithCorrelationId = new WithCorrelationId<string>(
            correlationId, string.Empty);

            var serviceResult = await _service.GetAllParameterStandardTypesAsync(requestWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out var defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(GetAllParameterStandardTypesAsync)}", correlationId);

                if (serviceResult.ErrorCode == "400")
                {
                    return BadRequest(serviceResult.Result.Value);
                }
                if (serviceResult.ErrorCode == "404")
                {
                    return NotFound(serviceResult.Result.Value);
                }

                return defaultInvalidStatusCodeResult;
            }

            var response = TimeSeriesMapper.MapParameterStandardTypes(requestWithCorrelationId.CorrelationId, serviceResult);

            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(GetAllParameterStandardTypesAsync)}", correlationId);

            return Ok(response);

        }

        /// <summary>
        /// Handles requests for validating customer tokens
        /// </summary>
        /// <param name="request">The filters contains user account, ApiToken Key and Apitoken Value</param> 
        [ProducesResponseType(typeof(ValidateCustomerDataResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(string), StatusCodes.Status501NotImplemented)]
        [HttpGet("ValidateToken", Name = "ValidateToken")]
        public async Task<IActionResult> ValidateCustomerAsync([FromQuery]ValidateTokenRequest request)
        {
            GetOrCreateCorrelationId(out var correlationId);

            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(RealTimeDataController)} {nameof(ValidateCustomerAsync)}", correlationId);
            
            if (request == null || request.UserAccount.IsNullOrEmpty())
            {
                ControllerLogger.WriteCId(Level.Info, "Invalid Defaults", correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(ValidateCustomerAsync)}", correlationId);

                return BadRequest("UserAccount is required.");
            }

            if (request.ApiTokenKey.IsNullOrEmpty())
            {
                ControllerLogger.WriteCId(Level.Info, "Invalid Defaults", correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(ValidateCustomerAsync)}", correlationId);

                return BadRequest("Token Key is required.");
            }

            if (request.ApiTokenValue.IsNullOrEmpty())
            {
                ControllerLogger.WriteCId(Level.Info, "Invalid Defaults", correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(ValidateCustomerAsync)}", correlationId);

                return BadRequest("Token Value is required.");
            }
            var input = new ValidateCustomerInput()
            {
                UserAccount = request.UserAccount.Trim(),
                TokenKey = request.ApiTokenKey.Trim(),
                TokenValue = request.ApiTokenValue.Trim(),
            };

            var requestWithCorrelationId = new WithCorrelationId<ValidateCustomerInput>(
            correlationId, input);

            var serviceResult = await _service.ValidateCustomerAsync(requestWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out var defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(ValidateCustomerAsync)}", correlationId);

                if (serviceResult.ErrorCode == "400")
                {
                    return BadRequest(serviceResult.Result.Value);
                }
                if (serviceResult.ErrorCode == "404")
                {
                    return NotFound(serviceResult.Result.Value);
                }

                return defaultInvalidStatusCodeResult;
            }

            var response = TimeSeriesMapper.MapValidateCustomer(requestWithCorrelationId.CorrelationId, serviceResult);

            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RealTimeDataController)} {nameof(ValidateCustomerAsync)}", correlationId);

            return Ok(response);

        }
        #endregion

    }
}