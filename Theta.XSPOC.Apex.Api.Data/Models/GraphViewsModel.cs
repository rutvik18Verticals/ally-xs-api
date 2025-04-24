using System;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The Graph View model.
    /// </summary>
    public class GraphViewsModel
    {

        /// <summary>
        /// Gets or sets the user id value.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the view id.
        /// </summary>
        public int ViewId { get; set; }

        /// <summary>
        /// Gets or sets the view name.
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// Gets or sets the start date of the default trene view.
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date of the default trene view.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets the default selected view.
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets the default Global.
        /// </summary>
        public bool IsGlobal { get; set; }

    }
}
