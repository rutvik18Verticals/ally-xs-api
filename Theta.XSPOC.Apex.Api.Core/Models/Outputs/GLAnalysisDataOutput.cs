using Theta.XSPOC.Apex.Api.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represent the class for GLAnalysis  data output.
    /// </summary>
    public class GLAnalysisDataOutput : CoreOutputBase
    {

        /// <summary>
        /// Gets or sets the list of values.
        /// </summary>
        public GLAnalysisValues Values { get; set; }

    }
}
