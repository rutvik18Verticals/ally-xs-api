using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Contracts.Responses;
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
    public class WellTestsControllerTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLoggerTest()
        {
            _ = new WellTestsController(null, null);
        }

        [TestMethod]
        public void GetESPWellTestsResultsTest()
        {
            var mockService = new Mock<IWellTestsProcessingService>();
            var values = new List<WellTestData>();

            values.Add(
                new WellTestData()
                {
                    Date = new DateTime(2023, 3, 1),
                    AnalysisTypeName = AnalysisType.WellTest
                });
            var methodResult = new MethodResult<string>(true, string.Empty);

            var responseData = new WellTestDataOutput
            {
                Values = values,
                Result = methodResult
            };

            mockService.Setup(x => x.GetESPAnalysisWellTestData(It.IsAny<WithCorrelationId<WellTestInput>>()))
                .Returns(responseData);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new WellTestsController(mockThetaLoggerFactory.Object, mockService.Object);

            var filter = new Dictionary<string, string>
            {
                {
                    "assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
                },
                {
                    "type", "esp"
                }
            };
            var result = controller.Get(filter);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
            mockService.Verify(x => x.GetESPAnalysisWellTestData(It.IsAny<WithCorrelationId<WellTestInput>>()), Times.Once);
            Assert.AreEqual(1, ((WellTestResponse)((OkObjectResult)result).Value).Values.Count);
        }

        [TestMethod]
        public void GetGLAnalysisWellTestsResultsTest()
        {
            var mockService = new Mock<IWellTestsProcessingService>();
            var values = new List<GLAnalysisWellTestData>();
            values.Add(
                new GLAnalysisWellTestData()
                {
                    Date = new DateTime(2023, 3, 1),
                    AnalysisTypeName = AnalysisType.WellTest,
                    AnalysisResultId = 1,
                    AnalysisTypeId = 1,
                });
            var methodResult = new MethodResult<string>(true, string.Empty);

            var responseData = new GLAnalysisWellTestDataOutput
            {
                Values = values,
                Result = methodResult
            };

            mockService.Setup(x => x.GetGLAnalysisWellTestData(It.IsAny<WithCorrelationId<WellTestInput>>()))
                .Returns(responseData);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new WellTestsController(mockThetaLoggerFactory.Object, mockService.Object);

            var filter = new Dictionary<string, string>
            {
                {
                    "assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
                },
                {
                    "type", "gl"
                }
            };
            var result = controller.Get(filter);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
            mockService.Verify(x => x.GetGLAnalysisWellTestData(It.IsAny<WithCorrelationId<WellTestInput>>()), Times.Once);
            Assert.AreEqual(1, ((GLAnalysisWellTestResponse)((OkObjectResult)result).Value).Values.Count);
        }

        [TestMethod]
        public void GetWellTestsResultsInvalidInputTest()
        {
            var mockService = new Mock<IWellTestsProcessingService>();
            var values = new List<WellTestData>();

            values.Add(
                new WellTestData()
                {
                    Date = new DateTime(2023, 3, 1),
                    AnalysisTypeName = AnalysisType.WellTest
                });
            var methodResult = new MethodResult<string>(true, string.Empty);

            var responseData = new WellTestDataOutput
            {
                Values = values,
                Result = methodResult
            };

            mockService.Setup(x => x.GetESPAnalysisWellTestData(It.IsAny<WithCorrelationId<WellTestInput>>()))
                .Returns(responseData);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new WellTestsController(mockThetaLoggerFactory.Object, mockService.Object);

            var filter = new Dictionary<string, string>
            {
                {
                    "id", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
                },
                {
                    "type", "test"
                }
            };
            var result = controller.Get(filter);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            Assert.AreEqual(400, ((BadRequestResult)result).StatusCode);
            mockService.Verify(x => x.GetESPAnalysisWellTestData(It.IsAny<WithCorrelationId<WellTestInput>>()), Times.Never);
        }

        [TestMethod]
        public void GetWellTestsResultsInvalidTypeTest()
        {
            var mockService = new Mock<IWellTestsProcessingService>();
            var values = new List<WellTestData>();

            values.Add(
                new WellTestData()
                {
                    Date = new DateTime(2023, 3, 1),
                    AnalysisTypeName = AnalysisType.WellTest
                });
            var methodResult = new MethodResult<string>(true, string.Empty);

            var responseData = new WellTestDataOutput
            {
                Values = values,
                Result = methodResult
            };

            mockService.Setup(x => x.GetESPAnalysisWellTestData(It.IsAny<WithCorrelationId<WellTestInput>>()))
                .Returns(responseData);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new WellTestsController(mockThetaLoggerFactory.Object, mockService.Object);

            var filter = new Dictionary<string, string>
            {
                {
                    "assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
                },
                {
                    "type", "test"
                }
            };
            var result = controller.Get(filter);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            Assert.AreEqual(400, ((BadRequestResult)result).StatusCode);
            mockService.Verify(x => x.GetESPAnalysisWellTestData(It.IsAny<WithCorrelationId<WellTestInput>>()), Times.Never);
        }

        [TestMethod]
        public void GetWellTestsESPResultsNullResponseTest()
        {
            var mockService = new Mock<IWellTestsProcessingService>();

            var methodResult = new MethodResult<string>(true, string.Empty);

            mockService.Setup(x => x.GetESPAnalysisWellTestData(It.IsAny<WithCorrelationId<WellTestInput>>()));

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new WellTestsController(mockThetaLoggerFactory.Object, mockService.Object);

            var filter = new Dictionary<string, string>
            {
                {
                    "assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
                },
                {
                    "type", "esp"
                }
            };
            var result = controller.Get(filter);

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(x => x.GetESPAnalysisWellTestData(It.IsAny<WithCorrelationId<WellTestInput>>()), Times.Once);
        }

        [TestMethod]
        public void GetWellTestsGLResultsNullResponseTest()
        {
            var mockService = new Mock<IWellTestsProcessingService>();

            var methodResult = new MethodResult<string>(true, string.Empty);

            mockService.Setup(x => x.GetGLAnalysisWellTestData(It.IsAny<WithCorrelationId<WellTestInput>>()));

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new WellTestsController(mockThetaLoggerFactory.Object, mockService.Object);

            var filter = new Dictionary<string, string>
            {
                {
                    "assetId", "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
                },
                {
                    "type", "gl"
                }
            };
            var result = controller.Get(filter);

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
            mockService.Verify(x => x.GetGLAnalysisWellTestData(It.IsAny<WithCorrelationId<WellTestInput>>()), Times.Once);
        }

        [TestMethod]
        public void GetWellTestInvalidAssetIdTest()
        {
            var mockService = new Mock<IWellTestsProcessingService>();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new WellTestsController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>
            {
                {
                    "assetId", "abc"
                },
                {
                    "type", "esp"
                }
            };

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void GetCardCoordinateInvalidAssetIdTest()
        {
            var mockService = new Mock<IWellTestsProcessingService>();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var controller = new WellTestsController(mockThetaLoggerFactory.Object, mockService.Object);

            var filters = new Dictionary<string, string>
            {
                {
                    "assetId", "abc"
                },
                {
                    "type", "esp"
                }
            };

            var result = controller.Get(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        #endregion

    }
}
