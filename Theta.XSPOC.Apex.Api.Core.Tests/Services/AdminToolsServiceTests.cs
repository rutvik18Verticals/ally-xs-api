using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models.Identity;

namespace Theta.XSPOC.Apex.Api.Core.Tests.Services
{
    [TestClass]
    public class AdminToolsServiceTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLoggerServiceTest()
        {
            var mockAuthService = new Mock<IAuthService>();
            var mockUserService = new Mock<IUserService>();

            _ = new AdminToolsService(mockAuthService.Object, mockUserService.Object, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullAuthServiceTest()
        {
            var mockAuthService = new Mock<IAuthService>();
            var mockUserService = new Mock<IUserService>();
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            _ = new AdminToolsService(null, mockUserService.Object, mockThetaLoggerFactory.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullUserServiceTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockAuthService = new Mock<IAuthService>();

            _ = new AdminToolsService(mockAuthService.Object, null, mockThetaLoggerFactory.Object);
        }

        [TestMethod]
        public void GetFindByNameTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var correlationId = Guid.NewGuid().ToString();

            var mockUserService = new Mock<IUserService>();

            var events = new AppUser()
            {
                UserName = "test",
                PasswordHash = "CXerPCaspVFfSdbRP2ydI54qVGWs4OuQHjHzxFkVkFYNnRug",
            };

            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(x => x.FindByNameAsync("test", "test", correlationId))
                .Returns(events);

            var service = new AdminToolsService(mockAuthService.Object, mockUserService.Object, mockThetaLoggerFactory.Object);
            var payload = new FormLoginInput
            {
                UserName = "test",
                Password = "test",
            };

            var message = new WithCorrelationId<FormLoginInput>(correlationId, payload);
            var result = service.FindByName(message);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(AppUser));
            mockAuthService.Verify(x => x.FindByNameAsync("test", "test", correlationId), Times.Once);
        }

        [TestMethod]
        public void GetInvalidPasswordTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var correlationId = Guid.NewGuid().ToString();

            var mockUserService = new Mock<IUserService>();

            var events = new AppUser()
            {
                UserName = "test",
                PasswordHash = "CXerPCaspfSdbRP2ydI54qVGWs4OuQHjHzxFkVkFYNnRug",
            };

            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(x => x.FindByNameAsync("test", "test", "CorrelationId"))
                .Returns(events);

            var service = new AdminToolsService(mockAuthService.Object, mockUserService.Object, mockThetaLoggerFactory.Object);
            var payload = new FormLoginInput
            {
                UserName = "test",
                Password = "test",
            };

            var message = new WithCorrelationId<FormLoginInput>("CorrelationId", payload);
            var result = service.FindByName(message);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetUserNotExistTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var correlationId = Guid.NewGuid().ToString();

            var mockUserService = new Mock<IUserService>();

            var events = new AppUser()
            {
                UserName = "test",
                PasswordHash = "CXerPCaspfSdbRP2ydI54qVGWs4OuQHjHzxFkVkFYNnRug",
            };

            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(x => x.FindByNameAsync("test", "test", "CorrelationId"))
                .Returns(events);

            var service = new AdminToolsService(mockAuthService.Object, mockUserService.Object, mockThetaLoggerFactory.Object);
            var payload = new FormLoginInput
            {
                UserName = "tests",
                Password = "test",
            };

            var message = new WithCorrelationId<FormLoginInput>("CorrelationId", payload);
            var result = service.FindByName(message);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetUserNameNullTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var correlationId = Guid.NewGuid().ToString();

            var mockUserService = new Mock<IUserService>();

            var events = new AppUser()
            {
                UserName = "test",
                PasswordHash = "CXerPCaspfSdbRP2ydI54qVGWs4OuQHjHzxFkVkFYNnRug",
            };

            var mockAuthService = new Mock<IAuthService>();
            mockAuthService.Setup(x => x.FindByNameAsync("test", "test", "CorrelationId"))
                .Returns(events);

            var service = new AdminToolsService(mockAuthService.Object, mockUserService.Object, mockThetaLoggerFactory.Object);
            var payload = new FormLoginInput
            {
                UserName = "",
                Password = "",
            };

            var message = new WithCorrelationId<FormLoginInput>("CorrelationId", payload);
            var result = service.FindByName(message);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetJwtRefreshTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockUserService = new Mock<IUserService>();
            var mockAuthService = new Mock<IAuthService>();

            var service = new AdminToolsService(mockAuthService.Object, mockUserService.Object, mockThetaLoggerFactory.Object);
            AdminToolsService.TimeOut = 10;

            AdminToolsService.AudienceSecret = "$^ujVGXDtAb^L3422j";
            var result = service.GetJwtRefresh(new WithCorrelationId<string>(string.Empty, "test"));

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
        }

        [TestMethod]
        public void GetJwtTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockUserService = new Mock<IUserService>();
            var mockAuthService = new Mock<IAuthService>();

            var service = new AdminToolsService(mockAuthService.Object, mockUserService.Object, mockThetaLoggerFactory.Object);
            AdminToolsService.TimeOut = 10;
            AdminToolsService.AudienceSecret = "$^ujVGXDtAb^L3422j";
            AppUser appUser = new AppUser();
            appUser.UserName = "test";
            appUser.Email = "test@123";
            appUser.WellConfig = true;
            appUser.WellControl = true;
            appUser.Admin = true;
            appUser.AdminLite = true;
            appUser.WellAdmin = true;
            appUser.WellConfigLite = true;

            var result = service.GetJwt(new WithCorrelationId<AppUser>(string.Empty, appUser));

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(string));
        }

        #endregion

    }
}