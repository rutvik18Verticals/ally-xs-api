using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;
using Theta.XSPOC.Apex.Api.Data.Models.Identity;

namespace Theta.XSPOC.Apex.Api.Core.Services.UserAccountService
{
    /// <summary>
    /// 
    /// </summary>
    public class LoggedInUserProvider : ILoggedInUserProvider
    {

        #region Private Members

        private readonly IHttpContextAccessor _contextAccessor;
        private AppUser _userDetails;

        #endregion

        #region Contructor

        /// <summary>
        /// Creates a new instance of <seealso cref="LoggedInUserProvider"/>
        /// </summary>
        /// <param name="contextAccessor"></param>
        public LoggedInUserProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        #endregion

        #region ILoggedInUserProvider implementation

        /// <summary>
        /// Get the logged in user deatils from HttpContext
        /// </summary>
        /// <returns></returns>
        public AppUser GetUserDetails()
        {

            //logged in through the app then use User Claims
            if (_contextAccessor.HttpContext?.User != null)
            {
                var user = _contextAccessor.HttpContext.User;

                var userName = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var userObjectId = user.Claims.FirstOrDefault(c => c.Type == "UserObjectId")?.Value;
                var blnIsAdmin = bool.TryParse(user.Claims?.FirstOrDefault(c => c.Type == "Admin")?.Value, out var isAdmin);
                var blnIsWellAdmin = bool.TryParse(user.Claims?.FirstOrDefault(c => c.Type == "WellAdmin")?.Value, out var isWellAdmin);
                var blnIsWellControl = bool.TryParse(user.Claims?.FirstOrDefault(c => c.Type == "WellControl")?.Value, out var isWellControl);
                var blnIswellConfig = bool.TryParse(user.Claims?.FirstOrDefault(c => c.Type == "WellConfig")?.Value, out var isWellConfig);
                var blnIsAdminLite = bool.TryParse(user.Claims?.FirstOrDefault(c => c.Type == "AdminLite")?.Value, out var isAdminLite);
                var blnIsWellConfigLit = bool.TryParse(user.Claims?.FirstOrDefault(c => c.Type == "WellConfigLite")?.Value, out var isWellConfigLite);

                _userDetails = new AppUser
                {
                    UserObjectId = userObjectId,
                    UserName = userName,
                    Admin = isAdmin,
                    WellAdmin = isWellAdmin,
                    WellControl = isWellControl,
                    WellConfig = isWellConfig,
                    AdminLite = isAdminLite,
                    WellConfigLite = isWellConfigLite
                };
            }

            return _userDetails;
        }

        #endregion

    }
}
