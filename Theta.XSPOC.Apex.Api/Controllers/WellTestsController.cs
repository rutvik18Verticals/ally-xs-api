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
    /// The controller that fields requests for WellTestsController.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WellTestsController : BaseController
    {

        #region BaseController Members

        /// <summary>
        /// Gets or sets the controller logger.
        /// </summary>
        protected override IThetaLogger ControllerLogger => LoggerFactory.Create(LoggingModel.WellTest);

        #endregion

        #region Private Fields

        private readonly IWellTestsProcessingService _service;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the <seealso cref="WellTestsController"/>.
        /// </summary>
        /// <param name="wellTestProcessingService">The <seealso cref="IWellTestsProcessingService"/> to call 
        /// for handling requests.</param> 
        /// <param name="loggerFactory">The <seealso cref="IThetaLoggerFactory"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="loggerFactory"/> is null.
        /// </exception>
        public WellTestsController(IThetaLoggerFactory loggerFactory, IWellTestsProcessingService wellTestProcessingService) : base(loggerFactory)
        {
            _service = wellTestProcessingService ?? throw new ArgumentNullException(nameof(wellTestProcessingService));
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Handles requests for getting well tests data.
        /// </summary>
        /// <param name="filters">The filters contains the id, type and notificationTypeId in the dictionary.</param>
        /// <returns>The IAction Result. If success result the response json
        /// if failed, returns the code.
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet(Name = "GetWellTests")]
        public IActionResult Get([FromQuery] Dictionary<string, string> filters)
        {
            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(WellTestsController)} {nameof(Get)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters,
                    QueryParams.AssetId, QueryParams.Type))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(WellTestsController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateAssetId(correlationId, out defaultInvalidStatusCodeResult,
                    out var parsedAssetId,
                    filters?[QueryParams.AssetId]))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(WellTestsController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var type = filters[QueryParams.Type];

            if (type.ToLower() != "esp" && type.ToLower() != "gl")
            {
                var message = $"{nameof(type)}, Invalid Type.";
                ControllerLogger.WriteCId(Level.Info, message, correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(WellTestsController)} {nameof(Get)}", correlationId);

                return BadRequest();
            }

            var wellTestInput = new WellTestInput()
            {
                AssetId = parsedAssetId
            };

            var requestWithCorrelationId = new WithCorrelationId<WellTestInput>
                (correlationId, wellTestInput);

            if (type.ToLower() == "esp")
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(WellTestsController)} {nameof(Get)}", correlationId);
                return ProcessESPWellTestData(requestWithCorrelationId);
            }
            else
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(WellTestsController)} {nameof(Get)}", correlationId);
                return ProcessGLWellTestData(requestWithCorrelationId);
            }
        }

        #endregion

        #region Private Method

        private IActionResult ProcessESPWellTestData(WithCorrelationId<WellTestInput> requestWithCorrelationId)
        {
            var serviceResult = _service.GetESPAnalysisWellTestData(requestWithCorrelationId);
            if (!ValidateServiceResult(requestWithCorrelationId.CorrelationId, out var defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(WellTestsController)} {nameof(ProcessESPWellTestData)}",
                    requestWithCorrelationId?.CorrelationId);

                return defaultInvalidStatusCodeResult;
            }

            var response = WellTestValues.Map(requestWithCorrelationId.CorrelationId, serviceResult);

            return Ok(response);
        }

        private IActionResult ProcessGLWellTestData(WithCorrelationId<WellTestInput> requestWithCorrelationId)
        {
            var serviceResult = _service.GetGLAnalysisWellTestData(requestWithCorrelationId);
            if (!ValidateServiceResult(requestWithCorrelationId.CorrelationId, out var defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(WellTestsController)} {nameof(ProcessGLWellTestData)}",
                    requestWithCorrelationId?.CorrelationId);

                return defaultInvalidStatusCodeResult;
            }

            var response = WellTestValues.MapGLAnalysis(requestWithCorrelationId.CorrelationId, serviceResult);

            return Ok(response);
        }

        #endregion

    }
}
