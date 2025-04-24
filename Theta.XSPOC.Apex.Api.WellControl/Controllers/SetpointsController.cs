using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.WellControl.Contracts.Mappers;
using Theta.XSPOC.Apex.Api.WellControl.Contracts.Requests;
using Theta.XSPOC.Apex.Api.WellControl.Logging;
using Theta.XSPOC.Apex.Api.WellControl.Services;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.WellControl.Controllers
{
    /// <summary>
    /// The controller that fields requests for Setpoint.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class SetpointsController : BaseController
    {

        #region BaseController Members

        /// <summary>
        /// Gets or sets the controller logger.
        /// </summary>
        protected override IThetaLogger ControllerLogger => LoggerFactory.Create(LoggingModel.WellControl);

        #endregion

        #region Private Methods

        private readonly IProcessingDataUpdatesService _service;
        private readonly ISetpointGroupProcessingService _setpointGroupService;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the <seealso cref="SetpointsController"/>.
        /// </summary>
        public SetpointsController(
            ISetpointGroupProcessingService setpointGroupService, IProcessingDataUpdatesService service, IThetaLoggerFactory loggerFactory) : base(loggerFactory)
        {
            _setpointGroupService = setpointGroupService ?? throw new ArgumentNullException(nameof(setpointGroupService));
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Handles requests for Setpoint.
        /// </summary>
        /// <param name="filters">The filters contains the assetId and Setpoint in the dictionary.</param>
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet(Name = "GetSetpoint")]
        public async Task<IActionResult> Get([FromQuery] IDictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);

            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(SetpointsController)} {nameof(Get)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters,
                   QueryParams.AssetId, QueryParams.Addresses, QueryParams.SocketId))
            {
                var message = $"{nameof(defaultInvalidStatusCodeResult)} , Invalid Query Params.";
                ControllerLogger.WriteCId(Level.Info, message, correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateAssetId(correlationId, out defaultInvalidStatusCodeResult,
                    out var parsedAssetId,
                    filters?[QueryParams.AssetId]))
            {
                var message = $"{nameof(defaultInvalidStatusCodeResult)} , Invalid Asset Id.";
                ControllerLogger.WriteCId(Level.Info, message, correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }
            if (!ValidateAddressesArray(correlationId, out defaultInvalidStatusCodeResult,
                    out var addresses,
                    filters?[QueryParams.Addresses]))
            {
                ControllerLogger.WriteCId(Level.Info, "Invalid addresses", correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            filters.TryGetValue(QueryParams.SocketId, out var socketId);

            if (string.IsNullOrEmpty(socketId))
            {
                ControllerLogger.WriteCId(Level.Info, "Missing socket id", correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var result = await _service.SendReadRegisterTransaction(parsedAssetId, addresses, socketId);

            if (result.Status == false)
            {
                ControllerLogger.WriteCId(Level.Info, "Result status is false", correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(Get)}", correlationId);

                return BadRequest(result.Value);
            }

            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(Get)}", correlationId);

            return Accepted(result.Value);
        }

        /// <summary>
        /// Handles requests for Setpoint to return list of SetpointGroups.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet("GetSetpointGroups", Name = "GetSetpointGroups")]
        public IActionResult GetSetpointGroups([FromQuery] IDictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);

            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(SetpointsController)} {nameof(GetSetpointGroups)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters,
                   QueryParams.AssetId))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(GetSetpointGroups)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateAssetId(correlationId, out defaultInvalidStatusCodeResult,
                    out var parsedAssetId,
                    filters?[QueryParams.AssetId]))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(GetSetpointGroups)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var serviceResult = _setpointGroupService.GetSetpointGroups(parsedAssetId, correlationId);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(GetSetpointGroups)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var response = SetpointGroupDataMapper.Map(serviceResult);

            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(GetSetpointGroups)}", correlationId);

            return Ok(response);
        }

        /// <summary>
        /// Handles requests for returning mock setpoint json.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet("GetMockJson", Name = "GetMockJson")]
        public IActionResult GetMockJson()
        {
            var setpointsGroups = new MockSetpointGroup();
            using (var r = new StreamReader("MockJson/setpointmockjson.json"))
            {
                var json = r.ReadToEnd();
                setpointsGroups = JsonConvert.DeserializeObject<MockSetpointGroup>(json);
            }

            return Ok(setpointsGroups);
        }

        /// <summary>
        /// Handles requests for Put Setpoint.
        /// </summary>
        /// <param name="request">The AssetId addressValues contains the addresses and Setpoint in the dictionary.</param>
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpPut(Name = "PutSetpoint")]
        public async Task<IActionResult> Put([FromBody] PutRequest request)
        {
            GetOrCreateCorrelationId(out var correlationId);

            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(SetpointsController)} {nameof(Put)}", correlationId);

            if (request == null)
            {
                ControllerLogger.WriteCId(Level.Info, "Missing request", correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(Put)}", correlationId);

                return BadRequest();
            }

            if (!ValidateBodyParams(correlationId, out var defaultInvalidStatusCodeResult, request.AddressValues))
            {
                ControllerLogger.WriteCId(Level.Info, "Invalid body params", correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(Put)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateAssetId(correlationId, out defaultInvalidStatusCodeResult,
                    out var parsedAssetId,
                    request.AssetId))
            {
                var message = $"{nameof(defaultInvalidStatusCodeResult)} , Invalid Asset Id.";
                ControllerLogger.WriteCId(Level.Info, message, correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(Put)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (string.IsNullOrEmpty(request.SocketId))
            {
                ControllerLogger.WriteCId(Level.Info, "Missing socket id", correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(Put)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var result = await _service.SendWriteRegisterTransaction(parsedAssetId, request.AddressValues, request.SocketId);

            if (result.Status == false)
            {
                ControllerLogger.WriteCId(Level.Info, "Result status is false", correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(Put)}", correlationId);

                return BadRequest(result.Value);
            }

            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(Put)}", correlationId);

            return Accepted(result.Value);
        }

        #endregion

    }
}
