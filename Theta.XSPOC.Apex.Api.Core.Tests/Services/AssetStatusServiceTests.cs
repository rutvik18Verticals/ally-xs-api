using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Theta.XSPOC.Apex.Api.Core.Models;
using Theta.XSPOC.Apex.Api.Core.Models.Inputs;
using Theta.XSPOC.Apex.Api.Core.Services.AssetStatus;
using Theta.XSPOC.Apex.Api.Data;
using Theta.XSPOC.Apex.Api.Data.Alarms;
using Theta.XSPOC.Apex.Api.Data.Asset;
using Theta.XSPOC.Apex.Api.Data.HistoricalData;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.Asset;
using Theta.XSPOC.Apex.Kernel.Collaboration.Models;
using Theta.XSPOC.Apex.Kernel.Quantity;
using Theta.XSPOC.Apex.Kernel.Data;
using Theta.XSPOC.Apex.Kernel.Quantity.Measures;
using Theta.XSPOC.Apex.Kernel.UnitConversion;
using Theta.XSPOC.Apex.Kernel.UnitConversion.Models;
using UnitCategory = Theta.XSPOC.Apex.Kernel.UnitConversion.Models.UnitCategory;
using AnalogValue = Theta.XSPOC.Apex.Kernel.UnitConversion.Models.AnalogValue;
using Theta.XSPOC.Apex.Kernel.Logging;
using Theta.XSPOC.Apex.Api.Core.Logging;
using Theta.XSPOC.Apex.Api.Core.Services;
using Microsoft.Extensions.Configuration;
using Theta.XSPOC.Apex.Kernel.DateTimeConversion;
using Theta.XSPOC.Apex.Kernel.Logging.Models;

namespace Theta.XSPOC.Apex.Api.Core.Tests.Services
{
    [TestClass]
    public class AssetStatusServiceTests
    {

        #region Private Fields

        private Mock<IThetaLoggerFactory> _loggerFactory;
        private Mock<IThetaLogger> _logger;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _loggerFactory = new Mock<IThetaLoggerFactory>();
            _logger = new Mock<IThetaLogger>();

            SetupThetaLoggerFactory();
        }

