using System;

namespace Theta.XSPOC.Apex.Api.Core.Models.Inputs
{
    /// <summary>
    /// Represent the class for GLAnalysisInput Input Data
    /// </summary>
    public class GLAnalysisCurveCoordinateInput
    {

        /// <summary>
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Gets or sets the surveydate.
        /// </summary>
        public string SurveyDate { get; set; }

        /// <summary>
        /// Gets or sets the asset GUID.
        /// </summary>
        public Guid AssetId { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public string TestDate { get; set; }

        /// <summary>
        /// Gets or sets the Analysis Type Id .
        /// </summary>
        public int AnalysisTypeId { get; set; }

        /// <summary>
        /// Gets or sets the Analysis Result Id.
        /// </summary>
        public int AnalysisResultId { get; set; }

    }
}
