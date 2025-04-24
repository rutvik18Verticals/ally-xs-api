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
    public class CommColumnFormatterTests
    {

        private CommColumnFormatter _commColumnFormatter;

        [TestInitialize]
        public void TestInitialize()
        {
            _commColumnFormatter = new CommColumnFormatter();
        }

        [TestMethod]
        public void CalculateValueNullDataRowTest()
        {
            // Arrange
            var column = new RowColumnModel();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => _commColumnFormatter.CalculateValue(null, column, string.Empty));
        }

        [TestMethod]
        public void CalculateValueNullRowColumnModelTest()
        {
            // Arrange
            var dr = new Dictionary<string, object>();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => _commColumnFormatter.CalculateValue(dr, null, string.Empty));
        }

        [TestMethod]
        public void CalculateValueEnabledKeyFalseTest()
        {
            // Arrange
            var dr = new Dictionary<string, object>
            {
                {
                    "Enabled", "False"
                },
                {
                    "tblNodeMaster.CommStatus", "Test"
                }
            };
            var column = new RowColumnModel();

            // Act
            _commColumnFormatter.CalculateValue(dr, column, string.Empty);

            // Assert
            Assert.AreEqual(string.Empty, column.Value);
        }

        [TestMethod]
        public void CalculateValueEnabledKeyTrueTest()
        {
            // Arrange
            var dr = new Dictionary<string, object>
            {
                {
                    "Enabled", "True"
                },
                {
                    "tblNodeMaster.CommStatus", "Test"
                }
            };
            var column = new RowColumnModel();

            // Act
            _commColumnFormatter.CalculateValue(dr, column, string.Empty);

            // Assert
            Assert.AreEqual("Test", column.Value);
        }

        [TestMethod]
        public void PerformFormatNullDataRowTest()
        {
            // Arrange
            var column = new RowColumnModel();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
                _commColumnFormatter.PerformFormat(null, column, new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty));
        }

        [TestMethod]
        public void PerformFormatNullRowColumnModelTest()
        {
            // Arrange
            var dr = new Dictionary<string, object>();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
                _commColumnFormatter.PerformFormat(dr, null, new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty));
        }

        [TestMethod]
        public void PerformFormatEmptyValueTest()
        {
            // Arrange
            var dr = new Dictionary<string, object>
            {
                {
                    "Comm", ""
                }
            };
            var column = new RowColumnModel();

            // Act
            _commColumnFormatter.PerformFormat(dr, column, new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty);

            // Assert
            Assert.AreEqual(null, null);
        }

        [TestMethod]
        public void PerformFormatOkValueTest()
        {
            // Arrange
            var dr = new Dictionary<string, object>
            {
                {
                    "Comm", "OK"
                }
            };
            var column = new RowColumnModel();

            // Act
            _commColumnFormatter.PerformFormat(dr, column, new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty);

            // Assert
            Assert.AreEqual(null, null);
        }

        [TestMethod]
        public void PerformFormatOtherValueTest()
        {
            // Arrange
            var dr = new Dictionary<string, object>
            {
                {
                    "Comm", "Not OK"
                }
            };
            var column = new RowColumnModel();

            // Act
            _commColumnFormatter.PerformFormat(dr, column, new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty);

            // Assert
            Assert.AreEqual("#FF0000", column.BackColor);
            Assert.AreEqual("#FFFFFF", column.ForeColor);
        }

    }
}
