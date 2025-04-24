using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.RealTimeData.Contracts.Responses
{
    /// <summary>
    /// Represent the class for data history trend output response.
    /// </summary>
    public class TimeSeriesTrendDataResponse : ResponseBase
    {
        /// <summary>
        /// Gets or sets the list of <seealso cref="TimeSeriesTrendDataResponseValues"/> values.
        /// </summary>
        public List<TimeSeriesTrendDataResponseValues> Values { get; set; }
    }

    /// <summary>
    /// Represent the class for data history trend output response.
    /// </summary>
    public class AllyTimeSeriesTrendDataResponse : ResponseBase
    {
        /// <summary>
        /// Gets or sets the list of <seealso cref="AllyTimeSeriesTrendDataResponseValues"/> values.
        /// </summary>
        public List<AllyTimeSeriesTrendDataResponseValues> Values { get; set; }

        /// <summary>
        /// Gets or sets the list of <seealso cref="AllyTimeSeriesTrendDataResponseValues"/> values.
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Gets or sets the list of <seealso cref="AllyTimeSeriesTrendDataResponseValues"/> values.
        /// </summary>        
        public int TotalPages { get; set; }        
    }

    /// <summary>
    /// Represent the class for data history trend output response.
    /// </summary>
    public class TimeSeriesDataResponse : ResponseBase
    {
        /// <summary>
        /// Gets or sets the list of <seealso cref="TimeSeriesDataResponse"/> values.
        /// </summary>
        public List<TimeSeriesDataResponseValues> Values { get; set; }

        /// <summary>
        /// Gets or sets the list of <seealso cref="TimeSeriesDataResponse"/> values.
        /// </summary>
        /// <example>1</example>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Gets or sets the list of <seealso cref="TimeSeriesDataResponse"/> values.
        /// </summary>
        /// <example>1</example>
        public int TotalPages { get; set; }
    }

    /// <summary>
    /// Represent the class for asset details output response.
    /// </summary>
    public class AssetDetailsDataResponse : ResponseBase
    {
        /// <summary>
        /// Gets or sets the list of <seealso cref="AssetDetailsDataResponse"/> values.
        /// </summary>
        public List<AssetDetailsResponseValues> Values { get; set; }       
    }

    /// <summary>
    /// Represent the class for Parameter Standard Type data output response.
    /// </summary>
    public class ParameterStandardTypeDataResponse : ResponseBase
    {
        /// <summary>
        /// Gets or sets the list of <seealso cref="ParameterStandardTypeDataResponse"/> values.
        /// </summary>
        public List<ParameterStandardTypeResponseValues> Values { get; set; }
    }
    /// <summary>
    /// Represent the class for validate customer details output response.
    /// </summary>
    public class ValidateCustomerDataResponse : ResponseBase
    {
        /// <summary>
        /// Gets or sets the list of <seealso cref="ValidateCustomerDataResponse"/> values.
        /// </summary>
        public List<ValidateCustomerResponseValues> Values { get; set; }
    }
}
