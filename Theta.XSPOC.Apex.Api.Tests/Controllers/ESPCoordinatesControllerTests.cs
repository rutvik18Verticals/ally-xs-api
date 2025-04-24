using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
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
    public class ESPCoordinatesControllerTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullESPAnalysisProcessingServiceTest()
        {
            _ = new ESPCoordinatesController(null, null);
        }

        [TestMethod]
        public void GetCurveCoordinateResult()
        {
            var coordinateValues = new List<CoordinatesData<double>>();
            var coordinateValue = new CoordinatesData<double>();

            coordinateValue.X = 1;
            coordinateValue.Y = 222;

            coordinateValues.Add(coordinateValue);

            coordinateValue.X = 12;
            coordinateValue.Y = 223;

            coordinateValues.Add(coordinateValue);

            var curveCoordinateValues = new List<CurveCoordinateValues>();
            var curveCoordinateValue = new CurveCoordinateValues();

            curveCoordinateValue.Id = 1;
            curveCoordinateValue.CurveTypeId = 1;
            curveCoordinateValue.Name = "Test";
            curveCoordinateValue.DisplayName = "Test";
            curveCoordinateValue.Coordinates = coordinateValues;

            curveCoordinateValues.Add(curveCoordinateValue);

            var responseData = new CurveCoordinateDataOutput
            {
                Values = curveCoordinateValues,
                Result = new MethodResult<string>(true, "Curve Coordinate Data"),
            };

            var mockService = new Mock<IESPAnalysisProcessingService>();
            mockService.Setup(x => x.GetCurveCoordinate(It.IsAny<WithCorrelationId<CurveCoordinatesInput>>()))
                .Returns(responseData);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new ESPCoordinatesController(mockService.Object, mockThetaLoggerFactory.Object);

            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            filters.Add("testDate", "2023-05-11 22:29:55.000");

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
            mockService.Verify(x => x.GetCurveCoordinate(
                It.IsAny<WithCorrelationId<CurveCoordinatesInput>>()), Times.Once);
        }

        [TestMethod]
        public void GetCurveCoordinateResultEmptyPayloadTest()
        {
            CurveCoordinateDataOutput responseData = null;

            var mockService = new Mock<IESPAnalysisProcessingService>();
            mockService.Setup(x => x.GetCurveCoordinate(It.IsAny<WithCorrelationId<CurveCoordinatesInput>>()))
                .Returns(responseData);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new ESPCoordinatesController(mockService.Object, mockThetaLoggerFactory.Object);

            var filters = new Dictionary<string, string>();
            filters.Add("assetid", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            filters.Add("testDate", "2023-05-11 22:29:55.000");

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            Assert.AreEqual(400, ((StatusCodeResult)result).StatusCode);
        }

        [TestMethod]
        public void GetCurveCoordinateResultInvalidInputTest()
        {
            CurveCoordinateDataOutput responseData = null;

            var mockService = new Mock<IESPAnalysisProcessingService>();
            mockService.Setup(x => x.GetCurveCoordinate(It.IsAny<WithCorrelationId<CurveCoordinatesInput>>()))
                .Returns(responseData);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new ESPCoordinatesController(mockService.Object, mockThetaLoggerFactory.Object);

            var filters = new Dictionary<string, string>();
            filters.Add("assetid", Guid.Empty.ToString());
            filters.Add("testDate", "");

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            Assert.AreEqual(400, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(x => x.GetCurveCoordinate(It.IsAny<WithCorrelationId<CurveCoordinatesInput>>()),
                Times.Never);
        }

        [TestMethod]
        public void GetCurveCoordinateResultNullResponseTest()
        {
            CurveCoordinateDataOutput responseData = null;

            var mockService = new Mock<IESPAnalysisProcessingService>();
            mockService.Setup(x => x.GetCurveCoordinate(It.IsAny<WithCorrelationId<CurveCoordinatesInput>>()))
                .Returns(responseData);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new ESPCoordinatesController(mockService.Object, mockThetaLoggerFactory.Object);

            var filters = new Dictionary<string, string>();
            filters.Add("assetid", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            filters.Add("testDate", "2023-05-11 22:29:55.000");

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            Assert.AreEqual(400, ((StatusCodeResult)result).StatusCode);
        }

        [TestMethod]
        public void GetCurveCoordinateInvalidAssetIdTest()
        {
            var mockService = new Mock<IESPAnalysisProcessingService>();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new ESPCoordinatesController(mockService.Object, mockThetaLoggerFactory.Object);

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

        [TestMethod]
        public void GetCurveCoordinateInvalidServiceResultTest()
        {
            var mockService = new Mock<IESPAnalysisProcessingService>();
            mockService.Setup(x => x.GetCurveCoordinate(It.IsAny<WithCorrelationId<CurveCoordinatesInput>>()))
                .Returns((CurveCoordinateDataOutput)null);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new ESPCoordinatesController(mockService.Object, mockThetaLoggerFactory.Object);

            var filters = new Dictionary<string, string>
            {
                {
                    "assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
                },
                {
                    "testDate", "2023-05-11 22:29:55.000"
                }
            };

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(x => x.GetCurveCoordinate(
                It.IsAny<WithCorrelationId<CurveCoordinatesInput>>()), Times.Once);
        }

        #endregion

    }
}
