using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents the filters for downtime data in InfluxDB.
    /// </summary>
    public class DowntimeFiltersWithChannelIdInflux
    {

        /// <summary>
        /// Gets or sets the type of POC (Point of Control).
        /// </summary>
        public string POCType { get; set; }

        /// <summary>
        /// Gets or sets the list of asset IDs.
        /// </summary>
        public IList<Guid> AssetIds { get; set; }

        /// <summary>
        /// Gets or sets the list of customer IDs.
        /// </summary>
        public IList<Guid> CustomerIds { get; set; }

        /// <summary>
        /// Gets or sets the list of channelIds.
        /// </summary>
        public IList<string> ChannelIds { get; set; }

    }
}
