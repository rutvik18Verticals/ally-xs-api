using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Common.Communications;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Setpoint.Contracts.Requests;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Setpoint.Controllers
{
    /// <summary>
    /// The controller that fields requests for Setpoint.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class SetpointsController : BaseController
    {

        #region BaseController Members

        /// <summary>
        /// Gets or sets the controller logger.
        /// </summary>
        protected override IThetaLogger ControllerLogger => LoggerFactory.Create(LoggingModel.APIService);

        #endregion

        #region Private Methods

        private readonly ITransactionPayloadCreator _transactionPayloadCreator;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the <seealso cref="SetpointsController"/>.
        /// </summary>
        public SetpointsController(ITransactionPayloadCreator transactionPayloadCreator, IThetaLoggerFactory loggerFactory) : base(loggerFactory)
        {
            _transactionPayloadCreator = transactionPayloadCreator ?? throw new ArgumentNullException(nameof(transactionPayloadCreator));
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
        public IActionResult Get([FromQuery] IDictionary<string, string> filters)
        {
            if (!ValidateQueryParams(string.Empty, out var defaultInvalidStatusCodeResult, filters,
                   QueryParams.AssetId, QueryParams.Addresses))
            {
                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateAssetId(string.Empty, out defaultInvalidStatusCodeResult,
                    out var parsedAssetId,
                    filters?[QueryParams.AssetId]))
            {
                return defaultInvalidStatusCodeResult;
            }
            if (!ValidateAddressesArray(string.Empty, out defaultInvalidStatusCodeResult,
                    out var addresses,
                    filters?[QueryParams.Addresses]))
            {
                return defaultInvalidStatusCodeResult;
            }

            // todo: this is obsolete, remove this project. Adding to build.
            var correlationId = Guid.NewGuid().ToString();

            _transactionPayloadCreator.CreateReadRegisterPayload(
                parsedAssetId, addresses, correlationId, out var transactionPayload);

            return Accepted(parsedAssetId + "-" + addresses.Length, transactionPayload);
        }

        /// <summary>
        /// Handles requests for Put Setpoint.
        /// </summary>
        /// <param name="request">The AssetId addressValues contains the addresses and Setpoint in the dictionary.</param>
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpPut(Name = "PutSetpoint")]
        public IActionResult Put([FromBody] PutRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }

            if (!ValidateBodyParams(string.Empty, out var defaultInvalidStatusCodeResult, request.AddressValues))
            {
                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateAssetId(string.Empty, out defaultInvalidStatusCodeResult,
                    out var parsedAssetId,
                    request.AssetId))
            {
                return defaultInvalidStatusCodeResult;
            }

            // todo: this is obsolete, remove this project. Adding to build.
            var correlationId = Guid.NewGuid().ToString();

            _transactionPayloadCreator.CreateWriteRegisterPayload(
                parsedAssetId, request.AddressValues, correlationId, out var transactionPayload);

            return Accepted(transactionPayload);
        }

        #endregion

    }
}
