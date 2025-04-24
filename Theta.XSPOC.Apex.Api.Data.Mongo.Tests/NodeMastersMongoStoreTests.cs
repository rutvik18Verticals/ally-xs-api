using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Ports;
using Theta.XSPOC.Apex.Kernel.Logging;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;

namespace Theta.XSPOC.Apex.Api.Data.Mongo.Tests
{

    [TestClass]
    public class NodeMastersMongoStoreTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest()
        {
            _ = new NodeMastersMongoStore(null, null);
        }

        [TestMethod]
        public void GetNodeTest()
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

            var _nodeMasterMongoStore = new NodeMastersMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);

            var result = _nodeMasterMongoStore.GetNode(new Guid("61e72096-72d4-4878-afb7-f042e0a30118"), "correlationId");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.NodeId == "001 DigiUltra");
            Assert.IsTrue(result.AssetGuid == new Guid("61e72096-72d4-4878-afb7-f042e0a30118"));
        }

        [TestMethod]
        public void GetNodeIdByAssetTest()
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

            var _nodeMasterMongoStore = new NodeMastersMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);

            var result = _nodeMasterMongoStore.GetNodeIdByAsset(new Guid("61e72096-72d4-4878-afb7-f042e0a30118"), "correlationId");

            Assert.IsNotNull(result);
            Assert.IsTrue(result == "001 DigiUltra");
        }

        [TestMethod]
        public void TryGetPortIdByAssetGUIDTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<MongoAssetCollection.Asset>>();
            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<MongoAssetCollection.Asset>>(),
                                   It.IsAny<FindOptions<MongoAssetCollection.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(GetAssetModel());

            var mockPortCollection = new Mock<IMongoCollection<Ports>>();
            mockPortCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Ports>>(),
                                               It.IsAny<FindOptions<Ports>>(), It.IsAny<CancellationToken>()))
                            .Returns(GetPorts());
            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<MongoAssetCollection.Asset>("Asset", null))
                .Returns(mockCollection.Object);

            mockDatabase.Setup(m => m.GetCollection<Ports>("Port", null))
                .Returns(mockPortCollection.Object);

            var _nodeMasterMongoStore = new NodeMastersMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);
            var result = _nodeMasterMongoStore.TryGetPortIdByAssetGUID(new Guid("61e72096-72d4-4878-afb7-f042e0a30118"), out var portId, "correlationId");

            Assert.IsNotNull(result);
            Assert.IsTrue(result);
            Assert.IsTrue(portId == 1);
        }

        [TestMethod]
        public void GetNodeMasterDataTest()
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

            var _nodeMasterMongoStore = new NodeMastersMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);
            string[] columns = new string[] { "AssetGuid", "Node", "CommStatus", "LastGoodScanTime", "PortId" };
            var result = _nodeMasterMongoStore.GetNodeMasterData(new Guid("61e72096-72d4-4878-afb7-f042e0a30118"), columns, "correlationId");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Data.Count == 5);
            Assert.IsTrue(result.Data["AssetGuid"] == "61e72096-72d4-4878-afb7-f042e0a30118");
            Assert.IsTrue(result.Data["PortId"] == "1");
            Assert.IsTrue(result.Data["CommStatus"] == null);
            Assert.IsTrue(result.Data["Node"] == null);
            Assert.IsTrue(result.Data["LastGoodScanTime"] == null);
        }

        [TestMethod]
        public void TryGetPocTypeIdByAssetGUIDTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<MongoAssetCollection.Asset>>();
            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<MongoAssetCollection.Asset>>(),
                                   It.IsAny<FindOptions<MongoAssetCollection.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(GetAssetModel());

            var mockPortCollection = new Mock<IMongoCollection<Ports>>();
            mockPortCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Ports>>(),
                                               It.IsAny<FindOptions<Ports>>(), It.IsAny<CancellationToken>()))
                            .Returns(GetPorts());
            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<MongoAssetCollection.Asset>("Asset", null))
                .Returns(mockCollection.Object);

            mockDatabase.Setup(m => m.GetCollection<Ports>("Port", null))
                .Returns(mockPortCollection.Object);

            var _nodeMasterMongoStore = new NodeMastersMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);
            var result = _nodeMasterMongoStore.TryGetPocTypeIdByAssetGUID(new Guid("61e72096-72d4-4878-afb7-f042e0a30118"), out var pocType, "correlationId");

            Assert.IsNotNull(result);
            Assert.IsTrue(result);
            Assert.IsTrue(pocType == 8);
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

        private IAsyncCursor<Ports> GetPorts()
        {
            var result = new Mock<IAsyncCursor<Ports>>();
            result.Setup(m => m.Current).Returns(new List<Ports>
            {
                new()
                {
                    PortID = 1,
                    PortType = 6
                }
            });

            result.SetupSequence(m => m.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);

            return result.Object;
        }

        private IList<MongoAssetCollection.Asset> GetAssetModelData()
        {
            return new List<MongoAssetCollection.Asset>()
            {
                new() {
                    Name = "001 DigiUltra",
                    ArtificialLiftType = "RodLift",
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "NodeId", "001 DigiUltra" },
                        { "AssetGUID", "61e72096-72d4-4878-afb7-f042e0a30118" }
                    },
                    POCType = new Lookup()
                    {
                        LookupType ="POCTypesId",
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "POCTypesId", "8" }
                        },
                    },
                    AssetConfig = new AssetConfig()
                    {
                        RunStatus = "Running",
                        TimeInState = 6,
                        TodayCycles = 4,
                        TodayRuntime = 5,
                        InferredProduction = 4,
                        IsEnabled = true,
                        PortId = "1",
                    },
                }
            };
        }

        #endregion

    }
}
