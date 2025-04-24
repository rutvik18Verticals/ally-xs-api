using MongoDB.Bson.Serialization.Attributes;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter
{
    /// <summary>
    /// An abstract class used with specific parameters to expand details.
    /// </summary>
    [BsonKnownTypes(typeof(ParameterDetails), typeof(FacilityTagDetails))]
    public abstract class ParameterDocumentBase
    {

    }
}
