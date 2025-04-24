using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Contracts.JWTToken;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models;
using Theta.XSPOC.Apex.Api.Core.Models.Configuration;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.JWTToken;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models.Identity;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Controllers
{
    /// <summary>
    /// Handles Account login.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {

        #region BaseController Members

        /// <summary>
        /// Gets or sets the controller logger.
        /// </summary>
        protected override IThetaLogger ControllerLogger => LoggerFactory.Create(LoggingModel.GroupAndAsset);

        #endregion

        #region Private Fields

        private readonly IAdminToolsService _adminToolsService;
        private readonly IAuthService _authService;
        private readonly HttpClient _httpClient;
        private readonly AppSettings _appSettings;
        private readonly ITokenValidation _tokenValidation;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new instance of the <seealso cref="AccountController"/>.
        /// </summary>
        /// <param name="adminToolsService">The <seealso cref="IAdminToolsService"/> to call 
        /// for handling requests.</param>
        /// <param name="loggerFactory">
        /// The <seealso cref="IThetaLoggerFactory"/> service.</param>
        /// <param name="authService"></param>
        /// <param name="httpClient"></param>
        /// <param name="appSettings"></param>
        /// <param name="tokenValidation"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="adminToolsService"/> is null.
        ///  or
        /// <paramref name="loggerFactory"/>is null.
        /// </exception>
        public AccountController(IAdminToolsService adminToolsService, IThetaLoggerFactory loggerFactory, IAuthService authService,
            HttpClient httpClient, IOptionsSnapshot<AppSettings> appSettings, ITokenValidation tokenValidation) : base(loggerFactory)
        {
            _adminToolsService = adminToolsService ?? throw new ArgumentNullException(nameof(adminToolsService));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _tokenValidation = tokenValidation ?? throw new ArgumentNullException(nameof(tokenValidation));
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Method to handle and validate the token.
        /// </summary>
        /// <returns>True or False boolean value</returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("/User/ValidateToken")]
        public ActionResult<bool> ValidateToken()
        {
            var logger = LoggerFactory.Create(LoggingModel.Login);

            GetOrCreateCorrelationId(out var correlationId);

            logger.WriteCId(Level.Trace, $"Starting {nameof(AccountController)} {nameof(ValidateToken)}", correlationId);

            var identity = User.Identity;

            if (identity != null)
            {
                logger.WriteCId(Level.Info, "Authentication Succeeded", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AccountController)} {nameof(ValidateToken)}", correlationId);

                return identity.IsAuthenticated;
            }

            logger.WriteCId(Level.Info, "Authentication Failed", correlationId);
            logger.WriteCId(Level.Trace, $"Finished {nameof(AccountController)} {nameof(ValidateToken)}", correlationId);

            return false;
        }

        /// <summary>
        /// Method to generate the access token.
        /// </summary>
        /// <param name="parameters">The <seealso cref="Parameters"/> to call 
        /// for handling requests.</param>
        /// <returns>The JWTAccessToken json.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("/AzureADLogin")]
        public IActionResult AuthAsync([FromBody] Parameters parameters)
        {
            var logger = LoggerFactory.Create(LoggingModel.Login);

            GetOrCreateCorrelationId(out var correlationId);

            logger.WriteCId(Level.Trace, $"Starting {nameof(AccountController)} {nameof(AuthAsync)}", correlationId);

            var messageLoginAttempt = "Invalid login attempt";
            var messageInvalidReference = "Invalid reference";

            if (parameters == null)
            {
                logger.WriteCId(Level.Info, messageLoginAttempt, correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AccountController)} {nameof(AuthAsync)}", correlationId);

                return BadRequest(messageLoginAttempt);
            }

            if (parameters.GrantType == "sso_external")
            {
                if (string.IsNullOrEmpty(parameters.Reference))
                {
                    logger.WriteCId(Level.Info, messageInvalidReference, correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(AccountController)} {nameof(AuthAsync)}", correlationId);

                    return BadRequest(messageInvalidReference);
                }

                var reference = parameters.Reference;
                string email;

                try
                {
                    var emailandTime = Security.Decrypt(reference.Replace(" ", "+"));
                    var results = emailandTime.Split(",");
                    DateTime dateTime = new DateTime(Convert.ToInt64(results[1]));
                    email = results[0];
                    if (DateTime.UtcNow > dateTime)
                    {
                        var messageLinkExpired = "Link has expired";
                        logger.WriteCId(Level.Info, messageLinkExpired, correlationId);
                        logger.WriteCId(Level.Trace, $"Finished {nameof(AccountController)} {nameof(AuthAsync)}", correlationId);

                        return BadRequest(messageLinkExpired);
                    }
                }
                catch (Exception)
                {
                    logger.WriteCId(Level.Info, messageInvalidReference, correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(AccountController)} {nameof(AuthAsync)}", correlationId);

                    return BadRequest(messageInvalidReference);
                }

                var loginInput = new FormLoginInput()
                {
                    UserName = email,
                    Password = "",
                };

                var requestWithCorrelationId = new WithCorrelationId<FormLoginInput>(
                    correlationId, loginInput);

                var user = _adminToolsService.FindByName(requestWithCorrelationId);

                if (user == null)
                {
                    var messageInvalidUser = "Invalid User";
                    logger.WriteCId(Level.Info, messageInvalidUser, correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(AccountController)} {nameof(AuthAsync)}", correlationId);

                    return BadRequest(messageInvalidUser);
                }

                _adminToolsService.GetRolesAsync(correlationId);

                if (user.Email != null)
                {
                    var inputWithCorrelationId = new WithCorrelationId<string>(correlationId, user.Email);
                    var refresh_token = _adminToolsService.GetJwtRefresh(inputWithCorrelationId);
                    logger.WriteCId(Level.Info, "Refresh Token Obtained Successfully", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(AccountController)} {nameof(AuthAsync)}", correlationId);

                    var cookieOptions = GetCookieOptions();
                    var jwt = GetJwt(user, refresh_token, correlationId);
                    Response.Cookies.Append("Ally-Authorization", jwt.AccessToken, cookieOptions);

                    return Ok(jwt);
                }
            } // if (parameters.GrantType == "sso_external")

            logger.Write(Level.Info, messageLoginAttempt);
            logger.WriteCId(Level.Trace, $"Finished {nameof(AccountController)} {nameof(AuthAsync)}", correlationId);

            return BadRequest(messageLoginAttempt);
        }

        /// <summary>
        /// Method to generate the access token for form authentication.
        /// </summary>
        /// <param name="user">The <seealso cref="User"/> to call 
        /// for handling requests.</param>
        /// <returns>The JWTAccessToken json.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("/FormLogin")]
        public IActionResult Login([FromBody] User user)
        {
            var logger = LoggerFactory.Create(LoggingModel.Login);

            var messageInvalidUser = "Invalid User";

            GetOrCreateCorrelationId(out var correlationId);

            logger.WriteCId(Level.Trace, $"Starting {nameof(AccountController)} {nameof(Login)}", correlationId);

            if (user == null)
            {
                logger.WriteCId(Level.Info, messageInvalidUser, correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AccountController)} {nameof(Login)}", correlationId);

                return BadRequest(messageInvalidUser);
            }

            if (user.GrantType == "form")
            {
                if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.PasswordHash))
                {
                    var errorMessage = "Username or password is required.";
                    logger.WriteCId(Level.Info, errorMessage, correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(AccountController)} {nameof(Login)}", correlationId);

                    return BadRequest(errorMessage);
                }

                try
                {
                    var loginInput = new FormLoginInput()
                    {
                        UserName = user.UserName,
                        Password = user.PasswordHash,
                    };
                    var requestWithCorrelationId = new WithCorrelationId<FormLoginInput>(
                        correlationId, loginInput);

                    var userDetails = _adminToolsService.FindByName(requestWithCorrelationId);

                    if (userDetails != null)
                    {
                        var inputWithCorrelationId = new WithCorrelationId<string>(correlationId, user.UserName);
                        var refresh_token = _adminToolsService.GetJwtRefresh(inputWithCorrelationId);

                        logger.WriteCId(Level.Info, "Refresh Token Obtained Successfully", correlationId);
                        logger.WriteCId(Level.Trace, $"Finished {nameof(AccountController)} {nameof(Login)}", correlationId);

                        var cookieOptions = GetCookieOptions();
                        var jwt = GetJwt(userDetails, refresh_token, correlationId);
                        Response.Cookies.Append("Ally-Authorization", jwt.AccessToken, cookieOptions);

                        var response = new JWTAccessTokenCloud
                        {
                            AccessToken = jwt.AccessToken,
                            RefreshToken = jwt.RefreshToken,
                            ExpiresIn = jwt.ExpiresIn,
                            IsAdmin = jwt.IsAdmin,
                            IsFirstTimeLogin = jwt.IsFirstTimeLogin,
                            UserName = jwt.UserName,
                        };

                        return Ok(response);
                    }

                    logger.WriteCId(Level.Info, messageInvalidUser, correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(AccountController)} {nameof(Login)}", correlationId);

                    return BadRequest(messageInvalidUser);
                }
                catch (Exception)
                {
                    logger.WriteCId(Level.Info, messageInvalidUser, correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(AccountController)} {nameof(Login)}", correlationId);

                    return BadRequest(messageInvalidUser);
                }
            }

            logger.WriteCId(Level.Info, messageInvalidUser, correlationId);
            logger.WriteCId(Level.Trace, $"Finished {nameof(AccountController)} {nameof(Login)}", correlationId);

            return BadRequest(messageInvalidUser);
        }

        /// <summary>
        /// Method to generate the access token for from reference token.
        /// </summary>
        /// <param name="reference">The reference string to validate.</param>
        /// <returns>The JWTAccessToken json.</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("/LoginByRefToken")]
        public async Task<IActionResult> LoginByRefToken([FromBody] string reference)
        {
            var logger = LoggerFactory.Create(LoggingModel.Login);

            var messageInvalidUser = "Invalid User";

            GetOrCreateCorrelationId(out var correlationId);

            logger.WriteCId(Level.Trace, $"Starting {nameof(AccountController)} {nameof(reference)}", correlationId);

            if (string.IsNullOrWhiteSpace(reference))
            {
                logger.WriteCId(Level.Info, messageInvalidUser, correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AccountController)} {nameof(reference)}", correlationId);

                return BadRequest(messageInvalidUser);
            }

            var jwtToken = JsonConvert.DeserializeObject<JWTAccessToken>(await _tokenValidation.ValidateToken(Response, reference));

            if (string.IsNullOrEmpty(jwtToken?.AccessToken) == false)
            {
                logger.WriteCId(Level.Info, "Refresh Token Obtained Successfully", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AccountController)} {nameof(reference)}", correlationId);

                var userDetails = _authService.FindByNameAsync(jwtToken.UserName, string.Empty, correlationId);

                if (userDetails == null)
                {
                    logger.WriteCId(Level.Info, messageInvalidUser, correlationId);
                    logger.WriteCId(Level.Info, $"Finished {nameof(AccountController)} {nameof(reference)}", correlationId);

                    return StatusCode(403, messageInvalidUser);
                }

                var inputWithCorrelationId = new WithCorrelationId<string>(correlationId, jwtToken.UserName);
                var refreshToken = _adminToolsService.GetJwtRefresh(inputWithCorrelationId);

                var cookieOptions = GetCookieOptions();
                var jwt = GetJwt(userDetails, refreshToken, correlationId);

                Response.Cookies.Append("Ally-Authorization", jwt.AccessToken, cookieOptions);

                logger.WriteCId(Level.Info, "Refresh Token Obtained Successfully", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AccountController)} {nameof(reference)}", correlationId);

                return Ok(jwt);
            }

            logger.WriteCId(Level.Info, "Refresh Token Failed", correlationId);
            logger.WriteCId(Level.Trace, $"Finished {nameof(AccountController)} {nameof(reference)}", correlationId);

            return BadRequest(messageInvalidUser);
        }

        /// <summary>
        /// Method to handle and validate the token.
        /// </summary>
        /// <returns>True or False boolean value</returns>
        [HttpPost("/RefreshToken")]
        [Authorize]
        public IActionResult RefreshToken()
        {
            var logger = LoggerFactory.Create(LoggingModel.Login);

            GetOrCreateCorrelationId(out var correlationId);

            logger.WriteCId(Level.Trace, $"Starting {nameof(AccountController)} {nameof(RefreshToken)}", correlationId);

            var accessToken = Request.Cookies["Ally-Authorization"].ToString();

            if (string.IsNullOrEmpty(accessToken))
            {
                var messageRefreshTokenisRequired = "Access token is missing";
                logger.WriteCId(Level.Info, messageRefreshTokenisRequired, correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AccountController)} {nameof(RefreshToken)}",
                    correlationId);

                return new BadRequestObjectResult(messageRefreshTokenisRequired);
            }
            else
            {
                var principal = _adminToolsService.ValidateToken(accessToken, correlationId);

                if (principal == null)
                {
                    return Unauthorized(new { message = "Invalid access token" });
                }

                var identity = (ClaimsIdentity)principal.Identity;
                if (identity == null)
                {
                    logger.WriteCId(Level.Debug, $"Invalid Access Token", correlationId);
                    return BadRequest("Invalid Access Token");
                }

                var userName = identity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var inputWithCorrelationId = new WithCorrelationId<string>(correlationId, userName);
                var refresh_token = _adminToolsService.GetJwtRefresh(inputWithCorrelationId);

                if (string.IsNullOrEmpty(refresh_token))
                {
                    return Unauthorized(new { message = "Refresh token expired or invalid" });
                }

                logger.WriteCId(Level.Debug, $"Refresh Token Obtained Successfully", correlationId);

                var newJwt = GetJwt(identity, refresh_token, correlationId);

                var cookieOptions = GetCookieOptions();

                Response.Cookies.Append("Ally-Authorization", newJwt.AccessToken, cookieOptions);

                return Ok(newJwt);
            }

        }

        /// <summary>
        /// Method to handle and validate the token.
        /// </summary>
        /// <returns>True or False boolean value</returns>
        [HttpGet("/Logout")]
        [Authorize]
        public IActionResult Logout()
        {
            var logger = LoggerFactory.Create(LoggingModel.Login);
            GetOrCreateCorrelationId(out var correlationId);
            logger.WriteCId(Level.Trace, $"Starting {nameof(AccountController)} {nameof(Logout)}", correlationId);

            Response.Cookies.Delete("Ally-Authorization");
            Response.Cookies.Delete("Ally-Onboarding-Authorization");
            Response.Cookies.Delete("AllyAuthToken");
            Response.Cookies.Delete("AllyRefreshToken");
            Response.Cookies.Delete("JwtRefresh");
            Response.Cookies.Delete("JwtToken");

            logger.WriteCId(Level.Trace, $"Finished {nameof(AccountController)} {nameof(Logout)}", correlationId);

            return Ok();
        }

        #endregion

        #region Private Method

        private JWTAccessToken GetJwt(ClaimsIdentity userDetails, string refresh_token, string correlationId)
        {
            var appUser = new AppUser()
            {
                Email = userDetails.GetSpecificClaim("UserEmailId"),
                UserName = userDetails.GetSpecificClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"),
                Admin = userDetails.GetSpecificClaim("Admin") == "True",
                AdminLite = userDetails.GetSpecificClaim("AdminLite") == "True",
                WellAdmin = userDetails.GetSpecificClaim("WellAdmin") == "True",
                WellConfig = userDetails.GetSpecificClaim("WellConfig") == "True",
                WellConfigLite = userDetails.GetSpecificClaim("WellConfigLite") == "True",
                WellControl = userDetails.GetSpecificClaim("WellControl") == "True",
            };

            var inputWithCorrelationId = new WithCorrelationId<AppUser>(correlationId, appUser);
            var encodedJwt = _adminToolsService.GetJwt(inputWithCorrelationId);

            var response = new JWTAccessToken
            {
                AccessToken = encodedJwt,
                ExpiresIn = (int)TimeSpan.FromMinutes(60).TotalSeconds,
                RefreshToken = refresh_token,
                UserName = appUser.UserName,
                IsFirstTimeLogin = appUser.IsFirstTimeLogin,
                IsAdmin = appUser.Admin || appUser.WellAdmin
            };

            return response;
        }

        private JWTAccessToken GetJwt(AppUser user, string refresh_token, string correlationId)
        {
            var inputWithCorrelationId = new WithCorrelationId<AppUser>(correlationId, user);
            var encodedJwt = _adminToolsService.GetJwt(inputWithCorrelationId);

            var response = new JWTAccessToken
            {
                AccessToken = encodedJwt,
                ExpiresIn = (int)TimeSpan.FromMinutes(60).TotalSeconds,
                RefreshToken = refresh_token,
                UserName = user.UserName,
                IsFirstTimeLogin = user.IsFirstTimeLogin,
                IsAdmin = user.Admin || user.WellAdmin
            };

            return response;
        }

        private CookieOptions GetCookieOptions()
        {
            return new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Secure = AdminToolsService.UseSecureCookie,
                IsEssential = true,
            };
        }

        #endregion

    }
}
