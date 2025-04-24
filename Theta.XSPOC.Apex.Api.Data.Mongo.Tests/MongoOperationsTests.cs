using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Data.Mongo.Tests
{
    [TestClass]
    public class MongoOperationsTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest()
        {
            _ = new MongoOperations(null, null);
        }

        [TestMethod]
        public void FindAllTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<GroupAndAssetModel>>();
            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<GroupAndAssetModel>>(),
                It.IsAny<FindOptions<GroupAndAssetModel>>(), It.IsAny<CancellationToken>())).Returns(GetGroupModel());

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<GroupAndAssetModel>("Group", null)).Returns(mockCollection.Object);

            var mongoStore = new MongoOperations(mockDatabase.Object, mockThetaLoggerFactory.Object);

            var result = mongoStore.FindAll<GroupAndAssetModel>("Group", "correlationId");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void FindWithFilterTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<GroupAndAssetModel>>();
            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<GroupAndAssetModel>>(),
                It.IsAny<FindOptions<GroupAndAssetModel>>(), It.IsAny<CancellationToken>())).Returns(GetGroupModel());

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<GroupAndAssetModel>("Group", null)).Returns(mockCollection.Object);

            var mongoStore = new MongoOperations(mockDatabase.Object, mockThetaLoggerFactory.Object);

            var filter = Builders<GroupAndAssetModel>.Filter.Eq("GroupName", "Well Group");

            var result = mongoStore.Find<GroupAndAssetModel>("Group", filter, "correlationId");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void FindWithFilterAndKeyTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<GroupAndAssetModel>>();
            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<GroupAndAssetModel>>(),
                It.IsAny<FindOptions<GroupAndAssetModel>>(), It.IsAny<CancellationToken>())).Returns(GetGroupModel());

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<GroupAndAssetModel>("Group", null)).Returns(mockCollection.Object);

            var mongoStore = new MongoOperations(mockDatabase.Object, mockThetaLoggerFactory.Object);

            var result = mongoStore.Find<GroupAndAssetModel, string>("Group", "GroupName", "<100 Intake Pressure", "correlationId");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.ChildGroups.Count > 0);
            Assert.IsTrue(result.ChildGroups[0].GroupName == "<100 Intake Pressure");
        }

        #endregion

        #region Private Methods

        private static IAsyncCursor<GroupAndAssetModel> GetGroupModel()
        {
            var input = new GroupAndAssetModel()
            {
                GroupName = "Well Group",
                ChildGroups = new List<GroupAndAssetModel>()
                {
                    new()
                    {
                        GroupName = "<100 Intake Pressure",
                        Assets = new List<AssetModel>()
                        {
                            new()
                            {
                                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                                AssetName = "AssetId1",
                                IndustryApplicationId = 1,
                            },
                        },
                    },
                    new()
                    {
                        GroupName = "> 300 Days Run Time",
                        Assets = new List<AssetModel>()
                        {
                            new()
                            {
                                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                                AssetName = "AssetId1",
                                IndustryApplicationId = 1,
                            },
                            new()
                            {
                                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD2"),
                                AssetName = "AssetId2",
                                IndustryApplicationId = 2,
                            },
                        },
                    },
                }
            };

            var result = new Mock<IAsyncCursor<GroupAndAssetModel>>();
            result.Setup(m => m.Current).Returns(new List<GroupAndAssetModel>()
            {
                input
            });
            result.SetupSequence(m => m.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);

            return result.Object;
        }

        #endregion

    }
}
