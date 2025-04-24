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
    public class ControlActionMongoStoreTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest()
        {
            _ = new ControlActionMongoStore(null, null);
        }

        [TestMethod]
        public void GetControlActionsTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockAssetCollection = new Mock<IMongoCollection<MongoAssetCollection.Asset>>();
            mockAssetCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<MongoAssetCollection.Asset>>(),
                It.IsAny<FindOptions<MongoAssetCollection.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<MongoAssetCollection.Asset>("Asset"));

            var mockPoctypeActionsCollection = new Mock<IMongoCollection<Lookup>>();
            mockPoctypeActionsCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.POCTypeAction));

            var mockControlActionsCollection = new Mock<IMongoCollection<Lookup>>();
            mockControlActionsCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.ControlActions));

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<MongoAssetCollection.Asset>("Asset", null))
                .Returns(mockAssetCollection.Object);
            mockDatabase.SetupSequence(m => m.GetCollection<Lookup>("Lookup", null))
                .Returns(mockPoctypeActionsCollection.Object)
                .Returns(mockControlActionsCollection.Object);

            var dataStore = new ControlActionMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);

            var result = dataStore.GetControlActions(new Guid("61e72096-72d4-4878-afb7-f042e0a30118"), "correlationId");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count == 2);
        }

        #endregion

    }

}
