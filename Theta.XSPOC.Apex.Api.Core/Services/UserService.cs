using System;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// The implementation for a service which handles user operations.
    /// </summary>
    public class UserService : IUserService
    {

        #region Private Fields

        private readonly IUserDefaultStore _userDefaultStore;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes an new intance of a <see cref="UserService"/>.
        /// </summary>
        /// <param name="userDefaultStore">The <seealso cref="IUserDefaultStore"/>.</param>
        /// <param name="loggerFactory">The <seealso cref="IThetaLoggerFactory"/> service.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="userDefaultStore"/> is null
        /// or
        /// <paramref name="loggerFactory"/> is null.
        /// </exception>
        public UserService(IUserDefaultStore userDefaultStore, IThetaLoggerFactory loggerFactory)
        {
            _userDefaultStore = userDefaultStore ?? throw new ArgumentNullException(nameof(userDefaultStore));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IUserService Implementation

        /// <summary>
        /// Determines if the user represented by the provided <paramref name="username"/> is a first time login.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="correlationId"></param>
        /// <returns>True if this is the first time logged in, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">
        /// when <paramref name="username"/> is null or empty.
        /// </exception>
        public bool IsUserFirstTimeLogin(string username, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.UserDefault);
            logger.WriteCId(Level.Trace, $"Starting {nameof(UserService)} {nameof(IsUserFirstTimeLogin)}", correlationId);
            if (string.IsNullOrWhiteSpace(username))
            {
                var message = $"{nameof(username)}" +
                    $" should check Is User First Time Login.";
                logger.WriteCId(Level.Error, message, correlationId);
                throw new ArgumentNullException(nameof(username));
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(UserService)} {nameof(IsUserFirstTimeLogin)}", correlationId);

            return _userDefaultStore.GetItem(username, "HasLoggedIn", "Login", correlationId) != "1";
        }

        /// <summary>
        /// Sets the user represented by the provided <paramref name="username"/> as having logged in.
        /// </summary>
        /// <param name="username">The username to set the value for.</param>
        /// <param name="correlationId"></param>
        /// <returns>True if the value was set, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">
        /// when <paramref name="username"/> is null or empty.
        /// </exception>
        public bool SetUserLoggedIn(string username, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.UserDefault);
            logger.WriteCId(Level.Trace, $"Starting {nameof(UserService)} {nameof(SetUserLoggedIn)}", correlationId);
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException(nameof(username));
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(UserService)} {nameof(SetUserLoggedIn)}", correlationId);

            return _userDefaultStore.SetItem(username, "HasLoggedIn", "Login", "1", correlationId);
        }

        /// <summary>
        /// Retrieves the user default data and maps it to the output model.
        /// </summary>
        /// <param name="data">The <seealso cref="UserDefaultInput"/> to act on, annotated.
        /// with a correlation id.</param>
        /// <param name="username">The user name to get the user default data.</param>
        /// <returns>A string containing the result user default data.</returns>
        public string GetUserDefault(WithCorrelationId<UserDefaultInput> data, string username)
        {
            var logger = _loggerFactory.Create(LoggingModel.UserDefault);
            logger.WriteCId(Level.Trace, $"Starting {nameof(UserService)} {nameof(GetUserDefault)}", data?.CorrelationId);

            string userDefaultOutput = null;

            if (data == null)
            {
                return userDefaultOutput;
            }

            if (data.Value == null)
            {
                var message = $"{nameof(data)} is null, cannot get groupstatus.";
                logger.WriteCId(Level.Error, message, data.CorrelationId);

                return userDefaultOutput;
            }

            var userDefaultRequest = data.Value;

            try
            {
                userDefaultOutput = _userDefaultStore.GetItem(username, userDefaultRequest.Property, userDefaultRequest.Group, data.CorrelationId);
            }
            catch (Exception ex)
            {
                var message = $"An error occurred while getting user default data: {ex.Message}";
                logger.WriteCId(Level.Error, message, data.CorrelationId);
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(UserService)} {nameof(SetUserLoggedIn)}", data?.CorrelationId);

            return userDefaultOutput;
        }

        /// <summary>
        /// Set the user default data by provided data.
        /// </summary>
        /// <param name="username">The username to set the value for.</param>
        /// <param name="property">The property to set the value for.</param>
        /// <param name="group">The group to set the value for.</param>
        /// <param name="value">The value to set the value for.</param>
        /// <param name="correlationId"></param>
        /// <returns>True if the value was set, false otherwise.</returns>
        public bool SaveUserDefault(string username, string property, string group, string value, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.UserDefault);
            logger.WriteCId(Level.Trace, $"Starting {nameof(UserService)} {nameof(SaveUserDefault)}", correlationId);

            bool userDefaultOutput = false;

            try
            {
                userDefaultOutput = _userDefaultStore.SetItem(username, property, group, value, correlationId);
            }
            catch (Exception ex)
            {
                var message = $"An error occurred while getting user default data: {ex.Message}";
                logger.WriteCId(Level.Error, message, correlationId);
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(UserService)} {nameof(SaveUserDefault)}", correlationId);

            return userDefaultOutput;
        }

        #endregion

    }
}
