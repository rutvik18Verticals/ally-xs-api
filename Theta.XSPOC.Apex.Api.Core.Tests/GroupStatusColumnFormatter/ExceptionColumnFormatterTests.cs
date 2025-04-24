using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Core.Tests.GroupStatusColumnFormatter
{
    [TestClass]
    public class ExceptionColumnFormatterTests
    {

        private Mock<IException> _exceptionStore;
        private ExceptionColumnFormatter _formatter;

        [TestInitialize]
        public void TestInitialize()
        {
            _exceptionStore = new Mock<IException>();
            SetupException();

            _formatter = new ExceptionColumnFormatter(_exceptionStore.Object);
        }

        [TestMethod]
        public void CalculateValueNullDataRowTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                _formatter.CalculateValue(null, new RowColumnModel(), string.Empty));
        }

        [TestMethod]
        public void CalculateValueNullColumnTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                _formatter.CalculateValue(new Dictionary<string, object>(), null, string.Empty));
        }

        [TestMethod]
        public void CalculateValueTest()
        {
            // Arrange
            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "TestNodeId"
                }
            };
            var column = new RowColumnModel();

            _exceptionStore.Setup(x => x.GetExceptions(It.IsAny<IList<string>>(), It.IsAny<string>()))
                .Returns(new List<ExceptionModel>()
                {
                    new ExceptionModel()
                    {
                        NodeId = "TestNodeId",
                        ExceptionGroupName = "TestExceptionGroupName",
                        Priority = 1,
                    }
                });

            // Act
            _formatter.CalculateValue(dataRow, column, string.Empty);

            // Assert
            Assert.AreEqual("TestExceptionGroupName", column.Value);
        }

        [TestMethod]
        public void CalculateValueLengthLessThanEqual2EnabledTest()
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

            _exceptionStore.Setup(x => x.GetExceptions(It.IsAny<IList<string>>(), It.IsAny<string>()))
                .Returns(new List<ExceptionModel>()
                {
                    new ExceptionModel()
                    {
                        NodeId = "TestNodeId",
                        ExceptionGroupName = "T",
                        Priority = 1,
                    }
                });

            // Act
            _formatter.CalculateValue(dataRow, column, string.Empty);

            // Assert
            Assert.AreEqual("-", column.Value);
        }

        [TestMethod]
        public void CalculateValueLengthLessThanEqual2Test()
        {
            // Arrange
            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "TestNodeId"
                }
            };
            var column = new RowColumnModel();

            _exceptionStore.Setup(x => x.GetExceptions(It.IsAny<IList<string>>(), It.IsAny<string>()))
                .Returns(new List<ExceptionModel>()
                {
                    new ExceptionModel()
                    {
                        NodeId = "TestNodeId",
                        ExceptionGroupName = "T",
                        Priority = 1,
                    }
                });

            // Act
            _formatter.CalculateValue(dataRow, column, string.Empty);

            // Assert
            Assert.AreEqual(string.Empty, column.Value);
        }

        [TestMethod]
        public void PerformFormatNullDataRowTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                _formatter.PerformFormat(null, new RowColumnModel(),
                    new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty));
        }

        [TestMethod]
        public void PerformFormatNullColumnTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                _formatter.PerformFormat(new Dictionary<string, object>(), null,
                    new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty));
        }

        [TestMethod]
        public void PerformFormatTest()
        {
            // Arrange
            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "Well1"
                }
            };
            var column = new RowColumnModel();
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable());

            // Act
            _formatter.PerformFormat(dataRow, column, groupStatusColumn, string.Empty);

            // Assert
            Assert.AreEqual(null, column.BackColor);
            Assert.AreEqual(null, column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatPriorityGreater100Test()
        {
            // Arrange
            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "Well1"
                }
            };
            var column = new RowColumnModel();
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable());

            _exceptionStore.Setup(x => x.GetExceptions(It.IsAny<IList<string>>(), It.IsAny<string>()))
                .Returns(new List<ExceptionModel>()
                {
                    new ExceptionModel()
                    {
                        NodeId = "TestNodeId",
                        ExceptionGroupName = "TestExceptionGroupName",
                        Priority = 101,
                    }
                });

            // Act
            _formatter.PerformFormat(dataRow, column, groupStatusColumn, string.Empty);

            // Assert
            Assert.AreEqual("#FF0000", column.BackColor);
            Assert.AreEqual("#FFFFFF", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatPriorityLessThanEqual100Test()
        {
            // Arrange
            var dataRow = new Dictionary<string, object>
            {
                {
                    "Well", "Well1"
                }
            };
            var column = new RowColumnModel();
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable());
            var correlationId = Guid.NewGuid().ToString();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            _exceptionStore.Setup(x => x.GetExceptions(It.IsAny<IList<string>>(), correlationId))
                .Returns(new List<ExceptionModel>()
                {
                    new ExceptionModel()
                    {
                        NodeId = "TestNodeId",
                        ExceptionGroupName = "TestExceptionGroupName",
                        Priority = 100,
                    }
                });

            // Act
            _formatter.PerformFormat(dataRow, column, groupStatusColumn, correlationId);

            // Assert
            Assert.AreEqual("#FFFF00", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        private void SetupException()
        {
            _exceptionStore.Setup(x => x.GetExceptions(It.IsAny<IList<string>>(), It.IsAny<string>()))
                .Returns(new List<ExceptionModel>());
        }

    }
}
