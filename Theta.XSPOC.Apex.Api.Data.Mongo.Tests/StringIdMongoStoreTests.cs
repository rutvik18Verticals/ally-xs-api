using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using System;
using System.Threading;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Strings;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Data.Mongo.Tests
{
    [TestClass]
    public class StringIdMongoStoreTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest()
        {
            _ = new StringIdMongoStore(null, null);
        }

        [TestMethod]
        public void GetStringIdTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<Route>>();
            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Route>>(),
                It.IsAny<FindOptions<Route>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Route>("Route"));

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<Route>("Route", null))
                .Returns(mockCollection.Object);

            var dataStore = new StringIdMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);

            var result = dataStore.GetStringId("correlationId");

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            Assert.AreEqual("Kindersley", result[0].StringName);
        }

        #endregion

    }

}
