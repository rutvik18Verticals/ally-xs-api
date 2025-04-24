using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Theta.XSPOC.Apex.Api.Common;
using Theta.XSPOC.Apex.Api.Contracts.Mappers;
using Theta.XSPOC.Apex.Api.Controllers;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Tests.Controllers
{
    [TestClass]
    public class AssetsControllerTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullGroupAndAssetServiceTest()
        {
            _ = new AssetsController(null, null);
        }

        [TestMethod]
        public void GetGroupAndAssetTest()
        {
            GroupAndAssetDataOutput groupAndAssetDataOutput = new GroupAndAssetDataOutput
            {
                Result = new MethodResult<string>(true, string.Empty),
                Values = new GroupAndAssetModel()
                {
                    GroupName = "Asset Group 1",
                    Assets = new List<AssetModel>()
                    {
                        new()
                        {
                            AssetName = "Asset 1",
                            AssetId = Guid.NewGuid(),
                        }
                    }
                }
            };

            var mockService = new Mock<IGroupAndAssetService>();
            mockService.Setup(x => x.GetGroupAndAssetData(It.IsAny<string>(), It.IsAny<string>(), false))
                .Returns(groupAndAssetDataOutput);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new AssetsController(mockService.Object, mockThetaLoggerFactory.Object);

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("UserEmailId", "testuser@championx.com"),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "testuser")
            }));

            string[] groupBy = new string[]
            {
                "AssetGroupName"
            };

            var result = controller.Get(groupBy);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
            mockService.Verify(x => x.GetGroupAndAssetData("testuser", It.IsAny<string>(), false), Times.Once);
        }

        [TestMethod]
        public void GetGroupAndAssetWhenAssetGroupnameisNullTest()
        {
            GroupAndAssetDataOutput groupAndAssetDataOutput = new GroupAndAssetDataOutput
            {
                Values = new GroupAndAssetModel()
                {
                    GroupName = "Asset Group 1",
                    Assets = new List<AssetModel>()
                    {
                        new AssetModel()
                        {
                            AssetName = "Asset 1",
                            AssetId = Guid.NewGuid(),
                        }
                    }
                }
            };

            var mockService = new Mock<IGroupAndAssetService>();
            mockService.Setup(x => x.GetGroupAndAssetData(It.IsAny<string>(), It.IsAny<string>(), false))
                .Returns(groupAndAssetDataOutput);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new AssetsController(mockService.Object, mockThetaLoggerFactory.Object);

            string[] groupBy = new string[]
            {
                ""
            };

            var result = controller.Get(groupBy);

            Assert.AreEqual(400, ((BadRequestResult)result).StatusCode);
        }

        [TestMethod]
        public void MapWithValidInputReturnsGroupAndAssetResponse()
        {
            var coreModel = new GroupAndAssetDataOutput
            {
                Values = new GroupAndAssetModel()
                {
                    GroupName = "Group1",
                    Assets = new List<AssetModel>
                    {
                        new AssetModel
                        {
                            AssetId = new Guid(),
                            AssetName = "Asset1"
                        },
                        new AssetModel
                        {
                            AssetId = new Guid(),
                            AssetName = "Asset2"
                        }
                    }
                }
            };

            var assetsInput = new AssetsInput()
            {
                AssetGroup = "AssetGroupName"
            };

            var correlationId = Guid.NewGuid().ToString();
            var requestWithCorrelationId = new WithCorrelationId<AssetsInput>
                (correlationId, assetsInput);

            var result = GroupAndAssetDataMapper.Map(coreModel, requestWithCorrelationId.CorrelationId);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Values);
            Assert.AreEqual(1, result.Values.Count);
            var groupAndAssetData = result.Values[0];
            Assert.AreEqual("Group1", groupAndAssetData.GroupName);
            Assert.IsNotNull(groupAndAssetData.Assets);
            Assert.AreEqual(2, groupAndAssetData.Assets.Count);
        }

        [TestMethod]
        public void GetGroupAndAssetMissingGroupByTest()
        {
            var mockService = new Mock<IGroupAndAssetService>();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new AssetsController(mockService.Object, mockThetaLoggerFactory.Object);

            var result = controller.Get(null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void GetGroupAndAssetUnAuthorizedTest()
        {
            var mockService = new Mock<IGroupAndAssetService>();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new AssetsController(mockService.Object, mockThetaLoggerFactory.Object);

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("UserEmailId", "testuser@championx.com"),
            }));

            string[] groupBy = new string[]
            {
                "AssetGroupName"
            };

            var result = controller.Get(groupBy);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public void GetGroupAndAssetMissingAssetGroupNameTest()
        {
            var mockService = new Mock<IGroupAndAssetService>();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new AssetsController(mockService.Object, mockThetaLoggerFactory.Object);

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("UserEmailId", "testuser@championx.com"),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "testuser")
            }));

            string[] groupBy = new string[]
            {
                "abc"
            };

            var result = controller.Get(groupBy);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void GetGroupAndAssetServiceResultNullTest()
        {
            var mockService = new Mock<IGroupAndAssetService>();
            mockService.Setup(x => x.GetGroupAndAssetData(It.IsAny<string>(), It.IsAny<string>(), false))
                .Returns((GroupAndAssetDataOutput)null);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new AssetsController(mockService.Object, mockThetaLoggerFactory.Object);

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("UserEmailId", "testuser@championx.com"),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "testuser")
            }));

            string[] groupBy = new string[]
            {
                "AssetGroupName"
            };

            var result = controller.Get(groupBy);

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
        }

        #endregion

    }
}
