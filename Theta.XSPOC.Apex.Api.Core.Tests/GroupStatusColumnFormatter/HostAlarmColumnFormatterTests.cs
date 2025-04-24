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
    public class HostAlarmColumnFormatterTests
    {

        private Mock<IHostAlarm> _hostAlarmStore;
        private HostAlarmColumnFormatter _formatter;

        [TestInitialize]
        public void TestInitialize()
        {
            _hostAlarmStore = new Mock<IHostAlarm>();
            SetupIHostAlarm();

            _formatter = new HostAlarmColumnFormatter(_hostAlarmStore.Object);
        }

        [TestMethod]
        public void CalculateValueNullDataRowTest()
        {
            // Arrange
            var column = new RowColumnModel();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => _formatter.CalculateValue(null, column, string.Empty));
        }

        [TestMethod]
        public void CalculateValueNullRowColumnModelTest()
        {
            // Arrange
            var dr = new Dictionary<string, object>();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => _formatter.CalculateValue(dr, null, string.Empty));
        }

        [TestMethod]
        public void CalculateValueTest()
        {
            // Arrange
            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "TestNodeId"
                },
                {
                    "Enabled", "True"
                },
            };
            var column = new RowColumnModel();

            _hostAlarmStore.Setup(x => x.GetAllGroupStatusHostAlarms(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(
                new List<HostAlarmForGroupStatus>
                {
                    new HostAlarmForGroupStatus
                    {
                        NodeId = "TestNodeId",
                        AlarmDescription = "TestAlarmDescription"
                    }
                });

            // Act
            _formatter.CalculateValue(dataRow, column, string.Empty);

            // Assert
            Assert.IsNotNull(column.Value);
        }

        [TestMethod]
        public void CalculateValueValueEmptyEnabledTrueTest()
        {
            // Arrange
            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "TestNodeId"
                },
                {
                    "Enabled", "True"
                },
            };
            var column = new RowColumnModel();

            _hostAlarmStore.Setup(x => x.GetAllGroupStatusHostAlarms(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(
                new List<HostAlarmForGroupStatus>
                {
                    new HostAlarmForGroupStatus
                    {
                        NodeId = "TestNodeId",
                        AlarmDescription = string.Empty,
                    }
                });

            // Act
            _formatter.CalculateValue(dataRow, column, string.Empty);

            // Assert
            Assert.AreEqual("OK", column.Value);
        }

        [TestMethod]
        public void CalculateValueAlarmState1Test()
        {
            // Arrange
            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "TestNodeId"
                },
                {
                    "Enabled", "True"
                },
            };
            var column = new RowColumnModel();

            _hostAlarmStore.Setup(x => x.GetAllGroupStatusHostAlarms(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(
                new List<HostAlarmForGroupStatus>
                {
                    new HostAlarmForGroupStatus
                    {
                        NodeId = "TestNodeId",
                        AlarmDescription = "TestAlarmDescription",
                        AlarmState = 1
                    }
                });

            // Act
            _formatter.CalculateValue(dataRow, column, string.Empty);

            // Assert
            Assert.AreEqual("TestAlarmDescription-Hi", column.Value);
        }

        [TestMethod]
        public void CalculateValueAlarmState2Test()
        {
            // Arrange
            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "TestNodeId"
                },
                {
                    "Enabled", "True"
                },
            };
            var column = new RowColumnModel();

            _hostAlarmStore.Setup(x => x.GetAllGroupStatusHostAlarms(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(
                new List<HostAlarmForGroupStatus>
                {
                    new HostAlarmForGroupStatus
                    {
                        NodeId = "TestNodeId",
                        AlarmDescription = "TestAlarmDescription",
                        AlarmState = 2
                    }
                });

            // Act
            _formatter.CalculateValue(dataRow, column, string.Empty);

            // Assert
            Assert.AreEqual("TestAlarmDescription-HiHi", column.Value);
        }

        [TestMethod]
        public void CalculateValueAlarmState3Test()
        {
            // Arrange
            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "TestNodeId"
                },
                {
                    "Enabled", "True"
                },
            };
            var column = new RowColumnModel();

            _hostAlarmStore.Setup(x => x.GetAllGroupStatusHostAlarms(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(
                new List<HostAlarmForGroupStatus>
                {
                    new HostAlarmForGroupStatus
                    {
                        NodeId = "TestNodeId",
                        AlarmDescription = "TestAlarmDescription",
                        AlarmState = 3
                    }
                });

            // Act
            _formatter.CalculateValue(dataRow, column, string.Empty);

            // Assert
            Assert.AreEqual("TestAlarmDescription-Lo", column.Value);
        }

        [TestMethod]
        public void CalculateValueAlarmState4Test()
        {
            // Arrange
            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "TestNodeId"
                },
                {
                    "Enabled", "True"
                },
            };
            var column = new RowColumnModel();

            _hostAlarmStore.Setup(x => x.GetAllGroupStatusHostAlarms(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(
                new List<HostAlarmForGroupStatus>
                {
                    new HostAlarmForGroupStatus
                    {
                        NodeId = "TestNodeId",
                        AlarmDescription = "TestAlarmDescription",
                        AlarmState = 4
                    }
                });

            // Act
            _formatter.CalculateValue(dataRow, column, string.Empty);

            // Assert
            Assert.AreEqual("TestAlarmDescription-LoLo", column.Value);
        }

        [TestMethod]
        public void CalculateValueAlarmState5AlarmType4Test()
        {
            // Arrange
            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "TestNodeId"
                },
                {
                    "Enabled", "True"
                },
            };
            var column = new RowColumnModel();

            _hostAlarmStore.Setup(x => x.GetAllGroupStatusHostAlarms(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(
                new List<HostAlarmForGroupStatus>
                {
                    new HostAlarmForGroupStatus
                    {
                        NodeId = "TestNodeId",
                        AlarmDescription = "TestAlarmDescription",
                        AlarmState = 5,
                        AlarmType = 4,
                        PercentChange = 50,
                    }
                });

            // Act
            _formatter.CalculateValue(dataRow, column, string.Empty);

            // Assert
            Assert.AreEqual("TestAlarmDescription-ROC: +50%", column.Value);
        }

        [TestMethod]
        public void CalculateValueAlarmState5AlarmType5Test()
        {
            // Arrange
            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "TestNodeId"
                },
                {
                    "Enabled", "True"
                },
            };
            var column = new RowColumnModel();

            _hostAlarmStore.Setup(x => x.GetAllGroupStatusHostAlarms(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(
                new List<HostAlarmForGroupStatus>
                {
                    new HostAlarmForGroupStatus
                    {
                        NodeId = "TestNodeId",
                        AlarmDescription = "TestAlarmDescription",
                        AlarmState = 5,
                        AlarmType = 5,
                        PercentChange = 50,
                    }
                });

            // Act
            _formatter.CalculateValue(dataRow, column, string.Empty);

            // Assert
            Assert.AreEqual("TestAlarmDescription-ROC: -50%", column.Value);
        }

        [TestMethod]
        public void CalculateValueAlarmState6Test()
        {
            // Arrange
            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "TestNodeId"
                },
                {
                    "Enabled", "True"
                },
            };
            var column = new RowColumnModel();

            _hostAlarmStore.Setup(x => x.GetAllGroupStatusHostAlarms(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(
                new List<HostAlarmForGroupStatus>
                {
                    new HostAlarmForGroupStatus
                    {
                        NodeId = "TestNodeId",
                        AlarmDescription = "TestAlarmDescription",
                        AlarmState = 6,
                        MinToMaxLimit = 25,
                    }
                });

            // Act
            _formatter.CalculateValue(dataRow, column, string.Empty);

            // Assert
            Assert.AreEqual("TestAlarmDescription-MaxSpan: 25", column.Value);
        }

        [TestMethod]
        public void CalculateValueAlarmState7Test()
        {
            // Arrange
            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "TestNodeId"
                },
                {
                    "Enabled", "True"
                },
            };
            var column = new RowColumnModel();

            _hostAlarmStore.Setup(x => x.GetAllGroupStatusHostAlarms(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(
                new List<HostAlarmForGroupStatus>
                {
                    new HostAlarmForGroupStatus
                    {
                        NodeId = "TestNodeId",
                        AlarmDescription = "TestAlarmDescription",
                        AlarmState = 7,
                    }
                });

            // Act
            _formatter.CalculateValue(dataRow, column, string.Empty);

            // Assert
            Assert.AreEqual("TestAlarmDescription-NearPumpoff", column.Value);
        }

        [TestMethod]
        public void CalculateValueAlarmState8AlarmType9Test()
        {
            // Arrange
            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "TestNodeId"
                },
                {
                    "Enabled", "True"
                },
            };
            var column = new RowColumnModel();

            _hostAlarmStore.Setup(x => x.GetAllGroupStatusHostAlarms(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(
                new List<HostAlarmForGroupStatus>
                {
                    new HostAlarmForGroupStatus
                    {
                        NodeId = "TestNodeId",
                        AlarmDescription = "TestAlarmDescription",
                        AlarmState = 8,
                        AlarmType = 9,
                    }
                });

            // Act
            _formatter.CalculateValue(dataRow, column, string.Empty);

            // Assert
            Assert.AreEqual("TestAlarmDescription-Val Chg: -", column.Value);
        }

        [TestMethod]
        public void CalculateValueAlarmState8AlarmTypeNot9Test()
        {
            // Arrange
            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "TestNodeId"
                },
                {
                    "Enabled", "True"
                },
            };
            var column = new RowColumnModel();

            _hostAlarmStore.Setup(x => x.GetAllGroupStatusHostAlarms(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(
                new List<HostAlarmForGroupStatus>
                {
                    new HostAlarmForGroupStatus
                    {
                        NodeId = "TestNodeId",
                        AlarmDescription = "TestAlarmDescription",
                        AlarmState = 8,
                        AlarmType = 1,
                    }
                });

            // Act
            _formatter.CalculateValue(dataRow, column, string.Empty);

            // Assert
            Assert.AreEqual("TestAlarmDescription-Val Chg: +", column.Value);
        }

        [TestMethod]
        public void CalculateValueAlarmState10Test()
        {
            // Arrange
            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "TestNodeId"
                },
                {
                    "Enabled", "True"
                },
            };
            var column = new RowColumnModel();

            _hostAlarmStore.Setup(x => x.GetAllGroupStatusHostAlarms(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(
                new List<HostAlarmForGroupStatus>
                {
                    new HostAlarmForGroupStatus
                    {
                        NodeId = "TestNodeId",
                        AlarmDescription = "TestAlarmDescription",
                        AlarmState = 10,
                        MinToMaxLimit = 11,
                    }
                });

            // Act
            _formatter.CalculateValue(dataRow, column, string.Empty);

            // Assert
            Assert.AreEqual("TestAlarmDescription-MinSpan: 11", column.Value);
        }

        [TestMethod]
        public void CalculateValueAlarmState115Test()
        {
            // Arrange
            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "TestNodeId"
                },
                {
                    "Enabled", "True"
                },
            };
            var column = new RowColumnModel();

            _hostAlarmStore.Setup(x => x.GetAllGroupStatusHostAlarms(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(
                new List<HostAlarmForGroupStatus>
                {
                    new HostAlarmForGroupStatus
                    {
                        NodeId = "TestNodeId",
                        AlarmDescription = "TestAlarmDescription",
                        AlarmState = 115,
                    }
                });

            // Act
            _formatter.CalculateValue(dataRow, column, string.Empty);

            // Assert
            Assert.AreEqual("TestAlarmDescription (Acknowledged)", column.Value);
        }

        [TestMethod]
        public void CalculateValueAlarmState1000Test()
        {
            // Arrange
            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "TestNodeId"
                },
                {
                    "Enabled", "True"
                },
            };
            var column = new RowColumnModel();

            _hostAlarmStore.Setup(x => x.GetAllGroupStatusHostAlarms(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(
                new List<HostAlarmForGroupStatus>
                {
                    new HostAlarmForGroupStatus
                    {
                        NodeId = "TestNodeId",
                        AlarmDescription = "TestAlarmDescription",
                        AlarmState = 1000,
                    }
                });

            // Act
            _formatter.CalculateValue(dataRow, column, string.Empty);

            // Assert
            Assert.AreEqual("TestAlarmDescription", column.Value);
        }

        [TestMethod]
        public void PerformFormatNullDataRowTest()
        {
            // Arrange
            var column = new RowColumnModel();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
                _formatter.PerformFormat(null, column, new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty));
        }

        [TestMethod]
        public void PerformFormatNullRowColumnModelTest()
        {
            // Arrange
            var dr = new Dictionary<string, object>();

            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() =>
                _formatter.PerformFormat(dr, null, new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty));
        }

        [TestMethod]
        public void PerformFormatAlarmState1Test()
        {
            // Arrange
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable());

            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "TestNodeId"
                },
                {
                    "Enabled", "True"
                },
            };
            var column = new RowColumnModel();

            _hostAlarmStore.Setup(x => x.GetAllGroupStatusHostAlarms(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(
                new List<HostAlarmForGroupStatus>
                {
                    new HostAlarmForGroupStatus
                    {
                        NodeId = "TestNodeId",
                        AlarmDescription = "TestAlarmDescription",
                        AlarmState = 1,
                    }
                });

            // Act
            _formatter.PerformFormat(dataRow, column, groupStatusColumn, string.Empty);

            // Assert
            Assert.AreEqual("#FFC0CB", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatAlarmStateNullTest()
        {
            // Arrange
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable());

            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "TestNodeId"
                },
                {
                    "Enabled", "True"
                },
            };
            var column = new RowColumnModel();

            _hostAlarmStore.Setup(x => x.GetAllGroupStatusHostAlarms(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(
                new List<HostAlarmForGroupStatus>
                {
                    new HostAlarmForGroupStatus
                    {
                        NodeId = "TestNodeId",
                        AlarmDescription = "TestAlarmDescription",
                        AlarmState = null,
                    }
                });

            // Act
            _formatter.PerformFormat(dataRow, column, groupStatusColumn, string.Empty);

            // Assert
            Assert.IsNull(column.BackColor);
            Assert.IsNull(column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatAlarmState0Test()
        {
            // Arrange
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable());

            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "TestNodeId"
                },
                {
                    "Enabled", "True"
                },
            };
            var column = new RowColumnModel();

            _hostAlarmStore.Setup(x => x.GetAllGroupStatusHostAlarms(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(
                new List<HostAlarmForGroupStatus>
                {
                    new HostAlarmForGroupStatus
                    {
                        NodeId = "TestNodeId",
                        AlarmDescription = "TestAlarmDescription",
                        AlarmState = 0,
                    }
                });

            // Act
            _formatter.PerformFormat(dataRow, column, groupStatusColumn, string.Empty);

            // Assert
            Assert.AreEqual(null, null);
        }

        [TestMethod]
        public void PerformFormatAlarmState50Test()
        {
            // Arrange
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable());

            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "TestNodeId"
                },
                {
                    "Enabled", "True"
                },
            };
            var column = new RowColumnModel();

            _hostAlarmStore.Setup(x => x.GetAllGroupStatusHostAlarms(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(
                new List<HostAlarmForGroupStatus>
                {
                    new HostAlarmForGroupStatus
                    {
                        NodeId = "TestNodeId",
                        AlarmDescription = "TestAlarmDescription",
                        AlarmState = 50,
                    }
                });

            // Act
            _formatter.PerformFormat(dataRow, column, groupStatusColumn, string.Empty);

            // Assert
            Assert.AreEqual("#FF0000", column.BackColor);
            Assert.AreEqual("#FFFFFF", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatAlarmState150Test()
        {
            // Arrange
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable());

            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "TestNodeId"
                },
                {
                    "Enabled", "True"
                },
            };
            var column = new RowColumnModel();

            _hostAlarmStore.Setup(x => x.GetAllGroupStatusHostAlarms(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(
                new List<HostAlarmForGroupStatus>
                {
                    new HostAlarmForGroupStatus
                    {
                        NodeId = "TestNodeId",
                        AlarmDescription = "TestAlarmDescription",
                        AlarmState = 150,
                    }
                });

            // Act
            _formatter.PerformFormat(dataRow, column, groupStatusColumn, string.Empty);

            // Assert
            Assert.AreEqual("#FFFF00", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatAlarmState250Test()
        {
            // Arrange
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable());

            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "TestNodeId"
                },
                {
                    "Enabled", "True"
                },
            };
            var column = new RowColumnModel();

            _hostAlarmStore.Setup(x => x.GetAllGroupStatusHostAlarms(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(
                new List<HostAlarmForGroupStatus>
                {
                    new HostAlarmForGroupStatus
                    {
                        NodeId = "TestNodeId",
                        AlarmDescription = "TestAlarmDescription",
                        AlarmState = 250,
                    }
                });

            // Act
            _formatter.PerformFormat(dataRow, column, groupStatusColumn, string.Empty);

            // Assert
            Assert.AreEqual("#00FFFF", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatAlarmState350Test()
        {
            // Arrange
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable());

            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "TestNodeId"
                },
                {
                    "Enabled", "True"
                },
            };
            var column = new RowColumnModel();

            _hostAlarmStore.Setup(x => x.GetAllGroupStatusHostAlarms(It.IsAny<IList<string>>(), It.IsAny<string>())).Returns(
                new List<HostAlarmForGroupStatus>
                {
                    new HostAlarmForGroupStatus
                    {
                        NodeId = "TestNodeId",
                        AlarmDescription = "TestAlarmDescription",
                        AlarmState = 350,
                    }
                });

            // Act
            _formatter.PerformFormat(dataRow, column, groupStatusColumn, string.Empty);

            // Assert
            Assert.AreEqual("#FF0000", column.BackColor);
            Assert.AreEqual("#FFFFFF", column.ForeColor);
        }

        #region Private Methods

        private void SetupIHostAlarm()
        {
            _hostAlarmStore.Setup(x => x.GetLimitsForDataHistory(It.IsAny<Guid>(), It.IsAny<int[]>(), It.IsAny<string>()))
                .Returns(new List<HostAlarmForDataHistoryModel>());
            _hostAlarmStore.Setup(x => x.GetAllGroupStatusHostAlarms(It.IsAny<IList<string>>(), It.IsAny<string>()))
                .Returns(new List<HostAlarmForGroupStatus>());
        }

        #endregion

    }
}
