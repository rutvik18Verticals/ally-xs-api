using Theta.XSPOC.Apex.Api.Contracts.Responses.Values;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// Describes a esp analysis value response that needs to be send out.
    /// </summary>
    public class ESPAnalysisResponse : ResponseBase
    {

        /// <summary>
        /// Gets or sets the list of values.
        /// </summary>
        public ESPAnalysisValues Values { get; set; }

        /// <summary>
        /// Gets or sets the pressure profile data.
        /// </summary>
        public ESPPressureProfile PressureProfile { get; set; }

    }
}
