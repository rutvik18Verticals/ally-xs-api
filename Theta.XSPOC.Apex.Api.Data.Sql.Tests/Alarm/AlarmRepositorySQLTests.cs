using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Entity.Alarms;
using Theta.XSPOC.Apex.Api.Data.Entity.Camera;
using Theta.XSPOC.Apex.Api.Data.Entity.XDIAG;
using Theta.XSPOC.Apex.Api.Data.Sql.Alarm;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;

namespace Theta.XSPOC.Apex.Api.Data.Sql.Tests.Alarm
{
    [TestClass]
    public class AlarmRepositorySQLTests : DataStoreTestBase
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullTest()
        {
            _ = new AlarmSQLStore(null);
        }

        [TestMethod]
        public async Task GetAlarmConfigurationAsyncEmptyAssetIdTest()
        {
            var dbContext = SetupMockContext();
            var mockFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(dbContext.Object);

            var repo = new AlarmSQLStore(mockFactory.Object);

            var result = await repo.GetAlarmConfigurationAsync(Guid.Empty, Guid.NewGuid());

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetAlarmConfigurationAsyncTest()
        {
            var nodeMasterData = GetNodeMasterData();
            var currentRawScanData = GetCurrentRawScanData();
            var parameterData = GetParameterData();
            var alarmConfigByPoctypeData = GetAlarmConfigByPoctypeData();
            var stateData = GetStateData();

            var mockNodeMasterDbSet = SetupMockDbSet(nodeMasterData);
            var mockRawScanDbSet = SetupMockDbSet(currentRawScanData);
            var mockParameterDbSet = SetupMockDbSet(parameterData);
            var mockAlarmConfigDbSet = SetupMockDbSet(alarmConfigByPoctypeData);
            var mockStateDbSet = SetupMockDbSet(stateData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContext.Setup(x => x.CurrentRawScans).Returns(mockRawScanDbSet.Object);
            mockContext.Setup(x => x.Parameters).Returns(mockParameterDbSet.Object);
            mockContext.Setup(x => x.AlarmConfigByPocTypes).Returns(mockAlarmConfigDbSet.Object);
            mockContext.Setup(x => x.States).Returns(mockStateDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var assetId = Guid.Parse("12E97D89-FD91-46A2-8A90-99ECB9E4E26E");

            var repo = new AlarmSQLStore(mockFactory.Object);

            var result = await repo.GetAlarmConfigurationAsync(assetId, Guid.NewGuid());

            Assert.IsNotNull(result);

            Assert.AreEqual(2, result.Count);

            Assert.AreEqual("1", result[0].AlarmRegister);
            Assert.AreEqual("State Text", result[0].StateText);
            Assert.AreEqual("Alarm 1", result[0].AlarmDescription);
            Assert.AreEqual(1, result[0].AlarmPriority);
            Assert.AreEqual("RTU", result[0].AlarmType);
            Assert.AreEqual(1, result[0].AlarmBit);
            Assert.AreEqual(0, result[0].AlarmNormalState);
            Assert.AreEqual(1, result[0].CurrentValue);

            Assert.AreEqual("4", result[1].AlarmRegister);
            Assert.IsNull(result[1].StateText);
            Assert.AreEqual("Alarm 2", result[1].AlarmDescription);
            Assert.AreEqual(2, result[1].AlarmPriority);
            Assert.AreEqual("RTU", result[1].AlarmType);
            Assert.AreEqual(4, result[1].AlarmBit);
            Assert.AreEqual(1, result[1].AlarmNormalState);
            Assert.AreEqual(4, result[1].CurrentValue);
        }

        [TestMethod]
        public async Task GetHostAlarmsAsyncEmptyAssetIdTest()
        {
            var dbContext = SetupMockContext();
            var mockFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(dbContext.Object);

            var repo = new AlarmSQLStore(mockFactory.Object);

            var result = await repo.GetHostAlarmsAsync(Guid.Empty, Guid.NewGuid());

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetHostAlarmsAsyncAssetIdNotFoundTest()
        {
            var nodeMasterData = GetNodeMasterData();

            var mockNodeMasterDbSet = SetupMockDbSetAsync(nodeMasterData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var assetId = Guid.Parse("22E97D89-FD91-46A2-8190-99EEB9A4E26E");

            var repo = new AlarmSQLStore(mockFactory.Object);

            var result = await repo.GetHostAlarmsAsync(assetId, Guid.NewGuid());

            Assert.IsNotNull(result);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetHostAlarmsAsyncTest()
        {
            var nodeMasterData = GetNodeMasterData();
            var hostAlarmData = GetHostAlarmData();
            var parameterData = GetParameterData();
            var facilityTagData = GetFacilityTagData();
            var phraseData = GetPhraseData();
            var xdiagOutputData = GetXDIAGOutputData();

            var mockNodeMasterDbSet = SetupMockDbSetWithProjectionAsync(nodeMasterData, GetNodeProjectedData());
            var mockHostAlarmDbSet = SetupMockDbSet(hostAlarmData);
            var mockParameterDbSet = SetupMockDbSet(parameterData);
            var mockFacilityTagDbSet = SetupMockDbSet(facilityTagData);
            var mockPhraseDbSet = SetupMockDbSet(phraseData);
            var mockXDIAGDbSet = SetupMockDbSet(xdiagOutputData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContext.SetupSequence(x => x.HostAlarm).Returns(mockHostAlarmDbSet.Object)
                .Returns(mockHostAlarmDbSet.Object);
            mockContext.Setup(x => x.Parameters).Returns(mockParameterDbSet.Object);
            mockContext.Setup(x => x.FacilityTags).Returns(mockFacilityTagDbSet.Object);
            mockContext.Setup(x => x.LocalePhrases).Returns(mockPhraseDbSet.Object);
            mockContext.Setup(x => x.XDIAGOutputs).Returns(mockXDIAGDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var assetId = Guid.Parse("12E97D89-FD91-46A2-8A90-99ECB9E4E26E");

            var repo = new AlarmSQLStore(mockFactory.Object);

            var result = await repo.GetHostAlarmsAsync(assetId, Guid.NewGuid());

            Assert.IsNotNull(result);

            Assert.AreEqual(3, result.Count);

            Assert.AreEqual("2", result[0].AlarmRegister);
            Assert.IsNull(result[0].StateText);
            Assert.AreEqual("XDiag Host Alarm 2", result[0].AlarmDescription);
            Assert.AreEqual(100, result[0].AlarmPriority);
            Assert.AreEqual("Host", result[0].AlarmType);
            Assert.AreEqual(0, result[0].AlarmBit);
            Assert.AreEqual(0, result[0].AlarmNormalState);
            Assert.AreEqual(0, result[0].CurrentValue);

            Assert.AreEqual("1", result[1].AlarmRegister);
            Assert.IsNull(result[1].StateText);
            Assert.AreEqual("Phrase 1", result[1].AlarmDescription);
            Assert.AreEqual(30, result[1].AlarmPriority);
            Assert.AreEqual("Host", result[1].AlarmType);
            Assert.AreEqual(0, result[1].AlarmBit);
            Assert.AreEqual(0, result[1].AlarmNormalState);
            Assert.AreEqual(0, result[1].CurrentValue);

            Assert.AreEqual("4", result[2].AlarmRegister);
            Assert.IsNull(result[2].StateText);
            Assert.AreEqual("Parameter Description 4", result[2].AlarmDescription);
            Assert.AreEqual(50, result[2].AlarmPriority);
            Assert.AreEqual("Host", result[2].AlarmType);
            Assert.AreEqual(0, result[2].AlarmBit);
            Assert.AreEqual(0, result[2].AlarmNormalState);
            Assert.AreEqual(0, result[2].CurrentValue);
        }

        [TestMethod]
        public async Task GetFacilityTagAlarmsAsyncEmptyAssetIdTest()
        {
            var dbContext = SetupMockContext();
            var mockFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(dbContext.Object);

            var repo = new AlarmSQLStore(mockFactory.Object);

            var result = await repo.GetFacilityTagAlarmsAsync(Guid.Empty, Guid.NewGuid());

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetFacilityTagAlarmsAsyncAssetIdNotFoundTest()
        {
            var nodeMasterData = GetNodeMasterData();

            var mockNodeMasterDbSet = SetupMockDbSetAsync(nodeMasterData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var assetId = Guid.Parse("22E97D89-FD91-46A2-8190-99EEB9A4E26E");

            var repo = new AlarmSQLStore(mockFactory.Object);

            var result = await repo.GetFacilityTagAlarmsAsync(assetId, Guid.NewGuid());

            Assert.IsNotNull(result);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetFacilityTagAlarmsAsyncTest()
        {
            var nodeMasterData = GetNodeMasterData();
            var parameterData = GetParameterData();
            var facilityTagData = GetFacilityTagData();

            var mockNodeMasterDbSet = SetupMockDbSetWithProjectionAsync(nodeMasterData, GetNodeProjectedData());
            var mockParameterDbSet = SetupMockDbSet(parameterData);
            var mockFacilityTagDbSet = SetupMockDbSet(facilityTagData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContext.Setup(x => x.Parameters).Returns(mockParameterDbSet.Object);
            mockContext.Setup(x => x.FacilityTags).Returns(mockFacilityTagDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var assetId = Guid.Parse("12E97D89-FD91-46A2-8A90-99ECB9E4E26E");

            var repo = new AlarmSQLStore(mockFactory.Object);

            var result = await repo.GetFacilityTagAlarmsAsync(assetId, Guid.NewGuid());

            Assert.IsNotNull(result);

            Assert.AreEqual(3, result.Count);

            Assert.AreEqual("Facility Tag Alarm 3 ", result[0].AlarmDescription);
            Assert.AreEqual("Facility Tag", result[0].AlarmType);

            Assert.AreEqual("Facility Tag Alarm 2 -Lo", result[1].AlarmDescription);
            Assert.AreEqual("Facility Tag", result[1].AlarmType);

            Assert.AreEqual("Facility Tag Alarm 1 -Hi", result[2].AlarmDescription);
            Assert.AreEqual("Facility Tag", result[2].AlarmType);
        }

        [TestMethod]
        public async Task GetCameraAlarmsAsyncEmptyAssetIdTest()
        {
            var dbContext = SetupMockContext();
            var mockFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(dbContext.Object);

            var repo = new AlarmSQLStore(mockFactory.Object);

            var result = await repo.GetCameraAlarmsAsync(Guid.Empty, Guid.NewGuid());

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task GetCameraAlarmsAsyncAssetIdNotFoundTest()
        {
            var nodeMasterData = GetNodeMasterData();
            var cameraData = GetCameraData();
            var cameraAlarmData = GetCameraAlarmsData();
            var cameraEventAlarmData = GetCameraEventsAlarmsData();
            var phraseData = GetPhraseData();
            var cameraAlarmTypeData = GetCameraAlarmTypeData();

            var mockNodeMasterDbSet = SetupMockDbSet(nodeMasterData);
            var cameraDbSet = SetupMockDbSet(cameraData);
            var cameraAlarmsDbSet = SetupMockDbSet(cameraAlarmData);
            var cameraEventAlarmDbSet = SetupMockDbSet(cameraEventAlarmData);
            var phraseDbSet = SetupMockDbSet(phraseData);
            var cameraAlarmTypeDbSet = SetupMockDbSet(cameraAlarmTypeData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContext.Setup(x => x.Cameras).Returns(cameraDbSet.Object);
            mockContext.Setup(x => x.CameraAlarms).Returns(cameraAlarmsDbSet.Object);
            mockContext.Setup(x => x.AlarmEvents).Returns(cameraEventAlarmDbSet.Object);
            mockContext.Setup(x => x.LocalePhrases).Returns(phraseDbSet.Object);
            mockContext.Setup(x => x.CameraAlarmTypes).Returns(cameraAlarmTypeDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var assetId = Guid.Parse("22E97D89-FD91-46A2-8190-99EEB9A4E26E");

            var repo = new AlarmSQLStore(mockFactory.Object);

            var result = await repo.GetCameraAlarmsAsync(assetId, Guid.NewGuid());

            Assert.IsNotNull(result);

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetCameraAlarmsAsyncTest()
        {
            var nodeMasterData = GetNodeMasterData();
            var cameraData = GetCameraData();
            var cameraAlarmData = GetCameraAlarmsData();
            var cameraEventAlarmData = GetCameraEventsAlarmsData();
            var phraseData = GetPhraseData();
            var cameraAlarmTypeData = GetCameraAlarmTypeData();

            var mockNodeMasterDbSet = SetupMockDbSet(nodeMasterData);
            var cameraDbSet = SetupMockDbSet(cameraData);
            var cameraAlarmsDbSet = SetupMockDbSet(cameraAlarmData);
            var cameraEventAlarmDbSet = SetupMockDbSet(cameraEventAlarmData);
            var phraseDbSet = SetupMockDbSet(phraseData);
            var cameraAlarmTypeDbSet = SetupMockDbSet(cameraAlarmTypeData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContext.Setup(x => x.Cameras).Returns(cameraDbSet.Object);
            mockContext.Setup(x => x.CameraAlarms).Returns(cameraAlarmsDbSet.Object);
            mockContext.Setup(x => x.AlarmEvents).Returns(cameraEventAlarmDbSet.Object);
            mockContext.Setup(x => x.LocalePhrases).Returns(phraseDbSet.Object);
            mockContext.Setup(x => x.CameraAlarmTypes).Returns(cameraAlarmTypeDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var assetId = Guid.Parse("12E97D89-FD91-46A2-8A90-99ECB9E4E26E");

            var repo = new AlarmSQLStore(mockFactory.Object);

            var result = await repo.GetCameraAlarmsAsync(assetId, Guid.NewGuid());

            Assert.IsNotNull(result);

            Assert.AreEqual(1, result.Count);

            Assert.AreEqual(" - Alarm type 1", result[0].AlarmDescription);
            Assert.AreEqual("Camera", result[0].AlarmType);
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
                    PocType = 8,
                },
                new NodeMasterEntity()
                {
                    AssetGuid = Guid.Parse("E13E3F8A-5D32-4670-A5C1-94993BD311CA"),
                    NodeId = "Test 2",
                    PocType = 17,
                },
            }.AsQueryable();
        }

        private IQueryable<CurrentRawScanDataEntity> GetCurrentRawScanData()
        {
            return new List<CurrentRawScanDataEntity>()
            {
                new CurrentRawScanDataEntity()
                {
                    NodeId = "Test 1",
                    Address = 1,
                    Value = 1f,
                },
                new CurrentRawScanDataEntity()
                {
                    NodeId = "Test 2",
                    Address = 9,
                    Value = 2f,
                },
                new CurrentRawScanDataEntity()
                {
                    NodeId = "Test 1",
                    Address = 2,
                    Value = 3f,
                },
                new CurrentRawScanDataEntity()
                {
                    NodeId = "Test 2",
                    Address = 3,
                    Value = 4f,
                },
                new CurrentRawScanDataEntity()
                {
                    NodeId = "Test 1",
                    Address = 4,
                    Value = 4f,
                },
            }.AsQueryable();
        }

        private IQueryable<ParameterEntity> GetParameterData()
        {
            return new List<ParameterEntity>()
            {
                new ParameterEntity()
                {
                    Poctype = 8,
                    Address = 1,
                    StateId = 5,
                    PhraseId = 1,
                },
                new ParameterEntity()
                {
                    Poctype = 17,
                    Address = 1,
                    StateId = null,
                },
                new ParameterEntity()
                {
                    Poctype = 8,
                    Address = 2,
                    StateId = null,
                },
                new ParameterEntity()
                {
                    Poctype = 8,
                    Address = 3,
                    StateId = null,
                },
                new ParameterEntity()
                {
                    Poctype = 8,
                    Address = 4,
                    StateId = null,
                    Description = "Parameter Description 4",
                },
                new ParameterEntity()
                {
                    Poctype = 8,
                    Address = 5,
                    StateId = null,
                },
            }.AsQueryable();
        }

        private IQueryable<AlarmConfigByPocTypeEntity> GetAlarmConfigByPoctypeData()
        {
            return new List<AlarmConfigByPocTypeEntity>
            {
                new AlarmConfigByPocTypeEntity()
                {
                    PocType = 8,
                    Register = 1,
                    Bit = 1,
                    Description = "Alarm 1",
                    Priority = 1,
                    NormalState = false,
                    Enabled = true,
                },
                new AlarmConfigByPocTypeEntity()
                {
                    PocType = 8,
                    Register = 4,
                    Bit = 4,
                    Description = "Alarm 2",
                    Priority = 2,
                    NormalState = true,
                    Enabled = true,
                },
                new AlarmConfigByPocTypeEntity()
                {
                    PocType = 17,
                    Register = 1,
                    Bit = 2,
                    Description = "Alarm 3",
                    Priority = 3,
                    NormalState = true,
                    Enabled = true,
                },
                new AlarmConfigByPocTypeEntity()
                {
                    PocType = 8,
                    Register = 4,
                    Bit = 2,
                    Description = "Alarm 3",
                    Priority = 4,
                    NormalState = true,
                    Enabled = false,
                },
            }.AsQueryable();
        }

        private IQueryable<StatesEntity> GetStateData()
        {
            return new List<StatesEntity>
            {
                new StatesEntity()
                {
                    StateId = 5,
                    Text = "State Text",
                    Value = 1,

                },
                new StatesEntity()
                {
                    StateId = 7,
                    Text = "State Text 7",
                    Value = 1,
                },
            }.AsQueryable();
        }

        private IQueryable<HostAlarmEntity> GetHostAlarmData()
        {
            return new List<HostAlarmEntity>
            {
                new HostAlarmEntity()
                {
                    NodeId = "Test 1",
                    Address = 1,
                    Id = 1,
                    Enabled = true,
                    AlarmType = 1,
                    AlarmState = 2,
                    LoLimit = 3,
                    LoLoLimit = 4,
                    HiLimit = 5,
                    HiHiLimit = 6,
                    NumDays = 7,
                    ExactValue = 8,
                    ValueChange = 9,
                    PercentChange = 10,
                    MinToMaxLimit = 11,
                    IgnoreZeroAddress = 12,
                    AlarmAction = 13,
                    IgnoreValue = 14,
                    Priority = 30,
                    PagingEnabled = true,
                },
                new HostAlarmEntity()
                {
                    NodeId = "Test 1",
                    Address = 4,
                    Id = 2,
                    Enabled = true,
                    AlarmType = 16,
                    AlarmState = 17,
                    LoLimit = 18,
                    LoLoLimit = 19,
                    HiLimit = 20,
                    HiHiLimit = 21,
                    NumDays = 22,
                    ExactValue = 23,
                    ValueChange = 24,
                    PercentChange = 25,
                    MinToMaxLimit = 26,
                    IgnoreZeroAddress = 27,
                    AlarmAction = 28,
                    IgnoreValue = 29,
                    Priority = 50,
                    PushEnabled = true,
                },
                new HostAlarmEntity()
                {
                    NodeId = "Test 2",
                    Address = 4,
                    Id = 3,
                    Enabled = true,
                    AlarmType = 16,
                    AlarmState = 17,
                    LoLimit = 18,
                    LoLoLimit = 19,
                    HiLimit = 20,
                    HiHiLimit = 21,
                    NumDays = 22,
                    ExactValue = 23,
                    ValueChange = 24,
                    PercentChange = 25,
                    MinToMaxLimit = 26,
                    IgnoreZeroAddress = 27,
                    AlarmAction = 28,
                    IgnoreValue = 29,
                    Priority = 60,
                    PushEnabled = true,
                },
                new HostAlarmEntity()
                {
                    NodeId = "Test 1",
                    Address = null,
                    Id = 4,
                    Enabled = true,
                    AlarmType = 16,
                    AlarmState = 17,
                    LoLimit = 18,
                    LoLoLimit = 19,
                    HiLimit = 20,
                    HiHiLimit = 21,
                    NumDays = 22,
                    ExactValue = 23,
                    ValueChange = 24,
                    PercentChange = 25,
                    MinToMaxLimit = 26,
                    IgnoreZeroAddress = 27,
                    AlarmAction = 28,
                    IgnoreValue = 29,
                    Priority = 100,
                    PushEnabled = true,
                    XdiagOutputsId = 2,
                },
            }.AsQueryable();
        }

        private IQueryable<LocalePhraseEntity> GetPhraseData()
        {
            return new List<LocalePhraseEntity>
            {
                new LocalePhraseEntity()
                {
                    PhraseId = 1,
                    English = "Phrase 1",
                },
            }.AsQueryable();
        }

        private IQueryable<FacilityTagsEntity> GetFacilityTagData()
        {
            return new List<FacilityTagsEntity>()
            {
                new FacilityTagsEntity()
                {
                    Address = 1,
                    Bit = 1,
                    NodeId = "Test 1",
                    Description = "Facility Tag Alarm 1",
                    AlarmState = 1,
                },
                new FacilityTagsEntity()
                {
                    Address = 2,
                    Bit = 1,
                    NodeId = "Test 1",
                    Description = "Facility Tag Alarm 2",
                    AlarmState = 2,
                },
                new FacilityTagsEntity()
                {
                    Address = 3,
                    Bit = 1,
                    NodeId = "Test 1",
                    Description = "Facility Tag Alarm 3",
                    AlarmState = 6,
                },
                new FacilityTagsEntity()
                {
                    Address = 100,
                    Bit = 1,
                    NodeId = "Test 100",
                    Description = "Facility Tag Alarm 4",
                },
            }.AsQueryable();
        }

        private IQueryable<XDIAGOutputEntity> GetXDIAGOutputData()
        {
            return new List<XDIAGOutputEntity>()
            {
                new XDIAGOutputEntity()
                {
                    Id = 1,
                    Name = "XDiag Host Alarm 1",
                },
                new XDIAGOutputEntity()
                {
                    Id = 2,
                    Name = "XDiag Host Alarm 2",
                }
            }.AsQueryable();
        }

        private IQueryable<NodeProjected> GetNodeProjectedData()
        {
            return GetNodeMasterData().Select(m => new NodeProjected()
            {
                AssetGUID = m.AssetGuid,
                NodeId = m.NodeId,
                PocType = m.PocType,
            });
        }

        private IQueryable<CameraEntity> GetCameraData()
        {
            return new List<CameraEntity>()
            {
                new CameraEntity()
                {
                    NodeId = "Test 1",
                    Id = 1,
                },
                new CameraEntity()
                {
                    NodeId = "Test 1",
                    Id = 2,
                },
                new CameraEntity()
                {
                    NodeId = "Test 2",
                    Id = 3,
                },
            }.AsQueryable();
        }

        private IQueryable<CameraAlarmEntity> GetCameraAlarmsData()
        {
            return new List<CameraAlarmEntity>()
            {
                new CameraAlarmEntity()
                {
                    CameraId = 1,
                    Id = 1,
                    AlarmType = 1,
                    IsEnabled = true,
                },
                new CameraAlarmEntity()
                {
                    CameraId = 2,
                    Id = 2,
                    AlarmType = 1,
                    IsEnabled = false,
                },
                new CameraAlarmEntity()
                {
                    CameraId = 3,
                    Id = 3,
                    AlarmType = 1,
                    IsEnabled = true,
                },
                new CameraAlarmEntity()
                {
                    CameraId = 1,
                    Id = 4,
                    AlarmType = 8,
                    IsEnabled = true,
                },
            }.AsQueryable();
        }

        private IQueryable<AlarmEventEntity> GetCameraEventsAlarmsData()
        {
            return new List<AlarmEventEntity>()
            {
                new AlarmEventEntity()
                {
                    Id = 1,
                    AlarmType = 1,
                    EventDateTime = new DateTime(2021, 1, 2, 3, 4 , 5),
                    AlarmId = 1,
                },
                new AlarmEventEntity()
                {
                    Id = 2,
                    AlarmType = 1,
                    EventDateTime = new DateTime(2022, 1, 2, 3, 4 , 5),
                    AlarmId = 1,
                },
                new AlarmEventEntity()
                {
                    Id = 3,
                    AlarmType = 2,
                },
                new AlarmEventEntity()
                {
                    Id = 4,
                    AlarmType = 8,
                },
            }.AsQueryable();
        }

        private IQueryable<CameraAlarmTypeEntity> GetCameraAlarmTypeData()
        {
            return new List<CameraAlarmTypeEntity>()
            {
                new CameraAlarmTypeEntity()
                {
                    Id = 1,
                    Name = "Alarm type 1",
                },
                new CameraAlarmTypeEntity()
                {
                    Id = 2,
                    Name = "Alarm type 2",
                },
            }.AsQueryable();
        }

        #endregion

    }
}