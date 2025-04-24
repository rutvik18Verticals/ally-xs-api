using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Data.Sql.Tests
{
    [TestClass]
    public class ParameterDataTypesTests
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
            new ParameterDataTypeSQLStore(null, _loggerFactory.Object);
        }

        [TestMethod]
        public void GetParametersDataTypesTest()
        {
            var assetId = Guid.Parse("c2a2d069-e6f9-4a6e-b43c-7e9e5e03842b");
            var addresses = new List<int>() { 10311, 10312, 10313 };

            var contextFactory = GetMockContextFactory(GetNodes(), GetParameters(), out _, out _, out _);

            var store = new ParameterDataTypeSQLStore(contextFactory.Object, _loggerFactory.Object);

            var result = store.GetParametersDataTypes(assetId, addresses, string.Empty);

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.ContainsKey(10311));
            Assert.AreEqual((short)1, result[10311]);
            Assert.IsTrue(result.ContainsKey(10312));
            Assert.AreEqual((short)2, result[10312]);
        }

        [TestMethod]
        public void GetParametersDataTypesNoDataTest()
        {
            var assetId = Guid.Parse("c2a2d169-e6f9-4a6e-b43c-7e9e5e03842b");
            var addresses = new List<int>() { 10311, 10312, 10313 };

            var contextFactory = GetMockContextFactory(GetNodes(), GetParameters(), out _, out _, out _);

            var store = new ParameterDataTypeSQLStore(contextFactory.Object, _loggerFactory.Object);

            var result = store.GetParametersDataTypes(assetId, addresses, It.IsAny<string>());

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        #endregion

        #region Private Methods

        private IList<NodeMasterEntity> GetNodes()
        {
            return new List<NodeMasterEntity>()
            {
                new NodeMasterEntity()
                {
                    NodeId = "theta sam",
                    PocType = 8,
                    PortId = 32,
                    AssetGuid = Guid.Parse("c2a2d069-e6f9-4a6e-b43c-7e9e5e03842b")
                },
                new NodeMasterEntity()
                {
                    NodeId = "theta sam 2",
                    PocType = 9,
                    PortId = 31,
                    AssetGuid = Guid.Parse("c3b3d069-e6f9-4a6e-b43c-7e9e5e03842b")
                },
                new NodeMasterEntity()
                {
                    NodeId = "theta sam 3",
                    PocType = 10,
                    PortId = 31,
                    AssetGuid = Guid.Parse("c3b3d169-e6f9-4a6e-b43c-7e9e5e03842b")
                },
            };
        }

        private IList<ParameterEntity> GetParameters()
        {
            return new List<ParameterEntity>()
            {
                new ParameterEntity()
                {
                    Address = 10311,
                    DataType = 1,
                    Poctype = 8,
                },
                new ParameterEntity()
                {
                    Address = 10312,
                    DataType = 2,
                    Poctype = 8,
                },
                new ParameterEntity()
                {
                    Address = 10313,
                    DataType = 3,
                    Poctype = 9,
                }
            };
        }

        private Mock<IThetaDbContextFactory<NoLockXspocDbContext>> GetMockContextFactory(
            IList<NodeMasterEntity> nodes,
            IList<ParameterEntity> parameters,
            out Mock<DbSet<NodeMasterEntity>> mockNodeMasterDbSet,
            out Mock<DbSet<ParameterEntity>> mockParametersDbSet,
            out Mock<NoLockXspocDbContext> mockContext
            )
        {
            mockNodeMasterDbSet = SetupDbSet(nodes.AsQueryable());
            mockParametersDbSet = SetupDbSet(parameters.AsQueryable());

            mockContext = SetupMockContext();
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContext.Setup(x => x.Parameters).Returns(mockParametersDbSet.Object);

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