        #region Test Exceptions

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void AssetRepositoryNullTest()
        {
            _ = new AssetStatusService(null, new Mock<IHistoricalStore>().Object,
                new Mock<IAlarmStore>().Object, new Mock<IException>().Object,
                new Mock<IUserDefaultStore>().Object, new Mock<ISystemSettingStore>().Object,
                new Mock<ILocalePhrases>().Object, new Mock<IUnitConversion>().Object,
                new Mock<IThetaLoggerFactory>().Object, new Mock<ICommonService>().Object,
                new Mock<IConfiguration>().Object,
                new Mock<IDateTimeConverter>().Object);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void HistoricalRepositoryNullTest()
        {
            _ = new AssetStatusService(new Mock<IAssetStore>().Object, null,
                new Mock<IAlarmStore>().Object, new Mock<IException>().Object,
                new Mock<IUserDefaultStore>().Object, new Mock<ISystemSettingStore>().Object,
                new Mock<ILocalePhrases>().Object, new Mock<IUnitConversion>().Object,
                new Mock<IThetaLoggerFactory>().Object, new Mock<ICommonService>().Object,
                new Mock<IConfiguration>().Object,
                new Mock<IDateTimeConverter>().Object);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void AlarmRepositoryNullTest()
        {
            _ = new AssetStatusService(new Mock<IAssetStore>().Object, new Mock<IHistoricalStore>().Object,
                null, new Mock<IException>().Object, new Mock<IUserDefaultStore>().Object,
                new Mock<ISystemSettingStore>().Object, new Mock<ILocalePhrases>().Object,
                new Mock<IUnitConversion>().Object, new Mock<IThetaLoggerFactory>().Object, new Mock<ICommonService>().Object,
                new Mock<IConfiguration>().Object,
                new Mock<IDateTimeConverter>().Object); 
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ExceptionRepositoryNullTest()
        {
            _ = new AssetStatusService(new Mock<IAssetStore>().Object, new Mock<IHistoricalStore>().Object,
                new Mock<IAlarmStore>().Object, null, new Mock<IUserDefaultStore>().Object,
                new Mock<ISystemSettingStore>().Object, new Mock<ILocalePhrases>().Object,
                new Mock<IUnitConversion>().Object, new Mock<IThetaLoggerFactory>().Object, new Mock<ICommonService>().Object,
                new Mock<IConfiguration>().Object,
                new Mock<IDateTimeConverter>().Object);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void UserDefaultsRepositoryNullTest()
        {
            _ = new AssetStatusService(new Mock<IAssetStore>().Object, new Mock<IHistoricalStore>().Object,
                new Mock<IAlarmStore>().Object, new Mock<IException>().Object,
                null, new Mock<ISystemSettingStore>().Object, new Mock<ILocalePhrases>().Object,
                new Mock<IUnitConversion>().Object, new Mock<IThetaLoggerFactory>().Object, new Mock<ICommonService>().Object,
                new Mock<IConfiguration>().Object,
                new Mock<IDateTimeConverter>().Object);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void SystemSettingStoreNullTest()
        {
            _ = new AssetStatusService(new Mock<IAssetStore>().Object, new Mock<IHistoricalStore>().Object,
                new Mock<IAlarmStore>().Object, new Mock<IException>().Object,
                new Mock<IUserDefaultStore>().Object, null, new Mock<ILocalePhrases>().Object,
                new Mock<IUnitConversion>().Object, new Mock<IThetaLoggerFactory>().Object, new Mock<ICommonService>().Object,
                new Mock<IConfiguration>().Object,
                new Mock<IDateTimeConverter>().Object);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void PhraseRepositoryNullTest()
        {
            _ = new AssetStatusService(new Mock<IAssetStore>().Object, new Mock<IHistoricalStore>().Object,
                new Mock<IAlarmStore>().Object, new Mock<IException>().Object,
                new Mock<IUserDefaultStore>().Object, new Mock<ISystemSettingStore>().Object,
                null, new Mock<IUnitConversion>().Object, new Mock<IThetaLoggerFactory>().Object, new Mock<ICommonService>().Object,
                new Mock<IConfiguration>().Object,
                new Mock<IDateTimeConverter>().Object);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void UnitConversionNullTest()
        {
            _ = new AssetStatusService(new Mock<IAssetStore>().Object, new Mock<IHistoricalStore>().Object,
                new Mock<IAlarmStore>().Object, new Mock<IException>().Object,
                new Mock<IUserDefaultStore>().Object, new Mock<ISystemSettingStore>().Object,
                new Mock<ILocalePhrases>().Object, null, new Mock<IThetaLoggerFactory>().Object, new Mock<ICommonService>().Object,
                new Mock<IConfiguration>().Object,
                new Mock<IDateTimeConverter>().Object);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void NullLoggerTest()
        {
            _ = new AssetStatusService(new Mock<IAssetStore>().Object, new Mock<IHistoricalStore>().Object,
                new Mock<IAlarmStore>().Object, new Mock<IException>().Object,
                new Mock<IUserDefaultStore>().Object, new Mock<ISystemSettingStore>().Object,
                new Mock<ILocalePhrases>().Object, new Mock<IUnitConversion>().Object, null, new Mock<ICommonService>().Object,
                new Mock<IConfiguration>().Object,
                new Mock<IDateTimeConverter>().Object);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void CommonServiceNullTest()
        {
            _ = new AssetStatusService(new Mock<IAssetStore>().Object, new Mock<IHistoricalStore>().Object,
                new Mock<IAlarmStore>().Object, new Mock<IException>().Object,
                new Mock<IUserDefaultStore>().Object, new Mock<ISystemSettingStore>().Object,
                new Mock<ILocalePhrases>().Object, new Mock<IUnitConversion>().Object, new Mock<IThetaLoggerFactory>().Object, null, new Mock<IConfiguration>().Object,new Mock<IDateTimeConverter>().Object);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void ConfigurationNullTest()
        {
            _ = new AssetStatusService(new Mock<IAssetStore>().Object, new Mock<IHistoricalStore>().Object,
                new Mock<IAlarmStore>().Object, new Mock<IException>().Object,
                new Mock<IUserDefaultStore>().Object, new Mock<ISystemSettingStore>().Object,
                new Mock<ILocalePhrases>().Object, new Mock<IUnitConversion>().Object, new Mock<IThetaLoggerFactory>().Object, 
                new Mock<ICommonService>().Object, null, new Mock<IDateTimeConverter>().Object);
        }

        #endregion

        #region Test Methods

        [TestMethod]
        public async Task UserIdIsNullTest()
        {
            var correlationId = Guid.NewGuid().ToString();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var service = new AssetStatusService(new Mock<IAssetStore>().Object,
                new Mock<IHistoricalStore>().Object, new Mock<IAlarmStore>().Object,
                new Mock<IException>().Object, new Mock<IUserDefaultStore>().Object,
                new Mock<ISystemSettingStore>().Object, new Mock<ILocalePhrases>().Object,
                new Mock<IUnitConversion>().Object, mockThetaLoggerFactory.Object, new Mock<ICommonService>().Object,
                new Mock<IConfiguration>().Object,
                new Mock<IDateTimeConverter>().Object);

            var input = new WithCorrelationId<AssetStatusInput>(correlationId, new AssetStatusInput()
            {
                UserId = null,
                AssetId = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
            });

            var result = await service.GetAssetStatusDataAsync(input);

            Assert.IsNotNull(result);
            Assert.IsNull(result.Value);
            Assert.AreEqual(correlationId, result.CorrelationId);
        }

        [TestMethod]
        public async Task UserIdIsEmptyTest()
        {
            var correlationId = Guid.NewGuid().ToString();

            var logger = new Mock<IThetaLogger>();
            var mockThetaLoggerFactory = new Mock<IThetaLoggerFactory>();
            mockThetaLoggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(logger.Object);

            var service = new AssetStatusService(new Mock<IAssetStore>().Object,
                new Mock<IHistoricalStore>().Object, new Mock<IAlarmStore>().Object,
                new Mock<IException>().Object, new Mock<IUserDefaultStore>().Object,
                new Mock<ISystemSettingStore>().Object, new Mock<ILocalePhrases>().Object,
                new Mock<IUnitConversion>().Object, mockThetaLoggerFactory.Object, new Mock<ICommonService>().Object,
                new Mock<IConfiguration>().Object,
                new Mock<IDateTimeConverter>().Object);

            var input = new WithCorrelationId<AssetStatusInput>(correlationId, new AssetStatusInput()
            {
                UserId = string.Empty,
                AssetId = Guid.NewGuid(),
                CustomerId = Guid.NewGuid(),
            });

            var result = await service.GetAssetStatusDataAsync(input);

            Assert.IsNotNull(result);
            Assert.IsNull(result.Value);
            Assert.AreEqual(correlationId, result.CorrelationId);
        }

        [TestMethod]
        public async Task GetRodLiftAssetStatusDataAsyncTest()
        {
            var correlationId = Guid.NewGuid().ToString();
            var userId = "User";
            var assetId = Guid.NewGuid();
            var customerId = Guid.NewGuid();

            var service = GetService(userId, assetId, customerId, out var mockUnitConversion);

            mockUnitConversion.SetupSequence(m => m.Convert(It.IsAny<object>(), It.IsAny<UnitCategory>(),
                    It.IsAny<IDictionary<int, string>>(), It.IsAny<IDictionary<string, string>>(),
                    It.IsAny<ConversionAction>(), 15))
                .Returns(new ConversionData()
                {
                    Value = 100,
                    Units = "bpd",
                }).Returns(new ConversionData()
                {
                    Value = 20,
                    Units = "in",
                }).Returns(new ConversionData()
                {
                    Value = 200,
                    Units = "psi",
                }).Returns(new ConversionData()
                {
                    Value = 150,
                    Units = "mscf",
                }).Returns(new ConversionData()
                {
                    Value = 125,
                    Units = "psi",
                }).Returns(new ConversionData()
                {
                    Value = 54,
                    Units = "ft",
                }).Returns(new ConversionData()
                {
                    Value = 47,
                    Units = "in",
                }).Returns(new ConversionData()
                {
                    Value = 2000,
                    Units = "ft",
                }).Returns(new ConversionData()
                {
                    Value = 333,
                    Units = "ft",
                }).Returns(new ConversionData()
                {
                    Value = 222,
                    Units = "ft",
                }).Returns(new ConversionData()
                {
                    Value = 2,
                    Units = "bpd",
                }).Returns(new ConversionData()
                {
                    Value = 2,
                    Units = "bpd",
                });

            mockUnitConversion.SetupSequence(m => m.CreateUnitObject(It.IsAny<int>(), It.IsAny<float>()))
                .Returns(LiquidFlowRate.FromBarrelsPerDay(100))
                .Returns(Length.FromInches(20))
                .Returns(Pressure.FromPoundsPerSquareInch(200))
                .Returns(GasFlowRate.FromThousandStandardCubicFeetPerDay(150))
                .Returns(Pressure.FromPoundsPerSquareInch(125))
                .Returns(Length.FromFeet(54))
                .Returns(Length.FromInches(47))
                .Returns(Length.FromFeet(2000))
                .Returns(Length.FromFeet(333))
                .Returns(Length.FromFeet(222))
                .Returns(new AnalogValue(123))
                .Returns((IValue)null);

            var input = new WithCorrelationId<AssetStatusInput>(correlationId, new AssetStatusInput()
            {
                UserId = userId,
                AssetId = assetId,
                CustomerId = customerId,
            });

            var resultWithCorrelationId = await service.GetAssetStatusDataAsync(input);

            Assert.IsNotNull(resultWithCorrelationId);
            Assert.AreEqual(correlationId, resultWithCorrelationId.CorrelationId);

            var result = resultWithCorrelationId.Value;

            Assert.AreEqual(3, result.StatusRegisters.Count);
            Assert.AreEqual(6, result.Alarms.Count);
            Assert.AreEqual(1, result.Exceptions.Count);
            Assert.AreEqual(5, result.LastWellTest.Count);
            Assert.AreEqual(3, result.WellStatusOverview.Count);
            Assert.AreEqual(2, result.RodStrings.Count);
            Assert.AreEqual(36, result.ImageOverlayItems.Count);

            #region Test Status Registers

            Assert.IsNotNull(result.StatusRegisters[0]);
            Assert.AreEqual("A register", result.StatusRegisters[0].Label);
            Assert.AreEqual("123", result.StatusRegisters[0].Value);

            Assert.IsNotNull(result.StatusRegisters[1]);
            Assert.AreEqual("A string register", result.StatusRegisters[1].Label);
            Assert.AreEqual("The String Value", result.StatusRegisters[1].Value);

            #endregion

            #region Test Alarms

            Assert.IsNotNull(result.Alarms[0]);
            Assert.AreEqual("Alarm Config 2", result.Alarms[0].Label);
            Assert.AreEqual("A new alarm", result.Alarms[0].Value);

            #endregion

            #region Test Exceptions

            Assert.IsNotNull(result.Exceptions[0]);
            Assert.AreEqual(string.Empty, result.Exceptions[0].Label);
            Assert.AreEqual("Exception Description", result.Exceptions[0].Value);

            #endregion

            #region Test Well Tests

            Assert.IsNotNull(result.LastWellTest[0]);
            Assert.AreEqual("Test date", result.LastWellTest[0].Label);
            Assert.AreEqual(new DateTime(2001, 3, 4, 2, 5, 4).ToString(), result.LastWellTest[0].Value);

            Assert.IsNotNull(result.LastWellTest[1]);
            Assert.AreEqual("Test gas", result.LastWellTest[1].Label);
            Assert.AreEqual("150", result.LastWellTest[1].Value);

            Assert.IsNotNull(result.LastWellTest[2]);
            Assert.AreEqual("Test oil", result.LastWellTest[2].Label);
            Assert.AreEqual("50", result.LastWellTest[2].Value);

            Assert.IsNotNull(result.LastWellTest[3]);
            Assert.AreEqual("Test water", result.LastWellTest[3].Label);
            Assert.AreEqual("50", result.LastWellTest[3].Value);

            Assert.IsNotNull(result.LastWellTest[4]);
            Assert.AreEqual("Test gross", result.LastWellTest[4].Label);
            Assert.AreEqual("100", result.LastWellTest[4].Value);

            #endregion

            #region Test Well Status Overview

            Assert.IsNotNull(result.WellStatusOverview[0]);
            Assert.AreEqual("Communication yesterday", result.WellStatusOverview[0].Label);
            Assert.AreEqual("99", result.WellStatusOverview[0].Value);

            Assert.IsNotNull(result.WellStatusOverview[1]);
            Assert.AreEqual("Runtime today", result.WellStatusOverview[1].Label);
            Assert.AreEqual("91", result.WellStatusOverview[1].Value);

            Assert.IsNotNull(result.WellStatusOverview[2]);
            Assert.AreEqual("Runtime yesterday", result.WellStatusOverview[2].Label);
            Assert.AreEqual("97", result.WellStatusOverview[2].Value);

            #endregion

            #region Test Rod Strings

            Assert.IsNotNull(result.RodStrings[0]);
            Assert.AreEqual("1 Rod size name Rod Grade", result.RodStrings[0].Label);
            Assert.AreEqual("222 ft", result.RodStrings[0].Value);

            Assert.IsNotNull(result.RodStrings[1]);
            Assert.AreEqual("2 Rod size name 2 Rod Grade 2", result.RodStrings[1].Label);
            Assert.AreEqual("333 ft", result.RodStrings[1].Value);

            #endregion

            #region Test Overlay Fields

            var item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.StrokeLength);

            Assert.IsNotNull(item);

            Assert.AreEqual("Stroke length", item.Label);
            Assert.AreEqual("20 in", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.StrokeLength, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.LastGoodScan);

            Assert.IsNotNull(item);

            Assert.AreEqual("Last good scan", item.Label);
            Assert.AreEqual(new DateTime(2002, 1, 3, 2, 5, 4).ToString("M/d/yyyy, h:mm:ss tt"), item.Value);
            Assert.AreEqual(DisplayState.Emphasis, item.DisplayState);
            Assert.AreEqual(OverlayFields.LastGoodScan, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.RunStatus);

            Assert.IsNotNull(item);

            Assert.AreEqual("Run status", item.Label);
            Assert.AreEqual("Running", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.RunStatus, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.ControllerType);

            Assert.IsNotNull(item);

            Assert.AreEqual("Controller", item.Label);
            Assert.AreEqual("Lufkin Sam", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.ControllerType, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.StrokesPerMinute);

            Assert.IsNotNull(item);

            Assert.AreEqual("SPM", item.Label);
            Assert.AreEqual("8.5", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.StrokesPerMinute, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.TimeInState);

            Assert.IsNotNull(item);

            Assert.AreEqual("Time in state", item.Label);
            Assert.AreEqual("1.3 h", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.TimeInState, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.TubingPressure);

            Assert.IsNotNull(item);

            Assert.AreEqual("Tubing pressure", item.Label);
            Assert.AreEqual("200 psi", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.TubingPressure, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.MotorType);

            Assert.IsNotNull(item);

            Assert.AreEqual("Motor type", item.Label);
            Assert.AreEqual("Prime [Size (HP): 145]", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.MotorType, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.PumpFillage);

            Assert.IsNotNull(item);

            Assert.AreEqual("Pump fillage", item.Label);
            Assert.AreEqual("95 %", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.PumpFillage, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.StructuralLoading);

            Assert.IsNotNull(item);

            Assert.AreEqual("Structural loading", item.Label);
            Assert.AreEqual("85", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.StructuralLoading, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.RodLoading);

            Assert.IsNotNull(item);

            Assert.AreEqual("Rod loading", item.Label);
            Assert.AreEqual("75", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.RodLoading, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.CasingPressure);

            Assert.IsNotNull(item);

            Assert.AreEqual("Casing pressure", item.Label);
            Assert.AreEqual("125 psi", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.CasingPressure, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.MotorLoading);

            Assert.IsNotNull(item);

            Assert.AreEqual("Motor loading", item.Label);
            Assert.AreEqual("87 %", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.MotorLoading, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m =>
                m.OverlayField == OverlayFields.CurrentCommunicationStatus);

            Assert.IsNotNull(item);

            Assert.AreEqual("Comms status", item.Label);
            Assert.AreEqual("OK", item.Value);
            Assert.AreEqual(DisplayState.Ok, item.DisplayState);
            Assert.AreEqual(OverlayFields.CurrentCommunicationStatus, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.GearboxLoading);

            Assert.IsNotNull(item);

            Assert.AreEqual("Gearbox loading (%)", item.Label);
            Assert.AreEqual("10", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.GearboxLoading, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.CyclesYesterday);

            Assert.IsNotNull(item);

            Assert.AreEqual("Cycles yesterday", item.Label);
            Assert.AreEqual("111", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.CyclesYesterday, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.CyclesToday);

            Assert.IsNotNull(item);

            Assert.AreEqual("Cycles today", item.Label);
            Assert.AreEqual("112", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.CyclesToday, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            #endregion

            _logger.Verify(x => x.WriteCId(Kernel.Logging.Models.Level.Debug,
                It.Is<string>(m => m.Contains("ParamStandardType 183 Address 1 Scale is greater than 15")), It.IsAny<string>()));
            _logger.Verify(x => x.WriteCId(Kernel.Logging.Models.Level.Debug,
                It.Is<string>(m => m.Contains("Address 1 Scale is greater than 15")), It.IsAny<string>()));
        }

        [TestMethod]
        public async Task GetRodLiftAssetStatusDataChemicalInjectionAsyncTest()
        {
            var correlationId = Guid.NewGuid().ToString();
            var userId = "User";
            var assetId = Guid.NewGuid();
            var customerId = Guid.NewGuid();

            var service = GetServiceChemicalInjection(userId, assetId, customerId, out var mockUnitConversion);

            mockUnitConversion.SetupSequence(m => m.Convert(It.IsAny<object>(), It.IsAny<UnitCategory>(),
                    It.IsAny<IDictionary<int, string>>(), It.IsAny<IDictionary<string, string>>(),
                    It.IsAny<ConversionAction>(), 15))
                .Returns(new ConversionData()
                {
                    Value = 100,
                    Units = "bpd",
                }).Returns(new ConversionData()
                {
                    Value = 20,
                    Units = "in",
                }).Returns(new ConversionData()
                {
                    Value = 200,
                    Units = "psi",
                }).Returns(new ConversionData()
                {
                    Value = 150,
                    Units = "mscf",
                }).Returns(new ConversionData()
                {
                    Value = 125,
                    Units = "psi",
                }).Returns(new ConversionData()
                {
                    Value = 54,
                    Units = "ft",
                }).Returns(new ConversionData()
                {
                    Value = 47,
                    Units = "in",
                }).Returns(new ConversionData()
                {
                    Value = 2000,
                    Units = "ft",
                }).Returns(new ConversionData()
                {
                    Value = 333,
                    Units = "ft",
                }).Returns(new ConversionData()
                {
                    Value = 222,
                    Units = "ft",
                }).Returns(new ConversionData()
                {
                    Value = 2,
                    Units = "bpd",
                }).Returns(new ConversionData()
                {
                    Value = 2,
                    Units = "bpd",
                });

            mockUnitConversion.SetupSequence(m => m.CreateUnitObject(It.IsAny<int>(), It.IsAny<float>()))
                .Returns(LiquidFlowRate.FromBarrelsPerDay(100))
                .Returns(Length.FromInches(20))
                .Returns(Pressure.FromPoundsPerSquareInch(200))
                .Returns(GasFlowRate.FromThousandStandardCubicFeetPerDay(150))
                .Returns(Pressure.FromPoundsPerSquareInch(125))
                .Returns(Length.FromFeet(54))
                .Returns(Length.FromInches(47))
                .Returns(Length.FromFeet(2000))
                .Returns(Length.FromFeet(333))
                .Returns(Length.FromFeet(222))
                .Returns(new AnalogValue(123))
                .Returns((IValue)null);

            var input = new WithCorrelationId<AssetStatusInput>(correlationId, new AssetStatusInput()
            {
                UserId = userId,
                AssetId = assetId,
                CustomerId = customerId,
            });

            var resultWithCorrelationId = await service.GetAssetStatusDataAsync(input);

            Assert.IsNotNull(resultWithCorrelationId);
            Assert.AreEqual(correlationId, resultWithCorrelationId.CorrelationId);

            var result = resultWithCorrelationId.Value;

            Assert.AreEqual(3, result.StatusRegisters.Count);
            Assert.AreEqual(6, result.Alarms.Count);
            Assert.AreEqual(1, result.Exceptions.Count);
            Assert.AreEqual(5, result.LastWellTest.Count);
            Assert.AreEqual(3, result.WellStatusOverview.Count);
            Assert.AreEqual(0, result.RodStrings.Count);
            Assert.AreEqual(36, result.ImageOverlayItems.Count);

            #region Test Status Registers

            Assert.IsNotNull(result.StatusRegisters[0]);
            Assert.AreEqual("A register", result.StatusRegisters[0].Label);
            Assert.AreEqual("123", result.StatusRegisters[0].Value);

            Assert.IsNotNull(result.StatusRegisters[1]);
            Assert.AreEqual("A string register", result.StatusRegisters[1].Label);
            Assert.AreEqual("The String Value", result.StatusRegisters[1].Value);

            #endregion

            #region Test Alarms

            Assert.IsNotNull(result.Alarms[0]);
            Assert.AreEqual("Alarm Config 2", result.Alarms[0].Label);
            Assert.AreEqual("A new alarm", result.Alarms[0].Value);

            #endregion

            #region Test Exceptions

            Assert.IsNotNull(result.Exceptions[0]);
            Assert.AreEqual(string.Empty, result.Exceptions[0].Label);
            Assert.AreEqual("Exception Description", result.Exceptions[0].Value);

            #endregion

            #region Test Well Tests

            Assert.IsNotNull(result.LastWellTest[0]);
            Assert.AreEqual("Test date", result.LastWellTest[0].Label);
            Assert.AreEqual(new DateTime(2001, 3, 4, 2, 5, 4).ToString(), result.LastWellTest[0].Value);

            Assert.IsNotNull(result.LastWellTest[1]);
            Assert.AreEqual("Test gas", result.LastWellTest[1].Label);
            Assert.AreEqual("150", result.LastWellTest[1].Value);

            Assert.IsNotNull(result.LastWellTest[2]);
            Assert.AreEqual("Test oil", result.LastWellTest[2].Label);
            Assert.AreEqual("50", result.LastWellTest[2].Value);

            Assert.IsNotNull(result.LastWellTest[3]);
            Assert.AreEqual("Test water", result.LastWellTest[3].Label);
            Assert.AreEqual("50", result.LastWellTest[3].Value);

            Assert.IsNotNull(result.LastWellTest[4]);
            Assert.AreEqual("Test gross", result.LastWellTest[4].Label);
            Assert.AreEqual("100", result.LastWellTest[4].Value);

            #endregion

            #region Test Well Status Overview

            Assert.IsNotNull(result.WellStatusOverview[0]);
            Assert.AreEqual("Communication yesterday", result.WellStatusOverview[0].Label);
            Assert.AreEqual("99", result.WellStatusOverview[0].Value);

            Assert.IsNotNull(result.WellStatusOverview[1]);
            Assert.AreEqual("Runtime today", result.WellStatusOverview[1].Label);
            Assert.AreEqual("91", result.WellStatusOverview[1].Value);

            Assert.IsNotNull(result.WellStatusOverview[2]);
            Assert.AreEqual("Runtime yesterday", result.WellStatusOverview[2].Label);
            Assert.AreEqual("97", result.WellStatusOverview[2].Value);

            #endregion

            #region Test Overlay Fields

            var item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.StrokeLength);

            Assert.IsNotNull(item);

            Assert.AreEqual("Stroke length", item.Label);
            Assert.AreEqual("20 in", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.StrokeLength, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.LastGoodScan);

            Assert.IsNotNull(item);

            Assert.AreEqual("Last good scan", item.Label);
            Assert.AreEqual(new DateTime(2002, 1, 3, 2, 5, 4).ToString("M/d/yyyy, h:mm:ss tt"), item.Value);
            Assert.AreEqual(DisplayState.Emphasis, item.DisplayState);
            Assert.AreEqual(OverlayFields.LastGoodScan, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.RunStatus);

            Assert.IsNotNull(item);

            Assert.AreEqual("Run status", item.Label);
            Assert.AreEqual("Running", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.RunStatus, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.ControllerType);

            Assert.IsNotNull(item);

            Assert.AreEqual("Controller", item.Label);
            Assert.AreEqual("Lufkin Sam", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.ControllerType, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.StrokesPerMinute);

            Assert.IsNotNull(item);

            Assert.AreEqual("SPM", item.Label);
            Assert.AreEqual("8.5", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.StrokesPerMinute, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.TimeInState);

            Assert.IsNotNull(item);

            Assert.AreEqual("Time in state", item.Label);
            Assert.AreEqual("1.3 h", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.TimeInState, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.TubingPressure);

            Assert.IsNotNull(item);

            Assert.AreEqual("Tubing pressure", item.Label);
            Assert.AreEqual("200 psi", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.TubingPressure, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.MotorType);

            Assert.IsNotNull(item);

            Assert.AreEqual("Motor type", item.Label);
            Assert.AreEqual("Prime [Size (HP): 145]", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.MotorType, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.PumpFillage);

            Assert.IsNotNull(item);

            Assert.AreEqual("Pump fillage", item.Label);
            Assert.AreEqual("95 %", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.PumpFillage, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.StructuralLoading);

            Assert.IsNotNull(item);

            Assert.AreEqual("Structural loading", item.Label);
            Assert.AreEqual("85", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.StructuralLoading, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.RodLoading);

            Assert.IsNotNull(item);

            Assert.AreEqual("Rod loading", item.Label);
            Assert.AreEqual("75", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.RodLoading, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.CasingPressure);

            Assert.IsNotNull(item);

            Assert.AreEqual("Casing pressure", item.Label);
            Assert.AreEqual("125 psi", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.CasingPressure, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.MotorLoading);

            Assert.IsNotNull(item);

            Assert.AreEqual("Motor loading", item.Label);
            Assert.AreEqual("87 %", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.MotorLoading, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m =>
                m.OverlayField == OverlayFields.CurrentCommunicationStatus);

            Assert.IsNotNull(item);

            Assert.AreEqual("Comms status", item.Label);
            Assert.AreEqual("OK", item.Value);
            Assert.AreEqual(DisplayState.Ok, item.DisplayState);
            Assert.AreEqual(OverlayFields.CurrentCommunicationStatus, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.GearboxLoading);

            Assert.IsNotNull(item);

            Assert.AreEqual("Gearbox loading (%)", item.Label);
            Assert.AreEqual("10", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.GearboxLoading, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.CyclesYesterday);

            Assert.IsNotNull(item);

            Assert.AreEqual("Cycles yesterday", item.Label);
            Assert.AreEqual("111", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.CyclesYesterday, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.CyclesToday);

            Assert.IsNotNull(item);

            Assert.AreEqual("Cycles today", item.Label);
            Assert.AreEqual("112", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.CyclesToday, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            #endregion

            _logger.Verify(x => x.WriteCId(Kernel.Logging.Models.Level.Debug,
                It.Is<string>(m => m.Contains("ParamStandardType 183 Address 1 Scale is greater than 15")), It.IsAny<string>()));
            _logger.Verify(x => x.WriteCId(Kernel.Logging.Models.Level.Debug,
                It.Is<string>(m => m.Contains("Address 1 Scale is greater than 15")), It.IsAny<string>()));
        }

        #endregion

        #region Private Data Setup Methods

        private static IDictionary<string, UserDefaultItem> GetEmptyDefaults()
        {
            return new Dictionary<string, UserDefaultItem>();
        }

        private static RodLiftAssetStatusCoreData GetCoreData(Guid assetId, Guid customerId)
        {
            return new RodLiftAssetStatusCoreData()
            {
                APIDesignation = "Test",
                NodeId = "Test Node",
                AssetGUID = assetId,
                GrossRate = LiquidFlowRate.FromBarrelsPerDay(100),
                StrokeLength = Length.FromInches(20),
                LastGoodScan = new DateTime(2002, 1, 3, 2, 5, 4),
                RunStatus = "Running",
                StrokesPerMinute = 8.5f,
                TimeInState = 80,
                TubingPressure = Pressure.FromPoundsPerSquareInch(200),
                MotorTypeId = 4,
                PumpFillage = 95,
                StructuralLoading = 85,
                LastWellTestDate = new DateTime(2001, 3, 4, 2, 5, 4),
                WaterCut = Fraction.FromPercentage(50),
                GasRate = GasFlowRate.FromThousandStandardCubicFeetPerDay(150),
                MaxRodLoading = 75,
                YesterdayRuntimePercentage = 97,
                CasingPressure = Pressure.FromPoundsPerSquareInch(125),
                CommunicationPercentageYesterday = 99,
                MotorLoad = 87,
                PumpEfficiencyPercentage = 88,
                RatedHorsePower = 145,
                TodayRuntimePercentage = 91,
                PrimeMoverType = "Prime",
                CommunicationStatus = "OK",
                CalculatedFluidLevelAbovePump = 105,
                FirmwareVersion = 1.01f,
                FluidLevel = Length.FromFeet(54),
                GearBoxLoadPercentage = 10,
                PlungerDiameter = Length.FromInches(47),
                PumpDepth = Length.FromFeet(2000),
                PumpEfficiency = 96,
                ESPResultTestDate = new DateTime(2002, 1, 1, 2, 5, 4),
                IsNodeEnabled = true,
                MotorKindName = "Kind",
                NodeAddress = "i127.0.0.1|502|1",
                PocTypeDescription = "Lufkin Sam",
                PumpingUnitManufacturer = "Manufacturer",
                PumpingUnitName = "Pump Name",
                StrokeLengthUnitString = "ft",
                CasingPressureUnitString = "psi",
                TubingPressureUnitString = "psi 2",
                FluidLevelUnitString = "ft 2",
                PlungerDiameterUnitString = "in",
                PumpDepthUnitString = "ft 3",
                CustomerGUID = customerId,
                ApplicationId = 3,
                PumpingUnitTypeId = "7",
            };
        }

        private static RodLiftAssetStatusCoreData GetCoreDataChemicalInjection(Guid assetId, Guid customerId)
        {
            return new RodLiftAssetStatusCoreData()
            {
                APIDesignation = "Test",
                NodeId = "Test Node",
                AssetGUID = assetId,
                GrossRate = LiquidFlowRate.FromBarrelsPerDay(100),
                StrokeLength = Length.FromInches(20),
                LastGoodScan = new DateTime(2002, 1, 3, 2, 5, 4),
                RunStatus = "Running",
                StrokesPerMinute = 8.5f,
                TimeInState = 80,
                TubingPressure = Pressure.FromPoundsPerSquareInch(200),
                MotorTypeId = 4,
                PumpFillage = 95,
                StructuralLoading = 85,
                LastWellTestDate = new DateTime(2001, 3, 4, 2, 5, 4),
                WaterCut = Fraction.FromPercentage(50),
                GasRate = GasFlowRate.FromThousandStandardCubicFeetPerDay(150),
                MaxRodLoading = 75,
                YesterdayRuntimePercentage = 97,
                CasingPressure = Pressure.FromPoundsPerSquareInch(125),
                CommunicationPercentageYesterday = 99,
                MotorLoad = 87,
                PumpEfficiencyPercentage = 88,
                RatedHorsePower = 145,
                TodayRuntimePercentage = 91,
                PrimeMoverType = "Prime",
                CommunicationStatus = "OK",
                CalculatedFluidLevelAbovePump = 105,
                FirmwareVersion = 1.01f,
                FluidLevel = Length.FromFeet(54),
                GearBoxLoadPercentage = 10,
                PlungerDiameter = Length.FromInches(47),
                PumpDepth = Length.FromFeet(2000),
                PumpEfficiency = 96,
                ESPResultTestDate = new DateTime(2002, 1, 1, 2, 5, 4),
                IsNodeEnabled = true,
                MotorKindName = "Kind",
                NodeAddress = "i127.0.0.1|502|1",
                PocTypeDescription = "Lufkin Sam",
                PumpingUnitManufacturer = "Manufacturer",
                PumpingUnitName = "Pump Name",
                StrokeLengthUnitString = "ft",
                CasingPressureUnitString = "psi",
                TubingPressureUnitString = "psi 2",
                FluidLevelUnitString = "ft 2",
                PlungerDiameterUnitString = "in",
                PumpDepthUnitString = "ft 3",
                CustomerGUID = customerId,
                ApplicationId = 17,
                PumpingUnitTypeId = "7",
            };
        }

        private static IList<AlarmData> GetAlarmConfigData()
        {
            return new List<AlarmData>()
            {
                new AlarmData()
                {
                    AlarmType = "Alarm Config 1",
                    AlarmDescription = "Alarm Config Description 1",
                    AlarmPriority = 4,
                    AlarmBit = 1,
                    CurrentValue = 1,
                },
                new AlarmData()
                {
                    AlarmType = "Alarm Config 2",
                    AlarmDescription = "Alarm Config Description 2",
                    AlarmPriority = 4,
                    AlarmBit = 1,
                    CurrentValue = 1,
                    StateText = "A new alarm",
                },
            };
        }

        private static IList<AlarmData> GetHostAlarmData()
        {
            return new List<AlarmData>()
            {
                new AlarmData()
                {
                    AlarmType = "Host Alarm 1",
                    AlarmDescription = "Host Alarm Description 1",
                    AlarmPriority = 1,
                    AlarmBit = 1,
                    CurrentValue = 1,
                    AlarmState = 1,
                },
                new AlarmData()
                {
                    AlarmType = "Host Alarm 2",
                    AlarmDescription = "Host Alarm Description 2",
                    AlarmPriority = 1,
                    AlarmBit = 1,
                    CurrentValue = 1,
                    AlarmState = 101,
                },
                new AlarmData()
                {
                    AlarmType = "Host Alarm 3",
                    AlarmDescription = "Host Alarm Description 3",
                    AlarmPriority = 1,
                    AlarmBit = 1,
                    CurrentValue = 1,
                    AlarmState = 100,
                },
            };
        }

        private static IList<AlarmData> GetFacilityTagAlarmData()
        {
            return new List<AlarmData>()
            {
                new AlarmData()
                {
                    AlarmType = "Facility Tag Alarm 1",
                    AlarmDescription = "Facility Tag Alarm Description 1",
                    AlarmPriority = 2,
                },
            };
        }

        private static IList<AlarmData> GetCameraAlarmData()
        {
            return new List<AlarmData>()
            {
                new AlarmData()
                {
                    AlarmType = "Camera Alarm 1",
                    AlarmDescription = "Camera Alarm Description 1",
                    AlarmPriority = 3,
                },
            };
        }

        private static IList<ExceptionData> GetExceptionData()
        {
            return new List<ExceptionData>()
            {
                new ExceptionData()
                {
                    Description = "Exception Description",
                },
            };
        }

        public static IList<ParamStandardData> GetParamStandardData()
        {
            return new List<ParamStandardData>()
            {
                new ParamStandardData()
                {
                    ParamStandardType = 181,
                    Value = new AnalogValue(111),
                },
                new ParamStandardData()
                {
                    ParamStandardType = 182,
                    Value = new AnalogValue(112),
                },
                new ParamStandardData()
                {
                    ParamStandardType = 183,
                    Address = 1,
                    Value = LiquidFlowRate.FromBarrelsPerDay(1.7999999523162842),
                }
            };
        }

        public static IList<RegisterData> GetRegisterData()
        {
            return new List<RegisterData>()
            {
                new RegisterData()
                {
                    Value = new AnalogValue(123),
                    Description = "A register",
                },
                new RegisterData()
                {
                    StringValue = "The String Value",
                    Description = "A String Register",
                },
                new RegisterData()
                {
                    Address = 1,
                    Value = LiquidFlowRate.FromBarrelsPerDay(1.7999999523162842),
                }
            };
        }

        private static IList<RodStringData> GetRodStringData()
        {
            return new List<RodStringData>()
            {
                new RodStringData()
                {
                    Length = Length.FromFeet(333),
                    Diameter = 2.75f,
                    RodStringGradeName = "Rod Grade 2",
                    RodStringPositionNumber = 2,
                    RodStringSizeDisplayName = "Rod size name 2",
                    UnitString = "ft 2",
                },
                new RodStringData()
                {
                    Length = Length.FromFeet(222),
                    Diameter = 1.75f,
                    RodStringGradeName = "Rod Grade",
                    RodStringPositionNumber = 1,
                    RodStringSizeDisplayName = "Rod size name",
                    UnitString = "ft",
                }
            };
        }

        private AssetStatusService GetService(string userId, Guid assetId, Guid customerId,
            out Mock<IUnitConversion> mockUnitConversion)
        {
            var mockAssetRepository = new Mock<IAssetStore>();
            mockAssetRepository.Setup(m => m.GetAssetStatusDataAsync(assetId))
                .ReturnsAsync(GetCoreData(assetId, customerId));
            mockAssetRepository.Setup(m => m.GetRodStringAsync(assetId)).ReturnsAsync(GetRodStringData);

            var mockHistoricalRepository = new Mock<IHistoricalStore>();
            mockHistoricalRepository.Setup(m => m.GetAssetStatusRegisterDataAsync(assetId, customerId))
                .ReturnsAsync(GetRegisterData);
            mockHistoricalRepository.Setup(m => m.GetParamStandardDataAsync(assetId, customerId))
                .ReturnsAsync(GetParamStandardData);

            var mockAlarmRepository = new Mock<IAlarmStore>();
            mockAlarmRepository.Setup(m => m.GetAlarmConfigurationAsync(assetId, customerId))
                .ReturnsAsync(GetAlarmConfigData);
            mockAlarmRepository.Setup(m => m.GetCameraAlarmsAsync(assetId, customerId))
                .ReturnsAsync(GetCameraAlarmData);
            mockAlarmRepository.Setup(m => m.GetFacilityTagAlarmsAsync(assetId, customerId))
                .ReturnsAsync(GetFacilityTagAlarmData);
            mockAlarmRepository.Setup(m => m.GetHostAlarmsAsync(assetId, customerId)).ReturnsAsync(GetHostAlarmData);

            var mockExceptionRepository = new Mock<IException>();
            mockExceptionRepository.Setup(m => m.GetAssetStatusExceptionsAsync(assetId, customerId, It.IsAny<string>()))
                .ReturnsAsync(GetExceptionData);

            var mockUserDefaultRepository = new Mock<IUserDefaultStore>();
            mockUserDefaultRepository.Setup(m => m.GetDefaultItemByGroup(userId, It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(GetEmptyDefaults);
            var mockSystemSettingsStore = new Mock<ISystemSettingStore>();

            var mockPhraseRepository = new Mock<ILocalePhrases>();
            mockPhraseRepository.Setup(m => m.GetAll(It.IsAny<string>(), It.IsAny<int[]>()))
                .Returns(new Dictionary<int, string>());

            mockUnitConversion = new Mock<IUnitConversion>();

            var mockConfiguration = new Mock<IConfiguration>();
            var mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(x => x.Value).Returns("15");
            mockConfiguration.Setup(m => m.GetSection("AppSettings:AssetStatusRefreshInterval")).Returns(mockSection.Object);

            var mockDateTimeConverter = new Mock<IDateTimeConverter>();
            var lastGoodScanTime = new DateTime(2002, 1, 3, 2, 05, 04);

            mockDateTimeConverter.Setup(m => m.ConvertToApplicationServerTimeFromUTC(It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<LoggingModel>())).Returns(lastGoodScanTime);

            mockDateTimeConverter.Setup(x => x.GetTimeZoneAdjustedTime(It.IsAny<float>(), It.IsAny<bool>(),
             It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<LoggingModel>())).Returns(lastGoodScanTime);

            return new AssetStatusService(mockAssetRepository.Object,
                mockHistoricalRepository.Object, mockAlarmRepository.Object,
                mockExceptionRepository.Object, mockUserDefaultRepository.Object,
                mockSystemSettingsStore.Object, mockPhraseRepository.Object,
                mockUnitConversion.Object, _loggerFactory.Object, new Mock<ICommonService>().Object,
                mockConfiguration.Object,
                mockDateTimeConverter.Object);
        }

        private AssetStatusService GetServiceChemicalInjection(string userId, Guid assetId, Guid customerId,
            out Mock<IUnitConversion> mockUnitConversion)
        {
            var mockAssetRepository = new Mock<IAssetStore>();
            mockAssetRepository.Setup(m => m.GetAssetStatusDataAsync(assetId))
                .ReturnsAsync(GetCoreDataChemicalInjection(assetId, customerId));
            mockAssetRepository.Setup(m => m.GetRodStringAsync(assetId)).ReturnsAsync(GetRodStringData);

            var mockHistoricalRepository = new Mock<IHistoricalStore>();
            mockHistoricalRepository.Setup(m => m.GetAssetStatusRegisterDataAsync(assetId, customerId))
                .ReturnsAsync(GetRegisterData);
            mockHistoricalRepository.Setup(m => m.GetParamStandardDataAsync(assetId, customerId))
                .ReturnsAsync(GetParamStandardData);

            var mockAlarmRepository = new Mock<IAlarmStore>();
            mockAlarmRepository.Setup(m => m.GetAlarmConfigurationAsync(assetId, customerId))
                .ReturnsAsync(GetAlarmConfigData);
            mockAlarmRepository.Setup(m => m.GetCameraAlarmsAsync(assetId, customerId))
                .ReturnsAsync(GetCameraAlarmData);
            mockAlarmRepository.Setup(m => m.GetFacilityTagAlarmsAsync(assetId, customerId))
                .ReturnsAsync(GetFacilityTagAlarmData);
            mockAlarmRepository.Setup(m => m.GetHostAlarmsAsync(assetId, customerId)).ReturnsAsync(GetHostAlarmData);

            var mockExceptionRepository = new Mock<IException>();
            mockExceptionRepository.Setup(m => m.GetAssetStatusExceptionsAsync(assetId, customerId, It.IsAny<string>()))
                .ReturnsAsync(GetExceptionData);

            var mockUserDefaultRepository = new Mock<IUserDefaultStore>();
            mockUserDefaultRepository.Setup(m => m.GetDefaultItemByGroup(userId, It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(GetEmptyDefaults);
            var mockSystemSettingsStore = new Mock<ISystemSettingStore>();

            var mockPhraseRepository = new Mock<ILocalePhrases>();
            mockPhraseRepository.Setup(m => m.GetAll(It.IsAny<string>(), It.IsAny<int[]>()))
                .Returns(new Dictionary<int, string>());

            mockUnitConversion = new Mock<IUnitConversion>();

            var mockConfiguration = new Mock<IConfiguration>();
            var mockSection = new Mock<IConfigurationSection>();
            mockSection.Setup(x => x.Value).Returns("15");
            mockConfiguration.Setup(m => m.GetSection("AppSettings:AssetStatusRefreshInterval")).Returns(mockSection.Object);

            var mockDateTimeConverter = new Mock<IDateTimeConverter>();
            var lastGoodScanTime = new DateTime(2002, 1, 3, 2, 05, 04);
           
            mockDateTimeConverter.Setup(m => m.ConvertToApplicationServerTimeFromUTC(It.IsAny<DateTime>(),
                It.IsAny<string>(), It.IsAny<LoggingModel>())).Returns(lastGoodScanTime);

            mockDateTimeConverter.Setup(x => x.GetTimeZoneAdjustedTime(It.IsAny<float>(), It.IsAny<bool>(),
             It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<LoggingModel>())).Returns(lastGoodScanTime);

            return new AssetStatusService(mockAssetRepository.Object,
                mockHistoricalRepository.Object, mockAlarmRepository.Object,
                mockExceptionRepository.Object, mockUserDefaultRepository.Object,
                mockSystemSettingsStore.Object, mockPhraseRepository.Object,
                mockUnitConversion.Object, _loggerFactory.Object, new Mock<ICommonService>().Object,
                mockConfiguration.Object,
                mockDateTimeConverter.Object);
        }

        private void SetupThetaLoggerFactory()
        {
            _loggerFactory.Setup(x => x.Create(It.IsAny<LoggingModel>())).Returns(_logger.Object);
        }

        #endregion

    }
}