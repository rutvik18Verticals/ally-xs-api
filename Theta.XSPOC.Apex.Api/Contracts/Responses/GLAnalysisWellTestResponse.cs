using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Contracts.Responses.Values;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// Describes a AnalysisKeyResponse value that needs to be send out.
    /// </summary>
    public class GLAnalysisWellTestResponse : ResponseBase
    {

        /// <summary>
        /// Gets or sets the list gl analysis well test data.
        /// </summary>
        public IList<GLAnalysisWellTestData> Values { get; set; }

    }
}
