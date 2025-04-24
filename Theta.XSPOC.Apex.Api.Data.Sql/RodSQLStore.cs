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
    /// Represents a SQL store for managing rods.
    /// </summary>
    public class RodSQLStore : SQLStoreBase, IRod
    {

        #region Private Fields

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RodSQLStore"/> class.
        /// </summary>
        /// <param name="contextFactory">The factory for creating the database context.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public RodSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        /// <summary>
        /// Gets the rod grades for the specified node Ids.
        /// </summary>
        /// <param name="nodeIds">The list of node Ids.</param>
        /// <param name="correlationId"></param>
        /// <returns>The list of rod grades for the specified node Ids.</returns>
        public IList<RodForGroupStatusModel> GetRodForGroupStatus(IList<string> nodeIds, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(RodSQLStore)} {nameof(GetRodForGroupStatus)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var result = context.Rod.AsNoTracking()
                    .Join(context.RodGrade.AsNoTracking(), r => r.RodGradeId, rg => rg.RodGradeId, (r, rg) => new
                    {
                        Rod = r,
                        RodGrade = rg
                    })
                    .Where(x => nodeIds.Contains(x.Rod.NodeId))
                    .Select(x => new
                    {
                        x.Rod.NodeId,
                        x.Rod.RodNum,
                        x.RodGrade.Name
                    })
                    .Distinct()
                    .OrderBy(r => r.NodeId)
                    .ThenBy(r => r.RodNum)
                    .ToList()
                    .Select(x => new RodForGroupStatusModel()
                    {
                        NodeId = x.NodeId,
                        RodNum = x.RodNum,
                        Name = x.Name
                    }).ToList();

                logger.WriteCId(Level.Trace, $"Finished {nameof(RodSQLStore)} {nameof(GetRodForGroupStatus)}", correlationId);

                return result;
            }
        }

    }
}
