using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Contracts.Responses.Values;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// Represent the class for RodLiftAnalysis data values.
    /// </summary>
    public class RodLiftAnalysisValues
    {

        /// <summary>
        /// Gets or sets the list of input ValueItem.
        /// </summary>
        public IList<ValueItem> Input { get; set; }

        /// <summary>
        /// Gets or sets the list of output ValueItem.
        /// </summary>
        public IList<ValueItem> Output { get; set; }

    }
}
