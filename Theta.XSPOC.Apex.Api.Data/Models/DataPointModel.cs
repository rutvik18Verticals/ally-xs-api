using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The historical trend data model. 
    /// </summary>
    public class DataPointModel
    {
        /// <summary>
        /// Gets or set the time.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Gets or set the value.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Gets or set the Trend Name.
        /// </summary>
        public string TrendName { get; set; }

        /// <summary>
        /// Gets or set the AssetId
        /// </summary>
        public string AssetId { get; set; }

        /// <summary>
        /// Gets or set the AssetId
        /// </summary>
        public string ChannelId { get; set; }

        /// <summary>
        /// Gets or set the POC Type Id.
        /// </summary>
        public string POCTypeId { get; set; }

        /// <summary>
        /// Gets or set the Total Count
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or set the Total Count
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Gets or set the value.
        /// </summary>
        public double ValueOfTimeSeries { get; set; }

        /// <summary>
        /// Gets or set the time.
        /// </summary>
        public string TimeOfTimeSeries { get; set; }

        /// <summary>
        /// Gets or set the value.
        /// </summary>
        public IDictionary<string, string> ColumnValues { get; set; }
    }
}
