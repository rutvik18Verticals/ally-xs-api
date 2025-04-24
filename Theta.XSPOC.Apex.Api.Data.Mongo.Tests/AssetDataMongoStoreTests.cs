using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;

namespace Theta.XSPOC.Apex.Api.Data.Mongo.Tests
{
    [TestClass]
    public class AssetDataMongoStoreTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest()
        {
            _ = new AssetDataMongoStore(null, null);
        }

        [TestMethod]
        public void GetWellEnabledStatusTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<MongoAssetCollection.Asset>>();
            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<MongoAssetCollection.Asset>>(),
                It.IsAny<FindOptions<MongoAssetCollection.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(GetAssetModel());

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<MongoAssetCollection.Asset>("Asset", null))
                .Returns(mockCollection.Object);

            var assetStore = new AssetDataMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);

            var result = assetStore.GetWellEnabledStatus(new Guid("61e72096-72d4-4878-afb7-f042e0a30118"), "correlationId");

            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void GetWellEnabledStatusWellNotFoundTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<MongoAssetCollection.Asset>>();

            var filter = new FilterDefinitionBuilder<MongoAssetCollection.Asset>()
                .Eq("LegacyId.NodeId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            mockCollection.Setup(m => m.FindSync(filter,
                It.IsAny<FindOptions<MongoAssetCollection.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(GetAssetModel());

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<MongoAssetCollection.Asset>("Asset", null))
                .Returns(mockCollection.Object);

            var assetStore = new AssetDataMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);

            var result = assetStore.GetWellEnabledStatus(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "correlationId");

            Assert.IsNull(result);
            logger.Verify(m => m.WriteCId(Level.Info, "Missing node", "correlationId"), Times.Once);
        }

        #endregion

        #region Private Methods

        private IAsyncCursor<MongoAssetCollection.Asset> GetAssetModel()
        {
            var result = new Mock<IAsyncCursor<MongoAssetCollection.Asset>>();
            result.Setup(m => m.Current).Returns(GetAssetModelData());
            result.SetupSequence(m => m.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
            return result.Object;
        }

        private IList<MongoAssetCollection.Asset> GetAssetModelData()
        {
            return new List<MongoAssetCollection.Asset>()
            {
                new() {
                    Name = "001 DigiUltra",
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "NodeId", "001 DigiUltra" },
                        { "AssetGUID", "61e72096-72d4-4878-afb7-f042e0a30118" }
                    },
                    AssetConfig = new AssetConfig()
                    {
                        RunStatus = "Running",
                        IsEnabled = true,
                        PortId = "1",
                    },
                }
            };
        }

        #endregion

    }
}
