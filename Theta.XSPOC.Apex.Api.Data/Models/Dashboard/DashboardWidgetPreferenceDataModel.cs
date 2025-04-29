

using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Dashboard;

namespace Theta.XSPOC.Apex.Api.Data.Models.Dashboard
{
    /// <summary>
    /// Class for defining the data model of a dashboard widget mongo collection.
    /// </summary>
    public class DashboardWidgetPreferenceDataModel
    {
        /// <summary>
        /// Gets or sets if the dashboard name.
        /// </summary>
        public string DashboardName { get; set; }

        /// <summary>
        /// Gets or sets the widget full name.
        /// </summary>
        public string WidgetName { get; set; }

        /// <summary>
        /// Gets or sets WidgetProperties.
        /// </summary>
        public WidgetType Preferences { get; set; }
    }
}
