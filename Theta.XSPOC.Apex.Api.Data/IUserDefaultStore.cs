using System.Collections.Generic;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface that represents user defaults store.
    /// </summary>
    public interface IUserDefaultStore
    {

        /// <summary>
        /// Gets the default item for the provided <paramref name="username"/>, <paramref name="property"/>, and <paramref name="group"/>.
        /// </summary>
        /// <param name="username">The username to get the default for.</param>
        /// <param name="property">The property to get the default for.</param>
        /// <param name="group">The group to get the default for.</param>
        /// <param name="correlationId">The correlation GUID.</param>
        /// <returns>The <seealso cref="string"/> value of the default</returns>
        string GetItem(string username, string property, string group, string correlationId);

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
        bool SetItem(string username, string property, string group, string value, string correlationId);

        /// <summary>
        /// Gets a user default item for the specified <paramref name="userId"/>, <paramref name="property"/>, and
        /// <paramref name="defaultGroup"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="defaultGroup">The default group.</param>
        /// <param name="property">The property. Optional parameter.</param>
        /// <param name="correlationId">The correlation GUID.</param>
        /// <returns></returns>
        Task<UserDefaultItem> GetDefaultItem(string userId, string defaultGroup, string correlationId, string property = null);

        /// <summary>
        /// Gets a sorted dictionary of user default items for the specified <paramref name="userId"/> and
        /// <paramref name="defaultGroup"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="defaultGroup">The default group.</param>
        /// <param name="correlationId">The correlation GUID.</param>
        /// <returns></returns>
        Task<IDictionary<string, UserDefaultItem>> GetDefaultItemByGroup(string userId,
            string defaultGroup, string correlationId);

    }
}
