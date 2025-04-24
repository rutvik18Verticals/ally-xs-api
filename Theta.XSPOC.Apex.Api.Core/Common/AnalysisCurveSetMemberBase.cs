using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Common.Interface;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents a base class for analysis result curve set.
    /// </summary>
    public abstract class AnalysisCurveSetMemberBase
    {

        /// <summary>
        /// The Curve Set Member Id
        /// </summary>
        public int? CurveSetMemberId { get; set; }

        /// <summary>
        /// The annotation information that goes with the curve
        /// </summary>
        public abstract IAnalysisCurveSetMemberAnnotationData AnnotationData { get; set; }

        /// <summary>
        /// The curve
        /// </summary>
        public IList<CurveCoordinate> Curve { get; set; }

        /// <summary>
        /// Gets an IList of Coordinates for the curve
        /// </summary>
        public IList<Coordinate<double, double>> GetCurveCoordinateList()
        {
            return Curve != null
                ? (from coordinate in Curve select coordinate.Coordinate).ToList()
                : new List<Coordinate<double, double>>();
        }

    }
}
