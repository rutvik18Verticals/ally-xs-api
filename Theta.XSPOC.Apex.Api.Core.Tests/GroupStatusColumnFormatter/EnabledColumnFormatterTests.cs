using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Tests.GroupStatusColumnFormatter
{
    [TestClass]
    public class EnabledColumnFormatterTests
    {

        #region Private Fields

        private EnabledColumnFormatter _formatter;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _formatter = new EnabledColumnFormatter();

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PerformFormatNullDataRowTest()
        {
            _formatter.PerformFormat(null, new RowColumnModel(), new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty);
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
            dr.Add("Enbld", " ");
            dr.Add("DisableCode", " ");
            dr.Add("Enabled", "True");

            var column = new RowColumnModel();
            _formatter.CalculateValue(dr, column, string.Empty, true);

            Assert.AreEqual("  ", column.Value);
        }

        [TestMethod]
        public void PerformFormatTest()
        {
            var dr = new Dictionary<string, object>();
            dr.Add("Enbld", " ");
            dr.Add("DisableCode", " ");
            dr.Add("Enabled", "True");

            var column = new RowColumnModel();
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable());

            _formatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#00FA9A", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

    }
}
