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
    public class LocalePhrasesMongoStoreTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest()
        {
            _ = new LocalePhrasesMongoStore(null, null);
        }

        [TestMethod]
        public void GetLocalePhraseTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<Lookup>>();
            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.LocalePhrases));

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<Lookup>("Lookup", null))
                .Returns(mockCollection.Object);

            var dataStore = new LocalePhrasesMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);

            var result = dataStore.Get(20082, "correlationId");

            Assert.IsNotNull(result);
            Assert.AreEqual("Plunger Runtimes", result);
        }

        [TestMethod]
        public void GetAllTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<Lookup>>();
            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.LocalePhrases));

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<Lookup>("Lookup", null))
                .Returns(mockCollection.Object);

            var dataStore = new LocalePhrasesMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);
            int[] ids = { 20082, 20083 };

            var result = dataStore.GetAll("correlationId", ids);

            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);
        }

        #endregion

    }

}
