using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection
{
    /// <summary>
    /// A base class with properties that are common to all Document models.
    /// </summary>
    public class DocumentBase
    {

        /// <summary>
        /// Every object should have an Id. As per Microsoft documentation, this is:
        /// 1. Required for mapping the Common Language Runtime (CLR) object to the MongoDB collection.
        /// 2. Annotated with [BsonId] in order to enforce this as the primary key for the document.
        /// 3. Annotated with [BsonRepresentation(BsonType.ObjectId)] so Mongo will handle converting from a string,
        ///    which is convenient for GUIDs and such, to an ObjectId.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// The user id that created the document.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// The date the document was created.
        /// </summary>
        public DateTime? CreatedOnDate { get; set; }

        /// <summary>
        /// The user id that last modified the document.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string LastModifiedBy { get; set; }

        /// <summary>
        /// The date the document was last modified.
        /// </summary>
        public DateTime? LastModifiedDate { get; set; }

    }
}