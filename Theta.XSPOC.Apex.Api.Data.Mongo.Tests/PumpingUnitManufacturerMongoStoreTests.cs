using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using Theta.XSPOC.Apex.Kernel.Logging;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;

namespace Theta.XSPOC.Apex.Api.Data.Mongo.Tests
{

    [TestClass]
    public class PumpingUnitManufacturerMongoStoreTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest()
        {
            _ = new PumpingUnitManufacturerMongoStore(null, null);
        }

        [TestMethod]
        public void GetManufacturersTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockAssetCollection = new Mock<IMongoCollection<MongoAssetCollection.Asset>>();
            mockAssetCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<MongoAssetCollection.Asset>>(),
                               It.IsAny<FindOptions<MongoAssetCollection.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<MongoAssetCollection.Asset>("Asset"));

            var mockCollection = new Mock<IMongoCollection<Lookup>>();
            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.PumpingUnitManufacturer));

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<MongoAssetCollection.Asset>("Asset", null))
                           .Returns(mockAssetCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<Lookup>("Lookup", null))
                .Returns(mockCollection.Object);

            var dataStore = new PumpingUnitManufacturerMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);

            var nodeIds = new List<string> { "001 DigiUltra", "Well1" };
            var result = dataStore.GetManufacturers(nodeIds, "correlationId");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
        }

        #endregion

    }
}
