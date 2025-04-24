using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Customers;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Ports;
using Theta.XSPOC.Apex.Kernel.Logging;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;

namespace Theta.XSPOC.Apex.Api.Data.Mongo.Tests
{
    [TestClass]
    public class PortConfigurationMongoStoreTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest()
        {
            _ = new PortConfigurationMongoStore(null, null);
        }

        [TestMethod]
        public void IsLegacyWellTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<Ports>>();
            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Ports>>(),
                It.IsAny<FindOptions<Ports>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Ports>("Port"));

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<Ports>("Port", null))
                .Returns(mockCollection.Object);

            var dataStore = new PortConfigurationMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);
            Task<bool> result = Task.FromResult(false);

            Task.Run(async () =>
            {
                result = dataStore.IsLegacyWellAsync(1, "correlationId");
            }).GetAwaiter().GetResult();

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Result);
        }

        [TestMethod]
        public void GetNodeTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCollection = new Mock<IMongoCollection<MongoAssetCollection.Asset>>();
            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<MongoAssetCollection.Asset>>(),
                It.IsAny<FindOptions<MongoAssetCollection.Asset>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<MongoAssetCollection.Asset>("Asset"));

            var mockPortCollection = new Mock<IMongoCollection<Ports>>();
            mockPortCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Ports>>(),
                It.IsAny<FindOptions<Ports>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Ports>("Port"));

            var mockCustomerCollection = new Mock<IMongoCollection<Customer>>();
            mockCustomerCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<Customer>>(),
                It.IsAny<FindOptions<Customer>>(), It.IsAny<CancellationToken>()))
                .Returns(TestUtility.GetMockMongoData<Customer>("Customers"));

            var mockDatabase = new Mock<IMongoDatabase>();
            mockDatabase.Setup(m => m.GetCollection<MongoAssetCollection.Asset>("Asset", null))
                .Returns(mockCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<Ports>("Port", null))
                .Returns(mockPortCollection.Object);
            mockDatabase.Setup(m => m.GetCollection<Customer>("Customers", null))
                .Returns(mockCustomerCollection.Object);

            var dataStore = new PortConfigurationMongoStore(mockDatabase.Object, mockThetaLoggerFactory.Object);

            var result = dataStore.GetNode(new Guid("61e72096-72d4-4878-afb7-f042e0a30118"), "correlationId");

            Assert.IsNotNull(result);
            Assert.AreEqual("61e72096-72d4-4878-afb7-f042e0a30118", result.AssetGuid.ToString());
            Assert.AreEqual("0", result.PortId.ToString());
            Assert.AreEqual("a5fa13b2-56e8-4aaa-a2d5-61375609de9e", result.CompanyGuid.ToString());
        }

        #endregion

    }
}
