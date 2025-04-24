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
    public class ESPPumpDataTests
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
        public void ConstructorNullContextFactoryTest()
        {
            _ = new ESPPumpDataSQLStore(null, _loggerFactory.Object);
        }

        [TestMethod]
        public void ESPPumpDataTest()
        {
            var contextFactory = new Mock<IThetaDbContextFactory<NoLockXspocDbContext>>();
            var nodeData = new List<NodeMasterEntity>()
            {
                new NodeMasterEntity()
                {
                    NodeId = "AssetId1",
                    PocType = 8,
                    InferredProd = 4,
                    AssetGuid = new Guid("DFC1D0AD-A824-4965-B78D-AB7755E32DD3")
                }
            }.AsQueryable();
            var correlationId = Guid.NewGuid().ToString();

            var mockContext = TestUtilities.SetupMockContext();
            mockContext.Setup(x => x.NodeMasters)
                .Returns(TestUtilities.SetupMockData(nodeData).Object);

            mockContext.Setup(x => x.ESPPumps)
                .Returns(TestUtilities.SetupMockData(TestUtilities.ESPPumpData().AsQueryable()).Object);
            mockContext.Setup(x => x.ESPManufacturers)
                .Returns(TestUtilities.SetupMockData(TestUtilities.ESPManufacturerData().AsQueryable()).Object);
            mockContext.Setup(x => x.ESPCurvePoints)
                .Returns(TestUtilities.SetupMockData(TestUtilities.ESPCurvePointData().AsQueryable()).Object);

            contextFactory.Setup(m => m.GetContext()).Returns(mockContext.Object);

            var espPumpData = new ESPPumpDataSQLStore(contextFactory.Object, _loggerFactory.Object);

            var response = espPumpData.GetESPPumpData(4, correlationId);
            Assert.IsNotNull(response);
        }

        #endregion

        private void SetupThetaLoggerFactory()
        {
            _loggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(_logger.Object);
        }

    }
}
