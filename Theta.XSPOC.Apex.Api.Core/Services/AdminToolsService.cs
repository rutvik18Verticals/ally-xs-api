using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models.Identity;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Represents the Admin Tools Service.
    /// </summary>
    public class AdminToolsService : IAdminToolsService
    {

        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IThetaLoggerFactory _loggerFactory;

        #region Public Properties

        /// <summary>
        /// Gets or sets the AudienceSecret.
        /// </summary>
        public static string AudienceSecret { get; set; }

        /// <summary>
        /// Gets or sets the AudienceIssuer.
        /// </summary>
        public static string AudienceIssuer { get; set; }

        /// <summary>
        /// Gets or sets the Audience.
        /// </summary>
        public static string Audience { get; set; }

        /// <summary>
        /// Gets or sets the TimeOut.
        /// </summary>
        public static int TimeOut { get; set; }

        /// <summary>
        /// Gets or sets the use secure cookies flag.
        /// </summary>
        public static bool UseSecureCookie { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for AdminToolsService.
        /// </summary>
        /// <param name="authService">The <seealso cref="IAuthService"/> to call 
        /// for handling requests.</param>
        /// <param name="userService">The <seealso cref="IUserService"/> to call</param>
        /// <param name="loggerFactory">The <seealso cref="IThetaLoggerFactory"/> service.</param>
        /// <exception cref="ArgumentNullException">
        /// when <paramref name="authService"/> is null
        /// or
        /// <paramref name="userService"/> is null.
        /// </exception>
        public AdminToolsService(IAuthService authService, IUserService userService, IThetaLoggerFactory loggerFactory)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get Roles Async.
        /// </summary>
        /// <param name="correlationId"></param>
        /// <returns>string.</returns>
        public IList<string> GetRolesAsync(string correlationId)
        {
            var result = new List<string>();
            return result;
        }

        /// <summary>
        /// Processes the provided user credentials and validate the user exist.
        /// </summary>
        /// <param name="requestWithCorrelationId">The <seealso cref="FormLoginInput"/> to act on, annotated.
        /// with a correlation id.</param>
        /// <returns>The list of.<seealso cref="AppUser"/></returns>
        public AppUser FindByName(WithCorrelationId<FormLoginInput> requestWithCorrelationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.Login);
            logger.WriteCId(Level.Trace, $"Starting {nameof(AdminToolsService)} {nameof(FindByName)}", requestWithCorrelationId?.CorrelationId);

            if (requestWithCorrelationId == null)
            {
                var message = $"{nameof(requestWithCorrelationId)} is null, cannot get login details.";
                logger.Write(Level.Info, message);

                return null;
            }

            if (requestWithCorrelationId?.Value == null)
            {
                var message = $"{nameof(requestWithCorrelationId)} is null, cannot get login details.";
                logger.WriteCId(Level.Info, message, requestWithCorrelationId?.CorrelationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AdminToolsService)} {nameof(FindByName)}", requestWithCorrelationId.CorrelationId);

                return null;
            }

            var correlationId = requestWithCorrelationId?.CorrelationId;

            var loginRequest = requestWithCorrelationId.Value;

            if (string.IsNullOrEmpty(loginRequest.UserName) &&
                string.IsNullOrEmpty(loginRequest.Password))
            {
                var message = $"Either {nameof(loginRequest.UserName)} or {nameof(loginRequest.Password)}" +
                    $" should be provided to get access token.";
                logger.WriteCId(Level.Info, message, correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AdminToolsService)} {nameof(FindByName)}", requestWithCorrelationId.CorrelationId);

                return null;
            }

            if (!string.IsNullOrEmpty(loginRequest.UserName) &&
                string.IsNullOrEmpty(loginRequest.Password))
            {
                var message = $"{nameof(loginRequest.Password)}" +
                   $" should be provided to get access token.";
                logger.WriteCId(Level.Info, message, correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AdminToolsService)} {nameof(FindByName)}", requestWithCorrelationId.CorrelationId);

                return null;
            }
            else if (!string.IsNullOrEmpty(loginRequest.UserName) &&
                     !string.IsNullOrEmpty(loginRequest.Password))
            {
                var userDetails = _authService.FindByNameAsync(loginRequest.UserName, loginRequest.Password, correlationId);

                if (userDetails == null)
                {
                    var message = $"user does not exist.";
                    logger.WriteCId(Level.Info, message, correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(AdminToolsService)} {nameof(FindByName)}", requestWithCorrelationId.CorrelationId);

                    return null;
                }

                var validatePassword = PasswordUtility.VerifyPassword(loginRequest.UserName, loginRequest.Password, userDetails.PasswordHash);
                if (validatePassword)
                {

                    if (_userService.IsUserFirstTimeLogin(loginRequest.UserName, correlationId))
                    {
                        _userService.SetUserLoggedIn(loginRequest.UserName, correlationId);
                    }
                    logger.WriteCId(Level.Trace, $"Finished {nameof(AdminToolsService)} {nameof(FindByName)}", requestWithCorrelationId.CorrelationId);

                    return userDetails;
                }
                else
                {
                    var message = $"incorrect password.";
                    logger.WriteCId(Level.Info, message, correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(AdminToolsService)} {nameof(FindByName)}", requestWithCorrelationId.CorrelationId);

                    return null;
                }
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(AdminToolsService)} {nameof(FindByName)}", requestWithCorrelationId.CorrelationId);

            return null;
        }

        /// <summary>
        /// Get Jwt Refresh.
        /// </summary>
        /// <param name="input">The email".</param>
        /// <returns>string.</returns>
        public string GetJwtRefresh(WithCorrelationId<string> input)
        {
            var logger = _loggerFactory.Create(LoggingModel.Login);
            logger.WriteCId(Level.Trace, $"Starting {nameof(AdminToolsService)} {nameof(GetJwtRefresh)}", input?.CorrelationId);

            if (input == null)
            {
                logger.Write(Level.Info, "Missing input");
                logger.Write(Level.Trace, $"Finished {nameof(AdminToolsService)} {nameof(GetJwtRefresh)}");

                throw new ArgumentNullException(nameof(input));
            }

            var now = DateTime.UtcNow;

            var email = input.Value;

            var claims = new List<Claim>
            {
                new Claim("UserEmailId", email)
            };

            var symmetricKeyAsBase64 = AudienceSecret;
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var jwt = new JwtSecurityToken(
            issuer: AudienceIssuer,
                audience: Audience,
                notBefore: now,
                claims: claims,
                expires: now.Add(TimeSpan.FromMinutes(TimeOut)),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            logger.WriteCId(Level.Trace, $"Finished {nameof(AdminToolsService)} {nameof(GetJwtRefresh)}", input.CorrelationId);

            return encodedJwt;
        }

        /// <summary>
        /// GetJwt token.
        /// </summary>
        /// <param name="input">The user.</param>
        /// <returns>string.</returns>
        public string GetJwt(WithCorrelationId<AppUser> input)
        {
            var logger = _loggerFactory.Create(LoggingModel.Login);
            logger.WriteCId(Level.Trace, $"Starting {nameof(AdminToolsService)} {nameof(GetJwt)}", input?.CorrelationId);

            if (input == null)
            {
                logger.Write(Level.Info, "Missing input");
                logger.Write(Level.Trace, $"Finished {nameof(AdminToolsService)} {nameof(GetJwt)}");

                throw new ArgumentNullException(nameof(input));
            }

            var now = DateTime.UtcNow;

            var user = input.Value;

            var claims = new List<Claim>
            {
                new Claim("UserEmailId", user != null && user.Email != null ? user.Email.ToString() : ""),
                new Claim(JwtRegisteredClaimNames.Sub, user != null ? user.UserName.ToString() : ""),
                new Claim(JwtRegisteredClaimNames.Jti, user != null ?  user.Email ?? "" : ""),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64),
            };

            if (user != null)
            {

                if (user.WellControl)
                {
                    claims.Add(new Claim("WellControl", user.WellControl.ToString()));
                }
                if (user.WellConfig)
                {
                    claims.Add(new Claim("WellConfig", user.WellConfig.ToString()));
                }
                if (user.Admin)
                {
                    claims.Add(new Claim("Admin", user.Admin.ToString()));
                }
                if (user.AdminLite)
                {
                    claims.Add(new Claim("AdminLite", user.AdminLite.ToString()));
                }
                if (user.WellAdmin)
                {
                    claims.Add(new Claim("WellAdmin", user.WellAdmin.ToString()));
                }
                if (user.WellConfigLite)
                {
                    claims.Add(new Claim("WellConfigLite", user.WellConfigLite.ToString()));
                }
            }

            // To Do, Add Roles
            var symmetricKeyAsBase64 = AudienceSecret;
            if (symmetricKeyAsBase64 != null)
            {
                var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
                var signingKey = new SymmetricSecurityKey(keyByteArray);

                var jwt = new JwtSecurityToken(
                    issuer: AudienceIssuer,
                    audience: Audience,
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(TimeSpan.FromMinutes(TimeOut)),
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                logger.WriteCId(Level.Trace, $"Finished {nameof(AdminToolsService)} {nameof(GetJwt)}", input.CorrelationId);

                return encodedJwt;
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(AdminToolsService)} {nameof(GetJwt)}", input.CorrelationId);

            return string.Empty;
        }

        /// <summary>
        /// Gets the claims principal from token
        /// </summary>
        /// <param name="token">the jwt accesstoken</param>
        /// <param name="correlationId">The correlation Id.</param>
        /// <returns><seealso cref="ClaimsPrincipal"/></returns>
        public ClaimsPrincipal ValidateToken(string token, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.Login);

            var symmetricKeyAsBase64 = AudienceSecret;
            if (symmetricKeyAsBase64 != null)
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(symmetricKeyAsBase64)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true, // Ensure token is not expired
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                try
                {
                    var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                    var jwtSecurityToken = securityToken as JwtSecurityToken;

                    if (jwtSecurityToken == null)
                    {
                        throw new SecurityTokenException("Invalid token");
                    }

                    return principal;
                }
                catch (SecurityTokenExpiredException)
                {
                    logger.WriteCId(Level.Warn, "Access token expired.", correlationId);
                    return null;
                }
                catch
                {
                    logger.WriteCId(Level.Warn, "Invalid access token.", correlationId);
                    return null;
                }
            }

            return null;
        }
        #endregion

    }
}
