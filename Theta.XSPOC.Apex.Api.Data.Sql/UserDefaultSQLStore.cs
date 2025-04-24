using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Data.Sql
{
    /// <summary>
    /// This SQL implementation for the user default store.
    /// </summary>
    public class UserDefaultSQLStore : IUserDefaultStore
    {

        #region Private Fields

        private readonly IThetaDbContextFactory<XspocDbContext> _thetaDbContextFactory;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="UserDefaultSQLStore"/> using the provided
        /// <paramref name="thetaDbContextFactory"/>.
        /// </summary>
        /// <param name="thetaDbContextFactory">The theta db context factory used to get a db context.</param>
        /// <param name="loggerFactory"></param>
        /// <exception cref="ArgumentNullException">
        /// when <paramref name="thetaDbContextFactory"/> is null.
        /// </exception>
        public UserDefaultSQLStore(IThetaDbContextFactory<XspocDbContext> thetaDbContextFactory, IThetaLoggerFactory loggerFactory)
        {
            _thetaDbContextFactory =
                thetaDbContextFactory ?? throw new ArgumentNullException(nameof(thetaDbContextFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IUserDefaultRepository Implementation

        /// <summary>
        /// Gets a user default item for the specified <paramref name="userId"/>, <paramref name="property"/>, and
        /// <paramref name="defaultGroup"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="defaultGroup">The default group.</param>
        /// <param name="correlationId"></param>
        /// <param name="property">The property. Optional parameter.</param>
        /// <returns></returns>
        public async Task<UserDefaultItem> GetDefaultItem(string userId, string defaultGroup, string correlationId, string property = null)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(UserDefaultSQLStore)} {nameof(GetDefaultItem)}", correlationId);

            if (string.IsNullOrWhiteSpace(userId))
            {
                var message = $"{nameof(userId)}" +
                    $"{nameof(defaultGroup)}" +
                    $"{nameof(property)}" +
                    $" should be provided to get Default Item.";
                logger.WriteCId(Level.Info, message, correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultSQLStore)} {nameof(GetDefaultItem)}", correlationId);

                return null;
            }

            await using (var context = _thetaDbContextFactory.GetContext())
            {
                var query = context.UserDefaults.AsNoTracking().Select(m => new UserDefaultItem()
                {
                    Value = m.Value,
                    DefaultsGroup = m.DefaultsGroup,
                    Property = m.Property,
                    UserId = m.UserId,
                }).Where(m => m.UserId == userId && m.DefaultsGroup == defaultGroup);

                if (string.IsNullOrWhiteSpace(property) == false)
                {
                    query = query.Where(m => m.Property == property);
                }

                var result = await query.FirstOrDefaultAsync();

                logger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultSQLStore)} {nameof(GetDefaultItem)}", correlationId);

                return result;
            }
        }

        /// <summary>
        /// Gets a sorted dictionary of user default items for the specified <paramref name="userId"/> and
        /// <paramref name="defaultGroup"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="defaultGroup">The default group.</param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        public async Task<IDictionary<string, UserDefaultItem>> GetDefaultItemByGroup(string userId,
            string defaultGroup, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(UserDefaultSQLStore)} {nameof(GetDefaultItemByGroup)}", correlationId);

            if (string.IsNullOrWhiteSpace(userId))
            {
                var message = $"{nameof(userId)}" +
                    $"{nameof(defaultGroup)}" +
                   $" should be provided to get Default Item.";
                logger.WriteCId(Level.Info, message, correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultSQLStore)} {nameof(GetDefaultItemByGroup)}", correlationId);

                return new ConcurrentDictionary<string, UserDefaultItem>();
            }

            await using (var context = _thetaDbContextFactory.GetContext())
            {
                var query = context.UserDefaults.AsNoTracking().Select(m => new UserDefaultItem()
                {
                    Value = m.Value,
                    DefaultsGroup = m.DefaultsGroup,
                    Property = m.Property,
                    UserId = m.UserId,
                }).Where(m => m.UserId == userId && m.DefaultsGroup == defaultGroup);

                var result = query.ToImmutableSortedDictionary(m => m.Property, x => x);

                logger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultSQLStore)} {nameof(GetDefaultItemByGroup)}", correlationId);

                return result;
            }
        }

        /// <summary>
        /// Gets the default item for the provided <paramref name="username"/>, <paramref name="property"/>, and <paramref name="group"/>.
        /// </summary>
        /// <param name="username">The username to get the default for.</param>
        /// <param name="property">The property to get the default for.</param>
        /// <param name="group">The group to get the default for.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="string"/> value of the default</returns>
        public string GetItem(string username, string property, string group, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(UserDefaultSQLStore)} {nameof(GetItem)}", correlationId);

            using (var context = _thetaDbContextFactory.GetContext())
            {
                var userDefaults = context.UserDefaults.AsNoTracking().FirstOrDefault(ud => ud.UserId == username &&
                    ud.Property == property && ud.DefaultsGroup == group);

                logger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultSQLStore)} {nameof(GetItem)}", correlationId);

                return userDefaults?.Value;
            }
        }

        /// <summary>
        /// Sets the default item for the provided <paramref name="username"/>, <paramref name="property"/>,
        /// <paramref name="group"/>, and <paramref name="value"/>.
        /// </summary>
        /// <param name="username">The username to set the default value for.</param>
        /// <param name="property">The property to set the default value for.</param>
        /// <param name="group">The group to set the default value for.</param>
        /// <param name="value">The value to set.</param>
        /// <param name="correlationId"></param>
        /// <returns>True if successful, false otherwise.</returns>
        public bool SetItem(string username, string property, string group, string value, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(UserDefaultSQLStore)} {nameof(SetItem)}", correlationId);

            using (var context = _thetaDbContextFactory.GetContext())
            {
                var userDefaults = context.UserDefaults.AsNoTracking().FirstOrDefault(ud => ud.UserId == username &&
                                   ud.Property == property && ud.DefaultsGroup == group);

                if (userDefaults == null)
                {
                    userDefaults = new UserDefaultEntity()
                    {
                        UserId = username,
                        Property = property,
                        DefaultsGroup = group,
                        Value = value
                    };

                    context.UserDefaults.Add(userDefaults);
                }
                else
                {
                    userDefaults.Value = value;
                }

                logger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultSQLStore)} {nameof(SetItem)}", correlationId);

                return context.SaveChanges() > 0;
            }
        }

        #endregion

    }
}