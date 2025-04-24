using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the Interface for IGLAnalysisGetCurveCoordinate.
    /// </summary>
    public interface IGLAnalysisGetCurveCoordinate
    {

        /// <summary>
        /// Get the List AnalysisResultCurvesModel by analysisResultId and application id .
        /// </summary>
        /// <param name="analysisResultId">Analysis Result Id</param>
        /// <param name="application">Application ID</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{AnalysisResultCurvesModel}"/>.</returns>
        IList<AnalysisResultCurvesModel> GetAnalysisResultCurve(int analysisResultId, int application, string correlationId);

        /// <summary>
        /// Get the GL Analysis Result Data data by asset id,testDate,analysisResultId and analysisTypeId.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="testDate">Test Date</param>
        /// <param name="analysisResultId">Analysis Result Id</param>
        /// <param name="analysisTypeId">Analysis Type Id</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="GLAnalysisResultModel"/>.</returns>
        GLAnalysisResultModel GetGLAnalysisResultData(Guid assetId, string testDate, int analysisResultId, int analysisTypeId, string correlationId);

        /// <summary>
        /// Get the List GLAnalysisCurveCoordinateModel by analysisResultId.
        /// </summary>
        /// <param name="analysisResultId">Analysis Result Id</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{GLAnalysisCurveCoordinateModel}"/>.</returns>
        IList<GLAnalysisCurveCoordinateModel> GetGLWellValveData(int analysisResultId, string correlationId);

        /// <summary>
        /// Get the List OrificeStatusModel by analysisResultId.
        /// </summary>
        /// <param name="analysisResultId">Analysis Result Id</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="OrificeStatusModel"/>.</returns>
        OrificeStatusModel GetOrificeStatus(int analysisResultId, string correlationId);

        /// <summary>
        /// Get the gas lift analysis ipr curve curve coordinates.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="testDateString">The test date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The curve coordinate <seealso cref="IPRAnalysisCurveCoordinateModel"/> model data.</returns>
        IPRAnalysisCurveCoordinateModel GetGLAnalysisIPRCurveCoordinate(Guid assetId, string testDateString, string correlationId);

        /// <summary>
        /// Fetches the curve coordinates.
        /// </summary>
        /// <param name="curveId">The curve identifier.</param>
        /// <param name="correlationId"></param>
        /// <returns>The list of <seealso cref="IList{CurveCoordinatesModel}"/> object.</returns>
        IList<CurveCoordinatesModel> FetchCurveCoordinates(int curveId, string correlationId);

        /// <summary>
        /// Get Data For Static Fluid Curve.
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="correlationId"></param>
        /// <returns>The model <seealso cref="StaticFluidCurveModel"/> model data.</returns>
        StaticFluidCurveModel GetDataForStaticFluidCurve(Guid assetId, string correlationId);

    }
}
