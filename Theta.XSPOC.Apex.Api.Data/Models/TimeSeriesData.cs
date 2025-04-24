using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The historical trend data model. 
    /// </summary>
    public class TimeSeriesData
    {
        /// <summary>
        /// Gets or set the AssetId
        /// </summary>
        public string AssetId { get; set; }

        /// <summary>
        /// Gets or set the POCTypeId
        /// </summary>
        public string POCTypeId { get; set; }

        /// <summary>
        /// Gets or set the ChannelIds
        /// </summary>
        public List<string> ChannelIds { get; set; }

        /// <summary>
        /// Gets or set the Values.
        /// </summary>
        public List<double?> Values { get; set; }

        /// <summary>
        /// Gets or set the Timestamp.
        /// </summary>
        public string Timestamp { get; set; }

        /// <summary>
        /// Gets or set the TotalCount.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets or set the TotalCount.
        /// </summary>
        public int TotalPages { get; set; }

    }
}
