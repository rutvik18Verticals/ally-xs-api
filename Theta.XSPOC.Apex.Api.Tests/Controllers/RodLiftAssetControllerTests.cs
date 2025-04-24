using Amazon.Runtime.Internal.Transform;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Contracts.Responses.AssetStatus;
using Theta.XSPOC.Apex.Api.Controllers;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs.AssetStatus;
using Theta.XSPOC.Apex.Api.Core.Services.AssetStatus;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Tests.Controllers
{
    [TestClass]
    public class RodLiftAssetControllerTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullAssetStatusServiceTest()
        {
            _ = new AssetStatusController(null, null);
        }

        [TestMethod]
        public async Task GetAssetIdDefaultGUIDTest()
        {

            var mockService = new Mock<IAssetStatusService>();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new AssetStatusController(mockService.Object, mockThetaLoggerFactory.Object);

            var filters = new Dictionary<string, string>();
            filters.Add("assetId", Guid.Empty.ToString());

            var result = await controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            Assert.AreEqual(400, ((BadRequestResult)result).StatusCode);
            mockService.Verify(
                x => x.GetAssetStatusDataAsync(It.IsAny<WithCorrelationId<AssetStatusInput>>()),
                Times.Never);
        }

        [TestMethod]
        public async Task GetServiceReturnsNullTest()
        {

            var mockService = new Mock<IAssetStatusService>();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new AssetStatusController(mockService.Object, mockThetaLoggerFactory.Object);
            var identity = new GenericIdentity("UserEmailId", "test");
            var contextUser = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new Claim[]
                            {
                                new Claim("UserEmailId", "test"),
                                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "test")
                            },
                            "Basic")
                        );

            var httpContext = new DefaultHttpContext()
            {
                User = contextUser
            };

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            controller.ControllerContext = controllerContext;
            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            var result = await controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(
                x => x.GetAssetStatusDataAsync(
                    It.Is<WithCorrelationId<AssetStatusInput>>(m => m.Value.AssetId == new Guid(filters["assetId"]))), Times.Once);
        }

        [TestMethod]
        public async Task GetServiceReturnsOKTest()
        {
            var assetId = Guid.NewGuid();
            var correlationId = Guid.NewGuid().ToString();

            var task = new Task<WithCorrelationId<RodLiftAssetStatusDataOutput>>(() =>
                new WithCorrelationId<RodLiftAssetStatusDataOutput>(correlationId, new RodLiftAssetStatusDataOutput()
                {
                    ImageOverlayItems = new List<OverlayStatusDataOutput>()
                    {
                        new OverlayStatusDataOutput()
                        {
                            OverlayField = Core.Models.OverlayFields.ApiDesignation,
                            Value = "Test Node",
                            Label = "API Designation"
                        }
                    },
                }));

            task.Start();
            var mockService = new Mock<IAssetStatusService>();
            mockService.Setup(m =>
                m.GetAssetStatusDataAsync(
                    It.Is<WithCorrelationId<AssetStatusInput>>(x => x.Value.AssetId == assetId))).Returns(task);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new AssetStatusController(mockService.Object, mockThetaLoggerFactory.Object);
            var identity = new GenericIdentity("UserEmailId", "test");
            var contextUser = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new Claim[]
                            {
                                new Claim("UserEmailId", "test"),
                                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "test")
                            },
                            "Basic")
                        );

            var httpContext = new DefaultHttpContext()
            {
                User = contextUser
            };

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };
            controller.ControllerContext = controllerContext;
            var filters = new Dictionary<string, string>();
            filters.Add("assetId", assetId.ToString());
            var result = await controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
            Assert.AreEqual("API Designation",
                ((AssetStatusDataResponse)((OkObjectResult)result).Value).Values.ImageOverlayItems
                .FirstOrDefault().Label);
            Assert.AreEqual("Test Node",
                ((AssetStatusDataResponse)((OkObjectResult)result).Value).Values.ImageOverlayItems
                .FirstOrDefault().Value);
            mockService.Verify(
                x => x.GetAssetStatusDataAsync(
                    It.Is<WithCorrelationId<AssetStatusInput>>(m => m.Value.AssetId == assetId)), Times.Once);

        }

        #endregion

    }
}