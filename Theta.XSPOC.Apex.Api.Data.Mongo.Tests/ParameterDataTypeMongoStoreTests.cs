using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;

namespace Theta.XSPOC.Apex.Api.Data.Mongo.Tests
{
    [TestClass]
    public class ParameterDataTypeMongoStoreTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest()
        {
            _ = new ParameterDataTypeMongoStore(null, null);
        }

        [TestMethod]
        public void GetItemsTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<Lookup>>();
            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Lookup>("Lookup", (int)LookupTypes.DataTypes));

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<Lookup>("Lookup", null))
                .Returns(mockCollection.Object);

            var dataStore = new ParameterDataTypeMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);

            var result = dataStore.GetItems("correlationId");

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void GetItemsNullTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<Lookup>>();
            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                               It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns((IAsyncCursor<Lookup>)null);

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<Lookup>("Lookup", null))
                .Returns(mockCollection.Object);

            var dataStore = new ParameterDataTypeMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);

            var result = dataStore.GetItems("correlationId");

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
            logger.Verify(x => x.WriteCId(Level.Info, "Missing lookup data", "correlationId"), Times.Once);
        }

        [TestMethod]
        public void GetItemsEmptyTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<Lookup>>();
            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Lookup>>(),
                                              It.IsAny<FindOptions<Lookup>>(), It.IsAny<CancellationToken>()))
                .Returns(new Mock<IAsyncCursor<Lookup>>().Object);

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<Lookup>("Lookup", null))
                .Returns(mockCollection.Object);

            var dataStore = new ParameterDataTypeMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);

            var result = dataStore.GetItems("correlationId");

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetParametersDataTypesTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockAssetCollection = new Mock<IMongoCollection<MongoAssetCollection.Asset>>();
            mockAssetCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<MongoAssetCollection.Asset>>(),
                               It.IsAny<FindOptions<MongoAssetCollection.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<MongoAssetCollection.Asset>("Asset"));

            var mockParameterCollection = new Mock<IMongoCollection<Parameters>>();
            mockParameterCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Parameters>>(),
                               It.IsAny<FindOptions<Parameters>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Parameters>("MasterVariables"));

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<MongoAssetCollection.Asset>("Asset", null))
                .Returns(mockAssetCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<Parameters>("MasterVariables", null))
                .Returns(mockParameterCollection.Object);

            var dataStore = new ParameterDataTypeMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);

            var result = dataStore.GetParametersDataTypes(Guid.NewGuid(), new List<int> { 1001, 1002, 1003 }, "correlationId");

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual("1", result[1001].ToString());
            Assert.AreEqual("2", result[1002].ToString());
            Assert.AreEqual("3", result[1003].ToString());
            Assert.AreEqual("1", result[10001].ToString());
        }

        [TestMethod]
        public void GetParameterDataNullAssetData()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockAssetCollection = new Mock<IMongoCollection<MongoAssetCollection.Asset>>();
            mockAssetCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<MongoAssetCollection.Asset>>(),
                                          It.IsAny<FindOptions<MongoAssetCollection.Asset>>(), It.IsAny<CancellationToken>()))
                                                         .Returns((IAsyncCursor<MongoAssetCollection.Asset>)null);

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<MongoAssetCollection.Asset>("Asset", null))
                           .Returns(mockAssetCollection.Object);

            var dataStore = new ParameterDataTypeMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);

            var result = dataStore.GetParametersDataTypes(Guid.NewGuid(), new List<int> { 1001, 1002, 1003 }, "correlationId");

            Assert.IsNull(result);
            logger.Verify(x => x.WriteCId(Level.Info, "Missing node", "correlationId"), Times.Once);
        }

        #endregion

    }

}
