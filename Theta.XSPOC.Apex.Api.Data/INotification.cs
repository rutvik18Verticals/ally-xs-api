using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface that represents events.
    /// </summary>
    public interface INotification
    {

        /// <summary>
        /// Get the notifications by asset id and notification type id.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="notificationTypeId">The notification type.</param>
        /// <param name="correlationId"></param>
        /// <returns>The List of <seealso cref="NotificationsModel"/>.</returns>
        IList<NotificationsModel> GetEventsByAssetId(string assetId, int? notificationTypeId, string correlationId);

        /// <summary>
        /// Get the notifications by asset group name and notification type id.
        /// </summary>
        /// <param name="assetGroupName">The asset group name.</param>
        /// <param name="notificationTypeId">The notification type.</param>
        /// <param name="correlationId"></param>
        /// <returns>The List<seealso cref="IList{NotificationsModel}"/>.</returns>
        IList<NotificationsModel> GetEventsByAssetGroupName(string assetGroupName, int? notificationTypeId, string correlationId);

        /// <summary>
        /// Get the notifications by asset id and notification type id.
        /// </summary>
        /// <param name="assetId">The asset id/node id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{NotificationsTypesModel}"/>.</returns>
        IList<NotificationsTypesModel> GetEventsGroupsByAssetId(string assetId, string correlationId);

        /// <summary>
        /// Get the notifications by asset group name and notification type id.
        /// </summary>
        /// <param name="assetGroupName">The asset group name.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{NotificationsTypesModel}"/>.</returns>
        IList<NotificationsTypesModel> GetEventsGroupsByAssetGroupName(string assetGroupName, string correlationId);

        /// <summary>
        /// Get the event type name by event type id.
        /// </summary>
        /// <param name="eventTypeId">The event type id to get the event name by.</param>
        /// <param name="correlationId"></param>
        /// <returns>The event type name as a string.</returns>
        string GetEventTypeName(int eventTypeId, string correlationId);

    }
}
