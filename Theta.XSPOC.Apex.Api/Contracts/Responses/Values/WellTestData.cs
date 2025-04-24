using System;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses.Values
{
    /// <summary>
    /// Represent the class for Well test Values.
    /// </summary>
    public class WellTestData
    {

        /// <summary>
        /// The well test date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The analysis type name.
        /// </summary>
        public string AnalysisTypeName { get; set; }

    }
}
