using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Theta.XSPOC.Apex.Api.Controllers;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Tests.Controllers
{

    #region Test Methods

    [TestClass]
    public class GroupStatusViewsControllerTest
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullAvailableViewProcessingServiceTest()
        {
            _ = new GroupStatusViewsController(null, null);
        }

        [TestMethod]
        public void GetAvailableViewResultsTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var values = new List<AvailableViewData>()
            {
                new AvailableViewData()
                {
                    ViewId = 0,
                    ViewName = "",
                },
                new AvailableViewData()
                {
                    ViewId = 1,
                    ViewName = "",
                },
            };

            var methodResult = new MethodResult<string>(true, string.Empty);

            var responseData = new AvailableViewOutput
            {
                Values = values,
                Result = methodResult
            };

            var mockService = new Mock<IGroupStatusProcessingService>();
            mockService.Setup(x => x.GetAvailableViews(It.IsAny<WithCorrelationId<AvailableViewInput>>()))
                .Returns(responseData);

            var controller = new GroupStatusViewsController(mockService.Object, mockThetaLoggerFactory.Object);

            controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext();
            controller.ControllerContext.HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("UserEmailId", "testuser@championx.com"),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "testuser")
            }));

            var result = controller.Get();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
            mockService.Verify(x => x.GetAvailableViews(
                It.IsAny<WithCorrelationId<AvailableViewInput>>()), Times.Once);
        }

        [TestMethod]
        public void GetAvailableViewResultsEmptyPayloadTest()
        {
            AvailableViewOutput responseData = null;

            var mockService = new Mock<IGroupStatusProcessingService>();
            mockService.Setup(x => x.GetAvailableViews(It.IsAny<WithCorrelationId<AvailableViewInput>>()))
                .Returns(responseData);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new GroupStatusViewsController(mockService.Object, mockThetaLoggerFactory.Object);

            controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext();
            controller.ControllerContext.HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("UserEmailId", "testuser@championx.com"),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "testuser")
            }));

            var result = controller.Get();

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
        }

        [TestMethod]
        public void GetAvailableViewNullResponseTest()
        {
            AvailableViewOutput responseData = null;

            var mockService = new Mock<IGroupStatusProcessingService>();
            mockService.Setup(x => x.GetAvailableViews(It.IsAny<WithCorrelationId<AvailableViewInput>>()))
                .Returns(responseData);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new GroupStatusViewsController(mockService.Object, mockThetaLoggerFactory.Object);

            controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext();
            controller.ControllerContext.HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("UserEmailId", "testuser@championx.com"),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "testuser")
            }));

            var result = controller.Get();

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(x => x.GetAvailableViews(It.IsAny<WithCorrelationId<AvailableViewInput>>()),
                Times.Once);
        }

        [TestMethod]
        public void GetAvailableViewInvalidUserTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var values = new List<AvailableViewData>()
            {
                new AvailableViewData()
                {
                    ViewId = 0,
                    ViewName = "",
                },
                new AvailableViewData()
                {
                    ViewId = 1,
                    ViewName = "",
                },
            };

            var methodResult = new MethodResult<string>(true, string.Empty);

            var responseData = new AvailableViewOutput
            {
                Values = values,
                Result = methodResult
            };

            var mockService = new Mock<IGroupStatusProcessingService>();
            mockService.Setup(x => x.GetAvailableViews(It.IsAny<WithCorrelationId<AvailableViewInput>>()))
                .Returns(responseData);

            var controller = new GroupStatusViewsController(mockService.Object, mockThetaLoggerFactory.Object);

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("UserEmailId", "testuser@championx.com"),
            }));

            var result = controller.Get();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
        }

        #endregion

    }
}
