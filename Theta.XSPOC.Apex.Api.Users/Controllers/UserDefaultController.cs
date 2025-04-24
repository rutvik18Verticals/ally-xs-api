using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.Users.Contracts.Requests;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Users.Controllers
{
    /// <summary>
    /// The controller that fields requests for user default data.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class UserDefaultController : BaseController
    {

        #region BaseController Members

        /// <summary>
        /// Gets or sets the controller logger.
        /// </summary>
        protected override IThetaLogger ControllerLogger => LoggerFactory.Create(LoggingModel.UserDefault);

        #endregion

        #region Private Fields

        private readonly IUserService _userService;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the <seealso cref="UserDefaultController"/>.
        /// </summary>
        /// <param name="userService">The <seealso cref="IUserService"/> to call 
        /// for handling requests.</param>
        /// <param name="loggerFactory">The <see cref="IThetaLoggerFactory"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="userService"/> is null.
        /// </exception>
        public UserDefaultController(IUserService userService, IThetaLoggerFactory loggerFactory) : base(loggerFactory)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Sets the user default represented by the provided data.
        /// </summary>
        /// <param name="userDefault">The user default data to set the value for.</param>
        /// <returns>True if the value was set, false otherwise.</returns>
        [HttpPost("UserPreference")]
        public IActionResult UserPreference([FromBody] UserDefaultRequest userDefault)
        {
            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(UserDefaultController)} {nameof(UserPreference)}", correlationId);

            if (userDefault == null)
            {
                ControllerLogger.WriteCId(Level.Info, "Invalid Defaults", correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultController)} {nameof(UserPreference)}", correlationId);

                return BadRequest("Invalid Defaults");
            }

            if (string.IsNullOrEmpty(userDefault.Property))
            {
                ControllerLogger.WriteCId(Level.Info, "Property is required", correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultController)} {nameof(UserPreference)}", correlationId);

                return BadRequest("Property is required");
            }

            if (string.IsNullOrEmpty(userDefault.Group))
            {
                ControllerLogger.WriteCId(Level.Info, "Group is required", correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultController)} {nameof(UserPreference)}", correlationId);

                return BadRequest("Group is required");
            }

            if (string.IsNullOrEmpty(userDefault.Value))
            {
                ControllerLogger.WriteCId(Level.Info, "Group is required", correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultController)} {nameof(UserPreference)}", correlationId);

                return BadRequest("Group is required");
            }

            if (!ValidateUserAuthorized(correlationId, out var defaultInvalidStatusCodeResult, out var user))
            {
                ControllerLogger.WriteCId(Level.Info, "Invalid user", correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultController)} {nameof(UserPreference)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            try
            {
                var response = _userService.SaveUserDefault(user, userDefault.Property, userDefault.Group, userDefault.Value, correlationId);

                ControllerLogger.WriteCId(Level.Info, $"Saved user defaults for {user} {response}", correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultController)} {nameof(UserPreference)}", correlationId);

                return Ok(response);
            }
            catch (Exception)
            {
                ControllerLogger.WriteCId(Level.Error, "Invalid Request", correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultController)} {nameof(UserPreference)}", correlationId);

                return BadRequest("Invalid Request");
            }
        }

        /// <summary>
        /// Get the request for user default.
        /// </summary>
        /// <param name="filters">The filters to the user default.</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("UserPreference")]
        public IActionResult UserPreference([FromQuery] IDictionary<string, string> filters)
        {
            var logger = LoggerFactory.Create(LoggingModel.UserDefault);

            GetOrCreateCorrelationId(out var correlationId);
            logger.WriteCId(Level.Trace, $"Starting {nameof(UserDefaultController)} {nameof(UserPreference)}", correlationId);

            if (!ValidateQueryParams(correlationId, out var defaultInvalidStatusCodeResult, filters,
                    QueryParams.GroupName))
            {
                var message = $"{nameof(defaultInvalidStatusCodeResult)} , Invalid Query Params";
                logger.WriteCId(Level.Info, message, correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultController)} {nameof(UserPreference)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            if (!ValidateUserAuthorized(correlationId, out defaultInvalidStatusCodeResult, out var user))
            {
                logger.WriteCId(Level.Info, "Unauthorized user", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultController)} {nameof(UserPreference)}", correlationId);

                return defaultInvalidStatusCodeResult;
            }

            var userDefaultInput = new UserDefaultInput
            {
                Property = filters?[QueryParams.Property],
                Group = filters?[QueryParams.GroupName]
            };

            var requestWithCorrelationId = new WithCorrelationId<UserDefaultInput>(
                correlationId, userDefaultInput);

            var serviceResult = _userService.GetUserDefault(requestWithCorrelationId, user);
            logger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultController)} {nameof(UserPreference)}", correlationId);

            return Ok(serviceResult);
        }

        #endregion

    }
}
