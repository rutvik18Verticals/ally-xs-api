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
    /// This is the implementation that represents the configuration of a curve coordinate.
    /// </summary>
    public class CurveCoordinateSQLStore : SQLStoreBase, ICurveCoordinate
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
        /// <param name="loggerFactory">loggerFactory</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/>is null.
        /// <paramref name="loggerFactory"/>is null.
        /// </exception>
        public CurveCoordinateSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region ICurveCoordinate Implementation

        /// <summary>
        /// Get the curve coordinate by curve id.
        /// </summary>
        /// <param name = "curveId" > The curve id.</param>
        /// <param name = "correlationId" > The correlation id.</param>
        /// <returns>The List of<seealso cref="CurveCoordinatesModel"/>.</returns>
        /// <exception cref = "NotImplementedException" >.</exception >
        public IList<CurveCoordinatesModel> GetCurvesCoordinates(int curveId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(CurveCoordinateSQLStore)}" +
                $" {nameof(GetCurvesCoordinates)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var curve = new List<CurveCoordinatesModel>();

                IList<CurveCoordinatesEntity> savedCurveCoordinates
                   = context.CurveCoordinates.AsNoTracking().Where(c => c.CurveId == curveId).OrderBy(c => c.X).ToList();

                if (curve != null && savedCurveCoordinates.Count > 0)
                {
                    foreach (var coordinate in savedCurveCoordinates)
                    {
                        curve.Add(new CurveCoordinatesModel
                        {
                            CurveId = coordinate.CurveId,
                            X = coordinate.X,
                            Y = coordinate.Y,
                            Id = coordinate.Id
                        });
                    }
                }

                logger.WriteCId(Level.Trace, $"Finished {nameof(CurveCoordinateSQLStore)} " +
                    $"{nameof(GetCurvesCoordinates)}", correlationId);

                return curve;
            }
        }

        #endregion

    }
}