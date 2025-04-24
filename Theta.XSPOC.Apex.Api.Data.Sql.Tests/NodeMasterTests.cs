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
    public class NodeMasterTests
    {

        #region Private Fields

        private Mock<IThetaLoggerFactory> _loggerFactory;
        private Mock<IThetaLogger> _logger;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _loggerFactory = new Mock<IThetaLoggerFactory>();
            _logger = new Mock<IThetaLogger>();

            SetupThetaLoggerFactory();
        }

        #region Test Methods

        #region Constructor Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullContextFactoryTest()
        {
            new NodeMastersSQLStore(null, _loggerFactory.Object);
        }

        #endregion

        #region TryGetPortIdByAssetGUID Tests

        [TestMethod]
        public void TryGetPortIdByAssetGUIDTest()
        {
            var assetId = Guid.Parse("c2a2d069-e6f9-4a6e-b43c-7e9e5e03842b");

            var nodes = GetNodes();
            var contextFactory = GetMockContextFactory(nodes, out _, out _);

            var store = new NodeMastersSQLStore(contextFactory.Object, _loggerFactory.Object);

            var result = store.TryGetPortIdByAssetGUID(assetId, out var portId, string.Empty);

            Assert.IsTrue(result);
            Assert.AreEqual(32, portId);
        }

        [TestMethod]
        public void TryGetPortIdByAssetGUIDNodeDoesNotExistTest()
        {
            var assetId = Guid.NewGuid();

            var nodes = new List<NodeMasterEntity>();

            var contextFactory = GetMockContextFactory(nodes, out _, out _);

            var store = new NodeMastersSQLStore(contextFactory.Object, _loggerFactory.Object);

            var result = store.TryGetPortIdByAssetGUID(assetId, out _, string.Empty);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void TryGetPortIdByAssetGUIDPortIdIsEmptyTest()
        {
            var assetId = Guid.NewGuid();

            var nodes = new List<NodeMasterEntity>()
            {
                new NodeMasterEntity()
                {
                    NodeId = "theta sam",
                    PortId = null,
                    AssetGuid = assetId,
                }
            };

            var contextFactory = GetMockContextFactory(nodes, out _, out _);

            var store = new NodeMastersSQLStore(contextFactory.Object, _loggerFactory.Object);

            var result = store.TryGetPortIdByAssetGUID(assetId, out _, string.Empty);

            Assert.IsFalse(result);
        }

        #endregion

        #region TryGetPortIdByNodeId Tests

        [TestMethod]
        public void TryGetPocTypeIdByAssetGUIDTest()
        {
            var assetId = Guid.Parse("c2a2d069-e6f9-4a6e-b43c-7e9e5e03842b");

            var nodes = GetNodes();
            var contextFactory = GetMockContextFactory(nodes, out _, out _);

            var store = new NodeMastersSQLStore(contextFactory.Object, _loggerFactory.Object);

            var result = store.TryGetPocTypeIdByAssetGUID(assetId, out var pocTypeId, string.Empty);

            Assert.IsTrue(result);
            Assert.AreEqual(8, pocTypeId);
        }

        [TestMethod]
        public void TryGetPocTypeIdByAssetGUIDNodeDoesNotExistTest()
        {
            var assetId = Guid.NewGuid();

            var nodes = new List<NodeMasterEntity>();

            var contextFactory = GetMockContextFactory(nodes, out _, out _);

            var store = new NodeMastersSQLStore(contextFactory.Object, _loggerFactory.Object);

            var result = store.TryGetPocTypeIdByAssetGUID(assetId, out _, string.Empty);

            Assert.IsFalse(result);
        }

        #endregion

        #endregion

        #region Private Methods

        private IList<NodeMasterEntity> GetNodes()
        {
            return new List<NodeMasterEntity>()
            {
                new NodeMasterEntity()
                {
                    NodeId = "theta sam",
                    PortId = 32,
                    PocType = 8,
                    AssetGuid = Guid.Parse("c2a2d069-e6f9-4a6e-b43c-7e9e5e03842b")
                },
            };
        }

        private Mock<IThetaDbContextFactory<NoLockXspocDbContext>> GetMockContextFactory(
            IList<NodeMasterEntity> nodes,
            out Mock<DbSet<NodeMasterEntity>> mockNodeMasterDbSet,
            out Mock<NoLockXspocDbContext> mockContext
            )
        {
            var nodeMaster = nodes.AsQueryable();
            mockNodeMasterDbSet = SetupNodeMaster(nodeMaster);

            mockContext = SetupMockContext();
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);

            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            return contextFactory;
        }

        private Mock<DbSet<NodeMasterEntity>> SetupNodeMaster(IQueryable<NodeMasterEntity> data)
        {
            var mockDbSet = new Mock<DbSet<NodeMasterEntity>>();
            mockDbSet.As<IQueryable<NodeMasterEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<NodeMasterEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<NodeMasterEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<NodeMasterEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

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
