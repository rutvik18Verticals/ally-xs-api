using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Controllers;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.Core.Services.DashboardService;
using Theta.XSPOC.Apex.Api.Core.Services.UserAccountService;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Tests.Controllers
{
    [TestClass]
    public class DashboardControllerTests
    {
        private DashboardController controller;
        private Mock<IDashboardWidgetService> mockService;

        [TestInitialize]
        public void Setup()
        {
            // Setup code can be added here if needed
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            mockService = new Mock<IDashboardWidgetService>();
            mockService.Setup(x => x.SaveWidgetUserPreferences(It.IsAny<DashboardWidgetUserPreferencesInput>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            mockService.Setup(x => x.SaveWidgetUserPreferences(null, It.IsAny<string>(), It.IsAny<string>()))
                .Callback(() => { throw new ArgumentNullException(); });

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);
            var userProvider = new LoggedInUserProvider(mockHttpContextAccessor.Object);

            controller = new DashboardController(mockThetaLoggerFactory.Object, mockService.Object, userProvider);

            controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext();
            controller.ControllerContext.HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("UserEmailId", "testuser@championx.com"),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "testuser"),
                new Claim("UserObjectId", "67e1b77cca0b04f26987a123"),
            }));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullTest()
        {
            _ = new DashboardController(null, null, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SaveUserPreferences_ServiceMethodThrowsErrorTest()
        {
            _ = controller.SaveUserPreferences(null);
        }

        [TestMethod]
        public void SaveUserPreferences_UnauthorizedUserTest()
        {
            controller.ControllerContext.HttpContext.User = null;

            var result = controller.SaveUserPreferences(null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(401, ((StatusCodeResult)result).StatusCode);
        }

        [TestMethod]
        public void SaveUserPreferences_SuccessTest()
        {
            var result = controller.SaveUserPreferences(new DashboardWidgetUserPreferencesInput()
            {
                DashboardName = "esp-well-charts",
                WidgetName = "wellTestTable",
                PropertyType = "Columns",
                Preferences = "["
                + "{'Order' : 23,'Label' : 'H2O Dens (s.g)','Key' : 'WaterDensity','Selected' : true}"
                + "]"
            });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkResult));
            Assert.AreEqual(200, ((OkResult)result).StatusCode);
            mockService.Verify(x => x.SaveWidgetUserPreferences(
                It.IsAny<DashboardWidgetUserPreferencesInput>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
