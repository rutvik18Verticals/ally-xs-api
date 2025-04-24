using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the class for GL Analysis Curve Coordinate Values.
    /// </summary>
    public class GLAnalysisCurveCoordinateValues
    {

        /// <summary>
        /// Gets or sets the list of input ValueItem.
        /// </summary>
        public IList<ValueItem> Input { get; set; }

        /// <summary>
        /// Gets or sets the list of output ValueItem.
        /// </summary>
        public IList<ValueItem> Output { get; set; }

    }
}
