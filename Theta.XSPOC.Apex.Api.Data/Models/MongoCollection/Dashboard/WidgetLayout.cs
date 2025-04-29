namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Dashboard
{
    /// <summary>
    /// WidgetLayout class.
    /// </summary>
    public class WidgetLayout
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
        public Layout Layout { get; set; }
    }
}