using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Contracts.Mappers;
using Theta.XSPOC.Apex.Api.Contracts.Responses.AssetStatus;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Services.AssetStatus;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Level = Theta.XSPOC.Apex.Kernel.Logging.Models.Level;

namespace Theta.XSPOC.Apex.Api.Controllers
{
    /// <summary>
    /// The rod lift asset controller.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AssetStatusController : BaseController
    {

        #region BaseController Members

        /// <summary>
        /// Gets or sets the controller logger.
        /// </summary>
        protected override IThetaLogger ControllerLogger => LoggerFactory.Create(LoggingModel.GroupAndAsset);

        #endregion

        #region Private Fields

        private readonly IAssetStatusService _assetService;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="AssetStatusController"/> using the provided
        /// <paramref name="assetService"/>. 
        /// </summary>
        /// <param name="assetService"></param>
        /// <param name="loggerFactory">
        /// The <seealso cref="IThetaLoggerFactory"/> service.</param>
        /// <exception cref="ArgumentException">
        /// When <paramref name="assetService"/> is null.
        /// or
        /// <paramref name="loggerFactory"/>is null.
        /// </exception>
        public AssetStatusController(IAssetStatusService assetService, IThetaLoggerFactory loggerFactory) : base(loggerFactory)
        {
            _assetService = assetService ?? throw new ArgumentNullException(nameof(assetService));
        }

        #endregion

        #region Api Methods

        /// <summary>
        /// This is the api endpoint to get rod lift asset status data for the provided <paramref name="filters"/>
        /// </summary>
        /// <param name="filters">The filter contain asset id used to get the status data.</param>
        /// <returns>
        /// The <seealso cref="AssetStatusDataResponse"/> asset data. If the <paramref name="filters"/> is null
        /// then null is returned.
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(Name = "Status/{assetId}/")]
        public async Task<IActionResult> Get([FromQuery] IDictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(AssetStatusController)} " +
               $"{nameof(Get)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters,
                   QueryParams.AssetId))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished " +
                    $"{nameof(AssetStatusController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateAssetId(correlationId, out defaultInvalidStatusCodeResult,
                    out var parsedAssetId,
                    filters?[QueryParams.AssetId]))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished " +
                    $"{nameof(AssetStatusController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateUserAuthorized(correlationId, out defaultInvalidStatusCodeResult, out var user))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished " +
                    $"{nameof(AssetStatusController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var assetStatusInput = new AssetStatusInput()
            {
                AssetId = parsedAssetId,
                UserId = user,
            };

            var assetStatusData =
                await _assetService.GetAssetStatusDataAsync(
                    new WithCorrelationId<AssetStatusInput>(correlationId, assetStatusInput));

            if (assetStatusData == null)
            {
                ControllerLogger.WriteCId(Level.Info, "Unable to retrieve asset status.", correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished " +
                    $"{nameof(AssetStatusController)} {nameof(Get)}", correlationId);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            var result = AssetStatusMapper.Map(correlationId, assetStatusData?.Value);

            if (result != null)
            {
                ControllerLogger.WriteCId(Level.Info, "Mapped asset status.", correlationId);
            }
            else
            {
                ControllerLogger.WriteCId(Level.Info, "Unable to map asset status.", correlationId);
            }
            ControllerLogger.WriteCId(Level.Trace, $"Finished " +
                $"{nameof(AssetStatusController)} {nameof(Get)}", correlationId);

            return Ok(result);
        }

        #endregion

    }
}