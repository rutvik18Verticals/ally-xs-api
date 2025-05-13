using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.Dashboard
{
    /// <summary>
    /// Class for defining the data model of a dashboard widget mongo collection.
    /// </summary>
    public class DashboardWidgetDataModel
    {

        /// <summary>
        /// Gets or sets the name of the dashboard
        /// </summary>
        public string DashboardName { get; set; }

        /// <summary>
        /// Gets or sets a boolean specifying the value is editable.
        /// </summary>
        public bool Editable { get; set; }

        /// <summary>
        /// Gets or sets Widget Coordinates.
        /// </summary>  
        public List<WidgetLayoutDataModel> Widgets { get; set; }
    }

    /// <summary>
    /// WidgetLayout class.
    /// </summary>
    public class WidgetLayoutDataModel
    {
        /// <summary>
        /// Gets or sets flag for displaying the widget.
        /// </summary>
        public bool Show { get; set; }

        /// <summary>
        /// Gets or sets the widget Key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the widget Layout.
        /// </summary>
        public LayoutDataModel Layout { get; set; }

        /// <summary>
        /// Gets or sets the widget Layout.
        /// </summary>
        public WidgetPropertyDataModel WidgetProperty { get; set; }
    }

    /// <summary>
    /// Layout class.
    /// </summary>
    public class LayoutDataModel
    {

        /// <summary>
        /// Gets or sets the Lg
        /// </summary>
        public LgDataModel Lg { get; set; }
    }

    /// <summary>
    /// Class for defining the layout coordinates of a widget in a dashboard.
    /// </summary>
    public class LgDataModel
    {
        /// <summary>
        /// Gets or sets the x coordinate.
        /// </summary>
        public float? X { get; set; }

        /// <summary>
        /// Gets or sets the y coordinate.
        /// </summary>
        public float Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the widget.
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the widget.
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Gets or sets the min height of the widget.
        /// </summary>
        public float MinH { get; set; }

        /// <summary>
        /// Gets or sets the min width of the widget.
        /// </summary>
        public float MinW { get; set; }
    }

    /// <summary>
    /// WidgetProperty class.
    /// </summary>
    public class WidgetPropertyDataModel
    {
        /// <summary>
        /// Gets or sets the dashboard name.
        /// </summary>
        public string Dashboard { get; set; }

        /// <summary>
        /// Gets or sets if the widget is expandable.
        /// </summary>
        public bool Expandable { get; set; }

        /// <summary>
        /// Gets or sets the widget full name.
        /// </summary>
        public string Widget { get; set; }

        /// <summary>
        /// Gets or sets the widget key.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets or sets the description of the widget.
        /// </summary>
        public string Description { get; set; }

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
        /// Gets or sets WidgetProperties.
        /// </summary>
        public object WidgetProperties { get; set; }

        /// <summary>
        /// Gets or sets if the widget property is default/not.
        /// </summary>
        public bool IsDefault { get; set; }

    }
}
