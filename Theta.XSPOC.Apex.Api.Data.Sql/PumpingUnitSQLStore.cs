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
    /// Represents a SQL store for PumpingUnit entities.
    /// </summary>
    public class PumpingUnitSQLStore : SQLStoreBase, IPumpingUnit
    {

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;
        private readonly IThetaLoggerFactory _loggerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PumpingUnitSQLStore"/> class.
        /// </summary>
        /// <param name="contextFactory">The factory for creating instances of the XspocDbContext.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public PumpingUnitSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <summary>
        /// Gets the unit names for the specified node Ids.
        /// </summary>
        /// <param name="nodeIds">The list of node Ids.</param>
        /// <param name="correlationId"></param>
        /// <returns>The list of PumpingUnitForUnitNameModel objects.</returns>
        public IList<PumpingUnitForUnitNameModel> GetUnitNames(IList<string> nodeIds, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(PumpingUnitSQLStore)} {nameof(GetUnitNames)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var wellDetailsUnitIDs = context.WellDetails.AsNoTracking()
                    .Where(wd => nodeIds.Contains(wd.NodeId))
                    .Select(wd => wd.PumpingUnitId);

                var pumpingUnitsQuery = context.PumpingUnits.AsNoTracking()
                    .Where(pu => wellDetailsUnitIDs.Contains(pu.UnitId))
                    .Select(pu => new
                    {
                        pu.UnitId,
                        pu.APIDesignation
                    })
                    .Distinct().ToList();

                var puCustomQuery = context.CustomPumpingUnits.AsNoTracking()
                    .Where(puc => wellDetailsUnitIDs.Contains(puc.Id))
                    .Select(puc => new
                    {
                        UnitId = puc.Id,
                        puc.APIDesignation
                    })
                    .Distinct().ToList();

                var unionQuery = pumpingUnitsQuery.Union(puCustomQuery).ToList()
                    .Select(x => new PumpingUnitForUnitNameModel()
                    {
                        APIDesignation = x.APIDesignation,
                        UnitId = x.UnitId,
                    });

                logger.WriteCId(Level.Trace, $"Finished {nameof(PumpingUnitSQLStore)} {nameof(GetUnitNames)}", correlationId);

                return unionQuery.ToList();
            }
        }

    }
}
