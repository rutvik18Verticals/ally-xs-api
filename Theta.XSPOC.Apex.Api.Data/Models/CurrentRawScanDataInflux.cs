using System;
namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// The Current Row Scan Data model. 
    /// </summary>
    public class CurrentRawScanDataInflux
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the register address.
        /// </summary>
        public int Address { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public float? Value { get; set; }

        /// <summary>
        /// Gets or sets the string value.
        /// </summary>
        public string StringValue { get; set; }

        /// <summary>
        /// Gets or sets the date and time updated.
        /// </summary>
        public DateTime? DateTimeUpdated { get; set; }

    }

}
