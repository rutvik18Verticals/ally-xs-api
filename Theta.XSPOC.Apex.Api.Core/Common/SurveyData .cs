using System;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents a survey, such as a temperature or pressure survey
    /// </summary>
    public class SurveyData : IdentityBase
    {

        /// <summary>
        /// Creates a new SurveyData object
        /// </summary>
        /// <param name="id">The id of the object</param>
        public SurveyData(object id) : base(id)
        {
        }

        /// <summary>
        /// Initializes a new SurveyData with a default ID
        /// </summary>
        public SurveyData()
        {
        }

        /// <summary>
        /// Gets or sets the survey date
        /// </summary>
        public DateTime SurveyDate { get; set; }

        /// <summary>
        /// Gets or sets the survey curve
        /// </summary>
        public AnalysisCurve SurveyCurve { get; set; }

    }
}
