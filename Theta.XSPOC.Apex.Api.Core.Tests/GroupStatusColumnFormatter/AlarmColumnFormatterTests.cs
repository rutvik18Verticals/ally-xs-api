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
    public class AlarmColumnFormatterTests
    {

        #region Private Fields

        private AlarmColumnFormatter _formatter;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _formatter = new AlarmColumnFormatter();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CalculateValueNullDataRowTest()
        {
            _formatter.CalculateValue(null, new RowColumnModel(), string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CalculateValueNullColumnTest()
        {
            _formatter.CalculateValue(new Dictionary<string, object>(), null, string.Empty);
        }

        [TestMethod]
        public void CalculateValueTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "tblNodeMaster.HighPriAlarm", "Alarm"
                }
            };
            var column = new RowColumnModel();

            _formatter.CalculateValue(dr, column, string.Empty);

            Assert.AreEqual("Alarm", column.Value);
        }

        [TestMethod]
        public void CalculateValueOKTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "tblNodeMaster.HighPriAlarm", "OK"
                },
                {
                    "Enabled", "True"
                }
            };
            var column = new RowColumnModel();

            _formatter.CalculateValue(dr, column, string.Empty);

            Assert.AreEqual("OK", column.Value);
        }

        [TestMethod]
        public void CalculateValueNullTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "tblNodeMaster.HighPriAlarm", null
                },
                {
                    "Enabled", "True"
                }
            };
            var column = new RowColumnModel();

            _formatter.CalculateValue(dr, column, string.Empty);

            Assert.AreEqual("OK", column.Value);
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
            var dr = new Dictionary<string, object>
            {
                {
                    "tblNodeMaster.HighPriAlarm", "Alarm"
                }
            };
            var column = new RowColumnModel();
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable());

            _formatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#FF0000", column.BackColor);
            Assert.AreEqual("#FFFFFF", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatOKTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "tblNodeMaster.HighPriAlarm", "OK"
                },
                {
                    "Enabled", "True"
                }
            };
            var column = new RowColumnModel();
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable());

            _formatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.IsNull(column.BackColor);
            Assert.IsNull(column.ForeColor);
        }

    }

}
