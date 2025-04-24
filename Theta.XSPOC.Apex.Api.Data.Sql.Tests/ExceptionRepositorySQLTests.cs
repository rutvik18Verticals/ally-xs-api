using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Data.Sql.Tests
{
    [TestClass]
    public class ExceptionRepositorySQLTests : DataStoreTestBase
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullTest()
        {
            _ = new ExceptionSQLStore(null, null);
        }

        [TestMethod]
        public async Task EmptyAssetIdTest()
        {
            var dbContext = SetupMockContext();
            var mockFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(dbContext.Object);
            var correlationId = Guid.NewGuid().ToString();
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var repo = new ExceptionSQLStore(mockFactory.Object, mockThetaLoggerFactory.Object);

            var result = await repo.GetAssetStatusExceptionsAsync(Guid.Empty, Guid.NewGuid(), correlationId);

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAssetStatusExceptionsAsyncTest()
        {
            var nodeMasterData = GetNodeMasterData();
            var exceptionData = GetExceptionData();

            var mockNodeMasterDbSet = SetupMockDbSet(nodeMasterData);
            var mockCardDataDbSet = SetupMockDbSet(exceptionData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContext.Setup(x => x.Exceptions).Returns(mockCardDataDbSet.Object);

            var correlationId = Guid.NewGuid().ToString();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var assetId = Guid.Parse("E13E3F8A-5D32-4670-A5C1-94993BD311CA");

            var repo = new ExceptionSQLStore(mockFactory.Object, mockThetaLoggerFactory.Object);

            var result = await repo.GetAssetStatusExceptionsAsync(assetId, Guid.NewGuid(), correlationId);

            Assert.IsNotNull(result);

            Assert.AreEqual(1, result.Count);

            Assert.AreEqual("Group 2", result[0].Description);
            Assert.AreEqual(2, result[0].Priority);
        }

        #endregion

        #region Private Data Setup Methods

        private IQueryable<NodeMasterEntity> GetNodeMasterData()
        {
            return new List<NodeMasterEntity>()
            {
                new NodeMasterEntity()
                {
                    AssetGuid = Guid.Parse("12E97D89-FD91-46A2-8A90-99ECB9E4E26E"),
                    NodeId = "Test 1",
                },
                new NodeMasterEntity()
                {
                    AssetGuid = Guid.Parse("E13E3F8A-5D32-4670-A5C1-94993BD311CA"),
                    NodeId = "Test 2",
                },
            }.AsQueryable();
        }

        private IQueryable<ExceptionEntity> GetExceptionData()
        {
            return new List<ExceptionEntity>()
            {
                new ExceptionEntity()
                {
                    NodeId = "Test 1",
                    Priority = 1,
                    ExceptionGroupName = "Group 1",
                },
                new ExceptionEntity()
                {
                    NodeId = "Test 2",
                    Priority = 2,
                    ExceptionGroupName = "Group 2",
                },
            }.AsQueryable();
        }

        #endregion

    }
}