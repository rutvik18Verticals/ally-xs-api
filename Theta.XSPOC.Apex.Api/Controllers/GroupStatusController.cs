using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Contracts.Mappers;
using Theta.XSPOC.Apex.Api.Contracts.Responses;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Controllers
{
    /// <summary>
    /// The controller that fields requests for GroupStatusTypes.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class GroupStatusController : BaseController
    {

        #region BaseController Members

        /// <summary>
        /// Gets or sets the controller logger.
        /// </summary>
        protected override IThetaLogger ControllerLogger => LoggerFactory.Create(LoggingModel.GroupStatus);

        #endregion

        #region Private Fields

        private readonly IGroupStatusProcessingService _service;
        private readonly ICommonService _commonService;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the <seealso cref="GroupStatusController"/>.
        /// </summary>
        /// <param name="loggerFactory">The <seealso cref="IThetaLoggerFactory"/>.</param>
        /// <param name="groupStatusProcessingService">The <seealso cref="IGroupStatusProcessingService"/> to call 
        /// for handling requests.</param>
        /// <param name="commonService"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="loggerFactory"/> is null.
        /// or
        /// <paramref name="groupStatusProcessingService"/> is null.
        /// </exception>
        public GroupStatusController(IThetaLoggerFactory loggerFactory,
            IGroupStatusProcessingService groupStatusProcessingService,
            ICommonService commonService) : base(loggerFactory)
        {
            _service = groupStatusProcessingService ?? throw new ArgumentNullException(nameof(groupStatusProcessingService));
            _commonService = commonService;
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Handles requests for group status.
        /// </summary>
        /// <param name="filters">A dictionary of filters with the view Id as a key-value pair.</param>
        /// <returns>Returns the group status for the given view Id and asset Ids.</returns>
        [HttpGet(Name = "GetGroupStatus")]
        [ProducesResponseType(typeof(GroupStatusResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get([FromQuery] IDictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusController)} {nameof(Get)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters, QueryParams.ViewId,
                QueryParams.GroupName))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var groupStatusInput = new GroupStatusInput
            {
                ViewId = filters?[QueryParams.ViewId],
                GroupName = filters?[QueryParams.GroupName]
            };

            var requestWithCorrelationId = new WithCorrelationId<GroupStatusInput>(
                correlationId, groupStatusInput);

            var serviceResult = _service.GetGroupStatus(requestWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }
            var digits = _commonService.GetSystemParameterNextGenSignificantDigits(correlationId);
            var response = GroupStatusDataMapper.Map(requestWithCorrelationId.CorrelationId, serviceResult, digits);
            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusController)} {nameof(Get)}", correlationId);

            return Ok(response);
        }

        /// <summary>
        /// Handles requests for getting classification status.
        /// </summary>
        /// <param name="filters">A dictionary of filters with the view Id and group name as a key-value pair.</param>
        /// <returns>Returns the data for classification widgets.</returns>
        [HttpGet("GetGroupClassificationWidgetData", Name = "GetGroupClassificationWidgetData")]
        [ProducesResponseType(typeof(GroupStatusResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetGroupClassificationWidgetData([FromQuery] IDictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusController)} {nameof(GetGroupClassificationWidgetData)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters,
                QueryParams.GroupName))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusController)} {nameof(GetGroupClassificationWidgetData)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var groupStatusInput = new GroupStatusInput
            {
                GroupName = filters?[QueryParams.GroupName]
            };

            var requestWithCorrelationId = new WithCorrelationId<GroupStatusInput>(
                correlationId, groupStatusInput);

            var serviceResult = _service.GetClassificationWidgetData(requestWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusController)} {nameof(GetGroupClassificationWidgetData)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var digits = _commonService.GetSystemParameterNextGenSignificantDigits(correlationId);
            var response = GroupStatusDataMapper.MapGroupClassifications(requestWithCorrelationId.CorrelationId, serviceResult, digits);
            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusController)} {nameof(GetGroupClassificationWidgetData)}", correlationId);

            return Ok(response);
        }

        /// <summary>
        /// Handles requests for getting classification status.
        /// </summary>
        /// <param name="filters">A dictionary of filters with the view Id and group name as a key-value pair.</param>
        /// <returns>Returns the data for classification widgets.</returns>
        [HttpGet("GetAlarmsWidgetDataAsync", Name = "GetAlarmsWidgetDataAsync")]
        [ProducesResponseType(typeof(GroupStatusResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAlarmsWidgetDataAsync([FromQuery] IDictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusController)} {nameof(GetAlarmsWidgetDataAsync)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters,
                QueryParams.GroupName))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusController)} {nameof(GetAlarmsWidgetDataAsync)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var groupStatusInput = new GroupStatusInput
            {
                GroupName = filters?[QueryParams.GroupName]
            };

            var requestWithCorrelationId = new WithCorrelationId<GroupStatusInput>(
                correlationId, groupStatusInput);

            var serviceResult = await _service.GetAlarmsWidgetDataAsync(requestWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusController)} {nameof(GetAlarmsWidgetDataAsync)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var digits = _commonService.GetSystemParameterNextGenSignificantDigits(correlationId);
            var response = GroupStatusDataMapper.Map(requestWithCorrelationId.CorrelationId, serviceResult, digits);
            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusController)} {nameof(GetAlarmsWidgetDataAsync)}", correlationId);

            return Ok(response);
        }

        /// <summary>
        /// Gets the downtime by wells.
        /// </summary>
        /// <param name="filters">A dictionary of filters with the group name as a key-value pair.</param>
        /// <returns>The downtime by wells KPI.</returns>
        [HttpGet("DowntimeByWells", Name = "GetDowntimeByWells")]
        [ProducesResponseType(typeof(GroupStatusResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDowntimeByWells([FromQuery] IDictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusController)} {nameof(GetDowntimeByWells)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters,
                    QueryParams.GroupName))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusController)} {nameof(GetDowntimeByWells)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var groupStatusInput = new GroupStatusInput
            {
                GroupName = filters?[QueryParams.GroupName]
            };

            var requestWithCorrelationId = new WithCorrelationId<GroupStatusInput>(
                correlationId, groupStatusInput);

            var serviceResult = await _service.GetDowntimeByWellsAsync(requestWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusController)} {nameof(GetDowntimeByWells)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var digits = _commonService.GetSystemParameterNextGenSignificantDigits(correlationId);
            var response = GroupStatusDataMapper.Map(requestWithCorrelationId.CorrelationId, serviceResult, digits);
            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusController)} {nameof(GetDowntimeByWells)}", correlationId);

            return Ok(response);
        }

        /// <summary>
        /// Gets the downtime by run status.
        /// </summary>
        /// <param name="filters">A dictionary of filters with the group name as a key-value pair.</param>
        /// <returns>The downtime by run status KPI.</returns>
        [HttpGet("DowntimeByRunStatus", Name = "GetDowntimeByRunStatus")]
        [ProducesResponseType(typeof(GroupStatusResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDowntimeByRunStatus([FromQuery] IDictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusController)} {nameof(GetDowntimeByRunStatus)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters,
                    QueryParams.GroupName))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusController)} {nameof(GetDowntimeByRunStatus)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var groupStatusInput = new GroupStatusInput
            {
                GroupName = filters?[QueryParams.GroupName]
            };

            var requestWithCorrelationId = new WithCorrelationId<GroupStatusInput>(
                correlationId, groupStatusInput);

            var serviceResult = await _service.GetDowntimeByRunStatusAsync(requestWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusController)} {nameof(GetDowntimeByRunStatus)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var digits = _commonService.GetSystemParameterNextGenSignificantDigits(correlationId);
            var response = GroupStatusDataMapper.Map(requestWithCorrelationId.CorrelationId, serviceResult, digits);
            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusController)} {nameof(GetDowntimeByRunStatus)}", correlationId);

            return Ok(response);
        }

        #endregion

    }
}
