using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Influx.Services;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset.RodPump;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.Logging;
using MongoAssetCollection = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Customers;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter;
using MongoLookup = Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Lookup;

namespace Theta.XSPOC.Apex.Api.Data.Sql.Tests
{
    [TestClass]
    public class RodLiftAnalysisTests : DataStoreTestBase
    {

        #region Private Fields

        private Mock<IThetaLoggerFactory> _loggerFactory;
        private Mock<IThetaLogger> _logger;
        private Mock<IDateTimeConverter> _mockDateTimeConverter;
        private IConfiguration _configuration;
        private Mock<IAllyTimeSeriesNodeMaster> _allyNodeMasterStore;
        private Mock<IGetDataHistoryItemsService> _dataHistoryInfluxStore;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _loggerFactory = new Mock<IThetaLoggerFactory>();
            _logger = new Mock<IThetaLogger>();
            _mockDateTimeConverter = new Mock<IDateTimeConverter>();

            // Mongo services.
            _allyNodeMasterStore = new Mock<IAllyTimeSeriesNodeMaster>();

            // Influx services.
            _dataHistoryInfluxStore = new Mock<IGetDataHistoryItemsService>();

            // Setup mongo data.
            SetupMongoMockData();

            // Setup influx data.
            SetupInfluxMockData();

            // Setup configuration.
            SetupAppSettingsMockData();

