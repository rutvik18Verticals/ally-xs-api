using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Common;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Data.Sql
{
    /// <summary>
    /// This is the implementation that represents the configuration of a group status
    /// on the current XSPOC database.
    /// </summary>
    public class GroupStatusSQLStore : SQLStoreBase, IGroupStatus
    {

        #region Private Members

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="NoLockXspocDbContext"/> using the provided <paramref name="contextFactory"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{XspocDbContext}"/> to get the <seealso cref="XspocDbContext"/> 
        /// to use for database operations.
        /// </param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/> is null.
        /// </exception>
        public GroupStatusSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IGroupStatus Implementation

        /// <summary>
        /// Get the ConditionalFormats.
        /// </summary>
        /// <param name="currentViewId">The currentViewId.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="ConditionalFormatModel"/>.</returns>
        public IList<ConditionalFormatModel> GetConditionalFormats(string currentViewId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusSQLStore)} {nameof(GetConditionalFormats)}", correlationId);

            if (int.TryParse(currentViewId, out var currentViewIdInt) == false)
            {
                return new List<ConditionalFormatModel>();
            }

            using (var context = _contextFactory.GetContext())
            {
                var result = context.GroupStatusView.AsNoTracking()
                    .Where(gsv => gsv.ViewId == currentViewIdInt)
                    .Join(context.GroupStatusViewsColumns,
                        gsv => gsv.ViewId,
                        gsvc => gsvc.ViewId,
                        (gsv, gsvc) => new { gsv, gsvc })
                    .Join(context.ConditionalFormats.AsNoTracking(),
                        x => x.gsvc.ColumnId,
                        c => c.ColumnId,
                        (x, c) => new ConditionalFormatModel
                        {
                            Id = c.Id,
                            ColumnId = c.ColumnId,
                            OperatorId = c.OperatorId,
                            MinValue = c.MinValue,
                            MaxValue = c.MaxValue,
                            BackColor = c.BackColor,
                            ForeColor = c.ForeColor,
                            StringValue = c.StringValue,
                            Value = c.Value,
                        })
                    .Distinct()
                    .ToList();

                logger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusSQLStore)} {nameof(GetConditionalFormats)}", correlationId);

                return result;
            }
        }

        /// <summary>
        /// Get the ItemsGroupStatus.
        /// </summary>
        /// <param name="nodeList">The nodeList.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="FacilityTagsEntity"/>.</returns>
        public IList<FacilityTagsModel> GetItemsGroupStatus(string[] nodeList, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusSQLStore)} {nameof(GetItemsGroupStatus)}", correlationId);

            IList<FacilityTagsModel> facilityTags = new List<FacilityTagsModel>();

            using (var context = _contextFactory.GetContext())
            {
                facilityTags = context.FacilityTags.AsNoTracking()
                    .Where(x => x.Enabled && x.GroupStatusView == 1)
                    .Select(a => new FacilityTagsModel
                    {
                        NodeId = a.NodeId,
                        Address = a.Address.ToString(),
                        Description = a.Description,
                        Enabled = a.Enabled,
                        TrendType = a.TrendType,
                        RawLo = a.RawLo,
                        RawHi = a.RawHi,
                        EngLo = a.EngLo,
                        EngHi = a.EngHi,
                        LimitLo = a.LimitLo,
                        LimitHi = a.LimitHi,
                        CurrentValue = a.CurrentValue,
                        EngUnits = a.EngUnits,
                        UpdateDate = a.UpdateDate,
                        Writeable = a.Writeable,
                        Topic = a.Topic,
                        GroupNodeId = a.GroupNodeId,
                        DisplayOrder = a.DisplayOrder,
                        AlarmState = a.AlarmState,
                        AlarmAction = a.AlarmAction,
                        WellGroupName = a.WellGroupName,
                        PagingGroup = a.PagingGroup,
                        AlarmArg = a.AlarmArg,
                        AlarmTextLo = a.AlarmTextLo,
                        AlarmTextHi = a.AlarmTextHi,
                        AlarmTextClear = a.AlarmTextClear,
                        GroupStatusView = a.GroupStatusView,
                        ResponderListId = a.ResponderListId,
                        VoiceTextLo = a.VoiceTextLo,
                        VoiceTextHi = a.VoiceTextHi,
                        VoiceTextClear = a.VoiceTextClear,
                        DataType = a.DataType,
                        Decimals = a.Decimals,
                        VoiceNodeId = a.VoiceNodeId,
                        ContactListId = a.ContactListId,
                        Bit = a.Bit,
                        Deadband = a.Deadband,
                        DestinationType = a.DestinationType,
                        StateId = a.StateId,
                        CaptureType = a.CaptureType,
                        LastCaptureDate = a.LastCaptureDate,
                        NumConsecAlarmsAllowed = a.NumConsecAlarmsAllowed,
                        NumConsecAlarms = a.NumConsecAlarms,
                        UnitType = a.UnitType,
                        Name = a.Name,
                        FacilityTagGroupId = a.FacilityTagGroupId,
                        ParamStandardType = a.ParamStandardType,
                        Latitude = a.Latitude,
                        Longitude = a.Longitude,
                        ArchiveFunction = a.ArchiveFunction,
                        Tag = a.Tag,
                        DetailViewOnly = a.DetailViewOnly,
                        Expression = a.Expression,
                        StringValue = a.StringValue,
                    })
                    .ToList();

                if (nodeList != null)
                {
                    facilityTags = facilityTags.Where(x => nodeList.Contains(x.NodeId)).ToList();
                }

                var result = facilityTags.OrderBy(x => x.NodeId).ThenBy(x => x.Description).ToList();

                logger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusSQLStore)} {nameof(GetItemsGroupStatus)}", correlationId);

                return result;
            }
        }

        /// <summary>
        /// Get the LoadViewParameters by currentViewId.
        /// </summary>
        /// <param name="currentViewId">The currentViewId.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="ParameterItem"/>.</returns>
        public SortedList<string, ParameterItem> LoadViewParameters(string currentViewId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusSQLStore)} {nameof(LoadViewParameters)}", correlationId);

            var sortedList = new SortedList<string, ParameterItem>();

            using (var context = _contextFactory.GetContext())
            {
                if (int.TryParse(currentViewId, out var currentViewIdInt) == false)
                {
                    return sortedList;
                }

                var groupStatusViewEntity = context.GroupStatusView.AsNoTracking().Where(gsv => gsv.ViewId == currentViewIdInt);

                var result = groupStatusViewEntity.AsNoTracking()
                    .Join(context.GroupStatusViewsColumns.AsNoTracking(), gsv => gsv.ViewId, gsvc => gsvc.ViewId,
                        (groupStatusView, groupStatusViewsColumn) => new
                        {
                            groupStatusView,
                            groupStatusViewsColumn,
                        })
                    .Join(context.GroupStatusColumns.AsNoTracking(), x => x.groupStatusViewsColumn.ColumnId, gsc => gsc.ColumnId,
                        (x, groupStatusColumn) => new
                        {
                            x.groupStatusView,
                            x.groupStatusViewsColumn,
                            groupStatusColumn,
                        })
                    .GroupJoin(context.Parameters.AsNoTracking(),
                        x => x.groupStatusColumn.ColumnName == null ? null : x.groupStatusColumn.ColumnName.Trim(),
                        p => p.Address + "-" + (p.Description == null ? null : p.Description.Trim()), (x, parameter) =>
                            new
                            {
                                x.groupStatusView,
                                x.groupStatusViewsColumn,
                                x.groupStatusColumn,
                                parameter,
                            })
                    .SelectMany(x => x.parameter.DefaultIfEmpty(), (x, parameter) => new
                    {
                        x.groupStatusView,
                        x.groupStatusViewsColumn,
                        x.groupStatusColumn,
                        parameter,
                    })
                    .GroupBy(x => new
                    {
                        Address = x.parameter == null ? null : (int?)x.parameter.Address,
                        Description = x.parameter == null || x.parameter.Description == null
                            ? null
                            : x.parameter.Description.Trim(),
                    })
                    .Select(x => new
                    {
                        x.Key.Address,
                        Description = x.Key.Description == null ? null : x.Key.Description.Trim(),
                        UnitType = x.Max(m => m.parameter == null ? null : m.parameter.UnitType),
                        DataType = x.Max(m => m.parameter == null ? null : m.parameter.DataType),
                        StateId = x.Max(m => m.parameter == null ? null : m.parameter.StateId),
                    });

                foreach (var item in result)
                {
                    var parameterItem = new ParameterItem();

                    parameterItem.Address = item.Address.HasValue ? item.Address.Value.ToString() : string.Empty;
                    parameterItem.Description = item.Description ?? string.Empty;
                    parameterItem.UnitType = item.UnitType ?? 0;
                    parameterItem.DataType = item.DataType ?? 0;
                    parameterItem.StateID = item.StateId ?? 0;

                    var key = parameterItem.Address + "-" + parameterItem.Description;
                    sortedList.Add(key, parameterItem);
                }
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusSQLStore)} {nameof(LoadViewParameters)}", correlationId);

            return sortedList;
        }

        /// <summary>Get the columns for the selected View from the database.</summary>
        /// <returns>GroupStatusColumnsModels</returns>
        /// <remarks>The database contains 1-based column indexes, but are not necessarly consecutive, without gaps.
        /// This function orders the position based on the database position, but assigns a sequential index (1-based).</remarks>
        public IList<GroupStatusColumnsModels> LoadViewColumns(string currentViewId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusSQLStore)} {nameof(LoadViewColumns)}", correlationId);

            var groupStatusColumnsModels = new List<GroupStatusColumnsModels>().AsQueryable();

            using (var context = _contextFactory.GetContext())
            {
                groupStatusColumnsModels = context.GroupStatusView.AsNoTracking()
                    .Join(context.GroupStatusViewsColumns.AsNoTracking(), gsv => gsv.ViewId,
                        gsvc => gsvc.ViewId, (gsv, gsvc) => new
                        {
                            GroupStatusViews = gsv,
                            GroupStatusViewsColumns = gsvc,
                        })
                    .Where(x => x.GroupStatusViews.ViewId == int.Parse(currentViewId))
                    .Join(context.GroupStatusColumns.AsNoTracking(), x => x.GroupStatusViewsColumns.ColumnId, gsc => gsc.ColumnId,
                        (x, groupStatusColumns) => new
                        {
                            x.GroupStatusViews,
                            x.GroupStatusViewsColumns,
                            groupStatusColumns,
                        })
                    .Join(context.GroupStatusColumnFormats.AsNoTracking(), x => x.GroupStatusViewsColumns.FormatId,
                        gscf => gscf.FormatId,
                        (x, groupStatusColumnFormats) => new
                        {
                            x.GroupStatusViews,
                            x.GroupStatusViewsColumns,
                            x.groupStatusColumns,
                            groupStatusColumnFormats,
                        })
                    .GroupJoin(context.ParamStandardTypes.AsNoTracking(), x => x.groupStatusColumns.ColumnName,
                        pst => pst.Description,
                        (x, paramStandardTypes) => new
                        {
                            x.GroupStatusViews,
                            x.GroupStatusViewsColumns,
                            x.groupStatusColumns,
                            x.groupStatusColumnFormats,
                            paramStandardTypes,
                        })
                    .SelectMany(x => x.paramStandardTypes.DefaultIfEmpty(), (x, paramStandardTypes) => new
                    {
                        x.GroupStatusViews,
                        x.GroupStatusViewsColumns,
                        x.groupStatusColumns,
                        x.groupStatusColumnFormats,
                        paramStandardTypes,
                    })
                    .Select(x => new GroupStatusColumnsModels
                    {
                        ViewId = x.GroupStatusViews.ViewId,
                        ColumnName = x.groupStatusColumns.ColumnName == null
                            ? string.Empty
                            : x.groupStatusColumns.ColumnName.Trim(),
                        UserId = x.GroupStatusViews.UserId,
                        ColumnId = x.GroupStatusViewsColumns.ColumnId,
                        Width = x.GroupStatusViewsColumns.Width,
                        Position = x.GroupStatusViewsColumns.Position,
                        Orientation = x.GroupStatusViewsColumns.Orientation,
                        Alias = x.groupStatusColumns.Alias,
                        SourceId = x.groupStatusColumns.SourceId,
                        Align = x.groupStatusColumns.Align,
                        Visible = x.groupStatusColumns.Visible,
                        Measure = x.groupStatusColumns.Measure,
                        FormatId = x.GroupStatusViewsColumns.FormatId,
                        Formula = x.groupStatusColumns.Formula,
                        FormatMask = x.groupStatusColumnFormats.FormatMask,
                        ParamStandardType = x.paramStandardTypes == null
                            ? null
                            : x.paramStandardTypes.ParamStandardType,
                        UnitType = 0, //TODO Need to figure out this column
                    }).Distinct()
                    .OrderBy(x => x.Position);

                var result = groupStatusColumnsModels.ToList();

                logger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusSQLStore)} {nameof(LoadViewColumns)}", correlationId);

                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeList">The node List.</param>
        /// <param name="hasCameraAlarm">The flag indicating if camera alarm is present.</param>
        /// <param name="hasOperationalScore">The flag indicating if operational score is present.</param>
        /// <param name="hasRuntimeAverage">The flag indicating if runtime average is present.</param>
        /// <param name="sColumns">The columns to include in the result.</param>
        /// <param name="tables">The tables to query.</param>
        /// <param name="correlationId"></param>
        /// <returns>The list of dictionaries representing the SQL result.</returns>
        public IList<Dictionary<string, object>> BuildSQLCommonResult(IList<string> nodeList, bool hasCameraAlarm,
            bool hasOperationalScore,
            bool hasRuntimeAverage, string sColumns, IList<string> tables, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusSQLStore)} {nameof(BuildSQLCommonResult)}", correlationId);

            if (tables == null)
            {
                logger.WriteCId(Level.Info, "Missing list of tables", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusSQLStore)} {nameof(BuildSQLCommonResult)}", correlationId);

                throw new ArgumentNullException(nameof(tables));
            }

            var sql = $"SELECT {sColumns}, tblNodeMaster.POCType AS [tblNodeMaster.POCType], " +
                "CAST(Case tblNodeMaster.Enabled WHEN 1 THEN 'True' ELSE 'False' END AS VARCHAR(5)) as [Enabled], " +
                "'' as StateID, '' as AlarmState, '' as DataType, '' as AlarmTextClear, " +
                "'' as AlarmTextHi, '' as AlarmTextLo, '' as CurrentValue, '' as Text, '' as BackColor, '' as ForeColor " +
                "FROM dbo.tblWellStatistics RIGHT OUTER JOIN dbo.tblNodeMaster on tblWellStatistics.NodeID = tblNodeMaster.NodeID " +
                "LEFT OUTER JOIN dbo.tblWellDetails on tblNodeMaster.NodeID = tblWellDetails.NodeID " +
                "LEFT OUTER JOIN dbo.tblstrings ON tblNodeMaster.stringid = tblstrings.stringid ";

            if (hasCameraAlarm)
            {
                sql += "LEFT JOIN dbo.vwAggregateCameraAlarmStatus " +
                    "ON tblNodeMaster.NodeID = vwAggregateCameraAlarmStatus.NodeID ";
            }

            if (hasOperationalScore)
            {
                sql += "LEFT JOIN dbo.vwOperationalScoresLast " +
                    "ON tblNodeMaster.NodeID = vwOperationalScoresLast.NodeID ";
            }

            if (hasRuntimeAverage)
            {
                sql += "LEFT JOIN dbo.vw30DayRuntimeAverage " +
                    "ON tblNodeMaster.NodeID = vw30DayRuntimeAverage.NodeID ";
            }

            foreach (var table in tables)
            {
                sql += $"LEFT OUTER JOIN {table} ON tblNodeMaster.NodeID = {table}.NodeID ";
            }

            var commaSeparatedNodeIds = string.Join(",", nodeList.Select(x => $"'{x}'"));

            sql += $"WHERE tblNodeMaster.NodeID in ({commaSeparatedNodeIds}) ";

            sql += "ORDER BY tblNodeMaster.NodeID";

            var list = new List<Dictionary<string, object>>();

            using (var context = _contextFactory.GetContext())
            {
                using (var connection = new SqlConnection(context.Database.GetDbConnection().ConnectionString))
                {
                    try
                    {
                        connection.Open();

                        using (var command = new SqlCommand(sql, connection))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                var result = new DataTable();
                                result.Load(reader);

                                foreach (DataRow row in result.Rows)
                                {
                                    var dict = new Dictionary<string, object>();
                                    foreach (DataColumn column in result.Columns)
                                    {
                                        dict[column.ColumnName] = row[column];
                                    }
                                    list.Add(dict);
                                }

                                logger.WriteCId(Level.Info, $"{nameof(list)}", correlationId);
                            }
                        }
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusSQLStore)} {nameof(BuildSQLCommonResult)}", correlationId);

            return list;
        }

        /// <summary>
        /// Get the Build SQL Parameter Result.
        /// </summary>
        /// <param name="nodeList">The nodeList.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="ParameterTypeResult"/>.</returns>
        public IList<ParameterTypeResult> BuildSQLParameterResult(IList<string> nodeList, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusSQLStore)} {nameof(BuildSQLParameterResult)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var result = context.NodeMasters.AsNoTracking().Where(x => nodeList.Contains(x.NodeId))
                    .Join(context.Parameters.AsNoTracking().Where(x => x.GroupStatusView == 1), nm => nm.PocType,
                        p => p.Poctype, (nodeMaster, parameter) => new
                        {
                            nodeMaster,
                            parameter,
                        })
                    .GroupJoin(context.CurrentRawScans.AsNoTracking(), x => new
                    {
                        x.nodeMaster.NodeId,
                        x.parameter.Address
                    },
                        crsd => new
                        {
                            crsd.NodeId,
                            crsd.Address
                        }, (x, currentRawScanData) => new
                        {
                            x.nodeMaster,
                            x.parameter,
                            currentRawScanData,
                        })
                    .SelectMany(x => x.currentRawScanData.DefaultIfEmpty(), (x, currentRawScanData) => new
                    {
                        x.nodeMaster,
                        x.parameter,
                        currentRawScanData,
                    })
                    .GroupJoin(context.States, x => new
                    {
                        x.parameter.StateId,
                        x.currentRawScanData.Value
                    },
                        s => new
                        {
                            StateId = (int?)s.StateId,
                            Value = (float?)s.Value
                        }, (x, state) => new
                        {
                            x.nodeMaster,
                            x.parameter,
                            x.currentRawScanData,
                            state,
                        })
                    .SelectMany(x => x.state.DefaultIfEmpty(), (x, state) => new
                    {
                        x.nodeMaster,
                        x.parameter,
                        x.currentRawScanData,
                        state,
                    })
                    .Select(x => new
                    {
                        x.state.Text,
                        x.state.BackColor,
                        x.state.ForeColor,
                        x.nodeMaster.NodeId,
                        x.parameter.Address,
                        x.parameter.Description,
                        x.parameter.Decimals,
                        x.currentRawScanData.Value,
                    })
                    .ToList()
                    .Select(x => new ParameterTypeResult()
                    {
                        Text = x.Text,
                        BackColor = x.BackColor,
                        ForeColor = x.ForeColor,
                        NodeId = x.NodeId,
                        Description =
                            x.Address + "-" + (x.Description?.Trim()),
                        Decimal = x.Decimals,
                        V1 = x.Value,
                    });

                logger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusSQLStore)} {nameof(BuildSQLParameterResult)}", correlationId);

                return result.ToList();
            }
        }

        /// <summary>
        /// Get the Build SQL CurrRaw Scan Data by nodelist.
        /// </summary>
        /// <param name="nodeList">The nodeList.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="CurrRawScanDataTypeResult"/>.</returns>
        public IList<CurrRawScanDataTypeResult> BuildSQLCurrRawScanData(IList<string> nodeList, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusSQLStore)} {nameof(BuildSQLCurrRawScanData)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var result = context.Parameters.AsNoTracking()
                    .Join(context.NodeMasters.AsNoTracking(), p => p.Poctype, nm => nm.PocType, (parameter, nodeMaster) => new
                    {
                        parameter,
                        nodeMaster,
                    })
                    .GroupJoin(context.CurrentRawScans.AsNoTracking(), x => new
                    {
                        x.nodeMaster.NodeId,
                        x.parameter.Address
                    },
                    crsd => new
                    {
                        crsd.NodeId,
                        crsd.Address
                    }, (x, currentRawScanData) => new
                    {
                        x.parameter,
                        x.nodeMaster,
                        currentRawScanData,
                    })
                    .SelectMany(x => x.currentRawScanData.DefaultIfEmpty(), (x, currentRawScanData) => new
                    {
                        x.parameter,
                        x.nodeMaster,
                        currentRawScanData,
                    })
                    .Where(x => x.parameter.GroupStatusView == 1 && nodeList.Contains(x.nodeMaster.NodeId)
                        && (x.currentRawScanData.StringValue != null && x.currentRawScanData.StringValue != string.Empty))
                    .OrderBy(x => x.nodeMaster.NodeId)
                    .ThenBy(x => x.parameter.Description.Trim())
                    .ThenBy(x => x.parameter.Address)
                    .Select(x => new CurrRawScanDataTypeResult()
                    {
                        NodeId = x.nodeMaster.NodeId,
                        Description = $"{x.parameter.Address}-{x.parameter.Description.Trim()}",
                        StringValue = x.currentRawScanData.StringValue,
                    }).ToList();

                logger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusSQLStore)} {nameof(BuildSQLCurrRawScanData)}", correlationId);

                return result;
            }
        }

        /// <summary>
        /// Get the Build SQL Facility Result by nodelist.
        /// </summary>
        /// <param name="nodeList">The nodeList.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="FacilityTypeResult"/>.</returns>
        public IList<FacilityTypeResult> BuildSQLFacilityResult(IList<string> nodeList, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusSQLStore)} {nameof(BuildSQLFacilityResult)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var result = context.FacilityTags.AsNoTracking().Join(context.NodeMasters.AsNoTracking(), ft => ft.GroupNodeId, nm => nm.NodeId,
                        (facilityTags, nodeMaster) => new
                        {
                            facilityTags,
                            nodeMaster,
                        })
                    .GroupJoin(context.States.AsNoTracking(), x => new
                    {
                        x.facilityTags.StateId,
                        x.facilityTags.CurrentValue,
                    }, s => new
                    {
                        StateId = (int?)s.StateId,
                        CurrentValue = (float?)s.Value,
                    }, (x, state) => new
                    {
                        x.facilityTags,
                        x.nodeMaster,
                        state,
                    })
                    .SelectMany(x => x.state.DefaultIfEmpty(), (x, state) => new
                    {
                        x.facilityTags,
                        x.nodeMaster,
                        state,
                    })
                    .Where(x => nodeList.Contains(x.facilityTags.GroupNodeId))
                    .Select(x => new FacilityTypeResult()
                    {
                        FacilityNodeId = x.facilityTags.NodeId,
                        GroupNodeId = x.facilityTags.GroupNodeId,
                        AlarmState = x.facilityTags.AlarmState,
                        AlarmTextColor = x.facilityTags.AlarmTextClear,
                        Description = x.facilityTags.Description == null ? null : x.facilityTags.Description.Trim(),
                        AlarmTextLo = x.facilityTags.AlarmTextLo,
                        CurrentValue = x.facilityTags.StringValue ?? x.facilityTags.CurrentValue.ToString(),
                        DataType = x.facilityTags.DataType,
                        StateId = x.facilityTags.StateId,
                        Text = x.state.Text,
                        BackColor = x.state.BackColor,
                        ForeColor = x.state.ForeColor,
                        ParamStandardType = x.facilityTags.ParamStandardType,
                        Decimals = x.facilityTags.Decimals,
                        FacilityTagAlarm = (context.FacilityTags
                            .GroupJoin(context.Parameters, ft => ft.Address, p => p.Address,
                                (facilityTags, parameter) =>
                                    new
                                    {
                                        facilityTags,
                                        parameter,
                                    })
                            .SelectMany(fta => fta.parameter.DefaultIfEmpty(), (fta, parameter) => new
                            {
                                fta.facilityTags,
                                parameter,
                            })
                            .Where(fta =>
                                fta.facilityTags.GroupNodeId == x.facilityTags.GroupNodeId &&
                                fta.facilityTags.AlarmState > 0 &&
                                (fta.parameter.Poctype == x.nodeMaster.PocType ||
                                    fta.parameter.Poctype == 99))
                            .OrderBy(fta => fta.facilityTags.Description.Trim())
                            .FirstOrDefault().facilityTags.Description.Trim()),
                    }).ToList();

                logger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusSQLStore)} {nameof(BuildSQLFacilityResult)}", correlationId);

                return result;
            }
        }

        /// <summary>
        /// Get the Build SQL Param Standard Type Result.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="nodeList">The nodeList.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="ParamStandardTypeSumResult"/>.</returns>
        public IList<ParamStandardTypeSumResult> BuildSQLParamStandardTypeResult(IList<FieldParamStandardTypeNameValues> list,
            IList<string> nodeList, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusSQLStore)} {nameof(BuildSQLParamStandardTypeResult)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                IQueryable<ParamStandardTypeWorkingResult> latestValuesAllQuery = null;
                foreach (var latestValuesWorkingQuery in from item in list
                                                         let latestValuesWorkingQuery = context
                                                             .TvfGetLatestValuesByParamStandardType(item.ParamStandardTypeId)
                                                             .Where(x => nodeList.Contains(x.NodeId))
                                                             .Select(x => new ParamStandardTypeWorkingResult()
                                                             {
                                                                 NodeId = x.NodeId,
                                                                 Value = x.Value,
                                                                 Formula = x.Value,
                                                                 ParamStandardTypeId = item.ParamStandardTypeId,
                                                             })
                                                         select latestValuesWorkingQuery)
                {
                    latestValuesAllQuery = latestValuesAllQuery == null
                        ? latestValuesWorkingQuery
                        : latestValuesAllQuery.Union(latestValuesWorkingQuery);
                }

                var latestValuesSumQuery = latestValuesAllQuery
                    .GroupBy(x => new
                    {
                        x.NodeId,
                        x.ParamStandardTypeId
                    }).Select(x => new ParamStandardTypeSumResult
                    {
                        NodeId = x.Key.NodeId,
                        ParamStandardTypeId = x.Key.ParamStandardTypeId,
                        SumValue = x.Sum(s => s.Value),
                    }).ToList();

                logger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusSQLStore)} {nameof(BuildSQLParamStandardTypeResult)}", correlationId);

                if (latestValuesSumQuery != null && latestValuesSumQuery.Any())
                {
                    return latestValuesSumQuery;
                }
                else
                {
                    return new List<ParamStandardTypeSumResult>();
                }
            }
        }

        /// <summary>
        /// Get the Build SQL Param Standard Type State Result.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="nodeList">The nodeList.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="ParamStandardTypeMaxResult"/>.</returns>
        public IList<ParamStandardTypeMaxResult> BuildSQLParamStandardTypeStateResult(
            IList<FieldParamStandardTypeNameValues> list,
            IList<string> nodeList, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusSQLStore)} {nameof(BuildSQLParamStandardTypeStateResult)}", correlationId);

            IQueryable<ParamStandardTypeStateWorkingResult> latestValuesStatesAllQuery = null;

            using (var context = _contextFactory.GetContext())
            {
                foreach (var latestValuesStatesWorkingQuery in from item in list
                                                               let latestValuesStatesWorkingQuery = context
                                                                   .TvfGetLatestValuesByParamStandardTypeWithStateIdText(
                                                                       item.ParamStandardTypeId)
                                                                   .Where(x => nodeList.Contains(x.NodeId))
                                                                   .Select(x => new ParamStandardTypeStateWorkingResult()
                                                                   {
                                                                       NodeId = x.NodeId,
                                                                       Value = x.Value,
                                                                       Text = x.Text,
                                                                       ParamStandardTypeId = item.ParamStandardTypeId,
                                                                   })
                                                               select latestValuesStatesWorkingQuery)
                {
                    latestValuesStatesAllQuery = latestValuesStatesAllQuery == null
                        ? latestValuesStatesWorkingQuery
                        : latestValuesStatesAllQuery.Union(latestValuesStatesWorkingQuery);
                }

                var latestValuesMaxQuery = latestValuesStatesAllQuery
                    .GroupBy(x => new
                    {
                        x.NodeId,
                        x.ParamStandardTypeId
                    }).Select(x => new ParamStandardTypeMaxResult()
                    {
                        NodeId = x.Key.NodeId,
                        ParamStandardTypeId = x.Key.ParamStandardTypeId,
                        MaxValue = x.Max(s => s.Text),
                    }).ToList();

                logger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusSQLStore)} {nameof(BuildSQLParamStandardTypeStateResult)}", correlationId);

                return latestValuesMaxQuery;
            }
        }

        /// <summary>
        /// Get the Facility Param Standard Types.
        /// </summary>
        /// <param name="nodeList">The nodeList.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="string"/>.</returns>
        public SortedList<string, int?> GetFacilityParamStandardTypes(string[] nodeList, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusSQLStore)} {nameof(GetFacilityParamStandardTypes)}", correlationId);

            var paramStandardTypes = new SortedList<string, int?>();

            using (var context = _contextFactory.GetContext())
            {
                var result = context.FacilityTags.AsNoTracking().GroupJoin(context.ParamStandardTypes.AsNoTracking(),
                        ft => ft.ParamStandardType, pst => pst.ParamStandardType, (facilityTags, paramStandardTypes) =>
                            new
                            {
                                facilityTags,
                                paramStandardTypes,
                            })
                    .SelectMany(x => x.paramStandardTypes.DefaultIfEmpty(),
                        (x, paramStandardTypes) => new
                        {
                            x.facilityTags,
                            paramStandardTypes,
                        })
                    .Where(x => nodeList.Contains(x.facilityTags.GroupNodeId))
                    .GroupBy(x => x.facilityTags.Description)
                    .Select(x => new
                    {
                        Description = x.Key,
                        ParamStandardType = x.Max(p =>
                            p.paramStandardTypes == null ? null : p.paramStandardTypes.ParamStandardType),
                    }).ToList();

                foreach (var item in result)
                {
                    paramStandardTypes.Add($"tblFacilityTags.{item.Description}", 1);
                }
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusSQLStore)} {nameof(GetFacilityParamStandardTypes)}", correlationId);

            return paramStandardTypes;
        }

        /// <summary>
        /// Provides the GroupStatusFacilityTag.
        /// </summary>
        /// <param name="groupstatus"></param>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        public string GroupStatusFacilityTag(string groupstatus, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusSQLStore)} {nameof(GroupStatusFacilityTag)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var result = context.SystemParameters.AsNoTracking().FirstOrDefault(x => x.Parameter == groupstatus)?.Value;

                logger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusSQLStore)} {nameof(GroupStatusFacilityTag)}", correlationId);

                return result;
            }

        }

        /// <summary>Get the additional common columns defined in the database.</summary>
        /// <param name="correlationId"></param>
        /// <returns>A hashtable of columns combining the .</returns>
        /// <remarks>The database contains 1-based column indexes, but are not necessarly consecutive, without gaps.
        /// This function orders the position based on the database position, but assigns a sequential index (1-based).</remarks>
        public Hashtable LoadCommonColumns(string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusSQLStore)} {nameof(LoadCommonColumns)}", correlationId);

            var commonColumns = new Hashtable();

            using (var context = _contextFactory.GetContext())
            {
                var result = context.GroupStandardColumns.AsNoTracking().ToList();

                foreach (var item in result)
                {
                    commonColumns.Add(item.Name, item.Definition);
                }
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusSQLStore)} {nameof(LoadCommonColumns)}", correlationId);

            return commonColumns;
        }

        /// <summary>
        /// Get the Available views  by UserId .
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="List{AvailableViewModel}" />.</returns>
        public IList<AvailableViewModel> GetAvailableViewsByUserId(string userId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusSQLStore)} {nameof(GetAvailableViewsByUserId)}", correlationId);

            List<AvailableViewModel> avaiableViews = new List<AvailableViewModel>();
            using (var context = _contextFactory.GetContext())
            {
                // TODO Missing check for UserName "WHERE (v.UserID = @UserName) " &
                var result = context.GroupStatusView.AsNoTracking().GroupJoin(context.GroupStatusUserView.AsNoTracking(), gsv => gsv.ViewId,
                        gsuv => gsuv.ViewId, (groupStatusView, groupStatusUserView) => new
                        {
                            groupStatusView,
                            groupStatusUserView,
                        })
                    .SelectMany(x => x.groupStatusUserView.DefaultIfEmpty(),
                        (x, groupStatusUserView) => new
                        {
                            x.groupStatusView,
                            groupStatusUserView,
                        })
                    .Where(x => x.groupStatusView.UserId == "Global")
                    .Select(x => new
                    {
                        x.groupStatusView.UserId,
                        x.groupStatusView.ViewName,
                        GroupName = x.groupStatusUserView != null ? x.groupStatusUserView.GroupName : string.Empty,
                        x.groupStatusView.ViewId
                    }).Distinct()
                    .OrderBy(x => x.ViewName)
                    .GroupBy(x => x.ViewId)
                    .Select(x => x.First())
                    .ToList()
                .OrderBy(x => x.ViewName);

                UserDefaultEntity viewIDSelected = context.UserDefaults
                    .Where(x => x.UserId == userId
                    && x.DefaultsGroup == "frmGroupStatus"
                    && x.Property == "LastView").FirstOrDefault();

                avaiableViews = result.Select(item => new AvailableViewModel()
                {
                    ViewId = item.ViewId,
                    ViewName = item.ViewName,
                    IsSelectedView = viewIDSelected != null && viewIDSelected.Value == item.ViewId.ToString(),
                }).OrderByDescending(x => x.IsSelectedView).ToList();
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusSQLStore)} {nameof(GetAvailableViewsByUserId)}", correlationId);

            return avaiableViews;
        }

        /// <summary>
        /// Gets the view tables.
        /// </summary>
        /// <param name="correlationId"></param>
        /// <returns>The list of <seealso cref="GroupStatusTableModel"/>.</returns>
        public IList<GroupStatusTableModel> GetViewTables(string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusSQLStore)} {nameof(GetViewTables)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var entities = context.GroupStatusTable.AsNoTracking();

                var result = entities.ToList().Select(x => new GroupStatusTableModel()
                {
                    TableId = x.TableId,
                    TableName = x.TableName,
                }).ToList();

                logger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusSQLStore)} {nameof(GetViewTables)}", correlationId);

                return result;
            }
        }

        /// <summary>
        /// Gets the classifications data for the group widget.
        /// </summary>
        /// <param name="assetIds">The list of node ids in the group.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="assetCount">The count of unique assets.</param>
        /// <returns>The <seealso cref="IList{AssetGroupStatusClassificationModel}"/>.</returns>
        public IList<AssetGroupStatusClassificationModel> GetClassificationsData(List<string> assetIds,
            string correlationId, out int assetCount)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusSQLStore)} {nameof(GetClassificationsData)}", correlationId);
            assetCount = 0;
            using (var context = _contextFactory.GetContext())
            {
                var uniqueAssets = new List<string>();
                var queryResult = context.AnalyticsClassification.AsNoTracking()
                    .Join(context.AnalyticsClassificationType.AsNoTracking(),
                    ac => ac.ClassificationTypeId, act => act.Id,
                    (ac, act) => new { ac, act })
                    .Join(context.LocalePhrases.AsNoTracking(),
                    acAct => acAct.act.PhraseID,
                    l => l.PhraseId, (acAct, l) => new { acAct.ac, acAct.act, l })
                    .Where(acActL => assetIds.Contains(acActL.ac.NodeId)
                    && (acActL.ac.EndDate >= DateTime.UtcNow.AddDays(-15) || acActL.ac.EndDate == null))
                    .AsQueryable();

                var classificationData = queryResult.OrderBy(x => x.act.Priority)
                    .Select(analytics => new AssetGroupStatusClassificationModel
                    {
                        Id = analytics.act.Id,
                        Name = analytics.l.English,
                        Priority = analytics.act.Priority,
                    }).Distinct().ToList();
                var totalAssetCount = 0;
                foreach (var item in classificationData)
                {
                    var countByAsset = queryResult
                    .Where(x => x.ac.ClassificationTypeId == item.Id)
                    .Select(data => new
                    {
                        assetcount = data.ac.NodeId
                    }).Distinct().ToArray();
                    totalAssetCount += countByAsset.Length;
                    item.Count = countByAsset.Length;

                    var assets = queryResult
                    .Where(x => x.ac.ClassificationTypeId == item.Id)
                    .Select(data => new
                    {
                        asset = data.ac.NodeId.ToString()
                    }).Distinct();

                    uniqueAssets.AddRange(assets.Select(x => x.asset).ToList());
                }

                foreach (var item in classificationData.OrderBy(x => x.Priority))
                {
                    item.Percent = item.Count / totalAssetCount * 100;
                }
                assetCount = uniqueAssets.Distinct().Count();

                logger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusSQLStore)} {nameof(GetClassificationsData)}", correlationId);

                return classificationData.OrderBy(x => x.Priority).ToList();
            }
        }

        /// <summary>
        /// Gets the active alarms data for the group widget.
        /// </summary>
        /// <param name="assetIds">The list of node ids in the group.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="AssetGroupStatusAlarmsModel"/>.</returns>
        public async Task<AssetGroupStatusAlarmsModel> GetAlarmsData(List<string> assetIds, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GroupStatusSQLStore)} {nameof(GetAlarmsData)}", correlationId);

            if (assetIds == null || assetIds.Count == 0)
            {
                logger.WriteCId(Level.Info, "Missing asset ids", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusSQLStore)} {nameof(GetAlarmsData)}", correlationId);

                return null;
            }

            var alarmConfiguration = await GetAlarmConfigurationAsync(assetIds);
            var hostAlarms = await GetHostAlarmsAsync(assetIds);
            var facilityTagAlarms = await GetFacilityTagAlarmsAsync(assetIds);
            var cameraAlarms = await GetCameraAlarmsAsync(assetIds);

            var totalAlarms = alarmConfiguration + hostAlarms + facilityTagAlarms + cameraAlarms;

            var percent = ((float)totalAlarms / (assetIds.Count * 4)) * 100;

            var result = new AssetGroupStatusAlarmsModel
            {
                Id = 1,
                Name = "Alarm",
                Count = totalAlarms,
                Percent = percent
            };

            logger.WriteCId(Level.Trace, $"Finished {nameof(GroupStatusSQLStore)} {nameof(GetAlarmsData)}", correlationId);

            return result;
        }

        #endregion

        #region Private Methods

        private async Task<int> GetAlarmConfigurationAsync(List<string> assetIds)
        {
            if (assetIds == null || assetIds.Count == 0)
            {
                return 0;
            }

            await using (var context = _contextFactory.GetContext())
            {
                var result = context.NodeMasters.AsNoTracking().Join(context.AlarmConfigByPocType.AsNoTracking(), l => l.PocType, r => r.PocType,
                        (node, alarm) => new
                        {
                            node,
                            alarm,
                        })
                    .Join(context.CurrentRawScans.AsNoTracking(), l => new
                    {
                        Key1 = l.alarm.Register,
                        Key2 = l.node.NodeId,
                    },
                    r => new
                    {
                        Key1 = r.Address,
                        Key2 = r.NodeId,
                    }, (alarm, scan) => new
                    {
                        alarm,
                        scan,
                    })
                    .Join(context.Parameters.AsNoTracking(), l => new
                    {
                        Key1 = l.alarm.alarm.PocType,
                        Key2 = l.alarm.alarm.Register,
                    }
                    , r => new
                    {
                        Key1 = r.Poctype,
                        Key2 = r.Address,
                    },
                    (alarmWithScan, parameters) => new
                    {
                        alarmWithScan,
                        parameters,
                    })
                    .GroupJoin(context.States.AsNoTracking(), l => new
                    {
                        Key1 = l.parameters.StateId,
                        Key2 = l.alarmWithScan.scan.Value,
                    }, r => new
                    {
                        Key1 = (int?)r.StateId,
                        Key2 = (float?)r.Value,
                    }, (alarmWithParameter, state) => new
                    {
                        alarmWithParameter,
                        state,
                    })
                    .SelectMany(m => m.state.DefaultIfEmpty(), (left, right) => new
                    {
                        left,
                        right,
                    })
                    .Where(m => assetIds.Contains(m.left.alarmWithParameter.alarmWithScan.alarm.node.NodeId) &&
                                m.left.alarmWithParameter.alarmWithScan.alarm.alarm.Enabled &&
                                m.left.alarmWithParameter.alarmWithScan.scan.DateTimeUpdated >= DateTime.UtcNow.AddDays(-15))
                    .Select(n => new { n.left.alarmWithParameter.alarmWithScan.alarm.node.NodeId }).Distinct()
                    .Count();

                return result;
            }
        }

        private async Task<int> GetHostAlarmsAsync(List<string> assetIds)
        {
            if (assetIds == null || assetIds.Count == 0)
            {
                return 0;
            }

            await using (var context = _contextFactory.GetContext())
            {
                var nodes = await context.NodeMasters.AsNoTracking()
                    .Where(n => assetIds.Contains(n.NodeId))
                    .Select(m => new NodeMasterModel()
                    {
                        AssetGuid = m.AssetGuid,
                        NodeId = m.NodeId,
                        PocType = m.PocType,
                    }).ToListAsync();

                if (nodes == null)
                {
                    return 0;
                }
                var nodeIds = nodes.Select(n => n.NodeId).ToArray();

                var allAlarmCount = 0;

                var parameterHostAlarms = context.NodeMasters.AsNoTracking()
                                    .Where(n => nodeIds.Contains(n.NodeId))
                                    .GroupJoin(context.HostAlarm.AsNoTracking()
                                    .Where(h => h.Enabled == true && h.Address != null),
                                        n => n.NodeId,
                                        h => h.NodeId,
                                        (n, hostAlarms) => new { n, hostAlarms })
                                    .SelectMany(nha => nha.hostAlarms.DefaultIfEmpty(),
                                        (nha, h) => new { nha.n, h })
                                    .GroupJoin(context.Parameters.AsNoTracking(),
                                        nh => nh.h.Address,
                                        p => p.Address,
                                        (nh, parameters) => new { nh.n, nh.h, parameters })
                                    .SelectMany(nhh => nhh.parameters
                                        .Where(p => nhh.n.PocType == p.Poctype || p.Poctype == 99)
                                        .DefaultIfEmpty(),
                                        (nhh, p) => new { nhh.n, nhh.h, p })
                                    .GroupJoin(context.LocalePhrases.AsNoTracking(),
                                        nhp => nhp.p.PhraseId,
                                        l => l.PhraseId,
                                        (nhp, localePhrases) => new { nhp.n, nhp.h, nhp.p, localePhrases })
                                    .SelectMany(nhlp => nhlp.localePhrases.DefaultIfEmpty(),
                                        (nhlp, l) => new { nhlp.n, nhlp.h, nhlp.p, l })
                                    .GroupJoin(context.FacilityTags.AsNoTracking(),
                                        nhpl => nhpl.h.Address,
                                        ft => ft.Address,
                                        (nhpl, facilityTags) => new { nhpl.n, nhpl.h, nhpl.p, nhpl.l, facilityTags })
                                    .SelectMany(nhplt => nhplt.facilityTags.DefaultIfEmpty(),
                                        (nhplt, ft) => new { nhplt.n, nhplt.h, nhplt.p, nhplt.l, ft })
                                    .Where(nhpltft => nhpltft.ft != null || nhpltft.p != null)
                                    .Select(nhpltft => nhpltft.n.NodeId)
                                    .Distinct()
                                    .ToList();

                var xDiagAlarms = context.NodeMasters.AsNoTracking()
                                    .Where(n => nodeIds.Contains(n.NodeId))
                                    .GroupJoin(context.HostAlarm.AsNoTracking().Where(h => h.Enabled == true && h.Address == null),
                                        n => n.NodeId,
                                        h => h.NodeId,
                                        (n, hostAlarms) => new { n, hostAlarms })
                                    .SelectMany(nha => nha.hostAlarms.DefaultIfEmpty(),
                                        (nha, h) => new { nha.n, h })
                                    .GroupJoin(context.XDIAGOutputs.AsNoTracking(),
                                        nh => nh.h.XdiagOutputsId,
                                        x => x.Id,
                                        (nh, xDiagOutputs) => new { nh.n, nh.h, xDiagOutputs })
                                    .SelectMany(nhx => nhx.xDiagOutputs.DefaultIfEmpty(),
                                        (nhx, x) => new { nhx.n, nhx.h, x })
                                    .Where(nhx => nhx.h != null && nhx.h.Address == null
                                    && nhx.n.NodeId != nhx.h.NodeId && nhx.h.Enabled == true)
                                    .Select(nhx => nhx.n.NodeId)
                                    .Distinct()
                                    .ToList();

                var allAlarms = parameterHostAlarms.Union(xDiagAlarms)?.Distinct();

                if (allAlarms != null)
                {
                    allAlarmCount = allAlarms.Count();
                }

                return allAlarmCount;
            }
        }

        private async Task<int> GetFacilityTagAlarmsAsync(List<string> assetIds)
        {
            if (assetIds == null || assetIds.Count == 0)
            {
                return 0;
            }

            await using (var context = _contextFactory.GetContext())
            {
                var node = await context.NodeMasters.AsNoTracking()
                    .Where(n => assetIds.Contains(n.NodeId))
                    .Select(m => new NodeMasterModel()
                    {
                        AssetGuid = m.AssetGuid,
                        NodeId = m.NodeId,
                        PocType = m.PocType,
                    }).ToListAsync();

                if (node == null)
                {
                    return 0;
                }
                var pocTypes = node.Select(x => x.PocType.ToString()).ToArray();

                var result = context.FacilityTags.AsNoTracking().GroupJoin(context.Parameters.AsNoTracking(), l => l.Address, r => r.Address,
                        (fac, param) => new
                        {
                            fac,
                            param,
                        })
                    .SelectMany(m => m.param.DefaultIfEmpty(), (fac, param) => new
                    {
                        fac,
                        param,
                    })
                    .Where(m => ((m.fac.fac.GroupNodeId == null && assetIds.Contains(m.fac.fac.NodeId)) ||
                                 assetIds.Contains(m.fac.fac.GroupNodeId)) && m.fac.fac.AlarmState > 0 &&
                                (pocTypes.Contains(m.param.Poctype.ToString()) || m.param.Poctype == 99) &&
                                m.fac.fac.UpdateDate >= DateTime.UtcNow.AddDays(-15))
                    .Select(m => m.fac.fac.NodeId).Distinct()
                    .Count();

                return result;
            }
        }

        private async Task<int> GetCameraAlarmsAsync(List<string> assetIds)
        {
            if (assetIds == null || assetIds.Count == 0)
            {
                return 0;
            }

            await using (var context = _contextFactory.GetContext())
            {
                var cameras = context.NodeMasters.AsNoTracking().Join(context.Cameras.AsNoTracking(), l => l.NodeId, r => r.NodeId, (node, camera) =>
                        new
                        {
                            node,
                            camera,
                        })
                    .Where(m => assetIds.Contains(m.node.NodeId));
                var cameraAlarms = context.CameraAlarm.AsNoTracking().Where(m => m.IsEnabled).Select(m => new
                {
                    m.Id,
                    m.CameraId,
                    m.AlarmType,
                });

                var alarmEventsGrouped = context.AlarmEvent.AsNoTracking().Where(m => m.AlarmType == 1).GroupBy(m => m.AlarmId)
                    .Select(m => new
                    {
                        AlarmId = m.Key,
                        EventDateTime = m.Max(x => x.EventDateTime),
                    }).Where(x => x.EventDateTime >= DateTime.UtcNow.AddDays(-15))
                    .Join(context.AlarmEvent.AsNoTracking(), l => new
                    {
                        Key1 = l.AlarmId,
                        Key2 = l.EventDateTime,
                    }, r => new
                    {
                        Key1 = r.AlarmId,
                        Key2 = r.EventDateTime,
                    }, (left, right) => new
                    {
                        left,
                        right,
                    }).Where(m => m.right.AcknowledgedDateTime == null);

                var result = cameras.Join(cameraAlarms.AsNoTracking(), l => l.camera.Id, r => r.CameraId, (camera, cameraAlarm) => new
                {
                    camera,
                    cameraAlarm,
                }).Join(context.CameraAlarmType.AsNoTracking(), l => l.cameraAlarm.AlarmType, r => r.Id, (left, right) => new
                {
                    left,
                    right,
                })
                    .GroupJoin(context.LocalePhrases.AsNoTracking(), l => l.right.PhraseId, r => r.PhraseId, (left, right) => new
                    {
                        left,
                        right,
                    })
                    .SelectMany(m => m.right.DefaultIfEmpty(), (left, right) => new
                    {
                        left,
                        right,
                    })
                    .Join(alarmEventsGrouped, l => l.left.left.left.cameraAlarm.Id, r => r.right.AlarmId,
                        (left, right) => new
                        {
                            left,
                            right,
                        })
                    .Select(m => m.left.left.left.left.camera.node.NodeId).Distinct();

                return result.Count();
            }
        }

        #endregion

    }
}
