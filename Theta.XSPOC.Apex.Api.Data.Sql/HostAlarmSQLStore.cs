using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Data.Sql
{
    /// <summary>
    /// This is the implementation that represents the configuration of a host alarm.
    /// </summary>
    public class HostAlarmSQLStore : SQLStoreBase, IHostAlarm
    {

        #region Private Members

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="HostAlarmSQLStore"/> using the provided <paramref name="contextFactory"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{NoLockXspocDbContext}"/> to get the <seealso cref="NoLockXspocDbContext"/>
        /// to use for database operations.
        /// </param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/>is null.
        /// </exception>
        public HostAlarmSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IHostAlarm Implementation

        /// <summary>
        /// Get the IHostAlarm limits for data history.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="addresses">The addresses.</param>
        /// <param name="correlationId"></param>
        /// <returns>The List of <seealso cref="HostAlarmForDataHistoryModel"/>.</returns>
        public IList<HostAlarmForDataHistoryModel> GetLimitsForDataHistory(Guid assetId, int[] addresses, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(HostAlarmSQLStore)} {nameof(GetLimitsForDataHistory)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var entities = context.NodeMasters.AsNoTracking().Where(x => x.AssetGuid == assetId)
                    .Join(context.HostAlarm.AsNoTracking(), nm => nm.NodeId, ha => ha.NodeId, (nodeMaster, hostAlarm) => new
                    {
                        nodeMaster,
                        hostAlarm,
                    })
                    .Select(x => x.hostAlarm).Where(x =>
                        x.AlarmType <= 3 && x.Address.HasValue && addresses.Contains(x.Address.Value))
                    .GroupBy(x => x.Address)
                    .Select(x => x.OrderByDescending(o => o.AlarmType)
                        .Select(s => new
                        {
                            Address = s.Address.Value,
                            s.HiHiLimit,
                            s.HiLimit,
                            s.LoLoLimit,
                            s.LoLimit,
                            s.AlarmType,
                        })
                        .FirstOrDefault()).ToList()
                    .Select(x => new HostAlarmForDataHistoryModel()
                    {
                        Address = x.Address,
                        AlarmType = x.AlarmType,
                        HiHiLimit = x.HiHiLimit,
                        HiLimit = x.HiLimit,
                        LoLimit = x.LoLimit,
                        LoLoLimit = x.LoLoLimit,
                    }).ToList();

                logger.WriteCId(Level.Trace, $"Finished {nameof(HostAlarmSQLStore)} {nameof(GetLimitsForDataHistory)}", correlationId);

                return entities;
            }
        }

        /// <summary>
        /// Get all host alarms for group status.
        /// </summary>
        /// <param name="nodeIds">The list of node ids.</param>
        /// <param name="correlationId"></param>
        /// <returns>The List of <seealso cref="HostAlarmForGroupStatus"/>.</returns>
        public IList<HostAlarmForGroupStatus> GetAllGroupStatusHostAlarms(IList<string> nodeIds, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(HostAlarmSQLStore)} {nameof(GetAllGroupStatusHostAlarms)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var query = context.HostAlarm.AsNoTracking()
                    .Where(a => nodeIds.Contains(a.NodeId) && a.Enabled == true && a.AlarmState > 0)
                    .Join(context.NodeMasters.AsNoTracking(), a => a.NodeId, n => n.NodeId, (hostAlarm, nodeMaster) => new
                    {
                        hostAlarm,
                        nodeMaster
                    })
                    .GroupJoin(context.Parameters.AsNoTracking(), t => t.hostAlarm.Address, p => p.Address, (x, parameter) => new
                    {
                        x.hostAlarm,
                        x.nodeMaster,
                        parameter
                    })
                    .SelectMany(t => t.parameter.DefaultIfEmpty(), (x, parameter) => new
                    {
                        x.hostAlarm,
                        x.nodeMaster,
                        parameter
                    })
                    .Where(x => x.parameter.Poctype == x.nodeMaster.PocType || x.parameter.Poctype == 99)
                    .GroupJoin(context.FacilityTags.AsNoTracking(), t => new
                    {
                        t.hostAlarm.NodeId,
                        t.hostAlarm.Address
                    }, f => new
                    {
                        NodeId = f.GroupNodeId ?? f.NodeId,
                        Address = (int?)f.Address
                    }, (x, facilityTag) =>
                        new
                        {
                            x.hostAlarm,
                            x.nodeMaster,
                            x.parameter,
                            facilityTag
                        })
                    .SelectMany(t => t.facilityTag.DefaultIfEmpty(), (x, facilityTag) => new
                    {
                        x.hostAlarm,
                        x.nodeMaster,
                        x.parameter,
                        facilityTag
                    })
                    .GroupJoin(context.LocalePhrases.AsNoTracking(), t => t.parameter.PhraseId, l => l.PhraseId, (x, localePhrase) => new
                    {
                        x.hostAlarm,
                        x.nodeMaster,
                        x.parameter,
                        x.facilityTag,
                        localePhrase
                    })
                    .SelectMany(t => t.localePhrase.DefaultIfEmpty(), (x, localePhrase) => new
                    {
                        x.hostAlarm.NodeId,
                        x.hostAlarm.Address,
                        Description = x.facilityTag.Description ?? localePhrase.English ?? x.parameter.Description,
                        x.hostAlarm.AlarmState,
                        x.hostAlarm.AlarmType,
                        x.hostAlarm.Priority,
                        x.hostAlarm.ValueChange,
                        x.hostAlarm.PercentChange,
                        x.hostAlarm.LoLimit,
                        x.hostAlarm.HiLimit,
                        x.hostAlarm.LoLoLimit,
                        x.hostAlarm.HiHiLimit,
                        x.hostAlarm.MinToMaxLimit,
                        x.hostAlarm.LastStateChange,
                        x.hostAlarm.PushEnabled
                    })
                    .Select(x => new HostAlarmForGroupStatus()
                    {
                        NodeId = x.NodeId,
                        Address = x.Address,
                        AlarmDescription = x.Description,
                        AlarmState = x.AlarmState,
                        AlarmType = x.AlarmType,
                        Priority = x.Priority,
                        ValueChange = x.ValueChange,
                        PercentChange = x.PercentChange,
                        LoLimit = x.LoLimit,
                        HiLimit = x.HiLimit,
                        LoLoLimit = x.LoLoLimit,
                        HiHiLimit = x.HiHiLimit,
                        MinToMaxLimit = x.MinToMaxLimit,
                        LastStateChange = x.LastStateChange,
                        PushEnabled = x.PushEnabled
                    })
                    .OrderBy(t => t.NodeId)
                    .ThenByDescending(t => t.Priority)
                    .ThenBy(t => t.AlarmDescription)
                    ;

                logger.WriteCId(Level.Trace, $"Finished {nameof(HostAlarmSQLStore)} {nameof(GetAllGroupStatusHostAlarms)}", correlationId);

                return query.ToList();
            }
        }

        #endregion

    }
}
