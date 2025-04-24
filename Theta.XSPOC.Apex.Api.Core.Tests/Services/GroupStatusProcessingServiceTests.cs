using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Common;
using Theta.XSPOC.Apex.Api.Data.Influx.Services;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Parameter;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Kernel.Logging.Models;
using UnitCategory = Theta.XSPOC.Apex.Api.Core.Common.UnitCategory;

namespace Theta.XSPOC.Apex.Api.Core.Tests.Services
{
    [TestClass]
    public class GroupStatusProcessingServiceTests
    {

        private Mock<IThetaLoggerFactory> _loggerFactory;
        private Mock<IThetaLogger> _logger;
        private Mock<IGroupStatus> _groupStatus;
        private Mock<IParameterDataType> _parameterDataType;
        private Mock<INodeMaster> _nodeMaster;
        private Mock<IColumnFormatterFactory> _columnFormatterFactory;
        private Mock<IColumnFormatter> _columnFormatter;
        private GroupStatusProcessingService _groupStatusProcessingService;
        private Mock<ILocalePhrases> _localePhrase;
        private Mock<IGroupAndAsset> _groupAndAsset;
        private Mock<IDataHistorySQLStore> _trendDataStore;
        private Mock<IGetDataHistoryItemsService> _trendDataInfluxStore;
        private Mock<IServiceScopeFactory> _serviceScopeFactory;
        private Mock<IParameterMongoStore> _mongoDbStore;
        private Mock<ICommonService> _commonService;

