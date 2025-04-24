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
    [Route("Analytics/[controller]")]
    [Authorize]
    public class GLAnalysisController : BaseController
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
        /// Constructs a new instance of the <seealso cref="GLAnalysisController"/>.
        /// </summary>
        /// <param name="glAnalysisProcessingService">The <seealso cref="IGLAnalysisProcessingService"/> to call 
        /// for handling requests.</param>
        /// <param name="loggerFactory">The <seealso cref="IThetaLoggerFactory"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// or
        /// <paramref name="glAnalysisProcessingService"/> is null.
        /// </exception>
        /// <paramref name="loggerFactory"/> is null.
        public GLAnalysisController(IThetaLoggerFactory loggerFactory, IGLAnalysisProcessingService glAnalysisProcessingService) : base(loggerFactory)
        {
            _service = glAnalysisProcessingService ?? throw new ArgumentNullException(nameof(glAnalysisProcessingService));
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Handles requests for gas lift analysis.
        /// </summary>
        /// <param name="filters">The filters for Get GLAnalysis</param>
        /// <returns>The IAction Result. If success result the response json
        /// if failed, retures the code.
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet(Name = "GetGLAnalysis")]
        public IActionResult Get([FromQuery] IDictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(GLAnalysisController)} {nameof(Get)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters,
                    QueryParams.AssetId, QueryParams.TestDate, QueryParams.AnalysisTypeId))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GLAnalysisController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateAssetId(correlationId, out defaultInvalidStatusCodeResult,
                    out var parsedAssetId,
                    filters?[QueryParams.AssetId]))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GLAnalysisController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var glAnalysisInput = new GLAnalysisInput()
            {
                AssetId = parsedAssetId,
                TestDate = filters[QueryParams.TestDate],
                AnalysisTypeId = int.Parse(filters[QueryParams.AnalysisTypeId]),
                AnalysisResultId = filters.TryGetValue(QueryParams.AnalysisResultId, out var analysisResultIdString)
                    ? int.TryParse(analysisResultIdString, out var analysisResultId) ? analysisResultId : null
                    : null,
            };

            var requestWithCorrelationId = new WithCorrelationId<GLAnalysisInput>
                (correlationId, glAnalysisInput);

            var serviceResult = _service.GetGLAnalysisResults(requestWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GLAnalysisController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var response = GLAnalysisDataMapper.Map(correlationId, serviceResult);
            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GLAnalysisController)} {nameof(Get)}", correlationId);

            return Ok(response);
        }

        #endregion

    }
}
