using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Theta.XSPOC.Apex.Api.Data.Entity;
using Theta.XSPOC.Apex.Api.Data.Models.Asset;
using Theta.XSPOC.Apex.Api.Data.Sql.HistoricalData;
using Theta.XSPOC.Apex.Kernel.Data.Sql.Entity;
using Theta.XSPOC.Apex.Kernel.UnitConversion;
using Theta.XSPOC.Apex.Kernel.UnitConversion.Models;

namespace Theta.XSPOC.Apex.Api.Data.Sql.Tests.HistoricalData
{
    [TestClass]
    public class HistoricalRepositorySQLTests : DataStoreTestBase
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThetaDBContextFactoryConstructorNullTest()
        {
            _ = new HistoricalSQLStore(null, new Mock<IUnitConversion>().Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UnitConversionConstructorNullTest()
        {
            _ = new HistoricalSQLStore(new Mock<IThetaDbContextFactory<XspocDbContext>>().Object, null);
        }

        [TestMethod]
        public async Task GetAssetStatusRegisterDataAsyncEmptyAssetIdTest()
        {
            var dbContext = SetupMockContext();
            var mockFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(dbContext.Object);

            var mockUnitConversion = new Mock<IUnitConversion>();

            var repo = new HistoricalSQLStore(mockFactory.Object, mockUnitConversion.Object);

            var result = await repo.GetAssetStatusRegisterDataAsync(Guid.Empty, Guid.NewGuid());

            Assert.IsNull(result);
        }

        //[TestMethod]
        public async Task GetAssetStatusRegisterDataAsyncAssetIdNotFoundTest()
        {
            var nodeMasterData = GetNodeMasterData();
            var statusRegisterData = GetStatusRegisterData();
            var facilityTagData = GetFacilityTagData();
            var paramStandardData = GetParamStandardData();
            var phraseData = GetPhraseData();
            var stateData = GetStateData();
            var parameterMapData = GetParameterMapData();
            var currentRawScanData = GetCurrentRawScanData();

            var mockNodeMasterDbSet = SetupMockDbSet(nodeMasterData);
            var mockStatusRegisterDbSet = SetupMockDbSet(statusRegisterData);
            var mockFacilityTagDbSet = SetupMockDbSet(facilityTagData);
            var mockParamStandardDbSet = SetupMockDbSet(paramStandardData);
            var mockPhraseDbSet = SetupMockDbSet(phraseData);
            var mockStateDbSet = SetupMockDbSet(stateData);
            var mockParameterMapDbSet = SetupMockDbSet(parameterMapData);
            var mockCurrentRawScanDbSet = SetupMockDbSet(currentRawScanData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContext.Setup(x => x.StatusRegisters).Returns(mockStatusRegisterDbSet.Object);
            mockContext.Setup(x => x.FacilityTags).Returns(mockFacilityTagDbSet.Object);
            mockContext.Setup(x => x.ParamStandardTypes).Returns(mockParamStandardDbSet.Object);
            mockContext.Setup(x => x.LocalePhrases).Returns(mockPhraseDbSet.Object);
            mockContext.Setup(x => x.States).Returns(mockStateDbSet.Object);
            mockContext.Setup(x => x.Parameters).Returns(mockParameterMapDbSet.Object);
            mockContext.Setup(x => x.CurrentRawScans).Returns(mockCurrentRawScanDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var mockUnitConversion = new Mock<IUnitConversion>();

            var repo = new HistoricalSQLStore(mockFactory.Object, mockUnitConversion.Object);

            var assetId = Guid.Parse("22E97D89-FD91-46A2-8190-99EEB9A4E26E");

            var result = await repo.GetAssetStatusRegisterDataAsync(assetId, Guid.NewGuid());

            Assert.IsNotNull(result);

            Assert.AreEqual(0, result.Count);
        }

        //[TestMethod]
        public async Task GetAssetStatusRegisterDataAsyncTest()
        {
            var nodeMasterData = GetNodeMasterData();
            var statusRegisterData = GetStatusRegisterData();
            var facilityTagData = GetFacilityTagData();
            var paramStandardData = GetParamStandardData();
            var phraseData = GetPhraseData();
            var stateData = GetStateData();
            var parameterMapData = GetParameterMapData();
            var currentRawScanData = GetCurrentRawScanData();

            var mockNodeMasterDbSet = SetupMockDbSet(nodeMasterData);
            var mockStatusRegisterDbSet = SetupMockDbSet(statusRegisterData);
            var mockFacilityTagDbSet = SetupMockDbSet(facilityTagData);
            var mockParamStandardDbSet = SetupMockDbSet(paramStandardData);
            var mockPhraseDbSet = SetupMockDbSet(phraseData);
            var mockStateDbSet = SetupMockDbSet(stateData);
            var mockParameterMapDbSet = SetupMockDbSet(parameterMapData);
            var mockCurrentRawScanDbSet = SetupMockDbSet(currentRawScanData);

            var mockContext = SetupMockContext();
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContext.Setup(x => x.StatusRegisters).Returns(mockStatusRegisterDbSet.Object);
            mockContext.Setup(x => x.FacilityTags).Returns(mockFacilityTagDbSet.Object);
            mockContext.Setup(x => x.ParamStandardTypes).Returns(mockParamStandardDbSet.Object);
            mockContext.Setup(x => x.LocalePhrases).Returns(mockPhraseDbSet.Object);
            mockContext.Setup(x => x.States).Returns(mockStateDbSet.Object);
            mockContext.Setup(x => x.Parameters).Returns(mockParameterMapDbSet.Object);
            mockContext.Setup(x => x.CurrentRawScans).Returns(mockCurrentRawScanDbSet.Object);

            var mockFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            mockFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var mockUnitConversion = new Mock<IUnitConversion>();
            mockUnitConversion
                .Setup(m => m.CreateUnitObject(It.IsAny<int>(), It.IsAny<float>()))
                .Returns((int a, float? b) => b == null ? null : new AnalogValue(b.Value));

            var assetId = Guid.Parse("12E97D89-FD91-46A2-8A90-99ECB9E4E26E");

            var repo = new HistoricalSQLStore(mockFactory.Object, new Mock<IUnitConversion>().Object);

            var result = await repo.GetAssetStatusRegisterDataAsync(assetId, Guid.NewGuid());

            Assert.IsNotNull(result);

            Assert.AreEqual(3, result.Count);

            Assert.AreEqual("", result[0].Value);
            Assert.AreEqual("", result[0].Description);
            Assert.AreEqual("", result[0].Address);
            Assert.AreEqual("", result[0].Bit);
            Assert.AreEqual("", result[0].DataType);
            Assert.AreEqual("", result[0].Decimals);
            Assert.AreEqual("", result[0].StateId);
            Assert.AreEqual("", result[0].PhraseId);
            Assert.AreEqual("", result[0].Format);
            Assert.AreEqual("", result[0].Order);
            Assert.AreEqual("", result[0].StringValue);

            Assert.AreEqual("", result[1].Value);
            Assert.AreEqual("", result[1].Description);
            Assert.AreEqual("", result[1].Address);
            Assert.AreEqual("", result[1].Bit);
            Assert.AreEqual("", result[1].DataType);
            Assert.AreEqual("", result[1].Decimals);
            Assert.AreEqual("", result[1].StateId);
            Assert.AreEqual("", result[1].PhraseId);
            Assert.AreEqual("", result[1].Format);
            Assert.AreEqual("", result[1].Order);
            Assert.AreEqual("", result[1].StringValue);

            Assert.AreEqual("", result[2].Value);
            Assert.AreEqual("", result[2].Description);
            Assert.AreEqual("", result[2].Address);
            Assert.AreEqual("", result[2].Bit);
            Assert.AreEqual("", result[2].DataType);
            Assert.AreEqual("", result[2].Decimals);
            Assert.AreEqual("", result[2].StateId);
            Assert.AreEqual("", result[2].PhraseId);
            Assert.AreEqual("", result[2].Format);
            Assert.AreEqual("", result[2].Order);
            Assert.AreEqual("", result[2].StringValue);
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
                    PercentCommunicationsYesterday = 87,
                    CommStatus = "OK",
                    FirmwareVersion = 1.043f,
                    LastGoodScanTime = new DateTime(2023, 2, 1, 2, 3, 4),
                    PumpFillage = 78,
                    Node = "i127.0.0.1|502|1",
                    RunStatus = "Running",
                    TimeInState = 6000,
                    TodayRuntimePercent = 98,
                    YesterdayRuntimePercent = 67,
                    Enabled = true,
                }
            }.AsQueryable();
        }

        private IQueryable<StatusRegisterEntity> GetStatusRegisterData()
        {
            return new List<StatusRegisterEntity>()
            {
                new StatusRegisterEntity()
                {
                    PocType = 8,
                    Bit = 1,
                    Order = 1,
                    RegisterAddress = 1,
                },
                new StatusRegisterEntity()
                {
                    PocType = 8,
                    Bit = 1,
                    Order = 2,
                    RegisterAddress = 2,
                },
                new StatusRegisterEntity()
                {
                    PocType = 8,
                    Bit = 1,
                    Order = 3,
                    RegisterAddress = 6,
                },
            }.AsQueryable();
        }

        private IQueryable<FacilityTagsEntity> GetFacilityTagData()
        {
            return new List<FacilityTagsEntity>()
            {
                new FacilityTagsEntity()
                {
                    NodeId = "Test 1",
                    Description = "Facility Tag 1",
                    Address = 1,
                    DataType = 1,
                    Decimals = 1,
                    CurrentValue = 123f,
                    ParamStandardType = 1,
                    StateId = 5,
                },
                new FacilityTagsEntity()
                {
                    NodeId = "Test 1",
                    Description = "Facility Tag 6",
                    Address = 6,
                    ParamStandardType = 3,
                    DataType = 2,
                    Decimals = 9,
                    CurrentValue = 222f,
                    StateId = 7,
                },
            }.AsQueryable();
        }

        private IQueryable<ParamStandardTypesEntity> GetParamStandardData()
        {
            return new List<ParamStandardTypesEntity>()
            {
                new ParamStandardTypesEntity()
                {
                    ParamStandardType = 1,
                    Description = "Param Standard Type 1",
                    UnitTypeId = 1,
                    PhraseId = 3,
                },
                new ParamStandardTypesEntity()
                {
                    ParamStandardType = 3,
                    Description = "Param Standard Type 3",
                    UnitTypeId = 9,
                    PhraseId = 4,
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
                new LocalePhraseEntity()
                {
                    PhraseId = 3,
                    English = "Phrase 3",
                },
                new LocalePhraseEntity()
                {
                    PhraseId = 4,
                    English = "Phrase 4",
                }
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
                    PhraseId = 3,
                },
                new StatesEntity()
                {
                    StateId = 7,
                    Text = "State Text 7",
                    Value = 1,
                    PhraseId = 1,
                },
            }.AsQueryable();
        }

        private IQueryable<ParameterEntity> GetParameterMapData()
        {
            return new List<ParameterEntity>()
            {
                new ParameterEntity()
                {
                    Poctype = 8,
                    Address = 2,
                    ParamStandardType = 1,
                    StateId = 5,
                    Description = "Parameter 2",
                    DataType = 1,
                    Decimals = 1,
                    UnitType = 1,
                    PhraseId = 3,
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
                    Address = 2,
                    Value = 1f,
                },
            }.AsQueryable();
        }

        #endregion

    }
}