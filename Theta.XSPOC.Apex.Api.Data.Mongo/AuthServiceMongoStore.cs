using MongoDB.Driver;
using System;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models.Identity;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Security;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using User = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Security.User;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// Represents the implementation of IAuthService with mongo data store.
    /// </summary>
    public class AuthServiceMongoStore : MongoOperations, IAuthService
    {

        #region Private Constants

        private const string USER_COLLECTION = "User";
        private const string ROLE_COLLECTION = "Role";
        private const string USERROLE_COLLECTION = "UserRole";
        private const string USER_PREFERENCE_COLLECTION = "UserPreference";

        #endregion

        #region Private Fields

        private readonly IMongoDatabase _database;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="AuthServiceMongoStore"/> using the provided 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public AuthServiceMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory)
            : base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IAuthService Implementation

        /// <summary>
        /// Find by username against the local database.
        /// </summary>
        /// <param name="userId">The userName.</param>
        /// <param name="password">The userName.</param>
        /// <param name="correlationId">The Correlation Id.</param>
        /// <returns>AppUser.</returns>
        public AppUser FindByNameAsync(string userId, string password, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(AuthServiceMongoStore)} {nameof(FindByNameAsync)}", correlationId);
            var result = new AppUser();
            try
            {
                var filter = new FilterDefinitionBuilder<User>().Where(x => x.UserName.ToLower() == userId.ToLower());

                var userData = Find<User>(USER_COLLECTION, filter, correlationId);

                var user = userData.FirstOrDefault();

                if (user != null)
                {
                    var filterUserPreference = new FilterDefinitionBuilder<UserPreference>().Where(x => x.UserId == user.Id
                    && x.PreferenceItem.GroupName == "Login" && x.PreferenceItem.PropertyItem.Any(a => a.PropertyName == "HasLoggedIn" && a.Value == "1"));

                    var userPreferenceData = Find<UserPreference>(USER_PREFERENCE_COLLECTION, filterUserPreference, correlationId);
                    var firstTimeLogin = userPreferenceData.FirstOrDefault()?.PreferenceItem.PropertyItem[0].Value;

                    var roles = FindAll<Role>(ROLE_COLLECTION, correlationId);

                    var filterUserRoles = new FilterDefinitionBuilder<UserRole>().Where(x => x.UserId == user.Id);
                    var userRolesData = Find<UserRole>(USERROLE_COLLECTION, filterUserRoles, correlationId);

                    result = new AppUser
                    {
                        UserName = user.UserName,
                        PasswordHash = user.Password,
                        WellControl = userRolesData.Any(ur => ur.RoleIds.Contains(roles.First(r => r.RoleName == "Well Control").Id)),
                        WellConfig = userRolesData.Any(ur => ur.RoleIds.Contains(roles.First(r => r.RoleName == "Well Config").Id)),
                        Admin = userRolesData.Any(ur => ur.RoleIds.Contains(roles.First(r => r.RoleName == "Admin").Id)),
                        AdminLite = userRolesData.Any(ur => ur.RoleIds.Contains(roles.First(r => r.RoleName == "Admin Lite").Id)),
                        WellAdmin = userRolesData.Any(ur => ur.RoleIds.Contains(roles.First(r => r.RoleName == "Well Admin").Id)),
                        WellConfigLite = userRolesData.Any(ur => ur.RoleIds.Contains(roles.First(r => r.RoleName == "Well Config Lite").Id)),
                        IsFirstTimeLogin = string.IsNullOrWhiteSpace(firstTimeLogin) || firstTimeLogin != "1"
                    };
                }
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(AuthServiceMongoStore)} {nameof(FindByNameAsync)}", correlationId);

            return result;
        }

        #endregion

    }
}
