using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Alarms;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;

namespace Theta.XSPOC.Apex.Api.Data.Sql.Alarm
{
    /// <summary>
    /// This is the SQL implementation that defines the methods for the alarm store.
    /// </summary>
    public class AlarmSQLStore : IAlarmStore
    {

        #region Private Fields

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _thetaDbContextFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="AlarmSQLStore"/> using the provided
        /// <paramref name="thetaDbContextFactory"/>.
        /// </summary>
        /// <param name="thetaDbContextFactory">The theta db context factory used to get a db context.</param>
        /// <exception cref="ArgumentNullException">
        /// when <paramref name="thetaDbContextFactory"/> is null.
        /// </exception>
        public AlarmSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> thetaDbContextFactory)
        {
            _thetaDbContextFactory =
                thetaDbContextFactory ?? throw new ArgumentNullException(nameof(thetaDbContextFactory));
        }

        #endregion

        #region IAlarmConfigRepository Implementation

        /// <summary>
        /// Gets the list of alarm configuration for the provided <paramref name="assetId"/>
        /// </summary>
        /// <param name="assetId">The asset id used to get the alarm configuration data.</param>
        /// <param name="customerId">The customer id.</param>
        /// <returns>
        /// A <seealso cref="IList{AlarmConfig}"/> that contains the alarm configuration data for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then and empty list is returned. If
        /// the <paramref name="assetId"/> is the default GUID then null is returned.
        /// </returns>
        public async Task<IList<AlarmData>> GetAlarmConfigurationAsync(Guid assetId, Guid customerId)
        {
            if (assetId == Guid.Empty)
            {
                return null;
            }

            await using (var context = _thetaDbContextFactory.GetContext())
            {
                var result = context.NodeMasters.Join(context.AlarmConfigByPocTypes, l => l.PocType, r => r.PocType,
                        (node, alarm) => new
                        {
                            node,
                            alarm,
                        })
                    .Join(context.CurrentRawScans, l => new
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
                    .Join(context.Parameters, l => new
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
                    .GroupJoin(context.States, l => new
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
                    .Where(m => m.left.alarmWithParameter.alarmWithScan.alarm.node.AssetGuid == assetId &&
                                m.left.alarmWithParameter.alarmWithScan.alarm.alarm.Enabled)
                    .Select(m => new AlarmData()
                    {
                        AlarmType = "RTU",
                        AlarmRegister = m.left.alarmWithParameter.alarmWithScan.alarm.alarm.Register.ToString(),
                        AlarmBit = m.left.alarmWithParameter.alarmWithScan.alarm.alarm.Bit,
                        AlarmDescription = m.left.alarmWithParameter.alarmWithScan.alarm.alarm.Description,
                        AlarmPriority = m.left.alarmWithParameter.alarmWithScan.alarm.alarm.Priority,
                        AlarmNormalState = m.left.alarmWithParameter.alarmWithScan.alarm.alarm.NormalState
                            ? 1
                            : 0,
                        CurrentValue = m.left.alarmWithParameter.alarmWithScan.scan.Value != null
                            ? (float)m.left.alarmWithParameter.alarmWithScan.scan.Value
                            : 0,
                        StateText = m.right == null ? null : m.right.Text,
                    });

                return result.ToList();
            }
        }

        /// <summary>
        /// Gets the list of host alarms for the provided <paramref name="assetId"/>
        /// </summary>
        /// <param name="assetId">The asset id used to get the alarm configuration data.</param>
        /// <param name="customerId">The customer id.</param>
        /// <returns>
        /// A <seealso cref="IList{AlarmConfig}"/> that contains the host alarm data for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then and empty list is returned. If
        /// the <paramref name="assetId"/> is the default GUID then null is returned.
        /// </returns>
        public async Task<IList<AlarmData>> GetHostAlarmsAsync(Guid assetId, Guid customerId)
        {
            if (assetId == Guid.Empty)
            {
                return null;
            }

            await using (var context = _thetaDbContextFactory.GetContext())
            {
                var node = await context.NodeMasters
                    .Select(m => new NodeProjected()
                    {
                        AssetGUID = m.AssetGuid,
                        NodeId = m.NodeId,
                        PocType = m.PocType,
                    })
                    .FirstOrDefaultAsync(m => m.AssetGUID == assetId);

                if (node == null)
                {
                    return new List<AlarmData>();
                }

                var parameterHostAlarms = context.HostAlarm.GroupJoin(context.Parameters, l => new
                {
                    Key1 = l.Address,
                    Key2 = l.NodeId,
                }, r => new
                {
                    Key1 = (int?)r.Address,
                    Key2 = node.NodeId,
                }, (hostAlarm, parameter) => new
                {
                    hostAlarm,
                    parameter,
                })
                .SelectMany(m => m.parameter.DefaultIfEmpty(), (hostAlarms, parameters) => new
                {
                    hostAlarms,
                    parameters,
                })
                .Where(m => m.parameters != null)
                .Where(m => m.parameters.Poctype == node.PocType || m.parameters.Poctype == 99)
                .GroupJoin(context.LocalePhrases, l => l.parameters.PhraseId, r => r.PhraseId,
                    (hostAlarms, localPhrases) => new
                    {
                        hostAlarms,
                        localPhrases,
                    }).SelectMany(m => m.localPhrases.DefaultIfEmpty(), (hostAlarms, localPhrases) => new
                    {
                        hostAlarms,
                        localPhrases,
                    })
                .GroupJoin(context.FacilityTags, l => new
                {
                    Key1 = l.hostAlarms.hostAlarms.hostAlarms.hostAlarm.Address,
                    Key2 = l.hostAlarms.hostAlarms.hostAlarms.hostAlarm.NodeId,
                    Key3 = 0,

                }, r => new
                {
                    Key1 = (int?)r.Address,
                    Key2 = r.GroupNodeId ?? r.NodeId,
                    Key3 = r.Bit,
                }, (hostAlarms, facilityTag) => new
                {
                    hostAlarms,
                    facilityTag,
                }).SelectMany(m => m.facilityTag.DefaultIfEmpty(), (hostAlarms, facilityTag) => new
                {
                    hostAlarms,
                    facilityTag,
                })
                .Where(m => m.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarm.Address != null &&
                            (m.hostAlarms.hostAlarms.hostAlarms.hostAlarms.parameters != null ||
                                m.facilityTag != null) &&
                            m.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarm.NodeId ==
                            node.NodeId &&
                            m.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarm.Enabled == true)
                .Select(m => new HostAlarmEntry()
                {
                    Id = m.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarm.Id,
                    IsEnabled = m.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarm.Enabled.Value,
                    SourceId = m.facilityTag != null
                        ? m.facilityTag.Address
                        : m.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarm.Address ?? m.hostAlarms
                            .hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarm.XdiagOutputsId,
                    IsParameter = true,
                    Description = m.facilityTag != null && m.facilityTag.Description != null
                        ? m.facilityTag.Description
                        : m.hostAlarms.hostAlarms.localPhrases != null
                            ? m.hostAlarms.hostAlarms.localPhrases.English
                            : m.hostAlarms.hostAlarms.hostAlarms.hostAlarms.parameters.Description,
                    AlarmType = m.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarm.AlarmType,
                    AlarmState = m.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarm.AlarmState,
                    LoLimit = m.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarm.LoLimit,
                    LoLoLimit = m.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarm.LoLoLimit,
                    HiLimit = m.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarm.HiLimit,
                    HiHiLimit = m.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarm.HiHiLimit,
                    NumberDays = m.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarm.NumDays,
                    ExactValue = m.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarm.ExactValue,
                    ValueChange = m.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarm.ValueChange,
                    PercentChange =
                        m.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarm.PercentChange,
                    SpanLimit = m.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarm.MinToMaxLimit,
                    IgnoreZeroAddress = m.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarm
                        .IgnoreZeroAddress,
                    AlarmAction = m.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarm.AlarmAction,
                    IgnoreValue = m.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarm.IgnoreValue,
                    EmailOnAlarm = 0,
                    EmailOnLimit = 0,
                    EmailOnClear = 0,
                    PageOnAlarm = 0,
                    PageOnLimit = 0,
                    PageOnClear = 0,
                    Priority = m.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarm.Priority,
                    IsPushEnabled =
                        m.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarms.hostAlarm.PushEnabled,
                }).ToList();

                var xdiagHostAlarms = context.HostAlarm.GroupJoin(context.XDIAGOutputs, l => l.XdiagOutputsId,
                        r => r.Id,
                        (hostAlarm, xdiagOutputs) => new
                        {
                            hostAlarm,
                            xdiagOutputs,
                        }).SelectMany(m => m.xdiagOutputs.DefaultIfEmpty(), (hostAlarms, xdiagOutputs) => new
                        {
                            hostAlarms,
                            xdiagOutputs,
                        })
                    .Where(m => m.hostAlarms.hostAlarm.Address == null &&
                                m.hostAlarms.hostAlarm.NodeId == node.NodeId &&
                                m.hostAlarms.hostAlarm.Enabled == true)
                    .Select(m => new HostAlarmEntry()
                    {
                        Id = m.hostAlarms.hostAlarm.Id,
                        IsEnabled = m.hostAlarms.hostAlarm.Enabled.Value,
                        SourceId = m.hostAlarms.hostAlarm.Address ?? m.xdiagOutputs.Id,
                        IsParameter = false,
                        Description = m.xdiagOutputs.Name,
                        AlarmType = m.hostAlarms.hostAlarm.AlarmType,
                        AlarmState = m.hostAlarms.hostAlarm.AlarmState,
                        LoLimit = m.hostAlarms.hostAlarm.LoLimit,
                        LoLoLimit = m.hostAlarms.hostAlarm.LoLoLimit,
                        HiLimit = m.hostAlarms.hostAlarm.HiLimit,
                        HiHiLimit = m.hostAlarms.hostAlarm.HiHiLimit,
                        NumberDays = m.hostAlarms.hostAlarm.NumDays,
                        ExactValue = m.hostAlarms.hostAlarm.ExactValue,
                        ValueChange = m.hostAlarms.hostAlarm.ValueChange,
                        PercentChange = m.hostAlarms.hostAlarm.PercentChange,
                        SpanLimit = m.hostAlarms.hostAlarm.MinToMaxLimit,
                        IgnoreZeroAddress = m.hostAlarms.hostAlarm.IgnoreZeroAddress,
                        AlarmAction = m.hostAlarms.hostAlarm.AlarmAction,
                        IgnoreValue = m.hostAlarms.hostAlarm.IgnoreValue,
                        EmailOnAlarm = 0,
                        EmailOnLimit = 0,
                        EmailOnClear = 0,
                        PageOnAlarm = 0,
                        PageOnLimit = 0,
                        PageOnClear = 0,
                        Priority = m.hostAlarms.hostAlarm.Priority,
                        IsPushEnabled = m.hostAlarms.hostAlarm.PushEnabled,
                    }).ToList();

                var result = parameterHostAlarms.Union(xdiagHostAlarms).OrderByDescending(m => m.Description)
                    .ThenByDescending(m => m.SourceId);

                // This is required to do a ToList before the Select because the columns in the DB are of a different
                // data type and the select before the ToList will cause entity framework to not be able to 
                // generate the SQL.
                return result.ToList().Select(Map).ToList();
            }
        }

        /// <summary>
        /// Gets the list of facility tag alarms for the provided <paramref name="assetId"/>
        /// </summary>
        /// <param name="assetId">The asset id used to get the alarm configuration data.</param>
        /// <param name="customerId">The customer id.</param>
        /// <returns>
        /// A <seealso cref="IList{AlarmConfig}"/> that contains the facility tag alarms data for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then and empty list is returned. If
        /// the <paramref name="assetId"/> is the default GUID then null is returned. 
        /// </returns>
        public async Task<IList<AlarmData>> GetFacilityTagAlarmsAsync(Guid assetId, Guid customerId)
        {
            if (assetId == Guid.Empty)
            {
                return null;
            }

            await using (var context = _thetaDbContextFactory.GetContext())
            {
                var node = await context.NodeMasters.Select(m => new NodeProjected()
                {
                    AssetGUID = m.AssetGuid,
                    NodeId = m.NodeId,
                    PocType = m.PocType,
                }).FirstOrDefaultAsync(m => m.AssetGUID == assetId);

                if (node == null)
                {
                    return new List<AlarmData>();
                }

                var result = context.FacilityTags.GroupJoin(context.Parameters, l => l.Address, r => r.Address,
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
                    .Where(m => ((m.fac.fac.GroupNodeId == null && m.fac.fac.NodeId == node.NodeId) ||
                                 m.fac.fac.GroupNodeId == node.NodeId) && m.fac.fac.AlarmState > 0 &&
                                (m.param.Poctype == node.PocType || m.param.Poctype == 99))
                    .OrderByDescending(m => m.fac.fac.Description)
                    .Select(m => MapFacilityTagAlarm(m.fac.fac.Description, m.fac.fac.AlarmState));

                return result.ToList();
            }
        }

        /// <summary>
        /// Gets the list of camera alarms for the provided <paramref name="assetId"/>
        /// </summary>
        /// <param name="assetId">The asset id used to get the alarm configuration data.</param>
        /// <param name="customerId">The customer id.</param>
        /// <returns>
        /// A <seealso cref="IList{AlarmConfig}"/> that contains the camera alarms data for the provided
        /// <paramref name="assetId"/>. If no data or the asset id is not found then and empty list is returned. If
        /// the <paramref name="assetId"/> is the default GUID then null is returned.
        /// </returns>
        public async Task<IList<AlarmData>> GetCameraAlarmsAsync(Guid assetId, Guid customerId)
        {
            if (assetId == Guid.Empty)
            {
                return null;
            }

            await using (var context = _thetaDbContextFactory.GetContext())
            {
                var cameras = context.NodeMasters.Join(context.Cameras, l => l.NodeId, r => r.NodeId, (node, camera) =>
                        new
                        {
                            node,
                            camera,
                        })
                    .Where(m => m.node.AssetGuid == assetId);
                var cameraAlarms = context.CameraAlarms.Where(m => m.IsEnabled).Select(m => new
                {
                    m.Id,
                    m.CameraId,
                    m.AlarmType,
                });

                var alarmEventsGrouped = context.AlarmEvents.Where(m => m.AlarmType == 1).GroupBy(m => m.AlarmId)
                    .Select(m => new
                    {
                        AlarmId = m.Key,
                        EventDateTime = m.Max(x => x.EventDateTime),
                    })
                    .Join(context.AlarmEvents, l => new
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

                var result = cameras.Join(cameraAlarms, l => l.camera.Id, r => r.CameraId, (camera, cameraAlarm) => new
                {
                    camera,
                    cameraAlarm,
                }).Join(context.CameraAlarmTypes, l => l.cameraAlarm.AlarmType, r => r.Id, (left, right) => new
                {
                    left,
                    right,
                })
                    .GroupJoin(context.LocalePhrases, l => l.right.PhraseId, r => r.PhraseId, (left, right) => new
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
                        }).OrderBy(m => m.left.left.left.right.Id).Select(m =>
                        MapCameraAlarm(m.left.left.left.left.camera.camera.Name, m.left.left.left.right.Name,
                            m.left.right == null ? null : m.left.right.English));

                return result.ToList();
            }
        }

        /// <summary>
        /// Gets the facility tag header and details <paramref name="assetId"/>
        /// </summary>
        /// <param name="assetId">The asset id used to get the facility configuration data.</param>
        /// <returns>The <seealso cref="FacilityDataModel"/> data.</returns>
        public async Task<FacilityDataModel> GetFacilityHeaderAndDetails(Guid assetId)
        {
            if (assetId == Guid.Empty)
            {
                return null;
            }

            var responseData = new FacilityDataModel();

            NodeProjected nodeData;

            using (var context = _thetaDbContextFactory.GetContext())
            {
                nodeData = await context.NodeMasters.Select(m => new NodeProjected()
                {
                    AssetGUID = m.AssetGuid,
                    NodeId = m.NodeId,
                    PocType = m.PocType,
                }).FirstOrDefaultAsync(m => m.AssetGUID == assetId);

                if (nodeData == null)
                {
                    return responseData;
                }

                var nodeId = nodeData.NodeId;

                var sqlNodeData = string.Empty;
                var sqlTagGroups = string.Empty;
                var sqlTagData = string.Empty;

                // Table 1: Node data
                var facilityTagsData = context.NodeMasters
                    .Where(headers => headers.NodeId == nodeData.NodeId)
                    .OrderBy(headers => headers.DisplayName)
                    .Select(headers => new FacilityDataModel
                    {
                        NodeId = headers.NodeId,
                        Facility = headers.DisplayName,
                        CommStatus = headers.CommStatus,
                        Indicator = "",
                        Enabled = headers.Enabled,
                        NodeHostAlarm = "",
                        NodeHostAlarmState = 0,
                        HostAlarmBackColor = "",
                        HostAlarmForeColor = "",
                        TagCount = context.FacilityTags
                        .Count(s1 => (s1.GroupNodeId == headers.NodeId || s1.NodeId == headers.NodeId) && s1.Enabled),
                        AlarmCount = context.FacilityTags
                        .Count(s2 => ((s2.GroupNodeId == headers.NodeId || s2.NodeId == headers.NodeId) && s2.AlarmState != 0 && s2.Enabled)) +
                        context.HostAlarm
                        .Count(h => h.NodeId == headers.NodeId && h.Enabled == true && h.AlarmState > 0),
                        Comment = headers.Comment
                    }).AsEnumerable();

                if (facilityTagsData != null && facilityTagsData.Any())
                {
                    responseData = facilityTagsData.FirstOrDefault();
                    responseData.TagGroups = new List<FacilityTagGroupModel>();
                }

                // Table 2: Tag Group Information
                var tagGroup1 = (from nm in context.NodeMasters
                                 join ft in context.FacilityTags on nm.NodeId equals ft.NodeId into ftGroup
                                 from ft in ftGroup.DefaultIfEmpty()
                                 join ftg in context.FacilityTagGroups on ft.FacilityTagGroupId equals ftg.Id
                                 into ftgGroup
                                 from ftg in ftgGroup.DefaultIfEmpty()
                                 where nm.NodeId == nodeId && ft.FacilityTagGroupId != null
                                 group new { ft, ftg } by new { ftg.DisplayOrder, ftg.Id, ftg.Name, nm.NodeId }
                                 into grp
                                 select new FacilityTagGroupModel
                                 {
                                     DisplayOrder = grp.Key.DisplayOrder.ToString(),
                                     Id = grp.Key.Id.ToString(),
                                     TagGroupName = grp.Key.Name,
                                     TagCount = grp.Sum(x => x.ft.Enabled && (x.ft.NodeId == grp.Key.NodeId || x.ft.GroupNodeId == grp.Key.NodeId) ? 1 : 0).ToString(),
                                     AlarmCount = grp.Sum(x => x.ft.Enabled && (x.ft.NodeId == grp.Key.NodeId || x.ft.GroupNodeId == grp.Key.NodeId) && (x.ft.AlarmState == 1 || x.ft.AlarmState == 2) ? 1 : 0).ToString(),
                                     NodeId = grp.Key.NodeId,
                                     TagGroupNodeId = grp.Key.Name + grp.Key.NodeId,
                                     GroupHostAlarm = "",
                                     HostAlarmBackColor = "",
                                     HostAlarmForeColor = "",
                                     FacilityTags = new List<FacilityTagsModel>()
                                 }).AsEnumerable();

                var tagGroup2 = (from nm in context.NodeMasters
                                 join ft in context.FacilityTags on nm.NodeId equals ft.NodeId into ftGroup
                                 from ft in ftGroup.DefaultIfEmpty()
                                 where nm.NodeId == nodeId && ft.Enabled && ft.FacilityTagGroupId == null
                                 group ft by nm.NodeId into g
                                 select new FacilityTagGroupModel
                                 {
                                     DisplayOrder = "",
                                     Id = "",
                                     TagGroupName = "",
                                     TagCount = g.Sum(x => x.Enabled && (x.NodeId == g.Key || x.GroupNodeId == g.Key) ? 1 : 0).ToString(),
                                     AlarmCount = g.Sum(x => (x.AlarmState == 1 || x.AlarmState == 2) && x.Enabled && (x.NodeId == g.Key || x.GroupNodeId == g.Key) ? 1 : 0).ToString(),
                                     NodeId = g.Key,
                                     TagGroupNodeId = g.Key,
                                     GroupHostAlarm = "",
                                     HostAlarmBackColor = "",
                                     HostAlarmForeColor = "",
                                     FacilityTags = new List<FacilityTagsModel>()
                                 }).AsEnumerable();

                var tagGroups = tagGroup1.Union(tagGroup2)
                    .Select(group => new FacilityTagGroupModel
                    {
                        NodeId = group.NodeId,
                        TagGroupName = group.TagGroupName,
                        AlarmCount = group.AlarmCount,
                        DisplayOrder = group.DisplayOrder,
                        TagCount = group.TagCount,
                        HostAlarmBackColor = group.HostAlarmBackColor,
                        HostAlarmForeColor = group.HostAlarmForeColor,
                        Id = group.Id,
                        GroupHostAlarm = group.GroupHostAlarm,
                        TagGroupNodeId = group.TagGroupNodeId,
                        FacilityTags = new List<FacilityTagsModel>(),
                    });
                responseData.TagGroups = tagGroups.ToList();

                // Table 3: Tags in Tag Groups
                var facilityTags = (from ft in context.FacilityTags
                                    join ftg in context.FacilityTagGroups on ft.FacilityTagGroupId equals ftg.Id
                                    into ftgGroup
                                    from ftg in ftgGroup.DefaultIfEmpty()
                                    join pst in context.ParamStandardTypes on ft.ParamStandardType equals pst.ParamStandardType
                                    into pstd
                                    from pst in pstd.DefaultIfEmpty()
                                    join st in context.States on new { stateId = ft.StateId, value = ft.CurrentValue } equals
                                    new { stateId = (int?)st.StateId, value = (float?)st.Value }
                                    into std
                                    from st in std.DefaultIfEmpty()
                                    where ft.NodeId == nodeId && ft.Enabled
                                    select new
                                    {
                                        ft.NodeId,
                                        TagGroupNodeID = (ft.FacilityTagGroupId == null ? "" : ft.FacilityTagGroupId) + ft.NodeId,
                                        ft.Description,
                                        Value = string.IsNullOrEmpty(ft.StringValue) ? ft.CurrentValue.ToString() : ft.StringValue,
                                        FloatValue = Convert.ToDouble(ft.CurrentValue),
                                        State = st != null ? st.Text : ft.CurrentValue.ToString(),
                                        Units = ft.EngUnits,
                                        UnitType = (pst != null ? pst.UnitTypeId : ft.UnitType),
                                        UnitTypePhrase = "",
                                        HostAlarm = (ft.AlarmState == 1 ? ft.AlarmTextHi : (ft.AlarmState == 2 ? ft.AlarmTextLo : ft.AlarmTextClear)),
                                        LastUpdate = ft.UpdateDate,
                                        WriteAddress = ft.Address,
                                        HostAlarmBackColor = "",
                                        HostAlarmForeColor = "",
                                        Address = string.IsNullOrEmpty(ft.Tag) ? ft.Address.ToString() : ft.Tag.ToString(),
                                        ft.AlarmTextHi,
                                        ft.AlarmTextLo,
                                        ft.AlarmTextClear,
                                        ft.AlarmState,
                                        ft.EngLo,
                                        ft.EngHi,
                                        ft.RawLo,
                                        ft.RawHi,
                                        ft.Enabled,
                                        ft.Writeable,
                                        ft.DataType,
                                        ft.Name,
                                        ft.StateId,
                                        BackColor = (st != null ? st.BackColor : 0),
                                        ForeColor = (st != null ? st.ForeColor : 0),
                                        ft.FacilityTagGroupId,
                                        ft.DisplayOrder,
                                        DisplayOrderIsNull = (ft.DisplayOrder == null ? 1 : 0)
                                    }).AsEnumerable();

                foreach (var group in responseData.TagGroups)
                {
                    var tags = new List<FacilityTagsModel>();
                    foreach (var tag in facilityTags)
                    {
                        if (tag != null)
                        {
                            if (group.NodeId == tag.NodeId
                                && group.Id == tag.FacilityTagGroupId.ToString())
                            {
                                tags.Add(new FacilityTagsModel
                                {
                                    NodeId = tag.NodeId,
                                    Description = tag.Description,
                                    Address = tag.Address.ToString(),
                                    StringValue = tag.Value,
                                    StateId = tag.StateId,
                                    EngUnits = tag.Units,
                                    UnitType = (short)tag.UnitType,
                                    AlarmState = tag.AlarmState,
                                    HostAlarm = tag.HostAlarm,
                                    UpdateDate = tag.LastUpdate,
                                });
                            }
                        }
                    }

                    group.FacilityTags = tags.OrderBy(t => t.Description).ToList();
                }
            }

            return responseData;
        }

        #endregion

        #region Private Methods

        private static AlarmData MapCameraAlarm(string cameraName, string cameraTypeName, string localName)
        {
            return new AlarmData()
            {
                AlarmDescription = $"{cameraName} - {localName ?? cameraTypeName}",
                AlarmType = "Camera",
            };
        }

        private static AlarmData MapFacilityTagAlarm(string description, int? alarmState)
        {
            var alarmStateString = (alarmState) switch
            {
                1 => "-Hi",
                2 => "-Lo",
                _ => string.Empty
            };

            return new AlarmData()
            {
                AlarmDescription = $"{description} {alarmStateString}",
                AlarmType = "Facility Tag",
            };
        }

        private AlarmData Map(HostAlarmEntry hostAlarmEntry)
        {
            if (hostAlarmEntry == null)
            {
                return null;
            }

            return new AlarmData()
            {
                AlarmState = hostAlarmEntry.AlarmState,
                AlarmDescription = hostAlarmEntry.Description,
                AlarmRegister = hostAlarmEntry.SourceId.ToString(),
                AlarmPriority = hostAlarmEntry.Priority ?? 0,
                AlarmType = "Host",
            };
        }

        #endregion

        #region Private Data Structures

        private class HostAlarmEntry
        {

            /// <summary>
            /// Gets or set the id.
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// Gets or set is enabled flag.
            /// </summary>
            public bool IsEnabled { get; set; }

            /// <summary>
            /// Gets or set source id.
            /// </summary>
            public int? SourceId { get; set; }

            /// <summary>
            /// Gets or set is parameter flag.
            /// </summary>
            public bool IsParameter { get; set; }

            /// <summary>
            /// Gets or set the description.
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// Gets or set alarm type.
            /// </summary>
            public int? AlarmType { get; set; }

            /// <summary>
            /// Gets or set alarm state.
            /// </summary>
            public int? AlarmState { get; set; }

            /// <summary>
            /// Gets or set the low limit.
            /// </summary>
            public float? LoLimit { get; set; }

            /// <summary>
            /// Gets or set the low low limit.
            /// </summary>
            public float? LoLoLimit { get; set; }

            /// <summary>
            /// Gets or set the high limit.
            /// </summary>
            public float? HiLimit { get; set; }

            /// <summary>
            /// Gets or set the high high limit.
            /// </summary>
            public float? HiHiLimit { get; set; }

            /// <summary>
            /// Gets or set the number days.
            /// </summary>
            public float? NumberDays { get; set; }

            /// <summary>
            /// Gets or set the exact value.
            /// </summary>
            public float? ExactValue { get; set; }

            /// <summary>
            /// Gets or set the value change.
            /// </summary>
            public float? ValueChange { get; set; }

            /// <summary>
            /// Gets or set the percent change.
            /// </summary>
            public int? PercentChange { get; set; }

            /// <summary>
            /// Gets or set the span limit.
            /// </summary>
            public int? SpanLimit { get; set; }

            /// <summary>
            /// Gets or set the ignore zero address.
            /// </summary>
            public int? IgnoreZeroAddress { get; set; }

            /// <summary>
            /// Gets or set the alarm action.
            /// </summary>
            public int? AlarmAction { get; set; }

            /// <summary>
            /// Gets or set the ignore value.
            /// </summary>
            public float? IgnoreValue { get; set; }

            /// <summary>
            /// Gets or set the email on alarm value.
            /// </summary>
            public int EmailOnAlarm { get; set; }

            /// <summary>
            /// Gets or set the email on limit value.
            /// </summary>
            public int EmailOnLimit { get; set; }

            /// <summary>
            /// Gets or set the email on clear value.
            /// </summary>
            public int EmailOnClear { get; set; }

            /// <summary>
            /// Gets or set the page on alarm value.
            /// </summary>
            public int PageOnAlarm { get; set; }

            /// <summary>
            /// Gets or set the page on limit value.
            /// </summary>
            public int PageOnLimit { get; set; }

            /// <summary>
            /// Gets or set the page on clear value.
            /// </summary>
            public int PageOnClear { get; set; }

            /// <summary>
            /// Gets or set the priority.
            /// </summary>
            public int? Priority { get; set; }

            /// <summary>
            /// Gets or set the is push enabled flag.
            /// </summary>
            public bool? IsPushEnabled { get; set; }

        }

        #endregion

    }
}