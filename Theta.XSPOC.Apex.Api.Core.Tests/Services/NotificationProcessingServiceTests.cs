using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Core.Tests.Services
{
    [TestClass]
    public class NotificationProcessingServiceTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLoggerServiceTest()
        {
            var mockService = new Mock<INotification>();
            var mockDateTimeConverter = new Mock<IDateTimeConverter>();

            _ = new NotificationProcessingService(mockService.Object, null, mockDateTimeConverter.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullNotificationServiceTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockDateTimeConverter = new Mock<IDateTimeConverter>();

            _ = new NotificationProcessingService(null, mockThetaLoggerFactory.Object, mockDateTimeConverter.Object);
        }

        [TestMethod]
        public void GetNotificationsByAssetIdTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockDateTimeConverter = new Mock<IDateTimeConverter>();

            var events = new List<NotificationsModel>()
            {
                new NotificationsModel()
                {
                    EventId = "104",
                    NodeId = "AssetId2",
                    EventTypeId = 0,
                    Date = DateTime.Now,
                    Status = "abc",
                    Note = "Runtime, Limit= Clear,Unack",
                    UserId = "",
                    TransactionId = 1111,
                    EventTypeName = "Host Alarm"
                }
            };

            var mockNotificationService = new Mock<INotification>();
            mockNotificationService.Setup(x => x.GetEventsByAssetId("DFC1D0AD-A824-4965-B78D-AB7755E32DD3", 0, It.IsAny<string>()))
                .Returns(events);

            var service = new NotificationProcessingService(mockNotificationService.Object, 
                mockThetaLoggerFactory.Object, mockDateTimeConverter.Object);
            var payload = new NotificationsInput
            {
                AssetId = "DFC1D0AD-A824-4965-B78D-AB7755E32DD3",
                AssetGroupName = "",
                NotificationTypeId = 0,
            };

            var message = new WithCorrelationId<NotificationsInput>("CorrelationId", payload);
            var result = service.GetNotifications(message);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotificationDataOutput));
            mockNotificationService.Verify(x => x.GetEventsByAssetId("DFC1D0AD-A824-4965-B78D-AB7755E32DD3", 0, It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void GetNotificationsByAssetGroupNameTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>()))
                .Returns(logger.Object);
            var mockDateTimeConverter = new Mock<IDateTimeConverter>();

            var events = new List<NotificationsModel>()
            {
                new NotificationsModel()
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

            var mockNotificationService = new Mock<INotification>();
            mockNotificationService.Setup(x => x.GetEventsByAssetGroupName("<100 Intake Pressure", 1, It.IsAny<string>()))
                .Returns(events);

            var service = new NotificationProcessingService(mockNotificationService.Object,
                mockThetaLoggerFactory.Object, mockDateTimeConverter.Object);
            var payload = new NotificationsInput
            {
                AssetId = "",
                AssetGroupName = "<100 Intake Pressure",
                NotificationTypeId = 1
            };

            var message = new WithCorrelationId<NotificationsInput>("CorrelationId", payload);
            var result = service.GetNotifications(message);

            Assert.IsNotNull(result);
            mockNotificationService.Verify(x => x.GetEventsByAssetGroupName("<100 Intake Pressure", 1, It.IsAny<string>()),
                Times.Once);
        }

        [TestMethod]
        public void GetNotificationsNullMessageTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockNotificationService = new Mock<INotification>();
            mockNotificationService.Setup(x => x.GetEventsByAssetId("Test", 1, It.IsAny<string>()));

            var mockDateTimeConverter = new Mock<IDateTimeConverter>();

            var service = new NotificationProcessingService(mockNotificationService.Object,
                mockThetaLoggerFactory.Object, mockDateTimeConverter.Object);

            var message = new WithCorrelationId<NotificationsInput>("CorrelationId1", null);
            var result = service.GetNotifications(message);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Result.Status);
            mockNotificationService.Verify(x => x.GetEventsByAssetId("Test", 1, It.IsAny<string>()), Times.Never);
            logger.Verify(x => x.WriteCId(Level.Info,
                    It.Is<string>(x => x.Contains("requestWithCorrelationId is null, cannot get notifications.")),
                    "CorrelationId1"),
                Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetNotificationsEmptyAssetIdAndGroupNameTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockNotificationService = new Mock<INotification>();
            mockNotificationService.Setup(x => x.GetEventsByAssetId("", 1, It.IsAny<string>()));

            var mockDateTimeConverter = new Mock<IDateTimeConverter>();

            var service = new NotificationProcessingService(mockNotificationService.Object,
                mockThetaLoggerFactory.Object, mockDateTimeConverter.Object);

            var payload = new NotificationsInput
            {
                AssetId = "",
                AssetGroupName = "",
            };

            var message = new WithCorrelationId<NotificationsInput>("CorrelationId1", payload);
            var result = service.GetNotifications(message);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Result.Status);
            mockNotificationService.Verify(x => x.GetEventsByAssetId("", 1, It.IsAny<string>()), Times.Never);
            logger.Verify(x => x.WriteCId(Level.Info,
                    It.Is<string>(x => x.Contains("Either AssetId or AssetGroupName should be provided to get notifications.")),
                    "CorrelationId1"),
                Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetNotificationsTypesByAssetIdTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockDateTimeConverter = new Mock<IDateTimeConverter>();

            var events = new List<NotificationsTypesModel>()
            {
                new NotificationsTypesModel()
                {
                    Id = 101,
                    Name = "Well1",
                }
            };

            var mockNotificationService = new Mock<INotification>();
            mockNotificationService.Setup(x => x.GetEventsGroupsByAssetId("Well1", It.IsAny<string>()))
                .Returns(events);

            var service = new NotificationProcessingService(mockNotificationService.Object, 
                mockThetaLoggerFactory.Object, mockDateTimeConverter.Object);
            var payload = new NotificationsInput
            {
                AssetId = "Well1",
                AssetGroupName = "",
            };

            var message = new WithCorrelationId<NotificationsInput>("0", payload);
            var result = service.GetNotificationsTypes(message);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NotificationTypesDataOutput));
            mockNotificationService.Verify(x => x.GetEventsGroupsByAssetId("Well1", It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void GetNotificationsTypesByAssetGroupNameTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>()))
                .Returns(logger.Object);
            var mockDateTimeConverter = new Mock<IDateTimeConverter>();

            var events = new List<NotificationsTypesModel>()
            {
                new NotificationsTypesModel()
                {
                    Id = 101,
                    Name = "Well1",
                }
            };

            var mockNotificationService = new Mock<INotification>();
            mockNotificationService.Setup(x => x.GetEventsGroupsByAssetGroupName("<100 Intake Pressure", It.IsAny<string>()))
                .Returns(events);

            var service = new NotificationProcessingService(mockNotificationService.Object,
                mockThetaLoggerFactory.Object, mockDateTimeConverter.Object);
            var payload = new NotificationsInput
            {
                AssetId = null,
                AssetGroupName = "<100 Intake Pressure",
                NotificationTypeId = 1
            };

            var message = new WithCorrelationId<NotificationsInput>("2", payload);
            var result = service.GetNotificationsTypes(message);

            Assert.IsNotNull(result);
            mockNotificationService.Verify(x => x.GetEventsGroupsByAssetGroupName("<100 Intake Pressure", It.IsAny<string>()),
                Times.Once);
        }

        [TestMethod]
        public void GetNotificationsTypesNullMessageTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockNotificationService = new Mock<INotification>();
            mockNotificationService.Setup(x => x.GetEventsGroupsByAssetId("Test", It.IsAny<string>()));
            var mockDateTimeConverter = new Mock<IDateTimeConverter>();

            var service = new NotificationProcessingService(mockNotificationService.Object, 
                mockThetaLoggerFactory.Object, mockDateTimeConverter.Object);

            var message = new WithCorrelationId<NotificationsInput>("CorrelationId1", null);
            var result = service.GetNotificationsTypes(message);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Result.Status);
            mockNotificationService.Verify(x => x.GetEventsGroupsByAssetId("Test", It.IsAny<string>()), Times.Never);
            logger.Verify(x => x.WriteCId(Level.Info,
                    It.Is<string>(x => x.Contains("requestWithCorrelationId is null, cannot get notifications Types.")),
                    "CorrelationId1"),
                Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetNotificationTypesWithEmptyAssetIdAndGroupNameTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockNotificationService = new Mock<INotification>();
            mockNotificationService.Setup(x => x.GetEventsGroupsByAssetId("", It.IsAny<string>()));
            var mockDateTimeConverter = new Mock<IDateTimeConverter>();

            var service = new NotificationProcessingService(mockNotificationService.Object,
                mockThetaLoggerFactory.Object, mockDateTimeConverter.Object);

            var payload = new NotificationsInput
            {
                AssetId = "",
                AssetGroupName = "",
            };

            var message = new WithCorrelationId<NotificationsInput>("CorrelationId1", payload);
            var result = service.GetNotificationsTypes(message);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Result.Status);
            mockNotificationService.Verify(x => x.GetEventsGroupsByAssetGroupName("", It.IsAny<string>()), Times.Never);
            logger.Verify(x => x.WriteCId(Level.Info,
                    It.Is<string>(x => x.Contains("Either AssetId or AssetGroupName should be provided to get notifications.")),
                    "CorrelationId1"),
                Times.AtLeastOnce);
        }

        #endregion

    }
}