        [TestInitialize]
        public void TestInitialize()
        {
            _loggerFactory = new Mock<IThetaLoggerFactory>();
            _logger = new Mock<IThetaLogger>();
            _groupStatus = new Mock<IGroupStatus>();
            _parameterDataType = new Mock<IParameterDataType>();
            _nodeMaster = new Mock<INodeMaster>();
            _columnFormatterFactory = new Mock<IColumnFormatterFactory>();
            _columnFormatter = new Mock<IColumnFormatter>();
            _localePhrase = new Mock<ILocalePhrases>();
            _groupAndAsset = new Mock<IGroupAndAsset>();
            _trendDataStore = new Mock<IDataHistorySQLStore>();
            _trendDataInfluxStore = new Mock<IGetDataHistoryItemsService>();
            _serviceScopeFactory = new Mock<IServiceScopeFactory>();
            _mongoDbStore = new Mock<IParameterMongoStore>();
            _commonService = new Mock<ICommonService>();

            SetupThetaLoggerFactory();
            SetupGroupStatus();
            SetupParameterDataType();
            SetupNodeMaster();
            SetupColumnFormatterFactory();
            SetupGroupStatusProcessingService();
            SetupLocalePhrase();
            SetupAssetAndGroup();
            SetupTrendDataStore();
            SetupTrendDataInfluxStore();
            SetupServiceScopeFactory();
            SetupMongoDbStore();
            SetupCommonService();
        }

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoggerFactoryNullParameterTest()
        {
            _ = new GroupStatusProcessingService(null, new Mock<IGroupStatus>().Object, new Mock<IParameterDataType>().Object,
                new Mock<INodeMaster>().Object, new Mock<IColumnFormatterFactory>().Object, new Mock<ILocalePhrases>().Object,
                new Mock<IGroupAndAsset>().Object, new Mock<IDataHistorySQLStore>().Object, new Mock<IGetDataHistoryItemsService>().Object,
                new Mock<IServiceScopeFactory>().Object, new Mock<IParameterMongoStore>().Object, _commonService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GroupStatusNullParameterTest()
        {
            _ = new GroupStatusProcessingService(new Mock<IThetaLoggerFactory>().Object, null,
                new Mock<IParameterDataType>().Object, new Mock<INodeMaster>().Object, new Mock<IColumnFormatterFactory>().Object,
                new Mock<ILocalePhrases>().Object, new Mock<IGroupAndAsset>().Object, new Mock<IDataHistorySQLStore>().Object,
                new Mock<IGetDataHistoryItemsService>().Object, new Mock<IServiceScopeFactory>().Object, new Mock<IParameterMongoStore>().Object, _commonService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ParameterDataTypeNullParameterTest()
        {
            _ = new GroupStatusProcessingService(new Mock<IThetaLoggerFactory>().Object, new Mock<IGroupStatus>().Object, null,
                new Mock<INodeMaster>().Object, new Mock<IColumnFormatterFactory>().Object, new Mock<ILocalePhrases>().Object,
                new Mock<IGroupAndAsset>().Object, new Mock<IDataHistorySQLStore>().Object, new Mock<IGetDataHistoryItemsService>().Object,
                new Mock<IServiceScopeFactory>().Object, new Mock<IParameterMongoStore>().Object, _commonService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NodeMasterStoreNullParameterTest()
        {
            _ = new GroupStatusProcessingService(new Mock<IThetaLoggerFactory>().Object, new Mock<IGroupStatus>().Object,
                new Mock<IParameterDataType>().Object, null, new Mock<IColumnFormatterFactory>().Object,
                new Mock<ILocalePhrases>().Object, new Mock<IGroupAndAsset>().Object, new Mock<IDataHistorySQLStore>().Object,
                new Mock<IGetDataHistoryItemsService>().Object, new Mock<IServiceScopeFactory>().Object,
                new Mock<IParameterMongoStore>().Object, _commonService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ColumnFormatterFactoryNullParameterTest()
        {
            _ = new GroupStatusProcessingService(new Mock<IThetaLoggerFactory>().Object, new Mock<IGroupStatus>().Object,
                new Mock<IParameterDataType>().Object, new Mock<INodeMaster>().Object, null, new Mock<ILocalePhrases>().Object,
                new Mock<IGroupAndAsset>().Object, new Mock<IDataHistorySQLStore>().Object, new Mock<IGetDataHistoryItemsService>().Object,
                new Mock<IServiceScopeFactory>().Object, new Mock<IParameterMongoStore>().Object, _commonService.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LocalePhrasesNullParameterTest()
        {
            _ = new GroupStatusProcessingService(new Mock<IThetaLoggerFactory>().Object, new Mock<IGroupStatus>().Object,
                new Mock<IParameterDataType>().Object, new Mock<INodeMaster>().Object, null, null,
                new Mock<IGroupAndAsset>().Object, new Mock<IDataHistorySQLStore>().Object, new Mock<IGetDataHistoryItemsService>().Object,
                new Mock<IServiceScopeFactory>().Object, new Mock<IParameterMongoStore>().Object, _commonService.Object);
        }

        [TestMethod]
        public void GetAvailableViewResultsTest()
        {
            var views = new List<AvailableViewModel>()
            {
                new AvailableViewModel()
                {
                    ViewId = 1,
                    ViewName = "Default",
                },
                new AvailableViewModel()
                {
                    ViewId = 3,
                    ViewName = "PCP Global",
                },
            };

            _groupStatus.Setup(x => x.GetAvailableViewsByUserId("Global", It.IsAny<string>())).Returns(views);

            var request = new WithCorrelationId<AvailableViewInput>("correlationId1", new AvailableViewInput
            {
                UserId = "Global"
            });

            var result = _groupStatusProcessingService.GetAvailableViews(request);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(AvailableViewOutput));
            Assert.AreEqual(true, result.Result.Status);
            _groupStatus.Verify(x => x.GetAvailableViewsByUserId("Global", It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void GetAvailableViewResultsNullDataTest()
        {
            _groupStatus.Setup(x =>
                x.GetAvailableViewsByUserId("Test", It.IsAny<string>()));

            var result = _groupStatusProcessingService.GetAvailableViews(null);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Result.Status);
            _logger.Verify(x => x.Write(Level.Info,
                    It.Is<string>(x => x.Contains("requestWithCorrelationId is null, cannot get available views."))),
                Times.AtLeastOnce);
            _groupStatus.Verify(x => x.GetAvailableViewsByUserId("Global", It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void GetAvailableViewResultsNullPayloadValueTest()
        {
            _groupStatus.Setup(x =>
                x.GetAvailableViewsByUserId("test", It.IsAny<string>()));

            var request = new WithCorrelationId<AvailableViewInput>("correlationId1", null);

            var result = _groupStatusProcessingService.GetAvailableViews(request);

            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.Result.Status);
            _logger.Verify(x => x.WriteCId(Level.Info,
                    It.Is<string>(x => x.Contains("requestWithCorrelationId is null, cannot get available views.")),
                    "correlationId1"),
                Times.AtLeastOnce);
            _groupStatus.Verify(x => x.GetAvailableViewsByUserId
                ("test", It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void GetAvailableViewsUserIdEmptyReturnsErrorMessageTest()
        {
            // Arrange
            var requestWithCorrelationId = new WithCorrelationId<AvailableViewInput>(
                Guid.NewGuid().ToString(), new AvailableViewInput
                {
                    UserId = string.Empty
                });

            // Act
            var result = _groupStatusProcessingService.GetAvailableViews(requestWithCorrelationId);

            // Assert
            Assert.IsFalse(result.Result.Status);
            Assert.AreEqual("Missing UserId.", result.Result.Value);
        }

        [TestMethod]
        public void GetGroupStatusWithNullRequestTest()
        {
            // Act
            var result = _groupStatusProcessingService.GetGroupStatus(null);

            // Assert
            Assert.IsFalse(result.Result.Status);
            Assert.AreEqual("Correlation Id is null.", result.Result.Value);
        }

        [TestMethod]
        public void GetGroupStatusWithNullValueInRequestTest()
        {
            // Arrange
            var request = new WithCorrelationId<GroupStatusInput>("correlationId1", null);

            // Act
            var result = _groupStatusProcessingService.GetGroupStatus(request);

            // Assert
            Assert.IsFalse(result.Result.Status);
            Assert.AreEqual("requestWithCorrelationId is null, cannot get group status.", result.Result.Value);
        }

        [TestMethod]
        public void GetGroupStatusTest()
        {
            // Arrange
            _groupStatus.Setup(x => x.BuildSQLCommonResult(
                    It.IsAny<string[]>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<string>(),
                    It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        {
                            "Well", "TestNode"
                        },
                    }
                });

            _groupStatus.Setup(gs => gs.LoadCommonColumns(It.IsAny<string>())).Returns(new Hashtable());

            var viewParameters = new SortedList<string, ParameterItem>
            {
                {
                    "testKey", new ParameterItem
                    {
                        Address = "TestAddress",
                        Description = "TestDescription",
                        DataType = 1,
                        StateID = 1,
                        UnitType = 1
                    }
                }
            };
            var itemsGroupStatus = new List<FacilityTagsModel>
            {
                new FacilityTagsModel
                {
                    NodeId = "TestNodeId",
                    Address = "1",
                    Description = "TestDescription",
                    Enabled = true,
                    TrendType = 1,
                    RawLo = 1,
                    RawHi = 1,
                    EngLo = 1,
                    EngHi = 1,
                    LimitLo = 1,
                    LimitHi = 1,
                    CurrentValue = 1,
                    EngUnits = "TestEngUnits",
                    UpdateDate = DateTime.Now,
                    Writeable = true,
                    Topic = "TestTopic",
                    GroupNodeId = "TestGroupNodeId",
                    DisplayOrder = 1,
                    AlarmState = 1,
                    AlarmAction = 1,
                    WellGroupName = "TestWellGroupName",
                    PagingGroup = "TestPagingGroup",
                    AlarmArg = "TestAlarmArg",
                    AlarmTextLo = "TestAlarmTextLo",
                    AlarmTextHi = "TestAlarmTextHi",
                    AlarmTextClear = "TestAlarmTextClear",
                    GroupStatusView = 1,
                    ResponderListId = 1,
                    VoiceTextLo = "TestVoiceTextLo",
                    VoiceTextHi = "TestVoiceTextHi",
                    VoiceTextClear = "TestVoiceTextClear",
                    DataType = 1,
                    Decimals = 1,
                    VoiceNodeId = "TestVoiceNodeId",
                    Bit = 1,
                    ContactListId = 1,
                    Deadband = 1,
                    UnitType = 1,
                    DestinationType = 1,
                    StateId = 1,
                    Name = "TestName",
                    DisplayName = "TestDisplayName",
                    CaptureType = 1,
                    ParamStandardType = 1,
                    Latitude = 1,
                    Longitude = 1,
                    ArchiveFunction = 1,
                    Tag = "TestTag",
                    NumConsecAlarmsAllowed = 1,
                    DetailViewOnly = true,
                    StringValue = "TestStringValue",
                    Expression = "TestExpression",
                    FacilityTagGroupId = 1
                }
            };
            var conditionalFormats = new List<ConditionalFormatModel>
            {
                new ConditionalFormatModel
                {
                    Id = 1,
                    ColumnId = 1,
                    OperatorId = 1,
                    Value = 1,
                    MinValue = 1,
                    MaxValue = 1,
                    BackColor = 1,
                    ForeColor = 1,
                    StringValue = "TestStringValue"
                }
            };
            var viewColumns = new List<GroupStatusColumnsModels>
            {
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "TestColumnName",
                    UserId = "TestUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "TestAlias",
                    SourceId = 1,
                    Align = 1,
                    Visible = 1,
                    Measure = "TestMeasure",
                    FormatId = 1,
                    Formula = "TestFormula",
                    ParamStandardType = 1,
                    FormatMask = "TestFormatMask",
                    UnitType = 1
                },
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "CAMERAALARMS",
                    UserId = "TestUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "TestAlias",
                    SourceId = 1,
                    Align = 1,
                    Visible = 1,
                    Measure = "TestMeasure",
                    FormatId = 1,
                    Formula = "TestFormula",
                    ParamStandardType = 1,
                    FormatMask = "TestFormatMask",
                    UnitType = 1
                },
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "OPERATIONALSCORE",
                    UserId = "TestUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "TestAlias",
                    SourceId = 1,
                    Align = 1,
                    Visible = 1,
                    Measure = "TestMeasure",
                    FormatId = 1,
                    Formula = "TestFormula",
                    ParamStandardType = 1,
                    FormatMask = "TestFormatMask",
                    UnitType = 1
                },
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "%RT 30D",
                    UserId = "TestUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "TestAlias",
                    SourceId = 1,
                    Align = 1,
                    Visible = 1,
                    Measure = "TestMeasure",
                    FormatId = 1,
                    Formula = "TestFormula",
                    ParamStandardType = 1,
                    FormatMask = "TestFormatMask",
                    UnitType = 1
                },
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "CAMERAALARMS",
                    UserId = "TestUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "TestAlias",
                    SourceId = (int)GroupStatusColumns.SourceType.Parameter,
                    Align = 1,
                    Visible = 1,
                    Measure = "TestMeasure",
                    FormatId = 1,
                    Formula = "TestFormula",
                    ParamStandardType = 1,
                    FormatMask = "TestFormatMask",
                    UnitType = 1
                },
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "OPERATIONALSCORE",
                    UserId = "TestUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "TestAlias",
                    SourceId = (int)GroupStatusColumns.SourceType.Facility,
                    Align = 1,
                    Visible = 1,
                    Measure = "TestMeasure",
                    FormatId = 1,
                    Formula = "TestFormula",
                    ParamStandardType = 1,
                    FormatMask = "TestFormatMask",
                    UnitType = 1
                },
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "%RT 30D",
                    UserId = "TestUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "TestAlias",
                    SourceId = (int)GroupStatusColumns.SourceType.ParamStandard,
                    Align = 1,
                    Visible = 1,
                    Measure = "TestMeasure",
                    FormatId = 1,
                    Formula = "TestFormula",
                    ParamStandardType = 1,
                    FormatMask = "TestFormatMask",
                    UnitType = 1
                },
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "%RT 30D",
                    UserId = "TestUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "TestAlias",
                    SourceId = (int)GroupStatusColumns.SourceType.Formula,
                    Align = 1,
                    Visible = 1,
                    Measure = "TestMeasure",
                    FormatId = 1,
                    Formula = "TestFormula",
                    ParamStandardType = 1,
                    FormatMask = "TestFormatMask",
                    UnitType = 1
                },
            };
            var parameterDataTypes = new List<DataTypesModel>
            {
                new DataTypesModel()
                {
                    Description = "TestDescription",
                }
            };

            var groupAndAssetModel = new GroupAndAssetModel
            {
                GroupName = "TestGroup",
                Assets = new List<AssetModel>
                {
                    new AssetModel
                    {
                        AssetId = new Guid(),
                        AssetName = "TestNodeId",
                        IndustryApplicationId = 3
                    }
                }
            };

            _groupStatus.Setup(x => x.LoadViewParameters(It.IsAny<string>(), It.IsAny<string>())).Returns(viewParameters);
            _groupStatus.Setup(x => x.GetItemsGroupStatus(It.IsAny<string[]>(), It.IsAny<string>())).Returns(itemsGroupStatus);
            _groupStatus.Setup(x => x.GetConditionalFormats(It.IsAny<string>(), It.IsAny<string>())).Returns(conditionalFormats);
            _groupStatus.Setup(x => x.LoadViewColumns(It.IsAny<string>(), It.IsAny<string>())).Returns(viewColumns);
            _parameterDataType.Setup(x => x.GetItems(It.IsAny<string>())).Returns(parameterDataTypes);
            _groupAndAsset.Setup(x => x.GetGroupAssetAndRelationshipData(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(groupAndAssetModel);

            var request = new WithCorrelationId<GroupStatusInput>("CorrelationId1", new GroupStatusInput
            {
                ViewId = "TestViewId",
                GroupName = "TestGroup"
            });

            // Act
            GroupStatusOutput result = _groupStatusProcessingService.GetGroupStatus(request);

            // Assert
            Assert.IsTrue(result.Result.Status);
            Assert.IsNotNull(result.Result.Value);
            Assert.AreEqual(65, result.Values.Columns.Count);
            Assert.AreEqual(1, result.Values.Rows.Count);
        }

        [TestMethod]
        public void GetGroupStatusMissingViewIdTest()
        {
            var request = new WithCorrelationId<GroupStatusInput>("CorrelationId1", new GroupStatusInput
            {
                ViewId = "",
                GroupName = "TestGroup"
            });

            // Act
            GroupStatusOutput result = _groupStatusProcessingService.GetGroupStatus(request);

            // Assert
            Assert.IsFalse(result.Result.Status);
        }

        [TestMethod]
        public void GetGroupStatusMapGroupStatusColumnsTISTest()
        {
            // Arrange
            _groupStatus.Setup(x => x.BuildSQLCommonResult(
                    It.IsAny<string[]>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<string>(),
                    It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        {
                            "Well", "TestNode"
                        },
                    }
                });

            var viewParameters = new SortedList<string, ParameterItem>
            {
                {
                    "testKey", new ParameterItem
                    {
                        Address = "TestAddress",
                        Description = "TestDescription",
                        DataType = 1,
                        StateID = 1,
                        UnitType = 1
                    }
                }
            };
            var itemsGroupStatus = new List<FacilityTagsModel>
            {
                new FacilityTagsModel
                {
                    NodeId = "testNodeId",
                    Address = "1",
                    Description = "TestDescription",
                    Enabled = true,
                    TrendType = 1,
                    RawLo = 1,
                    RawHi = 1,
                    EngLo = 1,
                    EngHi = 1,
                    LimitLo = 1,
                    LimitHi = 1,
                    CurrentValue = 1,
                    EngUnits = "testEngUnits",
                    UpdateDate = DateTime.Now,
                    Writeable = true,
                    Topic = "testTopic",
                    GroupNodeId = "testGroupNodeId",
                    DisplayOrder = 1,
                    AlarmState = 1,
                    AlarmAction = 1,
                    WellGroupName = "testWellGroupName",
                    PagingGroup = "testPagingGroup",
                    AlarmArg = "testAlarmArg",
                    AlarmTextLo = "testAlarmTextLo",
                    AlarmTextHi = "testAlarmTextHi",
                    AlarmTextClear = "testAlarmTextClear",
                    GroupStatusView = 1,
                    ResponderListId = 1,
                    VoiceTextLo = "testVoiceTextLo",
                    VoiceTextHi = "testVoiceTextHi",
                    VoiceTextClear = "testVoiceTextClear",
                    DataType = 1,
                    Decimals = 1,
                    VoiceNodeId = "testVoiceNodeId",
                    Bit = 1,
                    ContactListId = 1,
                    Deadband = 1,
                    UnitType = 1,
                    DestinationType = 1,
                    StateId = 1,
                    Name = "testName",
                    DisplayName = "testDisplayName",
                    CaptureType = 1,
                    ParamStandardType = 1,
                    Latitude = 1,
                    Longitude = 1,
                    ArchiveFunction = 1,
                    Tag = "testTag",
                    NumConsecAlarmsAllowed = 1,
                    DetailViewOnly = true,
                    StringValue = "testStringValue",
                    Expression = "testExpression",
                    FacilityTagGroupId = 1
                }
            };
            var conditionalFormats = new List<ConditionalFormatModel>
            {
                new ConditionalFormatModel
                {
                    Id = 1,
                    ColumnId = 1,
                    OperatorId = 1,
                    Value = 1,
                    MinValue = 1,
                    MaxValue = 1,
                    BackColor = 1,
                    ForeColor = 1,
                    StringValue = "testStringValue"
                }
            };
            var viewColumns = new List<GroupStatusColumnsModels>
            {
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "testColumnName",
                    UserId = "testUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "testAlias",
                    SourceId = 1,
                    Align = 1,
                    Visible = 1,
                    Measure = "testMeasure",
                    FormatId = 1,
                    Formula = "testFormula",
                    ParamStandardType = 1,
                    FormatMask = "testFormatMask",
                    UnitType = 1
                },
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "TIS",
                    UserId = "testUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "testAlias",
                    SourceId = (int)GroupStatusColumns.SourceType.Common,
                    Align = 1,
                    Visible = 1,
                    Measure = "testMeasure",
                    FormatId = 1,
                    Formula = "testFormula",
                    ParamStandardType = 1,
                    FormatMask = "testFormatMask",
                    UnitType = 1
                },
            };
            var parameterDataTypes = new List<DataTypesModel>
            {
                new DataTypesModel()
                {
                    Description = "TestDescription",
                }
            };

            var groupAndAssetModel = new GroupAndAssetModel
            {
                GroupName = "TestGroup",
                Assets = new List<AssetModel>
                {
                    new AssetModel
                    {
                        AssetId = new Guid(),
                        AssetName = "TestNodeId",
                        IndustryApplicationId = 3
                    }
                }
            };

            _groupStatus.Setup(x => x.LoadViewParameters(It.IsAny<string>(), It.IsAny<string>())).Returns(viewParameters);
            _groupStatus.Setup(x => x.GetItemsGroupStatus(It.IsAny<string[]>(), It.IsAny<string>())).Returns(itemsGroupStatus);
            _groupStatus.Setup(x => x.GetConditionalFormats(It.IsAny<string>(), It.IsAny<string>())).Returns(conditionalFormats);
            _groupStatus.Setup(x => x.LoadViewColumns(It.IsAny<string>(), It.IsAny<string>())).Returns(viewColumns);
            _parameterDataType.Setup(x => x.GetItems(It.IsAny<string>())).Returns(parameterDataTypes);
            _groupAndAsset.Setup(x => x.GetGroupAssetAndRelationshipData(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(groupAndAssetModel);

            var request = new WithCorrelationId<GroupStatusInput>("correlationId1", new GroupStatusInput
            {
                ViewId = "testViewId",
                GroupName = "TestGroup"
            });

            // Act
            GroupStatusOutput result = _groupStatusProcessingService.GetGroupStatus(request);

            // Assert
            Assert.IsTrue(result.Result.Status);
            Assert.IsNotNull(result.Result.Value);
            Assert.AreEqual(5, result.Values.Columns.Count);
            Assert.AreEqual(1, result.Values.Rows.Count);
        }

        [TestMethod]
        public void GetGroupStatusParametersTest()
        {
            // Arrange
            var requestWithCorrelationId = new WithCorrelationId<GroupStatusInput>(Guid.NewGuid().ToString(), new GroupStatusInput
            {
                ViewId = "TestViewId",
                GroupName = "TestGroup",
            });

            var parameterList = new SortedList<string, ParameterItem>
            {
                {
                    "TestColumnName", new ParameterItem
                    {
                        Address = "TestAddress",
                        Description = "TestDescription",
                        DataType = (int)UnitCategory.Type.Discrete,
                        StateID = 0,
                        UnitType = 1
                    }
                },
                {
                    "TestColumnEnron", new ParameterItem
                    {
                        Address = "TestAddress",
                        Description = "TestDescription",
                        StateID = 0,
                        DataType = (int)UnitCategory.Type.TimeEnron,
                        UnitType = 1
                    }
                },
                {
                    "TestColumnBakerDate", new ParameterItem
                    {
                        Address = "TestAddress",
                        Description = "TestDescription",
                        StateID = 0,
                        DataType = (int)UnitCategory.Type.BakerDate,
                        UnitType = 1
                    }
                },
                {
                    "TestColumnUseV1", new ParameterItem
                    {
                        Address = "TestAddress",
                        Description = "TestDescription",
                        StateID = 0,
                        DataType = (int)UnitCategory.Type.Discrete,
                        UnitType = 1
                    }
                }
            };

            _groupStatus.Setup(gs => gs.LoadViewParameters(It.IsAny<string>(), It.IsAny<string>())).Returns(parameterList);

            var parameterTypeResult = new List<ParameterTypeResult>
            {
                new ParameterTypeResult
                {
                    NodeId = "TestNode",
                    Description = "TestColumnName",
                    Text = "TestText",
                    BackColor = 123456,
                    ForeColor = 654321,
                    V1 = 10,
                    Decimal = 0,
                },
                new ParameterTypeResult
                {
                    NodeId = "TestNode",
                    Description = "TestColumnEnron",
                    Text = string.Empty,
                    BackColor = 123456,
                    ForeColor = 654321,
                    V1 = 28500,
                    Decimal = 0,
                },
                new ParameterTypeResult
                {
                    NodeId = "TestNode",
                    Description = "TestColumnBakerDate",
                    Text = string.Empty,
                    BackColor = 123456,
                    ForeColor = 654321,
                    V1 = 5,
                    Decimal = 0,
                },
                new ParameterTypeResult
                {
                    NodeId = "TestNode",
                    Description = "TestColumnUseV1",
                    Text = string.Empty,
                    BackColor = 123456,
                    ForeColor = 654321,
                    V1 = 20,
                    Decimal = 0,
                },
            };

            _groupStatus.Setup(gs => gs.BuildSQLParameterResult(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(parameterTypeResult);

            var dataTypesModels = new List<DataTypesModel>
            {
                new DataTypesModel()
                {
                    Description = "TestDescription",
                },
                new DataTypesModel
                {
                    DataType = (int)UnitCategory.Type.Discrete,
                    Description = "discrete"
                }
            };

            _parameterDataType.Setup(pdt => pdt.GetItems(It.IsAny<string>())).Returns(dataTypesModels);

            var viewColumns = new List<GroupStatusColumnsModels>
            {
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "TestColumnName",
                    UserId = "testUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "testAlias",
                    SourceId = (int)GroupStatusColumns.SourceType.Parameter,
                    Align = 1,
                    Visible = 1,
                    Measure = "testMeasure",
                    FormatId = 1,
                    Formula = "testFormula",
                    ParamStandardType = 1,
                    FormatMask = "testFormatMask",
                    UnitType = 1
                },
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "TestColumnEnron",
                    UserId = "testUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "testAlias",
                    SourceId = (int)GroupStatusColumns.SourceType.Parameter,
                    Align = 1,
                    Visible = 1,
                    Measure = "testMeasure",
                    FormatId = 1,
                    Formula = "testFormula",
                    ParamStandardType = 1,
                    FormatMask = "testFormatMask",
                    UnitType = 1
                },
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "TestColumnBakerDate",
                    UserId = "testUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "testAlias",
                    SourceId = (int)GroupStatusColumns.SourceType.Parameter,
                    Align = 1,
                    Visible = 1,
                    Measure = "testMeasure",
                    FormatId = 1,
                    Formula = "testFormula",
                    ParamStandardType = 1,
                    FormatMask = "testFormatMask",
                    UnitType = 1
                },
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "TestColumnUseV1",
                    UserId = "testUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "testAlias",
                    SourceId = (int)GroupStatusColumns.SourceType.Parameter,
                    Align = 1,
                    Visible = 1,
                    Measure = "testMeasure",
                    FormatId = 1,
                    Formula = "testFormula",
                    ParamStandardType = 1,
                    FormatMask = "testFormatMask",
                    UnitType = 1
                },
            };

            _groupStatus.Setup(gs => gs.LoadViewColumns(It.IsAny<string>(), It.IsAny<string>())).Returns(viewColumns);

            _groupStatus.Setup(gs => gs.BuildSQLCommonResult(
                    It.IsAny<IList<string>>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<string>(),
                    It.IsAny<IList<string>>(), It.IsAny<string>()))
                .Returns(new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        {
                            "Well", "TestNode"
                        },
                    },
                });

            var groupAndAssetModel = new GroupAndAssetModel
            {
                GroupName = "TestGroup",
                Assets = new List<AssetModel>
                {
                    new AssetModel
                    {
                        AssetId = new Guid(),
                        AssetName = "Asset1",
                        IndustryApplicationId = 3
                    }
                }
            };

            _groupAndAsset.Setup(x => x.GetGroupAssetAndRelationshipData(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(groupAndAssetModel);

            _nodeMaster.Setup(x => x.GetNodeIdsByAssetGuid(It.IsAny<string[]>(), It.IsAny<string>())).Returns(new List<NodeMasterModel>()
            {
                new NodeMasterModel()
                {
                    NodeId = "TestNodeId",
                }
            });

            // Act
            var result = _groupStatusProcessingService.GetGroupStatus(requestWithCorrelationId);

            // Assert
            Assert.IsTrue(result.Result.Status);
            Assert.IsNotNull(result.Result.Value);
            Assert.AreEqual(17, result.Values.Columns.Count);
            Assert.AreEqual(1, result.Values.Rows.Count);
        }

        [TestMethod]
        public void GetGroupStatusFacilityTagsTest()
        {
            _groupStatus.Setup(gs => gs.BuildSQLFacilityResult(It.IsAny<IList<string>>(), It.IsAny<string>()))
                .Returns(new List<FacilityTypeResult>
                {
                    new FacilityTypeResult
                    {
                        FacilityNodeId = "TestFacilityNodeId",
                        GroupNodeId = "TestNode",
                        AlarmState = 1,
                        AlarmTextColor = "TestAlarmTextColor",
                        Description = "TestColumnName",
                        AlarmTextHi = "TestAlarmTextHi",
                        AlarmTextLo = "TestAlarmTextLo",
                        CurrentValue = "TestCurrentValue",
                        DataType = 1,
                        StateId = 1,
                        Text = "TestText",
                        BackColor = 1,
                        ForeColor = 1,
                        ParamStandardType = 1,
                        Decimals = 1,
                        FacilityTagAlarm = "TestColumnName"
                    },
                    new FacilityTypeResult
                    {
                        FacilityNodeId = "TestFacilityNodeId",
                        GroupNodeId = "TestNode",
                        AlarmState = 2,
                        AlarmTextColor = "TestAlarmTextColor",
                        Description = "TestCurrentValueFloat",
                        AlarmTextHi = "TestAlarmTextHi",
                        AlarmTextLo = "TestAlarmTextLo",
                        CurrentValue = "5.5",
                        DataType = 1,
                        StateId = null,
                        Text = "TestText",
                        BackColor = 1,
                        ForeColor = 1,
                        ParamStandardType = 1,
                        Decimals = 1,
                        FacilityTagAlarm = "TestCurrentValueFloat"
                    },
                    new FacilityTypeResult
                    {
                        FacilityNodeId = "TestFacilityNodeId",
                        GroupNodeId = "TestNode",
                        AlarmState = 3,
                        AlarmTextColor = "TestAlarmTextColor",
                        Description = "TestCurrentValueDate",
                        AlarmTextHi = "TestAlarmTextHi",
                        AlarmTextLo = "TestAlarmTextLo",
                        CurrentValue = "111111",
                        DataType = 9,
                        StateId = null,
                        Text = "TestText",
                        BackColor = 1,
                        ForeColor = 1,
                        ParamStandardType = 1,
                        Decimals = 1,
                        FacilityTagAlarm = "TestCurrentValueDate"
                    },
                    new FacilityTypeResult
                    {
                        FacilityNodeId = "TestFacilityNodeId",
                        GroupNodeId = "TestNode",
                        AlarmState = 1,
                        AlarmTextColor = "TestAlarmTextColor",
                        Description = "TestCurrentValueInt",
                        AlarmTextHi = "TestAlarmTextHi",
                        AlarmTextLo = "TestAlarmTextLo",
                        CurrentValue = "5",
                        DataType = 1,
                        StateId = null,
                        Text = "TestText",
                        BackColor = 1,
                        ForeColor = 1,
                        ParamStandardType = 1,
                        Decimals = 1,
                        FacilityTagAlarm = "TestFacilityTagAlarm"
                    },
                    new FacilityTypeResult
                    {
                        FacilityNodeId = "TestFacilityNodeId",
                        GroupNodeId = "TestNode",
                        AlarmState = 1,
                        AlarmTextColor = "TestAlarmTextColor",
                        Description = "TestParameter",
                        AlarmTextHi = "TestAlarmTextHi",
                        AlarmTextLo = "TestAlarmTextLo",
                        CurrentValue = "TestCurrentValue",
                        DataType = 1,
                        StateId = 1,
                        Text = "TestText",
                        BackColor = 1,
                        ForeColor = 1,
                        ParamStandardType = 1,
                        Decimals = 1,
                        FacilityTagAlarm = "TestFacilityTagAlarm"
                    },
                });

            var viewColumns = new List<GroupStatusColumnsModels>
            {
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "TestColumnName",
                    UserId = "TestUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "TestAlias",
                    SourceId = (int)GroupStatusColumns.SourceType.Facility,
                    Align = 1,
                    Visible = 1,
                    Measure = "TestMeasure",
                    FormatId = 1,
                    Formula = "TestFormula",
                    ParamStandardType = 1,
                    FormatMask = "TestFormatMask",
                    UnitType = 1
                },
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "TestCurrentValueFloat",
                    UserId = "TestUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "TestAlias",
                    SourceId = (int)GroupStatusColumns.SourceType.Facility,
                    Align = 1,
                    Visible = 1,
                    Measure = "TestMeasure",
                    FormatId = 1,
                    Formula = "TestFormula",
                    ParamStandardType = 1,
                    FormatMask = "TestFormatMask",
                    UnitType = 1
                },
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "TestCurrentValueDate",
                    UserId = "TestUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "TestAlias",
                    SourceId = (int)GroupStatusColumns.SourceType.Facility,
                    Align = 1,
                    Visible = 1,
                    Measure = "TestMeasure",
                    FormatId = 1,
                    Formula = "TestFormula",
                    ParamStandardType = 1,
                    FormatMask = "TestFormatMask",
                    UnitType = 1
                },
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "TestCurrentValueInt",
                    UserId = "TestUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "TestAlias",
                    SourceId = (int)GroupStatusColumns.SourceType.Facility,
                    Align = 1,
                    Visible = 1,
                    Measure = "TestMeasure",
                    FormatId = 1,
                    Formula = "TestFormula",
                    ParamStandardType = 1,
                    FormatMask = "TestFormatMask",
                    UnitType = 1
                },
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "TestParameter",
                    UserId = "TestUserId",
                    ColumnId = 2,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "TestAlias",
                    SourceId = (int)GroupStatusColumns.SourceType.Parameter,
                    Align = 1,
                    Visible = 1,
                    Measure = "TestMeasure",
                    FormatId = 1,
                    Formula = "TestFormula",
                    ParamStandardType = 1,
                    FormatMask = "TestFormatMask",
                    UnitType = 1
                },
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "FacilityTagAlarms",
                    UserId = "TestUserId",
                    ColumnId = 3,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "TestAlias",
                    SourceId = (int)GroupStatusColumns.SourceType.Common,
                    Align = 1,
                    Visible = 1,
                    Measure = "TestMeasure",
                    FormatId = 1,
                    Formula = "TestFormula",
                    ParamStandardType = 1,
                    FormatMask = "TestFormatMask",
                    UnitType = 1
                },
            };

            _groupStatus.Setup(gs => gs.LoadViewColumns(It.IsAny<string>(), It.IsAny<string>())).Returns(viewColumns);

            _groupStatus.Setup(x => x.GroupStatusFacilityTag(It.IsAny<string>(), It.IsAny<string>())).Returns("1");

            _groupStatus.Setup(gs => gs.BuildSQLCommonResult(
                    It.IsAny<IList<string>>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<string>(),
                    It.IsAny<IList<string>>(), It.IsAny<string>()))
                .Returns(new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        {
                            "Well", "TestNode"
                        },
                    },
                });

