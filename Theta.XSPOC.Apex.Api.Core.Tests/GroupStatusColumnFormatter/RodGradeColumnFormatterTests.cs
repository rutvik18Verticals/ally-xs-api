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
    public class RodGradeColumnFormatterTests
    {

        private RodGradeColumnFormatter _formatter;
        private Mock<IRod> _mockRodStore;

        [TestInitialize]
        public void TestInitialize()
        {
            _mockRodStore = new Mock<IRod>();
            _formatter = new RodGradeColumnFormatter(_mockRodStore.Object);
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
                    "Well", "TestNodeId"
                }
            };
            var column = new RowColumnModel();
            _mockRodStore.Setup(m => m.GetRodForGroupStatus(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(new List<RodForGroupStatusModel>
            {
                new RodForGroupStatusModel()
                {
                    NodeId = "TestNodeId",
                    Name = "TestName1",
                    RodNum = 1,
                },
                new RodForGroupStatusModel()
                {
                    NodeId = "TestNodeId",
                    Name = "TestName2",
                    RodNum = 2,
                }
            });

            _formatter.CalculateValue(dr, column, string.Empty);

            Assert.AreEqual("TestName1,TestName2", column.Value);
        }

        [TestMethod]
        public void PerformFormatTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "Well", "Well1"
                }
            };
            var column = new RowColumnModel();
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable());
            _mockRodStore.Setup(m => m.GetRodForGroupStatus(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(new List<RodForGroupStatusModel>
            {
                new RodForGroupStatusModel()
            });

            _formatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.IsNull(column.BackColor);
            Assert.IsNull(column.ForeColor);
        }

    }
}
