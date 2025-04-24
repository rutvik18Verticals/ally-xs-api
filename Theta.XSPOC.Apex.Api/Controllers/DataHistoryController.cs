using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Contracts.Mappers;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Level = Theta.XSPOC.Apex.Kernel.Logging.Models.Level;

namespace Theta.XSPOC.Apex.Api.Controllers
{
    /// <summary>
    /// The controller that fields requests for Data History.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class DataHistoryController : BaseController
    {

        #region BaseController Members

        /// <summary>
        /// Gets or sets the controller logger.
        /// </summary>
        protected override IThetaLogger ControllerLogger => LoggerFactory.Create(LoggingModel.TrendData);

        #endregion

        #region Private Fields

        private readonly IDataHistoryProcessingService _service;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the <seealso cref="DataHistoryController"/>.
        /// </summary>
        /// <param name="loggerFactory">The <seealso cref="IThetaLoggerFactory"/>.</param>
        /// <param name="dataHistoryProcessingService">The <seealso cref="IDataHistoryProcessingService"/> to call 
        /// for handling requests.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="loggerFactory"/> is null.
        /// or
        /// <paramref name="dataHistoryProcessingService"/> is null.
        /// or
        /// <paramref name="loggerFactory"/>is null.        
        /// </exception>
        public DataHistoryController(IThetaLoggerFactory loggerFactory,
            IDataHistoryProcessingService dataHistoryProcessingService) : base(loggerFactory)
        {
            _service = dataHistoryProcessingService ?? throw new ArgumentNullException(nameof(dataHistoryProcessingService));            
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Handles requests for fetching data history trends data.
        /// </summary>
        /// <param name="filters">The filters contains the assetid and groupname in the dictionary.</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet(Name = "GetDataHistoryTrends")]
        public IActionResult Get([FromQuery] IDictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(DataHistoryController)} {nameof(Get)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters,
                    QueryParams.AssetId, QueryParams.GroupName))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateAssetId(correlationId, out defaultInvalidStatusCodeResult,
                    out var parsedAssetId,
                    filters?[QueryParams.AssetId]))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var groupname = filters[QueryParams.GroupName];

            var input = new DataHistoryTrendInput()
            {
                AssetId = parsedAssetId,
                GroupName = groupname,
            };
            var requestWithCorrelationId = new WithCorrelationId<DataHistoryTrendInput>(
                correlationId, input);

