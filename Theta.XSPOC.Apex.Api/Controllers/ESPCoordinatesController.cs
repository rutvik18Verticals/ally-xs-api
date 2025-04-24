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
    /// The controller that fields requests for curve coordinate.
    /// </summary>
    [Route("Analytics/ESPAnalysis/[controller]")]
    [ApiController]
    [Authorize]
    public class ESPCoordinatesController : BaseController
    {

        #region BaseController Members

        /// <summary>
        /// Gets or sets the controller logger.
        /// </summary>
        protected override IThetaLogger ControllerLogger => LoggerFactory.Create(LoggingModel.ESPAnalysis);

        #endregion

        #region Private Fields

        private readonly IESPAnalysisProcessingService _resultService;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the <seealso cref="ESPCoordinatesController"/>.
        /// </summary>
        /// <param name="resultService">The <seealso cref="IESPAnalysisProcessingService"/> to call.</param>
        /// <param name="loggerFactory">
        /// The <seealso cref="IThetaLoggerFactory"/> service.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="resultService"/> is null.
        /// </exception>
        public ESPCoordinatesController(IESPAnalysisProcessingService resultService, IThetaLoggerFactory loggerFactory) : base(loggerFactory)
        {
            _resultService = resultService ?? throw new ArgumentNullException(nameof(resultService));
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Handles requests for GetCurveCoordinate.
        /// </summary>
        /// <param name="filters">The filters for Get Curve Coordinate</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet(Name = "GetCurveCoordinate")]
        public IActionResult Get([FromQuery] IDictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(ESPCoordinatesController)} {nameof(Get)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters,
                    QueryParams.AssetId, QueryParams.TestDate))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(ESPCoordinatesController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateAssetId(correlationId, out defaultInvalidStatusCodeResult,
                    out var parsedAssetId,
                    filters?[QueryParams.AssetId]))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(ESPCoordinatesController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var curveCoordinatesInput = new CurveCoordinatesInput()
            {
                AssetId = parsedAssetId,
                TestDate = filters["testDate"].ToString(),
            };

            var requestWithCorrelationId = new WithCorrelationId<CurveCoordinatesInput>
                (correlationId, curveCoordinatesInput);
            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(ESPCoordinatesController)} {nameof(Get)}", correlationId);

            return ProcessCurveCoordinate(requestWithCorrelationId);
        }

        #endregion

        #region Private Method

        private IActionResult ProcessCurveCoordinate(WithCorrelationId<CurveCoordinatesInput> requestWithCorrelationId)
        {
            var serviceResult = _resultService.GetCurveCoordinate(requestWithCorrelationId);

            if (!ValidateServiceResult(requestWithCorrelationId.CorrelationId, out var defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(ESPCoordinatesController)} {nameof(ProcessCurveCoordinate)}",
                    requestWithCorrelationId?.CorrelationId);

                return defaultInvalidStatusCodeResult;
            }

            var response = CurveCoordinateMapper.Map(requestWithCorrelationId.CorrelationId, serviceResult.Values);

            return Ok(response);
        }

        #endregion

    }
}

