using System;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the data history trends response.
    /// </summary>
    public class DataHistoryDefaultTrendData
    {

        /// <summary>
        /// Gets or sets the view id.
        /// </summary>
        public int ViewId { get; set; }

        /// <summary>
        /// Gets or sets the view name.
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the default selected view.
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets the Global view.
        /// </summary>
        public bool IsGlobal { get; set; }

    }
}
