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
    /// This is the implementation that represents the configuration of a analysis result curves set.
    /// </summary>
    public class AnalysisCurveSetsSQLStore : SQLStoreBase, IAnalysisCurveSets
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
        /// <param name="loggerFactory"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/>is null.
        /// <paramref name="loggerFactory"/>is null.
        /// </exception>
        public AnalysisCurveSetsSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IAnalysisCurveSets Implementation

        /// <summary>
        /// Get the analysis curve set.
        /// </summary>
        /// <param name="analysisResultId">The analysis result id.</param>
        /// <param name="analysisResultSourceId">The analysis result source id.</param>
        /// <param name="curveSetTypeId">The curve set type id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The List of <seealso cref="AnalysisCurveSetModel"/>.</returns>
        public IList<AnalysisCurveSetModel> GetAnalysisCurvesSet(int analysisResultId,
            int analysisResultSourceId, int curveSetTypeId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(AnalysisCurveSetsSQLStore)} " +
                $"{nameof(GetAnalysisCurvesSet)}", correlationId);

            var analysisCurveSets = new List<AnalysisCurveSetModel>();

            using (var context = _contextFactory.GetContext())
            {
                var tableAnalysisCurveSet = context.AnalysisCurveSets;
                var tableAnalysisCurveSetMembers = context.AnalysisCurveSetMembers;
                var tableCurveSetCoordinates = context.CurveSetCoordinates;

                var entityQuery = tableAnalysisCurveSet.AsNoTracking()
                    .Where(x => x.AnalysisResultId == analysisResultId
                        && x.AnalysisResultSourceId == analysisResultSourceId
                        && x.CurveSetTypeId == curveSetTypeId)
                    .SelectMany(
                        x => tableAnalysisCurveSetMembers.AsNoTracking().Where(csm => csm.CurveSetId == x.CurveSetId).DefaultIfEmpty(),
                        (analysisCurveSet, analysisCurveSetMember) => new
                        {
                            analysisCurveSet,
                            analysisCurveSetMember
                        })
                    .SelectMany(x => tableCurveSetCoordinates.AsNoTracking().Where(csc => x.analysisCurveSetMember != null
                            && x.analysisCurveSetMember.CurveSetMemberId == csc.CurveId).DefaultIfEmpty(),
                        (cs, curveSetCoordinate) => new
                        {
                            cs,
                            curveSetCoordinate
                        })
                    .Select(x => new
                    {
                        x.curveSetCoordinate,
                        x.cs.analysisCurveSetMember,
                        x.cs.analysisCurveSet,
                    });

                AnalysisCurveSetModel analysisCurveSet;

                foreach (var data in entityQuery)
                {
                    analysisCurveSet = new AnalysisCurveSetModel();
                    var curveSetCoordinates = new CurveSetCoordinatesModel();
                    var analysisCurveSetMember = new AnalysisCurveSetMemberModel();
                    var analysisCurveSetData = new AnalysisCurveSetDataModel();

                    if (data.curveSetCoordinate != null)
                    {
                        curveSetCoordinates.Id = data.curveSetCoordinate.Id;
                        curveSetCoordinates.X = data.curveSetCoordinate.X;
                        curveSetCoordinates.Y = data.curveSetCoordinate.Y;
                        curveSetCoordinates.CurveId = data.curveSetCoordinate.CurveId;
                        analysisCurveSet.CurveSetCoordinatesModels = curveSetCoordinates;
                    }

                    if (data.analysisCurveSetMember != null)
                    {
                        analysisCurveSetMember.CurveSetId = data.analysisCurveSetMember.CurveSetId;
                        analysisCurveSetMember.CurveSetMemberId = data.analysisCurveSetMember.CurveSetMemberId;
                        analysisCurveSet.AnalysisCurveSetMemberModels = analysisCurveSetMember;
                    }

                    analysisCurveSetData.AnalysisResultSourceId = data.analysisCurveSet.AnalysisResultSourceId;
                    analysisCurveSetData.AnalysisResultId = data.analysisCurveSet.AnalysisResultId;
                    analysisCurveSetData.CurveSetId = data.analysisCurveSet.CurveSetId;
                    analysisCurveSetData.CurveSetTypeId = data.analysisCurveSet.CurveSetTypeId;
                    analysisCurveSet.AnalysisCurveSetDataModels = analysisCurveSetData;

                    analysisCurveSets.Add(analysisCurveSet);
                }
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(AnalysisCurveSetsSQLStore)} " +
                $"{nameof(GetAnalysisCurvesSet)}", correlationId);

            return analysisCurveSets;
        }

        /// <summary>
        /// Get the glr curve set annotations.
        /// </summary>
        /// <param name="curveSetMemberIds">The curve set member ids.</param>
        /// <param name="correlationId"></param>
        /// <returns>The List of <seealso cref="GasLiquidRatioCurveAnnotationModel"/>.</returns>
        public IList<GasLiquidRatioCurveAnnotationModel> GetGLRCurveSetAnnotations(IList<int> curveSetMemberIds, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(AnalysisCurveSetsSQLStore)} " +
                $"{nameof(GetGLRCurveSetAnnotations)}", correlationId);

            if (curveSetMemberIds == null)
            {
                logger.WriteCId(Level.Info, "Missing curve set member ids", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(AnalysisCurveSetsSQLStore)} " +
                    $"{nameof(GetGLRCurveSetAnnotations)}", correlationId);

                throw new ArgumentNullException(nameof(curveSetMemberIds));
            }
            using (var context = _contextFactory.GetContext())
            {
                var entities = context.GLCurveSetAnnotation.AsNoTracking()
                    .Where(x => curveSetMemberIds.Contains(x.CurveSetMemberId))
                    .Select(x => new GasLiquidRatioCurveAnnotationModel()
                    {
                        CurveSetMemberId = x.CurveSetMemberId,
                        GasLiquidRatio = x.AssociatedGasLiquidRatio,
                        IsPrimaryCurve = x.IsPrimary,
                    });

                logger.WriteCId(Level.Trace, $"Finished {nameof(AnalysisCurveSetsSQLStore)} " +
                    $"{nameof(GetGLRCurveSetAnnotations)}", correlationId);

                return entities.ToList();
            }
        }

        #endregion

    }
}
