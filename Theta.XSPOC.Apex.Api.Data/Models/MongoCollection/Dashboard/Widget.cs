using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Dashboard
{
    /// <summary>
    /// WidgetCoordinate class.
    /// </summary>
    public class Widget
    {

        /// <summary>
        /// Gets or sets the widget layout for the default dashboard.
        /// </summary>
        public List<WidgetLayout> Default { get; set; }

        /// <summary>
        /// Gets or sets the widget layout for the stacked dashboard.
        /// </summary>
        public List<WidgetLayout> Stacked { get; set; }

    }
}
