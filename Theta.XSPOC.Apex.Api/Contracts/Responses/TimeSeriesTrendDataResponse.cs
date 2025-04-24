using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
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
}
