using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Common;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Data
{
    /// <summary>
    /// This is the interface that represents events.
    /// </summary>
    public interface IGroupStatus
    {

        /// <summary>
        /// Get the LoadViewParameters by currentViewId.
        /// </summary>
        /// <param name="currentViewId">The currentViewId.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="ParameterItem"/>.</returns>
        public SortedList<string, ParameterItem> LoadViewParameters(string currentViewId, string correlationId);

        /// <summary>
        /// Get the ItemsGroupStatus.
        /// </summary>
        /// <param name="nodeList">The nodeList.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="FacilityTagsModel"/>.</returns>
        public IList<FacilityTagsModel> GetItemsGroupStatus(string[] nodeList, string correlationId);

        /// <summary>
        /// Get the ConditionalFormats by currentViewId.
        /// </summary>
        /// <param name="currentViewId">The currentViewId.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="ConditionalFormatModel"/>.</returns>
        public IList<ConditionalFormatModel> GetConditionalFormats(string currentViewId, string correlationId);

        /// <summary>
        /// Get the ViewColumns by currentViewId.
        /// </summary>
        /// <param name="currentViewId">The asset id/node id.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="GroupStatusColumnsModels"/>.</returns>
        public IList<GroupStatusColumnsModels> LoadViewColumns(string currentViewId, string correlationId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeList">The node List.</param>
        /// <param name="hasCameraAlarm">The flag indicating if camera alarm is present.</param>
        /// <param name="hasOperationalScore">The flag indicating if operational score is present.</param>
        /// <param name="hasRuntimeAverage">The flag indicating if runtime average is present.</param>
        /// <param name="sColumns">The columns to include in the result.</param>
        /// <param name="tables">The tables to query.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The list of dictionaries representing the SQL result.</returns>
        public IList<Dictionary<string, object>> BuildSQLCommonResult(IList<string> nodeList, bool hasCameraAlarm,
            bool hasOperationalScore, bool hasRuntimeAverage, string sColumns, IList<string> tables,
            string correlationId);

        /// <summary>
        /// Get the Build SQL Parameter Result.
        /// </summary>
        /// <param name="nodeList">The nodeList.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="ParameterTypeResult"/>.</returns>
        public IList<ParameterTypeResult> BuildSQLParameterResult(IList<string> nodeList, string correlationId);

        /// <summary>
        /// Get the Build SQL CurrRaw Scan Data by nodelist.
        /// </summary>
        /// <param name="nodeList">The nodeList.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="CurrRawScanDataTypeResult"/>.</returns>
        public IList<CurrRawScanDataTypeResult> BuildSQLCurrRawScanData(IList<string> nodeList, string correlationId);

        /// <summary>
        /// Get the Build SQL Facility Result by nodelist.
        /// </summary>
        /// <param name="nodeList">The nodeList.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="FacilityTypeResult"/>.</returns>
        public IList<FacilityTypeResult> BuildSQLFacilityResult(IList<string> nodeList, string correlationId);

        /// <summary>
        /// Get the Build SQL Param Standard Type Result.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="nodeList">The nodeList.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="ParamStandardTypeSumResult"/>.</returns>
        public IList<ParamStandardTypeSumResult> BuildSQLParamStandardTypeResult(IList<FieldParamStandardTypeNameValues> list,
            IList<string> nodeList, string correlationId);

        /// <summary>
        /// Get the Build SQL Param Standard Type State Result.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="nodeList">The nodeList.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="ParamStandardTypeMaxResult"/>.</returns>
        public IList<ParamStandardTypeMaxResult> BuildSQLParamStandardTypeStateResult(IList<FieldParamStandardTypeNameValues> list,
            IList<string> nodeList, string correlationId);

        /// <summary>
        /// Get the Facility Param Standard Types.
        /// </summary>
        /// <param name="nodeList">The nodeList.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The <seealso cref="string"/>.</returns>
        public SortedList<string, int?> GetFacilityParamStandardTypes(string[] nodeList, string correlationId);

        /// <summary>
        /// Provides the GroupStatusFacilityTag.
        /// </summary>
        /// <param name="groupstatus">The group status.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The facility tag.</returns>
        public string GroupStatusFacilityTag(string groupstatus, string correlationId);

        /// <summary>Get the additional common columns defined in the database.</summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>A hashtable of columns combining the .</returns>
        /// <remarks>The database contains 1-based column indexes, but are not necessarly consecutive, without gaps.
        /// This function orders the position based on the database position, but assigns a sequential index (1-based).</remarks>
        public Hashtable LoadCommonColumns(string correlationId);

        /// <summary>
        /// Get the available views by user id .
        /// </summary>
        /// <param name="userId">The asset GUID.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The List of <seealso cref="AvailableViewModel"/>.</returns>
        IList<AvailableViewModel> GetAvailableViewsByUserId(string userId, string correlationId);

        /// <summary>
        /// Gets the view tables.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>The list of <seealso cref="GroupStatusTableModel"/>.</returns>
        IList<GroupStatusTableModel> GetViewTables(string correlationId);

        /// <summary>
        /// Gets the classifications data for the group widget.
        /// </summary>
        /// <param name="assetIds">The list of node ids in the group.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="assetCount">The count of unique assets</param>
        /// <returns>The <seealso cref="IList{AssetGroupStatusClassificationModel}"/>.</returns>
        public IList<AssetGroupStatusClassificationModel> GetClassificationsData(List<string> assetIds,
            string correlationId, out int assetCount);

        /// <summary>
        /// Gets the active alarms data for the group widget.
        /// </summary>
        /// <param name="assetIds">The list of node ids in the group.</param>
        /// <param name="correlationId">The correlation id.</param>
        public Task<AssetGroupStatusAlarmsModel> GetAlarmsData(List<string> assetIds,
            string correlationId);
    }
}
