using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Contracts.Responses;
using Theta.XSPOC.Apex.Api.Controllers;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Tests.Controllers
{
    [TestClass]
    public class NodeMasterColumnsControllerTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLoggerTest()
        {
            _ = new NodeMasterColumnsController(null, null);
        }

        [TestMethod]
        public void GetNodeMasterColumnTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<INodeMasterEndpointProcessingService>();
            mockService.Setup(x => x.GetNodeMasterColumnData(It.IsAny<WithCorrelationId<NodeMasterColumnsInput>>()))
                .Returns(new NodeMasterSelectedColumnsOutput
                {
                    Result = new MethodResult<string>(true, string.Empty),
                    Data = new Dictionary<string, string>
                    {
                        {
                            "Node","123243"
                        },
                        {
                            "NodeId","Well1"
                        }
                    }
                });

            var controller = new NodeMasterColumnsController(mockService.Object, mockThetaLoggerFactory.Object);

            var result = controller.GetNodeMasterData("DFC1D0AD-A824-4965-B78D-AB7755E32DD3", new string[2]
            {
                "Node", "NodeId"
            });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
            mockService.Verify(x => x.GetNodeMasterColumnData(It.IsAny<WithCorrelationId<NodeMasterColumnsInput>>()), Times.Once);
            Assert.AreEqual(2, ((NodeMasterSelectedColumnResponse)((OkObjectResult)result).Value).Values.Data.Count);
        }

        [TestMethod]
        public void GetNodeMasterColumnMissingFiltersTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<INodeMasterEndpointProcessingService>();

            var controller = new NodeMasterColumnsController(mockService.Object, mockThetaLoggerFactory.Object);

            var result = controller.GetNodeMasterData(null, null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void GetNodeMasterColumnInvalidAssetIdTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<INodeMasterEndpointProcessingService>();

            var controller = new NodeMasterColumnsController(mockService.Object, mockThetaLoggerFactory.Object);

            var result = controller.GetNodeMasterData("abc", new string[2]
            {
                "Node", "NodeId"
            });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void GetNodeMasterColumnServiceResultNullTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<INodeMasterEndpointProcessingService>();
            mockService.Setup(x => x.GetNodeMasterColumnData(It.IsAny<WithCorrelationId<NodeMasterColumnsInput>>()))
                .Returns((NodeMasterSelectedColumnsOutput)null);

            var controller = new NodeMasterColumnsController(mockService.Object, mockThetaLoggerFactory.Object);

            var result = controller.GetNodeMasterData("DFC1D0AD-A824-4965-B78D-AB7755E32DD3", new string[2]
            {
                "Node", "NodeId"
            });

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
        }

        #endregion

    }
}
