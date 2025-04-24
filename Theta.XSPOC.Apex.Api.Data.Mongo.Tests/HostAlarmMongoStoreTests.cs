using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using System;
using System.Threading;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Alarms;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter;
using Theta.XSPOC.Apex.Kernel.Logging;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;

namespace Theta.XSPOC.Apex.Api.Data.Mongo.Tests
{
    [TestClass]
    public class HostAlarmMongoStoreTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest()
        {
            _ = new HostAlarmMongoStore(null, null);
        }

        [TestMethod]
        public void GetLimitsForDataHistoryTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<MongoAssetCollection.Asset>>();
            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<MongoAssetCollection.Asset>>(),
                It.IsAny<FindOptions<MongoAssetCollection.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<MongoAssetCollection.Asset>("Asset"));

            var mockHostAlarmCollection = new Mock<IMongoCollection<AlarmConfiguration>>();
            mockHostAlarmCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<AlarmConfiguration>>(),
                It.IsAny<FindOptions<AlarmConfiguration>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<AlarmConfiguration>("HostAlarm"));

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<MongoAssetCollection.Asset>("Asset", null))
                .Returns(mockCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<AlarmConfiguration>("AlarmConfiguration", null))
                .Returns(mockHostAlarmCollection.Object);

            var dataStore = new HostAlarmMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);

            var result = dataStore.GetLimitsForDataHistory(new Guid("61e72096-72d4-4878-afb7-f042e0a30118"),
                new int[] { 2005, 2008 }, "correlationId");

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(2005, result[0].Address);
            Assert.AreEqual(2008, result[1].Address);
        }

        [TestMethod]
        public void GetAllGroupStatusHostAlarmsTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<MongoAssetCollection.Asset>>();
            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<MongoAssetCollection.Asset>>(),
                It.IsAny<FindOptions<MongoAssetCollection.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<MongoAssetCollection.Asset>("Asset"));

            var mockHostAlarmCollection = new Mock<IMongoCollection<AlarmConfiguration>>();
            mockHostAlarmCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<AlarmConfiguration>>(),
                It.IsAny<FindOptions<AlarmConfiguration>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<AlarmConfiguration>("HostAlarm"));

            var mockParameterCollection = new Mock<IMongoCollection<Parameters>>();
            mockParameterCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Parameters>>(),
                               It.IsAny<FindOptions<Parameters>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Parameters>("MasterVariables"));

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<MongoAssetCollection.Asset>("Asset", null))
                .Returns(mockCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<AlarmConfiguration>("AlarmConfiguration", null))
                .Returns(mockHostAlarmCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<Parameters>("MasterVariables", null))
                .Returns(mockParameterCollection.Object);

            var dataStore = new HostAlarmMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);

            var result = dataStore.GetAllGroupStatusHostAlarms(new string[] { "001 DigiUltra" }, "correlationId");

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(2005, result[0].Address);
            Assert.AreEqual(2008, result[1].Address);
        }

        #endregion

    }

}