            // Setup Logger.
            SetupThetaLoggerFactory();
        }

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullContextFactoryTest()
        {
            var mockLocalePhrase = new Mock<ILocalePhrases>();

            _ = new RodLiftAnalysisSQLStore(null, mockLocalePhrase.Object, _loggerFactory.Object, _mockDateTimeConverter.Object, _configuration, _allyNodeMasterStore.Object, 
                _dataHistoryInfluxStore.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullLocalePhrasesTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();

            _ = new RodLiftAnalysisSQLStore(contextFactory.Object, null, _loggerFactory.Object, _mockDateTimeConverter.Object, _configuration, _allyNodeMasterStore.Object,
                _dataHistoryInfluxStore.Object);
        }

        [TestMethod]
        public void RodLiftAnalysisTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockSqlStore = new Mock<ISQLStoreBase>();
            mockSqlStore.Setup(x => x.GetNodeMasterData(It.IsAny<Guid>())).Returns(GetNodeMasterData().AsQueryable().FirstOrDefault());

            mockSqlStore.Setup(x => x.GetCardData(It.IsAny<Guid>(), It.IsAny<DateTime>())).Returns(GetCardData().AsQueryable().FirstOrDefault());
            mockSqlStore.Setup(x => x.GetWellDetails(It.IsAny<Guid>())).Returns(GetWellDetails().AsQueryable().FirstOrDefault());

            var nodeData = new List<NodeMasterEntity>()
            {
                new NodeMasterEntity()
                {
                    NodeId = "AssetId1",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                    CompanyId = 1
                }
            }.AsQueryable();

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.WellDetails)
                .Returns(SetupMockDbSet(TestUtilities.GetWellDetails().AsQueryable()).Object);
            mockContext.Setup(x => x.CardData)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCardDatas().AsQueryable()).Object);
            mockContext.Setup(x => x.WellTest)
                .Returns(SetupMockDbSet(TestUtilities.GetWellTestData().AsQueryable()).Object);
            mockContext.Setup(x => x.LocalePhrases)
                .Returns(SetupMockDbSet(TestUtilities.GetLocalePhraseData().AsQueryable()).Object);
            mockContext.Setup(x => x.NodeMasters)
                .Returns(SetupMockDbSet(nodeData).Object);
            mockContext.Setup(x => x.CurrentRawScans)
                .Returns(SetupMockDbSet(TestUtilities.GetCurrentRawScanData().AsQueryable()).Object);
            mockContext.Setup(x => x.CustomPumpingUnits)
                .Returns(SetupMockDbSet(TestUtilities.GetCustomPumpingUnitsData().AsQueryable()).Object);
            mockContext.Setup(x => x.PumpingUnits)
                .Returns(SetupMockDbSet(TestUtilities.GetPumpingUnitsData().AsQueryable()).Object);
            mockContext.Setup(x => x.PumpingUnitManufacturer)
                .Returns(SetupMockDbSet(TestUtilities.GetPumpingUnitsManufacturersData().AsQueryable()).Object);
            mockContext.Setup(x => x.XDiagResult)
                .Returns(SetupMockDbSet(TestUtilities.GetXdiagResultsData().AsQueryable()).Object);
            mockContext.Setup(x => x.SystemParameters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetSystemParameter().AsQueryable()).Object);
            mockContext.Setup(x => x.Company)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCompanyData().AsQueryable()).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

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

            var rodLiftAnalysis = new RodLiftAnalysisSQLStore(contextFactory.Object, mockLocalePhrase.Object, _loggerFactory.Object, _mockDateTimeConverter.Object, _configuration, 
                _allyNodeMasterStore.Object, _dataHistoryInfluxStore.Object);

            var response = rodLiftAnalysis.GetRodLiftAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                "2023-05-11 22:29:55.000", "correlationId");
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void RodLiftAnalysisNullWellDetailsTest()
        {
            var nodeData = new List<NodeMasterEntity>()
            {
                new NodeMasterEntity()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.WellDetails)
                .Returns(SetupMockDbSet(TestUtilities.GetWellDetails().AsQueryable()).Object);
            mockContext.Setup(x => x.NodeMasters).Returns(SetupMockDbSet(nodeData).Object);

            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var mockLocalePhrase = new Mock<ILocalePhrases>();

            var rodLiftAnalysis = new RodLiftAnalysisSQLStore(contextFactory.Object, mockLocalePhrase.Object, _loggerFactory.Object, _mockDateTimeConverter.Object, _configuration, 
                _allyNodeMasterStore.Object, _dataHistoryInfluxStore.Object);

            var response = rodLiftAnalysis.GetRodLiftAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                "2023-05-11 22:29:55.000", "correlationId");
            Assert.IsNull(response);
        }

        [TestMethod]
        public void GetCardDatesByAssetIdTest()
        {
            var assetId = Guid.Parse("E432CB3B-295C-4ECB-8737-90D5D76AC6CF");
            var correlationId = "CorrelationId1";
            var nodeMasterData = TestUtilities.GetNodeMasterData().AsQueryable();
            var cardData = TestUtilities.GetCardData().AsQueryable();

            var mockNodeMasterDbSet = TestUtilities.SetupNodeMaster(nodeMasterData);
            var mockCardDataDbSet = TestUtilities.SetupCardData(cardData);

            var mockContextFactory = TestUtilities.SetupMockContext();
            mockContextFactory.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContextFactory.Setup(x => x.CardData).Returns(mockCardDataDbSet.Object);

            var mockSqlStore = new Mock<ISQLStoreBase>();

            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContextFactory.Object);

            var mockLocalePhrase = new Mock<ILocalePhrases>();
            var rodLiftAnalysis = new RodLiftAnalysisSQLStore(contextFactory.Object, mockLocalePhrase.Object, _loggerFactory.Object, _mockDateTimeConverter.Object, _configuration, 
                _allyNodeMasterStore.Object, _dataHistoryInfluxStore.Object);

            var result = rodLiftAnalysis.GetCardDatesByAssetId(assetId, correlationId);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        #region Private Methods

        private IList<NodeMasterModel> GetNodeMasterData()
        {

            var nodeData = new List<NodeMasterModel>()
            {
                new NodeMasterModel()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                    CompanyId = 1
                }
            };

            return nodeData;
        }

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

        private void SetupThetaLoggerFactory()
        {
            _loggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(_logger.Object);
        }

        /// <summary>
        /// set up influx current.
        /// </summary>
        private void SetupInfluxMockData()
        {
            _dataHistoryInfluxStore
                .Setup(x => x.GetCurrentRawScanData(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(GetCurrentRawScanData()));
        }

        /// <summary>
        /// set up appsetting json.
        /// </summary>
        private void SetupAppSettingsMockData()
        {
            var mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(s => s.Value).Returns("true");

            var mockConfig = new Mock<IConfiguration>();
            mockConfig.Setup(c => c.GetSection("EnableInflux")).Returns(mockSection.Object);

            _configuration = mockConfig.Object;
        }

        /// <summary>
        /// set up mongo data.
        /// </summary>
        private void SetupMongoMockData()
        {
            _allyNodeMasterStore
              .Setup(x => x.GetAssetAsync(It.IsAny<Guid>(), It.IsAny<string>()))
              .Returns((Guid id, string correlationId) => Task.FromResult(GetAssetModelData().FirstOrDefault()));

            _allyNodeMasterStore
              .Setup(x => x.GetCustomerAsync(It.IsAny<string>(), It.IsAny<string>()))
              .Returns((string id, string correlationId) => Task.FromResult(GetCustomerData().FirstOrDefault()));

            _allyNodeMasterStore
             .Setup(x => x.GetParametersBulk(It.IsAny<List<(int POCType, string ChannelId)>>(), It.IsAny<string>()))
             .Returns(Task.FromResult(GetParametersBulkMock())); //(Guid id, string correlationId) => 
        }

        private IList<MongoAssetCollection.Asset> GetAssetModelData()
        {
            return new List<MongoAssetCollection.Asset>()
            {
                new() {
                    Name = "AssetId1",
                    ArtificialLiftType = "RodLift",
                    CustomerId = "66d74f86c66365161d7ca943",
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "NodeId", "AssetId1" },
                        { "AssetGUID", "61e72096-72d4-4878-afb7-f042e0a30118" }
                    },
                    POCType = new Lookup()
                    {
                        LookupType = LookupTypes.POCTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "POCTypesId", "8" }
                        },
                    },
                    AssetConfig = new AssetConfig()
                    {
                        RunStatus = "Running",
                        TimeInState = 6,
                        TodayCycles = 4,
                        TodayRuntime = 5,
                        InferredProduction = 4,
                        IsEnabled = true,
                        PortId = "1",
                    },
                    AssetDetails = new RodPumpDetail()
                    {
                        PumpingUnit = new Lookup()
                        {
                            LookupType = LookupTypes.PumpingUnit.ToString(),
                            LegacyId = new Dictionary<string, string>()
                            {
                                { "UnitId", "AC1" }
                            },
                            LookupDocument = new PumpingUnit()
                            {
                                 APIDesignation = "A-320-270-100",
                                 UnitId = "AC1",
                                 ManufId = "AC",
                            }
                        },
                        Rods = new List<Rod>()
                        {
                            new()
                            {
                                RodNumber = 1,
                                RodGrade = new Lookup()
                                {
                                    LookupType = LookupTypes.RodGrade.ToString(),
                                    LegacyId = new Dictionary<string, string>()
                                    {
                                        { "RodGradeId", "1" }
                                    },
                                    LookupDocument = new RodGrade()
                                    {
                                        Name = "D",
                                        RodGradeId = 1,
                                    }
                                }
                            },
                            new()
                            {
                                RodNumber = 2,
                                RodGrade = new Lookup()
                                {
                                    LookupType = LookupTypes.RodGrade.ToString(),
                                    LegacyId = new Dictionary<string, string>()
                                    {
                                        { "RodGradeId", "2" }
                                    },
                                    LookupDocument = new RodGrade()
                                    {
                                        Name = "D",
                                        RodGradeId = 2,
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        private IEnumerable<Customer> GetCustomerData()
        {
            return new List<Customer>()
            {
                new()
                {
                    Id = "66d74f86c66365161d7ca943",
                    Name = "Customer 1",
                    LegacyId =  new Dictionary<string, string>()
                    {
                        { "CustomerGUID", "a5fa13b2-56e8-4aaa-a2d5-61375609de9e" },
                        { "CustomerId", "1"}
                    }
                },
            };
        }

        private IDictionary<(int POCType, string ChannelId), Parameters> GetParametersBulkMock()
        {
            var mockData = new List<Parameters>()
            {
                new()
                {
                    Name = "SPM",
                    Address = 39748,
                    Description = "SPM",
                    ChannelId = "C1",
                    POCType = new Lookup()
                    {
                        LookupType = LookupTypes.POCTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "POCTypesId", "8" }
                        },
                        LookupDocument = new MongoLookup.POCTypes()
                        {
                            POCType = 8
                        }
                    },
                    DataType = new Lookup()
                    {
                        LookupType = LookupTypes.DataTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "DataTypeId", "1" }
                        },
                        LookupDocument = new MongoLookup.DataTypes()
                        {
                            Comment = "Comment",
                            DataTypeId = 1,
                            Description = "Description",
                        }
                    },
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "POCType", "8" },
                        { "Address", "39748" },
                        { "Bit", "0" }
                    },
                    UnitType = new Lookup()
                    {
                        LookupType = LookupTypes.UnitTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "UnitTypesId", "0" }
                        },
                        LookupDocument = new MongoLookup.UnitTypes()
                        {
                            UnitTypesId = 0,
                            Description = "None",
                            PhraseId = 731
                        }
                    },
                    ParameterDocument = new FacilityTagDetails()
                    {
                        FacilityTagGroupID = 1
                    }
                },
                new()
                {
                    Name = "SPM2",
                    Address = 39749,
                    Description = "SPM",
                    ChannelId = "C2",
                    POCType = new Lookup()
                    {
                        LookupType = LookupTypes.POCTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "POCTypesId", "8" }
                        },
                        LookupDocument = new MongoLookup.POCTypes()
                        {
                            POCType = 8
                        }
                    },
                    DataType = new Lookup()
                    {
                        LookupType = LookupTypes.DataTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "DataTypeId", "1" }
                        },
                        LookupDocument = new MongoLookup.DataTypes()
                        {
                            Comment = "Comment",
                            DataTypeId = 1,
                            Description = "Description",
                        }
                    },
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "POCType", "8" },
                        { "Address", "39749" },
                        { "Bit", "0" }
                    },
                    UnitType = new Lookup()
                    {
                        LookupType = LookupTypes.UnitTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "UnitTypesId", "0" }
                        },
                        LookupDocument = new MongoLookup.UnitTypes()
                        {
                            UnitTypesId = 0,
                            Description = "None",
                            PhraseId = 731
                        }
                    },
                    ParameterDocument = new FacilityTagDetails()
                    {
                        FacilityTagGroupID = 1
                    }
                },
                new()
                {
                    Name = "SPM3",
                    Address = 39750,
                    Description = "SPM",
                    ChannelId = "C3",
                    POCType = new Lookup()
                    {
                        LookupType = LookupTypes.POCTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "POCTypesId", "8" }
                        },
                        LookupDocument = new MongoLookup.POCTypes()
                        {
                            POCType = 8
                        }
                    },
                    DataType = new Lookup()
                    {
                        LookupType = LookupTypes.DataTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "DataTypeId", "1" }
                        },
                        LookupDocument = new MongoLookup.DataTypes()
                        {
                            Comment = "Comment",
                            DataTypeId = 1,
                            Description = "Description",
                        }
                    },
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "POCType", "8" },
                        { "Address", "39750" },
                        { "Bit", "0" }
                    },
                    UnitType = new Lookup()
                    {
                        LookupType = LookupTypes.UnitTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "UnitTypesId", "0" }
                        },
                        LookupDocument = new MongoLookup.UnitTypes()
                        {
                            UnitTypesId = 0,
                            Description = "None",
                            PhraseId = 731
                        }
                    },
                    ParameterDocument = new FacilityTagDetails()
                    {
                        FacilityTagGroupID = 1
                    }
                },
                new()
                {
                    Name = "SPM4",
                    Address = 39751,
                    Description = "SPM",
                    ChannelId = "C4",
                    POCType = new Lookup()
                    {
                        LookupType = LookupTypes.POCTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "POCTypesId", "8" }
                        },
                        LookupDocument = new MongoLookup.POCTypes()
                        {
                            POCType = 8
                        }
                    },
                    DataType = new Lookup()
                    {
                        LookupType = LookupTypes.DataTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "DataTypeId", "1" }
                        },
                        LookupDocument = new MongoLookup.DataTypes()
                        {
                            Comment = "Comment",
                            DataTypeId = 1,
                            Description = "Description",
                        }
                    },
                    LegacyId = new Dictionary<string, string>()
                    {
                        { "POCType", "8" },
                        { "Address", "39751" },
                        { "Bit", "0" }
                    },
                    UnitType = new Lookup()
                    {
                        LookupType = LookupTypes.UnitTypes.ToString(),
                        LegacyId = new Dictionary<string, string>()
                        {
                            { "UnitTypesId", "0" }
                        },
                        LookupDocument = new MongoLookup.UnitTypes()
                        {
                            UnitTypesId = 0,
                            Description = "None",
                            PhraseId = 731
                        }
                    },
                    ParameterDocument = new FacilityTagDetails()
                    {
                        FacilityTagGroupID = 1
                    }
                }
            };

            // Map results to a dictionary
            var parameterDict = mockData //filteredData
                .GroupBy(param => (((POCTypes)param.POCType.LookupDocument).POCType, param.ChannelId))
                .ToDictionary(
                    group => group.Key,
                    group => group.First()
                );

            return parameterDict;
        }

        private IList<DataPointModel> GetCurrentRawScanData()
        {
            var mockInfluxData = new List<DataPointModel>
            {
                new()
                {
                    Time = DateTime.Parse("2023-05-11 02:02:00.000", new CultureInfo("us-en")),
                    Value = 120,
                    POCTypeId = "8",
                    TrendName = "C1"
                },
                new()
                {
                    Time = DateTime.Parse("2023-05-11 02:02:00.000", new CultureInfo("us-en")),
                    Value = 120,
                    POCTypeId = "8",
                    TrendName = "C2"
                },
                new()
                {
                    Time = DateTime.Parse("2023-05-11 02:02:00.000", new CultureInfo("us-en")),
                    Value = 89,
                    POCTypeId = "8",
                    TrendName = "C3"
                },
                new()
                {
                    Time = DateTime.Parse("2023-05-11 02:02:00.000", new CultureInfo("us-en")),
                    Value = 110,
                    POCTypeId = "8",
                    TrendName = "C4"
                 }
            };

            return mockInfluxData.ToList();
        }
        #endregion

        #endregion

    }
}
