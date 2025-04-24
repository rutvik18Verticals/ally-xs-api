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
    public class GroupAndAssetMongoStoreTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest()
        {
            _ = new GroupAndAssetMongoStore(null, null);
        }

        [TestMethod]
        public void GroupAndAssetGetTest()
        {
            var mockCollection = new Mock<IMongoCollection<GroupAndAssetModel>>();
            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<GroupAndAssetModel>>(),
                It.IsAny<FindOptions<GroupAndAssetModel>>(), It.IsAny<CancellationToken>())).Returns(GetGroupModel());
            var correlationId = Guid.NewGuid().ToString();
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<GroupAndAssetModel>("Group", null)).Returns(mockCollection.Object);

            var groupAndAsset = new GroupAndAssetMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);

            var result = groupAndAsset.GetGroupAssetAndRelationshipData(correlationId);

            Assert.IsNotNull(result);

            Assert.AreEqual(2, result.ChildGroups.Count);
            Assert.AreEqual(1, result.ChildGroups[0].Assets.Count);
            Assert.AreEqual("AssetId1", result.ChildGroups[0].Assets[0].AssetName);
            Assert.AreEqual(1, result.ChildGroups[0].Assets[0].IndustryApplicationId);
            Assert.AreEqual("DFC1D0AD-A824-4965-B78D-AB7755E32DD3",
                result.ChildGroups[0].Assets[0].AssetId.ToString().ToUpper());

            Assert.AreEqual(2, result.ChildGroups[1].Assets.Count);
            Assert.AreEqual("AssetId1", result.ChildGroups[1].Assets[0].AssetName);
            Assert.AreEqual(1, result.ChildGroups[1].Assets[0].IndustryApplicationId);
            Assert.AreEqual("DFC1D0AD-A824-4965-B78D-AB7755E32DD3",
                result.ChildGroups[1].Assets[0].AssetId.ToString().ToUpper());
            Assert.AreEqual("AssetId2", result.ChildGroups[1].Assets[1].AssetName);
            Assert.AreEqual(2, result.ChildGroups[1].Assets[1].IndustryApplicationId);
            Assert.AreEqual("DFC1D0AD-A824-4965-B78D-AB7755E32DD2",
                result.ChildGroups[1].Assets[1].AssetId.ToString().ToUpper());
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
                    new GroupAndAssetModel()
                    {
                        GroupName = "<100 Intake Pressure",
                        Assets = new List<AssetModel>()
                        {
                            new AssetModel()
                            {
                                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                                AssetName = "AssetId1",
                                IndustryApplicationId = 1,
                            },
                        },
                    },
                    new GroupAndAssetModel()
                    {
                        GroupName = "> 300 Days Run Time",
                        Assets = new List<AssetModel>()
                        {
                            new AssetModel()
                            {
                                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                                AssetName = "AssetId1",
                                IndustryApplicationId = 1,
                            },
                            new AssetModel()
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
