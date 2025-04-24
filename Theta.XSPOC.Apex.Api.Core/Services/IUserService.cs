using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// A service which handles user operations.
    /// </summary>
    public interface IUserService
    {

        /// <summary>
        /// Determines if the user represented by the provided <paramref name="username"/> is a first time login.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="correlationId"></param>
        /// <returns>True if this is the first time logged in, false otherwise.</returns>
        bool IsUserFirstTimeLogin(string username, string correlationId);

        /// <summary>
        /// Sets the user represented by the provided <paramref name="username"/> as having logged in.
        /// </summary>
        /// <param name="username">The username to set the value for.</param>
        /// <param name="correlationId"></param>
        /// <returns>True if the value was set, false otherwise.</returns>
        bool SetUserLoggedIn(string username, string correlationId);

        /// <summary>
        /// Sets the user default represented by the provided data.
        /// </summary>
        /// <param name="username">The username to set the value for.</param>
        /// <param name="property">The property to set the value for.</param>
        /// <param name="group">The group to set the value for.</param>
        /// <param name="value">The value to set the value for.</param>
        /// <param name="correlationId"></param>
        /// <returns>True if the value was set, false otherwise.</returns>
        bool SaveUserDefault(string username, string property, string group, string value, string correlationId);

        /// <summary>
        /// Retrieves the user default data and maps it to the output model.
        /// </summary>
        /// <param name="data">The <seealso cref="UserDefaultInput"/> to act on, annotated.
        /// with a correlation id.</param>
        /// <param name="username">The user name to get the user default data.</param>
        /// <returns>A string containing the result user default data.</returns>
        string GetUserDefault(WithCorrelationId<UserDefaultInput> data, string username);

    }
}
