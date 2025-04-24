using System;

namespace Theta.XSPOC.Apex.Api.Core.Models.Inputs
{
    /// <summary>
    /// Represent the class for data history trends input data.
    /// </summary>
    public class TrendIDataInput
    {

        /// <summary>
        /// Gets or sets the asset id.
        /// </summary>
        public Guid AssetId { get; set; }

        /// <summary>
        /// Gets or sets the customer id.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the poc-type.
        /// </summary>
        public string POCType { get; set; }

        /// <summary>
        /// Gets or sets the chart 1 tree node type id.
        /// </summary>
        public string Chart1Type { get; set; }

        /// <summary>
        /// Gets or sets the chart 1 item id.
        /// </summary>
        public string Chart1ItemId { get; set; }

        /// <summary>
        /// Gets or sets the chart 2 tree node type id.
        /// </summary>
        public string Chart2Type { get; set; }

        /// <summary>
        /// Gets or sets the chart 2 item id.
        /// </summary>
        public string Chart2ItemId { get; set; }

        /// <summary>
        /// Gets or sets the chart 3 tree node type id.
        /// </summary>
        public string Chart3Type { get; set; }

        /// <summary>
        /// Gets or sets the chart 3 item id.
        /// </summary>
        public string Chart3ItemId { get; set; }

        /// <summary>
        /// Gets or sets the chart 4 tree node type id.
        /// </summary>
        public string Chart4Type { get; set; }

        /// <summary>
        /// Gets or sets the chart 4 item id.
        /// </summary>
        public string Chart4ItemId { get; set; }

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        public string EndDate { get; set; }

        /// <summary>
        /// Gets or sets the value indicating chart is grouped/overlay.
        /// </summary>
        public bool IsOverlay { get; set; }

        /// <summary>
        /// Gets or sets the group name.
        /// </summary>
        public string GroupName { get; set; }

    }
}
