using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Contracts.Mappers;
using Theta.XSPOC.Apex.Api.Controllers;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Tests.Controllers
{
    [TestClass]
    public class DataHistoryControllerTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DataHistoryControllerConstructorNullLoggerTest()
        {
            var mockService = new Mock<IDataHistoryProcessingService>();

            _ = new DataHistoryController(null, mockService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DataHistoryControllerConstructorNullServiceTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            _ = new DataHistoryController(mockThetaLoggerFactory.Object, null);
        }

        [TestMethod]
        public void GetDataHistoryTrendItemsTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IDataHistoryProcessingService>();

            var controller = new DataHistoryController(mockThetaLoggerFactory.Object, mockService.Object);

            var responseData = new DataHistoryTrendItemsOutput
            {
                Values = new List<DataPoint>()
                {
                    new DataPoint()
                    {
                        X = DateTime.Now,
                        Y = 10,
                        TrendName = "Fluid Load"
                    },
                    new DataPoint()
                    {
                        X = DateTime.Now,
                        Y = 12,
                        TrendName = "Fluid Load"
                    },
                },
                Result = new MethodResult<string>(true, string.Empty)
            };

            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            filters.Add("type", "21");
            filters.Add("itemId", "Fluid Load");
            filters.Add("startDate", "2024-01-01 22:29:55.000");
            filters.Add("endDate", "2024-03-02 22:29:55.000");

            mockService.Setup(x => x.GetDataHistoryTrendDataItemsAsync(It.IsAny<WithCorrelationId<DataHistoryTrendInput>>()))
                .Returns(Task.FromResult(responseData));

            var result = controller.GetDataHistoryTrendItemsAsync(filters);
            var response = DataHistoryMapper.Map("CorrelationId", responseData);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result.Result).StatusCode);
            mockService.Verify(x => x.GetDataHistoryTrendDataItemsAsync(
                It.IsAny<WithCorrelationId<DataHistoryTrendInput>>()), Times.Once);
        }

        [TestMethod]
        public void GetDataHistoryTrendItemsInvalidParamsTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IDataHistoryProcessingService>();

            var controller = new DataHistoryController(mockThetaLoggerFactory.Object, mockService.Object);

            mockService.Setup(x => x.GetDataHistoryTrendDataItemsAsync(It.IsAny<WithCorrelationId<DataHistoryTrendInput>>()));

            var filters = new Dictionary<string, string>();
            filters.Add("address", "2022");
            filters.Add("startDate", "2024-01-01 22:29:55.000");
            filters.Add("endDate", "2024-03-02 22:29:55.000");

            var result = controller.GetDataHistoryTrendItemsAsync(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
            Assert.AreEqual(400, ((BadRequestResult)result.Result).StatusCode);
        }

        [TestMethod]
        public void GetDataHistoryTrendItemsInvalidAssetGuidTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IDataHistoryProcessingService>();

            var controller = new DataHistoryController(mockThetaLoggerFactory.Object, mockService.Object);

            mockService.Setup(x => x.GetDataHistoryTrendDataItemsAsync(It.IsAny<WithCorrelationId<DataHistoryTrendInput>>()));

            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "ASSETGUID");
            filters.Add("type", "21");
            filters.Add("itemId", "Fluid Load");
            filters.Add("startDate", "2024-01-01 22:29:55.000");
            filters.Add("endDate", "2024-03-02 22:29:55.000");

            var result = controller.GetDataHistoryTrendItemsAsync(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(BadRequestResult));
            Assert.AreEqual(400, ((BadRequestResult)result.Result).StatusCode);
        }

        [TestMethod]
        public void GetDataHistoryTrendItemsNullDataTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IDataHistoryProcessingService>();

            var controller = new DataHistoryController(mockThetaLoggerFactory.Object, mockService.Object);

            mockService.Setup(x => x.GetDataHistoryTrendDataItemsAsync(It.IsAny<WithCorrelationId<DataHistoryTrendInput>>()));

            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            filters.Add("type", "21");
            filters.Add("itemId", "Fluid Load");
            filters.Add("startDate", "2024-01-01 22:29:55.000");
            filters.Add("endDate", "2024-03-02 22:29:55.000");

            var result = controller.GetDataHistoryTrendItemsAsync(filters);

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result.Result).StatusCode);
            mockService.Verify(x => x.GetDataHistoryTrendDataItemsAsync(
                It.IsAny<WithCorrelationId<DataHistoryTrendInput>>()), Times.Once);
        }

        [TestMethod]
        public void GetDataHistoryListTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IDataHistoryProcessingService>();

            var controller = new DataHistoryController(mockThetaLoggerFactory.Object, mockService.Object);

            var responseData = new DataHistoryListOutput
            {
                Values = new List<DataHistoryListItem>()
                {
                    new DataHistoryListItem()
                    {
                        Id = "1",
                        Name = "TestName",
                        TypeId = 1,
                        Items = new List<DataHistoryListItem>()
                        {
                            new DataHistoryListItem()
                            {
                                Id = "2",
                                Name = "TestName2",
                                TypeId = 2,
                            }
                        }
                    },
                },
                Result = new MethodResult<string>(true, string.Empty)
            };

            var filters = new Dictionary<string, string>
            {
                {
                    "assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
                },
                {
                    "groupName", "abc"
                },
            };

            mockService.Setup(x => x.GetDataHistoryListData(It.IsAny<WithCorrelationId<DataHistoryTrendInput>>()))
                .Returns(responseData);

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetDataHistoryListMissingFiltersTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IDataHistoryProcessingService>();

            var controller = new DataHistoryController(mockThetaLoggerFactory.Object, mockService.Object);

            var result = controller.Get(null);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void GetDataHistoryListInvalidAssetIdTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IDataHistoryProcessingService>();

            var controller = new DataHistoryController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>
            {
                {
                    "assetId", "abc"
                },
                {
                    "groupName", "abc"
                },
            };

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void GetDataHistoryListServiceResultNullTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IDataHistoryProcessingService>();

            var controller = new DataHistoryController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>
            {
                {
                    "assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
                },
                {
                    "groupName", "abc"
                },
            };

            mockService.Setup(x => x.GetDataHistoryListData(It.IsAny<WithCorrelationId<DataHistoryTrendInput>>()))
                .Returns((DataHistoryListOutput)null);

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
        }

        [TestMethod]
        public void GetAlarmLimitsTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IDataHistoryProcessingService>();

            var controller = new DataHistoryController(mockThetaLoggerFactory.Object, mockService.Object);

            var responseData = new DataHistoryAlarmLimitsOutput
            {
                Values = new List<DataHistoryAlarmLimitsValues>()
                {
                    new DataHistoryAlarmLimitsValues()
                    {
                        Address = 12345,
                        LoLimit = 5,
                        HiLimit = 10,
                        LoLoLimit = 1,
                        HiHiLimit = 2,
                    }
                },
                Result = new MethodResult<string>(true, string.Empty)
            };

            var filters = new Dictionary<string, string>
            {
                {
                    "assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
                },
                {
                    "addresses", "12345"
                },
            };

            mockService.Setup(x => x.GetAlarmLimits(It.IsAny<WithCorrelationId<DataHistoryAlarmLimitsInput>>()))
                .Returns(responseData);

            var result = controller.GetAlarmLimits(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetAlarmLimitsInvalidAssetIdTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IDataHistoryProcessingService>();

            var controller = new DataHistoryController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>
            {
                {
                    "assetId", "abc"
                },
                {
                    "addresses", "12345"
                },
            };

            var result = controller.GetAlarmLimits(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void GetAlarmLimitsInvalidAddressesTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IDataHistoryProcessingService>();

            var controller = new DataHistoryController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>
            {
                {
                    "assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
                },
                {
                    "addresses", ""
                },
            };

            var result = controller.GetAlarmLimits(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void GetAlarmLimitsServiceResultNullTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IDataHistoryProcessingService>();

            var controller = new DataHistoryController(mockThetaLoggerFactory.Object, mockService.Object);

            var responseData = new DataHistoryAlarmLimitsOutput
            {
                Values = new List<DataHistoryAlarmLimitsValues>()
                {
                    new DataHistoryAlarmLimitsValues()
                    {
                        Address = 12345,
                        LoLimit = 5,
                        HiLimit = 10,
                        LoLoLimit = 1,
                        HiHiLimit = 2,
                    }
                },
                Result = new MethodResult<string>(true, string.Empty)
            };

            var filters = new Dictionary<string, string>
            {
                {
                    "assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
                },
                {
                    "addresses", "12345"
                },
            };

            mockService.Setup(x => x.GetAlarmLimits(It.IsAny<WithCorrelationId<DataHistoryAlarmLimitsInput>>()))
                .Returns((DataHistoryAlarmLimitsOutput)null);

            var result = controller.GetAlarmLimits(filters);

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
        }

        #endregion

    }
}
