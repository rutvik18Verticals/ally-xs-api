using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Data.Sql.Tests
{
    [TestClass]
    public class NotificationTests : DataStoreTestBase
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
            _ = new NotificationSQLStore(null, _loggerFactory.Object);
        }

        [TestMethod]
        public void NotificationGetEventsByAssetIdTest()
        {
            var data = GetEventsData().AsQueryable();
            var eventGroups = GetEventGroupsData().AsQueryable();
            var groupMembershipCacheData = GetGroupMembershipCacheData().AsQueryable();
            var nodeMasterData = GetNodeMasterData().AsQueryable();

            var mockEventDbSet = SetupMockDbSet(data);
            var mockEventGroupsDbSet = SetupMockDbSet(eventGroups);
            var mockGroupMembershipCacheDbSet = SetupMockDbSet(groupMembershipCacheData);
            var mockNodeMasterDbSet = SetupMockDbSet(nodeMasterData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.Events).Returns(mockEventDbSet.Object);
            mockContext.Setup(x => x.GroupMembershipCache).Returns(mockGroupMembershipCacheDbSet.Object);
            mockContext.Setup(x => x.EventGroups).Returns(mockEventGroupsDbSet.Object);
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);

            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var notification = new NotificationSQLStore(contextFactory.Object, _loggerFactory.Object);

            var resultByAssetIdNullNotificationTypeId =
                notification.GetEventsByAssetId("DFC1D0AD-A824-4965-B78D-AB7755E32DD3", null, It.IsAny<string>());
            Assert.IsNotNull(resultByAssetIdNullNotificationTypeId);
            Assert.AreEqual(8, resultByAssetIdNullNotificationTypeId.Count);

            var resultByAssetId = notification.GetEventsByAssetId("DFC1D0AD-A824-4965-B78D-AB7755E32DD3", 0, It.IsAny<string>());
            Assert.IsNotNull(resultByAssetId);
            Assert.AreEqual(4, resultByAssetId.Count);

            var resultByAssetIdAndNotificationTypeId = notification.GetEventsByAssetId("DFC1D0AD-A824-4965-B78D-AB7755E32DD3", 1, It.IsAny<string>());
            Assert.IsNotNull(resultByAssetIdAndNotificationTypeId);
            Assert.AreEqual(4, resultByAssetIdAndNotificationTypeId.Count);
        }

        [TestMethod]
        public void NotificationGetEventsByGroupNameTest()
        {
            var data = GetEventsData().AsQueryable();
            var eventGroups = GetEventGroupsData().AsQueryable();
            var groupMembershipCacheData = GetGroupMembershipCacheData().AsQueryable();

            var mockEventDbSet = SetupMockDbSet(data);
            var mockEventGroupsDbSet = SetupMockDbSet(eventGroups);
            var mockGroupMembershipCacheDbSet = SetupMockDbSet(groupMembershipCacheData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.Events).Returns(mockEventDbSet.Object);
            mockContext.Setup(x => x.GroupMembershipCache).Returns(mockGroupMembershipCacheDbSet.Object);
            mockContext.Setup(x => x.EventGroups).Returns(mockEventGroupsDbSet.Object);
            mockContext.Setup(x => x.NodeMasters).Returns(SetupMockDbSet(GetNodeMasterData().AsQueryable()).Object);

            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var notification = new NotificationSQLStore(contextFactory.Object, _loggerFactory.Object);

            var result = notification.GetEventsByAssetGroupName("<100 Intake Pressure", null, It.IsAny<string>());
            Assert.IsNotNull(result);
            Assert.AreEqual(8, result.Count);

            result = notification.GetEventsByAssetGroupName("<100 Intake Pressure", 0, It.IsAny<string>());
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);

            result = notification.GetEventsByAssetGroupName("> 300 Days Run Time", null, It.IsAny<string>());
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);

            result = notification.GetEventsByAssetGroupName("> 300 Days Run Time", 1, It.IsAny<string>());
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void NotificationTypesGetEventsByAssetIdTest()
        {
            var eventGroups = GetEventGroupsData().AsQueryable();
            var mockeventGroupsDbSet = SetupEventGroups(eventGroups);

            var events = GetEventsData().AsQueryable();
            var mockeventDbSet = SetupEvents(events);

            var nodeMasterData = GetNodeMasterData().AsQueryable();
            var mocknodeMaster = SetupNodeMaster(nodeMasterData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.EventGroups).Returns(mockeventGroupsDbSet.Object);
            mockContext.Setup(x => x.Events).Returns(mockeventDbSet.Object);
            mockContext.Setup(x => x.NodeMasters).Returns(mocknodeMaster.Object);

            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var notification = new NotificationSQLStore(contextFactory.Object, _loggerFactory.Object);

            var resultByAssetId = notification.GetEventsGroupsByAssetId("DFC1D0AD-A824-4965-B78D-AB7755E32DD3", It.IsAny<string>());
            Assert.IsNotNull(resultByAssetId);
            Assert.AreEqual(2, resultByAssetId.Count);

            var resultByAssetIdAndNotificationTypeId =
                notification.GetEventsGroupsByAssetId("DFC1D0AD-A824-4965-B78D-AB7755E32DD3", It.IsAny<string>());
            Assert.IsNotNull(resultByAssetIdAndNotificationTypeId);
            Assert.AreEqual(resultByAssetIdAndNotificationTypeId.Count, GetDistinctNodeCountByAssetId());
        }

        [TestMethod]
        public void NotificationTypesGetEventsByGroupNameTest()
        {
            var eventGroupData = GetEventGroupsData().AsQueryable();

            var groupMembershipCacheData = GetGroupMembershipCacheData().AsQueryable();

            var events = GetEventsData().AsQueryable();

            var mockEventGroupDbSet = SetupEventGroups(eventGroupData);
            var mockGroupMembershipCacheDbSet = SetupGroupMembershipCache(groupMembershipCacheData);
            var mockeventDbSet = SetupEvents(events);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.EventGroups).Returns(mockEventGroupDbSet.Object);
            mockContext.Setup(x => x.GroupMembershipCache).Returns(mockGroupMembershipCacheDbSet.Object);
            mockContext.Setup(x => x.Events).Returns(mockeventDbSet.Object);

            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var notification = new NotificationSQLStore(contextFactory.Object, _loggerFactory.Object);

            var result = notification.GetEventsGroupsByAssetGroupName("<100 Intake Pressure", It.IsAny<string>());
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);

            var resultByGroupName = notification.GetEventsGroupsByAssetGroupName("<100 Intake Pressure", It.IsAny<string>());
            Assert.IsNotNull(resultByGroupName);
            Assert.AreEqual(resultByGroupName.Count, GetDistinctNodeCountByGroupName());
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
                    NodeId = "AssetId1",
                    Tzdaylight = false,
                    Tzoffset = 0,
                },
                new NodeMasterEntity()
                {
                    AssetGuid = new Guid("87A038CA-E0FD-4F60-A3A7-4A3B482D6FE1"),
                    NodeId = "AssetId2",
                    Tzdaylight = false,
                    Tzoffset = 1,
                },
            };

            return nodeMasters;
        }

        private IList<EventsEntity> GetEventsData()
        {
            var eventData = new List<EventsEntity>()
            {
                new EventsEntity()
                {
                    EventId = 101,
                    NodeId = "AssetId1",
                    EventTypeId = 1,
                    Date = DateTime.UtcNow,
                    Status = "abc",
                    Note = "Runtime, Limit= Clear,Unack",
                    UserId = "",
                    TransactionId = 1111,
                },
                new EventsEntity()
                {
                    EventId = 102,
                    NodeId = "AssetId2",
                    EventTypeId = 1,
                    Date = DateTime.UtcNow,
                    Status = "abc",
                    Note = "Idle, Lo Load, Clear",
                    UserId = "",
                    TransactionId = 1112,
                },
                new EventsEntity()
                {
                    EventId = 103,
                    NodeId = "AssetId1",
                    EventTypeId = 1,
                    Date = DateTime.UtcNow,
                    Status = "abc",
                    Note = "VFD Hi Load, Set",
                    UserId = "",
                    TransactionId = 1113,
                },
                new EventsEntity()
                {
                    EventId = 104,
                    NodeId = "AssetId2",
                    EventTypeId = 2,
                    Date = DateTime.UtcNow,
                    Status = "abc",
                    Note = "Startup",
                    UserId = "",
                    TransactionId = 1114,
                },
                new EventsEntity()
                {
                    EventId = 105,
                    NodeId = "AssetId1",
                    EventTypeId = 0,
                    Date = DateTime.UtcNow,
                    Status = "abc",
                    Note = "Startup",
                    UserId = "",
                    TransactionId = 1115,
                },
                new EventsEntity()
                {
                    EventId = 106,
                    NodeId = "AssetId1",
                    EventTypeId = 0,
                    Date = DateTime.UtcNow,
                    Status = "abc",
                    Note = "Runtime, Limit= Clear,Unack",
                    UserId = "",
                    TransactionId = 1116,
                },
            };

            return eventData;
        }

        private IList<EventGroupsEntity> GetEventGroupsData()
        {
            var eventData = new List<EventGroupsEntity>()
            {
                new EventGroupsEntity()
                {
                    EventTypeId = 1,
                    Name = "Comment",
                    PhraseId = 918,
                    AllowUserCreation = true,
                },
                new EventGroupsEntity()
                {
                    EventTypeId = 0,
                    Name = "Param Change",
                    PhraseId = 504,
                    AllowUserCreation = true,
                },
                new EventGroupsEntity()
                {
                    EventTypeId = 0,
                    Name = "Comment",
                    PhraseId = 919,
                    AllowUserCreation = true,
                },
                new EventGroupsEntity()
                {
                    EventTypeId = 1,
                    Name = "Host Alarm",
                    PhraseId = 920,
                    AllowUserCreation = true,
                }
            };

            return eventData;
        }

        private IList<GroupMembershipCacheEntity> GetGroupMembershipCacheData()
        {
            var groupMembershipCacheData = new List<GroupMembershipCacheEntity>()
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

            return groupMembershipCacheData;
        }

        private int GetDistinctNodeCountByAssetId()
        {
            var nodeMastersData = GetNodeMasterData();
            var eventGroupsData = GetEventGroupsData();
            var eventsData = GetEventsData();

            var distinctTypeCount = (from e in eventsData
                                     join eg in eventGroupsData on e.EventTypeId equals eg.EventTypeId
                                     join nm in nodeMastersData on e.NodeId equals nm.NodeId
                                     where nm.AssetGuid == new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                                     group e by e.EventTypeId
                                     into distinctNodes
                                     select distinctNodes).Count();

            return distinctTypeCount;
        }

        private int GetDistinctNodeCountByGroupName()
        {
            var eventGroupsData = GetEventGroupsData();
            var eventsData = GetEventsData();
            var groupMembershipCache = GetGroupMembershipCacheData();
            string assetGroupName = "<100 Intake Pressure";

            var distinctTypeCount = groupMembershipCache
                .Where(a => a.GroupName == assetGroupName)
                .Join(eventsData, g => g.NodeId, e => e.NodeId, (g, e) => e.EventTypeId)
                .Distinct()
                .Join(eventGroupsData, et => et, eg => eg.EventTypeId, (et, eg) => eg)
                .DistinctBy(eg => eg.EventTypeId)
                .Count();

            return distinctTypeCount;
        }

        #endregion

        #region Private Setup Methods

        private Mock<DbSet<EventsEntity>> SetupEvents(IQueryable<EventsEntity> data)
        {
            var mockDbSet = new Mock<DbSet<EventsEntity>>();
            mockDbSet.As<IQueryable<EventsEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<EventsEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<EventsEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<EventsEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet;
        }

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

        private Mock<DbSet<EventGroupsEntity>> SetupEventGroups(IQueryable<EventGroupsEntity> data)
        {
            var mockDbSet = new Mock<DbSet<EventGroupsEntity>>();
            mockDbSet.As<IQueryable<EventGroupsEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<EventGroupsEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<EventGroupsEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<EventGroupsEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet;
        }

        private void SetupThetaLoggerFactory()
        {
            _loggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(_logger.Object);
        }

        #endregion

    }
}
