using MongoDB.Bson.Serialization.Attributes;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Dashboard
{
    /// <summary>
    /// DashboardWidgets class.
    /// </summary>
    [BsonIgnoreExtraElements]
    public class DashboardWidgets : DocumentBase
    {
        /// <summary>
        /// Gets or sets the flag if widget is deleted.
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Gets or sets if the widget is expandable.
        /// </summary>
        public bool Expandable { get; set; }

        /// <summary>
        /// Gets or sets if the dashboard name.
        /// </summary>
        public string Dashboard { get; set; }

        /// <summary>
        /// Gets or sets the widget full name.
        /// </summary>
        public string Widget { get; set; }

        /// <summary>
        /// Gets or sets the widget description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the widget key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the external api uri.
        /// </summary>
        public string ExternalUri { get; set; }

        /// <summary>
        /// Gets or sets http method of the api call.
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        /// Gets or sets http body of the api call.
        /// </summary>
        public string HttpBody { get; set; }

        /// <summary>
        /// Gets or sets the WidgetType.
        /// </summary>
        public WidgetType WidgetProperties { get; set; }
    }
}
