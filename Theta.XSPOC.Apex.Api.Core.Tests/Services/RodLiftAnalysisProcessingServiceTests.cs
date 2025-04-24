using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Core.Tests.Services
{
    [TestClass]
    public class RodLiftAnalysisProcessingServiceTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLoggerTest()
        {
            var mockService = new Mock<IRodLiftAnalysis>();

            var mockCommonService = new Mock<ICommonService>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockCardCoordinateService = new Mock<ICardCoordinate>();

            _ = new RodLiftAnalysisProcessingService(mockService.Object, null, mockCommonService.Object, mockLocalePhrase.Object,
                mockCardCoordinateService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullRodLiftAnalysisProcessingServiceTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockCommonService = new Mock<ICommonService>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockCardCoordinateService = new Mock<ICardCoordinate>();

            _ = new RodLiftAnalysisProcessingService(null, mockThetaLoggerFactory.Object, mockCommonService.Object,
                mockLocalePhrase.Object, mockCardCoordinateService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullCommonServiceProcessingServiceTest()
        {
            var mockService = new Mock<IRodLiftAnalysis>();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockCardCoordinateService = new Mock<ICardCoordinate>();

            _ = new RodLiftAnalysisProcessingService(mockService.Object, mockThetaLoggerFactory.Object, null,
                mockLocalePhrase.Object, mockCardCoordinateService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLocalePhraseProcessingServiceTest()
        {
            var mockService = new Mock<IRodLiftAnalysis>();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockCommonService = new Mock<ICommonService>();
            var mockCardCoordinateService = new Mock<ICardCoordinate>();
            _ = new RodLiftAnalysisProcessingService(mockService.Object, mockThetaLoggerFactory.Object, mockCommonService.Object,
                null, mockCardCoordinateService.Object);
        }

        [TestMethod]
        public async Task GetRodLiftAnalysisResultsTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockCommonService = new Mock<ICommonService>();
            mockCommonService.Setup(x => x.GetSystemParameterNextGenSignificantDigits(It.IsAny<string>())).Returns(3);

            var mockLocalePhrase = new Mock<ILocalePhrases>();
            var phrases = new Dictionary<int, string>()
            {
                {
                    7048,
                    "XDIAG has determined the pumping unit can increase speed to {0} SPM. This will result in an additional {1} barrels of gross and {2} barrels of oil."
                },
                {
                    7049,
                    "XDIAG has determined that the pumping unit is already operating close to maximum speed for rod pump system. In order to capture potential uplift, design changes will be necessary."
                },
                {
                    7117, "Incremental production at {0} SPM is less than {1} bbls of oil per day."
                },
                {
                    6849,
                    "An incremental oil uplift opportunity has been discovered for this well test, this well is capable of producing an additional {0} ({1}) of total fluid, resulting in an additional {2} ({3}) of oil."
                },
                {
                    98, "SPM"
                },
                {
                    99, "Str. Length"
                },
                {
                    100, "Pmp Diam"
                },
                {
                    102, "Test Date"
                },
                {
                    103, "Test Gas"
                },
                {
                    104, "Test Oil"
                },
                {
                    105, "Test Gross"
                },
                {
                    106, "SurCap@24"
                },
                {
                    107, "Pumping Unit"
                },
                {
                    109, "Cycles"
                },
                {
                    115, "DH Stroke"
                },
                {
                    116, "DH Cap@24"
                },
                {
                    117, "DH Cap@RT"
                },
                {
                    118, "DH Cap@RT, Fillage"
                },
                {
                    120, "Fluid Load"
                },
                {
                    121, "Buoyant Rod Weight"
                },
                {
                    122, "Dry Rod Weight"
                },
                {
                    123, "Pump Friction Load"
                },
                {
                    124, "PO Fluid Load "
                },
                {
                    125, "Analysis Data"
                },
                {
                    157, "Inf Prd, Today"
                },
                {
                    158, "Inf Prd, Yest"
                },
                {
                    200, "N/A"
                },
                {
                    271, "Pump Depth"
                },
                {
                    536, "Runtime"
                },
                {
                    1110, "Test Water"
                },
                {
                    1164, "Pump Efficiency"
                },
                {
                    1178, "Fillage Setpoint"
                },
                {
                    1980, "Pump Fillage"
                },
                {
                    2091, "SWT Oil Yesterday"
                },
                {
                    2092, "SWT Water Yesterday"
                },
                {
                    2093, "SWT Gas Yesterday"
                },
                {
                    2732, "Idle Time"
                },
                {
                    6819,
                    "An incremental oil uplift opportunity has been discovered for this well test, this well is capable of producing an additional {0} ({1}) of total fluid, resulting in an additional {2} ({3}) of oil."
                },
                {
                    6848, "Peak Net Torque"
                },
            };

            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), It.IsAny<int[]>())).Returns(phrases);

            var values = new RodLiftAnalysisValues
            {
                Input = new List<ValueItem>()
                {
                    new ValueItem()
                    {
                        Id = "Runtime",
                        Name = "",
                        DisplayName = "",
                        Value = "",
                        DisplayValue = "24",
                        DataTypeId = 0,
                        MeasurementAbbreviation = "",
                        SourceId = 0
                    },
                    new ValueItem()
                    {
                        Id = "StrokesPerMinute",
                        Name = "",
                        DisplayName = "",
                        Value = "",
                        DisplayValue = "24",
                        DataTypeId = 0,
                        MeasurementAbbreviation = "",
                        SourceId = 0
                    },
                },
                Output = new List<ValueItem>()
                {
                    new ValueItem()
                    {
                        Id = "StrokeLength",
                        Name = "",
                        DisplayName = "",
                        Value = "",
                        DisplayValue = "24",
                        DataTypeId = 0,
                        MeasurementAbbreviation = "",
                        SourceId = 0
                    },
                    new ValueItem()
                    {
                        Id = "SurfaceCapacity24",
                        Name = "",
                        DisplayName = "SurCap@24",
                        Value = 100,
                        DisplayValue = "100 bpd",
                        DataTypeId = 0,
                        MeasurementAbbreviation = "bpd",
                        SourceId = null
                    },
                }
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

            var responseData = new RodLiftAnalysisResponse()
            {
                PocType = 8,
                CardType = "P",
                CauseId = 0,
                NodeMasterData = nodeData.FirstOrDefault(),
                WellDetails = GetWellDetails().FirstOrDefault(),
                CardData = GetCardData().FirstOrDefault(),
                WellTestData = GetWellTestData().FirstOrDefault(),
                XDiagResults = GetXdiagResultsData().FirstOrDefault(),
                CurrentRawScanData = GetCurrentRawScanData()
            };

            var mockRodLiftAnalysisService = new Mock<IRodLiftAnalysis>();
            mockRodLiftAnalysisService.Setup(x =>
                    x.GetRodLiftAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000",
                        "correlationId1"))
                .Returns(responseData);

            var mockCardCoordinateService = new Mock<ICardCoordinate>();
            var service = new RodLiftAnalysisProcessingService(mockRodLiftAnalysisService.Object,
                mockThetaLoggerFactory.Object, mockCommonService.Object, mockLocalePhrase.Object, mockCardCoordinateService.Object);

            var request = new WithCorrelationId<RodLiftAnalysisInput>("correlationId1", new RodLiftAnalysisInput
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                CardDate = "2023-05-11 22:29:55.000"
            });

            var result = await service.GetRodLiftAnalysisResultsAsync(request);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RodLiftAnalysisDataOutput));
            Assert.AreEqual(true, result.Result.Status);
            mockRodLiftAnalysisService.Verify(x => x.GetRodLiftAnalysisData
                    (new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000", "correlationId1"),
                Times.Once);

            var resultItem = result.Values.Input.First(x => x.Id == "SurfaceCapacity24");
            Assert.AreEqual("131 bpd", resultItem.DisplayValue);
            Assert.AreEqual("bpd", resultItem.MeasurementAbbreviation);
            Assert.AreEqual("131.0919952392578", resultItem.Value.ToString());
        }

        [TestMethod]
        public async Task GetRodLiftAnalysisResultsRoundValueTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockCommonService = new Mock<ICommonService>();
            mockCommonService.Setup(x => x.GetSystemParameterNextGenSignificantDigits(It.IsAny<string>())).Returns(3);

            var mockLocalePhrase = new Mock<ILocalePhrases>();
            var phrases = new Dictionary<int, string>()
            {
                {
                    7048,
                    "XDIAG has determined the pumping unit can increase speed to {0} SPM. This will result in an additional {1} barrels of gross and {2} barrels of oil."
                },
                {
                    7049,
                    "XDIAG has determined that the pumping unit is already operating close to maximum speed for rod pump system. In order to capture potential uplift, design changes will be necessary."
                },
                {
                    7117, "Incremental production at {0} SPM is less than {1} bbls of oil per day."
                },
                {
                    6849,
                    "An incremental oil uplift opportunity has been discovered for this well test, this well is capable of producing an additional {0} ({1}) of total fluid, resulting in an additional {2} ({3}) of oil."
                },
                {
                    98, "SPM"
                },
                {
                    99, "Str. Length"
                },
                {
                    100, "Pmp Diam"
                },
                {
                    102, "Test Date"
                },
                {
                    103, "Test Gas"
                },
                {
                    104, "Test Oil"
                },
                {
                    105, "Test Gross"
                },
                {
                    106, "SurCap@24"
                },
                {
                    107, "Pumping Unit"
                },
                {
                    109, "Cycles"
                },
                {
                    115, "DH Stroke"
                },
                {
                    116, "DH Cap@24"
                },
                {
                    117, "DH Cap@RT"
                },
                {
                    118, "DH Cap@RT, Fillage"
                },
                {
                    120, "Fluid Load"
                },
                {
                    121, "Buoyant Rod Weight"
                },
                {
                    122, "Dry Rod Weight"
                },
                {
                    123, "Pump Friction Load"
                },
                {
                    124, "PO Fluid Load "
                },
                {
                    125, "Analysis Data"
                },
                {
                    157, "Inf Prd, Today"
                },
                {
                    158, "Inf Prd, Yest"
                },
                {
                    200, "N/A"
                },
                {
                    271, "Pump Depth"
                },
                {
                    536, "Runtime"
                },
                {
                    1110, "Test Water"
                },
                {
                    1164, "Pump Efficiency"
                },
                {
                    1178, "Fillage Setpoint"
                },
                {
                    1980, "Pump Fillage"
                },
                {
                    2091, "SWT Oil Yesterday"
                },
                {
                    2092, "SWT Water Yesterday"
                },
                {
                    2093, "SWT Gas Yesterday"
                },
                {
                    2732, "Idle Time"
                },
                {
                    6819,
                    "An incremental oil uplift opportunity has been discovered for this well test, this well is capable of producing an additional {0} ({1}) of total fluid, resulting in an additional {2} ({3}) of oil."
                },
                {
                    6848, "Peak Net Torque"
                },
            };

            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), It.IsAny<int[]>())).Returns(phrases);

            var values = new RodLiftAnalysisValues
            {
                Input = new List<ValueItem>()
                {
                    new ValueItem()
                    {
                        Id = "Runtime",
                        Name = "",
                        DisplayName = "",
                        Value = "",
                        DisplayValue = "24",
                        DataTypeId = 0,
                        MeasurementAbbreviation = "",
                        SourceId = 0
                    },
                    new ValueItem()
                    {
                        Id = "StrokesPerMinute",
                        Name = "",
                        DisplayName = "",
                        Value = "",
                        DisplayValue = "24",
                        DataTypeId = 0,
                        MeasurementAbbreviation = "",
                        SourceId = 0
                    },
                },
                Output = new List<ValueItem>()
                {
                    new ValueItem()
                    {
                        Id = "StrokeLength",
                        Name = "",
                        DisplayName = "",
                        Value = "",
                        DisplayValue = "24",
                        DataTypeId = 0,
                        MeasurementAbbreviation = "",
                        SourceId = 0
                    },
                    new ValueItem()
                    {
                        Id = "SurfaceCapacity24",
                        Name = "",
                        DisplayName = "SurCap@24",
                        Value = 100,
                        DisplayValue = "100 bpd",
                        DataTypeId = 0,
                        MeasurementAbbreviation = "bpd",
                        SourceId = null
                    },
                }
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

            var responseData = new RodLiftAnalysisResponse()
            {
                PocType = 8,
                CardType = "P",
                CauseId = 0,
                NodeMasterData = nodeData.FirstOrDefault(),
                WellDetails = GetWellDetails().FirstOrDefault(),
                CardData = GetCardData().FirstOrDefault(),
                WellTestData = GetWellTestData().FirstOrDefault(),
                XDiagResults = GetXdiagResultsData().FirstOrDefault(),
                CurrentRawScanData = GetCurrentRawScanData()
            };

            var mockRodLiftAnalysisService = new Mock<IRodLiftAnalysis>();
            mockRodLiftAnalysisService.Setup(x =>
                    x.GetRodLiftAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000",
                        "correlationId1"))
                .Returns(responseData);

            var mockCardCoordinateService = new Mock<ICardCoordinate>();
            var service = new RodLiftAnalysisProcessingService(mockRodLiftAnalysisService.Object,
                mockThetaLoggerFactory.Object, mockCommonService.Object, mockLocalePhrase.Object, mockCardCoordinateService.Object);

            var request = new WithCorrelationId<RodLiftAnalysisInput>("correlationId1", new RodLiftAnalysisInput
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                CardDate = "2023-05-11 22:29:55.000"
            });

            var result = await service.GetRodLiftAnalysisResultsAsync(request);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(RodLiftAnalysisDataOutput));
            Assert.AreEqual(true, result.Result.Status);
            mockRodLiftAnalysisService.Verify(x => x.GetRodLiftAnalysisData
                    (new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000", "correlationId1"),
                Times.Once);
            #region Input Validation
            //SurfaceCapacity24
            Assert.AreEqual("131 bpd", result.Values.Input.First(x => x.Id == "SurfaceCapacity24").DisplayValue);
            Assert.AreEqual("bpd", result.Values.Input.First(x => x.Id == "SurfaceCapacity24").MeasurementAbbreviation);
            Assert.AreEqual("131.0919952392578", result.Values.Input.First(x => x.Id == "SurfaceCapacity24").Value.ToString());
            //Runtime
            Assert.AreEqual("24 h", result.Values.Input.First(x => x.Id == "Runtime").DisplayValue);
            Assert.AreEqual("h", result.Values.Input.First(x => x.Id == "Runtime").MeasurementAbbreviation);
            Assert.AreEqual("24", result.Values.Input.First(x => x.Id == "Runtime").Value.ToString());
            //StrokesPerMinute
            Assert.AreEqual("8", result.Values.Input.First(x => x.Id == "StrokesPerMinute").DisplayValue.Trim());
            Assert.AreEqual("8", result.Values.Input.First(x => x.Id == "StrokesPerMinute").Value.ToString());
            //StrokeLength
            Assert.AreEqual("35 in", result.Values.Input.First(x => x.Id == "StrokeLength").DisplayValue);
            Assert.AreEqual("in", result.Values.Input.First(x => x.Id == "StrokeLength").MeasurementAbbreviation);
            Assert.AreEqual("35", result.Values.Input.First(x => x.Id == "StrokeLength").Value.ToString());
            //PumpDiameter
            Assert.AreEqual("2 in", result.Values.Input.First(x => x.Id == "PumpDiameter").DisplayValue);
            Assert.AreEqual("in", result.Values.Input.First(x => x.Id == "PumpDiameter").MeasurementAbbreviation);
            Assert.AreEqual("2", result.Values.Input.First(x => x.Id == "PumpDiameter").Value.ToString());
            //PumpDepth
            Assert.AreEqual("999 ft", result.Values.Input.First(x => x.Id == "PumpDepth").DisplayValue);
            Assert.AreEqual("ft", result.Values.Input.First(x => x.Id == "PumpDepth").MeasurementAbbreviation);
            Assert.AreEqual("999", result.Values.Input.First(x => x.Id == "PumpDepth").Value.ToString());
            //WellTestGas
            Assert.AreEqual("6 mscf/d", result.Values.Input.First(x => x.Id == "WellTestGas").DisplayValue);
            Assert.AreEqual("mscf/d", result.Values.Input.First(x => x.Id == "WellTestGas").MeasurementAbbreviation);
            Assert.AreEqual("6", result.Values.Input.First(x => x.Id == "WellTestGas").Value.ToString());
            //WellTestOil
            Assert.AreEqual("37.7 bpd", result.Values.Input.First(x => x.Id == "WellTestOil").DisplayValue);
            Assert.AreEqual("bpd", result.Values.Input.First(x => x.Id == "WellTestOil").MeasurementAbbreviation);
            Assert.AreEqual("37.70000076293945", result.Values.Input.First(x => x.Id == "WellTestOil").Value.ToString());
            //WellTestWater
            Assert.AreEqual("37.7 bpd", result.Values.Input.First(x => x.Id == "WellTestWater").DisplayValue);
            Assert.AreEqual("bpd", result.Values.Input.First(x => x.Id == "WellTestWater").MeasurementAbbreviation);
            Assert.AreEqual("37.70000076293945", result.Values.Input.First(x => x.Id == "WellTestWater").Value.ToString());
            //WellTestGross
            Assert.AreEqual("75.4 bpd", result.Values.Input.First(x => x.Id == "WellTestGross").DisplayValue);
            Assert.AreEqual("bpd", result.Values.Input.First(x => x.Id == "WellTestGross").MeasurementAbbreviation);
            Assert.AreEqual("75.4000015258789", result.Values.Input.First(x => x.Id == "WellTestGross").Value.ToString());
            #endregion
            #region Output validation
            //Fluid Load
            Assert.AreEqual("2 lb", result.Values.Output.First(x => x.Id == "FluidLoad").DisplayValue);
            Assert.AreEqual("lb", result.Values.Output.First(x => x.Id == "FluidLoad").MeasurementAbbreviation);
            Assert.AreEqual("2", result.Values.Output.First(x => x.Id == "FluidLoad").Value.ToString());
            //Buoyant Rod Weight
            Assert.AreEqual("2 lb", result.Values.Output.First(x => x.Id == "BuoyantRodWeight").DisplayValue);
            Assert.AreEqual("lb", result.Values.Output.First(x => x.Id == "BuoyantRodWeight").MeasurementAbbreviation);
            Assert.AreEqual("2", result.Values.Output.First(x => x.Id == "BuoyantRodWeight").Value.ToString());
            //Dry Rod Weight
            Assert.AreEqual("2 lb", result.Values.Output.First(x => x.Id == "DryRodWeight").DisplayValue);
            Assert.AreEqual("lb", result.Values.Output.First(x => x.Id == "DryRodWeight").MeasurementAbbreviation);
            Assert.AreEqual("2", result.Values.Output.First(x => x.Id == "DryRodWeight").Value.ToString());
            //PO Fluid Load 
            Assert.AreEqual("2 lb", result.Values.Output.First(x => x.Id == "POFluidLoad").DisplayValue);
            Assert.AreEqual("lb", result.Values.Output.First(x => x.Id == "POFluidLoad").MeasurementAbbreviation);
            Assert.AreEqual("2", result.Values.Output.First(x => x.Id == "POFluidLoad").Value.ToString());
            #endregion

        }

        [TestMethod]
        public async Task GetRodLiftAnalysisResultsNullDataTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockCommonService = new Mock<ICommonService>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockRodLiftAnalysisService = new Mock<IRodLiftAnalysis>();
            mockRodLiftAnalysisService.Setup(x =>
                x.GetRodLiftAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000",
                    "correlationId1"));

            var mockCardCoordinateService = new Mock<ICardCoordinate>();

            var service = new RodLiftAnalysisProcessingService(mockRodLiftAnalysisService.Object,
                mockThetaLoggerFactory.Object, mockCommonService.Object, mockLocalePhrase.Object, mockCardCoordinateService.Object);

            var result = await service.GetRodLiftAnalysisResultsAsync(null);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Result.Status);
            logger.Verify(x => x.Write(Level.Info,
                    It.Is<string>(x => x.Contains("data is null, cannot get rod lift analysis results."))),
                Times.AtLeastOnce);
            mockRodLiftAnalysisService.Verify(x => x.GetRodLiftAnalysisData
                (new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000", "correlationId1"), Times.Never);
        }

        [TestMethod]
        public async Task GetRodLiftAnalysisResultsNullPayloadValueTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockCommonService = new Mock<ICommonService>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockRodLiftAnalysisService = new Mock<IRodLiftAnalysis>();
            mockRodLiftAnalysisService.Setup(x =>
                x.GetRodLiftAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000",
                    "correlationId1"));
            var mockCardCoordinateService = new Mock<ICardCoordinate>();
            var service = new RodLiftAnalysisProcessingService(mockRodLiftAnalysisService.Object,
                mockThetaLoggerFactory.Object, mockCommonService.Object, mockLocalePhrase.Object, mockCardCoordinateService.Object);

            var request = new WithCorrelationId<RodLiftAnalysisInput>("correlationId1", null);

            var result = await service.GetRodLiftAnalysisResultsAsync(request);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Result.Status);
            logger.Verify(x => x.WriteCId(Level.Info,
                    It.Is<string>(x => x.Contains("data is null, cannot get rod lift analysis results.")), "correlationId1"),
                Times.AtLeastOnce);
            mockRodLiftAnalysisService.Verify(x => x.GetRodLiftAnalysisData
                (new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000", "correlationId1"), Times.Never);
        }

        [TestMethod]
        public async Task GetRodLiftAnalysisResultsNullAssetIdOrCardDateTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockCommonService = new Mock<ICommonService>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var mockRodLiftAnalysisService = new Mock<IRodLiftAnalysis>();
            mockRodLiftAnalysisService.Setup(x =>
                x.GetRodLiftAnalysisData(Guid.Empty, "2023 -05-11 22:29:55.000", "correlationId1"));
            var mockCardCoordinateService = new Mock<ICardCoordinate>();
            var service = new RodLiftAnalysisProcessingService(mockRodLiftAnalysisService.Object,
                mockThetaLoggerFactory.Object, mockCommonService.Object, mockLocalePhrase.Object, mockCardCoordinateService.Object);

            var request = new WithCorrelationId<RodLiftAnalysisInput>("correlationId1", new RodLiftAnalysisInput
            {
                AssetId = Guid.Empty,
                CardDate = ""
            });

            var result = await service.GetRodLiftAnalysisResultsAsync(request);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Result.Status);
            logger.Verify(x => x.WriteCId(Level.Info,
                    It.Is<string>(x => x.Contains("CardDate and AssetId should be provided to get rod lift analysis results.")),
                    "correlationId1"),
                Times.AtLeastOnce);
            mockRodLiftAnalysisService.Verify(
                x => x.GetRodLiftAnalysisData(Guid.Empty, "2023-05-11 22:29:55.000", "correlationId1"), Times.Never);
        }

        [TestMethod]
        public void GetCardDateTest()
        {
            var cardDateMockService = new Mock<IRodLiftAnalysis>();
            var loggerMock = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            var commonMockService = new Mock<ICommonService>();
            var localPhrasesMockService = new Mock<ILocalePhrases>();
            var statesMockService = new Mock<IStates>();

            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(loggerMock.Object);

            var correlationId = "CorrelationId1";
            var assetId = Guid.Parse("E432CB3B-295C-4ECB-8737-90D5D76AC6CF");

            cardDateMockService
                .Setup(x => x.GetCardDatesByAssetId(assetId, correlationId))
                .Returns(new List<CardDateModel>
                {
                    new CardDateModel
                    {
                        Date = DateTime.Now,
                        CauseId = 99,
                        CardTypeId = "P",
                        PocType = 419,
                    }
                });

            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            commonMockService
                .Setup(x => x.GetCardTypeName(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<short>(), ref phraseCache, ref causeIdPhraseCache, correlationId))
                .Returns("TestCardTypeName");

            var mockCardCoordinateService = new Mock<ICardCoordinate>();
            var service = new RodLiftAnalysisProcessingService(cardDateMockService.Object,
                mockThetaLoggerFactory.Object, commonMockService.Object, localPhrasesMockService.Object,
                mockCardCoordinateService.Object);

            var cardDateData = new CardDateInput()
            {
                AssetId = Guid.Parse("E432CB3B-295C-4ECB-8737-90D5D76AC6CF"),
            };

            var data = new WithCorrelationId<CardDateInput>(correlationId, cardDateData);
            var result = service.GetCardDate(data);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Values.Count);
        }

        [TestMethod]
        public void GetCardDateNullAssetIdTest()
        {
            var cardDateMockService = new Mock<IRodLiftAnalysis>();
            var loggerMock = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            var commonMockService = new Mock<ICommonService>();
            var localPhrasesMockService = new Mock<ILocalePhrases>();

            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(loggerMock.Object);

            var correlationId = "CorrelationId1";
            var assetId = Guid.Empty;

            cardDateMockService
                .Setup(x => x.GetCardDatesByAssetId(assetId, correlationId))
                .Returns(new List<CardDateModel>
                {
                    new CardDateModel
                    {
                        Date = DateTime.Now,
                        CauseId = 99,
                        CardTypeId = "P",
                        PocType = 419,
                    }
                });

            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            commonMockService
                .Setup(x => x.GetCardTypeName(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<short>(), ref phraseCache, ref causeIdPhraseCache, correlationId))
                .Returns("TestCardTypeName");

            var mockCardCoordinateService = new Mock<ICardCoordinate>();
            var cardDateService = new RodLiftAnalysisProcessingService(cardDateMockService.Object,
                mockThetaLoggerFactory.Object, commonMockService.Object, localPhrasesMockService.Object, mockCardCoordinateService.Object);

            var cardDateData = new CardDateInput()
            {
                AssetId = assetId,
            };

            var data = new WithCorrelationId<CardDateInput>(correlationId, cardDateData);
            var result = cardDateService.GetCardDate(data);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Values.Count);
            Assert.IsFalse(result.Result.Status);
            Assert.IsTrue(result.Result.Value.Contains("request should be provided to get card date."));

            loggerMock.Verify(x => x.WriteCId(Level.Info,
                    It.Is<string>(x => x.Contains("request should be provided to get card date.")), "CorrelationId1"),
                Times.Once);
        }

        [TestMethod]
        public void GetCardDateNullCardDateTest()
        {
            var cardDateMockService = new Mock<IRodLiftAnalysis>();
            var loggerMock = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            var commonMockService = new Mock<ICommonService>();
            var localPhrasesMockService = new Mock<ILocalePhrases>();

            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(loggerMock.Object);

            var correlationId = "CorrelationId1";
            var assetId = Guid.Empty;

            cardDateMockService
                .Setup(x => x.GetCardDatesByAssetId(assetId, correlationId))
                .Returns(new List<CardDateModel>
                {
                    new CardDateModel
                    {
                        Date = DateTime.Now,
                        CauseId = 99,
                        CardTypeId = "P",
                        PocType = 419,
                    }
                });

            IDictionary<int, string> phraseCache = new Dictionary<int, string>();
            IDictionary<int, string> causeIdPhraseCache = new Dictionary<int, string>();

            commonMockService
                .Setup(x => x.GetCardTypeName(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<short>(), ref phraseCache, ref causeIdPhraseCache, correlationId))
                .Returns("TestCardTypeName");

            var mockCardCoordinateService = new Mock<ICardCoordinate>();
            var cardDateService = new RodLiftAnalysisProcessingService(cardDateMockService.Object,
                mockThetaLoggerFactory.Object, commonMockService.Object, localPhrasesMockService.Object, mockCardCoordinateService.Object);

            var data = new WithCorrelationId<CardDateInput>(correlationId, null);
            var result = cardDateService.GetCardDate(data);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Values.Count);
            Assert.IsFalse(result.Result.Status);
            Assert.IsTrue(result.Result.Value.Contains("requestWithCorrelationId is null, cannot get card date."));

            loggerMock.Verify(x => x.WriteCId(Level.Info,
                    It.Is<string>(x => x.Contains("requestWithCorrelationId is null, cannot get card date.")), "CorrelationId1"),
                Times.Once);
        }

        [TestMethod]
        public void GetCardCoordinateResultsTest()
        {
            var mockService = new Mock<IRodLiftAnalysis>();
            var mockCommonService = new Mock<ICommonService>();
            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var correlationId = Guid.NewGuid().ToString();

            var mockCardCoordinateService = new Mock<ICardCoordinate>();
            mockCardCoordinateService.Setup(x =>
                    x.GetCardCoordinateData(new Guid("f3eb743c-f890-44f3-80e5-6a46df7ce2b7"), "2022-11-09 23:48:13.000", "correlationId"))
                .Returns(new CardCoordinateModel
                {
                    StrokesPerMinute = 8,
                    StrokeLength = 35,
                    SecondaryPumpFillage = 40,
                    AreaLimit = 0,
                    Area = 3800,
                    DownHoleCardBinary = null,
                    Fillage = 96,
                    FillBasePercent = 45,
                    HiLoadLimit = 50000,
                    LoadLimit = 456,
                    LoadLimit2 = 0,
                    LowLoadLimit = 0,
                    MalfunctionLoadLimit = 0,
                    MalfunctionPositionLimit = 28,
                    PermissibleLoadDownBinary = null,
                    PermissibleLoadUpBinary = null,
                    POCDownholeCard = null,
                    PocDownHoleCardBinary = null,
                    PositionLimit = 55,
                    PositionLimit2 = 0,
                    PredictedCard = null,
                    PredictedCardBinary = null,
                    SurfaceCardBinary = null,
                    PocType = 8
                });

            var service = new RodLiftAnalysisProcessingService(mockService.Object, mockThetaLoggerFactory.Object,
                mockCommonService.Object,
                mockLocalePhrase.Object,
                mockCardCoordinateService.Object);

            var request = new WithCorrelationId<CardCoordinateInput>(correlationId, new CardCoordinateInput
            {
                AssetId = new Guid("f3eb743c-f890-44f3-80e5-6a46df7ce2b7"),
                CardDate = "2022-11-09 23:48:13.000"
            });
            var result = service.GetCardCoordinateResults(request);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CardCoordinateDataOutput));
            Assert.AreEqual(false, result.Result.Status);
            mockCardCoordinateService.Verify(x => x.GetCardCoordinateData
                    (new Guid("f3eb743c-f890-44f3-80e5-6a46df7ce2b7"), "2022-11-09 23:48:13.000", "correlationId"),
                Times.Never);
        }

        [TestMethod]
        public void GetCardCoordinateResultsNullDataTest()
        {
            var mockService = new Mock<IRodLiftAnalysis>();
            var mockLocalePhrase = new Mock<ILocalePhrases>();
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockCommonService = new Mock<ICommonService>();
            var correlationId = Guid.NewGuid().ToString();

            var mockCardCoordinateService = new Mock<ICardCoordinate>();
            mockCardCoordinateService.Setup(x =>
                x.GetCardCoordinateData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000", "correlationId"));

            var service = new RodLiftAnalysisProcessingService(mockService.Object, mockThetaLoggerFactory.Object,
                mockCommonService.Object,
                mockLocalePhrase.Object,
                mockCardCoordinateService.Object);

            var result = service.GetCardCoordinateResults(null);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Result.Status);
            mockCardCoordinateService.Verify(x => x.GetCardCoordinateData
                (new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000", "correlationId"), Times.Never);
        }

        [TestMethod]
        public void GetCardCoordinateResultsNullPayloadValueTest()
        {
            var mockService = new Mock<IRodLiftAnalysis>();
            var mockCommonService = new Mock<ICommonService>();
            var mockLocalePhrase = new Mock<ILocalePhrases>();
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var correlationId = Guid.NewGuid().ToString();

            var mockCardCoordinateService = new Mock<ICardCoordinate>();
            mockCardCoordinateService.Setup(x =>
                x.GetCardCoordinateData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000", "correlationId"));

            var service = new RodLiftAnalysisProcessingService(mockService.Object, mockThetaLoggerFactory.Object,
                mockCommonService.Object,
                mockLocalePhrase.Object,
                mockCardCoordinateService.Object);

            var request = new WithCorrelationId<CardCoordinateInput>("correlationId1", null);

            var result = service.GetCardCoordinateResults(request);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Result.Status);
            mockCardCoordinateService.Verify(x => x.GetCardCoordinateData
                (new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000", "correlationId"), Times.Never);
        }

        [TestMethod]
        public void GetCardCoordinateResultsNullAssetIdOrCardDateTest()
        {
            var mockService = new Mock<IRodLiftAnalysis>();
            var mockCommonService = new Mock<ICommonService>();
            var mockLocalePhrase = new Mock<ILocalePhrases>();
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var correlationId = Guid.NewGuid().ToString();

            var mockCardCoordinateService = new Mock<ICardCoordinate>();
            mockCardCoordinateService.Setup(x =>
                x.GetCardCoordinateData(Guid.Empty, "2023 -05-11 22:29:55.000", "correlationId"));

            var service = new RodLiftAnalysisProcessingService(mockService.Object, mockThetaLoggerFactory.Object,
               mockCommonService.Object,
               mockLocalePhrase.Object,
               mockCardCoordinateService.Object);

            var request = new WithCorrelationId<CardCoordinateInput>(correlationId, new CardCoordinateInput
            {
                AssetId = Guid.Empty,
                CardDate = ""
            });

            var result = service.GetCardCoordinateResults(request);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Result.Status);
            mockCardCoordinateService.Verify(x => x.GetCardCoordinateData(Guid.Empty, "2023-05-11 22:29:55.000", "correlationId"), Times.Never);
        }

        [TestMethod]
        public void GetCardCoordinatesecondaryFillageSetpointPercentTest()
        {
            var mockService = new Mock<IRodLiftAnalysis>();
            var mockCommonService = new Mock<ICommonService>();
            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var correlationId = Guid.NewGuid().ToString();

            var mockCardCoordinateService = new Mock<ICardCoordinate>();
            mockCardCoordinateService.Setup(x =>
                    x.GetCardCoordinateData(new Guid("f3eb743c-f890-44f3-80e5-6a46df7ce2b7"), "2022-11-09 23:48:13.000", "correlationId"))
                .Returns(new CardCoordinateModel
                {
                    StrokesPerMinute = 8,
                    StrokeLength = 35,
                    SecondaryPumpFillage = 40,
                    AreaLimit = 0,
                    Area = 3800,
                    DownHoleCardBinary = null,
                    Fillage = 96,
                    FillBasePercent = 45,
                    HiLoadLimit = 50000,
                    LoadLimit = 456,
                    LoadLimit2 = 0,
                    LowLoadLimit = 0,
                    MalfunctionLoadLimit = 0,
                    MalfunctionPositionLimit = 28,
                    PermissibleLoadDownBinary = null,
                    PermissibleLoadUpBinary = null,
                    POCDownholeCard = null,
                    PocDownHoleCardBinary = Encoding.ASCII.GetBytes("��3FߣKF"),
                    PositionLimit = 55,
                    PositionLimit2 = 0,
                    PredictedCard = null,
                    PredictedCardBinary = null,
                    SurfaceCardBinary = null,
                    PocType = 8
                });

            var service = new RodLiftAnalysisProcessingService(mockService.Object, mockThetaLoggerFactory.Object,
                mockCommonService.Object,
                mockLocalePhrase.Object,
                mockCardCoordinateService.Object);

            var request = new WithCorrelationId<CardCoordinateInput>(correlationId, new CardCoordinateInput
            {
                AssetId = new Guid("f3eb743c-f890-44f3-80e5-6a46df7ce2b7"),
                CardDate = "2022-11-09 23:48:13.000"
            });
            var result = service.GetCardCoordinateResults(request);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CardCoordinateDataOutput));
        }

        #endregion

        #region Private Methods

        private IList<WellDetailsModel> GetWellDetails()
        {
            var wellDetails = new List<WellDetailsModel>()
            {
                new WellDetailsModel()
                {
                    NodeId = "AssetId1",
                    PlungerDiameter = 2,
                    PumpDepth = 999,
                    Cycles = 0,
                    IdleTime = 5,
                    PumpingUnitId = "CP1",
                    POCGrossRate = 23926
                },
            };

            return wellDetails;
        }

        private IList<CardDataModel> GetCardData()
        {
            var eventData = new List<CardDataModel>()
            {
                new CardDataModel()
                {
                    NodeId = "AssetId1",
                    CardDate = new DateTime(2023, 5, 11, 22, 29, 55),
                    CardType = "P",
                    Runtime = 24,
                    StrokesPerMinute = 8,
                    StrokeLength = 35,
                    SecondaryPumpFillage = 40,
                    AreaLimit = 70,
                    LoadSpanLimit = 0,
                    CauseId = 99
                }
            };

            return eventData;
        }

        private IList<WellTestModel> GetWellTestData()
        {
            var wellTests = new List<WellTestModel>()
            {
                new WellTestModel()
                {
                    NodeId = "AssetId1",
                    TestDate = new DateTime(2023, 5, 11, 22, 29, 55),
                    OilRate = 37.7f,
                    WaterRate = 37.7f,
                    GasRate = 6f,
                    Approved = true,
                }
            };

            return wellTests;
        }

        public IList<CurrentRawScanDataModel> GetCurrentRawScanData()
        {
            return new List<CurrentRawScanDataModel>
            {
                new CurrentRawScanDataModel
                {
                    NodeId = "AssetId1",
                    Address = 39748,
                    Value = 120,
                    DateTimeUpdated = DateTime.Parse("2023-05-11 02:02:00.000", new CultureInfo("us-en")),
                },
                new CurrentRawScanDataModel
                {
                    NodeId = "AssetId1",
                    Address = 39749,
                    Value = 120,
                    DateTimeUpdated = DateTime.Parse("2023-05-11 02:02:00.000", new CultureInfo("us-en")),
                },
                new CurrentRawScanDataModel
                {
                    NodeId = "AssetId1",
                    Address = 39750,
                    Value = 89,
                    DateTimeUpdated = DateTime.Parse("2023-05-11 02:02:00.000", new CultureInfo("us-en")),
                },
                new CurrentRawScanDataModel
                {
                    NodeId = "AssetId1",
                    Address = 39751,
                    Value = 110,
                    DateTimeUpdated = DateTime.Parse("2023-05-11 02:02:00.000", new CultureInfo("us-en")),
                }
            };
        }

        private IList<XDiagResultsModel> GetXdiagResultsData()
        {
            var xDiagResults = new List<XDiagResultsModel>()
            {
                new XDiagResultsModel()
                {
                    NodeId = "AssetId1",
                    Date = new DateTime(2023, 05, 10, 12, 12, 12),
                    PumpIntakePressure = 60,
                    GrossPumpStroke = 2,
                    FluidLoadonPump = 2,
                    BouyRodWeight = 2,
                    DryRodWeight = 2,
                    PumpFriction = 2,
                    PofluidLoad = 2,
                    AdditionalUplift = 2,
                    AdditionalUpliftGross = 2,
                    PumpEfficiency = 2,
                    DownholeAnalysis = "",
                    InputAnalysis = "",
                    RodAnalysis = "",
                    SurfaceAnalysis = ""
                },
                new XDiagResultsModel()
                {
                    NodeId = "AssetId1",
                    Date = new DateTime(2023, 5, 11, 22, 29, 55),
                    PumpIntakePressure = 70,
                    GrossPumpStroke = 2,
                    FluidLoadonPump = 2,
                    BouyRodWeight = 2,
                    DryRodWeight = 2,
                    PumpFriction = 2,
                    PofluidLoad = 2,
                    AdditionalUplift = 2,
                    AdditionalUpliftGross = 2,
                    PumpEfficiency = 2,
                    DownholeAnalysis = "",
                    InputAnalysis = "",
                    RodAnalysis = "",
                    SurfaceAnalysis = ""
                },
                new XDiagResultsModel()
                {
                    NodeId = "AssetId1",
                    Date = new DateTime(2023, 05, 12, 12, 14, 14, 14),
                    PumpIntakePressure = 70,
                    GrossPumpStroke = 2,
                    FluidLoadonPump = 2,
                    BouyRodWeight = 2,
                    DryRodWeight = 2,
                    PumpFriction = 2,
                    PofluidLoad = 2,
                    AdditionalUplift = 2,
                    AdditionalUpliftGross = 2,
                    PumpEfficiency = 2,
                    DownholeAnalysis = "",
                    InputAnalysis = "",
                    RodAnalysis = "",
                    SurfaceAnalysis = ""
                }
            };

            return xDiagResults;
        }

        #endregion

    }
}
