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
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Controllers
{
    /// <summary>
    /// The controller that fields requests for Rod Lift Analysis results.
    /// </summary>
    [ApiController]
    [Route("Analytics/[controller]")]
    [Authorize]
    public class RLAnalysisController : BaseController
    {

        #region BaseController Members

        /// <summary>
        /// Gets or sets the controller logger.
        /// </summary>
        protected override IThetaLogger ControllerLogger => LoggerFactory.Create(LoggingModel.RodLiftAnalysis);

        #endregion

        #region Private Fields

        private readonly IRodLiftAnalysisProcessingService _service;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the <seealso cref="RLAnalysisController"/>.
        /// </summary>
        /// <param name="loggerFactory">The <seealso cref="IThetaLoggerFactory"/>.</param>
        /// <param name="rodLiftAnalysisProcessingService">The <seealso cref="IRodLiftAnalysisProcessingService"/> to call 
        /// for handling requests.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="loggerFactory"/> is null.
        /// or
        /// <paramref name="rodLiftAnalysisProcessingService"/> is null.
        /// </exception>
        public RLAnalysisController(IThetaLoggerFactory loggerFactory,
            IRodLiftAnalysisProcessingService rodLiftAnalysisProcessingService) : base(loggerFactory)
        {
            _service = rodLiftAnalysisProcessingService ??
                throw new ArgumentNullException(nameof(rodLiftAnalysisProcessingService));
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Handles requests for getting  Rod Lift Analysis results.
        /// </summary>
        /// <param name="filters">The filters for rod lift analysis.</param>
        /// <returns>The RodLiftAnalysisResponse json</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet(Name = "GetRodLiftAnalysis")]
        public async Task<IActionResult> GetRodLiftAnalysis([FromQuery] IDictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(RLAnalysisController)} {nameof(GetRodLiftAnalysis)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters, QueryParams.AssetId, QueryParams.CardDate))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RLAnalysisController)} {nameof(GetRodLiftAnalysis)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateAssetId(correlationId, out defaultInvalidStatusCodeResult, out var parsedAssetId, filters?[QueryParams.AssetId]))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RLAnalysisController)} {nameof(GetRodLiftAnalysis)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var rodLiftAnalysisInput = new RodLiftAnalysisInput()
            {
                AssetId = parsedAssetId,
                CardDate = filters?[QueryParams.CardDate],
            };

            var requestWithCorrelationId = new WithCorrelationId<RodLiftAnalysisInput>
                (correlationId, rodLiftAnalysisInput);

            var serviceResult = await _service.GetRodLiftAnalysisResultsAsync(requestWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RLAnalysisController)} {nameof(GetRodLiftAnalysis)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }
            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RLAnalysisController)} {nameof(GetRodLiftAnalysis)}", correlationId);

            return Ok(RodLiftAnalysisDataMapper.Map(correlationId, serviceResult));
        }

        #endregion

    }
}
