using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses.Values
{
    /// <summary>
    /// Represent the class for Gas Lift Analysis response values.
    /// </summary>
    public class GLAnalysisValues
    {

        /// <summary>
        /// Gets or sets the list of input ValueItem.
        /// </summary>
        public IList<ValueItem> Inputs { get; set; }

        /// <summary>
        /// Gets or sets the list of output ValueItem.
        /// </summary>
        public IList<ValueItem> Outputs { get; set; }

        /// <summary>
        /// Gets or sets the list of flow control device analysis values.
        /// </summary>
        public IList<FlowControlDeviceAnalysisValues> Valves { get; set; }

        /// <summary>
        /// Gets or sets the wellbore view data.
        /// </summary>
        public GLAnalysisWellboreViewData WellboreViewData { get; set; }

    }
}
