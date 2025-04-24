using MongoDB.Bson.Serialization.Attributes;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection
{
    /// <summary>
    /// This is the base class for equipment specifications.
    /// </summary>
    [BsonKnownTypes(typeof(POCTypeSpecification))]
    public class EquipmentSpecification
    {
    }
}
