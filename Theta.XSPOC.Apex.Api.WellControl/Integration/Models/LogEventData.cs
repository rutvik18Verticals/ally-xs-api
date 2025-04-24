using System;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.WellControl.Integration.Models
{
    /// <summary>
    /// Represents the log event data.
    /// </summary>
    public class LogEventData
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the event type.
        /// </summary>
        public EventType EventType { get; set; }

        /// <summary>
        /// Gets or sets the event type name.
        /// </summary>
        public string EventTypeName { get; set; }

        /// <summary>
        /// Gets or sets the note.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime? Date { get; set; }

    }
}
