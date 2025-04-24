using System;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses.Values
{
    /// <summary>
    /// Represent the class for GLAnalysisWellTestData.
    /// </summary>
    public class GLAnalysisWellTestData
    {

        /// <summary>
        /// The gl analysis well test date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// The analysis type id.
        /// </summary>
        public int AnalysisTypeId { get; set; }

        /// <summary>
        /// The analysis type name.
        /// </summary>
        public string AnalysisTypeName { get; set; }

        /// <summary>
        /// The analysis result id.
        /// </summary>
        public int? AnalysisResultId { get; set; }

    }
}
