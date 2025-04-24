using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Contracts.Mappers;
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
    public class ESPAnalysisControllerTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullESPAnalysisProcessingServiceTest()
        {
            _ = new ESPAnalysisController(null, null);
        }

        [TestMethod]
        public void GetESPAnalysisResultsTest()
        {
            var values = new ESPAnalysisValues();

            values.Inputs = new List<AnalysisInputOutput>()
            {
                new AnalysisInputOutput()
                {
                    Id = "PumpName",
                    Name = "",
                    DisplayName = "",
                    Value = "",
                    DisplayValue = "Pump 1",
                    DataTypeId = 0,
                    MeasurementAbbreviation = "",
                    SourceId = 0
                },
                new AnalysisInputOutput()
                {
                    Id = "NumberOfStages",
                    Name = "",
                    DisplayName = "",
                    Value = "",
                    DisplayValue = "4",
                    DataTypeId = 0,
                    MeasurementAbbreviation = "",
                    SourceId = 0
                },
            };

            values.Outputs = new List<AnalysisInputOutput>()
            {
                new AnalysisInputOutput()
                {
                    Id = "ProductivityIndex",
                    Name = "",
                    DisplayName = "",
                    Value = "",
                    DisplayValue = "1",
                    DataTypeId = 0,
                    MeasurementAbbreviation = "",
                    SourceId = 0
                },
                new AnalysisInputOutput()
                {
                    Id = "PressureAcrossPumpLength",
                    Name = "",
                    DisplayName = "",
                    Value = "",
                    DisplayValue = "70",
                    DataTypeId = 0,
                    MeasurementAbbreviation = "",
                    SourceId = 0
                },
            };
            values.IsGasHandlingEnabled = true;
            values.GasHandlingInputs = new List<AnalysisInputOutput>()
            {
                new AnalysisInputOutput()
                {
                    Id = "CasingValveState",
                    Name = "",
                    DisplayName = "",
                    Value = "",
                    DisplayValue = "1",
                    DataTypeId = 0,
                    MeasurementAbbreviation = "",
                    SourceId = 0
                },
                new AnalysisInputOutput()
                {
                    Id = "SpecificGravityGas",
                    Name = "",
                    DisplayName = "",
                    Value = "",
                    DisplayValue = "10",
                    DataTypeId = 0,
                    MeasurementAbbreviation = "",
                    SourceId = 0
                },
            };

            values.GasHandlingOutputs = new List<AnalysisInputOutput>()
            {
                new AnalysisInputOutput()
                {
                    Id = "SolutionGasOilRatio",
                    Name = "",
                    DisplayName = "",
                    Value = "",
                    DisplayValue = "24",
                    DataTypeId = 0,
                    MeasurementAbbreviation = "",
                    SourceId = 0
                },
                new AnalysisInputOutput()
                {
                    Id = "SpecificGravityOil",
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

            var responseData = new ESPAnalysisDataOutput
            {
                Values = values,
                Result = methodResult
            };

            var mockService = new Mock<IESPAnalysisProcessingService>();
            mockService.Setup(x => x.GetESPAnalysisResults(It.IsAny<WithCorrelationId<ESPAnalysisInput>>()))
                .Returns(responseData);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new ESPAnalysisController(mockService.Object, mockThetaLoggerFactory.Object);
            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            filters.Add("testDate", "2023-05-11 22:29:55.000");

            var result = controller.Get(filters);
            ESPAnalysisDataMapper.Map("CorrelationId", responseData);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
            mockService.Verify(x => x.GetESPAnalysisResults(
                It.IsAny<WithCorrelationId<ESPAnalysisInput>>()), Times.Once);
            Assert.AreEqual(true,
                (((Api.Contracts.Responses.ESPAnalysisResponse)((OkObjectResult)result).Value))
                .Values.IsGasHandlingEnabled);
        }

        [TestMethod]
        public void GetESPAnalysisResultsEmptyPayloadTest()
        {
            ESPAnalysisDataOutput responseData = null;

            var mockService = new Mock<IESPAnalysisProcessingService>();
            mockService.Setup(x => x.GetESPAnalysisResults(It.IsAny<WithCorrelationId<ESPAnalysisInput>>()))
                .Returns(responseData);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new ESPAnalysisController(mockService.Object, mockThetaLoggerFactory.Object);

            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            filters.Add("testDate", "2023-05-11 22:29:55.000");

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(x => x.GetESPAnalysisResults(It.IsAny<WithCorrelationId<ESPAnalysisInput>>()),
                Times.Once);
        }

        [TestMethod]
        public void GetESPAnalysisResultsInvalidInputTest()
        {
            ESPAnalysisDataOutput responseData = null;

            var mockService = new Mock<IESPAnalysisProcessingService>();
            mockService.Setup(x => x.GetESPAnalysisResults(It.IsAny<WithCorrelationId<ESPAnalysisInput>>()))
                .Returns(responseData);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new ESPAnalysisController(mockService.Object, mockThetaLoggerFactory.Object);

            var filters = new Dictionary<string, string>();
            filters.Add("assetId", Guid.Empty.ToString());
            filters.Add("testDate", "");
            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            Assert.AreEqual(400, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(x => x.GetESPAnalysisResults(It.IsAny<WithCorrelationId<ESPAnalysisInput>>()),
                Times.Never);
        }

        [TestMethod]
        public void GetESPAnalysisNullResponseTest()
        {
            ESPAnalysisDataOutput responseData = null;

            var mockService = new Mock<IESPAnalysisProcessingService>();
            mockService.Setup(x => x.GetESPAnalysisResults(It.IsAny<WithCorrelationId<ESPAnalysisInput>>()))
                .Returns(responseData);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new ESPAnalysisController(mockService.Object, mockThetaLoggerFactory.Object);

            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            filters.Add("testDate", "2023-05-11 22:29:55.000");

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(x => x.GetESPAnalysisResults(It.IsAny<WithCorrelationId<ESPAnalysisInput>>()),
                Times.Once);
        }

        [TestMethod]
        public void GetGLAnalysisInvalidAssetIdTest()
        {
            var mockService = new Mock<IESPAnalysisProcessingService>();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new ESPAnalysisController(mockService.Object, mockThetaLoggerFactory.Object);

            var filters = new Dictionary<string, string>
            {
                {
                    "assetId", "abc"
                },
                {
                    "testDate", "2023-05-11 22:29:55.000"
                }
            };

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        #endregion

    }
}
