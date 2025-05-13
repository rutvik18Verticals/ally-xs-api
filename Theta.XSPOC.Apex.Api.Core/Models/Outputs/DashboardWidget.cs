using Theta.XSPOC.Apex.Api.Data.Models.Dashboard;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Class for dashboard widgets.
    /// </summary>
    public class DashboardWidget
    {
        /// <summary>
        /// Gets or sets the name of the dashboard.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets flag to show/hide the widget.
        /// </summary>
        public bool Show { get; set; }

        /// <summary>
        /// Gets or sets the widget layout.
        /// </summary>
        public LayoutDataModel Layout { get; set; }

        /// <summary>
        /// Gets or sets the widget property.
        /// </summary>
        public WidgetPropertyDataModel WidgetProperty { get; set; }
    }
}
