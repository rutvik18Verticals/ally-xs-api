using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter;
using Theta.XSPOC.Apex.Api.Data.Models;

namespace Theta.XSPOC.Apex.Api.Core.Tests.GroupStatusColumnFormatter
{
    [TestClass]
    public class ConditionalColumnFormatterTests
    {

        private ConditionalColumnFormatter _conditionalColumnFormatter;

        [TestInitialize]
        public void TestInitialize()
        {
            _conditionalColumnFormatter = new ConditionalColumnFormatter();
        }

        [TestMethod]
        public void PerformFormatNullDrTest()
        {
            var formatter = new ConditionalColumnFormatter();
            Assert.ThrowsException<ArgumentNullException>(() =>
                formatter.PerformFormat(null, new RowColumnModel(), new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty));
        }

        [TestMethod]
        public void PerformFormatNullColumnTest()
        {
            var formatter = new ConditionalColumnFormatter();
            Assert.ThrowsException<ArgumentNullException>(() =>
                formatter.PerformFormat(new Dictionary<string, object>(), null,
                    new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable()), string.Empty));
        }

        [TestMethod]
        public void PerformFormatNullGroupStatusColumnTest()
        {
            var formatter = new ConditionalColumnFormatter();
            Assert.ThrowsException<ArgumentNullException>(() =>
                formatter.PerformFormat(new Dictionary<string, object>(), new RowColumnModel(), null, string.Empty));
        }

