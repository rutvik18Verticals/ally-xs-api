using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;
using MongoCardCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Card;

namespace Theta.XSPOC.Apex.Api.Data.Mongo.Tests
{
    [TestClass]
    public class CardCoordinateMongoStoreTests
    {
        #region Test Methods
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest()
        {
            _ = new CardCoordinateMongoStore(null, null);
        }

        [TestMethod]
        public void GetCardCoordinateDataTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockAssetCollection = new Mock<IMongoCollection<MongoAssetCollection.Asset>>();
            mockAssetCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<MongoAssetCollection.Asset>>(),
                It.IsAny<FindOptions<MongoAssetCollection.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(GetAssetModel());

            var mockCardCollection = new Mock<IMongoCollection<MongoCardCollection.Card>>();
            mockCardCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<MongoCardCollection.Card>>(),
                It.IsAny<FindOptions<MongoCardCollection.Card>>(), It.IsAny<CancellationToken>()))
                .Returns(GetCardModel());

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<MongoAssetCollection.Asset>("Asset", null))
                .Returns(mockAssetCollection.Object);

            mockDatabase.SetupSequence(m => m.GetCollection<MongoCardCollection.Card>("Card", null))
                .Returns(mockCardCollection.Object);

            var assetStore = new CardCoordinateMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);

            var result = assetStore.GetCardCoordinateData(new Guid("B1681138-E9A3-4364-9AF2-935815FA8776"), "2023-11-15 11:55:21.000", "correlationId");

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetCardCoordinateDataNotFoundTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockAssetCollection = new Mock<IMongoCollection<MongoAssetCollection.Asset>>();
            var filter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
               .Eq("LegacyId.NodeId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            mockAssetCollection.Setup(m => m.FindSync(filter,
                It.IsAny<FindOptions<MongoAssetCollection.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(GetAssetModel());

            var mockCardCollection = new Mock<IMongoCollection<MongoCardCollection.Card>>();
            mockCardCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<MongoCardCollection.Card>>(),
                It.IsAny<FindOptions<MongoCardCollection.Card>>(), It.IsAny<CancellationToken>()))
                .Returns(GetCardModel());

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<MongoAssetCollection.Asset>("Asset", null))
                .Returns(mockAssetCollection.Object);

            mockDatabase.SetupSequence(m => m.GetCollection<MongoCardCollection.Card>("Card", null))
                .Returns(mockCardCollection.Object);

            var assetStore = new CardCoordinateMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);

            var result = assetStore.GetCardCoordinateData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-11-15 11:55:21.000", "correlationId");

            Assert.IsNull(result);
            logger.Verify(m => m.WriteCId(Level.Info, "Missing node", "correlationId"), Times.Once);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Get Asset Model Data
        /// </summary>
        /// <returns></returns>
        private IAsyncCursor<MongoAssetCollection.Asset> GetAssetModel()
        {
            var result = new Mock<IAsyncCursor<MongoAssetCollection.Asset>>();
            result.Setup(m => m.Current).Returns(GetAssetModelData());
            result.SetupSequence(m => m.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
            return result.Object;
        }

        /// <summary>
        /// Set Asset Model Data
        /// </summary>
        /// <returns></returns>
        private IList<MongoAssetCollection.Asset> GetAssetModelData()
        {
            return new List<MongoAssetCollection.Asset>()
            {
                new() {
                    Id = "66d74ffac66365161d7d8f0a",
                    Name = "Theta SMARTEN",
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "NodeId", "Theta SMARTEN" },
                        { "AssetGUID", "B1681138-E9A3-4364-9AF2-935815FA8776" }
                    },
                    AssetConfig = new AssetConfig()
                    {
                        RunStatus = "Running",
                        IsEnabled = true,
                        PortId = "1",
                    },
                    POCType = new Lookup()
                    {
                        LookupDocument = new POCTypes { POCType = 17}
                    },
                }
            };
        }

        /// <summary>
        /// Get Card Model Data
        /// </summary>
        /// <returns></returns>
        private IAsyncCursor<MongoCardCollection.Card> GetCardModel()
        {
            var result = new Mock<IAsyncCursor<MongoCardCollection.Card>>();
            result.Setup(m => m.Current).Returns(GetCardModelData());
            result.SetupSequence(m => m.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
            return result.Object;
        }

        /// <summary>
        /// Set Card Model Data
        /// </summary>
        /// <returns></returns>
        private IList<MongoCardCollection.Card> GetCardModelData()
        {
            var pointsData = new List<CoordinatesDataModel<float>>()
            {
                new() { X = 1.2f, Y = 3.4f },
                new() { X = 5.6f, Y = 7.8f },
                new() { X = 9.0f, Y = 10.1f }
            };

            return new List<MongoCardCollection.Card>()
            {
                new() {
                    Area = 0,
                    AreaLimit = 70,
                    DownholeCardPoints = pointsData,
                    Fillage = 94.6,
                    FillBasePct =65,
                    HiLoadLimit = 50000,
                    LoadLimit = 47,
                    LoadLimit2 = 0,
                    LoLoadLimit = 4000,
                    MalfuncationLoadLimit = 71,
                    MalfuncationPositionLimit = 64,
                    POCDownholeCardPoints = pointsData,
                    PositionLimit = 40,
                    PositionLimit2 = 0,
                    StrokesPerMinute = 7.6,
                    StrokeLength = 144,
                    SurfaceCardPoints = pointsData,
                    PredictedCardPoints= pointsData,
                    TorquePlotMinEnergyPoints = pointsData,
                    TorquePlotMinTorquePoints = pointsData,
                    TorquePlotCurrentPoints = pointsData,
                    PermissibleLoadUpPoints = pointsData,
                    PermissibleLoadDownPoints = pointsData,
                    ElectrogramCardPoints = pointsData
                }
            };
        }
        #endregion
    }
}
