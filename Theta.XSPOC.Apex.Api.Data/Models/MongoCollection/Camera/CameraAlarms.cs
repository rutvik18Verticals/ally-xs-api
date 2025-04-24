using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Camera
{
    /// <summary>
    /// Represents a CameraAlarm collection.
    /// </summary>
    public class CameraAlarms : DocumentBase
    {
        /// <summary>
        /// Gets or sets the CameraID
        /// </summary>
        public int CameraID { get; set; }

        /// <summary>
        /// Gets or sets the AlarmType.
        /// </summary>
        public int AlarmType { get; set; }

        /// <summary>
        /// Gets or sets the AlarmType.
        /// </summary>
        public int Enabled { get; set; }

        /// <summary>
        /// Gets or sets the AlarmType.
        /// </summary>
        public int EmailOnAlarm { get; set; }

        /// <summary>
        /// Gets or sets the ContactListID
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string ContactListID { get; set; }

        /// <summary>
        /// Gets or sets the legacy Id of the Contacts.
        /// </summary>
        public IDictionary<string, string> LegacyId { get; set; }
    }
}
