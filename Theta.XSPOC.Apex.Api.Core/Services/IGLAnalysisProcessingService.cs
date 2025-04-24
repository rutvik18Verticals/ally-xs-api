using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Interface for processing gas lift analysis. Runs gas lift analytics in response 
    /// to requests.
    /// </summary>
    public interface IGLAnalysisProcessingService
    {

        /// <summary>
        /// Processes the IGLAnalysis survey and gets the date.
        /// </summary>
        /// <param name="data">The <seealso cref="GLSurveyAnalysisInput"/> to act on, annotated. 
        /// with a correlation id.</param>
        /// <returns>The list of.<seealso cref="GLAnalysisSurveyDataOutput"/></returns>
        GLAnalysisSurveyDataOutput GetGLAnalysisSurveyDate(WithCorrelationId<GLSurveyAnalysisInput> data);

        /// <summary>
        /// Processes the provided gas lift analysis request and generates response based on that data.
        /// </summary>
        /// <param name="data">The <seealso cref="GLAnalysisInput"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="GLAnalysisSurveyDataOutput"/></returns>
        public GLAnalysisDataOutput GetGLAnalysisResults(WithCorrelationId<GLAnalysisInput> data);

        /// <summary>
        /// Processes the IGLAnalysis survey and gets the CurveCoordinates.
        /// </summary>
        /// <param name="requestWithCorrelationId">The <seealso cref="GLAnalysisCurveCoordinateInput"/> to act on, annotated. 
        /// with a correlation id.</param>
        /// <returns>The list of.<seealso cref="GLAnalysisCurveCoordinateDataOutput"/></returns>
        GLAnalysisCurveCoordinateDataOutput GetGLAnalysisSurveyCurveCoordinate(
            WithCorrelationId<GLAnalysisCurveCoordinateInput> requestWithCorrelationId);

        /// <summary>
        /// Processes the provided GL Analysis Curve Coordinate request and generates response based on that data.
        /// </summary>
        /// <param name="data">The <seealso cref="GLAnalysisCurveCoordinateInput"/> to act on, annotated 
        /// with a correlation id.</param>
        /// <returns>The <seealso cref="GLAnalysisCurveCoordinateDataOutput"/></returns>
        GLAnalysisCurveCoordinateDataOutput GetGLAnalysisCurveCoordinateResults(
            WithCorrelationId<GLAnalysisCurveCoordinateInput> data);

    }
}
