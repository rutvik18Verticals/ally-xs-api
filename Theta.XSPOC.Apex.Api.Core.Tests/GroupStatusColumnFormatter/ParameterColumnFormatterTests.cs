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
    public class ParameterColumnFormatterTests
    {

        #region Private Fields

        private ParameterColumnFormatter _formatter;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _formatter = new ParameterColumnFormatter();
        }

        [TestMethod]
        public void PerformFormatNullDataRowParameterTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _formatter.PerformFormat(null, new RowColumnModel(),
                new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty));
        }

        [TestMethod]
        public void PerformFormatNullColumnParameterTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _formatter.PerformFormat(new Dictionary<string, object>(), null,
                new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty));
        }

        [TestMethod]
        public void PerformFormatNullGroupStatusColumnParameterTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => _formatter.PerformFormat(new Dictionary<string, object>(),
                new RowColumnModel(),
                null, string.Empty));
        }

        [TestMethod]
        public void PerformFormatTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", string.Empty
                },
                {
                    "TestFieldHeading.BackColor", "Red"
                },
                {
                    "TestFieldHeading.ForeColor", "White"
                },
            };
            var column = new RowColumnModel();
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading"
            };

            _formatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#FF0000", column.BackColor);
            Assert.AreEqual("#FFFFFF", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatDefaultColorTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", string.Empty
                },
                {
                    "TestFieldHeading.BackColor", string.Empty
                },
                {
                    "TestFieldHeading.ForeColor", string.Empty
                },
            };
            var column = new RowColumnModel();
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading"
            };

            _formatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.IsNull(column.BackColor);
            Assert.IsNull(column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatDescriptionTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "Description", string.Empty
                },
                {
                    "BackColor", "Red"
                },
                {
                    "ForeColor", "White"
                },
            };
            var column = new RowColumnModel();
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "Description"
            };

            _formatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#FFFF00", column.BackColor);
            Assert.AreEqual("#FFFFFF", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatDescriptionDefaultColorTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "Description", string.Empty
                },
                {
                    "BackColor", string.Empty
                },
                {
                    "ForeColor", string.Empty
                },
            };
            var column = new RowColumnModel();
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "Description"
            };

            _formatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.IsNull(column.BackColor);
            Assert.IsNull(column.ForeColor);
        }

    }
}
