using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
    /// The controller that fields requests for GroupStatusViews.
    /// </summary>
    [Route("Lookup/[controller]")]
    [ApiController]
    [Authorize]
    public class GroupStatusViewsController : BaseController
    {

        #region BaseController Members

        /// <summary>
        /// Gets or sets the controller logger.
        /// </summary>
        protected override IThetaLogger ControllerLogger => LoggerFactory.Create(LoggingModel.GroupStatus);

        #endregion

        #region Private Fields

        private readonly IGroupStatusProcessingService _service;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the <seealso cref="GroupStatusViewsController"/>.
        /// </summary>
        /// <param name="groupStatusProcessingService">The <seealso cref="IGroupStatusProcessingService"/> to call 
        /// for handling requests.</param>
        /// <param name="loggerFactory">The <seealso cref="IThetaLoggerFactory"/>.</param>
        /// <exception cref="ArgumentNullException">
        ///  <paramref name="loggerFactory"/> is null.
        /// or
        /// <paramref name="groupStatusProcessingService"/> is null.
        /// </exception>
        public GroupStatusViewsController(
            IGroupStatusProcessingService groupStatusProcessingService, IThetaLoggerFactory loggerFactory) : base(loggerFactory)
        {
            _service = groupStatusProcessingService ?? throw new ArgumentNullException(nameof(groupStatusProcessingService));
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Handles requests for group status views.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet(Name = "GetGroupStatusViews")]
        public IActionResult Get()
        {
            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusViewsController)} {nameof(Get)}", correlationId);

            if (ValidateUserAuthorized(correlationId, out var defaultInvalidStatusCodeResult, out var user) == false)
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusViewsController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var availableViewInput = new AvailableViewInput()
            {
                UserId = user
            };

            var requestWithCorrelationId = new WithCorrelationId<AvailableViewInput>(
                correlationId, availableViewInput);

            var serviceResult = _service.GetAvailableViews(requestWithCorrelationId);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusViewsController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var response = AvailableViewDataMapper.Map(requestWithCorrelationId.CorrelationId, serviceResult);
            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusViewsController)} {nameof(Get)}", correlationId);

            return Ok(response);
        }

        #endregion

    }
}
