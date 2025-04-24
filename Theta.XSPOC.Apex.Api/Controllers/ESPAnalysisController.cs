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
    /// The controller that fields requests for ESP Analysis.
    /// </summary>
    [ApiController]
    [Route("Analytics/[controller]")]
    [Authorize]
    public class ESPAnalysisController : BaseController
    {

        #region BaseController Members

        /// <summary>
        /// Gets or sets the controller logger.
        /// </summary>
        protected override IThetaLogger ControllerLogger => LoggerFactory.Create(LoggingModel.ESPAnalysis);

        #endregion

        #region Private Fields

        private readonly IESPAnalysisProcessingService _service;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the <seealso cref="ESPAnalysisController"/>.
        /// </summary>
        /// <param name="espAnalysisProcessingService">The <seealso cref="IESPAnalysisProcessingService"/> to call 
        /// for handling requests.</param>
        /// <param name="loggerFactory">
        /// The <seealso cref="IThetaLoggerFactory"/> service.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="espAnalysisProcessingService"/> is null.
        /// or
        /// <paramref name="loggerFactory"/>is null.
        /// </exception>
        public ESPAnalysisController(IESPAnalysisProcessingService espAnalysisProcessingService, IThetaLoggerFactory loggerFactory) : base(loggerFactory)
        {
            _service = espAnalysisProcessingService ?? throw new ArgumentNullException(nameof(espAnalysisProcessingService));
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Handles requests for esp analysis.
        /// </summary>
        /// <param name="filters">The filters for esp lift analysis</param>
        /// <returns>The IAction Result. If success result the response json
        /// if failed, retures the code.
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet(Name = "GetESPAnalysis")]
        public IActionResult Get([FromQuery] IDictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(ESPAnalysisController)} {nameof(Get)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters,
                    QueryParams.AssetId, QueryParams.TestDate))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(ESPAnalysisController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateAssetId(correlationId, out defaultInvalidStatusCodeResult,
                    out var parsedAssetId,
                    filters?[QueryParams.AssetId]))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(ESPAnalysisController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var testDate = filters[QueryParams.TestDate];

            var espAnalysisInput = new ESPAnalysisInput()
            {
                AssetId = parsedAssetId,
                TestDate = testDate
            };

            var requestWithCorrelationId = new WithCorrelationId<ESPAnalysisInput>
                (correlationId, espAnalysisInput);

            var serviceResult = _service.GetESPAnalysisResults(requestWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(ESPAnalysisController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var response = ESPAnalysisDataMapper.Map(correlationId, serviceResult);
            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(ESPAnalysisController)} {nameof(Get)}", correlationId);

            return Ok(response);
        }

        #endregion

    }
}
