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
    public class GLAnalysisControllerTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLoggerTest()
        {
            var mockService = new Mock<IGLAnalysisProcessingService>();

            _ = new GLAnalysisController(null, mockService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullGLAnalysisProcessingServiceTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            _ = new GLAnalysisController(mockThetaLoggerFactory.Object, null);
        }

        [TestMethod]
        public void GetGLAnalysisByAssetIdTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var events = new List<GLAnalysisData>()
            {
                new GLAnalysisData()
                {
                    Date = DateTime.Now,
                }
            };

            var mockService = new Mock<IGLAnalysisProcessingService>();
            mockService.Setup(x => x.GetGLAnalysisSurveyDate(It.IsAny<WithCorrelationId<GLSurveyAnalysisInput>>()))
                .Returns(new GLAnalysisSurveyDataOutput
                {
                    Result = new MethodResult<string>(true, string.Empty),
                    Values = events
                });

            var controller = new SurveysController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
            mockService.Verify(x => x.GetGLAnalysisSurveyDate(It.IsAny<WithCorrelationId<GLSurveyAnalysisInput>>()), Times.Once);
            Assert.AreEqual(1, ((GLAnalysisSurveyDateResponse)((OkObjectResult)result).Value).Values.Count);
        }

        [TestMethod]
        public void GetGLAnalysisNullResponseTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            GLAnalysisSurveyDataOutput output = null;
            var mockService = new Mock<IGLAnalysisProcessingService>();
            mockService.Setup(x => x.GetGLAnalysisSurveyDate(It.IsAny<WithCorrelationId<GLSurveyAnalysisInput>>()))
                .Returns(output);

            var controller = new SurveysController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(x => x.GetGLAnalysisSurveyDate(It.IsAny<WithCorrelationId<GLSurveyAnalysisInput>>()), Times.Once);
        }

        [TestMethod]
        public void GetGLAnalysisResultsTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var values = new GLAnalysisValues();

            values.Inputs = new List<AnalysisInputOutput>()
            {
                new AnalysisInputOutput()
                {
                    Id = "OilRate",
                    Name = null,
                    DisplayName = "Oil Rate",
                    Value = 99,
                    DisplayValue = "99 bpd",
                    DataTypeId = 0,
                    MeasurementAbbreviation = "bpd",
                    SourceId = 0
                },
                new AnalysisInputOutput()
                {
                    Id = "WaterRate",
                    Name = null,
                    DisplayName = "Water Rate",
                    Value = 109,
                    DisplayValue = "109 bpd",
                    DataTypeId = 0,
                    MeasurementAbbreviation = "bpd",
                    SourceId = null
                },
            };

            values.Outputs = new List<AnalysisInputOutput>()
            {
                new AnalysisInputOutput()
                {
                    Id = "WaterCut",
                    Name = null,
                    DisplayName = "Water Cut",
                    Value = 52,
                    DisplayValue = "52 %",
                    DataTypeId = 0,
                    MeasurementAbbreviation = "%",
                    SourceId = null
                },
                new AnalysisInputOutput()
                {
                    Id = "ProductionRate",
                    Name = "Production Rate",
                    DisplayName = null,
                    Value = 208,
                    DisplayValue = "208 bpd",
                    DataTypeId = 0,
                    MeasurementAbbreviation = "bpd",
                    SourceId = null
                },
            };

            var methodResult = new MethodResult<string>(true, string.Empty);

            var responseData = new GLAnalysisDataOutput
            {
                Values = values,
                Result = methodResult
            };

            var mockService = new Mock<IGLAnalysisProcessingService>();
            mockService.Setup(x => x.GetGLAnalysisResults(It.IsAny<WithCorrelationId<GLAnalysisInput>>()))
                .Returns(responseData);

            var controller = new GLAnalysisController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            filters.Add("testDate", "2023-05-11 22:29:55.000");
            filters.Add("analysisTypeId", "1");

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
            mockService.Verify(x => x.GetGLAnalysisResults(
                It.IsAny<WithCorrelationId<GLAnalysisInput>>()), Times.Once);
        }

        [TestMethod]
        public void GetGLAnalysisResultsEmptyPayloadTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            GLAnalysisDataOutput responseData = null;

            var mockService = new Mock<IGLAnalysisProcessingService>();
            mockService.Setup(x => x.GetGLAnalysisResults(It.IsAny<WithCorrelationId<GLAnalysisInput>>()))
                .Returns(responseData);

            var controller = new GLAnalysisController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            filters.Add("testDate", "2023-05-11 22:29:55.000");
            filters.Add("analysisTypeId", "1");

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(x => x.GetGLAnalysisResults(It.IsAny<WithCorrelationId<GLAnalysisInput>>()),
                Times.Once);
        }

        [TestMethod]
        public void GetGLAnalysisResultsInvalidInputTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            GLAnalysisDataOutput responseData = null;

            var mockService = new Mock<IGLAnalysisProcessingService>();
            mockService.Setup(x => x.GetGLAnalysisResults(It.IsAny<WithCorrelationId<GLAnalysisInput>>()))
                .Returns(responseData);

            var controller = new GLAnalysisController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>();
            filters.Add("assetId", Guid.Empty.ToString());
            filters.Add("testDate", "");
            filters.Add("analysisTypeId", "0");

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.AreEqual(400, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(x => x.GetGLAnalysisResults(It.IsAny<WithCorrelationId<GLAnalysisInput>>()),
                Times.Never);
        }

        [TestMethod]
        public void GetGLAnalysisResultsNullResponseTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            GLAnalysisDataOutput responseData = null;

            var mockService = new Mock<IGLAnalysisProcessingService>();
            mockService.Setup(x => x.GetGLAnalysisResults(It.IsAny<WithCorrelationId<GLAnalysisInput>>()))
                .Returns(responseData);

            var controller = new GLAnalysisController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            filters.Add("testDate", "2023-05-11 22:29:55.000");
            filters.Add("analysisTypeId", "1");

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(x => x.GetGLAnalysisResults(It.IsAny<WithCorrelationId<GLAnalysisInput>>()),
                Times.Once);
        }

        [TestMethod]
        public void GetSurveyMissingAssetIdTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IGLAnalysisProcessingService>();

            var controller = new SurveysController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>();

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void GetSurveyInvalidAssetIdTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IGLAnalysisProcessingService>();

            var controller = new SurveysController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>
            {
                {
                    "assetId", "abc"
                }
            };

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        # region GL Analysis Survey Curve Coordinates

        [TestMethod]
        public void GetSurveyCurveCoordinateResult()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var coordinateValues = new List<CoordinatesData<float>>();
            var coordinateValue = new CoordinatesData<float>();

            coordinateValue.X = 1;
            coordinateValue.Y = 222;

            coordinateValues.Add(coordinateValue);

            coordinateValue.X = 12;
            coordinateValue.Y = 223;

            coordinateValues.Add(coordinateValue);

            var curveCoordinateValues = new List<GLAnalysisCurveCoordinateData>();
            var curveCoordinateValue = new GLAnalysisCurveCoordinateData();

            curveCoordinateValue.Id = 1;
            curveCoordinateValue.CurveTypeId = 1;
            curveCoordinateValue.DisplayName = "Test";
            curveCoordinateValue.CoordinatesOutput = coordinateValues;

            curveCoordinateValues.Add(curveCoordinateValue);

            var responseData = new GLAnalysisCurveCoordinateDataOutput
            {
                Result = new MethodResult<string>(true, string.Empty),
                Values = curveCoordinateValues
            };

            var mockService = new Mock<IGLAnalysisProcessingService>();
            mockService.Setup(x =>
                    x.GetGLAnalysisSurveyCurveCoordinate(It.IsAny<WithCorrelationId<GLAnalysisCurveCoordinateInput>>()))
                .Returns(responseData);

            var controller = new SurveyCoordinatesController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            filters.Add("surveyDate", "2023-05-11 22:29:55.000");

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
            mockService.Verify(x => x.GetGLAnalysisSurveyCurveCoordinate(
                It.IsAny<WithCorrelationId<GLAnalysisCurveCoordinateInput>>()), Times.Once);
        }

        [TestMethod]
        public void GetSurveyCurveCoordinateResultEmptyPayloadTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            GLAnalysisCurveCoordinateDataOutput responseData = null;

            var mockService = new Mock<IGLAnalysisProcessingService>();
            mockService.Setup(x =>
                    x.GetGLAnalysisSurveyCurveCoordinate(It.IsAny<WithCorrelationId<GLAnalysisCurveCoordinateInput>>()))
                .Returns(responseData);

            var controller = new SurveyCoordinatesController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            filters.Add("surveyDate", "2023-05-11 22:29:55.000");

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
        }

        [TestMethod]
        public void GetSurveyCurveCoordinateResultInvalidInputTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            GLAnalysisCurveCoordinateDataOutput responseData = null;

            var mockService = new Mock<IGLAnalysisProcessingService>();
            mockService.Setup(x =>
                    x.GetGLAnalysisSurveyCurveCoordinate(It.IsAny<WithCorrelationId<GLAnalysisCurveCoordinateInput>>()))
                .Returns(responseData);

            var controller = new SurveyCoordinatesController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>();
            filters.Add("assetId", Guid.Empty.ToString());
            filters.Add("surveydate", "");

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.AreEqual(400, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(
                x => x.GetGLAnalysisSurveyCurveCoordinate(It.IsAny<WithCorrelationId<GLAnalysisCurveCoordinateInput>>()),
                Times.Never);
        }

        [TestMethod]
        public void GetSurveyCurveCoordinateResultNullResponseTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            GLAnalysisCurveCoordinateDataOutput responseData = null;

            var mockService = new Mock<IGLAnalysisProcessingService>();
            mockService.Setup(x =>
                    x.GetGLAnalysisSurveyCurveCoordinate(It.IsAny<WithCorrelationId<GLAnalysisCurveCoordinateInput>>()))
                .Returns(responseData);

            var controller = new SurveyCoordinatesController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            filters.Add("surveyDate", "2023-05-11 22:29:55.000");

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
        }

        [TestMethod]
        public void GetSurveyCurveCoordinateInvalidAssetIdTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IGLAnalysisProcessingService>();

            var controller = new SurveyCoordinatesController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>
            {
                {
                    "assetId", "abc"
                },
                {
                    "surveyDate", "2023-05-11 22:29:55.000"
                }
            };

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        #endregion

        [TestMethod]
        public void GetGLAnalysisInvalidAssetIdTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IGLAnalysisProcessingService>();

            var controller = new GLAnalysisController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>
            {
                {
                    "assetId", "abc"
                },
                {
                    "testDate", "2023-05-11 22:29:55.000"
                },
                {
                    "analysisTypeId", "1"
                }
            };

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        #endregion

    }
}
