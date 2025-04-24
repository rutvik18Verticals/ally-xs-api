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
    /// Implements the IESPAnalysis interface
    /// </summary>
    public class ESPAnalysisSQLStore : SQLStoreBase, IESPAnalysis
    {

        #region Private Members

        private readonly IThetaDbContextFactory<NoLockXspocDbContext> _contextFactory;
        private readonly IThetaLoggerFactory _loggerFactory;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new <seealso cref="ESPAnalysisSQLStore"/> using the provided <paramref name="contextFactory"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{NoLockXspocDbContext}"/> to get the <seealso cref="NoLockXspocDbContext"/> 
        /// to use for database operations.
        /// </param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/> is null.
        /// or
        /// </exception>
        public ESPAnalysisSQLStore(IThetaDbContextFactory<NoLockXspocDbContext> contextFactory,
            IThetaLoggerFactory loggerFactory) : base(contextFactory, loggerFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region IESPAnalysis Implementation

        /// <summary>
        /// Get the esp analysis data by asset id and card date.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="testDateString">The test date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The <seealso cref="ESPAnalysisResponse"/>.</returns>
        public ESPAnalysisResponse GetESPAnalysisData(Guid assetId, string testDateString, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(ESPAnalysisSQLStore)} " +
                $"{nameof(GetESPAnalysisData)}", correlationId);
            var testDate = DateTime.Parse(testDateString);

            var nodeData = GetNodeMasterData(assetId);

            var resultEntity = GetESPAnalysisResultData(assetId, testDate);

            var pumpEntities = GetESPWellPumpData(assetId);

            if (nodeData == null || pumpEntities == null || resultEntity == null)
            {
                return null;
            }

            ESPAnalysisResponse response = new ESPAnalysisResponse();

            response.NodeMasterData = nodeData;
            response.AnalysisResultEntity = resultEntity;
            response.WellPumpEntities = pumpEntities;
            response.TestDate = testDate;

            logger.WriteCId(Level.Trace, $"Finished {nameof(ESPAnalysisSQLStore)} " +
                $"{nameof(GetESPAnalysisData)}", correlationId);

            return response;
        }

        /// <summary>
        /// Get the esp analysis result by node id and test date.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="testDate">The test date.</param>
        /// <param name="correlationId"></param>
        /// <returns>
        /// The <seealso cref="ESPAnalysisResultModel"/>.
        /// </returns>
        /// <exception cref="NotImplementedException">.</exception>
        public ESPAnalysisResultModel GetESPAnalysisResult(string nodeId, DateTime testDate, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(ESPAnalysisSQLStore)} " +
                $"{nameof(GetESPAnalysisResult)}", correlationId);

            var espAnalysisResult = new ESPAnalysisResultModel();

            using (var context = _contextFactory.GetContext())
            {
                var result = context.ESPAnalysisResults.AsNoTracking()
                    .FirstOrDefault(x => x.NodeId == nodeId &&
                        x.TestDate == testDate);

                if (result != null)
                {
                    espAnalysisResult.Id = result.Id;
                    espAnalysisResult.NodeId = result.NodeId;
                    espAnalysisResult.TestDate = result.TestDate;
                    espAnalysisResult.ProcessedDate = result.ProcessedDate;
                    espAnalysisResult.EsppumpId = result.EsppumpId;
                    espAnalysisResult.GrossRate = result.GrossRate;
                    espAnalysisResult.HeadAcrossPump = result.HeadAcrossPump;
                    espAnalysisResult.TotalVolumeAtPump = result.TotalVolumeAtPump;
                }
            }

            logger.WriteCId(Level.Trace, $"Finished {nameof(ESPAnalysisSQLStore)} " +
              $"{nameof(GetESPAnalysisResult)}", correlationId);

            return espAnalysisResult;
        }

        /// <summary>
        /// Gets the esp pressure profile data corresponding 
        /// to the <paramref name="assetId"/> and <paramref name="testDate"/>.
        /// </summary>
        /// <param name="assetId">The asset guid.</param>
        /// <param name="testDate">The test date.</param>
        /// <param name="correlationId"></param>
        /// <returns>The esp pressure profile data.</returns>
        public ESPPressureProfileModel GetESPPressureProfileData(Guid assetId, DateTime testDate, string correlationId)
        {
            var logger = _loggerFactory.Create(LoggingModel.SQLStore);
            logger.WriteCId(Level.Trace, $"Starting {nameof(ESPAnalysisSQLStore)} " +
                $"{nameof(GetESPPressureProfileData)}", correlationId);

            using (var context = _contextFactory.GetContext())
            {
                var result = context.ESPAnalysisResults.AsNoTracking()
                    .Join(context.NodeMasters.AsNoTracking(), ar => ar.NodeId, nm => nm.NodeId,
                        (espAnalysisResults, nodeMaster) => new { espAnalysisResults, nodeMaster })
                    .Where(x => x.nodeMaster.AssetGuid == assetId && x.espAnalysisResults.TestDate == testDate)
                    .GroupJoin(context.WellDetails.AsNoTracking(),
                        combined => combined.nodeMaster.NodeId,
                        wellDetails => wellDetails.NodeId,
                        (combined, wellDetails) => new { combined, wellDetails })
                    .SelectMany(
                        x => x.wellDetails.DefaultIfEmpty(),
                        (joined, y) => new ESPPressureProfileModel()
                        {
                            PumpIntakePressure = joined.combined.espAnalysisResults.PumpIntakePressure,
                            PumpDischargePressure = joined.combined.espAnalysisResults.PumpDischargePressure,
                            PumpStaticPressure = joined.combined.espAnalysisResults.PumpStaticPressure,
                            PressureAcrossPump = joined.combined.espAnalysisResults.PressureAcrossPump,
                            FrictionalPressureDrop = joined.combined.espAnalysisResults.FrictionalLossInTubing,
                            CasingPressure = joined.combined.espAnalysisResults.CasingPressure,
                            TubingPressure = joined.combined.espAnalysisResults.TubingPressure,
                            FlowingBottomholePressure = joined.combined.espAnalysisResults.FlowingBhp,
                            CompositeTubingSpecificGravity = joined.combined.espAnalysisResults.CompositeTubingSpecificGravity,
                            WaterRate = joined.combined.espAnalysisResults.WaterRate,
                            OilRate = joined.combined.espAnalysisResults.OilRate,
                            WaterSpecificGravity = joined.combined.espAnalysisResults.WaterSpecificGravity,
                            IsGasHandlingEnabled = joined.combined.espAnalysisResults.EnableGasHandling,
                            SpecificGravityOfOil = joined.combined.espAnalysisResults.SpecificGravityOfOil,
                            UseDischargeGaugeInAnalysis = joined.combined.espAnalysisResults.UseDischargeGageInAnalysis,
                            DischargeGaugePressure = joined.combined.espAnalysisResults.DischargeGaugePressure,
                            DischargeGaugeDepth = joined.combined.espAnalysisResults.DischargeGaugeDepth,
                            VerticalPumpDepth = joined.combined.espAnalysisResults.VerticalPumpDepth,
                            CalculatedFluidLevelAbovePump = joined.combined.espAnalysisResults.CalculatedFluidLevelAbovePump,
                            Perforations = context.Perforation.AsNoTracking().
                                Join(context.NodeMasters.AsNoTracking(),
                                    perforations => perforations.NodeId,
                                    nodeMaster => nodeMaster.NodeId,
                                    (perforations, nodeMaster) => new { perforations, nodeMaster })
                                .Where(x => x.nodeMaster.AssetGuid == assetId)
                                .Select(x => new ESPPerforation()
                                {
                                    Length = x.perforations.Interval,
                                    TopDepth = x.perforations.Depth,
                                }).ToList(),
                            ProductionDepth = y != null ? y.ProductionDepth : null,
                        })
                    .FirstOrDefault();

                logger.WriteCId(Level.Trace, $"Finished {nameof(ESPAnalysisSQLStore)} " +
                    $"{nameof(GetESPPressureProfileData)}", correlationId);

                return result;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get the <seealso cref="ESPAnalysisResultModel"/> based on asset id and test date.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="testDate">The test date.</param>
        /// <returns>The <seealso cref="ESPAnalysisResultModel"/> entity</returns>
        private ESPAnalysisResultModel GetESPAnalysisResultData(Guid assetId, DateTime testDate)
        {
            if (assetId == Guid.Empty)
            {
                return null;
            }

            using (var context = _contextFactory.GetContext())
            {
                var espAnalysisResultsData = context.ESPAnalysisResults.AsNoTracking()
                    .Join(context.NodeMasters.AsNoTracking(), l => l.NodeId, r => r.NodeId,
                        (espanalysisresults, nodemaster) => new
                        {
                            espanalysisresults,
                            nodemaster
                        }).Where(x => x.nodemaster.AssetGuid == assetId && x.espanalysisresults.TestDate == testDate)
                    .Select(x => x.espanalysisresults);

                if (espAnalysisResultsData != null && espAnalysisResultsData.Any())
                {
                    var espAnalysisResults = espAnalysisResultsData.FirstOrDefault();
                    var espAnalysisResultsModel = new ESPAnalysisResultModel
                    {
                        ProcessedDate = espAnalysisResults.ProcessedDate,
                        VerticalPumpDepth = espAnalysisResults.VerticalPumpDepth,
                        MeasuredPumpDepth = espAnalysisResults.MeasuredPumpDepth,
                        OilRate = espAnalysisResults.OilRate,
                        WaterRate = espAnalysisResults.WaterRate,
                        GasRate = espAnalysisResults.GasRate,
                        PumpIntakePressure = espAnalysisResults.PumpIntakePressure,
                        GrossRate = espAnalysisResults.GrossRate,
                        FluidLevelAbovePump = espAnalysisResults.FluidLevelAbovePump,
                        TubingPressure = espAnalysisResults.TubingPressure,
                        CasingPressure = espAnalysisResults.CasingPressure,
                        Frequency = espAnalysisResults.Frequency,
                        DischargeGaugePressure = espAnalysisResults.DischargeGaugePressure,
                        DischargeGageDepth = espAnalysisResults.DischargeGaugeDepth,
                        UseDischargeGageInAnalysis = espAnalysisResults.UseDischargeGageInAnalysis,
                        EnableGasHandling = espAnalysisResults.EnableGasHandling,
                        SpecificGravityOfGas = espAnalysisResults.SpecificGravityOfGas,
                        BottomholeTemperature = espAnalysisResults.BottomholeTemperature,
                        GasSeparatorEfficiency = espAnalysisResults.GasSeparatorEfficiency,
                        OilApi = espAnalysisResults.OilApi,
                        CasingId = espAnalysisResults.CasingId,
                        TubingOd = espAnalysisResults.TubingOd,
                        CasingValveClosed = espAnalysisResults.CasingValveClosed,
                        ProductivityIndex = espAnalysisResults.ProductivityIndex,
                        PressureAcrossPump = espAnalysisResults.PressureAcrossPump,
                        PumpDischargePressure = espAnalysisResults.PumpDischargePressure,
                        HeadAcrossPump = espAnalysisResults.HeadAcrossPump,
                        FrictionalLossInTubing = espAnalysisResults.FrictionalLossInTubing,
                        PumpEfficiency = espAnalysisResults.PumpEfficiency,
                        CalculatedFluidLevelAbovePump = espAnalysisResults.CalculatedFluidLevelAbovePump,
                        FluidSpecificGravity = espAnalysisResults.FluidSpecificGravity,
                        PumpStaticPressure = espAnalysisResults.PumpStaticPressure,
                        RateAtBubblePoint = espAnalysisResults.RateAtBubblePoint,
                        RateAtMaxOil = espAnalysisResults.RateAtMaxOil,
                        RateAtMaxLiquid = espAnalysisResults.RateAtMaxLiquid,
                        Iprslope = espAnalysisResults.IPRSlope,
                        WaterCut = espAnalysisResults.WaterCut,
                        GasOilRatioAtPump = espAnalysisResults.GasOilRatioAtPump,
                        SpecificGravityOfOil = espAnalysisResults.SpecificGravityOfOil,
                        FormationVolumeFactor = espAnalysisResults.FormationVolumeFactor,
                        GasCompressibilityFactor = espAnalysisResults.GasCompressibilityFactor,
                        GasVolumeFactor = espAnalysisResults.GasVolumeFactor,
                        ProducingGor = espAnalysisResults.ProducingGor,
                        GasInSolution = espAnalysisResults.GasInSolution,
                        FreeGasAtPump = espAnalysisResults.FreeGasAtPump,
                        OilVolumeAtPump = espAnalysisResults.OilVolumeAtPump,
                        GasVolumeAtPump = espAnalysisResults.GasVolumeAtPump,
                        TotalVolumeAtPump = espAnalysisResults.TotalVolumeAtPump,
                        FreeGas = espAnalysisResults.FreeGas,
                        TurpinParameter = espAnalysisResults.TurpinParameter,
                        CompositeTubingSpecificGravity = espAnalysisResults.CompositeTubingSpecificGravity,
                        GasDensity = espAnalysisResults.GasDensity,
                        LiquidDensity = espAnalysisResults.LiquidDensity,
                        AnnularSeparationEfficiency = espAnalysisResults.AnnularSeparationEfficiency,
                        TubingGas = espAnalysisResults.TubingGas,
                        TubingGor = espAnalysisResults.TubingGor,
                        Success = espAnalysisResults.Success,
                        ResultMessage = espAnalysisResults.ResultMessage,
                        ResultMessageTemplate = espAnalysisResults.ResultMessageTemplate,
                        PumpIntakePressureSource = espAnalysisResults.PumpIntakePressureSource,
                        FluidLevelAbovePumpSource = espAnalysisResults.FluidLevelAbovePumpSource,
                        TubingPressureSource = espAnalysisResults.TubingPressureSource,
                        CasingPressureSource = espAnalysisResults.CasingPressureSource,
                        FrequencySource = espAnalysisResults.FrequencySource,
                        WellHeadTemperatureSource = espAnalysisResults.WellHeadTemperatureSource,
                        BottomholeTemperatureSource = espAnalysisResults.BottomholeTemperatureSource,
                        OilSpecificGravitySource = espAnalysisResults.OilSpecificGravitySource,
                        WaterSpecificGravitySource = espAnalysisResults.WaterSpecificGravitySource,
                        GasSpecificGravitySource = espAnalysisResults.GasSpecificGravitySource,
                        OilRateSource = espAnalysisResults.OilRateSource,
                        WaterRateSource = espAnalysisResults.WaterRateSource,
                        GasRateSource = espAnalysisResults.GasRateSource,
                        DischargeGagePressureSource = espAnalysisResults.DischargeGagePressureSource,
                        MaxRunningFrequency = espAnalysisResults.MaxRunningFrequency,
                        MotorLoadPercentage = espAnalysisResults.MotorLoadPercentage,
                        FlowingBhp = espAnalysisResults.FlowingBhp,
                        WaterSpecificGravity = espAnalysisResults.WaterSpecificGravity,
                        TubingId = espAnalysisResults.TubingId,
                        WellHeadTemperature = espAnalysisResults.WellHeadTemperature,
                        HeadRelativeToPumpCurve = espAnalysisResults.HeadRelativeToPumpCurve,
                        HeadRelativeToWellPerformance = espAnalysisResults.HeadRelativeToWellPerformance,
                        HeadRelativeToRecommendedRange = espAnalysisResults.HeadRelativeToRecommendedRange,
                        FlowRelativeToRecommendedRange = espAnalysisResults.FlowRelativeToRecommendedRange,
                        DesignScore = espAnalysisResults.DesignScore,
                        PumpDegradation = espAnalysisResults.PumpDegradation,
                        Id = espAnalysisResults.Id,
                        MaxPotentialProductionRate = espAnalysisResults.MaxPotentialProductionRate,
                        MaxPotentialFrequency = espAnalysisResults.MaxPotentialFrequency,
                        ProductionIncreasePercentage = espAnalysisResults.ProductionIncreasePercentage,
                        UseTVD = espAnalysisResults.UseTVD,
                        NumberOfStages = espAnalysisResults.NumberOfStages,
                    };

                    return espAnalysisResultsModel;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Get the <seealso cref="List{ESPWellPumpModel}"/> based on asset id.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <returns>The <seealso cref="CardDataEntity"/> entity</returns>
        private IList<ESPWellPumpModel> GetESPWellPumpData(Guid assetId)
        {
            if (assetId == Guid.Empty)
            {
                return null;
            }

            using (var context = _contextFactory.GetContext())
            {
                var espWellPumpData = context.ESPWellPumps.AsNoTracking()
                    .Join(context.NodeMasters.AsNoTracking(), l => l.ESPWellId, r => r.NodeId,
                        (espwellpump, nodemaster) => new
                        {
                            espwellpump,
                            nodemaster
                        }).Where(x => x.nodemaster.AssetGuid == assetId)
                    .Select(x => x.espwellpump).ToList();

                if (espWellPumpData != null && espWellPumpData.Count > 0)
                {
                    List<ESPWellPumpModel> espPumps = new List<ESPWellPumpModel>();
                    foreach (var item in espWellPumpData)
                    {
                        espPumps.Add(new ESPWellPumpModel
                        {
                            ESPPumpId = item.ESPPumpId,
                            ESPWellId = item.ESPWellId,
                            NumberOfStages = item.NumberOfStages,
                            OrderNumber = item.OrderNumber,
                        });
                    }

                    return espPumps;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

    }
}
