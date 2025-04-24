using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface for data retrival from db.
    /// </summary>
    public interface ISQLStoreBase
    {

        /// <summary>
        /// Get the carddata based on asset id and card date.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="cardDate">The card date.</param>
        /// <returns>The <seealso cref="CardDataModel"/> entity</returns>
        CardDataModel GetCardData(Guid assetId, DateTime cardDate);

        /// <summary>
        /// Get the well details based on asset guid
        /// </summary>
        /// <param name="assetId">The asset GUID</param>
        /// <returns>The <seealso cref="WellDetailsModel"/></returns>
        WellDetailsModel GetWellDetails(Guid assetId);

        /// <summary>
        /// Gets a list of all well tests for esp analysis by asset id.
        /// </summary>
        /// <param name="assetId">The asset GUID</param>
        /// <returns>The <seealso cref="List{WellTestModel}"/></returns>
        IList<WellTestModel> GetListWellTest(Guid assetId);

        /// <summary>
        /// Get the current raw scan data based on asset id
        /// </summary>
        /// <param name="assetId">The asset GUID</param>
        /// <returns>The <seealso cref="List{CurrentRawScanDataModel}" /></returns>
        IList<CurrentRawScanDataModel> GetCurrentRawScanData(Guid assetId);

        /// <summary>
        /// Gets the CustomPumpingUnit based on pumping unit id.
        /// </summary>
        /// <param name="pumpingUnitId">The pumping unit id.</param>
        /// <returns>The <seealso cref="CustomPumpingUnitModel"/></returns>
        CustomPumpingUnitModel GetCustomPumpingUnits(string pumpingUnitId);

        /// <summary>
        /// Gets the NodeMaster data based on asset guid.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <returns>The <seealso cref="NodeMasterModel"/></returns>
        NodeMasterModel GetNodeMasterData(Guid assetId);

        /// <summary>
        /// Get the well details based on asset guid
        /// </summary>
        /// <param name="assetId">The asset GUID</param>
        /// <param name="cardDate">The card date</param>
        /// <returns>The <seealso cref="WellTestModel"/></returns>
        WellTestModel GetWellTestData(Guid assetId, DateTime cardDate);

        /// <summary>
        /// Get the well details based on asset guid
        /// </summary>
        /// <param name="assetId">The asset GUID</param>
        /// <param name="cardDate">The card date</param>
        /// <returns>The <seealso cref="XDiagResultsModel"/></returns>
        XDiagResultsModel GetXDiagResultData(Guid assetId, DateTime cardDate);

        /// <summary>
        /// Get the value of system paramter
        /// </summary>
        /// <param name="paramName">The parameter name</param>
        /// <returns>The value of the paramter</returns>
        string GetSystemParameterData(string paramName);

        /// <summary>
        /// Gets GLAnalysis Result Model by asset id,testDate,analysisResultId and analysisTypeId.
        /// </summary>
        /// <param name="assetId">The asset GUID</param>
        /// <param name="testDate">test Date</param>
        /// <param name="analysisResultId">analysis Result Id</param>
        /// <param name="analysisTypeId">analysis Type Id</param>
        /// <returns>The <seealso cref="GLAnalysisResultModel"/></returns>
        GLAnalysisResultModel GetGLAnalysisResult(Guid assetId, string testDate, int? analysisResultId,
            int? analysisTypeId);

        /// <summary>
        /// Gets List GLAnalysisCurveCoordinateModel Model by analysisResultId.
        /// </summary>
        /// <param name="analysisResultId">analysis Result Id</param>
        /// <returns>The <seealso cref="List{GLAnalysisCurveCoordinateModel}"/></returns>
        IList<GLAnalysisCurveCoordinateModel> GetGLWellValveData(int? analysisResultId);

        /// <summary>
        /// Gets List OrificeStatusModel Model by analysisResultId.
        /// </summary>
        /// <param name="analysisResultId">analysis Result Id</param>
        /// <returns>The <seealso cref="OrificeStatusModel"/></returns>
        OrificeStatusModel GetOrificeStatus(int? analysisResultId);

        /// <summary>
        /// Gets List AnalysisResultCurvesModel Model by analysisResultId and application.
        /// </summary>
        /// <param name="analysisResultId">Analysis Result Id</param>
        /// <param name="application">Application ID</param>
        /// <returns>The <seealso cref="List{AnalysisResultCurvesModel}"/></returns>
        IList<AnalysisResultCurvesModel> GetAnalysisResultCurve(int? analysisResultId, int? application);

    }
}
