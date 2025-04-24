using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Core.Tests.Services
{
    [TestClass]
    public class NodeMasterColumnDataProcessingServiceTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLoggerServiceTest()
        {
            var mockService = new Mock<INodeMaster>();

            _ = new NodeMasterEndpointProcessingService(mockService.Object, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullNodeMasterServiceTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockDateTimeConverter = new Mock<IDateTimeConverter>();

            _ = new NotificationProcessingService(null, mockThetaLoggerFactory.Object, mockDateTimeConverter.Object);
        }

        [TestMethod]
        public void GetNodeMasterColumnDataTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var nodeData = new NodeMasterDictionary()
            {
                Data = new Dictionary<string, string>
                {
                    { "AssetGuid", "AA50D05A-FCB5-4C8E-859B-3C4AF11A7BCA" },
                    { "NodeId", "Well1" },
                    { "Node", "12345" }
                }
            };

            var mockNotificationService = new Mock<INodeMaster>();

            Guid guid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            mockNotificationService.Setup(x => x.GetNodeMasterData(guid,
                new string[2] { "Node", "NodeId" }, It.IsAny<string>()))
                .Returns(nodeData);

            var service = new NodeMasterEndpointProcessingService(mockNotificationService.Object, mockThetaLoggerFactory.Object);
            var input = new NodeMasterColumnsInput()
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                Columns = new string[2] { "Node", "NodeId" }
            };

            var message = new WithCorrelationId<NodeMasterColumnsInput>("CorrelationId", input);
            var result = service.GetNodeMasterColumnData(message);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NodeMasterSelectedColumnsOutput));
            mockNotificationService.Verify(x => x.GetNodeMasterData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                new string[2] { "Node", "NodeId" }, It.IsAny<string>()), Times.Once);
            Assert.AreEqual(3, (result).Data.Count);
        }

        #endregion

    }
}