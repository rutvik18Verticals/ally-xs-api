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
        /// Gets or sets the chart 1 tree node trend type ids.
        /// </summary>
        public string Chart1TrendTypes { get; set; }

        /// <summary>
        /// Gets or sets the chart 1 trend names.
        /// </summary>
        public string Chart1TrendNames { get; set; }

        /// <summary>
        /// Gets or sets the chart 1 trend addresses.
        /// </summary>
        public string Chart1TrendAddresses { get; set; }

        /// <summary>
        /// Gets or sets the chart 2 tree node trend type ids.
        /// </summary>
        public string Chart2TrendTypes { get; set; }

        /// <summary>
        /// Gets or sets the chart 2 trend names.
        /// </summary>
        public string Chart2TrendNames { get; set; }

        /// <summary>
        /// Gets or sets the chart 2 trend addresses.
        /// </summary>
        public string Chart2TrendAddresses { get; set; }

        /// <summary>
        /// Gets or sets the chart 3 tree node trend type ids.
        /// </summary>
        public string Chart3TrendTypes { get; set; }

        /// <summary>
        /// Gets or sets the chart 3 trend names.
        /// </summary>
        public string Chart3TrendNames { get; set; }

        /// <summary>
        /// Gets or sets the chart 3 trend addresses.
        /// </summary>
        public string Chart3TrendAddresses { get; set; }

        /// <summary>
        /// Gets or sets the chart 4 tree node trend type ids.
        /// </summary>
        public string Chart4TrendTypes { get; set; }

        /// <summary>
        /// Gets or sets the chart 4 trend names.
        /// </summary>
        public string Chart4TrendNames { get; set; }

        /// <summary>
        /// Gets or sets the chart 4 trend addresses.
        /// </summary>
        public string Chart4TrendAddresses { get; set; }

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
