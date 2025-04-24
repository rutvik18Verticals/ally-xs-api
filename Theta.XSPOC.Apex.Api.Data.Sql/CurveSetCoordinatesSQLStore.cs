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
    /// This is the implementation that represents the configuration of a curve set coordinate.
    /// </summary>
    public class CurveSetCoordinatesSQLStore : SQLStoreBase, ICurveSetCoordinates
    {

        #region Private Members

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="CurveCoordinateSQLStore"/> using the provided <paramref name="contextFactory"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{NoLockXspocDbContext}"/> to get the <seealso cref="NoLockXspocDbContext"/> 
        /// to use for database operations.
        /// </param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/>is null.
        /// </exception>
        public CurveSetCoordinatesSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory,
            IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region ICurveSetCoordinates Implementation

        /// <summary>
        /// Get the curve set coordinate by curve id.
        /// </summary>
        /// <param name="curveId">the curve id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The List of <seealso cref="CurveSetCoordinatesModel"/>.</returns>
        /// <exception cref="NotImplementedException">.</exception>
        public IList<CurveSetCoordinatesModel> GetCurveSetCoordinates(int curveId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(CurveSetCoordinatesSQLStore)} " +
                $"{nameof(GetCurveSetCoordinates)}", correlationId);

            var curveCoordinates = new List<CurveSetCoordinatesModel>();

            using (var context = _contextFactory.GetContext())
            {
                curveCoordinates = context.CurveSetCoordinates.AsNoTracking()
                    .Where(x => x.CurveId == curveId)
                    .Select(c => new CurveSetCoordinatesModel()
                    {
                        Id = c.Id,
                        CurveId = c.CurveId,
                        X = c.X,
                        Y = c.Y,
                    }).OrderBy(x => x.X).ToList();
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(CurveSetCoordinatesSQLStore)} " +
                $"{nameof(GetCurveSetCoordinates)}", correlationId);

            return curveCoordinates;
        }

        #endregion

    }
}
