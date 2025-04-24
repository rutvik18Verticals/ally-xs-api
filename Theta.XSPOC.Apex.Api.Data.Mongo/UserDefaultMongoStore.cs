using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Security;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Data.Mongo
{
    /// <summary>
    /// Represents a Mongo store for UserDefault entities.
    /// </summary>
    public class UserDefaultMongoStore : MongoOperations, IUserDefaultStore
    {

        #region Private Constants

        private const string USER_COLLECTION = "User";
        private const string USER_PREFERENCE_COLLECTION = "UserPreference";

        #endregion

        #region Private Fields

        private readonly IMongoDatabase _database;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="UserDefaultMongoStore"/> using the provided 
        /// </summary>
        /// <param name="database">The mongo database.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public UserDefaultMongoStore(IMongoDatabase database, IThetaLoggerFactory loggerFactory)
            : base(database, loggerFactory)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
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
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(UserDefaultMongoStore)} {nameof(GetItem)}", correlationId);

            string userDefaultValue = string.Empty;
            try
            {
                var filter = new FilterDefinitionBuilder<User>().Where(x => x.UserName.ToLower() == username.ToLower());

                var userData = Find<User>(USER_COLLECTION, filter, correlationId);

                if (userData == null || userData.Count == 0)
                {
                    logger.WriteCId(Level.Info, "Missing user data", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultMongoStore)} {nameof(GetItem)}", correlationId);

                    return userDefaultValue;
                }
                else
                {
                    var user = userData.FirstOrDefault();

                    var filterUserPreference = new FilterDefinitionBuilder<UserPreference>().Where(x => x.UserId == user.Id
                    && x.PreferenceItem.GroupName == group && x.PreferenceItem.PropertyItem.Any(p => p.PropertyName == property));

                    var userPreferenceData = Find<UserPreference>(USER_PREFERENCE_COLLECTION, filterUserPreference, correlationId);
                    if (userPreferenceData != null && userPreferenceData.Count > 0)
                    {
                        var properties = userPreferenceData.FirstOrDefault()?.PreferenceItem?.PropertyItem;

                        if (properties != null && properties.Count > 0)
                        {
                            userDefaultValue = properties.FirstOrDefault(p => p.PropertyName == property)?.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultMongoStore)} {nameof(GetItem)}", correlationId);

            return userDefaultValue;
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
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(UserDefaultMongoStore)} {nameof(GetItem)}", correlationId);

            string userDefaultValue = string.Empty;
            try
            {
                var filter = new FilterDefinitionBuilder<User>().Where(x => x.UserName.ToLower() == username.ToLower());

                var userData = Find<User>(USER_COLLECTION, filter, correlationId);

                if (userData != null && userData.Count > 0)
                {
                    var user = userData.FirstOrDefault();

                    var filterUserPreference = new FilterDefinitionBuilder<UserPreference>().Where(x => x.UserId == user.Id
                    && x.PreferenceItem.GroupName == group);

                    var userPreferenceData = Find<UserPreference>(USER_PREFERENCE_COLLECTION, filterUserPreference, correlationId);
                    if (userPreferenceData == null || userPreferenceData.Count == 0)
                    {
                        UserPreference userPreference = new UserPreference()
                        {
                            UserId = user.Id,
                            PreferenceItem = new Preference()
                            {
                                GroupName = group,
                                PropertyItem = new List<Property>()
                                {
                                    new()
                                    {
                                        PropertyName = property,
                                        Value = value
                                    }
                                }
                            }
                        };

                        var mongoCollection = _database.GetCollection<UserPreference>(USER_PREFERENCE_COLLECTION, null);
                        mongoCollection.InsertOne(userPreference);
                    }
                    else
                    {

                        var update = Builders<UserPreference>.Update
                            .Set(t => t.PreferenceItem.PropertyItem.First().Value, value);

                        var mongoCollection = _database.GetCollection<UserPreference>(USER_PREFERENCE_COLLECTION, null);
                        mongoCollection.UpdateOne(filterUserPreference, update);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
                return false;
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultMongoStore)} {nameof(GetItem)}", correlationId);

            return true;
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
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(UserDefaultMongoStore)} {nameof(GetDefaultItem)}", correlationId);

            if (string.IsNullOrWhiteSpace(userId))
            {
                var message = $"{nameof(userId)}" +
                    $"{nameof(defaultGroup)}" +
                    $"{nameof(property)}" +
                    $" should be provided to get Default Item.";
                logger.WriteCId(Level.Info, message, correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultMongoStore)} {nameof(GetDefaultItem)}", correlationId);

                return null;
            }
            await Task.Yield();

            var result = new UserDefaultItem();

            try
            {
                var filter = new FilterDefinitionBuilder<User>().Where(x => x.UserName.ToLower() == userId.ToLower());

                var userData = Find<User>(USER_COLLECTION, filter, correlationId);

                if (userData == null || userData.Count == 0)
                {
                    logger.WriteCId(Level.Info, "Missing user data", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultMongoStore)} {nameof(GetItem)}", correlationId);

                    return result;
                }
                else
                {
                    var user = userData.FirstOrDefault();

                    var filterUserPreference = new FilterDefinitionBuilder<UserPreference>().Where(x => x.UserId == user.Id
                    && x.PreferenceItem.GroupName == defaultGroup);

                    var userPreferenceData = Find<UserPreference>(USER_PREFERENCE_COLLECTION, filterUserPreference, correlationId);
                    if (userPreferenceData != null && userPreferenceData.Count > 0)
                    {
                        var properties = userPreferenceData.FirstOrDefault()?.PreferenceItem?.PropertyItem;

                        if (properties != null && properties.Count > 0 && property != null)
                        {
                            var propertyValue = properties.FirstOrDefault(p => p.PropertyName == property)?.Value;

                            result = new UserDefaultItem()
                            {
                                Value = propertyValue,
                                DefaultsGroup = defaultGroup,
                                Property = property,
                                UserId = userId,
                            };
                        }
                        else if (properties != null && properties.Count > 0 && property == null)
                        {
                            result = new UserDefaultItem()
                            {
                                Value = properties.FirstOrDefault().Value,
                                DefaultsGroup = defaultGroup,
                                Property = properties.FirstOrDefault().PropertyName,
                                UserId = userId,
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultMongoStore)} {nameof(GetDefaultItem)}", correlationId);

            return result;
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
            await Task.Yield();
            var logger = _loggerFactory.Create(LoggingModel.MongoDataStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(UserDefaultMongoStore)} {nameof(GetDefaultItem)}", correlationId);

            if (string.IsNullOrWhiteSpace(userId))
            {
                var message = $"{nameof(userId)}" +
                    $"{nameof(defaultGroup)}" +
                    $" should be provided to get Default Item.";
                logger.WriteCId(Level.Info, message, correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultMongoStore)} {nameof(GetDefaultItem)}", correlationId);

                return null;
            }
            await Task.Yield();

            var result = new Dictionary<string, UserDefaultItem>();

            try
            {
                var filter = new FilterDefinitionBuilder<User>().Where(x => x.UserName.ToLower() == userId.ToLower());

                var userData = Find<User>(USER_COLLECTION, filter, correlationId);

                if (userData == null || userData.Count == 0)
                {
                    logger.WriteCId(Level.Info, "Missing user data", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultMongoStore)} {nameof(GetItem)}", correlationId);

                    return result;
                }
                else
                {
                    var user = userData.FirstOrDefault();

                    var filterUserPreference = new FilterDefinitionBuilder<UserPreference>().Where(x => x.UserId == user.Id
                    && x.PreferenceItem.GroupName == defaultGroup);

                    var userPreferenceData = Find<UserPreference>(USER_PREFERENCE_COLLECTION, filterUserPreference, correlationId);
                    if (userPreferenceData != null && userPreferenceData.Count > 0)
                    {
                        var properties = userPreferenceData.FirstOrDefault()?.PreferenceItem?.PropertyItem;

                        if (properties != null && properties.Count > 0)
                        {
                            var userDefaults = properties.Select(p => new UserDefaultItem()
                            {
                                Value = p.Value,
                                DefaultsGroup = defaultGroup,
                                Property = p.PropertyName,
                                UserId = userId,
                            }).ToList();

                            return userDefaults.ToImmutableSortedDictionary(m => m.Property, x => x);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.WriteCId(Level.Error, "An error occurred", ex, correlationId);
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(UserDefaultMongoStore)} {nameof(GetDefaultItem)}", correlationId);

            return result;
        }

        #endregion

    }
}
