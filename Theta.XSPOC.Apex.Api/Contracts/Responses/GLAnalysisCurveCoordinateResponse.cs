using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Contracts.Responses
{
    /// <summary>
    /// Describes GL Analysis Curve Coordinate Response Values that needs to be send out.
    /// </summary>
    public class GLAnalysisCurveCoordinateResponse : ResponseBase
    {

        /// <summary>
        /// Gets or sets the <seealso cref="List{GLAnalysisCurveCoordinateResponseValues}"/>.
        /// </summary>
        public IList<GLAnalysisCurveCoordinateResponseValues> Values { get; set; }

    }
}
