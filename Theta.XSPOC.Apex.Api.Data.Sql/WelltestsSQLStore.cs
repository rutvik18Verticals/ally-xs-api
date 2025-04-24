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
    /// Implements the IWellTests interface
    /// </summary>
    public class WellTestsSQLStore : SQLStoreBase, IWellTests
    {

        #region Private Members

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new <seealso cref="WellTestsSQLStore"/> using the provided <paramref name="contextFactory"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{NoLockXspocDbContext}"/> to get the <seealso cref="XspocDbContext"/> 
        /// to use for database operations.
        /// </param>
        /// <param name="loggerFactory"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/> is null.
        /// or
        /// </exception>
        public WellTestsSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IWelltests Implementation

        /// <summary>
        /// Get the Well tests data by asset id.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="correlationId">The correlation GUID.</param>
        /// <returns>The <seealso cref="IList{WellTestModel}"/>.</returns>
        public IList<WellTestModel> GetESPWellTestsData(Guid assetId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(WellTestsSQLStore)} {nameof(GetESPWellTestsData)}", correlationId);

            var wellTestModel = GetListWellTest(assetId);

            if (wellTestModel == null)
            {
                logger.WriteCId(Level.Info, "Missing well test", correlationId);
                logger.WriteCId(Level.Trace, $"Finished {nameof(WellTestsSQLStore)} {nameof(GetESPWellTestsData)}", correlationId);

                return null;
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(WellTestsSQLStore)} {nameof(GetESPWellTestsData)}", correlationId);

            return wellTestModel;
        }

        /// <summary>
        /// Get the Well tests data by asset id.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="analysisTypeSensitivityKey"></param>
        /// <param name="analysisTypeWellTest"></param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="List{GLAnalysisResultModel}"/>.</returns>
        public IList<GLAnalysisResultModel> GetGLAnalysisWellTestsData(Guid assetId,
            int analysisTypeSensitivityKey, int analysisTypeWellTest, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(WellTestsSQLStore)} {nameof(GetGLAnalysisWellTestsData)}", correlationId);

            IList<GLAnalysisResultModel> response = new List<GLAnalysisResultModel>();
            var wellTestModel =
                GetGLAnalysisWellTests(assetId, analysisTypeSensitivityKey, analysisTypeWellTest);

            if (wellTestModel != null)
            {
                response = wellTestModel;
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(WellTestsSQLStore)} {nameof(GetGLAnalysisWellTestsData)}", correlationId);

            return response;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets a list of all well tests for esp analysis by asset id.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="analysisTypeSensitivityKey">The analysis Type Sensitivity Key.</param>
        /// <param name="analysisTypeWellTest">The analysis Type Well Test.</param>
        /// <returns>The <seealso cref="List{GLAnalysisResultModel}"/></returns>
        private IList<GLAnalysisResultModel> GetGLAnalysisWellTests(Guid assetId, int analysisTypeSensitivityKey,
            int analysisTypeWellTest)
        {
            List<GLAnalysisResultModel> gLAnalysisResultsEntityList = new List<GLAnalysisResultModel>();
            using (var context = _contextFactory.GetContext())
            {
                var nodeMaster = context.NodeMasters.AsNoTracking().FirstOrDefault(x => x.AssetGuid == assetId);
                if (nodeMaster == null)
                {
                    return null;
                }
                var nodeId = nodeMaster.NodeId;
                var result = context.WellTest.AsNoTracking().GroupJoin(context.GLAnalysisResults.AsNoTracking(), wt => new
                {
                    wt.NodeId,
                    wt.TestDate
                }, ar => new
                {
                    ar.NodeId,
                    ar.TestDate
                },
                        (wellTest, analysisResult) => new
                        {
                            wellTest,
                            analysisResult
                        })
                    .SelectMany(x => x.analysisResult.DefaultIfEmpty(),
                        (x, analysisResult) => new
                        {
                            x.wellTest,
                            analysisResult
                        })
                    .Where(x => x.wellTest.Approved == true && x.wellTest.NodeId == nodeId && x.analysisResult.AnalysisType == analysisTypeWellTest)
                    .OrderByDescending(x => x.wellTest.TestDate)
                    .ToList();
                foreach (var item in result)
                {
                    gLAnalysisResultsEntityList.Add(new GLAnalysisResultModel()
                    {
                        TestDate = item.wellTest.TestDate,
                        Id = item.analysisResult?.Id ?? 0,
                        AnalysisType = analysisTypeWellTest,
                    });
                }
                var testDates = result.Select(item => item.wellTest.TestDate).ToList();
                var resultEntity = context.GLAnalysisResults.AsNoTracking()
                    .Where(ar => ar.NodeId == nodeId && ar.AnalysisType == analysisTypeSensitivityKey &&
                        testDates.Contains(ar.TestDate))
                    .Select(item => new GLAnalysisResultModel
                    {
                        TestDate = item.TestDate,
                        Id = item.Id,
                        AnalysisType = analysisTypeSensitivityKey,
                    })
                    .ToList();
                gLAnalysisResultsEntityList.AddRange(resultEntity);
                return gLAnalysisResultsEntityList;
            }
        }

        #endregion

    }
}
