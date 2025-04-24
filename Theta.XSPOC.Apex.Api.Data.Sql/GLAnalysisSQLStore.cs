using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
    /// This is the implementation that represents the configuration of a notification
    /// on the current XSPOC database.
    /// </summary>
    public class GLAnalysisSQLStore : SQLStoreBase, IGLAnalysis
    {

        #region Private Members

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new <seealso cref="GLAnalysisSQLStore"/> using the provided <paramref name="contextFactory"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{NoLockXspocDbContext}"/> to get the <seealso cref="NoLockXspocDbContext"/> 
        /// to use for database operations.
        /// </param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/> is null.
        /// </exception>
        public GLAnalysisSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory
            , IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IGLAnalysis Implementation

        /// <summary>
        /// Retrieves the GL Analysis Survey Date.
        /// </summary>
        /// <param name="id">The id of the analysis.</param>
        /// <param name="gasArtificialLiftKey">The gas artificial lift key.</param>
        /// <param name="temperatureCurveKey">The temperature curve key.</param>
        /// <param name="pressureCurveKey">The pressure curve key.</param>
        /// <param name="correlationId"></param>
        /// <returns>The list of <see cref="GlAnalysisSurveyDateModel"/>.</returns>
        public IList<GlAnalysisSurveyDateModel> GetGLAnalysisSurveyDate(Guid id,
            int gasArtificialLiftKey, int temperatureCurveKey, int pressureCurveKey, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GLAnalysisSQLStore)}" +
                $" {nameof(GetGLAnalysisSurveyDate)}", correlationId);

            var surveyDateModel = new List<GlAnalysisSurveyDateModel>();

            using (var context = _contextFactory.GetContext())
            {
                var nodeMasters = context.NodeMasters.AsNoTracking().Where(x => x.AssetGuid == id).SingleOrDefault();

                if (nodeMasters != null)
                {

                    var entities = context.SurveyData.AsNoTracking()
                    .Join(context.AnalysisResultCurves.AsNoTracking(), sd => sd.Id, arc => arc.AnalysisResultId,
                        (surveyData, analysisResultCurves) => new
                        {
                            surveyData,
                            analysisResultCurves,
                        })
                    .Join(context.CurveTypes.AsNoTracking(), x => x.analysisResultCurves.CurveTypeId, ct => ct.Id, (x, curveTypes) => new
                    {
                        x.surveyData,
                        x.analysisResultCurves,
                        curveTypes,
                    })
                    .Where(x => x.surveyData.NodeId == nodeMasters.NodeId &&
                        x.curveTypes.ApplicationTypeId == gasArtificialLiftKey
                        && (x.analysisResultCurves.CurveTypeId == temperatureCurveKey
                            || x.analysisResultCurves.CurveTypeId == pressureCurveKey))
                    .Select(x => x.surveyData.SurveyDate).Distinct()
                    .OrderByDescending(x => x)
                    .ToList();

                    surveyDateModel = entities.Select(x => new GlAnalysisSurveyDateModel()
                    {
                        SurveyDate = x,
                    }).ToList();
                }

            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(GLAnalysisSQLStore)}" +
                $" {nameof(GetGLAnalysisSurveyDate)}", correlationId);

            return surveyDateModel;
        }

        /// <summary>
        /// Retrieves the GL sensitivity analysis data.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="testDateString">The test date string.</param>
        /// <param name="analysisResultId">The analysis result id.</param>
        /// <param name="correlationId"></param>
        /// <returns>The GL analysis response.</returns>
        public GLAnalysisResponse GetGLSensitivityAnalysisData(Guid assetId, string testDateString
            , int analysisResultId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GLAnalysisSQLStore)}" +
                $" {nameof(GetGLSensitivityAnalysisData)}", correlationId);
            var testDate = DateTime.Parse(testDateString);

            var response = GetGLAnalysisResultData(analysisResultId);

            CreateAnalysisData(ref response, assetId, testDate);
            logger.WriteCId(Level.Trace, $"Finished {nameof(GLAnalysisSQLStore)}" +
               $" {nameof(GetGLSensitivityAnalysisData)}", correlationId);

            return response;
        }

        /// <summary>
        /// Get the gas analysis data by asset id, test date and analysisType.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="testDateString">The test date.</param>
        /// <param name="analysisType">The analysis type.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="GLAnalysisResponse"/>.</returns>
        public GLAnalysisResponse GetGLAnalysisData(Guid assetId, string testDateString
            , int analysisType, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GLAnalysisSQLStore)}" +
                $" {nameof(GetGLAnalysisData)}", correlationId);
            var testDate = DateTime.Parse(testDateString);

            var response = GetGLAnalysisResultData(assetId, testDate, analysisType);

            CreateAnalysisData(ref response, assetId, testDate);

            logger.WriteCId(Level.Trace, $"Finished {nameof(GLAnalysisSQLStore)}" +
                $" {nameof(GetGLAnalysisData)}", correlationId);

            return response;
        }

        /// <summary>
        /// Get the GLAnalysis Curve Coordinates.
        /// </summary>
        /// <param name="id">The asset GUID.</param>
        /// <param name="surveyDateString">The surveyDate.</param>
        /// <param name="correlationId"></param>
        /// <returns>The List of <seealso cref="GlAnalysisSurveyCurveCoordinateDataModel"/>.</returns>
        public IList<GlAnalysisSurveyCurveCoordinateDataModel> GetGLAnalysisCurveCoordinatesSurveyDate(Guid id,
            string surveyDateString, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GLAnalysisSQLStore)}" +
                $" {nameof(GetGLAnalysisCurveCoordinatesSurveyDate)}", correlationId);
            var surveyDate = DateTime.Parse(surveyDateString);

            using (var context = _contextFactory.GetContext())
            {
                var surveyDataEntities =
                    context.SurveyData.AsNoTracking().Where(x => x.SurveyDate.Year == surveyDate.Year &&
                            x.SurveyDate.Month == surveyDate.Month &&
                            x.SurveyDate.Day == surveyDate.Day &&
                            x.SurveyDate.Hour == surveyDate.Hour &&
                            x.SurveyDate.Minute == surveyDate.Minute &&
                            x.SurveyDate.Second == surveyDate.Second)
                        .Join(context.NodeMasters.AsNoTracking(), s => s.NodeId, nm => nm.NodeId, (surveyData, node) => new
                        {
                            surveyData,
                            node
                        })
                        .Where(x => x.node.AssetGuid == id)
                        .Select(x => new GlAnalysisSurveyCurveCoordinateDataModel()
                        {
                            Id = x.surveyData.Id,
                            NodeId = x.surveyData.NodeId,
                            SurveyDate = x.surveyData.SurveyDate
                        }
                        ).ToList();
                logger.WriteCId(Level.Trace, $"Finished {nameof(GLAnalysisSQLStore)}" +
                $" {nameof(GetGLAnalysisCurveCoordinatesSurveyDate)}", correlationId);

                return surveyDataEntities;
            }
        }

        /// <summary>
        /// Get the wellbore data.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="correlationId"></param>
        /// <returns>A <seealso cref="WellboreDataModel"/></returns>
        public WellboreDataModel GetWellboreData(Guid assetId, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(GLAnalysisSQLStore)}" +
                $" {nameof(GetWellboreData)}", correlationId);
            var wellboreData = new WellboreDataModel();

            wellboreData.Perforations = GetPerforations(assetId);

            using (var context = _contextFactory.GetContext())
            {
                var wellDetail = context.WellDetails.AsNoTracking()
                    .Join(context.NodeMasters.AsNoTracking(), x => x.NodeId, x => x.NodeId, ((wellDetail, node) => new
                    {
                        wellDetail,
                        node
                    })).FirstOrDefault(x => x.node.AssetGuid == assetId)?.wellDetail;

                if (wellDetail == null)
                {
                    return null;
                }

                wellboreData.ProductionDepth = wellDetail?.ProductionDepth;
                wellboreData.PackerDepth = wellDetail?.PackerDepth;
                wellboreData.HasPacker = wellDetail?.HasPacker;

                var tubings = context.Tubings.AsNoTracking().Join(context.NodeMasters.AsNoTracking(), x => x.NodeId, x => x.NodeId, ((tubing, node) => new
                {
                    tubing,
                    node
                })).Where(x => x.node.AssetGuid == assetId)
                .Select(x => new TubingModel
                {
                    Length = x.tubing.Length,
                }).ToList();

                wellboreData.Tubings = tubings;
            }
            logger.WriteCId(Level.Trace, $"Finished {nameof(GLAnalysisSQLStore)}" +
                $" {nameof(GetWellboreData)}", correlationId);

            return wellboreData;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get the <seealso cref="GLAnalysisResponse"/> based on asset id,test date and analysis type.
        /// </summary>
        /// <param name="analysisResultId"></param>
        /// <returns>The <seealso cref="GLAnalysisResponse"/> data model.</returns>
        private GLAnalysisResponse GetGLAnalysisResultData(int analysisResultId)
        {
            GLAnalysisResponse result = new GLAnalysisResponse();

            using (var context = _contextFactory.GetContext())
            {
                var resultEntity = context.GLAnalysisResults.AsNoTracking().FirstOrDefault(x => x.Id == analysisResultId);

                if (resultEntity == null)
                {
                    return null;
                }

                CompileResult(ref result, resultEntity, analysisResultId, context);

                return result;
            }
        }

        /// <summary>
        /// Get the <seealso cref="GLAnalysisResponse"/> based on asset id,test date and analysis type.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="testDate">The test date.</param>
        /// <param name="analysisType">The analysis type.</param>
        /// <returns>The <seealso cref="GLAnalysisResponse"/> data model.</returns>
        private GLAnalysisResponse GetGLAnalysisResultData(Guid assetId, DateTime testDate, int analysisType)
        {
            if (assetId == Guid.Empty)
            {
                return null;
            }

            GLAnalysisResponse result = new GLAnalysisResponse();

            using (var context = _contextFactory.GetContext())
            {
                var resultEntity = context.GLAnalysisResults.AsNoTracking()
                    .Join(context.NodeMasters.AsNoTracking(), l => l.NodeId, r => r.NodeId,
                        (glanalysisresults, nodemaster) => new
                        {
                            glanalysisresults,
                            nodemaster
                        }).Where(x => x.nodemaster.AssetGuid == assetId
                        && x.glanalysisresults.AnalysisType == analysisType
                        && x.glanalysisresults.TestDate == (DateTime)new SqlDateTime(testDate))
                    .Select(x => x.glanalysisresults).FirstOrDefault();

                if (resultEntity == null)
                {
                    return null;
                }

                result.AnalysisResultEntity = MapAnalysisResultEntityToModel(resultEntity);

                var analysisResultId = resultEntity.Id;

                if (analysisResultId == 0)
                {
                    var paramAnalysisResultId = nameof(analysisResultId);
                    throw new ArgumentNullException(paramAnalysisResultId);
                }

                var results = context.GLWellValve.AsNoTracking()
                    .Join(context.GLValve.AsNoTracking(), wv => wv.GLValveId, v => v.Id, (wv, v) => new
                    {
                        wv,
                        v
                    })
                    .Join(context.GLValveStatus.AsNoTracking(), @t => @t.wv.Id, vs => vs.GLWellValveId, (@t, vs) => new
                    {
                        @t,
                        vs
                    })
                    .Where(@t => @t.vs.GLAnalysisResultId == analysisResultId)
                    .OrderBy(@t => @t.@t.wv.MeasuredDepth)
                    .Select(g => new
                    {
                        g.t,
                        g.vs
                    });

                var valveStatus = results.Select(a => a.vs);
                var glWellValve = results.Select(a => a.t);

                var valveStatusModel = new List<GLValveStatusModel>();

                foreach (var item in valveStatus.ToList())
                {
                    valveStatusModel.Add(new GLValveStatusModel
                    {
                        Id = item.Id,
                        ClosingPressureAtDepth = item.ClosingPressureAtDepth,
                        Depth = item.Depth,
                        GasRate = item.GasRate,
                        GLAnalysisResultId = analysisResultId,
                        GlwellValveId = analysisResultId,
                        InjectionPressureAtDepth = item.InjectionPressureAtDepth,
                        InjectionRateForTubingCriticalVelocity = item.InjectionRateForTubingCriticalVelocity,
                        IsInjectingGas = item.IsInjectingGas,
                        OpeningPressureAtDepth = item.OpeningPressureAtDepth,
                        PercentOpen = item.PercentOpen,
                        TubingCriticalVelocityAtDepth = item.TubingCriticalVelocityAtDepth,
                        ValveState = item.ValveState,
                    });
                }

                result.ValveStatusEntities = valveStatusModel;

                var wellValveModel = new List<GLWellValveModel>();

                foreach (var item in glWellValve.ToList())
                {
                    wellValveModel.Add(new GLWellValveModel
                    {
                        Id = item.wv.Id,
                        NodeId = item.wv.NodeId,
                        GLValveId = item.wv.GLValveId,
                        MeasuredDepth = item.wv.MeasuredDepth,
                        ClosingPressureAtDepth = item.wv.ClosingPressureAtDepth,
                        ClosingPressureAtSurface = item.wv.ClosingPressureAtSurface,
                        OpeningPressureAtDepth = item.wv.OpeningPressureAtDepth,
                        OpeningPressureAtSurface = item.wv.OpeningPressureAtSurface,
                        TestRackOpeningPressure = item.wv.TestRackOpeningPressure,
                        TrueVerticalDepth = item.wv.TrueVerticalDepth,
                        VerticalDepth = item.wv.VerticalDepth,
                        ValveId = item.v.Id,
                        Diameter = item.v.Diameter,
                        BellowsArea = item.v.BellowsArea,
                        Description = item.v.Description,
                        ManufacturerId = item.v.ManufacturerId,
                        OneMinusR = item.v.OneMinusR,
                        PortArea = item.v.PortArea,
                        PortSize = item.v.PortSize,
                        PortToBellowsAreaRatio = item.v.PortToBellowsAreaRatio,
                        ProductionPressureEffectFactor = item.v.ProductionPressureEffectFactor,
                    });
                }

                result.WellValveEntities = wellValveModel;

                result.WellOrificeStatus = GetWellOrificeStatus(analysisResultId);

                return result;
            }
        }

        private void CompileResult(ref GLAnalysisResponse result, GLAnalysisResultsEntity entity, int analysisResultId,
            XspocDbContext context)
        {
            if (entity == null)
            {
                return;
            }

            result.AnalysisResultEntity = MapAnalysisResultEntityToModel(entity);

            if (analysisResultId == 0)
            {
                var paramAnalysisResultId = nameof(analysisResultId);
                throw new ArgumentNullException(paramAnalysisResultId);
            }

            var results = context.GLWellValve.AsNoTracking()
                .Join(context.GLValve.AsNoTracking(), wv => wv.GLValveId, v => v.Id, (wv, v) => new
                {
                    wv,
                    v
                })
                .Join(context.GLValveStatus.AsNoTracking(), @t => @t.wv.Id, vs => vs.GLWellValveId, (@t, vs) => new
                {
                    @t,
                    vs
                })
                .Where(@t => @t.vs.GLAnalysisResultId == analysisResultId)
                .OrderBy(@t => @t.@t.wv.MeasuredDepth)
                .Select(g => new
                {
                    g.t,
                    g.vs
                });

            var valveStatus = results.Select(a => a.vs);
            var glWellValve = results.Select(a => a.t);

            var valveStatusModel = new List<GLValveStatusModel>();

            foreach (var item in valveStatus.ToList())
            {
                valveStatusModel.Add(new GLValveStatusModel
                {
                    Id = item.Id,
                    ClosingPressureAtDepth = item.ClosingPressureAtDepth,
                    Depth = item.Depth,
                    GasRate = item.GasRate,
                    GLAnalysisResultId = analysisResultId,
                    GlwellValveId = analysisResultId,
                    InjectionPressureAtDepth = item.InjectionPressureAtDepth,
                    InjectionRateForTubingCriticalVelocity = item.InjectionRateForTubingCriticalVelocity,
                    IsInjectingGas = item.IsInjectingGas,
                    OpeningPressureAtDepth = item.OpeningPressureAtDepth,
                    PercentOpen = item.PercentOpen,
                    TubingCriticalVelocityAtDepth = item.TubingCriticalVelocityAtDepth,
                    ValveState = item.ValveState,
                });
            }

            result.ValveStatusEntities = valveStatusModel;

            var wellValveModel = new List<GLWellValveModel>();

            foreach (var item in glWellValve.ToList())
            {
                wellValveModel.Add(new GLWellValveModel
                {
                    Id = item.wv.Id,
                    NodeId = item.wv.NodeId,
                    GLValveId = item.wv.GLValveId,
                    MeasuredDepth = item.wv.MeasuredDepth,
                    ClosingPressureAtDepth = item.wv.ClosingPressureAtDepth,
                    ClosingPressureAtSurface = item.wv.ClosingPressureAtSurface,
                    OpeningPressureAtDepth = item.wv.OpeningPressureAtDepth,
                    OpeningPressureAtSurface = item.wv.OpeningPressureAtSurface,
                    TestRackOpeningPressure = item.wv.TestRackOpeningPressure,
                    TrueVerticalDepth = item.wv.TrueVerticalDepth,
                    VerticalDepth = item.wv.VerticalDepth,
                    ValveId = item.v.Id,
                    Diameter = item.v.Diameter,
                    BellowsArea = item.v.BellowsArea,
                    Description = item.v.Description,
                    ManufacturerId = item.v.ManufacturerId,
                    OneMinusR = item.v.OneMinusR,
                    PortArea = item.v.PortArea,
                    PortSize = item.v.PortSize,
                    PortToBellowsAreaRatio = item.v.PortToBellowsAreaRatio,
                    ProductionPressureEffectFactor = item.v.ProductionPressureEffectFactor,
                });
            }

            result.WellValveEntities = wellValveModel;
            result.WellOrificeStatus = GetWellOrificeStatus(analysisResultId);
        }

        private GLWellOrificeStatusModel GetWellOrificeStatus(int analysisResultId)
        {
            if (analysisResultId == 0)
            {
                throw new ArgumentNullException(nameof(analysisResultId));
            }

            using (var context = _contextFactory.GetContext())
            {
                var status = context.GLOrificeStatus.AsNoTracking()
                    .Join(context.GLWellOrifice.AsNoTracking(), os => os.NodeId, o => o.NodeId, (os, o) => new
                    {
                        os,
                        o
                    })
                    .Where(@t => @t.os.GLAnalysisResultId == analysisResultId)
                    .Select(@t => new GLWellOrificeStatusModel
                    {
                        NodeId = @t.o.NodeId,
                        ManufacturerId = @t.o.ManufacturerId,
                        MeasuredDepth = @t.o.MeasuredDepth,
                        VerticalDepth = @t.o.VerticalDepth,
                        PortSize = @t.o.PortSize,
                        TrueVerticalDepth = @t.o.TrueVerticalDepth,
                        OrificeState = @t.os.OrificeState,
                        Depth = @t.os.Depth,
                        GLAnalysisResultId = @t.os.GLAnalysisResultId,
                        InjectionPressureAtDepth = @t.os.InjectionPressureAtDepth,
                        InjectionRateForTubingCriticalVelocity = @t.os.InjectionRateForTubingCriticalVelocity,
                        IsInjectingGas = @t.os.IsInjectingGas,
                        TubingCriticalVelocityAtDepth = @t.os.TubingCriticalVelocityAtDepth
                    }).FirstOrDefault();

                return status;
            }
        }

        private GLAnalysisResultModel MapAnalysisResultEntityToModel(GLAnalysisResultsEntity resultEntity)
        {
            var analysisResultModel = new GLAnalysisResultModel
            {
                Id = resultEntity.Id,
                NodeId = resultEntity.NodeId,
                TestDate = resultEntity.TestDate,
                ProcessedDate = resultEntity.ProcessedDate,
                Success = resultEntity.Success,
                ResultMessage = resultEntity.ResultMessage,
                ResultMessageTemplate = resultEntity.ResultMessageTemplate,
                GasInjectionDepth = resultEntity.GasInjectionDepth,
                MeasuredWellDepth = resultEntity.MeasuredWellDepth,
                OilRate = resultEntity.OilRate,
                WaterRate = resultEntity?.WaterRate,
                GasRate = resultEntity.GasRate,
                WellheadPressure = resultEntity?.WellheadPressure,
                CasingPressure = resultEntity?.CasingPressure,
                WaterCut = resultEntity?.WaterCut,
                GasSpecificGravity = resultEntity?.GasSpecificGravity,
                WaterSpecificGravity = resultEntity.WaterSpecificGravity,
                WellheadTemperature = resultEntity?.WellheadTemperature,
                BottomholeTemperature = resultEntity.BottomholeTemperature,
                OilSpecificGravity = resultEntity.OilSpecificGravity,
                CasingId = resultEntity.CasingId,
                TubingId = resultEntity.TubingId,
                TubingOD = resultEntity.TubingOD,
                ReservoirPressure = resultEntity.ReservoirPressure,
                BubblepointPressure = resultEntity.BubblepointPressure,
                FormationGor = resultEntity.FormationGor,
                ProductivityIndex = resultEntity.ProductivityIndex,
                RateAtBubblePoint = resultEntity.RateAtBubblePoint,
                RateAtMaxOil = resultEntity.RateAtMaxOil,
                RateAtMaxLiquid = resultEntity.RateAtMaxLiquid,
                IPRSlope = resultEntity.IPRSlope,
                FlowingBhp = resultEntity.FlowingBhp,
                MinimumFbhp = resultEntity.MinimumFbhp,
                InjectedGLR = resultEntity.InjectedGLR,
                InjectedGasRate = resultEntity.InjectedGasRate,
                MaxLiquidRate = resultEntity.MaxLiquidRate,
                InjectionRateForMaxLiquidRate = resultEntity.InjectionRateForMaxLiquidRate,
                GLRForMaxLiquidRate = resultEntity.GLRForMaxLiquidRate,
                OptimumLiquidRate = resultEntity.OptimumLiquidRate,
                InjectionRateForOptimumLiquidRate = resultEntity.InjectionRateForOptimumLiquidRate,
                GlrforOptimumLiquidRate = resultEntity.GlrforOptimumLiquidRate,
                KillFluidLevel = resultEntity.KillFluidLevel,
                ReservoirFluidLevel = resultEntity.ReservoirFluidLevel,
                FlowCorrelationId = resultEntity.FlowCorrelationId,
                OilViscosityCorrelationId = resultEntity.OilViscosityCorrelationId,
                OilFormationVolumeFactorCorrelationId = resultEntity.OilFormationVolumeFactorCorrelationId,
                SolutionGasOilRatioCorrelationId = resultEntity.SolutionGasOilRatioCorrelationId,
                TubingCriticalVelocityCorrelationId = resultEntity.TubingCriticalVelocityCorrelationId,
                TubingPressureSource = resultEntity.TubingPressureSource,
                CasingPressureSource = resultEntity.CasingPressureSource,
                PercentCO2 = resultEntity.PercentCO2,
                PercentN2 = resultEntity.PercentN2,
                PercentH2S = resultEntity.PercentH2S,
                WellHeadTemperatureSource = resultEntity.WellHeadTemperatureSource,
                BottomholeTemperatureSource = resultEntity.BottomholeTemperatureSource,
                OilSpecificGravitySource = resultEntity.OilSpecificGravitySource,
                WaterSpecificGravitySource = resultEntity.WaterSpecificGravitySource,
                GasSpecificGravitySource = resultEntity.GasSpecificGravitySource,
                InjectedGasRateSource = resultEntity.InjectedGasRateSource,
                OilRateSource = resultEntity.OilRateSource,
                WaterRateSource = resultEntity.WaterRateSource,
                GasRateSource = resultEntity.GasRateSource,
                DownholeGageDepth = resultEntity.DownholeGageDepth,
                DownholeGagePressure = resultEntity.DownholeGagePressure,
                DownholeGagePressureSource = resultEntity.DownholeGagePressureSource,
                UseDownholeGageInAnalysis = resultEntity.UseDownholeGageInAnalysis,
                AdjustedAnalysisToDownholeGaugeReading = resultEntity.AdjustedAnalysisToDownholeGaugeReading,
                AnalysisType = resultEntity.AnalysisType,
                IsProcessed = resultEntity.IsProcessed,
                MultiphaseFlowCorrelationSource = resultEntity.MultiphaseFlowCorrelationSource,
                MeasuredInjectionDepthFromAnalysis = resultEntity.MeasuredInjectionDepthFromAnalysis,
                VerticalInjectionDepthFromAnalysis = resultEntity.VerticalInjectionDepthFromAnalysis,
                VerticalWellDepth = resultEntity.VerticalWellDepth,
                ZfactorCorrelationId = resultEntity.ZfactorCorrelationId,
                IsInjectingBelowTubing = resultEntity.IsInjectingBelowTubing,
                GrossRate = resultEntity.GrossRate,
                ValveCriticalVelocity = resultEntity.ValveCriticalVelocity,
                TubingCriticalVelocity = resultEntity.TubingCriticalVelocity,
                FlowingBHPAtInjectionDepth = resultEntity.FlowingBHPAtInjectionDepth,
                EstimateInjectionDepth = resultEntity.EstimateInjectionDepth,
                InjectionRateForTubingCriticalVelocity = resultEntity.InjectionRateForTubingCriticalVelocity,
                FbhpforOptimumLiquidRate = resultEntity.FbhpforOptimumLiquidRate,
            };

            return analysisResultModel;
        }

        private GLWellDetailModel GetGLWellData(Guid assetId)
        {
            using (var context = _contextFactory.GetContext())
            {
                var wellDetail = context.GLWellDetail.AsNoTracking()
                    .Join(context.NodeMasters.AsNoTracking(), l => l.NodeId, r => r.NodeId,
                        (welldetail, nodemaster) => new
                        {
                            welldetail,
                            nodemaster
                        }).Where(x => x.nodemaster.AssetGuid == assetId)
                    .Select(x => x.welldetail).FirstOrDefault();

                if (wellDetail != null)
                {
                    var wellDetailModel = new GLWellDetailModel
                    {
                        NodeId = wellDetail.NodeId,
                        CostTypeId = wellDetail.CostTypeId,
                        CompressionCost = wellDetail.CompressionCost,
                        DownholeGageDepth = wellDetail.DownholeGageDepth,
                        EstimateInjectionDepth = wellDetail.EstimateInjectionDepth,
                        FormationGasOilRatio = wellDetail.FormationGasOilRatio,
                        GasInjectionDepth = wellDetail.GasInjectionDepth,
                        GasInjectionPressure = wellDetail.GasInjectionPressure,
                        InjectedGasSpecificGravity = wellDetail.InjectedGasSpecificGravity,
                        InjectingBelowTubing = wellDetail.InjectingBelowTubing,
                        InjectionCost = wellDetail.InjectionCost,
                        PercentCO2 = wellDetail.PercentCO2,
                        PercentH2S = wellDetail.PercentH2S,
                        PercentN2 = wellDetail.PercentN2,
                        SeparationCost = wellDetail.SeparationCost,
                        UseDownholeGageInAnalysis = wellDetail.UseDownholeGageInAnalysis,
                        ValveConfigurationOption = wellDetail.ValveConfigurationOption,
                    };

                    return wellDetailModel;
                }
                else
                {
                    return null;
                }
            }
        }

        private IList<PerforationModel> GetPerforations(Guid assetId)
        {
            var perforations = new List<PerforationModel>();

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

                perforations = perforationResult.ToList();
            }

            return perforations;
        }

        private void CreateAnalysisData(ref GLAnalysisResponse response, Guid assetId, DateTime testDate)
        {
            var nodeData = GetNodeMasterData(assetId);

            var glWellData = GetGLWellData(assetId);

            if (nodeData == null || response == null || glWellData == null)
            {
                return;
            }

            response.NodeMasterData = nodeData;
            response.WellDetail = glWellData;
            response.TestDate = testDate;
            response.GasRatePhrase = GetSystemParameterData("GLIncludeInjGasInTest");
        }

        #endregion

    }
}
