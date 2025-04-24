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
    /// Implements the IGLAnalysisGetCurveCoordinate interface
    /// </summary>
    public class GLAnalysisGetCurveCoordinateSQLStore : SQLStoreBase, IGLAnalysisGetCurveCoordinate
    {

        #region Private Members

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new <seealso cref="GLAnalysisGetCurveCoordinateSQLStore"/> using the provided <paramref name="contextFactory"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{NoLockXspocDbContext}"/> to get the <seealso cref="NoLockXspocDbContext"/> 
        /// to use for database operations.
        /// </param>
        /// <param name="loggerFactory"></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/>is null.
        /// or
        /// </exception>
        public GLAnalysisGetCurveCoordinateSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        #endregion

        #region IGLAnalysisGetCurveCoordinate Implementation

        /// <summary>
        /// Get the GL Analysis Result Model data by asset id,testDate,analysisResultId and analysisTypeId.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="testDate">test Date</param>
        /// <param name="analysisResultId">analysis Result Id</param>
        /// <param name="analysisTypeId">analysis Type Id</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="GLAnalysisResultModel"/>.</returns>        
        public GLAnalysisResultModel GetGLAnalysisResultData(Guid assetId, string testDate, int analysisResultId,
            int analysisTypeId, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(GLAnalysisGetCurveCoordinateSQLStore)} {nameof(GetGLAnalysisResultData)}", correlationId);

            GLAnalysisResultModel gLAnalysisResultModel = new GLAnalysisResultModel();

            gLAnalysisResultModel = GetGLAnalysisResult(assetId,
                testDate,
                analysisResultId,
                analysisTypeId);

            logger.WriteCId(Level.Trace,
                $"Finished {nameof(GLAnalysisGetCurveCoordinateSQLStore)} {nameof(GetGLAnalysisResultData)}", correlationId);

            return gLAnalysisResultModel;
        }

        /// <summary>
        /// Get the GLAnalysisCurveCoordinateModel data by analysisResultId.
        /// </summary>
        /// <param name="analysisResultId">analysis Result Id</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IList{GLAnalysisCurveCoordinateModel}"/>.</returns>
        public IList<GLAnalysisCurveCoordinateModel> GetGLWellValveData(int analysisResultId, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(GLAnalysisGetCurveCoordinateSQLStore)} {nameof(GetGLWellValveData)}", correlationId);

            IList<GLAnalysisCurveCoordinateModel> listGLAnalysisCurveCoordinateModel
                = new List<GLAnalysisCurveCoordinateModel>();

            listGLAnalysisCurveCoordinateModel = GetGLWellValveData(analysisResultId);

            logger.WriteCId(Level.Trace,
                $"Finished {nameof(GLAnalysisGetCurveCoordinateSQLStore)} {nameof(GetGLWellValveData)}", correlationId);

            return listGLAnalysisCurveCoordinateModel;
        }

        /// <summary>
        /// Get the OrificeStatusModel data by analysisResultId.
        /// </summary>
        /// <param name="analysisResultId">analysis Result Id</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="OrificeStatusModel"/>.</returns>
        public OrificeStatusModel GetOrificeStatus(int analysisResultId, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(GLAnalysisGetCurveCoordinateSQLStore)} {nameof(GetOrificeStatus)}", correlationId);

            OrificeStatusModel orificeStatusModel = new OrificeStatusModel();
            orificeStatusModel = base.GetOrificeStatus(analysisResultId);

            logger.WriteCId(Level.Trace,
                $"Finished {nameof(GLAnalysisGetCurveCoordinateSQLStore)} {nameof(GetOrificeStatus)}", correlationId);

            return orificeStatusModel;
        }

        /// <summary>
        /// Get the AnalysisResultCurvesModel data by analysisResultId and application id.
        /// </summary>
        /// <param name="analysisResultId">Analysis Result Id</param>
        /// <param name="application">Application ID</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="List{AnalysisResultCurvesModel}"/>.</returns>
        public IList<AnalysisResultCurvesModel> GetAnalysisResultCurve(int analysisResultId, int application,
            string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(GLAnalysisGetCurveCoordinateSQLStore)} {nameof(GetOrificeStatus)}", correlationId);

            IList<AnalysisResultCurvesModel> analysisResultCurvesModellList = new List<AnalysisResultCurvesModel>();
            analysisResultCurvesModellList = base.GetAnalysisResultCurve(analysisResultId, application);

            logger.WriteCId(Level.Trace,
                $"Finished {nameof(GLAnalysisGetCurveCoordinateSQLStore)} {nameof(GetAnalysisResultCurve)}", correlationId);

            return analysisResultCurvesModellList;
        }

        /// <summary>
        /// Get the AnalysisCurveSetModel data by analysisResultId,analysisResultSourceId and curveSetTypeId.
        /// </summary>
        /// <param name="analysisResultId">Analysis Result Id</param>
        /// <param name="analysisResultSourceId"> Analysis Result Source Id</param>
        /// <param name="curveSetTypeId">Curve Set Type Id</param>
        /// <returns>The <seealso cref="List{AnalysisCurveSetModel}"/>.</returns>
        public IList<AnalysisCurveSetModel> GetAnalysisCurvesSet(int analysisResultId, int analysisResultSourceId, int curveSetTypeId)
        {
            IList<AnalysisCurveSetModel> analysisCurveSetModelList = new List<AnalysisCurveSetModel>();
            using (var context = _contextFactory.GetContext())
            {
                analysisCurveSetModelList = GetAnalysisCurveSet(analysisResultId,
                    analysisResultSourceId,
                    curveSetTypeId);
            }

            return analysisCurveSetModelList;
        }

        /// <summary>
        /// Get the gas lift analysis ipr curve curve coordinates.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="testDateString">The test date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="IPRAnalysisCurveCoordinateModel"/>.</returns>
        public IPRAnalysisCurveCoordinateModel GetGLAnalysisIPRCurveCoordinate(Guid assetId, string testDateString,
            string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(GLAnalysisGetCurveCoordinateSQLStore)} {nameof(GetGLAnalysisIPRCurveCoordinate)}", correlationId);

            var iprAnalysisResults = new IPRAnalysisCurveCoordinateModel();
            var nodeMasterEntity = GetNodeMasterData(assetId);

            if (nodeMasterEntity != null)
            {
                var nodeId = nodeMasterEntity.NodeId;
                var testDate = DateTime.Parse(testDateString);

                iprAnalysisResults = FetchIPRAnalysisResults(nodeId, testDate, nodeMasterEntity.ApplicationId);

                if (iprAnalysisResults != null)
                {
                    iprAnalysisResults.NodeMasterData = nodeMasterEntity;
                }
            }

            logger.WriteCId(Level.Trace,
                $"Finished {nameof(GLAnalysisGetCurveCoordinateSQLStore)} {nameof(GetGLAnalysisIPRCurveCoordinate)}", correlationId);

            return iprAnalysisResults;
        }

        /// <summary>
        /// Fetches the curve coordinates.
        /// </summary>
        /// <param name="curveId">The curve identifier.</param>
        /// <param name="correlationId"></param>
        /// <returns>The list of <seealso cref="IList{CurveCoordinatesModel}"/></returns>
        public IList<CurveCoordinatesModel> FetchCurveCoordinates(int curveId, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(GLAnalysisGetCurveCoordinateSQLStore)} {nameof(FetchCurveCoordinates)}", correlationId);

            var curveCoordinatesModelList = GetCurveCoordinates(curveId);

            logger.WriteCId(Level.Trace,
                $"Finished {nameof(GLAnalysisGetCurveCoordinateSQLStore)} {nameof(FetchCurveCoordinates)}", correlationId);

            return curveCoordinatesModelList;
        }

        /// <summary>
        /// Get Data For Static Fluid Curve.
        /// </summary>
        /// <param name="assetId"></param>
        /// <param name="correlationId"></param>
        /// <returns>The model <seealso cref="StaticFluidCurveModel"/> model data.</returns>
        public StaticFluidCurveModel GetDataForStaticFluidCurve(Guid assetId, string correlationId)
        {
            var logger = LoggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace,
                $"Starting {nameof(GLAnalysisGetCurveCoordinateSQLStore)} {nameof(GetDataForStaticFluidCurve)}", correlationId);

            var model = new StaticFluidCurveModel();
            using (var context = _contextFactory.GetContext())
            {
                var perforationResult = context.Perforation.AsNoTracking()
                    .Join(context.NodeMasters.AsNoTracking(), x => x.NodeId, x => x.NodeId, ((perforation, node) => new
                    {
                        perforation,
                        node
                    }))
                    .Where(x => x.node.AssetGuid == assetId)
                    .OrderBy(x => x.perforation.Depth)
                    .Select(x => new PerforationModel()
                    {
                        TopDepth = x.perforation.Depth,
                        Diameter = x.perforation.Diameter,
                        HoleCountPerUnit = x.perforation.HolesPerFt,
                        Length = x.perforation.Interval,
                        NodeId = x.perforation.NodeId
                    });

                model.Perforations = perforationResult.ToList();

                model.ProductionDepth = context.WellDetails.AsNoTracking().Join(context.NodeMasters.AsNoTracking(), x => x.NodeId, x => x.NodeId, ((wellDetail, node) => new
                {
                    wellDetail,
                    node
                })).FirstOrDefault(x => x.node.AssetGuid == assetId)?.wellDetail?.ProductionDepth;

                var analysisResult = context.GLAnalysisResults.AsNoTracking().Join(context.NodeMasters.AsNoTracking(), x => x.NodeId, x => x.NodeId, ((analysisResult, node) => new
                {
                    analysisResult,
                    node
                })).FirstOrDefault(x => x.node.AssetGuid == assetId)?.analysisResult;

                model.ReservoirPressure = analysisResult?.ReservoirPressure;
                model.KillFluidLevel = analysisResult?.KillFluidLevel;
                model.ReservoirFluidLevel = analysisResult?.ReservoirFluidLevel;

                logger.WriteCId(Level.Trace,
                    $"Finished {nameof(GLAnalysisGetCurveCoordinateSQLStore)} {nameof(GetDataForStaticFluidCurve)}", correlationId);

                return model;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the IPR Analysis Results based in node id and test date.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="testDate">The test date.</param>
        /// <param name="applicationId">The application id.</param>
        /// <returns>The <seealso cref="IPRAnalysisResultsEntity"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="nodeId"/> is null.
        /// </exception>
        private IPRAnalysisCurveCoordinateModel FetchIPRAnalysisResults(string nodeId, DateTime testDate, int? applicationId)
        {
            if (nodeId == null)
            {
                throw new ArgumentNullException(nameof(nodeId));
            }

            IPRAnalysisCurveCoordinateModel model = new IPRAnalysisCurveCoordinateModel();

            using (var context = _contextFactory.GetContext())
            {
                var resultEntities = context.IPRAnalysisResult.AsNoTracking()
                    .Where(x => x.NodeId == nodeId && x.TestDate == testDate)
                    .GroupJoin(context.SensitivityAnalysisIPR.AsNoTracking(), ipr => ipr.Id, s => s.IPRAnalysisResultId,
                        (ipr, sensitivityIPR) => new
                        {
                            iprAnalysisResult = ipr,
                            sensitivityIPR
                        })
                    .SelectMany(@t => @t.sensitivityIPR.DefaultIfEmpty(), (x, sensitivityIPR) => new
                    {
                        x.iprAnalysisResult,
                        sensitivityIPR
                    })
                    .Where(x => x.sensitivityIPR == null)
                    .Select(x => x.iprAnalysisResult);

                var resultEntity = resultEntities.FirstOrDefault();

                if (resultEntity != null)
                {
                    var iprAnalysisResultModel = new IPRAnalysisResultModel
                    {
                        Id = resultEntity.Id,
                        NodeId = resultEntity.NodeId,
                        TestDate = resultEntity.TestDate,
                        StaticBottomholePressure = resultEntity.StaticBottomholePressure,
                        BubblepointPressure = resultEntity.BubblepointPressure,
                        FlowingBottomholePressure = resultEntity.FlowingBottomholePressure,
                        GrossRate = resultEntity.GrossRate,
                        WaterRate = resultEntity.WaterRate,
                        WaterCut = resultEntity.WaterCut,
                        IPRSlope = resultEntity.IPRSlope,
                        ProductivityIndex = resultEntity.ProductivityIndex,
                        RateAtBubblePoint = resultEntity.RateAtBubblePoint,
                        RateAtMaxOil = resultEntity.RateAtMaxOil,
                        RateAtMaxLiquid = resultEntity.RateAtMaxLiquid,
                        AnalysisCorrelationId = resultEntity.AnalysisCorrelationId,
                        FBHPCalculationUsedDefaults = resultEntity.FBHPCalculationUsedDefaults,
                        FBHPCalculationResult = resultEntity.FBHPCalculationResult,
                        SBHPCalculationResult = resultEntity.SBHPCalculationResult,
                        BubblepointPressureSource = resultEntity.BubblepointPressureSource,
                        FBHPCalculationUsedMeasuredDepth = resultEntity.FBHPCalculationUsedMeasuredDepth,
                    };

                    var iprAnalysisCurve = FetchIPRAnalysisCurves(resultEntity.Id, (int)applicationId);
                    model.IPRAnalysisResultEntity = iprAnalysisResultModel;
                    model.AnalysisResultCurvesEntities = iprAnalysisCurve;

                    return model;
                }
            }

            return null;
        }

        private IList<AnalysisResultCurvesModel> FetchIPRAnalysisCurves(int analysisResultId, int application)
        {
            var analysisCurves = new List<AnalysisCurveModel>();
            using (var context = _contextFactory.GetContext())
            {
                var tableAnalysisResultCurve = context.AnalysisResultCurves.AsNoTracking();
                var tableCurveTypes = context.CurveTypes;

                var analysisResultCurveEntities = from arc in tableAnalysisResultCurve.AsNoTracking()
                                                  join ct in tableCurveTypes.AsNoTracking() on arc.CurveTypeId equals ct.Id
                                                  where arc.AnalysisResultId == analysisResultId && ct.ApplicationTypeId == application
                                                  select new { arc.Id, arc.CurveTypeId };

                var analysisResultCurves = analysisResultCurveEntities.ToList();
                List<AnalysisResultCurvesModel> result = new List<AnalysisResultCurvesModel>();

                if (analysisResultCurves != null)
                {
                    foreach (var analysisCurveEntity in analysisResultCurves)
                    {
                        result.Add(new AnalysisResultCurvesModel
                        {
                            AnalysisResultCurveID = analysisCurveEntity.Id,
                            CurveTypesID = analysisCurveEntity.CurveTypeId,
                        });
                    } // foreach ( var analysisResultCurve in analysisResultCurves )
                } // if (analysisResultCurves != null)

                return result;
            }
        }

        private IList<CurveCoordinatesModel> GetCurveCoordinates(int curveId)
        {
            IList<CurveCoordinatesModel> curveCoordinatesModelList = new List<CurveCoordinatesModel>();

            using (var context = ContextFactory.GetContext())
            {
                curveCoordinatesModelList = context.CurveCoordinates.AsNoTracking()
                    .Where(c => c.CurveId == curveId)
                    .Select(x => new CurveCoordinatesModel
                    {
                        CurveId = x.CurveId,
                        Id = x.Id,
                        X = x.X,
                        Y = x.Y,
                    }).OrderBy(c => c.X).ToList();
            }

            return curveCoordinatesModelList;
        }

        private IList<AnalysisCurveSetModel> GetAnalysisCurveSet(int analysisResultId, int analysisResultSourceId,
            int curveSetTypeId)
        {
            IList<AnalysisCurveSetModel> analysisCurveSetModelList = new List<AnalysisCurveSetModel>();
            using (var context = ContextFactory.GetContext())
            {
                var tableAnalysisCurveSet = context.AnalysisCurveSets.AsNoTracking();
                var tableAnalysisCurveSetMembers = context.AnalysisCurveSetMembers.AsNoTracking();
                var tableCurveSetCoordinates = context.CurveSetCoordinates.AsNoTracking();

                analysisCurveSetModelList = tableAnalysisCurveSet
                    .Where(x => x.AnalysisResultId == analysisResultId
                        && x.AnalysisResultSourceId == analysisResultSourceId
                        && x.CurveSetTypeId == curveSetTypeId)
                    .SelectMany(
                        x => tableAnalysisCurveSetMembers.Where(csm => csm.CurveSetId == x.CurveSetId).DefaultIfEmpty(),
                        (analysisCurveSet, analysisCurveSetMember) => new
                        {
                            analysisCurveSet,
                            analysisCurveSetMember
                        })
                    .SelectMany(x => tableCurveSetCoordinates.Where(csc => x.analysisCurveSetMember != null
                            && x.analysisCurveSetMember.CurveSetMemberId == csc.CurveId).DefaultIfEmpty(),
                        (cs, curveSetCoordinate) => new
                        {
                            cs,
                            curveSetCoordinate
                        })
                    .Select(x => new AnalysisCurveSetModel
                    {
                        CurveSetCoordinatesModels = new CurveSetCoordinatesModel
                        {
                            CurveId = x.curveSetCoordinate.CurveId,
                            Id = x.curveSetCoordinate.Id,
                            X = x.curveSetCoordinate.X,
                            Y = x.curveSetCoordinate.Y,
                        },
                        AnalysisCurveSetDataModels = new AnalysisCurveSetDataModel
                        {
                            AnalysisResultId = x.cs.analysisCurveSet.AnalysisResultId,
                            AnalysisResultSourceId = x.cs.analysisCurveSet.AnalysisResultSourceId,
                            CurveSetTypeId = x.cs.analysisCurveSet.CurveSetTypeId,
                            CurveSetId = x.cs.analysisCurveSet.CurveSetId
                        },
                        AnalysisCurveSetMemberModels = new AnalysisCurveSetMemberModel
                        {
                            CurveSetId = x.cs.analysisCurveSetMember.CurveSetId,
                            CurveSetMemberId = x.cs.analysisCurveSetMember.CurveSetMemberId,
                        }
                    }).ToList();
            }

            return analysisCurveSetModelList;
        }
    }

    #endregion

}
