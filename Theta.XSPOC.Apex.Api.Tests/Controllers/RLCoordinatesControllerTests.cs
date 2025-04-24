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
    public class RLCoordinatesControllerTests
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullCardCoordinateProcessingServiceTest()
        {
            _ = new RLCoordinatesController(null, null);
        }

        [TestMethod]
        public void GetCardCoordinatesResultsTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var valuesList = new List<CardResponseValuesOutput>();
            var values = new CardResponseValuesOutput();
            values.Id = 1;
            values.Name = "SurfaceCard";
            values.CoordinatesOutput = new List<CoordinatesData<float>>()
            {
                new CoordinatesData<float>()
                {
                    X = 1,
                    Y = 222
                },
                new CoordinatesData<float>()
                {
                    X = 12,
                    Y = 2224
                }
            };
            valuesList.Add(values);
            var values2 = new CardResponseValuesOutput();
            values.Id = 1;
            values.Name = "DownholeCard";
            values.CoordinatesOutput = new List<CoordinatesData<float>>()
            {
                new CoordinatesData<float>()
                {
                    X = 1,
                    Y = 222
                },
                new CoordinatesData<float>()
                {
                    X = 12,
                    Y = 2224
                }
            };
            valuesList.Add(values2);

            CardCoordinateDataOutput responseData = new CardCoordinateDataOutput()
            {
                Result = new MethodResult<string>(true, string.Empty),
                Values = valuesList
            };

            var mockService = new Mock<IRodLiftAnalysisProcessingService>();
            mockService.Setup(x => x.GetCardCoordinateResults(It.IsAny<WithCorrelationId<CardCoordinateInput>>()))
                .Returns(responseData);

            var controller = new RLCoordinatesController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            filters.Add("cardDate", "2023-05-11 22:29:55.000");

            var result = controller.GetCardCoordinate(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
            mockService.Verify(x => x.GetCardCoordinateResults(
                It.IsAny<WithCorrelationId<CardCoordinateInput>>()), Times.Once);
        }

        [TestMethod]
        public void GetCardCoordinateResultsEmptyPayloadTest()
        {
            CardCoordinateDataOutput responseData = null;

            var mockService = new Mock<IRodLiftAnalysisProcessingService>();
            mockService.Setup(x => x.GetCardCoordinateResults(It.IsAny<WithCorrelationId<CardCoordinateInput>>()))
                .Returns(responseData);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new RLCoordinatesController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            filters.Add("cardDate", "2023-05-11 22:29:55.000");

            var result = controller.GetCardCoordinate(filters);

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(x => x.GetCardCoordinateResults(It.IsAny<WithCorrelationId<CardCoordinateInput>>()),
                Times.Once);
        }

        [TestMethod]
        public void GetCardCoordinateResultsInvalidInputTest()
        {
            CardCoordinateDataOutput responseData = null;

            var mockService = new Mock<IRodLiftAnalysisProcessingService>();
            mockService.Setup(x => x.GetCardCoordinateResults(It.IsAny<WithCorrelationId<CardCoordinateInput>>()))
                .Returns(responseData);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new RLCoordinatesController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>();
            filters.Add("assetId", Guid.Empty.ToString());
            filters.Add("cardDate", "");

            var result = controller.GetCardCoordinate(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            Assert.AreEqual(400, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(x => x.GetCardCoordinateResults(It.IsAny<WithCorrelationId<CardCoordinateInput>>()),
                Times.Never);
        }

        [TestMethod]
        public void GetCardCoordinateNullResponseTest()
        {
            CardCoordinateDataOutput responseData = null;

            var mockService = new Mock<IRodLiftAnalysisProcessingService>();
            mockService.Setup(x => x.GetCardCoordinateResults(It.IsAny<WithCorrelationId<CardCoordinateInput>>()))
                .Returns(responseData);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new RLCoordinatesController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "f3eb743c-f890-44f3-80e5-6a46df7ce2b7");
            filters.Add("cardDate", "2022-11-09 23:48:13.000");

            var result = controller.GetCardCoordinate(filters);

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(x => x.GetCardCoordinateResults(It.IsAny<WithCorrelationId<CardCoordinateInput>>()),
                Times.Once);
        }

        [TestMethod]
        public void GetCardCoordinateInvalidAssetIdTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IRodLiftAnalysisProcessingService>();

            var controller = new RLCoordinatesController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>
            {
                {
                    "assetId", "abc"
                },
                {
                    "cardDate", "2022-11-09 23:48:13.000"
                }
            };

            var result = controller.GetCardCoordinate(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

    }
}
