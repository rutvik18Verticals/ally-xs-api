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
    public class GroupAndAssetTests
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
            _ = new GroupAndAssetSQLStore(null, _loggerFactory.Object);
        }

        [TestMethod]
        public void GroupAndAssetGetTest()
        {
            var groupMembershipCacheData = GetGroupMembershipCacheData().AsQueryable();
            var nodeMasterData = GetNodeMasterData().AsQueryable();
            var nodeTreeData = GetNodeTreeData().AsQueryable();

            var mockGroupMembershipCacheDbSet = SetupGroupMembershipCache(groupMembershipCacheData);
            var mockNodeTreeDbSet = SetupNodeTree(nodeTreeData);
            var mockNodeMasterDbSet = SetupNodeMaster(nodeMasterData);
            var correlationId = Guid.NewGuid().ToString();

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.GroupMembershipCache).Returns(mockGroupMembershipCacheDbSet.Object);
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContext.Setup(x => x.NodeTree).Returns(mockNodeTreeDbSet.Object);

            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var groupAndAsset = new GroupAndAssetSQLStore(contextFactory.Object, _loggerFactory.Object);

            var result = groupAndAsset.GetGroupAssetAndRelationshipData(correlationId);

            Assert.IsNotNull(result);

            Assert.AreEqual(2, result.ChildGroups.Count);
            Assert.AreEqual(1, result.ChildGroups[0].Assets.Count);
            Assert.AreEqual("AssetId1", result.ChildGroups[0].Assets[0].AssetName);

            Assert.IsNull(result.ChildGroups[1].Assets);
        }

        #endregion

        #region Private Methods

        private IList<NodeMasterEntity> GetNodeMasterData()
        {
            return new List<NodeMasterEntity>()
            {
                new NodeMasterEntity()
                {
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                    NodeId = "AssetId1",
                    ApplicationId = 1,
                },
                new NodeMasterEntity()
                {
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD2"),
                    NodeId = "AssetId2",
                    ApplicationId = 1,
                },
            };
        }

        private IList<GroupMembershipCacheEntity> GetGroupMembershipCacheData()
        {
            return new List<GroupMembershipCacheEntity>()
            {
                new GroupMembershipCacheEntity()
                {
                    GroupName = "<100 Intake Pressure",
                    NodeId = "AssetId1",
                },
                new GroupMembershipCacheEntity()
                {
                    GroupName = "> 300 Days Run Time",
                    NodeId = "AssetId2",
                },
            };
        }

        private IList<NodeTreeEntity> GetNodeTreeData()
        {
            return new List<NodeTreeEntity>()
            {
                new NodeTreeEntity()
                {
                    Id = 1,
                    Node = "AssetId1",
                    Type = 2,
                    Parent = "<100 Intake Pressure",
                    NumDescendants = 0
                },
                new NodeTreeEntity()
                {
                    Id = 2,
                    Node = "AssetId1",
                    Type = 2,
                    Parent = "> 300 Days Run Time",
                    NumDescendants = 0
                },
                new NodeTreeEntity()
                {
                    Id = 3,
                    Node = "<100 Intake Pressure",
                    Type = 1,
                    Parent = "root",
                    NumDescendants = 0
                },
                new NodeTreeEntity()
                {
                    Id = 4,
                    Node = "> 300 Days Run Time",
                    Type = 1,
                    Parent = "root",
                    NumDescendants = 0
                },
            };
        }

        #endregion

        #region Private Setup Methods

        private Mock<DbSet<GroupMembershipCacheEntity>> SetupGroupMembershipCache(IQueryable<GroupMembershipCacheEntity> data)
        {
            var mockDbSet = new Mock<DbSet<GroupMembershipCacheEntity>>();
            mockDbSet.As<IQueryable<GroupMembershipCacheEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<GroupMembershipCacheEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<GroupMembershipCacheEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<GroupMembershipCacheEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet;
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

        private Mock<DbSet<NodeTreeEntity>> SetupNodeTree(IQueryable<NodeTreeEntity> data)
        {
            var mockDbSet = new Mock<DbSet<NodeTreeEntity>>();
            mockDbSet.As<IQueryable<NodeTreeEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<NodeTreeEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<NodeTreeEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<NodeTreeEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

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
