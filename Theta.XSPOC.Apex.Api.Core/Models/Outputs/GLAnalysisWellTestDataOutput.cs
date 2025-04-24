using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the core layer's output for analysis key data.
    /// </summary>
    public class GLAnalysisWellTestDataOutput : CoreOutputBase
    {

        /// <summary>
        /// Gets or sets the list of values.
        /// </summary>
        public IList<GLAnalysisWellTestData> Values { get; set; }

    }
}
