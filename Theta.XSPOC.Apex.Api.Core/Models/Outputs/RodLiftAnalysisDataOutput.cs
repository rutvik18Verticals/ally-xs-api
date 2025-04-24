using Theta.XSPOC.Apex.Api.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the core layer's output for rod lift analysis data.
    /// </summary>
    public class RodLiftAnalysisDataOutput : CoreOutputBase
    {

        /// <summary>
        /// Gets or sets the list of values.
        /// </summary>
        public RodLiftAnalysisValues Values { get; set; }

    }
}
