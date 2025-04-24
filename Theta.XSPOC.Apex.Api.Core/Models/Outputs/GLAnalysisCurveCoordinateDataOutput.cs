using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the core layer's output for gl analysis curve coordinate data.
    /// </summary>
    public class GLAnalysisCurveCoordinateDataOutput : CoreOutputBase
    {

        /// <summary>
        /// Gets or sets the list of values.
        /// </summary>
        public IList<GLAnalysisCurveCoordinateData> Values { get; set; } = new List<GLAnalysisCurveCoordinateData>();

        /// <summary>
        /// Gets or sets the list of values.
        /// </summary>
        public GLAnalysisCurveCoordinateValues Value { get; set; }

    }
}
