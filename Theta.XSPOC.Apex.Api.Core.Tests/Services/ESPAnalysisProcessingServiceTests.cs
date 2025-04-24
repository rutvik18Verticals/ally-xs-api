using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Common.Calculators.ESP;
using Theta.XSPOC.Apex.Api.Common.Calculators.Well;
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
using ESPAnalysisInput = Theta.XSPOC.Apex.Api.Core.Models.Inputs.ESPAnalysisInput;

namespace Theta.XSPOC.Apex.Api.Core.Tests.Services
{
    [TestClass]
    public class ESPAnalysisProcessingServiceTests
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
            var mockService = new Mock<IESPAnalysis>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockESPPump = new Mock<IESPPump>();

            var mockNodeMaster = new Mock<INodeMaster>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var mockCurveCoordinate = new Mock<ICurveCoordinate>();

            var mockAnalysisCurveSet = new Mock<IAnalysisCurveSets>();

            var mockESPTornadoCurveSetAnnotation = new Mock<IESPTornadoCurveSetAnnotation>();

            var mockGasAwareEspCalculator = new Mock<IGasAwareESPCalculator>();

            var mockGasAwareWellCalculator = new Mock<IGasAwareWellCalculator>();

            _ = new ESPAnalysisProcessingService(mockService.Object, null, mockLocalePhrase.Object, mockESPPump.Object,
                mockNodeMaster.Object, mockAnalysisCurve.Object, mockCurveCoordinate.Object,
                mockAnalysisCurveSet.Object, mockESPTornadoCurveSetAnnotation.Object, mockGasAwareEspCalculator.Object,
                mockGasAwareWellCalculator.Object, _commonService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullESPAnalysisServiceTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockESPPump = new Mock<IESPPump>();

            var mockNodeMaster = new Mock<INodeMaster>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var mockCurveCoordinate = new Mock<ICurveCoordinate>();

            var mockAnalysisCurveSet = new Mock<IAnalysisCurveSets>();

            var mockESPTornadoCurveSetAnnotation = new Mock<IESPTornadoCurveSetAnnotation>();

            var mockGasAwareEspCalculator = new Mock<IGasAwareESPCalculator>();

            var mockGasAwareWellCalculator = new Mock<IGasAwareWellCalculator>();

            _ = new ESPAnalysisProcessingService(null, mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockESPPump.Object,
                mockNodeMaster.Object, mockAnalysisCurve.Object, mockCurveCoordinate.Object,
                mockAnalysisCurveSet.Object, mockESPTornadoCurveSetAnnotation.Object, mockGasAwareEspCalculator.Object,
                mockGasAwareWellCalculator.Object, _commonService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLocalePhraseProcessingServiceTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IESPAnalysis>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockESPPump = new Mock<IESPPump>();

            var mockNodeMaster = new Mock<INodeMaster>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var mockCurveCoordinate = new Mock<ICurveCoordinate>();

            var mockAnalysisCurveSet = new Mock<IAnalysisCurveSets>();

            var mockESPTornadoCurveSetAnnotation = new Mock<IESPTornadoCurveSetAnnotation>();

            var mockGasAwareEspCalculator = new Mock<IGasAwareESPCalculator>();

            var mockGasAwareWellCalculator = new Mock<IGasAwareWellCalculator>();

            _ = new ESPAnalysisProcessingService(mockService.Object, mockThetaLoggerFactory.Object, null, mockESPPump.Object,
                 mockNodeMaster.Object, mockAnalysisCurve.Object, mockCurveCoordinate.Object,
                mockAnalysisCurveSet.Object, mockESPTornadoCurveSetAnnotation.Object, mockGasAwareEspCalculator.Object,
                mockGasAwareWellCalculator.Object, _commonService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullESPPumpServiceTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IESPAnalysis>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockNodeMaster = new Mock<INodeMaster>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var mockCurveCoordinate = new Mock<ICurveCoordinate>();

            var mockAnalysisCurveSet = new Mock<IAnalysisCurveSets>();

            var mockESPTornadoCurveSetAnnotation = new Mock<IESPTornadoCurveSetAnnotation>();

            var mockGasAwareEspCalculator = new Mock<IGasAwareESPCalculator>();

            var mockGasAwareWellCalculator = new Mock<IGasAwareWellCalculator>();

            _ = new ESPAnalysisProcessingService(mockService.Object, mockThetaLoggerFactory.Object, mockLocalePhrase.Object, null,
                 mockNodeMaster.Object, mockAnalysisCurve.Object, mockCurveCoordinate.Object,
                mockAnalysisCurveSet.Object, mockESPTornadoCurveSetAnnotation.Object, mockGasAwareEspCalculator.Object,
                mockGasAwareWellCalculator.Object, _commonService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullNodeMasterTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IESPAnalysis>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockESPPump = new Mock<IESPPump>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var mockCurveCoordinate = new Mock<ICurveCoordinate>();

            var mockAnalysisCurveSet = new Mock<IAnalysisCurveSets>();

            var mockESPTornadoCurveSetAnnotation = new Mock<IESPTornadoCurveSetAnnotation>();

            var mockGasAwareEspCalculator = new Mock<IGasAwareESPCalculator>();

            var mockGasAwareWellCalculator = new Mock<IGasAwareWellCalculator>();

            _ = new ESPAnalysisProcessingService(mockService.Object, mockThetaLoggerFactory.Object, mockLocalePhrase.Object,
                mockESPPump.Object,
                 null, mockAnalysisCurve.Object, mockCurveCoordinate.Object,
                mockAnalysisCurveSet.Object, mockESPTornadoCurveSetAnnotation.Object, mockGasAwareEspCalculator.Object,
                mockGasAwareWellCalculator.Object, _commonService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullAnalysisCurveTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IESPAnalysis>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockESPPump = new Mock<IESPPump>();

            var mockNodeMaster = new Mock<INodeMaster>();

            var mockCurveCoordinate = new Mock<ICurveCoordinate>();

            var mockAnalysisCurveSet = new Mock<IAnalysisCurveSets>();

            var mockESPTornadoCurveSetAnnotation = new Mock<IESPTornadoCurveSetAnnotation>();

            var mockGasAwareEspCalculator = new Mock<IGasAwareESPCalculator>();

            var mockGasAwareWellCalculator = new Mock<IGasAwareWellCalculator>();

            _ = new ESPAnalysisProcessingService(mockService.Object, mockThetaLoggerFactory.Object, mockLocalePhrase.Object,
                mockESPPump.Object,
                 mockNodeMaster.Object, null, mockCurveCoordinate.Object,
                mockAnalysisCurveSet.Object, mockESPTornadoCurveSetAnnotation.Object, mockGasAwareEspCalculator.Object,
                mockGasAwareWellCalculator.Object, _commonService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullCurveCoordinateTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IESPAnalysis>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockESPPump = new Mock<IESPPump>();

            var mockNodeMaster = new Mock<INodeMaster>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var mockCurveCoordinate = new Mock<ICurveCoordinate>();

            var mockAnalysisCurveSet = new Mock<IAnalysisCurveSets>();

            var mockESPTornadoCurveSetAnnotation = new Mock<IESPTornadoCurveSetAnnotation>();

            var mockGasAwareEspCalculator = new Mock<IGasAwareESPCalculator>();

            var mockGasAwareWellCalculator = new Mock<IGasAwareWellCalculator>();

            _ = new ESPAnalysisProcessingService(mockService.Object, mockThetaLoggerFactory.Object, mockLocalePhrase.Object,
                mockESPPump.Object,
                 mockNodeMaster.Object, mockAnalysisCurve.Object, null,
                mockAnalysisCurveSet.Object, mockESPTornadoCurveSetAnnotation.Object, mockGasAwareEspCalculator.Object,
                mockGasAwareWellCalculator.Object, _commonService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullAnalysisCurveSetTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IESPAnalysis>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockESPPump = new Mock<IESPPump>();

            var mockNodeMaster = new Mock<INodeMaster>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var mockCurveCoordinate = new Mock<ICurveCoordinate>();

            var mockESPTornadoCurveSetAnnotation = new Mock<IESPTornadoCurveSetAnnotation>();

            var mockGasAwareEspCalculator = new Mock<IGasAwareESPCalculator>();

            var mockGasAwareWellCalculator = new Mock<IGasAwareWellCalculator>();

            _ = new ESPAnalysisProcessingService(mockService.Object, mockThetaLoggerFactory.Object, mockLocalePhrase.Object,
                mockESPPump.Object,
                 mockNodeMaster.Object, mockAnalysisCurve.Object, mockCurveCoordinate.Object,
                null, mockESPTornadoCurveSetAnnotation.Object, mockGasAwareEspCalculator.Object,
                mockGasAwareWellCalculator.Object, _commonService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullESPTornadoCurveSetAnnotationTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IESPAnalysis>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockESPPump = new Mock<IESPPump>();

            var mockNodeMaster = new Mock<INodeMaster>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var mockCurveCoordinate = new Mock<ICurveCoordinate>();

            var mockAnalysisCurveSet = new Mock<IAnalysisCurveSets>();

            var mockGasAwareEspCalculator = new Mock<IGasAwareESPCalculator>();

            var mockGasAwareWellCalculator = new Mock<IGasAwareWellCalculator>();

            _ = new ESPAnalysisProcessingService(mockService.Object, mockThetaLoggerFactory.Object, mockLocalePhrase.Object,
                mockESPPump.Object,
                 mockNodeMaster.Object, mockAnalysisCurve.Object, mockCurveCoordinate.Object,
                mockAnalysisCurveSet.Object, null, mockGasAwareEspCalculator.Object,
                mockGasAwareWellCalculator.Object, _commonService.Object);
        }

        [TestMethod]
        public void GetESPAnalysisResultsTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            var correlationId = Guid.NewGuid().ToString();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockESPPump = new Mock<IESPPump>();
            mockESPPump.Setup(x => x.GetESPPumpData(It.IsAny<object>(), correlationId))
                .Returns(new ESPPumpDataModel
                {
                    ESPPumpId = 4,
                    Pump = "P10",
                    MinCasingSize = 1,
                    MinBPD = 800,
                    MaxBPD = 1000,
                    UseCoefficients = true,
                    Series = "400",
                    PumpModel = "P10"
                });

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockNodeMaster = new Mock<INodeMaster>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var mockCurveCoordinate = new Mock<ICurveCoordinate>();

            var mockAnalysisCurveSet = new Mock<IAnalysisCurveSets>();

            var mockESPTornadoCurveSetAnnotation = new Mock<IESPTornadoCurveSetAnnotation>();

            var mockGasAwareEspCalculator = new Mock<IGasAwareESPCalculator>();

            var mockGasAwareWellCalculator = new Mock<IGasAwareWellCalculator>();

            var phrases = GetPhraseData();

            var phraseid = GetPhraseIdData();

            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseid)).Returns(phrases);

            var pressureProfilePhrases = GetPhraseDataPressureProfile();

            var pressureProfilePhraseIds = GetPhraseIdDataPressureProfile();

            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), pressureProfilePhraseIds)).Returns(pressureProfilePhrases);

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

            var responseData = new ESPAnalysisResponse()
            {
                PocType = 8,
                CardType = "P",
                CauseId = 0,
                NodeMasterData = nodeData.FirstOrDefault(),
                AnalysisResultEntity = GetAnalysisResultData(),
                WellPumpEntities = GetESPWellPumpData(),
            };

            var mockESPAnalysisService = new Mock<IESPAnalysis>();
            mockESPAnalysisService.Setup(x =>
                    x.GetESPAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000", correlationId))
                .Returns(responseData);

            var service = new ESPAnalysisProcessingService(mockESPAnalysisService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockESPPump.Object,
                 mockNodeMaster.Object,
                mockAnalysisCurve.Object, mockCurveCoordinate.Object, mockAnalysisCurveSet.Object,
                mockESPTornadoCurveSetAnnotation.Object, mockGasAwareEspCalculator.Object,
                mockGasAwareWellCalculator.Object, _commonService.Object);

            var request = new WithCorrelationId<ESPAnalysisInput>(correlationId, new ESPAnalysisInput
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                TestDate = "2023-05-11 22:29:55.000"
            });

            var result = service.GetESPAnalysisResults(request);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ESPAnalysisDataOutput));
            Assert.AreEqual(true, result.Result.Status);
            mockESPAnalysisService.Verify(x => x.GetESPAnalysisData
                    (new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                    "2023-05-11 22:29:55.000", correlationId),
                Times.Once);

