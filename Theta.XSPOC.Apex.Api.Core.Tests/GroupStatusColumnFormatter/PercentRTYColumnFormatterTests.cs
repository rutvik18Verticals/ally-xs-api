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
    public class PercentRTYColumnFormatterTests
    {
        #region Private Fields

        private PercentRTYColumnFormatter _formatter;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _formatter = new PercentRTYColumnFormatter();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PerformFormatNullDataRowTest()
        {
            _formatter.PerformFormat(null, new RowColumnModel(), new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PerformFormatNullColumnTest()
        {
            _formatter.PerformFormat(new Dictionary<string, object>(), null, new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty);
        }

        [TestMethod]
        public void PerformFormatTest()
        {
            var dr = new Dictionary<string, object>();
            dr.Add("tblWellStatistics.AlarmStateRuntime", "1");
            dr.Add("Enabled", "True");

            var column = new RowColumnModel();
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable());

            _formatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#FF0000", column.BackColor);
            Assert.AreEqual("#FFFFFF", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatOKTest()
        {
            var dr = new Dictionary<string, object>();
            dr.Add("tblWellStatistics.AlarmStateRuntime", "Alarm");

            var column = new RowColumnModel();
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable());

            _formatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.IsNull(column.BackColor);
            Assert.IsNull(column.ForeColor);
        }

    }

}
