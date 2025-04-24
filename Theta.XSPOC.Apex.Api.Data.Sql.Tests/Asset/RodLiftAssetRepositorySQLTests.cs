using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup;
using Theta.XSPOC.Apex.Api.Data.Entity.RodLift;
using Theta.XSPOC.Apex.Api.Data.Entity.XDIAG;
using Theta.XSPOC.Apex.Api.Data.Sql.Asset;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Quantity.Measures;

namespace Theta.XSPOC.Apex.Api.Data.Sql.Tests.Asset
{
    [TestClass]
    public class RodLiftAssetRepositorySQLTests : DataStoreTestBase
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullTest()
        {
            _ = new RodLiftAssetSQLStore(null);
        }

        [TestMethod]
        public async Task GetAssetStatusDataAsyncEmptyAssetIdTest()
        {
            var dbContext = SetupMockContext();
            var mockFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(dbContext.Object);

            var repo = new RodLiftAssetSQLStore(mockFactory.Object);

            var result = await repo.GetAssetStatusDataAsync(Guid.Empty);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAssetStatusDataAsyncAssetIdNotFoundTest()
        {
            var nodeMasterData = GetNodeMasterData();
            var companyData = TestUtilities.GetCompanyData().AsQueryable();
            var mockCompanyDbSet = SetupMockDbSetAsync(companyData);

            var mockNodeMasterDbSet = SetupMockDbSetAsync(nodeMasterData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContext.Setup(x => x.Company).Returns(mockCompanyDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var assetId = Guid.Parse("22E97D89-FD91-46A2-8190-99EEB9A4E26E");

            var repo = new RodLiftAssetSQLStore(mockFactory.Object);

            var result = await repo.GetAssetStatusDataAsync(assetId);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAssetStatusDataAsyncTest()
        {
            var nodeMasterData = GetNodeMasterData();
            var companyData = TestUtilities.GetCompanyData().AsQueryable();
            var rodMotorSettingData = GetRodMotorSettingData();
            var pocTypeData = GetPocTypeData();
            var wellDetailsData = GetWellDetailData();
            var xdiagResultLastData = GetXDIAGResultLastData();
            var pumpingMotorData = GetRodPumpingUnitData();
            var pumpingManufacturerData = GetRodPumpingUnitManufacturerData();
            var customPumpingUnitData = GetCustomPumpingUnitData();
            var motorKindData = GetRodMotorKindData();
            var espAnalysisResultData = GetESPAnalysisResultData();

            var mockNodeMasterDbSet = SetupMockDbSetWithProjectionAsync(nodeMasterData, GetNodeProjectedData());
            var mockRodMotorSettingDbSet = SetupMockDbSet(rodMotorSettingData);
            var mockPocTypeDbSet = SetupMockDbSet(pocTypeData);
            var mockWellDetailDbSet = SetupMockDbSet(wellDetailsData);
            var mockXDIAGResultLastDbSet = SetupMockDbSet(xdiagResultLastData);
            var mockPumpingMotorDbSet = SetupMockDbSet(pumpingMotorData);
            var mockPumpingManufacturerDbSet = SetupMockDbSet(pumpingManufacturerData);
            var mockCustomPumpingUnitDbSet = SetupMockDbSet(customPumpingUnitData);
            var mockMotorKindDbSet = SetupMockDbSet(motorKindData);
            var mockESPAnlysisResultDbSet = SetupMockDbSet(espAnalysisResultData);
            var mockCompanyDbSet = SetupMockDbSetAsync(companyData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContext.Setup(x => x.RodMotorSettings).Returns(mockRodMotorSettingDbSet.Object);
            mockContext.Setup(x => x.PocType).Returns(mockPocTypeDbSet.Object);
            mockContext.Setup(x => x.WellDetails).Returns(mockWellDetailDbSet.Object);
            mockContext.Setup(x => x.XDIAGResultLast).Returns(mockXDIAGResultLastDbSet.Object);
            mockContext.Setup(x => x.PumpingUnits).Returns(mockPumpingMotorDbSet.Object);
            mockContext.Setup(x => x.PumpingUnitManufacturer).Returns(mockPumpingManufacturerDbSet.Object);
            mockContext.Setup(x => x.CustomPumpingUnits).Returns(mockCustomPumpingUnitDbSet.Object);
            mockContext.Setup(x => x.RodMotorKinds).Returns(mockMotorKindDbSet.Object);
            mockContext.Setup(x => x.ESPAnalysisResults).Returns(mockESPAnlysisResultDbSet.Object);
            mockContext.Setup(x => x.Company).Returns(mockCompanyDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var assetId = Guid.Parse("12E97D89-FD91-46A2-8A90-99ECB9E4E26E");

            var repo = new RodLiftAssetSQLStore(mockFactory.Object);

            var result = await repo.GetAssetStatusDataAsync(assetId);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetAssetStatusDataAsyncMissingDataTest()
        {
            var nodeMasterData = GetNodeMasterData();
            var companyData = TestUtilities.GetCompanyData().AsQueryable();
            var rodMotorSettingData = GetRodMotorSettingData();
            var pocTypeData = GetPocTypeData();
            var wellDetailsData = GetWellDetailData();
            var xdiagResultLastData = GetXDIAGResultLastData();
            var pumpingMotorData = GetRodPumpingUnitData();
            var pumpingManufacturerData = GetRodPumpingUnitManufacturerData();
            var customPumpingUnitData = GetCustomPumpingUnitData();
            var motorKindData = GetRodMotorKindData();
            var espAnalysisResultData = GetESPAnalysisResultData();

            var mockNodeMasterDbSet = SetupMockDbSetWithProjectionAsync(nodeMasterData, GetNodeProjectedData());
            var mockRodMotorSettingDbSet = SetupMockDbSet(rodMotorSettingData);
            var mockPocTypeDbSet = SetupMockDbSet(pocTypeData);
            var mockWellDetailDbSet = SetupMockDbSet(wellDetailsData);
            var mockXDIAGResultLastDbSet = SetupMockDbSet(xdiagResultLastData);
            var mockPumpingMotorDbSet = SetupMockDbSet(pumpingMotorData);
            var mockPumpingManufacturerDbSet = SetupMockDbSet(pumpingManufacturerData);
            var mockCustomPumpingUnitDbSet = SetupMockDbSet(customPumpingUnitData);
            var mockMotorKindDbSet = SetupMockDbSet(motorKindData);
            var mockESPAnlysisResultDbSet = SetupMockDbSet(espAnalysisResultData);
            var mockCompanyDbSet = SetupMockDbSetAsync(companyData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContext.Setup(x => x.RodMotorSettings).Returns(mockRodMotorSettingDbSet.Object);
            mockContext.Setup(x => x.PocType).Returns(mockPocTypeDbSet.Object);
            mockContext.Setup(x => x.WellDetails).Returns(mockWellDetailDbSet.Object);
            mockContext.Setup(x => x.XDIAGResultLast).Returns(mockXDIAGResultLastDbSet.Object);
            mockContext.Setup(x => x.PumpingUnits).Returns(mockPumpingMotorDbSet.Object);
            mockContext.Setup(x => x.PumpingUnitManufacturer).Returns(mockPumpingManufacturerDbSet.Object);
            mockContext.Setup(x => x.CustomPumpingUnits).Returns(mockCustomPumpingUnitDbSet.Object);
            mockContext.Setup(x => x.RodMotorKinds).Returns(mockMotorKindDbSet.Object);
            mockContext.Setup(x => x.ESPAnalysisResults).Returns(mockESPAnlysisResultDbSet.Object);
            mockContext.Setup(x => x.Company).Returns(mockCompanyDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var assetId = Guid.Parse("E14E3F8A-5D32-4670-A5C1-94993BD311CA");

            var repo = new RodLiftAssetSQLStore(mockFactory.Object);

            var result = await repo.GetAssetStatusDataAsync(assetId);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetRodStringAsyncEmptyAssetIdTest()
        {
            var dbContext = SetupMockContext();
            var mockFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(dbContext.Object);

            var repo = new RodLiftAssetSQLStore(mockFactory.Object);

            var result = await repo.GetRodStringAsync(Guid.Empty);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetRodStringAsyncAssetIdNotFoundTest()
        {
            var nodeMasterData = GetNodeMasterData();
            var rodStringData = GetRodStringData();
            var rodStringSizeData = GetRodStringSizeData();
            var rodGradeData = GetRodGradeData();

            var mockNodeMasterDbSet = SetupMockDbSet(nodeMasterData);
            var mockRodStringDbSet = SetupMockDbSet(rodStringData);
            var mockRodStringSizeDbSet = SetupMockDbSet(rodStringSizeData);
            var mockRodGradeDbSet = SetupMockDbSet(rodGradeData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContext.Setup(x => x.RodStrings).Returns(mockRodStringDbSet.Object);
            mockContext.Setup(x => x.RodStringSizes).Returns(mockRodStringSizeDbSet.Object);
            mockContext.Setup(x => x.RodStringGrades).Returns(mockRodGradeDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var assetId = Guid.Parse("22E97D89-FD91-46A2-8190-99EEB9A4E26E");

            var repo = new RodLiftAssetSQLStore(mockFactory.Object);

            var result = await repo.GetRodStringAsync(assetId);

            Assert.IsNotNull(result);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetRodStringAsyncTest()
        {
            var nodeMasterData = GetNodeMasterData();
            var rodStringData = GetRodStringData();
            var rodStringSizeData = GetRodStringSizeData();
            var rodGradeData = GetRodGradeData();

            var mockNodeMasterDbSet = SetupMockDbSet(nodeMasterData);
            var mockRodStringDbSet = SetupMockDbSet(rodStringData);
            var mockRodStringSizeDbSet = SetupMockDbSet(rodStringSizeData);
            var mockRodGradeDbSet = SetupMockDbSet(rodGradeData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContext.Setup(x => x.RodStrings).Returns(mockRodStringDbSet.Object);
            mockContext.Setup(x => x.RodStringSizes).Returns(mockRodStringSizeDbSet.Object);
            mockContext.Setup(x => x.RodStringGrades).Returns(mockRodGradeDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var assetId = Guid.Parse("12E97D89-FD91-46A2-8A90-99ECB9E4E26E");

            var repo = new RodLiftAssetSQLStore(mockFactory.Object);

            var result = await repo.GetRodStringAsync(assetId);

            Assert.IsNotNull(result);

            Assert.AreEqual(2, result.Count);

            Assert.AreEqual(Length.FromFeet(1000), result[0].Length);
            Assert.AreEqual(1.5d, result[0].Diameter);
            Assert.AreEqual("Rod Grade 1", result[0].RodStringGradeName);
            Assert.AreEqual((short)1, result[0].RodStringPositionNumber);
            Assert.AreEqual("Rod Grade Size 2", result[0].RodStringSizeDisplayName);
            Assert.AreEqual("ft", result[0].UnitString);

            Assert.AreEqual(Length.FromFeet(500), result[1].Length);
            Assert.AreEqual(2d, result[1].Diameter);
            Assert.AreEqual("Rod Grade 2", result[1].RodStringGradeName);
            Assert.AreEqual((short)2, result[1].RodStringPositionNumber);
            Assert.AreEqual("Rod Grade Size 1", result[1].RodStringSizeDisplayName);
            Assert.AreEqual("ft", result[1].UnitString);
        }

        #endregion

        #region Private Data Setup Methods

        private IQueryable<NodeMasterEntity> GetNodeMasterData()
        {
            return new List<NodeMasterEntity>()
            {
                new NodeMasterEntity()
                {
                    AssetGuid = Guid.Parse("12E97D89-FD91-46A2-8A90-99ECB9E4E26E"),
                    NodeId = "Test 1",
                    PocType = 8,
                    PercentCommunicationsYesterday = 87,
                    CommStatus = "OK",
                    FirmwareVersion = 1.043f,
                    LastGoodScanTime = new DateTime(2023, 2, 1, 2, 3, 4),
                    PumpFillage = 78,
                    Node = "i127.0.0.1|502|1",
                    RunStatus = "Running",
                    TimeInState = 6000,
                    TodayRuntimePercent = 98,
                    YesterdayRuntimePercent = 67,
                    Enabled = true,
                    CompanyId = 1
                },
                new NodeMasterEntity()
                {
                    AssetGuid = Guid.Parse("E13E3F8A-5D32-4670-A5C1-94993BD311CA"),
                    NodeId = "Test 2",
                    PocType = 17,
                    PercentCommunicationsYesterday = 88,
                    CommStatus = "Timeout",
                    FirmwareVersion = 2.21f,
                    PumpFillage = 87,
                    Node = "i127.0.0.1|502|100",
                    RunStatus = "Idle",
                    TimeInState = 5000,
                    TodayRuntimePercent = 100,
                    YesterdayRuntimePercent = 47,
                    CompanyId = 1
                },
                new NodeMasterEntity()
                {
                    AssetGuid = Guid.Parse("E14E3F8A-5D32-4670-A5C1-94993BD311CA"),
                    NodeId = "Test 3",
                    PocType = 17,
                    PercentCommunicationsYesterday = 88,
                    CommStatus = "Timeout",
                    FirmwareVersion = 2.21f,
                    PumpFillage = 87,
                    Node = "i127.0.0.1|502|100",
                    RunStatus = "Idle",
                    TimeInState = 5000,
                    TodayRuntimePercent = 100,
                    YesterdayRuntimePercent = 47,
                    CompanyId = 1
                },
            }.AsQueryable();
        }

        private IQueryable<RodEntity> GetRodStringData()
        {
            return new List<RodEntity>()
            {
                new RodEntity()
                {
                    NodeId = "Test 1",
                    RodNum = 2,
                    Length = 500,
                    Diameter = 2,
                    Grade = "Rod Grade 2",
                    RodGradeId = 2,
                    RodGuideId = 1,
                    RodSizeId = 1,
                },
                new RodEntity()
                {
                    NodeId = "Test 1",
                    RodNum = 1,
                    Length = 1000,
                    Diameter = 1.5,
                    Grade = "Rod Grade 1",
                    RodGradeId = 1,
                    RodGuideId = 1,
                    RodSizeId = 2,
                },
                new RodEntity()
                {
                    NodeId = "Test 2",
                    RodNum = 1,
                    Length = 2000,
                    Diameter = 1,
                    Grade = "Rod Grade 100",
                    RodGradeId = 100,
                    RodGuideId = 3,
                    RodSizeId = 4,
                },
            }.AsQueryable();
        }

        private IQueryable<RodGradeEntity> GetRodGradeData()
        {
            return new List<RodGradeEntity>()
            {
                new RodGradeEntity()
                {
                    RodGradeId = 2,
                    Name = "Rod Grade Name 2",
                },
                new RodGradeEntity()
                {
                    RodGradeId = 1,
                    Name = "Rod Grade Name 1",
                },
                new RodGradeEntity()
                {
                    RodGradeId = 100,
                    Name = "Rod Grade Name 100",
                },
            }.AsQueryable();
        }

        private IQueryable<RodStringSizeEntity> GetRodStringSizeData()
        {
            return new List<RodStringSizeEntity>()
            {
                new RodStringSizeEntity()
                {
                    Diameter = 2,
                    RodSizeId = 1,
                    DisplayName = "Rod Grade Size 1",
                },
                new RodStringSizeEntity()
                {
                    Diameter = 1.5f,
                    RodSizeId = 2,
                    DisplayName = "Rod Grade Size 2",
                },
                new RodStringSizeEntity()
                {
                    Diameter = 1,
                    RodSizeId = 4,
                    DisplayName = "Rod Grade Size 4",
                },
            }.AsQueryable();
        }

        private IQueryable<POCTypeEntity> GetPocTypeData()
        {
            return new List<POCTypeEntity>()
            {
                new POCTypeEntity()
                {
                    PocType = 8,
                    Description = "Poc Type 8"
                },
                new POCTypeEntity()
                {
                    PocType = 9,
                    Description = "Poc Type 9"
                },
                new POCTypeEntity()
                {
                    PocType = 17,
                    Description = "Poc Type 17"
                },
            }.AsQueryable();
        }

        private IQueryable<WellDetailsEntity> GetWellDetailData()
        {
            return new List<WellDetailsEntity>()
            {
                new WellDetailsEntity()
                {
                    NodeId = "Test 1",
                    PumpingUnitId = "PU1",
                    MotorSettingId = 1,
                    MotorTypeId = 2,
                    CasingPressure = 123,
                    StrokesPerMinute = 5,
                    PlungerDiameter = 54.5f,
                    PrimeMoverType = "Primer Mover Type 1",
                    StrokeLength = 8.5f,
                    FluidLevel = 321,
                    MotorSizeId = 22,
                    LastWellTestDate = new DateTime(2022, 1, 2, 3, 4, 5),
                    GasRate = 444f,
                    GrossRate = 555f,
                    WaterCut = 0.5f,
                    PumpDepth = 332,
                    TubingPressure = 657,
                },
                new WellDetailsEntity()
                {
                    NodeId = "Test 2",
                    PumpingUnitId = "PU1",
                    MotorSettingId = 9,
                    MotorTypeId = 8,
                    CasingPressure = 222,
                    StrokesPerMinute = 4,
                    PlungerDiameter = 14.3f,
                    PrimeMoverType = "Primer Mover Type 2",
                    StrokeLength = 7.5f,
                    FluidLevel = 888,
                    MotorSizeId = 33,
                    LastWellTestDate = new DateTime(2023, 1, 2, 3, 4, 5),
                    GasRate = 888f,
                    GrossRate = 777f,
                    WaterCut = 0.3f,
                    PumpDepth = 223,
                    TubingPressure = 7958,
                },
            }.AsQueryable();
        }

        private IQueryable<XDIAGResultsLastEntity> GetXDIAGResultLastData()
        {
            return new List<XDIAGResultsLastEntity>()
            {
                new XDIAGResultsLastEntity()
                {
                    NodeId = "Test 1",
                    PumpEfficiencyPercentage = 33,
                    GearBoxLoadPercentage = 22,
                    UnitStructuralLoad = 11,
                    MaxRodLoad = 44,
                    PumpEfficiency = 55,
                    FillagePercentage = 66,
                    MotorLoad = 77,
                },
                new XDIAGResultsLastEntity()
                {
                    NodeId = "Test 2",
                    PumpEfficiencyPercentage = 333,
                    GearBoxLoadPercentage = 222,
                    UnitStructuralLoad = 111,
                    MaxRodLoad = 444,
                    PumpEfficiency = 555,
                    FillagePercentage = 661,
                    MotorLoad = 777,
                },
            }.AsQueryable();
        }

        private IQueryable<RodMotorSettingEntity> GetRodMotorSettingData()
        {
            return new List<RodMotorSettingEntity>()
            {
                new RodMotorSettingEntity()
                {
                    Id = 1,
                    MotorSizeId = 22,
                    RatedHP = 75,
                },
                new RodMotorSettingEntity()
                {
                    Id = 2,
                    MotorSizeId = 15,
                    RatedHP = 150,
                },
            }.AsQueryable();
        }

        private IQueryable<RodMotorKindEntity> GetRodMotorKindData()
        {
            return new List<RodMotorKindEntity>()
            {
                new RodMotorKindEntity()
                {
                    Id = 2,
                    Name = "Motor Kind Name 2",
                },
                new RodMotorKindEntity()
                {
                    Id = 12,
                    Name = "Motor Kind Name 12",
                },
            }.AsQueryable();
        }

        private IQueryable<PumpingUnitsEntity> GetRodPumpingUnitData()
        {
            return new List<PumpingUnitsEntity>()
            {
                new PumpingUnitsEntity()
                {
                    Id = 1,
                    UnitId = "PU1",
                    ManufacturerId = "LPU1",
                    APIDesignation = "API Designation 1",
                    UnitName = "Unit Name 1",

                },
                new PumpingUnitsEntity()
                {
                    Id = 2,
                    UnitId = "PU2",
                    ManufacturerId = "LPU1",
                    APIDesignation = "API Designation 2",
                    UnitName = "Unit Name 2",

                },
                new PumpingUnitsEntity()
                {
                    Id = 3,
                    UnitId = "PU3",
                    ManufacturerId  = "SPU1",
                    APIDesignation = "API Designation 3",
                    UnitName = "Unit Name 3",

                },
            }.AsQueryable();
        }

        private IQueryable<PumpingUnitManufacturerEntity> GetRodPumpingUnitManufacturerData()
        {
            return new List<PumpingUnitManufacturerEntity>()
            {
                new PumpingUnitManufacturerEntity()
                {
                    Id = 1,
                    UnitTypeId = 1,
                    ManufacturerAbbreviation = "LPU1",
                },
                new PumpingUnitManufacturerEntity()
                {
                    Id = 2,
                    UnitTypeId = 2,
                    ManufacturerAbbreviation = "LPU2",
                },
            }.AsQueryable();
        }

        private IQueryable<CustomPumpingUnitEntity> GetCustomPumpingUnitData()
        {
            return new List<CustomPumpingUnitEntity>()
            {
                new CustomPumpingUnitEntity()
                {
                    Id = "PU1",
                    APIDesignation = "Custom API Designation 1",
                    Name = "Custom Unit Name 1",
                    Manufacturer = "Custom Manufacturer 1",

                },
                new CustomPumpingUnitEntity()
                {
                    Id = "PU12",
                    APIDesignation = "Custom API Designation 25",
                    Name = "Custom Unit Name 25",
                    Manufacturer = "Custom Manufacturer 25",

                },
            }.AsQueryable();
        }

        private IQueryable<ESPAnalysisResultsEntity> GetESPAnalysisResultData()
        {
            return new List<ESPAnalysisResultsEntity>()
            {
                new ESPAnalysisResultsEntity()
                {
                    NodeId = "Test 1",
                    CalculatedFluidLevelAbovePump = 43.21f,
                    TestDate = new DateTime(2021, 1, 2, 3, 4, 5),
                },
                new ESPAnalysisResultsEntity()
                {
                    NodeId = "Test 1",
                    CalculatedFluidLevelAbovePump = 41.31f,
                    TestDate = new DateTime(2022, 1, 2, 3, 4, 5),
                },
                new ESPAnalysisResultsEntity()
                {
                    NodeId = "Test 2",
                    CalculatedFluidLevelAbovePump = 33.21f,
                    TestDate = new DateTime(2021, 1, 2, 3, 4, 5),
                },
            }.AsQueryable();
        }

        private IQueryable<NodeProjected> GetNodeProjectedData()
        {
            return GetNodeMasterData().Select(m => new NodeProjected()
            {
                AssetGUID = m.AssetGuid,
                NodeId = m.NodeId,
                PocType = m.PocType,
            });
        }

        #endregion

    }
}