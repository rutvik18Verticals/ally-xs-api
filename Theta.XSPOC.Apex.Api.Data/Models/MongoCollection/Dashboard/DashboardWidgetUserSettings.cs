using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Dashboard
{
    /// <summary>
    /// DashboardWidgets class.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class DashboardWidgetUserSettings : DocumentBase
    {
        /// <summary>
        /// Gets or sets if widget is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets if the dashboard name.
        /// </summary>
        public string DashboardName { get; set; }

        /// <summary>
        /// Gets or sets the widget full name.
        /// </summary>
        public string WidgetName { get; set; }

        /// <summary>
        /// Gets or sets the lift type.
        /// </summary>
        public string LiftType { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets property name of the settings.
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets WidgetProperties.
        /// </summary>
        public WidgetType PropertyValue { get; set; }
    }
}
