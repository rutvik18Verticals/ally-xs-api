using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Exceptions
{

    /// <summary>
    /// Represent Exceptions collection 
    /// </summary>
    public class Exceptions : DocumentBase
    {

        /// <summary>
        /// Gets or sets the AssetId.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string AssetId { get; set; }

        /// <summary>
        /// Gets or sets the ExceptionGroupName
        /// </summary>
        public string ExceptionGroupName { get; set; }

        /// <summary>
        /// Gets or sets Priority
        /// </summary>
        public int? Priority { get; set; }

    }
}
