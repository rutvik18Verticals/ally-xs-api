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
    public class NotificationTypesControllerTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLoggerTest()
        {
            var mockService = new Mock<INotificationProcessingService>();

            _ = new NotificationTypesController(null, mockService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullNotificationTypesProcessingServiceTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            _ = new NotificationTypesController(mockThetaLoggerFactory.Object, null);
        }

        [TestMethod]
        public void GetNotificationsTypesByAssetIdTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var events = new List<NotificationTypesData>()
            {
                new NotificationTypesData()
                {
                    Id = "104",
                    Name = "AssetId2",
                }
            };

            var mockService = new Mock<INotificationProcessingService>();
            mockService.Setup(x => x.GetNotificationsTypes(It.IsAny<WithCorrelationId<NotificationsInput>>()))
                .Returns(new NotificationTypesDataOutput
                {
                    Result = new MethodResult<string>(true, string.Empty),
                    Values = events
                });

            var controller = new NotificationTypesController(mockThetaLoggerFactory.Object, mockService.Object);

            var asset = new Dictionary<string, string>
            {
                {
                    "id", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
                },
                {
                    "type", "asset"
                }
            };

            var result = controller.Get(asset);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
            mockService.Verify(x => x.GetNotificationsTypes(It.IsAny<WithCorrelationId<NotificationsInput>>()), Times.Once);
            Assert.AreEqual(1, ((NotificationTypesResponse)((OkObjectResult)result).Value).Values.Count);
        }

        [TestMethod]
        public void GetNotificationsTypesByAssetIdWhenfilterisNullTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var events = new List<NotificationTypesData>()
            {
                new NotificationTypesData()
                {
                    Id = "104",
                    Name = "AssetId2",
                }
            };

            var mockService = new Mock<INotificationProcessingService>();
            mockService.Setup(x => x.GetNotificationsTypes(It.IsAny<WithCorrelationId<NotificationsInput>>()))
                .Returns(new NotificationTypesDataOutput
                {
                    Result = new MethodResult<string>(true, string.Empty),
                    Values = events
                });

            var controller = new NotificationTypesController(mockThetaLoggerFactory.Object, mockService.Object);

            IDictionary<string, string> asset = null;

            var result = controller.Get(asset);

            Assert.AreEqual(400, ((StatusCodeResult)result).StatusCode);
        }

        [TestMethod]
        public void GetNotificationsTypesByAssetGroupNameAndNotificationTypeIdTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var events = new List<NotificationTypesData>()
            {
                new NotificationTypesData()
                {
                    Id = "104",
                    Name = "AssetId2",
                },
                new NotificationTypesData()
                {
                    Id = "105",
                    Name = "AssetId2",
                },
            };

            var mockService = new Mock<INotificationProcessingService>();
            mockService.Setup(x => x.GetNotificationsTypes(It.IsAny<WithCorrelationId<NotificationsInput>>()))
                .Returns(new NotificationTypesDataOutput
                {
                    Result = new MethodResult<string>(true, string.Empty),
                    Values = events
                });

            var controller = new NotificationTypesController(mockThetaLoggerFactory.Object, mockService.Object);

            var asset = new Dictionary<string, string>
            {
                {
                    "id", "66FC0809-091C-4A65-94FA-0394C3BEE1FB"
                },
                {
                    "type", "group"
                }
            };

            var result = controller.Get(asset);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
            mockService.Verify(x => x.GetNotificationsTypes(It.IsAny<WithCorrelationId<NotificationsInput>>()), Times.Once);
            Assert.AreEqual(2, ((NotificationTypesResponse)((OkObjectResult)result).Value).Values.Count);
        }

        [TestMethod]
        public void GetNotificationsTypesNullResponseTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            NotificationTypesDataOutput output = null;
            var mockService = new Mock<INotificationProcessingService>();
            mockService.Setup(x => x.GetNotificationsTypes(It.IsAny<WithCorrelationId<NotificationsInput>>()))
                .Returns(output);

            var controller = new NotificationTypesController(mockThetaLoggerFactory.Object, mockService.Object);

            var asset = new Dictionary<string, string>
            {
                {
                    "id", "66FC0809-091C-4A65-94FA-0394C3BEE1FB"
                },
                {
                    "type", "asset"
                }
            };

            var result = controller.Get(asset);

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(x => x.GetNotificationsTypes(It.IsAny<WithCorrelationId<NotificationsInput>>()), Times.Once);
        }

        [TestMethod]
        public void GetNotificationTypesInvalidTypeTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<INotificationProcessingService>();

            var controller = new NotificationTypesController(mockThetaLoggerFactory.Object, mockService.Object);

            var asset = new Dictionary<string, string>
            {
                {
                    "id", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
                },
                {
                    "type", "abc"
                },
            };

            var result = controller.Get(asset);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        #endregion

    }
}
