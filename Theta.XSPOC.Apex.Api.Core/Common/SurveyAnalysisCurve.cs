namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// This is the implementation that represents the configuration of a survey analysis curves.
    /// </summary>
    public class SurveyAnalysisCurve : AnalysisCurve
    {

        /// <summary>
        /// Initializes a new survey curve with a specified ID and curve type
        /// </summary>
        /// <param name="id">The ID</param>
        /// <param name="curveType">The SurveyCurveType of this analysis curve.</param>
        public SurveyAnalysisCurve(object id, SurveyCurveType curveType)
            : base(id, curveType)
        {
        }

        /// <summary>
        /// Initializes a new survey curve with a specified curve type
        /// </summary>
        /// <param name="curveType">The SurveyCurveType of this analysis curve.</param>
        public SurveyAnalysisCurve(SurveyCurveType curveType)
            : base(curveType)
        {
        }

        /// <summary>
        /// Return the IndustryApplication of the analysis curve
        /// </summary>
        public override IndustryApplication IndustryApplication => IndustryApplication.GasArtificialLift;

    }
}
