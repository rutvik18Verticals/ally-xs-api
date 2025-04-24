using Theta.XSPOC.Apex.Api.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the core layer's output for esp analysis data.
    /// </summary>
    public class ESPAnalysisDataOutput : CoreOutputBase
    {

        /// <summary>
        /// Gets or sets the list of values.
        /// </summary>
        public ESPAnalysisValues Values { get; set; }

        /// <summary>
        /// Gets or sets the pressure profile.
        /// </summary>
        public ESPPressureProfileData PressureProfile { get; set; }

    }
}
