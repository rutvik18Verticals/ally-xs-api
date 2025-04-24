using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// Describes a gl analysis survey curve coordinate response that needs to be send out.
    /// </summary>
    public class GLAnalysisSurveyCurveCoordinatesResponse : ResponseBase
    {

        /// <summary>
        /// Gets or sets the list of values.
        /// </summary>
        public IList<GLAnalysisCurveCoordinateData> Values { get; set; }

    }
}
