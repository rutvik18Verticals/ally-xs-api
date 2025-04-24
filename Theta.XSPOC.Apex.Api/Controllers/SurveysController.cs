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
    /// The controller that fields requests for GL Analysis.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class SurveysController : BaseController
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
        /// Constructs a new instance of the <seealso cref="SurveysController"/>.
        /// </summary>
        /// <param name="glAnalysisProcessingService">The <seealso cref="IGLAnalysisProcessingService"/> to call 
        /// for handling requests.</param>
        /// <param name="loggerFactory">The <seealso cref="IThetaLoggerFactory"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="loggerFactory"/> is null.
        /// or
        /// <paramref name="glAnalysisProcessingService"/> is null.
        /// </exception>

        public SurveysController(IThetaLoggerFactory loggerFactory, IGLAnalysisProcessingService glAnalysisProcessingService) : base(loggerFactory)
        {
            _service = glAnalysisProcessingService ?? throw new ArgumentNullException(nameof(glAnalysisProcessingService));
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Handles requests for gLAnalysis.
        /// </summary>
        /// <param name="filters">The filters for nodeId</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet(Name = "GetGLAnalysisSurveyDate")]
        public IActionResult Get([FromQuery] IDictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(SurveysController)} {nameof(Get)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters,
                   QueryParams.AssetId))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SurveysController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateAssetId(correlationId, out defaultInvalidStatusCodeResult,
                    out var parsedAssetId,
                    filters?[QueryParams.AssetId]))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SurveysController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }
            var glAnalysisInput = new GLSurveyAnalysisInput()
            {
                Guid = parsedAssetId
            };

            var requestWithCorrelationId =
                new WithCorrelationId<GLSurveyAnalysisInput>(correlationId, glAnalysisInput);

            var serviceResult = _service.GetGLAnalysisSurveyDate(requestWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SurveysController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var response = GLAnalysisSurveyDateDataMapper.Map(requestWithCorrelationId.CorrelationId, serviceResult);
            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SurveysController)} {nameof(Get)}", correlationId);

            return Ok(response);
        }

        #endregion

    }
}
