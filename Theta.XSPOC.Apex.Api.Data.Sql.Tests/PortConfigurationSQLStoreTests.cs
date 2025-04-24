using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Data.Sql.Tests
{
    [TestClass]
    public class PortConfigurationSQLStoreTests
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

        #region Constructor Tests

        [TestMethod]
        public void ConstructorNullContextFactoryTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new PortConfigurationSQLStore(null, _loggerFactory.Object));
        }

        [TestMethod]
        public void ConstructorTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            var store = new PortConfigurationSQLStore(contextFactory.Object, _loggerFactory.Object);
            Assert.IsNotNull(store);
        }

        #endregion

        #region Test Methods

        [TestMethod]
        public async Task IsLegacyWellAsyncTest()
        {
            short portId = 1;
            var ports = new List<PortMaster>()
                {
                    new PortMaster()
                    {
                        PortId = portId,
                        PortType = 5
                    },
                }.AsQueryable();

            var dbSet = GetMockDbSet(ports);
            var contextFactory = GetMockDbContext(dbSet);

            var store = new PortConfigurationSQLStore(contextFactory.Object, _loggerFactory.Object);
            var result = await store.IsLegacyWellAsync(portId, It.IsAny<string>());
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task IsLegacyWellAsyncPortConfigurationNullTest()
        {
            short portId = 1;
            var ports = new List<PortMaster>()
            {
                    new PortMaster()
                    {
                        PortId = 2,
                        PortType = 5
                    },
                }.AsQueryable();

            var dbSet = GetMockDbSet(ports);
            var contextFactory = GetMockDbContext(dbSet);

            var store = new PortConfigurationSQLStore(contextFactory.Object, _loggerFactory.Object);
            var result = await store.IsLegacyWellAsync(portId, It.IsAny<string>());
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task IsLegacyWellAsyncPortTypeNullTest()
        {
            short portId = 1;
            var ports = new List<PortMaster>()
            {
                    new PortMaster()
                    {
                        PortId = portId,
                        PortType = null
                    },
                }.AsQueryable();

            var dbSet = GetMockDbSet(ports);
            var contextFactory = GetMockDbContext(dbSet);

            var store = new PortConfigurationSQLStore(contextFactory.Object, _loggerFactory.Object);
            var result = await store.IsLegacyWellAsync(portId, It.IsAny<string>());
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task IsLegacyWellAsyncPortTypeLessThanOrEqualTo5Test()
        {
            short portId = 1;
            var ports = new List<PortMaster>()
            {
                    new PortMaster()
                    {
                        PortId = portId,
                        PortType = 2
                    },
                }.AsQueryable();

            var dbSet = GetMockDbSet(ports);
            var contextFactory = GetMockDbContext(dbSet);

            var store = new PortConfigurationSQLStore(contextFactory.Object, _loggerFactory.Object);
            var result = await store.IsLegacyWellAsync(portId, It.IsAny<string>());
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task IsLegacyWellAsyncPortTypeGreaterThan5Test()
        {
            short portId = 1;
            var ports = new List<PortMaster>()
            {
                    new PortMaster()
                    {
                        PortId = portId,
                        PortType = 6
                    },
                }.AsQueryable();

            var dbSet = GetMockDbSet(ports);
            var contextFactory = GetMockDbContext(dbSet);

            var store = new PortConfigurationSQLStore(contextFactory.Object, _loggerFactory.Object);
            var result = await store.IsLegacyWellAsync(portId, It.IsAny<string>());
            Assert.IsFalse(result);
        }

        #endregion

        #region Private Setup Methods

        private Mock<DbSet<T>> GetMockDbSet<T>(IQueryable<T> data) where T : class
        {
            var dbSetMock = new Mock<DbSet<T>>();
            dbSetMock.As<IAsyncEnumerable<T>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<T>(data.GetEnumerator()));

            dbSetMock.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<T>(data.Provider));

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return dbSetMock;
        }

        private Mock<IThetaDbContextFactory<XspocDbContext>> GetMockDbContext(Mock<DbSet<PortMaster>> mockDbSet)
        {

            var contextOptions = new Mock<DbContextOptions<XspocDbContext>>();
            contextOptions.Setup(m => m.ContextType).Returns(typeof(XspocDbContext));
            contextOptions.Setup(m => m.Extensions).Returns(new List<IDbContextOptionsExtension>());

            var mockDateTimeConverter = new Mock<IDateTimeConverter>();

            var dbContextMock = new Mock<XspocDbContext>(mockDateTimeConverter.Object, contextOptions.Object);
            dbContextMock.Setup(x => x.PortConfigurations).Returns(mockDbSet.Object);

            var contextFactoryMock = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            contextFactoryMock.Setup(x => x.GetContext()).Returns(dbContextMock.Object);

            return contextFactoryMock;
        }

        private void SetupThetaLoggerFactory()
        {
            _loggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(_logger.Object);
        }

        #endregion

    }
}
