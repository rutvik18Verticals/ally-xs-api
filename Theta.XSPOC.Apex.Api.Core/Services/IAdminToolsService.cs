using System.Collections.Generic;
using System.Security.Claims;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Data.Models.Identity;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Represents the IAdminToolsService.
    /// </summary>
    public interface IAdminToolsService
    {

        /// <summary>
        /// Processes the provided user credentials and validate the user exist.
        /// </summary>
        /// <param name="data">The <seealso cref="FormLoginInput"/> to act on, annotated.
        /// with a correlation id.</param>
        /// <returns>The list of.<seealso cref="AppUser"/></returns>
        AppUser FindByName(WithCorrelationId<FormLoginInput> data);

        /// <summary>
        /// Generates the Jwt Refresh token.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns>string.</returns>
        string GetJwtRefresh(WithCorrelationId<string> email);

        /// <summary>
        /// Generates the Jwt token.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>string.</returns>
        public string GetJwt(WithCorrelationId<AppUser> user);

        /// <summary>
        /// Gets the Roles from local database.
        /// </summary>
        /// <param name="correlationId"></param>
        /// <returns>string.</returns>
        public IList<string> GetRolesAsync(string correlationId);

        /// <summary>
        /// Gets the claims principal from token
        /// </summary>
        /// <param name="token">the jwt accesstoken</param>
        /// <param name="correlationId">The correlation Id.</param>
        /// <returns><seealso cref="ClaimsPrincipal"/></returns>
        ClaimsPrincipal ValidateToken(string token, string correlationId);

    }
}