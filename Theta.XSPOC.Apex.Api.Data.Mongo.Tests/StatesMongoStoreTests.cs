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
    public class StatesMongoStoreTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest()
        {
            _ = new StatesMongoStore(null, null);
        }

        [TestMethod]
        public void GetCardTypeNamePocType17CardTypeMSTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollectionStates = new Mock<IMongoCollection<Lookup>>();
            mockCollectionStates.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.States));

            var mockCollectionLocalePhrase = new Mock<IMongoCollection<Lookup>>();
            mockCollectionLocalePhrase.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.LocalePhrases));

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.SetupSequence(m => m.GetCollection<Lookup>("Lookup", null))
                .Returns(mockCollectionStates.Object)
                .Returns(mockCollectionLocalePhrase.Object);

            var dataStore = new StatesMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);

            var result = dataStore.GetCardTypeNamePocType17CardTypeMS(57, "correlationId");

            Assert.IsNotNull(result);
        }

        #endregion

    }

}
