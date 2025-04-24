using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the core layer's output for curve coordinate data.
    /// </summary>
    public class CurveCoordinateDataOutput : CoreOutputBase
    {

        /// <summary>
        /// Gets or sets the list of curve coordinate values.
        /// </summary>
        public IList<CurveCoordinateValues> Values { get; set; } = new List<CurveCoordinateValues>();

    }
}
