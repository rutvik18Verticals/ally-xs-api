using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using System;
using System.Threading;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using Theta.XSPOC.Apex.Kernel.Logging;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;

namespace Theta.XSPOC.Apex.Api.Data.Mongo.Tests
{
    [TestClass]
    public class RodMongoStoreTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest()
        {
            _ = new RodMongoStore(null, null);
        }

        [TestMethod]
        public void GetRodForGroupStatusTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockAssetCollection = new Mock<IMongoCollection<MongoAssetCollection.Asset>>();
            mockAssetCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<MongoAssetCollection.Asset>>(),
                It.IsAny<FindOptions<MongoAssetCollection.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<MongoAssetCollection.Asset>("Asset"));

            var mockRodsCollection = new Mock<IMongoCollection<Lookup>>();
            mockRodsCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.RodGrade));

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<MongoAssetCollection.Asset>("Asset", null))
                .Returns(mockAssetCollection.Object);
            mockDatabase.SetupSequence(m => m.GetCollection<Lookup>("Lookup", null))
                .Returns(mockRodsCollection.Object);

            var dataStore = new RodMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);

            var nodeIds = new string[] { "001 DigiUltra" };

            var result = dataStore.GetRodForGroupStatus(nodeIds, "correlationId");
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("D", result[0].Name);
            Assert.AreEqual("1", result[0].RodNum.ToString());
        }

        #endregion

    }

}
