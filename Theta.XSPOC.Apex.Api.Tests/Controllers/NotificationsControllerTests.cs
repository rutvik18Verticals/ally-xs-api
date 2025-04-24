using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class NotificationsControllerTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLoggerTest()
        {
            var mockService = new Mock<INotificationProcessingService>();

            _ = new NotificationsController(null, mockService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullNotificationProcessingServiceTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            _ = new NotificationsController(mockThetaLoggerFactory.Object, null);
        }

        [TestMethod]
        public void GetNotificationsByAssetIdTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var events = new List<NotificationData>()
            {
                new NotificationData()
                {
                    EventId = "104",
                    NodeId = "AssetId2",
                    EventTypeId = 1,
                    Date = DateTime.Now,
                    Status = "abc",
                    Note = "Runtime, Limit= Clear,Unack",
                    UserId = "",
                    TransactionId = 1111,
                    EventTypeName = "Comment"
                }
            };

            var eventypes = GetNotificationTypes();

            var mockService = new Mock<INotificationProcessingService>();
            mockService.Setup(x => x.GetNotifications(It.IsAny<WithCorrelationId<NotificationsInput>>()))
                .Returns(new NotificationDataOutput
                {
                    Result = new MethodResult<string>(true, string.Empty),
                    Values = events
                });

            var controller = new NotificationsController(mockThetaLoggerFactory.Object, mockService.Object);

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
            mockService.Verify(x => x.GetNotifications(It.IsAny<WithCorrelationId<NotificationsInput>>()), Times.Once);
            Assert.AreEqual(1, ((NotificationResponse)((OkObjectResult)result).Value).Values.Count);
            Assert.IsTrue(eventypes.Any(e =>
                e.Id == ((NotificationResponse)((OkObjectResult)result).Value).Values[0].EventTypeId.ToString()));
            Assert.IsTrue(eventypes.Any(e =>
                e.Name == ((NotificationResponse)((OkObjectResult)result).Value).Values[0].EventTypeName));
        }

        [TestMethod]
        public void GetNotificationsByAssetIdDateDescTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            DateTime varDateTimeNow = DateTime.Now;

            var events = new List<NotificationData>()
            {
                new NotificationData()
                {
                    EventId = "102",
                    NodeId = "AssetId1",
                    EventTypeId = 1,
                    Date = varDateTimeNow,
                    Status = "abc1",
                    Note = "Runtime, Limit= Clear,Unack",
                    UserId = "",
                    TransactionId = 1111,
                    EventTypeName = "Comment1"
                },
                new NotificationData()
                {
                    EventId = "103",
                    NodeId = "AssetId2",
                    EventTypeId = 2,
                    Date = varDateTimeNow.AddHours(-1),
                    Status = "abc2",
                    Note = "Runtime, Limit= Clear,Unack",
                    UserId = "",
                    TransactionId = 11112,
                    EventTypeName = "Comment2"
                },
                new NotificationData()
                {
                    EventId = "104",
                    NodeId = "AssetId3",
                    EventTypeId = 3,
                    Date = varDateTimeNow.AddHours(-2),
                    Status = "abc3",
                    Note = "Runtime, Limit= Clear,Unack",
                    UserId = "",
                    TransactionId = 11113,
                    EventTypeName = "Comment3"
                }
            };

            var eventypes = GetNotificationTypes();

            var mockService = new Mock<INotificationProcessingService>();
            mockService.Setup(x => x.GetNotifications(It.IsAny<WithCorrelationId<NotificationsInput>>()))
                .Returns(new NotificationDataOutput
                {
                    Result = new MethodResult<string>(true, string.Empty),
                    Values = events
                });

            var controller = new NotificationsController(mockThetaLoggerFactory.Object, mockService.Object);

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
            mockService.Verify(x => x.GetNotifications(It.IsAny<WithCorrelationId<NotificationsInput>>()), Times.Once);
            Assert.AreEqual(3, ((NotificationResponse)((OkObjectResult)result).Value).Values.Count);
            Assert.AreEqual(varDateTimeNow, ((NotificationResponse)((OkObjectResult)result).Value).Values[0].Date);
            Assert.AreEqual(events.Select(x => x.Date).OrderByDescending(x => x.Date).Take(1).SingleOrDefault(),
                ((NotificationResponse)((OkObjectResult)result).Value).Values[0].Date);
        }

        [TestMethod]
        public void GetNotificationsByAssetIdWhenfilterNullTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var events = new List<NotificationData>()
            {
                new NotificationData()
                {
                    EventId = "104",
                    NodeId = "AssetId2",
                    EventTypeId = 1,
                    Date = DateTime.Now,
                    Status = "abc",
                    Note = "Runtime, Limit= Clear,Unack",
                    UserId = "",
                    TransactionId = 1111,
                    EventTypeName = "Host Alarm"
                }
            };

            var mockService = new Mock<INotificationProcessingService>();
            mockService.Setup(x => x.GetNotifications(It.IsAny<WithCorrelationId<NotificationsInput>>()))
                .Returns(new NotificationDataOutput
                {
                    Result = new MethodResult<string>(true, string.Empty),
                    Values = events
                });

            var controller = new NotificationsController(mockThetaLoggerFactory.Object, mockService.Object);

            IDictionary<string, string> asset = null;

            var result = controller.Get(asset);

            Assert.AreEqual(400, ((StatusCodeResult)result).StatusCode);
        }

        [TestMethod]
        public void GetNotificationsByAssetGroupNameAndNotificationTypeIdTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var events = new List<NotificationData>()
            {
                new NotificationData()
                {
                    EventId = "104",
                    NodeId = "AssetId2",
                    EventTypeId = 1,
                    Date = DateTime.Now,
                    Status = "abc",
                    Note = "Runtime, Limit= Clear,Unack",
                    UserId = "",
                    TransactionId = 1111,
                    EventTypeName = "Comment"
                },
                new NotificationData()
                {
                    EventId = "105",
                    NodeId = "AssetId2",
                    EventTypeId = 1,
                    Date = DateTime.Now,
                    Status = "abc",
                    Note = "Runtime, Limit= Clear,Unack",
                    UserId = "",
                    TransactionId = 1112,
                    EventTypeName = "Comment"
                },
            };

            var eventypes = GetNotificationTypes();

            var mockService = new Mock<INotificationProcessingService>();
            mockService.Setup(x => x.GetNotifications(It.IsAny<WithCorrelationId<NotificationsInput>>()))
                .Returns(new NotificationDataOutput
                {
                    Result = new MethodResult<string>(true, string.Empty),
                    Values = events
                });

            var controller = new NotificationsController(mockThetaLoggerFactory.Object, mockService.Object);

            var asset = new Dictionary<string, string>
            {
                {
                    "id", "66FC0809-091C-4A65-94FA-0394C3BEE1FB"
                },
                {
                    "type", "group"
                },
                {
                    "notificationTypeId", "1"
                }
            };

            var result = controller.Get(asset);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
            mockService.Verify(x => x.GetNotifications(It.IsAny<WithCorrelationId<NotificationsInput>>()), Times.Once);
            Assert.AreEqual(2, ((NotificationResponse)((OkObjectResult)result).Value).Values.Count);
            Assert.IsTrue(eventypes.Any(e =>
                e.Id == ((NotificationResponse)((OkObjectResult)result).Value).Values[0].EventTypeId.ToString()));
            Assert.IsTrue(eventypes.Any(e =>
                e.Name == ((NotificationResponse)((OkObjectResult)result).Value).Values[0].EventTypeName));
        }

        [TestMethod]
        public void GetNotificationsInvalidTypeTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<INotificationProcessingService>();

            var controller = new NotificationsController(mockThetaLoggerFactory.Object, mockService.Object);

            var asset = new Dictionary<string, string>
            {
                {
                    "id", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
                },
                {
                    "type", "abc"
                },
                {
                    "notificationTypeId", "1"
                }
            };

            var result = controller.Get(asset);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void GetNotificationsNullResponseTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            NotificationDataOutput output = null;
            var mockService = new Mock<INotificationProcessingService>();
            mockService.Setup(x => x.GetNotifications(It.IsAny<WithCorrelationId<NotificationsInput>>()))
                .Returns(output);

            var controller = new NotificationsController(mockThetaLoggerFactory.Object, mockService.Object);

            var asset = new Dictionary<string, string>
            {
                {
                    "id", "66FC0809-091C-4A65-94FA-0394C3BEE1FB"
                },
                {
                    "type", "asset"
                },
                {
                    "notificationTypeId", "1"
                }
            };

            var result = controller.Get(asset);

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(x => x.GetNotifications(It.IsAny<WithCorrelationId<NotificationsInput>>()), Times.Once);
        }

        #endregion

        #region Private Methods

        private IList<NotificationTypesData> GetNotificationTypes()
        {
            return new List<NotificationTypesData>()
            {
                new NotificationTypesData()
                {
                    Id = "1",
                    Name = "Comment",
                },
                new NotificationTypesData()
                {
                    Id = "2",
                    Name = "Param Change",
                },
                new NotificationTypesData()
                {
                    Id = "3",
                    Name = "Status Change",
                },
                new NotificationTypesData()
                {
                    Id = "4",
                    Name = "RTU Alarm",
                },
            };
        }

        #endregion

    }
}
