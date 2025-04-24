using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Security
{
    /// <summary>
    /// This class defines the user preference MongoDB document.
    /// </summary>
    public class UserPreference : DocumentBase
    {

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the preference item.
        /// </summary>
        public Preference PreferenceItem { get; set; }

    }
}