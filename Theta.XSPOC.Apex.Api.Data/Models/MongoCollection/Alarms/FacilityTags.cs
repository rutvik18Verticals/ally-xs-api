using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Alarms
{
    /// <summary>
    /// FacilityTag propeties representation. Referenced in Alarm Configuration as sub-document type. 
    /// </summary>
    public class FacilityTagAlarm : AlarmDocumentBase
    {
        /// <summary>
        /// Gets or sets the group node identifier.
        /// </summary>
        public string GroupNodeId { get; set; }

        /// <summary>
        /// Gets or sets the alarm state.
        /// </summary>
        public int? AlarmState { get; set; }

        /// <summary>
        /// Gets or sets the number of consecutive alarms.
        /// </summary>
        public int? NumConsecAlarms { get; set; }

        /// <summary>
        /// Gets or sets the allowed number of consecutive alarms.
        /// </summary>
        public int? NumConsecAlarmsAllowed { get; set; }

        /// <summary>
        /// Gets or sets the alarm action identifier.
        /// </summary>
        public int? AlarmAction { get; set; }

        /// <summary>
        /// Gets or sets the responder list identifier, nullable if not set.
        /// </summary>
        public int? ResponderListId { get; set; }
        //TODO : Need to reference as objectId from Contacts after contact migration

        /// <summary>
        /// Gets or sets the voice text high.
        /// </summary>
        public string VoiceTextHi { get; set; }

        /// <summary>
        /// Gets or sets the voice text low.
        /// </summary>
        public string VoiceTextLo { get; set; }

        /// <summary>
        /// Gets or sets the clear voice text.
        /// </summary>
        public string VoiceTextClear { get; set; }

        /// <summary>
        /// Gets or sets the high alarm text.
        /// </summary>
        public string AlarmTextHi { get; set; }

        /// <summary>
        /// Gets or sets the low alarm text.
        /// </summary>
        public string AlarmTextLo { get; set; }

        /// <summary>
        /// Gets or sets the clear alarm text.
        /// </summary>
        public string AlarmTextClear { get; set; }

        /// <summary>
        /// Gets or sets the contact list identifier, nullable if not set.
        /// </summary>
        public int? ContactListId { get; set; }
        //TODO : Need to reference as objectId from Contacts after contact migration

        /// <summary>
        /// Gets or sets the name of the well group.
        /// </summary>
        public string WellGroupName { get; set; }

        /// <summary>
        /// Gets or sets the deadband value.
        /// </summary>
        public double? Deadband { get; set; }

        /// <summary>
        /// Gets or sets the state identifier.
        /// </summary>
        public Lookup.Lookup StateID { get; set; }

        /// <summary>
        /// Gets or sets the node identifier.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string NodeId { get; set; }

    }

}
