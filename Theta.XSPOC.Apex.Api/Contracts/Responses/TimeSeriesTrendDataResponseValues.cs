using System;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// Represent the class for data history trends axis data output response.
    /// </summary>
    public class TimeSeriesTrendDataResponseValues
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
        /// Gets or sets the Tag Id.
        /// </summary>
        public string TagId { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public decimal? Value { get; set; }

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

    /// <summary>
    /// Represent the class for data history trends axis data output response.
    /// </summary>
    public class AllyTimeSeriesTrendDataResponseValues
    {

        /// <summary>
        /// Gets or sets the Asset Id.
        /// </summary>
        public string AssetId { get; set; }

        /// <summary>
        /// Gets or sets the Tag Id.
        /// </summary>
        public int TagId { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the time written.
        /// </summary>
        public string Timestamp { get; set; }

    }
}
