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
    public class CardsControllerTests
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullCardDateTest()
        {

            _ = new CardsController(null, null);
        }

        [TestMethod]
        public void GetCardDateTest()
        {
            var loggerMock = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            var rodLiftAnalysisMockService = new Mock<IRodLiftAnalysisProcessingService>();

            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(loggerMock.Object);

            rodLiftAnalysisMockService
                .Setup(x => x.GetCardDate(It.IsAny<WithCorrelationId<CardDateInput>>()))
                .Returns(new CardDatesOutput()
                {
                    Values = new List<CardDateItem>()
                    {
                        new CardDateItem
                        {
                            Date = DateTime.UtcNow,
                            CardTypeId = "P",
                            CardTypeName = "",
                        },
                    },
                    Result = new MethodResult<string>(true, String.Empty),
                });

            var service = new CardsController(rodLiftAnalysisMockService.Object, mockThetaLoggerFactory.Object);
            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "E432CB3B-295C-4ECB-8737-90D5D76AC6CF");

            var result = service.GetCardDates(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.AreEqual(200, ((ObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetCardDateAssetIdNullResponseTest()
        {
            var loggerMock = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            var rodLiftAnalysisMockService = new Mock<IRodLiftAnalysisProcessingService>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(loggerMock.Object);

            rodLiftAnalysisMockService
                .Setup(x => x.GetCardDate(It.IsAny<WithCorrelationId<CardDateInput>>()))
                .Returns(new CardDatesOutput()
                {
                    Values = new List<CardDateItem>()
                    {
                        new CardDateItem
                        {
                            Date = DateTime.UtcNow,
                            CardTypeId = "P",
                            CardTypeName = "",
                        }
                    }
                });

            var service = new CardsController(rodLiftAnalysisMockService.Object, mockThetaLoggerFactory.Object);
            var filters = new Dictionary<string, string>();
            filters.Add("assetId", Guid.Empty.ToString());

            var result = service.GetCardDates(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            Assert.AreEqual(400, ((StatusCodeResult)result).StatusCode);
        }

        [TestMethod]
        public void GetCardDateEmptyResultsTest()
        {
            var loggerMock = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            var rodLiftAnalysisMockService = new Mock<IRodLiftAnalysisProcessingService>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(loggerMock.Object);

            rodLiftAnalysisMockService
                .Setup(x => x.GetCardDate(It.IsAny<WithCorrelationId<CardDateInput>>()))
                .Returns(new CardDatesOutput()
                {
                    Result = new MethodResult<string>(true, String.Empty)
                });

            var service = new CardsController(rodLiftAnalysisMockService.Object, mockThetaLoggerFactory.Object);
            var filters = new Dictionary<string, string>();
            filters.Add("assetId", "E431CB3B-295C-4ECB-8737-90D5D76AC6CF");

            var result = service.GetCardDates(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ObjectResult));
            Assert.AreEqual(200, ((ObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetCardDateMissingAssetIdTest()
        {
            var loggerMock = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(loggerMock.Object);

            var rodLiftAnalysisMockService = new Mock<IRodLiftAnalysisProcessingService>();
            var service = new CardsController(rodLiftAnalysisMockService.Object, mockThetaLoggerFactory.Object);
            var filters = new Dictionary<string, string>();

            var result = service.GetCardDates(filters);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void GetCardDateServiceResultNullTest()
        {
            var loggerMock = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(loggerMock.Object);

            var rodLiftAnalysisMockService = new Mock<IRodLiftAnalysisProcessingService>();
            rodLiftAnalysisMockService
                .Setup(x => x.GetCardDate(It.IsAny<WithCorrelationId<CardDateInput>>()))
                .Returns((CardDatesOutput)null);

            var service = new CardsController(rodLiftAnalysisMockService.Object, mockThetaLoggerFactory.Object);
            var filters = new Dictionary<string, string>
            {
                {
                    "assetId", "E432CB3B-295C-4ECB-8737-90D5D76AC6CF"
                }
            };

            var result = service.GetCardDates(filters);

            Assert.IsNotNull(result);
            Assert.AreEqual(500, ((StatusCodeResult)result).StatusCode);
        }

    }
}
