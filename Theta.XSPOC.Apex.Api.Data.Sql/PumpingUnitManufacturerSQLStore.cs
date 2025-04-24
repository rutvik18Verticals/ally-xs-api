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
    /// Represents a SQL store for PumpingUnitManufacturer entities.
    /// </summary>
    public class PumpingUnitManufacturerSQLStore : SQLStoreBase, IPumpingUnitManufacturer
    {

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;
        private readonly IThetaLoggerFactory _loggerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PumpingUnitManufacturerSQLStore"/> class.
        /// </summary>
        /// <param name="contextFactory">The factory for creating instances of the XspocDbContext.</param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        public PumpingUnitManufacturerSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        /// <summary>
        /// Gets the manufacturers for the specified node IDs.
        /// </summary>
        /// <param name="nodeIds">The list of node IDs.</param>
        /// <param name="correlationId"></param>
        /// <returns>The list of PumpingUnitManufacturerForGroupStatus entities.</returns>
        public IList<PumpingUnitManufacturerForGroupStatus> GetManufacturers(IList<string> nodeIds, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(PumpingUnitManufacturerSQLStore)} {nameof(GetManufacturers)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var wellDetailsUnitIDs = context.WellDetails.AsNoTracking()
                    .Where(wd => nodeIds.Contains(wd.NodeId))
                    .Select(wd => wd.PumpingUnitId);

                var pumpingUnitsQuery = context.PumpingUnits.AsNoTracking()
                    .Join(context.PumpingUnitManufacturer.AsNoTracking(),
                        u => u.ManufacturerId,
                        m => m.ManufacturerAbbreviation,
                        (u, m) => new
                        {
                            u.UnitId,
                            u.ManufacturerId,
                            m.Id,
                            m.Manuf
                        })
                    .Where(joined => wellDetailsUnitIDs.Contains(joined.UnitId))
                    .Distinct().ToList();

                var puCustomQuery = context.CustomPumpingUnits.AsNoTracking()
                    .Where(puc => wellDetailsUnitIDs.Contains(puc.Id))
                    .Select(puc => new
                    {
                        UnitId = puc.Id,
                        ManufacturerId = puc.Manufacturer,
                        Id = 0,
                        Manuf = puc.Manufacturer
                    })
                    .Distinct().ToList();

                var unionQuery = pumpingUnitsQuery.Union(puCustomQuery)
                    .Select(x => new PumpingUnitManufacturerForGroupStatus()
                    {
                        Manufacturer = x.Manuf,
                        UnitId = x.UnitId,
                    });

                logger.WriteCId(Level.Trace, $"Finished {nameof(PumpingUnitManufacturerSQLStore)} {nameof(GetManufacturers)}", correlationId);

                return unionQuery.ToList();
            }
        }

    }
}
