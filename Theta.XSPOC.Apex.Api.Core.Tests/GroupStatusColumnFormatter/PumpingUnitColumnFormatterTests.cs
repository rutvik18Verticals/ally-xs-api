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
    public class PumpingUnitColumnFormatterTests
    {

        #region Private Fields

        private Mock<IPumpingUnit> _pumpingUnit;
        private PumpingUnitColumnFormatter _formatter;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _pumpingUnit = new Mock<IPumpingUnit>();
            SetupPumpingUnit();

            _formatter = new PumpingUnitColumnFormatter(_pumpingUnit.Object);
        }

        [TestMethod]
        public void ConstructorNullParameterTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new PumpingUnitColumnFormatter(null));
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
        public void CalculateValueTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "Well", "Well1"
                },
                {
                    "Pumping Unit", "Unit1"
                }
            };
            var column = new RowColumnModel();
            _pumpingUnit.Setup(p => p.GetUnitNames(It.IsAny<List<string>>(), It.IsAny<string>())).Returns(new List<PumpingUnitForUnitNameModel>
            {
                new PumpingUnitForUnitNameModel()
                {
                    UnitId = "Unit1",
                    APIDesignation = "API1"
                }
            });

            _formatter.CalculateValue(dr, column, string.Empty, true);

            Assert.AreEqual("API1", column.Value);
        }

        [TestMethod]
        public void PerformFormatNullColumnParameterTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _formatter.PerformFormat(new Dictionary<string, object>(), null,
                new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty));
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

        #region Private Methods

        private void SetupPumpingUnit()
        {
            _pumpingUnit.Setup(x => x.GetUnitNames(It.IsAny<IList<string>>(), It.IsAny<string>()))
                .Returns(new List<PumpingUnitForUnitNameModel>());
        }

        #endregion

    }
}
