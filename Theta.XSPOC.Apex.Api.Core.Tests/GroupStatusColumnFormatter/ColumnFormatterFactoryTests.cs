using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Common.GroupStatus.ColumnFormatter;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Services;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Kernel.Logging;

namespace Theta.XSPOC.Apex.Api.Core.Tests.GroupStatusColumnFormatter
{
    [TestClass]
    public class ColumnFormatterFactoryTests
    {

        #region Private Fields

        private Mock<IThetaLoggerFactory> _loggerFactory;
        private Mock<IThetaLogger> _logger;
        private ColumnFormatterFactory _columnFormatterFactory;
        private List<IColumnFormatter> _groupStatusColumnFormat;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _loggerFactory = new Mock<IThetaLoggerFactory>();
            _logger = new Mock<IThetaLogger>();
            _groupStatusColumnFormat = new List<IColumnFormatter>();

            SetupThetaLoggerFactory();
            SetupGroupStatusColumnFormat();

            _columnFormatterFactory = new ColumnFormatterFactory(_groupStatusColumnFormat);
        }

        [TestMethod]
        public void CreateColumnFormatterControllerTest()
        {
            // Arrange
            var sourceId = (int)GroupStatusColumns.SourceType.Common;
            var name = "CONTROLLER";

            // Act
            var result = _columnFormatterFactory.Create(sourceId, name, new List<ConditionalFormat>());

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateColumnFormatterCommTest()
        {
            // Arrange
            var sourceId = (int)GroupStatusColumns.SourceType.Common;
            var name = "COMM";

            // Act
            var result = _columnFormatterFactory.Create(sourceId, name, new List<ConditionalFormat>());

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateColumnPercentFillTest()
        {
            // Arrange
            var sourceId = (int)GroupStatusColumns.SourceType.Common;
            var name = "%FILL";

            // Act
            var result = _columnFormatterFactory.Create(sourceId, name, new List<ConditionalFormat>());

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateRunStatusColumnTest()
        {
            // Arrange
            var sourceId = (int)GroupStatusColumns.SourceType.Common;
            var name = "RUN STATUS";

            // Act
            var result = _columnFormatterFactory.Create(sourceId, name, new List<ConditionalFormat>());

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateFacilityTagAlarmsTest()
        {
            // Arrange
            var sourceId = (int)GroupStatusColumns.SourceType.Common;
            var name = "FACILITYTAGALARMS";

            // Act
            var result = _columnFormatterFactory.Create(sourceId, name, new List<ConditionalFormat>());

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateColumnPercentRTTest()
        {
            // Arrange
            var sourceId = (int)GroupStatusColumns.SourceType.Common;
            var name = "%RT";

            // Act
            var result = _columnFormatterFactory.Create(sourceId, name, new List<ConditionalFormat>());

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateFacilityColumnTest()
        {
            // Arrange
            var sourceId = (int)GroupStatusColumns.SourceType.Facility;
            var name = "FACILITY";

            // Act
            var result = _columnFormatterFactory.Create(sourceId, name, new List<ConditionalFormat>());

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateTISColumnTest()
        {
            // Arrange
            var sourceId = (int)GroupStatusColumns.SourceType.Common;
            var name = "TIS";

            // Act
            var result = _columnFormatterFactory.Create(sourceId, name, new List<ConditionalFormat>());

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateColumnFormatterNullTest()
        {
            // Arrange
            var sourceId = (int)GroupStatusColumns.SourceType.Common;
            var name = "UNKNOWN";

            // Act
            var result = _columnFormatterFactory.Create(sourceId, name, new List<ConditionalFormat>());

            // Assert
            Assert.IsNull(result);
        }

        public void CreateDRCCColumnFillTest()
        {
            // Arrange
            var sourceId = (int)GroupStatusColumns.SourceType.Common;
            var name = "DRC";
            // Act
            var result = _columnFormatterFactory.Create(sourceId, name, new List<ConditionalFormat>());
            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateColumnFormatterNullSourceIdTest()
        {
            // Arrange
            var sourceId = 1000;
            var name = "UNKNOWN";

            // Act
            var result = _columnFormatterFactory.Create(sourceId, name, new List<ConditionalFormat>());

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void CreateColumnFormatterParameterTest()
        {
            // Arrange
            var sourceId = (int)GroupStatusColumns.SourceType.Parameter;
            var name = "PARAMETER";

            // Act
            var result = _columnFormatterFactory.Create(sourceId, name, new List<ConditionalFormat>());

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateColumnFormatterParamStandardTest()
        {
            // Arrange
            var sourceId = (int)GroupStatusColumns.SourceType.ParamStandard;
            var name = "PARAMSTANDARD";

            // Act
            var result = _columnFormatterFactory.Create(sourceId, name, new List<ConditionalFormat>());

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateColumnFormatterFacilityTest()
        {
            // Arrange
            var sourceId = (int)GroupStatusColumns.SourceType.Facility;
            var name = "FACILITY";

            // Act
            var result = _columnFormatterFactory.Create(sourceId, name, new List<ConditionalFormat>());

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateColumnFormatterFormulaTest()
        {
            // Arrange
            var sourceId = (int)GroupStatusColumns.SourceType.Formula;
            var name = string.Empty;

            // Act
            var result = _columnFormatterFactory.Create(sourceId, name, new List<ConditionalFormat>());

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void CreateConditionalFormatsTest()
        {
            var result = _columnFormatterFactory.Create(1, "CONDITIONAL", new List<ConditionalFormat>
            {
                new ConditionalFormat()
            });

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreatePercentCommColumnFormatterTest()
        {
            var result = _columnFormatterFactory.Create((int)GroupStatusColumns.SourceType.Common, "%COM",
                new List<ConditionalFormat>());

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreatePercentCommColumnFormatterCommSuccessTest()
        {
            var result = _columnFormatterFactory.Create((int)GroupStatusColumns.SourceType.Common, "TBLNODEMASTER.COMMSUCCESS",
                new List<ConditionalFormat>());

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateHostAlarmColumnFormatterTest()
        {
            var result = _columnFormatterFactory.Create((int)GroupStatusColumns.SourceType.Common, "HOSTALARMS",
                new List<ConditionalFormat>());

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreatePercentCommColumnFormatterExceptionGroupNameTest()
        {
            var result = _columnFormatterFactory.Create((int)GroupStatusColumns.SourceType.Common, "EXCEPTIONGROUPNAME",
                new List<ConditionalFormat>());

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreatePumpingUnitColumnFormatterTest()
        {
            var result = _columnFormatterFactory.Create((int)GroupStatusColumns.SourceType.Common, "PUMPING UNIT",
                new List<ConditionalFormat>());

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreatePumpingUnitManufacturerColumnFormatterTest()
        {
            var result = _columnFormatterFactory.Create((int)GroupStatusColumns.SourceType.Common, "PUMPING UNIT MANUFACTURER",
                new List<ConditionalFormat>());

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateRodGradeColumnFormatterTest()
        {
            var result = _columnFormatterFactory.Create((int)GroupStatusColumns.SourceType.Common, "ROD GRADE",
                new List<ConditionalFormat>());

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateAlarmColumnFormatterTest()
        {
            var result = _columnFormatterFactory.Create((int)GroupStatusColumns.SourceType.Common, "ALARMS",
                new List<ConditionalFormat>());

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateRTYColumnFormatterTest()
        {
            var result = _columnFormatterFactory.Create((int)GroupStatusColumns.SourceType.Common, "%RTY",
                new List<ConditionalFormat>());

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateCameraAlarmColumnFormatterTest()
        {
            var result = _columnFormatterFactory.Create((int)GroupStatusColumns.SourceType.Common, "CAMERAALARMS",
                new List<ConditionalFormat>());

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void CreateEnabledColumnFormatterTest()
        {
            var result = _columnFormatterFactory.Create((int)GroupStatusColumns.SourceType.Common, "ENBLD",
                new List<ConditionalFormat>());

            Assert.IsNotNull(result);
        }

        #region Private Methods

        private void SetupThetaLoggerFactory()
        {
            _loggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(_logger.Object);
        }

        private void SetupGroupStatusColumnFormat()
        {
            _groupStatusColumnFormat.Add(new CommColumnFormatter());
            _groupStatusColumnFormat.Add(new ControllerColumnFormatter(new Mock<IPocType>().Object));
            _groupStatusColumnFormat.Add(new ConditionalColumnFormatter());
            _groupStatusColumnFormat.Add(new PercentCommColumnFormatter());
            _groupStatusColumnFormat.Add(new HostAlarmColumnFormatter(new Mock<IHostAlarm>().Object));
            _groupStatusColumnFormat.Add(new ExceptionColumnFormatter(new Mock<IException>().Object));
            _groupStatusColumnFormat.Add(new PumpingUnitColumnFormatter(new Mock<IPumpingUnit>().Object));
            _groupStatusColumnFormat.Add(new PumpingUnitManufacturerColumnFormatter(new Mock<IPumpingUnitManufacturer>().Object));
            _groupStatusColumnFormat.Add(new RodGradeColumnFormatter(new Mock<IRod>().Object));
            _groupStatusColumnFormat.Add(new PercentFillColumnFormatter());
            _groupStatusColumnFormat.Add(new AlarmColumnFormatter());
            _groupStatusColumnFormat.Add(new PercentRTYColumnFormatter());
            _groupStatusColumnFormat.Add(new PercentRTColumnFormatter(new Mock<ICommonService>().Object));
            _groupStatusColumnFormat.Add(new TISColumnFormatter());
            _groupStatusColumnFormat.Add(new RunStatusColumnFormatter());
            _groupStatusColumnFormat.Add(new FacilityTagAlarmsColumnFormatter());
            _groupStatusColumnFormat.Add(new CameraAlarmColumnFormatter(new Mock<ILocalePhrases>().Object, new Mock<IMemoryCache>().Object));
            _groupStatusColumnFormat.Add(new EnabledColumnFormatter());
            _groupStatusColumnFormat.Add(new FacilityColumnFormatter());
            _groupStatusColumnFormat.Add(new ParamStandardTypeColumnFormatter());
            _groupStatusColumnFormat.Add(new ParameterColumnFormatter());
        }

        #endregion

    }
}
