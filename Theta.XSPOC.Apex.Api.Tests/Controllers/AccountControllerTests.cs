using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using Theta.XSPOC.Apex.Api.Contracts.JWTToken;
using Theta.XSPOC.Apex.Api.Controllers;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Configuration;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.JWTToken;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models.Identity;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLoggerTest()
        {
            _ = new AccountController(null, null, null, null, null, null);
        }

        [TestMethod]
        public void GetFormloginAccessTokenTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IAdminToolsService>();

            var appUser = new AppUser();
            appUser.UserName = "Test";
            appUser.PasswordHash = "AssetId2";

            mockService.Setup(x => x.FindByName(It.IsAny<WithCorrelationId<FormLoginInput>>()))
                .Returns(appUser);

            mockService.Setup(x => x.GetJwtRefresh(It.IsAny<WithCorrelationId<string>>()))
                .Returns("accesstoken");

            mockService.Setup(x => x.GetJwt(It.IsAny<WithCorrelationId<AppUser>>()))
              .Returns("accesstoken");

            var mockAuthService = new Mock<IAuthService>();
            var mockHttpClient = new Mock<HttpClient>();
            var mockOptionsSnapshot = new Mock<IOptionsSnapshot<AppSettings>>();
            var mockTokenValidation = new Mock<ITokenValidation>();
            var appSettings = new AppSettings { AllyConnectApiURL = "http://localhost:5240" };
            mockOptionsSnapshot.Setup(m => m.Value).Returns(appSettings);

            var httpContext = new DefaultHttpContext();

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var controller = new AccountController(mockService.Object, mockThetaLoggerFactory.Object, mockAuthService.Object,
                mockHttpClient.Object, mockOptionsSnapshot.Object, mockTokenValidation.Object)
            {
                ControllerContext = controllerContext,
            };

            var user = new User
            {
                UserName = "TestUser",
                PasswordHash = "test",
                GrantType = "form"
            };

            var result = controller.Login(user);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
            mockService.Verify(x => x.FindByName(It.IsAny<WithCorrelationId<FormLoginInput>>()), Times.Once);
            Assert.AreEqual("accesstoken", ((JWTAccessTokenCloud)((OkObjectResult)result).Value).AccessToken);
        }

        [TestMethod]
        public void GetRefreshTokenTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IAdminToolsService>();

            var appUser = new AppUser();

            mockService.Setup(x => x.GetJwtRefresh(It.IsAny<WithCorrelationId<string>>()))
                .Returns("accesstoken");

            mockService.Setup(x => x.GetJwt(It.IsAny<WithCorrelationId<AppUser>>()))
                .Returns("accesstoken");
            mockService.Setup(x => x.ValidateToken(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(SetUpClaims());

            var user = new User
            {
                UserName = "TestUser",
                PasswordHash = "",
                GrantType = "refresh_token",
                RefreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJVc2VyRW1haWxJZCI6IkNzdGFyciIsInN1YiI6IkNzdGFyciIsImp0aSI6IkNzdG" +
                "FyciIsImlhdCI6IjA0LTEwLTIwMjQgMDk6NTE6MDAiLCJXZWxsQ29udHJvbCI6IlRydWUiLCJXZWxsQ29uZmlnIjoiVHJ1ZSIsIkFkbWluIjoiVHJ1ZSIsIk" +
                "FkbWluTGl0ZSI6IlRydWUiLCJXZWxsQWRtaW4iOiJUcnVlIiwiV2VsbENvbmZpZ0xpdGUiOiJUcnVlIiwibmJmIjoxNzEyNzQyNjYwLCJleHAiOjMzMjQ4N" +
                "zQyNjYwLCJpc3MiOiJYU1BPQy5Db25uZXhpYS5hcGkiLCJhdWQiOiJDaGFtcGlvbnguQ29ubmV4aWEuWFNQT0MifQ.HLQwAjT-ncqNCuJJs83wYCCHN7L8" +
                "ildojNgbjJdYmLA"
            };

            var identity = new GenericIdentity("UserEmailId", "test");
            var contextUser = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new Claim[]
                            {
                                new("UserEmailId", "test"),
                                new("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "test")
                            },
                            "Basic")
                        );

            var httpContext = new DefaultHttpContext()
            {
                User = contextUser,
            };

            httpContext.Request.Headers["Cookie"] = $"Ally-Authorization={user.RefreshToken}";

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            var mockAuthService = new Mock<IAuthService>();
            var mockHttpClient = new Mock<HttpClient>();
            var mockOptionsSnapshot = new Mock<IOptionsSnapshot<AppSettings>>();
            var mockTokenValidation = new Mock<ITokenValidation>();
            var appSettings = new AppSettings { AllyConnectApiURL = "http://localhost:5240" };
            mockOptionsSnapshot.Setup(m => m.Value).Returns(appSettings);

            var controller = new AccountController(mockService.Object, mockThetaLoggerFactory.Object, mockAuthService.Object,
                mockHttpClient.Object, mockOptionsSnapshot.Object, mockTokenValidation.Object)
            {
                ControllerContext = controllerContext,
            };
            var result = controller.RefreshToken();

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
            mockService.Verify(x => x.GetJwtRefresh(It.Is<WithCorrelationId<string>>(c => c.CorrelationId != null && c.Value == "test@test.com")), Times.Once);
            Assert.AreEqual("accesstoken", ((JWTAccessToken)((OkObjectResult)result).Value).RefreshToken);
        }

        [TestMethod]
        public void GetFormloginUserNameNullTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IAdminToolsService>();

            var appUser = new AppUser();
            appUser.UserName = "Test";
            appUser.PasswordHash = "AssetId2";

            var mockAuthService = new Mock<IAuthService>();
            var mockHttpClient = new Mock<HttpClient>();
            var mockOptionsSnapshot = new Mock<IOptionsSnapshot<AppSettings>>();
            var mockTokenValidation = new Mock<ITokenValidation>();
            var appSettings = new AppSettings { AllyConnectApiURL = "http://localhost:5240" };
            mockOptionsSnapshot.Setup(m => m.Value).Returns(appSettings);

            var controller = new AccountController(mockService.Object, mockThetaLoggerFactory.Object, mockAuthService.Object,
                mockHttpClient.Object, mockOptionsSnapshot.Object, mockTokenValidation.Object);

            var user = new User
            {
                UserName = "",
                PasswordHash = "",
                GrantType = "form"
            };

            var result = controller.Login(user);

            Assert.IsNotNull(result);
            Assert.AreEqual(400, ((ObjectResult)result).StatusCode);
        }

        #endregion

        #region Private Methods

        private ClaimsPrincipal SetUpClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "test@test.com"),  
                new Claim(ClaimTypes.Name, "testuser"),        
                new Claim(ClaimTypes.Role, "Admin")           
            };

            var identity = new ClaimsIdentity(claims, "TestAuthType");
            return new ClaimsPrincipal(identity);
        }

        #endregion

    }
}
