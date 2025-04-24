using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Entity.EntitySetup;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;

namespace Theta.XSPOC.Apex.Api.Data.Sql.Tests
{
    /// <summary>
    /// Functions that are useful when setting up mock data in unit tests.
    /// </summary>
    public class TestUtilities
    {

        #region Public Static Methods

        /// <summary>
        /// Set up the mock context.
        /// </summary>
        /// <returns>The <seealso cref="Mock{NoLockXspocDbContext}"/> that represents the db context object.</returns>
        public static Mock<NoLockXspocDbContext> SetupMockContext()
        {
            var contextOptions = new Mock<DbContextOptions<NoLockXspocDbContext>>();
            contextOptions.Setup(m => m.ContextType).Returns(typeof(NoLockXspocDbContext));
            contextOptions.Setup(m => m.Extensions).Returns(new List<IDbContextOptionsExtension>());

            var mockDateTimeConverter = new Mock<IDateTimeConverter>();
            var mockInterceptor = new Mock<IDbConnectionInterceptor>();
            var mockContext = new Mock<NoLockXspocDbContext>(contextOptions.Object, mockInterceptor.Object, mockDateTimeConverter.Object);

            return mockContext;
        }

        /// <summary>
        /// Gets the mock db set of <seealso cref="T"/>
        /// </summary>
        /// <typeparam name="T">The entity of the db set.</typeparam>
        /// <param name="data">The data to populate the db set.</param>
        /// <returns>The mocked <seealso cref="DbSet{T}"/>.</returns>
        public static Mock<DbSet<T>> SetupMockData<T>(IQueryable<T> data) where T : class
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var mockDbSet = new Mock<DbSet<T>>();

            mockDbSet.As<IQueryable<T>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<T>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(data.ElementType);
            var mockDbSetSetupSequence = mockDbSet.As<IQueryable<T>>().SetupSequence(x => x.GetEnumerator())
                .Returns(data.GetEnumerator());

            for (var i = 0; i < 100; i++)
            {
                mockDbSetSetupSequence.Returns(data.GetEnumerator());
            }

            return mockDbSet;
        }

        public static Mock<DbSet<NodeMasterEntity>> SetupNodeMaster(IQueryable<NodeMasterEntity> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var mockDbSet = new Mock<DbSet<NodeMasterEntity>>();
            mockDbSet.As<IQueryable<NodeMasterEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<NodeMasterEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<NodeMasterEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<NodeMasterEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet;
        }

        public static Mock<DbSet<CardDataEntity>> SetupCardData(IQueryable<CardDataEntity> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            var mockDbSet = new Mock<DbSet<CardDataEntity>>();
            mockDbSet.As<IQueryable<CardDataEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<CardDataEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<CardDataEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<CardDataEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet;
        }

        #region Entity Data Setups

        /// <summary>
        /// Gets the sample card data.
        /// </summary>
        /// <returns>The <seealso cref="List{CardDataEntity}"/> data.</returns>
        public static List<CardDataEntity> GetCardData()
        {
            var cardDates = new List<CardDataEntity>()
            {
                new CardDataEntity()
                {
                    AnalysisDate = new DateTime(),
                    Area = 1,
                    AreaLimit = 1,
                    CardArea = 1,
                    CardType = "1",
                    CauseId = 1,
                    CorrectedCard = "1",
                    CardDate = new DateTime(),
                    DownHoleCard = "1",
                    DownHoleCardBinary = null,
                    ElectrogramCardBinary = null,
                    Fillage = 1,
                    FillBasePercent = 1,
                    HiLoadLimit = 1,
                    LoadLimit = 1,
                    LoadLimit2 = 1,
                    LoadSpanLimit = 1,
                    LowLoadLimit = 1,
                    MalfunctionLoadLimit = 1,
                    MalfunctionPositionLimit = 1,
                    NodeId = null,
                    PermissibleLoadDownBinary = null,
                    PermissibleLoadUpBinary = null,
                    PocDHCard = null,
                    POCDownholeCard = null,
                    PocDownHoleCardBinary = null,
                    PositionLimit = 1,
                    PositionLimit2 = 1,
                    PredictedCard = null,
                    PredictedCardBinary = null,
                    ProcessCard = 1,
                    Runtime = 1,
                    Saved = true,
                    SecondaryPumpFillage = 1,
                    StrokesPerMinute = 1,
                    StrokeLength = 1,
                    SurfaceCard = "1",
                    SurfaceCardBinary = null,
                    TorquePlotCurrent = "1",
                    TorquePlotCurrentBinary = null,
                    TorquePlotMinimumEnergy = "1",
                    TorquePlotMinimumEnergyBinary = null,
                    TorquePlotMinimumTorque = "1",
                    TorquePlotMinimumTorqueBinary = null,
                }
            };

            return cardDates;
        }

