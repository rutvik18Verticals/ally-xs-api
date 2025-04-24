using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Common.Communications;
using Theta.XSPOC.Apex.Api.Common.Communications.Models;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.WellControl.Contracts.Mappers;
using Theta.XSPOC.Apex.Api.WellControl.Contracts.Requests;
using Theta.XSPOC.Apex.Api.WellControl.Contracts.Responses;
using Theta.XSPOC.Apex.Api.WellControl.Contracts.V2;
using Theta.XSPOC.Apex.Api.WellControl.Logging;
using Theta.XSPOC.Apex.Api.WellControl.Services;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Data.Updates.Models;
using Theta.XSPOC.Apex.Kernel.Integration;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Theta.XSPOC.Apex.Api.WellControl.Controllers
{
    /// <summary>
    /// The controller that fields requests for WellControls.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WellControlController : BaseController
    {

        #region BaseController Members

        /// <summary>
        /// Gets or sets the controller logger.
        /// </summary>
        protected override IThetaLogger ControllerLogger => LoggerFactory.Create(LoggingModel.WellControl);

        #endregion

        #region Private Fields

        private readonly IProcessingDataUpdatesService _service;
        private readonly ITransactionPayloadCreator _transactionPayloadCreator;
        private readonly IWellEnableDisableService _wellEnableDisableService;
        private readonly IWellControlService _wellControlService;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the <seealso cref="WellControlController"/>.
        /// </summary>
        /// <param name="service">
        /// The <seealso cref="IProcessingDataUpdatesService"/> to call for handling requests.
        /// </param>
        /// <param name="transactionPayloadCreator">The transaction payload creator.</param>
        /// <param name="wellEnableDisableService"></param>
        /// <param name="wellControlService">The well control service.</param>
        /// <param name="loggerFactory">The logging factory.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="service"/> is null or
        /// <paramref name="transactionPayloadCreator"/> is null or
        /// <paramref name="wellEnableDisableService"/> is null or 
        /// <paramref name="transactionPayloadCreator"/> is null or
        /// <paramref name="wellControlService"/> is null or
        /// <paramref name="loggerFactory"/> is null.
        /// </exception>
        public WellControlController(IProcessingDataUpdatesService service,
            ITransactionPayloadCreator transactionPayloadCreator, IWellEnableDisableService wellEnableDisableService,
            IWellControlService wellControlService, IThetaLoggerFactory loggerFactory) : base(loggerFactory)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _transactionPayloadCreator = transactionPayloadCreator
                ?? throw new ArgumentNullException(nameof(transactionPayloadCreator));
            _wellEnableDisableService = wellEnableDisableService ?? throw new ArgumentNullException(nameof(wellEnableDisableService));
            _wellControlService = wellControlService ?? throw new ArgumentNullException(nameof(wellControlService));
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Runs well control.
        /// </summary>
        /// <param name="filters">The filters.</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [Authorize(Policy = "WellControl")]
        [HttpPost(Name = "RunWellControl")]
        public async Task<IActionResult> RunWellControl([FromBody] IDictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);

            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(SetpointsController)} {nameof(RunWellControl)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters,
                QueryParams.AssetId,
                QueryParams.ControlType))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(RunWellControl)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateAssetId(correlationId, out defaultInvalidStatusCodeResult,
                out var parsedAssetId,
                filters?[QueryParams.AssetId]))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(RunWellControl)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            filters.TryGetValue("controlType", out var controlTypeRaw);
            filters.TryGetValue("socketId", out var socketId);
            filters.TryGetValue("equipmentSelection", out var equipmentSelectionRaw);

            var controlType = (DeviceControlType)int.Parse(controlTypeRaw);
            int? equipmentSelection = !string.IsNullOrEmpty(equipmentSelectionRaw) ?
                int.Parse(equipmentSelectionRaw) : null;

            var result = await SendWellControlTransaction(parsedAssetId, controlType, socketId, equipmentSelection);

            if (result.Status == false)
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(RunWellControl)}", correlationId);

                return BadRequest(result.Value);
            }

            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(RunWellControl)}", correlationId);

            return Ok(result.Value);
        }

        /// <summary>
        /// Handles requests for disabling and enabling a well.
        /// </summary>
        /// <param name="request">The filters contains the nodeID, enabled, dataCollection and disableCode</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [Authorize(Policy = "WellConfig")]
        [HttpPut("WellEnableDisable", Name = "WellEnableDisable")]
        public IActionResult WellEnableDisable([FromBody] EnableDisableWellRequest request)
        {
            GetOrCreateCorrelationId(out var correlationId);

            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(SetpointsController)} {nameof(WellEnableDisable)}", correlationId);

            if (request == null)
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(WellEnableDisable)}", correlationId);

                return BadRequest();
            }

            if (!ValidateAssetId(correlationId, out var defaultInvalidStatusCodeResult,
                    out var parsedAssetId,
                    request.AssetId))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(WellEnableDisable)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (string.IsNullOrEmpty(request.Enabled) ||
                string.IsNullOrEmpty(request.DataCollection) ||
                string.IsNullOrEmpty(request.SocketId))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(WellEnableDisable)}", correlationId);

                return BadRequest();
            }

            var data = _wellEnableDisableService.WellEnableDisableAsync(parsedAssetId,
                request.Enabled, request.DataCollection, request.DisableCode, request.SocketId);

            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(WellEnableDisable)}", correlationId);

            return Accepted(data.Result);
        }

        /// <summary>
        /// Gets available well control actions for a well with specified <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <returns>A <see cref="GetWellControlActionsResponse"/> containing supported well control actions.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("GetWellControlActions", Name = "GetWellControlActions")]
        public IActionResult GetWellControlActions([FromQuery] Guid assetId)
        {
            GetOrCreateCorrelationId(out var correlationId);

            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(SetpointsController)} {nameof(GetWellControlActions)}", correlationId);

            if (assetId == Guid.Empty)
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(GetWellControlActions)}", correlationId);

                return BadRequest();
            }

            var controlActionsServiceOutput = _wellControlService.GetWellControlActions(assetId, correlationId);

            if (!ValidateServiceResult(correlationId, out var invalidStatusCode, controlActionsServiceOutput))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(GetWellControlActions)}", correlationId);

                return invalidStatusCode;
            }

            var filteredActions = FilterWellControlPermissions(controlActionsServiceOutput);

            var response = WellControlMapper.Map(filteredActions);

            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SetpointsController)} {nameof(GetWellControlActions)}", correlationId);

            return Ok(response);
        }

        #endregion

        #region Private Methods

        private GetWellControlActionsOutput FilterWellControlPermissions(GetWellControlActionsOutput controlActionsServiceOutput)
        {
            var filteredActions = controlActionsServiceOutput;
            var hasWellControlPermissions = User.Claims.FirstOrDefault(c => c.Type == "WellControl")?.Value == "True";
            var hasWellConfigPermissions = User.Claims.FirstOrDefault(c => c.Type == "WellConfig")?.Value == "True";

            filteredActions.CanConfigWell = hasWellConfigPermissions;

            if (hasWellControlPermissions)
            {
                return filteredActions;
            }

            filteredActions.WellControlActions = filteredActions.WellControlActions.Where(a => a.Name == "Scan" || a.Name == "Fast Scan").ToList();

            return filteredActions;
        }

        private async Task<MethodResult<string>> SendWellControlTransaction(Guid assetGuid, DeviceControlType controlType,
            string socketId, int? equipmentSelection)
        {
            UpdatePayload payload;
            MethodResult<string> payloadCreationResult;

            if (!equipmentSelection.HasValue)
            {
                payloadCreationResult = _transactionPayloadCreator.CreateWellControlPayload(out payload, assetGuid, controlType, socketId);
            }
            else
            {
                payloadCreationResult = _transactionPayloadCreator.CreateWellControlPayload(out payload, assetGuid,
                    controlType, equipmentSelection.Value, socketId);
            }

            if (payloadCreationResult.Status != true)
            {
                return payloadCreationResult;
            }

            var dataUpdateEvent = new DataUpdateEvent
            {
                Action = "insert",
                PayloadType = "tblTransactions",
                Payload = JsonSerializer.Serialize(payload),
            };

            var request = new WithCorrelationId<DataUpdateEvent>(socketId, dataUpdateEvent);

            var sendTransactionResult = await _service.ProcessDataUpdatesAsync(request, assetGuid);

            if (sendTransactionResult != ConsumerBaseAction.Success)
            {
                return new MethodResult<string>(false, "Transaction could not be sent");
            }

            return new MethodResult<string>(true, "Transaction sent successfully");
        }

        #endregion

    }
}
