using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the core layer's output for gl analysis survey data.
    /// </summary>
    public class GLAnalysisSurveyDataOutput : CoreOutputBase
    {

        /// <summary>
        /// Gets or sets the list of values.
        /// </summary>
        public IList<GLAnalysisData> Values { get; set; } = new List<GLAnalysisData>();

    }
}