        /// <summary>
        /// Gets the sample card data.
        /// </summary>
        /// <returns>The <seealso cref="List{CardDataEntity}"/> data.</returns>
        public static List<CardDataEntity> GetCardDatas()
        {
            var eventData = new List<CardDataEntity>()
            {
                new CardDataEntity()
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

        /// <summary>
        /// Gets the sample node master data.
        /// </summary>
        /// <returns>The <seealso cref="List{NodeMasterEntity}"/> data.</returns>
        public static List<NodeMasterEntity> GetNodeMasterData()
        {
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
            };

            return nodeData;
        }

        /// <summary>
        /// Gets the sample well details data.
        /// </summary>
        /// <returns>The <seealso cref="List{WellDetailsEntity}"/> data.</returns>
        public static List<WellDetailsEntity> GetWellDetails()
        {
            var wellDetails = new List<WellDetailsEntity>()
            {
                new WellDetailsEntity()
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

        /// <summary>
        /// Gets the sample well test data.
        /// </summary>
        /// <returns>The <seealso cref="List{WellTestEntity}"/> data.</returns>
        public static List<WellTestEntity> GetWellTestData()
        {
            var wellTests = new List<WellTestEntity>()
            {
                new WellTestEntity()
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

        /// <summary>
        /// Gets the sample current raw scan data.
        /// </summary>
        /// <returns>The <seealso cref="List{CurrentRawScanDataEntity}"/> data.</returns>
        public static List<CurrentRawScanDataEntity> GetCurrentRawScanData()
        {
            return new List<CurrentRawScanDataEntity>
            {
                new CurrentRawScanDataEntity
                {
                    NodeId = "AssetId1",
                    Address = 39748,
                    Value = 120,
                    DateTimeUpdated = DateTime.Parse("2023-05-11 02:02:00.000", new CultureInfo("us-en")),
                },
                new CurrentRawScanDataEntity
                {
                    NodeId = "AssetId1",
                    Address = 39749,
                    Value = 120,
                    DateTimeUpdated = DateTime.Parse("2023-05-11 02:02:00.000", new CultureInfo("us-en")),
                },
                new CurrentRawScanDataEntity
                {
                    NodeId = "AssetId1",
                    Address = 39750,
                    Value = 89,
                    DateTimeUpdated = DateTime.Parse("2023-05-11 02:02:00.000", new CultureInfo("us-en")),
                },
                new CurrentRawScanDataEntity
                {
                    NodeId = "AssetId1",
                    Address = 39751,
                    Value = 110,
                    DateTimeUpdated = DateTime.Parse("2023-05-11 02:02:00.000", new CultureInfo("us-en")),
                }
            };
        }

        /// <summary>
        /// Gets the sample locale phrase data.
        /// </summary>
        /// <returns>The <seealso cref="List{LocalePhraseEntity}"/> data.</returns>
        public static List<LocalePhraseEntity> GetLocalePhraseData()
        {
            var localePhrase = new List<LocalePhraseEntity>()
            {
                new LocalePhraseEntity()
                {
                    PhraseId = 7048,
                    German =
                        "XDIAG has determined the pumping unit can increase speed to {0} SPM. This will result in an additional {1} barrels of gross and {2} barrels of oil.",
                    English =
                        "XDIAG has determined the pumping unit can increase speed to {0} SPM. This will result in an additional {1} barrels of gross and {2} barrels of oil.",
                    Spanish =
                        "XDIAG has determined the pumping unit can increase speed to {0} SPM. This will result in an additional {1} barrels of gross and {2} barrels of oil.",
                    French =
                        "XDIAG has determined the pumping unit can increase speed to {0} SPM. This will result in an additional {1} barrels of gross and {2} barrels of oil.",
                    Russian =
                        "XDIAG has determined the pumping unit can increase speed to {0} SPM. This will result in an additional {1} barrels of gross and {2} barrels of oil.",
                    Chinese =
                        "XDIAG has determined the pumping unit can increase speed to {0} SPM. This will result in an additional {1} barrels of gross and {2} barrels of oil."
                },
                new LocalePhraseEntity()
                {
                    PhraseId = 7049,
                    German =
                        "XDIAG has determined that the pumping unit is already operating close to maximum speed for rod pump system. In order to capture potential uplift, design changes will be necessary.",
                    English =
                        "XDIAG has determined that the pumping unit is already operating close to maximum speed for rod pump system. In order to capture potential uplift, design changes will be necessary.",
                    Spanish =
                        "XDIAG has determined that the pumping unit is already operating close to maximum speed for rod pump system. In order to capture potential uplift, design changes will be necessary.",
                    French =
                        "XDIAG has determined that the pumping unit is already operating close to maximum speed for rod pump system. In order to capture potential uplift, design changes will be necessary.",
                    Russian =
                        "XDIAG has determined that the pumping unit is already operating close to maximum speed for rod pump system. In order to capture potential uplift, design changes will be necessary.",
                    Chinese =
                        "XDIAG has determined that the pumping unit is already operating close to maximum speed for rod pump system. In order to capture potential uplift, design changes will be necessary."
                },
                new LocalePhraseEntity()
                {
                    PhraseId = 7117,
                    German = "Incremental production at {0} SPM is less than {1} bbls of oil per day.",
                    English = "Incremental production at {0} SPM is less than {1} bbls of oil per day.",
                    Spanish = "Incremental production at {0} SPM is less than {1} bbls of oil per day.",
                    French = "Incremental production at {0} SPM is less than {1} bbls of oil per day.",
                    Russian = "Incremental production at {0} SPM is less than {1} bbls of oil per day.",
                    Chinese = "Incremental production at {0} SPM is less than {1} bbls of oil per day."
                },
                new LocalePhraseEntity()
                {
                    PhraseId = 6819,
                    German =
                        "An incremental oil uplift opportunity has been discovered for this well test, this well is capable of producing an additional {0} ({1}) of total fluid, resulting in an additional {2} ({3}) of oil.",
                    English =
                        "An incremental oil uplift opportunity has been discovered for this well test, this well is capable of producing an additional {0} ({1}) of total fluid, resulting in an additional {2} ({3}) of oil.",
                    Spanish =
                        "An incremental oil uplift opportunity has been discovered for this well test, this well is capable of producing an additional {0} ({1}) of total fluid, resulting in an additional {2} ({3}) of oil.",
                    French =
                        "An incremental oil uplift opportunity has been discovered for this well test, this well is capable of producing an additional {0} ({1}) of total fluid, resulting in an additional {2} ({3}) of oil.",
                    Russian =
                        "An incremental oil uplift opportunity has been discovered for this well test, this well is capable of producing an additional {0} ({1}) of total fluid, resulting in an additional {2} ({3}) of oil.",
                    Chinese =
                        "An incremental oil uplift opportunity has been discovered for this well test, this well is capable of producing an additional {0} ({1}) of total fluid, resulting in an additional {2} ({3}) of oil."
                },
                new LocalePhraseEntity()
                {
                    PhraseId = 200,
                    German = "",
                    English = "",
                    Spanish = "",
                    French = "",
                    Russian = "",
                    Chinese = ""
                },
                new LocalePhraseEntity()
                {
                    PhraseId = 8009,
                    German = "Motor Overloaded",
                    English = "Motor Overloaded",
                    Spanish = "",
                    French = "",
                    Russian = "",
                    Chinese = ""
                },
            };

            return localePhrase;
        }

        /// <summary>
        /// Gets the sample custome pumping units data.
        /// </summary>
        /// <returns>The <seealso cref="List{CustomPumpingUnitEntity}"/> data.</returns>
        public static List<CustomPumpingUnitEntity> GetCustomPumpingUnitsData()
        {
            var pumpingUnits = new List<CustomPumpingUnitEntity>()
            {
                new CustomPumpingUnitEntity()
                {
                    Id = "CP1",
                    Manufacturer = "AC",
                    APIDesignation = "A-1500-400-192",
                }
            };

            return pumpingUnits;
        }

        /// <summary>
        /// Gets the sample pumping unit data.
        /// </summary>
        /// <returns>The <seealso cref="List{PumpingUnitsEntity}"/> data.</returns>
        public static List<PumpingUnitsEntity> GetPumpingUnitsData()
        {
            var pumpingUnits = new List<PumpingUnitsEntity>()
            {
                new PumpingUnitsEntity()
                {
                    ManufacturerId = "AC",
                    APIDesignation = "A-1500-400-192",
                    Id = 4900001,
                    UnitId = "CP1",
                    UnitName = "ABH-1500-40",
                    CrankHoles = 2,
                    Stroke1 = 2,
                    Stroke2 = 192,
                    Stroke3 = 170,
                    Stroke4 = 0,
                    Stroke5 = 0,
                    StructuralRating = 400,
                }
            };

            return pumpingUnits;
        }

        /// <summary>
        /// Gets the sample pumping unit manufacturers data.
        /// </summary>
        /// <returns>The <seealso cref="List{PumpingUnitManufacturerEntity}"/> data.</returns>
        public static List<PumpingUnitManufacturerEntity> GetPumpingUnitsManufacturersData()
        {
            var pumpingUnitsManufacturers = new List<PumpingUnitManufacturerEntity>()
            {
                new PumpingUnitManufacturerEntity()
                {
                    Id = 1,
                    ManufacturerAbbreviation = "AC",
                    UnitTypeId = 1,
                    RequiredRotation = 0
                }
            };

            return pumpingUnitsManufacturers;
        }

        /// <summary>
        /// Gets the sample xdiag results data.
        /// </summary>
        /// <returns>The <seealso cref="List{XDiagResultEntity}"/> data.</returns>
        public static List<XDiagResultEntity> GetXdiagResultsData()
        {
            var xDiagResults = new List<XDiagResultEntity>()
            {
                new XDiagResultEntity()
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
                new XDiagResultEntity()
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
                new XDiagResultEntity()
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

        /// <summary>
        /// Gets the sample states data.
        /// </summary>
        /// <returns>The <seealso cref="List{StatesEntity}"/> data.</returns>
        public static List<StatesEntity> GetStatesData()
        {
            var states = new List<StatesEntity>()
            {
                new StatesEntity()
                {
                    StateId = 308,
                    Value = 18,
                    Text = "Idle - Pumped Off",
                    Locked = false
                },
                new StatesEntity()
                {
                    StateId = 99,
                    Value = 18,
                    Text = "Idle - Pumped Off",
                    Locked = false
                },
            };

            return states;
        }

        /// <summary>
        /// Gets the sample states data.
        /// </summary>
        /// <returns>The <seealso cref="List{SystemParametersEntity}"/> data.</returns>
        public static List<SystemParametersEntity> GetSystemParameter()
        {
            var systemParameter = new List<SystemParametersEntity>()
            {
                new SystemParametersEntity()
                {
                    Parameter = "UpliftOppMinimumProductionThreshold",
                    Value = "3",
                },
                new SystemParametersEntity()
                {
                    Parameter = "GLIncludeInjGasInTest",
                    Value = "0",
                },
            };

            return systemParameter;
        }

        /// <summary>
        /// Gets the sample espanalysisresult data.
        /// </summary>
        /// <returns>The <seealso cref="List{ESPAnalysisResultsEntity}"/> data.</returns>
        public static List<ESPAnalysisResultsEntity> GetESPAnalysisResults()
        {
            var espAnalysisResults = new List<ESPAnalysisResultsEntity>()
            {
                new ESPAnalysisResultsEntity()
                {
                    NodeId = "AssetId1",
                    VerticalPumpDepth = 5,
                    MeasuredPumpDepth = 5,
                    PumpIntakePressure = 498.392f,
                    GrossRate = 421.625f,
                    FluidLevelAbovePump = 1204,
                    TubingPressure = -124,
                    CasingPressure = -124,
                    WaterSpecificGravity = 1.05f,
                    TestDate = new DateTime(2023, 5, 11, 22, 29, 55),
                    OilRate = 37.7f,
                    WaterRate = 37.7f,
                    GasRate = 6f,
                    HeadAcrossPump = 123.1f,
                    TotalVolumeAtPump = 235.1f
                }
            };

            return espAnalysisResults;
        }

        /// <summary>
        /// Gets the sample esp well pump data.
        /// </summary>
        /// <returns>The <seealso cref="List{ESPWellPumpEntity}"/> data.</returns>
        public static List<ESPWellPumpEntity> ESPWellPumpData()
        {
            var espWellPumps = new List<ESPWellPumpEntity>()
            {
                new ESPWellPumpEntity
                {
                    ESPPumpId = 1247,
                    ESPWellId = "AssetId1",
                    NumberOfStages = 5,
                    OrderNumber = 1,
                },
                new ESPWellPumpEntity
                {
                    ESPPumpId = 1246,
                    ESPWellId = "AssetId1",
                    NumberOfStages = 4,
                    OrderNumber = 2,
                },
                new ESPWellPumpEntity
                {
                    ESPPumpId = 1245,
                    ESPWellId = "AssetId1",
                    NumberOfStages = 7,
                    OrderNumber = 3,
                },
                new ESPWellPumpEntity
                {
                    ESPPumpId = 1243,
                    ESPWellId = "AssetId1",
                    NumberOfStages = 2,
                    OrderNumber = 4,
                }
            };

            return espWellPumps;
        }

        /// <summary>
        /// Gets the sample esp pump data.
        /// </summary>
        /// <returns>The <seealso cref="List{ESPPumpEntity}"/> data.</returns>
        public static List<ESPPumpEntity> ESPPumpData()
        {
            var espPumps = new List<ESPPumpEntity>()
            {
                new ESPPumpEntity
                {
                    ESPPumpId = 4,
                    Pump = "P10",
                    MinCasingSize = 1,
                    MinBPD = 800,
                    MaxBPD = 1000,
                    UseCoefficients = true,
                    Series = "400",
                    PumpModel = "P10",
                    ManufacturerId = 1,
                },
                new ESPPumpEntity
                {
                    ESPPumpId = 5,
                    Pump = "P22",
                    MinCasingSize = 1,
                    MinBPD = 1760,
                    MaxBPD = 2640,
                    UseCoefficients = true,
                    Series = "400",
                    PumpModel = "P22",
                    ManufacturerId = 1,
                }
            };

            return espPumps;
        }

        /// <summary>
        /// Gets the sample esp curvepoint data.
        /// </summary>
        /// <returns>The <seealso cref="List{ESPCurvePointEntity}"/> data.</returns>
        public static List<ESPCurvePointEntity> ESPCurvePointData()
        {
            var espPumps = new List<ESPCurvePointEntity>()
            {
                new ESPCurvePointEntity
                {
                    ESPPumpID = 4,
                    FlowRate = 3500,
                    Efficiency = 65.5,
                    HeadFeetPerStage = 39.1100006103516,
                    PowerInHP = 1.53999996185303,
                },
                new ESPCurvePointEntity
                {
                    ESPPumpID = 4,
                    FlowRate = 4000,
                    Efficiency = 68,
                    HeadFeetPerStage = 36.0999984741211,
                    PowerInHP = 1.55999994277954,
                }
            };

            return espPumps;
        }

        /// <summary>
        /// Gets the sample esp manufacturer data.
        /// </summary>
        /// <returns>The <seealso cref="List{ESPManufacturerEntity}"/> data.</returns>
        public static List<ESPManufacturerEntity> ESPManufacturerData()
        {
            var espManufacturer = new List<ESPManufacturerEntity>()
            {
                new ESPManufacturerEntity
                {
                    Manufacturer = "Baker Centrilift",
                    ManufacturerId = 1,
                },
                new ESPManufacturerEntity
                {
                    Manufacturer = "Weatherford",
                    ManufacturerId = 2,
                }
            };

            return espManufacturer;
        }

        /// <summary>
        /// Gets the sample group status user view data.
        /// </summary>
        /// <returns>The <seealso cref="List{GroupStatusUserViewEntity}"/> data.</returns>
        public static List<GroupStatusUserViewEntity> GetGroupStatusUserViewData()
        {
            var views = new List<GroupStatusUserViewEntity>()
            {
                new GroupStatusUserViewEntity()
                {
                    ViewId = 1,
                    GroupName = "test",
                    UserId = "Global"
                },
                new GroupStatusUserViewEntity()
                {
                    ViewId = 2,
                    GroupName = "test2",
                    UserId = "Global"
                },
            };

            return views;
        }

        /// <summary>
        /// Gets the sample group status  view data.
        /// </summary>
        /// <returns>The <seealso cref="List{GroupStatusViewEntity}"/> data.</returns>
        public static List<GroupStatusViewEntity> GetGroupStatusViewData()
        {
            var views = new List<GroupStatusViewEntity>()
            {
                new GroupStatusViewEntity()
                {
                    ViewId = 1,
                    ViewName = "Default",
                    FilterId = 1,
                    UserId = "Global",
                },
                new GroupStatusViewEntity()
                {
                    ViewId = 3,
                    ViewName = "PCP Global",
                    FilterId = 1,
                    UserId = "Global"
                },
            };

            return views;
        }

        /// <summary>
        /// Gets the sample glanalysisresults data.
        /// </summary>
        /// <returns>The <seealso cref="List{GLAnalysisResultsEntity}"/> data.</returns>
        public static List<GLAnalysisResultsEntity> GetGLAnalysisResults()
        {
            var glAnalysisResults = new List<GLAnalysisResultsEntity>()
            {
                new GLAnalysisResultsEntity()
                {
                    Id = 1,
                    NodeId = "AssetId1",
                    GrossRate = 421.625f,
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
                    AnalysisType = 1,
                },
                new GLAnalysisResultsEntity()
                {
                    Id = 2,
                    NodeId = "AssetId1",
                    GrossRate = 421.625f,
                    TestDate = new DateTime(2023, 5, 11, 22, 29, 55),
                    ProcessedDate = new DateTime(2023, 5, 11, 22, 29, 55),
                    Success = true,
                    GasInjectionDepth = 10586f,
                    OilRate = 180f,
                    WaterRate = 53f,
                    GasRate = 1720f,
                    WellheadPressure = 150,
                    WaterCut = 53,
                    GasSpecificGravity = 0.86f,
                    WaterSpecificGravity = 1.32f,
                    WellheadTemperature = 100,
                    BottomholeTemperature = 194,
                    OilSpecificGravity = 0.8250729f,
                    CasingId = 6.135f,
                    TubingId = 2.441f,
                    TubingOD = 2.875f,
                    ReservoirPressure = 2100,
                    BubblepointPressure = 2500,
                    CasingPressure = 900,
                    AnalysisType = 1,
                }
            };

            return glAnalysisResults;
        }

        /// <summary>
        /// Gets the sample gl well details data.
        /// </summary>
        /// <returns>The <seealso cref="List{GLWellDetailEntity}"/> data.</returns>
        public static List<GLWellDetailEntity> GetGLWellDataResults()
        {
            var glAnalysisResults = new List<GLWellDetailEntity>()
            {
                new GLWellDetailEntity()
                {
                    NodeId = "AssetId1",
                    GasInjectionPressure = 2100,
                    InjectedGasSpecificGravity = 1,
                    InjectingBelowTubing = false,
                    EstimateInjectionDepth = true,
                    ValveConfigurationOption = 1,
                    UseDownholeGageInAnalysis = false
                }
            };

            return glAnalysisResults;
        }

        /// <summary>
        /// Gets the sample gl well valve data.
        /// </summary>
        /// <returns>The <seealso cref="List{GLWellValveEntity}"/> data.</returns>
        public static List<GLWellValveEntity> GetGLWellValveResultData()
        {
            var result = new List<GLWellValveEntity>
            {
                new GLWellValveEntity
                {
                    Id = 1,
                    NodeId = "AssetId1",
                    GLValveId = 1,
                    VerticalDepth = 2226.12f,
                    TestRackOpeningPressure = 980,
                    ClosingPressureAtDepth = 1012.254f,
                    MeasuredDepth = 2226.12f,
                    OpeningPressureAtDepth = 1063.807f,
                    OpeningPressureAtSurface = 1000,
                    ClosingPressureAtSurface = 948
                },
                new GLWellValveEntity
                {
                    Id = 2,
                    NodeId = "AssetId1",
                    GLValveId = 1,
                    VerticalDepth = 3453.61f,
                    TestRackOpeningPressure = 965,
                    ClosingPressureAtDepth = 1018.697f,
                    MeasuredDepth = 3453.61f,
                    OpeningPressureAtDepth = 1063.259f,
                    OpeningPressureAtSurface = 963,
                    ClosingPressureAtSurface = 926,
                }
            };
            return result;
        }

        /// <summary>
        /// Gets the sample gl well orifice data.
        /// </summary>
        /// <returns>The <seealso cref="List{GLWellOrificeEntity}"/> data.</returns>
        public static List<GLWellOrificeEntity> GetGLWellOrificeData()
        {
            return new List<GLWellOrificeEntity>
            {
                new GLWellOrificeEntity()
                {
                    NodeId = "AssetId1",
                    ManufacturerId = 1,
                    MeasuredDepth = 7113.62f,
                    VerticalDepth = 7113.62f,
                    PortSize = 0.1875f,
                }
            };
        }

        /// <summary>
        /// Gets the sample gl valve status data.
        /// </summary>
        /// <returns>The <seealso cref="List{GLValveStatusEntity}"/> data.</returns>
        public static List<GLValveStatusEntity> GetGLValveStatusResultData()
        {
            return new List<GLValveStatusEntity>
            {
                new GLValveStatusEntity()
                {
                    GLWellValveId = 1,
                    PercentOpen = 0,
                    GLAnalysisResultId = 1,
                    InjectionPressureAtDepth = 1027.482f,
                    ValveState = 1,
                    IsInjectingGas = true,
                    InjectionRateForTubingCriticalVelocity = 905.3361f,
                    TubingCriticalVelocityAtDepth = 1005.336f,
                },
                new GLValveStatusEntity()
                {
                    GLWellValveId = 1,
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

        /// <summary>
        /// Gets the sample gl valve data.
        /// </summary>
        /// <returns>The <seealso cref="List{GLValveEntity}"/> data.</returns>
        public static List<GLValveEntity> GetGLValveResultData()
        {
            return new List<GLValveEntity>
            {
                new GLValveEntity()
                {
                    Id = 1,
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
                new GLValveEntity()
                {
                    Id = 2,
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
        }

        /// <summary>
        /// Gets the sample gl orifice status data.
        /// </summary>
        /// <returns>The <seealso cref="List{GLOrificeStatusEntity}"/> data.</returns>
        public static List<GLOrificeStatusEntity> GetGLOrificeStatusData()
        {
            return new List<GLOrificeStatusEntity>
            {
                new GLOrificeStatusEntity()
                {
                    NodeId = "AssetId1",
                    OrificeState = 4,
                    IsInjectingGas = true,
                    GLAnalysisResultId = 1,
                    InjectionRateForTubingCriticalVelocity = 995.8148f,
                    TubingCriticalVelocityAtDepth = 1095.815f,
                }
            };
        }

        /// <summary>
        /// Gets the sample gl well orifice data.
        /// </summary>
        /// <returns>The <seealso cref="List{GLWellOrificeEntity}"/> data.</returns>
        public static List<GLWellOrificeEntity> GLWellOrificeData()
        {
            return new List<GLWellOrificeEntity>
            {
                new GLWellOrificeEntity()
                {
                    NodeId = "AssetId1",
                    ManufacturerId = 1,
                    MeasuredDepth = 7113.62f,
                    VerticalDepth = 7113.62f,
                    PortSize = 0.1875f,
                }
            };
        }

        /// <summary>
        /// Gets the sample group parameter data.
        /// </summary>
        /// <returns>The <seealso cref="List{GroupParameterEntity}"/> data.</returns>
        public static List<GroupParameterEntity> GetGroupParameters()
        {
            return new List<GroupParameterEntity>
            {
                new GroupParameterEntity()
                {
                    Id = 1,
                    PhraseId = null,
                    Description = "Fluid Load",
                    FunctionType = 1
                }
            };
        }

        /// <summary>
        /// Gets the sample Get Group Data History.
        /// </summary>
        /// <returns>The <seealso cref="List{GroupDataHistoryEntity}"/> data.</returns>
        public static List<GroupDataHistoryEntity> GetGroupDataHistory()
        {
            return new List<GroupDataHistoryEntity>
            {
                new GroupDataHistoryEntity()
                {
                    ID = 1,
                    GroupParameterID = 1,
                    Name = "Fluid Load",
                    Date = Convert.ToDateTime("2023-08-24 09:28:09.000"),
                    Value = (decimal?)0.2f
                },
                new GroupDataHistoryEntity()
                {
                    ID = 2,
                    GroupParameterID = 1,
                    Name = "Fluid Load",
                    Date = Convert.ToDateTime("2023-08-23 09:28:09.000"),
                    Value = (decimal?)0.2f
                },
                new GroupDataHistoryEntity()
                {
                    ID = 3,
                    GroupParameterID = 1,
                    Name = "Fluid Load",
                    Date = Convert.ToDateTime("2023-08-25 09:28:09.000"),
                    Value = (decimal?)0.2f
                },
            };
        }

        /// <summary>
        /// Gets the sample parameter data.
        /// </summary>
        /// <returns>The <seealso cref="List{ParameterEntity}"/> data.</returns>
        public static List<ParameterEntity> GetParameters()
        {
            return new List<ParameterEntity>
            {
                new ParameterEntity()
                {
                    Poctype = 8,
                    Address = 1,
                    Description = "Fluid Load",
                    PhraseId = 6819,
                    DataType = 1,
                    Access = "",
                    ScaleFactor = 0.1f,
                    Offset = 0.2f,
                    Setpoint = true,
                    StatusScan = true,
                    DataCollection = true,
                    Decimals = 1,
                    CollectionMode = 1,
                    SetpointGroup = 1,
                    GroupStatusView = 1,
                    Tag = "",
                    StateId = 1,
                    Locked = true,
                    FastScan = true,
                    UnitType = 1,
                    DestinationType = 1,
                    ParamStandardType = 1,
                    ArchiveFunction = 1,
                    EarliestSupportedVersion = 1,
                }
            };
        }

        /// <summary>
        /// Gets the sample facility tag data.
        /// </summary>
        /// <returns>The <seealso cref="List{FacilityTagsEntity}"/> data.</returns>
        public static List<FacilityTagsEntity> GetFacilityTags()
        {
            return new List<FacilityTagsEntity>
            {
                new FacilityTagsEntity()
                {
                    NodeId = "AssetId1",
                    Address = 1,
                    Description = "Fluid Load",
                    Enabled = true,
                    TrendType = 1,
                    RawLo = 1,
                    RawHi = 2,
                    EngLo = 1,
                    EngHi = 2,
                    LimitLo = 1,
                    LimitHi = 2,
                    CurrentValue = 1,
                    EngUnits = "",
                    Writeable = true,
                    Topic = "",
                    GroupNodeId = "AssetId1",
                    AlarmState = 1,
                    AlarmAction = 1,
                    WellGroupName = "Well Group Test 1",
                    PagingGroup = "Paging Group Test 1",
                    AlarmArg = "",
                    AlarmTextLo = "",
                    AlarmTextHi = "",
                    AlarmTextClear = "",
                    VoiceTextLo = "",
                    FacilityTagGroupId = 1,
                    ParamStandardType = null,
                    Bit = 0,
                }
            };
        }

        /// <summary>
        /// Gets the sample parameter data.
        /// </summary>
        /// <returns>The <seealso cref="List{ParameterEntity}"/> data.</returns>
        public static List<ParameterEntity> GetParameter()
        {
            return new List<ParameterEntity>
            {
                new ParameterEntity()
                {
                    PhraseId = null,
                    Description = "Fluid Load",
                    Address=2002,
                    Poctype=99,
                    ParamStandardType=268,
                },
                new ParameterEntity()
                {
                    PhraseId = null,
                    Description = "Fluid Load Test",
                    Address=2005,
                    Poctype=99,
                    ParamStandardType=268,
                },
                new ParameterEntity()
                {
                    PhraseId = null,
                    Description = "Fluid Load",
                    Address=32569,
                    Poctype=8,
                    ParamStandardType=268,
                }
            };
        }
        /// <summary>
        /// Gets the Facility Tags data.
        /// </summary>
        /// <returns>The <seealso cref="List{FacilityTagsEntity}"/> data.</returns>
        public static List<FacilityTagsEntity> GetFacilityTag()
        {
            return new List<FacilityTagsEntity>
            {
                new FacilityTagsEntity()
                {
                    Description = "Fluid Load",
                    Address=2002,
                    Bit=0,
                    ParamStandardType=268,
                    GroupNodeId="Fluid Load",
                },
                 new FacilityTagsEntity()
                {
                    Description = "Fluid Load Test",
                    Address=2005,
                    Bit=0,
                    ParamStandardType=268,
                    GroupNodeId="Fluid Load Test",
                }
            };
        }

        /// <summary>
        /// Gets the sample data history data.
        /// </summary>
        /// <returns>The <seealso cref="List{DataHistoryEntity}"/> data.</returns>
        public static List<DataHistoryEntity> GetDataHistories()
        {
            return new List<DataHistoryEntity>
            {
                new DataHistoryEntity()
                {
                    Date = DateTime.Now,
                    NodeID = "AssetId1",
                    Address = 1,
                    Value = 0,
                }
            };
        }

        /// <summary>
        /// Gets the sample data history archive data.
        /// </summary>
        /// <returns>The <seealso cref="List{DataHistoryArchiveEntity}"/> data.</returns>
        public static List<DataHistoryArchiveEntity> GetDataHistoryArchives()
        {
            return new List<DataHistoryArchiveEntity>
            {
                new DataHistoryArchiveEntity()
                {
                    Date = DateTime.Now,
                    NodeID = "AssetId1",
                    Address = 1,
                    Value = 0,
                }
            };
        }

        /// <summary>
        /// Gets the Data History.
        /// </summary>
        /// <returns>The <seealso cref="List{DataHistoryEntity}"/> data.</returns>
        public static List<DataHistoryEntity> GetDataHistory()
        {
            return new List<DataHistoryEntity>
            {
                new DataHistoryEntity()
                {
                    Address = 2002,
                    Date = new DateTime(2023, 11, 01, 01, 00, 00),
                    Value = 1,
                    NodeID = "AssetId1",
                },
                new DataHistoryEntity()
                {
                    Address = 2005,
                    Date = new DateTime(2023, 11, 01, 01, 00, 00),
                    Value = 1,
                    NodeID = "AssetId1",
                },
                new DataHistoryEntity()
                {
                    Address = 32569,
                    Date = new DateTime(2023, 11, 01, 01, 00, 00),
                    Value = 1,
                    NodeID = "AssetId1",
                },
                new DataHistoryEntity()
                {
                    Address = 32569,
                    Date = new DateTime(2023, 11, 01, 01, 15, 00),
                    Value = 1,
                    NodeID = "AssetId1",
                },
                new DataHistoryEntity()
                {
                    Address = 32569,
                    Date = new DateTime(2023, 11, 01, 01, 20, 00),
                    Value = 1,
                    NodeID = "AssetId1",
                },
                new DataHistoryEntity()
                {
                    Address = 32569,
                    Date = new DateTime(2023, 11, 01, 01, 25, 00),
                    Value = 1,
                    NodeID = "AssetId1",
                }
            };
        }
        /// <summary>
        /// Gets the Data History Archive.
        /// </summary>
        /// <returns>The <seealso cref="List{DataHistoryArchiveEntity}"/> data.</returns>
        public static List<DataHistoryArchiveEntity> GetDataHistoryArchive()
        {
            return new List<DataHistoryArchiveEntity>
            {
                new DataHistoryArchiveEntity()
                {
                    Address=123,
                    Date=DateTime.Now.AddDays(-1),
                    Value= 1,
                    NodeID="Fluid Load",
                }
            };
        }

        /// <summary>
        /// Gets the sample data for events data.
        /// </summary>
        /// <returns>The <seealso cref="List{EventsEntity}"/> data.</returns>
        public static List<EventsEntity> GetEventsData()
        {
            return new List<EventsEntity>
            {
                new EventsEntity()
                {
                    EventId = 1,
                    NodeId = "well1",
                    EventTypeId = 7,
                    Date = new DateTime(2023, 11, 9, 12, 14, 14, 14),
                    Description = "",
                    Status = "",
                    Note = "note",
                    UserId = "Theta",
                    TransactionId = 1
                },
                new EventsEntity()
                {
                    EventId = 2,
                    NodeId = "well1",
                    EventTypeId = 7,
                    Date = new DateTime(2023, 11, 9, 12, 14, 14, 14),
                    Description = "",
                    Status = "",
                    Note = "note",
                    UserId = "Theta",
                    TransactionId = 1
                }
            };
        }

        /// <summary>
        /// Gets the sample Get AnalysisTrendData.
        /// </summary>
        /// <returns>The <seealso cref="List{XDiagResultEntity}"/> data.</returns>
        public static List<XDiagResultEntity> GetAnalysisTrendData()
        {
            return new List<XDiagResultEntity>
            {
                new XDiagResultEntity()
                {
                    NodeId = "1",
                    Date = Convert.ToDateTime("2023-08-24 09:28:09.000"),
                },
            };
        }

        /// <summary>
        /// Gets the sample meter history data.
        /// </summary>
        /// <returns>The <seealso cref="List{MeterHistoryEntity}"/> data.</returns>
        public static List<MeterHistoryEntity> GetMeterHistory()
        {
            return new List<MeterHistoryEntity>
            {
                new MeterHistoryEntity()
                {
                    Date = Convert.ToDateTime("2023-08-24 09:28:09.000"),
                    NodeId = "1",
                    UserId = "Global",
                    RecordType = 1,
                    Approved = true,
                }
            };
        }

        /// <summary>
        /// Gets the sample Operational Score Entity data.
        /// </summary>
        /// <returns>The <seealso cref="List{OperationalScoreEntity}"/> data.</returns>
        public static List<OperationalScoreEntity> GetOperationalScoreEntity()
        {
            return new List<OperationalScoreEntity>
            {
                new OperationalScoreEntity()
                {
                   Id = 1,
                   NodeId="KIMBERLY 807MS",
                   ScoreDateTime=Convert.ToDateTime("2017-11-16"),
                   OperationalScore=45,
                },
                 new OperationalScoreEntity()
                {
                   Id = 2,
                   NodeId="Holden 2-18WH",
                   ScoreDateTime=Convert.ToDateTime("2017-11-16"),
                   OperationalScore=43,
                }
            };
        }

        /// <summary>
        /// Gets the sample Get Production Statistics Data.
        /// </summary>
        /// <returns>The <seealso cref="List{ProductionStatisticsEntity}"/> data.</returns>
        public static List<ProductionStatisticsEntity> GetProductionStatisticsData()
        {
            return new List<ProductionStatisticsEntity>
            {
                new ProductionStatisticsEntity()
                {
                    NodeId = "1",
                    ProcessedDate = Convert.ToDateTime("2023-08-24 09:28:09.000"),
                },
            };
        }

        /// <summary>
        /// Gets the sample Get XDiagRodResultsEntity.
        /// </summary>
        /// <returns>The <seealso cref="List{XDiagRodResultsEntity}"/> data.</returns>
        public static List<XDiagRodResultsEntity> GetXDiagRodResultData()
        {
            return new List<XDiagRodResultsEntity>
            {
                new XDiagRodResultsEntity()
                {
                    NodeId = "1",
                    Date = Convert.ToDateTime("2023-11-22 09:28:09.000"),
                    RodNum = 111,
                    Grade = "D",
                    Length = 1,
                    Diameter = 1,
                    Loading = 3.4f,
                    BottomMinStress = 1,
                    TopMaxStress = 1,
                    TopMinStress = 1,
                    RodGuideID = 1,
                    DragFrictionCoefficient = 1,
                    GuideCountPerRod = 1,
                },
                new XDiagRodResultsEntity()
                {
                    NodeId = "2",
                    Date = Convert.ToDateTime("2023-11-23 09:28:09.000"),
                    RodNum = 111,
                    Grade = "D",
                    Length = 1,
                    Diameter = 1,
                    Loading = 3.4f,
                    BottomMinStress = 1,
                    TopMaxStress = 1,
                    TopMinStress = 1,
                    RodGuideID = 1,
                    DragFrictionCoefficient = 1,
                    GuideCountPerRod = 1,
                }
            };
        }

        /// <summary>
        /// Gets the sample plunger lift trend data..
        /// </summary>
        /// <returns>The <seealso cref="List{PlungerLiftDataHistoryEntity}"/> data.</returns>
        public static List<PlungerLiftDataHistoryEntity> GetPlungerLiftTrendData()
        {
            return new List<PlungerLiftDataHistoryEntity>
            {
                new PlungerLiftDataHistoryEntity()
                {
                    NodeId = "1",
                    CasingPressure = 1,
                    TubingPressure = 1,
                    DifferentialPressure = 1,
                    Date = Convert.ToDateTime("2023-08-24 09:28:09.000"),
                },
            };
        }

        /// <summary>
        /// Gets the sample company data.
        /// </summary>
        /// <returns>The <seealso cref="List{CompanyEntity}"/> data.</returns>
        public static List<CompanyEntity> GetCompanyData()
        {
            var eventData = new List<CompanyEntity>()
            {
                new CompanyEntity()
                {
                    CustomerGUID =new Guid("F64E6D1D-F16F-4A04-82E5-DB0365CF4990"),
                    Id = 1,
                    Name = "Company1"
                }
            };

            return eventData;
        }

        /// <summary>
        /// Gets the sample analytics classification entity data.
        /// </summary>
        /// <returns>The <seealso cref="List{AnalyticsClassificationEntity}"/> data.</returns>
        public static List<AnalyticsClassificationEntity> GetAnalyticsClassification()
        {
            var analyticsClassification = new List<AnalyticsClassificationEntity>()
            {
                new AnalyticsClassificationEntity()
                {
                    Id = 402024,
                    NodeId= "Turner A 39",
                    StartDate =  DateTime.Now.AddDays(-5),
                    EndDate = DateTime.Now.AddDays(-5),
                    ClassificationTypeId = 27
                },
                 new AnalyticsClassificationEntity()
                {
                    Id = 402024,
                    NodeId= "Turner A 39",
                    StartDate =  DateTime.Now.AddDays(-5),
                    EndDate = null,
                    ClassificationTypeId = 27
                }
            };

            return analyticsClassification;
        }

        /// <summary>
        /// Gets the sample analytics classification types entity data.
        /// </summary>
        /// <returns>The <seealso cref="List{AnalyticsClassificationTypeEntity}"/> data.</returns>
        public static List<AnalyticsClassificationTypeEntity> GetAnalyticsClassificationTypes()
        {
            var analyticsClassification = new List<AnalyticsClassificationTypeEntity>()
            {
                new AnalyticsClassificationTypeEntity()
                {
                    Id= 27,
                    Name = "Motor Overloaded",
                    PhraseID = 8009,
                },
            };

            return analyticsClassification;
        }

        /// <summary>
        /// Gets the User Default Entity entity data.
        /// </summary>
        /// <returns>The <seealso cref="List{UserDefaultEntity}"/> data.</returns>
        public static List<UserDefaultEntity> GetUserDefaults()
        {
            var userDefaultEntity = new List<UserDefaultEntity>()
            {
                new UserDefaultEntity()
                {
                   DefaultsGroup="frmGroupStatus",
                   Property="LastView",
                   UserId="Global",
                   Value="461",
                },
            };

            return userDefaultEntity;
        }

        #endregion

        #endregion

    }
}
