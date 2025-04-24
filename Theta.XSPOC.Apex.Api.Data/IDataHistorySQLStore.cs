using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface that represents data history sql store.
    /// </summary>
    public interface IDataHistorySQLStore
    {

        /// <summary>
        /// Gets the <seealso cref="IList{GroupTrendDataModel}"/>.
        /// </summary>
        /// <returns>The <seealso cref="IList{GroupTrendDataModel}"/>.</returns>
        IList<GroupTrendDataModel> GetGroupTrendData(string correlationId);

        /// <summary>
        /// Gets the <seealso cref="List{AnalysisTrendDataModel}"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="name">The analysis trend name.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{AnalysisTrendDataModel}"/>.</returns>
        IList<AnalysisTrendDataModel> GetAnalysisTrendData(string nodeId, DateTime startDate,
            DateTime endDate, string name, string correlationId);

        /// <summary>
        /// Gets the <seealso cref="IList{MeasurementTrendDataModel}"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{MeasurementTrendDataModel}"/>.</returns>
        IList<MeasurementTrendItemModel> GetMeasurementTrendItems(string nodeId, string correlationId);

        /// <summary>
        /// Gets the <seealso cref="IList{GroupTrendDataHistoryModel}"/> for the group <seealso ref="groupName"/>
        /// in the given date range.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="groupParameterID">The group parameter id.</param>
        /// <param name="groupName">The group name.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{GroupTrendDataHistoryModel}"/>.</returns>
        IList<GroupTrendDataHistoryModel> GetGroupTrendData(DateTime startDate,
            DateTime endDate, int groupParameterID, string groupName, string correlationId);

        /// <summary>
        /// Gets the <seealso cref="DataHistoryItemModel"/>.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="DataHistoryItemModel"/>.</returns>
        DataHistoryItemModel GetDataHistoryTrends(string assetId, string correlationId);

        /// <summary>
        /// Get the controller trend item by node id and poc type.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="pocType">The poc type.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="ControllerTrendItemModel"/>.</returns>
        IList<ControllerTrendItemModel> GetControllerTrendItems(string nodeId, int pocType, string correlationId);

        /// <summary>
        /// Gets the <seealso cref="IList{RodStressTrendItemModel}"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{RodStressTrendItemModel}"/>.</returns>
        IList<RodStressTrendItemModel> GetRodStressTrendItems(string nodeId, string correlationId);

        /// <summary>
        /// Gets the event trend data.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="eventTypeID">The event type id.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="EventTrendDataModel"/>.</returns>
        IList<EventTrendDataModel> GetEventTrendData(string nodeId, int eventTypeID,
            DateTime startDate, DateTime endDate, string correlationId);

        /// <summary>
        /// Gets the rod stress trend data <seealso cref="IList{RodStressTrendDataModel}"/>.
        /// </summary>
        /// <param name="stressColumn">The stressColumn.</param>
        /// <param name="nodeId">The node Id.</param>
        /// <param name="rodNum">The rod number.</param>
        /// <param name="grade">The grade.</param>
        /// <param name="diameter">The diameter.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{RodStressTrendDataModel}"/>.</returns>
        public IList<RodStressTrendDataModel> GetRodStressTrendData(string stressColumn, string nodeId, int rodNum,
            string grade, float diameter, DateTime startDate, DateTime endDate, string correlationId);

        /// <summary>
        /// Gets the <seealso cref="IList{PlungerLiftTrendDataModel}"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="name">The name of the column.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{PlungerLiftTrendDataModel}"/>.</returns>
        IList<PlungerLiftTrendDataModel> GetPlungerLiftTrendData(string nodeId,
            DateTime startDate, DateTime endDate, string name, string correlationId);

        /// <summary>
        /// Gets the well test trend data.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="name">The trend parameter name.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="wellTestType">The wellTestType.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="WellTestTrendDataModel"/>.</returns>
        IList<WellTestTrendDataModel> GetWellTestTrendData(string nodeId, string name,
            DateTime startDate, DateTime endDate, bool wellTestType, string correlationId);

        /// <summary>
        /// Gets the <seealso cref="List{MeterTrendDataModel}"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="name">The trend parameter name.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="List{MeterTrendDataModel}"/>.</returns>
        IList<MeterTrendDataModel> GetMeterHistoryTrendData(string nodeId,
            DateTime startDate, DateTime endDate, string name, string correlationId);

        /// <summary>
        /// Gets the <seealso cref="DataHistoryModel"/>.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="DataHistoryModel"/>.</returns>
        DataHistoryModel GetDataHistoryList(Guid assetId, string correlationId);

        /// <summary>
        /// Gets the <seealso cref="PCSFDatalogConfigurationItemModel"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="datalogNumber">The datalog Number.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="PCSFDatalogConfigurationItemModel"/>.</returns>
        PCSFDatalogConfigurationItemModel GetPCSFDatalogConfigurationData(string nodeId,
            int datalogNumber, string correlationId);

        /// <summary>
        /// Get MasterNodeID for input Well NodeID
        /// </summary>
        /// <param name="masterNodeAddress">Node address with "+offset" removed</param>
        /// <param name="masterPocType">POCType of the Slave Device</param>
        /// <param name="wellPortId">PortID of the Slave Device, must be same as Master.</param>
        /// <param name="correlationId"></param>
        /// <returns>master controllers NodeID string</returns>
        /// <remarks>Implemented for PCSF Master Controllers, not Totalflow Master Controllers(but it can be)</remarks>
        string GetMasterNodeId(string masterNodeAddress,
            int masterPocType, int wellPortId, string correlationId);

        /// <summary>
        /// Gets the most recent pcsf datalog record.
        /// </summary>
        /// <param name="wellNames">List of well names.</param>
        /// <param name="datalogNumber">The datalog number.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="PCSFDatalogRecordModel"/>.</returns>
        PCSFDatalogRecordModel FindMostRecentPCSFDatalogRecord(IDictionary<int, string> wellNames,
            int datalogNumber, string correlationId);

        /// <summary>
        /// Gets the list of analysis trend items.
        /// </summary>
        /// <returns>The <seealso cref="IList{String}"/>.</returns>
        IList<string> GetAnalysisTrendItems(string correlationId);

        /// <summary>
        /// Get the controller trend item by start date and end date.
        /// </summary>
        /// <param name="name">The trend column name.</param>
        /// <param name="nodeId">The node id.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="List{OperationalScoreModel}"/>.</returns>
        IList<OperationalScoreDataModel> GetOperationalScoreTrendData(string name, string nodeId,
            DateTime startDate, DateTime endDate, string correlationId);

        /// <summary>
        /// Gets the <seealso cref="IList{ProductionStatisticsTrendData}"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="name">The trend parameter name.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{ProductionStatisticsTrendDataModel}"/>.</returns>
        IList<ProductionStatisticsTrendDataModel> GetProductionStatisticsTrendData(string nodeId,
                    DateTime startDate, DateTime endDate, string name, string correlationId);

        /// <summary>
        /// Gets the PCSF Datalog Record data based on node id, datalog number that are saved between the input 
        /// start date and end date.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="datalogNumber">The datalog number.</param>
        /// <param name="startDateTime">The input start date.</param>
        /// <param name="endDateTime">The input end date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{PCSFDatalogRecordModel}"/> records.</returns>
        public IList<PCSFDatalogRecordModel> GetPCSFDatalogRecordItems(string nodeId, int datalogNumber,
            DateTime startDateTime, DateTime endDateTime, string correlationId);

        /// <summary>
        /// Gets the ESPAnalysisResults data for a node in the given date range.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{ESPAnalysisResultModel}"/> object.</returns>
        public IList<ESPAnalysisResultModel> SearchESPAnalysisResult(string nodeId,
            DateTime startDate, DateTime endDate, string correlationId);

        /// <summary>
        /// Gets the <seealso cref="DataHistoryItemModel"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="DataHistoryItemModel"/>.</returns>
        DataHistoryItemModel GetDataHistoryTrendDataItems(string nodeId, string correlationId);

        /// <summary>
        /// Gets the failure components <seealso cref="IList{FailureModel}"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="failureComponentId">The failure component id.</param>
        /// <param name="failureSubComponentId">The failure sub component id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="DataHistoryItemModel"/>.</returns>
        public IList<FailureModel> GetFailureComponentItems(string nodeId,
                    DateTime startDate, DateTime endDate,
                    int? failureComponentId, int? failureSubComponentId, string correlationId);

        /// <summary>
        /// Gets the ESPAnalysisResults data for a node in the given date range.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{GLAnalysisResultModel}"/> object.</returns>
        public IList<GLAnalysisResultModel> SearchGLAnalysisResult(string nodeId,
            DateTime startDate, DateTime endDate, string correlationId);

        /// <summary>
        /// Gets the first injecting flow control device depth.
        /// </summary>
        /// <param name="analysisResultId">The analysis result id.</param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        public float? GetFirstInjectingFlowControlDeviceDepth(int analysisResultId, string correlationId);

        /// <summary>
        /// Gets the value of GLIncludeInjGasInTest system parameter.
        /// </summary>
        /// <returns>The value of GLIncludeInjGasInTest system parameter</returns>
        public string GetGLIncludeInjGasInTestValue(string correlationId);

        /// <summary>
        /// Gets the default trends based on <paramref name="viewId"/>.
        /// </summary>
        /// <param name="viewId">The view id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <see cref="List{GraphViewTrendsModel}"/>.</returns>
        public List<GraphViewTrendsModel> GetDefaultTrendsData(string viewId, string correlationId);

        /// <summary>
        /// Gets the default trends view settings based on <paramref name="viewId"/>.
        /// </summary>
        /// <param name="viewId">The view id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <see cref="GraphViewSettingsModel"/>.</returns>
        public GraphViewSettingsModel GetDefaultTrendViewSettings(string viewId, string correlationId);

        /// <summary>
        /// Gets the default trends views  based on <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <see cref="List{GraphViewsModel}"/>.</returns>
        public List<GraphViewsModel> GetDefaultTrendViews(string userId, string correlationId);

        /// <summary>
        /// Gets the <seealso cref="List{MeasurementTrendDataModel}"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="paramStandardType">The param standard type.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="List{MeasurementTrendDataModel}"/>.</returns>
        IList<MeasurementTrendDataModel> GetMeasurementTrendData(string nodeId, int paramStandardType,
            DateTime startDate, DateTime endDate, string correlationId);

        /// <summary>
        /// Gets the <seealso cref="IList{ControllerTrendDataModel}"/>.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="address">The address.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{ControllerTrendDataModel}"/>.</returns>
        IList<ControllerTrendDataModel> GetControllerTrendData(string nodeId, int address,
            DateTime startDate, DateTime endDate, string correlationId);

        /// <summary>
        /// Gets the downtime by wells.
        /// </summary>
        /// <param name="nodeIds">The node ids.</param>
        /// <param name="numberOfDays">The number of days.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <see cref="DowntimeByWellsModel"/>.</returns>
        DowntimeByWellsModel GetDowntime(IList<string> nodeIds, int numberOfDays, string correlationId);
    }
}