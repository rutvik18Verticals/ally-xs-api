using System;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class to represent model for EventTrendDataModel.
    /// </summary>
    public class EventTrendDataModel
    {

        /// <summary>
        /// Gets and sets the Date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets and sets the EventTypeId.
        /// </summary>
        public int EventTypeId { get; set; }

        /// <summary>
        /// Gets and sets the Note.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Gets and sets the UserId.
        /// </summary>
        public string UserId { get; set; }

    }
}
