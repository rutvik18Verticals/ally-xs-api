using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Security
{
    /// <summary>
    /// This class defines the user role MongoDB document.
    /// </summary>
    public class UserRole : DocumentBase
    {

        /// <summary>
        /// Gets or sets role ids.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public IList<string> RoleIds { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

    }
}