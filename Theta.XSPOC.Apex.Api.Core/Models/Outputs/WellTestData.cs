using System;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represent the class for <seealso cref="WellTestData"/> response.
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
