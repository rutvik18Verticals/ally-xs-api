using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Data.Sql.Tests
{
    [TestClass]
    public class AnalysisCurveSQLStoreTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest()
        {
            _ = new AnalysisCurveSQLStore(null, null);
        }

        [TestMethod]
        public void AnalysisResultCurvesDataTest()
        {
            var analysisResultCurvesData = GetAnalysisResultCurvesData().AsQueryable();
            var mockanalysisResultCurvesDbSet = SetupAnalysisResultCurves(analysisResultCurvesData);
            var correlationId = Guid.NewGuid().ToString();

            var curveTypesData = GetCurveTypesData().AsQueryable();
            var mockcurveTypesDbSet = SetupCurveTypes(curveTypesData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.AnalysisResultCurves).Returns(mockanalysisResultCurvesDbSet.Object);
            mockContext.Setup(x => x.CurveTypes).Returns(mockcurveTypesDbSet.Object);

            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var analysisCurve = new AnalysisCurveSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);
            IEnumerable<int> curveTypeIds = new List<int>() { 1, 2 };

            var resultByAssetId = analysisCurve.Fetch(101, 1, curveTypeIds, correlationId);
            Assert.IsNotNull(resultByAssetId);
            Assert.AreEqual(3, resultByAssetId.Count);
        }

        #endregion

        #region Private Methods

        private IList<AnalysisResultCurvesEntity> GetAnalysisResultCurvesData()
        {
            var eventData = new List<AnalysisResultCurvesEntity>()
            {
                new AnalysisResultCurvesEntity()
                {
                    Id = 101,
                    AnalysisResultId = 101,
                    CurveTypeId = 1,
                },
                new AnalysisResultCurvesEntity()
                {
                    Id = 102,
                    AnalysisResultId = 101,
                    CurveTypeId = 2,
                },
                new AnalysisResultCurvesEntity()
                {
                    Id = 103,
                    AnalysisResultId = 101,
                    CurveTypeId = 2,
                },
            };

            return eventData;
        }

        private IList<CurveTypesEntity> GetCurveTypesData()
        {
            var groupMembershipCacheData = new List<CurveTypesEntity>()
            {
                new CurveTypesEntity()
                {
                    Id = 1,
                    ApplicationTypeId = 1,
                    Name = "Name",
                    PhraseId = 1,
                },
                new CurveTypesEntity()
                {
                    Id = 2,
                    ApplicationTypeId = 1,
                    Name = "Name",
                    PhraseId = 1,
                },
            };

            return groupMembershipCacheData;
        }

        #endregion

        #region Private Setup Methods

        private Mock<DbSet<AnalysisResultCurvesEntity>> SetupAnalysisResultCurves(IQueryable<AnalysisResultCurvesEntity> data)
        {
            var mockDbSet = new Mock<DbSet<AnalysisResultCurvesEntity>>();
            mockDbSet.As<IQueryable<AnalysisResultCurvesEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<AnalysisResultCurvesEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<AnalysisResultCurvesEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<AnalysisResultCurvesEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet;
        }

        private Mock<DbSet<CurveTypesEntity>> SetupCurveTypes(IQueryable<CurveTypesEntity> data)
        {
            var mockDbSet = new Mock<DbSet<CurveTypesEntity>>();
            mockDbSet.As<IQueryable<CurveTypesEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<CurveTypesEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<CurveTypesEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<CurveTypesEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

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

        #endregion

    }
}