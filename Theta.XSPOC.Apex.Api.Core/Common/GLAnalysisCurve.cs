namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents a GL analysis result curve with a curve type and an industry application. 
    /// </summary>
    public class GLAnalysisCurve : AnalysisCurve
    {

        /// <summary>
        /// Initializes a new GL analysis curve with a specified ID and curve type
        /// </summary>
        /// <param name="id">The ID</param>
        /// <param name="curveType">The GLCurveType of this analysis curve.</param>
        public GLAnalysisCurve(object id, GLCurveType curveType)
            : base(id, curveType)
        {
        }

        /// <summary>
        /// Initializes a new GL analysis curve with a specified curve type
        /// </summary>
        /// <param name="curveType">The GLCurveType of this analysis curve.</param>
        public GLAnalysisCurve(GLCurveType curveType)
            : base(curveType)
        {
        }

        /// <summary>
        /// Return the IndustryApplication of the analysis curve
        /// </summary>
        public override IndustryApplication IndustryApplication => IndustryApplication.GasArtificialLift;

    }
}
