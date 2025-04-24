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
    /// This is the implementation that represents the configuration of a esp tornado  curve set annotations.
    /// </summary>
    public class ESPTornadoCurveSetAnnotationSQLStore : SQLStoreBase, IESPTornadoCurveSetAnnotation
    {

        #region Private Members

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="AnalysisCurveSQLStore"/> using the provided <paramref name="contextFactory"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{NoLockXspocDbContext}"/> to get the <seealso cref="NoLockXspocDbContext"/> 
        /// to use for database operations.
        /// </param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/>is null.
        /// </exception>
        public ESPTornadoCurveSetAnnotationSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory,
            IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IESPTornadoCurveSetAnnotation Implementation

        /// <summary>
        /// Get the esp tornado curve set annotation.
        /// </summary>
        /// <returns>The <seealso cref="ESPTornadoCurveSetAnnotationModel"/>.</returns>
        public IList<ESPTornadoCurveSetAnnotationModel> GetESPTornadoCurveSetAnnotations(string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(ESPTornadoCurveSetAnnotationSQLStore)}" +
                $" {nameof(GetESPTornadoCurveSetAnnotations)}", correlationId);

            var espTornadoCurveSetAnnotationData = new List<ESPTornadoCurveSetAnnotationModel>();

            using (var context = _contextFactory.GetContext())
            {
                var result = context.ESPTornadoCurveSetAnnotations.AsNoTracking().ToList();

                foreach (var annotation in result)
                {
                    var espTornadoCurveSetAnnotation = new ESPTornadoCurveSetAnnotationModel();

                    espTornadoCurveSetAnnotation.CurveSetMemberId = annotation.CurveSetMemberId;
                    espTornadoCurveSetAnnotation.Frequency = annotation.Frequency;

                    espTornadoCurveSetAnnotationData.Add(espTornadoCurveSetAnnotation);
                }
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(ESPTornadoCurveSetAnnotationSQLStore)}" +
                $" {nameof(GetESPTornadoCurveSetAnnotations)}", correlationId);

            return espTornadoCurveSetAnnotationData;
        }

        #endregion

    }
}
