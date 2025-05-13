using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Customers;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Mongo.Models.Lookup;
using MongoParameters = Theta.XSPOC.Apex.Kernel.Mongo.Models.Parameter.Parameters;
using MongoAsset= Theta.XSPOC.Apex.Kernel.Mongo.Models.Asset.Asset;
using Theta.XSPOC.Apex.Api.Data.Influx.Services;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Kernel.Mongo.Models.Parameter;
using Theta.XSPOC.Apex.Api.Data.Mongo;
using MongoDB.Driver;
using System.Reflection;
using System.IO;
using System.Threading;
namespace Theta.XSPOC.Apex.Api.Data.Sql.Tests
{
    [TestClass]
    public class DataHistorySQLStoreTests
    {

        private Mock<IThetaLoggerFactory> _loggerFactory;
        private Mock<IThetaLogger> _logger;
        private Mock<IDataHistoryMongoStore> _mockDataHistoryMongoStore;
        private Mock<IConfiguration> _mockConfiguration;
        private Mock<IGetDataHistoryItemsService> _dataHistoryInfluxStore;
        private Mock<IDataHistoryMongoStore> _dataHistoryMongoStore;
        private IConfiguration config;
        [TestInitialize]
        public void TestInitialize()
        {
            _loggerFactory = new Mock<IThetaLoggerFactory>();
            _logger = new Mock<IThetaLogger>();
            _mockDataHistoryMongoStore = new Mock<IDataHistoryMongoStore>();
            _dataHistoryInfluxStore = new Mock<IGetDataHistoryItemsService>();
            _dataHistoryMongoStore = new Mock<IDataHistoryMongoStore>();
            _mockConfiguration = new Mock<IConfiguration>();
            config = SetConfiguration(true);
            SetupThetaLoggerFactory();
            // Setup mongo data.
            SetupMongoMockData();

            // Setup influx data.
            SetupInfluxMockData();

        }

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullContextFactoryTest()
        {
            var mockMemoryCache = new Mock<IMemoryCache>();
            _ = new DataHistorySQLStore(null, mockMemoryCache.Object, _loggerFactory.Object, _mockDataHistoryMongoStore.Object,_mockConfiguration.Object);
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
            
            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object, _mockDataHistoryMongoStore.Object, config);

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
            
            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object, _mockDataHistoryMongoStore.Object, config);

            DateTime startDate = Convert.ToDateTime("2023-08-23 09:28:09.000");
            DateTime endDate = Convert.ToDateTime("2023-08-25 09:28:09.000");

            var response = dataHistorySqlStore.GetGroupTrendData(startDate, endDate, 1, "Fluid Load", correlationId);

