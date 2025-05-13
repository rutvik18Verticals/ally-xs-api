using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data.Models.Dashboard;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Dashboard;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Data.Mongo.Tests
{
    [TestClass]
    public class DashboardMongoStoreTests
    {
        private IDashboardStore _dashboardStore;
        private Mock<IThetaLoggerFactory> _mockThetaLoggerFactory;
        private Mock<IMongoDatabase> _mockMongoDatabase;


        #region Test Setup

        [TestInitialize]
        public void Setup()
        {
            var logger = new Mock<IThetaLogger>();
            _mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            _mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            // Initialize the dashboard store with a mock or a real implementation
            var mockCollection = new Mock<IMongoCollection<DashboardWidgetUserSettings>>();
            mockCollection.Setup(m => m.InsertOneAsync(It.IsAny<DashboardWidgetUserSettings>(), null, default))
                .Returns(Task.CompletedTask);

            mockCollection.Setup(m => m.FindSync(It.IsAny<FilterDefinition<DashboardWidgetUserSettings>>(),
               It.IsAny<FindOptions<DashboardWidgetUserSettings>>(), It.IsAny<CancellationToken>()))
               .Returns(GetDashboardWidgetUserSettingsModel());

            _mockMongoDatabase = new Mock<IMongoDatabase>();
            _mockMongoDatabase.Setup(m => m.GetCollection<DashboardWidgetUserSettings>("DashboardWidgetUserSettings", null)).Returns(mockCollection.Object);
            
            _dashboardStore = new DashboardMongoStore(_mockMongoDatabase.Object, _mockThetaLoggerFactory.Object);
        }

        #endregion

        #region Test Methods

        [TestMethod]
        public void Constructor_ValidParameters_ShouldNotThrow()
        {
            // Arrange
            var loggerFactory = new Mock<IThetaLoggerFactory>();
            var dashboardStore = new Mock<IDashboardStore>();
            // Act & Assert
            var service = new DashboardMongoStore(_mockMongoDatabase.Object, loggerFactory.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullLoggerFactory_ShouldThrow()
        {
            // Arrange
            IDashboardStore dashboardStore = new Mock<IDashboardStore>().Object;
            // Act
            var service = new DashboardMongoStore(_mockMongoDatabase.Object, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullDashboardStore_ShouldThrow()
        {
            // Arrange
            IThetaLoggerFactory loggerFactory = new Mock<IThetaLoggerFactory>().Object;
            // Act
            var service = new DashboardMongoStore(null, loggerFactory);
        }

        [TestMethod]
        public void SaveDashboardWidgetUserPrefeernces_ShouldSuccess()
        {
            // Arrange
            var userId = "TestUserId";
            var correlationId = "TestCorrelationId";

            var data = new DashboardWidgetPreferenceDataModel()
            {
                DashboardName = "TestDashboard",
                WidgetName = "TestWidget",              
                Preferences = new TimeSeriesChart()
                {
                    ChartType = "TestChartType",
                    DateSelector = new DateSelector()
                    {
                        Interval = "TestInterval",
                        Pop = "TestPop",
                    }
                }
            };

            // Act
            var result = _dashboardStore.SaveDashboardWidgetUserPrefeernces(data, userId, correlationId);

            // Assert   
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Result);

        }

        #endregion

        #region Private Methods

        private IAsyncCursor<DashboardWidgetUserSettings> GetDashboardWidgetUserSettingsModel()
        {
            var result = new Mock<IAsyncCursor<DashboardWidgetUserSettings>>();
            result.Setup(m => m.Current).Returns(new List<DashboardWidgetUserSettings>());

            result.SetupSequence(m => m.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);

            return result.Object;
        }

        #endregion
    }
}
