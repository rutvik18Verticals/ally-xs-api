
namespace Theta.XSPOC.Apex.Api.Core.Models.Inputs
{
    /// <summary>
    /// Class for defining the input to update the User Preference for dashboard widget.
    /// </summary>
    public class DashboardWidgetUserPreferencesInput
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
        public object Preferences { get; set; }

        /// <summary>
        /// Gets or sets the property type to be updated.
        /// </summary>
        public string PropertyType { get; set; }
    }
}
