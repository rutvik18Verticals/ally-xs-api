using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Services.DashboardService;
using Theta.XSPOC.Apex.Api.Core.Services.UserAccountService;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Controllers
{
    /// <summary>
    /// Controller for managing dashboards.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class DashboardController : BaseController
    {

        #region BaseController Members

        /// <summary>
        /// Gets or sets the controller logger.
        /// </summary>
        protected override IThetaLogger ControllerLogger => LoggerFactory.Create(LoggingModel.TrendData);

        #endregion

        #region Private Fields

        private readonly IDashboardWidgetService _service;
        private readonly ILoggedInUserProvider _loggedInUserProvider;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the <seealso cref="DashboardController"/>.
        /// </summary>
        /// <param name="loggerFactory">The <seealso cref="IThetaLoggerFactory"/>.</param>
        /// <param name="widgetService">The <seealso cref="IDashboardWidgetService"/> to call 
        /// for handling requests.</param>
        /// <param name="loggedinUserService">The <seealso cref="ILoggedInUserProvider"/></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="loggerFactory"/> is null.
        /// or
        /// <paramref name="widgetService"/> is null.
        /// or
        /// <paramref name="loggedinUserService"/> is null.
        /// or
        /// <paramref name="loggerFactory"/>is null.        
        /// </exception>
        public DashboardController(IThetaLoggerFactory loggerFactory,
            IDashboardWidgetService widgetService, ILoggedInUserProvider loggedinUserService) : base(loggerFactory)
        {
            _service = widgetService ?? throw new ArgumentNullException(nameof(widgetService));
            _loggedInUserProvider = loggedinUserService ?? throw new ArgumentNullException(nameof(loggedinUserService));
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Handles requests for fetching the esp well charts widgets.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet]
        [Authorize]
        [Route("/widgets/{dashboardType}")]
        public IActionResult Get(string dashboardType, bool isStacked = false)
        {
            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(DashboardController)} {nameof(Get)}", correlationId);

            if (!ValidateUserAuthorized(correlationId, out var defaultInvalidStatusCodeResult, out var user))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DashboardController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }
            var userId = _loggedInUserProvider.GetUserDetails().UserObjectId;

            var serviceResult = _service.GetESPWellChartWidgets(dashboardType, userId, correlationId, isStacked);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DashboardController)} {nameof(Get)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            return Ok(serviceResult);
        }

        /// <summary>
        /// Handles the requests to save user preferences for widgets
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpPost]
        [Authorize]
        [Route("/widgets/saveuserpreferences")]
        public IActionResult SaveUserPreferences(DashboardWidgetUserPreferencesInput input)
        {
            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(DashboardController)} {nameof(SaveUserPreferences)}", correlationId);

            if (!ValidateUserAuthorized(correlationId, out var defaultInvalidStatusCodeResult, out var user))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DashboardController)} {nameof(SaveUserPreferences)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }
            var userId = _loggedInUserProvider.GetUserDetails().UserObjectId;

            var serviceResult = _service.SaveWidgetUserPreferences(input, userId, correlationId);

            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DashboardController)} {nameof(SaveUserPreferences)}", correlationId);

            if (!serviceResult.Result)
            {
                return defaultInvalidStatusCodeResult;
            }

            return Ok();
        }

        /// <summary>
        /// Handles the requests to save user preferences for widgets
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpPost]
        [Authorize]
        [Route("/widgets/ResetUserPreferences")]
        public IActionResult ResetUserPreferences(DashboardWidgetResetUserPreferencesInput input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input), "Input cannot be null.");
            }

            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(DashboardController)} {nameof(ResetUserPreferences)}", correlationId);

            if (!ValidateUserAuthorized(correlationId, out var defaultInvalidStatusCodeResult, out var user))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DashboardController)} {nameof(ResetUserPreferences)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }
            var userId = _loggedInUserProvider.GetUserDetails().UserObjectId;

            var serviceResult = _service.ResetWidgetUserPreferences(input, userId, correlationId);

            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DashboardController)} {nameof(ResetUserPreferences)}", correlationId);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, serviceResult.Result))
            {
                return defaultInvalidStatusCodeResult;
            }

            return Ok(serviceResult.Result);
        }

        /// <summary>
        /// Handles requests for fetching the esp well charts widgets.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet]
        [Authorize]
        [Route("/widgets/tags/{liftType}")]
        public async Task<IActionResult> GetTags(string liftType)
        {
            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(DashboardController)} {nameof(GetTags)}", correlationId);

            if (!ValidateUserAuthorized(correlationId, out var defaultInvalidStatusCodeResult, out var user))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DashboardController)} {nameof(GetTags)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (string.IsNullOrWhiteSpace(liftType))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DashboardController)} {nameof(GetTags)}", correlationId);

                throw new ArgumentNullException(nameof(liftType), "Input cannot be null.");
            }

            var serviceResult = await _service.GetAllDefaultParameters(liftType, correlationId);

            if (!ValidateServiceResult(correlationId, out defaultInvalidStatusCodeResult, serviceResult))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(DashboardController)} {nameof(GetTags)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            return Ok(serviceResult);
        }
        #endregion

    }
}
