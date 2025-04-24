namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// This is the implementation that represents the configuration of a analysis curves.
    /// </summary>
    public class IPRAnalysisCurve : AnalysisCurve
    {

        /// <summary>
        /// Initializes a new IPR analysis curve with a specified ID and curve type
        /// </summary>
        /// <param name="id">The ID</param>
        /// <param name="curveType">The <see cref="IPRCurveType"/> of this analysis curve.</param>
        public IPRAnalysisCurve(object id, IPRCurveType curveType)
            : base(id, curveType)
        {
        }

        /// <summary>
        /// Initializes a new IPR analysis curve with a specified curve type
        /// </summary>
        /// <param name="curveType">The <see cref="IPRCurveType"/> of this analysis curve.</param>
        public IPRAnalysisCurve(IPRCurveType curveType)
            : base(curveType)
        {
        }

        /// <summary>
        /// Return the <see cref="IndustryApplication"/> of the analysis curve
        /// </summary>
        public override IndustryApplication IndustryApplication => ((IPRCurveType)CurveType).IndustryApplication;

    }
}
