using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Data.Sql.Tests
{
    [TestClass]
    public class GLAnalysisGetCurveCoordinateTest
    {

        private Mock<IThetaLoggerFactory> _loggerFactory;
        private Mock<IThetaLogger> _logger;

        [TestInitialize]
        public void TestInitialize()
        {
            _loggerFactory = new Mock<IThetaLoggerFactory>();
            _logger = new Mock<IThetaLogger>();

            SetupThetaLoggerFactory();
        }

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest()
        {
            _ = new GLAnalysisGetCurveCoordinateSQLStore(null, _loggerFactory.Object);
        }

        [TestMethod]
        public void GLAnaylsisGetSurveyDateTest()
        {
            var nodeMasters = GetNodeMasterData().AsQueryable();
            var mocknodeMastersDbSet = SetupNodeMaster(nodeMasters);

            var glAnalysisResults = GetGLAnalysisResults().AsQueryable();
            var mockglAnalysisResultsDbSet = SetupGLAnalysisResults(glAnalysisResults);

            var wellDataResults = GetWellDataResults().AsQueryable();
            var mockwellDataResults = SetupGLWellDetails(wellDataResults);

            var perforation = GetPerforationEntity().AsQueryable();
            var mockperforation = SetupPerforationEntity(perforation);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.NodeMasters).Returns(mocknodeMastersDbSet.Object);
            mockContext.Setup(x => x.GLAnalysisResults).Returns(mockglAnalysisResultsDbSet.Object);
            mockContext.Setup(x => x.WellDetails).Returns(mockwellDataResults.Object);
            mockContext.Setup(x => x.Perforation).Returns(mockperforation.Object);

            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var glAnalysis = new GLAnalysisGetCurveCoordinateSQLStore(contextFactory.Object, _loggerFactory.Object);

            var id = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            var resultByAssetId = glAnalysis.GetDataForStaticFluidCurve(id, string.Empty);
            Assert.IsNotNull(resultByAssetId);
            Assert.AreEqual(resultByAssetId.KillFluidLevel, 2);
            Assert.AreEqual(resultByAssetId.ProductionDepth, 1);
            Assert.AreEqual(resultByAssetId.ReservoirFluidLevel, 3);
            Assert.AreEqual(resultByAssetId.ReservoirPressure, 1500);
            Assert.AreEqual(resultByAssetId.Perforations.Count, 2);
        }

        #endregion

        #region Private Methods

        private IList<NodeMasterEntity> GetNodeMasterData()
        {
            var nodeMasters = new List<NodeMasterEntity>()
            {
                new NodeMasterEntity()
                {
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                    NodeId = "Well1",
                },
                new NodeMasterEntity()
                {
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                    NodeId = "Well2",
                },
                new NodeMasterEntity()
                {
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                    NodeId = "Well3",
                },
            };

            return nodeMasters;
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
                    NodeId = "Well1",
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
                    KillFluidLevel = 2,
                    ReservoirFluidLevel = 3,
                },
                new GLAnalysisResultsEntity()
                {
                    Id = 2,
                    NodeId = "Well1",
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
                    KillFluidLevel = 2,
                    ReservoirFluidLevel = 3,
                }
            };

            return glAnalysisResults;
        }

        /// <summary>
        /// Gets the sample gl well details data.
        /// </summary>
        /// <returns>The <seealso cref="List{GLWellDetailEntity}"/> data.</returns>
        public static List<WellDetailsEntity> GetWellDataResults()
        {
            var glAnalysisResults = new List<WellDetailsEntity>()
            {
               new WellDetailsEntity()
                {
                    NodeId = "Well1",
                    PlungerDiameter = 2,
                    PumpDepth = 999,
                    Cycles = 0,
                    IdleTime = 5,
                    PumpingUnitId = "CP1",
                    POCGrossRate = 23926,
                    ProductionDepth = 1
                },
            };

            return glAnalysisResults;
        }

        /// <summary>
        /// Gets the sample Perforation Entity data.
        /// </summary>
        /// <returns>The <seealso cref="List{PerforationEntity}"/> data.</returns>
        public static List<PerforationEntity> GetPerforationEntity()
        {
            var perforation = new List<PerforationEntity>()
            {
                new PerforationEntity()
                {
                    NodeId = "Well1",
                    Depth = 2100,
                    Interval = 1,
                    Diameter = 1,
                    HolesPerFt = 1
                },
                new PerforationEntity()
                {
                    NodeId = "Well2",
                    Depth = 2200,
                    Interval = 1,
                    Diameter = 1,
                    HolesPerFt = 1
                },
            };

            return perforation;
        }

        #endregion

        #region Private Setup Methods

        private Mock<DbSet<NodeMasterEntity>> SetupNodeMaster(IQueryable<NodeMasterEntity> data)
        {
            var mockDbSet = new Mock<DbSet<NodeMasterEntity>>();
            mockDbSet.As<IQueryable<NodeMasterEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<NodeMasterEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<NodeMasterEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<NodeMasterEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet;
        }

        private Mock<DbSet<WellDetailsEntity>> SetupGLWellDetails(IQueryable<WellDetailsEntity> data)
        {
            var mockDbSet = new Mock<DbSet<WellDetailsEntity>>();
            mockDbSet.As<IQueryable<WellDetailsEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<WellDetailsEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<WellDetailsEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<WellDetailsEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet;
        }

        private Mock<DbSet<GLAnalysisResultsEntity>> SetupGLAnalysisResults(IQueryable<GLAnalysisResultsEntity> data)
        {
            var mockDbSet = new Mock<DbSet<GLAnalysisResultsEntity>>();
            mockDbSet.As<IQueryable<GLAnalysisResultsEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<GLAnalysisResultsEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<GLAnalysisResultsEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<GLAnalysisResultsEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet;
        }

        private Mock<DbSet<PerforationEntity>> SetupPerforationEntity(IQueryable<PerforationEntity> data)
        {
            var mockDbSet = new Mock<DbSet<PerforationEntity>>();
            mockDbSet.As<IQueryable<PerforationEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<PerforationEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<PerforationEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<PerforationEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet;
        }

        private Mock<NoLockXspocDbContext> SetupMockContext()
        {
            var contextOptions = new Mock<DbContextOptions<NoLockXspocDbContext>>();
            contextOptions.Setup(m => m.ContextType).Returns(typeof(NoLockXspocDbContext));
            contextOptions.Setup(m => m.Extensions).Returns(new List<IDbContextOptionsExtension>());

            var mockDateTimeConverter = new Mock<IDateTimeConverter>();
            var mockInterceptor = new Mock<IDbConnectionInterceptor>();
            var mockContext = new Mock<NoLockXspocDbContext>(contextOptions.Object, mockInterceptor.Object, mockDateTimeConverter.Object);

            return mockContext;
        }

        private void SetupThetaLoggerFactory()
        {
            _loggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(_logger.Object);
        }

        #endregion

    }
}