using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Theta.XSPOC.Apex.Api.Common.Communications;
using Theta.XSPOC.Apex.Api.WellControl.Logging;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.WellControl.Contracts.Requests;
using Theta.XSPOC.Apex.Api.WellControl.Controllers;
using Theta.XSPOC.Apex.Api.WellControl.Services;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.WellControl.Test.Controllers
{
    [TestClass]
    public class WellControlControllerTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullProcessingServiceTest()
        {
            _ = new WellControlController(
                null,
                new Mock<ITransactionPayloadCreator>().Object,
                new Mock<IWellEnableDisableService>().Object,
                new Mock<IWellControlService>().Object,
                new Mock<IThetaLoggerFactory>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullTransactionPayloadCreatorTest()
        {
            _ = new WellControlController(
                new Mock<IProcessingDataUpdatesService>().Object,
                null,
                new Mock<IWellEnableDisableService>().Object,
                new Mock<IWellControlService>().Object,
                new Mock<IThetaLoggerFactory>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullWellEnableDisableServiceTest()
        {
            _ = new WellControlController(
                new Mock<IProcessingDataUpdatesService>().Object,
                new Mock<ITransactionPayloadCreator>().Object,
                null,
                new Mock<IWellControlService>().Object,
                new Mock<IThetaLoggerFactory>().Object);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullWellControlServiceTest()
        {
            _ = new WellControlController(
                new Mock<IProcessingDataUpdatesService>().Object,
                new Mock<ITransactionPayloadCreator>().Object,
                new Mock<IWellEnableDisableService>().Object,
                null,
                new Mock<IThetaLoggerFactory>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNulLoggingFactoryTest()
        {
            _ = new WellControlController(
                new Mock<IProcessingDataUpdatesService>().Object,
                new Mock<ITransactionPayloadCreator>().Object,
                new Mock<IWellEnableDisableService>().Object,
                new Mock<IWellControlService>().Object,
                null);
        }

        [TestMethod]
        public void GetWellEnableDisableByAssetIdTest()
        {
            var mockService = new Mock<IProcessingDataUpdatesService>();
            var mockPayloadCreator = new Mock<ITransactionPayloadCreator>();
            var mockWellService = new Mock<IWellEnableDisableService>();
            var mockWellControlService = new Mock<IWellControlService>();
            var mockLogger = new Mock<IThetaLogger>();

            var mockLoggingFactory = new Mock<IThetaLoggerFactory>();
            mockLoggingFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(mockLogger.Object);
            
            var controller = new WellControlController(mockService.Object, mockPayloadCreator.Object,
                mockWellService.Object, mockWellControlService.Object, mockLoggingFactory.Object);

            EnableDisableWellRequest putWellRequest = new EnableDisableWellRequest()
            {
                AssetId = "b27d9e0e-7c9f-4a0b-bd42-da0aefdd8264",
                Enabled = "-1",
                DataCollection = "-1",
                DisableCode = "-1",
                SocketId = "socketIdTest",
            };

            var result = controller.WellEnableDisable(putWellRequest);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(AcceptedResult));
        }

        [TestMethod]
        public void GetWellEnableDisableByFilterNullTest()
        {
            var mockService = new Mock<IProcessingDataUpdatesService>();
            var mockWellService = new Mock<IWellEnableDisableService>();
            var mockPayloadCreator = new Mock<ITransactionPayloadCreator>();
            var mockWellControlService = new Mock<IWellControlService>();
            var mockLogger = new Mock<IThetaLogger>();

            var mockLoggingFactory = new Mock<IThetaLoggerFactory>();
            mockLoggingFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(mockLogger.Object);

            var controller = new WellControlController(mockService.Object, mockPayloadCreator.Object,
                mockWellService.Object, mockWellControlService.Object, mockLoggingFactory.Object);

            var result = controller.WellEnableDisable(null);

            Assert.AreEqual(400, ((StatusCodeResult)result).StatusCode);
        }

        #endregion

    }
}
