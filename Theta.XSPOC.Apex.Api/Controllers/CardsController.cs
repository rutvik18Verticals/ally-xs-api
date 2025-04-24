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
    /// The controller that fields requests for card dates.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CardsController : BaseController
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
        /// Constructs a new instance of the <seealso cref="CardsController"/>.
        /// </summary>
        /// <param name="rodLiftAnalysisProcessingService">The <seealso cref="IRodLiftAnalysisProcessingService"/> to call 
        /// for handling requests.</param>
        /// <param name="loggerFactory">
        /// The <seealso cref="IThetaLoggerFactory"/> service.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="rodLiftAnalysisProcessingService"/> is null.
        /// or
        /// <paramref name="loggerFactory"/>is null.
        /// </exception>
        public CardsController(IRodLiftAnalysisProcessingService rodLiftAnalysisProcessingService, IThetaLoggerFactory loggerFactory) : base(loggerFactory)
        {
            _service = rodLiftAnalysisProcessingService ??
                throw new ArgumentNullException(nameof(rodLiftAnalysisProcessingService));
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Handles requests for retrieving card dates.
        /// </summary>
        /// <param name="filters">The filters for card dates</param>
        /// <returns>The CardDate json.</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(Name = "GetCardDates")]
        public IActionResult GetCardDates([FromQuery] IDictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(CardsController)} {nameof(GetCardDates)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters,
                    QueryParams.AssetId))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(CardsController)} {nameof(GetCardDates)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateAssetId(correlationId, out defaultInvalidStatusCodeResult,
                out var parsedAssetId,
                filters?[QueryParams.AssetId]))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(CardsController)} {nameof(GetCardDates)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var cardDateInput = new CardDateInput()
            {
                AssetId = parsedAssetId
            };

            var inputWithCorrelationId = new WithCorrelationId<CardDateInput>(
                    correlationId, cardDateInput);

            var serviceResult = _service.GetCardDate(inputWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(CardsController)} {nameof(GetCardDates)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var response = CardDateMapper.Map(inputWithCorrelationId.CorrelationId, serviceResult);
            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(CardsController)} {nameof(GetCardDates)}", correlationId);

            return Ok(response);
        }

        #endregion

    }
}
