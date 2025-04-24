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
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Core.Tests.Services
{
    [TestClass]
    public class WellTestsProcessingServiceTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLoggerTest()
        {
            var mockService = new Mock<IWellTests>();
            _ = new WellTestsProcessingService(null, mockService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullServiceTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            _ = new WellTestsProcessingService(mockThetaLoggerFactory.Object, null);
        }

        [TestMethod]
        public void GetESPWellTestResultsPassTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            DateTime dateTime = DateTime.Now;

            var responseData = GetESPWellTestData(dateTime);

            var wellTestsProcessingServiceMock = new Mock<IWellTests>();
            var wellTestInput = new WellTestInput()
            {
                AssetId = Guid.NewGuid()
            };
            var correlationId = Guid.NewGuid().ToString();
            var requestWithCorrelationId = new WithCorrelationId<WellTestInput>(correlationId, wellTestInput);
            wellTestsProcessingServiceMock.Setup(x => x.GetESPWellTestsData(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(responseData);
            var service = new WellTestsProcessingService(mockThetaLoggerFactory.Object, wellTestsProcessingServiceMock.Object);

            var result = service.GetESPAnalysisWellTestData(requestWithCorrelationId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(WellTestDataOutput));
            wellTestsProcessingServiceMock.Verify(x => x.GetESPWellTestsData(It.IsAny<Guid>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void GetESPWellTestResultsFailTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            DateTime dateTime = DateTime.Now;

            var responseData = GetWellFailTest();
            var wellTestInput = new WellTestInput()
            {
                AssetId = Guid.NewGuid()
            };
            var correlationId = Guid.NewGuid().ToString();
            var requestWithCorrelationId = new WithCorrelationId<WellTestInput>(correlationId, wellTestInput);

            var wellTestsProcessingService = new Mock<IWellTests>();
            wellTestsProcessingService.Setup(x => x.GetESPWellTestsData(It.IsAny<Guid>(), It.IsAny<string>())).Returns(responseData);
            var service = new WellTestsProcessingService(mockThetaLoggerFactory.Object, wellTestsProcessingService.Object);

            var result = service.GetESPAnalysisWellTestData(requestWithCorrelationId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(WellTestDataOutput));
            wellTestsProcessingService.Verify(x => x.GetESPWellTestsData(It.IsAny<Guid>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void GetGLAnalysisWellTestResultsPassTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            DateTime dateTime = DateTime.Now;

            var responseData = GLAnalysisResultModelKey();
            var wellTestInput = new WellTestInput()
            {
                AssetId = Guid.NewGuid()
            };
            var correlationId = Guid.NewGuid().ToString();
            var requestWithCorrelationId = new WithCorrelationId<WellTestInput>(correlationId, wellTestInput);

            var wellTestsProcessingService = new Mock<IWellTests>();
            wellTestsProcessingService
                .Setup(x => x.GetGLAnalysisWellTestsData(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(responseData);
            var service = new WellTestsProcessingService(mockThetaLoggerFactory.Object, wellTestsProcessingService.Object);

            var result = service.GetGLAnalysisWellTestData(requestWithCorrelationId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GLAnalysisWellTestDataOutput));
            wellTestsProcessingService.Verify(
                x => x.GetGLAnalysisWellTestsData(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()),
                Times.Once);
        }

        [TestMethod]
        public void GetGLAnalysisWellTestResultsFailTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            DateTime dateTime = DateTime.Now;

            var responseData = GLAnalysisResultModelFailTest();
            var wellTestInput = new WellTestInput()
            {
                AssetId = Guid.NewGuid()
            };
            var correlationId = Guid.NewGuid().ToString();
            var requestWithCorrelationId = new WithCorrelationId<WellTestInput>(correlationId, wellTestInput);
            var wellTestsProcessingService = new Mock<IWellTests>();
            wellTestsProcessingService
                .Setup(x => x.GetGLAnalysisWellTestsData(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(responseData);
            var service = new WellTestsProcessingService(mockThetaLoggerFactory.Object, wellTestsProcessingService.Object);

            var result = service.GetGLAnalysisWellTestData(requestWithCorrelationId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GLAnalysisWellTestDataOutput));
            wellTestsProcessingService.Verify(
                x => x.GetGLAnalysisWellTestsData(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()),
                Times.Once);
        }

        #endregion

        #region Private Methods

        private static List<WellTestModel> GetESPWellTestData(DateTime dateTime)
        {
            var wellTest = new List<WellTestModel>()
            {
                new WellTestModel()
                {
                    TestDate = dateTime,
                    Approved = true
                }
            };

            return wellTest;
        }

        private static List<WellTestModel> GetWellFailTest()
        {
            var wellTest = new List<WellTestModel>()
            {
            };

            return wellTest;
        }

        private static List<GLAnalysisResultModel> GLAnalysisResultModelKey()
        {
            var gLAnalysisResultModel = new List<GLAnalysisResultModel>()
            {
                new GLAnalysisResultModel()
                {
                    Id = 1,
                    AnalysisType = 1
                }
            };

            return gLAnalysisResultModel;
        }

        private static List<GLAnalysisResultModel> GLAnalysisResultModelFailTest()
        {
            var gLAnalysisResultModel = new List<GLAnalysisResultModel>()
            {
            };

            return gLAnalysisResultModel;
        }

        #endregion

    }
}
