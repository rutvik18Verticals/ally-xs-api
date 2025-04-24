using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Models.Identity;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Data.Sql
{
    /// <summary>
    /// Represents the Admin Tools Service.
    /// </summary>
    public class AuthServiceSQLStore : SQLStoreBase, IAuthService
    {

        #region Private Members

        private readonly IThetaDbContextFactory<XspocDbContext> _contextFactory;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for AdminToolsService.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{XspocDbContext}"/> to get the <seealso cref="XspocDbContext"/> 
        /// to use for database operations.
        /// </param>
        /// <param name="loggerFactory"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/> is null.
        /// <paramref name="loggerFactory"/> is null.
        /// </exception>
        public AuthServiceSQLStore(IThetaDbContextFactory<XspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        #endregion

        #region IAuthService Implementation

        /// <summary>
        /// Get Roles Async.
        /// </summary>
        /// <returns>List of roles.</returns>
        public IList<string> GetRolesAsync(string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(AuthServiceSQLStore)} {nameof(GetRolesAsync)}", correlationId);

            var result = new List<string>();

            logger.WriteCId(Level.Trace, $"Finished {nameof(AuthServiceSQLStore)} {nameof(GetRolesAsync)}", correlationId);

            return result;
        }

        /// <summary>
        /// Find By Name Async.
        /// </summary>
        /// <param name="email">The userName.</param>
        /// <param name="password">The userName.</param>
        /// <param name="correlationId">The Correlation Id.</param>
        /// <returns>AppUser.</returns>
        public AppUser FindByNameAsync(string email, string password, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(AuthServiceSQLStore)} {nameof(FindByNameAsync)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var firstTimeLogin = context.UserDefaults.AsNoTracking().FirstOrDefault(userDefaults => userDefaults.UserId == email &&
                                   userDefaults.Property == "HasLoggedIn" && userDefaults.DefaultsGroup == "Login")?.Value;

                var result = context.UserSecurity.Where(a => a.UserName == email)
                    .Select(a => new AppUser
                    {
                        Email = a.Email,
                        UserName = a.UserName,
                        PasswordHash = a.Password,
                        WellControl = a.WellControl,
                        WellConfig = a.WellConfig,
                        Admin = a.Admin,
                        AdminLite = a.AdminLite,
                        WellAdmin = a.WellAdmin,
                        WellConfigLite = a.WellConfigLite,
                        IsFirstTimeLogin = string.IsNullOrWhiteSpace(firstTimeLogin) || firstTimeLogin != "1"
                    }).FirstOrDefault();

                logger.WriteCId(Level.Trace, $"Finished {nameof(AuthServiceSQLStore)} {nameof(FindByNameAsync)}", correlationId);

                return result;
            }
        }

        #endregion

    }
}
