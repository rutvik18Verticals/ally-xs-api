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
    /// This is the implementation that represents the configuration of a analysis result curves.
    /// </summary>
    public class AnalysisCurveSQLStore : SQLStoreBase, IAnalysisCurve
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
        /// <param name="loggerFactory"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/>is null.
        /// <paramref name="loggerFactory"/>is null.
        /// </exception>
        public AnalysisCurveSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IAnalysisResultCurves Implementation

        /// <summary>
        /// Get the analysis result curve by analysis result id.
        /// </summary>
        /// <param name="analysisResultId">The analysis result id.</param>
        /// <param name="applicationTypeId">The application type id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The List of <seealso cref="AnalysisCurveModel"/>.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public IList<AnalysisCurveModel> GetAnalysisResultCurves(int analysisResultId, int applicationTypeId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(AnalysisCurveSQLStore)} " +
                $"{nameof(GetAnalysisResultCurves)}", correlationId);
            var analysisResultCurves = new List<AnalysisCurveModel>();

            using (var context = _contextFactory.GetContext())
            {
                var tableAnalysisResultCurve = context.AnalysisResultCurves;
                var tableCurveTypes = context.CurveTypes;

                var analysisResultCurveEntities = from arc in tableAnalysisResultCurve.AsNoTracking()
                                                  join ct in tableCurveTypes on arc.CurveTypeId equals ct.Id
                                                  where arc.AnalysisResultId == analysisResultId &&
                                                      ct.ApplicationTypeId == applicationTypeId
                                                  select new
                                                  {
                                                      ID = arc.Id,
                                                      CurveTypeID = arc.CurveTypeId
                                                  };

                foreach (var curve in analysisResultCurveEntities)
                {
                    var analysisResultCurve = new AnalysisCurveModel();

                    analysisResultCurve.Id = curve.ID;
                    analysisResultCurve.CurveTypeId = curve.CurveTypeID;

                    analysisResultCurves.Add(analysisResultCurve);
                }
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(AnalysisCurveSQLStore)} " +
                $"{nameof(GetAnalysisResultCurves)}", correlationId);

            return analysisResultCurves;
        }

        /// <summary>
        /// Get the Analysis Curve Model.
        /// </summary>
        /// <param name="analysisResultId"></param>
        /// <param name="applicationTypeId"></param>
        /// <param name="curveTypeIds"></param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="AnalysisCurveModel"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public IList<AnalysisCurveModel> Fetch(int analysisResultId, int applicationTypeId,
            IEnumerable<int> curveTypeIds, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(AnalysisCurveSQLStore)} " +
                $"{nameof(Fetch)}", correlationId);

            if (curveTypeIds == null)
            {
                logger.WriteCId(Level.Info, "Missing curve type ids", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AnalysisCurveSQLStore)} " +
                    $"{nameof(Fetch)}", correlationId);

                throw new ArgumentNullException(nameof(curveTypeIds));
            }

            if (!curveTypeIds.Any())
            {
                logger.WriteCId(Level.Info, "Missing curve type ids", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AnalysisCurveSQLStore)} " +
                    $"{nameof(Fetch)}", correlationId);

                throw new ArgumentException("Missing curve type ids");
            }

            var analysisResultCurves = new List<AnalysisCurveModel>();

            using (var context = _contextFactory.GetContext())
            {
                var analysisResultCurveEntities = context.AnalysisResultCurves.AsNoTracking()
                    .Join(context.CurveTypes, x => x.CurveTypeId, x => x.Id, (analysisResultCurve, curveType) => new
                    {
                        analysisResultCurve,
                        curveType,
                    })
                    .Where(x => x.analysisResultCurve.AnalysisResultId == analysisResultId &&
                        x.curveType.ApplicationTypeId == applicationTypeId &&
                        curveTypeIds.Contains(x.analysisResultCurve.CurveTypeId))
                    .Select(x => new
                    {
                        x.analysisResultCurve.Id,
                        x.analysisResultCurve.CurveTypeId
                    });

                foreach (var curve in analysisResultCurveEntities)
                {
                    var analysisResultCurve = new AnalysisCurveModel
                    {
                        Id = curve.Id,
                        CurveTypeId = curve.CurveTypeId
                    };

                    analysisResultCurves.Add(analysisResultCurve);
                }
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(AnalysisCurveSQLStore)} " +
               $"{nameof(Fetch)}", correlationId);

            return analysisResultCurves;
        }

        #endregion

    }
}
