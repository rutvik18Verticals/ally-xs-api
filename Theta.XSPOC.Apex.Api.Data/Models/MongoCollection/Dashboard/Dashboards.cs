using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Dashboard
{
    /// <summary>
    /// 
    /// </summary>
    public class Dashboards : DocumentBase
    {

        /// <summary>
        /// Gets or sets the userId.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets a boolean specifying the value is default.
        /// </summary>
        public bool? Default { get; set; }

        /// <summary>
        /// Gets or sets the name of the dashboard
        /// </summary>
        public string Dashboard { get; set; }

        /// <summary>
        /// Gets or sets a boolean specifying the value is editable.
        /// </summary>
        public bool Editable { get; set; }

        /// <summary>
        /// Gets or sets Widget Coordinates.
        /// </summary>  
        public Widget Widgets { get; set; }
    }
}
