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
    /// This is the implementation that represents the configuration of a setpoint group
    /// on the current XSPOC database.
    /// </summary>
    public class SetpointGroupSQLStore : SQLStoreBase, ISetpointGroup
    {
        #region Private Members

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="SetpointGroupSQLStore"/> using the provided <paramref name="contextFactory"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{NoLockXspocDbContext}"/> to get the <seealso cref="NoLockXspocDbContext"/> 
        /// </param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/>is null.
        /// </exception>
        public SetpointGroupSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region ISetpointGroup Implementation

        /// <summary>
        /// Get setpoint groups and setpoint registers
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{SetpointGroupModel}"/> data.</returns>
        public IList<SetpointGroupModel> GetSetPointGroupData(Guid assetId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(SetpointGroupSQLStore)} {nameof(GetSetPointGroupData)}", correlationId);

            string nodeId = string.Empty;
            var setpointGroups = new List<SetpointGroupModel>();

            using (var context = _contextFactory.GetContext())
            {
                var node = context.NodeMasters.AsNoTracking()
                    .FirstOrDefault(x => x.AssetGuid == assetId);

                if (node != null)
                {
                    var result = from SG in context.SetpointGroups.AsNoTracking()
                                 join P in context.Parameters.AsNoTracking() on SG.SetpointGroupId equals P.SetpointGroup
                                 join N in context.NodeMasters.AsNoTracking() on P.Poctype equals N.PocType
                                 join SP in context.SavedParameters.AsNoTracking().Where(sp => sp.NodeId == node.NodeId) on P.Address equals SP.Address into SP_join
                                 from SP in SP_join.DefaultIfEmpty()
                                 where SG.SetpointGroupId > 0 && P.Poctype == node.PocType && N.NodeId == node.NodeId
                                 orderby SG.DisplayOrder
                                 select new
                                 {
                                     SG.SetpointGroupId,
                                     SG.DisplayName,
                                     P.Address,
                                     P.Description,
                                     SP.BackupDate,
                                     Value = SP.Value != null ? SP.Value : null,
                                     IsSupported = (P.EarliestSupportedVersion <= N.FirmwareVersion) ? 1 : 0,
                                     P.StateId,
                                     SG.DisplayOrder
                                 };

                    if (result != null)
                    {
                        setpointGroups = result
                            .GroupBy(x => new { x.DisplayName, x.SetpointGroupId, x.DisplayOrder })
                            .Select(g => new SetpointGroupModel()
                            {
                                SetpointGroup = g.Key.SetpointGroupId,
                                SetpointGroupName = g.Key.DisplayName,
                                RegisterCount = g.Count(),
                                DisplayOrder = g.Key.DisplayOrder == null ? int.MaxValue : g.Key.DisplayOrder,
                                Setpoints = g.Select(a => new SetpointModel()
                                {
                                    Description = a.Description,
                                    Parameter = a.Address.ToString(),
                                    BackupDate = a.BackupDate ?? null,
                                    BackupValue = a.Value != null ? a.Value.ToString() : string.Empty,
                                    IsSupported = a.IsSupported == 1,
                                    StateId = a.StateId,
                                    BackUpLookUpValues = a.StateId != null ? context.States.Where(s => s.StateId == a.StateId)
                                            .Select(l => new LookupValueModel
                                            {
                                                Value = l.Value,
                                                Text = l.Text
                                            }).ToList() : null
                                }).ToList()
                            }).OrderBy(a => a.DisplayOrder).ToList();
                    }
                }
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(SetpointGroupSQLStore)} {nameof(GetSetPointGroupData)}", correlationId);

            return setpointGroups;
        }

        #endregion
    }
}
