using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Sql.Logging;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Data.Sql.Tests
{
    [TestClass]
    public class GroupStatusSQLStoreTests
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

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest()
        {
            _ = new GroupStatusSQLStore(null, _loggerFactory.Object);
        }

        [TestMethod]
        public void GetItemsGroupStatusTest()
        {
            var facilityTags = GetFacilitytags().AsQueryable();
            var mockfacilityTagsDbSet = SetupFacilityTags(facilityTags);

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.FacilityTags).Returns(mockfacilityTagsDbSet.Object);

            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var notification = new GroupStatusSQLStore(contextFactory.Object, _loggerFactory.Object);

            string[] nodelist = new string[1]
            {
                "DFC1D0AD-A824-4965-B78D-AB7755E32DD3"
            };

            var resultByAssetId = notification.GetItemsGroupStatus(nodelist, string.Empty);
            Assert.IsNotNull(resultByAssetId);
            Assert.AreEqual(1, resultByAssetId.Count);
        }

        [TestMethod]
        public void LoadViewParametersTest()
        {
            var groupStatusView = GetGroupStatusViewData().AsQueryable();
            var groupStatusViewColumn = GetGroupStatusViewColumnData().AsQueryable();
            var groupStatusColumn = GetGroupStatusColumnData().AsQueryable();
            var parameter = GetParameterData().AsQueryable();

            var mockgroupStatusViewDbSet = SetupGroupStatusView(groupStatusView);
            var mockgroupStatusViewColumnDbSet = SetupGroupStatusViewsColumn(groupStatusViewColumn);
            var mockgroupStatusColumnDbSet = SetupGroupStatusColumn(groupStatusColumn);
            var mockParameterDbSet = SetupParameter(parameter);

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.GroupStatusView.AsQueryable()).Returns(groupStatusView);
            mockContext.Setup(x => x.GroupStatusView).Returns(mockgroupStatusViewDbSet.Object);
            mockContext.Setup(x => x.GroupStatusViewsColumns).Returns(mockgroupStatusViewColumnDbSet.Object);
            mockContext.Setup(x => x.GroupStatusColumns).Returns(mockgroupStatusColumnDbSet.Object);
            mockContext.Setup(x => x.Parameters).Returns(mockParameterDbSet.Object);

            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);
            var notification = new GroupStatusSQLStore(contextFactory.Object, _loggerFactory.Object);

            var resultByAssetId = notification.LoadViewParameters("1", string.Empty);
            Assert.IsNotNull(resultByAssetId);
            Assert.AreEqual(1, resultByAssetId.Count);
        }

        [TestMethod]
        public void AvailableViewTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.GroupStatusUserView)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGroupStatusUserViewData().AsQueryable()).Object);
            mockContext.Setup(x => x.GroupStatusView)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGroupStatusViewData().AsQueryable()).Object);
            mockContext.Setup(x => x.UserDefaults)
               .Returns(TestUtilities.SetupMockData(TestUtilities.GetUserDefaults().AsQueryable()).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var availableView = new GroupStatusSQLStore(contextFactory.Object, _loggerFactory.Object);

            var response = availableView.GetAvailableViewsByUserId("Global", string.Empty);
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public void GetConditionalFormatsTest()
        {
            // Arrange
            var viewId = "1";
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.GroupStatusUserView)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGroupStatusUserViewData().AsQueryable()).Object);
            mockContext.Setup(x => x.GroupStatusView)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGroupStatusViewData().AsQueryable()).Object);

            var groupStatusViewsColumns = new List<GroupStatusViewsColumnEntity>
            {
                new GroupStatusViewsColumnEntity
                {
                    ViewId = 1,
                    ColumnId = 2,
                    Width = 3,
                    Position = 4,
                    Orientation = 5,
                    FormatId = 6
                }
            }.AsQueryable();

            mockContext.Setup(x => x.GroupStatusViewsColumns)
                .Returns(TestUtilities.SetupMockData(groupStatusViewsColumns).Object);

            var conditionalFormats = new List<ConditionalFormatEntity>
            {
                new ConditionalFormatEntity
                {
                    Id = 1,
                    ColumnId = 1
                },
                new ConditionalFormatEntity
                {
                    Id = 2,
                    ColumnId = 2
                }
            };

            mockContext.Setup(x => x.ConditionalFormats)
                .Returns(TestUtilities.SetupMockData(conditionalFormats.AsQueryable()).Object);
            contextFactory.Setup(cf => cf.GetContext()).Returns(mockContext.Object);

            // Act
            var store = new GroupStatusSQLStore(contextFactory.Object, _loggerFactory.Object);
            var result = store.GetConditionalFormats(viewId, string.Empty);

            // Assert
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void GetConditionalFormatsInvalidViewIdTest()
        {
            // Arrange
            var viewId = "invalid";
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockContext = TestUtilities.SetupMockContext();
            contextFactory.Setup(cf => cf.GetContext()).Returns(mockContext.Object);

            // Act
            var store = new GroupStatusSQLStore(contextFactory.Object, _loggerFactory.Object);
            var result = store.GetConditionalFormats(viewId, string.Empty);

            // Assert
            Assert.IsInstanceOfType(result, typeof(List<ConditionalFormatModel>));
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void LoadViewColumnsNoDataTest()
        {
            var viewId = "1";
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.GroupStatusView)
                .Returns(TestUtilities.SetupMockData(new List<GroupStatusViewEntity>().AsQueryable()).Object);
            mockContext.Setup(x => x.GroupStatusViewsColumns)
                .Returns(TestUtilities.SetupMockData(new List<GroupStatusViewsColumnEntity>().AsQueryable()).Object);
            mockContext.Setup(x => x.GroupStatusColumns)
                .Returns(TestUtilities.SetupMockData(new List<GroupStatusColumnEntity>().AsQueryable()).Object);
            mockContext.Setup(x => x.GroupStatusColumnFormats)
                .Returns(TestUtilities.SetupMockData(new List<GroupStatusColumnFormatEntity>().AsQueryable()).Object);
            mockContext.Setup(x => x.ParamStandardTypes)
                .Returns(TestUtilities.SetupMockData(new List<ParamStandardTypesEntity>().AsQueryable()).Object);

            contextFactory.Setup(cf => cf.GetContext()).Returns(mockContext.Object);

            var store = new GroupStatusSQLStore(contextFactory.Object, _loggerFactory.Object);
            var result = store.LoadViewColumns(viewId, string.Empty);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void BuildSQLCommonResultSqlConnectionNullTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.GroupStatusView)
                .Returns(TestUtilities.SetupMockData(new List<GroupStatusViewEntity>().AsQueryable()).Object);
            mockContext.Setup(x => x.GroupStatusViewsColumns)
                .Returns(TestUtilities.SetupMockData(new List<GroupStatusViewsColumnEntity>().AsQueryable()).Object);
            mockContext.Setup(x => x.GroupStatusColumns)
                .Returns(TestUtilities.SetupMockData(new List<GroupStatusColumnEntity>().AsQueryable()).Object);
            mockContext.Setup(x => x.GroupStatusColumnFormats)
                .Returns(TestUtilities.SetupMockData(new List<GroupStatusColumnFormatEntity>().AsQueryable()).Object);
            mockContext.Setup(x => x.ParamStandardTypes)
                .Returns(TestUtilities.SetupMockData(new List<ParamStandardTypesEntity>().AsQueryable()).Object);

            contextFactory.Setup(cf => cf.GetContext()).Returns(mockContext.Object);

            var store = new GroupStatusSQLStore(contextFactory.Object, _loggerFactory.Object);
            var result = store.BuildSQLCommonResult(new List<string>(), true, true, true, string.Empty, new List<string>(), string.Empty);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void BuildSQLParameterResultNoDataTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(new List<NodeMasterEntity>().AsQueryable()).Object);
            mockContext.Setup(x => x.Parameters)
                .Returns(TestUtilities.SetupMockData(new List<ParameterEntity>().AsQueryable()).Object);
            mockContext.Setup(x => x.CurrentRawScans)
                .Returns(TestUtilities.SetupMockData(new List<CurrentRawScanDataEntity>().AsQueryable()).Object);
            mockContext.Setup(x => x.States).Returns(TestUtilities.SetupMockData(new List<StatesEntity>().AsQueryable()).Object);

            contextFactory.Setup(cf => cf.GetContext()).Returns(mockContext.Object);

            var store = new GroupStatusSQLStore(contextFactory.Object, _loggerFactory.Object);
            var result = store.BuildSQLParameterResult(new List<string>(), string.Empty);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void BuildSQLCurrRawScanDataNoDataTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(new List<NodeMasterEntity>().AsQueryable()).Object);
            mockContext.Setup(x => x.Parameters)
                .Returns(TestUtilities.SetupMockData(new List<ParameterEntity>().AsQueryable()).Object);
            mockContext.Setup(x => x.CurrentRawScans)
                .Returns(TestUtilities.SetupMockData(new List<CurrentRawScanDataEntity>().AsQueryable()).Object);

            contextFactory.Setup(cf => cf.GetContext()).Returns(mockContext.Object);

            var store = new GroupStatusSQLStore(contextFactory.Object, _loggerFactory.Object);
            var result = store.BuildSQLCurrRawScanData(new List<string>(), string.Empty);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void BuildSQLFacilityResultNoDataTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(new List<NodeMasterEntity>().AsQueryable()).Object);
            mockContext.Setup(x => x.FacilityTags)
                .Returns(TestUtilities.SetupMockData(new List<FacilityTagsEntity>().AsQueryable()).Object);
            mockContext.Setup(x => x.States).Returns(TestUtilities.SetupMockData(new List<StatesEntity>().AsQueryable()).Object);

            contextFactory.Setup(cf => cf.GetContext()).Returns(mockContext.Object);

            var store = new GroupStatusSQLStore(contextFactory.Object, _loggerFactory.Object);
            var result = store.BuildSQLFacilityResult(new List<string>(), string.Empty);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetFacilityParamStandardTypesNoDataTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.ParamStandardTypes)
                .Returns(TestUtilities.SetupMockData(new List<ParamStandardTypesEntity>().AsQueryable()).Object);
            mockContext.Setup(x => x.FacilityTags)
                .Returns(TestUtilities.SetupMockData(new List<FacilityTagsEntity>().AsQueryable()).Object);

            contextFactory.Setup(cf => cf.GetContext()).Returns(mockContext.Object);

            var store = new GroupStatusSQLStore(contextFactory.Object, _loggerFactory.Object);
            var result = store.GetFacilityParamStandardTypes(new List<string>().ToArray(), string.Empty);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetClassificationsDataTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.AnalyticsClassification)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetAnalyticsClassification().AsQueryable()).Object);
            mockContext.Setup(x => x.AnalyticsClassificationType)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetAnalyticsClassificationTypes().AsQueryable()).Object);
            mockContext.Setup(x => x.LocalePhrases)
                .Returns(TestUtilities.SetupMockData(new List<LocalePhraseEntity>().AsQueryable()).Object);

            contextFactory.Setup(cf => cf.GetContext()).Returns(mockContext.Object);

            var store = new GroupStatusSQLStore(contextFactory.Object, _loggerFactory.Object);
            var result = store.GetClassificationsData(new List<string>(), string.Empty, out var assetCount);

            Assert.IsInstanceOfType(result, typeof(List<AssetGroupStatusClassificationModel>));
        }

        #endregion

        #region Private Methods

        private IList<FacilityTagsEntity> GetFacilitytags()
        {
            var eventData = new List<FacilityTagsEntity>()
            {
                new FacilityTagsEntity()
                {
                    NodeId = "DFC1D0AD-A824-4965-B78D-AB7755E32DD3",
                    Address = 1,
                    Description = "Well1",
                    Enabled = true,
                    TrendType = 101,
                    RawLo = 101,
                    RawHi = 101,
                    EngLo = 101,
                    EngHi = 101,
                    LimitLo = 1000,
                    LimitHi = 1000,
                    CurrentValue = 0,
                    EngUnits = "eng",
                    UpdateDate = DateTime.Now,
                    Writeable = true,
                    Topic = "test",
                    GroupNodeId = "test",
                    DisplayOrder = null,
                    AlarmState = 2,
                    AlarmAction = 1,
                    WellGroupName = "test",
                    PagingGroup = "test",
                    AlarmArg = "test",
                    AlarmTextLo = null,
                    AlarmTextHi = null,
                    AlarmTextClear = null,
                    GroupStatusView = 1,
                    ResponderListId = null,
                    VoiceTextLo = null,
                    VoiceTextHi = null,
                    VoiceTextClear = null,
                    DataType = null,
                    Decimals = null,
                    VoiceNodeId = null,
                    ContactListId = null,
                    Bit = 1,
                    Deadband = 1,
                    DestinationType = null,
                    StateId = null,
                    CaptureType = 1,
                    LastCaptureDate = DateTime.Now,
                    NumConsecAlarmsAllowed = 0,
                    NumConsecAlarms = 1,
                    UnitType = 1,
                    Name = null,
                    FacilityTagGroupId = null,
                    ParamStandardType = null,
                    Latitude = 1,
                    Longitude = 1,
                    ArchiveFunction = 1,
                    Tag = "test",
                    DetailViewOnly = true,
                    Expression = null,
                    StringValue = null,
                },
            };

            return eventData;
        }

        /// <summary>
        /// Gets the sample group status  view data.
        /// </summary>
        /// <returns>The <seealso cref="IList{GroupStatusView}"/> data.</returns>
        public static IList<GroupStatusViewEntity> GetGroupStatusViewData()
        {
            var views = new List<GroupStatusViewEntity>()
            {
                new GroupStatusViewEntity()
                {
                    ViewId = 1,
                    ViewName = "Default",
                    FilterId = 1,
                    UserId = "Global"
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
        /// Gets the sample group status  view data.
        /// </summary>
        /// <returns>The <seealso cref="IList{GroupStatusViewsColumn}"/> data.</returns>
        public static IList<GroupStatusViewsColumnEntity> GetGroupStatusViewColumnData()
        {
            var views = new List<GroupStatusViewsColumnEntity>()
            {
                new GroupStatusViewsColumnEntity()
                {
                    ViewId = 1,
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    FormatId = 1,
                },
                new GroupStatusViewsColumnEntity()
                {
                    ViewId = 2,
                    ColumnId = 2,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    FormatId = 1,
                },
            };

            return views;
        }

        /// <summary>
        /// Gets the sample group status column data.
        /// </summary>
        /// <returns>The <seealso cref="IList{GroupStatusViewsColumn}"/> data.</returns>
        public static IList<GroupStatusColumnEntity> GetGroupStatusColumnData()
        {
            var views = new List<GroupStatusColumnEntity>()
            {
                new GroupStatusColumnEntity()
                {
                    Alias = "Alias",
                    Align = 1,
                    ColumnId = 1,
                    ColumnName = "Name",
                    Formula = "Formula",
                    Measure = "Measure",
                    SourceId = 1,
                    Visible = 1
                },
                new GroupStatusColumnEntity()
                {
                    Alias = "Alias",
                    Align = 1,
                    ColumnId = 2,
                    ColumnName = "Name",
                    Formula = "Formula",
                    Measure = "Measure",
                    SourceId = 1,
                    Visible = 1
                },
            };

            return views;
        }

        /// <summary>
        /// Gets the parameter data.
        /// </summary>
        /// <returns>The <seealso cref="IList{parameter}"/> data.</returns>
        public static IList<ParameterEntity> GetParameterData()
        {
            var views = new List<ParameterEntity>()
            {
                new ParameterEntity()
                {
                    Access = "Access",
                    Address = 1,
                    ArchiveFunction = 1,
                    CollectionMode = 1,
                    DataCollection = true,
                    DataType = 1,
                    Decimals = 1,
                    Description = "Name",
                    DestinationType = 1,
                    EarliestSupportedVersion = 1,
                    FastScan = true,
                    GroupStatusView = 1,
                    Locked = true,
                    Offset = 0,
                    ParamStandardType = 0,
                    PhraseId = 1,
                    Poctype = 1,
                    ScaleFactor = 1,
                    Setpoint = true,
                    SetpointGroup = 1,
                    StateId = 2,
                    StatusScan = true,
                    Tag = "",
                    UnitType = 1
                },
                new ParameterEntity()
                {
                    Access = "Access",
                    Address = 1,
                    ArchiveFunction = 1,
                    CollectionMode = 1,
                    DataCollection = true,
                    DataType = 1,
                    Decimals = 1,
                    Description = "Description",
                    DestinationType = 1,
                    EarliestSupportedVersion = 1,
                    FastScan = true,
                    GroupStatusView = 1,
                    Locked = true,
                    Offset = 0,
                    ParamStandardType = 0,
                    PhraseId = 1,
                    Poctype = 2,
                    ScaleFactor = 1,
                    Setpoint = true,
                    SetpointGroup = 1,
                    StateId = 2,
                    StatusScan = true,
                    Tag = "",
                    UnitType = 1
                },
            };

            return views;
        }

        #endregion

        #region Private Setup Methods

        private Mock<DbSet<FacilityTagsEntity>> SetupFacilityTags(IQueryable<FacilityTagsEntity> data)
        {
            var mockDbSet = new Mock<DbSet<FacilityTagsEntity>>();
            mockDbSet.As<IQueryable<FacilityTagsEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<FacilityTagsEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<FacilityTagsEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<FacilityTagsEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet;
        }

        private Mock<DbSet<GroupStatusViewEntity>> SetupGroupStatusView(IQueryable<GroupStatusViewEntity> data)
        {
            var mockDbSet = new Mock<DbSet<GroupStatusViewEntity>>();
            mockDbSet.As<IQueryable<GroupStatusViewEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<GroupStatusViewEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<GroupStatusViewEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<GroupStatusViewEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet;
        }

        private Mock<DbSet<GroupStatusViewsColumnEntity>> SetupGroupStatusViewsColumn(
            IQueryable<GroupStatusViewsColumnEntity> data)
        {
            var mockDbSet = new Mock<DbSet<GroupStatusViewsColumnEntity>>();
            mockDbSet.As<IQueryable<GroupStatusViewsColumnEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<GroupStatusViewsColumnEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<GroupStatusViewsColumnEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<GroupStatusViewsColumnEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet;
        }

        private Mock<DbSet<GroupStatusColumnEntity>> SetupGroupStatusColumn(IQueryable<GroupStatusColumnEntity> data)
        {
            var mockDbSet = new Mock<DbSet<GroupStatusColumnEntity>>();
            mockDbSet.As<IQueryable<GroupStatusColumnEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<GroupStatusColumnEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<GroupStatusColumnEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<GroupStatusColumnEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet;
        }

        private Mock<DbSet<ParameterEntity>> SetupParameter(IQueryable<ParameterEntity> data)
        {
            var mockDbSet = new Mock<DbSet<ParameterEntity>>();
            mockDbSet.As<IQueryable<ParameterEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<ParameterEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<ParameterEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<ParameterEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet;
        }

        private void SetupThetaLoggerFactory()
        {
            _loggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(_logger.Object);
        }

        #endregion

    }
}
