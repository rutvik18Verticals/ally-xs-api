using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection
{
    /// <summary>
    /// A customer base class with properties that are common to all Document models tied to a customer.
    /// </summary>
    public class CustomerDocumentBase : DocumentBase
    {

        /// <summary>
        /// Gets or sets the customer Id, which is a GUID.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string CustomerId { get; set; }

    }
}