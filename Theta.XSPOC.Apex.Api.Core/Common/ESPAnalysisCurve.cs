namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents an ESP analysis result curve.
    /// </summary>
    public class ESPAnalysisCurve : AnalysisCurve
    {

        /// <summary>
        /// Initializes a new ESP analysis curve with a specified ID and curve type
        /// </summary>
        /// <param name="id">The ID</param>
        /// <param name="curveType">The ESPCurveType of this analysis curve.</param>
        public ESPAnalysisCurve(object id, ESPCurveType curveType)
            : base(id, curveType)
        {
        }

        /// <summary>
        /// Initializes a new ESP analysis curve with a specified curve type
        /// </summary>
        /// <param name="curveType">The ESPCurveType of this analysis curve.</param>
        public ESPAnalysisCurve(ESPCurveType curveType)
            : base(curveType)
        {
        }

        /// <summary>
        /// Return the IndustryApplication of the analysis curve
        /// </summary>
        public override IndustryApplication IndustryApplication
            => IndustryApplication.ESPArtificialLift;

    }
}
