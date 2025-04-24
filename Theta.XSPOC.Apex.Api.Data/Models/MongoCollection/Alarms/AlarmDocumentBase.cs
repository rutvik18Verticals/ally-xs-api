using MongoDB.Bson.Serialization.Attributes;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Alarms
{
    /// <summary>
    /// This is a base class for the Alarm sub document.
    /// </summary>
    [BsonKnownTypes(typeof(FacilityTagAlarm), typeof(HostAlarm), typeof(RTU))]
    public abstract class AlarmDocumentBase
    {

    }
}
