using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
    /// The controller for the card coordinates.
    /// </summary>
    [Route("Analytics/RLAnalysis/[controller]")]
    [ApiController]
    [Authorize]
    public class RLCoordinatesController : BaseController
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
        /// Constructs a new instance of the <seealso cref="RLCoordinatesController"/>.
        /// </summary>
        /// <param name="cardCoordinateProcessingService">The <seealso cref="IRodLiftAnalysisProcessingService"/> to call 
        /// for handling requests.</param>
        /// <param name="loggerFactory">The <seealso cref="IThetaLoggerFactory"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="loggerFactory"/> is null.
        /// or
        /// <paramref name="cardCoordinateProcessingService"/> is null.
        /// </exception>
        public RLCoordinatesController(IThetaLoggerFactory loggerFactory,
            IRodLiftAnalysisProcessingService cardCoordinateProcessingService) : base(loggerFactory)
        {
            _service = cardCoordinateProcessingService ??
                throw new ArgumentNullException(nameof(cardCoordinateProcessingService));
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Handles requests for getting  Card Coordinate results.
        /// </summary>
        /// <param name="filters">The filters for getting Card Coordinate</param>
        /// <returns>The CardCoordinateResponse json</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet(Name = "GetCardCoordinate")]
        public IActionResult GetCardCoordinate([FromQuery] IDictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(RLCoordinatesController)} {nameof(GetCardCoordinate)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters,
                    QueryParams.AssetId, QueryParams.CardDate))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RLCoordinatesController)} {nameof(GetCardCoordinate)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var cardDate = filters?[QueryParams.CardDate];

            if (!ValidateAssetId(correlationId, out defaultInvalidStatusCodeResult,
                    out var parsedAssetId,
                    filters?[QueryParams.AssetId]))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RLCoordinatesController)} {nameof(GetCardCoordinate)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var cardCoordinateInput = new CardCoordinateInput()
            {
                AssetId = parsedAssetId,
                CardDate = cardDate
            };

            var requestWithCorrelationId = new WithCorrelationId<CardCoordinateInput>
                (correlationId, cardCoordinateInput);

            var data = _service.GetCardCoordinateResults(requestWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, data))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RLCoordinatesController)} {nameof(GetCardCoordinate)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var response = CardCoordinateDataMapper.Map(correlationId, data);
            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(RLCoordinatesController)} {nameof(GetCardCoordinate)}", correlationId);

            return Ok(response);
        }

        #endregion

    }
}
