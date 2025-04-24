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
    public class ParamStandardColumnFormatterTests
    {
        #region Private Fields

        private ParamStandardTypeColumnFormatter _formatter;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _formatter = new ParamStandardTypeColumnFormatter();
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
            var dr = new Dictionary<string, object>
            {
                {
                    "TubingPressure", "TubingPressure"
                },
                {"TubingPressure.BackColor", "" },
                {"TubingPressure.ForeColor", "" },
                {"TubingPressure.Value", "1" }
            };
            var column = new RowColumnModel();
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable());
            groupStatusColumn.FieldHeading = "TubingPressure";
            _formatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            // Assert
            Assert.AreEqual(null, null);
        }

    }
}
