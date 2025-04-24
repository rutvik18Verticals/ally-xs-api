using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Interface for processing notifications.
    /// </summary>
    public interface INotificationProcessingService
    {

        /// <summary>
        /// Processes the provided notification request and generates notifications based on that data.
        /// </summary>
        /// <param name="data">The <seealso cref="NotificationsInput"/> to act on, annotated. 
        /// with a correlation id.</param>
        /// <returns>The list of.<seealso cref="NotificationDataOutput"/></returns>
        NotificationDataOutput GetNotifications(WithCorrelationId<NotificationsInput> data);

        /// <summary>
        /// Processes the provided notification types request and generates notifications based on that data.
        /// </summary>
        /// <param name="data">The <seealso cref="NotificationsInput"/> to act on, annotated.
        /// with a correlation id.</param>
        /// <returns>The list of.<seealso cref="NotificationTypesDataOutput"/></returns>
        NotificationTypesDataOutput GetNotificationsTypes(WithCorrelationId<NotificationsInput> data);

    }
}