            var resultItem = result.Values.Inputs.First(x => x.Id == "WaterRate");
            Assert.AreEqual("37.7 bpd", resultItem.DisplayValue);
            Assert.AreEqual("bpd", resultItem.MeasurementAbbreviation);
            Assert.AreEqual("37.70000076293945", resultItem.Value.ToString());
        }

        [TestMethod]
        public void GetESPAnalysisResultsNullDataTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockESPPump = new Mock<IESPPump>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockNodeMaster = new Mock<INodeMaster>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var mockCurveCoordinate = new Mock<ICurveCoordinate>();

            var mockAnalysisCurveSet = new Mock<IAnalysisCurveSets>();

            var mockESPTornadoCurveSetAnnotation = new Mock<IESPTornadoCurveSetAnnotation>();

            var mockESPAnalysisService = new Mock<IESPAnalysis>();

            var mockGasAwareEspCalculator = new Mock<IGasAwareESPCalculator>();

            var mockGasAwareWellCalculator = new Mock<IGasAwareWellCalculator>();

            var correlationId = Guid.NewGuid().ToString();

            mockESPAnalysisService.Setup(x =>
                x.GetESPAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000", correlationId));

            var service = new ESPAnalysisProcessingService(mockESPAnalysisService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockESPPump.Object,
                 mockNodeMaster.Object,
                mockAnalysisCurve.Object, mockCurveCoordinate.Object, mockAnalysisCurveSet.Object,
                mockESPTornadoCurveSetAnnotation.Object, mockGasAwareEspCalculator.Object,
                mockGasAwareWellCalculator.Object, _commonService.Object);

            var result = service.GetESPAnalysisResults(null);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Result.Status);
            logger.Verify(x => x.Write(Level.Info,
                    It.Is<string>(x => x.Contains("data is null, cannot get esp analysis results."))),
                Times.AtLeastOnce);
            mockESPAnalysisService.Verify(x => x.GetESPAnalysisData
                (new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000", correlationId), Times.Never);
        }

        [TestMethod]
        public void GetESPAnalysisResultsNullPayloadValueTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockESPPump = new Mock<IESPPump>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockNodeMaster = new Mock<INodeMaster>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var mockCurveCoordinate = new Mock<ICurveCoordinate>();

            var mockAnalysisCurveSet = new Mock<IAnalysisCurveSets>();

            var mockESPTornadoCurveSetAnnotation = new Mock<IESPTornadoCurveSetAnnotation>();

            var mockESPAnalysisService = new Mock<IESPAnalysis>();

            var mockGasAwareEspCalculator = new Mock<IGasAwareESPCalculator>();

            var mockGasAwareWellCalculator = new Mock<IGasAwareWellCalculator>();

            var correlationId = Guid.NewGuid().ToString();

            mockESPAnalysisService.Setup(x =>
                x.GetESPAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000"
                , correlationId));

            var service = new ESPAnalysisProcessingService(mockESPAnalysisService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockESPPump.Object,
                 mockNodeMaster.Object,
                mockAnalysisCurve.Object, mockCurveCoordinate.Object, mockAnalysisCurveSet.Object,
                mockESPTornadoCurveSetAnnotation.Object, mockGasAwareEspCalculator.Object,
                mockGasAwareWellCalculator.Object, _commonService.Object);

            var request = new WithCorrelationId<ESPAnalysisInput>(correlationId, null);

            var result = service.GetESPAnalysisResults(request);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Result.Status);
            logger.Verify(x => x.WriteCId(Level.Info,
                    It.Is<string>(x => x.Contains("data is null, cannot get esp analysis results.")), correlationId),
                Times.AtLeastOnce);
            mockESPAnalysisService.Verify(x => x.GetESPAnalysisData
                (new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000", correlationId), Times.Never);
        }

        [TestMethod]
        public void GetESPAnalysisResultsNullAssetIdOrTestDateTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockESPPump = new Mock<IESPPump>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockNodeMaster = new Mock<INodeMaster>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var mockCurveCoordinate = new Mock<ICurveCoordinate>();

            var mockAnalysisCurveSet = new Mock<IAnalysisCurveSets>();

            var mockESPTornadoCurveSetAnnotation = new Mock<IESPTornadoCurveSetAnnotation>();

            var mockESPAnalysisService = new Mock<IESPAnalysis>();

            var mockGasAwareEspCalculator = new Mock<IGasAwareESPCalculator>();

            var mockGasAwareWellCalculator = new Mock<IGasAwareWellCalculator>();

            var correlationId = Guid.NewGuid().ToString();

            mockESPAnalysisService.Setup(x =>
                x.GetESPAnalysisData(Guid.Empty, "2023 -05-11 22:29:55.000", correlationId));

            var service = new ESPAnalysisProcessingService(mockESPAnalysisService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockESPPump.Object,
                 mockNodeMaster.Object,
                mockAnalysisCurve.Object, mockCurveCoordinate.Object, mockAnalysisCurveSet.Object,
                mockESPTornadoCurveSetAnnotation.Object, mockGasAwareEspCalculator.Object,
                mockGasAwareWellCalculator.Object, _commonService.Object);

            var request = new WithCorrelationId<ESPAnalysisInput>(correlationId, new ESPAnalysisInput
            {
                AssetId = Guid.Empty,
                TestDate = ""
            });

            var result = service.GetESPAnalysisResults(request);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Result.Status);
            logger.Verify(x => x.WriteCId(Level.Info,
                    It.Is<string>(x => x.Contains("TestDate and AssetId should be provided to get esp analysis results.")),
                    correlationId),
                Times.AtLeastOnce);
            mockESPAnalysisService.Verify(x => x.GetESPAnalysisData(Guid.Empty, "2023-05-11 22:29:55.000", correlationId), Times.Never);
        }

        [TestMethod]
        public void GetESPAnalysisResultsNullNodeMasterResponseTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockESPPump = new Mock<IESPPump>();
            var correlationId = Guid.NewGuid().ToString();
            mockESPPump.Setup(x => x.GetESPPumpData(It.IsAny<object>(), correlationId))
                .Returns(new ESPPumpDataModel
                {
                    ESPPumpId = 4,
                    Pump = "P10",
                    MinCasingSize = 1,
                    MinBPD = 800,
                    MaxBPD = 1000,
                    UseCoefficients = true,
                    Series = "400",
                    PumpModel = "P10"
                });

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockNodeMaster = new Mock<INodeMaster>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var mockCurveCoordinate = new Mock<ICurveCoordinate>();

            var mockAnalysisCurveSet = new Mock<IAnalysisCurveSets>();

            var mockESPTornadoCurveSetAnnotation = new Mock<IESPTornadoCurveSetAnnotation>();

            var mockGasAwareEspCalculator = new Mock<IGasAwareESPCalculator>();

            var mockGasAwareWellCalculator = new Mock<IGasAwareWellCalculator>();

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
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();

            var responseData = new ESPAnalysisResponse()
            {
                PocType = 8,
                CardType = "P",
                CauseId = 0,
                NodeMasterData = null,
                AnalysisResultEntity = GetAnalysisResultData(),
                WellPumpEntities = GetESPWellPumpData(),
            };

            var mockESPAnalysisService = new Mock<IESPAnalysis>();
            mockESPAnalysisService.Setup(x =>
                    x.GetESPAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000"
                    , correlationId))
                .Returns(responseData);

            var service = new ESPAnalysisProcessingService(mockESPAnalysisService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockESPPump.Object,
                 mockNodeMaster.Object,
                mockAnalysisCurve.Object, mockCurveCoordinate.Object, mockAnalysisCurveSet.Object,
                mockESPTornadoCurveSetAnnotation.Object, mockGasAwareEspCalculator.Object,
                mockGasAwareWellCalculator.Object, _commonService.Object);

            var request = new WithCorrelationId<ESPAnalysisInput>(correlationId, new ESPAnalysisInput
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                TestDate = "2023-05-11 22:29:55.000"
            });

            var response = service.GetESPAnalysisResults(request);

            Assert.AreEqual(false, response.Result.Status);
            logger.Verify(x => x.WriteCId(Level.Info,
                    It.Is<string>(x => x.Contains("NodeMasterData is null, cannot get esp analysis results.")), correlationId),
                Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetESPAnalysisResultsNullAnalysisResultEntityResponseTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockESPPump = new Mock<IESPPump>();
            var correlationId = Guid.NewGuid().ToString();
            mockESPPump.Setup(x => x.GetESPPumpData(It.IsAny<object>(), correlationId))
                .Returns(new ESPPumpDataModel
                {
                    ESPPumpId = 4,
                    Pump = "P10",
                    MinCasingSize = 1,
                    MinBPD = 800,
                    MaxBPD = 1000,
                    UseCoefficients = true,
                    Series = "400",
                    PumpModel = "P10"
                });

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockNodeMaster = new Mock<INodeMaster>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var mockCurveCoordinate = new Mock<ICurveCoordinate>();

            var mockAnalysisCurveSet = new Mock<IAnalysisCurveSets>();

            var mockESPTornadoCurveSetAnnotation = new Mock<IESPTornadoCurveSetAnnotation>();

            var mockGasAwareEspCalculator = new Mock<IGasAwareESPCalculator>();

            var mockGasAwareWellCalculator = new Mock<IGasAwareWellCalculator>();

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
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();

            var responseData = new ESPAnalysisResponse()
            {
                PocType = 8,
                CardType = "P",
                CauseId = 0,
                NodeMasterData = nodeData.FirstOrDefault(),
                AnalysisResultEntity = null,
                WellPumpEntities = GetESPWellPumpData(),
            };

            var mockESPAnalysisService = new Mock<IESPAnalysis>();
            mockESPAnalysisService.Setup(x =>
                    x.GetESPAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000",
                    correlationId))
                .Returns(responseData);

            var service = new ESPAnalysisProcessingService(mockESPAnalysisService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockESPPump.Object,
                 mockNodeMaster.Object,
                mockAnalysisCurve.Object, mockCurveCoordinate.Object, mockAnalysisCurveSet.Object,
                mockESPTornadoCurveSetAnnotation.Object, mockGasAwareEspCalculator.Object,
                mockGasAwareWellCalculator.Object, _commonService.Object);

            var request = new WithCorrelationId<ESPAnalysisInput>(correlationId, new ESPAnalysisInput
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                TestDate = "2023-05-11 22:29:55.000"
            });

            var response = service.GetESPAnalysisResults(request);

            Assert.AreEqual(false, response.Result.Status);
            logger.Verify(x => x.WriteCId(Level.Info,
                    It.Is<string>(x => x.Contains("AnalysisResultEntity is null, cannot get esp analysis results.")),
                    correlationId),
                Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetESPAnalysisResultsNullWellPumpEntitiesResponseTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockESPPump = new Mock<IESPPump>();
            var correlationId = Guid.NewGuid().ToString();
            mockESPPump.Setup(x => x.GetESPPumpData(It.IsAny<object>(), correlationId))
                .Returns(new ESPPumpDataModel
                {
                    ESPPumpId = 4,
                    Pump = "P10",
                    MinCasingSize = 1,
                    MinBPD = 800,
                    MaxBPD = 1000,
                    UseCoefficients = true,
                    Series = "400",
                    PumpModel = "P10"
                });

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockNodeMaster = new Mock<INodeMaster>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var mockCurveCoordinate = new Mock<ICurveCoordinate>();

            var mockAnalysisCurveSet = new Mock<IAnalysisCurveSets>();

            var mockESPTornadoCurveSetAnnotation = new Mock<IESPTornadoCurveSetAnnotation>();

            var mockGasAwareEspCalculator = new Mock<IGasAwareESPCalculator>();

            var mockGasAwareWellCalculator = new Mock<IGasAwareWellCalculator>();

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
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();

            var responseData = new ESPAnalysisResponse()
            {
                PocType = 8,
                CardType = "P",
                CauseId = 0,
                NodeMasterData = nodeData.FirstOrDefault(),
                AnalysisResultEntity = GetAnalysisResultData(),
                WellPumpEntities = null,
            };

            var mockESPAnalysisService = new Mock<IESPAnalysis>();
            mockESPAnalysisService.Setup(x =>
                    x.GetESPAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000"
                    , correlationId))
                .Returns(responseData);

            var service = new ESPAnalysisProcessingService(mockESPAnalysisService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockESPPump.Object,
                 mockNodeMaster.Object,
                mockAnalysisCurve.Object, mockCurveCoordinate.Object, mockAnalysisCurveSet.Object,
                mockESPTornadoCurveSetAnnotation.Object, mockGasAwareEspCalculator.Object,
                mockGasAwareWellCalculator.Object, _commonService.Object);

            var request = new WithCorrelationId<ESPAnalysisInput>(correlationId, new ESPAnalysisInput
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                TestDate = "2023-05-11 22:29:55.000"
            });

            var response = service.GetESPAnalysisResults(request);

            Assert.AreEqual(false, response.Result.Status);
            logger.Verify(x => x.WriteCId(Level.Info,
                    It.Is<string>(x => x.Contains("WellPumpEntities is null, cannot get esp analysis results.")),
                    correlationId),
                Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetPumpDegradationResultTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockESPPump = new Mock<IESPPump>();
            var correlationId = Guid.NewGuid().ToString();
            mockESPPump.Setup(x => x.GetESPPumpData(It.IsAny<object>(), correlationId))
                .Returns(new ESPPumpDataModel
                {
                    ESPPumpId = 4,
                    Pump = "P10",
                    MinCasingSize = 1,
                    MinBPD = 800,
                    MaxBPD = 1000,
                    UseCoefficients = true,
                    Series = "400",
                    PumpModel = "P10"
                });

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockNodeMaster = new Mock<INodeMaster>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var mockCurveCoordinate = new Mock<ICurveCoordinate>();

            var mockAnalysisCurveSet = new Mock<IAnalysisCurveSets>();

            var mockESPTornadoCurveSetAnnotation = new Mock<IESPTornadoCurveSetAnnotation>();

            var mockGasAwareEspCalculator = new Mock<IGasAwareESPCalculator>();

            var mockGasAwareWellCalculator = new Mock<IGasAwareWellCalculator>();

            var phrases = GetPhraseData();

            var phraseid = GetPhraseIdData();

            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseid)).Returns(phrases);

            var pressureProfilePhrases = GetPhraseDataPressureProfile();

            var pressureProfilePhraseIds = GetPhraseIdDataPressureProfile();

            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), pressureProfilePhraseIds)).Returns(pressureProfilePhrases);

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

            var responseData = new ESPAnalysisResponse()
            {
                PocType = 8,
                CardType = "P",
                CauseId = 0,
                NodeMasterData = nodeData.FirstOrDefault(),
                AnalysisResultEntity = GetAnalysisResultData(),
                WellPumpEntities = GetESPWellPumpData(),
            };

            var mockESPAnalysisService = new Mock<IESPAnalysis>();
            mockESPAnalysisService.Setup(x =>
                    x.GetESPAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000"
                    , correlationId))
                .Returns(responseData);

            var service = new ESPAnalysisProcessingService(mockESPAnalysisService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockESPPump.Object,
                 mockNodeMaster.Object,
                mockAnalysisCurve.Object, mockCurveCoordinate.Object, mockAnalysisCurveSet.Object,
                mockESPTornadoCurveSetAnnotation.Object, mockGasAwareEspCalculator.Object,
                mockGasAwareWellCalculator.Object, _commonService.Object);

            var request = new WithCorrelationId<ESPAnalysisInput>(correlationId, new ESPAnalysisInput
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                TestDate = "2023-05-11 22:29:55.000"
            });

            var result = service.GetESPAnalysisResults(request);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ESPAnalysisDataOutput));
            Assert.AreEqual(true, result.Result.Status);
            mockESPAnalysisService.Verify(x => x.GetESPAnalysisData
                    (new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000", correlationId),
                Times.Once);

            var resultItem = result.Values.Outputs.First(x => x.Id == "PumpDegradation");
            Assert.AreEqual("8233 %", resultItem.DisplayValue);
            Assert.AreEqual("%", resultItem.MeasurementAbbreviation);
            Assert.AreEqual("8233.000183105469", resultItem.Value.ToString());
        }

        [TestMethod]
        public void GetMaxRunningFrequencyResultTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockESPPump = new Mock<IESPPump>();
            var correlationId = Guid.NewGuid().ToString();
            mockESPPump.Setup(x => x.GetESPPumpData(It.IsAny<object>(), correlationId))
                .Returns(new ESPPumpDataModel
                {
                    ESPPumpId = 4,
                    Pump = "P10",
                    MinCasingSize = 1,
                    MinBPD = 800,
                    MaxBPD = 1000,
                    UseCoefficients = true,
                    Series = "400",
                    PumpModel = "P10"
                });

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockNodeMaster = new Mock<INodeMaster>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var mockCurveCoordinate = new Mock<ICurveCoordinate>();

            var mockAnalysisCurveSet = new Mock<IAnalysisCurveSets>();

            var mockESPTornadoCurveSetAnnotation = new Mock<IESPTornadoCurveSetAnnotation>();

            var mockGasAwareEspCalculator = new Mock<IGasAwareESPCalculator>();

            var mockGasAwareWellCalculator = new Mock<IGasAwareWellCalculator>();

            var phrases = GetPhraseData();

            var phraseid = GetPhraseIdData();

            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseid)).Returns(phrases);

            var pressureProfilePhrases = GetPhraseDataPressureProfile();

            var pressureProfilePhraseIds = GetPhraseIdDataPressureProfile();

            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), pressureProfilePhraseIds)).Returns(pressureProfilePhrases);

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

            var responseData = new ESPAnalysisResponse()
            {
                PocType = 8,
                CardType = "P",
                CauseId = 0,
                NodeMasterData = nodeData.FirstOrDefault(),
                AnalysisResultEntity = GetAnalysisResultData(),
                WellPumpEntities = GetESPWellPumpData(),
            };

            var mockESPAnalysisService = new Mock<IESPAnalysis>();
            mockESPAnalysisService.Setup(x =>
                     x.GetESPAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000"
                     , correlationId))
                 .Returns(responseData);

            var service = new ESPAnalysisProcessingService(mockESPAnalysisService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockESPPump.Object,
                 mockNodeMaster.Object,
                mockAnalysisCurve.Object, mockCurveCoordinate.Object, mockAnalysisCurveSet.Object,
                mockESPTornadoCurveSetAnnotation.Object, mockGasAwareEspCalculator.Object,
                mockGasAwareWellCalculator.Object, _commonService.Object);

            var request = new WithCorrelationId<ESPAnalysisInput>(correlationId, new ESPAnalysisInput
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                TestDate = "2023-05-11 22:29:55.000"
            });

            var result = service.GetESPAnalysisResults(request);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ESPAnalysisDataOutput));
            Assert.AreEqual(true, result.Result.Status);
            mockESPAnalysisService.Verify(x => x.GetESPAnalysisData
                    (new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                    "2023-05-11 22:29:55.000", correlationId),
                Times.Once);

            var resultItem = result.Values.Outputs.First(x => x.Id == "MaximumRunningFrequency");
            Assert.AreEqual("3.22 Hz", resultItem.DisplayValue);
            Assert.AreEqual("Hz", resultItem.MeasurementAbbreviation);
            Assert.AreEqual("3.22", resultItem.Value.ToString());
        }

        [TestMethod]
        public void GetMotorLoadPercentageResultTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockESPPump = new Mock<IESPPump>();
            var correlationId = Guid.NewGuid().ToString();
            mockESPPump.Setup(x => x.GetESPPumpData(It.IsAny<object>(), correlationId))
                .Returns(new ESPPumpDataModel
                {
                    ESPPumpId = 4,
                    Pump = "P10",
                    MinCasingSize = 1,
                    MinBPD = 800,
                    MaxBPD = 1000,
                    UseCoefficients = true,
                    Series = "400",
                    PumpModel = "P10"
                });

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockNodeMaster = new Mock<INodeMaster>();

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();

            var mockCurveCoordinate = new Mock<ICurveCoordinate>();

            var mockAnalysisCurveSet = new Mock<IAnalysisCurveSets>();

            var mockESPTornadoCurveSetAnnotation = new Mock<IESPTornadoCurveSetAnnotation>();

            var mockGasAwareEspCalculator = new Mock<IGasAwareESPCalculator>();

            var mockGasAwareWellCalculator = new Mock<IGasAwareWellCalculator>();

            var phrases = GetPhraseData();

            var phraseid = GetPhraseIdData();

            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseid)).Returns(phrases);

            var pressureProfilePhrases = GetPhraseDataPressureProfile();

            var pressureProfilePhraseIds = GetPhraseIdDataPressureProfile();

            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), pressureProfilePhraseIds)).Returns(pressureProfilePhrases);

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

            var responseData = new ESPAnalysisResponse()
            {
                PocType = 8,
                CardType = "P",
                CauseId = 0,
                NodeMasterData = nodeData.FirstOrDefault(),
                AnalysisResultEntity = GetAnalysisResultData(),
                WellPumpEntities = GetESPWellPumpData(),
            };

            var mockESPAnalysisService = new Mock<IESPAnalysis>();
            mockESPAnalysisService.Setup(x =>
                    x.GetESPAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                    "2023-05-11 22:29:55.000", correlationId))
                .Returns(responseData);

            var service = new ESPAnalysisProcessingService(mockESPAnalysisService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockESPPump.Object,
                 mockNodeMaster.Object,
                mockAnalysisCurve.Object, mockCurveCoordinate.Object, mockAnalysisCurveSet.Object,
                mockESPTornadoCurveSetAnnotation.Object, mockGasAwareEspCalculator.Object,
                mockGasAwareWellCalculator.Object, _commonService.Object);

            var request = new WithCorrelationId<ESPAnalysisInput>(correlationId, new ESPAnalysisInput
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                TestDate = "2023-05-11 22:29:55.000"
            });

            var result = service.GetESPAnalysisResults(request);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ESPAnalysisDataOutput));
            Assert.AreEqual(true, result.Result.Status);
            mockESPAnalysisService.Verify(x => x.GetESPAnalysisData
                    (new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000", correlationId),
                Times.Once);

            var resultItem = result.Values.Outputs.First(x => x.Id == "MotorLoadPercentage");
            Assert.AreEqual("2.22 %", resultItem.DisplayValue);
            Assert.AreEqual("%", resultItem.MeasurementAbbreviation);
            Assert.AreEqual("2.22", resultItem.Value.ToString());
        }

        [TestMethod]
        public void GetGetCurveCoordinateOperatingPointTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockESPPump = new Mock<IESPPump>();
            var mockLocalePhrase = new Mock<ILocalePhrases>();
            var mockNodeMaster = new Mock<INodeMaster>();
            var correlationId = Guid.NewGuid().ToString();

            var mockGasAwareEspCalculator = new Mock<IGasAwareESPCalculator>();

            var mockGasAwareWellCalculator = new Mock<IGasAwareWellCalculator>();

            var getAnalysisResultCurvesResult = new List<AnalysisCurveModel>()
            {
                new AnalysisCurveModel()
                {
                    CurveTypeId = ESPCurveType.PumpCurve.Key,
                    Id = 1,
                },
            };

            var mockAnalysisCurve = new Mock<IAnalysisCurve>();
            mockAnalysisCurve.Setup(x => x.GetAnalysisResultCurves(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).Returns(getAnalysisResultCurvesResult);

            var getCurvesCoordinatesResult = new List<CurveCoordinatesModel>()
            {
                new CurveCoordinatesModel()
                {
                    CurveId = 1,
                    Id = 1,
                    X = 0,
                    Y = 0,
                }
            };

            var mockCurveCoordinate = new Mock<ICurveCoordinate>();
            mockCurveCoordinate.Setup(x => x.GetCurvesCoordinates(It.IsAny<int>(), It.IsAny<string>())).Returns(getCurvesCoordinatesResult);

            var mockAnalysisCurveSet = new Mock<IAnalysisCurveSets>();
            mockAnalysisCurveSet.Setup(x => x.GetAnalysisCurvesSet(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new List<AnalysisCurveSetModel>());

            var mockESPTornadoCurveSetAnnotation = new Mock<IESPTornadoCurveSetAnnotation>();
            mockESPTornadoCurveSetAnnotation.Setup(x => x.GetESPTornadoCurveSetAnnotations(correlationId))
                .Returns(new List<ESPTornadoCurveSetAnnotationModel>());

            var phrases = GetPhraseData();

            var phraseId = GetPhraseIdData();

            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseId)).Returns(phrases);

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

            var responseData = GetAnalysisResultData();

            var espAnalysisService = new Mock<IESPAnalysis>();
            espAnalysisService.Setup(x =>
                    x.GetESPAnalysisResult(It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .Returns(responseData);

            var service = new ESPAnalysisProcessingService(espAnalysisService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockESPPump.Object,
                 mockNodeMaster.Object,
                mockAnalysisCurve.Object, mockCurveCoordinate.Object, mockAnalysisCurveSet.Object,
                mockESPTornadoCurveSetAnnotation.Object, mockGasAwareEspCalculator.Object,
                mockGasAwareWellCalculator.Object, _commonService.Object);

            var request = new WithCorrelationId<CurveCoordinatesInput>(correlationId, new CurveCoordinatesInput()
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                TestDate = "2023-05-11 22:29:55.000"
            });

            var result = service.GetCurveCoordinate(request);

            Assert.IsNotNull(result);

            var operatingPoint = result.Values.First(x => x.CurveTypeId == -6);
            Assert.AreEqual(-6, operatingPoint.Id);
            Assert.AreEqual("OperatingPoint", operatingPoint.Name);
            Assert.AreEqual("Operating Point", operatingPoint.DisplayName);
            Assert.AreEqual(5000, operatingPoint.Coordinates[0].X);
            Assert.AreEqual(250, operatingPoint.Coordinates[0].Y);
        }

        #endregion

        #region Private Methods

        private ESPAnalysisResultModel GetAnalysisResultData()
        {
            var espAnalysisResults = new ESPAnalysisResultModel()
            {
                VerticalPumpDepth = 5,
                MeasuredPumpDepth = 5,
                PumpIntakePressure = 498.392f,
                GrossRate = 421.625f,
                FluidLevelAbovePump = 1204,
                TubingPressure = -124,
                CasingPressure = -124,
                WaterSpecificGravity = 1.05f,
                ProcessedDate = new DateTime(2023, 5, 11, 22, 29, 55),
                OilRate = 37.7f,
                WaterRate = 37.7f,
                GasRate = 6f,
                PumpDegradation = 82.33f,
                MaxRunningFrequency = 3.22f,
                MotorLoadPercentage = 2.22f,
                TotalVolumeAtPump = 5000,
                HeadAcrossPump = 250,
            };

            return espAnalysisResults;
        }

        private IList<ESPWellPumpModel> GetESPWellPumpData()
        {
            var espWellPumpModel = new List<ESPWellPumpModel>()
            {
                new ESPWellPumpModel
                {
                    ESPPumpId = 1247,
                    ESPWellId = "Well1",
                    NumberOfStages = 5,
                    OrderNumber = 1,
                },
                new ESPWellPumpModel
                {
                    ESPPumpId = 1246,
                    ESPWellId = "Well1",
                    NumberOfStages = 4,
                    OrderNumber = 2,
                },
                new ESPWellPumpModel
                {
                    ESPPumpId = 1245,
                    ESPWellId = "Well1",
                    NumberOfStages = 7,
                    OrderNumber = 3,
                },
                new ESPWellPumpModel
                {
                    ESPPumpId = 1243,
                    ESPWellId = "Well1",
                    NumberOfStages = 2,
                    OrderNumber = 4,
                }
            };

            return espWellPumpModel;
        }

        private Dictionary<int, string> GetPhraseData()
        {
            var phrases = new Dictionary<int, string>()
            {
                {102, "Test Date"},
                {258, "Pump Intake Pressure"},
                {264, "Tubing Pressure"},
                {265, "Casing Pressure"},
                {532, "Oil Rate"},
                {533, "Gross Rate"},
                {666, "Manufacturer"},
                {928, "Pump"},
                {1164, "Pump Efficiency"},
                {1250, "Water Rate"},
                {1251, "Gas Rate"},
                {1414, "Frequency"},
                {2459, "Formation Volume Factor"},
                {2460, "Solution GOR"},
                {2461, "Oil API"},
                {2790, "Pump Discharge Pressure"},
                {2953, "Number Of Stages"},
                {2954, "Pump Depth (Vertical)"},
                {2955, "Pump Depth (Measured)"},
                {4603, "Productivity Index"},
                {4816, "The selected well test was processed successfully."},
                {4830, "Specific Gravity of Gas"},
                {4831, "Bottomhole Temperature"},
                {4834, "Gas Compressibility Factor"},
                {4835, "Gas Volume Factor"},
                {4839, "Producing Gas Oil Ratio"},
                {4840, "Gas in Solution"},
                {4841, "Free Gas at Pump"},
                {4842, "Oil Volume at Pump"},
                {4843, "Gas Volume at Pump"},
                {4845, "Turpin Parameter"},
                {4846, "Composite Tubing Specific Gravity"},
                {4847, "Gas Density"},
                {4849, "Liquid Density"},
                {4850, "Tubing Gas"},
                {5086, "Fluid Level Above Pump"},
                {5099, "ΔPtr"},
                {5106, "Pressure Across Pump"},
                {5576, "lb/cf"},
                {5579, "Gas Separator Efficiency"},
                {5580, "Casing Inner Diameter"},
                {5581, "Tubing Outer Diameter"},
                {5584, "Specific Gravity of Oil"},
                {5593, "Total Volume at Pump"},
                {5594, "Free Gas"},
                {5599, "Annular Separation"},
                {5633, "Casing Valve State"},
                {5636, "b/mscf"},
                {5642, "scf/b"},
                {5644, "Flowing BHP"},
                {5985, "Pump Degradation"},
                {6481, "Mid-Perforation Depth" },
                {6818, "Max Running Frequency"},
                {6823, "Motor Load"},
            };
            return phrases;
        }

        private int[] GetPhraseIdData()
        {
            var phraseid = new int[]
            {
                102,
                258,
                264,
                265,
                532,
                533,
                666,
                928,
                1164,
                1250,
                1251,
                1414,
                2459,
                2460,
                2461,
                2790,
                2953,
                2954,
                2955,
                4603,
                4816,
                4830,
                4831,
                4834,
                4835,
                4839,
                4840,
                4841,
                4842,
                4843,
                4845,
                4846,
                4847,
                4849,
                4850,
                5086,
                5099,
                5106,
                5576,
                5579,
                5580,
                5581,
                5584,
                5593,
                5594,
                5599,
                5633,
                5636,
                5642,
                5644,
                5985,
                6481,
                6818,
                6823,
            };
            return phraseid;
        }

        private Dictionary<int, string> GetPhraseDataPressureProfile()
        {
            var phrases = new Dictionary<int, string>()
            {
                { 258, "258" },
                { 2760, "2760" },
                { 5561, "5561" },
                { 6483, "6483" },
                { 2790, "2790" },
                { 5097, "5097" },
                { 5106, "5106" },
                { 5099, "5099" },
                { 265, "265" },
                { 264, "264" },
                { 5577, "5577" },
                { 1250, "1250" },
                { 532, "532" },
                { 5617, "5617" },
                { 2954, "2954" },
                { 5557, "5557" },
                { 6481, "6481" }
            };
            return phrases;
        }

        private int[] GetPhraseIdDataPressureProfile()
        {
            var phraseid = new int[]
            {
                258,
                264,
                265,
                532,
                1250,
                2760,
                2790,
                2954,
                5097,
                5099,
                5106,
                5557,
                5561,
                5577,
                5617,
                6481,
                6483,
            };
            return phraseid;
        }

        private void SetupCommonService()
        {
            _commonService.Setup(x => x.GetSystemParameterNextGenSignificantDigits(It.IsAny<string>())).Returns(3);
        }

        #endregion

    }
}
