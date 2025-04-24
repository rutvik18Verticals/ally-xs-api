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
    /// The controller that fields requests for Notification.
    /// The controller that fields requests for GL Analysis.
    /// </summary>
    [ApiController]
    [Route("Surveys/[controller]")]
    [Authorize]
    public class SurveyCoordinatesController : BaseController
    {

        #region BaseController Members

        /// <summary>
        /// Gets or sets the controller logger.
        /// </summary>
        protected override IThetaLogger ControllerLogger => LoggerFactory.Create(LoggingModel.GLAnalysis);

        #endregion

        #region Private Fields

        private readonly IGLAnalysisProcessingService _service;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the <seealso cref="SurveyCoordinatesController"/>.
        /// </summary>
        /// <param name="glAnalysisProcessingService">The <seealso cref="IGLAnalysisProcessingService"/> to call 
        /// for handling requests.</param>
        /// <param name="loggerFactory">The <seealso cref="IThetaLoggerFactory"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// or
        /// <paramref name="glAnalysisProcessingService"/> is null.
        /// </exception>
        /// <paramref name="loggerFactory"/> is null.
        public SurveyCoordinatesController(IThetaLoggerFactory loggerFactory, IGLAnalysisProcessingService glAnalysisProcessingService) : base(loggerFactory)
        {
            _service = glAnalysisProcessingService ?? throw new ArgumentNullException(nameof(glAnalysisProcessingService));
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Handles requests for gLAnalysisSurveyCurveCoordinate.
        /// </summary>
        /// <param name="filters">The filters for Get GLAnalysis</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet(Name = "GetGLSurveyCurveCoordinate")]
        public IActionResult Get([FromQuery] IDictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(SurveyCoordinatesController)} {nameof(Get)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters,
                    QueryParams.AssetId, QueryParams.SurveyDate))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SurveyCoordinatesController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }
            var surveyDate = filters?[QueryParams.SurveyDate];

            if (!ValidateAssetId(correlationId, out defaultInvalidStatusCodeResult,
                    out var parsedAssetId,
                    filters?[QueryParams.AssetId]))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SurveyCoordinatesController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var input = new GLAnalysisCurveCoordinateInput()
            {
                Guid = parsedAssetId,
                SurveyDate = surveyDate
            };

            var requestWithCorrelationId =
                new WithCorrelationId<GLAnalysisCurveCoordinateInput>(correlationId, input);

            var serviceResult = _service.GetGLAnalysisSurveyCurveCoordinate(requestWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SurveyCoordinatesController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }
            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SurveyCoordinatesController)} {nameof(Get)}", correlationId);

            return Ok(GLAnalysisSurveryCurveCoordinatesDataMapper.Map(requestWithCorrelationId.CorrelationId, serviceResult));
        }

        #endregion

    }
}
