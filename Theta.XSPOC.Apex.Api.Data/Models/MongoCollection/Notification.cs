using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection
{
    /// <summary>
    /// Represents a notification master.
    /// </summary>
    public class Notification : DocumentBase
    {
        /// <summary>
        /// Gets or sets the legacy Id of the Notifications.
        /// </summary>
        public IDictionary<string, string> LegacyId { get; set; }

        /// <summary>
        /// Gets or sets AlaramId Identifier reference to Alarm Configuration
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string AlaramId { get; set; }

        /// <summary>
        /// Gets or sets the TextGroup Id
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public IList<string> TextGroupId { get; set; }

        /// <summary>
        /// Gets or sets the Text Enabled.
        /// </summary>
        public bool? TextEnabled { get; set; }

        /// <summary>
        /// Gets or sets the Text Lo Message.
        /// </summary>
        public string TextLoMessage { get; set; }

        /// <summary>
        /// Gets or sets the Text Hi Message.
        /// </summary>
        public string TextHiMessage { get; set; }

        /// <summary>
        /// Gets or sets the Text Clear Message.
        /// </summary>
        public string TextClearMessage { get; set; }

        /// <summary>
        /// Gets or sets the Push Group Id.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public IList<string> PushGroupId { get; set; }

        /// <summary>
        /// Gets or sets the Push Enabled.
        /// </summary>
        public bool? PushEnabled { get; set; }

        /// <summary>
        /// Gets or sets the Push Lo Message.
        /// </summary>
        public string PushLoMessage { get; set; }

        /// <summary>
        /// Gets or sets a the  Push Hi Message.
        /// </summary>
        public string PushHiMessage { get; set; }

        /// <summary>
        /// Gets or sets the Push Clear Message.
        /// </summary>
        public string PushClearMessage { get; set; }
    }
}
