using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public class GroupStatusControllerTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLoggerTest()
        {
            var mockService = new Mock<IGroupStatusProcessingService>();
            var mockCommonService = new Mock<ICommonService>();

            _ = new GroupStatusController(null, mockService.Object, mockCommonService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullGroupStatusProcessingServiceTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            var mockCommonService = new Mock<ICommonService>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            _ = new GroupStatusController(mockThetaLoggerFactory.Object, null, mockCommonService.Object);
        }

        [TestMethod]
        public void GetGroupStatusbyAssetIdTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockCommonService = new Mock<ICommonService>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var events = new GroupStatusOutput()
            {
                Result = new MethodResult<string>(true, string.Empty),
                Values = new GroupStatusValues()
                {
                    Columns = new List<GroupStatusColumn>()
                    {
                        new GroupStatusColumn
                        {
                            Name = "Name",
                            Id = 1,
                        }
                    },
                    Rows = new List<GroupStatusRow>()
                    {
                        new GroupStatusRow()
                        {
                            Columns = new List<GroupStatusRowColumn>()
                            {
                                new GroupStatusRowColumn
                                {
                                    ColumnId = 1,
                                    Value = "Value"
                                }
                            },
                        }
                    }
                }
            };

            var mockService = new Mock<IGroupStatusProcessingService>();
            mockService.Setup(x => x.GetGroupStatus(It.IsAny<WithCorrelationId<GroupStatusInput>>()))
                .Returns(events);

            var controller = new GroupStatusController(mockThetaLoggerFactory.Object, mockService.Object, mockCommonService.Object);
            string[] assetId = new string[1]
            {
                "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
            };

            var result = controller.Get(new Dictionary<string, string> { { "viewId", "1" }, { "groupName", "TestGroup" } });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
            mockService.Verify(x => x.GetGroupStatus(It.IsAny<WithCorrelationId<GroupStatusInput>>()), Times.Once);
        }

        [TestMethod]
        public void GetGroupStatusInvalidViewIdTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockCommonService = new Mock<ICommonService>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IGroupStatusProcessingService>();

            var controller = new GroupStatusController(mockThetaLoggerFactory.Object, mockService.Object, mockCommonService.Object);
            string[] assetId = new string[1] { "DFC1D0AD-A824-4965-B78D-AB7755E32DD3" };

            var result = controller.Get(null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void GetGroupStatusServiceFailureTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockCommonService = new Mock<ICommonService>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IGroupStatusProcessingService>();
            mockService.Setup(x => x.GetGroupStatus(It.IsAny<WithCorrelationId<GroupStatusInput>>()))
                .Returns((GroupStatusOutput)null);

            var controller = new GroupStatusController(mockThetaLoggerFactory.Object, mockService.Object, mockCommonService.Object);
            string[] assetId = new string[1] { "DFC1D0AD-A824-4965-B78D-AB7755E32DD3" };

            var result = controller.Get(new Dictionary<string, string> { { "viewId", "1" }, { "groupName", "TestGroup" } });

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
        }

        [TestMethod]
        public void GetGroupStatusGroupNameNullTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockCommonService = new Mock<ICommonService>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IGroupStatusProcessingService>();

            var controller = new GroupStatusController(mockThetaLoggerFactory.Object, mockService.Object, mockCommonService.Object);

            var result = controller.Get(new Dictionary<string, string> { { "viewId", "1" } });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void GetGroupStatusEmptyGroupNameTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockCommonService = new Mock<ICommonService>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IGroupStatusProcessingService>();
            var controller = new GroupStatusController(mockThetaLoggerFactory.Object, mockService.Object, mockCommonService.Object);

            var result = controller.Get(new Dictionary<string, string> { { "viewId", "1" }, { "groupName", "" } });

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void GetGroupStatusClassificationsTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockCommonService = new Mock<ICommonService>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            mockCommonService.Setup(x => x.GetSystemParameterNextGenSignificantDigits(It.IsAny<string>())).Returns(4);

            var response = new GroupStatusWidgetOutput()
            {
                Result = new MethodResult<string>(true, string.Empty),
                ClassificationValues = new List<GroupStatusClassification>()
                {
                    new GroupStatusClassification()
                    {
                        Id = 25,
                        Name ="Gearbox Overloaded",
                        Hours = 1738,
                        Percent = 14
                    },
                    new GroupStatusClassification()
                    {
                        Id = 27,
                        Name ="Motor Overloaded",
                        Hours = 1735,
                        Percent = 12
                    },
                }
            };

            var mockService = new Mock<IGroupStatusProcessingService>();
            mockService.Setup(x => x.GetClassificationWidgetData(It.IsAny<WithCorrelationId<GroupStatusInput>>()))
                .Returns(response);

            var controller = new GroupStatusController(mockThetaLoggerFactory.Object, mockService.Object, mockCommonService.Object);

            var result = controller.GetGroupClassificationWidgetData(new Dictionary<string, string> { { "groupName", "TestGroup" } });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
            mockService.Verify(x => x.GetClassificationWidgetData(It.IsAny<WithCorrelationId<GroupStatusInput>>()), Times.Once);
            Assert.IsTrue(
                ((Api.Contracts.Responses.GroupStatusWidgetResponse)((OkObjectResult)result).Value).ClassificationValues.Count > 0);
        }

        [TestMethod]
        public void GetGroupStatusClassificationsEmptyGroupNameTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockCommonService = new Mock<ICommonService>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var response = new GroupStatusWidgetOutput()
            {
                Result = new MethodResult<string>(true, string.Empty),
                ClassificationValues = new List<GroupStatusClassification>()
                {
                    new GroupStatusClassification()
                    {
                        Id = 25,
                        Name ="Gearbox Overloaded",
                        Hours = 1738,
                        Percent = 14
                    },
                    new GroupStatusClassification()
                    {
                        Id = 27,
                        Name ="Motor Overloaded",
                        Hours = 1735,
                        Percent = 12
                    },
                }
            };

            var mockService = new Mock<IGroupStatusProcessingService>();
            mockService.Setup(x => x.GetClassificationWidgetData(It.IsAny<WithCorrelationId<GroupStatusInput>>()))
                .Returns(response);

            var controller = new GroupStatusController(mockThetaLoggerFactory.Object, mockService.Object, mockCommonService.Object);

            var result = controller.GetGroupClassificationWidgetData(new Dictionary<string, string> { { "test", "test" } });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            Assert.AreEqual(400, ((BadRequestResult)result).StatusCode);
            mockService.Verify(x => x.GetClassificationWidgetData(It.IsAny<WithCorrelationId<GroupStatusInput>>()), Times.Never);
        }

        [TestMethod]
        public void GetGroupStatusClassificationsNullResponseTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockCommonService = new Mock<ICommonService>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IGroupStatusProcessingService>();
            mockService.Setup(x => x.GetClassificationWidgetData(It.IsAny<WithCorrelationId<GroupStatusInput>>()));

            var controller = new GroupStatusController(mockThetaLoggerFactory.Object, mockService.Object, mockCommonService.Object);

            var result = controller.GetGroupClassificationWidgetData(new Dictionary<string, string> { { "groupName", "test" } });

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(x => x.GetClassificationWidgetData(It.IsAny<WithCorrelationId<GroupStatusInput>>()), Times.Once);
        }

        [TestMethod]
        public void GetGroupStatusClassificationsNullResponseValueTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockCommonService = new Mock<ICommonService>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var response = new GroupStatusWidgetOutput()
            {
                Result = new MethodResult<string>(true, string.Empty),
            };

            var mockService = new Mock<IGroupStatusProcessingService>();
            mockService.Setup(x => x.GetClassificationWidgetData(It.IsAny<WithCorrelationId<GroupStatusInput>>()))
                .Returns(response);

            var controller = new GroupStatusController(mockThetaLoggerFactory.Object, mockService.Object, mockCommonService.Object);

            var result = controller.GetGroupClassificationWidgetData(new Dictionary<string, string> { { "groupName", "test" } });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
            Assert.IsTrue(
             ((Api.Contracts.Responses.GroupStatusWidgetResponse)((OkObjectResult)result).Value).ClassificationValues.Count == 0);

            mockService.Verify(x => x.GetClassificationWidgetData(It.IsAny<WithCorrelationId<GroupStatusInput>>()), Times.Once);
        }

        [TestMethod]
        public void GetGroupStatusAlarmsTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockCommonService = new Mock<ICommonService>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            mockCommonService.Setup(x => x.GetSystemParameterNextGenSignificantDigits(It.IsAny<string>())).Returns(4);

            var response = new GroupStatusKPIOutput()
            {
                Result = new MethodResult<string>(true, string.Empty),
                Values = new List<GroupStatusKPIValues>
                {
                    new GroupStatusKPIValues()
                    {
                        Id = "25",
                        Name = "Alarm",
                        Count = 20,
                        Percent = 2
                    }
                }
            };

            var mockService = new Mock<IGroupStatusProcessingService>();
            mockService.Setup(x => x.GetAlarmsWidgetDataAsync(It.IsAny<WithCorrelationId<GroupStatusInput>>()))
                .Returns(Task.FromResult(response));

            var controller = new GroupStatusController(mockThetaLoggerFactory.Object, mockService.Object, mockCommonService.Object);

            var result = controller.GetAlarmsWidgetDataAsync(new Dictionary<string, string> { { "groupName", "TestGroup" } });

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result.Result).StatusCode);
            mockService.Verify(x => x.GetAlarmsWidgetDataAsync(It.IsAny<WithCorrelationId<GroupStatusInput>>()), Times.Once);
            Assert.IsTrue(
                ((Api.Contracts.Responses.GroupStatusKPIResponse)((OkObjectResult)result.Result).Value).Values.Count > 0);
        }

        [TestMethod]
        public async Task GetGroupStatusGetDowntimeByWellsTest()
        {
            // Arrange
            var logger = new Mock<IThetaLogger>();
            var mockCommonService = new Mock<ICommonService>();
            var mockLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var filters = new Dictionary<string, string>
            {
                {
                    "groupName", "TestGroup"
                }
            };

            var mockService = new Mock<IGroupStatusProcessingService>();
            mockService
                .Setup(s => s.GetDowntimeByWellsAsync(It.IsAny<WithCorrelationId<GroupStatusInput>>()))
                .ReturnsAsync(new GroupStatusDowntimeByWellOutput()
                {
                    Result = new MethodResult<string>(true, string.Empty),
                    Assets = new List<GroupStatusKPIValues>(),
                    GroupByDuration = new List<GroupStatusKPIValues>(),
                });

            var controller = new GroupStatusController(mockLoggerFactory.Object, mockService.Object, mockCommonService.Object);

            // Act
            var result = await controller.GetDowntimeByWells(filters);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNotNull(((OkObjectResult)result).Value);
        }

        [TestMethod]
        public async Task GetGroupStatusGetDowntimeByWellsMissingFilterGroupNameTest()
        {
            // Arrange
            var logger = new Mock<IThetaLogger>();
            var mockCommonService = new Mock<ICommonService>();
            var mockLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var filters = new Dictionary<string, string>();

            var mockService = new Mock<IGroupStatusProcessingService>();
            mockService
                .Setup(s => s.GetDowntimeByWellsAsync(It.IsAny<WithCorrelationId<GroupStatusInput>>()))
                .ReturnsAsync(new GroupStatusDowntimeByWellOutput()
                {
                    Result = new MethodResult<string>(true, string.Empty),
                    Assets = new List<GroupStatusKPIValues>(),
                    GroupByDuration = new List<GroupStatusKPIValues>(),
                });

            var controller = new GroupStatusController(mockLoggerFactory.Object, mockService.Object, mockCommonService.Object);

            // Act
            var result = await controller.GetDowntimeByWells(filters);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task GetGroupStatusGetDowntimeByWellsServiceResultNullTest()
        {
            // Arrange
            var logger = new Mock<IThetaLogger>();
            var mockCommonService = new Mock<ICommonService>();
            var mockLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var filters = new Dictionary<string, string>
            {
                {
                    "groupName", "TestGroup"
                }
            };

            var mockService = new Mock<IGroupStatusProcessingService>();
            mockService
                .Setup(s => s.GetDowntimeByWellsAsync(It.IsAny<WithCorrelationId<GroupStatusInput>>()))
                .ReturnsAsync((GroupStatusDowntimeByWellOutput)null);

            var controller = new GroupStatusController(mockLoggerFactory.Object, mockService.Object, mockCommonService.Object);

            // Act
            var result = await controller.GetDowntimeByWells(filters);

            // Assert
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
        }

        [TestMethod]
        public async Task GetGroupStatusGetDowntimeByRunStatusTest()
        {
            // Arrange
            var logger = new Mock<IThetaLogger>();
            var mockCommonService = new Mock<ICommonService>();
            var mockLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var filters = new Dictionary<string, string>
            {
                {
                    "groupName", "TestGroup"
                }
            };

            var mockService = new Mock<IGroupStatusProcessingService>();
            mockService
                .Setup(s => s.GetDowntimeByRunStatusAsync(It.IsAny<WithCorrelationId<GroupStatusInput>>()))
                .ReturnsAsync(new GroupStatusKPIOutput()
                {
                    Result = new MethodResult<string>(true, string.Empty),
                    Values = new List<GroupStatusKPIValues>(),
                });

            var controller = new GroupStatusController(mockLoggerFactory.Object, mockService.Object, mockCommonService.Object);

            // Act
            var result = await controller.GetDowntimeByRunStatus(filters);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.IsNotNull(((OkObjectResult)result).Value);
        }

        [TestMethod]
        public async Task GetGroupStatusGetDowntimeByRunStatusMissingFilterGroupNameTest()
        {
            // Arrange
            var logger = new Mock<IThetaLogger>();
            var mockCommonService = new Mock<ICommonService>();
            var mockLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var filters = new Dictionary<string, string>();

            var mockService = new Mock<IGroupStatusProcessingService>();
            mockService
                .Setup(s => s.GetDowntimeByRunStatusAsync(It.IsAny<WithCorrelationId<GroupStatusInput>>()))
                .ReturnsAsync(new GroupStatusKPIOutput()
                {
                    Result = new MethodResult<string>(true, string.Empty),
                    Values = new List<GroupStatusKPIValues>(),
                });

            var controller = new GroupStatusController(mockLoggerFactory.Object, mockService.Object, mockCommonService.Object);

            // Act
            var result = await controller.GetDowntimeByRunStatus(filters);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public async Task GetGroupStatusGetDowntimeByRunStatusServiceResultNullTest()
        {
            // Arrange
            var logger = new Mock<IThetaLogger>();
            var mockCommonService = new Mock<ICommonService>();
            var mockLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var filters = new Dictionary<string, string>
            {
                {
                    "groupName", "TestGroup"
                }
            };

            var mockService = new Mock<IGroupStatusProcessingService>();
            mockService
                .Setup(s => s.GetDowntimeByRunStatusAsync(It.IsAny<WithCorrelationId<GroupStatusInput>>()))
                .ReturnsAsync((GroupStatusKPIOutput)null);

            var controller = new GroupStatusController(mockLoggerFactory.Object, mockService.Object, mockCommonService.Object);

            // Act
            var result = await controller.GetDowntimeByRunStatus(filters);

            // Assert
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
        }

        #endregion

    }
}
