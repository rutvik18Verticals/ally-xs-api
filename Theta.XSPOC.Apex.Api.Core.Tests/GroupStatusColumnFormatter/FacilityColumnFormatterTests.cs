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
    public class FacilityColumnFormatterTests
    {

        #region Private Fields

        private FacilityColumnFormatter _formatter;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _formatter = new FacilityColumnFormatter();
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
                    "tblFacilityTags.FacTag", "tblFacilityTags.FacTag"
                },
                {"FacTag.BackColor", "" },
                {"FacTag.ForeColor", "" },
                {"FacTag.Value", "1" }
            };
            var column = new RowColumnModel();
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "tblFacilityTags.FacTag",
            };

            _formatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual(null, column.BackColor);
            Assert.AreEqual(null, column.ForeColor);
        }
    }
}
