using System;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the current raw scan data model.
    /// </summary>
    public class CurrentRawScanDataInfluxModel
    {

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public float? Value { get; set; }

        /// <summary>
        /// Gets or sets the date and time updated.
        /// </summary>
        public DateTime? DateTimeUpdated { get; set; }

        /// <summary>
        /// Gets or sets the POCType
        /// </summary>
        public string POCType { get; set; }

        /// <summary>
        /// Gets or sets the ChannelId
        /// </summary>
        public string ChannelId { get; set; }

    }
}
