using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Data.Sql.Tests
{
    [TestClass]
    public class RodLiftAnalysisTests : DataStoreTestBase
    {

        #region Private Fields

        private Mock<IThetaLoggerFactory> _loggerFactory;
        private Mock<IThetaLogger> _logger;
        private Mock<IDateTimeConverter> _mockDateTimeConverter;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _loggerFactory = new Mock<IThetaLoggerFactory>();
            _logger = new Mock<IThetaLogger>();
            _mockDateTimeConverter = new Mock<IDateTimeConverter>();

            SetupThetaLoggerFactory();
        }

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullContextFactoryTest()
        {
            var mockLocalePhrase = new Mock<ILocalePhrases>();

            _ = new RodLiftAnalysisSQLStore(null, mockLocalePhrase.Object, _loggerFactory.Object, _mockDateTimeConverter.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLocalePhrasesTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();

            _ = new RodLiftAnalysisSQLStore(contextFactory.Object, null, _loggerFactory.Object, _mockDateTimeConverter.Object);
        }

        [TestMethod]
        public void RodLiftAnalysisTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockSqlStore = new Mock<ISQLStoreBase>();
            mockSqlStore.Setup(x => x.GetNodeMasterData(It.IsAny<Guid>())).Returns(GetNodeMasterData().AsQueryable().FirstOrDefault());

            mockSqlStore.Setup(x => x.GetCardData(It.IsAny<Guid>(), It.IsAny<DateTime>())).Returns(GetCardData().AsQueryable().FirstOrDefault());
            mockSqlStore.Setup(x => x.GetWellDetails(It.IsAny<Guid>())).Returns(GetWellDetails().AsQueryable().FirstOrDefault());

            var nodeData = new List<NodeMasterEntity>()
            {
                new NodeMasterEntity()
                {
                    NodeId = "AssetId1",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                    CompanyId = 1
                }
            }.AsQueryable();

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.WellDetails)
                .Returns(SetupMockDbSet(TestUtilities.GetWellDetails().AsQueryable()).Object);
            mockContext.Setup(x => x.CardData)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCardDatas().AsQueryable()).Object);
            mockContext.Setup(x => x.WellTest)
                .Returns(SetupMockDbSet(TestUtilities.GetWellTestData().AsQueryable()).Object);
            mockContext.Setup(x => x.LocalePhrases)
                .Returns(SetupMockDbSet(TestUtilities.GetLocalePhraseData().AsQueryable()).Object);
            mockContext.Setup(x => x.NodeMasters)
                .Returns(SetupMockDbSet(nodeData).Object);
            mockContext.Setup(x => x.CurrentRawScans)
                .Returns(SetupMockDbSet(TestUtilities.GetCurrentRawScanData().AsQueryable()).Object);
            mockContext.Setup(x => x.CustomPumpingUnits)
                .Returns(SetupMockDbSet(TestUtilities.GetCustomPumpingUnitsData().AsQueryable()).Object);
            mockContext.Setup(x => x.PumpingUnits)
                .Returns(SetupMockDbSet(TestUtilities.GetPumpingUnitsData().AsQueryable()).Object);
            mockContext.Setup(x => x.PumpingUnitManufacturer)
                .Returns(SetupMockDbSet(TestUtilities.GetPumpingUnitsManufacturersData().AsQueryable()).Object);
            mockContext.Setup(x => x.XDiagResult)
                .Returns(SetupMockDbSet(TestUtilities.GetXdiagResultsData().AsQueryable()).Object);
            mockContext.Setup(x => x.SystemParameters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetSystemParameter().AsQueryable()).Object);
            mockContext.Setup(x => x.Company)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCompanyData().AsQueryable()).Object);

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

            var rodLiftAnalysis = new RodLiftAnalysisSQLStore(contextFactory.Object, mockLocalePhrase.Object, _loggerFactory.Object, _mockDateTimeConverter.Object);

            var response = rodLiftAnalysis.GetRodLiftAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                "2023-05-11 22:29:55.000", "correlationId");
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void RodLiftAnalysisNullWellDetailsTest()
        {
            var nodeData = new List<NodeMasterEntity>()
            {
                new NodeMasterEntity()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.WellDetails)
                .Returns(SetupMockDbSet(TestUtilities.GetWellDetails().AsQueryable()).Object);
            mockContext.Setup(x => x.NodeMasters).Returns(SetupMockDbSet(nodeData).Object);

            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var rodLiftAnalysis = new RodLiftAnalysisSQLStore(contextFactory.Object, mockLocalePhrase.Object, _loggerFactory.Object, _mockDateTimeConverter.Object);

            var response = rodLiftAnalysis.GetRodLiftAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                "2023-05-11 22:29:55.000", "correlationId");
            Assert.IsNull(response);
        }

        [TestMethod]
        public void GetCardDatesByAssetIdTest()
        {
            var assetId = Guid.Parse("E432CB3B-295C-4ECB-8737-90D5D76AC6CF");
            var correlationId = "CorrelationId1";
            var nodeMasterData = TestUtilities.GetNodeMasterData().AsQueryable();
            var cardData = TestUtilities.GetCardData().AsQueryable();

            var mockNodeMasterDbSet = TestUtilities.SetupNodeMaster(nodeMasterData);
            var mockCardDataDbSet = TestUtilities.SetupCardData(cardData);

            var mockContextFactory = TestUtilities.SetupMockContext();
            mockContextFactory.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContextFactory.Setup(x => x.CardData).Returns(mockCardDataDbSet.Object);

            var mockSqlStore = new Mock<ISQLStoreBase>();

            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContextFactory.Object);

            var mockLocalePhrase = new Mock<ILocalePhrases>();
            var rodLiftAnalysis = new RodLiftAnalysisSQLStore(contextFactory.Object, mockLocalePhrase.Object, _loggerFactory.Object, _mockDateTimeConverter.Object);

            var result = rodLiftAnalysis.GetCardDatesByAssetId(assetId, correlationId);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        #region Private Methods

        private IList<NodeMasterModel> GetNodeMasterData()
        {

            var nodeData = new List<NodeMasterModel>()
            {
                new NodeMasterModel()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                    CompanyId = 1
                }
            };

            return nodeData;
        }

        private IList<WellDetailsModel> GetWellDetails()
        {
            var wellDetails = new List<WellDetailsModel>()
            {
                new WellDetailsModel()
                {
                    NodeId = "AssetId1",
                    PlungerDiameter = 2,
                    PumpDepth = 999,
                    Cycles = 0,
                    IdleTime = 5,
                    PumpingUnitId = "CP1",
                    POCGrossRate = 23926
                },
            };

            return wellDetails;
        }

        private IList<CardDataModel> GetCardData()
        {
            var eventData = new List<CardDataModel>()
            {
                new CardDataModel()
                {
                    NodeId = "AssetId1",
                    CardDate = new DateTime(2023, 5, 11, 22, 29, 55),
                    CardType = "P",
                    Runtime = 24,
                    StrokesPerMinute = 8,
                    StrokeLength = 35,
                    SecondaryPumpFillage = 40,
                    AreaLimit = 70,
                    LoadSpanLimit = 0,
                    CauseId = 99
                }
            };

            return eventData;
        }

        private void SetupThetaLoggerFactory()
        {
            _loggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(_logger.Object);
        }

        #endregion

        #endregion

    }
}
