using System;

namespace Theta.XSPOC.Apex.Api.WellControl.Integration.Models
{
    /// <summary>
    /// The contract to log events.
    /// </summary>
    public class EventLogEventPayload
    {

        /// <summary>
        /// The node id.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// The event type.
        /// </summary>
        public int EventTypeId { get; set; }

        /// <summary>
        /// The description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The date.
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// The transaction id.
        /// </summary>
        public int? TransactionId { get; set; }

    }
}
