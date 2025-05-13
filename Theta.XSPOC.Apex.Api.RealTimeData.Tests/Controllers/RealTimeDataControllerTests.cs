using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Globalization;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Influx.Services;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.RealTimeData.Contracts.Requests;
using Theta.XSPOC.Apex.Api.RealTimeData.Controllers;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.RealTimeData.Tests.Controllers
{
    [TestClass]
    public class RealTimeDataControllerTests
    {
        private RealTimeDataController _controller;
        private Mock<IThetaLoggerFactory> _loggerFactory;
        private Mock<IThetaLogger> _logger;
        private IRealTimeDataProcessingService _realTimeDataProcessingService;
        private Mock<IRealTimeDataProcessingService> _mockRealTimeDataProcessingService;
        private Mock<IAllyTimeSeriesNodeMaster> _allyNodeMasterStore;
        private Mock<IConfiguration> _config;
        private Mock<IParameterMongoStore> _parameterMongoStore;
        private Mock<IConfiguration> _configuration;

        [TestInitialize]
        public void Setup()
        {
            _loggerFactory = new Mock<IThetaLoggerFactory>();
            _logger = new Mock<IThetaLogger>();
            _allyNodeMasterStore = new Mock<IAllyTimeSeriesNodeMaster>();
            _mockRealTimeDataProcessingService = new Mock<IRealTimeDataProcessingService>();
            _parameterMongoStore = new Mock<IParameterMongoStore>();
            _parameterMongoStore.Setup(x => x.GetParameterByParamStdType(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new Data.Models.MongoCollection.Parameter.Parameters());
            
            _configuration = new Mock<IConfiguration> { };
            _configuration.Setup(x => x.GetSection("TimeZoneBehavior:ApplicationTimeZone").Value).Returns("India Standard Time");

            _realTimeDataProcessingService = new RealTimeDataProcessingService(_loggerFactory.Object, _allyNodeMasterStore.Object,
                _parameterMongoStore.Object, new Mock<IGetDataHistoryItemsService>().Object, new Mock<IDateTimeConverter>().Object, _configuration.Object);
            
            _config = new Mock<IConfiguration>();            

            _loggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(_logger.Object);
            _controller = new RealTimeDataController(_loggerFactory.Object, _mockRealTimeDataProcessingService.Object, _config.Object);
        }

        [TestMethod]
        public void Constructor_NullTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new RealTimeDataController(null, null, null));
        }

        [TestMethod]
        public void GetDowntimeData_NullRequest_ThrowsBadRequest()
        {   
            // Act
            var result = _controller.GetAssetDowntimeAndShutdownData(null).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, ((BadRequestObjectResult)result).StatusCode);
            Assert.AreEqual("Invalid Defaults", ((BadRequestObjectResult)result).Value);
        }

        [TestMethod]
        public void GetDowntimeData_NullAssetId_ThrowsBadRequest()
        {
            // Arrange
            var request = new GraphDataRequest
            {
                AssetId = null,
                StartDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture),
                EndDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture),
                Aggregate = "hour",
                AggregateMethod = "avg"
            };

            // Act
            var result = _controller.GetAssetDowntimeAndShutdownData(request).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, ((BadRequestObjectResult)result).StatusCode);
            Assert.AreEqual("AssetId is required.", ((BadRequestObjectResult)result).Value);
        }

        [TestMethod]
        public void GetDowntimeData_NullStartDate_ThrowsBadRequest()
        {
            // Arrange
            var request = new GraphDataRequest
            {
                AssetId = Guid.NewGuid().ToString(),
                StartDate = "",
                EndDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture),
                Aggregate = "hour",
                AggregateMethod = "avg"
            };

            // Act
            var result = _controller.GetAssetDowntimeAndShutdownData(request).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, ((BadRequestObjectResult)result).StatusCode);
            Assert.AreEqual("Start Date is not provided or provided format is incorrect.", ((BadRequestObjectResult)result).Value);
        }

        [TestMethod]
        public void GetDowntimeData_NullEndDate_ThrowsBadRequest()
        {
            // Arrange
            var request = new GraphDataRequest
            {
                AssetId = Guid.NewGuid().ToString(),
                StartDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture),
                EndDate = "",
                Aggregate = "hour",
                AggregateMethod = "avg"
            };

            // Act
            var result = _controller.GetAssetDowntimeAndShutdownData(request).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, ((BadRequestObjectResult)result).StatusCode);
            Assert.AreEqual("End Date is not provided or provided format is incorrect.", ((BadRequestObjectResult)result).Value);
        }

        [TestMethod]
        public void GetDowntimeData_StartDate_GreaterThan_EndDate_ThrowsBadRequest()
        {
            // Arrange
            var request = new GraphDataRequest
            {
                AssetId = Guid.NewGuid().ToString(),
                StartDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture),
                EndDate = DateTime.UtcNow.AddDays(-5).ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture),
                Aggregate = "hour",
                AggregateMethod = "avg"
            };

            // Act
            var result = _controller.GetAssetDowntimeAndShutdownData(request).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, ((BadRequestObjectResult)result).StatusCode);
            Assert.AreEqual("Start date cannot be greater than end date.", ((BadRequestObjectResult)result).Value);
        }

        [TestMethod]
        public void GetDowntimeData_ValidRequest_Calls_ServiceMethod()
        {
            // Arrange
            var request = new GraphDataRequest
            {
                AssetId = Guid.NewGuid().ToString(),
                StartDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture),
                EndDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture),
                Aggregate = "hour",
                AggregateMethod = "avg"
            };
           
            _mockRealTimeDataProcessingService.Setup(x => x.GetDowntime(It.IsAny<WithCorrelationId<GraphDataInput>>())).Returns(Task.FromResult(new Core.Models.Outputs.WellDowntimeDataOutput()));

            // Act
            var result = _controller.GetAssetDowntimeAndShutdownData(request).Result;
            
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
            _mockRealTimeDataProcessingService.Verify(x => x.GetDowntime(It.IsAny<WithCorrelationId<GraphDataInput>>()), Times.Once);
        }

        [TestMethod]
        public void GetDowntimeData_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new GraphDataRequest
            {
                AssetId = Guid.NewGuid().ToString(),
                StartDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture),
                EndDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture),
                Aggregate = "hour",
                AggregateMethod = "avg"
            };

            _mockRealTimeDataProcessingService.Setup(x => x.GetDowntime(It.IsAny<WithCorrelationId<GraphDataInput>>())).Returns(Task.FromResult(new Core.Models.Outputs.WellDowntimeDataOutput()));

            // Act
            var result = _controller.GetAssetDowntimeAndShutdownData(request).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetDowntimeData_ValidRequest_For_ESP_Wells_ReturnsOk()
        {
            // Arrange
            var request = new GraphDataRequest
            {
                AssetId = Guid.NewGuid().ToString(),
                StartDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture),
                EndDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture),
                Aggregate = "hour",
                AggregateMethod = "avg"
            };

            var nodeData = Task.FromResult((IList<NodeMasterModel>)new List<NodeMasterModel>() { new NodeMasterModel { ApplicationId = 4 } });  // ESP Wells

            _allyNodeMasterStore.Setup(x => x.GetByAssetIdsForAllyTimeSeriesAsync(It.IsAny<List<Guid>>(), It.IsAny<string>()))
                .Returns(nodeData);

            _controller = new RealTimeDataController(_loggerFactory.Object, _realTimeDataProcessingService, _config.Object);

            // Act
            var result = _controller.GetAssetDowntimeAndShutdownData(request).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetDowntimeData_ValidRequest_For_GL_Wells_ReturnsOk()
        {
            // Arrange
            var request = new GraphDataRequest
            {
                AssetId = Guid.NewGuid().ToString(),
                StartDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture),
                EndDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture),
                Aggregate = "hour",
                AggregateMethod = "avg"
            };

            var nodeData = Task.FromResult((IList<NodeMasterModel>)new List<NodeMasterModel>() { new NodeMasterModel { ApplicationId = 7 } });  // GL Wells

            _allyNodeMasterStore.Setup(x => x.GetByAssetIdsForAllyTimeSeriesAsync(It.IsAny<List<Guid>>(), It.IsAny<string>()))
                .Returns(nodeData);

            _controller = new RealTimeDataController(_loggerFactory.Object, _realTimeDataProcessingService, _config.Object);

            // Act
            var result = _controller.GetAssetDowntimeAndShutdownData(request).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetDowntimeData_InvalidRequest_For_Wrong_WellType_ThrowsError()
        {
            // Arrange
            var request = new GraphDataRequest
            {
                AssetId = Guid.NewGuid().ToString(),
                StartDate = DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture),
                EndDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture),
                Aggregate = "hour",
                AggregateMethod = "avg"
            };

            var nodeData = Task.FromResult((IList<NodeMasterModel>)new List<NodeMasterModel>() { new NodeMasterModel { ApplicationId = 0 } });  // None Wells

            _allyNodeMasterStore.Setup(x => x.GetByAssetIdsForAllyTimeSeriesAsync(It.IsAny<List<Guid>>(), It.IsAny<string>()))
                .Returns(nodeData);

            _controller = new RealTimeDataController(_loggerFactory.Object, _realTimeDataProcessingService, _config.Object);

            // Act
            var result = _controller.GetAssetDowntimeAndShutdownData(request).Result;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(404, ((NotFoundObjectResult)result).StatusCode);
        }

    }
}
