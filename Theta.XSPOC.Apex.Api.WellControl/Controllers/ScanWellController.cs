using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Common.Communications;
using Theta.XSPOC.Apex.Api.Common.Communications.Models;
using Theta.XSPOC.Apex.Api.WellControl.Contracts.V2;
using Theta.XSPOC.Apex.Api.WellControl.Logging;
using Theta.XSPOC.Apex.Api.WellControl.Services;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Data.Updates.Models;
using Theta.XSPOC.Apex.Kernel.Integration;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.WellControl.Controllers
{
    /// <summary>
    /// The controller that fields requests for scanning a well or reading registers..
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ScanWellController : BaseController
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

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the <seealso cref="WellControlController"/>.
        /// </summary>
        /// <param name="service">
        /// The <seealso cref="IProcessingDataUpdatesService"/> to call for handling requests.
        /// </param>
        /// <param name="transactionPayloadCreator">The transaction payload creator.</param>
        /// <param name="loggerFactory">The logging factory.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="service"/> is null or
        /// <paramref name="transactionPayloadCreator"/> is null or
        /// <paramref name="transactionPayloadCreator"/> is null or
        /// <paramref name="loggerFactory"/> is null.
        /// </exception>
        public ScanWellController(IProcessingDataUpdatesService service,
            ITransactionPayloadCreator transactionPayloadCreator, IThetaLoggerFactory loggerFactory) : base(loggerFactory)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _transactionPayloadCreator = transactionPayloadCreator
                ?? throw new ArgumentNullException(nameof(transactionPayloadCreator));
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
        [HttpPost(Name = "ScanWell")]
        public async Task<IActionResult> RunWellControl([FromBody] IDictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);

            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(ScanWellController)} {nameof(RunWellControl)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters,
                    QueryParams.AssetId,
                    QueryParams.ControlType))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(ScanWellController)} {nameof(RunWellControl)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateAssetId(correlationId, out defaultInvalidStatusCodeResult,
                    out var parsedAssetId,
                    filters?[QueryParams.AssetId]))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(ScanWellController)} {nameof(RunWellControl)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            filters.TryGetValue("controlType", out var controlTypeRaw);
            filters.TryGetValue("socketId", out var socketId);
            filters.TryGetValue("equipmentSelection", out var equipmentSelectionRaw);

            var controlType = (DeviceControlType)int.Parse(controlTypeRaw);

            if (controlType != DeviceControlType.Scan)
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(ScanWellController)} {nameof(RunWellControl)}", correlationId);

                return BadRequest("Invalid control type");
            }

            int? equipmentSelection = !string.IsNullOrEmpty(equipmentSelectionRaw) ? int.Parse(equipmentSelectionRaw) : null;

            var result = await SendWellControlTransaction(parsedAssetId, controlType, socketId, equipmentSelection);

            if (result.Status == false)
            {
                ControllerLogger.WriteCId(Level.Info, "Result status is false", correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(ScanWellController)} {nameof(RunWellControl)}", correlationId);

                return BadRequest(result.Value);
            }

            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(ScanWellController)} {nameof(RunWellControl)}", correlationId);

            return Ok(result.Value);
        }

        #endregion

        #region Private Methods

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
                Payload = JsonConvert.SerializeObject(payload),
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
