using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface that represents gas lift analysis.
    /// </summary>
    public interface IGLAnalysis
    {

        /// <summary>
        /// Get the IGLAnalysis survey date.
        /// </summary>
        /// <param name="id">The asset GUID.</param>
        /// <param name="gasArtificialLiftkey">The gasArtificialLiftkey.</param>
        /// <param name="temperatureCurvekey">The temperatureCurvekey.</param>
        /// <param name="pressureCurvekey">The pressureCurvekey.</param>
        /// <param name="correlationId"></param>
        /// <returns>The List of <seealso cref="GlAnalysisSurveyDateModel"/>.</returns>
        public IList<GlAnalysisSurveyDateModel> GetGLAnalysisSurveyDate(Guid id,
            int gasArtificialLiftkey, int temperatureCurvekey,
            int pressureCurvekey, string correlationId);

        /// <summary>
        /// Get the gas lift analysis data by asset id, test date and analysis type.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="testDate">The test date.</param>
        /// <param name="analysisType">The analysis type.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="GLAnalysisResponse"/>.</returns>
        GLAnalysisResponse GetGLAnalysisData(Guid assetId, string testDate,
            int analysisType, string correlationId);

        /// <summary>
        /// Get the IGLAnalysis survey date.
        /// </summary>
        /// <param name="id">The asset GUID.</param>
        /// <param name="surveyDate">The surveyDate.</param>
        /// <param name="correlationId"></param>
        /// <returns>The List of <seealso cref="GlAnalysisSurveyCurveCoordinateDataModel"/>.</returns>
        public IList<GlAnalysisSurveyCurveCoordinateDataModel> GetGLAnalysisCurveCoordinatesSurveyDate(Guid id,
            string surveyDate, string correlationId);

        /// <summary>
        /// Get the Get GL Sensitivity Analysis Data.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="testDateString">The test date.</param>
        /// <param name="analysisResultId">The analysis result id.</param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        GLAnalysisResponse GetGLSensitivityAnalysisData(Guid assetId, string testDateString,
            int analysisResultId, string correlationId);

        /// <summary>
        /// Get the wellbore data.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="correlationId"></param>
        /// <returns>A <seealso cref="WellboreDataModel"/></returns>
        public WellboreDataModel GetWellboreData(Guid assetId, string correlationId);

    }
}
