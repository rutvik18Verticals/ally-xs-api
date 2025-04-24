using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.WellControl.Contracts;
using Theta.XSPOC.Apex.Api.WellControl.Integration.Models;
using Theta.XSPOC.Apex.Api.WellControl.Logging;
using Theta.XSPOC.Apex.Api.WellControl.Services;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Integration;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.WellControl.Test.Services
{
    [TestClass]
    public class WellEnableDisableServiceTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLoggerTest()
        {
            var mockPublisher = new Mock<IEnumerable<IPublishMessage<ProcessDataUpdateContract, Responsibility>>>();
            var messages = new List<IPublishMessage<ProcessDataUpdateContract, Responsibility>>()
            {
                new TestPublishStoreUpdateDataToLegacyDBStore(),
            };
            mockPublisher.Setup(m => m.GetEnumerator()).Returns(() => messages.GetEnumerator());
            var mockNodeMaster = new Mock<INodeMaster>();

            _ = new WellEnableDisableService(null, mockPublisher.Object, mockNodeMaster.Object,
                new Mock<IConfiguration>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullPublisherTest()
        {
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            var mockNodeMaster = new Mock<INodeMaster>();

            _ = new WellEnableDisableService(mockThetaLoggerFactory.Object, null, mockNodeMaster.Object,
                new Mock<IConfiguration>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullNodeMasterServiceTest()
        {
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            var mockPublisher = new Mock<IEnumerable<IPublishMessage<ProcessDataUpdateContract, Responsibility>>>();
            var messages = new List<IPublishMessage<ProcessDataUpdateContract, Responsibility>>()
            {
                new TestPublishStoreUpdateDataToLegacyDBStore(),
            };
            mockPublisher.Setup(m => m.GetEnumerator()).Returns(() => messages.GetEnumerator());

            _ = new WellEnableDisableService(mockThetaLoggerFactory.Object, mockPublisher.Object, null,
                new Mock<IConfiguration>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullConfigurationTest()
        {
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            var mockPublisher = new Mock<IEnumerable<IPublishMessage<ProcessDataUpdateContract, Responsibility>>>();
            var messages = new List<IPublishMessage<ProcessDataUpdateContract, Responsibility>>()
            {
                new TestPublishStoreUpdateDataToLegacyDBStore(),
            };
            mockPublisher.Setup(m => m.GetEnumerator()).Returns(() => messages.GetEnumerator());

            _ = new WellEnableDisableService(mockThetaLoggerFactory.Object, mockPublisher.Object,
                new Mock<INodeMaster>().Object, null);
        }

        [TestMethod]
        public void WellEnableDisableServiceTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockConfigurationSection = new Mock<IConfigurationSection>();
            mockConfigurationSection.Setup(x => x.Value).Returns("Edge.Comms.Config.Update");

            var configuration = new Mock<IConfiguration>();
            configuration.Setup(x => x.GetSection(It.IsAny<string>())).Returns(mockConfigurationSection.Object);

            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var messages = new List<IPublishMessage<ProcessDataUpdateContract, Responsibility>>()
            {
                new TestPublishStoreUpdateDataToLegacyDBStore(),
                new TestPublishStoreUpdateDataToCommsWrapper(),
            };

            short portId = 1;
            var assetId = Guid.Parse("b27d9e0e-7c9f-4a0b-bd42-da0aefdd8264");
            var mockNodeMaster = new Mock<INodeMaster>();
            mockNodeMaster.Setup(m => m.GetNodeIdByAsset(assetId, It.IsAny<string>())).Returns("Theta Smarten");
            mockNodeMaster.Setup(m => m.TryGetPortIdByAssetGUID(assetId, out portId, It.IsAny<string>())).Returns(true);
            var service = new WellEnableDisableService(mockThetaLoggerFactory.Object, messages,
                mockNodeMaster.Object, configuration.Object);

            string enabled = "1";
            string dataCollection = "-1";
            string disableCode = "0";
            string socketId = "socketIdTest";

            var result = service.WellEnableDisableAsync(assetId, enabled, dataCollection, disableCode, socketId);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Result, ConsumerBaseAction.Success);
        }

        #endregion

        #region Utility Class

        private class TestPublishStoreUpdateDataToLegacyDBStore : IPublishMessage<ProcessDataUpdateContract, Responsibility>
        {

            #region Implementation of IDisposable

            /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
            public void Dispose()
            {
                throw new NotImplementedException();
            }

            #endregion

            #region Implementation of IPublishMessage

            public Task PublishMessageAsync(string data, string correlationId, string topic = null)
            {
                return Task.CompletedTask;
            }

            public Task PublishMessageAsync(WithCorrelationId<ProcessDataUpdateContract> data, string topic = null)
            {
                return Task.CompletedTask;
            }

            #endregion

            #region Implementation of IPublishMessage

            public Responsibility Responsibility => Responsibility.PublishStoreUpdateDataToLegacyDBStore;

            #endregion

        }

        private class TestPublishStoreUpdateDataToCommsWrapper : IPublishMessage<ProcessDataUpdateContract, Responsibility>
        {

            #region Implementation of IDisposable

            /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
            public void Dispose()
            {
                throw new NotImplementedException();
            }

            #endregion

            #region Implementation of IPublishMessage

            public Task PublishMessageAsync(string data, string correlationId, string topic = null)
            {
                return Task.CompletedTask;
            }

            public Task PublishMessageAsync(WithCorrelationId<ProcessDataUpdateContract> data, string topic = null)
            {
                return Task.CompletedTask;
            }

            #endregion

            #region Implementation of IPublishMessage

            public Responsibility Responsibility => Responsibility.PublishStoreUpdateDataToCommsWrapper;

            #endregion

        }

        #endregion

    }
}
