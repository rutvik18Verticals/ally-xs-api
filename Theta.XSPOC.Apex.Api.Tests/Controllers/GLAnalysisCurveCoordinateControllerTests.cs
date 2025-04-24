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
    public class GLAnalysisCurveCoordinateControllerTests
    {

        #region Test Methods

        [TestMethod]
        public void GetGLAnalysisCurveCoordinateResultsTest()
        {
            var mockService = new Mock<IGLAnalysisProcessingService>();
            GLAnalysisCurveCoordinateDataOutput gLAnalysisCurveCoordinateDataOutput = new GLAnalysisCurveCoordinateDataOutput()
            {
                Values = new List<GLAnalysisCurveCoordinateData>()
                {
                    new GLAnalysisCurveCoordinateData()
                    {
                        Id = 1,
                        CurveTypeId = 1,
                        DisplayName = "Test",
                        Name = "Test",
                        CoordinatesOutput = new List<CoordinatesData<float>>
                        {
                            new CoordinatesData<float>
                            {
                                X = 1,
                                Y = 1,
                            },
                            new CoordinatesData<float>
                            {
                                X = 2,
                                Y = 2,
                            },
                        }
                    }
                },
                Result = new MethodResult<string>(true, string.Empty),
            };

            mockService.Setup(x =>
                    x.GetGLAnalysisCurveCoordinateResults(It.IsAny<WithCorrelationId<GLAnalysisCurveCoordinateInput>>()))
                .Returns(gLAnalysisCurveCoordinateDataOutput);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var service = new GLCoordinatesController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "63F2ADD2-C79C-4B6B-A190-5CE6232E28F3");
            filters.Add("testDate", "2018-03-05");
            filters.Add("analysisResultId", "1");
            filters.Add("analysisTypeId", "1");

            var result = service.Get(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
            mockService.Verify(
                x => x.GetGLAnalysisCurveCoordinateResults(It.IsAny<WithCorrelationId<GLAnalysisCurveCoordinateInput>>()),
                Times.Once);
        }

        [TestMethod]
        public void GetGLAnalysisCurveCoordinateResultsNotFoundTest()
        {
            var mockService = new Mock<IGLAnalysisProcessingService>();
            GLAnalysisCurveCoordinateDataOutput gLAnalysisCurveCoordinateDataOutput = new GLAnalysisCurveCoordinateDataOutput()
            {
                Value = new GLAnalysisCurveCoordinateValues()
                {
                    Input = new List<ValueItem>()
                    {
                        new ValueItem()
                        {
                            Id = "AssetId",
                            Name = "AssetId",
                            Value = new Guid("63F2ADD2-C79C-4B6B-A190-5CE6232E28F3")
                        },
                        new ValueItem()
                        {
                            Id = "TestDate",
                            Name = "TestDate",
                            Value = DateTime.UtcNow.ToString()
                        },
                        new ValueItem()
                        {
                            Id = "AnalysisResultId",
                            Name = "AnalysisResultId",
                            Value = "1"
                        },
                        new ValueItem()
                        {
                            Id = "AnalysisResultId",
                            Name = "AnalysisResultId",
                            Value = "1"
                        },
                    },
                    Output = new List<ValueItem>()
                    {
                        new ValueItem()
                        {
                            Id = "Id",
                            Name = "Id",
                            Value = "1"
                        },
                        new ValueItem()
                        {
                            Id = "NodeID",
                            Name = "NodeID",
                            Value = "well"
                        },
                        new ValueItem()
                        {
                            Id = "TestDate",
                            Name = "TestDate",
                            Value = "2018-03-08"
                        },
                        new ValueItem()
                        {
                            Id = "ProcessedDate",
                            Name = "ProcessedDate",
                            Value = "2018-03-08"
                        },
                        new ValueItem()
                        {
                            Id = "Success",
                            Name = "Success",
                            Value = "1"
                        },
                    }
                }
            };
            mockService.Setup(x =>
                    x.GetGLAnalysisCurveCoordinateResults(It.IsAny<WithCorrelationId<GLAnalysisCurveCoordinateInput>>()))
                .Returns(gLAnalysisCurveCoordinateDataOutput);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var service = new GLCoordinatesController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "63F2ADD2-C79C-4B6B-A190-5CE6232E28F3");
            filters.Add("testDate", "");
            filters.Add("analysisResultId", "1");
            filters.Add("analysisTypeId", "1");

            var result = service.Get(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(400, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(
                x => x.GetGLAnalysisCurveCoordinateResults(It.IsAny<WithCorrelationId<GLAnalysisCurveCoordinateInput>>()),
                Times.Never);
        }

        [TestMethod]
        public void GetGLAnalysisCurveCoordinateResultsFiltersNullCheckTest()
        {
            var mockService = new Mock<IGLAnalysisProcessingService>();
            GLAnalysisCurveCoordinateDataOutput gLAnalysisCurveCoordinateDataOutput = new GLAnalysisCurveCoordinateDataOutput()
            {
                Value = new GLAnalysisCurveCoordinateValues()
                {
                    Input = new List<ValueItem>()
                    {
                        new ValueItem()
                        {
                            Id = "AssetId",
                            Name = "AssetId",
                            Value = new Guid("63F2ADD2-C79C-4B6B-A190-5CE6232E28F3")
                        },
                        new ValueItem()
                        {
                            Id = "TestDate",
                            Name = "TestDate",
                            Value = DateTime.UtcNow.ToString()
                        },
                        new ValueItem()
                        {
                            Id = "AnalysisResultId",
                            Name = "AnalysisResultId",
                            Value = "1"
                        },
                        new ValueItem()
                        {
                            Id = "AnalysisResultId",
                            Name = "AnalysisResultId",
                            Value = "1"
                        },
                    },
                    Output = new List<ValueItem>()
                    {
                        new ValueItem()
                        {
                            Id = "Id",
                            Name = "Id",
                            Value = "1"
                        },
                        new ValueItem()
                        {
                            Id = "NodeID",
                            Name = "NodeID",
                            Value = "well"
                        },
                        new ValueItem()
                        {
                            Id = "TestDate",
                            Name = "TestDate",
                            Value = "2018-03-08"
                        },
                        new ValueItem()
                        {
                            Id = "ProcessedDate",
                            Name = "ProcessedDate",
                            Value = "2018-03-08"
                        },
                        new ValueItem()
                        {
                            Id = "Success",
                            Name = "Success",
                            Value = "1"
                        },
                    }
                }
            };
            mockService.Setup(x => x.GetGLAnalysisCurveCoordinateResults(It.IsAny<WithCorrelationId<GLAnalysisCurveCoordinateInput>>())).Returns(gLAnalysisCurveCoordinateDataOutput);
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var service = new GLCoordinatesController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>();

            var result = service.Get(filters);

            Assert.IsNotNull(result);
            Assert.AreEqual(400, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(
                x => x.GetGLAnalysisCurveCoordinateResults(It.IsAny<WithCorrelationId<GLAnalysisCurveCoordinateInput>>()),
                Times.Never);
        }

        [TestMethod]
        public void GetGLAnalysisCurveCoordinateInvalidAssetIdTest()
        {
            var mockService = new Mock<IGLAnalysisProcessingService>();
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var service = new GLCoordinatesController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>
            {
                {
                    "assetId", "abc"
                },
                {
                    "testDate", "2018-03-05"
                },
                {
                    "analysisResultId", "1"
                },
                {
                    "analysisTypeId", "1"
                }
            };

            var result = service.Get(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void GetGLAnalysisCurveCoordinateInvalidServiceResultTest()
        {
            var mockService = new Mock<IGLAnalysisProcessingService>();
            mockService.Setup(x =>
                    x.GetGLAnalysisCurveCoordinateResults(It.IsAny<WithCorrelationId<GLAnalysisCurveCoordinateInput>>()))
                .Returns((GLAnalysisCurveCoordinateDataOutput)null);
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var service = new GLCoordinatesController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>
            {
                {
                    "assetId", "63F2ADD2-C79C-4B6B-A190-5CE6232E28F3"
                },
                {
                    "testDate", "2018-03-05"
                },
                {
                    "analysisResultId", "1"
                },
                {
                    "analysisTypeId", "1"
                }
            };

            var result = service.Get(filters);

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(
                x => x.GetGLAnalysisCurveCoordinateResults(It.IsAny<WithCorrelationId<GLAnalysisCurveCoordinateInput>>()),
                Times.Once);
        }

        #endregion

    }
}
