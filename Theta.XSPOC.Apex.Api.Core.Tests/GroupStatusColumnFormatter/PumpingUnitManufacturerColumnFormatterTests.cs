using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Tests.GroupStatusColumnFormatter
{
    [TestClass]
    public class PumpingUnitManufacturerColumnFormatterTests
    {

        private Mock<IPumpingUnitManufacturer> _pumpingUnitManufacturer;
        private PumpingUnitManufacturerColumnFormatter _formatter;

        [TestInitialize]
        public void TestInitialize()
        {
            _pumpingUnitManufacturer = new Mock<IPumpingUnitManufacturer>();
            _formatter = new PumpingUnitManufacturerColumnFormatter(_pumpingUnitManufacturer.Object);
        }

        [TestMethod]
        public void ConstructorNullParameterTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new PumpingUnitManufacturerColumnFormatter(null));
        }

        [TestMethod]
        public void CalculateValueNullDrParameterTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _formatter.CalculateValue(null, new RowColumnModel(), string.Empty, true));
        }

        [TestMethod]
        public void CalculateValueNullColumnParameterTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                _formatter.CalculateValue(new Dictionary<string, object>(), null, string.Empty, true));
        }

        [TestMethod]
        public void PerformFormatNullColumnParameterTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _formatter.PerformFormat(new Dictionary<string, object>(), null,
                new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty));
        }

        [TestMethod]
        public void CalculateValueTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "Well", "TestNodeId"
                },
                {
                    "Pumping Unit Manufacturer", "TestUnitId"
                }
            };
            var column = new RowColumnModel();
            var manufacturers = new List<PumpingUnitManufacturerForGroupStatus>
            {
                new PumpingUnitManufacturerForGroupStatus()
                {
                    UnitId = "TestUnitId",
                    Manufacturer = "TestManufacturer"
                }
            };
            _pumpingUnitManufacturer.Setup(p => p.GetManufacturers(It.IsAny<List<string>>(), It.IsAny<string>())).Returns(manufacturers);

            _formatter.CalculateValue(dr, column, string.Empty, true);

            Assert.AreEqual("TestManufacturer", column.Value);
        }

        [TestMethod]
        public void PerformFormatTest()
        {
            var column = new RowColumnModel();
            _formatter.PerformFormat(new Dictionary<string, object>(), column,
                new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty);

            Assert.AreEqual(null, column.BackColor);
            Assert.AreEqual(null, column.ForeColor);
        }

    }
}
