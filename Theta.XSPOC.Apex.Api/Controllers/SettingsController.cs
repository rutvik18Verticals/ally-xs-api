using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Contracts.Mappers;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Controllers
{
    /// <summary>
    /// The controller that fields requests application settings
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class SettingsController : BaseController
    {

        #region BaseController Members

        /// <summary>
        /// Gets or sets the controller logger.
        /// </summary>
        protected override IThetaLogger ControllerLogger => LoggerFactory.Create(LoggingModel.APIService);
        private readonly IAppSettingsService _appsettingsService;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the <seealso cref="SettingsController"/>.
        /// </summary>
        /// <param name="appsettingsService">The <seealso cref="IAppSettingsService"/> to call 
        /// for handling requests.</param>
        /// <param name="loggerFactory">
        /// The <seealso cref="IThetaLoggerFactory"/> service.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="loggerFactory"/>is null.
        /// </exception>
        public SettingsController(IAppSettingsService appsettingsService, IThetaLoggerFactory loggerFactory) : base(loggerFactory)
        {
            _appsettingsService = appsettingsService ?? throw new ArgumentNullException(nameof(appsettingsService));
        }

        #endregion

        #region Endpoints
               
        /// <summary>
        /// Gets the appsettings for ApplicationDeploymentMode and PumpChecker flag.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet("GetApplicationSettings")]
        public IActionResult Get()
        {
            GetOrCreateCorrelationId(out var correlationId);

            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(SettingsController)} {nameof(Get)}",
                correlationId);
            
            var appSettings = _appsettingsService.GetApplicationSettings();

            if (!ValidateServiceResult(correlationId, out var invalidStatusCode, appSettings))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SettingsController)} {nameof(Get)}",
                correlationId);

                return invalidStatusCode;
            }

            var response = AppSettingsMapper.Map(appSettings, correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SettingsController)} {nameof(Get)}",
                correlationId);

            return Ok(response);
        }

        /// <summary>
        /// Gets the redirect urls for onboarding app and ally connect web ui from app settings.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet("GetAppURLs")]
        public IActionResult GetAppURLs()
        {
            GetOrCreateCorrelationId(out var correlationId);

            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(SettingsController)} {nameof(GetAppURLs)}",
                correlationId);
            
            var appSettings = _appsettingsService.GetAppURLs();

            if (!ValidateServiceResult(correlationId, out var invalidStatusCode, appSettings))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SettingsController)} {nameof(GetAppURLs)}",
                correlationId);

                return invalidStatusCode;
            }

            var response = AppSettingsMapper.Map(appSettings, correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SettingsController)} {nameof(GetAppURLs)}",
                correlationId);

            return Ok(response);
        }

        /// <summary>
        /// Gets the login url from app settings.
        /// </summary>
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpGet("GetLoginURL")]
        public IActionResult GetLoginURL()
        {
            GetOrCreateCorrelationId(out var correlationId);

            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(SettingsController)} {nameof(GetLoginURL)}",
                correlationId);
            
            var appSettings = _appsettingsService.GetDeploymentSettings();

            if (!ValidateServiceResult(correlationId, out var invalidStatusCode, appSettings))
            {
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SettingsController)} {nameof(GetLoginURL)}",
                correlationId);

                return invalidStatusCode;
            }

            var response = AppSettingsMapper.Map(appSettings, correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(SettingsController)} {nameof(GetLoginURL)}",
                correlationId);

            return Ok(response);
        }

        #endregion

    }
}
