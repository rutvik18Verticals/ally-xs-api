using Azure;
using MathNet.Numerics.Statistics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using GLAnalysisInput = Theta.XSPOC.Apex.Api.Core.Models.Inputs.GLAnalysisInput;

namespace Theta.XSPOC.Apex.Api.Core.Tests.Services
{
    [TestClass]
    public class GLAnalysisProcessingServiceTests
    {

        #region Private Fields

        private Mock<ICommonService> _commonService;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _commonService = new Mock<ICommonService>();

            SetupCommonService();
        }

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLoggerTest()
        {
            var mockService = new Mock<IGLAnalysis>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockWellAnalysisCorrelation = new Mock<IWellAnalysisCorrelation>();

            var mockManufacturer = new Mock<IManufacturer>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var mockGLAnalysisService = new Mock<IGLAnalysisGetCurveCoordinate>();

            _ = new GLAnalysisProcessingService(mockService.Object, null, mockLocalePhrase.Object,
                mockWellAnalysisCorrelation.Object, mockManufacturer.Object, mockAnalysisCurve.Object,
                new Mock<IAnalysisCurveSets>().Object, mockGLAnalysisService.Object, _commonService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullGLAnalysisServiceTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockWellAnalysisCorrelation = new Mock<IWellAnalysisCorrelation>();

            var mockManufacturer = new Mock<IManufacturer>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var mockService = new Mock<IGLAnalysisGetCurveCoordinate>();

            _ = new GLAnalysisProcessingService(null, mockThetaLoggerFactory.Object, mockLocalePhrase.Object,
                mockWellAnalysisCorrelation.Object, mockManufacturer.Object, mockAnalysisCurve.Object,
                new Mock<IAnalysisCurveSets>().Object, mockService.Object, _commonService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLocalePhraseProcessingServiceTest()
        {
            var mockService = new Mock<IGLAnalysis>();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockWellAnalysisCorrelation = new Mock<IWellAnalysisCorrelation>();

            var mockManufacturer = new Mock<IManufacturer>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var mockGLAnalysisService = new Mock<IGLAnalysisGetCurveCoordinate>();

            _ = new GLAnalysisProcessingService(mockService.Object, mockThetaLoggerFactory.Object, null,
                mockWellAnalysisCorrelation.Object, mockManufacturer.Object, mockAnalysisCurve.Object,
                new Mock<IAnalysisCurveSets>().Object, mockGLAnalysisService.Object, _commonService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullGLPumpServiceTest()
        {
            var mockService = new Mock<IGLAnalysis>();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockManufacturer = new Mock<IManufacturer>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var mockGLAnalysisService = new Mock<IGLAnalysisGetCurveCoordinate>();

            _ = new GLAnalysisProcessingService(mockService.Object, mockThetaLoggerFactory.Object, mockLocalePhrase.Object,
                null, mockManufacturer.Object, mockAnalysisCurve.Object,
                new Mock<IAnalysisCurveSets>().Object, mockGLAnalysisService.Object, _commonService.Object);
        }

        [TestMethod]
        public void GetGLAnalysisAssetIdTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockLocalePhrase = new Mock<ILocalePhrases>();
            var mockManufacturer = new Mock<IManufacturer>();
            var mockWellAnalysisCorrelation = new Mock<IWellAnalysisCorrelation>();
            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var events = new List<GlAnalysisSurveyDateModel>()
            {
                new GlAnalysisSurveyDateModel()
                {
                    SurveyDate = DateTime.Now,
                }
            };

            var mockGLAnalysisService = new Mock<IGLAnalysis>();
            var correlationId = Guid.NewGuid().ToString();

            var id = new Guid("66FC0809-091C-4A65-94FA-0394C3BEE1FB");
            mockGLAnalysisService.Setup(x => x.GetGLAnalysisSurveyDate(id, 7, 25, 26, correlationId))
                .Returns(events);

            var industryApplication = new Mock<IndustryApplication>(7, Text.FromString("7"));

            var mockAnalysisService = new Mock<IGLAnalysisGetCurveCoordinate>();

            var service = new GLAnalysisProcessingService(mockGLAnalysisService.Object, mockThetaLoggerFactory.Object,
                mockLocalePhrase.Object,
                mockWellAnalysisCorrelation.Object, mockManufacturer.Object, mockAnalysisCurve.Object,
                new Mock<IAnalysisCurveSets>().Object, mockAnalysisService.Object, _commonService.Object);

            var payload = new GLSurveyAnalysisInput
            {
                Guid = id
            };

            var message = new WithCorrelationId<GLSurveyAnalysisInput>(correlationId, payload);
            var result = service.GetGLAnalysisSurveyDate(message);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GLAnalysisSurveyDataOutput));
            mockGLAnalysisService.Verify(x => x.GetGLAnalysisSurveyDate(id, 7, 25, 26, correlationId), Times.Once);
        }

        [TestMethod]
        public void GetGLAnalysisNullMessageTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockLocalePhrase = new Mock<ILocalePhrases>();
            var mockManufacturer = new Mock<IManufacturer>();
            var mockWellAnalysisCorrelation = new Mock<IWellAnalysisCorrelation>();
            var mockAnalysisCurve = new Mock<IAnalysisCurve>();
            var correlationId = Guid.NewGuid().ToString();

            var mockGLAnalysisService = new Mock<IGLAnalysis>();
            var id = new Guid("66FC0809-091C-4A65-94FA-0394C3BEE1FB");
            mockGLAnalysisService.Setup(x => x.GetGLAnalysisSurveyDate(id, 1, 3, 4, correlationId));

            var mockService = new Mock<IGLAnalysisGetCurveCoordinate>();

            var service = new GLAnalysisProcessingService(mockGLAnalysisService.Object, mockThetaLoggerFactory.Object,
                mockLocalePhrase.Object,
                mockWellAnalysisCorrelation.Object, mockManufacturer.Object, mockAnalysisCurve.Object,
                new Mock<IAnalysisCurveSets>().Object, mockService.Object, _commonService.Object);

            var message = new WithCorrelationId<GLSurveyAnalysisInput>(correlationId, null);
            var result = service.GetGLAnalysisSurveyDate(message);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Result.Status);
            mockGLAnalysisService.Verify(x => x.GetGLAnalysisSurveyDate(id, 1, 3, 4, correlationId), Times.Never);
            logger.Verify(x => x.WriteCId(Level.Info,
                    It.Is<string>(x => x.Contains("requestWithCorrelationId is null, cannot get GLAnalysis survey date.")),
                    correlationId),
                Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetGLAnalysisResultsTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockWellAnalysisCorrelation = new Mock<IWellAnalysisCorrelation>();
            mockWellAnalysisCorrelation.Setup(x => x.GetAnalysisCorrelation(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new AnalysisCorrelationModel
                {
                    CorrelationId = 1,
                    CorrelationTypeId = 1,
                    Id = 1
                });

            var phraseIds = SetupPhraseIds();
            var phrases = SetupPhrases();

            var mockLocalePhrase = new Mock<ILocalePhrases>();
            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseIds)).Returns(phrases);

            var mockManufacturer = new Mock<IManufacturer>();
            mockManufacturer.Setup(m => m.Get(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new GLManufacturerModel
                {
                    Manufacturer = "PCS",
                    ManufacturerID = 1
                });

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var nodeData = new List<NodeMasterModel>()
            {
                new NodeMasterModel()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();

            var responseData = new GLAnalysisResponse()
            {
                NodeMasterData = nodeData.FirstOrDefault(),
                AnalysisResultEntity = GetAnalysisResultData(),
                ValveStatusEntities = GetValveStatusResultData(),
                WellDetail = GetWellDetail(),
                WellOrificeStatus = GetWellOrificeData(),
                WellValveEntities = GetValveResultData()
            };

            var correlationId = Guid.NewGuid().ToString();
            var mockGLAnalysisService = new Mock<IGLAnalysis>();
            mockGLAnalysisService.Setup(x =>
                    x.GetGLAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                    "2023-05-11 22:29:55.000", 1, correlationId))
                .Returns(responseData);

            var mockService = new Mock<IGLAnalysisGetCurveCoordinate>();

            var service = new GLAnalysisProcessingService(mockGLAnalysisService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockWellAnalysisCorrelation.Object,
                mockManufacturer.Object, mockAnalysisCurve.Object,
                new Mock<IAnalysisCurveSets>().Object, mockService.Object, _commonService.Object);

            var request = new WithCorrelationId<GLAnalysisInput>(correlationId, new GLAnalysisInput
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                TestDate = "2023-05-11 22:29:55.000",
                AnalysisTypeId = 1,
                AnalysisResultId = 1,
            });

            var result = service.GetGLAnalysisResults(request);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GLAnalysisDataOutput));
            Assert.AreEqual(true, result.Result.Status);
            mockGLAnalysisService.Verify(x => x.GetGLAnalysisData
                    (new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000", 1, correlationId),
                Times.Once);
            var flowControlDeviceAnalysisValues = (List<FlowControlDeviceAnalysisValues>)result.Values.Valves;
            var countOfOrificeValues = flowControlDeviceAnalysisValues.Count(a => a.Description == "Orifice");
            Assert.IsTrue(countOfOrificeValues > 0);
        }

        [TestMethod]
        public void GetGLAnalysisResultsNullDataTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockWellAnalysisCorrelation = new Mock<IWellAnalysisCorrelation>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();
            var mockManufacturer = new Mock<IManufacturer>();
            mockManufacturer.Setup(m => m.Get(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new GLManufacturerModel
                {
                    Manufacturer = "PCS",
                    ManufacturerID = 1
                });
            var mockAnalysisCurve = new Mock<IAnalysisCurve>();
            var mockGLAnalysisService = new Mock<IGLAnalysis>();
            var correlationId = Guid.NewGuid().ToString();
            mockGLAnalysisService.Setup(x =>
                x.GetGLAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                "2023-05-11 22:29:55.000", 1, correlationId));

            var mockService = new Mock<IGLAnalysisGetCurveCoordinate>();

            var service = new GLAnalysisProcessingService(mockGLAnalysisService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockWellAnalysisCorrelation.Object,
                mockManufacturer.Object, mockAnalysisCurve.Object,
                new Mock<IAnalysisCurveSets>().Object, mockService.Object, _commonService.Object);

            var result = service.GetGLAnalysisResults(null);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Result.Status);
            logger.Verify(x => x.Write(Level.Info,
                    It.Is<string>(x => x.Contains("data is null, cannot get gas lift analysis results."))),
                Times.AtLeastOnce);
            mockGLAnalysisService.Verify(x => x.GetGLAnalysisData
                (new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                "2023-05-11 22:29:55.000", 1, correlationId), Times.Never);
        }

        [TestMethod]
        public void GetGLAnalysisResultsNullPayloadValueTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockWellAnalysisCorrelation = new Mock<IWellAnalysisCorrelation>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();
            var mockManufacturer = new Mock<IManufacturer>();
            var mockAnalysisCurve = new Mock<IAnalysisCurve>();
            var correlationId = Guid.NewGuid().ToString();

            var mockGLAnalysisService = new Mock<IGLAnalysis>();
            mockGLAnalysisService.Setup(x =>
                x.GetGLAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                "2023-05-11 22:29:55.000", 1, correlationId));

            var mockService = new Mock<IGLAnalysisGetCurveCoordinate>();

            var service = new GLAnalysisProcessingService(mockGLAnalysisService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockWellAnalysisCorrelation.Object,
                mockManufacturer.Object, mockAnalysisCurve.Object,
                new Mock<IAnalysisCurveSets>().Object, mockService.Object, _commonService.Object);

            var request = new WithCorrelationId<GLAnalysisInput>(correlationId, null);

            var result = service.GetGLAnalysisResults(request);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Result.Status);
            logger.Verify(x => x.WriteCId(Level.Info,
                    It.Is<string>(x => x.Contains("data is null, cannot get gas lift analysis results.")), correlationId),
                Times.AtLeastOnce);
            mockGLAnalysisService.Verify(x => x.GetGLAnalysisData
                (new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000", 1, correlationId), Times.Never);
        }

        [TestMethod]
        public void GetGLAnalysisResultsNullAssetIdOrTestDateTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockWellAnalysisCorrelation = new Mock<IWellAnalysisCorrelation>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();
            var mockManufacturer = new Mock<IManufacturer>();
            var mockAnalysisCurve = new Mock<IAnalysisCurve>();
            var correlationId = Guid.NewGuid().ToString();

            var mockGLAnalysisService = new Mock<IGLAnalysis>();
            mockGLAnalysisService.Setup(x =>
                x.GetGLAnalysisData(Guid.Empty, "2023 -05-11 22:29:55.000", 1, correlationId));

            var mockService = new Mock<IGLAnalysisGetCurveCoordinate>();

            var service = new GLAnalysisProcessingService(mockGLAnalysisService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockWellAnalysisCorrelation.Object,
                mockManufacturer.Object, mockAnalysisCurve.Object,
                new Mock<IAnalysisCurveSets>().Object, mockService.Object, _commonService.Object);

            var request = new WithCorrelationId<GLAnalysisInput>(correlationId, new GLAnalysisInput
            {
                AssetId = Guid.Empty,
                TestDate = "",
                AnalysisTypeId = 1,
                AnalysisResultId = 1,
            });

            var result = service.GetGLAnalysisResults(request);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Result.Status);
            logger.Verify(x => x.WriteCId(Level.Info,
                    It.Is<string>(x => x.Contains("TestDate and AssetId should be provided to get gas lift analysis results.")),
                    correlationId),
                Times.AtLeastOnce);
            mockGLAnalysisService.Verify(x => x.GetGLAnalysisData(Guid.Empty,
                "2023-05-11 22:29:55.000", 1, correlationId), Times.Never);
        }

        [TestMethod]
        public void GetGLAnalysisResultsNullWellDetailsResponseTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockWellAnalysisCorrelation = new Mock<IWellAnalysisCorrelation>();
            mockWellAnalysisCorrelation.Setup(x => x.GetAnalysisCorrelation(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new AnalysisCorrelationModel
                {
                    CorrelationId = 1,
                    CorrelationTypeId = 1,
                    Id = 1
                });
            var mockManufacturer = new Mock<IManufacturer>();
            mockManufacturer.Setup(m => m.Get(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new GLManufacturerModel
                {
                    Manufacturer = "PCS",
                    ManufacturerID = 1
                });

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();
            mockLocalePhrase.Setup(x => x.Get(7048, It.IsAny<string>()))
                .Returns(
                    "XDIAG has determined the pumping unit can increase speed to {0} SPM. This will result in an additional {1} barrels of gross and {2} barrels of oil.");
            mockLocalePhrase.Setup(x => x.Get(7049, It.IsAny<string>()))
                .Returns(
                    "XDIAG has determined that the pumping unit is already operating close to maximum speed for rod pump system. In order to capture potential uplift, design changes will be necessary.");
            mockLocalePhrase.Setup(x => x.Get(7117, It.IsAny<string>()))
                .Returns("Incremental production at {0} SPM is less than {1} bbls of oil per day.");
            mockLocalePhrase.Setup(x => x.Get(6819, It.IsAny<string>()))
                .Returns(
                    "An incremental oil uplift opportunity has been discovered for this well test, this well is capable of producing an additional {0} ({1}) of total fluid, resulting in an additional {2} ({3}) of oil.");
            mockLocalePhrase.Setup(x => x.Get(200, It.IsAny<string>())).Returns("N/A");
            mockLocalePhrase.Setup(x => x.Get(2091, It.IsAny<string>())).Returns("SWT Oil Yesterday");
            mockLocalePhrase.Setup(x => x.Get(2092, It.IsAny<string>())).Returns("SWT Water Yesterday");
            mockLocalePhrase.Setup(x => x.Get(2093, It.IsAny<string>())).Returns("SWT Gas Yesterday");

            var nodeData = new List<NodeMasterModel>()
            {
                new NodeMasterModel()
                {
                    NodeId = "Well1",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();

            var responseData = new GLAnalysisResponse()
            {
                NodeMasterData = nodeData.FirstOrDefault(),
                WellDetail = null,
                AnalysisResultEntity = GetAnalysisResultData(),
                ValveStatusEntities = GetValveStatusResultData(),
                WellOrificeStatus = GetWellOrificeData(),
                WellValveEntities = GetValveResultData()
            };

            var mockGLAnalysisService = new Mock<IGLAnalysis>();
            var correlationId = Guid.NewGuid().ToString();
            mockGLAnalysisService.Setup(x =>
                    x.GetGLAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                    "2023-05-11 22:29:55.000", 1, correlationId))
                .Returns(responseData);

            var mockService = new Mock<IGLAnalysisGetCurveCoordinate>();

            var service = new GLAnalysisProcessingService(mockGLAnalysisService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockWellAnalysisCorrelation.Object,
                mockManufacturer.Object, mockAnalysisCurve.Object,
                new Mock<IAnalysisCurveSets>().Object, mockService.Object, _commonService.Object);

            var request = new WithCorrelationId<GLAnalysisInput>(correlationId, new GLAnalysisInput
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                TestDate = "2023-05-11 22:29:55.000",
                AnalysisTypeId = 1,
                AnalysisResultId = 1
            });

            var response = service.GetGLAnalysisResults(request);

            Assert.AreEqual(false, response.Result.Status);
            logger.Verify(x => x.WriteCId(Level.Info,
                    It.Is<string>(x => x.Contains("WellDetail is null, cannot get gas lift analysis results.")),
                    correlationId),
                Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetGLAnalysisResultsNullWellOrificeResponseTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockWellAnalysisCorrelation = new Mock<IWellAnalysisCorrelation>();
            mockWellAnalysisCorrelation.Setup(x => x.GetAnalysisCorrelation(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new AnalysisCorrelationModel
                {
                    CorrelationId = 1,
                    CorrelationTypeId = 1,
                    Id = 1
                });
            var mockManufacturer = new Mock<IManufacturer>();
            mockManufacturer.Setup(m => m.Get(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new GLManufacturerModel
                {
                    Manufacturer = "PCS",
                    ManufacturerID = 1
                });

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var phraseIds = SetupPhraseIds();
            var phrases = SetupPhrases();

            var mockLocalePhrase = new Mock<ILocalePhrases>();
            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseIds)).Returns(phrases);

            var nodeData = new List<NodeMasterModel>()
            {
                new NodeMasterModel()
                {
                    NodeId = "Well1",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();

            var responseData = new GLAnalysisResponse()
            {
                NodeMasterData = nodeData.FirstOrDefault(),
                WellDetail = GetWellDetail(),
                AnalysisResultEntity = GetAnalysisResultData(),
                ValveStatusEntities = GetValveStatusResultData(),
                WellOrificeStatus = null,
                WellValveEntities = GetValveResultData()
            };

            var mockGLAnalysisService = new Mock<IGLAnalysis>();
            var correlationId = Guid.NewGuid().ToString();
            mockGLAnalysisService.Setup(x =>
                    x.GetGLAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                    "2023-05-11 22:29:55.000", 1, correlationId))
                .Returns(responseData);

            var mockService = new Mock<IGLAnalysisGetCurveCoordinate>();

            var service = new GLAnalysisProcessingService(mockGLAnalysisService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockWellAnalysisCorrelation.Object,
                mockManufacturer.Object, mockAnalysisCurve.Object,
                new Mock<IAnalysisCurveSets>().Object, mockService.Object, _commonService.Object);

            var request = new WithCorrelationId<GLAnalysisInput>(correlationId, new GLAnalysisInput
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                TestDate = "2023-05-11 22:29:55.000",
                AnalysisTypeId = 1,
                AnalysisResultId = 1
            });

            var response = service.GetGLAnalysisResults(request);

            Assert.AreEqual(true, response.Result.Status);

            var flowControlDeviceAnalysisValues = (List<FlowControlDeviceAnalysisValues>)response.Values.Valves;
            var countOfOrificeValues = flowControlDeviceAnalysisValues.Count(a => a.Description == "Orifice");
            Assert.IsTrue(countOfOrificeValues == 0);
        }

        [TestMethod]
        public void GetGLAnalysisResultsNullAnalysisResultEntityResponseTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockWellAnalysisCorrelation = new Mock<IWellAnalysisCorrelation>();
            mockWellAnalysisCorrelation.Setup(x => x.GetAnalysisCorrelation(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new AnalysisCorrelationModel
                {
                    CorrelationId = 1,
                    CorrelationTypeId = 1,
                    Id = 1
                });
            var mockManufacturer = new Mock<IManufacturer>();
            mockManufacturer.Setup(m => m.Get(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new GLManufacturerModel
                {
                    Manufacturer = "PCS",
                    ManufacturerID = 1
                });

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();
            var mockLocalePhrase = new Mock<ILocalePhrases>();
            mockLocalePhrase.Setup(x => x.Get(7048, It.IsAny<string>()))
                .Returns(
                    "XDIAG has determined the pumping unit can increase speed to {0} SPM. This will result in an additional {1} barrels of gross and {2} barrels of oil.");
            mockLocalePhrase.Setup(x => x.Get(7049, It.IsAny<string>()))
                .Returns(
                    "XDIAG has determined that the pumping unit is already operating close to maximum speed for rod pump system. In order to capture potential uplift, design changes will be necessary.");
            mockLocalePhrase.Setup(x => x.Get(7117, It.IsAny<string>()))
                .Returns("Incremental production at {0} SPM is less than {1} bbls of oil per day.");
            mockLocalePhrase.Setup(x => x.Get(6819, It.IsAny<string>()))
                .Returns(
                    "An incremental oil uplift opportunity has been discovered for this well test, this well is capable of producing an additional {0} ({1}) of total fluid, resulting in an additional {2} ({3}) of oil.");
            mockLocalePhrase.Setup(x => x.Get(200, It.IsAny<string>())).Returns("N/A");
            mockLocalePhrase.Setup(x => x.Get(2091, It.IsAny<string>())).Returns("SWT Oil Yesterday");
            mockLocalePhrase.Setup(x => x.Get(2092, It.IsAny<string>())).Returns("SWT Water Yesterday");
            mockLocalePhrase.Setup(x => x.Get(2093, It.IsAny<string>())).Returns("SWT Gas Yesterday");

            var nodeData = new List<NodeMasterModel>()
            {
                new NodeMasterModel()
                {
                    NodeId = "Well1",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();

            var responseData = new GLAnalysisResponse()
            {
                NodeMasterData = nodeData.FirstOrDefault(),
                WellDetail = GetWellDetail(),
                AnalysisResultEntity = null,
                ValveStatusEntities = GetValveStatusResultData(),
                WellOrificeStatus = GetWellOrificeData(),
                WellValveEntities = GetValveResultData()
            };

            var mockGLAnalysisService = new Mock<IGLAnalysis>();
            var correlationId = Guid.NewGuid().ToString();
            mockGLAnalysisService.Setup(x =>
                    x.GetGLAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                    "2023-05-11 22:29:55.000", 1, correlationId))
                .Returns(responseData);

            var mockService = new Mock<IGLAnalysisGetCurveCoordinate>();

            var service = new GLAnalysisProcessingService(mockGLAnalysisService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockWellAnalysisCorrelation.Object,
                mockManufacturer.Object, mockAnalysisCurve.Object,
                new Mock<IAnalysisCurveSets>().Object, mockService.Object, _commonService.Object);

            var request = new WithCorrelationId<GLAnalysisInput>(correlationId, new GLAnalysisInput
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                TestDate = "2023-05-11 22:29:55.000",
                AnalysisTypeId = 1,
                AnalysisResultId = 1
            });

            var response = service.GetGLAnalysisResults(request);

            Assert.AreEqual(false, response.Result.Status);
            logger.Verify(x => x.WriteCId(Level.Info,
                    It.Is<string>(x => x.Contains("AnalysisResultEntity is null, cannot get gas lift analysis results.")),
                    correlationId),
                Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetGLAnalysisCurveCoordinateResultsPassTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var responseData = new List<AnalysisResultCurvesModel>()
            {
                new AnalysisResultCurvesModel
                {
                    Id = 1,
                    AnalysisResultCurveID = 1,
                    CurveTypesID = 1,
                    Name = "Test",
                    Coordinates = new[]
                    {
                        new Coordinates()
                        {
                            X = 10,
                            Y = 10
                        }
                    }
                }
            };
            var gLAnalysisCurveCoordinateInput = new GLAnalysisCurveCoordinateInput()
            {
                AssetId = new Guid("63F2ADD2-C79C-4B6B-A190-5CE6232E28F3"),
                TestDate = "01-30-2024 10:59:18",
                AnalysisResultId = 1,
                AnalysisTypeId = 1
            };

            var nodeData = new List<NodeMasterModel>()
            {
                new NodeMasterModel()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();

            var glAnalysisData = new GLAnalysisResponse()
            {
                NodeMasterData = nodeData.FirstOrDefault(),
                AnalysisResultEntity = GetAnalysisResultData(),
                ValveStatusEntities = GetValveStatusResultData(),
                WellDetail = GetWellDetail(),
                WellOrificeStatus = GetWellOrificeData(),
                WellValveEntities = GetValveResultData()
            };

            var mockService = new Mock<IGLAnalysis>();
            var correlationId = Guid.NewGuid().ToString();
            mockService.Setup(x =>
                    x.GetGLAnalysisData(new Guid("63f2add2-c79c-4b6b-a190-5ce6232e28f3"),
                    "01-30-2024 10:59:18", 1, correlationId))
                .Returns(glAnalysisData);

            var phraseIds = SetupPhraseIds();
            var phrases = SetupPhrases();

            var mockLocalePhrase = new Mock<ILocalePhrases>();
            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseIds)).Returns(phrases);

            var requestWithCorrelationId = new WithCorrelationId<GLAnalysisCurveCoordinateInput>
                (correlationId, gLAnalysisCurveCoordinateInput);

            var mockWellAnalysisCorrelation = new Mock<IWellAnalysisCorrelation>();

            var mockManufacturer = new Mock<IManufacturer>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var gLAnalysisGetCurveCoordinate = new Mock<IGLAnalysisGetCurveCoordinate>();
            gLAnalysisGetCurveCoordinate
                .Setup(x => x.GetAnalysisResultCurve(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(responseData);

            var service = new GLAnalysisProcessingService(mockService.Object, mockThetaLoggerFactory.Object,
                mockLocalePhrase.Object,
                mockWellAnalysisCorrelation.Object, mockManufacturer.Object, mockAnalysisCurve.Object,
                new Mock<IAnalysisCurveSets>().Object, gLAnalysisGetCurveCoordinate.Object, _commonService.Object);

            var result = service.GetGLAnalysisCurveCoordinateResults(requestWithCorrelationId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GLAnalysisCurveCoordinateDataOutput));
            gLAnalysisGetCurveCoordinate.Verify(x => x.GetAnalysisResultCurve(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            gLAnalysisGetCurveCoordinate.Verify(x => x.GetGLAnalysisIPRCurveCoordinate(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Once);
            Assert.IsTrue(result.Values.Count > 0);
        }

        [TestMethod]
        public void GetGLAnalysisCurveCoordinateResultsFailTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var responseData = new GLAnalysisResultModel()
            {
                Id = 1,
                AnalysisType = 1,
                NodeId = "1",
            };
            var gLAnalysisCurveCoordinateInput = new GLAnalysisCurveCoordinateInput()
            {
                AssetId = new Guid("63F2ADD2-C79C-4B6B-A190-5CE6232E28F3"),
                TestDate = DateTime.UtcNow.ToString(),
                AnalysisResultId = 1,
                AnalysisTypeId = 1
            };

            var correlationId = Guid.NewGuid().ToString();
            var requestWithCorrelationId = new WithCorrelationId<GLAnalysisCurveCoordinateInput>
                (correlationId, gLAnalysisCurveCoordinateInput);

            var mockService = new Mock<IGLAnalysis>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockWellAnalysisCorrelation = new Mock<IWellAnalysisCorrelation>();

            var mockManufacturer = new Mock<IManufacturer>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var gLAnalysisGetCurveCoordinate = new Mock<IGLAnalysisGetCurveCoordinate>();
            gLAnalysisGetCurveCoordinate
                .Setup(x => x.GetAnalysisResultCurve(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(It.IsAny<List<AnalysisResultCurvesModel>>());

            var service = new GLAnalysisProcessingService(mockService.Object, mockThetaLoggerFactory.Object,
                mockLocalePhrase.Object,
                mockWellAnalysisCorrelation.Object, mockManufacturer.Object, mockAnalysisCurve.Object,
                new Mock<IAnalysisCurveSets>().Object, gLAnalysisGetCurveCoordinate.Object, _commonService.Object);

            var result = service.GetGLAnalysisCurveCoordinateResults(requestWithCorrelationId);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Values);
            Assert.AreEqual(0, result.Values.Count);
            gLAnalysisGetCurveCoordinate.Verify(x => x.GetAnalysisResultCurve(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void GetGLAnalysisCurveCoordinateNullResponseTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var gLAnalysisCurveCoordinateInput = new GLAnalysisCurveCoordinateInput()
            {
                AssetId = new Guid("63F2ADD2-C79C-4B6B-A190-5CE6232E28F3"),
                TestDate = DateTime.UtcNow.ToString(),
                AnalysisResultId = 1,
                AnalysisTypeId = 1
            };

            var correlationId = Guid.NewGuid().ToString();
            var requestWithCorrelationId = new WithCorrelationId<GLAnalysisCurveCoordinateInput>
                (correlationId, gLAnalysisCurveCoordinateInput);

            var mockService = new Mock<IGLAnalysis>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockWellAnalysisCorrelation = new Mock<IWellAnalysisCorrelation>();

            var mockManufacturer = new Mock<IManufacturer>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var gLAnalysisGetCurveCoordinate = new Mock<IGLAnalysisGetCurveCoordinate>();
            gLAnalysisGetCurveCoordinate
                .Setup(x => x.GetAnalysisResultCurve(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()));

            var service = new GLAnalysisProcessingService(mockService.Object, mockThetaLoggerFactory.Object,
                mockLocalePhrase.Object,
                mockWellAnalysisCorrelation.Object, mockManufacturer.Object, mockAnalysisCurve.Object,
                new Mock<IAnalysisCurveSets>().Object, gLAnalysisGetCurveCoordinate.Object, _commonService.Object);

            var result = service.GetGLAnalysisCurveCoordinateResults(requestWithCorrelationId);

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Result.Status, false);
            gLAnalysisGetCurveCoordinate.Verify(x => x.GetAnalysisResultCurve(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            gLAnalysisGetCurveCoordinate.Verify(x => x.GetGLAnalysisIPRCurveCoordinate(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Never);
        }

        [TestMethod]
        public void GetGLAnalysisCurveCoordinateResultsKillCurveTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var responseData = new List<AnalysisResultCurvesModel>()
            {
                new AnalysisResultCurvesModel
                {
                    Id = 1,
                    AnalysisResultCurveID = 1,
                    CurveTypesID = 1,
                    Name = "Test",
                    Coordinates = new[]
                    {
                        new Coordinates()
                        {
                            X = 10,
                            Y = 10
                        }
                    }
                }
            };

            var fluidCurveresponseData = new StaticFluidCurveModel
            {
                ProductionDepth = 1,
                ReservoirPressure = 1,
                KillFluidLevel = 1,
                ReservoirFluidLevel = 1,
                Perforations = new List<PerforationModel>
                {
                    new PerforationModel
                    {
                        NodeId = "1",
                        TopDepth = 1,
                        Diameter = 1,
                        HoleCountPerUnit = 1,
                        Length = 1
                    }
                }
            };

            var gLAnalysisCurveCoordinateInput = new GLAnalysisCurveCoordinateInput()
            {
                AssetId = new Guid("63F2ADD2-C79C-4B6B-A190-5CE6232E28F3"),
                TestDate = "01-30-2024 10:59:18",
                AnalysisResultId = 1,
                AnalysisTypeId = 1
            };

            var correlationId = Guid.NewGuid().ToString();
            var requestWithCorrelationId = new WithCorrelationId<GLAnalysisCurveCoordinateInput>
                (correlationId, gLAnalysisCurveCoordinateInput);

            var mockService = new Mock<IGLAnalysis>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockWellAnalysisCorrelation = new Mock<IWellAnalysisCorrelation>();

            var mockManufacturer = new Mock<IManufacturer>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var gLAnalysisGetCurveCoordinate = new Mock<IGLAnalysisGetCurveCoordinate>();
            gLAnalysisGetCurveCoordinate
                .Setup(x => x.GetAnalysisResultCurve(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(responseData);

            gLAnalysisGetCurveCoordinate
                .Setup(x => x.GetDataForStaticFluidCurve(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(fluidCurveresponseData);

            var nodeData = new List<NodeMasterModel>()
            {
                new NodeMasterModel()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();

            var glAnalysisData = new GLAnalysisResponse()
            {
                NodeMasterData = nodeData.FirstOrDefault(),
                AnalysisResultEntity = GetAnalysisResultData(),
                ValveStatusEntities = GetValveStatusResultData(),
                WellDetail = GetWellDetail(),
                WellOrificeStatus = GetWellOrificeData(),
                WellValveEntities = GetValveResultData()
            };

            mockService.Setup(x =>
                    x.GetGLAnalysisData(new Guid("63f2add2-c79c-4b6b-a190-5ce6232e28f3"),
                    "01-30-2024 10:59:18", 1, correlationId))
                .Returns(glAnalysisData);

            var phraseIds = SetupPhraseIds();
            var phrases = SetupPhrases();

            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseIds)).Returns(phrases);

            var service = new GLAnalysisProcessingService(mockService.Object, mockThetaLoggerFactory.Object,
                mockLocalePhrase.Object,
                mockWellAnalysisCorrelation.Object, mockManufacturer.Object, mockAnalysisCurve.Object,
                new Mock<IAnalysisCurveSets>().Object, gLAnalysisGetCurveCoordinate.Object, _commonService.Object);

            var result = service.GetGLAnalysisCurveCoordinateResults(requestWithCorrelationId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GLAnalysisCurveCoordinateDataOutput));
            gLAnalysisGetCurveCoordinate.Verify(x => x.GetAnalysisResultCurve(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            gLAnalysisGetCurveCoordinate.Verify(x => x.GetGLAnalysisIPRCurveCoordinate(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Once);
            gLAnalysisGetCurveCoordinate.Verify(x => x.GetDataForStaticFluidCurve(It.IsAny<Guid>(), It.IsAny<string>()), Times.Once);
            Assert.IsTrue(result.Values.Count > 1);
        }

        [TestMethod]
        public void GetGLAnalysisCurveCoordinateResultsNullDataTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IGLAnalysis>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockWellAnalysisCorrelation = new Mock<IWellAnalysisCorrelation>();

            var mockManufacturer = new Mock<IManufacturer>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var gLAnalysisGetCurveCoordinate = new Mock<IGLAnalysisGetCurveCoordinate>();

            var service = new GLAnalysisProcessingService(mockService.Object, mockThetaLoggerFactory.Object,
                mockLocalePhrase.Object,
                mockWellAnalysisCorrelation.Object, mockManufacturer.Object, mockAnalysisCurve.Object,
                new Mock<IAnalysisCurveSets>().Object, gLAnalysisGetCurveCoordinate.Object, _commonService.Object);

            var result = service.GetGLAnalysisCurveCoordinateResults(null);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Result.Status);
            logger.Verify(x => x.Write(Level.Info,
                    It.Is<string>(x => x.Contains("data is null, cannot get gas lift analysis curve coordinate results."))),
                Times.AtLeastOnce);
            gLAnalysisGetCurveCoordinate.Verify(x => x.GetGLAnalysisResultData
                (new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000", 1, 1, It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void GetGetStaticFluidCurveResponseTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var responseData = new List<AnalysisResultCurvesModel>()
            {
                new AnalysisResultCurvesModel
                {
                    Id = 1,
                    AnalysisResultCurveID = 1,
                    CurveTypesID = 1,
                    Name = "Test",
                    Coordinates = new[]
                    {
                        new Coordinates()
                        {
                            X = 10,
                            Y = 10
                        }
                    }
                }
            };

            var fluidCurveresponseData = new StaticFluidCurveModel
            {
                ProductionDepth = 1,
                ReservoirPressure = 1,
                KillFluidLevel = 1,
                ReservoirFluidLevel = 1,
                Perforations = new List<PerforationModel>()
            };

            var gLAnalysisCurveCoordinateInput = new GLAnalysisCurveCoordinateInput()
            {
                AssetId = new Guid("63F2ADD2-C79C-4B6B-A190-5CE6232E28F3"),
                TestDate = "01-30-2024 10:59:18",
                AnalysisResultId = 1,
                AnalysisTypeId = 1
            };

            var correlationId = Guid.NewGuid().ToString();
            var requestWithCorrelationId = new WithCorrelationId<GLAnalysisCurveCoordinateInput>
                (correlationId, gLAnalysisCurveCoordinateInput);

            var nodeData = new List<NodeMasterModel>()
            {
                new NodeMasterModel()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();

            var glAnalysisData = new GLAnalysisResponse()
            {
                NodeMasterData = nodeData.FirstOrDefault(),
                AnalysisResultEntity = GetAnalysisResultData(),
                ValveStatusEntities = GetValveStatusResultData(),
                WellDetail = GetWellDetail(),
                WellOrificeStatus = GetWellOrificeData(),
                WellValveEntities = GetValveResultData()
            };

            var mockService = new Mock<IGLAnalysis>();
            mockService.Setup(x =>
                    x.GetGLAnalysisData(new Guid("63f2add2-c79c-4b6b-a190-5ce6232e28f3"),
                    "01-30-2024 10:59:18", 1, correlationId))
                .Returns(glAnalysisData);

            var phraseIds = SetupPhraseIds();
            var phrases = SetupPhrases();

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockWellAnalysisCorrelation = new Mock<IWellAnalysisCorrelation>();

            var mockManufacturer = new Mock<IManufacturer>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var gLAnalysisGetCurveCoordinate = new Mock<IGLAnalysisGetCurveCoordinate>();
            gLAnalysisGetCurveCoordinate
                .Setup(x => x.GetAnalysisResultCurve(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(responseData);

            gLAnalysisGetCurveCoordinate
                .Setup(x => x.GetDataForStaticFluidCurve(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(fluidCurveresponseData);

            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseIds)).Returns(phrases);

            var service = new GLAnalysisProcessingService(mockService.Object, mockThetaLoggerFactory.Object,
                mockLocalePhrase.Object,
                mockWellAnalysisCorrelation.Object, mockManufacturer.Object, mockAnalysisCurve.Object,
                new Mock<IAnalysisCurveSets>().Object, gLAnalysisGetCurveCoordinate.Object, _commonService.Object);

            var result = service.GetGLAnalysisCurveCoordinateResults(requestWithCorrelationId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GLAnalysisCurveCoordinateDataOutput));
            gLAnalysisGetCurveCoordinate.Verify(x => x.GetAnalysisResultCurve(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            gLAnalysisGetCurveCoordinate.Verify(x => x.GetGLAnalysisIPRCurveCoordinate(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Once);
            gLAnalysisGetCurveCoordinate.Verify(x => x.GetDataForStaticFluidCurve(It.IsAny<Guid>(), It.IsAny<string>()), Times.Once);
            Assert.IsTrue(result.Values.Count == 3);
        }

        [TestMethod]
        public void GetGetColemanCriticalFlowRateResponseTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var responseData = new List<AnalysisResultCurvesModel>()
            {
                new AnalysisResultCurvesModel
                {
                    Id = 1,
                    AnalysisResultCurveID = 1,
                    CurveTypesID = 1,
                    Name = "Test",
                    Coordinates = new[]
                    {
                        new Coordinates()
                        {
                            X = 10,
                            Y = 10
                        }
                    }
                }
            };

            var fluidCurveresponseData = new StaticFluidCurveModel
            {
                ProductionDepth = 1,
                ReservoirPressure = 1,
                KillFluidLevel = 1,
                ReservoirFluidLevel = 1,
                Perforations = new List<PerforationModel>
                {
                    new PerforationModel
                    {
                        NodeId = "1",
                        TopDepth = 1,
                        Diameter = 1,
                        HoleCountPerUnit = 1,
                        Length = 1
                    }
                }
            };

            var gLAnalysisCurveCoordinateInput = new GLAnalysisCurveCoordinateInput()
            {
                AssetId = new Guid("63F2ADD2-C79C-4B6B-A190-5CE6232E28F3"),
                TestDate = "01-30-2024 10:59:18",
                AnalysisResultId = 1,
                AnalysisTypeId = 1
            };

            var correlationId = Guid.NewGuid().ToString();
            var requestWithCorrelationId = new WithCorrelationId<GLAnalysisCurveCoordinateInput>
                (correlationId, gLAnalysisCurveCoordinateInput);

            var nodeData = new List<NodeMasterModel>()
            {
                new NodeMasterModel()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();

            var glAnalysisData = new GLAnalysisResponse()
            {
                NodeMasterData = nodeData.FirstOrDefault(),
                AnalysisResultEntity = GetAnalysisResultData(),
                ValveStatusEntities = GetValveStatusResultData(),
                WellDetail = GetWellDetail(),
                WellOrificeStatus = GetWellOrificeData(),
                WellValveEntities = GetValveResultData()
            };

            var mockService = new Mock<IGLAnalysis>();
            mockService.Setup(x =>
                    x.GetGLAnalysisData(new Guid("63f2add2-c79c-4b6b-a190-5ce6232e28f3"),
                    "01-30-2024 10:59:18", 1, correlationId))
                .Returns(glAnalysisData);

            var phraseIds = SetupPhraseIds();
            var phrases = SetupPhrases();

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockWellAnalysisCorrelation = new Mock<IWellAnalysisCorrelation>();

            var mockManufacturer = new Mock<IManufacturer>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var gLAnalysisGetCurveCoordinate = new Mock<IGLAnalysisGetCurveCoordinate>();
            gLAnalysisGetCurveCoordinate
                .Setup(x => x.GetAnalysisResultCurve(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(responseData);

            gLAnalysisGetCurveCoordinate
                .Setup(x => x.GetDataForStaticFluidCurve(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(fluidCurveresponseData);

            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseIds)).Returns(phrases);

            var service = new GLAnalysisProcessingService(mockService.Object, mockThetaLoggerFactory.Object,
                mockLocalePhrase.Object,
                mockWellAnalysisCorrelation.Object, mockManufacturer.Object, mockAnalysisCurve.Object,
                new Mock<IAnalysisCurveSets>().Object, gLAnalysisGetCurveCoordinate.Object, _commonService.Object);

            var result = service.GetGLAnalysisCurveCoordinateResults(requestWithCorrelationId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GLAnalysisCurveCoordinateDataOutput));
            gLAnalysisGetCurveCoordinate.Verify(x => x.GetAnalysisResultCurve(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            gLAnalysisGetCurveCoordinate.Verify(x => x.GetGLAnalysisIPRCurveCoordinate(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Once);
            gLAnalysisGetCurveCoordinate.Verify(x => x.GetDataForStaticFluidCurve(It.IsAny<Guid>(), It.IsAny<string>()), Times.Once);
            Assert.IsTrue(result.Values.Count == 5);
        }

        [TestMethod]
        public void GetGLAnalysisCurveCoordinateResultsIPRCurveReturnsNameTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var responseData = new List<AnalysisResultCurvesModel>()
            {
                new AnalysisResultCurvesModel
                {
                    Id = 1,
                    AnalysisResultCurveID = 1,
                    CurveTypesID = 1,
                    Name = "Test",
                    Coordinates = new[]
                    {
                        new Coordinates()
                        {
                            X = 10,
                            Y = 10
                        }
                    }
                }
            };
            var gLAnalysisCurveCoordinateInput = new GLAnalysisCurveCoordinateInput()
            {
                AssetId = new Guid("63F2ADD2-C79C-4B6B-A190-5CE6232E28F3"),
                TestDate = "01-30-2024 10:59:18",
                AnalysisResultId = 1,
                AnalysisTypeId = 1
            };

            var nodeData = new List<NodeMasterModel>()
            {
                new NodeMasterModel()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();

            var glAnalysisData = new GLAnalysisResponse()
            {
                NodeMasterData = nodeData.FirstOrDefault(),
                AnalysisResultEntity = GetAnalysisResultData(),
                ValveStatusEntities = GetValveStatusResultData(),
                WellDetail = GetWellDetail(),
                WellOrificeStatus = GetWellOrificeData(),
                WellValveEntities = GetValveResultData()
            };

            var mockService = new Mock<IGLAnalysis>();
            var correlationId = Guid.NewGuid().ToString();
            mockService.Setup(x =>
                    x.GetGLAnalysisData(new Guid("63f2add2-c79c-4b6b-a190-5ce6232e28f3"),
                    "01-30-2024 10:59:18", 1, correlationId))
                .Returns(glAnalysisData);

            var phraseIds = SetupPhraseIds();
            var phrases = SetupPhrases();

            var mockLocalePhrase = new Mock<ILocalePhrases>();
            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseIds)).Returns(phrases);

            var requestWithCorrelationId = new WithCorrelationId<GLAnalysisCurveCoordinateInput>
                (correlationId, gLAnalysisCurveCoordinateInput);

            var mockWellAnalysisCorrelation = new Mock<IWellAnalysisCorrelation>();

            var mockManufacturer = new Mock<IManufacturer>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var gLAnalysisGetCurveCoordinate = new Mock<IGLAnalysisGetCurveCoordinate>();
            gLAnalysisGetCurveCoordinate
                .Setup(x => x.GetAnalysisResultCurve(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(responseData);

            var iprData = new IPRAnalysisCurveCoordinateModel()
            {
                IPRAnalysisResultEntity = new IPRAnalysisResultModel(),
                NodeMasterData = new NodeMasterModel()
                {
                    ApplicationId = 7,
                },
                AnalysisResultCurvesEntities = new List<AnalysisResultCurvesModel>()
                {
                    new AnalysisResultCurvesModel()
                    {
                        Id = 1,
                        AnalysisResultCurveID = 1,
                        CurveTypesID = 19,
                        Name = "Test",
                    }
                }
            };

            gLAnalysisGetCurveCoordinate.Setup(x => x.GetGLAnalysisIPRCurveCoordinate(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(iprData);
            gLAnalysisGetCurveCoordinate.Setup(x => x.FetchCurveCoordinates(It.IsAny<int>(), It.IsAny<string>())).Returns(
                new List<CurveCoordinatesModel>()
                {
                    new CurveCoordinatesModel()
                    {
                        CurveId = 1,
                        Id = 1,
                        X = 0,
                        Y = 0,
                    },
                    new CurveCoordinatesModel()
                    {
                        CurveId = 1,
                        Id = 2,
                        X = 10,
                        Y = 10,
                    },
                });

            var service = new GLAnalysisProcessingService(mockService.Object, mockThetaLoggerFactory.Object,
                mockLocalePhrase.Object,
                mockWellAnalysisCorrelation.Object, mockManufacturer.Object, mockAnalysisCurve.Object,
                new Mock<IAnalysisCurveSets>().Object, gLAnalysisGetCurveCoordinate.Object, _commonService.Object);

            var result = service.GetGLAnalysisCurveCoordinateResults(requestWithCorrelationId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(GLAnalysisCurveCoordinateDataOutput));
            gLAnalysisGetCurveCoordinate.Verify(x => x.GetAnalysisResultCurve(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            gLAnalysisGetCurveCoordinate.Verify(x => x.GetGLAnalysisIPRCurveCoordinate(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Once);
            Assert.IsTrue(result.Values.Count > 0);
            Assert.AreEqual("IPRCurve", result.Values.First(x => x.CurveTypeId == IPRCurveType.GasLiftIPRCurve.Key).Name);
        }

        [TestMethod]
        public void GetGLAnalysisFlowControlDeviceAnalysisValuesTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockWellAnalysisCorrelation = new Mock<IWellAnalysisCorrelation>();
            mockWellAnalysisCorrelation.Setup(x => x.GetAnalysisCorrelation(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new AnalysisCorrelationModel
                {
                    CorrelationId = 1,
                    CorrelationTypeId = 1,
                    Id = 1
                });

            var phraseIds = SetupPhraseIds();
            var phrases = SetupPhrases();

            var mockLocalePhrase = new Mock<ILocalePhrases>();
            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseIds)).Returns(phrases);

            var mockManufacturer = new Mock<IManufacturer>();
            mockManufacturer.Setup(m => m.Get(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new GLManufacturerModel
                {
                    Manufacturer = "PCS",
                    ManufacturerID = 1
                });

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var nodeData = new List<NodeMasterModel>()
            {
                new NodeMasterModel()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();

            var responseData = new GLAnalysisResponse()
            {
                NodeMasterData = nodeData.FirstOrDefault(),
                AnalysisResultEntity = GetAnalysisResultData(),
                ValveStatusEntities = GetFlowControlDeviceAnalysisValuesData(),
                WellDetail = GetWellDetail(),
                WellOrificeStatus = GetWellOrificeData(),
                WellValveEntities = GetValveResultData()
            };

            var mockGLAnalysisService = new Mock<IGLAnalysis>();
            var correlationId = Guid.NewGuid().ToString();
            mockGLAnalysisService.Setup(x =>
                    x.GetGLAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                    "2023-05-11 22:29:55.000", 1, correlationId))
                .Returns(responseData);

            var mockService = new Mock<IGLAnalysisGetCurveCoordinate>();

            var service = new GLAnalysisProcessingService(mockGLAnalysisService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockWellAnalysisCorrelation.Object,
                mockManufacturer.Object, mockAnalysisCurve.Object,
                new Mock<IAnalysisCurveSets>().Object, mockService.Object, _commonService.Object);

            var request = new WithCorrelationId<GLAnalysisInput>(correlationId, new GLAnalysisInput
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                TestDate = "2023-05-11 22:29:55.000",
                AnalysisTypeId = 1,
                AnalysisResultId = 1,
            });

            var result = service.GetGLAnalysisResults(request);

            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Result.Status);

            var resultValues = result.Values.Valves.First();
            Assert.AreEqual("1012 psi", resultValues.ClosingPressureAtDepth);
            Assert.AreEqual("1064 psi", resultValues.OpeningPressureAtDepth);
            Assert.AreEqual("1027 psi", resultValues.InjectionPressureAtDepth);
            Assert.AreEqual("1005 (905) mscf/d", resultValues.TubingCriticalVelocityAtDepth);
            Assert.AreEqual("998", resultValues.Depth);
        }

        #endregion

        #region Private Methods

        private GLAnalysisResultModel GetAnalysisResultData()
        {
            var espAnalysisResults = new GLAnalysisResultModel()
            {
                Id = 1,
                NodeId = "Well1",
                TestDate = new DateTime(2023, 5, 11, 22, 29, 55),
                ProcessedDate = new DateTime(2023, 5, 11, 22, 29, 55),
                Success = true,
                GasInjectionDepth = 6530.91f,
                OilRate = 75,
                WaterRate = 225,
                GasRate = 500,
                WellheadPressure = 150,
                CasingPressure = 850,
                WaterCut = 75,
                GasSpecificGravity = 0.758f,
                WaterSpecificGravity = 1.06f,
                WellheadTemperature = 100,
                BottomholeTemperature = 200,
                OilSpecificGravity = 0.8250729f,
                CasingId = 6.135f,
                TubingId = 2.441f,
                TubingOD = 2.875f,
                ReservoirPressure = 1500,
                BubblepointPressure = 1000,
                ValveCriticalVelocity = 1000,
            };

            return espAnalysisResults;
        }

        private IList<GLWellValveModel> GetValveResultData()
        {
            var result = new List<GLWellValveModel>
            {
                new GLWellValveModel
                {
                    NodeId = "Well1",
                    GLValveId = 56,
                    VerticalDepth = 2226.12f,
                    TestRackOpeningPressure = 980,
                    ClosingPressureAtDepth = 1012.254f,
                    MeasuredDepth = 2226.12f,
                    OpeningPressureAtDepth = 1063.807f,
                    OpeningPressureAtSurface = 1000,
                    ClosingPressureAtSurface = 948,
                    ValveId = 1,
                    Diameter = 0.625f,
                    BellowsArea = 0.12f,
                    PortSize = 0.15625f,
                    PortArea = 0.019f,
                    PortToBellowsAreaRatio = 0.1598f,
                    ProductionPressureEffectFactor = 0.19f,
                    Description = "TP-.625",
                    ManufacturerId = 1,
                    OneMinusR = 0.8402f
                },
                new GLWellValveModel
                {
                    NodeId = "Well1",
                    GLValveId = 56,
                    VerticalDepth = 3453.61f,
                    TestRackOpeningPressure = 965,
                    ClosingPressureAtDepth = 1018.697f,
                    MeasuredDepth = 3453.61f,
                    OpeningPressureAtDepth = 1063.259f,
                    OpeningPressureAtSurface = 963,
                    ClosingPressureAtSurface = 926,
                    ValveId = 1000,
                    Diameter = 0.625f,
                    BellowsArea = 0.12f,
                    PortSize = 0.1875f,
                    PortArea = 0.0276f,
                    PortToBellowsAreaRatio = 0.2301f,
                    ProductionPressureEffectFactor = 0.29f,
                    Description = "TP-.625",
                    ManufacturerId = 1,
                    OneMinusR = 0.7699f,
                }
            };
            return result;
        }

        private GLWellOrificeStatusModel GetWellOrificeData()
        {
            return new GLWellOrificeStatusModel()
            {
                NodeId = "Well1",
                OrificeState = 4,
                IsInjectingGas = false,
                GLAnalysisResultId = 1,
                InjectionRateForTubingCriticalVelocity = 995.8148f,
                TubingCriticalVelocityAtDepth = 1095.815f,
            };
        }

        private GLWellDetailModel GetWellDetail()
        {
            return new GLWellDetailModel()
            {
                NodeId = "Well1",
                InjectedGasSpecificGravity = 1f,
                EstimateInjectionDepth = true,
                ValveConfigurationOption = 1,
            };
        }

        private IList<GLValveStatusModel> GetValveStatusResultData()
        {
            return new List<GLValveStatusModel>
            {
                new GLValveStatusModel()
                {
                    GlwellValveId = 56,
                    PercentOpen = 0,
                    GLAnalysisResultId = 1,
                    InjectionPressureAtDepth = 1027.482f,
                    ValveState = 1,
                    IsInjectingGas = true,
                    InjectionRateForTubingCriticalVelocity = 905.3361f,
                    TubingCriticalVelocityAtDepth = 1005.336f,
                },
                new GLValveStatusModel()
                {
                    GlwellValveId = 56,
                    PercentOpen = 0,
                    GLAnalysisResultId = 1,
                    InjectionPressureAtDepth = 1033.482f,
                    ValveState = 1,
                    IsInjectingGas = true,
                    InjectionRateForTubingCriticalVelocity = 900.3361f,
                    TubingCriticalVelocityAtDepth = 1008.336f,
                },
            };
        }

        private IList<GLValveStatusModel> GetFlowControlDeviceAnalysisValuesData()
        {
            return new List<GLValveStatusModel>
            {
                new GLValveStatusModel()
                {
                   GlwellValveId = 56,
                   PercentOpen = 0,
                   GLAnalysisResultId = 1,
                   InjectionPressureAtDepth = 1027.482f,
                   ValveState = 1,
                   IsInjectingGas = true,
                   InjectionRateForTubingCriticalVelocity = 905.3361f,
                   TubingCriticalVelocityAtDepth = 1005.336f,
                   OpeningPressureAtDepth = 976.2344f,
                   ClosingPressureAtDepth = 564.87533f,
                   Depth = 997.68222f,
                },
                new GLValveStatusModel()
                {
                   GlwellValveId = 56,
                   PercentOpen = 0,
                   GLAnalysisResultId = 1,
                   InjectionPressureAtDepth = 1027.482f,
                   ValveState = 1,
                   IsInjectingGas = true,
                   InjectionRateForTubingCriticalVelocity = 905.3361f,
                   TubingCriticalVelocityAtDepth = 1005.336f,
                   OpeningPressureAtDepth = 976.2344f,
                   ClosingPressureAtDepth = 564.87533f,
                   Depth = 997.68222f,
                },
            };
        }

        private IDictionary<int, string> SetupPhrases()
        {
            return new Dictionary<int, string>()
            {
                {
                    102, "TestDate"
                },
                {
                    264, "TubingPressure"
                },
                {
                    265, "CasingPressure"
                },
                {
                    267, "WaterSpecificGravity"
                },
                {
                    532, "OilRate"
                },
                {
                    1134, "Injection"
                },
                {
                    1250, "WaterRate"
                },
                {
                    1251, "GasRate"
                },
                {
                    3115, "Surface"
                },
                {
                    4088, "FlowingBottomholePressure"
                },
                {
                    4816, "Theselectedwelltestwasprocessedsuccessfully"
                },
                {
                    4831, "BottomholeTemperature"
                },
                {
                    5560, "Theselectedwelltesthasnotbeenprocessed"
                },
                {
                    5584, "SpecificGravityofOil"
                },
                {
                    5613, "ProductionRate"
                },
                {
                    5646, "TubingInnerDiameter"
                },
                {
                    5758, "InjectionRate"
                },
                {
                    5760, "Vertical Injection Depth"
                },
                {
                    5765, "FormationGasOilRatio"
                },
                {
                    5766, "WellheadTemperature"
                },
                {
                    5767, "ReservoirPressure"
                },
                {
                    5769, "TestFlowCorrelationDate"
                },
                {
                    5771, "MaximumProductionRate"
                },
                {
                    5773, "InjectionRateForMaxProductionRate"
                },
                {
                    5774, "OptimalProductionRate"
                },
                {
                    5776, "InjectionRateForOptimalProdRate"
                },
                {
                    5798, "Vertical Well Depth"
                },
                {
                    5957, "Orifice"
                },
                {
                    5991, "Injecting"
                },
                {
                    5992, "NotInjecting"
                },
                {
                    6001, "// Please check the well configuration.\r\nWell depth / mid-perf depth, injection depth" +
                    " and the\r\nconfigurated valve or orifice depths do not match."
                },
                {
                    6002, "is missing from analysis result.\r\nWellbore diagram cannot be shown."
                },
                {
                    6078, "TesFBHPatInjectionDepthtDate"
                },
                {
                    6099, "InjectionRateforCriticalFlowrate"
                },
                {
                    6103, "TubingCriticalVelocityCorrelation"
                },
                {
                    6444, "PinjForMaximumProductionRate"
                },
                {
                    6445, "PinjForOptimalProductionRate"
                },
                {
                    6467, "Lastran"
                },
                {
                    6683, "Inclinj"
                },
                {
                    6684, "Exclinj"
                },
                {
                    6845, "CalculatedMeasuredInjectionDepth"
                },
                {
                    6846, "CalculatedVerticalInjectionDepth"
                },
                {
                    7215, "ProducedGasSpecificGravity"
                },
                {
                    7216, "InjectedGasSpecificGravity"
                },
                {
                    100199, "Watercut"
                },
            };
        }

        private int[] SetupPhraseIds()
        {
            return new int[]
            {
                102,
                264,
                265,
                267,
                532,
                1134,
                1250,
                1251,
                3115,
                4088,
                4816,
                4831,
                5560,
                5584,
                5613,
                5646,
                5758,
                5760,
                5765,
                5766,
                5767,
                5769,
                5771,
                5773,
                5774,
                5776,
                5798,
                5957,
                5991,
                5992,
                6001,
                6002,
                6078,
                6099,
                6103,
                6444,
                6445,
                6467,
                6683,
                6684,
                6845,
                6846,
                7215,
                7216,
                100199
            };
        }

        private void SetupCommonService()
        {
            _commonService.Setup(x => x.GetSystemParameterNextGenSignificantDigits(It.IsAny<string>())).Returns(3);
        }

        #endregion

    }
}