            var groupAndAssetModel = new GroupAndAssetModel
            {
                GroupName = "TestGroup",
                Assets = new List<AssetModel>
                {
                    new AssetModel
                    {
                        AssetId = new Guid(),
                        AssetName = "Asset1",
                        IndustryApplicationId = 3
                    }
                }
            };

            _groupAndAsset.Setup(x => x.GetGroupAssetAndRelationshipData(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(groupAndAssetModel);

            _nodeMaster.Setup(x => x.GetNodeIdsByAssetGuid(It.IsAny<string[]>(), It.IsAny<string>())).Returns(new List<NodeMasterModel>()
            {
                new NodeMasterModel()
                {
                    NodeId = "TestNodeId",
                }
            });

            var requestWithCorrelationId = new WithCorrelationId<GroupStatusInput>(Guid.NewGuid().ToString(), new GroupStatusInput
            {
                ViewId = "TestViewId",
                GroupName = "TestGroup"
            });

            // Act
            var result = _groupStatusProcessingService.GetGroupStatus(requestWithCorrelationId);

            // Assert
            Assert.IsTrue(result.Result.Status);
            Assert.IsNotNull(result.Result.Value);
            Assert.AreEqual(19, result.Values.Columns.Count);
            Assert.AreEqual(1, result.Values.Rows.Count);
        }

        [TestMethod]
        public void GetGroupStatusParamStandardTest()
        {
            _groupStatus.Setup(x =>
                    x.BuildSQLParamStandardTypeResult(It.IsAny<IList<FieldParamStandardTypeNameValues>>(),
                        It.IsAny<IList<string>>(), It.IsAny<string>()))
                .Returns(new List<ParamStandardTypeSumResult>
                {
                    new ParamStandardTypeSumResult
                    {
                        NodeId = "TestNode",
                        ParamStandardTypeId = 1,
                        SumValue = 0.0f
                    }
                });

            _groupStatus.Setup(x =>
                x.BuildSQLParamStandardTypeStateResult(It.IsAny<IList<FieldParamStandardTypeNameValues>>(),
                    It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(new List<ParamStandardTypeMaxResult>
            {
                new ParamStandardTypeMaxResult
                {
                    NodeId = "TestNode",
                    ParamStandardTypeId = 1,
                    MaxValue = "TestMaxValue"
                }
            });

            var viewColumns = new List<GroupStatusColumnsModels>
            {
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "TestColumnName",
                    UserId = "TestUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "TestAlias",
                    SourceId = (int)GroupStatusColumns.SourceType.ParamStandard,
                    Align = 1,
                    Visible = 1,
                    Measure = "TestMeasure",
                    FormatId = 1,
                    Formula = "TestFormula",
                    ParamStandardType = 1,
                    FormatMask = "TestFormatMask",
                    UnitType = 1
                },
            };

            _groupStatus.Setup(gs => gs.LoadViewColumns(It.IsAny<string>(), It.IsAny<string>())).Returns(viewColumns);

            _groupStatus.Setup(x => x.GroupStatusFacilityTag(It.IsAny<string>(), It.IsAny<string>())).Returns("1");

            _groupStatus.Setup(gs => gs.BuildSQLCommonResult(
                    It.IsAny<IList<string>>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>(),
                    It.IsAny<string>(),
                    It.IsAny<IList<string>>(), It.IsAny<string>()))
                .Returns(new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        {
                            "Well", "TestNode"
                        },
                    },
                });

            var groupAndAssetModel = new GroupAndAssetModel
            {
                GroupName = "TestGroup",
                Assets = new List<AssetModel>
                {
                    new AssetModel
                    {
                        AssetId = new Guid(),
                        AssetName = "Asset1",
                        IndustryApplicationId = 3
                    }
                }
            };

            _groupAndAsset.Setup(x => x.GetGroupAssetAndRelationshipData(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(groupAndAssetModel);

            _nodeMaster.Setup(x => x.GetNodeIdsByAssetGuid(It.IsAny<string[]>(), It.IsAny<string>())).Returns(new List<NodeMasterModel>()
            {
                new NodeMasterModel()
                {
                    NodeId = "TestNodeId",
                }
            });

            var requestWithCorrelationId = new WithCorrelationId<GroupStatusInput>(Guid.NewGuid().ToString(), new GroupStatusInput
            {
                ViewId = "TestViewId",
                GroupName = "TestGroup",
            });

            // Act
            var result = _groupStatusProcessingService.GetGroupStatus(requestWithCorrelationId);

            // Assert
            Assert.IsTrue(result.Result.Status);
            Assert.IsNotNull(result.Result.Value);
            Assert.AreEqual(2, result.Values.Columns.Count);
            Assert.AreEqual(1, result.Values.Rows.Count);
        }

        [TestMethod]
        public void GetGroupStatusClassificationTest()
        {
            var groupAndAssetModel = new GroupAndAssetModel
            {
                GroupName = "TestGroup",
                ChildGroups = new List<GroupAndAssetModel>
                {
                    new GroupAndAssetModel
                    {
                        Assets = new List<AssetModel>
                        {
                            new AssetModel
                            {
                                AssetId = new Guid(),
                                AssetName = "Asset1",
                                IndustryApplicationId = 3
                            }
                        }
                    },
                },
            };
            _groupAndAsset.Setup(x => x.GetGroupAssetAndRelationshipData(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(groupAndAssetModel);
            var requestWithCorrelationId = new WithCorrelationId<GroupStatusInput>(Guid.NewGuid().ToString(),
                new GroupStatusInput
                {
                    GroupName = "TestGroup",
                });

            var responseData = new List<AssetGroupStatusClassificationModel>
            {
                new AssetGroupStatusClassificationModel
                {
                    Id = 25,
                    Name = "Gearbox Overloaded",
                    Count = 5,
                },
                new AssetGroupStatusClassificationModel
                {
                    Id = 27,
                    Name = "Motor Overloaded",
                    Count = 12
                }
            };
            int assetCount = 0;
            _groupStatus.Setup(x => x.GetClassificationsData(It.IsAny<List<string>>(), It.IsAny<string>(), out assetCount))
                .Returns(responseData);

            var response = _groupStatusProcessingService.GetClassificationWidgetData(requestWithCorrelationId);

            Assert.IsTrue(response.Result.Status);
            Assert.IsNotNull(response.ClassificationValues);
            Assert.AreEqual(2, response.ClassificationValues.Count);
            Assert.AreEqual(27, response.ClassificationValues[0].Id);
            Assert.AreEqual(25, response.ClassificationValues[1].Id);
        }

        [TestMethod]
        public void GetGroupStatusClassificationNullRequestTest()
        {
            var result = _groupStatusProcessingService.GetClassificationWidgetData(null);

            Assert.IsFalse(result.Result.Status);
            _logger.Verify(x => x.Write(Level.Info,
                    It.Is<string>(x => x.Contains("request is null, cannot get group widgets."))),
                Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetGroupStatusClassificationNullRequestValueTest()
        {
            var requestWithCorrelationId = new WithCorrelationId<GroupStatusInput>("correlationId1", null);
            var response = _groupStatusProcessingService.GetClassificationWidgetData(requestWithCorrelationId);

            Assert.IsFalse(response.Result.Status);
            _logger.Verify(x => x.WriteCId(Level.Info,
                It.Is<string>(x => x.Contains("request is null, cannot get group widgets.")),
                "correlationId1"), Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetGroupStatusClassificationEmptyAssetIdsTest()
        {
            _groupAndAsset.Setup(x => x.GetGroupAssetAndRelationshipData(It.IsAny<string>(), It.IsAny<string>()));

            var requestWithCorrelationId = new WithCorrelationId<GroupStatusInput>("correlationId1", new GroupStatusInput
            {
                GroupName = "TestGroup",
            });

            var result = _groupStatusProcessingService.GetClassificationWidgetData(requestWithCorrelationId);

            Assert.IsFalse(result.Result.Status);
            _logger.Verify(x => x.WriteCId(Level.Info,
                It.Is<string>(x => x.Contains("classificationsData is null, cannot get group widgets.")),
                "correlationId1"), Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetGroupStatusClassificationEmptyResponseTest()
        {
            var groupAndAssetModel = new GroupAndAssetModel
            {
                GroupName = "TestGroup",
                ChildGroups = new List<GroupAndAssetModel>
                {
                    new GroupAndAssetModel
                    {
                        Assets = new List<AssetModel>
                        {
                            new AssetModel
                            {
                                AssetId = new Guid(),
                                AssetName = "Asset1",
                                IndustryApplicationId = 3
                            }
                        }
                    },
                },
            };
            _groupAndAsset.Setup(x => x.GetGroupAssetAndRelationshipData(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(groupAndAssetModel);
            var requestWithCorrelationId = new WithCorrelationId<GroupStatusInput>("correlationId1", new GroupStatusInput
            {
                GroupName = "TestGroup",
            });
            int assetCount;
            _groupStatus.Setup(x => x.GetClassificationsData(It.IsAny<List<string>>(), It.IsAny<string>(), out assetCount));

            var result = _groupStatusProcessingService.GetClassificationWidgetData(requestWithCorrelationId);

            Assert.IsFalse(result.Result.Status);
            _logger.Verify(x => x.WriteCId(Level.Info,
                It.Is<string>(x => x.Contains("classificationsData is null, cannot get group widgets.")),
                "correlationId1"), Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetGroupStatusAlarmsTest()
        {
            var groupAndAssetModel = new GroupAndAssetModel
            {
                GroupName = "TestGroup",
                ChildGroups = new List<GroupAndAssetModel>
                {
                    new GroupAndAssetModel
                    {
                        Assets = new List<AssetModel>
                        {
                            new AssetModel
                            {
                                AssetId = new Guid(),
                                AssetName = "Asset1",
                                IndustryApplicationId = 3
                            }
                        }
                    },
                },
            };
            _groupAndAsset.Setup(x => x.GetGroupAssetAndRelationshipData(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(groupAndAssetModel);
            var requestWithCorrelationId = new WithCorrelationId<GroupStatusInput>(Guid.NewGuid().ToString(),
                new GroupStatusInput
                {
                    GroupName = "TestGroup",
                });

            var responseData = new AssetGroupStatusAlarmsModel
            {
                Id = 25,
                Name = "Gearbox Overloaded",
                Count = 25,
                Percent = 2
            };

            _groupStatus.Setup(x => x.GetAlarmsData(It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(Task.FromResult(responseData));

            var response = _groupStatusProcessingService.GetAlarmsWidgetDataAsync(requestWithCorrelationId);

            Assert.IsNotNull(response.Result);
            Assert.IsNotNull(response.Result.Result.Value);
            Assert.IsTrue(response.Result.Values[0].Count == 25);
        }

        [TestMethod]
        public async Task GetGroupStatusDowntimeByWellsTest()
        {
            var testAssetNameRodLiftLegacyGuid = Guid.NewGuid();
            var testAssetNameESPLegacyGuid = Guid.NewGuid();
            var testAssetNameGLLegacyGuid = Guid.NewGuid();
            var testAssetNameRodLiftGuid = Guid.NewGuid();
            var testAssetNameESPGuid = Guid.NewGuid();
            var testAssetNameGLGuid = Guid.NewGuid();

            var groupAndAssetModel = new GroupAndAssetModel
            {
                GroupName = "TestGroupName",
                Assets = new List<AssetModel>
                {
                    new AssetModel
                    {
                        AssetId = testAssetNameRodLiftLegacyGuid,
                        AssetName = "TestAssetNameRodLiftLegacy",
                        IndustryApplicationId = IndustryApplication.RodArtificialLift.Key
                    },
                    new AssetModel
                    {
                        AssetId = testAssetNameESPLegacyGuid,
                        AssetName = "TestAssetNameESPLegacy",
                        IndustryApplicationId = IndustryApplication.ESPArtificialLift.Key
                    },
                    new AssetModel
                    {
                        AssetId = testAssetNameGLLegacyGuid,
                        AssetName = "TestAssetNameGLLegacy",
                        IndustryApplicationId = IndustryApplication.GasArtificialLift.Key
                    },
                    new AssetModel
                    {
                        AssetId = testAssetNameRodLiftGuid,
                        AssetName = "TestAssetNameRodLift",
                        IndustryApplicationId = IndustryApplication.RodArtificialLift.Key
                    },
                    new AssetModel
                    {
                        AssetId = testAssetNameESPGuid,
                        AssetName = "TestAssetNameESP",
                        IndustryApplicationId = IndustryApplication.ESPArtificialLift.Key
                    },
                    new AssetModel
                    {
                        AssetId = testAssetNameGLGuid,
                        AssetName = "TestAssetNameGL",
                        IndustryApplicationId = IndustryApplication.GasArtificialLift.Key
                    },
                },
            };

            _groupAndAsset.Setup(x => x.GetGroupAssetAndRelationshipData(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(groupAndAssetModel);

            _nodeMaster.Setup(x => x.GetLegacyWellAsync(It.IsAny<IList<Guid>>(), It.IsAny<string>()))
                .ReturnsAsync(new Dictionary<Guid, bool>
                {
                    {
                        testAssetNameRodLiftLegacyGuid, true
                    },
                    {
                        testAssetNameESPLegacyGuid, true
                    },
                    {
                        testAssetNameGLLegacyGuid, true
                    },
                    {
                        testAssetNameRodLiftGuid, false
                    },
                    {
                        testAssetNameESPGuid, false
                    },
                    {
                        testAssetNameGLGuid, false
                    },
                });

            _nodeMaster.Setup(x => x.GetByAssetIdsAsync(It.IsAny<IList<Guid>>(), It.IsAny<string>())).ReturnsAsync((IList<Guid> guidList, string correlationId) =>
            {
                var result = new List<NodeMasterModel>();

                foreach (var item in guidList)
                {
                    if (item == testAssetNameRodLiftLegacyGuid || item == testAssetNameRodLiftGuid)
                    {
                        result.Add(new NodeMasterModel()
                        {
                            AssetGuid = item,
                            ApplicationId = IndustryApplication.RodArtificialLift.Key,
                            PocType = 8,
                        });
                    }

                    if (item == testAssetNameESPLegacyGuid || item == testAssetNameESPGuid)
                    {
                        result.Add(new NodeMasterModel()
                        {
                            AssetGuid = item,
                            ApplicationId = IndustryApplication.ESPArtificialLift.Key,
                            PocType = 39,
                        });
                    }

                    if (item == testAssetNameGLLegacyGuid || item == testAssetNameGLGuid)
                    {
                        result.Add(new NodeMasterModel()
                        {
                            AssetGuid = item,
                            ApplicationId = IndustryApplication.GasArtificialLift.Key,
                            PocType = 219,
                        });
                    }
                }

                return result;
            });

            _trendDataStore.Setup(x => x.GetDowntime(It.IsAny<IList<string>>(), It.IsAny<int>(), It.IsAny<string>())).Returns(
                new DowntimeByWellsModel()
                {
                    RodPump = new List<DowntimeByWellsRodPumpModel>()
                    {
                        new DowntimeByWellsRodPumpModel()
                        {
                            Id = "TestAssetNameRodLiftLegacy",
                            Date = DateTime.UtcNow.Date.AddDays(-1),
                            Runtime = 10,
                            IdleTime = 10,
                            Cycles = 10,
                        }
                    },
                    ESP = new List<DowntimeByWellsValueModel>()
                    {
                        new DowntimeByWellsValueModel()
                        {
                            Id = "TestAssetNameESPLegacy",
                            Date = DateTime.UtcNow.Date.AddDays(-1),
                            Value = 10,
                        },
                        new DowntimeByWellsValueModel()
                        {
                            Id = "TestAssetNameESPLegacy",
                            Date = DateTime.UtcNow.Date.AddDays(-1),
                            Value = 0,
                        },
                    },
                    GL = new List<DowntimeByWellsValueModel>()
                    {
                        new DowntimeByWellsValueModel()
                        {
                            Id = "TestAssetNameGLLegacy",
                            Date = DateTime.UtcNow.Date.AddDays(-1),
                            Value = 10,
                        },
                        new DowntimeByWellsValueModel()
                        {
                            Id = "TestAssetNameGLLegacy",
                            Date = DateTime.UtcNow.Date.AddDays(-1),
                            Value = 0,
                        },
                    }
                });

            _trendDataInfluxStore.Setup(x =>
                    x.GetDowntimeAsync(It.IsAny<IList<DowntimeFiltersWithChannelIdInflux>>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(
                    (IList<DowntimeFiltersWithChannelIdInflux> filters, string startDate, string endDate) =>
                    {
                        var result = new List<DowntimeByWellsInfluxModel>();

                        foreach (var filter in filters)
                        {
                            foreach (var assetId in filter.AssetIds)
                            {
                                if (assetId == testAssetNameRodLiftGuid)
                                {
                                    result.Add(new DowntimeByWellsInfluxModel()
                                    {
                                        Id = assetId.ToString(),
                                        Date = DateTime.UtcNow.Date.AddDays(-1),
                                        Value = 10,
                                        ParamStandardType = "179",
                                    });

                                    result.Add(new DowntimeByWellsInfluxModel()
                                    {
                                        Id = assetId.ToString(),
                                        Date = DateTime.UtcNow.Date.AddDays(-1),
                                        Value = 10,
                                        ParamStandardType = "180",
                                    });

                                    result.Add(new DowntimeByWellsInfluxModel()
                                    {
                                        Id = assetId.ToString(),
                                        Date = DateTime.UtcNow.Date.AddDays(-1),
                                        Value = 10,
                                        ParamStandardType = "181",
                                    });
                                }

                                if (assetId == testAssetNameESPGuid)
                                {
                                    result.Add(new DowntimeByWellsInfluxModel()
                                    {
                                        Id = assetId.ToString(),
                                        Date = DateTime.UtcNow.Date.AddDays(-1),
                                        Value = 1,
                                        ParamStandardType = "2",
                                    });

                                    result.Add(new DowntimeByWellsInfluxModel()
                                    {
                                        Id = assetId.ToString(),
                                        Date = DateTime.UtcNow.Date.AddDays(-1),
                                        Value = 0,
                                        ParamStandardType = "2",
                                    });
                                }

                                if (assetId == testAssetNameGLGuid)
                                {
                                    result.Add(new DowntimeByWellsInfluxModel()
                                    {
                                        Id = assetId.ToString(),
                                        Date = DateTime.UtcNow.Date.AddDays(-1),
                                        Value = 1,
                                        ParamStandardType = "191",
                                    });

                                    result.Add(new DowntimeByWellsInfluxModel()
                                    {
                                        Id = assetId.ToString(),
                                        Date = DateTime.UtcNow.Date.AddDays(-1),
                                        Value = 0,
                                        ParamStandardType = "191",
                                    });
                                }
                            }
                        }

                        return result;
                    });

            var param = new List<Parameters>();
            param.Add(new Parameters { ChannelId = "C123" });
            _mongoDbStore.Setup(x => x.GetParameterByParamStdType(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(param);

            var requestWithCorrelationId = new WithCorrelationId<GroupStatusInput>("12345", new GroupStatusInput
            {
                GroupName = "TestGroupName",
            });

            var result = await _groupStatusProcessingService.GetDowntimeByWellsAsync(requestWithCorrelationId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result.Status);
            Assert.IsNotNull(result.Result.Value);
            Assert.AreEqual(6, result.Assets.Count);
            Assert.AreEqual(testAssetNameESPLegacyGuid.ToString(), result.Assets[0].Id);
            Assert.AreEqual("TestAssetNameESPLegacy", result.Assets[0].Name);
            Assert.AreEqual(84, result.Assets[0].Count);
            Assert.AreEqual(50, result.Assets[0].Percent);
            Assert.AreEqual(testAssetNameESPGuid.ToString(), result.Assets[1].Id);
            Assert.AreEqual("TestAssetNameESP", result.Assets[1].Name);
            Assert.AreEqual(84, result.Assets[1].Count);
            Assert.AreEqual(50, result.Assets[1].Percent);
            Assert.AreEqual(testAssetNameGLLegacyGuid.ToString(), result.Assets[2].Id);
            Assert.AreEqual("TestAssetNameGLLegacy", result.Assets[2].Name);
            Assert.AreEqual(84, result.Assets[2].Count);
            Assert.AreEqual(50, result.Assets[2].Percent);
            Assert.AreEqual(testAssetNameGLGuid.ToString(), result.Assets[3].Id);
            Assert.AreEqual("TestAssetNameGL", result.Assets[3].Name);
            Assert.AreEqual(84, result.Assets[3].Count);
            Assert.AreEqual(50, result.Assets[3].Percent);
            Assert.AreEqual(testAssetNameRodLiftLegacyGuid.ToString(), result.Assets[4].Id);
            Assert.AreEqual("TestAssetNameRodLiftLegacy", result.Assets[4].Name);
            Assert.AreEqual(12, result.Assets[4].Count);
            Assert.AreEqual(7.3, result.Assets[4].Percent);
            Assert.AreEqual(testAssetNameRodLiftGuid.ToString(), result.Assets[5].Id);
            Assert.AreEqual("TestAssetNameRodLift", result.Assets[5].Name);
            Assert.AreEqual(12, result.Assets[5].Count);
            Assert.AreEqual(7.3, result.Assets[5].Percent);
            Assert.AreEqual(7, result.GroupByDuration.Count);
            Assert.AreEqual("Less6", result.GroupByDuration[0].Id);
            Assert.AreEqual("< 6 hours", result.GroupByDuration[0].Name);
            Assert.AreEqual(0, result.GroupByDuration[0].Count);
            Assert.AreEqual(0, result.GroupByDuration[0].Percent);
            Assert.AreEqual("Less12", result.GroupByDuration[1].Id);
            Assert.AreEqual("6-12 hours", result.GroupByDuration[1].Name);
            Assert.AreEqual(2, result.GroupByDuration[1].Count);
            Assert.AreEqual(33.3, result.GroupByDuration[1].Percent);
            Assert.AreEqual("Less24", result.GroupByDuration[2].Id);
            Assert.AreEqual("12-24 hours", result.GroupByDuration[2].Name);
            Assert.AreEqual(0, result.GroupByDuration[2].Count);
            Assert.AreEqual(0, result.GroupByDuration[2].Percent);
            Assert.AreEqual("Less48", result.GroupByDuration[3].Id);
            Assert.AreEqual("24-48 hours", result.GroupByDuration[3].Name);
            Assert.AreEqual(0, result.GroupByDuration[3].Count);
            Assert.AreEqual(0, result.GroupByDuration[3].Percent);
            Assert.AreEqual("Less72", result.GroupByDuration[4].Id);
            Assert.AreEqual("48-72 hours", result.GroupByDuration[4].Name);
            Assert.AreEqual(0, result.GroupByDuration[4].Count);
            Assert.AreEqual(0, result.GroupByDuration[4].Percent);
            Assert.AreEqual("Less96", result.GroupByDuration[5].Id);
            Assert.AreEqual("72-96 hours", result.GroupByDuration[5].Name);
            Assert.AreEqual(4, result.GroupByDuration[5].Count);
            Assert.AreEqual(66.7, result.GroupByDuration[5].Percent);
            Assert.AreEqual("Greater96", result.GroupByDuration[6].Id);
            Assert.AreEqual("> 96 hours", result.GroupByDuration[6].Name);
            Assert.AreEqual(0, result.GroupByDuration[6].Count);
            Assert.AreEqual(0, result.GroupByDuration[6].Percent);
        }

        [TestMethod]
        public async Task GetGroupStatusDowntimeByWellsInputNullTest()
        {
            var result = await _groupStatusProcessingService.GetDowntimeByWellsAsync(null);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Result.Status);
        }

        [TestMethod]
        public async Task GetGroupStatusDowntimeByWellsInputValueNullTest()
        {
            var requestWithCorrelationId = new WithCorrelationId<GroupStatusInput>("12345", null);

            var result = await _groupStatusProcessingService.GetDowntimeByWellsAsync(requestWithCorrelationId);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Result.Status);
        }

        [TestMethod]
        public async Task GetGroupStatusDowntimeByWellsNoAssetsTest()
        {
            _groupAndAsset.Setup(x => x.GetGroupAssetAndRelationshipData(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new GroupAndAssetModel());

            var requestWithCorrelationId = new WithCorrelationId<GroupStatusInput>("12345", new GroupStatusInput
            {
                GroupName = "TestGroupName",
            });

            var result = await _groupStatusProcessingService.GetDowntimeByWellsAsync(requestWithCorrelationId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result.Status);
        }

        [TestMethod]
        public async Task GetGroupStatusDowntimeByWellsMissingNodeMasterDataTest()
        {
            var testAssetNameRodLiftLegacyGuid = Guid.NewGuid();
            var testAssetNameESPLegacyGuid = Guid.NewGuid();
            var testAssetNameGLLegacyGuid = Guid.NewGuid();
            var testAssetNameRodLiftGuid = Guid.NewGuid();
            var testAssetNameESPGuid = Guid.NewGuid();
            var testAssetNameGLGuid = Guid.NewGuid();

            var groupAndAssetModel = new GroupAndAssetModel
            {
                GroupName = "TestGroupName",
                Assets = new List<AssetModel>
                {
                    new AssetModel
                    {
                        AssetId = testAssetNameRodLiftLegacyGuid,
                        AssetName = "TestAssetNameRodLiftLegacy",
                        IndustryApplicationId = IndustryApplication.RodArtificialLift.Key
                    },
                    new AssetModel
                    {
                        AssetId = testAssetNameESPLegacyGuid,
                        AssetName = "TestAssetNameESPLegacy",
                        IndustryApplicationId = IndustryApplication.ESPArtificialLift.Key
                    },
                    new AssetModel
                    {
                        AssetId = testAssetNameGLLegacyGuid,
                        AssetName = "TestAssetNameGLLegacy",
                        IndustryApplicationId = IndustryApplication.GasArtificialLift.Key
                    },
                    new AssetModel
                    {
                        AssetId = testAssetNameRodLiftGuid,
                        AssetName = "TestAssetNameRodLift",
                        IndustryApplicationId = IndustryApplication.RodArtificialLift.Key
                    },
                    new AssetModel
                    {
                        AssetId = testAssetNameESPGuid,
                        AssetName = "TestAssetNameESP",
                        IndustryApplicationId = IndustryApplication.ESPArtificialLift.Key
                    },
                    new AssetModel
                    {
                        AssetId = testAssetNameGLGuid,
                        AssetName = "TestAssetNameGL",
                        IndustryApplicationId = IndustryApplication.GasArtificialLift.Key
                    },
                },
            };

            _groupAndAsset.Setup(x => x.GetGroupAssetAndRelationshipData(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(groupAndAssetModel);

            _nodeMaster.Setup(x => x.GetLegacyWellAsync(It.IsAny<IList<Guid>>(), It.IsAny<string>()))
                .ReturnsAsync(new Dictionary<Guid, bool>
                {
                    {
                        testAssetNameRodLiftLegacyGuid, true
                    },
                    {
                        testAssetNameESPLegacyGuid, true
                    },
                    {
                        testAssetNameGLLegacyGuid, true
                    },
                    {
                        testAssetNameRodLiftGuid, false
                    },
                    {
                        testAssetNameESPGuid, false
                    },
                    {
                        testAssetNameGLGuid, false
                    },
                });

            _nodeMaster.Setup(x => x.GetByAssetIdsAsync(It.IsAny<IList<Guid>>(), It.IsAny<string>()))
                .ReturnsAsync(() => null);

            var requestWithCorrelationId = new WithCorrelationId<GroupStatusInput>("12345", new GroupStatusInput
            {
                GroupName = "TestGroupName",
            });

            var result = await _groupStatusProcessingService.GetDowntimeByWellsAsync(requestWithCorrelationId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result.Status);
            Assert.IsNotNull(result.Result.Value);
            Assert.IsNull(result.Assets);
            Assert.IsNull(result.GroupByDuration);
        }

        [TestMethod]
        public async Task GetGroupStatusDowntimeByWellsNoNodeDataForInfluxTest()
        {
            var testAssetNameRodLiftLegacyGuid = Guid.NewGuid();
            var testAssetNameESPLegacyGuid = Guid.NewGuid();
            var testAssetNameGLLegacyGuid = Guid.NewGuid();
            var testAssetNameRodLiftGuid = Guid.NewGuid();
            var testAssetNameESPGuid = Guid.NewGuid();
            var testAssetNameGLGuid = Guid.NewGuid();

            var groupAndAssetModel = new GroupAndAssetModel
            {
                GroupName = "TestGroupName",
                Assets = new List<AssetModel>
                {
                    new AssetModel
                    {
                        AssetId = testAssetNameRodLiftLegacyGuid,
                        AssetName = "TestAssetNameRodLiftLegacy",
                        IndustryApplicationId = IndustryApplication.RodArtificialLift.Key
                    },
                    new AssetModel
                    {
                        AssetId = testAssetNameESPLegacyGuid,
                        AssetName = "TestAssetNameESPLegacy",
                        IndustryApplicationId = IndustryApplication.ESPArtificialLift.Key
                    },
                    new AssetModel
                    {
                        AssetId = testAssetNameGLLegacyGuid,
                        AssetName = "TestAssetNameGLLegacy",
                        IndustryApplicationId = IndustryApplication.GasArtificialLift.Key
                    },
                    new AssetModel
                    {
                        AssetId = testAssetNameRodLiftGuid,
                        AssetName = "TestAssetNameRodLift",
                        IndustryApplicationId = IndustryApplication.RodArtificialLift.Key
                    },
                    new AssetModel
                    {
                        AssetId = testAssetNameESPGuid,
                        AssetName = "TestAssetNameESP",
                        IndustryApplicationId = IndustryApplication.ESPArtificialLift.Key
                    },
                    new AssetModel
                    {
                        AssetId = testAssetNameGLGuid,
                        AssetName = "TestAssetNameGL",
                        IndustryApplicationId = IndustryApplication.GasArtificialLift.Key
                    },
                },
            };

            _groupAndAsset.Setup(x => x.GetGroupAssetAndRelationshipData(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(groupAndAssetModel);

            _nodeMaster.Setup(x => x.GetLegacyWellAsync(It.IsAny<IList<Guid>>(), It.IsAny<string>()))
                .ReturnsAsync(new Dictionary<Guid, bool>
                {
                    {
                        testAssetNameRodLiftLegacyGuid, true
                    },
                    {
                        testAssetNameESPLegacyGuid, true
                    },
                    {
                        testAssetNameGLLegacyGuid, true
                    },
                    {
                        testAssetNameRodLiftGuid, false
                    },
                    {
                        testAssetNameESPGuid, false
                    },
                    {
                        testAssetNameGLGuid, false
                    },
                });

            _nodeMaster.Setup(x => x.GetByAssetIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<string>())).ReturnsAsync(new List<NodeMasterModel>());

            var requestWithCorrelationId = new WithCorrelationId<GroupStatusInput>("12345", new GroupStatusInput
            {
                GroupName = "TestGroupName",
            });

            var result = await _groupStatusProcessingService.GetDowntimeByWellsAsync(requestWithCorrelationId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result.Status);
            Assert.AreEqual(0, result.Assets.Count);
            Assert.AreEqual(0, result.GroupByDuration.Count);
        }

        [TestMethod]
        public async Task GetGroupStatusDowntimeByWellsRodPumpMultipleDowntime0Test()
        {
            var testAssetNameRodLiftLegacyGuid = Guid.NewGuid();
            var groupAndAssetModel = new GroupAndAssetModel
            {
                GroupName = "TestGroupName",
                Assets = new List<AssetModel>
                {
                    new AssetModel
                    {
                        AssetId = testAssetNameRodLiftLegacyGuid,
                        AssetName = "TestAssetNameRodLiftLegacy",
                        IndustryApplicationId = IndustryApplication.RodArtificialLift.Key
                    },
                },
            };

            _groupAndAsset.Setup(x => x.GetGroupAssetAndRelationshipData(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(groupAndAssetModel);

            _nodeMaster.Setup(x => x.GetLegacyWellAsync(It.IsAny<IList<Guid>>(), It.IsAny<string>()))
                .ReturnsAsync(new Dictionary<Guid, bool>
                {
                    {
                        testAssetNameRodLiftLegacyGuid, true
                    },
                });

            _trendDataStore.Setup(x => x.GetDowntime(It.IsAny<IList<string>>(), It.IsAny<int>(), It.IsAny<string>())).Returns(
                new DowntimeByWellsModel()
                {
                    RodPump = new List<DowntimeByWellsRodPumpModel>()
                    {
                        new DowntimeByWellsRodPumpModel()
                        {
                            Id = "TestAssetNameRodLiftLegacy",
                            Date = DateTime.UtcNow.Date.AddDays(-1),
                            Runtime = 24,
                            IdleTime = 10,
                            Cycles = 10,
                        },
                        new DowntimeByWellsRodPumpModel()
                        {
                            Id = "TestAssetNameRodLiftLegacy",
                            Date = DateTime.UtcNow.Date.AddDays(-1),
                            Runtime = 24,
                            IdleTime = 10,
                            Cycles = 10,
                        }
                    },
                    ESP = new List<DowntimeByWellsValueModel>(),
                    GL = new List<DowntimeByWellsValueModel>(),
                });

            var requestWithCorrelationId = new WithCorrelationId<GroupStatusInput>("12345", new GroupStatusInput
            {
                GroupName = "TestGroupName",
            });

            var result = await _groupStatusProcessingService.GetDowntimeByWellsAsync(requestWithCorrelationId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result.Status);
            Assert.IsNotNull(result.Result.Value);
            Assert.AreEqual(0, result.Assets.Count);
            Assert.AreEqual(0, result.GroupByDuration.Count);
        }

        [TestMethod]
        public void GetGroupStatusNoRowsTest()
        {
            //// Arrange
            var correlationId = Guid.NewGuid().ToString();
            var viewColumns = new List<GroupStatusColumnsModels>
            {
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "TestColumnName",
                    UserId = "TestUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "TestAlias",
                    SourceId = 1,
                    Align = 1,
                    Visible = 1,
                    Measure = "TestMeasure",
                    FormatId = 1,
                    Formula = "TestFormula",
                    ParamStandardType = 1,
                    FormatMask = "TestFormatMask",
                    UnitType = 1
                },
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "CAMERAALARMS",
                    UserId = "TestUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "TestAlias",
                    SourceId = 1,
                    Align = 1,
                    Visible = 1,
                    Measure = "TestMeasure",
                    FormatId = 1,
                    Formula = "TestFormula",
                    ParamStandardType = 1,
                    FormatMask = "TestFormatMask",
                    UnitType = 1
                },
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "OPERATIONALSCORE",
                    UserId = "TestUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "TestAlias",
                    SourceId = 1,
                    Align = 1,
                    Visible = 1,
                    Measure = "TestMeasure",
                    FormatId = 1,
                    Formula = "TestFormula",
                    ParamStandardType = 1,
                    FormatMask = "TestFormatMask",
                    UnitType = 1
                },
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "%RT 30D",
                    UserId = "TestUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "TestAlias",
                    SourceId = 1,
                    Align = 1,
                    Visible = 1,
                    Measure = "TestMeasure",
                    FormatId = 1,
                    Formula = "TestFormula",
                    ParamStandardType = 1,
                    FormatMask = "TestFormatMask",
                    UnitType = 1
                },
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "CAMERAALARMS",
                    UserId = "TestUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "TestAlias",
                    SourceId = (int)GroupStatusColumns.SourceType.Parameter,
                    Align = 1,
                    Visible = 1,
                    Measure = "TestMeasure",
                    FormatId = 1,
                    Formula = "TestFormula",
                    ParamStandardType = 1,
                    FormatMask = "TestFormatMask",
                    UnitType = 1
                },
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "OPERATIONALSCORE",
                    UserId = "TestUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "TestAlias",
                    SourceId = (int)GroupStatusColumns.SourceType.Facility,
                    Align = 1,
                    Visible = 1,
                    Measure = "TestMeasure",
                    FormatId = 1,
                    Formula = "TestFormula",
                    ParamStandardType = 1,
                    FormatMask = "TestFormatMask",
                    UnitType = 1
                },
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "%RT 30D",
                    UserId = "TestUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "TestAlias",
                    SourceId = (int)GroupStatusColumns.SourceType.ParamStandard,
                    Align = 1,
                    Visible = 1,
                    Measure = "TestMeasure",
                    FormatId = 1,
                    Formula = "TestFormula",
                    ParamStandardType = 1,
                    FormatMask = "TestFormatMask",
                    UnitType = 1
                },
                new GroupStatusColumnsModels
                {
                    ViewId = 1,
                    ColumnName = "%RT 30D",
                    UserId = "TestUserId",
                    ColumnId = 1,
                    Width = 1,
                    Position = 1,
                    Orientation = 1,
                    Alias = "TestAlias",
                    SourceId = (int)GroupStatusColumns.SourceType.Formula,
                    Align = 1,
                    Visible = 1,
                    Measure = "TestMeasure",
                    FormatId = 1,
                    Formula = "TestFormula",
                    ParamStandardType = 1,
                    FormatMask = "TestFormatMask",
                    UnitType = 1
                },
            };

            var groupAndAssetModel = new GroupAndAssetModel
            {
                GroupName = "TestGroup",
                Assets = new List<AssetModel>(),
            };

            _groupStatus.Setup(x => x.LoadViewColumns(It.IsAny<string>(), correlationId)).Returns(viewColumns);
            _groupAndAsset.Setup(x => x.GetGroupAssetAndRelationshipData(correlationId, It.IsAny<string>()))
                .Returns(groupAndAssetModel);

            var request = new WithCorrelationId<GroupStatusInput>(correlationId, new GroupStatusInput
            {
                ViewId = "TestViewId",
                GroupName = "TestGroup"
            });

            // Act
            GroupStatusOutput result = _groupStatusProcessingService.GetGroupStatus(request);

            // Assert
            Assert.IsTrue(result.Result.Status);
            Assert.IsNotNull(result.Result.Value);
            Assert.AreEqual(65, result.Values.Columns.Count);
            Assert.AreEqual(0, result.Values.Rows.Count);
        }

        [TestMethod]
        public void GetGroupStatusNoColumnsTest()
        {
            //// Arrange
            var correlationId = Guid.NewGuid().ToString();
            var viewColumns = new List<GroupStatusColumnsModels>();

            var testAssetNameRodLiftLegacyGuid = Guid.NewGuid();

            _groupStatus.Setup(x => x.LoadViewColumns(It.IsAny<string>(), correlationId)).Returns(viewColumns);

            var groupAndAssetModel = new GroupAndAssetModel
            {
                GroupName = "TestGroupName",
                Assets = new List<AssetModel>
                {
                    new AssetModel
                    {
                        AssetId = testAssetNameRodLiftLegacyGuid,
                        AssetName = "TestAssetNameRodLiftLegacy",
                        IndustryApplicationId = IndustryApplication.RodArtificialLift.Key
                    },
                },
            };

            _groupAndAsset.Setup(x => x.GetGroupAssetAndRelationshipData(correlationId, It.IsAny<string>()))
                .Returns(groupAndAssetModel);

            var request = new WithCorrelationId<GroupStatusInput>(correlationId, new GroupStatusInput
            {
                ViewId = "TestViewId",
                GroupName = "TestGroup"
            });

            // Act
            GroupStatusOutput result = _groupStatusProcessingService.GetGroupStatus(request);

            // Assert
            Assert.IsTrue(result.Result.Status);
            Assert.IsNotNull(result.Result.Value);
            Assert.AreEqual(1, result.Values.Columns.Count);
            Assert.AreEqual(0, result.Values.Rows.Count);
        }

        [TestMethod]
        public async Task GetDowntimeByRunStatusAsyncTest()
        {
            // Arrange
            var requestWithCorrelationId = new WithCorrelationId<GroupStatusInput>("12345", new GroupStatusInput
            {
                GroupName = "TestGroupName",
            });

            var groupAndAssetModel = new GroupAndAssetModel
            {
                GroupName = "TestGroupName",
                Assets = new List<AssetModel>
                {
                    new AssetModel
                    {
                        AssetId = new Guid(),
                        AssetName = "TestAssetNameShutdown",
                        IndustryApplicationId = 3
                    },
                    new AssetModel()
                    {
                        AssetId = new Guid(),
                        AssetName = "TestAssetNameShutdownAuto",
                        IndustryApplicationId = 3
                    },
                    new AssetModel()
                    {
                        AssetId = new Guid(),
                        AssetName = "TestAssetNameShutdownAuto",
                        IndustryApplicationId = 3
                    },
                    new AssetModel()
                    {
                        AssetId = new Guid(),
                        AssetName = "TestAssetNameIdle",
                        IndustryApplicationId = 3
                    },
                    new AssetModel()
                    {
                        AssetId = new Guid(),
                        AssetName = "TestAssetNameRunning",
                        IndustryApplicationId = 3
                    },
                }
            };

            _groupAndAsset.Setup(s => s.GetGroupAssetAndRelationshipData(It.IsAny<string>(),
                It.IsAny<string>())).Returns(groupAndAssetModel);

            _nodeMaster.Setup(x => x.GetByAssetIdsAsync(It.IsAny<IList<Guid>>(), It.IsAny<string>()))
                .ReturnsAsync((IList<Guid> guidList, string correlationId) =>
            {
                var result = new List<NodeMasterModel>();

                result.Add(new NodeMasterModel()
                {
                    AssetGuid = guidList[0],
                    RunStatus = "Shutdown",
                });

                result.Add(new NodeMasterModel()
                {
                    AssetGuid = guidList[1],
                    RunStatus = "Shutdown, Auto",
                });

                result.Add(new NodeMasterModel()
                {
                    AssetGuid = guidList[2],
                    RunStatus = "Shutdown, Auto",
                });

                result.Add(new NodeMasterModel()
                {
                    AssetGuid = guidList[3],
                    RunStatus = "Idle",
                });

                result.Add(new NodeMasterModel()
                {
                    AssetGuid = guidList[4],
                    RunStatus = "Running",
                });

                return result;
            });

            // Act
            var result = await _groupStatusProcessingService.GetDowntimeByRunStatusAsync(requestWithCorrelationId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Values.Count);
            Assert.AreEqual("Shutdown, Auto", result.Values[0].Id);
            Assert.AreEqual(2, result.Values[0].Count);
            Assert.AreEqual(40, result.Values[0].Percent);
            Assert.AreEqual("Shutdown", result.Values[1].Id);
            Assert.AreEqual(1, result.Values[1].Count);
            Assert.AreEqual(20, result.Values[1].Percent);
        }

        [TestMethod]
        public async Task GetGroupStatusDowntimeByRunStatusInputNullTest()
        {
            var result = await _groupStatusProcessingService.GetDowntimeByRunStatusAsync(null);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Result.Status);
        }

        [TestMethod]
        public async Task GetGroupStatusDowntimeByRunStatusInputValueNullTest()
        {
            var requestWithCorrelationId = new WithCorrelationId<GroupStatusInput>("12345", null);

            var result = await _groupStatusProcessingService.GetDowntimeByRunStatusAsync(requestWithCorrelationId);

            Assert.IsNotNull(result);
            Assert.IsFalse(result.Result.Status);
        }

        [TestMethod]
        public async Task GetGroupStatusDowntimeByRunStatusNoAssetsTest()
        {
            var correlationId = Guid.NewGuid().ToString();
            _groupAndAsset.Setup(x => x.GetGroupAssetAndRelationshipData(correlationId, It.IsAny<string>()))
                .Returns(new GroupAndAssetModel());

            var requestWithCorrelationId = new WithCorrelationId<GroupStatusInput>(correlationId, new GroupStatusInput
            {
                GroupName = "TestGroupName",
            });

            var result = await _groupStatusProcessingService.GetDowntimeByRunStatusAsync(requestWithCorrelationId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result.Status);
        }

        [TestMethod]
        public async Task GetGroupStatusDowntimeByRunStatusNoNodeMasterDataTest()
        {
            var correlationId = Guid.NewGuid().ToString();
            _groupAndAsset.Setup(x => x.GetGroupAssetAndRelationshipData(correlationId, It.IsAny<string>()))
                .Returns(new GroupAndAssetModel()
                {
                    GroupName = "TestGroupName",
                    Assets = new List<AssetModel>
                    {
                        new AssetModel
                        {
                            AssetId = new Guid(),
                            AssetName = "TestAssetNameShutdown",
                            IndustryApplicationId = 3
                        },
                    }
                });

            _nodeMaster.Setup(x => x.GetByAssetIdsAsync(It.IsAny<IList<Guid>>(), correlationId)).ReturnsAsync((List<NodeMasterModel>)null);

            var requestWithCorrelationId = new WithCorrelationId<GroupStatusInput>(correlationId, new GroupStatusInput
            {
                GroupName = "TestGroupName",
            });

            var result = await _groupStatusProcessingService.GetDowntimeByRunStatusAsync(requestWithCorrelationId);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result.Status);
        }

        #endregion

        #region Private Methods

        private void SetupGroupStatus()
        {
            _groupStatus.Setup(x => x.LoadViewParameters(It.IsAny<string>(), It.IsAny<string>())).Returns(new SortedList<string, ParameterItem>());
            _groupStatus.Setup(x => x.GetItemsGroupStatus(It.IsAny<string[]>(), It.IsAny<string>())).Returns(new List<FacilityTagsModel>());
            _groupStatus.Setup(x => x.GetConditionalFormats(It.IsAny<string>(), It.IsAny<string>())).Returns(new List<ConditionalFormatModel>());
            _groupStatus.Setup(x => x.LoadViewColumns(It.IsAny<string>(), It.IsAny<string>())).Returns(new List<GroupStatusColumnsModels>());
            _groupStatus.Setup(x => x.BuildSQLCommonResult(It.IsAny<IList<string>>(), It.IsAny<bool>(), It.IsAny<bool>(),
                It.IsAny<bool>(), It.IsAny<string>(), It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(new List<Dictionary<string, object>>());
            _groupStatus.Setup(x => x.BuildSQLParameterResult(It.IsAny<IList<string>>(), It.IsAny<string>()))
                .Returns(new List<ParameterTypeResult>());
            _groupStatus.Setup(x => x.BuildSQLCurrRawScanData(It.IsAny<IList<string>>(), It.IsAny<string>()))
                .Returns(new List<CurrRawScanDataTypeResult>());
            _groupStatus.Setup(x => x.BuildSQLFacilityResult(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(new List<FacilityTypeResult>());
            _groupStatus.Setup(x =>
                    x.BuildSQLParamStandardTypeResult(It.IsAny<IList<FieldParamStandardTypeNameValues>>(),
                        It.IsAny<IList<string>>(), It.IsAny<string>()))
                .Returns(new List<ParamStandardTypeSumResult>());
            _groupStatus.Setup(x =>
                x.BuildSQLParamStandardTypeStateResult(It.IsAny<IList<FieldParamStandardTypeNameValues>>(),
                    It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(new List<ParamStandardTypeMaxResult>());
            _groupStatus.Setup(x => x.GetFacilityParamStandardTypes(It.IsAny<string[]>(), It.IsAny<string>()))
                .Returns(new SortedList<string, int?>());
            _groupStatus.Setup(x => x.GroupStatusFacilityTag(It.IsAny<string>(), It.IsAny<string>())).Returns(string.Empty);
            _groupStatus.Setup(x => x.LoadCommonColumns(It.IsAny<string>())).Returns(new Hashtable());
            _groupStatus.Setup(x => x.GetAvailableViewsByUserId(It.IsAny<string>(), It.IsAny<string>())).Returns(new List<AvailableViewModel>());
            _groupStatus.Setup(x => x.GetViewTables(It.IsAny<string>())).Returns(new List<GroupStatusTableModel>());
        }

        private void SetupParameterDataType()
        {
            _parameterDataType.Setup(x => x.GetItems(It.IsAny<string>())).Returns(new List<DataTypesModel>());
            _parameterDataType.Setup(x => x.GetParametersDataTypes(It.IsAny<Guid>(), It.IsAny<IList<int>>(), It.IsAny<string>()))
                .Returns(new Dictionary<int, short?>());
        }

        private void SetupNodeMaster()
        {
            _nodeMaster.Setup(x => x.GetNodeIdsByAssetGuid(It.IsAny<string[]>(), It.IsAny<string>())).Returns(new List<NodeMasterModel>());
            _nodeMaster.Setup(x => x.TryGetPortIdByAssetGUID(It.IsAny<Guid>(), out It.Ref<short>.IsAny, It.IsAny<string>())).Returns(true);
            _nodeMaster.Setup(x => x.TryGetPocTypeIdByAssetGUID(It.IsAny<Guid>(), out It.Ref<short>.IsAny, It.IsAny<string>())).Returns(true);
            _nodeMaster.Setup(x => x.GetNode(It.IsAny<Guid>(), It.IsAny<string>())).Returns(new NodeMasterModel());
            _groupAndAsset.Setup(x => x.GetGroupAssetAndRelationshipData(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new GroupAndAssetModel());
            _nodeMaster.Setup(x => x.GetLegacyWellAsync(It.IsAny<IList<Guid>>(), It.IsAny<string>())).ReturnsAsync(new Dictionary<Guid, bool>());
            _nodeMaster.Setup(x => x.GetByAssetIdsAsync(It.IsAny<List<Guid>>(), It.IsAny<string>())).ReturnsAsync(new List<NodeMasterModel>());
        }

        private void SetupThetaLoggerFactory()
        {
            _loggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(_logger.Object);
        }

        private void SetupColumnFormatterFactory()
        {
            _columnFormatterFactory.Setup(factory =>
                    factory.Create(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<IList<ConditionalFormat>>()))
                .Returns(_columnFormatter.Object);
        }

        private void SetupLocalePhrase()
        {
            _localePhrase.Setup(x => x.Get(It.IsAny<int>(), It.IsAny<string>())).Returns(string.Empty);
        }

        private void SetupAssetAndGroup()
        {
            _groupAndAsset.Setup(x => x.GetGroupAssetAndRelationshipData(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new GroupAndAssetModel());
        }

        private void SetupTrendDataStore()
        {
            _trendDataStore.Setup(x => x.GetDowntime(It.IsAny<IList<string>>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new DowntimeByWellsModel()
                {
                    RodPump = new List<DowntimeByWellsRodPumpModel>(),
                    ESP = new List<DowntimeByWellsValueModel>(),
                    GL = new List<DowntimeByWellsValueModel>(),
                });
        }

        private void SetupTrendDataInfluxStore()
        {
            _trendDataInfluxStore.Setup(x =>
                    x.GetDowntimeAsync(It.IsAny<IList<DowntimeFiltersInflux>>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => new List<DowntimeByWellsInfluxModel>());
            _trendDataInfluxStore.Setup(x =>
                    x.GetDowntimeAsync(It.IsAny<IList<DowntimeFiltersWithChannelIdInflux>>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => new List<DowntimeByWellsInfluxModel>());
        }

        private void SetupServiceScopeFactory()
        {
            var serviceScope = new Mock<IServiceScope>();
            var columnFormatterCacheService = new Mock<IColumnFormatterCacheService>();

            serviceScope.Setup(x => x.ServiceProvider.GetService(typeof(IColumnFormatterCacheService)))
                .Returns(columnFormatterCacheService.Object);
            _serviceScopeFactory.Setup(x => x.CreateScope()).Returns(serviceScope.Object);
        }

        private void SetupMongoDbStore()
        {
            var mongoStore = new Mock<IParameterMongoStore>();
            mongoStore.Setup(m => m.GetParameterData(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new List<Parameters>());
        }

        private void SetupCommonService()
        {
            _commonService.Setup(x => x.GetSystemParameterNextGenSignificantDigits(It.IsAny<string>())).Returns(3);
        }

        private void SetupGroupStatusProcessingService()
        {
            _groupStatusProcessingService = new GroupStatusProcessingService(
                _loggerFactory.Object,
                _groupStatus.Object,
                _parameterDataType.Object,
                _nodeMaster.Object,
                _columnFormatterFactory.Object,
                _localePhrase.Object,
                _groupAndAsset.Object,
                _trendDataStore.Object,
                _trendDataInfluxStore.Object,
                _serviceScopeFactory.Object,
                _mongoDbStore.Object,
                _commonService.Object);
        }

        #endregion

    }
}
