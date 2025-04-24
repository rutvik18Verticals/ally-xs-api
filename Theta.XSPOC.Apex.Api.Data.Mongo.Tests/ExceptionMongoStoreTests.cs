using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Exceptions;
using MongoAsset = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Data.Mongo.Tests
{
    [TestClass]
    public class ExceptionMongoStoreTests
    {
        private Mock<IMongoDatabase> _mockDatabase;
        private Mock<IThetaLoggerFactory> _mockLoggerFactory;
        private Mock<IThetaLogger> _mockLogger;
        private Mock<IMongoCollection<Exceptions>> _mockExceptionsCollection;
        private Mock<IMongoCollection<MongoAsset.Asset>> _mockAssetCollection;
        private ExceptionMongoStore _exceptionMongoStore;

        [TestInitialize]
        public void Setup()
        {
            _mockDatabase = new Mock<IMongoDatabase>();
            _mockLoggerFactory = new Mock<IThetaLoggerFactory>();
            _mockLogger = new Mock<IThetaLogger>();

            _mockLoggerFactory.Setup(factory => factory.Create(It.IsAny<LoggingModel>())).Returns(_mockLogger.Object);

            _mockExceptionsCollection = new Mock<IMongoCollection<Exceptions>>();
            _mockAssetCollection = new Mock<IMongoCollection<MongoAsset.Asset>>();

            _mockDatabase.Setup(db => db.GetCollection<Exceptions>("Exceptions", null)).Returns(_mockExceptionsCollection.Object);
            _mockDatabase.Setup(db => db.GetCollection<MongoAsset.Asset>("Asset", null)).Returns(_mockAssetCollection.Object);

            _exceptionMongoStore = new ExceptionMongoStore(_mockDatabase.Object, _mockLoggerFactory.Object);
        }

        [TestMethod]
        public void GetExceptions_ReturnsCorrectExceptions()
        {
            // Arrange
            var nodeIds = new List<string> { "Node1", "Node2" };
            var correlationId = "test-correlation-id";

            var assets = new List<MongoAsset.Asset>
            {
                new() { Id = ObjectId.GenerateNewId().ToString(), Name = "Node1" },
                new() { Id = ObjectId.GenerateNewId().ToString(), Name = "Node2" }
            };

            var exceptions = new List<Exceptions>
            {
                new() { AssetId = assets[0].Id, ExceptionGroupName = "Group1", Priority = 1 },
                new() { AssetId = assets[1].Id, ExceptionGroupName = "Group2", Priority = 2 },
                new() { AssetId = assets[1].Id, ExceptionGroupName = "Group3", Priority = 3 }
            };

            _mockAssetCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<MongoAsset.Asset>>(), It.IsAny<FindOptions<MongoAsset.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(GetAsyncCursorMock(assets));

            _mockExceptionsCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Exceptions>>(), It.IsAny<FindOptions<Exceptions>>(), It.IsAny<CancellationToken>()))
                .Returns(GetAsyncCursorMock(exceptions));

            // Act
            var result = _exceptionMongoStore.GetExceptions(nodeIds, correlationId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Group1", result[0].ExceptionGroupName);
            Assert.AreEqual(1, result[0].Priority);
            Assert.AreEqual("Group3", result[1].ExceptionGroupName);
            Assert.AreEqual(3, result[1].Priority);
        }

        [TestMethod]
        public async Task GetAssetStatusExceptionsAsync_ReturnsCorrectData()
        {
            // Arrange
            var assetId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var correlationId = "test-correlation-id";

            var asset = new MongoAsset.Asset { Id = ObjectId.GenerateNewId().ToString(), LegacyId = new Dictionary<string, string> { { "AssetGUID", assetId.ToString() } } };
            var exceptions = new List<Exceptions>
            {
                new() { AssetId = asset.Id, ExceptionGroupName = "Group1", Priority = 1 },
                new() { AssetId = asset.Id, ExceptionGroupName = "Group2", Priority = 2 }
            };

            _mockAssetCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<MongoAsset.Asset>>(), It.IsAny<FindOptions<MongoAsset.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(GetAsyncCursorMock(new List<MongoAsset.Asset> { asset }));

            _mockExceptionsCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Exceptions>>(), It.IsAny<FindOptions<Exceptions>>(), It.IsAny<CancellationToken>()))
                .Returns(GetAsyncCursorMock(exceptions));

            // Act
            var result = await _exceptionMongoStore.GetAssetStatusExceptionsAsync(assetId, customerId, correlationId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Group1", result[0].Description);
            Assert.AreEqual(1, result[0].Priority);
            Assert.AreEqual("Group2", result[1].Description);
            Assert.AreEqual(2, result[1].Priority);
        }

        private IAsyncCursor<T> GetAsyncCursorMock<T>(List<T> data)
        {
            var mockCursor = new Mock<IAsyncCursor<T>>();
            mockCursor.SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
            mockCursor.SetupGet(_ => _.Current).Returns(data);
            return mockCursor.Object;
        }

        [TestMethod]
        public void GetExceptions_ReturnsEmptyList_WhenAssetNotFound()
        {
            // Arrange
            var nodeIds = new List<string> { "NonExistentNode" };
            var correlationId = "test-correlation-id";

            // Set up the asset collection to return an empty cursor, simulating no assets found
            _mockAssetCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<MongoAsset.Asset>>(),
                It.IsAny<FindOptions<MongoAsset.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(GetAsyncCursorMock(new List<MongoAsset.Asset>()));

            // Act
            var result = _exceptionMongoStore.GetExceptions(nodeIds, correlationId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAssetStatusExceptionsAsync_ReturnsEmptyList_WhenAssetNotFound()
        {
            // Arrange
            var assetId = Guid.NewGuid(); // An ID that doesn't exist in the mock data
            var customerId = Guid.NewGuid();
            var correlationId = "test-correlation-id";

            // Set up the asset collection to return an empty cursor, simulating no asset found
            _mockAssetCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<MongoAsset.Asset>>(),
                It.IsAny<FindOptions<MongoAsset.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(GetAsyncCursorMock(new List<MongoAsset.Asset>()));

            // Act
            var result = await _exceptionMongoStore.GetAssetStatusExceptionsAsync(assetId, customerId, correlationId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

    }
}
