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
    public class SQLStoreBaseTest
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
        public void SqlDataStoreBaseTestNullContextFactoryTest()
        {
            _ = new SQLStoreBase(null, _loggerFactory.Object);
        }

        [TestMethod]
        public void SqlDataStoreBaseTestConstructorTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            var objSqlDataStore = new SQLStoreBase(contextFactory.Object, _loggerFactory.Object);
            Assert.IsNotNull(objSqlDataStore);
        }

        [TestMethod]
        public void GetCardDataTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            var mockContext = TestUtilities.SetupMockContext();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetNodeMasterData().AsQueryable()).Object);
            mockContext.Setup(x => x.CardData)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCardDatas().AsQueryable()).Object);

            var objCardData = new SQLStoreBase(contextFactory.Object, _loggerFactory.Object);
            var result = objCardData.GetCardData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                new DateTime(2023, 5, 11, 22, 29, 55));

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CardDataModel));
        }

        [TestMethod]
        public void GetCardDataEmptyAssetIdTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            var mockContext = TestUtilities.SetupMockContext();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetNodeMasterData().AsQueryable()).Object);
            mockContext.Setup(x => x.CardData)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCardData().AsQueryable()).Object);

            var objCardData = new SQLStoreBase(contextFactory.Object, _loggerFactory.Object);
            var result = objCardData.GetCardData(Guid.Empty, new DateTime(2023, 5, 11, 22, 29, 55));

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetCurrentRawScanDatasTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            var mockContext = TestUtilities.SetupMockContext();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetNodeMasterData().AsQueryable()).Object);
            mockContext.Setup(x => x.CurrentRawScans)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCurrentRawScanData().AsQueryable()).Object);

            var objCurrentRawScanDatas = new SQLStoreBase(contextFactory.Object, _loggerFactory.Object);
            var result = objCurrentRawScanDatas.GetCurrentRawScanData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"));

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(IList<CurrentRawScanDataModel>));
            Assert.IsTrue(result.Count > 0);
        }

        [TestMethod]
        public void GetCurrentRawScanDatasEmptyAssetIdTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            var mockContext = TestUtilities.SetupMockContext();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetNodeMasterData().AsQueryable()).Object);
            mockContext.Setup(x => x.CurrentRawScans)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCurrentRawScanData().AsQueryable()).Object);

            var objCurrentRawScanDatas = new SQLStoreBase(contextFactory.Object, _loggerFactory.Object);
            var result = objCurrentRawScanDatas.GetCurrentRawScanData(Guid.Empty);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetCustomPumpingUnitTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            var mockContext = TestUtilities.SetupMockContext();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetNodeMasterData().AsQueryable()).Object);
            mockContext.Setup(x => x.CustomPumpingUnits)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCustomPumpingUnitsData().AsQueryable()).Object);

            var objCustomPumpingUnit = new SQLStoreBase(contextFactory.Object, _loggerFactory.Object);
            var result = objCustomPumpingUnit.GetCustomPumpingUnits("CP1");

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CustomPumpingUnitModel));
        }

        [TestMethod]
        public void GetNodeMasterDataTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            var mockContext = TestUtilities.SetupMockContext();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetNodeMasterData().AsQueryable()).Object);
            mockContext.Setup(x => x.Company)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetCompanyData().AsQueryable()).Object);

            var objNodeMasterData = new SQLStoreBase(contextFactory.Object, _loggerFactory.Object);
            var result = objNodeMasterData.GetNodeMasterData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"));

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NodeMasterModel));
        }

        [TestMethod]
        public void GetWellDetailsTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            var mockContext = TestUtilities.SetupMockContext();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetNodeMasterData().AsQueryable()).Object);
            mockContext.Setup(x => x.WellDetails)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetWellDetails().AsQueryable()).Object);

            var objWellDetails = new SQLStoreBase(contextFactory.Object, _loggerFactory.Object);
            var result = objWellDetails.GetWellDetails(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"));

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(WellDetailsModel));
        }

        [TestMethod]
        public void GetWellTestDetailsTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            var mockContext = TestUtilities.SetupMockContext();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetNodeMasterData().AsQueryable()).Object);
            mockContext.Setup(x => x.WellTest)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetWellTestData().AsQueryable()).Object);

            var objWellTestDetails = new SQLStoreBase(contextFactory.Object, _loggerFactory.Object);
            var result = objWellTestDetails.GetWellTestData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                new DateTime(2023, 5, 11, 22, 29, 55));

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(WellTestModel));
        }

        [TestMethod]
        public void GetXDiagResultsTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<XspocDbContext>>();
            var mockContext = TestUtilities.SetupMockContext();
            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetNodeMasterData().AsQueryable()).Object);
            mockContext.Setup(x => x.XDiagResult)
                .Returns(TestUtilities.SetupMockData(TestUtilities.GetXdiagResultsData().AsQueryable()).Object);

            var objXDiagResults = new SQLStoreBase(contextFactory.Object, _loggerFactory.Object);
            var result = objXDiagResults.GetXDiagResultData(new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3"),
                new DateTime(2023, 5, 11, 22, 29, 55));

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(XDiagResultsModel));
        }

        #endregion

        private void SetupThetaLoggerFactory()
        {
            _loggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(_logger.Object);
        }

    }
}
