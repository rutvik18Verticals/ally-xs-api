using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Mongo;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Api.Data.Influx.Services;
using System.Threading;
using Microsoft.Extensions.Configuration;

namespace Theta.XSPOC.Apex.Api.Core.Tests.Services
{
    [TestClass]
    public class RodLiftAnalysisMongoStoreTests
    {
        private Mock<IMongoDatabase> _mockDatabase;
        private Mock<IThetaLoggerFactory> _mockLoggerFactory;
        private Mock<IGetDataHistoryItemsService> _mockDataHistoryInfluxStore;
        private Mock<IDateTimeConverter> _mockDateTimeConverter;
        private RodLiftAnalysisMongoStore _store;

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullConfigurationTest()
        {
            var mockDatabase = new Mock<IMongoDatabase>();
            var mockLoggerFactory = new Mock<IThetaLoggerFactory>();
            var mockDataHistoryInfluxStore = new Mock<IGetDataHistoryItemsService>();
            var mockDateTimeConverter = new Mock<IDateTimeConverter>();

            _ = new RodLiftAnalysisMongoStore(null, mockDatabase.Object, mockLoggerFactory.Object,
                mockDataHistoryInfluxStore.Object, mockDateTimeConverter.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullMongoDbTest()
        {
            var mockConfigurationSection = new Mock<IConfigurationSection>();
            var mockLoggerFactory = new Mock<IThetaLoggerFactory>();
            var mockDataHistoryInfluxStore = new Mock<IGetDataHistoryItemsService>();
            var mockDateTimeConverter = new Mock<IDateTimeConverter>();

            _ = new RodLiftAnalysisMongoStore(mockConfigurationSection.Object, null, mockLoggerFactory.Object,
                mockDataHistoryInfluxStore.Object, mockDateTimeConverter.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLoggerTest()
        {
            var mockConfigurationSection = new Mock<IConfigurationSection>();
            var mockDatabase = new Mock<IMongoDatabase>();
            var mockDataHistoryInfluxStore = new Mock<IGetDataHistoryItemsService>();
            var mockDateTimeConverter = new Mock<IDateTimeConverter>();

            _ = new RodLiftAnalysisMongoStore(mockConfigurationSection.Object, mockDatabase.Object, null,
                mockDataHistoryInfluxStore.Object,mockDateTimeConverter.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullDataHistoryItemsServiceTest()
        {
            var mockConfigurationSection = new Mock<IConfigurationSection>();
            var mockDatabase = new Mock<IMongoDatabase>();
            var mockLoggerFactory = new Mock<IThetaLoggerFactory>();
            var mockDateTimeConverter = new Mock<IDateTimeConverter>();

            _ = new RodLiftAnalysisMongoStore(mockConfigurationSection.Object, mockDatabase.Object, mockLoggerFactory.Object,
                null, mockDateTimeConverter.Object);
        }

        [TestInitialize]
        public void Setup()
        {
            _mockDatabase = new Mock<IMongoDatabase>();
            _mockLoggerFactory = new Mock<IThetaLoggerFactory>();
            _mockDataHistoryInfluxStore = new Mock<IGetDataHistoryItemsService>();
            _mockDateTimeConverter = new Mock<IDateTimeConverter>();

            var logger = new Mock<IThetaLogger>();
            _mockLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockConfigurationSection = new Mock<IConfigurationSection>();
            mockConfigurationSection.Setup(x => x.Value).Returns("UTC");
            var mockConfiguration = new Mock<IConfiguration>();

            var mockUseAppTimeZoneSection = new Mock<IConfigurationSection>();
            mockUseAppTimeZoneSection.Setup(x => x.Value).Returns("true");
            var mockAppTimeZoneSection = new Mock<IConfigurationSection>();
            mockAppTimeZoneSection.Setup(x => x.Value).Returns("Pacific Standard Time");
            mockConfiguration.Setup(x => x.GetSection("TimeZoneBehavior:UseApplicationTimeZone")).Returns(mockUseAppTimeZoneSection.Object);
            mockConfiguration.Setup(x => x.GetSection("TimeZoneBehavior:ApplicationTimeZone")).Returns(mockAppTimeZoneSection.Object);

            _store = new RodLiftAnalysisMongoStore(
                mockConfiguration.Object,
                _mockDatabase.Object,
                _mockLoggerFactory.Object,
                _mockDataHistoryInfluxStore.Object,
                _mockDateTimeConverter.Object
            );
        }

        [TestMethod]
        public void GetRodLiftAnalysisData_ReturnsNull_WhenNoAssetMatch()
        {
            // Arrange
            var assetId = Guid.NewGuid(); // Using a random GUID that won't match any mocked asset.
            var cardDateString = "2024-05-20 12:00:00";
            var correlationId = "test-correlation";

            // Mock Asset Collection to return no results
            var mockAssetCollection = new Mock<IMongoCollection<Asset>>();
            _mockDatabase.Setup(db => db.GetCollection<Asset>("Asset", null)).Returns(mockAssetCollection.Object);

            var emptyAssetCursor = new Mock<IAsyncCursor<Asset>>();
            emptyAssetCursor.Setup(_ => _.MoveNext(It.IsAny<CancellationToken>())).Returns(false);
            emptyAssetCursor.Setup(_ => _.Current).Returns(new List<Asset>());

            mockAssetCollection.Setup(x => x.FindSync(It.IsAny<FilterDefinition<Asset>>(),
                It.IsAny<FindOptions<Asset>>(), It.IsAny<CancellationToken>())).Returns(emptyAssetCursor.Object);

            // Act
            var result = _store.GetRodLiftAnalysisData(assetId, cardDateString, correlationId);

            // Assert
            Assert.IsNull(result, "Expected result to be null when no asset matches the provided assetId.");
        }

        [TestMethod]
        public void GetCardDatesByAssetId_ReturnsEmptyList_WhenNoAsset()
        {
            // Arrange
            var assetId = Guid.NewGuid();
            var correlationId = "test-correlation";

            // Mock Asset Retrieval to return no results
            var mockAssetCollection = new Mock<IMongoCollection<Asset>>();
            _mockDatabase.Setup(db => db.GetCollection<Asset>("Asset", null)).Returns(mockAssetCollection.Object);

            var emptyAssetCursor = new Mock<IAsyncCursor<Asset>>();
            emptyAssetCursor.Setup(_ => _.MoveNext(It.IsAny<CancellationToken>())).Returns(false);

            mockAssetCollection.Setup(x => x.FindSync(It.IsAny<FilterDefinition<Asset>>(),
                It.IsAny<FindOptions<Asset>>(), It.IsAny<CancellationToken>())).Returns(emptyAssetCursor.Object);

            // Act
            var result = _store.GetCardDatesByAssetId(assetId, correlationId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
      
    }
}
