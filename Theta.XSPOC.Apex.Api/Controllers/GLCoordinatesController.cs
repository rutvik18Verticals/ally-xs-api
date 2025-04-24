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
    /// The controller that fields requests for GL Analysis Curve Coordinate.
    /// </summary>
    [Route("Analytics/GLAnalysis/[controller]")]
    [ApiController]
    [Authorize]
    public class GLCoordinatesController : BaseController
    {

        #region BaseController Members

        /// <summary>
        /// Gets or sets the controller logger.
        /// </summary>
        protected override IThetaLogger ControllerLogger => LoggerFactory.Create(LoggingModel.GLAnalysis);

        #endregion

        #region Private Fields

        private readonly IGLAnalysisProcessingService _analysisGetCurveCoordinateService;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the <seealso cref="GLCoordinatesController"/>.
        /// </summary>
        /// <param name="analysisGetCurveCoordinateService">
        /// The <seealso cref="IGLAnalysisProcessingService"/> to call.</param>
        /// <param name="loggerFactory">The <seealso cref="IThetaLoggerFactory"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="analysisGetCurveCoordinateService"/> is null.
        /// </exception>
        public GLCoordinatesController(IThetaLoggerFactory loggerFactory,
            IGLAnalysisProcessingService analysisGetCurveCoordinateService) : base(loggerFactory)
        {
            _analysisGetCurveCoordinateService = analysisGetCurveCoordinateService;

        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Handles requests for GetCurveCoordinate.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet(Name = "GetGLCurveCoordinate")]
        public IActionResult Get([FromQuery] IDictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(GLCoordinatesController)} {nameof(Get)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters,
                    QueryParams.AssetId, QueryParams.TestDate, QueryParams.AnalysisTypeId, QueryParams.AnalysisResultId))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GLCoordinatesController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateAssetId(correlationId, out defaultInvalidStatusCodeResult,
                    out var parsedAssetId,
                    filters?[QueryParams.AssetId]))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GLCoordinatesController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var gLAnalysisCurveCoordinateInput = new GLAnalysisCurveCoordinateInput()
            {
                AssetId = parsedAssetId,
                TestDate = filters["testDate"],
                AnalysisResultId = int.Parse(filters["analysisResultId"]),
                AnalysisTypeId = int.Parse(filters["analysisTypeId"])
            };

            var requestWithCorrelationId = new WithCorrelationId<GLAnalysisCurveCoordinateInput>
                (correlationId, gLAnalysisCurveCoordinateInput);

            var serviceResult = _analysisGetCurveCoordinateService.GetGLAnalysisCurveCoordinateResults(requestWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GLCoordinatesController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var response = GLAnalysisCurveCoordinateDataMapper.Map(correlationId, serviceResult);
            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GLCoordinatesController)} {nameof(Get)}", correlationId);

            return Ok(response);
        }

        #endregion

    }
}
