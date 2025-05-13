using Theta.XSPOC.Apex.Api.Data.Models.Identity;

namespace Theta.XSPOC.Apex.Api.Core.Services.UserAccountService
{
    /// <summary>
    /// Interface for User Provider service
    /// </summary>
    public interface ILoggedInUserProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        AppUser GetUserDetails();
    }
}
