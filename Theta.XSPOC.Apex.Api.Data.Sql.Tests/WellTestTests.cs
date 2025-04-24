using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Logging;
using NodeMaster = Theta.XSPOC.Apex.Api.Data.Entity.NodeMasterEntity;

namespace Theta.XSPOC.Apex.Api.Data.Sql.Tests
{
    [TestClass]
    public class WellTestTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNulDbContextTest()
        {
            _ = new WellTestsSQLStore(null, null);
        }

        [TestMethod]
        public void WellTestGetTblGLAnalysisResultDataTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockSqlStore = new Mock<ISQLStoreBase>();
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            mockSqlStore.Setup(x => x.GetListWellTest(It.IsAny<Guid>())).Returns(GetWellTestData());

            var nodeData = new List<NodeMaster>()
            {
                new NodeMaster()
                {
                    NodeId = "AssetId1",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.WellDetails)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetWellDetails().AsQueryable()).Object);
            mockContext.Setup(x => x.CardData)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCardData().AsQueryable()).Object);
            mockContext.Setup(x => x.WellTest)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetWellTestData().AsQueryable()).Object);
            mockContext.Setup(x => x.LocalePhrases)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetLocalePhraseData().AsQueryable()).Object);
            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(nodeData).Object);
            mockContext.Setup(x => x.CurrentRawScans)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCurrentRawScanData().AsQueryable()).Object);
            mockContext.Setup(x => x.CustomPumpingUnits)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCustomPumpingUnitsData().AsQueryable()).Object);
            mockContext.Setup(x => x.PumpingUnits)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetPumpingUnitsData().AsQueryable()).Object);
            mockContext.Setup(x => x.PumpingUnitManufacturer)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetPumpingUnitsManufacturersData().AsQueryable()).Object);
            mockContext.Setup(x => x.XDiagResult)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetXdiagResultsData().AsQueryable()).Object);
            mockContext.Setup(x => x.SystemParameters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetSystemParameter().AsQueryable()).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var mockLocalePhrase = new Mock<ILocalePhrases>();
            mockLocalePhrase.Setup(x => x.Get(7048, It.IsAny<string>()))
                .Returns(
                    "XDIAG has determined the pumping unit can increase speed to {0} SPM. This will result in an additional {1} barrels of gross and {2} barrels of oil.");
            mockLocalePhrase.Setup(x => x.Get(7049, It.IsAny<string>()))
                .Returns(
                    "XDIAG has determined that the pumping unit is already operating close to maximum speed for rod pump system. In order to capture potential uplift, design changes will be necessary.");
            mockLocalePhrase.Setup(x => x.Get(7117, It.IsAny<string>()))
                .Returns("Incremental production at {0} SPM is less than {1} bbls of oil per day.");
            mockLocalePhrase.Setup(x => x.Get(6819, It.IsAny<string>()))
                .Returns(
                    "An incremental oil uplift opportunity has been discovered for this well test, this well is capable of producing an additional {0} ({1}) of total fluid, resulting in an additional {2} ({3}) of oil.");
            mockLocalePhrase.Setup(x => x.Get(200, It.IsAny<string>())).Returns("N/A");
            mockLocalePhrase.Setup(x => x.Get(2091, It.IsAny<string>())).Returns("SWT Oil Yesterday");
            mockLocalePhrase.Setup(x => x.Get(2092, It.IsAny<string>())).Returns("SWT Water Yesterday");
            mockLocalePhrase.Setup(x => x.Get(2093, It.IsAny<string>())).Returns("SWT Gas Yesterday");

            var listAnalysisKeyModel = new List<GLAnalysisResultModel>()
            {
                new GLAnalysisResultModel()
                {
                    TestDate = DateTime.Now
                }
            };
            var welltests = new WellTestsSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);
            var response = welltests.GetESPWellTestsData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                , "DFC1D0AD-A824-4965-B78D-AB7755E32DD4");
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void WellTestGetTblGLAnalysisResultData()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockSqlStore = new Mock<ISQLStoreBase>();
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            mockSqlStore.Setup(x => x.GetListWellTest(It.IsAny<Guid>())).Returns(GetWellTestData());

            var nodeData = new List<NodeMaster>()
            {
                new NodeMaster()
                {
                    NodeId = "AssetId1",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.WellDetails)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetWellDetails().AsQueryable()).Object);
            mockContext.Setup(x => x.CardData)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCardData().AsQueryable()).Object);
            mockContext.Setup(x => x.WellTest)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetWellTestData().AsQueryable()).Object);
            mockContext.Setup(x => x.LocalePhrases)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetLocalePhraseData().AsQueryable()).Object);
            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(nodeData).Object);
            mockContext.Setup(x => x.CurrentRawScans)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCurrentRawScanData().AsQueryable()).Object);
            mockContext.Setup(x => x.CustomPumpingUnits)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCustomPumpingUnitsData().AsQueryable()).Object);
            mockContext.Setup(x => x.PumpingUnits)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetPumpingUnitsData().AsQueryable()).Object);
            mockContext.Setup(x => x.PumpingUnitManufacturer)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetPumpingUnitsManufacturersData().AsQueryable()).Object);
            mockContext.Setup(x => x.XDiagResult)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetXdiagResultsData().AsQueryable()).Object);
            mockContext.Setup(x => x.SystemParameters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetSystemParameter().AsQueryable()).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var mockLocalePhrase = new Mock<ILocalePhrases>();
            mockLocalePhrase.Setup(x => x.Get(7048, It.IsAny<string>()))
                .Returns(
                    "XDIAG has determined the pumping unit can increase speed to {0} SPM. This will result in an additional {1} barrels of gross and {2} barrels of oil.");
            mockLocalePhrase.Setup(x => x.Get(7049, It.IsAny<string>()))
                .Returns(
                    "XDIAG has determined that the pumping unit is already operating close to maximum speed for rod pump system. In order to capture potential uplift, design changes will be necessary.");
            mockLocalePhrase.Setup(x => x.Get(7117, It.IsAny<string>()))
                .Returns("Incremental production at {0} SPM is less than {1} bbls of oil per day.");
            mockLocalePhrase.Setup(x => x.Get(6819, It.IsAny<string>()))
                .Returns(
                    "An incremental oil uplift opportunity has been discovered for this well test, this well is capable of producing an additional {0} ({1}) of total fluid, resulting in an additional {2} ({3}) of oil.");
            mockLocalePhrase.Setup(x => x.Get(200, It.IsAny<string>())).Returns("N/A");
            mockLocalePhrase.Setup(x => x.Get(2091, It.IsAny<string>())).Returns("SWT Oil Yesterday");
            mockLocalePhrase.Setup(x => x.Get(2092, It.IsAny<string>())).Returns("SWT Water Yesterday");
            mockLocalePhrase.Setup(x => x.Get(2093, It.IsAny<string>())).Returns("SWT Gas Yesterday");

            var listAnalysisKeyModel = new List<GLAnalysisResultModel>()
            {
                new GLAnalysisResultModel()
                {
                    TestDate = DateTime.Now
                }
            };
            var welltests = new WellTestsSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);

            var response = welltests.GetESPWellTestsData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void GetGLAnalysisWellTestsPassWellTests()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockSqlStore = new Mock<ISQLStoreBase>();
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            mockSqlStore.Setup(x => x.GetListWellTest(It.IsAny<Guid>())).Returns(GetWellTestData());
            var nodeData = new List<NodeMaster>()
            {
                new NodeMaster()
                {
                    NodeId = "AssetId1",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.WellDetails)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetWellDetails().AsQueryable()).Object);
            mockContext.Setup(x => x.CardData)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCardData().AsQueryable()).Object);
            mockContext.Setup(x => x.WellTest)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetWellTestData().AsQueryable()).Object);
            mockContext.Setup(x => x.LocalePhrases)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetLocalePhraseData().AsQueryable()).Object);
            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(nodeData).Object);
            mockContext.Setup(x => x.CurrentRawScans)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCurrentRawScanData().AsQueryable()).Object);
            mockContext.Setup(x => x.CustomPumpingUnits)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCustomPumpingUnitsData().AsQueryable()).Object);
            mockContext.Setup(x => x.PumpingUnits)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetPumpingUnitsData().AsQueryable()).Object);
            mockContext.Setup(x => x.PumpingUnitManufacturer)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetPumpingUnitsManufacturersData().AsQueryable()).Object);
            mockContext.Setup(x => x.XDiagResult)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetXdiagResultsData().AsQueryable()).Object);
            mockContext.Setup(x => x.SystemParameters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetSystemParameter().AsQueryable()).Object);
            mockContext.Setup(x => x.GLAnalysisResults)
                .Returns(TestUtilities.SetupMockData(GetGLAnalysisResults().AsQueryable()).Object);
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var listAnalysisKeyModel = new List<GLAnalysisResultModel>()
            {
                new GLAnalysisResultModel()
                {
                    TestDate = DateTime.Now
                }
            };
            var welltests = new WellTestsSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);
            var response = welltests.GetGLAnalysisWellTestsData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), 1, 1, It.IsAny<string>());
            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.Count);
        }

        [TestMethod]
        public void GetGLAnalysisWellTestsNullDataWellTests()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockSqlStore = new Mock<ISQLStoreBase>();
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            mockSqlStore.Setup(x => x.GetListWellTest(It.IsAny<Guid>())).Returns(GetWellTestData());
            var nodeData = new List<NodeMaster>()
            {
                new NodeMaster()
                {
                    NodeId = "AssetId3",
                    PocType = 4,
                    InferredProd = 2,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD2")
                }
            }.AsQueryable();

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.WellDetails)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetWellDetails().AsQueryable()).Object);
            mockContext.Setup(x => x.CardData)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCardData().AsQueryable()).Object);
            mockContext.Setup(x => x.WellTest)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetWellTestData().AsQueryable()).Object);
            mockContext.Setup(x => x.LocalePhrases)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetLocalePhraseData().AsQueryable()).Object);
            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(nodeData).Object);
            mockContext.Setup(x => x.CurrentRawScans)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCurrentRawScanData().AsQueryable()).Object);
            mockContext.Setup(x => x.CustomPumpingUnits)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCustomPumpingUnitsData().AsQueryable()).Object);
            mockContext.Setup(x => x.PumpingUnits)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetPumpingUnitsData().AsQueryable()).Object);
            mockContext.Setup(x => x.PumpingUnitManufacturer)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetPumpingUnitsManufacturersData().AsQueryable()).Object);
            mockContext.Setup(x => x.XDiagResult)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetXdiagResultsData().AsQueryable()).Object);
            mockContext.Setup(x => x.SystemParameters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetSystemParameter().AsQueryable()).Object);
            mockContext.Setup(x => x.GLAnalysisResults)
                .Returns(TestUtilities.SetupMockData(GetGLAnalysisResults().AsQueryable()).Object);
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var listAnalysisKeyModel = new List<GLAnalysisResultModel>()
            {
                new GLAnalysisResultModel()
                {
                    TestDate = DateTime.Now
                }
            };
            var welltests = new WellTestsSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);
            var response = welltests.GetGLAnalysisWellTestsData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), 1, 1, It.IsAny<string>());
            Assert.IsNotNull(response);
            Assert.AreEqual(0, response.Count);
        }

        #endregion

        #region Private Methods

        private IList<WellTestModel> GetWellTestData()
        {
            var wellTests = new List<WellTestModel>()
            {
                new WellTestModel()
                {
                    NodeId = "AssetId1",
                    TestDate = new DateTime(2023, 5, 11, 22, 29, 55),
                    OilRate = 37.7f,
                    WaterRate = 37.7f,
                    GasRate = 6f,
                    Approved = true,
                }
            };

            return wellTests;
        }

        private List<GLAnalysisResultsEntity> GetGLAnalysisResults()
        {
            var glAnalysisResults = new List<GLAnalysisResultsEntity>()
            {
                new GLAnalysisResultsEntity()
                {
                    Id = 1,
                    NodeId = "AssetId1",
                    GrossRate = 421.625f,
                    TestDate = new DateTime(2023, 5, 11, 22, 29, 55),
                    ProcessedDate = new DateTime(2023, 5, 11, 22, 29, 55),
                    Success = true,
                    GasInjectionDepth = 6530.91f,
                    OilRate = 75,
                    WaterRate = 225,
                    GasRate = 500,
                    WellheadPressure = 150,
                    CasingPressure = 850,
                    WaterCut = 75,
                    GasSpecificGravity = 0.758f,
                    WaterSpecificGravity = 1.06f,
                    WellheadTemperature = 100,
                    BottomholeTemperature = 200,
                    OilSpecificGravity = 0.8250729f,
                    CasingId = 6.135f,
                    TubingId = 2.441f,
                    TubingOD = 2.875f,
                    ReservoirPressure = 1500,
                    BubblepointPressure = 1000,
                    AnalysisType = 1,
                    KillFluidLevel = 2,
                    ReservoirFluidLevel = 3,
                },
                new GLAnalysisResultsEntity()
                {
                    Id = 2,
                    NodeId = "AssetId2",
                    GrossRate = 421.625f,
                    TestDate = new DateTime(2023, 5, 11, 22, 29, 55),
                    ProcessedDate = new DateTime(2023, 5, 11, 22, 29, 55),
                    Success = true,
                    GasInjectionDepth = 10586f,
                    OilRate = 180f,
                    WaterRate = 53f,
                    GasRate = 1720f,
                    WellheadPressure = 150,
                    WaterCut = 53,
                    GasSpecificGravity = 0.86f,
                    WaterSpecificGravity = 1.32f,
                    WellheadTemperature = 100,
                    BottomholeTemperature = 194,
                    OilSpecificGravity = 0.8250729f,
                    CasingId = 6.135f,
                    TubingId = 2.441f,
                    TubingOD = 2.875f,
                    ReservoirPressure = 2100,
                    BubblepointPressure = 2500,
                    CasingPressure = 900,
                    AnalysisType = 1,
                    KillFluidLevel = 2,
                    ReservoirFluidLevel = 3,
                }
            };

            return glAnalysisResults;
        }

        #endregion

    }
}
