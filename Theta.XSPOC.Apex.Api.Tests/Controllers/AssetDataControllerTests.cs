using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Theta.XSPOC.Apex.Api.Controllers;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Tests.Controllers
{
    [TestClass]
    public class AssetDataControllerTests
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullAssetDataServiceTest()
        {
            _ = new AssetDataController(null, null);
        }

        [TestMethod]
        public void GetEnabledStatusTest()
        {
            var service = new Mock<IAssetDataService>();
            service.Setup(x => x.GetEnabledStatus(It.IsAny<WithCorrelationId<Guid>>())).Returns(new WellEnabledStatusOutput()
            {
                Result = new Kernel.Collaboration.Models.MethodResult<string>(true, string.Empty),
                Enabled = true,
            });

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new AssetDataController(service.Object, mockThetaLoggerFactory.Object);

            var result = controller.GetEnabledStatus(Guid.NewGuid());

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void GetEnabledStatusInvalidGuidTest()
        {
            var service = new Mock<IAssetDataService>();
            service.Setup(x => x.GetEnabledStatus(It.IsAny<WithCorrelationId<Guid>>())).Returns(new WellEnabledStatusOutput()
            {
                Result = new Kernel.Collaboration.Models.MethodResult<string>(true, string.Empty),
                Enabled = true,
            });

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new AssetDataController(service.Object, mockThetaLoggerFactory.Object);

            var result = controller.GetEnabledStatus(Guid.Empty);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void GetEnabledStatusServiceResultNullTest()
        {
            var service = new Mock<IAssetDataService>();
            service.Setup(x => x.GetEnabledStatus(It.IsAny<WithCorrelationId<Guid>>())).Returns((WellEnabledStatusOutput)null);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new AssetDataController(service.Object, mockThetaLoggerFactory.Object);

            var result = controller.GetEnabledStatus(Guid.NewGuid());

            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
        }

    }
}
