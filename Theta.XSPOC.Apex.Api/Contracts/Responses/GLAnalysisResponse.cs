using Theta.XSPOC.Apex.Api.Contracts.Responses.Values;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// Describes gaslift analysis value response that needs to be send out.
    /// </summary>
    public class GLAnalysisResponse : ResponseBase
    {

        /// <summary>
        /// Gets or sets the list of values.
        /// </summary>
        public GLAnalysisValues Values { get; set; }

    }
}
