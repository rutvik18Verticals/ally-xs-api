using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Theta.XSPOC.Apex.Api.Core.Models;
using Theta.XSPOC.Apex.Api.Core.Models.Mappers;
using Theta.XSPOC.Apex.Api.Data.Models;
using Theta.XSPOC.Apex.Api.Data.Models.Asset;
using Theta.XSPOC.Apex.Kernel.Quantity.Measures;
using Theta.XSPOC.Apex.Kernel.UnitConversion.Models;

namespace Theta.XSPOC.Apex.Api.Core.Tests.Models.Mappers
{
    [TestClass]
    public class RodLiftAssetStatusMapperTests
    {

        #region Test Methods

        [TestMethod]
        public void RodLiftAssetStatusDataContractMapDataIsNullTest()
        {
            var result = AssetStatusMapper.Map(null);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void RodLiftAssetStatusDataContractMapCoreDataIsNullTest()
        {
            var result = AssetStatusMapper.Map(new RodLiftAssetStatusData()
            {
                CoreData = null,
            });

            Assert.IsNull(result);
        }

        [TestMethod]
        public void RodLiftAssetStatusDataContractMapCoreDataIValuesAreNullTest()
        {
            var result = AssetStatusMapper.Map(new RodLiftAssetStatusData()
            {
                CoreData = new RodLiftAssetStatusCoreData()
                {
                    APIDesignation = "Test",
                },
            });

            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty,
                result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.CasingPressure)?.Value);
            Assert.AreEqual(string.Empty,
                result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.TubingPressure)?.Value);
            Assert.AreEqual(string.Empty,
                result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.StrokeLength)?.Value);
            Assert.AreEqual("0.0 m",
                result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.TimeInState)?.Value);
        }

        [TestMethod]
        public void RodLiftAssetStatusDataContractMapTest()
        {
            var assetGUID = Guid.NewGuid();
            var lastGoodScanTime = new DateTime(2001, 3, 4, 2, 5, 4);
            var lastWellTest = new DateTime(2001, 3, 4, 2, 5, 4);
            var espTestResultDate = new DateTime(2002, 1, 1, 2, 5, 4);

            var result = AssetStatusMapper.Map(new RodLiftAssetStatusData()
            {
                CoreData = new RodLiftAssetStatusCoreData()
                {
                    APIDesignation = "Test",
                    NodeId = "Test Node",
                    AssetGUID = assetGUID,
                    GrossRate = LiquidFlowRate.FromBarrelsPerDay(100),
                    StrokeLength = Length.FromInches(20),
                    LastGoodScan = lastGoodScanTime,
                    RunStatus = "Running",
                    StrokesPerMinute = 8.5f,
                    TimeInState = 80,
                    TubingPressure = Pressure.FromPoundsPerSquareInch(200),
                    MotorTypeId = 4,
                    PumpFillage = 95,
                    StructuralLoading = 85,
                    LastWellTestDate = lastWellTest,
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
                    ESPResultTestDate = espTestResultDate,
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
                },
                AlarmData = new List<AlarmData>()
                {
                    new AlarmData()
                    {
                        AlarmType = "Alarm 1",
                        AlarmDescription = "Alarm Description 1",
                    },
                },
                ExceptionData = new List<ExceptionData>()
                {
                    new ExceptionData()
                    {
                        Description = "Exception Description",
                    },
                },
                ParamStandardData = new List<ParamStandardData>()
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
                },
                RegisterData = new List<RegisterData>()
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
                },
                RodStrings = new List<RodStringData>()
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
                },
            });

            Assert.IsNotNull(result);

            Assert.AreEqual(2, result.StatusRegisters.Count);
            Assert.AreEqual(1, result.Alarms.Count);
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
            Assert.AreEqual("Alarm 1", result.Alarms[0].Label);
            Assert.AreEqual("Alarm Description 1", result.Alarms[0].Value);

            #endregion

            #region Test Exceptions

            Assert.IsNotNull(result.Exceptions[0]);
            Assert.AreEqual(string.Empty, result.Exceptions[0].Label);
            Assert.AreEqual("Exception Description", result.Exceptions[0].Value);

            #endregion

            #region Test Well Tests

            Assert.IsNotNull(result.LastWellTest[0]);
            Assert.AreEqual("Test date", result.LastWellTest[0].Label);
            Assert.AreEqual(lastWellTest.ToString(), result.LastWellTest[0].Value);

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
            Assert.AreEqual("333 ft 2", result.RodStrings[1].Value);

            #endregion

            #region Test Overlay Fields

            var item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.StrokeLength);

            Assert.IsNotNull(item);

            Assert.AreEqual("Stroke length", item.Label);
            Assert.AreEqual("20 ft", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.StrokeLength, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.LastGoodScan);

            Assert.IsNotNull(item);

            Assert.AreEqual("Last good scan", item.Label);
            Assert.AreEqual(lastGoodScanTime.ToString("M/d/yyyy, h:mm:ss tt"), item.Value);
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
            Assert.AreEqual("200 psi 2", item.Value);
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

        }

        [TestMethod]
        public void RodLiftAssetStatusDataContractMapListValuesNullTest()
        {
            var assetGUID = Guid.NewGuid();
            var lastGoodScanTime = new DateTime(2002, 1, 3, 2, 5, 4);
            var lastWellTest = new DateTime(2002, 1, 3, 2, 5, 4);
            var espTestResultDate = new DateTime(2002, 1, 1, 2, 5, 4);

            var result = AssetStatusMapper.Map(new RodLiftAssetStatusData()
            {
                CoreData = new RodLiftAssetStatusCoreData()
                {
                    APIDesignation = "Test",
                    NodeId = "Test Node",
                    AssetGUID = assetGUID,
                    GrossRate = LiquidFlowRate.FromBarrelsPerDay(100),
                    StrokeLength = Length.FromInches(20),
                    LastGoodScan = lastGoodScanTime,
                    RunStatus = "Running",
                    StrokesPerMinute = 8.5f,
                    TimeInState = 80,
                    TubingPressure = Pressure.FromPoundsPerSquareInch(200),
                    MotorTypeId = 4,
                    PumpFillage = 95,
                    StructuralLoading = 85,
                    LastWellTestDate = lastWellTest,
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
                    ESPResultTestDate = espTestResultDate,
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
                },
                AlarmData = new List<AlarmData>()
                {
                    null,
                },
                ExceptionData = new List<ExceptionData>()
                {
                    null,
                },
                ParamStandardData = new List<ParamStandardData>()
                {
                    null,
                },
                RegisterData = new List<RegisterData>()
                {
                    null,
                },
                RodStrings = new List<RodStringData>()
                {
                    null,
                },
            });

            Assert.IsNotNull(result);

            Assert.AreEqual(0, result.StatusRegisters.Count);
            Assert.AreEqual(0, result.Alarms.Count);
            Assert.AreEqual(0, result.Exceptions.Count);
            Assert.AreEqual(5, result.LastWellTest.Count);
            Assert.AreEqual(3, result.WellStatusOverview.Count);
            Assert.AreEqual(0, result.RodStrings.Count);
            Assert.AreEqual(36, result.ImageOverlayItems.Count);

            #region Test Well Tests

            Assert.IsNotNull(result.LastWellTest[0]);
            Assert.AreEqual("Test date", result.LastWellTest[0].Label);
            Assert.AreEqual(lastWellTest.ToString(), result.LastWellTest[0].Value);

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
            Assert.AreEqual("20 ft", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.StrokeLength, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.LastGoodScan);

            Assert.IsNotNull(item);

            Assert.AreEqual("Last good scan", item.Label);
            Assert.AreEqual(lastGoodScanTime.ToString("M/d/yyyy, h:mm:ss tt"), item.Value);
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
            Assert.AreEqual("200 psi 2", item.Value);
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

            Assert.IsTrue(item.Value == string.Empty);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.CyclesToday);

            Assert.IsTrue(item.Value == string.Empty);

            #endregion

        }

        [TestMethod]
        public void RodLiftAssetStatusDataContractMapCommunicationStatusTimeoutTest()
        {
            var assetGUID = Guid.NewGuid();
            var lastGoodScanTime = new DateTime(2002, 1, 3, 2, 5, 4);
            var lastWellTest = new DateTime(2002, 1, 3, 2, 5, 4);
            var espTestResultDate = new DateTime(2002, 1, 1, 2, 5, 4);

            var result = AssetStatusMapper.Map(new RodLiftAssetStatusData()
            {
                CoreData = new RodLiftAssetStatusCoreData()
                {
                    APIDesignation = "Test",
                    NodeId = "Test Node",
                    AssetGUID = assetGUID,
                    GrossRate = LiquidFlowRate.FromBarrelsPerDay(100),
                    StrokeLength = Length.FromInches(20),
                    LastGoodScan = lastGoodScanTime,
                    RunStatus = "Running",
                    StrokesPerMinute = 8.5f,
                    TimeInState = 80,
                    TubingPressure = Pressure.FromPoundsPerSquareInch(200),
                    MotorTypeId = 4,
                    PumpFillage = 95,
                    StructuralLoading = 85,
                    LastWellTestDate = lastWellTest,
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
                    CommunicationStatus = "Timeout",
                    CalculatedFluidLevelAbovePump = 105,
                    FirmwareVersion = 1.01f,
                    FluidLevel = Length.FromFeet(54),
                    GearBoxLoadPercentage = 10,
                    PlungerDiameter = Length.FromInches(47),
                    PumpDepth = Length.FromFeet(2000),
                    PumpEfficiency = 96,
                    ESPResultTestDate = espTestResultDate,
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
                },
                AlarmData = new List<AlarmData>()
                {
                    null,
                },
                ExceptionData = new List<ExceptionData>()
                {
                    null,
                },
                ParamStandardData = new List<ParamStandardData>()
                {
                    null,
                },
                RegisterData = new List<RegisterData>()
                {
                    null,
                },
                RodStrings = new List<RodStringData>()
                {
                    null,
                },
            });

            Assert.IsNotNull(result);

            Assert.AreEqual(0, result.StatusRegisters.Count);
            Assert.AreEqual(0, result.Alarms.Count);
            Assert.AreEqual(0, result.Exceptions.Count);
            Assert.AreEqual(5, result.LastWellTest.Count);
            Assert.AreEqual(3, result.WellStatusOverview.Count);
            Assert.AreEqual(0, result.RodStrings.Count);
            Assert.AreEqual(36, result.ImageOverlayItems.Count);

            #region Test Well Tests

            Assert.IsNotNull(result.LastWellTest[0]);
            Assert.AreEqual("Test date", result.LastWellTest[0].Label);
            Assert.AreEqual(lastWellTest.ToString(), result.LastWellTest[0].Value);

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
            Assert.AreEqual("20 ft", item.Value);
            Assert.AreEqual(DisplayState.Normal, item.DisplayState);
            Assert.AreEqual(OverlayFields.StrokeLength, item.OverlayField);
            Assert.IsTrue(item.IsVisible);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.LastGoodScan);

            Assert.IsNotNull(item);

            Assert.AreEqual("Last good scan", item.Label);
            Assert.AreEqual(lastGoodScanTime.ToString("M/d/yyyy, h:mm:ss tt"), item.Value);
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
            Assert.AreEqual("200 psi 2", item.Value);
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
            Assert.AreEqual("Timeout", item.Value);
            Assert.AreEqual(DisplayState.Warning, item.DisplayState);
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

            Assert.IsTrue(item.Value == string.Empty);

            item = result.ImageOverlayItems.FirstOrDefault(m => m.OverlayField == OverlayFields.CyclesToday);

            Assert.IsTrue(item.Value == string.Empty);

            #endregion

        }

        #endregion

    }
}