            var serviceResult = _service.GetDataHistoryListData(requestWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var response = DataHistoryMapper.Map(requestWithCorrelationId.CorrelationId, serviceResult);
            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryController)} {nameof(Get)}", correlationId);

            return Ok(response);
        }

        /// <summary>
        /// Handles requests for fetching data history trends data.
        /// </summary>
        /// <param name="filters">The filters contains the assetid, itemid, startdate and end date.</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet("GetDataHistoryTrendItems", Name = "GetDataHistoryTrendItems")]
        public async Task<IActionResult> GetDataHistoryTrendItemsAsync([FromQuery] IDictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);

            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(DataHistoryController)} {nameof(GetDataHistoryTrendItemsAsync)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters,
                    QueryParams.AssetId, QueryParams.Type, QueryParams.ItemId,
                    QueryParams.StartDate, QueryParams.EndDate))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryController)} {nameof(GetDataHistoryTrendItemsAsync)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateAssetId(correlationId, out defaultInvalidStatusCodeResult,
                    out var parsedAssetId,
                    filters?[QueryParams.AssetId]))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryController)} {nameof(GetDataHistoryTrendItemsAsync)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var startDate = filters[QueryParams.StartDate];
            var endDate = filters[QueryParams.EndDate];
            filters.TryGetValue(QueryParams.Type, out var type);
            filters.TryGetValue(QueryParams.ItemId, out var itemId);

            var input = new DataHistoryTrendInput()
            {
                AssetId = parsedAssetId,
                TypeId = type,
                ItemId = itemId,
                StartDate = startDate,
                EndDate = endDate,
            };

            var requestWithCorrelationId = new WithCorrelationId<DataHistoryTrendInput>(
                correlationId, input);

            var serviceResult = await _service.GetDataHistoryTrendDataItemsAsync(requestWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryController)} {nameof(GetDataHistoryTrendItemsAsync)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var response = DataHistoryMapper.Map(requestWithCorrelationId.CorrelationId, serviceResult);

            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryController)} {nameof(GetDataHistoryTrendItemsAsync)}", correlationId);

            return Ok(response);

        }

        /// <summary>
        /// Handles requests for fetching data history alarm limits.
        /// </summary>
        /// <param name="filters">The filters contains the assetid and addresses in the dictionary.</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet("AlarmLimits", Name = "GetAlarmLimits")]
        public IActionResult GetAlarmLimits([FromQuery] IDictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);

            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(DataHistoryController)} {nameof(GetAlarmLimits)}", correlationId);

            if (!ValidateAssetId(correlationId, out var defaultInvalidStatusCodeResult,
                    out var parsedAssetId,
                    filters?[QueryParams.AssetId]))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryController)} {nameof(GetAlarmLimits)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }
            if (!ValidateAddressesArray(correlationId, out defaultInvalidStatusCodeResult,
                    out var addresses,
                    filters?[QueryParams.Addresses]))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryController)} {nameof(GetAlarmLimits)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }
            var input = new DataHistoryAlarmLimitsInput()
            {
                AssetId = parsedAssetId,
                Addresses = addresses,
            };
            var requestWithCorrelationId = new WithCorrelationId<DataHistoryAlarmLimitsInput>(
                correlationId, input);
            var serviceResult = _service.GetAlarmLimits(requestWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryController)} {nameof(GetAlarmLimits)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var response = DataHistoryAlarmLimitMapper.Map(requestWithCorrelationId.CorrelationId, serviceResult);

            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryController)} {nameof(GetAlarmLimits)}", correlationId);

            return Ok(response);
        }

        /// <summary>
        /// Handles requests for returning defaults trends data.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet("GetDefaultTrendsData", Name = "GetDefaultTrendsData")]
        public async Task<IActionResult> GetDefaultTrendsDataAsync([FromQuery] IDictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);

            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(DataHistoryController)} {nameof(GetDefaultTrendsDataAsync)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters,
                    QueryParams.AssetId, QueryParams.ViewId))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryController)} {nameof(GetDefaultTrendsDataAsync)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateAssetId(correlationId, out defaultInvalidStatusCodeResult,
                    out var parsedAssetId,
                    filters?[QueryParams.AssetId]))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryController)} {nameof(GetDefaultTrendsDataAsync)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var viewId = filters[QueryParams.ViewId];

            bool isOverlay = false;
            if (filters.TryGetValue(QueryParams.IsOverlay, out var value))
            {
                isOverlay = !string.IsNullOrEmpty(value)
                    && (value == "1" || value.ToLower() == "true");
            }

            var input = new DataHistoryTrendInput()
            {
                AssetId = parsedAssetId,
                ViewId = viewId,
                IsOverlay = isOverlay,
            };
            if (filters.TryGetValue(QueryParams.StartDate, out var startDate))
            {
                input.StartDate = startDate;
            }
            else
            {
                input.StartDate = null;
            }

            if (filters.TryGetValue(QueryParams.EndDate, out var endDate))
            {
                input.EndDate = endDate;
            }
            else
            {
                input.EndDate = null;
            }

            var requestWithCorrelationId = new WithCorrelationId<DataHistoryTrendInput>(
                correlationId, input);

            var serviceResult = await _service.GetDefaultTrendDataItemsAsync(requestWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryController)} {nameof(GetDefaultTrendsDataAsync)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var response = DataHistoryMapper.Map(requestWithCorrelationId.CorrelationId, serviceResult);

            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryController)} {nameof(GetDefaultTrendsDataAsync)}", correlationId);

            return Ok(response);
        }

        /// <summary>
        /// Handles requests for returning defaults trends data.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet("GetDefaultTrendViews", Name = "GetDefaultTrendViews")]
        public IActionResult GetDefaultTrendViews()
        {
            GetOrCreateCorrelationId(out var correlationId);

            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(DataHistoryController)} {nameof(GetDefaultTrendViews)}", correlationId);

            if (!ValidateUserAuthorized(correlationId, out var defaultInvalidStatusCodeResult, out var user))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryController)} {nameof(GetDefaultTrendViews)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var input = new DataHistoryTrendInput()
            {
                UserId = user,
            };

            var requestWithCorrelationId = new WithCorrelationId<DataHistoryTrendInput>(
                correlationId, input);

            var serviceResult = _service.GetDefaultTrendsViews(requestWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryController)} {nameof(GetDefaultTrendViews)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var response = DataHistoryMapper.Map(requestWithCorrelationId.CorrelationId, serviceResult);

            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryController)} {nameof(GetDefaultTrendViews)}", correlationId);

            return Ok(response);
        }

        /// <summary>
        /// Handles requests for fetching data history trends data.
        /// </summary>
        /// <param name="filters">The filters contains the assetid, itemid, startdate and end date.</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet("GetTrendDataAsync", Name = "GetTrendDataAsync")]
        public async Task<IActionResult> GetTrendDataAsync([FromQuery] IDictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);

            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(DataHistoryController)} {nameof(GetTrendDataAsync)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters,
                    QueryParams.AssetId, QueryParams.StartDate, QueryParams.EndDate, QueryParams.GroupName))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryController)} {nameof(GetTrendDataAsync)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateAssetId(correlationId, out defaultInvalidStatusCodeResult,
                    out var parsedAssetId,
                    filters?[QueryParams.AssetId]))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryController)} {nameof(GetTrendDataAsync)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var startDate = filters[QueryParams.StartDate];
            var endDate = filters[QueryParams.EndDate];
            var chart1Type = filters[QueryParams.Chart1Type];
            var chart1ItemId = filters[QueryParams.Chart1ItemId];
            var chart2Type = filters[QueryParams.Chart2Type];
            var chart2ItemId = filters[QueryParams.Chart2ItemId];
            var chart3Type = filters[QueryParams.Chart3Type];
            var chart3ItemId = filters[QueryParams.Chart3ItemId];
            var chart4Type = filters[QueryParams.Chart4Type];
            var chart4ItemId = filters[QueryParams.Chart4ItemId];
            var groupName = filters[QueryParams.GroupName];

            bool isOverlay = false;
            if (filters.TryGetValue(QueryParams.IsOverlay, out var value))
            {
                isOverlay = !string.IsNullOrEmpty(value)
                    && (value == "1" || value.ToLower() == "true");
            }

            var input = new TrendIDataInput()
            {
                AssetId = parsedAssetId,
                StartDate = startDate,
                EndDate = endDate,
                Chart1ItemId = chart1ItemId ?? string.Empty,
                Chart1Type = chart1Type ?? string.Empty,
                Chart2ItemId = chart2ItemId ?? string.Empty,
                Chart2Type = chart2Type ?? string.Empty,
                Chart3ItemId = chart3ItemId ?? string.Empty,
                Chart3Type = chart3Type ?? string.Empty,
                Chart4ItemId = chart4ItemId ?? string.Empty,
                Chart4Type = chart4Type ?? string.Empty,
                IsOverlay = isOverlay,
                GroupName = groupName,
            };

            var requestWithCorrelationId = new WithCorrelationId<TrendIDataInput>(
                correlationId, input);

            var serviceResult = await _service.GetTrendDataAsync(requestWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryController)} {nameof(GetTrendDataAsync)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var response = DataHistoryMapper.Map(requestWithCorrelationId.CorrelationId, serviceResult);

            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DataHistoryController)} {nameof(GetTrendDataAsync)}", correlationId);

            return Ok(response);

        }

        #endregion

    }
}
