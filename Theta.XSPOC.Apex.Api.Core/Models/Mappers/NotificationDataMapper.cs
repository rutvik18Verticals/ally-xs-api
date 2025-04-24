using System;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Models.Mappers
{
    /// <summary>
    /// The mapper for the notification data.
    /// </summary>
    public class NotificationDataMapper
    {

        #region Public Methods

        /// <summary>
        /// Maps the <paramref name="entity"/> to a <seealso cref="NotificationsModel"/> domain object.
        /// </summary>
        /// <param name="entity">A <seealso cref="NotificationsModel"/> domain object.</param>
        /// <returns>A <seealso cref="NotificationData"/> representing the provided <paramref name="entity"/> 
        /// in the domain.</returns>
        public static NotificationData MapToDomainObject(NotificationsModel entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var result = new NotificationData()
            {
                EventId = entity.EventId.ToString(),
                NodeId = entity.NodeId,
                Date = entity.Date,
                EventTypeId = entity.EventTypeId,
                EventTypeName = entity.EventTypeName,
                Note = entity.Note,
                Status = entity.Status,
                TransactionId = entity.TransactionId,
                UserId = entity.UserId
            };

            return result;
        }

        #endregion

    }
}
