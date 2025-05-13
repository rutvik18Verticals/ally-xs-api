using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{

    /// <summary>
    /// Represent the class for Dashboard Widget output response.
    /// </summary>
    public class DashboardWidgetDataOutput : CoreOutputBase
    {

        /// <summary>
        /// Gets or sets the list of <seealso cref="DashboardWidget"/> values.
        /// </summary>
        public List<DashboardWidget> Widgets { get; set; }
    }
}
