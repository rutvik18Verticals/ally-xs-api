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
    public class PercentFillColumnFormatterTests
    {

        #region Private Fields

        private PercentFillColumnFormatter _formatter;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _formatter = new PercentFillColumnFormatter();
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
            var dr = new Dictionary<string, object>();
            dr.Add("Well", "Well1");
            dr.Add("tblNodeMaster.PumpFillage", "0");
            dr.Add("tblNodeMaster.WellOpType", "1");

            var column = new RowColumnModel();
            _formatter.CalculateValue(dr, column, string.Empty, true);

            Assert.AreEqual("0", column.Value);
        }

        [TestMethod]
        public void PerformFormatNullColumnParameterTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _formatter.PerformFormat(new Dictionary<string, object>(), null,
                new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty));
        }

    }
}
