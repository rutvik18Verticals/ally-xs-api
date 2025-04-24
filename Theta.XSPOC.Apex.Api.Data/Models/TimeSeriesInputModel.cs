using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The historical trend data model. 
    /// </summary>
    public class TimeSeriesInputModel
    {
        /// <summary>
        /// Gets or set the AssetId
        /// </summary>
        public Guid AssetId { get; set; }

        /// <summary>
        /// Gets or set the POCTypeId
        /// </summary>
        public string POCTypeId { get; set; }

        /// <summary>
        /// Gets or set the ChannelIds
        /// </summary>
        public List<string> ChannelIds { get; set; }

        /// <summary>
        /// Gets or set the ChannelIds
        /// </summary>
        public List<string> StandardParameterTypes { get; set; }

    }
}
