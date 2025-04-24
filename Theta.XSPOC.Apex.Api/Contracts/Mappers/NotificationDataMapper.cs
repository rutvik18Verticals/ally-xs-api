using System;
using System.Linq;
using Theta.XSPOC.Apex.Api.Contracts.Responses;
using Theta.XSPOC.Apex.Api.Contracts.Responses.Values;

namespace Theta.XSPOC.Apex.Api.Contracts.Mappers
{
    /// <summary>
    /// Maps Core.Models.Outputs.NotificationData to NotificationResponse object
    /// </summary>
    public static class NotificationDataMapper
    {

        /// <summary>
        /// Maps the <seealso cref="Core.Models.Outputs.NotificationDataOutput"/> core model to
        /// <seealso cref="NotificationResponse"/> object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="coreModel">The <seealso cref="Core.Models.Outputs.NotificationDataOutput"/> object</param>
        /// <returns>The <seealso cref="NotificationResponse"/></returns>
        public static NotificationResponse Map(string correlationId, Core.Models.Outputs.NotificationDataOutput coreModel)
        {
            if (coreModel == null || coreModel.Values == null || coreModel.Values.Count == 0)
            {
                return null;
            }

            var values = coreModel.Values.Select(x => new NotificationData()
            {
                EventId = x.EventId,
                EventTypeName = x.EventTypeName,
                NodeId = x.NodeId,
                EventTypeId = x.EventTypeId,
                Note = x.Note,
                TransactionId = x.TransactionId,
                UserId = x.UserId,
                Status = x.Status,
                Date = x.Date,
            }).ToList();

            return new NotificationResponse()
            {
                DateCreated = DateTime.UtcNow,
                Id = correlationId,
                Values = values
            };
        }

    }
}
