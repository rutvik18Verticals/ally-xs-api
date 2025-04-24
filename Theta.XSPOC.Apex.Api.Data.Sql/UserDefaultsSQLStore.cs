using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Data.Sql
{
    /// <summary>
    /// This is the implementation that represents the user default settings stored in the XSPOC database.
    /// </summary>
    public class UserDefaultsSQLStore : SQLStoreBase, IUserDefaultStore
    {

        #region Private Members

        private readonly IThetaDbContextFactory<XspocDbContext> _contextFactory;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new <seealso cref="UserDefaultsSQLStore"/> using the provided <paramref name="contextFactory"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{XspocDbContext}"/> to get the <seealso cref="XspocDbContext"/> 
        /// to use for database operations.
        /// </param>
        /// <param name="loggerFactory"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/> is null.
        /// </exception>
        public UserDefaultsSQLStore(IThetaDbContextFactory<XspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IUserDefaultStore Implementation

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
            logger.WriteCId(Level.Trace, $"Starting {nameof(UserDefaultsSQLStore)} {nameof(GetItem)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var userDefaults = context.UserDefaults.AsNoTracking().FirstOrDefault(ud => ud.UserId == username &&
                    ud.Property == property && ud.DefaultsGroup == group);

                logger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultsSQLStore)} {nameof(GetItem)}", correlationId);

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
            logger.WriteCId(Level.Trace, $"Starting {nameof(UserDefaultsSQLStore)} {nameof(SetItem)}", correlationId);

            using (var context = _contextFactory.GetContext())
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

                logger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultsSQLStore)} {nameof(SetItem)}", correlationId);

                return context.SaveChanges() > 0;
            }
        }
        /// <summary>
        /// Gets a user default item for the specified <paramref name="userId"/>, <paramref name="property"/>, and
        /// <paramref name="defaultGroup"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="defaultGroup">The default group.</param>
        /// <param name="property">The property. Optional parameter.</param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        public async Task<UserDefaultItem> GetDefaultItem(string userId, string defaultGroup, string correlationId, string property = null)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(UserDefaultsSQLStore)} {nameof(GetDefaultItem)}", correlationId);

            if (string.IsNullOrWhiteSpace(userId))
            {
                var message = $"{nameof(userId)}" +
                    $"{nameof(defaultGroup)}" +
                    $"{nameof(property)}" +
                    $" should be provided to get default Item.";
                logger.WriteCId(Level.Info, message, correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultsSQLStore)} {nameof(GetDefaultItem)}", correlationId);

                return null;
            }

            await using (var context = _contextFactory.GetContext())
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

                logger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultsSQLStore)} {nameof(GetDefaultItem)}", correlationId);

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
            logger.WriteCId(Level.Trace, $"Starting {nameof(UserDefaultsSQLStore)} {nameof(GetDefaultItemByGroup)}", correlationId);

            if (string.IsNullOrWhiteSpace(userId))
            {
                var message = $"{nameof(userId)}" +
                    $"{nameof(defaultGroup)}" +
                   $" should be provided to get default Item.";
                logger.WriteCId(Level.Info, message, correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultsSQLStore)} {nameof(GetDefaultItemByGroup)}", correlationId);

                return new ConcurrentDictionary<string, UserDefaultItem>();
            }

            await using (var context = _contextFactory.GetContext())
            {
                var query = context.UserDefaults.AsNoTracking().Select(m => new UserDefaultItem()
                {
                    Value = m.Value,
                    DefaultsGroup = m.DefaultsGroup,
                    Property = m.Property,
                    UserId = m.UserId,
                }).Where(m => m.UserId == userId && m.DefaultsGroup == defaultGroup);

                var result = query.ToImmutableSortedDictionary(m => m.Property, x => x);

                logger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultsSQLStore)} {nameof(GetDefaultItemByGroup)}", correlationId);

                return result;
            }
        }

        #endregion

    }
}
