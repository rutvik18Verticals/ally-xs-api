
namespace Theta.XSPOC.Apex.Api.Core.Models.Inputs
{
    /// <summary>
    /// Class for defining the input for resetting user preferences for dashboard widgets.
    /// </summary>
    public class DashboardWidgetResetUserPreferencesInput
    {
        /// <summary>
        /// Gets or sets if the dashboard name.
        /// </summary>
        public string DashboardName { get; set; }

        /// <summary>
        /// Gets or sets the widget full name.
        /// </summary>
        public string WidgetName { get; set; }

    }
}
