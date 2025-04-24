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
    public class GLAnalysisSQLStoreTests
    {

        #region Test Methods

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorTest()
        {
            _ = new GLAnalysisSQLStore(null, null);
        }

        [TestMethod]
        public void GLAnalysisGetSurveyDateTest()
        {
            var surveyDate = GetSurveyDate().AsQueryable();
            var mockSurveyDateDbSet = SetupSurveyDate(surveyDate);

            var analysisResult = GetAnalysisResultCurvesData().AsQueryable();
            var mockAnalysisResultDbSet = SetupAnalysisResultCurve(analysisResult);

            var nodeMasterData = GetNodeMasterData().AsQueryable();
            var mockNodeMasterDbSet = SetupNodeMasterGroups(nodeMasterData);

            var nodeCurveTypes = GetCurveTypesData().AsQueryable();
            var mockCurveTypesDbSet = SetupCurveType(nodeCurveTypes);

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.SurveyData).Returns(mockSurveyDateDbSet.Object);
            mockContext.Setup(x => x.AnalysisResultCurves).Returns(mockAnalysisResultDbSet.Object);
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContext.Setup(x => x.CurveTypes).Returns(mockCurveTypesDbSet.Object);

            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var correlationId = Guid.NewGuid().ToString();
            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var glAnalysis = new GLAnalysisSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);

            var id = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            var resultByAssetId = glAnalysis.GetGLAnalysisSurveyDate(id, 1, 2, 2, correlationId);
            Assert.IsNotNull(resultByAssetId);
            Assert.AreEqual(1, resultByAssetId.Count);
        }

        [TestMethod]
        public void GasLiftAnalysisTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockSqlStore = new Mock<ISQLStoreBase>();
            mockSqlStore.Setup(x => x.GetNodeMasterData(It.IsAny<Guid>())).Returns(GetNodeMasterModel().AsQueryable().FirstOrDefault());
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
            mockContext.Setup(x => x.GLAnalysisResults)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLAnalysisResults().AsQueryable()).Object);
            mockContext.Setup(x => x.GLWellDetail)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLWellDataResults().AsQueryable()).Object);
            mockContext.Setup(x => x.GLWellOrifice)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLWellOrificeData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLWellValve)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLWellValveResultData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLValveStatus)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLValveStatusResultData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLValve)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLValveResultData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLOrificeStatus)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLOrificeStatusData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLWellOrifice)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GLWellOrificeData().AsQueryable()).Object);
            mockContext.Setup(x => x.SystemParameters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetSystemParameter().AsQueryable()).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var correlationId = Guid.NewGuid().ToString();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var glAnalysis = new GLAnalysisSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);

            var response =
                glAnalysis.GetGLAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                "2023-05-11 22:29:55.000", 1, correlationId);
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response.NodeMasterData, typeof(NodeMasterModel));
            Assert.IsNotNull(response.NodeMasterData);
            Assert.IsInstanceOfType(response.AnalysisResultEntity, typeof(GLAnalysisResultModel));
            Assert.IsNotNull(response.AnalysisResultEntity);
            Assert.IsInstanceOfType(response.WellValveEntities, typeof(IList<GLWellValveModel>));
            Assert.IsNotNull(response.WellValveEntities);
            Assert.IsInstanceOfType(response.WellOrificeStatus, typeof(GLWellOrificeStatusModel));
            Assert.IsNotNull(response.WellOrificeStatus);
            Assert.IsInstanceOfType(response.WellDetail, typeof(GLWellDetailModel));
            Assert.IsNotNull(response.WellDetail);
            Assert.IsInstanceOfType(response.ValveStatusEntities, typeof(IList<GLValveStatusModel>));
            Assert.IsNotNull(response.ValveStatusEntities);
        }

        [TestMethod]
        public void GLAnalysisNullGLAnalysisResultTest()
        {
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
            mockContext.Setup(x => x.GLAnalysisResults)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLAnalysisResults().AsQueryable()).Object);
            mockContext.Setup(x => x.GLWellDetail)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLWellDataResults().AsQueryable()).Object);
            mockContext.Setup(x => x.GLWellOrifice)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLWellOrificeData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLWellValve)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLWellValveResultData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLValveStatus)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLValveStatusResultData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLValve)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLValveResultData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLOrificeStatus)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLOrificeStatusData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLWellOrifice)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GLWellOrificeData().AsQueryable()).Object);

            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var correlationId = Guid.NewGuid().ToString();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var glAnalysis = new GLAnalysisSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);

            var response =
                glAnalysis.GetGLAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                "2023-05-11 22:29:55.000", 1, correlationId);
            Assert.IsNull(response);
        }

        [TestMethod]
        public void GetGLAnalysisCurveCoordinatesSurveyDateTest()
        {
            var surveyDate = GetCurveCoordinateSurveyDate().AsQueryable();
            var mockSurveyDateDbSet = SetupSurveyDate(surveyDate);

            var nodeMasterData = GetNodeMasterData().AsQueryable();
            var mockNodeMasterDbSet = SetupNodeMasterGroups(nodeMasterData);

            var companyData = TestUtilities.GetCompanyData().AsQueryable();
            var mockCompanyDbSet = SetupCompany(companyData);

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.SurveyData).Returns(mockSurveyDateDbSet.Object);
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContext.Setup(x => x.Company).Returns(mockCompanyDbSet.Object);

            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var correlationId = Guid.NewGuid().ToString();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var glAnalysis = new GLAnalysisSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);

            var id = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3");
            var resultByAssetId = glAnalysis.GetGLAnalysisCurveCoordinatesSurveyDate(id,
                "2023-11-30 22:27:13", correlationId);
            Assert.IsNotNull(resultByAssetId);
            Assert.AreEqual(4, resultByAssetId.Count);
        }

        [TestMethod]
        public void GetGLAnalysisCurveCoordinatesSurveyDateInvalidIdTest()
        {
            var surveyDate = GetCurveCoordinateSurveyDate().AsQueryable();
            var mockSurveyDateDbSet = SetupSurveyDate(surveyDate);

            var nodeMasterData = GetNodeMasterData().AsQueryable();
            var mockNodeMasterDbSet = SetupNodeMasterGroups(nodeMasterData);

            var companyData = TestUtilities.GetCompanyData().AsQueryable();
            var mockCompanyDbSet = SetupCompany(companyData);

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.SurveyData).Returns(mockSurveyDateDbSet.Object);
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContext.Setup(x => x.Company).Returns(mockCompanyDbSet.Object);

            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var correlationId = Guid.NewGuid().ToString();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var glAnalysis = new GLAnalysisSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);

            var id = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD4");
            var resultByAssetId = glAnalysis.GetGLAnalysisCurveCoordinatesSurveyDate(id, "2023-11-30 22:27:13"
                , correlationId);

            Assert.AreEqual(0, resultByAssetId.Count);
        }

        [TestMethod]
        public void GetWellboreDataReturnsWellboreData()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();

            var nodeMasterData = GetNodeMasterData().AsQueryable();
            var tubingData = GetTubingData().AsQueryable();
            var perforationsData = GetPerforationData().AsQueryable();
            var wellDetailsData = GetWellDetailsData().AsQueryable();
            var mockNodeMasterDbSet = SetupNodeMasterGroups(nodeMasterData);

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.Tubings).Returns(TestUtilities.SetupMockData(tubingData).Object);
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContext.Setup(x => x.Perforation).Returns(TestUtilities.SetupMockData(perforationsData).Object);
            mockContext.Setup(x => x.WellDetails).Returns(TestUtilities.SetupMockData(wellDetailsData).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var correlationId = Guid.NewGuid().ToString();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var glAnalysis = new GLAnalysisSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);

            var result = glAnalysis.GetWellboreData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"), correlationId);

            Assert.IsNotNull(result);
            Assert.AreEqual(1000, result.ProductionDepth);
            Assert.AreEqual(500, result.PackerDepth);
            Assert.IsTrue(result.HasPacker);

            Assert.IsNotNull(result.Tubings);
            Assert.AreEqual(3, result.Tubings.Count);
            Assert.AreEqual(100, result.Tubings[0].Length);
            Assert.AreEqual(150, result.Tubings[1].Length);
            Assert.AreEqual(200, result.Tubings[2].Length);

            Assert.IsNotNull(result.Perforations);
            Assert.AreEqual(3, result.Perforations.Count);
            Assert.AreEqual(100, result.Perforations[0].TopDepth);
            Assert.AreEqual(150, result.Perforations[1].TopDepth);
            Assert.AreEqual(200, result.Perforations[2].TopDepth);

        }

        [TestMethod]
        public void GetWellboreDataWellDetailsNotFoundReturnsNull()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();

            var nodeMasterData = GetNodeMasterData().AsQueryable();
            var tubingData = GetTubingData().AsQueryable();
            var perforationsData = GetPerforationData().AsQueryable();
            var wellDetailsData = GetWellDetailsData().AsQueryable();
            var mockNodeMasterDbSet = SetupNodeMasterGroups(nodeMasterData);

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.Tubings).Returns(TestUtilities.SetupMockData(tubingData).Object);
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContext.Setup(x => x.Perforation).Returns(TestUtilities.SetupMockData(perforationsData).Object);
            mockContext.Setup(x => x.WellDetails).Returns(TestUtilities.SetupMockData(wellDetailsData).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var correlationId = Guid.NewGuid().ToString();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var glAnalysis = new GLAnalysisSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);

            var result = glAnalysis.GetWellboreData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755555555"), correlationId);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetGLSensitivityAnalysisDataTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();

            var nodeMasterData = TestUtilities.GetNodeMasterData().AsQueryable();
            var tubingData = GetTubingData().AsQueryable();
            var perforationsData = GetPerforationData().AsQueryable();
            var wellDetailsData = GetWellDetailsData().AsQueryable();
            var companyData = TestUtilities.GetCompanyData().AsQueryable();
            var mockNodeMasterDbSet = SetupNodeMasterGroups(nodeMasterData);

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.Tubings).Returns(TestUtilities.SetupMockData(tubingData).Object);
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContext.Setup(x => x.Perforation).Returns(TestUtilities.SetupMockData(perforationsData).Object);
            mockContext.Setup(x => x.WellDetails).Returns(TestUtilities.SetupMockData(wellDetailsData).Object);
            mockContext.Setup(x => x.Company).Returns(TestUtilities.SetupMockData(companyData).Object);
            mockContext.Setup(x => x.GLAnalysisResults).Returns(TestUtilities.SetupMockData(new List<GLAnalysisResultsEntity>()
            {
                new GLAnalysisResultsEntity()
                {
                    Id = 1,
                    NodeId = "AssetId1",
                    GrossRate = 421.625f,
                    TestDate = new DateTime(2023, 5, 11, 22, 29, 55),
                    ProcessedDate = new DateTime(2023, 5, 11, 22, 29, 55),
                    Success = true,
                    GasInjectionDepth = 6530.91f,
                    OilRate = 75,
                    WaterRate = 225,
                    GasRate = 500,
                    WellheadPressure = 150,
                    CasingPressure = 850,
                    WaterCut = 75,
                    GasSpecificGravity = 0.758f,
                    WaterSpecificGravity = 1.06f,
                    WellheadTemperature = 100,
                    BottomholeTemperature = 200,
                    OilSpecificGravity = 0.8250729f,
                    CasingId = 6.135f,
                    TubingId = 2.441f,
                    TubingOD = 2.875f,
                    ReservoirPressure = 1500,
                    BubblepointPressure = 1000,
                    AnalysisType = 2,
                }
            }.AsQueryable()).Object);
            mockContext.Setup(x => x.GLWellDetail)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLWellDataResults().AsQueryable()).Object);
            mockContext.Setup(x => x.GLWellValve)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLWellValveResultData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLValveStatus)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLValveStatusResultData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLValve)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLValveResultData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLOrificeStatus)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLOrificeStatusData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLWellOrifice)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GLWellOrificeData().AsQueryable()).Object);
            mockContext.Setup(x => x.SystemParameters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetSystemParameter().AsQueryable()).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var correlationId = Guid.NewGuid().ToString();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var glAnalysis = new GLAnalysisSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);

            var result = glAnalysis.GetGLSensitivityAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                "2023-05-11 22:29:55", 1, correlationId);

            Assert.IsNotNull(result);
            Assert.AreEqual("AssetId1", result.AnalysisResultEntity.NodeId);
        }

        [TestMethod]
        public void GetGLSensitivityAnalysisDataMissingAnalysisResultTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();

            var nodeMasterData = TestUtilities.GetNodeMasterData().AsQueryable();
            var tubingData = GetTubingData().AsQueryable();
            var perforationsData = GetPerforationData().AsQueryable();
            var wellDetailsData = GetWellDetailsData().AsQueryable();
            var mockNodeMasterDbSet = SetupNodeMasterGroups(nodeMasterData);
            var companyData = TestUtilities.GetCompanyData().AsQueryable();

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.Tubings).Returns(TestUtilities.SetupMockData(tubingData).Object);
            mockContext.Setup(x => x.NodeMasters).Returns(mockNodeMasterDbSet.Object);
            mockContext.Setup(x => x.Perforation).Returns(TestUtilities.SetupMockData(perforationsData).Object);
            mockContext.Setup(x => x.WellDetails).Returns(TestUtilities.SetupMockData(wellDetailsData).Object);
            mockContext.Setup(x => x.Company).Returns(TestUtilities.SetupMockData(companyData).Object);
            mockContext.Setup(x => x.GLAnalysisResults)
                .Returns(TestUtilities.SetupMockData(new List<GLAnalysisResultsEntity>().AsQueryable()).Object);
            mockContext.Setup(x => x.GLWellDetail)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLWellDataResults().AsQueryable()).Object);
            mockContext.Setup(x => x.GLWellValve)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLWellValveResultData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLValveStatus)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLValveStatusResultData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLValve)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLValveResultData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLOrificeStatus)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLOrificeStatusData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLWellOrifice)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GLWellOrificeData().AsQueryable()).Object);
            mockContext.Setup(x => x.SystemParameters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetSystemParameter().AsQueryable()).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);
            var correlationId = Guid.NewGuid().ToString();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var glAnalysis = new GLAnalysisSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);

            var result = glAnalysis.GetGLSensitivityAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                "2023-05-11 22:29:55", 1, correlationId);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetGLAnalysisDataGuidEmptyTest()
        {
            var nodeData = new List<NodeMasterEntity>().AsQueryable();

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.NodeMasters).Returns(TestUtilities.SetupMockData(nodeData).Object);
            mockContext.Setup(x => x.GLAnalysisResults)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLAnalysisResults().AsQueryable()).Object);
            mockContext.Setup(x => x.GLWellDetail)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLWellDataResults().AsQueryable()).Object);
            mockContext.Setup(x => x.GLWellOrifice)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLWellOrificeData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLWellValve)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLWellValveResultData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLValveStatus)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLValveStatusResultData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLValve)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLValveResultData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLOrificeStatus)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLOrificeStatusData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLWellOrifice)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GLWellOrificeData().AsQueryable()).Object);
            mockContext.Setup(x => x.Company)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCompanyData().AsQueryable()).Object);

            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var correlationId = Guid.NewGuid().ToString();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var glAnalysis = new GLAnalysisSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);

            var response =
                glAnalysis.GetGLAnalysisData(Guid.Empty, "2023-05-11 22:29:55.000", 1, correlationId);
            Assert.IsNull(response);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetGLAnalysisDataAnalysisResultIdZeroTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var mockSqlStore = new Mock<ISQLStoreBase>();
            mockSqlStore.Setup(x => x.GetNodeMasterData(It.IsAny<Guid>()))
                .Returns(GetNodeMasterModel().AsQueryable().FirstOrDefault());

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetNodeMasterData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLAnalysisResults).Returns(TestUtilities.SetupMockData(new List<GLAnalysisResultsEntity>()
            {
                new GLAnalysisResultsEntity()
                {
                    Id = 0,
                    NodeId = "AssetId1",
                    GrossRate = 421.625f,
                    TestDate = new DateTime(2023, 5, 11, 22, 29, 55),
                    ProcessedDate = new DateTime(2023, 5, 11, 22, 29, 55),
                    Success = true,
                    GasInjectionDepth = 6530.91f,
                    OilRate = 75,
                    WaterRate = 225,
                    GasRate = 500,
                    WellheadPressure = 150,
                    CasingPressure = 850,
                    WaterCut = 75,
                    GasSpecificGravity = 0.758f,
                    WaterSpecificGravity = 1.06f,
                    WellheadTemperature = 100,
                    BottomholeTemperature = 200,
                    OilSpecificGravity = 0.8250729f,
                    CasingId = 6.135f,
                    TubingId = 2.441f,
                    TubingOD = 2.875f,
                    ReservoirPressure = 1500,
                    BubblepointPressure = 1000,
                    AnalysisType = 1,
                }
            }.AsQueryable()).Object);
            mockContext.Setup(x => x.GLWellDetail)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLWellDataResults().AsQueryable()).Object);
            mockContext.Setup(x => x.GLWellOrifice)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLWellOrificeData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLWellValve)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLWellValveResultData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLValveStatus)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLValveStatusResultData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLValve)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLValveResultData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLOrificeStatus)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetGLOrificeStatusData().AsQueryable()).Object);
            mockContext.Setup(x => x.GLWellOrifice)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GLWellOrificeData().AsQueryable()).Object);
            mockContext.Setup(x => x.SystemParameters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetSystemParameter().AsQueryable()).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var correlationId = Guid.NewGuid().ToString();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var glAnalysis = new GLAnalysisSQLStore(contextFactory.Object, mockThetaLoggerFactory.Object);

            _ = glAnalysis.GetGLAnalysisData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                , "2023-05-11 22:29:55.000", 1, correlationId);
        }

        #endregion

        #region Private Methods

        private IList<SurveyDataEntity> GetSurveyDate()
        {
            var eventData = new List<SurveyDataEntity>()
            {
                new SurveyDataEntity()
                {
                    Id = 101,
                    NodeId = "Well1",
                    SurveyDate = DateTime.Now,
                },
                new SurveyDataEntity()
                {
                    Id = 102,
                    NodeId = "Well1",
                    SurveyDate = DateTime.Now,
                },
                new SurveyDataEntity()
                {
                    Id = 103,
                    NodeId = "Well1",
                    SurveyDate = DateTime.Now,
                },
                new SurveyDataEntity()
                {
                    Id = 104,
                    NodeId = "Well1",
                    SurveyDate = DateTime.Now,
                },
            };

            return eventData;
        }

        private IList<AnalysisResultCurvesEntity> GetAnalysisResultCurvesData()
        {
            var eventData = new List<AnalysisResultCurvesEntity>()
            {
                new AnalysisResultCurvesEntity()
                {
                    Id = 101,
                    AnalysisResultId = 101,
                    CurveTypeId = 2,
                },
                new AnalysisResultCurvesEntity()
                {
                    Id = 102,
                    AnalysisResultId = 101,
                    CurveTypeId = 2,
                },
                new AnalysisResultCurvesEntity()
                {
                    Id = 103,
                    AnalysisResultId = 101,
                    CurveTypeId = 2,
                },
            };

            return eventData;
        }

        private IList<CurveTypesEntity> GetCurveTypesData()
        {
            var groupMembershipCacheData = new List<CurveTypesEntity>()
            {
                new CurveTypesEntity()
                {
                    Id = 101,
                    ApplicationTypeId = 1,
                    Name = "Name",
                    PhraseId = 1,
                },
                new CurveTypesEntity()
                {
                    Id = 2,
                    ApplicationTypeId = 1,
                    Name = "Name",
                    PhraseId = 1,
                },
            };

            return groupMembershipCacheData;
        }

        private IList<NodeMasterEntity> GetNodeMasterData()
        {
            var nodeMasters = new List<NodeMasterEntity>()
            {
                new NodeMasterEntity()
                {
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                    NodeId = "Well1",
                    CompanyId = 1
                }
            };

            return nodeMasters;
        }

        private IList<NodeMasterModel> GetNodeMasterModel()
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

        #endregion

        #region Private Setup Methods

        private Mock<DbSet<SurveyDataEntity>> SetupSurveyDate(IQueryable<SurveyDataEntity> data)
        {
            var mockDbSet = new Mock<DbSet<SurveyDataEntity>>();
            mockDbSet.As<IQueryable<SurveyDataEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<SurveyDataEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<SurveyDataEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<SurveyDataEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet;
        }

        private Mock<DbSet<AnalysisResultCurvesEntity>> SetupAnalysisResultCurve(IQueryable<AnalysisResultCurvesEntity> data)
        {
            var mockDbSet = new Mock<DbSet<AnalysisResultCurvesEntity>>();
            mockDbSet.As<IQueryable<AnalysisResultCurvesEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<AnalysisResultCurvesEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<AnalysisResultCurvesEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<AnalysisResultCurvesEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet;
        }

        private Mock<DbSet<NodeMasterEntity>> SetupNodeMasterGroups(IQueryable<NodeMasterEntity> data)
        {
            var mockDbSet = new Mock<DbSet<NodeMasterEntity>>();
            mockDbSet.As<IQueryable<NodeMasterEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<NodeMasterEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<NodeMasterEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<NodeMasterEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet;
        }

        private Mock<DbSet<CurveTypesEntity>> SetupCurveType(IQueryable<CurveTypesEntity> data)
        {
            var mockDbSet = new Mock<DbSet<CurveTypesEntity>>();
            mockDbSet.As<IQueryable<CurveTypesEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<CurveTypesEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<CurveTypesEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<CurveTypesEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet;
        }

        private Mock<DbSet<CompanyEntity>> SetupCompany(IQueryable<CompanyEntity> data)
        {
            var mockDbSet = new Mock<DbSet<CompanyEntity>>();
            mockDbSet.As<IQueryable<CompanyEntity>>().Setup(x => x.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<CompanyEntity>>().Setup(x => x.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<CompanyEntity>>().Setup(x => x.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<CompanyEntity>>().Setup(x => x.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet;
        }

        private IList<SurveyDataEntity> GetCurveCoordinateSurveyDate()
        {
            var eventData = new List<SurveyDataEntity>()
            {
                new SurveyDataEntity()
                {
                    Id = 101,
                    NodeId = "Well1",
                    SurveyDate = Convert.ToDateTime("2023-11-30 22:27:13.930"),
                },
                new SurveyDataEntity()
                {
                    Id = 102,
                    NodeId = "Well1",
                    SurveyDate = Convert.ToDateTime("2023-11-30 22:27:13.930"),
                },
                new SurveyDataEntity()
                {
                    Id = 103,
                    NodeId = "Well1",
                    SurveyDate = Convert.ToDateTime("2023-11-30 22:27:13.930"),
                },
                new SurveyDataEntity()
                {
                    Id = 104,
                    NodeId = "Well1",
                    SurveyDate = Convert.ToDateTime("2023-11-30 22:27:13.930"),
                },
            };

            return eventData;
        }

        private IList<TubingEntity> GetTubingData()
        {
            var eventData = new List<TubingEntity>()
            {
                new TubingEntity()
                {
                    NodeId = "Well1",
                    Length = 100,
                },
                new TubingEntity()
                {
                    NodeId = "Well1",
                    Length = 150,
                },
                new TubingEntity()
                {
                    NodeId = "Well1",
                    Length = 200,
                },
            };

            return eventData;
        }

        private IList<PerforationEntity> GetPerforationData()
        {
            var eventData = new List<PerforationEntity>()
               {
                new PerforationEntity()
                {
                    NodeId = "Well1",
                    Depth = 100,
                },
                new PerforationEntity()
                {
                    NodeId = "Well1",
                    Depth = 150,
                },
                new PerforationEntity()
                {
                    NodeId = "Well1",
                    Depth = 200,
                },
            };

            return eventData;
        }

        private IList<WellDetailsEntity> GetWellDetailsData()
        {
            var eventData = new List<WellDetailsEntity>()
            {
                new WellDetailsEntity()
                {
                    NodeId = "Well1",
                    ProductionDepth = 1000,
                    PackerDepth = 500,
                    HasPacker = true,
                },
            };

            return eventData;
        }

        #endregion

    }
}
