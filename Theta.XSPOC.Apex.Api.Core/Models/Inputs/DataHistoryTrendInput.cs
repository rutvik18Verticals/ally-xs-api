using System;

namespace Theta.XSPOC.Apex.Api.Core.Models.Inputs
{
    /// <summary>
    /// Represent the class for data history trends input data.
    /// </summary>
    public class DataHistoryTrendInput
    {

        /// <summary>
        /// Gets or sets the asset id.
        /// </summary>
        public Guid AssetId { get; set; }

        /// <summary>
        /// Gets or sets the group name.
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Gets or sets the type id.
        /// </summary>
        public string TypeId { get; set; }

        /// <summary>
        /// Gets or sets the item id.
        /// </summary>
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// Gets or sets the customer id.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the poc-type.
        /// </summary>
        public string POCType { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the param standard type.
        /// </summary>
        public string ParamStandardType { get; set; }

        /// <summary>
        /// Gets or sets the default view id.
        /// </summary>
        public string ViewId { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets the value indicating chart is grouped/overlay.
        /// </summary>
        public bool IsOverlay { get; set; }

    }
}
