using Microsoft.Extensions.Caching.Memory;
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

namespace Theta.XSPOC.Apex.Api.Data.Sql.Tests
{
    [TestClass]
    public class DataHistorySQLStoreTests
    {

        private Mock<IThetaLoggerFactory> _loggerFactory;
        private Mock<IThetaLogger> _logger;

        [TestInitialize]
        public void TestInitialize()
        {
            _loggerFactory = new Mock<IThetaLoggerFactory>();
            _logger = new Mock<IThetaLogger>();

            SetupThetaLoggerFactory();
        }

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullContextFactoryTest()
        {
            var mockMemoryCache = new Mock<IMemoryCache>();
            _ = new DataHistorySQLStore(null, mockMemoryCache.Object, _loggerFactory.Object);
        }

        [TestMethod]
        public void GroupTrendDataTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockSqlStore = new Mock<ISQLStoreBase>();

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.GroupParameters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGroupParameters().AsQueryable()).Object);

            mockContext.Setup(x => x.LocalePhrases)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetLocalePhraseData().AsQueryable()).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);
            var correlationId = Guid.NewGuid().ToString();

            var mockMemoryCache = new Mock<IMemoryCache>();

            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object);

            var response = dataHistorySqlStore.GetGroupTrendData(correlationId);
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count);
            Assert.IsInstanceOfType(response, typeof(IList<GroupTrendDataModel>));
        }

        [TestMethod]
        public void DataHistoryValueTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockSqlStore = new Mock<ISQLStoreBase>();

            var mockContext = TestUtilities.SetupMockContext();

            mockContext.Setup(x => x.GroupDataHistory)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGroupDataHistory().AsQueryable()).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);
            var mockMemoryCache = new Mock<IMemoryCache>();
            var correlationId = Guid.NewGuid().ToString();

            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object);

            DateTime startDate = Convert.ToDateTime("2023-08-23 09:28:09.000");
            DateTime endDate = Convert.ToDateTime("2023-08-25 09:28:09.000");

            var response = dataHistorySqlStore.GetGroupTrendData(startDate, endDate, 1, "Fluid Load", correlationId);

            Assert.IsNotNull(response);
            Assert.AreEqual(3, response.Count);
        }

        [TestMethod]
        public void GetMeasurementTrendDataTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.Parameters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetParameter().AsQueryable()).Object);
            mockContext.Setup(x => x.FacilityTags)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetFacilityTag().AsQueryable()).Object);
            mockContext.Setup(x => x.DataHistory)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetDataHistory().AsQueryable()).Object);
            mockContext.Setup(x => x.DataHistoryArchive)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetDataHistoryArchive().AsQueryable()).Object);
            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetNodeMasterData().AsQueryable()).Object);
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var mockMemoryCache = new Mock<IMemoryCache>();

            IList<MeasurementTrendDataModel> result = new List<MeasurementTrendDataModel>();
            result.Add(new MeasurementTrendDataModel()
            {
                Address = 2002,
                Date = new DateTime(2023, 11, 01, 01, 00, 00),
                IsManual = false,
                Value = 1,
            });
            result.Add(new MeasurementTrendDataModel()
            {
                Address = 2005,
                Date = new DateTime(2023, 11, 01, 02, 00, 00),
                IsManual = false,
                Value = 1,
            });
            result.Add(new MeasurementTrendDataModel()
            {
                Address = 32569,
                Date = new DateTime(2023, 11, 01, 01, 00, 00),
                IsManual = false,
                Value = 1,
            });
            result.Add(new MeasurementTrendDataModel()
            {
                Address = 32569,
                Date = new DateTime(2023, 11, 01, 01, 15, 00),
                IsManual = false,
                Value = 1,
            });
            result.Add(new MeasurementTrendDataModel()
            {
                Address = 32569,
                Date = new DateTime(2023, 11, 01, 01, 20, 00),
                IsManual = false,
                Value = 1,
            });
            result.Add(new MeasurementTrendDataModel()
            {
                Address = 32569,
                Date = new DateTime(2023, 11, 01, 01, 25, 00),
                IsManual = false,
                Value = 1,
            });
            var correlationId = Guid.NewGuid().ToString();

            var mockSqlStore = new Mock<ISQLStoreBase>();
            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object);

            var response = dataHistorySqlStore.GetMeasurementTrendData("AssetId1",
                268, new DateTime(2023, 01, 01), DateTime.Now, correlationId);

            Assert.IsNotNull(response);
            Assert.AreEqual(6, response.Count);
            for (int i = 0; i < response.Count; i++)
            {
                Assert.AreEqual(result[i].Value, response[i].Value);
                Assert.AreEqual(result[i].Address, response[i].Address);
                Assert.AreEqual(result[i].IsManual, response[i].IsManual);
            }
            Assert.IsInstanceOfType(response, typeof(IList<MeasurementTrendDataModel>));
        }

        [TestMethod]
        public void GetControllerTrendItemsTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockContext = TestUtilities.SetupMockContext();
            var mockSqlStore = new Mock<ISQLStoreBase>();
            var correlationId = Guid.NewGuid().ToString();

            mockContext.Setup(x => x.Parameters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetParameters().AsQueryable()).Object);

            mockContext.Setup(x => x.FacilityTags)
               .Returns(TestUtilities.SetupMockData(TestUtilities.GetFacilityTags().AsQueryable()).Object);

            mockContext.Setup(x => x.LocalePhrases)
               .Returns(TestUtilities.SetupMockData(TestUtilities.GetLocalePhraseData().AsQueryable()).Object);

            mockContext.Setup(x => x.DataHistory)
               .Returns(TestUtilities.SetupMockData(TestUtilities.GetDataHistories().AsQueryable()).Object);

            mockContext.Setup(x => x.DataHistoryArchive)
               .Returns(TestUtilities.SetupMockData(TestUtilities.GetDataHistoryArchives().AsQueryable()).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);
            var mockMemoryCache = new Mock<IMemoryCache>();

            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object);

            var nodeId = "AssetId1";
            var pocType = 8;

            var response = dataHistorySqlStore.GetControllerTrendItems(nodeId, pocType, correlationId);

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count);
            Assert.IsInstanceOfType(response, typeof(IList<ControllerTrendItemModel>));
        }

        [TestMethod]
        public void EventTrendDataTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockSqlStore = new Mock<ISQLStoreBase>();

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.Events)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetEventsData().AsQueryable()).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);
            var mockMemoryCache = new Mock<IMemoryCache>();
            var correlationId = Guid.NewGuid().ToString();

            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object);

            var response = dataHistorySqlStore.GetEventTrendData("well1", 7, new DateTime(2023, 11, 8, 12, 14, 14, 14),
                new DateTime(2023, 11, 10, 12, 14, 14, 14), correlationId);
            Assert.IsNotNull(response);
            Assert.AreEqual(2, response.Count);
            Assert.IsInstanceOfType(response, typeof(IList<EventTrendDataModel>));
        }

        [TestMethod]
        public void AnalysisTrendDataTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockSqlStore = new Mock<ISQLStoreBase>();
            var correlationId = Guid.NewGuid().ToString();

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.XDiagResult)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetAnalysisTrendData().AsQueryable()).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);
            var mockMemoryCache = new Mock<IMemoryCache>();

            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object);
            DateTime startDate = Convert.ToDateTime("2023-08-23 09:28:09.000");
            DateTime endDate = Convert.ToDateTime("2023-08-25 09:28:09.000");
            DateTime date = Convert.ToDateTime("2023-08-24 09:28:09.000");
            string name = "AdditionalUplift";
            var response = dataHistorySqlStore.GetAnalysisTrendData("1", startDate,
                            endDate, name, correlationId);
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count);
            Assert.AreEqual(date, response[0].Date);
            Assert.IsInstanceOfType(response, typeof(IList<AnalysisTrendDataModel>));
        }

        [TestMethod]
        public void GetMeterHistoryTrendTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockContext = TestUtilities.SetupMockContext();
            var mockSqlStore = new Mock<ISQLStoreBase>();

            mockContext.Setup(x => x.MeterHistory)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetMeterHistory().AsQueryable()).Object);
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);
            var mockMemoryCache = new Mock<IMemoryCache>();
            var correlationId = Guid.NewGuid().ToString();

            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object);

            DateTime startDate = Convert.ToDateTime("2023-08-23 09:28:09.000");
            DateTime endDate = Convert.ToDateTime("2023-08-25 09:28:09.000");
            DateTime date = Convert.ToDateTime("2023-08-24 09:28:09.000");
            string name = "CasingPressure";

            var response = dataHistorySqlStore.GetMeterHistoryTrendData("1", startDate,
                endDate, name, correlationId);

            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count);
            Assert.AreEqual(date, response[0].Date);
            Assert.IsInstanceOfType(response, typeof(IList<MeterTrendDataModel>));
        }

        [TestMethod]
        public void WellTestTrendDataTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockSqlStore = new Mock<ISQLStoreBase>();

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.WellTest)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetWellTestData().AsQueryable()).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);
            var mockMemoryCache = new Mock<IMemoryCache>();
            var correlationId = Guid.NewGuid().ToString();

            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object);

            var response = dataHistorySqlStore.GetWellTestTrendData("AssetId1", "GasRate", new DateTime(2020, 5, 11, 22, 29, 55),
                new DateTime(2025, 5, 17, 22, 29, 55), true, correlationId);
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count);
            Assert.IsInstanceOfType(response, typeof(IList<WellTestTrendDataModel>));
        }

        [TestMethod]
        public void WellTestTrendDataNegativeTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockSqlStore = new Mock<ISQLStoreBase>();

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.WellTest)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetWellTestData().AsQueryable()).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);
            var mockMemoryCache = new Mock<IMemoryCache>();
            var correlationId = Guid.NewGuid().ToString();

            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object);

            var response = dataHistorySqlStore.GetWellTestTrendData("AssetId1",
                "Test", new DateTime(2020, 5, 11, 22, 29, 55),
                new DateTime(2025, 5, 11, 22, 29, 55), false, correlationId);
            Assert.IsNotNull(response);
            Assert.AreEqual(0, response.Count);
            Assert.IsInstanceOfType(response, typeof(IList<WellTestTrendDataModel>));
        }

        [TestMethod]
        public void GetOperationalScoreItemsPassTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();

            var mockContext = TestUtilities.SetupMockContext();

            mockContext.Setup(x => x.OperationalScore)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetOperationalScoreEntity().AsQueryable()).Object);
            var correlationId = Guid.NewGuid().ToString();

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);
            var mockMemoryCache = new Mock<IMemoryCache>();

            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object);

            DateTime startDate = Convert.ToDateTime("2017-11-15");
            DateTime endDate = Convert.ToDateTime("2017-11-18");

            var response = dataHistorySqlStore.GetOperationalScoreTrendData("OperationalScore", "KIMBERLY 807MS",
                startDate, endDate, correlationId);

            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(IList<OperationalScoreDataModel>));
            Assert.AreEqual(1, response.Count);
            Assert.AreEqual(response[0].Date, Convert.ToDateTime("2017-11-16"));
            Assert.AreEqual(response[0].Value, 45);
        }

        [TestMethod]
        public void GetOperationalScoreItemsNullValueTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();

            var mockContext = TestUtilities.SetupMockContext();
            var data = TestUtilities.GetOperationalScoreEntity();
            data[0].OperationalScore = null;

            mockContext.Setup(x => x.OperationalScore)
                .Returns(TestUtilities.SetupMockData(data.AsQueryable()).Object);
            var correlationId = Guid.NewGuid().ToString();

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);
            var mockMemoryCache = new Mock<IMemoryCache>();

            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object);

            DateTime startDate = Convert.ToDateTime("2017-11-15");
            DateTime endDate = Convert.ToDateTime("2017-11-18");

            var response = dataHistorySqlStore.GetOperationalScoreTrendData("OperationalScore", "KIMBERLY 807MS",
                startDate, endDate, correlationId);

            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(IList<OperationalScoreDataModel>));
            Assert.AreEqual(1, response.Count);
            Assert.AreEqual(response[0].Date, Convert.ToDateTime("2017-11-16"));
            Assert.AreEqual(response[0].Value, null);
        }

        [TestMethod]
        public void GetOperationalScoreItemsFailTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();

            var mockContext = TestUtilities.SetupMockContext();

            mockContext.Setup(x => x.OperationalScore)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetOperationalScoreEntity().AsQueryable()).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);
            var mockMemoryCache = new Mock<IMemoryCache>();
            var correlationId = Guid.NewGuid().ToString();

            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object);

            DateTime startDate = Convert.ToDateTime("2017-11-15");
            DateTime endDate = Convert.ToDateTime("2017-11-18");

            var response = dataHistorySqlStore.GetOperationalScoreTrendData("NameTest", "KIMBERLY",
                startDate, endDate, correlationId);

            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(IList<OperationalScoreDataModel>));
            Assert.AreEqual(0, response.Count);
        }

        [TestMethod]
        public void ProductionStatisticsTrendDataTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.ProductionStatistics)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetProductionStatisticsData().AsQueryable()).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);
            var mockMemoryCache = new Mock<IMemoryCache>();
            var correlationId = Guid.NewGuid().ToString();

            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object);
            DateTime startDate = Convert.ToDateTime("2023-08-23 09:28:09.000");
            DateTime endDate = Convert.ToDateTime("2023-08-25 09:28:09.000");

            var response = dataHistorySqlStore.GetProductionStatisticsTrendData("1", startDate,
                            endDate, "LatestProduction", correlationId);
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count);
            Assert.IsInstanceOfType(response, typeof(IList<ProductionStatisticsTrendDataModel>));
        }

        [TestMethod]
        public void GetRodStressTrendDataTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.XDiagRodResult)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetXDiagRodResultData().AsQueryable()).Object);
            var correlationId = Guid.NewGuid().ToString();

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);
            var mockMemoryCache = new Mock<IMemoryCache>();

            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object);

            var response = dataHistorySqlStore.GetRodStressTrendData("TopMaxStress", "1", 111, "D", 1,
                new DateTime(2023, 11, 21, 12, 14, 14, 14),
                new DateTime(2023, 11, 24, 12, 14, 14, 14), correlationId);
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count);
            Assert.IsInstanceOfType(response, typeof(IList<RodStressTrendDataModel>));
        }

        [TestMethod]
        public void PlungerLiftTrendDataTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.PlungerLiftDataHistory)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetPlungerLiftTrendData().AsQueryable()).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);
            var mockMemoryCache = new Mock<IMemoryCache>();
            var correlationId = Guid.NewGuid().ToString();

            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object);

            DateTime startDate = Convert.ToDateTime("2023-08-23 09:28:09.000");
            DateTime endDate = Convert.ToDateTime("2023-08-25 09:28:09.000");
            DateTime date = Convert.ToDateTime("2023-08-24 09:28:09.000");
            string name = "CasingPressure";

            var response = dataHistorySqlStore.GetPlungerLiftTrendData("1", startDate,
                            endDate, name, correlationId);
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Count);
            Assert.AreEqual(date, response[0].Date);
            Assert.IsInstanceOfType(response, typeof(IList<PlungerLiftTrendDataModel>));
        }

        #endregion

        private void SetupThetaLoggerFactory()
        {
            _loggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(_logger.Object);
        }

    }
}
