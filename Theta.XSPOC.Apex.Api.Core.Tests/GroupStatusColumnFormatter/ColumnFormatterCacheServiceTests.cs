using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Tests.GroupStatusColumnFormatter
{
    [TestClass]
    public class ColumnFormatterCacheServiceTests
    {

        private ColumnFormatterCacheService _service;
        private Mock<IRod> _mockRodStore;
        private Mock<IPumpingUnit> _mockPumpingUnitStore;
        private Mock<IPumpingUnitManufacturer> _mockPumpingUnitManufacturerStore;
        private Mock<IException> _mockExceptionStore;
        private Mock<IHostAlarm> _mockHostAlarmStore;
        private Mock<IPocType> _mockPocTypeStore;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRodStore = new Mock<IRod>();
            _mockPumpingUnitStore = new Mock<IPumpingUnit>();
            _mockPumpingUnitManufacturerStore = new Mock<IPumpingUnitManufacturer>();
            _mockExceptionStore = new Mock<IException>();
            _mockHostAlarmStore = new Mock<IHostAlarm>();
            _mockPocTypeStore = new Mock<IPocType>();

            _service = new ColumnFormatterCacheService(
                _mockRodStore.Object,
                _mockPumpingUnitStore.Object,
                _mockPumpingUnitManufacturerStore.Object,
                _mockExceptionStore.Object,
                _mockHostAlarmStore.Object,
                _mockPocTypeStore.Object);
        }

        [TestMethod]
        public void GetDataRodGradeTest()
        {
            // Arrange
            var name = "ROD GRADE";
            var nodeList = new List<string> { "Node1" };
            var expectedData = new List<RodForGroupStatusModel> { new RodForGroupStatusModel() };
            _mockRodStore.Setup(m => m.GetRodForGroupStatus(nodeList, It.IsAny<string>())).Returns(expectedData);

            // Act
            var result = _service.GetData(name, nodeList, string.Empty);

            // Assert
            Assert.AreEqual(expectedData, result);
        }

        [TestMethod]
        public void GetDataRodGradeUseCacheTest()
        {
            // Arrange
            var name = "ROD GRADE";
            var nodeList = new List<string> { "Node1" };
            var expectedData = new List<RodForGroupStatusModel> { new RodForGroupStatusModel() };
            _mockRodStore.Setup(m => m.GetRodForGroupStatus(nodeList, It.IsAny<string>())).Returns(expectedData);

            // Act
            var result = _service.GetData(name, nodeList, string.Empty);

            // Assert
            Assert.AreEqual(expectedData, result);

            result = _service.GetData(name, nodeList, string.Empty);

            Assert.AreEqual(expectedData, result);
        }

        [TestMethod]
        public void GetDataPumpingUnitManufacturerTest()
        {
            var name = "PUMPING UNIT MANUFACTURER";
            var nodeList = new List<string> { "Node1" };
            var expectedData = new List<PumpingUnitManufacturerForGroupStatus> { new PumpingUnitManufacturerForGroupStatus() };
            _mockPumpingUnitManufacturerStore.Setup(m => m.GetManufacturers(nodeList, It.IsAny<string>())).Returns(expectedData);

            var result = _service.GetData(name, nodeList, string.Empty);

            Assert.AreEqual(expectedData, result);
        }

        [TestMethod]
        public void GetDataPumpingUnitTest()
        {
            var name = "PUMPING UNIT";
            var nodeList = new List<string> { "Node1" };
            var expectedData = new List<PumpingUnitForUnitNameModel> { new PumpingUnitForUnitNameModel() };
            _mockPumpingUnitStore.Setup(m => m.GetUnitNames(nodeList, It.IsAny<string>())).Returns(expectedData);

            var result = _service.GetData(name, nodeList, string.Empty);

            Assert.AreEqual(expectedData, result);
        }

        [TestMethod]
        public void GetDataExceptionGroupNameTest()
        {
            var name = "EXCEPTIONGROUPNAME";
            var nodeList = new List<string> { "Node1" };
            var correlationId = Guid.NewGuid().ToString();
            var expectedData = new List<ExceptionModel> { new ExceptionModel() };
            _mockExceptionStore.Setup(m => m.GetExceptions(nodeList, correlationId)).Returns(expectedData);

            var result = _service.GetData(name, nodeList, correlationId);

            Assert.AreEqual(expectedData, result);
        }

        [TestMethod]
        public void GetDataHostAlarmsTest()
        {
            var name = "HOSTALARMS";
            var nodeList = new List<string> { "Node1" };
            var expectedData = new List<HostAlarmForGroupStatus> { new HostAlarmForGroupStatus() };
            _mockHostAlarmStore.Setup(m => m.GetAllGroupStatusHostAlarms(nodeList, It.IsAny<string>())).Returns(expectedData);

            var result = _service.GetData(name, nodeList, string.Empty);

            Assert.AreEqual(expectedData, result);
        }

        [TestMethod]
        public void GetDataControllerTest()
        {
            var name = "CONTROLLER";
            var nodeList = new List<string> { "Node1" };
            var expectedData = new List<PocTypeModel> { new PocTypeModel() };
            _mockPocTypeStore.Setup(m => m.GetAll(string.Empty)).Returns(expectedData);

            var result = _service.GetData(name, nodeList, string.Empty);

            Assert.AreEqual(expectedData, result);
        }

        [TestMethod]
        public void GetDataPocTypeTest()
        {
            var name = "POCTYPE";
            var nodeList = new List<string> { "Node1" };
            var expectedData = new List<PocTypeModel> { new PocTypeModel() };
            _mockPocTypeStore.Setup(m => m.GetAll(string.Empty)).Returns(expectedData);

            var result = _service.GetData(name, nodeList, string.Empty);

            Assert.AreEqual(expectedData, result);
        }

        [TestMethod]
        public void GetDataPocTypeWithSpaceTest()
        {
            var name = "POC TYPE";
            var nodeList = new List<string> { "Node1" };
            var expectedData = new List<PocTypeModel> { new PocTypeModel() };
            _mockPocTypeStore.Setup(m => m.GetAll(string.Empty)).Returns(expectedData);

            var result = _service.GetData(name, nodeList, string.Empty);

            Assert.AreEqual(expectedData, result);
        }

        [TestMethod]
        public void GetDataNullTest()
        {
            var name = "123";
            var nodeList = new List<string> { "Node1" };
            var expectedData = new List<HostAlarmForGroupStatus> { new HostAlarmForGroupStatus() };
            _mockHostAlarmStore.Setup(m => m.GetAllGroupStatusHostAlarms(nodeList, It.IsAny<string>())).Returns(expectedData);

            var result = _service.GetData(name, nodeList, string.Empty);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetDataMissingNameTest()
        {
            var name = "";
            var nodeList = new List<string> { "Node1" };

            var result = _service.GetData(name, nodeList, string.Empty);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetDataMissingNodeIdsTest()
        {
            var name = "123";

            var result = _service.GetData(name, null, string.Empty);

            Assert.IsNull(result);
        }

    }
}
