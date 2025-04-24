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
    public class RLAnalysisControllerTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLoggerTest()
        {
            var mockService = new Mock<IRodLiftAnalysisProcessingService>();

            _ = new RLAnalysisController(null, mockService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullRodLiftAnalysisProcessingServiceTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            _ = new RLAnalysisController(mockThetaLoggerFactory.Object, null);
        }

        [TestMethod]
        public async Task GetRodLiftAnalysisResultsTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var values = new RodLiftAnalysisValues();

            values.Input = new List<ValueItem>()
            {
                new ValueItem()
                {
                    Id = "Runtime",
                    Name = "",
                    DisplayName = "",
                    Value = "",
                    DisplayValue = "24",
                    DataTypeId = 0,
                    MeasurementAbbreviation = "",
                    SourceId = 0
                },
                new ValueItem()
                {
                    Id = "StrokesPerMinute",
                    Name = "",
                    DisplayName = "",
                    Value = "",
                    DisplayValue = "24",
                    DataTypeId = 0,
                    MeasurementAbbreviation = "",
                    SourceId = 0
                },
            };

            values.Output = new List<ValueItem>()
            {
                new ValueItem()
                {
                    Id = "StrokeLength",
                    Name = "",
                    DisplayName = "",
                    Value = "",
                    DisplayValue = "24",
                    DataTypeId = 0,
                    MeasurementAbbreviation = "",
                    SourceId = 0
                },
                new ValueItem()
                {
                    Id = "SurfaceCapacity24",
                    Name = "",
                    DisplayName = "",
                    Value = "",
                    DisplayValue = "24",
                    DataTypeId = 0,
                    MeasurementAbbreviation = "",
                    SourceId = 0
                },
            };
            var methodResult = new MethodResult<string>(true, string.Empty);

            var responseData = new RodLiftAnalysisDataOutput
            {
                Values = values,
                Result = methodResult
            };

            var mockService = new Mock<IRodLiftAnalysisProcessingService>();
            mockService.Setup(x => x.GetRodLiftAnalysisResultsAsync(It.IsAny<WithCorrelationId<RodLiftAnalysisInput>>()))
                .ReturnsAsync(responseData);

            var controller = new RLAnalysisController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            filters.Add("cardDate", "2023-05-11 22:29:55.000");

            var result = await controller.GetRodLiftAnalysis(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
            mockService.Verify(x => x.GetRodLiftAnalysisResultsAsync(
                It.IsAny<WithCorrelationId<RodLiftAnalysisInput>>()), Times.Once);
        }

        [TestMethod]
        public async Task GetRodLiftAnalysisResultsEmptyPayloadTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            RodLiftAnalysisDataOutput responseData = null;

            var mockService = new Mock<IRodLiftAnalysisProcessingService>();
            mockService.Setup(x => x.GetRodLiftAnalysisResultsAsync(It.IsAny<WithCorrelationId<RodLiftAnalysisInput>>()))
                .ReturnsAsync(responseData);

            var controller = new RLAnalysisController(mockThetaLoggerFactory.Object, mockService.Object);
            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            filters.Add("cardDate", "2023-05-11 22:29:55.000");

            var result = await controller.GetRodLiftAnalysis(filters);

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(x => x.GetRodLiftAnalysisResultsAsync(It.IsAny<WithCorrelationId<RodLiftAnalysisInput>>()),
                Times.Once);
        }

        [TestMethod]
        public async Task GetRodLiftAnalysisResultsInvalidInputTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            RodLiftAnalysisDataOutput responseData = null;

            var mockService = new Mock<IRodLiftAnalysisProcessingService>();
            mockService.Setup(x => x.GetRodLiftAnalysisResultsAsync(It.IsAny<WithCorrelationId<RodLiftAnalysisInput>>()))
                .ReturnsAsync(responseData);

            var controller = new RLAnalysisController(mockThetaLoggerFactory.Object, mockService.Object);
            var filters = new Dictionary<string, string>();
            filters.Add("assetId", Guid.Empty.ToString());
            filters.Add("cardDate", "");
            var result = await controller.GetRodLiftAnalysis(filters);

            Assert.IsNotNull(result);
            Assert.AreEqual(400, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(x => x.GetRodLiftAnalysisResultsAsync(It.IsAny<WithCorrelationId<RodLiftAnalysisInput>>()),
                Times.Never);
        }

        [TestMethod]
        public async Task GetRodLiftAnalysisNullResponseTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            RodLiftAnalysisDataOutput responseData = null;

            var mockService = new Mock<IRodLiftAnalysisProcessingService>();
            mockService.Setup(x => x.GetRodLiftAnalysisResultsAsync(It.IsAny<WithCorrelationId<RodLiftAnalysisInput>>()))
                .ReturnsAsync(responseData);

            var controller = new RLAnalysisController(mockThetaLoggerFactory.Object, mockService.Object);
            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            filters.Add("cardDate", "2023-05-11 22:29:55.000");

            var result = await controller.GetRodLiftAnalysis(filters);

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(x => x.GetRodLiftAnalysisResultsAsync(It.IsAny<WithCorrelationId<RodLiftAnalysisInput>>()),
                Times.Once);
        }

        [TestMethod]
        public async Task GetRodLiftAnalysisNullInputTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IRodLiftAnalysisProcessingService>();
            mockService.Setup(x => x.GetRodLiftAnalysisResultsAsync(It.IsAny<WithCorrelationId<RodLiftAnalysisInput>>()));

            var controller = new RLAnalysisController(mockThetaLoggerFactory.Object, mockService.Object);

            var result = await controller.GetRodLiftAnalysis(null);

            Assert.IsNotNull(result);
            Assert.AreEqual(400, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(x => x.GetRodLiftAnalysisResultsAsync(It.IsAny<WithCorrelationId<RodLiftAnalysisInput>>()),
                Times.Never);
        }

        [TestMethod]
        public async Task GetRodLiftAnalysisResultsInvalidAssetIdTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IRodLiftAnalysisProcessingService>();

            var controller = new RLAnalysisController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>
            {
                {
                    "assetId", "abc"
                },
                {
                    "cardDate", "2023-05-11 22:29:55.000"
                }
            };

            var result = await controller.GetRodLiftAnalysis(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        #endregion

    }
}
