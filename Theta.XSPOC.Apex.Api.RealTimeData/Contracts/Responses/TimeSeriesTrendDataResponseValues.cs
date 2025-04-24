using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.RealTimeData.Contracts.Responses
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

    /// <summary>
    /// Represent the class for data history trends axis data output response.
    /// </summary>
    public class TimeSeriesDataResponseValues
    {

        /// <summary>
        /// Gets or sets the Asset Id.
        /// </summary>
        /// <example>3333d3d3-3ad1-3333-3f3a-33b3333e3333</example>
        public string AssetId { get; set; }

        /// <summary>
        /// Gets or sets the Tag Id.
        /// </summary>
        /// <example>[271, 267, 268]</example>
        public List<int> TagIds { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <example>[11921,6,0]</example>
        public List<double?> Values { get; set; }

        /// <summary>
        /// Gets or sets the time written.
        /// </summary>
        /// <example>2024-10-30T00:05:00Z</example>
        public string Timestamp { get; set; }

    }

    /// <summary>
    /// Represent the class for asset details output response.
    /// </summary>
    public class AssetDetailsResponseValues
    {
        /// <summary>
        /// Gets or sets the CustomerId
        /// </summary>
        /// <example>1111d1d1-1ad1-1111-1f1a-11b1111e1111</example>
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the assetID
        /// </summary>
        /// <example>3333d3d3-3ad1-3333-3f3a-33b3333e3333</example>
        public string AssetID { get; set; }

        /// <summary>
        /// Gets or sets the asset name
        /// </summary>
        /// <example>Well 1</example>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the poc type.
        /// </summary>
        /// <example>17</example>
        public short PocType { get; set; }

        /// <summary>
        /// Gets or sets the application Id.
        /// </summary>
        /// <example>5</example>
        public int? ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the isEnabled property for as asset.
        /// </summary>
        /// <example>true</example>
        public bool IsEnabled { get; set; }

    }

    /// <summary>
    /// Represent the class for parameter standard type data output response.
    /// </summary>
    public class ParameterStandardTypeResponseValues
    {
        /// <summary>
        /// Gets or sets the ParameterStandardType Id
        /// </summary>
        /// <example>1</example>
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        /// <example>ESP</example>
        public string Description { get; set; }

    }

    /// <summary>
    /// Represent the class for validate customer output response.
    /// </summary>
    public class ValidateCustomerResponseValues
    {
        /// <summary>
        /// Gets or sets the CustomerId
        /// </summary>
        /// <example>abc@abc.com</example>
        public string UserAccount { get; set; }

        /// <summary>
        /// Gets or sets the is Valid property for customer
        /// </summary>
        /// <example>true</example>
        public bool IsValid { get; set; }
    }
}
