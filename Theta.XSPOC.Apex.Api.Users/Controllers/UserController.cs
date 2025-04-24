using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Users.Controllers
{
    /// <summary>
    /// The controller that fields requests for user data.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UserController : BaseController
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
        /// Constructs a new instance of the <seealso cref="UserController"/>.
        /// </summary>
        /// <param name="userService">The <seealso cref="IUserService"/> to call 
        /// for handling requests.</param>
        /// <param name="loggerFactory">The <see cref="IThetaLoggerFactory"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="userService"/> is null.
        /// </exception>
        public UserController(IUserService userService, IThetaLoggerFactory loggerFactory) : base(loggerFactory)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Gets the enabled status for a well.
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpPost("SetFirstTimeLogin")]
        public IActionResult SetFirstTimeLogin()
        {
            GetOrCreateCorrelationId(out var correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Starting {nameof(UserController)} {nameof(SetFirstTimeLogin)}", correlationId);

            var user = User?.Claims?
                .FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?
                .Value?.Split('@');

            if (user == null)
            {
                ControllerLogger.WriteCId(Level.Info, "Invalid user", correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(UserController)} {nameof(SetFirstTimeLogin)}", correlationId);

                return Unauthorized();
            }

            if (user.Length == 0)
            {
                ControllerLogger.WriteCId(Level.Info, "Invalid user", correlationId);
                ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(UserController)} {nameof(SetFirstTimeLogin)}", correlationId);

                return Unauthorized();
            }

            var userName = user[0];

            var response = _userService.SetUserLoggedIn(userName, correlationId);

            ControllerLogger.WriteCId(Level.Info, $"Set first time login for {userName} {response}", correlationId);
            ControllerLogger.WriteCId(Level.Trace, $"Finished {nameof(UserController)} {nameof(SetFirstTimeLogin)}", correlationId);

            return Ok(response);
        }

        #endregion

    }
}
