using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represent the class for gas lift analysis data values.
    /// </summary>
    public class GLAnalysisValues
    {

        /// <summary>
        /// Gets or sets the list of input ValueItem.
        /// </summary>
        public IList<AnalysisInputOutput> Inputs { get; set; }

        /// <summary>
        /// Gets or sets the list of output ValueItem.
        /// </summary>
        public IList<AnalysisInputOutput> Outputs { get; set; }

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
