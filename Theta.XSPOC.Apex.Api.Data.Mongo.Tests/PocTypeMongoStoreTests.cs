using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using System;
using System.Threading;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Data.Mongo.Tests
{
    [TestClass]
    public class PocTypeMongoStoreTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest()
        {
            _ = new PocTypeMongoStore(null, null);
        }

        [TestMethod]
        public void GetPocTypeTest()
        {
            // Arrange
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<Lookup>>();
            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.POCTypes));

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<Lookup>("Lookup", null))
                .Returns(mockCollection.Object);

            var dataStore = new PocTypeMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);

            // Act
            var result = dataStore.Get(1, "correlationId");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.PocType);
            Assert.AreEqual("RPC Baker/Weatherford", result.Description);
        }

        [TestMethod]
        public void GetAllTest()
        {
            // Arrange
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<Lookup>>();
            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.POCTypes));

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<Lookup>("Lookup", null))
                .Returns(mockCollection.Object);

            var dataStore = new PocTypeMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);

            // Act
            var result = dataStore.GetAll("correlationId");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(result[0].Description, "RPC Baker/Weatherford");
            Assert.AreEqual(result[0].PocType, 1);
        }

        #endregion

    }

}
