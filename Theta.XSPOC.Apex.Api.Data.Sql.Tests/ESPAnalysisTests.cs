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
    public class ESPAnalysisTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorNullContextFactoryTest()
        {
            _ = new ESPAnalysisSQLStore(null, null);
        }

        [TestMethod]
        public void ESPAnalysisTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockSqlStore = new Mock<ISQLStoreBase>();
            mockSqlStore.Setup(x => x.GetNodeMasterData(It.IsAny<Guid>())).Returns(GetNodeMasterData().AsQueryable().FirstOrDefault());
            var nodeData = new List<NodeMasterEntity>()
            {
                new NodeMasterEntity()
                {
                    NodeId = "AssetId1",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                    CompanyId = 1
                }
            }.AsQueryable();

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(nodeData).Object);
            mockContext.Setup(x => x.Company)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCompanyData().AsQueryable()).Object);
            mockContext.Setup(x => x.ESPAnalysisResults)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetESPAnalysisResults().AsQueryable()).Object);

            mockContext.Setup(x => x.ESPWellPumps)
                .Returns(TestUtilities.SetupMockData(TestUtilities.ESPWellPumpData().AsQueryable()).Object);
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            var correlationId = Guid.NewGuid().ToString();
            var espAnalysis = new ESPAnalysisSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);

            var response =
                espAnalysis.GetESPAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000", correlationId);
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response.NodeMasterData, typeof(NodeMasterModel));
            Assert.IsNotNull(response.NodeMasterData);
            Assert.IsInstanceOfType(response.AnalysisResultEntity, typeof(ESPAnalysisResultModel));
            Assert.IsNotNull(response.AnalysisResultEntity);
            Assert.IsInstanceOfType(response.WellPumpEntities, typeof(IList<ESPWellPumpModel>));
            Assert.IsNotNull(response.WellPumpEntities);
            Assert.IsInstanceOfType(response.TestDate, typeof(DateTime));
            Assert.IsNotNull(response.TestDate);
        }

        [TestMethod]
        public void ESPAnalysisNullESPAnalysisResultTest()
        {
            var mockSqlStore = new Mock<ISQLStoreBase>();
            var nodeData = new List<NodeMasterEntity>()
            {
                new NodeMasterEntity()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                    CompanyId = 1
                }
            }.AsQueryable();

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.NodeMasters).Returns(TestUtilities.SetupMockData(nodeData).Object);
            mockContext.Setup(x => x.Company)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCompanyData().AsQueryable()).Object);
            mockContext.Setup(x => x.ESPAnalysisResults)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetESPAnalysisResults().AsQueryable()).Object);
            mockContext.Setup(x => x.ESPWellPumps)
                .Returns(TestUtilities.SetupMockData(TestUtilities.ESPWellPumpData().AsQueryable()).Object);

            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);
            var correlationId = Guid.NewGuid().ToString();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var espAnalysis = new ESPAnalysisSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);

            var response =
                espAnalysis.GetESPAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000", correlationId);
            Assert.IsNull(response);
        }

        [TestMethod]
        public void ESPAnalysisNullESPWellPumpDataTest()
        {
            var mockSqlStore = new Mock<ISQLStoreBase>();
            var nodeData = new List<NodeMasterEntity>()
            {
                new NodeMasterEntity()
                {
                    NodeId = "AssetId1",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                    CompanyId = 1
                }
            }.AsQueryable();

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.NodeMasters).Returns(TestUtilities.SetupMockData(nodeData).Object);
            mockContext.Setup(x => x.ESPAnalysisResults)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetESPAnalysisResults().AsQueryable()).Object);
            mockContext.Setup(x => x.Company)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCompanyData().AsQueryable()).Object);

            var wellPumpData = TestUtilities.ESPWellPumpData().AsQueryable();
            foreach (var wellPump in wellPumpData)
            {
                wellPump.ESPWellId = "TestNode1";
            }

            mockContext.Setup(x => x.ESPWellPumps)
                .Returns(TestUtilities.SetupMockData(wellPumpData).Object);

            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var correlationId = Guid.NewGuid().ToString();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var espAnalysis = new ESPAnalysisSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);

            var response =
                espAnalysis.GetESPAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), "2023-05-11 22:29:55.000", correlationId);
            Assert.IsNull(response);
        }

        [TestMethod]
        public void GetESPAnalysisResultTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.ESPAnalysisResults)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetESPAnalysisResults().AsQueryable()).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var correlationId = Guid.NewGuid().ToString();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var espAnalysis = new ESPAnalysisSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);

            var result =
                espAnalysis.GetESPAnalysisResult("AssetId1", DateTime.Parse("2023-05-11 22:29:55.000"), correlationId);

            Assert.AreEqual(421.625f, result.GrossRate);
            Assert.AreEqual(123.1f, result.HeadAcrossPump);
            Assert.AreEqual(235.1f, result.TotalVolumeAtPump);
        }

        [TestMethod]
        public void GetESPPressureProfileDataTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.ESPAnalysisResults)
                .Returns(TestUtilities.SetupMockData(GetESPAnalysisResultsEntity().AsQueryable()).Object);
            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(GetNodeMasterEntity().AsQueryable()).Object);
            mockContext.Setup(x => x.Perforation)
                .Returns(TestUtilities.SetupMockData(GetPerforationsEntity().AsQueryable()).Object);
            mockContext.Setup(x => x.WellDetails)
                .Returns(TestUtilities.SetupMockData(GetWellDetailsEntity().AsQueryable()).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);
            var correlationId = Guid.NewGuid().ToString();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var espAnalysis = new ESPAnalysisSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);

            var assetGUID = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            var testDate = new DateTime(1997, 3, 12);

            var result = espAnalysis.GetESPPressureProfileData(assetGUID, testDate, correlationId);

            Assert.IsNotNull(result);

            Assert.AreEqual(1f, result.PumpIntakePressure);
            Assert.AreEqual(2f, result.PumpDischargePressure);
            Assert.AreEqual(3f, result.PumpStaticPressure);
            Assert.AreEqual(4f, result.PressureAcrossPump);
            Assert.AreEqual(5f, result.FrictionalPressureDrop);
            Assert.AreEqual(6f, result.CasingPressure);
            Assert.AreEqual(7f, result.TubingPressure);
            Assert.AreEqual(8f, result.FlowingBottomholePressure);
            Assert.AreEqual(9f, result.CompositeTubingSpecificGravity);
            Assert.AreEqual(10f, result.WaterRate);
            Assert.AreEqual(12f, result.OilRate);
            Assert.AreEqual(13f, result.WaterSpecificGravity);
            Assert.IsTrue(result.IsGasHandlingEnabled);
            Assert.AreEqual(15f, result.SpecificGravityOfOil);
            Assert.IsTrue(result.UseDischargeGaugeInAnalysis);
            Assert.AreEqual(17f, result.DischargeGaugePressure);
            Assert.AreEqual(18f, result.VerticalPumpDepth);
            Assert.AreEqual(19f, result.CalculatedFluidLevelAbovePump);

            Assert.AreEqual(2, result.Perforations.Count);
            Assert.AreEqual(25f, result.Perforations[0].TopDepth);
            Assert.AreEqual(10f, result.Perforations[0].Length);
            Assert.AreEqual(15f, result.Perforations[1].TopDepth);
            Assert.AreEqual(5f, result.Perforations[1].Length);

            Assert.AreEqual(26f, result.ProductionDepth);
        }

        [TestMethod]
        public void GetESPPressureProfileDataNoAnalysisResultsTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.ESPAnalysisResults)
                .Returns(TestUtilities.SetupMockData(GetESPAnalysisResultsEntity().AsQueryable()).Object);
            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(GetNodeMasterEntity().AsQueryable()).Object);
            mockContext.Setup(x => x.Perforation)
                .Returns(TestUtilities.SetupMockData(GetPerforationsEntity().AsQueryable()).Object);
            mockContext.Setup(x => x.WellDetails)
                .Returns(TestUtilities.SetupMockData(GetWellDetailsEntity().AsQueryable()).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);
            var correlationId = Guid.NewGuid().ToString();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var espAnalysis = new ESPAnalysisSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);

            var assetGUID = new Guid("AFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            var testDate = new DateTime(1997, 3, 12);

            var result = espAnalysis.GetESPPressureProfileData(assetGUID, testDate, correlationId);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetESPPressureProfileDataNoPerforationsTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.ESPAnalysisResults)
                .Returns(TestUtilities.SetupMockData(GetESPAnalysisResultsEntity().AsQueryable()).Object);
            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(GetNodeMasterEntity().AsQueryable()).Object);
            mockContext.Setup(x => x.Perforation)
                .Returns(TestUtilities.SetupMockData(GetPerforationsEntity().AsQueryable()).Object);
            mockContext.Setup(x => x.WellDetails)
                .Returns(TestUtilities.SetupMockData(GetWellDetailsEntity().AsQueryable()).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var correlationId = Guid.NewGuid().ToString();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var espAnalysis = new ESPAnalysisSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);

            var assetGUID = new Guid("CFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            var testDate = new DateTime(1997, 3, 12);

            var result = espAnalysis.GetESPPressureProfileData(assetGUID, testDate, correlationId);

            Assert.IsNotNull(result);

            Assert.AreEqual(1f, result.PumpIntakePressure);
            Assert.AreEqual(2f, result.PumpDischargePressure);
            Assert.AreEqual(3f, result.PumpStaticPressure);
            Assert.AreEqual(4f, result.PressureAcrossPump);
            Assert.AreEqual(5f, result.FrictionalPressureDrop);
            Assert.AreEqual(6f, result.CasingPressure);
            Assert.AreEqual(7f, result.TubingPressure);
            Assert.AreEqual(8f, result.FlowingBottomholePressure);
            Assert.AreEqual(9f, result.CompositeTubingSpecificGravity);
            Assert.AreEqual(10f, result.WaterRate);
            Assert.AreEqual(12f, result.OilRate);
            Assert.AreEqual(13f, result.WaterSpecificGravity);
            Assert.IsFalse(result.IsGasHandlingEnabled);
            Assert.AreEqual(15f, result.SpecificGravityOfOil);
            Assert.IsFalse(result.UseDischargeGaugeInAnalysis);
            Assert.AreEqual(17f, result.DischargeGaugePressure);
            Assert.AreEqual(18f, result.VerticalPumpDepth);
            Assert.AreEqual(19f, result.CalculatedFluidLevelAbovePump);

            Assert.AreEqual(0, result.Perforations.Count);
            Assert.IsNull(result.ProductionDepth);
        }

        [TestMethod]
        public void GetESPPressureProfileDataNoProductionDepthTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.ESPAnalysisResults)
                .Returns(TestUtilities.SetupMockData(GetESPAnalysisResultsEntity().AsQueryable()).Object);
            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(GetNodeMasterEntity().AsQueryable()).Object);
            mockContext.Setup(x => x.Perforation)
                .Returns(TestUtilities.SetupMockData(GetPerforationsEntity().AsQueryable()).Object);
            mockContext.Setup(x => x.WellDetails)
                .Returns(TestUtilities.SetupMockData(GetWellDetailsEntity().AsQueryable()).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var correlationId = Guid.NewGuid().ToString();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var espAnalysis = new ESPAnalysisSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);

            var assetGUID = new Guid("EFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            var testDate = new DateTime(1997, 3, 12);

            var result = espAnalysis.GetESPPressureProfileData(assetGUID, testDate, correlationId);

            Assert.IsNotNull(result);

            Assert.AreEqual(1f, result.PumpIntakePressure);
            Assert.AreEqual(2f, result.PumpDischargePressure);
            Assert.AreEqual(3f, result.PumpStaticPressure);
            Assert.AreEqual(4f, result.PressureAcrossPump);
            Assert.AreEqual(5f, result.FrictionalPressureDrop);
            Assert.AreEqual(6f, result.CasingPressure);
            Assert.AreEqual(7f, result.TubingPressure);
            Assert.AreEqual(8f, result.FlowingBottomholePressure);
            Assert.AreEqual(9f, result.CompositeTubingSpecificGravity);
            Assert.AreEqual(10f, result.WaterRate);
            Assert.AreEqual(12f, result.OilRate);
            Assert.AreEqual(13f, result.WaterSpecificGravity);
            Assert.IsTrue(result.IsGasHandlingEnabled);
            Assert.AreEqual(15f, result.SpecificGravityOfOil);
            Assert.IsTrue(result.UseDischargeGaugeInAnalysis);
            Assert.AreEqual(17f, result.DischargeGaugePressure);
            Assert.AreEqual(18f, result.VerticalPumpDepth);
            Assert.AreEqual(19f, result.CalculatedFluidLevelAbovePump);

            Assert.AreEqual(2, result.Perforations.Count);
            Assert.AreEqual(25f, result.Perforations[0].TopDepth);
            Assert.AreEqual(10f, result.Perforations[0].Length);
            Assert.AreEqual(15f, result.Perforations[1].TopDepth);
            Assert.AreEqual(5f, result.Perforations[1].Length);

            Assert.IsNull(result.ProductionDepth);
        }

        #endregion

        #region Private Methods

        private IList<NodeMasterModel> GetNodeMasterData()
        {
            var nodeData = new List<NodeMasterModel>()
            {
                new NodeMasterModel()
                {
                    NodeId = "TestNode",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            };

            return nodeData;
        }

        private IList<NodeMasterEntity> GetNodeMasterEntity()
        {
            return new List<NodeMasterEntity>()
            {
                new NodeMasterEntity()
                {
                    NodeId = "TestNodeId1",
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                },
                new NodeMasterEntity()
                {
                    NodeId = "TestNodeId2",
                    AssetGuid = new Guid("AFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                },
                new NodeMasterEntity()
                {
                    NodeId = "TestNodeId3",
                    AssetGuid = new Guid("CFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                },
                new NodeMasterEntity()
                {
                    NodeId = "TestNodeId4",
                    AssetGuid = new Guid("EFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            };
        }

        private IList<ESPAnalysisResultsEntity> GetESPAnalysisResultsEntity()
        {
            return new List<ESPAnalysisResultsEntity>()
            {
                new ESPAnalysisResultsEntity()
                {
                    NodeId = "TestNodeId1",
                    TestDate = new DateTime(1997, 3, 12),
                    PumpIntakePressure = 1,
                    PumpDischargePressure = 2,
                    PumpStaticPressure = 3,
                    PressureAcrossPump = 4,
                    FrictionalLossInTubing = 5,
                    CasingPressure = 6,
                    TubingPressure = 7,
                    FlowingBhp = 8,
                    CompositeTubingSpecificGravity = 9,
                    WaterRate = 10,
                    OilRate = 12,
                    WaterSpecificGravity = 13,
                    EnableGasHandling = true,
                    SpecificGravityOfOil = 15,
                    UseDischargeGageInAnalysis = true,
                    DischargeGaugePressure = 17,
                    VerticalPumpDepth = 18,
                    CalculatedFluidLevelAbovePump = 19,
                },
                new ESPAnalysisResultsEntity()
                {
                    NodeId = "TestNodeId3",
                    TestDate = new DateTime(1997, 3, 12),
                    PumpIntakePressure = 1,
                    PumpDischargePressure = 2,
                    PumpStaticPressure = 3,
                    PressureAcrossPump = 4,
                    FrictionalLossInTubing = 5,
                    CasingPressure = 6,
                    TubingPressure = 7,
                    FlowingBhp = 8,
                    CompositeTubingSpecificGravity = 9,
                    WaterRate = 10,
                    OilRate = 12,
                    WaterSpecificGravity = 13,
                    EnableGasHandling = false,
                    SpecificGravityOfOil = 15,
                    UseDischargeGageInAnalysis = false,
                    DischargeGaugePressure = 17,
                    VerticalPumpDepth = 18,
                    CalculatedFluidLevelAbovePump = 19,
                },
                new ESPAnalysisResultsEntity()
                {
                    NodeId = "TestNodeId4",
                    TestDate = new DateTime(1997, 3, 12),
                    PumpIntakePressure = 1,
                    PumpDischargePressure = 2,
                    PumpStaticPressure = 3,
                    PressureAcrossPump = 4,
                    FrictionalLossInTubing = 5,
                    CasingPressure = 6,
                    TubingPressure = 7,
                    FlowingBhp = 8,
                    CompositeTubingSpecificGravity = 9,
                    WaterRate = 10,
                    OilRate = 12,
                    WaterSpecificGravity = 13,
                    EnableGasHandling = true,
                    SpecificGravityOfOil = 15,
                    UseDischargeGageInAnalysis = true,
                    DischargeGaugePressure = 17,
                    VerticalPumpDepth = 18,
                    CalculatedFluidLevelAbovePump = 19,
                },
            };
        }

        private IList<PerforationEntity> GetPerforationsEntity()
        {
            return new List<PerforationEntity>()
            {
                new PerforationEntity()
                {
                    NodeId = "TestNodeId1",
                    Depth = 25,
                    Interval = 10,
                },
                new PerforationEntity()
                {
                    NodeId = "TestNodeId1",
                    Depth = 15,
                    Interval = 5,
                },
                new PerforationEntity()
                {
                    NodeId = "TestNodeId2",
                    Depth = 1,
                    Interval = 1,
                },
                new PerforationEntity()
                {
                    NodeId = "TestNodeId4",
                    Depth = 25,
                    Interval = 10,
                },
                new PerforationEntity()
                {
                    NodeId = "TestNodeId4",
                    Depth = 15,
                    Interval = 5,
                },
            };
        }

        private IList<WellDetailsEntity> GetWellDetailsEntity()
        {
            return new List<WellDetailsEntity>()
            {
                new WellDetailsEntity()
                {
                    NodeId = "TestNodeId1",
                    ProductionDepth = 26,
                }
            };
        }

        #endregion

    }
}
