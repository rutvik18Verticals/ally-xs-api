using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// This class to store tblEvents in the Mongo DB Store. In particular, it represents the
    /// EventModel model that would be saved to Mongo as a document.
    /// </summary>
    public class EventModel
    {

        /// <summary>
        /// The [BsonId] in order to enforce this as the primary key for the document.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the event id.
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the event type id.
        /// </summary>
        public int EventTypeId { get; set; }

        /// <summary>
        /// Gets or sets the event type name.
        /// </summary>
        public string EventTypeName { get; set; }

        /// <summary>
        /// Gets or sets the event date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the note.
        /// </summary>
        public string Note { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        public short? TransactionId { get; set; }

    }
}