            Assert.IsNotNull(response);
            Assert.AreEqual(3, response.Count);
        }

        [TestMethod]
        public void GetMeasurementTrendDataTest()
        {
           config = SetConfiguration(false);
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
            
            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object, _mockDataHistoryMongoStore.Object, config);

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
           
            mockContext.Setup(x => x.NodeMasters)
              .Returns(TestUtilities.SetupMockData(TestUtilities.GetNodeMasterData().AsQueryable()).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);
            var mockMemoryCache = new Mock<IMemoryCache>();

            
            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object, _mockDataHistoryMongoStore.Object, config);

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

            
            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object, _mockDataHistoryMongoStore.Object, config);

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

            
            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object, _mockDataHistoryMongoStore.Object, config);
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

            
            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object, _mockDataHistoryMongoStore.Object, config);

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

            
            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object, _mockDataHistoryMongoStore.Object, config);

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

            
            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object, _mockDataHistoryMongoStore.Object, config);

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

            
            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object, _mockDataHistoryMongoStore.Object, config);

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

            
            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object, _mockDataHistoryMongoStore.Object, config);

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

            
            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object, _mockDataHistoryMongoStore.Object, config);

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

            
            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object, _mockDataHistoryMongoStore.Object, config);
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

            
            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object, _mockDataHistoryMongoStore.Object, config);

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

            
            var dataHistorySqlStore = new DataHistorySQLStore(contextFactory.Object, mockMemoryCache.Object, _loggerFactory.Object, _mockDataHistoryMongoStore.Object, config);

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

        /// <summary>
        /// Sets the configuration for influx flag
        /// </summary>
        /// <param name="enableInflux"></param>
        /// <returns></returns>
        private IConfiguration SetConfiguration(bool enableInflux)
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Or your test project's base path
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddInMemoryCollection(
                new Dictionary<string, string>
                    {
                         { "EnableInflux", enableInflux.ToString() }
                    }
                ).Build();
        }

            private void SetupMockCollection<T>(Mock<IMongoCollection<T>> mockCollection, IEnumerable<T> data) where T : class
        {
            var asyncCursor = new Mock<IAsyncCursor<T>>();

            asyncCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<System.Threading.CancellationToken>()))
                .Returns(true)
                .Returns(false);

            mockCollection
                .Setup(c => c.FindSync(
                    It.IsAny<FilterDefinition<T>>(),
                    It.IsAny<FindOptions<T, T>>(),
                    It.IsAny<System.Threading.CancellationToken>()))
                .Returns((FilterDefinition<T> filter, FindOptions<T, T> options, System.Threading.CancellationToken token) =>
                {
                    var serializer = MongoDB.Bson.Serialization.BsonSerializer.SerializerRegistry.GetSerializer<T>();
                    var bsonFilter = filter.Render(serializer, MongoDB.Bson.Serialization.BsonSerializer.SerializerRegistry);

                    var filteredData = data.Where(item => MatchesFilter(item, bsonFilter)).ToList();

                    asyncCursor
                        .Setup(_ => _.Current)
                        .Returns(filteredData);

                    return asyncCursor.Object;
                });
        }

        /// <summary>
        /// Matches the Bson filter conditions with the object properties.
        /// </summary>
        private bool MatchesFilter<T>(T item, BsonDocument bsonFilter)
        {
            foreach (var element in bsonFilter.Elements)
            {
                string propertyName = element.Name;
                BsonValue filterValue = element.Value;

                if (propertyName == "$and" && filterValue.IsBsonArray)
                {
                    // Ensure all conditions inside $and are true
                    var conditions = filterValue.AsBsonArray;
                    foreach (var condition in conditions)
                    {
                        if (!MatchesFilter(item, condition.AsBsonDocument))
                        { 
                            return false;
                        }
                    }
                }
                else if (propertyName == "$or" && filterValue.IsBsonArray)
                {
                    // Ensure at least one condition inside $or is true
                    var conditions = filterValue.AsBsonArray;
                    bool anyMatch = conditions.Any(condition => MatchesFilter(item, condition.AsBsonDocument));
                    if (!anyMatch) 
                    { 
                        return false;
                    }
                }
                else
                {
                    object propertyValue = GetNestedPropertyValue(item, propertyName);
                    if (propertyName == "_id")
                    {
                        propertyValue = element.Value;
                    }
                    if (propertyValue == null) 
                    {
                        return false;
                    }

                    // Handle different filter operators (Eq, Gte, Lte, Ne, etc.)
                    if (filterValue.IsBsonDocument)
                    {
                        var filterDoc = filterValue.AsBsonDocument;
                        foreach (var condition in filterDoc.Elements)
                        {
                            switch (condition.Name)
                            {
                                case "$eq":
                                    if (!propertyValue.Equals(BsonTypeMapper.MapToDotNetValue(condition.Value)))
                                    {
                                        return false;
                                    };
                                    break;
                                default:
                                    throw new NotImplementedException($"Operator {condition.Name} not implemented");
                            }
                        }
                    }
                    else
                    {
                        if (propertyName == "_id")
                        {
                            if (propertyValue.ToString() == filterValue.ToString())
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (!propertyValue.Equals(BsonTypeMapper.MapToDotNetValue(filterValue)))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Recursively fetches nested properties from an object.
        /// Example: "LookupDocument.TubingOD" -> item.LookupDocument.TubingOD
        /// </summary>
        private object GetNestedPropertyValue(object obj, string propertyName)
        {
            string[] properties = propertyName.Split('.');
            object value = obj;

            foreach (string prop in properties)
            {
                if (value == null)
                {
                    return null;
                }

                PropertyInfo propInfo = value.GetType().GetProperty(prop);
                if (prop == "LegacyId")
                {
                    var nestedProp = propertyName.Split(".")[1];
                    var legacyValue = GetLegacyIdValue(obj, nestedProp);
                    return legacyValue ?? null;

                }
                value = propInfo?.GetValue(value);
            }

            return value;
        }

        /// <summary>
        /// to get LegacyId Value to check the condition to filter the data
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="key">The key to find inside the LegacyId </param>
        /// <returns></returns>
        private object GetLegacyIdValue(object obj, string key)
        {
            PropertyInfo prop = obj.GetType().GetProperty("LegacyId");
            if (prop == null) 
            {
                return null;
            }

            object dictionaryObject = prop.GetValue(obj);
            if (dictionaryObject is Dictionary<string, string> dictionary && dictionary.TryGetValue(key, out var value))
            {
                return value;
            }
            return null;
        }

        private IEnumerable<Customer> GetCustomerData()
        {
            return new List<Customer>()
            {
                new()
                {
                    Id = "66d74f86c66365161d7ca943",
                    Name = "Customer 1",
                    LegacyId =  new Dictionary<string, string>()
                    {
                        { "CustomerGUID", "a5fa13b2-56e8-4aaa-a2d5-61375609de9e" },
                        { "CustomerId", "1"}
                    }
                },
            };
        }

        private IDictionary<(int Address, string ChannelId), Parameters> GetParametersBulkMock()
        {
            var mockData = new List<Parameters>()
            {
                new()
                {
                    Name = "SPM",
                    Address = 2002,
                    Description = "SPM",
                    ChannelId = "C1",
                    POCType = new Lookup()
                    {
                        LookupType = LookupTypes.POCTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "POCTypesId", "8" }
                        },
                        LookupDocument = new POCTypes()
                        {
                            POCType = 8
                        }
                    },
                    DataType = new Lookup()
                    {
                        LookupType = LookupTypes.DataTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "DataTypeId", "1" }
                        },
                        LookupDocument = new DataTypes()
                        {
                            Comment = "Comment",
                            DataTypeId = 1,
                            Description = "Description",
                        }
                    },
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "POCType", "8" },
                        { "Address", "2002" },
                        { "Bit", "0" }
                    },
                    UnitType = new Lookup()
                    {
                        LookupType = LookupTypes.UnitTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "UnitTypesId", "0" }
                        },
                        LookupDocument = new UnitTypes()
                        {
                            UnitTypesId = 0,
                            Description = "None",
                            PhraseId = 731
                        }
                    },
                    ParameterDocument = new FacilityTagDetails()
                    {
                        FacilityTagGroupID = 1
                    }
                },
                new()
                {
                    Name = "SPM2",
                    Address = 2005,
                    Description = "SPM",
                    ChannelId = "C2",
                    POCType = new Lookup()
                    {
                        LookupType = LookupTypes.POCTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "POCTypesId", "8" }
                        },
                        LookupDocument = new POCTypes()
                        {
                            POCType = 8
                        }
                    },
                    DataType = new Lookup()
                    {
                        LookupType = LookupTypes.DataTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "DataTypeId", "1" }
                        },
                        LookupDocument = new DataTypes()
                        {
                            Comment = "Comment",
                            DataTypeId = 1,
                            Description = "Description",
                        }
                    },
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "POCType", "8" },
                        { "Address", "2005" },
                        { "Bit", "0" }
                    },
                    UnitType = new Lookup()
                    {
                        LookupType = LookupTypes.UnitTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "UnitTypesId", "0" }
                        },
                        LookupDocument = new UnitTypes()
                        {
                            UnitTypesId = 0,
                            Description = "None",
                            PhraseId = 731
                        }
                    },
                    ParameterDocument = new FacilityTagDetails()
                    {
                        FacilityTagGroupID = 1
                    }
                },
                new()
                {
                    Name = "SPM3",
                    Address = 1,
                    Description = "SPM",
                    ChannelId = "C3",
                    POCType = new Lookup()
                    {
                        LookupType = LookupTypes.POCTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "POCTypesId", "8" }
                        },
                        LookupDocument = new POCTypes()
                        {
                            POCType = 8
                        }
                    },
                    DataType = new Lookup()
                    {
                        LookupType = LookupTypes.DataTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "DataTypeId", "1" }
                        },
                        LookupDocument = new DataTypes()
                        {
                            Comment = "Comment",
                            DataTypeId = 1,
                            Description = "Description",
                        }
                    },
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "POCType", "8" },
                        { "Address", "1" },
                        { "Bit", "0" }
                    },
                    UnitType = new Lookup()
                    {
                        LookupType = LookupTypes.UnitTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "UnitTypesId", "0" }
                        },
                        LookupDocument = new UnitTypes()
                        {
                            UnitTypesId = 0,
                            Description = "None",
                            PhraseId = 731
                        }
                    },
                    ParameterDocument = new FacilityTagDetails()
                    {
                        FacilityTagGroupID = 1
                    }
                },
                new()
                {
                    Name = "SPM4",
                    Address = 1,
                    Description = "SPM",
                    ChannelId = "C4",
                    POCType = new Lookup()
                    {
                        LookupType = LookupTypes.POCTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "POCTypesId", "8" }
                        },
                        LookupDocument = new POCTypes()
                        {
                            POCType = 8
                        }
                    },
                    DataType = new Lookup()
                    {
                        LookupType = LookupTypes.DataTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "DataTypeId", "1" }
                        },
                        LookupDocument = new DataTypes()
                        {
                            Comment = "Comment",
                            DataTypeId = 1,
                            Description = "Description",
                        }
                    },
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "POCType", "8" },
                        { "Address", "1" },
                        { "Bit", "0" }
                    },
                    UnitType = new Lookup()
                    {
                        LookupType = LookupTypes.UnitTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "UnitTypesId", "0" }
                        },
                        LookupDocument = new UnitTypes()
                        {
                            UnitTypesId = 0,
                            Description = "None",
                            PhraseId = 731
                        }
                    },
                    ParameterDocument = new FacilityTagDetails()
                    {
                        FacilityTagGroupID = 1
                    }
                }
            };

            // Map results to a dictionary
            var parameterDict = mockData //filteredData
                .GroupBy(param => (param.Address, param.ChannelId))
                .ToDictionary(
                    group => group.Key,
                    group => group.First()
                );

            return parameterDict;
        }
        private List<MongoParameters> SetupParameterMock()
        {
            return new List<MongoParameters> {

                new MongoParameters
                {
                    ChannelId = "C1006",
                    Address = 0,
                    LegacyId = new Dictionary<string, string>
                    {
                        { "POCType", "8" }
                    }
                },
                new MongoParameters
                {
                    ChannelId = "C290",
                    Address = 32569,
                    LegacyId = new Dictionary<string, string>
                    {
                        { "POCType", "8" }
                    }
                }
            };
        }
        
        /// <summary>
        /// set up mongo data.
        /// </summary>
        private void SetupMongoMockData()
        {
            _mockDataHistoryMongoStore
             .Setup(x => x.GetControllerTrendItems(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
             .ReturnsAsync(SetupGetDataHistoryTrendDataMock());

            _mockDataHistoryMongoStore
             .Setup(x => x.GetMeasurementTrendData(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>()))
             .ReturnsAsync(GetDataHistoryItemsMock());

            _mockDataHistoryMongoStore
             .Setup(x => x.GetParametersBulk(It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
             .Returns(Task.FromResult(GetParametersBulkMock())); //(Guid id, string correlationId) => 
            _mockDataHistoryMongoStore
            .Setup(x => x.GetParametersBulk(It.IsAny<List<string>>(), It.IsAny<int>(), It.IsAny<string>()))
            .Returns(Task.FromResult(GetParametersBulkMock())); //(Guid id, string correlationId) => 
        }

        /// <summary>
        /// set up influx current.
        /// </summary>
        private void SetupInfluxMockData()
        {

            _dataHistoryInfluxStore.Setup(x => x.GetDataHistoryItems(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(GetDataHistoryItemsMock());
            _dataHistoryInfluxStore
                .Setup(x => x.GetDowntimeAsync(It.IsAny<IList<DowntimeFiltersInflux>>(),It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(SetupInfluxDataStoreDowntimeMock());
            _dataHistoryInfluxStore
                .Setup(x => x.GetDataHistoryTrendData(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(SetupGetDataHistoryTrendDataMock());
        }

        /// <summary>
        /// set up mongo data.
        /// </summary>
        private List<DataPointModel> GetDataHistoryItemsMock()
        {
            // Sample mock data to simulate the response from GetTrendData
            var mockData = new List<DataPointModel>
            {
                new DataPointModel
                {
                    Time = new DateTime(2023, 01, 01),
                    Value = 50.5,
                    TrendName = "C1"
                },
                new DataPointModel
                {
                    Time = new DateTime(2024, 01, 01),
                    Value = 60.2,
                    TrendName = "C2"
                },
                new DataPointModel
                {
                    Time = DateTime.UtcNow.AddMinutes(-10),
                    Value = 70.8,
                    TrendName = "C3"
                }
            };
               return mockData;
        }

        private List<DataPointModel> SetupGetDataHistoryTrendDataMock()
        {
            // Sample mock data to simulate the response from GetDataHistoryTrendData
            var mockTrendData = new List<DataPointModel>
            {
                new DataPointModel
                {
                    Time = DateTime.UtcNow.AddMinutes(-30),
                    Value = 45.3,
                    TrendName = "C1"
                },
                new DataPointModel
                {
                    Time = DateTime.UtcNow.AddMinutes(-20),
                    Value = 50.7,
                    TrendName = "C2"
                },
                new DataPointModel
                {
                    Time = DateTime.UtcNow.AddMinutes(-10),
                    Value = 55.1,
                    TrendName = "C3"
                }
            };
            return mockTrendData;
        }

        private List<DowntimeByWellsInfluxModel> SetupInfluxDataStoreDowntimeMock()
        {
            // Sample mock data to simulate the response from GetDowntimeAsync
            var mockDowntimeData = new List<DowntimeByWellsInfluxModel>
                {
                    new DowntimeByWellsInfluxModel
                    {
                        Date = DateTime.UtcNow.AddMinutes(-30),
                        Value = 5.5f,
                        Id = "Asset1",
                        ParamStandardType = "StandardType1"
                    },
                    new DowntimeByWellsInfluxModel
                    {
                        Date = DateTime.UtcNow.AddMinutes(-20),
                        Value = 3.2f,
                        Id = "Asset2",
                        ParamStandardType = "StandardType2"
                    },
                    new DowntimeByWellsInfluxModel
                    {
                        Date = DateTime.UtcNow.AddMinutes(-10),
                        Value = 7.8f,
                        Id = "Asset3",
                        ParamStandardType = "StandardType3"
                    }
                };

            return mockDowntimeData;
        }

    }
}
