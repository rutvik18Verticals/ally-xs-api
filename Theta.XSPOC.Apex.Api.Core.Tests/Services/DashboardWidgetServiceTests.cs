using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Services.DashboardService;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models.Dashboard;
using Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Dashboard;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Core.Tests.Services
{
    [TestClass]
    public class DashboardWidgetServiceTests
    {
        private IDashboardWidgetService _dashboardWidgetService;
        private Mock<IThetaLoggerFactory> _mockThetaLoggerFactory;
        private Mock<IDashboardStore> _mockDashboardStore;
        private Mock<IAllyTimeSeriesNodeMaster> _allyNodeMasterStore;

        [TestInitialize]
        public void Setup()
        {
            _mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            var logger = new Mock<IThetaLogger>();            
            _mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);
            _mockDashboardStore = new Mock<IDashboardStore>();
            _allyNodeMasterStore = new Mock<IAllyTimeSeriesNodeMaster>();
            _dashboardWidgetService = new DashboardWidgetService(_mockThetaLoggerFactory.Object, _mockDashboardStore.Object, _allyNodeMasterStore.Object);
        }

        [TestMethod]
        public void Constructor_ValidParameters_ShouldNotThrow()
        {
            // Arrange
            var loggerFactory = new Mock<IThetaLoggerFactory>();
            // Act & Assert
            _ = new DashboardWidgetService(loggerFactory.Object, _mockDashboardStore.Object, _allyNodeMasterStore.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullLoggerFactory_ShouldThrow()
        {
            // Act
            _ = new DashboardWidgetService(null, _mockDashboardStore.Object, _allyNodeMasterStore.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullDashboardStore_ShouldThrow()
        {
            // Act
            _ = new DashboardWidgetService(_mockThetaLoggerFactory.Object, null, _allyNodeMasterStore.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_NullNodeMasterStore_ShouldThrow()
        {
            // Act
            _ = new DashboardWidgetService(_mockThetaLoggerFactory.Object, _mockDashboardStore.Object, null);
        }

        [TestMethod]
        public void SaveWidgetUserPreferences_InvalidInput_ShouldThrowError()
        {
            // Arrange
            var input = new DashboardWidgetUserPreferencesInput
            {
                DashboardName = "TestDashboard",
                WidgetName = "TestWidget",
                PropertyType = "TestProperty",
                Preferences = JArray.Parse("[{'Key':'TestKey','Value':'TestValue'}]") 
            };

            var data = new DashboardWidgetPreferenceDataModel
            {
                DashboardName = input.DashboardName,
                WidgetName = input.WidgetName,
                Preferences = new TimeSeriesChart()
                {
                    ChartType = "TestChartType",
                    ViewOptions = new ViewOptions()
                }
            };

            var userId = "TestUserId";
            var correlationId = "TestCorrelationId";
            _mockDashboardStore.Setup(x => x.SaveDashboardWidgetUserPrefeernces(data, userId, correlationId))
                .Returns(Task.FromResult(true));
            try
            {
                // Act
                var result = _dashboardWidgetService.SaveWidgetUserPreferences(input, userId, correlationId).Result;

                // Assert
                Assert.Fail($"ValidationException was not thrown.");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.InnerException is ValidationException);
            }
        }

        [TestMethod]
        public void SaveWidgetUserPreferences_ValidInput_ShouldReturnTrue()
        {
            // Arrange
            var input = new DashboardWidgetUserPreferencesInput
            {
                DashboardName = "esp-well-charts",
                WidgetName = "timeserieschart",
                PropertyType = "DateSelector",
                Preferences = JObject.Parse(@"{ 'Interval' : '1d', 'Custom' : { 'StartDate' : '', 'EndDate' : '' }, 'CurrentCompletion' : 'false', 'Pop' : 'false' }")
            };
            
            var userId = "TestUserId";
            var correlationId = "TestCorrelationId";
            _mockDashboardStore.Setup(x => x.SaveDashboardWidgetUserPrefeernces(It.IsAny<DashboardWidgetPreferenceDataModel>(), userId, correlationId))
                .Returns(Task.FromResult(true));

            _mockDashboardStore.Setup(x => x.GetDashboardWidgetData(input.DashboardName, input.WidgetName, userId, correlationId))
                .Returns(Task.FromResult(new WidgetPropertyDataModel() { WidgetProperties = new TimeSeriesChart()}));
           
            // Act
            var result = _dashboardWidgetService.SaveWidgetUserPreferences(input, userId, correlationId).Result;

            // Assert
            Assert.IsTrue(result);
           
        }
    }
}