        [TestMethod]
        public void PerformFormatComparisonOperatorConditionNotMetTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", "11"
                }
            };
            var column = new RowColumnModel();
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading",
                ConditionalFormats = new List<ConditionalFormat>
                {
                    new ConditionalFormat
                    {
                        Operator = ComparisonOperator.Equal,
                        Value = 10,
                        BackColor = 1,
                        ForeColor = 2,
                    }
                }
            };

            _conditionalColumnFormatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.IsNull(column.BackColor);
            Assert.IsNull(column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatComparisonOperatorEqualTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", "10"
                }
            };
            var column = new RowColumnModel
            {
                Value = "10",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading",
                ConditionalFormats = new List<ConditionalFormat>
                {
                    new ConditionalFormat
                    {
                        Operator = ComparisonOperator.Equal,
                        Value = 10,
                        BackColor = 1,
                        ForeColor = 2,
                    }
                }
            };

            _conditionalColumnFormatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#000001", column.BackColor);
            Assert.AreEqual("#000002", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatComparisonOperatorNotEqualTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", "11"
                }
            };
            var column = new RowColumnModel
            {
                Value = "11",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading",
                ConditionalFormats = new List<ConditionalFormat>
                {
                    new ConditionalFormat
                    {
                        Operator = ComparisonOperator.NotEqual,
                        Value = 10
                    }
                }
            };

            _conditionalColumnFormatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#000000", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatComparisonOperatorGreaterThanTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", "11"
                }
            };
            var column = new RowColumnModel
            {
                Value = "11",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading",
                ConditionalFormats = new List<ConditionalFormat>
                {
                    new ConditionalFormat
                    {
                        Operator = ComparisonOperator.GreaterThan,
                        Value = 10
                    }
                }
            };

            _conditionalColumnFormatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#000000", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatComparisonOperatorGreaterThanOrEqualTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", "10"
                }
            };
            var column = new RowColumnModel
            {
                Value = "10",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading",
                ConditionalFormats = new List<ConditionalFormat>
                {
                    new ConditionalFormat
                    {
                        Operator = ComparisonOperator.GreaterThanOrEqual,
                        Value = 10
                    }
                }
            };

            _conditionalColumnFormatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#000000", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatComparisonOperatorLessThanTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", "9"
                }
            };
            var column = new RowColumnModel
            {
                Value = "9",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading",
                ConditionalFormats = new List<ConditionalFormat>
                {
                    new ConditionalFormat
                    {
                        Operator = ComparisonOperator.LessThan,
                        Value = 10
                    }
                }
            };

            _conditionalColumnFormatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#000000", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatComparisonOperatorLessThanOrEqualTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", "10"
                }
            };
            var column = new RowColumnModel
            {
                Value = "10",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading",
                ConditionalFormats = new List<ConditionalFormat>
                {
                    new ConditionalFormat
                    {
                        Operator = ComparisonOperator.LessThanOrEqual,
                        Value = 10
                    }
                }
            };

            _conditionalColumnFormatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#000000", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatComparisonOperatorBetweenTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", "10"
                }
            };
            var column = new RowColumnModel
            {
                Value = "10",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading",
                ConditionalFormats = new List<ConditionalFormat>
                {
                    new ConditionalFormat
                    {
                        Operator = ComparisonOperator.Between,
                        MinValue = 9,
                        MaxValue = 11
                    }
                }
            };

            _conditionalColumnFormatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#000000", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatComparisonOperatorBetweenFalseTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", "4"
                }
            };
            var column = new RowColumnModel
            {
                Value = "0",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading",
                ConditionalFormats = new List<ConditionalFormat>
                {
                    new ConditionalFormat
                    {
                        Operator = ComparisonOperator.NotBetween,
                        MinValue = 9,
                        MaxValue = 11
                    }
                }
            };

            _conditionalColumnFormatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#000000", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatComparisonOperatorStringEqualTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", "5"
                }
            };
            var column = new RowColumnModel
            {
                Value = "5",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading",
                ConditionalFormats = new List<ConditionalFormat>
                {
                    new ConditionalFormat
                    {
                        Operator = ComparisonOperator.StringEqual,
                        StringValue = "5"
                    }
                }
            };

            _conditionalColumnFormatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#000000", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatComparisonOperatorStringContainsTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", "45"
                }
            };
            var column = new RowColumnModel
            {
                Value = "45",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading",
                ConditionalFormats = new List<ConditionalFormat>
                {
                    new ConditionalFormat
                    {
                        Operator = ComparisonOperator.Contains,
                        StringValue = "5"
                    }
                }
            };

            _conditionalColumnFormatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#000000", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatComparisonOperatorStringStartsWithTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", "54"
                }
            };
            var column = new RowColumnModel
            {
                Value = "5466",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading",
                ConditionalFormats = new List<ConditionalFormat>
                {
                    new ConditionalFormat
                    {
                        Operator = ComparisonOperator.StartsWith,
                        StringValue = "5"
                    }
                }
            };

            _conditionalColumnFormatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#000000", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatComparisonOperatorStringEndsWithTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", "45"
                }
            };
            var column = new RowColumnModel
            {
                Value = "5465",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading",
                ConditionalFormats = new List<ConditionalFormat>
                {
                    new ConditionalFormat
                    {
                        Operator = ComparisonOperator.EndsWith,
                        StringValue = "5"
                    }
                }
            };

            _conditionalColumnFormatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#000000", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatComparisonOperatorStringNotEqualTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", "4"
                }
            };
            var column = new RowColumnModel
            {
                Value = "4",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading",
                ConditionalFormats = new List<ConditionalFormat>
                {
                    new ConditionalFormat
                    {
                        Operator = ComparisonOperator.DoesNotEqual,
                        StringValue = "5"
                    }
                }
            };

            _conditionalColumnFormatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#000000", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatComparisonOperatorStringNotContainTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", "44"
                }
            };
            var column = new RowColumnModel
            {
                Value = "44",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading",
                ConditionalFormats = new List<ConditionalFormat>
                {
                    new ConditionalFormat
                    {
                        Operator = ComparisonOperator.DoesNotContain,
                        StringValue = "5"
                    }
                }
            };

            _conditionalColumnFormatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#000000", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatComparisonOperatorStringNotStartWithTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", "45"
                }
            };
            var column = new RowColumnModel
            {
                Value = "45",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading",
                ConditionalFormats = new List<ConditionalFormat>
                {
                    new ConditionalFormat
                    {
                        Operator = ComparisonOperator.DoesNotStartWith,
                        StringValue = "5"
                    }
                }
            };

            _conditionalColumnFormatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#000000", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatComparisonOperatorStringNotEndWithTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", "54"
                }
            };
            var column = new RowColumnModel
            {
                Value = "54",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading",
                ConditionalFormats = new List<ConditionalFormat>
                {
                    new ConditionalFormat
                    {
                        Operator = ComparisonOperator.DoesNotEndWith,
                        StringValue = "5"
                    }
                }
            };

            _conditionalColumnFormatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#000000", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatComparisonOperatorNotFloatStringEqualTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", "a"
                }
            };
            var column = new RowColumnModel
            {
                Value = "a",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading",
                ConditionalFormats = new List<ConditionalFormat>
                {
                    new ConditionalFormat
                    {
                        Operator = ComparisonOperator.StringEqual,
                        StringValue = "a"
                    }
                }
            };

            _conditionalColumnFormatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#000000", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatComparisonOperatorNotFloatStringContainsTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", "abc"
                }
            };
            var column = new RowColumnModel
            {
                Value = "abc",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading",
                ConditionalFormats = new List<ConditionalFormat>
                {
                    new ConditionalFormat
                    {
                        Operator = ComparisonOperator.Contains,
                        StringValue = "b"
                    }
                }
            };

            _conditionalColumnFormatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#000000", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatComparisonOperatorNotFloatStringStartsWithTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", "abc"
                }
            };
            var column = new RowColumnModel
            {
                Value = "abc",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading",
                ConditionalFormats = new List<ConditionalFormat>
                {
                    new ConditionalFormat
                    {
                        Operator = ComparisonOperator.StartsWith,
                        StringValue = "a"
                    }
                }
            };

            _conditionalColumnFormatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#000000", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatComparisonOperatorNotFloatStringEndsWithTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", "abc"
                }
            };
            var column = new RowColumnModel
            {
                Value = "abc",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading",
                ConditionalFormats = new List<ConditionalFormat>
                {
                    new ConditionalFormat
                    {
                        Operator = ComparisonOperator.EndsWith,
                        StringValue = "c"
                    }
                }
            };

            _conditionalColumnFormatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#000000", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatComparisonOperatorNotFloatStringNotEqualTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", "a"
                }
            };
            var column = new RowColumnModel
            {
                Value = "a",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading",
                ConditionalFormats = new List<ConditionalFormat>
                {
                    new ConditionalFormat
                    {
                        Operator = ComparisonOperator.DoesNotEqual,
                        StringValue = "b"
                    }
                }
            };

            _conditionalColumnFormatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#000000", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatComparisonOperatorNotFloatStringNotContainTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", "abc"
                }
            };
            var column = new RowColumnModel
            {
                Value = "abc",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading",
                ConditionalFormats = new List<ConditionalFormat>
                {
                    new ConditionalFormat
                    {
                        Operator = ComparisonOperator.DoesNotContain,
                        StringValue = "d"
                    }
                }
            };

            _conditionalColumnFormatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#000000", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatComparisonOperatorNotFloatStringNotStartWithTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", "ab"
                }
            };
            var column = new RowColumnModel
            {
                Value = "ab",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading",
                ConditionalFormats = new List<ConditionalFormat>
                {
                    new ConditionalFormat
                    {
                        Operator = ComparisonOperator.DoesNotStartWith,
                        StringValue = "b"
                    }
                }
            };

            _conditionalColumnFormatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#000000", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

        [TestMethod]
        public void PerformFormatComparisonOperatorNotFloatStringNotEndWithTest()
        {
            var dr = new Dictionary<string, object>
            {
                {
                    "TestFieldHeading", "cba"
                }
            };
            var column = new RowColumnModel
            {
                Value = "cba",
            };
            var groupStatusColumn = new GroupStatusColumns(new List<GroupStatusTableModel>(), new Hashtable())
            {
                FieldHeading = "TestFieldHeading",
                ConditionalFormats = new List<ConditionalFormat>
                {
                    new ConditionalFormat
                    {
                        Operator = ComparisonOperator.DoesNotEndWith,
                        StringValue = "c"
                    }
                }
            };

            _conditionalColumnFormatter.PerformFormat(dr, column, groupStatusColumn, string.Empty);

            Assert.AreEqual("#000000", column.BackColor);
            Assert.AreEqual("#000000", column.ForeColor);
        }

    }
}
