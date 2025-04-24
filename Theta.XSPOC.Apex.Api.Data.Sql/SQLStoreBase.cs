using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Data.Sql
{
    /// <summary>
    /// Utility class for data retrieval from db.
    /// </summary>
    public class SQLStoreBase : ISQLStoreBase
    {

        #region Protected Members

        /// <summary>
        /// The <seealso cref="IThetaDbContextFactory{XspocDbContext}"/> to get the <seealso cref="XspocDbContext"/> to use
        /// for database operations.
        /// </summary>
        protected IThetaDbContextFactory<XspocDbContext> ContextFactory { get; private set; }

        /// <summary>
        /// Gets or sets the logger factory.
        /// </summary>
        protected IThetaLoggerFactory LoggerFactory { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructs a new <seealso cref="SQLStoreBase"/>.
        /// </summary>
        /// <param name="contextFactory">
        /// The <seealso cref="IThetaDbContextFactory{XspocDbContext}"/> to get the <seealso cref="XspocDbContext"/> 
        /// to use for database operations.
        /// </param>
        /// <param name="loggerFactory">The factory for creating the logger.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="contextFactory"/> is null.
        /// </exception>
        public SQLStoreBase(IThetaDbContextFactory<XspocDbContext> contextFactory, IThetaLoggerFactory loggerFactory)
        {
            ContextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
            LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        #endregion

        #region ISQLStoreBase Implementation

        /// <summary>
        /// Get the carddata based on asset id and card date.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <param name="cardDate">The card date.</param>
        /// <returns>The <seealso cref="CardDataModel"/> entity</returns>
        public CardDataModel GetCardData(Guid assetId, DateTime cardDate)
        {
            if (assetId == Guid.Empty)
            {
                return null;
            }

            using (var context = ContextFactory.GetContext())
            {
                var cardData = context.CardData.AsNoTracking()
                    .Join(context.NodeMasters.AsNoTracking(), l => l.NodeId, r => r.NodeId,
                        (carddata, nodemaster) => new
                        {
                            carddata,
                            nodemaster
                        }).Where(x => x.nodemaster.AssetGuid == assetId &&
                        x.carddata.CardDate.Day == cardDate.Day &&
                        x.carddata.CardDate.Month == cardDate.Month &&
                        x.carddata.CardDate.Year == cardDate.Year &&
                        x.carddata.CardDate.Hour == cardDate.Hour &&
                        x.carddata.CardDate.Minute == cardDate.Minute &&
                        x.carddata.CardDate.Second == cardDate.Second)
                    .Select(x => x.carddata).FirstOrDefault();

                if (cardData != null)
                {
                    var cardDataModel = new CardDataModel
                    {
                        NodeId = cardData.NodeId,
                        StrokesPerMinute = cardData.StrokesPerMinute,
                        CardDate = cardData.CardDate,
                        Fillage = cardData.Fillage,
                        FillBasePercent = cardData.FillBasePercent,
                        SecondaryPumpFillage = cardData.SecondaryPumpFillage,
                        AreaLimit = cardData.AreaLimit,
                        Area = cardData.Area,
                        LoadSpanLimit = cardData.LoadSpanLimit,
                        CardArea = cardData.CardArea,
                        CardType = cardData.CardType,
                        CauseId = cardData.CauseId,
                        Runtime = cardData.Runtime,
                        StrokeLength = cardData.StrokeLength,
                    };

                    return cardDataModel;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Get the well details based on asset guid
        /// </summary>
        /// <param name="assetId">The asset GUID</param>
        /// <returns>The <seealso cref="WellDetailsModel"/></returns>
        public WellDetailsModel GetWellDetails(Guid assetId)
        {
            using (var context = ContextFactory.GetContext())
            {
                var wellDetails = context.WellDetails.AsNoTracking()
                    .Join(context.NodeMasters.AsNoTracking(), l => l.NodeId, r => r.NodeId,
                        (welldetails, nodemaster) => new
                        {
                            welldetails,
                            nodemaster
                        }).Where(x => x.nodemaster.AssetGuid == assetId)
                    .Select(x => x.welldetails).FirstOrDefault();

                if (wellDetails != null)
                {
                    var wellDetailsModel = new WellDetailsModel
                    {
                        NodeId = wellDetails.NodeId,
                        Runtime = wellDetails.Runtime,
                        PlungerDiameter = wellDetails.PlungerDiameter,
                        PumpDepth = wellDetails.PumpDepth,
                        Cycles = wellDetails.Cycles,
                        IdleTime = wellDetails.IdleTime,
                        POCGrossRate = wellDetails.POCGrossRate,
                        FluidLevel = wellDetails.FluidLevel,
                        StrokesPerMinute = wellDetails.StrokesPerMinute,
                        PumpType = wellDetails.PumpType,
                        StrokeLength = wellDetails?.StrokeLength,
                    };

                    return wellDetailsModel;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets a list of all well tests for esp analysis by asset id.
        /// </summary>
        /// <param name="assetId">The asset GUID</param>
        /// <returns>The <seealso cref="List{WellTestModel}"/></returns>
        public IList<WellTestModel> GetListWellTest(Guid assetId)
        {
            IList<WellTestModel> wellTestModelList = new List<WellTestModel>();
            using (var context = ContextFactory.GetContext())
            {
                var wellTest = context.WellTest.AsNoTracking()
                    .Join(context.NodeMasters.AsNoTracking(), l => l.NodeId, r => r.NodeId,
                        (wellTest, nodemaster) => new
                        {
                            wellTest,
                            nodemaster
                        }).Where(x => x.nodemaster.AssetGuid == assetId && x.wellTest.Approved == true)
                    .Select(x => x.wellTest).ToList();

                if (wellTest != null)
                {
                    foreach (WellTestEntity wellDetail in wellTest)
                    {
                        var welltest = new WellTestModel
                        {
                            NodeId = wellDetail.NodeId,
                            TestDate = wellDetail.TestDate,
                            WaterRate = wellDetail.WaterRate,
                            GasRate = wellDetail.GasRate,
                            OilRate = wellDetail.OilRate,
                            Approved = wellDetail.Approved,
                            Duration = wellDetail.Duration,
                            FluidAbovePump = wellDetail.FluidAbovePump,
                        };
                        wellTestModelList.Add(welltest);
                    }

                    return wellTestModelList;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Get the current raw scan data based on asset id
        /// </summary>
        /// <param name="assetId">The asset GUID</param>
        /// <returns>The <seealso cref="IList{CurrentRawScanDataModel}" /></returns>
        public IList<CurrentRawScanDataModel> GetCurrentRawScanData(Guid assetId)
        {
            if (assetId == Guid.Empty)
            {
                return null;
            }

            using (var context = ContextFactory.GetContext())
            {
                var currentRawScanData = context.CurrentRawScans.AsNoTracking()
                    .Join(context.NodeMasters.AsNoTracking(), l => l.NodeId, r => r.NodeId,
                        (currentrawscandata, nodemaster) => new
                        {
                            currentrawscandata,
                            nodemaster
                        }).Where(x => x.nodemaster.AssetGuid == assetId)
                    .Select(x => x.currentrawscandata).ToList();

                if (currentRawScanData != null)
                {
                    var currentRawScanDataModel = new List<CurrentRawScanDataModel>();

                    foreach (var item in currentRawScanData)
                    {
                        currentRawScanDataModel.Add(new CurrentRawScanDataModel
                        {
                            NodeId = item.NodeId,
                            Address = item.Address,
                            DateTimeUpdated = item.DateTimeUpdated,
                            StringValue = item.StringValue,
                            Value = item.Value
                        });
                    }

                    return currentRawScanDataModel;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the CustomPumpingUnit based on pumping unit id.
        /// </summary>
        /// <param name="pumpingUnitId">The pumping unit id.</param>
        /// <returns>The <seealso cref="CustomPumpingUnitModel"/></returns>
        public CustomPumpingUnitModel GetCustomPumpingUnits(string pumpingUnitId)
        {
            using (var context = ContextFactory.GetContext())
            {
                var customPumpingUnit = context.CustomPumpingUnits.AsNoTracking().FirstOrDefault(x => x.Id == pumpingUnitId);
                if (customPumpingUnit != null)
                {
                    var customPumpingUnitModel = new CustomPumpingUnitModel
                    {
                        Manufacturer = customPumpingUnit.Manufacturer,
                        APIDesignation = customPumpingUnit.APIDesignation,
                        CrankHoles = customPumpingUnit.CrankHoles,
                        Id = customPumpingUnit.Id,
                        Name = customPumpingUnit.Name,
                        MaximumStroke = customPumpingUnit.MaximumStroke,
                        Type = customPumpingUnit.Type,
                    };

                    return customPumpingUnitModel;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the NodeMaster data based on asset guid.
        /// </summary>
        /// <param name="assetId">The asset GUID.</param>
        /// <returns>The <seealso cref="NodeMasterModel"/></returns>
        public NodeMasterModel GetNodeMasterData(Guid assetId)
        {
            var result = GetNodeMasterData(new List<Guid> { assetId });

            return result.FirstOrDefault();
        }

        /// <summary>
        /// Retrieves the node data based on asset ids.
        /// </summary>
        /// <param name="assetId">The asset ids.</param>
        /// <returns>The list of <seealso cref="NodeMasterModel"/>.</returns>
        public IList<NodeMasterModel> GetNodeMasterData(IList<Guid> assetId)
        {
            using (var context = ContextFactory.GetContext())
            {
                var result = context.NodeMasters.AsNoTracking().Where(x => assetId.Contains(x.AssetGuid))
                    .GroupJoin(context.Company.AsNoTracking(), nm => nm.CompanyId, c => c.Id,
                        (nm, c) => new
                        {
                            NodeMaster = nm,
                            Company = c,
                        })
                    .SelectMany(sp => sp.Company.DefaultIfEmpty(),
                        (x, r) => new
                        {
                            x.NodeMaster,
                            Company = r,
                        })
                    .ToList()
                .Select(x => new NodeMasterModel()
                {
                    NodeId = x.NodeMaster.NodeId,
                    AssetGuid = x.NodeMaster.AssetGuid,
                    PocType = x.NodeMaster.PocType,
                    RunStatus = x.NodeMaster.RunStatus,
                    TimeInState = x.NodeMaster.TimeInState,
                    TodayCycles = x.NodeMaster.TodayCycles,
                    TodayRuntime = x.NodeMaster.TodayRuntime,
                    InferredProd = x.NodeMaster.InferredProd,
                    Enabled = x.NodeMaster.Enabled,
                    ApplicationId = x.NodeMaster.ApplicationId,
                    PortId = x.NodeMaster.PortId,
                    CompanyGuid = x.Company?.CustomerGUID,
                    Tzoffset = x.NodeMaster.Tzoffset,
                    Tzdaylight = x.NodeMaster.Tzdaylight,
                });

                return result.ToList();
            }
        }

        /// <summary>
        /// Get the well details based on asset guid
        /// </summary>
        /// <param name="assetId">The asset GUID</param>
        /// <param name="cardDate">The card date</param>
        /// <returns>The <seealso cref="WellTestModel"/></returns>
        public WellTestModel GetWellTestData(Guid assetId, DateTime cardDate)
        {
            using (var context = ContextFactory.GetContext())
            {
                var wellTestData = context.WellTest.AsNoTracking()
                    .Join(context.NodeMasters.AsNoTracking(), l => l.NodeId, r => r.NodeId,
                        (welltest, nodemaster) => new
                        {
                            welltest,
                            nodemaster
                        }).Where(x => x.nodemaster.AssetGuid == assetId
                        && x.welltest.TestDate <= cardDate && x.welltest.Approved == true)
                    .Select(x => x.welltest).OrderByDescending(x => x.TestDate).FirstOrDefault();

                if (wellTestData != null)
                {
                    var wellTestModel = new WellTestModel
                    {
                        TestDate = wellTestData.TestDate,
                        OilRate = wellTestData.OilRate,
                        GasRate = wellTestData.GasRate,
                        WaterRate = wellTestData.WaterRate,
                        NodeId = wellTestData.NodeId,
                        Approved = wellTestData.Approved,
                    };

                    return wellTestModel;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Get the well details based on asset guid
        /// </summary>
        /// <param name="assetId">The asset GUID</param>
        /// <param name="cardDate">The card date</param>
        /// <returns>The <seealso cref="XDiagResultsModel"/></returns>
        public XDiagResultsModel GetXDiagResultData(Guid assetId, DateTime cardDate)
        {
            using (var context = ContextFactory.GetContext())
            {
                var xDiagResults = context.XDiagResult.AsNoTracking()
                    .Join(context.NodeMasters.AsNoTracking(), l => l.NodeId, r => r.NodeId,
                        (xdiagresult, nodemaster) => new
                        {
                            xdiagresult,
                            nodemaster
                        }).Where(x => x.nodemaster.AssetGuid == assetId
                        && x.xdiagresult.Date == cardDate)
                    .Select(x => x.xdiagresult).FirstOrDefault();

                if (xDiagResults != null)
                {
                    var xDiagResultsModel = new XDiagResultsModel
                    {
                        Date = xDiagResults.Date,
                        GrossPumpStroke = xDiagResults.GrossPumpStroke,
                        FluidLoadonPump = xDiagResults.FluidLoadonPump,
                        BouyRodWeight = xDiagResults.BouyRodWeight,
                        DryRodWeight = xDiagResults.DryRodWeight,
                        PumpFriction = xDiagResults.PumpFriction,
                        PofluidLoad = xDiagResults.PofluidLoad,
                        PumpSize = xDiagResults.PumpSize,
                        DownholeCapacity24 = xDiagResults.DownholeCapacity24,
                        DownholeCapacityRuntime = xDiagResults.DownholeCapacityRuntime,
                        DownholeCapacityRuntimeFillage = xDiagResults.DownholeCapacityRuntimeFillage,
                        AdditionalUplift = xDiagResults.AdditionalUplift,
                        AdditionalUpliftGross = xDiagResults?.AdditionalUpliftGross,
                        PumpEfficiency = xDiagResults?.PumpEfficiency,
                        UpliftCalculationMissingRequirements = xDiagResults.UpliftCalculationMissingRequirements,
                        MaximumSpm = xDiagResults?.MaximumSpm,
                        ProductionAtMaximumSpm = xDiagResults?.ProductionAtMaximumSpm,
                        OilProductionAtMaximumSpm = xDiagResults!.OilProductionAtMaximumSpm,
                        MaximumSpmoverloadMessage = xDiagResults?.MaximumSpmoverloadMessage,
                        DownholeAnalysis = xDiagResults?.DownholeAnalysis,
                        InputAnalysis = xDiagResults?.InputAnalysis,
                        RodAnalysis = xDiagResults?.RodAnalysis,
                        SurfaceAnalysis = xDiagResults?.SurfaceAnalysis,
                        NodeId = xDiagResults?.NodeId,
                        PumpIntakePressure = xDiagResults?.PumpIntakePressure,
                        AddOilProduction = xDiagResults?.AddOilProduction,
                        AvgDHDSLoad = xDiagResults?.AvgDHDSLoad,
                        AvgDHDSPOLoad = xDiagResults?.AvgDHDSPOLoad,
                        AvgDHUSLoad = xDiagResults?.AvgDHUSLoad,
                        AvgDHUSPOLoad = xDiagResults?.AvgDHUSPOLoad,
                        CasingPressure = xDiagResults?.CasingPressure,
                        CurrentCBE = xDiagResults?.CurrentCBE,
                        CurrentCLF = xDiagResults?.CurrentCLF,
                        CurrentElecBG = xDiagResults?.CurrentElecBG,
                        CurrentElecBO = xDiagResults?.CurrentElecBO,
                        CurrentKWH = xDiagResults?.CurrentKWH,
                        CurrentMCB = xDiagResults?.CurrentMCB,
                        CurrentMonthlyElec = xDiagResults?.CurrentMonthlyElec,
                        ElecCostMinTorquePerBO = xDiagResults.ElecCostMinTorquePerBO,
                        ElecCostMonthly = xDiagResults?.ElecCostMonthly,
                        ElecCostPerBG = xDiagResults?.ElecCostPerBG,
                        ElecCostPerBO = xDiagResults?.ElecCostPerBO,
                        ElectCostMinEnergyPerBO = xDiagResults?.ElectCostMinEnergyPerBO,
                        FillagePct = xDiagResults?.FillagePct,
                        FluidLevelXDiag = xDiagResults?.FluidLevelXDiag,
                        Friction = xDiagResults?.Friction,
                        GearBoxLoadPct = xDiagResults?.GearBoxLoadPct,
                        GearboxTorqueRating = xDiagResults?.GearboxTorqueRating,
                        GrossProd = xDiagResults?.GrossProd,
                        LoadShift = xDiagResults?.LoadShift,
                        MaxRodLoad = xDiagResults?.MaxRodLoad,
                        MinElecCostPerBG = xDiagResults.MinElecCostPerBG,
                        MinEnerGBTorque = xDiagResults?.MinEnerGBTorque,
                        MinEnergyCBE = xDiagResults?.MinEnergyCBE,
                        MinEnergyCLF = xDiagResults?.MinEnergyCLF,
                        MinEnergyElecBG = xDiagResults?.MinEnergyElecBG,
                        MinEnergyGBLoadPct = xDiagResults?.MinEnergyGBLoadPct,
                        MinEnergyMCB = xDiagResults?.MinEnergyMCB,
                        MinEnergyMonthlyElec = xDiagResults.MinEnergyMonthlyElec,
                        MinGBTorque = xDiagResults?.MinGBTorque,
                        MinHP = xDiagResults?.MinHP,
                        MinMonthlyElecCost = xDiagResults?.MinMonthlyElecCost,
                        MinTorqueCBE = xDiagResults?.MinTorqueCBE,
                        MinTorqueCLF = xDiagResults?.MinTorqueCLF,
                        MinTorqueElecBG = xDiagResults?.MinTorqueElecBG,
                        MinTorqueElecBO = xDiagResults?.MinTorqueElecBO,
                        MinTorqueGBLoadPct = xDiagResults?.MinTorqueGBLoadPct,
                        MinTorqueKWH = xDiagResults?.MinTorqueKWH,
                        MinTorqueMCB = xDiagResults?.MinTorqueMCB,
                        MinTorqueMonthlyElec = xDiagResults?.MinTorqueMonthlyElec,
                        MotorLoad = xDiagResults?.MotorLoad,
                        MPRL = xDiagResults?.MPRL,
                        NetProd = xDiagResults?.NetProd,
                        NetStrokeApparent = xDiagResults?.NetStrokeApparent,
                        OilAPIGravity = xDiagResults?.OilAPIGravity,
                        PeakGBTorque = xDiagResults?.PeakGBTorque,
                        POFluidLoad = xDiagResults?.POFluidLoad,
                        PolRodHP = xDiagResults?.PolRodHP,
                        PPRL = xDiagResults?.PPRL,
                        ProductionAtMaximumSPM = xDiagResults?.ProductionAtMaximumSPM,
                        PumpEffPct = xDiagResults?.PumpEffPct,
                        SystemEffPct = xDiagResults?.SystemEffPct,
                        TubingMovement = xDiagResults?.TubingMovement,
                        TubingPressure = xDiagResults?.TubingPressure,
                        UnitStructLoad = xDiagResults?.UnitStructLoad,
                        WaterCutPct = xDiagResults.WaterCutPct,
                        WaterSG = xDiagResults?.WaterSG,
                    };

                    return xDiagResultsModel;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Get the value of system paramter
        /// </summary>
        /// <param name="paramName">The parameter name</param>
        /// <returns>The value of the paramter</returns>
        public string GetSystemParameterData(string paramName)
        {
            using (var context = ContextFactory.GetContext())
            {
                return string.IsNullOrWhiteSpace(context.SystemParameters.AsNoTracking()
                    .FirstOrDefault(x => x.Parameter == paramName)?.Value)
                    ? ""
                    : context.SystemParameters.AsNoTracking()
                        .FirstOrDefault(x => x.Parameter == paramName)?.Value;
            }
        }

        /// <summary>
        /// Gets GLAnalysis Result Model by asset id,testDate,analysisResultId and analysisTypeId.
        /// </summary>
        /// <param name="assetId">The asset GUID</param>
        /// <param name="testDate">test Date</param>
        /// <param name="analysisResultId">analysis Result Id</param>
        /// <param name="analysisTypeId">analysis Type Id</param>
        /// <returns>The <seealso cref="GLAnalysisResultModel"/></returns>
        public GLAnalysisResultModel GetGLAnalysisResult(Guid assetId, string testDate, int? analysisResultId,
            int? analysisTypeId)
        {
            GLAnalysisResultsEntity gLAnalysisResultsEntity = new GLAnalysisResultsEntity();
            GLAnalysisResultModel gLAnalysisResultModel = new GLAnalysisResultModel();
            using (var context = ContextFactory.GetContext())
            {
                var nodeId = context.NodeMasters.AsNoTracking()
                    .Where(x => x.AssetGuid.ToString().ToLower() == assetId.ToString().ToLower())
                    .Select(x => x.NodeId).SingleOrDefault();
                var testDateConverted = DateTime.Parse(testDate);
                if (analysisResultId == null)
                {
                    gLAnalysisResultsEntity = context.GLAnalysisResults.AsNoTracking().FirstOrDefault(e => e.NodeId == nodeId
                        && e.AnalysisType == analysisTypeId && e.TestDate == (DateTime)new SqlDateTime(testDateConverted));
                    if (gLAnalysisResultsEntity == null)
                    {
                        return gLAnalysisResultModel;
                    }

                    analysisResultId = gLAnalysisResultsEntity.Id;
                }
                else
                {
                    gLAnalysisResultsEntity = context.GLAnalysisResults.AsNoTracking().First(x => x.Id == analysisResultId);
                }

                if (gLAnalysisResultsEntity != null)
                {
                    #region Property mapping

                    gLAnalysisResultModel.Id = gLAnalysisResultsEntity.Id;
                    gLAnalysisResultModel.NodeId = gLAnalysisResultsEntity.NodeId;
                    gLAnalysisResultModel.TestDate = gLAnalysisResultsEntity.TestDate;
                    gLAnalysisResultModel.ProcessedDate = gLAnalysisResultsEntity.ProcessedDate;
                    gLAnalysisResultModel.Success = gLAnalysisResultsEntity.Success;
                    gLAnalysisResultModel.ResultMessage = gLAnalysisResultsEntity.ResultMessage;
                    gLAnalysisResultModel.GasInjectionDepth = gLAnalysisResultsEntity.GasInjectionDepth;
                    gLAnalysisResultModel.MeasuredWellDepth = gLAnalysisResultsEntity.MeasuredWellDepth;
                    gLAnalysisResultModel.OilRate = gLAnalysisResultsEntity.OilRate;
                    gLAnalysisResultModel.WaterRate = gLAnalysisResultsEntity.WaterRate;
                    gLAnalysisResultModel.GasRate = gLAnalysisResultsEntity.GasRate;
                    gLAnalysisResultModel.WellheadPressure = gLAnalysisResultsEntity.WellheadPressure;
                    gLAnalysisResultModel.CasingPressure = gLAnalysisResultsEntity.CasingPressure;
                    gLAnalysisResultModel.WaterCut = gLAnalysisResultsEntity.WaterCut;
                    gLAnalysisResultModel.GasSpecificGravity = gLAnalysisResultsEntity.GasSpecificGravity;
                    gLAnalysisResultModel.WaterSpecificGravity = gLAnalysisResultsEntity.WaterSpecificGravity;
                    gLAnalysisResultModel.WellheadTemperature = gLAnalysisResultsEntity.WellheadTemperature;
                    gLAnalysisResultModel.BottomholeTemperature = gLAnalysisResultsEntity.BottomholeTemperature;
                    gLAnalysisResultModel.OilSpecificGravity = gLAnalysisResultsEntity.OilSpecificGravity;
                    gLAnalysisResultModel.CasingId = gLAnalysisResultsEntity.CasingId;
                    gLAnalysisResultModel.TubingId = gLAnalysisResultsEntity.TubingId;
                    gLAnalysisResultModel.TubingOD = gLAnalysisResultsEntity.TubingOD;
                    gLAnalysisResultModel.ReservoirPressure = gLAnalysisResultsEntity.ReservoirPressure;
                    gLAnalysisResultModel.BubblepointPressure = gLAnalysisResultsEntity.BubblepointPressure;
                    gLAnalysisResultModel.FormationGor = gLAnalysisResultsEntity.FormationGor;
                    gLAnalysisResultModel.ProductivityIndex = gLAnalysisResultsEntity.ProductivityIndex;
                    gLAnalysisResultModel.RateAtBubblePoint = gLAnalysisResultsEntity.RateAtBubblePoint;
                    gLAnalysisResultModel.RateAtMaxOil = gLAnalysisResultsEntity.RateAtMaxOil;
                    gLAnalysisResultModel.RateAtMaxLiquid = gLAnalysisResultsEntity.RateAtMaxLiquid;
                    gLAnalysisResultModel.IPRSlope = gLAnalysisResultsEntity.IPRSlope;
                    gLAnalysisResultModel.FlowingBhp = gLAnalysisResultsEntity.FlowingBhp;
                    gLAnalysisResultModel.MinimumFbhp = gLAnalysisResultsEntity.MinimumFbhp;
                    gLAnalysisResultModel.InjectedGLR = gLAnalysisResultsEntity.InjectedGLR;
                    gLAnalysisResultModel.InjectedGasRate = gLAnalysisResultsEntity.InjectedGasRate;
                    gLAnalysisResultModel.MaxLiquidRate = gLAnalysisResultsEntity.MaxLiquidRate;
                    gLAnalysisResultModel.InjectionRateForMaxLiquidRate =
                        gLAnalysisResultsEntity.InjectionRateForMaxLiquidRate;
                    gLAnalysisResultModel.GLRForMaxLiquidRate = gLAnalysisResultsEntity.GLRForMaxLiquidRate;
                    gLAnalysisResultModel.OptimumLiquidRate = gLAnalysisResultsEntity.OptimumLiquidRate;
                    gLAnalysisResultModel.InjectionRateForOptimumLiquidRate =
                        gLAnalysisResultsEntity.InjectionRateForOptimumLiquidRate;
                    gLAnalysisResultModel.GlrforOptimumLiquidRate = gLAnalysisResultsEntity.GlrforOptimumLiquidRate;
                    gLAnalysisResultModel.KillFluidLevel = gLAnalysisResultsEntity.KillFluidLevel;
                    gLAnalysisResultModel.ReservoirFluidLevel = gLAnalysisResultsEntity.ReservoirFluidLevel;
                    gLAnalysisResultModel.FlowCorrelationId = gLAnalysisResultsEntity.FlowCorrelationId;
                    gLAnalysisResultModel.OilViscosityCorrelationId = gLAnalysisResultsEntity.OilViscosityCorrelationId;
                    gLAnalysisResultModel.OilFormationVolumeFactorCorrelationId =
                        gLAnalysisResultsEntity.OilFormationVolumeFactorCorrelationId;
                    gLAnalysisResultModel.SolutionGasOilRatioCorrelationId =
                        gLAnalysisResultsEntity.SolutionGasOilRatioCorrelationId;
                    gLAnalysisResultModel.TubingPressureSource = gLAnalysisResultsEntity.TubingPressureSource;
                    gLAnalysisResultModel.CasingPressureSource = gLAnalysisResultsEntity.CasingPressureSource;
                    gLAnalysisResultModel.PercentCO2 = gLAnalysisResultsEntity.PercentCO2;
                    gLAnalysisResultModel.PercentN2 = gLAnalysisResultsEntity.PercentN2;
                    gLAnalysisResultModel.PercentH2S = gLAnalysisResultsEntity.PercentH2S;
                    gLAnalysisResultModel.VerticalWellDepth = gLAnalysisResultsEntity.VerticalWellDepth;
                    gLAnalysisResultModel.ZfactorCorrelationId = gLAnalysisResultsEntity.ZfactorCorrelationId;
                    gLAnalysisResultModel.IsInjectingBelowTubing = gLAnalysisResultsEntity.IsInjectingBelowTubing;
                    gLAnalysisResultModel.GrossRate = gLAnalysisResultsEntity.GrossRate;
                    gLAnalysisResultModel.TubingCriticalVelocityCorrelationId =
                        gLAnalysisResultsEntity.TubingCriticalVelocityCorrelationId;
                    gLAnalysisResultModel.ValveCriticalVelocity = gLAnalysisResultsEntity.ValveCriticalVelocity;
                    gLAnalysisResultModel.TubingCriticalVelocity = gLAnalysisResultsEntity.TubingCriticalVelocity;
                    gLAnalysisResultModel.FlowingBHPAtInjectionDepth = gLAnalysisResultsEntity.FlowingBHPAtInjectionDepth;
                    gLAnalysisResultModel.EstimateInjectionDepth = gLAnalysisResultsEntity.EstimateInjectionDepth;
                    gLAnalysisResultModel.InjectionRateForTubingCriticalVelocity =
                        gLAnalysisResultsEntity.InjectionRateForTubingCriticalVelocity;
                    gLAnalysisResultModel.FbhpforOptimumLiquidRate = gLAnalysisResultsEntity.FbhpforOptimumLiquidRate;
                    gLAnalysisResultModel.WellHeadTemperatureSource = gLAnalysisResultsEntity.WellHeadTemperatureSource;
                    gLAnalysisResultModel.BottomholeTemperatureSource = gLAnalysisResultsEntity.BottomholeTemperatureSource;
                    gLAnalysisResultModel.OilSpecificGravitySource = gLAnalysisResultsEntity.OilSpecificGravitySource;
                    gLAnalysisResultModel.WaterSpecificGravitySource = gLAnalysisResultsEntity.WaterSpecificGravitySource;
                    gLAnalysisResultModel.GasSpecificGravitySource = gLAnalysisResultsEntity.GasSpecificGravitySource;
                    gLAnalysisResultModel.InjectedGasRateSource = gLAnalysisResultsEntity.InjectedGasRateSource;
                    gLAnalysisResultModel.OilRateSource = gLAnalysisResultsEntity.OilRateSource;
                    gLAnalysisResultModel.WaterRateSource = gLAnalysisResultsEntity.WaterRateSource;
                    gLAnalysisResultModel.GasRateSource = gLAnalysisResultsEntity.GasRateSource;
                    gLAnalysisResultModel.DownholeGageDepth = gLAnalysisResultsEntity.DownholeGageDepth;
                    gLAnalysisResultModel.DownholeGagePressure = gLAnalysisResultsEntity.DownholeGagePressure;
                    gLAnalysisResultModel.DownholeGagePressureSource = gLAnalysisResultsEntity.DownholeGagePressureSource;
                    gLAnalysisResultModel.UseDownholeGageInAnalysis = gLAnalysisResultsEntity.UseDownholeGageInAnalysis;
                    gLAnalysisResultModel.AdjustedAnalysisToDownholeGaugeReading =
                        gLAnalysisResultsEntity.AdjustedAnalysisToDownholeGaugeReading;
                    gLAnalysisResultModel.AnalysisType = gLAnalysisResultsEntity.AnalysisType;
                    gLAnalysisResultModel.IsProcessed = gLAnalysisResultsEntity.IsProcessed;
                    gLAnalysisResultModel.MultiphaseFlowCorrelationSource =
                        gLAnalysisResultsEntity.MultiphaseFlowCorrelationSource;
                    gLAnalysisResultModel.MeasuredInjectionDepthFromAnalysis =
                        gLAnalysisResultsEntity.MeasuredInjectionDepthFromAnalysis;
                    gLAnalysisResultModel.VerticalInjectionDepthFromAnalysis =
                        gLAnalysisResultsEntity.VerticalInjectionDepthFromAnalysis;
                    gLAnalysisResultModel.UseTVD = gLAnalysisResultsEntity.UseTVD;
                    gLAnalysisResultModel.ResultMessageTemplate = gLAnalysisResultsEntity.ResultMessageTemplate;

                    #endregion
                }
            }

            return gLAnalysisResultModel;
        }

        /// <summary>
        /// Gets List GLAnalysisCurveCoordinateModel Model by analysisResultId.
        /// </summary>
        /// <param name="analysisResultId">analysis Result Id</param>
        /// <returns>The <seealso cref="IList{GLAnalysisCurveCoordinateModel}"/></returns>
        public IList<GLAnalysisCurveCoordinateModel> GetGLWellValveData(int? analysisResultId)
        {
            IList<GLAnalysisCurveCoordinateModel> listGLAnalysisCurveCoordinateModel = new List<GLAnalysisCurveCoordinateModel>();
            if (analysisResultId == null)
            {
                throw new ArgumentNullException(nameof(analysisResultId));
            }

            if (analysisResultId is int == false)
            {
                throw new ArgumentException($"{nameof(analysisResultId)} is not an int.", nameof(analysisResultId));
            }

            using (var context = ContextFactory.GetContext())
            {
                var listGLWellValveModel = context.GLWellValve.AsNoTracking()
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
                    .Where(@t => @t.vs.GLAnalysisResultId == (int)analysisResultId)
                    .OrderBy(@t => @t.@t.wv.MeasuredDepth)
                    .Select(@t => new GLAnalysisCurveCoordinateModel
                    {
                        Id = @t.vs.Id,
                        GasRate = @t.vs.GasRate,
                        IsInjectingGas = @t.vs.IsInjectingGas,
                        State = @t.vs.ValveState,
                        InjectionPressureAtDepth = @t.vs.InjectionPressureAtDepth,
                        TubingCriticalVelocityAtDepth = @t.vs.TubingCriticalVelocityAtDepth,
                        InjectionRateForTubingCriticalVelocity = @t.vs.InjectionRateForTubingCriticalVelocity,
                        ClosingPressureAtDepth = @t.vs.ClosingPressureAtDepth,
                        OpeningPressureAtDepth = @t.vs.OpeningPressureAtDepth,
                        Depth = @t.vs.Depth,
                        GLWellValveTestRackOpeningPressure = @t.@t.wv.TestRackOpeningPressure,
                        GLWellValveClosingPressureAtDepth = @t.@t.wv.ClosingPressureAtDepth,
                        GLWellValveOpeningPressureAtDepth = @t.@t.wv.OpeningPressureAtDepth,
                        GLWellValveVerticalDepth = @t.@t.wv.VerticalDepth,
                        GLWellValveMeasuredDepth = @t.@t.wv.MeasuredDepth,
                        BellowsArea = @t.@t.v.BellowsArea,
                        Description = @t.@t.v.Description,
                        Diameter = @t.@t.v.Diameter,
                        Manufacturer = context.GLManufacturer
                            .Where(x => x.ManufacturerId == @t.@t.v.ManufacturerId)
                            .Select(x => x.Manufacturer).SingleOrDefault(),
                        OneMinusR = @t.@t.v.OneMinusR,
                        PortArea = @t.@t.v.PortArea,
                        PortSize = @t.@t.v.PortSize,
                        PortToBellowsAreaRatio = @t.@t.v.PortToBellowsAreaRatio,
                        ProductionPressureEffectFactor = @t.@t.v.ProductionPressureEffectFactor,
                    }).ToList();
            }

            return listGLAnalysisCurveCoordinateModel;
        }

        /// <summary>
        /// Gets List OrificeStatusModel Model by analysisResultId.
        /// </summary>
        /// <param name="analysisResultId">analysis Result Id</param>
        /// <returns>The <seealso cref="OrificeStatusModel"/></returns>
        public OrificeStatusModel GetOrificeStatus(int? analysisResultId)
        {
            OrificeStatusModel orificeStatusModel = new OrificeStatusModel();
            if (analysisResultId == null)
            {
                throw new ArgumentNullException(nameof(analysisResultId));
            }

            if (analysisResultId is int == false)
            {
                throw new ArgumentException($"{nameof(analysisResultId)} is not an int.", nameof(analysisResultId));
            }

            using (var context = ContextFactory.GetContext())
            {
                orificeStatusModel = context.GLOrificeStatus.AsNoTracking()
                    .Join(context.GLWellOrifice.AsNoTracking(), os => os.NodeId, o => o.NodeId, (os, o) => new
                    {
                        os,
                        o
                    })
                    .Where(@t => @t.os.GLAnalysisResultId == (int)analysisResultId)
                    .Select(@t => new OrificeStatusModel
                    {
                        InjectionPressureAtDepth = @t.os.InjectionPressureAtDepth,
                        IsInjectingGas = @t.os.IsInjectingGas,
                        State = @t.os.OrificeState,
                        TubingCriticalVelocityAtDepth = @t.os.TubingCriticalVelocityAtDepth,
                        InjectionRateForTubingCriticalVelocity = @t.os.InjectionRateForTubingCriticalVelocity,
                        Depth = @t.os.Depth,
                        Id = @t.o.NodeId,
                        FlowManufacturer =
                            context.GLManufacturer.Where(x => x.ManufacturerId == @t.o.ManufacturerId).SingleOrDefault(),
                        FlowMeasuredDepth = @t.o.MeasuredDepth,
                        FlowVerticalDepth = @t.o.VerticalDepth,
                        FlowPortSize = @t.o.PortSize,
                    }).FirstOrDefault();
            }

            return orificeStatusModel;
        }

        /// <summary>
        /// Gets List AnalysisResultCurvesModel Model by analysisResultId and application.
        /// </summary>
        /// <param name="analysisResultId">Analysis Result Id</param>
        /// <param name="application">Application ID</param>
        /// <returns>The <seealso cref="IList{AnalysisResultCurvesModel}"/></returns>
        public IList<AnalysisResultCurvesModel> GetAnalysisResultCurve(int? analysisResultId, int? application)
        {
            IList<AnalysisResultCurvesModel> analysisResultCurvesModellList = new List<AnalysisResultCurvesModel>();
            if (analysisResultId == null || application == null)
            {
                throw new ArgumentNullException(nameof(analysisResultId));
            }

            if (analysisResultId is int == false)
            {
                throw new ArgumentException($"{nameof(analysisResultId)} is not an int.", nameof(analysisResultId));
            }

            using (var context = ContextFactory.GetContext())
            {
                analysisResultCurvesModellList = context.AnalysisResultCurves.AsNoTracking()
                    .Join(context.CurveTypes.AsNoTracking(), ac => ac.CurveTypeId, ct => ct.Id, (ac, ct) => new { ac, ct })
                    .Where(x => x.ac.AnalysisResultId == analysisResultId && x.ct.ApplicationTypeId == application
                    && (x.ac.CurveTypeId != 25 && x.ac.CurveTypeId != 26))
                    .Select(x => new AnalysisResultCurvesModel
                    {
                        AnalysisResultCurveID = x.ac.Id,
                        CurveTypesID = x.ac.CurveTypeId,
                        Name = x.ct.Name,
                        Id = x.ct.Id,
                        Coordinates = context.CurveCoordinates
                        .Where(y => y.CurveId == x.ac.Id)
                        .Select(z => new Coordinates
                        {
                            X = z.X,
                            Y = z.Y,
                        }).ToList(),
                    }).ToList();
            }

            return analysisResultCurvesModellList;
        }

        #endregion

    }
}
