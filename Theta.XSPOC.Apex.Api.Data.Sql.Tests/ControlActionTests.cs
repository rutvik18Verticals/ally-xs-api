using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Data.Sql.Tests
{
    [TestClass]
    public class ControlActionTests
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
        public void ConstructorNullContextFactoryTest()
        {
            new ControlActionSQLStore(null, _loggerFactory.Object);
        }

        [TestMethod]
        public void GetControlActionsAsyncTest()
        {
            var mockContextFactory = GetMockContextFactory(
                GetControlActions(),
                GetPOCTypeActions(),
                GetNodes(),
                out _,
                out _,
                out _,
                out _);

            var service = new ControlActionSQLStore(mockContextFactory.Object, _loggerFactory.Object);

            var assetGUID = Guid.Parse("ECD21AED-4115-4188-97AF-D0A17EFD1FD1");

            var result = service.GetControlActions(assetGUID, string.Empty);

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual(2, result[1].Id);
            Assert.AreEqual(3, result[2].Id);
            Assert.AreEqual(4, result[3].Id);
        }

        [TestMethod]
        public void GetControlActionsAsyncWhenControlActionIsSharedByManyPOCTypesTest()
        {
            var mockContextFactory = GetMockContextFactory(
                GetControlActions(),
                GetPOCTypeActions(),
                GetNodes(),
                out _,
                out _,
                out _,
                out _);

            var service = new ControlActionSQLStore(mockContextFactory.Object, _loggerFactory.Object);

            var assetGUID = Guid.Parse("ECD21AED-4115-4188-97AF-D0A17EFD1FD2");

            var result = service.GetControlActions(assetGUID, string.Empty);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(4, result[0].Id);
        }

        [TestMethod]
        public void GetControlActionsAsyncWhenAssetGuidDoesNotExistReturnsEmptyListTest()
        {
            var mockContextFactory = GetMockContextFactory(
                GetControlActions(),
                GetPOCTypeActions(),
                GetNodes(),
                out _,
                out _,
                out _,
                out _);

            var service = new ControlActionSQLStore(mockContextFactory.Object, _loggerFactory.Object);

            var assetGUID = Guid.Parse("ECD21AED-4115-4188-97AF-D0A17EFD1FD3");

            var result = service.GetControlActions(assetGUID, string.Empty);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        #endregion

        #region Private Methods

        private IList<ControlActionsEntity> GetControlActions()
        {
            return new List<ControlActionsEntity>()
            {
                new ControlActionsEntity()
                {
                    ControlActionId = 1,
                    Description = "Test 1",
                },
                new ControlActionsEntity()
                {
                    ControlActionId = 2,
                    Description = "Test 2",
                },
                new ControlActionsEntity()
                {
                    ControlActionId = 3,
                    Description = "Test 3",
                },
                new ControlActionsEntity()
                {
                    ControlActionId = 4,
                    Description = "Test 4",
                },
                new ControlActionsEntity()
                {
                    ControlActionId = 5,
                    Description = "Test 5",
                },
            };
        }

        private IList<POCTypeActionsEntity> GetPOCTypeActions()
        {
            return new List<POCTypeActionsEntity>()
            {
                new POCTypeActionsEntity()
                {
                    ControlActionId = 1,
                    POCType = 8,
                },
                new POCTypeActionsEntity()
                {
                    ControlActionId = 2,
                    POCType = 8,
                },
                new POCTypeActionsEntity()
                {
                    ControlActionId = 3,
                    POCType = 8,
                },
                new POCTypeActionsEntity()
                {
                    ControlActionId = 4,
                    POCType = 8,
                },
                new POCTypeActionsEntity()
                {
                    ControlActionId = 4,
                    POCType = 9,
                },
            };
        }

        private IList<NodeMasterEntity> GetNodes()
        {
            return new List<NodeMasterEntity>()
            {
                new NodeMasterEntity()
                {
                    NodeId = "Theta Sam",
                    AssetGuid = Guid.Parse("ECD21AED-4115-4188-97AF-D0A17EFD1FD1"),
                    PocType = 8,
                },
                new NodeMasterEntity()
                {
                    NodeId = "Theta Sam 2",
                    AssetGuid = Guid.Parse("ECD21AED-4115-4188-97AF-D0A17EFD1FD2"),
                    PocType = 9,
                },
                new NodeMasterEntity()
                {
                    NodeId = "Theta Sam 2",
                    AssetGuid = Guid.Parse("ECD21AED-4115-4188-97AF-D0A17EFD1FD3"),
                    PocType = 10,
                },
            };
        }

        private Mock<IThetaDbContextFactory<NoLockXspocDbContext>> GetMockContextFactory(
            IList<ControlActionsEntity> controlActions,
            IList<POCTypeActionsEntity> pocTypeActions,
            IList<NodeMasterEntity> nodes,
            out Mock<DbSet<ControlActionsEntity>> mockControlActionsDbSet,
            out Mock<DbSet<POCTypeActionsEntity>> mockPOCTypeActionsDbSet,
            out Mock<DbSet<NodeMasterEntity>> mockNodesDbSet,
            out Mock<NoLockXspocDbContext> mockContext)
        {
            mockControlActionsDbSet = SetupDbSet(controlActions.AsQueryable());
            mockPOCTypeActionsDbSet = SetupDbSet(pocTypeActions.AsQueryable());
            mockNodesDbSet = SetupDbSet(nodes.AsQueryable());

            mockContext = SetupMockContext();
            mockContext.Setup(x => x.ControlActions).Returns(mockControlActionsDbSet.Object);
            mockContext.Setup(x => x.POCTypeActions).Returns(mockPOCTypeActionsDbSet.Object);
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodesDbSet.Object);

            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            return contextFactory;
        }

        private Mock<DbSet<T>> SetupDbSet<T>(IQueryable<T> data) where T : class
        {
            var mockDbSet = new Mock<DbSet<T>>();
            mockDbSet.As<IQueryable<T>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<T>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

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
