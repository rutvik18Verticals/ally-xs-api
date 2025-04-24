using Theta.XSPOC.Apex.Api.Data.Models.Identity;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// Represents the IAuthService.
    /// </summary>
    public interface IAuthService
    {

        /// <summary>
        /// Find by username against the local database.
        /// </summary>
        /// <param name="email">The userName.</param>
        /// <param name="password">The userName.</param>
        /// <param name="correlationId">The Correlation Id.</param>
        /// <returns>AppUser.</returns>
        public AppUser FindByNameAsync(string email, string password, string correlationId);

    }
}
