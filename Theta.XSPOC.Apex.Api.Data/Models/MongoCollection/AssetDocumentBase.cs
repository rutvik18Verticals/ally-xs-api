using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection
{
    /// <summary>
    /// An Asset base class with properties that are common to all Document models tied to a Asset.
    /// </summary>
    public class AssetDocumentBase : CustomerDocumentBase
    {
        /// <summary>
        /// Gets or sets the Asset Id, which is a GUID.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string AssetId { get; set; }
    }
}
