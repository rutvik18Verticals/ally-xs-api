using MathNet.Numerics;
using MathNet.Numerics.Statistics;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Influx.Services;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Core.Tests.Services
{
    [TestClass]
    public class DataHistoryProcessingServiceTests
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
        public void ConstructorNullDataHistoryServiceTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockLocalePhrase = new Mock<ILocalePhrases>();
            var mockHostAlarm = new Mock<IHostAlarm>();
            var mockInfluxDataStore = new Mock<IGetDataHistoryItemsService>();
            var mockPortConfiguration = new Mock<IPortConfigurationStore>();
            var mockParameterStore = new Mock<IParameterMongoStore>();
            var mockConfiguration = new Mock<IConfiguration>();

            _ = new DataHistoryProcessingService(null, mockThetaLoggerFactory.Object, mockLocalePhrase.Object,
                mockHostAlarm.Object, mockInfluxDataStore.Object, mockPortConfiguration.Object,
                mockParameterStore.Object, _commonService.Object, mockConfiguration.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLoggerTest()
        {
            var mockService = new Mock<IDataHistorySQLStore>();

            var mockLocalePhrase = new Mock<ILocalePhrases>();
            var mockHostAlarm = new Mock<IHostAlarm>();
            var mockInfluxDataStore = new Mock<IGetDataHistoryItemsService>();
            var mockPortConfiguration = new Mock<IPortConfigurationStore>();
            var mockParameterStore = new Mock<IParameterMongoStore>();
            var mockConfiguration = new Mock<IConfiguration>();

            _ = new DataHistoryProcessingService(mockService.Object, null, mockLocalePhrase.Object,
                mockHostAlarm.Object, mockInfluxDataStore.Object, mockPortConfiguration.Object,
                mockParameterStore.Object, _commonService.Object, mockConfiguration.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLocalPhraseTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockService = new Mock<IDataHistorySQLStore>();
            var mockHostAlarm = new Mock<IHostAlarm>();
            var mockInfluxDataStore = new Mock<IGetDataHistoryItemsService>();
            var mockPortConfiguration = new Mock<IPortConfigurationStore>();
            var mockParameterStore = new Mock<IParameterMongoStore>();
            var mockConfiguration = new Mock<IConfiguration>();

            _ = new DataHistoryProcessingService(mockService.Object, mockThetaLoggerFactory.Object, null,
                mockHostAlarm.Object, mockInfluxDataStore.Object, mockPortConfiguration.Object,
                mockParameterStore.Object, _commonService.Object, mockConfiguration.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullHostAlarmTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockLocalePhrase = new Mock<ILocalePhrases>();
            var mockService = new Mock<IDataHistorySQLStore>();
            var mockInfluxDataStore = new Mock<IGetDataHistoryItemsService>();
            var mockPortConfiguration = new Mock<IPortConfigurationStore>();

            var mockParameterStore = new Mock<IParameterMongoStore>();
            var mockConfiguration = new Mock<IConfiguration>();

            _ = new DataHistoryProcessingService(mockService.Object, mockThetaLoggerFactory.Object, mockLocalePhrase.Object,
                null, mockInfluxDataStore.Object, mockPortConfiguration.Object,
                mockParameterStore.Object, _commonService.Object, mockConfiguration.Object);
        }

        [TestMethod]
        public void GetDataHistoryTrendsTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockInfluxDataStore = new Mock<IGetDataHistoryItemsService>();
            var mockParameterStore = new Mock<IParameterMongoStore>();

            var phraseid = new int[]
            {
                151,
                264,
                265,
                298,
                440,
                441,
                513,
                532,
                544,
                545,
                546,
                599,
                655,
                689,
                754,
                1074,
                1077,
                1101,
                1109,
                1111,
                1113,
                1250,
                1251,
                1252,
                1352,
                2994,
                3969,
                4160,
                4188,
                6340,
                6383,
                6384,
                6385,
                6475,
                6476,
                6683,
                6684,
                7022,
                20140,
                20141,
                20142,
                20143,
                20144,
                20145,
                20147,
                20148,
                20195,
                20196,
                20197,
                20198,
            };
            var mockLocalePhrase = new Mock<ILocalePhrases>();
            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseid)).Returns(GetLocalPhrases());

            var mockHostAlarm = new Mock<IHostAlarm>();

            var mockService = new Mock<IDataHistorySQLStore>();
            var mockPortConfiguration = new Mock<IPortConfigurationStore>();
            var mockConfiguration = new Mock<IConfiguration>();

            var nodeData = new List<NodeMasterModel>()
            {
                new()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();

            var dataHistoryService = new DataHistoryProcessingService(mockService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockHostAlarm.Object,
                mockInfluxDataStore.Object, mockPortConfiguration.Object,
                mockParameterStore.Object, _commonService.Object, mockConfiguration.Object);

            var responseData = new DataHistoryItemModel
            {
                NodeMasterData = nodeData.FirstOrDefault(),
                FailureComponentTrendData = GetFailureComponentTrendItems(),
                FailureSubComponentTrendData = GetFailureSubComponentTrendItems(),
                EventsTrendData = GetEventsTrendItems(),
                MeterTrendData = GetMeterTrendItems(),
                PCSFDatalogConfiguration = GetPCSFDatalogConfiguration()
            };
            var correlationId = Guid.NewGuid().ToString();

            mockService.Setup(x => x.GetDataHistoryTrends("DFC1D0AD-A824-4965-B78D-AB7755E32DD3", correlationId))
                .Returns(responseData);

            var result = dataHistoryService.GetDataHistoryTrendData("DFC1D0AD-A824-4965-B78D-AB7755E32DD3", correlationId);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DataHistoryTrendsListOutput));

            Assert.IsNotNull(result.ESPAnalysisTrendDataItems);
            Assert.AreEqual(54, result.ESPAnalysisTrendDataItems.Length);

            Assert.IsNotNull(result.GLAnalysisTrendDataItems);
            Assert.AreEqual(18, result.GLAnalysisTrendDataItems.Length);

            Assert.IsNotNull(result.OperationalScoreTrendDataItems);
            Assert.AreEqual(1, result.OperationalScoreTrendDataItems.Length);

            Assert.IsNotNull(result.ProductionStatisticsTrendDataItems);
            Assert.AreEqual(3, result.ProductionStatisticsTrendDataItems.Length);

            Assert.IsNotNull(result.FailureComponentTrendData);
            Assert.AreEqual(5, result.FailureComponentTrendData.Count);

            Assert.IsNotNull(result.FailureSubComponentTrendData);
            Assert.AreEqual(5, result.FailureSubComponentTrendData.Count);

            Assert.IsNotNull(result.EventsTrendDataItems);
            Assert.AreEqual(5, result.EventsTrendDataItems.Count);

        }

        [TestMethod]
        public void GetDataHistoryTrendsListTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var mockInfluxDataStore = new Mock<IGetDataHistoryItemsService>();
            var mockPortConfiguration = new Mock<IPortConfigurationStore>();

            var mockParameterStore = new Mock<IParameterMongoStore>();

            var phraseid = new int[]
            {
                151,
                264,
                265,
                298,
                440,
                441,
                513,
                532,
                544,
                545,
                546,
                599,
                655,
                689,
                754,
                1074,
                1077,
                1101,
                1109,
                1111,
                1113,
                1250,
                1251,
                1252,
                1352,
                2994,
                3969,
                4160,
                4188,
                6340,
                6383,
                6384,
                6385,
                6475,
                6476,
                6683,
                6684,
                7022,
                20140,
                20141,
                20142,
                20143,
                20144,
                20145,
                20147,
                20148,
                20195,
                20196,
                20197,
                20198,
            };
            var mockLocalePhrase = new Mock<ILocalePhrases>();
            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseid)).Returns(GetLocalPhrases());

            var mockHostAlarm = new Mock<IHostAlarm>();

            var mockService = new Mock<IDataHistorySQLStore>();
            var mockConfiguration = new Mock<IConfiguration>();

            var nodeData = new List<NodeMasterModel>()
            {
                new()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                    ApplicationId = 3
                }
            }.AsQueryable();

            var dataHistoryService = new DataHistoryProcessingService(mockService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockHostAlarm.Object,
                mockInfluxDataStore.Object, mockPortConfiguration.Object,
                mockParameterStore.Object, _commonService.Object, mockConfiguration.Object);

            var responseData = new DataHistoryModel
            {
                NodeMasterData = nodeData.FirstOrDefault(),
                FailureComponentTrendData = GetFailureComponentTrendItems(),
                FailureSubComponentTrendData = GetFailureSubComponentTrendItems(),
                EventsTrendData = GetEventsTrendItems(),
                MeterTrendData = GetMeterTrendItems(),
                PCSFDatalogConfiguration = GetPCSFDatalogConfiguration(),
            };
            var correlationId = Guid.NewGuid().ToString();

            mockService.Setup(x => x.GetDataHistoryList(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), correlationId))
                .Returns(responseData);

            var request = new WithCorrelationId<DataHistoryTrendInput>(correlationId, new DataHistoryTrendInput
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                GroupName = "Test"
            });

            var result = dataHistoryService.GetDataHistoryListData(request);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(DataHistoryListOutput));

            Assert.IsNotNull(result.Values);
            Assert.IsInstanceOfType(result.Values, typeof(IList<DataHistoryListItem>));
            var failureComponents = result.Values.FirstOrDefault(a => a.Name == "Failure: Component");
            var failureSubComponents = result.Values.FirstOrDefault(a => a.Name == "Failure: Subcomponent");

            Assert.IsTrue(result.Values.Count > 0);
            Assert.IsTrue(result.Values[0].Items[0].Name == "Operational Score");
            Assert.IsTrue(failureComponents.Items.Count > 0);
            Assert.IsTrue(failureSubComponents.Items.Count > 0);
        }

        [TestMethod]
        public void GetDataHistoryInvalidInputTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var phraseIds = SetPhraseIds();
            var mockLocalePhrase = new Mock<ILocalePhrases>();
            var mockPortConfiguration = new Mock<IPortConfigurationStore>();

            var mockParameterStore = new Mock<IParameterMongoStore>();

            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseIds)).Returns(GetLocalPhrases());

            var mockHostAlarm = new Mock<IHostAlarm>();
            var correlationId = Guid.NewGuid().ToString();

            var mockService = new Mock<IDataHistorySQLStore>();
            var mockInfluxDataStore = new Mock<IGetDataHistoryItemsService>();
            var mockConfiguration = new Mock<IConfiguration>();

            var dataHistoryService = new DataHistoryProcessingService(mockService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockHostAlarm.Object,
                mockInfluxDataStore.Object, mockPortConfiguration.Object,
                mockParameterStore.Object, _commonService.Object, mockConfiguration.Object);

            var response = dataHistoryService.GetTrendDataAsync(null);

            Assert.AreEqual(false, response.Result.Result.Status);
            logger.Verify(x => x.Write(Level.Error,
                    It.Is<string>(x => x.Contains("input is null, cannot get data history trend data items."))),
                Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetDataHistoryInvalidInputNullDataTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var phraseIds = SetPhraseIds();
            var mockLocalePhrase = new Mock<ILocalePhrases>();
            var mockPortConfiguration = new Mock<IPortConfigurationStore>();
            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseIds)).Returns(GetLocalPhrases());
            var mockInfluxDataStore = new Mock<IGetDataHistoryItemsService>();

            var mockParameterStore = new Mock<IParameterMongoStore>();

            var mockHostAlarm = new Mock<IHostAlarm>();

            var mockService = new Mock<IDataHistorySQLStore>();
            var mockConfiguration = new Mock<IConfiguration>();
            var correlationId = Guid.NewGuid().ToString();

            var dataHistoryService = new DataHistoryProcessingService(mockService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockHostAlarm.Object,
                mockInfluxDataStore.Object, mockPortConfiguration.Object,
                mockParameterStore.Object, _commonService.Object, mockConfiguration.Object);
            var request = new WithCorrelationId<TrendIDataInput>(correlationId, null);
            var response = dataHistoryService.GetTrendDataAsync(request);

            Assert.AreEqual(false, response.Result.Result.Status);
            logger.Verify(x => x.WriteCId(Level.Error,
                    It.Is<string>(x => x.Contains("input is null, cannot get data history trend data items.")),
                    correlationId), Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetDataHistoryNullResponseTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var phraseIds = SetPhraseIds();
            var mockLocalePhrase = new Mock<ILocalePhrases>();
            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseIds)).Returns(GetLocalPhrases());

            var mockInfluxDataStore = new Mock<IGetDataHistoryItemsService>();

            var mockHostAlarm = new Mock<IHostAlarm>();
            var correlationId = Guid.NewGuid().ToString();

            var mockService = new Mock<IDataHistorySQLStore>();
            var mockPortConfiguration = new Mock<IPortConfigurationStore>();

            var mockParameterStore = new Mock<IParameterMongoStore>();
            var mockConfiguration = new Mock<IConfiguration>();

            mockService.Setup(a => a.GetGroupTrendData(It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<int>(), It.IsAny<string>(), correlationId)).Returns(GetGroupTrendDataHistoryModel());

            mockService.Setup(x => x.GetGroupTrendData(correlationId)).Returns(GetGroupTrendDataModel());

            mockPortConfiguration.Setup(a => a.GetNode(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(GetNodeMasterData());
            mockPortConfiguration.Setup(a => a.IsLegacyWellAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            var dataHistoryService = new DataHistoryProcessingService(mockService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockHostAlarm.Object,
                mockInfluxDataStore.Object, mockPortConfiguration.Object,
                mockParameterStore.Object, _commonService.Object, mockConfiguration.Object);

            var request = new WithCorrelationId<TrendIDataInput>(correlationId, new TrendIDataInput
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                Chart1Type = "21",
                Chart1ItemId = "Fluid Load",
                Chart2Type = "",
                Chart2ItemId = "",
                Chart3Type = "",
                Chart3ItemId = "",
                Chart4Type = "",
                Chart4ItemId = "",
                StartDate = "2010-01-01",
                EndDate = "2024-01-01"
            });

            var nodeData = new List<NodeMasterModel>()
            {
                new()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();

            mockService.Setup(x => x.GetDataHistoryTrendDataItems(request.Value.AssetId.ToString(), correlationId));

            var response = dataHistoryService.GetTrendDataAsync(request);

            Assert.AreEqual(false, response.Result.Result.Status);
            logger.Verify(x => x.WriteCId(Level.Error,
                    It.Is<string>(x => x.Contains("Invalid data. Cannot get data history trend data items.")),
                    correlationId), Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetDataHistoryNullNodeMasterDataTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var phraseIds = SetPhraseIds();
            var mockLocalePhrase = new Mock<ILocalePhrases>();
            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseIds)).Returns(GetLocalPhrases());
            var mockInfluxDataStore = new Mock<IGetDataHistoryItemsService>();

            var mockParameterStore = new Mock<IParameterMongoStore>();
            var correlationId = Guid.NewGuid().ToString();

            var mockHostAlarm = new Mock<IHostAlarm>();

            var mockService = new Mock<IDataHistorySQLStore>();
            var mockConfiguration = new Mock<IConfiguration>();

            mockService.Setup(a => a.GetGroupTrendData(It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<int>(), It.IsAny<string>(), correlationId)).Returns(GetGroupTrendDataHistoryModel());

            mockService.Setup(x => x.GetGroupTrendData(correlationId)).Returns(GetGroupTrendDataModel());
            var mockPortConfiguration = new Mock<IPortConfigurationStore>();
            mockPortConfiguration.Setup(a => a.GetNode(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(GetNodeMasterData());
            mockPortConfiguration.Setup(a => a.IsLegacyWellAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            var dataHistoryService = new DataHistoryProcessingService(mockService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockHostAlarm.Object,
                mockInfluxDataStore.Object, mockPortConfiguration.Object,
                mockParameterStore.Object, _commonService.Object, mockConfiguration.Object);

            var request = new WithCorrelationId<TrendIDataInput>(correlationId, new TrendIDataInput
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                Chart1Type = "21",
                Chart1ItemId = "Fluid Load",
                Chart2Type = "",
                Chart2ItemId = "",
                Chart3Type = "",
                Chart3ItemId = "",
                Chart4Type = "",
                Chart4ItemId = "",
                StartDate = "2010-01-01",
                EndDate = "2024-01-01"
            });

            var responseData = new DataHistoryItemModel
            {
                FailureComponentTrendData = GetFailureComponentTrendItems(),
                FailureSubComponentTrendData = GetFailureSubComponentTrendItems(),
                EventsTrendData = GetEventsTrendItems(),
                MeterTrendData = GetMeterTrendItems(),
                PCSFDatalogConfiguration = GetPCSFDatalogConfiguration(),
            };

            mockService.Setup(x => x.GetDataHistoryTrendDataItems(request.Value.AssetId.ToString(), correlationId))
                .Returns(responseData);

            var response = dataHistoryService.GetTrendDataAsync(request);

            Assert.AreEqual(false, response.Result.Result.Status);
            logger.Verify(x => x.WriteCId(Level.Error,
                    It.Is<string>(x => x.Contains("Invalid data. Cannot get data history trend data items.")),
                   correlationId), Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetDataHistoryCommonTrendMeasurementTrendDataItemsTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var phraseIds = SetPhraseIds();
            var mockLocalePhrase = new Mock<ILocalePhrases>();
            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseIds)).Returns(GetLocalPhrases());

            var mockHostAlarm = new Mock<IHostAlarm>();
            var correlationId = Guid.NewGuid().ToString();

            var mockInfluxDataStore = new Mock<IGetDataHistoryItemsService>();
            var mockService = new Mock<IDataHistorySQLStore>();
            var mockPortConfiguration = new Mock<IPortConfigurationStore>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(x => x.Value).Returns("true");
            mockConfiguration.Setup(x => x.GetSection("TimeZoneBehavior:ApplicationTimeZone")).Returns(mockSection.Object);

            mockPortConfiguration.Setup(a => a.GetNode(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(GetNodeMasterData());
            mockPortConfiguration.Setup(a => a.IsLegacyWellAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            var mockParameterStore = new Mock<IParameterMongoStore>();

            var nodeData = new List<NodeMasterModel>()
            {
                new()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();

            var request = new WithCorrelationId<TrendIDataInput>(correlationId, new TrendIDataInput
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                Chart1Type = "14",
                Chart1ItemId = "Parameter",
                Chart2Type = "",
                Chart2ItemId = "",
                Chart3Type = "",
                Chart3ItemId = "",
                Chart4Type = "",
                Chart4ItemId = "",
                StartDate = "2010-01-01",
                EndDate = "2024-01-01"
            });

            mockService.Setup(x => x.GetMeasurementTrendItems("TestNode", correlationId))
                .Returns(GetMeasurementTrendItemModel());

            mockService.Setup(x => x.GetMeasurementTrendData("TestNode", 1, It.IsAny<DateTime>(), It.IsAny<DateTime>(), correlationId))
                .Returns(GetMeasurementTrendDataModel());

            var dataHistoryService = new DataHistoryProcessingService(mockService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockHostAlarm.Object,
                mockInfluxDataStore.Object, mockPortConfiguration.Object,
                mockParameterStore.Object, _commonService.Object, mockConfiguration.Object);

            var responseData = new DataHistoryItemModel
            {
                NodeMasterData = nodeData.FirstOrDefault(),
                FailureComponentTrendData = GetFailureComponentTrendItems(),
                FailureSubComponentTrendData = GetFailureSubComponentTrendItems(),
                EventsTrendData = GetEventsTrendItems(),
                MeterTrendData = GetMeterTrendItems(),
                PCSFDatalogConfiguration = GetPCSFDatalogConfiguration(),
            };

            mockService.Setup(x => x.GetDataHistoryTrendDataItems(request.Value.AssetId.ToString(), correlationId))
                .Returns(responseData);

            var response = dataHistoryService.GetTrendDataAsync(request);

            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response.Result, typeof(DataHistoryTrendOutput));

            Assert.IsNotNull(response.Result.Values);
            Assert.IsInstanceOfType(response.Result.Values, typeof(List<GraphViewTrendsData>));
        }

        [TestMethod]
        public void GetDataHistoryTrendDataItemsWithSimilarDescriptionsTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var phraseIds = SetPhraseIds();
            var mockLocalePhrase = new Mock<ILocalePhrases>();
            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseIds)).Returns(GetLocalPhrases());

            var mockHostAlarm = new Mock<IHostAlarm>();
            var correlationId = Guid.NewGuid().ToString();

            var mockInfluxDataStore = new Mock<IGetDataHistoryItemsService>();
            var mockService = new Mock<IDataHistorySQLStore>();
            var mockPortConfiguration = new Mock<IPortConfigurationStore>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(x => x.Value).Returns("true");
            mockConfiguration.Setup(x => x.GetSection("TimeZoneBehavior:ApplicationTimeZone")).Returns(mockSection.Object);

            mockPortConfiguration.Setup(a => a.GetNode(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(GetNodeMasterData());
            mockPortConfiguration.Setup(a => a.IsLegacyWellAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            var mockParameterStore = new Mock<IParameterMongoStore>();

            var nodeData = new List<NodeMasterModel>()
            {
                new()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();

            var request = new WithCorrelationId<TrendIDataInput>(correlationId, new TrendIDataInput
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                Chart1Type = "14",
                Chart1ItemId = "Parameter",
                Chart2Type = "",
                Chart2ItemId = "",
                Chart3Type = "",
                Chart3ItemId = "",
                Chart4Type = "",
                Chart4ItemId = "",
                StartDate = "2010-01-01",
                EndDate = "2024-01-01"
            });

            mockService.Setup(x => x.GetMeasurementTrendItems("TestNode", correlationId))
                .Returns(GetMeasurementTrendItemModel());

            mockService.Setup(x => x.GetMeasurementTrendData("TestNode", 1, It.IsAny<DateTime>(), It.IsAny<DateTime>(), correlationId))
                .Returns(GetMeasurementTrendDataModel());

            var dataHistoryService = new DataHistoryProcessingService(mockService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockHostAlarm.Object,
                mockInfluxDataStore.Object, mockPortConfiguration.Object,
                mockParameterStore.Object, _commonService.Object, mockConfiguration.Object);

            var responseData = new DataHistoryItemModel
            {
                NodeMasterData = nodeData.FirstOrDefault(),
                FailureComponentTrendData = GetFailureComponentTrendItems(),
                FailureSubComponentTrendData = GetFailureSubComponentTrendItems(),
                EventsTrendData = GetEventsTrendItems(),
                MeterTrendData = GetMeterTrendItems(),
                PCSFDatalogConfiguration = GetPCSFDatalogConfiguration(),
            };

            mockService.Setup(x => x.GetDataHistoryTrendDataItems(request.Value.AssetId.ToString(), correlationId))
                .Returns(responseData);

            var response = dataHistoryService.GetTrendDataAsync(request);

            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response.Result, typeof(DataHistoryTrendOutput));

            Assert.IsNotNull(response.Result.Values);
            Assert.IsInstanceOfType(response.Result.Values, typeof(List<GraphViewTrendsData>));

            Assert.AreEqual(1, response.Result.Values.Count);
            Assert.AreEqual("Parameter", response.Result.Values[0].AxisLabel);
            Assert.AreEqual(14, response.Result.Values[0].ItemId);
            Assert.AreEqual(200, response.Result.Values[0].AxisValues[0].Y);
        }

        [TestMethod]
        public void GetDataHistoryCommonTrendControllerTrendDataItemsTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var phraseIds = SetPhraseIds();
            var mockLocalePhrase = new Mock<ILocalePhrases>();
            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseIds)).Returns(GetLocalPhrases());
            var mockPortConfiguration = new Mock<IPortConfigurationStore>();
            mockPortConfiguration.Setup(a => a.GetNode(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(GetNodeMasterData());
            mockPortConfiguration.Setup(a => a.IsLegacyWellAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));
            var correlationId = Guid.NewGuid().ToString();

            var mockHostAlarm = new Mock<IHostAlarm>();

            var mockInfluxDataStore = new Mock<IGetDataHistoryItemsService>();

            var mockService = new Mock<IDataHistorySQLStore>();

            var mockParameterStore = new Mock<IParameterMongoStore>();

            var mockConfiguration = new Mock<IConfiguration>();
            var mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(x => x.Value).Returns("true");
            mockConfiguration.Setup(x => x.GetSection("TimeZoneBehavior:ApplicationTimeZone")).Returns(mockSection.Object);

            var nodeData = new List<NodeMasterModel>()
            {
                new()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();

            var request = new WithCorrelationId<TrendIDataInput>(correlationId, new TrendIDataInput
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                Chart1Type = "14",
                Chart1ItemId = "Parameter",
                Chart2Type = "",
                Chart2ItemId = "",
                Chart3Type = "",
                Chart3ItemId = "",
                Chart4Type = "",
                Chart4ItemId = "",
                StartDate = "2010-01-01",
                EndDate = "2024-01-01"
            });

            mockService.Setup(x => x.GetControllerTrendItems("TestNode", 8, correlationId))
                .Returns(GetControllerTrendItemModel());
            mockService.Setup(x => x.GetControllerTrendData("TestNode", 2002,
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), correlationId))
                .Returns(GetControllerTrendDataModel());

            var dataHistoryService = new DataHistoryProcessingService(mockService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockHostAlarm.Object,
                mockInfluxDataStore.Object, mockPortConfiguration.Object,
                mockParameterStore.Object, _commonService.Object, mockConfiguration.Object);

            var responseData = new DataHistoryItemModel
            {
                NodeMasterData = nodeData.FirstOrDefault(),
                FailureComponentTrendData = GetFailureComponentTrendItems(),
                FailureSubComponentTrendData = GetFailureSubComponentTrendItems(),
                EventsTrendData = GetEventsTrendItems(),
                MeterTrendData = GetMeterTrendItems(),
                PCSFDatalogConfiguration = GetPCSFDatalogConfiguration(),
            };

            mockService.Setup(x => x.GetDataHistoryTrendDataItems(request.Value.AssetId.ToString(), correlationId))
                .Returns(responseData);

            var response = dataHistoryService.GetTrendDataAsync(request);

            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response.Result, typeof(DataHistoryTrendOutput));

            Assert.IsNotNull(response.Result.Values);
            Assert.IsInstanceOfType(response.Result.Values, typeof(List<GraphViewTrendsData>));

            Assert.IsTrue(response.Result.Values.Count > 0);
        }

        [TestMethod]
        public void GetDataHistoryCommonTrendProductionStatTrendDataItemsTest()
        {
            var logger = new Mock<IThetaLogger>();
            var correlationId = Guid.NewGuid().ToString();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var phraseIds = SetPhraseIds();
            var mockLocalePhrase = new Mock<ILocalePhrases>();
            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseIds)).Returns(GetLocalPhrases());
            var mockPortConfiguration = new Mock<IPortConfigurationStore>();
            mockPortConfiguration.Setup(a => a.GetNode(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(GetNodeMasterData());
            mockPortConfiguration.Setup(a => a.IsLegacyWellAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            var mockHostAlarm = new Mock<IHostAlarm>();

            var mockInfluxDataStore = new Mock<IGetDataHistoryItemsService>();

            var mockParameterStore = new Mock<IParameterMongoStore>();

            var mockService = new Mock<IDataHistorySQLStore>();

            var mockConfiguration = new Mock<IConfiguration>();
            var mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(x => x.Value).Returns("true");
            mockConfiguration.Setup(x => x.GetSection("TimeZoneBehavior:ApplicationTimeZone")).Returns(mockSection.Object);

            var nodeData = new List<NodeMasterModel>()
            {
                new()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();

            var request = new WithCorrelationId<TrendIDataInput>(correlationId, new TrendIDataInput
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                Chart1Type = "14",
                Chart1ItemId = "Production Latest BOE",
                Chart2Type = "",
                Chart2ItemId = "",
                Chart3Type = "",
                Chart3ItemId = "",
                Chart4Type = "",
                Chart4ItemId = "",
                StartDate = "2010-01-01",
                EndDate = "2024-01-01"
            });

            mockService.Setup(x => x.GetProductionStatisticsTrendData("TestNode",
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), "LatestProductionBOE", correlationId))
                .Returns(GetProductionStatisticsTrendDataModel());

            var dataHistoryService = new DataHistoryProcessingService(mockService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockHostAlarm.Object,
                mockInfluxDataStore.Object, mockPortConfiguration.Object,
                mockParameterStore.Object, _commonService.Object, mockConfiguration.Object);

            var responseData = new DataHistoryItemModel
            {
                NodeMasterData = nodeData.FirstOrDefault(),
                FailureComponentTrendData = GetFailureComponentTrendItems(),
                FailureSubComponentTrendData = GetFailureSubComponentTrendItems(),
                EventsTrendData = GetEventsTrendItems(),
                MeterTrendData = GetMeterTrendItems(),
                PCSFDatalogConfiguration = GetPCSFDatalogConfiguration(),
            };

            mockService.Setup(x => x.GetDataHistoryTrendDataItems(request.Value.AssetId.ToString(), correlationId))
                .Returns(responseData);

            var response = dataHistoryService.GetTrendDataAsync(request);

            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response.Result, typeof(DataHistoryTrendOutput));

            Assert.IsNotNull(response.Result.Values);
            Assert.IsInstanceOfType(response.Result.Values, typeof(List<GraphViewTrendsData>));

            Assert.IsTrue(response.Result.Values.Count > 0);
            Assert.IsTrue(response.Result.Values.Count == 1);
        }

        [TestMethod]
        public void GetDataHistoryGroupTrendDataItemsTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var phraseIds = SetPhraseIds();
            var mockLocalePhrase = new Mock<ILocalePhrases>();
            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseIds)).Returns(GetLocalPhrases());

            var mockHostAlarm = new Mock<IHostAlarm>();

            var mockInfluxDataStore = new Mock<IGetDataHistoryItemsService>();
            var mockPortConfiguration = new Mock<IPortConfigurationStore>();
            mockPortConfiguration.Setup(a => a.GetNode(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(GetNodeMasterData());
            mockPortConfiguration.Setup(a => a.IsLegacyWellAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));
            var mockService = new Mock<IDataHistorySQLStore>();

            var mockParameterStore = new Mock<IParameterMongoStore>();

            var mockConfiguration = new Mock<IConfiguration>();
            var mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(x => x.Value).Returns("true");
            mockConfiguration.Setup(x => x.GetSection("TimeZoneBehavior:ApplicationTimeZone")).Returns(mockSection.Object);

            var nodeData = new List<NodeMasterModel>()
            {
                new()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();
            var correlationId = Guid.NewGuid().ToString();

            mockService.Setup(a => a.GetGroupTrendData(It.IsAny<DateTime>(), It.IsAny<DateTime>(),
                It.IsAny<int>(), It.IsAny<string>(), correlationId)).Returns(GetGroupTrendDataHistoryModel());

            mockService.Setup(x => x.GetGroupTrendData(correlationId)).Returns(GetGroupTrendDataModel());

            var dataHistoryService = new DataHistoryProcessingService(mockService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockHostAlarm.Object,
                mockInfluxDataStore.Object, mockPortConfiguration.Object,
                mockParameterStore.Object, _commonService.Object, mockConfiguration.Object);

            var responseData = new DataHistoryItemModel
            {
                NodeMasterData = nodeData.FirstOrDefault(),
                FailureComponentTrendData = GetFailureComponentTrendItems(),
                FailureSubComponentTrendData = GetFailureSubComponentTrendItems(),
                EventsTrendData = GetEventsTrendItems(),
                MeterTrendData = GetMeterTrendItems(),
                PCSFDatalogConfiguration = GetPCSFDatalogConfiguration(),
            };

            var request = new WithCorrelationId<TrendIDataInput>(correlationId, new TrendIDataInput
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                Chart1Type = "21",
                Chart1ItemId = "Fluid Load",
                Chart2Type = "",
                Chart2ItemId = "",
                Chart3Type = "",
                Chart3ItemId = "",
                Chart4Type = "",
                Chart4ItemId = "",
                StartDate = "2010-01-01",
                EndDate = "2024-01-01"
            });

            mockService.Setup(x => x.GetDataHistoryTrendDataItems(request.Value.AssetId.ToString(), correlationId))
                .Returns(responseData);

            var response = dataHistoryService.GetTrendDataAsync(request);

            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response.Result, typeof(DataHistoryTrendOutput));

            Assert.IsNotNull(response.Result.Values);
            Assert.IsInstanceOfType(response.Result.Values, typeof(List<GraphViewTrendsData>));

            Assert.IsTrue(response.Result.Values.Count > 0);
        }

        [TestMethod]
        public void GetDataHistoryFromInfluxDbStoreTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var phraseIds = SetPhraseIds();
            var mockLocalePhrase = new Mock<ILocalePhrases>();
            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseIds)).Returns(GetLocalPhrases());

            var mockHostAlarm = new Mock<IHostAlarm>();

            var mockInfluxDataStore = new Mock<IGetDataHistoryItemsService>();
            var mockService = new Mock<IDataHistorySQLStore>();
            var mockPortConfiguration = new Mock<IPortConfigurationStore>();
            var mockConfiguration = new Mock<IConfiguration>();
            var mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(x => x.Value).Returns("true");
            mockConfiguration.Setup(x => x.GetSection("TimeZoneBehavior:ApplicationTimeZone")).Returns(mockSection.Object);

            mockPortConfiguration.Setup(a => a.GetNode(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(GetNodeMasterData());
            mockPortConfiguration.Setup(a => a.IsLegacyWellAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            mockService.Setup(x => x.GetMeasurementTrendItems(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(GetMeasurementTrendItemModel());

            var mockParameterStore = new Mock<IParameterMongoStore>();

            var dataHistoryService = new DataHistoryProcessingService(mockService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockHostAlarm.Object,
                mockInfluxDataStore.Object, mockPortConfiguration.Object,
                mockParameterStore.Object, _commonService.Object, mockConfiguration.Object);

            var responseData = new List<DataPointModel>()
            {
                new()
                {
                    Time = DateTime.Now.AddHours(-3),
                    Value = 10,
                    ColumnValues = new Dictionary<string, string>
                    {
                        {"C1234", "10"},
                        {"C1235", "20"}
                    },
                },
                new()
                {
                    Time = DateTime.Now.AddHours(-2),
                    Value = 11,
                    ColumnValues = new Dictionary<string, string>
                    {
                        {"C1234", "11"},
                        {"C1235", "21"}
                    },
                },
                new()
                {
                    Time = DateTime.Now.AddHours(-1),
                    Value = 10,
                    ColumnValues = new Dictionary<string, string>
                    {
                        {"C1234", "12"},
                        {"C1235", "22"}
                    }
                },
                new()
                {
                    Time = DateTime.Now,
                    Value = 12,
                    ColumnValues = new Dictionary<string, string>
                    {
                        {"C1234", "11"},
                        {"C1235", "21"}
                    }
                },
            };

            var request = new WithCorrelationId<TrendIDataInput>("correlationId1", new TrendIDataInput
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                Chart1Type = "14",
                Chart1ItemId = "Parameter",
                Chart2Type = "",
                Chart2ItemId = "",
                Chart3Type = "",
                Chart3ItemId = "",
                Chart4Type = "",
                Chart4ItemId = "",
                StartDate = "2010-01-01",
                EndDate = "2024-01-01"
            });

            mockInfluxDataStore.Setup(x => x.GetDataHistoryItems(request.Value.AssetId, It.IsAny<Guid>(),
                It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<List<string>>(),
                It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult((IList<DataPointModel>)responseData));
            var nodeData = new List<NodeMasterModel>()
            {
                new()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();

            var rData = new DataHistoryItemModel
            {
                NodeMasterData = nodeData.FirstOrDefault(),
                FailureComponentTrendData = GetFailureComponentTrendItems(),
                FailureSubComponentTrendData = GetFailureSubComponentTrendItems(),
                EventsTrendData = GetEventsTrendItems(),
                MeterTrendData = GetMeterTrendItems(),
                PCSFDatalogConfiguration = GetPCSFDatalogConfiguration(),
            };
            mockService.Setup(x => x.GetDataHistoryTrendDataItems(request.Value.AssetId.ToString(), "correlationId1"))
                .Returns(rData);
            var response = dataHistoryService.GetTrendDataAsync(request);

            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response.Result, typeof(DataHistoryTrendOutput));

            Assert.IsNotNull(response.Result.Values);
            Assert.IsInstanceOfType(response.Result.Values, typeof(List<GraphViewTrendsData>));

            Assert.IsTrue(response.Result.Values.Count > 0);
            Assert.IsTrue(response.Result.Values.Count == 1);
        }

        [TestMethod]
        public void GetDataHistoryFromInfluxDbStoreNullInputParamTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var phraseIds = SetPhraseIds();
            var mockLocalePhrase = new Mock<ILocalePhrases>();
            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseIds)).Returns(GetLocalPhrases());

            var mockHostAlarm = new Mock<IHostAlarm>();

            var mockInfluxDataStore = new Mock<IGetDataHistoryItemsService>();
            var mockService = new Mock<IDataHistorySQLStore>();
            var mockPortConfiguration = new Mock<IPortConfigurationStore>();
            mockPortConfiguration.Setup(a => a.GetNode(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(GetNodeMasterData());
            mockPortConfiguration.Setup(a => a.IsLegacyWellAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            var mockParameterStore = new Mock<IParameterMongoStore>();
            var mockConfiguration = new Mock<IConfiguration>();

            var dataHistoryService = new DataHistoryProcessingService(mockService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockHostAlarm.Object,
                mockInfluxDataStore.Object, mockPortConfiguration.Object,
                mockParameterStore.Object, _commonService.Object, mockConfiguration.Object);

            var response = dataHistoryService.GetTrendDataAsync(null);

            Assert.IsNotNull(response);

            Assert.AreEqual(false, response.Result.Result.Status);
            logger.Verify(x => x.Write(Level.Error,
                It.Is<string>(x => x.Contains("input is null, cannot get data history trend data items."))),
                Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetDataHistoryFromInfluxDbStoreNullRequestTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var phraseIds = SetPhraseIds();
            var mockLocalePhrase = new Mock<ILocalePhrases>();
            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseIds)).Returns(GetLocalPhrases());

            var mockHostAlarm = new Mock<IHostAlarm>();

            var mockInfluxDataStore = new Mock<IGetDataHistoryItemsService>();
            var mockService = new Mock<IDataHistorySQLStore>();
            var mockPortConfiguration = new Mock<IPortConfigurationStore>();
            mockPortConfiguration.Setup(a => a.GetNode(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(GetNodeMasterData());
            mockPortConfiguration.Setup(a => a.IsLegacyWellAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            var mockParameterStore = new Mock<IParameterMongoStore>();
            var mockConfiguration = new Mock<IConfiguration>();

            var dataHistoryService = new DataHistoryProcessingService(mockService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockHostAlarm.Object,
                mockInfluxDataStore.Object, mockPortConfiguration.Object,
                mockParameterStore.Object, _commonService.Object, mockConfiguration.Object);
            var correlationId = Guid.NewGuid().ToString();

            var request = new WithCorrelationId<TrendIDataInput>(correlationId, null);

            var response = dataHistoryService.GetTrendDataAsync(request);

            Assert.IsNotNull(response);

            Assert.AreEqual(false, response.Result.Result.Status);
            logger.Verify(x => x.WriteCId(Level.Error,
                It.Is<string>(x => x.Contains("input is null, cannot get data history trend data items.")), correlationId),
                Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetTrendDataAsyncNullResponseDataTest()
        {
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var phraseIds = SetPhraseIds();
            var mockLocalePhrase = new Mock<ILocalePhrases>();
            mockLocalePhrase.Setup(x => x.GetAll(It.IsAny<string>(), phraseIds)).Returns(GetLocalPhrases());

            var mockHostAlarm = new Mock<IHostAlarm>();

            var mockInfluxDataStore = new Mock<IGetDataHistoryItemsService>();
            var mockService = new Mock<IDataHistorySQLStore>();
            var mockPortConfiguration = new Mock<IPortConfigurationStore>();
            var mockConfiguration = new Mock<IConfiguration>();
            mockPortConfiguration.Setup(a => a.GetNode(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(GetNodeMasterData());
            mockPortConfiguration.Setup(a => a.IsLegacyWellAsync(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(Task.FromResult(false));

            var mockParameterStore = new Mock<IParameterMongoStore>();
            mockService.Setup(x => x.GetMeasurementTrendItems(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(GetMeasurementTrendItemModel());

            var responseData = new List<DataPointModel>()
            {
                new()
                {
                    Time = DateTime.Now.AddHours(-3),
                    Value = 10,
                    ColumnValues = new Dictionary<string, string>
                    {
                        {"C1234", "10"},
                        {"C1235", "20"}
                    }
                },
                new()
                {
                    Time = DateTime.Now.AddHours(-2),
                    Value = null,
                    ColumnValues = null
                },
                new()
                {
                    Time = DateTime.Now.AddHours(-1),
                    Value = 10,
                    ColumnValues = new Dictionary<string, string>
                    {
                        {"C1234", "12"},
                        {"C1235", "22"}
                    }
                },
                new()
                {
                    Time = DateTime.Now,
                    Value = null,
                    ColumnValues = null
                },
            };

            var dataHistoryService = new DataHistoryProcessingService(mockService.Object,
                mockThetaLoggerFactory.Object, mockLocalePhrase.Object, mockHostAlarm.Object,
                mockInfluxDataStore.Object, mockPortConfiguration.Object,
                mockParameterStore.Object, _commonService.Object, mockConfiguration.Object);

            var request = new WithCorrelationId<TrendIDataInput>("correlationId1", new TrendIDataInput
            {
                AssetId = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                Chart1Type = "14",
                Chart1ItemId = "Parameter",
                Chart2Type = "",
                Chart2ItemId = "",
                Chart3Type = "",
                Chart3ItemId = "",
                Chart4Type = "",
                Chart4ItemId = "",
                StartDate = "2010-01-01",
                EndDate = "2024-01-01"
            });

            mockInfluxDataStore.Setup(x => x.GetDataHistoryItems(request.Value.AssetId, It.IsAny<Guid>(),
                It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<List<string>>(),
                It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult((IList<DataPointModel>)responseData));

            var response = dataHistoryService.GetTrendDataAsync(request);

            Assert.IsNotNull(response);
            Assert.AreEqual(false, response.Result.Result.Status);
        }

        #endregion

        #region Private Methods

        private IDictionary<int, string> GetLocalPhrases()
        {
            var phrases = new Dictionary<int, string>()
            {
                {151,"Failure"},
                {264,"Tubing Pressure"},
                {265,"Casing Pressure"},
                {298,"Group"},
                {440,"Analysis"},
                {441,"Events"},
                {513,"Volume"},
                {532,"OilRate"},
                {544,"Bottom Min Stress"},
                {545,"Top Min Stress"},
                {546,"Top Max Stress"},
                {599,"Rod Stress Analysis"},
                {655,"Parameters"},
                {689,"Duration"},
                {754,"Pressure"},
                {1074,"Target Pressure"},
                {1077,"Target Rate"},
                {1101,"Meter"},
                {1109,"Instant Rate"},
                {1111,"Accum. Volume"},
                {1113,"Valve Position"},
                {1250,"WaterRate"},
                {1251,"GasRate"},
                {1252,"TotalFluid"},
                {1352,"Well Test"},
                {2994,"Flow Rate"},
                {3969,"Gas lift"},
                {4160,"Production Temp."},
                {4188,"Custom"},
                {6340,"Operational Score"},
                {6383,"Production Latest BOE"},
                {6384,"Production Peak BOE"},
                {6385,"Production Down BOE"},
                {6475,"Component"},
                {6476,"Subcomponent"},
                {6683,"incl inj"},
                {6684,"excl inj"},
                {7022,"Fluid Above Pump"},
                {20140,"Injection Pressure"},
                {20141,"Differential Pressure"},
                {20142,"Line Pressure"},
                {20143,"Line Temperature"},
                {20144,"Unit Temperature"},
                {20145,"Board Temperature"},
                {20147,"Battery Voltage"},
                {20148,"Plunger Lift"},
                {20195,"Weight"},
                {20196,"FCU Line Pressure"},
                {20197,"FCU Line Temperature"},
                {20198,"FCU Rate"},
            };

            return phrases;
        }

        private IList<ComponentItemModel> GetFailureComponentTrendItems()
        {
            return new List<ComponentItemModel>
            {
                new()
                {
                    Id = 1,
                    Name = "None",
                    Application = 3
                },
                new()
                {
                    Id = 2,
                    Name = "Rod",
                    Application = 3
                },
                new()
                {
                    Id = 3,
                    Name = "Tubing",
                    Application = 3
                },
                new()
                {
                    Id = 4,
                    Name = "Pump",
                    Application = 3
                },
                new()
                {
                    Id = 5,
                    Name = "ESP Cable",
                    Application = 3
                }
            };
        }

        private IList<ComponentItemModel> GetFailureSubComponentTrendItems()
        {
            return new List<ComponentItemModel>
            {
                new()
                {
                    Id = 1,
                    Name = "Polished Rod",
                    ComponentId = 2
                },
                new()
                {
                    Id = 2,
                    Name = "Polished Rod Liner",
                    ComponentId = 3
                },
                new()
                {
                    Id = 3,
                    Name = "Polished Rod Clamp",
                    ComponentId = 3
                },
                new()
                {
                    Id = 4,
                    Name = "Rod Body",
                    ComponentId = 3
                },
                new()
                {
                    Id = 5,
                    Name = "Rod Connection Pin",
                    ComponentId = 3
                }
            };
        }

        private IList<MeterColumnItemModel> GetMeterTrendItems()
        {
            return new List<MeterColumnItemModel>
            {
                new()
                {
                    Id = 1,
                    MeterTypeId = 3,
                    Name = "Volume",
                },
                new()
                {
                    Id = 2,
                    Name = "AccumVolume",
                    MeterTypeId = 3
                },
                new()
                {
                    Id = 3,
                    Name = "Pressure",
                    MeterTypeId = 3
                },
                new()
                {
                    Id = 4,
                    Name = "TargetRate",
                    MeterTypeId = 3
                },
                new()
                {
                    Id = 5,
                    Name = "InstantRate",
                    MeterTypeId = 3
                }
            };
        }

        private IList<PCSFDatalogConfigurationItemModel> GetPCSFDatalogConfiguration()
        {
            return new List<PCSFDatalogConfigurationItemModel>
            {
                new()
                {
                    NodeId = "Well1",
                    DatalogNumber = 1,
                    ScheduledScanEnabled= true,
                    OnDemandScanEnabled = true,
                    LastSavedIndex = 1,
                    LastSavedDateTime = DateTime.Now,
                    DatalogName = ""
                },
            };
        }

        private IList<EventTrendItem> GetEventsTrendItems()
        {
            return new List<EventTrendItem>
            {
                new()
                {
                    EventTypeId = 1,
                    Name ="Comment",
                },
                new()
                {
                    EventTypeId = 2,
                    Name ="Param Change",
                },
                new()
                {
                    EventTypeId = 3,
                    Name ="Status Change",
                },
                new()
                {
                    EventTypeId = 4,
                    Name ="RTU Alarm",
                },
                new()
                {
                    EventTypeId = 5,
                    Name ="Host Alarm",
                },
            };
        }

        private int[] SetPhraseIds()
        {
            var phraseIds = new int[]
             {
                151,
                264,
                265,
                298,
                440,
                441,
                513,
                532,
                544,
                545,
                546,
                599,
                655,
                689,
                754,
                1074,
                1077,
                1101,
                1109,
                1111,
                1113,
                1250,
                1251,
                1252,
                1352,
                2994,
                3969,
                4160,
                4188,
                6340,
                6383,
                6384,
                6385,
                6475,
                6476,
                6683,
                6684,
                7022,
                20140,
                20141,
                20142,
                20143,
                20144,
                20145,
                20147,
                20148,
                20195,
                20196,
                20197,
                20198,
             };

            return phraseIds;
        }

        private IList<GroupTrendDataHistoryModel> GetGroupTrendDataHistoryModel()
        {
            return new List<GroupTrendDataHistoryModel>()
            {
                new()
                {
                    Date = DateTime.Now,
                    Value = 20,
                },
                new()
                {
                    Date = DateTime.Now,
                    Value = 23,
                },
                new()
                {
                    Date = DateTime.Now,
                    Value = 25,
                }
            };
        }

        private IList<GroupTrendDataModel> GetGroupTrendDataModel()
        {
            return new List<GroupTrendDataModel>
            {
                new()
                {
                    Id = 1,
                    Description = "Fluid Load"
                },
                new()
                {
                    Id = 2,
                    Description = "Group 2"
                },
                new()
                {
                    Id = 3,
                    Description = "Group 3"
                }
            };
        }

        private IList<MeasurementTrendItemModel> GetMeasurementTrendItemModel()
        {
            return new List<MeasurementTrendItemModel>
            {
                new()
                {
                    Description = "Parameter Test Equals",
                    ParamStandardType = 1,
                    Name = "Test Equals",
                    Address = 1235,
                },
                new()
                {
                    Description = "Parameter",
                    ParamStandardType = 1,
                    Name ="Test",
                    Address = 1234,
                }
            };
        }

        private IList<MeasurementTrendDataModel> GetMeasurementTrendDataModel()
        {
            return new List<MeasurementTrendDataModel>
            {
                new()
                {
                    Address = 1234,
                    Date = DateTime.Now,
                    IsManual = true,
                    Value = 200
                },
                new()
                {
                    Address = 1235,
                    Date = DateTime.Now,
                    IsManual = true,
                    Value = 200
                },
                new()
                {
                    Address = 1236,
                    Date = DateTime.Now,
                    IsManual = true,
                    Value = 200
                }
            };
        }

        private IList<ControllerTrendItemModel> GetControllerTrendItemModel()
        {
            return new List<ControllerTrendItemModel>
            {
                new()
                {
                    Address = 2002,
                    Description = "Parameter",
                    Name = "Test",
                },
                new()
                {
                    Address = 1235,
                    Description = "Parameter",
                    Name = "Test",
                },
                new()
                {
                    Address = 1236,
                    Description = "Parameter",
                    Name = "Test",
                }
            };
        }

        private IList<ControllerTrendDataModel> GetControllerTrendDataModel()
        {
            return new List<ControllerTrendDataModel>
            {
                new()
                {
                    Date = DateTime.Now,
                    Value = 1670
                },
                new()
                {
                    Date = DateTime.Now,
                    Value = 1330
                },
                new()
                {
                    Date = DateTime.Now,
                    Value = 1130
                }
            };
        }

        private IList<ProductionStatisticsTrendDataModel> GetProductionStatisticsTrendDataModel()
        {
            return new List<ProductionStatisticsTrendDataModel>
            {
                new()
                {
                    ProcessedDate = DateTime.Now,
                    Value = 1670
                },
                new()
                {
                    ProcessedDate = DateTime.Now,
                    Value = 1330
                },
                new()
                {
                    ProcessedDate = DateTime.Now,
                    Value = 1130
                },
                new()
                {
                    ProcessedDate = DateTime.Now,
                    Value = null
                },
                new()
                {
                    ProcessedDate = DateTime.Now,
                    Value = 1120
                }
            };
        }

        private NodeMasterModel GetNodeMasterData()
        {
            return new NodeMasterModel
            {
                NodeId = "TestNode",
                PocType = 8,
                InferredProd = 4,
                AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                PortId = 0
            };
        }

        private void SetupCommonService()
        {
            _commonService.Setup(x => x.GetSystemParameterNextGenSignificantDigits(It.IsAny<string>())).Returns(3);
        }

        #endregion

    }
}
