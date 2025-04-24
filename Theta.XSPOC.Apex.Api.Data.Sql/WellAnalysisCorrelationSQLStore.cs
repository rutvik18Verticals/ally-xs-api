using Microsoft.EntityFrameworkCore;
using System;
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
    /// This is the implementation that represents the configuration of a Analysis Correlation
    /// on the current XSPOC database.
    /// </summary>
    public class WellAnalysisCorrelationSQLStore : SQLStoreBase, IWellAnalysisCorrelation
    {

        #region Private Members

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="WellAnalysisCorrelationSQLStore"/> using the provided <paramref name="contextFactory"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{NoLockXspocDbContext}"/> to get the <seealso cref="NoLockXspocDbContext"/> 
        /// to use for database operations.
        /// </param>
        /// <param name="loggerFactory"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/> is null.
        /// </exception>
        public WellAnalysisCorrelationSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IWellAnalysisCorrelation Implementation

        /// <summary>
        /// Get the Analysis Correlation based on id.
        /// </summary>
        /// <param name="id">The correlation id</param>
        /// <param name="correlationId">The correlation GUID.</param>
        /// <returns>The <seealso cref="AnalysisCorrelationModel"/></returns>
        public AnalysisCorrelationModel GetAnalysisCorrelation(int? id, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(WellAnalysisCorrelationSQLStore)} {nameof(GetAnalysisCorrelation)}", correlationId);

            if (id == null)
            {
                logger.WriteCId(Level.Info, "Missing analysis correlation id", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(WellAnalysisCorrelationSQLStore)} {nameof(GetAnalysisCorrelation)}", correlationId);

                return null;
            }

            using (var context = _contextFactory.GetContext())
            {
                var entity = context.AnalysisCorrelation.AsNoTracking()
                    .FirstOrDefault(x => x.Id == id);

                if (entity == null)
                {
                    logger.WriteCId(Level.Info, "Missing analysis correlation", correlationId);
                    logger.WriteCId(Level.Trace, $"Finished {nameof(WellAnalysisCorrelationSQLStore)} {nameof(GetAnalysisCorrelation)}", correlationId);

                    return null;
                }

                var correlation = new AnalysisCorrelationModel
                {
                    CorrelationId = entity.CorrelationId,
                    Id = entity.Id,
                    CorrelationTypeId = entity.CorrelationTypeId,
                };

                logger.WriteCId(Level.Trace, $"Finished {nameof(WellAnalysisCorrelationSQLStore)} {nameof(GetAnalysisCorrelation)}", correlationId);

                return correlation;
            }
        }

        #endregion

    }
}
