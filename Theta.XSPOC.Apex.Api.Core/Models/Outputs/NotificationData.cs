using System;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represent the class for notification data
    /// </summary>
    public class NotificationData
    {

        /// <summary>
        /// The event id.
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// The node id.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// The event type id.
        /// </summary>
        public int EventTypeId { get; set; }

        /// <summary>
        /// The date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The note.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// The user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The transaction id.
        /// </summary>
        public int TransactionId { get; set; }

        /// <summary>
        /// The event type name.
        /// </summary>
        public string EventTypeName { get; set; }

    }
}
