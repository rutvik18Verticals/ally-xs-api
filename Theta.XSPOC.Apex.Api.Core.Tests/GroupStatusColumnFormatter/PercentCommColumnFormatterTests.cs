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
    public class PercentCommColumnFormatterTests
    {

        private PercentCommColumnFormatter _percentCommColumnFormatter;

        [TestInitialize]
        public void TestInitialize()
        {
            _percentCommColumnFormatter = new PercentCommColumnFormatter();
        }

        [TestMethod]
        public void CalculateValueNullDataRowTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                _percentCommColumnFormatter.CalculateValue(null, new RowColumnModel(), string.Empty));
        }

        [TestMethod]
        public void CalculateValueNullColumnTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                _percentCommColumnFormatter.CalculateValue(new Dictionary<string, object>(), null, string.Empty));
        }

        [TestMethod]
        public void CalculateValueCommSuccessKeyNullTest()
        {
            var dataRow = new Dictionary<string, object>
            {
                {
                    "tblNodeMaster.CommSuccess", null
                }
            };
            var column = new RowColumnModel();
            _percentCommColumnFormatter.CalculateValue(dataRow, column, string.Empty);
            Assert.AreEqual(null, column.Value);
        }

        [TestMethod]
        public void CalculateValueCommAttemptKeyNullTest()
        {
            var dataRow = new Dictionary<string, object>
            {
                {
                    "tblNodeMaster.CommSuccess", 1
                },
                {
                    "tblNodeMaster.CommAttempt", null
                }
            };
            var column = new RowColumnModel();
            _percentCommColumnFormatter.CalculateValue(dataRow, column, string.Empty);
            Assert.AreEqual(string.Empty, column.Value);
        }

        [TestMethod]
        public void CalculateValueCommAttemptKeyZeroTest()
        {
            var dataRow = new Dictionary<string, object>
            {
                {
                    "tblNodeMaster.CommSuccess", 1
                },
                {
                    "tblNodeMaster.CommAttempt", 0
                }
            };
            var column = new RowColumnModel();
            _percentCommColumnFormatter.CalculateValue(dataRow, column, string.Empty, true);
            Assert.AreEqual("0", column.Value);
        }

        [TestMethod]
        public void CalculateValueCommSuccessKeyZeroTest()
        {
            var dataRow = new Dictionary<string, object>
            {
                {
                    "tblNodeMaster.CommSuccess", 0
                },
                {
                    "tblNodeMaster.CommAttempt", 1
                }
            };
            var column = new RowColumnModel();
            _percentCommColumnFormatter.CalculateValue(dataRow, column, string.Empty, true);
            Assert.AreEqual("0", column.Value);
        }

        [TestMethod]
        public void CalculateValueCalculatedValueIsNotNaNTest()
        {
            var dataRow = new Dictionary<string, object>
            {
                {
                    "tblNodeMaster.CommSuccess", 1
                },
                {
                    "tblNodeMaster.CommAttempt", 2
                }
            };
            var column = new RowColumnModel();
            _percentCommColumnFormatter.CalculateValue(dataRow, column, string.Empty, true);
            Assert.AreEqual("50", column.Value);
        }

        [TestMethod]
        public void PerformFormatNullDataRowTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                _percentCommColumnFormatter.PerformFormat(null, new RowColumnModel(),
                    new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty));
        }

        [TestMethod]
        public void PerformFormatCommSuccessKeyNullTest()
        {
            var dataRow = new Dictionary<string, object>
            {
                {
                    "tblNodeMaster.CommSuccess", null
                }
            };
            var column = new RowColumnModel();
            _percentCommColumnFormatter.PerformFormat(dataRow, column, new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty);
            Assert.AreEqual(null, column.Value);
        }

        [TestMethod]
        public void PerformFormatNullColumnTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                _percentCommColumnFormatter.PerformFormat(new Dictionary<string, object>(), null,
                    new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty));
        }

        [TestMethod]
        public void PerformFormatCalculatedValueIsLessThan75Test()
        {
            var dataRow = new Dictionary<string, object>
            {
                {
                    "Enabled", "True"
                },
                {
                    "tblNodeMaster.CommSuccess", 1
                },
                {
                    "tblNodeMaster.CommAttempt", 2
                }
            };
            var column = new RowColumnModel();
            _percentCommColumnFormatter.PerformFormat(dataRow, column, new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty);
            Assert.AreEqual("#FF0000", column.BackColor);
            Assert.AreEqual("#FFFFFF", column.ForeColor);
        }

    }
}
