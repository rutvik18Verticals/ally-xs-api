using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Contracts.Mappers;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Controllers
{
    /// <summary>
    /// The controller that fields requests for asset data.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class AssetDataController : BaseController
    {

        #region BaseController Members

        /// <summary>
        /// Gets or sets the controller logger.
        /// </summary>
        protected override IThetaLogger ControllerLogger => LoggerFactory.Create(LoggingModel.APIService);

        #endregion

        #region Private Fields

        private readonly IAssetDataService _assetDataService;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the <seealso cref="AssetDataController"/>.
        /// </summary>
        /// <param name="assetDataService">The <seealso cref="IAssetDataService"/> to call 
        /// for handling requests.</param>
        /// <param name="loggerFactory">
        /// The <seealso cref="IThetaLoggerFactory"/> service.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="assetDataService"/> is null.
        /// or
        /// <paramref name="loggerFactory"/>is null.
        /// </exception>
        public AssetDataController(IAssetDataService assetDataService, IThetaLoggerFactory loggerFactory) : base(loggerFactory)
        {
            _assetDataService = assetDataService ?? throw new ArgumentNullException(nameof(assetDataService));
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Gets the enabled status for a well.
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet("GetEnabledStatus")]
        public IActionResult GetEnabledStatus([FromQuery] Guid assetId)
        {
            GetOrCreateCorrelationId(out var correlationId);

            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(AssetDataController)} {nameof(GetEnabledStatus)}",
                correlationId);

            if (assetId == Guid.Empty)
            {
                ControllerLogger.WriteCId(Level.Info, "assetId is Required", correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(AssetDataController)} {nameof(GetEnabledStatus)}",
                    correlationId);

                return BadRequest();
            }

            var inputWithCorrelationId = new WithCorrelationId<Guid>(correlationId, assetId);
            var wellEnabledStatusOutput = _assetDataService.GetEnabledStatus(inputWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out var invalidStatusCode, wellEnabledStatusOutput))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(AssetDataController)} {nameof(GetEnabledStatus)}",
                    correlationId);

                return invalidStatusCode;
            }

            var response = AssetDataMapper.Map(wellEnabledStatusOutput);
            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(AssetDataController)} {nameof(GetEnabledStatus)}",
                correlationId);

            return Ok(response);
        }

        #endregion

    }
}
