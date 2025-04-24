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
    public class FacilityTagAlarmsColumnFormatterTests
    {

        #region Private Fields

        private FacilityTagAlarmsColumnFormatter _formatter;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _formatter = new FacilityTagAlarmsColumnFormatter();
        }

        [TestMethod]
        public void ConstructorNullParameterTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new PumpingUnitColumnFormatter(null));
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
            var column = new RowColumnModel()
            {
                Value = "1",
            };

            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable());

            _formatter.PerformFormat(null, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#FF0000", column.BackColor);
            Assert.AreEqual("#FFFFFF", column.ForeColor);
        }

    }
}
