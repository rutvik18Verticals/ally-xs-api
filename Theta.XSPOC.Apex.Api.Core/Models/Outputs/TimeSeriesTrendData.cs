using System;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represent the class for data history trends axis data output response.
    /// </summary>
    public class TimeSeriesTrendData
    {

        /// <summary>
        /// Gets or sets the Asset Id.
        /// </summary>
        public Guid AssetId { get; set; }

        /// <summary>
        /// Gets or sets the Tag Name.
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the string value.
        /// </summary>
        public string StringValue { get; set; }

        /// <summary>
        /// Gets or sets the time recorded.
        /// </summary>
        public DateTime TimeRecorded { get; set; }

        /// <summary>
        /// Gets or sets the time written.
        /// </summary>
        public DateTime TimeWritten { get; set; }

    }
}
