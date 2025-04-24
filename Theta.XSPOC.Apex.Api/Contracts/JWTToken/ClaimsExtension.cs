using System.Linq;
using System.Security.Claims;

namespace Theta.XSPOC.Apex.Api.Contracts.JWTToken
{
    /// <summary>
    /// The ClaimsExtension.
    /// </summary>
    public static class ClaimsExtension
    {

        /// <summary>
        /// Gets the claims.
        /// </summary>
        /// <param name="claimsIdentity">The claimsIdentity.</param>
        /// <param name="claimType">The claimType.</param>
        /// <returns>string.</returns>
        public static string GetSpecificClaim(this ClaimsIdentity claimsIdentity, string claimType)
        {
            var claim = claimsIdentity?.Claims.FirstOrDefault(x => x.Type == claimType);
            return (claim != null) ? claim.Value : string.Empty;
        }

    }
}
