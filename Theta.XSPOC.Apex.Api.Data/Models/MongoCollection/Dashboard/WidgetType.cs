using MongoDB.Bson.Serialization.Attributes;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Dashboard
{
    /// <summary>
    /// This is a base class for the lookup sub document.
    /// </summary>
    [BsonKnownTypes(typeof(WellTest), typeof(TimeSeriesChart))]
    public class WidgetType
    {
    }
}
