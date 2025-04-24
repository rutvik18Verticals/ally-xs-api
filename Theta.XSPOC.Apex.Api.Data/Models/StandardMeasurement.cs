using System;
using System.Collections.Generic;
using System.Reflection;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents the real-world definition of a parameter.
    /// </summary>
    public class StandardMeasurement
    {

        #region Static Fields

        private static readonly IDictionary<Type, IDictionary<int, StandardMeasurement>> _dictionary =
            new Dictionary<Type, IDictionary<int, StandardMeasurement>>();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unit category associated with this measurement.
        /// </summary>
        public UnitCategory UnitCategory { get; set; }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public int Key { get; protected set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public Text Name { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the value is supported.
        /// </summary>
        public bool IsSupported { get; protected set; } = true;

        #endregion

        #region Static Properties

        /// <summary>
        /// Specifies that the parameter does not represent a standard measurement.
        /// </summary>
        public static StandardMeasurement None { get; } =
            CreateValue(0, "None", UnitCategory.None);

        /// <summary>
        /// Gets the default value.
        /// </summary>
        public static StandardMeasurement Default { get; } = None;

        ///<summary>
        ///Specifies that the parameter represents Pump Intake Pressure
        ///</summary>
        public static StandardMeasurement PumpIntakePressure { get; } =
            CreateValue(1, "Pump Intake Pressure", UnitCategory.Pressure);

        ///<summary>
        ///Specifies that the parameter represents Frequency
        ///</summary>
        public static StandardMeasurement Frequency { get; } =
            CreateValue(2, "Frequency", UnitCategory.Frequency);

        ///<summary>
        ///Specifies that the parameter represents Current Phase A
        ///</summary>
        public static StandardMeasurement CurrentPhaseA { get; } =
            CreateValue(3, "Current Phase A", UnitCategory.Current);

        ///<summary>
        ///Specifies that the parameter represents Current Phase B
        ///</summary>
        public static StandardMeasurement CurrentPhaseB { get; } =
            CreateValue(4, "Current Phase B", UnitCategory.Current);

        ///<summary>
        ///Specifies that the parameter represents Current Phase C
        ///</summary> 
        public static StandardMeasurement CurrentPhaseC { get; } =
            CreateValue(5, "Current Phase C", UnitCategory.Current);

        ///<summary>
        ///Specifies that the parameter represents Voltage Phase AB
        ///</summary>
        public static StandardMeasurement VoltagePhaseAB { get; } =
            CreateValue(6, "Voltage Phase AB", UnitCategory.Voltage);

        ///<summary>
        ///Specifies that the parameter represents Voltage Phase BC
        ///</summary>
        public static StandardMeasurement VoltagePhaseBC { get; } =
            CreateValue(7, "Voltage Phase BC", UnitCategory.Voltage);

        ///<summary>
        ///Specifies that the parameter represents Voltage Phase CA
        ///</summary>
        public static StandardMeasurement VoltagePhaseCA { get; } =
            CreateValue(8, "Voltage Phase CA", UnitCategory.Voltage);

        ///<summary>
        ///Specifies that the parameter represents Tubing Pressure
        ///</summary>
        public static StandardMeasurement TubingPress { get; } =
            CreateValue(9, "Tubing Pressure", UnitCategory.Pressure);

        ///<summary>
        ///Specifies that the parameter represents Casing Pressure
        ///</summary>
        public static StandardMeasurement CasingPress { get; } =
            CreateValue(10, "Casing Pressure", UnitCategory.Pressure);

        ///<summary>
        ///Specifies that the parameter represents Meter Pressure
        ///</summary>
        public static StandardMeasurement MeterPressure { get; } =
            CreateValue(11, "Meter Pressure", UnitCategory.Pressure);

        ///<summary>
        ///Specifies that the parameter represents Flow Rate
        ///</summary>
        public static StandardMeasurement FlowRate { get; } =
            CreateValue(12, "Flow Rate", UnitCategory.FluidRate);

        /// <summary>
        /// Specifies that the parameter represents EFMTempBase 
        /// </summary>
        public static StandardMeasurement EFMTempBase { get; } =
            CreateValue(13, "EFMTempBase", UnitCategory.Temperature);

        ///<summary>
        ///Specifies that the parameter represents BTU Wet/Dry
        ///</summary>
        public static StandardMeasurement BTUWetDry { get; } =
            CreateValue(14, "BTU Wet/Dry", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Pipe Material
        ///</summary>
        public static StandardMeasurement PipeMaterial { get; } =
            CreateValue(15, "Pipe Material", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Orifice Material
        ///</summary>
        public static StandardMeasurement OrificeMaterial { get; } =
            CreateValue(16, "Orifice Material", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents AGA Method
        ///</summary>
        public static StandardMeasurement AGAMethod { get; } =
            CreateValue(17, "AGA Method", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Default Temperature
        ///</summary>
        public static StandardMeasurement DefaultTemp { get; } =
            CreateValue(18, "Default Temperature", UnitCategory.Temperature);

        ///<summary>
        ///Specifies that the parameter represents Contract Hour
        ///</summary>
        public static StandardMeasurement ContractHour { get; } =
            CreateValue(19, "Contract Hour", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents PipeSize
        ///</summary>
        public static StandardMeasurement PipeSize { get; } =
            CreateValue(20, "Pipe Size", UnitCategory.ShortLength);

        ///<summary>
        ///Specifies that the parameter represents Pipe Reference Temperature
        ///</summary>
        public static StandardMeasurement PipeRefTemp { get; } =
            CreateValue(21, "Pipe Reference Temperature", UnitCategory.Temperature);

        ///<summary>
        ///Specifies that the parameter represents Orifice Size
        ///</summary>
        public static StandardMeasurement OrificeSize { get; } =
            CreateValue(22, "Orifice Size", UnitCategory.ShortLength);

        ///<summary>
        ///Specifies that the parameter represents Orifice Reference Temperature
        ///</summary>
        public static StandardMeasurement OrificeRefTemp { get; } =
            CreateValue(23, "Orifice Reference Temperature", UnitCategory.Temperature);

        ///<summary>
        ///Specifies that the parameter represents Low Differential Pressure Cutoff
        ///</summary>
        public static StandardMeasurement LowDiffPressCutoff { get; } =
            CreateValue(24, "Low Differential Pressure Cutoff", UnitCategory.Pressure);

        ///<summary>
        ///Specifies that the parameter represents DiffPressHighOpLimit
        ///</summary>
        public static StandardMeasurement DiffPressHighOpLimit { get; } =
            CreateValue(25, "Differential Pressure High Operating Limit", UnitCategory.Pressure);

        ///<summary>
        ///Specifies that the parameter represents Static Pressure High Operating Limit
        ///</summary>
        public static StandardMeasurement StatPressHighOpLimit { get; } =
            CreateValue(26, "Static Pressure High Operating Limit", UnitCategory.Pressure);

        ///<summary>
        ///Specifies that the parameter represents Flow Temperature High Operating Limit
        ///</summary>
        public static StandardMeasurement FlowTempHighOpLimit { get; } =
            CreateValue(27, "Flow Temperature High Operating Limit", UnitCategory.Temperature);

        ///<summary>
        ///Specifies that the parameter represents Flow Temperature Low Operating Limit
        ///</summary>
        public static StandardMeasurement FlowTempLowOpLimit { get; } =
            CreateValue(28, "Flow Temperature Low Operating Limit", UnitCategory.Temperature);

        ///<summary>
        ///Specifies that the parameter represents Atmospheric Pressure
        ///</summary>
        public static StandardMeasurement AtmPress { get; } =
            CreateValue(29, "Atmospheric Pressure", UnitCategory.Pressure);

        ///<summary>
        ///Specifies that the parameter represents Effective Date
        ///</summary>
        public static StandardMeasurement EffectiveDate { get; } =
            CreateValue(30, "Effective Date", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Isentropic Coefficient
        ///</summary>
        public static StandardMeasurement IsentropicCoefficient { get; } =
            CreateValue(31, "IsentropicCoefficient", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents BTU Factor
        ///</summary>
        public static StandardMeasurement BTUFactor { get; } =
            CreateValue(32, "BTU Factor", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents EFMSpecificGravity 
        /// </summary>
        public static StandardMeasurement EFMSpecificGravity { get; } =
            CreateValue(33, "EFMSpecificGravity", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Absolute Viscosity
        ///</summary>
        public static StandardMeasurement AbsViscosity { get; } =
            CreateValue(34, "Absolute Viscosity", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Percent CO2
        ///</summary>
        public static StandardMeasurement PercentCO2 { get; } =
            CreateValue(35, "Percent CO2", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Percent N2
        ///</summary>
        public static StandardMeasurement PercentN2 { get; } =
            CreateValue(36, "Percent N2", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Percent C1
        ///</summary>
        public static StandardMeasurement PercentC1 { get; } =
            CreateValue(37, "Percent C1", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Percent C2
        ///</summary>
        public static StandardMeasurement PercentC2 { get; } =
            CreateValue(38, "Percent C2", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Percent C3
        ///</summary>
        public static StandardMeasurement PercentC3 { get; } =
            CreateValue(39, "Percent C3", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Percent C4i
        ///</summary>
        public static StandardMeasurement PercentC4i { get; } =
            CreateValue(40, "Percent C4i", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Percent C4n
        ///</summary>
        public static StandardMeasurement PercentC4n { get; } =
            CreateValue(41, "Percent C4n", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Percent C5i
        ///</summary>
        public static StandardMeasurement PercentC5i { get; } =
            CreateValue(42, "Percent C5i", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Percent C5n
        ///</summary>
        public static StandardMeasurement PercentC5n { get; } =
            CreateValue(43, "Percent C5n", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Percent C6
        ///</summary>
        public static StandardMeasurement PercentC6 { get; } =
            CreateValue(44, "Percent C6", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Percent C7
        ///</summary>
        public static StandardMeasurement PercentC7 { get; } =
            CreateValue(45, "Percent C7", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Percent C8
        ///</summary>
        public static StandardMeasurement PercentC8 { get; } =
            CreateValue(56, "Percent C8", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Percent C9
        ///</summary>
        public static StandardMeasurement PercentC9 { get; } =
            CreateValue(57, "Percent C9", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Percent C10
        ///</summary>
        public static StandardMeasurement PercentC10 { get; } =
            CreateValue(58, "Percent C10", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Percent O2
        ///</summary>
        public static StandardMeasurement PercentO2 { get; } =
            CreateValue(59, "Percent O2", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Percent H2O
        ///</summary>
        public static StandardMeasurement PercentH2O { get; } =
            CreateValue(60, "Percent H2O", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Percent H2S
        ///</summary>
        public static StandardMeasurement PercentH2S { get; } =
            CreateValue(61, "Percent H2S", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Percent HE2
        ///</summary>
        public static StandardMeasurement PercentHE2 { get; } =
            CreateValue(62, "Percent HE2", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Percent H2
        ///</summary>
        public static StandardMeasurement PercentH2 { get; } =
            CreateValue(63, "Percent H2", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Percent CO
        ///</summary>
        public static StandardMeasurement PercentCO { get; } =
            CreateValue(64, "Percent CO", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Percent Ar
        ///</summary>
        public static StandardMeasurement PercentAr { get; } =
            CreateValue(65, "Percent Ar", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents RTD Sensor
        ///</summary>
        public static StandardMeasurement RTDSensor { get; } =
            CreateValue(66, "RTD Sensor", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents EFMPressBase 
        /// </summary>
        public static StandardMeasurement EFMPressBase { get; } =
            CreateValue(67, "EFMPressBase", UnitCategory.Pressure);

        ///<summary>
        ///Specifies that the parameter represents Use Measured Temperature
        ///</summary>
        public static StandardMeasurement UseMeasuredTemp { get; } =
            CreateValue(68, "Use Measured Temperature", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Battery Voltage
        ///</summary>
        public static StandardMeasurement BatteryVoltage { get; } =
            CreateValue(69, "Battery Voltage", UnitCategory.Voltage);

        ///<summary>
        ///Specifies that the parameter represents Differential Pressure
        ///</summary>
        public static StandardMeasurement DiffPress { get; } =
            CreateValue(70, "Differential Pressure", UnitCategory.Pressure);

        ///<summary>
        ///Specifies that the parameter represents Flowing Temperature
        ///</summary>
        public static StandardMeasurement FlowingTemperature { get; } =
            CreateValue(71, "Flowing Temperature", UnitCategory.Temperature);

        ///<summary>
        ///Specifies that the parameter represents Yesterday's Contract Volume
        ///</summary>
        public static StandardMeasurement YestContractVolume { get; } =
            CreateValue(72, "Yesterday's Contract Volume", UnitCategory.FluidVolume);

        ///<summary>
        ///Specifies that the parameter represents Today's Contract Volume
        ///</summary>
        public static StandardMeasurement TodayContractVolume { get; } =
            CreateValue(73, "Today's Contract Volume", UnitCategory.FluidVolume);

        ///<summary>
        ///Specifies that the parameter represents Stroke Length
        ///</summary>
        public static StandardMeasurement StrokeLength { get; } =
            CreateValue(80, "Stroke Length", UnitCategory.ShortLength);

        ///<summary>
        ///Specifies that the parameter represents Pump Depth
        ///</summary>
        public static StandardMeasurement PumpDepth { get; } =
            CreateValue(81, "Pump Depth", UnitCategory.LongLength);

        ///<summary>
        ///Specifies that the parameter represents Plunger Diameter
        ///</summary>
        public static StandardMeasurement PlungerDiameter { get; } =
            CreateValue(82, "Plunger Diameter", UnitCategory.ShortLength);

        ///<summary>
        ///Specifies that the parameter represents Rotation
        ///</summary>
        public static StandardMeasurement Rotation { get; } =
            CreateValue(83, "Rotation", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Motor HP
        ///</summary>
        public static StandardMeasurement MotorHP { get; } =
            CreateValue(84, "Motor HP", UnitCategory.Power);

        ///<summary>
        ///Specifies that the parameter represents Is Tubing Anchored
        ///</summary>
        public static StandardMeasurement IsTubingAnchored { get; } =
            CreateValue(85, "Is Tubing Anchored", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Tubing Anchor Depth
        ///</summary>
        public static StandardMeasurement TubingAnchorDepth { get; } =
            CreateValue(86, "Tubing Anchor Depth", UnitCategory.LongLength);

        ///<summary>
        ///Specifies that the parameter represents Tubing Diameter
        ///</summary>
        public static StandardMeasurement TubingDiameter { get; } =
            CreateValue(87, "Tubing Diameter", UnitCategory.ShortLength);

        ///<summary>
        ///Specifies that the parameter represents Pumping Unit Type
        ///</summary>
        public static StandardMeasurement PumpingUnitType { get; } =
            CreateValue(88, "Pumping Unit Type", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Crank Radius
        ///</summary>
        public static StandardMeasurement CrankRadius { get; } =
            CreateValue(89, "Crank Radius", UnitCategory.ShortLength);

        ///<summary>
        ///Specifies that the parameter represents Dimension K
        ///</summary>
        public static StandardMeasurement DimensionK { get; } =
            CreateValue(90, "Dimension K", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Dimension C
        ///</summary>
        public static StandardMeasurement DimensionC { get; } =
            CreateValue(91, "Dimension C", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Dimension P
        ///</summary>
        public static StandardMeasurement DimensionP { get; } =
            CreateValue(92, "Dimension P", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Dimension A
        ///</summary>
        public static StandardMeasurement DimensionA { get; } =
            CreateValue(93, "Dimension A", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Dimension I
        ///</summary>
        public static StandardMeasurement DimensionI { get; } =
            CreateValue(94, "Dimension I", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 1 Type
        ///</summary>
        public static StandardMeasurement RodTaper1Type { get; } =
            CreateValue(95, "Rod Taper 1 Type", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 2 Type
        ///</summary>
        public static StandardMeasurement RodTaper2Type { get; } =
            CreateValue(96, "Rod Taper 2 Type", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 3 Type
        ///</summary>
        public static StandardMeasurement RodTaper3Type { get; } =
            CreateValue(97, "Rod Taper 3 Type", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 4 Type
        ///</summary>
        public static StandardMeasurement RodTaper4Type { get; } =
            CreateValue(98, "Rod Taper 4 Type", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 5 Type
        ///</summary>
        public static StandardMeasurement RodTaper5Type { get; } =
            CreateValue(99, "Rod Taper 5 Type", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 6 Type
        ///</summary>
        public static StandardMeasurement RodTaper6Type { get; } =
            CreateValue(100, "Rod Taper 6 Type", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 1 Intervals
        ///</summary>
        public static StandardMeasurement RodTaper1Intervals { get; } =
            CreateValue(101, "Rod Taper 1 Intervals", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 2 Intervals
        ///</summary>
        public static StandardMeasurement RodTaper2Intervals { get; } =
            CreateValue(102, "Rod Taper 2 Intervals", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 3 Intervals
        ///</summary>
        public static StandardMeasurement RodTaper3Intervals { get; } =
            CreateValue(103, "Rod Taper 3 Intervals", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 4 Intervals
        ///</summary>
        public static StandardMeasurement RodTaper4Intervals { get; } =
            CreateValue(104, "Rod Taper 4 Intervals", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 5 Intervals
        ///</summary>
        public static StandardMeasurement RodTaper5Intervals { get; } =
            CreateValue(105, "Rod Taper 5 Intervals", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 6 Intervals
        ///</summary>
        public static StandardMeasurement RodTaper6Intervals { get; } =
            CreateValue(106, "Rod Taper 6 Intervals", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 1 Diameter
        ///</summary>
        public static StandardMeasurement RodTaper1Diameter { get; } =
            CreateValue(107, "Rod Taper 1 Diameter", UnitCategory.ShortLength);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 2 Diameter
        ///</summary>
        public static StandardMeasurement RodTaper2Diameter { get; } =
            CreateValue(108, "Rod Taper 2 Diameter", UnitCategory.ShortLength);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 3 Diameter
        ///</summary>
        public static StandardMeasurement RodTaper3Diameter { get; } =
            CreateValue(109, "Rod Taper 3 Diameter", UnitCategory.ShortLength);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 4 Diameter
        ///</summary>
        public static StandardMeasurement RodTaper4Diameter { get; } =
            CreateValue(110, "Rod Taper 4 Diameter", UnitCategory.ShortLength);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 5 Diameter
        ///</summary>
        public static StandardMeasurement RodTaper5Diameter { get; } =
            CreateValue(111, "Rod Taper 5 Diameter", UnitCategory.ShortLength);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 6 Diameter
        ///</summary>
        public static StandardMeasurement RodTaper6Diameter { get; } =
            CreateValue(112, "Rod Taper 6 Diameter", UnitCategory.ShortLength);

        ///<summary>
        ///Specifies that the parameter represents "Rod Taper 1 Weight
        ///</summary>
        public static StandardMeasurement RodTaper1Weight { get; } =
            CreateValue(113, "Rod Taper 1 Weight", UnitCategory.Weight);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 2 Weight
        ///</summary>
        public static StandardMeasurement RodTaper2Weight { get; } =
            CreateValue(114, "Rod Taper 2 Weight", UnitCategory.Weight);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 3 Weight
        ///</summary>
        public static StandardMeasurement RodTaper3Weight { get; } =
            CreateValue(115, "Rod Taper 3 Weight", UnitCategory.Weight);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 4 Weight
        ///</summary>
        public static StandardMeasurement RodTaper4Weight { get; } =
            CreateValue(116, "Rod Taper 4 Weight", UnitCategory.Weight);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 5 Weight
        ///</summary>
        public static StandardMeasurement RodTaper5Weight { get; } =
            CreateValue(117, "Rod Taper 5 Weight", UnitCategory.Weight);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 6 Weight
        ///</summary>
        public static StandardMeasurement RodTaper6Weight { get; } =
            CreateValue(118, "Rod Taper 6 Weight", UnitCategory.Weight);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 1 Modulus
        ///</summary>
        public static StandardMeasurement RodTaper1Modulus { get; } =
            CreateValue(119, "Rod Taper 1 Modulus", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 2 Modulus
        ///</summary>
        public static StandardMeasurement RodTaper2Modulus { get; } =
            CreateValue(120, "Rod Taper 2 Modulus", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 3 Modulus
        ///</summary>
        public static StandardMeasurement RodTaper3Modulus { get; } =
            CreateValue(121, "Rod Taper 3 Modulus", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 4 Modulus
        ///</summary>
        public static StandardMeasurement RodTaper4Modulus { get; } =
            CreateValue(122, "Rod Taper 4 Modulus", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 5 Modulus
        ///</summary>
        public static StandardMeasurement RodTaper5Modulus { get; } =
            CreateValue(123, "Rod Taper 5 Modulus", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper 6 Modulus
        ///</summary>
        public static StandardMeasurement RodTaper6Modulus { get; } =
            CreateValue(124, "Rod Taper 6 Modulus", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Rod Taper Count
        ///</summary>
        public static StandardMeasurement RodTaperCount { get; } =
            CreateValue(125, "Rod Taper Count", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Pump Efficiency
        ///</summary>
        public static StandardMeasurement PumpEfficiency { get; } =
            CreateValue(126, "Pump Efficiency", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Speed 
        /// </summary>
        public static StandardMeasurement Speed { get; } =
            CreateValue(127, "Speed", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Torque 
        /// </summary>
        public static StandardMeasurement Torque { get; } =
            CreateValue(128, "Torque", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Today's Mass
        ///</summary>
        public static StandardMeasurement TodaysMass { get; } =
            CreateValue(129, "Today's Mass", UnitCategory.Weight);

        ///<summary>
        ///Specifies that the parameter represents Yesterday's Mass
        ///</summary>
        public static StandardMeasurement YesterdaysMass { get; } =
            CreateValue(130, "Yesterday's Mass", UnitCategory.Weight);

        ///<summary>
        ///Specifies that the parameter represents Accumulated Mass
        ///</summary>
        public static StandardMeasurement AccumulatedMass { get; } =
            CreateValue(131, "Accumulated Mass", UnitCategory.Weight);

        /// <summary>
        ///Specifies that the parameter represents pulse count
        /// </summary>
        public static StandardMeasurement PulseCount { get; } =
            CreateValue(132, "Pulse Count", UnitCategory.None);

        /// <summary>
        ///Specifies that the parameter represents UC Flow Rate
        /// </summary>
        public static StandardMeasurement UCFlowRate { get; } =
            CreateValue(133, "UC Flow Rate", UnitCategory.FluidVolume);

        /// <summary>
        ///Specifies that the parameter represents Todays UC Volume
        /// </summary>
        public static StandardMeasurement TodaysUCVolume { get; } =
            CreateValue(134, "Today's UC Volume", UnitCategory.FluidVolume);

        /// <summary>
        ///Specifies that the parameter represents Yesterdays UC Volume
        /// </summary>
        public static StandardMeasurement YesterdaysUCVolume { get; } =
            CreateValue(135, "Yesterday's UC Volume", UnitCategory.FluidVolume);

        /// <summary>
        ///Specifies that the parameter represents Accumulated Uc Volume
        /// </summary>
        public static StandardMeasurement AccumulatedUCVolume { get; } =
            CreateValue(136, "Accumulated UC Volume", UnitCategory.FluidVolume);

        /// <summary>
        ///Specifies that the parameter represents Solar Voltage
        /// </summary>
        public static StandardMeasurement SolarVoltage { get; } =
            CreateValue(137, "Solar Voltage", UnitCategory.Voltage);

        ///<summary>
        ///Specifies that the parameter represents Temperature
        ///</summary>
        public static StandardMeasurement Temperature { get; } =
            CreateValue(138, "Temperature", UnitCategory.Temperature);

        ///<summary>
        ///Specifies that the parameter represents Volume
        ///</summary>
        public static StandardMeasurement Volume { get; } =
            CreateValue(139, "Volume", UnitCategory.FluidVolume);

        ///<summary>
        ///Specifies that the parameter represents Interface Level
        ///</summary>
        public static StandardMeasurement InterfaceLevel { get; } =
            CreateValue(140, "Interface Level", UnitCategory.LongLength);

        ///<summary>
        ///Specifies that the parameter represents Tank Level
        ///</summary>
        public static StandardMeasurement TankLevel { get; } =
            CreateValue(141, "Tank Level", UnitCategory.LongLength);

        ///<summary>
        ///Specifies that the parameter represents Last Sample
        ///</summary>
        public static StandardMeasurement LastSample { get; } =
            CreateValue(142, "Last Sample", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Poll Count 
        /// </summary>
        public static StandardMeasurement PollCount { get; } =
            CreateValue(143, "Poll Count", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Error Count 
        /// </summary>
        public static StandardMeasurement ErrorCount { get; } =
            CreateValue(144, "Error Count", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Flowing Pressure
        ///</summary>
        public static StandardMeasurement FlowingPressure { get; } =
            CreateValue(145, "Flowing Pressure", UnitCategory.Pressure);

        /// <summary>
        ///Specifies that the parameter represents Accumulated Volume
        /// </summary>
        public static StandardMeasurement AccumulatedVolume { get; } =
            CreateValue(146, "Accumulated Volume", UnitCategory.FluidVolume);

        /// <summary>
        /// Specifies that the parameter represents Tubing Temperature.
        /// </summary>
        public static StandardMeasurement TubingTemperature { get; } =
            CreateValue(149, "Tubing Temperature", UnitCategory.Temperature);

        /// <summary>
        /// Specifies that the parameter represents Casing Temperature
        /// </summary>
        public static StandardMeasurement CasingTemperature { get; } =
            CreateValue(150, "Casing Temperature", UnitCategory.Temperature);

        /// <summary>
        /// Specifies that the parameter represents Setpoint Pressure.
        /// </summary>
        public static StandardMeasurement SetpointPressure { get; } =
            CreateValue(151, "Setpoint Pressure", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents Static Pressure 
        /// </summary>
        public static StandardMeasurement StaticPressure { get; } =
            CreateValue(152, "Static Pressure", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents Production Flow Rate.
        /// </summary>
        public static StandardMeasurement ProductionFlowRate { get; } =
            CreateValue(153, "Production Flow Rate", UnitCategory.GasRate);

        /// <summary>
        /// Specifies that the parameter represents Valve Percent Open.
        /// </summary>
        public static StandardMeasurement ValvePercentOpen { get; } =
            CreateValue(154, "Valve Percent Open", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Primary Process Variable 
        /// </summary>
        public static StandardMeasurement PrimaryProcessVariable { get; } =
            CreateValue(155, "Primary Process Variable", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Primary Set Point 
        /// </summary>
        public static StandardMeasurement PrimarySetPoint { get; } =
            CreateValue(156, "Primary Set Point", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Primary Deadband 
        /// </summary>
        public static StandardMeasurement PrimaryDeadband { get; } =
            CreateValue(157, "Primary Deadband", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Primary Proportional Gain 
        /// </summary>
        public static StandardMeasurement PrimaryProportionalGain { get; } =
            CreateValue(158, "Primary Proportional Gain", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Primary Integral 
        /// </summary>
        public static StandardMeasurement PrimaryIntegral { get; } =
            CreateValue(159, "Primary Integral", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Primary Derivative 
        /// </summary>
        public static StandardMeasurement PrimaryDerivative { get; } =
            CreateValue(160, "Primary Derivative", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Primary Scale Factor 
        /// </summary>
        public static StandardMeasurement PrimaryScaleFactor { get; } =
            CreateValue(161, "Primary Scale Factor", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Primary Output 
        /// </summary>
        public static StandardMeasurement PrimaryOutput { get; } =
            CreateValue(162, "Primary Output", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Override Process Variable 
        /// </summary>
        public static StandardMeasurement OverrideProcessVariable { get; } =
            CreateValue(163, "Override Process Variable", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Override Set Point 
        /// </summary>
        public static StandardMeasurement OverrideSetPoint { get; } =
            CreateValue(164, "Override Set Point", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Override Deadband 
        /// </summary>
        public static StandardMeasurement OverrideDeadband { get; } =
            CreateValue(165, "Override Deadband", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Override Proportional Gain 
        /// </summary>
        public static StandardMeasurement OverrideProportionalGain { get; } =
            CreateValue(166, "Override Proportional Gain", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Override Integral 
        /// </summary>
        public static StandardMeasurement OverrideIntegral { get; } =
            CreateValue(167, "Override Integral", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Override Derivative 
        /// </summary>
        public static StandardMeasurement OverrideDerivative { get; } =
            CreateValue(168, "Override Derivative", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Override Scale Factor 
        /// </summary>
        public static StandardMeasurement OverrideScaleFactor { get; } =
            CreateValue(169, "Override Scale Factor", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Override Output 
        /// </summary>
        public static StandardMeasurement OverrideOutput { get; } =
            CreateValue(170, "Override Output", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents EFMStaticPressureType 
        /// </summary>
        public static StandardMeasurement EFMStaticPressureType { get; } =
            CreateValue(171, "EFMStaticPressureType", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents EFMTubeID 
        /// </summary>
        public static StandardMeasurement EFMTubeID { get; } =
            CreateValue(172, "EFMTubeID", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents EFMVolumeCalculationType 
        /// </summary>
        public static StandardMeasurement EFMVolumeCalculationType { get; } =
            CreateValue(173, "EFMVolumeCalculationType", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Energy Rate
        ///</summary>
        public static StandardMeasurement EnergyRate { get; } =
            CreateValue(174, "Energy Rate", UnitCategory.Energy);

        ///<summary>
        ///Specifies that the parameter represents Today's Energy
        ///</summary>
        public static StandardMeasurement TodaysEnergy { get; } =
            CreateValue(175, "Today's Energy", UnitCategory.Energy);

        ///<summary>
        ///Specifies that the parameter represents Yesterday's Energy
        ///</summary>
        public static StandardMeasurement YesterdaysEnergy { get; } =
            CreateValue(176, "Yesterday's Energy", UnitCategory.Energy);

        ///<summary>
        ///Specifies that the parameter represents Last Calculated Period Volume
        ///</summary>
        public static StandardMeasurement LastCalcPeriodVol { get; } =
            CreateValue(177, "Last Calculated Period Volume", UnitCategory.GasVolume);

        /// <summary>
        /// Specifies that the parameter represents Yesterdays Runtime 
        /// </summary>
        public static StandardMeasurement YesterdaysRuntime { get; } =
            CreateValue(178, "Yesterdays Runtime", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Runtime
        ///</summary>
        public static StandardMeasurement Runtime { get; } =
            CreateValue(179, "Runtime", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Idle Time
        ///</summary>
        public static StandardMeasurement IdleTime { get; } =
            CreateValue(180, "Idle Time", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Cycles 
        /// </summary>
        public static StandardMeasurement Cycles { get; } =
            CreateValue(181, "Cycles", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Cycles Today
        ///</summary>
        public static StandardMeasurement CyclesToday { get; } =
            CreateValue(182, "Cycles Today", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents SPM
        ///</summary>
        public static StandardMeasurement SPM { get; } =
            CreateValue(183, "SPM", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Flowline Temperature
        ///</summary>
        public static StandardMeasurement FlowlineTemp { get; } =
            CreateValue(184, "Flowline Temperature", UnitCategory.Temperature);

        /// <summary>
        /// Specifies that the parameter represents Flowline Pressure.
        /// </summary>
        public static StandardMeasurement FlowlinePressure { get; } =
            CreateValue(185, "Flowline Pressure", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the Pump Discharge Pressure.
        /// </summary>
        public static StandardMeasurement PumpDischargePressure { get; } =
            CreateValue(186, "Pump Discharge Pressure", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the Motor Temperature
        /// </summary>
        public static StandardMeasurement MotorTemperature { get; } =
            CreateValue(187, "Motor Temperature", UnitCategory.Temperature);

        /// <summary>
        /// Specifies that the parameter represents Line Pressure to Setting 
        /// </summary>
        public static StandardMeasurement LinePressuretoSetting { get; } =
            CreateValue(188, "Line Pressure to Setting", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Pump Intake Temperature 
        /// </summary>
        public static StandardMeasurement PumpIntakeTemperature { get; } =
            CreateValue(189, "Pump Intake Temperature", UnitCategory.Temperature);

        ///<summary>
        ///Specifies that the parameter represents Gas Rate
        ///</summary>
        public static StandardMeasurement GasRate { get; } =
            CreateValue(190, "Gas Rate", UnitCategory.GasRate);

        /// <summary>
        /// Specifies that the parameter represents Gas Injection Rate
        /// </summary>
        public static StandardMeasurement GasInjectionRate { get; } =
            CreateValue(191, "Gas Injection Rate", UnitCategory.GasRate);

        /// <summary>
        /// Specifies that the parameter represents Injection Water Rate 
        /// </summary>
        public static StandardMeasurement InjectionWaterRate { get; } =
            CreateValue(192, "Injection Water Rate", UnitCategory.FluidRate);

        /// <summary>
        /// Specifies that the parameter represents Rod RPM 
        /// </summary>
        public static StandardMeasurement RodRPM { get; } =
            CreateValue(193, "Rod RPM", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Rod Torque 
        /// </summary>
        public static StandardMeasurement RodTorque { get; } =
            CreateValue(194, "Rod Torque", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents PCP Speed 
        /// </summary>
        public static StandardMeasurement PCPSpeed { get; } =
            CreateValue(195, "PCP Speed", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Injection Pressure.
        /// </summary>
        public static StandardMeasurement InjectionPressure { get; } =
            CreateValue(196, "Injection Pressure", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents Injection Volume Yesterday.
        /// </summary>
        public static StandardMeasurement InjectionVolumeYesterday { get; } =
            CreateValue(197, "Injection Volume Yesterday", UnitCategory.GasVolume);

        /// <summary>
        /// Specifies that the parameter represents Todays Gas Volume 
        /// </summary>
        public static StandardMeasurement TodaysGasVolume { get; } =
            CreateValue(198, "Todays Gas Volume", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Injection Pressure Setpoint 
        /// </summary>
        public static StandardMeasurement InjectionPressureSetpoint { get; } =
            CreateValue(199, "Injection Pressure Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Injection Rate Setpoint 
        /// </summary>
        public static StandardMeasurement InjectionRateSetpoint { get; } =
            CreateValue(200, "Injection Rate Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Supply Pressure 
        /// </summary>
        public static StandardMeasurement SupplyPressure { get; } =
            CreateValue(201, "Supply Pressure", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents Yesterdays Fluid Volume 
        /// </summary>
        public static StandardMeasurement YesterdaysFluidVolume { get; } =
            CreateValue(202, "Yesterdays Fluid Volume", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Yesterdays Inferred Production
        /// </summary>
        public static StandardMeasurement YesterdaysInferredProduction { get; } =
            CreateValue(203, "Yesterdays Inferred Production", UnitCategory.FluidVolume);

        /// <summary>
        /// Specifies that the parameter represents Todays Inferred Production 
        /// </summary>
        public static StandardMeasurement TodaysInferredProduction { get; } =
            CreateValue(204, "Todays Inferred Production", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents EFM Flow Time Today 
        /// </summary>
        public static StandardMeasurement EFMFlowTimeToday { get; } =
            CreateValue(205, "EFM Flow Time Today", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents EFM Pressure Extension 
        /// </summary>
        public static StandardMeasurement EFMPressureExtension { get; } =
            CreateValue(206, "EFM Pressure Extension", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Accumulator Runtime
        ///</summary>
        public static StandardMeasurement AccumulatorRuntime { get; } =
            CreateValue(207, "Accumulator Runtime", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Accumulator Starts
        ///</summary>
        public static StandardMeasurement AccumulatorStarts { get; } =
            CreateValue(208, "Accumulator Starts", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Bottomhole Temperature
        /// </summary>
        public static StandardMeasurement BottomholeTemperature { get; } =
            CreateValue(209, "Bottomhole Temperature", UnitCategory.Temperature);

        ///<summary>
        ///Specifies that the parameter represents Estimated Gross Production
        ///</summary>
        public static StandardMeasurement EstGrossProd { get; } =
            CreateValue(210, "Estimated Gross Production", UnitCategory.FluidVolume);

        ///<summary>
        ///Specifies that the parameter represents Fluid Level Above Pump
        ///</summary>
        public static StandardMeasurement FluidLevelAbovePump { get; } =
            CreateValue(211, "Fluid Level Above Pump", UnitCategory.LongLength);

        ///<summary>
        ///Specifies that the parameter represents Min Load
        ///</summary>
        public static StandardMeasurement MinLoad { get; } =
            CreateValue(212, "Min Load", UnitCategory.Weight);

        ///<summary>
        ///Specifies that the parameter represents Oil API Gravity
        ///</summary>
        public static StandardMeasurement OilAPIGravity { get; } =
            CreateValue(213, "Oil API Gravity", UnitCategory.OilRelativeDensity);

        ///<summary>
        ///Specifies that the parameter represents On/Off Cycles
        ///</summary>
        public static StandardMeasurement OnOffCycles { get; } =
            CreateValue(214, "On/Off Cycles", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Peak Load
        ///</summary>
        public static StandardMeasurement PeakLoad { get; } =
            CreateValue(215, "Peak Load", UnitCategory.Weight);

        ///<summary>
        ///Specifies that the parameter represents Pump Size
        ///</summary>
        public static StandardMeasurement PumpSize { get; } =
            CreateValue(216, "Pump Size", UnitCategory.ShortLength);

        ///<summary>
        ///Specifies that the parameter represents Sample Period
        ///</summary>
        public static StandardMeasurement SamplePeriod { get; } =
            CreateValue(217, "Sample Period", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents Water Rate
        ///</summary>
        public static StandardMeasurement WaterRate { get; } =
            CreateValue(218, "Water Rate", UnitCategory.FluidRate);

        ///<summary>
        ///Specifies that the parameter represents Water Specific Gravity
        ///</summary>
        public static StandardMeasurement WaterSpecificGravity { get; } =
            CreateValue(219, "Water Specific Gravity", UnitCategory.RelativeDensity);

        ///<summary>
        ///Specifies that the parameter represents KWH
        ///</summary>
        public static StandardMeasurement KWH { get; } =
            CreateValue(220, "KWH", UnitCategory.Energy);

        ///<summary>
        ///Specifies that the parameter represents MWH
        ///</summary>
        public static StandardMeasurement MWH { get; } =
            CreateValue(221, "MWH", UnitCategory.Energy);

        /// <summary>
        /// Specifies that the parameter represents Gas Specific Gravity
        /// </summary>
        public static StandardMeasurement SpecificGravityOfGas { get; } =
            CreateValue(222, "Gas Specific Gravity", UnitCategory.RelativeDensity);

        ///<summary>
        ///Specifies that the parameter represents Oil Rate
        ///</summary>
        public static StandardMeasurement OilRate { get; } =
            CreateValue(223, "Oil Rate", UnitCategory.FluidRate);

        /// <summary>
        /// Specifies that the parameter represents Runtime Today Percent 
        /// </summary>
        public static StandardMeasurement RuntimeTodayPercent { get; } =
            CreateValue(224, "Runtime Today Percent", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Runtime Yesterday Percent 
        /// </summary>
        public static StandardMeasurement RuntimeYesterdayPercent { get; } =
            CreateValue(225, "Runtime Yesterday Percent", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Runtime Today Hours 
        /// </summary>
        public static StandardMeasurement RuntimeTodayHours { get; } =
            CreateValue(226, "Runtime Today Hours", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Runtime Yesterday Hours 
        /// </summary>
        public static StandardMeasurement RuntimeYesterdayHours { get; } =
            CreateValue(227, "Runtime Yesterday Hours", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Well Test Duration
        /// </summary>
        public static StandardMeasurement Duration { get; } =
            CreateValue(228, "Well Test Duration", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Flowing Bottomhole Pressure.
        /// </summary>
        public static StandardMeasurement FlowingBottomholePressure { get; } =
            CreateValue(229, "Flowing Bottomhole Pressure", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents Motor Current.
        /// </summary>
        public static StandardMeasurement MotorCurrent { get; } =
            CreateValue(230, "Motor Current", UnitCategory.Current);

        ///<summary>
        ///Specifies that the parameter represents the Meter Name
        ///</summary>
        public static StandardMeasurement MeterName { get; } =
            CreateValue(231, "Meter Name", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents the Comm Pct
        ///</summary>
        public static StandardMeasurement CommPercentage { get; } =
            CreateValue(235, "Comm Pct", UnitCategory.None);

        ///<summary>
        ///Specifies that the parameter represents the Downhole Pressure Gage
        ///</summary>
        public static StandardMeasurement DownholeGagePressure { get; } =
            CreateValue(238, "Downhole Gage Pressure", UnitCategory.Pressure);

        /// <summary>
        /// Specifics that the parameter represents the Bubblepoint Pressure.
        /// </summary>
        public static StandardMeasurement BubblepointPressure { get; } =
            CreateValue(239, "Bubblepoint Pressure", UnitCategory.Pressure);

        ///<summary>
        ///Specifies that the parameter represents the Fluid Level From Surface
        ///</summary>
        public static StandardMeasurement FluidLevelFromSurface { get; } =
            CreateValue(240, "Fluid Level From Surface", UnitCategory.LongLength);

        /// <summary>
        /// Specifies that the parameter represents Load Span 
        /// </summary>
        public static StandardMeasurement LoadSpan { get; } =
            CreateValue(241, "Load Span", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents Volume Gas Today Injection 
        /// </summary>
        public static StandardMeasurement VolumeGasTodayInjection { get; } =
            CreateValue(242, "Volume Gas Today Injection", UnitCategory.Voltage);

        /// <summary>
        /// Specifies that the parameter represents Volume Gas Yest Injection 
        /// </summary>
        public static StandardMeasurement VolumeGasYestInjection { get; } =
            CreateValue(243, "Volume Gas Yest Injection", UnitCategory.Voltage);

        /// <summary>
        /// Specifies that the parameter represents Volume Gas Rate Prod 
        /// </summary>
        public static StandardMeasurement VolumeGasRateProd { get; } =
            CreateValue(244, "Volume Gas Rate Prod", UnitCategory.GasRate);

        /// <summary>
        /// Specifies that the parameter represents Volume Gas Today Prod 
        /// </summary>
        public static StandardMeasurement VolumeGasTodayProd { get; } =
            CreateValue(245, "Volume Gas Today Prod", UnitCategory.Voltage);

        /// <summary>
        /// Specifies that the parameter represents Volume Gas Yest Prod 
        /// </summary>
        public static StandardMeasurement VolumeGasYestProd { get; } =
            CreateValue(246, "Volume Gas Yest Prod", UnitCategory.Voltage);

        /// <summary>
        /// Specifies that the parameter represents Volume Oil Today 
        /// </summary>
        public static StandardMeasurement VolumeOilToday { get; } =
            CreateValue(247, "Volume Oil Today", UnitCategory.FluidRate);

        /// <summary>
        /// Specifies that the parameter represents Volume Oil Yest 
        /// </summary>
        public static StandardMeasurement VolumeOilYest { get; } =
            CreateValue(248, "Volume Oil Yest", UnitCategory.FluidRate);

        /// <summary>
        /// Specifies that the parameter represents Volume Water Today 
        /// </summary>
        public static StandardMeasurement VolumeWaterToday { get; } =
            CreateValue(249, "Volume Water Today", UnitCategory.FluidRate);

        /// <summary>
        /// Specifies that the parameter represents Volume Water Yest 
        /// </summary>
        public static StandardMeasurement VolumeWaterYest { get; } =
            CreateValue(250, "Volume Water Yest", UnitCategory.FluidRate);

        /// <summary>
        /// Specifies that the parameter represents Tank Lvl 1 
        /// </summary>
        public static StandardMeasurement TankLvl1 { get; } =
            CreateValue(251, "Tank Lvl 1", UnitCategory.ShortLength);

        /// <summary>
        /// Specifies that the parameter represents Tank Lvl 2 
        /// </summary>
        public static StandardMeasurement TankLvl2 { get; } =
            CreateValue(252, "Tank Lvl 2", UnitCategory.ShortLength);

        /// <summary>
        /// Specifies that the parameter represents Tank Lvl 3 
        /// </summary>
        public static StandardMeasurement TankLvl3 { get; } =
            CreateValue(253, "Tank Lvl 3", UnitCategory.ShortLength);

        /// <summary>
        /// Specifies that the parameter represents Tank Lvl 4 
        /// </summary>
        public static StandardMeasurement TankLvl4 { get; } =
            CreateValue(254, "Tank Lvl 4", UnitCategory.ShortLength);

        /// <summary>
        /// Specifies that the parameter represents P1 Status 
        /// </summary>
        public static StandardMeasurement P1Status { get; } =
            CreateValue(255, "P1 Status", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents P2 Status 
        /// </summary>
        public static StandardMeasurement P2Status { get; } =
            CreateValue(256, "P2 Status", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents P1 Today Bbls 
        /// </summary>
        public static StandardMeasurement P1TodayBbls { get; } =
            CreateValue(257, "P1 Today Bbls", UnitCategory.FluidRate);

        /// <summary>
        /// Specifies that the parameter represents P2 Today Bbls 
        /// </summary>
        public static StandardMeasurement P2TodayBbls { get; } =
            CreateValue(258, "P2 Today Bbls", UnitCategory.FluidRate);

        /// <summary>
        /// Specifies that the parameter represents P1 Yest Bbls 
        /// </summary>
        public static StandardMeasurement P1YestBbls { get; } =
            CreateValue(259, "P1 Yest Bbls", UnitCategory.FluidRate);

        /// <summary>
        /// Specifies that the parameter represents P2 Yest Bbls 
        /// </summary>
        public static StandardMeasurement P2YestBbls { get; } =
            CreateValue(260, "P2 Yest Bbls", UnitCategory.FluidRate);

        /// <summary>
        /// Specifies that the parameter represents Tank Lvl 1 H2O 
        /// </summary>
        public static StandardMeasurement TankLvl1H2O { get; } =
            CreateValue(261, "Tank Lvl 1 H2O", UnitCategory.ShortLength);

        /// <summary>
        /// Specifies that the parameter represents Tank Lvl 2 H2O 
        /// </summary>
        public static StandardMeasurement TankLvl2H2O { get; } =
            CreateValue(262, "Tank Lvl 2 H2O", UnitCategory.ShortLength);

        /// <summary>
        /// Specifies that the parameter represents P3 Today Bbls 
        /// </summary>
        public static StandardMeasurement P3TodayBbls { get; } =
            CreateValue(263, "P3 Today Bbls", UnitCategory.FluidRate);

        /// <summary>
        /// Specifies that the parameter represents P3 Yest Bbls 
        /// </summary>
        public static StandardMeasurement P3YestBbls { get; } =
            CreateValue(264, "P3 Yest Bbls", UnitCategory.FluidRate);

        /// <summary>
        /// Specifies that the parameter represents BSW 
        /// </summary>
        public static StandardMeasurement BSW { get; } =
            CreateValue(265, "BSW", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Pump Fillage
        /// </summary>
        public static StandardMeasurement PumpFillage { get; } =
            CreateValue(266, "Pump Fillage", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the last stroke minimum load.
        /// </summary>
        public static StandardMeasurement MinLoadLastStroke { get; } =
            CreateValue(267, "Min Load Last Stroke", UnitCategory.Weight);

        /// <summary>
        /// Specifies that the parameter represents the last stroke peak load.
        /// </summary>
        public static StandardMeasurement PeakLoadLastStroke { get; } =
            CreateValue(268, "Peak Load Last Stroke", UnitCategory.Weight);

        /// <summary>
        /// Specifies that the parameter represents the Percent Base Fill Setpoint.
        /// </summary>
        public static StandardMeasurement PercentBaseFillSetpoint { get; } =
            CreateValue(269, "Percent Base Fill Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Percent Pump Fillage Setpoint.
        /// </summary>
        public static StandardMeasurement PercentPumpFillageSetpoint { get; } =
            CreateValue(270, "Percent Pump Fillage Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the hour idle time setpoint 
        /// for devices that stores the hour separately.
        /// </summary>
        public static StandardMeasurement IdleTimeHoursSetpoint { get; } =
            CreateValue(271, "Idle Time Hours Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the minutes idle time setpoint 
        /// for devices that stores the hour separately.
        /// </summary>
        public static StandardMeasurement IdleTimeMinutesSetpoint { get; } =
            CreateValue(272, "Idle Time Minutes Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Motor Current Downhole
        /// </summary>
        public static StandardMeasurement MotorCurrentDownhole { get; } =
            CreateValue(273, "Motor Current - Downhole", UnitCategory.Current);

        /// <summary>
        /// Specifies that the parameter represents the idle time setpoint for devices storing hours and minutes 
        /// together.
        /// </summary>
        public static StandardMeasurement IdleTimeSingleSetpoint { get; } =
            CreateValue(274, "Idle Time Single Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the hour run time as a percentage
        /// </summary>
        public static StandardMeasurement RunTimePercentage { get; } =
            CreateValue(275, "Run Time Percentage", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the minimum load setpoint.
        /// </summary>
        public static StandardMeasurement MinimumLoadSetpoint { get; } =
            CreateValue(276, "Minimum Load Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the peak load setpoint.
        /// </summary>
        public static StandardMeasurement PeakLoadSetpoint { get; } =
            CreateValue(277, "Peak Load Setpoint", UnitCategory.Weight);

        /// <summary>
        /// Specifies that the parameter represents the VFD ( variable frequency drive ) speed output.
        /// </summary>
        public static StandardMeasurement VFDSpeedOutput { get; } =
            CreateValue(280, "VFDSpeedOutput", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the pump fillage setpoint deadband.
        /// </summary>
        public static StandardMeasurement PumpFillageSetpointDeadband { get; } =
            CreateValue(281, "PumpFillageSetpointDeadband", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the speed change stroke delay.
        /// </summary>
        public static StandardMeasurement SpeedChangeStrokeDelay { get; } =
            CreateValue(282, "SpeedChangeStrokeDelay", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the start up speed.
        /// </summary>
        public static StandardMeasurement StartUpSpeed { get; } =
            CreateValue(283, "StartUpSpeed", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the speed increase size.
        /// </summary>
        public static StandardMeasurement SpeedIncreaseSize { get; } =
            CreateValue(284, "SpeedIncreaseSize", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the speed decrease size.
        /// </summary>
        public static StandardMeasurement SpeedDecreaseSize { get; } =
            CreateValue(285, "SpeedDecreaseSize", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the maximum scaling.
        /// </summary>
        public static StandardMeasurement MaxScaling { get; } =
            CreateValue(286, "MaxScaling", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the minimum scaling.
        /// </summary>
        public static StandardMeasurement MinScaling { get; } =
            CreateValue(287, "MinScaling", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the peak speed.
        /// </summary>
        public static StandardMeasurement PeakSpeed { get; } =
            CreateValue(288, "PeakSpeed", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the low speed.
        /// </summary>
        public static StandardMeasurement LowSpeed { get; } =
            CreateValue(289, "LowSpeed", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the slow speed.
        /// </summary>
        public static StandardMeasurement SlowSpeed { get; } =
            CreateValue(290, "SlowSpeed", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the maximum speed.
        /// </summary>
        public static StandardMeasurement MaxSpeed { get; } =
            CreateValue(291, "MaxSpeed", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the minimum speed.
        /// </summary>
        public static StandardMeasurement MinSpeed { get; } =
            CreateValue(292, "MinSpeed", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the reference pump fillage setpoint.
        /// </summary>
        public static StandardMeasurement ReferencePumpFillageSetpoint { get; } =
            CreateValue(293, "ReferencePumpFillageSetpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the secondary pump fillage setpoint.
        /// </summary>
        public static StandardMeasurement SecondaryPumpFillageSetpoint { get; } =
            CreateValue(294, "SecondaryPumpFillageSetpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the low speed time setpoint.
        /// </summary>
        public static StandardMeasurement LowSpeedTimeSetpoint { get; } =
            CreateValue(295, "LowSpeedTimeSetpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the slow speed time setpoint.
        /// </summary>
        public static StandardMeasurement SlowSpeedTimeSetpoint { get; } =
            CreateValue(296, "SlowSpeedTimeSetpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents if the VFD idling is enabled.
        /// </summary>
        public static StandardMeasurement VFDIdlingEnabled { get; } =
            CreateValue(297, "VFDIdlingEnabled", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the speed change stroke delay.
        /// </summary>
        public static StandardMeasurement ConsecutivePumpoffStrokesAllowed { get; } =
            CreateValue(298, "ConsecutivePumpoffStrokesAllowed", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the minimum pump strokes.
        /// </summary>
        public static StandardMeasurement ArrivalTime { get; } =
            CreateValue(301, "Arrival Time", UnitCategory.None);

        /// <summary> 
        /// Specifies that the parameter represents Arrival Velocity. 
        /// </summary>
        public static StandardMeasurement ArrivalVelocity { get; } =
            CreateValue(302, "Arrival Velocity", UnitCategory.None);

        /// <summary> 
        /// Specifies that the parameter represents Arrivals Today. 
        /// </summary>
        public static StandardMeasurement ArrivalsToday { get; } =
            CreateValue(303, "Arrivals Today", UnitCategory.None);

        /// <summary> 
        /// Specifies that the parameter represents Arrivals Yesterday. 
        /// </summary>
        public static StandardMeasurement ArrivalsYesterday { get; } =
            CreateValue(304, "Arrivals Yesterday", UnitCategory.None);

        /// <summary> 
        /// Specifies that the parameter represents Load Factor. 
        /// </summary>
        public static StandardMeasurement LoadFactor { get; } =
            CreateValue(305, "Load Factor", UnitCategory.None);

        /// <summary> 
        /// Specifies that the parameter represents Critical Flow Rate. 
        /// </summary>
        public static StandardMeasurement CriticalFlowRate { get; } =
            CreateValue(306, "Critical Flow Rate", UnitCategory.GasRate);

        /// <summary>
        /// Specifies that the parameter represents the minimum pump strokes.
        /// </summary>
        public static StandardMeasurement MinimumPumpStrokes { get; } =
            CreateValue(299, "MinimumPumpStrokes", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the enable/disable status of the malfunction setpoint.
        /// </summary>
        public static StandardMeasurement MalfunctionSetpointEnabled { get; } =
            CreateValue(307, "MalFunction Setpoint Enabled", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Position of the malfunction setpoint.
        /// </summary>
        public static StandardMeasurement MalfunctionSetpointPosition { get; } =
            CreateValue(308, "MalFunction Setpoint Position", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Load of the malfunction setpoint.
        /// </summary>
        public static StandardMeasurement MalfunctionSetpointLoad { get; } =
            CreateValue(309, "MalFunction Setpoint Load", UnitCategory.Weight);

        /// <summary>
        /// Specifies that the parameter represents the 
        /// VFD (variable frequency drive) startup duration.
        /// </summary>
        public static StandardMeasurement VFDStartupDuration { get; } =
            CreateValue(310, "VFDStartupDuration", UnitCategory.None);

        /// <summary> 
        /// Specifies that the parameter represents the Chemical Injection Rate Setpoint (qpd).
        /// </summary>
        public static StandardMeasurement ChemicalInjectionRateSetpointQPD { get; } =
            CreateValue(311, "Chemical Injection Rate Setpoint (qpd)", UnitCategory.None);

        /// <summary> 
        /// Specifies that the parameter represents the Plunger Cycle Time. 
        /// </summary>
        public static StandardMeasurement PlungerCycleTime { get; } =
            CreateValue(312, "Plunger Cycle Time", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Max Wait Preset Mins
        /// </summary>
        public static StandardMeasurement MaxWaitPresetMins { get; } =
            CreateValue(1050, "Max Wait Preset Mins", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Max Wait Elapsed Mins
        /// </summary>
        public static StandardMeasurement MaxWaitElapsedMins { get; } =
            CreateValue(1051, "Max Wait Elapsed Mins", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Max Wait Elapsed Secs
        /// </summary>
        public static StandardMeasurement MaxWaitElapsedSecs { get; } =
            CreateValue(1052, "Max Wait Elapsed Secs", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Min Aft Flw Preset Mins
        /// </summary>
        public static StandardMeasurement MinAftFlwPresetMins { get; } =
            CreateValue(1053, "Min Aft Flw Preset Mins", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Min Aft Flw Elapsed Mins
        /// </summary>
        public static StandardMeasurement MinAftFlwElapsedMins { get; } =
            CreateValue(1054, "Min Aft Flw Elapsed Mins", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Min Aft Flw Elapsed Secs
        /// </summary>
        public static StandardMeasurement MinAftFlwElapsedSecs { get; } =
            CreateValue(1055, "Min Aft Flw Elapsed Secs", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Max Aft Flw Mins
        /// </summary>
        public static StandardMeasurement MaxAftFlwMins { get; } =
            CreateValue(1056, "Max Aft Flw Mins", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Max Aft Flw Hrs
        /// </summary>
        public static StandardMeasurement MaxAftFlwHrs { get; } =
            CreateValue(1057, "Max Aft Flw Hrs", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Trigger Csg Less Than
        /// </summary>
        public static StandardMeasurement CloseTriggerCsgLessThan { get; } =
            CreateValue(1058, "Cl Trgr Csg Less Than", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the Close Trigger Tbg Less Than
        /// </summary>
        public static StandardMeasurement CloseTriggerTbgLessThan { get; } =
            CreateValue(1059, "Cl Trgr Tbg Less Than", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the Close Trigger CaMinTb GreaterThan
        /// </summary>
        public static StandardMeasurement CloseTriggerCaMinTbGreaterThan { get; } =
            CreateValue(1060, "Cl Trgr CaMinTb Greater Than", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Trigger TbMinLn Less Than
        /// </summary>
        public static StandardMeasurement CloseTriggerTbMinLnLessThan { get; } =
            CreateValue(1061, "Cl Trgr TbMinLn Less Than", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Trigger CaMinLn LessThan
        /// </summary>
        public static StandardMeasurement CloseTriggerCaMinLnLessThan { get; } =
            CreateValue(1062, "Cl Trgr CaMinLn Less Than", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Trigger Flow Less Than
        /// </summary>
        public static StandardMeasurement CloseTriggerFlowLessThan { get; } =
            CreateValue(1063, "Cl Trgr Flow Less Than", UnitCategory.GasRate);

        /// <summary>
        /// Specifies that the parameter represents the Plunger Drop Minutes
        /// </summary>
        public static StandardMeasurement PlungerDropMinutes { get; } =
            CreateValue(1064, "Plunger Drop Mins", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Max NonArrvl ShutIn Time
        /// </summary>
        public static StandardMeasurement MaxNonArrvlShutInTime { get; } =
            CreateValue(1065, "Max NonArrvl ShutIn Time", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Flow DP Set Point
        /// </summary>
        public static StandardMeasurement FlowDPSetPoint { get; } =
            CreateValue(1066, "Flow DP Set Point", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the PlungerLift Advance To
        /// </summary>
        public static StandardMeasurement PlungerLiftAdvanceTo { get; } =
            CreateValue(1067, "PlungerLift Advance To", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Trigger Csg LessThan New Value
        /// </summary>
        public static StandardMeasurement CloseTriggerCsgLessThanNewValue { get; } =
            CreateValue(1068, "Cl Trgr Csg Lt Val", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Trigger Tbg LessThan New Value
        /// </summary>
        public static StandardMeasurement CloseTriggerTbgLessThanNewValue { get; } =
            CreateValue(1069, "Cl Trgr Tbg Lt Val", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Trigger CaMinTb GreaterThan New Value
        /// </summary>
        public static StandardMeasurement CloseTriggerCaMinTbGreaterThanNewValue { get; } =
            CreateValue(1070, "Cl Trgr CaMinTb Gt Val", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Trigger TbMinLn LessThan New Value
        /// </summary>
        public static StandardMeasurement CloseTriggerTbMinLnLessThanNewValue { get; } =
            CreateValue(1071, "Cl Trgr TbMinLn Lt Val", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Trigger CaMinLn LessThan New Value
        /// </summary>
        public static StandardMeasurement CloseTriggerCaMinLnLessThanNewValue { get; } =
            CreateValue(1072, "Cl Trgr CaMinLn Lt Val", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Trigger Flow LessThan New Value
        /// </summary>
        public static StandardMeasurement CloseTriggerFlowLessThanNewValue { get; } =
            CreateValue(1073, "Cl Trgr Flow Lt Val", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Cycling Mode Manual Mode
        /// </summary>
        public static StandardMeasurement CyclingModeManualModeState { get; } =
            CreateValue(1074, "Cycle Mode Manual Mode", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Trigger Shut Time GreaterThan
        /// </summary>
        public static StandardMeasurement OpenTriggerShutTimeGreaterThan { get; } =
            CreateValue(1075, "OpnTrgrShutTime Gt", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Trigger Csg GreaterThan
        /// </summary>
        public static StandardMeasurement OpenTriggerCsgGreaterThan { get; } =
            CreateValue(1076, "OpnTrgrCsg Gt", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the Open Trigger Tbg GreaterThan
        /// </summary>
        public static StandardMeasurement OpenTriggerTbgGreaterThan { get; } =
            CreateValue(1077, "OpnTrgrTbg Gt", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the Open Trigger CsminTb LessThan
        /// </summary>
        public static StandardMeasurement OpenTriggerCsminTbLessThan { get; } =
            CreateValue(1078, "OpnTrgrCsminTb Lt", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Trigger TbMinLn GreaterThan
        /// </summary>
        public static StandardMeasurement OpenTriggerTbMinLnGreaterThan { get; } =
            CreateValue(1079, "OpnTrgrTbMinLn Gt", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Trigger CsMinLn GreaterThan
        /// </summary>
        public static StandardMeasurement OpenTriggerCsMinLnGreaterThan { get; } =
            CreateValue(1080, "OpnTrgrCsMinLn Gt", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Trigger LoadF LessThan
        /// </summary>
        public static StandardMeasurement OpenTriggerLoadFLessThan { get; } =
            CreateValue(1081, "OpnTrgrLoadF Lt", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Trigger And Selected Triggers
        /// </summary>
        public static StandardMeasurement OpenTriggerAndSelectedTriggers { get; } =
            CreateValue(1082, "OpnTrgr And Slctd Trgr", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Trigger Shut Time GreaterThan New Value
        /// </summary>
        public static StandardMeasurement OpenTriggerShutTimeGreaterThanNewValue { get; } =
            CreateValue(1083, "OpnTrgrShutTime Gt Val", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Trigger Csg GreaterThan New Value
        /// </summary>
        public static StandardMeasurement OpenTriggerCsgGreaterThanNewValue { get; } =
            CreateValue(1084, "OpnTrgrCsg Gt Val", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Trigger Tbg GreaterThan New Value
        /// </summary>
        public static StandardMeasurement OpenTriggerTbgGreaterThanNewValue { get; } =
            CreateValue(1085, "OpnTrgrTbg Gt Val", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the OpenTriggerCsminTbLessThanNewValue
        /// </summary>
        public static StandardMeasurement OpenTriggerCsminTbLessThanNewValue { get; } =
            CreateValue(1086, "OpnTrgrCsminTb Lt Val", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Trigger TbMinLn GreaterThan NewValue
        /// </summary>
        public static StandardMeasurement OpenTriggerTbMinLnGreaterThanNewValue { get; } =
            CreateValue(1087, "OpnTrgrTbMinLn Gt Val", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Trigger CsMinLn GreaterThan New Value
        /// </summary>
        public static StandardMeasurement OpenTriggerCsMinLnGreaterThanNewValue { get; } =
            CreateValue(1088, "OpnTrgrCsMinLn Gt Val", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Trigger LoadF LessThan New Value
        /// </summary>
        public static StandardMeasurement OpenTriggerLoadFLessThanNewValue { get; } =
            CreateValue(1089, "OpnTrgrLoadFLn Val", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Cycles Tdy
        /// </summary>
        public static StandardMeasurement CyclesTdy { get; } =
            CreateValue(1091, "Cycls Tdy", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Cycles Ydy
        /// </summary>
        public static StandardMeasurement CyclesYdy { get; } =
            CreateValue(1092, "Cycls Ydy", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Cycles Tot
        /// </summary>
        public static StandardMeasurement CyclesTot { get; } =
            CreateValue(1093, "Cycls Tot", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Cycles Tdy Arrivals
        /// </summary>
        public static StandardMeasurement CyclesTdyArrivals { get; } =
            CreateValue(1094, "Cycls Tdy Arvls", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Cycles Ydy Arrivals
        /// </summary>
        public static StandardMeasurement CyclesYdyArrivals { get; } =
            CreateValue(1095, "Cycls Ydy Arvls", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Cycles Tot Arrivals
        /// </summary>
        public static StandardMeasurement CyclesTotArrivals { get; } =
            CreateValue(1096, "Cycls Tot Arvls", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Cycles Tdy Non Arrivals
        /// </summary>
        public static StandardMeasurement CyclesTdyNonArrivals { get; } =
            CreateValue(1097, "Cycls Tdy NonArvls", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Cycles Ydy Non Arrivals
        /// </summary>
        public static StandardMeasurement CyclesYdyNonArrivals { get; } =
            CreateValue(1098, "Cycls Ydy NonArvls", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Cycles Tot Non Arrivals
        /// </summary>
        public static StandardMeasurement CyclesTotNonArrivals { get; } =
            CreateValue(1099, "Cycls Tot NonArvls", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Cycles Tdy Vents
        /// </summary>
        public static StandardMeasurement CyclesTdyVents { get; } =
            CreateValue(1100, "Cycls Tdy Vents", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Cycles Ydy Vents
        /// </summary>
        public static StandardMeasurement CyclesYdyVents { get; } =
            CreateValue(1101, "Cycls Ydy Vents", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Cycles Tot Vents
        /// </summary>
        public static StandardMeasurement CyclesTotVents { get; } =
            CreateValue(1102, "Cycls Tot Vents", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Rise Velocity Avg 3 Times
        /// </summary>
        public static StandardMeasurement RiseVelocityAvg3Times { get; } =
            CreateValue(1103, "RiseVel Avg 3", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Rise Velocity Avg 6 Times
        /// </summary>
        public static StandardMeasurement RiseVelocityAvg6Times { get; } =
            CreateValue(1104, "RiseVel Avg 6", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Last 10 Arv Mins 1
        /// </summary>
        public static StandardMeasurement Last10ArvMins1 { get; } =
            CreateValue(1105, "Last 10 Arv Mins 1", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Last 10 Arv Mins 2
        /// </summary>
        public static StandardMeasurement Last10ArvMins2 { get; } =
            CreateValue(1106, "Last 10 Arv Mins 2", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Last 10 Arv Mins 3
        /// </summary>
        public static StandardMeasurement Last10ArvMins3 { get; } =
            CreateValue(1107, "Last 10 Arv Mins 3", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Last 10 Arv Mins 4
        /// </summary>
        public static StandardMeasurement Last10ArvMins4 { get; } =
            CreateValue(1108, "Last 10 Arv Mins 4", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Last 10 Arv Mins 5
        /// </summary>
        public static StandardMeasurement Last10ArvMins5 { get; } =
            CreateValue(1109, "Last 10 Arv Mins 5", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Last 10 Arv Mins 6
        /// </summary>
        public static StandardMeasurement Last10ArvMins6 { get; } =
            CreateValue(1110, "Last 10 Arv Mins 6", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Last 10 Arv Mins 7
        /// </summary>
        public static StandardMeasurement Last10ArvMins7 { get; } =
            CreateValue(1111, "Last 10 Arv Mins 7", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Last 10 Arv Mins 8
        /// </summary>
        public static StandardMeasurement Last10ArvMins8 { get; } =
            CreateValue(1112, "Last 10 Arv Mins 8", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Last 10 Arv Mins 9
        /// </summary>
        public static StandardMeasurement Last10ArvMins9 { get; } =
            CreateValue(1113, "Last 10 Arv Mins 9", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Last 10 Arv Mins 10
        /// </summary>
        public static StandardMeasurement Last10ArvMins10 { get; } =
            CreateValue(1114, "Last 10 Arv Mins 10", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Last 10 Arv Feet Min 1
        /// </summary>
        public static StandardMeasurement Last10ArvFeetMin1 { get; } =
            CreateValue(1115, "Last 10 Arv FtMin 1", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Last 10 Arv Feet Min 2
        /// </summary>
        public static StandardMeasurement Last10ArvFeetMin2 { get; } =
            CreateValue(1116, "Last 10 Arv FtMin 2", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Last 10 Arv Feet Min 3
        /// </summary>
        public static StandardMeasurement Last10ArvFeetMin3 { get; } =
            CreateValue(1117, "Last 10 Arv FtMin 3", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Last 10 Arv Feet Min 4
        /// </summary>
        public static StandardMeasurement Last10ArvFeetMin4 { get; } =
            CreateValue(1118, "Last 10 Arv FtMin 4", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Last 10 Arv Feet Min 5
        /// </summary>
        public static StandardMeasurement Last10ArvFeetMin5 { get; } =
            CreateValue(1119, "Last 10 Arv FtMin 5", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Last 10 Arv Feet Min 6
        /// </summary>
        public static StandardMeasurement Last10ArvFeetMin6 { get; } =
            CreateValue(1120, "Last 10 Arv FtMin 6", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Last 10 Arv Feet Min 7
        /// </summary>
        public static StandardMeasurement Last10ArvFeetMin7 { get; } =
            CreateValue(1121, "Last 10 Arv FtMin 7", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Last 10 Arv Feet Min 8
        /// </summary>
        public static StandardMeasurement Last10ArvFeetMin8 { get; } =
            CreateValue(1122, "Last 10 Arv FtMin 8", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Last 10 Arv Feet Min 9
        /// </summary>
        public static StandardMeasurement Last10ArvFeetMin9 { get; } =
            CreateValue(1123, "Last 10 Arv FtMin 9", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Last 10 Arv Feet Min 10
        /// </summary>
        public static StandardMeasurement Last10ArvFeetMin10 { get; } =
            CreateValue(1124, "Last 10 Arv FtMin 10", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Days Cycle Reset
        /// </summary>
        public static StandardMeasurement DaysCycleReset { get; } =
            CreateValue(1125, "Days Cycl Reset", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Current Status
        /// </summary>
        public static StandardMeasurement CurrentStatus { get; } =
            CreateValue(1126, "Current Status", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Flow Mcfd
        /// </summary>
        public static StandardMeasurement FlowMcfd { get; } =
            CreateValue(1127, "Flow Mscfd", UnitCategory.GasRate);

        /// <summary>
        /// Specifies that the parameter represents the Mtr DP Hw
        /// </summary>
        public static StandardMeasurement MtrDPHw { get; } =
            CreateValue(1128, "Mtr DP Hw", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Flow Accum Mcf Today
        /// </summary>
        public static StandardMeasurement FlowAccumMcfToday { get; } =
            CreateValue(1133, "Flow Accum Mcf Tdy", UnitCategory.GasVolume);

        /// <summary>
        /// Specifies that the parameter represents the Flow Accum Mcf YDay
        /// </summary>
        public static StandardMeasurement FlowAccumMcfYDay { get; } =
            CreateValue(1134, "Flow Accum Mcf Ydy", UnitCategory.GasVolume);

        /// <summary>
        /// Specifies that the parameter represents the Flow Accum Tdy Hours On
        /// </summary>
        public static StandardMeasurement FlowAccumTdyHoursOn { get; } =
            CreateValue(1135, "Flow Accum Tdy Hrs On", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Flow Accum Ydy Hours On
        /// </summary>
        public static StandardMeasurement FlowAccumYdyHoursOn { get; } =
            CreateValue(1136, "Flow Accum Ydy Hrs On", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Cycling Mode
        /// </summary>
        public static StandardMeasurement CyclingMode { get; } =
            CreateValue(1137, "Cycling Mode", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Trigger LoadF Current Value
        /// </summary>
        public static StandardMeasurement OpenTriggerLoadFCurrentValue { get; } =
            CreateValue(1141, "OpnTrgrLoadFLn_Cur Val", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Trigger Sht Tm Current Value
        /// </summary>
        public static StandardMeasurement OpenTriggerShtTmCurrentValue { get; } =
            CreateValue(1142, "OpnTrgrShutTime Gt_Cur Val", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the PrevOpen MMDD
        /// </summary>
        public static StandardMeasurement PrevOpenMMDD { get; } =
            CreateValue(1154, "PrevOpen MMDD", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the PrevOpen HHMM
        /// </summary>
        public static StandardMeasurement PrevOpenHHMM { get; } =
            CreateValue(1155, "PrevOpen HHMM", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the PrevOpen Event
        /// </summary>
        public static StandardMeasurement PrevOpenEvent { get; } =
            CreateValue(1156, "PrevOpen Event", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the PrevOpen Ref Val
        /// </summary>
        public static StandardMeasurement PrevOpenRefVal { get; } =
            CreateValue(1157, "PrevOpen Ref Val", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the PrevOpen Casing Psig
        /// </summary>
        public static StandardMeasurement PrevOpenCasingPsig { get; } =
            CreateValue(1158, "PrevOpen Casing Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the PrevOpen Tubing Psig
        /// </summary>
        public static StandardMeasurement PrevOpenTubingPsig { get; } =
            CreateValue(1159, "PrevOpen Tubing Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the PrevOpen Load Factor
        /// </summary>
        public static StandardMeasurement PrevOpenLoadFactor { get; } =
            CreateValue(1160, "PrevOpen Load Factor", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the PrevOpen Grs Off Mins
        /// </summary>
        public static StandardMeasurement PrevOpenGrsOffMins { get; } =
            CreateValue(1161, "PrevOpen Total Mins Off", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the PrevOpen Net Off Mins
        /// </summary>
        public static StandardMeasurement PrevOpenNetOffMins { get; } =
            CreateValue(1162, "PrevClose Total Mins On", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the PrevOpen Casing Line
        /// </summary>
        public static StandardMeasurement PrevOpenCasingLine { get; } =
            CreateValue(1163, "PrevOpen Casing Line", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the PrevOpen Tubing Line
        /// </summary>
        public static StandardMeasurement PrevOpenTubingLine { get; } =
            CreateValue(1164, "PrevOpen Tubing Line", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the PrevOpen Line Psig
        /// </summary>
        public static StandardMeasurement PrevOpenLinePsig { get; } =
            CreateValue(1165, "PrevOpen Line Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the SecondPrevOpen MMDD
        /// </summary>
        public static StandardMeasurement SecondPrevOpenMMDD { get; } =
            CreateValue(1166, "2ndPrevOpen MMDD", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the SecondPrevOpen HHMM
        /// </summary>
        public static StandardMeasurement SecondPrevOpenHHMM { get; } =
            CreateValue(1167, "2ndPrevOpen HHMM", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the SecondPrevOpen Event
        /// </summary>
        public static StandardMeasurement SecondPrevOpenEvent { get; } =
            CreateValue(1168, "2ndPrevOpen Event", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the SecondPrevOpen Ref Val
        /// </summary>
        public static StandardMeasurement SecondPrevOpenRefVal { get; } =
            CreateValue(1169, "2ndPrevOpen Ref Val", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the SecondPrevOpen Casing Psig
        /// </summary> 
        public static StandardMeasurement SecondPrevOpenCasingPsig { get; } =
            CreateValue(1170, "2ndPrevOpen Casing Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the SecondPrevOpen Tubing Psig
        /// </summary>
        public static StandardMeasurement SecondPrevOpenTubingPsig { get; } =
            CreateValue(1171, "2ndPrevOpen Tubing Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the SecondPrevOpen Load Factor
        /// </summary>
        public static StandardMeasurement SecondPrevOpenLoadFactor { get; } =
            CreateValue(1172, "2ndPrevOpen Load Factor", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the SecondPrevOpen Grs Off Mins
        /// </summary>
        public static StandardMeasurement SecondPrevOpenGrsOffMins { get; } =
            CreateValue(1173, "2ndPrevOpen Total Mins Off", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the SecondPrevOpen Net Off Mins
        /// </summary>
        public static StandardMeasurement SecondPrevOpenNetOffMins { get; } =
            CreateValue(1174, "2ndPrevClose Total Mins O", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the SecondPrevOpen Casing Line
        /// </summary>
        public static StandardMeasurement SecondPrevOpenCasingLine { get; } =
            CreateValue(1175, "2ndPrevOpen Casing Line", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the SecondPrevOpen Tubing Line
        /// </summary>
        public static StandardMeasurement SecondPrevOpenTubingLine { get; } =
            CreateValue(1176, "2ndPrevOpen Tubing Line", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the SecondPrevOpen Line Psig
        /// </summary>
        public static StandardMeasurement SecondPrevOpenLinePsig { get; } =
            CreateValue(1177, "2ndPrevOpen Line Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the ThirdPrevOpen MMDD
        /// </summary>
        public static StandardMeasurement ThirdPrevOpenMMDD { get; } =
            CreateValue(1178, "3rdPrevOpen MMDD", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the ThirdPrevOpen HHMM
        /// </summary>
        public static StandardMeasurement ThirdPrevOpenHHMM { get; } =
            CreateValue(1179, "3rdPrevOpen HHMM", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the ThirdPrevOpen Event
        /// </summary>
        public static StandardMeasurement ThirdPrevOpenEvent { get; } =
            CreateValue(1180, "3rdPrevOpen Event", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the ThirdPrevOpen Ref Val
        /// </summary>
        public static StandardMeasurement ThirdPrevOpenRefVal { get; } =
            CreateValue(1181, "3rdPrevOpen Ref Val", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the ThirdPrevOpen Casing Psig
        /// </summary>
        public static StandardMeasurement ThirdPrevOpenCasingPsig { get; } =
            CreateValue(1182, "3rdPrevOpen Casing Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the ThirdPrevOpen Tubing Psig
        /// </summary>
        public static StandardMeasurement ThirdPrevOpenTubingPsig { get; } =
            CreateValue(1183, "3rdPrevOpen Tubing Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the ThirdPrevOpen Load Factor
        /// </summary>
        public static StandardMeasurement ThirdPrevOpenLoadFactor { get; } =
            CreateValue(1184, "3rdPrevOpen Load Factor", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the ThirdPrevOpen Grs Off Mins
        /// </summary>
        public static StandardMeasurement ThirdPrevOpenGrsOffMins { get; } =
            CreateValue(1185, "3rdPrevOpen Total Mins Off", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the ThirdPrevOpen Net Off Mins
        /// </summary>
        public static StandardMeasurement ThirdPrevOpenNetOffMins { get; } =
            CreateValue(1186, "3rdPrevClose Total Mins On", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the ThirdPrevOpen Casing Line
        /// </summary>
        public static StandardMeasurement ThirdPrevOpenCasingLine { get; } =
            CreateValue(1187, "3rdPrevOpen Casing Line", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the ThirdPrevOpen Tubing Line
        /// </summary>
        public static StandardMeasurement ThirdPrevOpenTubingLine { get; } =
            CreateValue(1188, "3rdPrevOpen Tubing Line", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the ThirdPrevOpen Line Psig
        /// </summary>
        public static StandardMeasurement ThirdPrevOpenLinePsig { get; } =
            CreateValue(1189, "3rdPrevOpen Line Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the FourthPrevOpen MMDD
        /// </summary>
        public static StandardMeasurement FourthPrevOpenMMDD { get; } =
            CreateValue(1190, "4thPrevOpen MMDD", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FourthPrevOpen HHMM
        /// </summary>
        public static StandardMeasurement FourthPrevOpenHHMM { get; } =
            CreateValue(1191, "4thPrevOpen HHMM", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FourthPrevOpen Event
        /// </summary>
        public static StandardMeasurement FourthPrevOpenEvent { get; } =
            CreateValue(1192, "4thPrevOpen Event", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FourthPrevOpen Ref Val
        /// </summary>
        public static StandardMeasurement FourthPrevOpenRefVal { get; } =
            CreateValue(1193, "4thPrevOpen Ref Val", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FourthPrevOpen Casing Psig
        /// </summary>
        public static StandardMeasurement FourthPrevOpenCasingPsig { get; } =
            CreateValue(1194, "4thPrevOpen Casing Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the FourthPrevOpen Tubing Psig
        /// </summary>
        public static StandardMeasurement FourthPrevOpenTubingPsig { get; } =
            CreateValue(1195, "4thPrevOpen Tubing Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the FourthPrevOpen Load Factor
        /// </summary>
        public static StandardMeasurement FourthPrevOpenLoadFactor { get; } =
            CreateValue(1196, "4thPrevOpen Load Factor", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FourthPrevOpen Grs Off Mins
        /// </summary>
        public static StandardMeasurement FourthPrevOpenGrsOffMins { get; } =
            CreateValue(1197, "4thPrevOpen Total Mins Off", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FourthPrevOpen Net Off Mins
        /// </summary>
        public static StandardMeasurement FourthPrevOpenNetOffMins { get; } =
            CreateValue(1198, "4thPrevClose Total Mins On", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FourthPrevOpen Casing Line
        /// </summary>
        public static StandardMeasurement FourthPrevOpenCasingLine { get; } =
            CreateValue(1199, "4thPrevOpen Casing Line", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FourthPrevOpen Tubing Line
        /// </summary>
        public static StandardMeasurement FourthPrevOpenTubingLine { get; } =
            CreateValue(1200, "4thPrevOpen Tubing Line", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FourthPrevOpen Line Psig
        /// </summary>
        public static StandardMeasurement FourthPrevOpenLinePsig { get; } =
            CreateValue(1201, "4thPrevOpen Line Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the FifthPrevOpen MMDD
        /// </summary>
        public static StandardMeasurement FifthPrevOpenMMDD { get; } =
            CreateValue(1202, "5thPrevOpen MMDD", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FifthPrevOpen HHMM
        /// </summary>
        public static StandardMeasurement FifthPrevOpenHHMM { get; } =
            CreateValue(1203, "5thPrevOpen HHMM", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FifthPrevOpen Event
        /// </summary>
        public static StandardMeasurement FifthPrevOpenEvent { get; } =
            CreateValue(1204, "5thPrevOpen Event", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FifthPrevOpen Ref Val
        /// </summary>
        public static StandardMeasurement FifthPrevOpenRefVal { get; } =
            CreateValue(1205, "5thPrevOpen Ref Val", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FifthPrevOpen Casing Psig
        /// </summary>
        public static StandardMeasurement FifthPrevOpenCasingPsig { get; } =
            CreateValue(1206, "5thPrevOpen Casing Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the FifthPrevOpen Tubing Psig
        /// </summary>
        public static StandardMeasurement FifthPrevOpenTubingPsig { get; } =
            CreateValue(1207, "5thPrevOpen Tubing Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the FifthPrevOpen Load Factor
        /// </summary>
        public static StandardMeasurement FifthPrevOpenLoadFactor { get; } =
            CreateValue(1208, "5thPrevOpen Load Factor", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FifthPrevOpen Grs Off Mins
        /// </summary>
        public static StandardMeasurement FifthPrevOpenGrsOffMins { get; } =
            CreateValue(1209, "5thPrevOpen Total Mins Off", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FifthPrevOpen Net Off Mins
        /// </summary>
        public static StandardMeasurement FifthPrevOpenNetOffMins { get; } =
            CreateValue(1210, "5thPrevClose Total Mins On", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FifthPrevOpen Casing Line
        /// </summary>
        public static StandardMeasurement FifthPrevOpenCasingLine { get; } =
            CreateValue(1211, "5thPrevOpen Casing Line", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FifthPrevOpen Tubing Line
        /// </summary>
        public static StandardMeasurement FifthPrevOpenTubingLine { get; } =
            CreateValue(1212, "5thPrevOpen Tubing Line", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FifthPrevOpen Line Psig
        /// </summary>
        public static StandardMeasurement FifthPrevOpenLinePsig { get; } =
            CreateValue(1213, "5thPrevOpen Line Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the PrevClose MMDD
        /// </summary>
        public static StandardMeasurement PrevCloseMMDD { get; } =
            CreateValue(1214, "PrevClose MMDD", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the PrevClose HHMM
        /// </summary>
        public static StandardMeasurement PrevCloseHHMM { get; } =
            CreateValue(1215, "PrevClose HHMM", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the PrevClose Event
        /// </summary>
        public static StandardMeasurement PrevCloseEvent { get; } =
            CreateValue(1216, "PrevClose Event", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the PrevClose Ref Val
        /// </summary>
        public static StandardMeasurement PrevCloseRefVal { get; } =
            CreateValue(1217, "PrevClose Ref Val", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the PrevClose Casing Psig
        /// </summary>
        public static StandardMeasurement PrevCloseCasingPsig { get; } =
            CreateValue(1218, "PrevClose Casing Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the PrevClose Tubing Psig
        /// </summary>
        public static StandardMeasurement PrevCloseTubingPsig { get; } =
            CreateValue(1219, "PrevClose Tubing Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the PrevClose Lowest Csg
        /// </summary>
        public static StandardMeasurement PrevCloseLowestCsg { get; } =
            CreateValue(1220, "PrevClose Lowest Csg", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the PrevClose Flow Rate
        /// </summary>
        public static StandardMeasurement PrevCloseFlowRate { get; } =
            CreateValue(1221, "PrevClose Flow Rate", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the PrevClose Critical Rate
        /// </summary>
        public static StandardMeasurement PrevCloseCriticalRate { get; } =
            CreateValue(1222, "PrevClose Critical Rate", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the PrevClose AftFlw Mins
        /// </summary>
        public static StandardMeasurement PrevCloseAftFlwMins { get; } =
            CreateValue(1223, "PrevClose AftFlw Mins", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the PrevClose Cycle Mcf
        /// </summary>
        public static StandardMeasurement PrevCloseCycleMcf { get; } =
            CreateValue(1224, "PrevClose Cycle Mcf", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the PrevClose Line Psig
        /// </summary>
        public static StandardMeasurement PrevCloseLinePsig { get; } =
            CreateValue(1225, "PrevClose Line Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the SecondPrevClose MMDD
        /// </summary>
        public static StandardMeasurement SecondPrevCloseMMDD { get; } =
            CreateValue(1226, "2ndPrevClose MMDD", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the SecondPrevClose HHMM
        /// </summary>
        public static StandardMeasurement SecondPrevCloseHHMM { get; } =
            CreateValue(1227, "2ndPrevClose HHMM", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the SecondPrevClose Event
        /// </summary>
        public static StandardMeasurement SecondPrevCloseEvent { get; } =
            CreateValue(1228, "2ndPrevClose Event", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the SecondPrevClose Ref Val
        /// </summary>
        public static StandardMeasurement SecondPrevCloseRefVal { get; } =
            CreateValue(1229, "2ndPrevClose Ref Val", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the SecondPrevClose Casing Psig
        /// </summary>
        public static StandardMeasurement SecondPrevCloseCasingPsig { get; } =
            CreateValue(1230, "2ndPrevClose Casing Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the SecondPrevClose Tubing Psig
        /// </summary>
        public static StandardMeasurement SecondPrevCloseTubingPsig { get; } =
            CreateValue(1231, "2ndPrevClose Tubing Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the SecondPrevClose Lowest Csg
        /// </summary>
        public static StandardMeasurement SecondPrevCloseLowestCsg { get; } =
            CreateValue(1232, "2ndPrevClose Lowest Csg", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the SecondPrevClose Flow Rate
        /// </summary>
        public static StandardMeasurement SecondPrevCloseFlowRate { get; } =
            CreateValue(1233, "2ndPrevClose Flow Rate", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the SecondPrevClose Critical Rate
        /// </summary>
        public static StandardMeasurement SecondPrevCloseCriticalRate { get; } =
            CreateValue(1234, "2ndPrevClose Critical Rate", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the SecondPrevClose AftFlw Mins
        /// </summary>
        public static StandardMeasurement SecondPrevCloseAftFlwMins { get; } =
            CreateValue(1235, "2ndPrevClose AftFlw Mins", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the SecondPrevClose Cycle Mcf
        /// </summary>
        public static StandardMeasurement SecondPrevCloseCycleMcf { get; } =
            CreateValue(1236, "2ndPrevClose Cycle Mcf", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the SecondPrevClose Line Psig
        /// </summary>
        public static StandardMeasurement SecondPrevCloseLinePsig { get; } =
            CreateValue(1237, "2ndPrevClose Line Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the ThirdPrevClose MMDD
        /// </summary>
        public static StandardMeasurement ThirdPrevCloseMMDD { get; } =
            CreateValue(1238, "3rdPrevClose MMDD", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the ThirdPrevClose HHMM
        /// </summary>
        public static StandardMeasurement ThirdPrevCloseHHMM { get; } =
            CreateValue(1239, "3rdPrevClose HHMM", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the ThirdPrevClose Event
        /// </summary>
        public static StandardMeasurement ThirdPrevCloseEvent { get; } =
            CreateValue(1240, "3rdPrevClose Event", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the ThirdPrevClose Ref Val
        /// </summary>
        public static StandardMeasurement ThirdPrevCloseRefVal { get; } =
            CreateValue(1241, "3rdPrevClose Ref Val", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the ThirdPrevClose Casing Psig
        /// </summary>
        public static StandardMeasurement ThirdPrevCloseCasingPsig { get; } =
            CreateValue(1242, "3rdPrevClose Casing Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the ThirdPrevClose Tubing Psig
        /// </summary>
        public static StandardMeasurement ThirdPrevCloseTubingPsig { get; } =
            CreateValue(1243, "3rdPrevClose Tubing Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the ThirdPrevClose Lowest Csg
        /// </summary>
        public static StandardMeasurement ThirdPrevCloseLowestCsg { get; } =
            CreateValue(1244, "3rdPrevClose Lowest Csg", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the ThirdPrevClose Flow Rate
        /// </summary>
        public static StandardMeasurement ThirdPrevCloseFlowRate { get; } =
            CreateValue(1245, "3rdPrevClose Flow Rate", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the ThirdPrevClose Critical Rate
        /// </summary>
        public static StandardMeasurement ThirdPrevCloseCriticalRate { get; } =
            CreateValue(1246, "3rdPrevClose Critical Rate", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the ThirdPrevClose AftFlw Mins
        /// </summary>
        public static StandardMeasurement ThirdPrevCloseAftFlwMins { get; } =
            CreateValue(1247, "3rdPrevClose AftFlw Mins", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the ThirdPrevClose Cycle Mcf
        /// </summary>
        public static StandardMeasurement ThirdPrevCloseCycleMcf { get; } =
            CreateValue(1248, "3rdPrevClose Cycle Mcf", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the ThirdPrevClose Line Psig
        /// </summary>
        public static StandardMeasurement ThirdPrevCloseLinePsig { get; } =
            CreateValue(1249, "3rdPrevClose Line Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the FourthPrevClose MMDD
        /// </summary>
        public static StandardMeasurement FourthPrevCloseMMDD { get; } =
            CreateValue(1250, "4thPrevClose MMDD", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FourthPrevClose HHMM
        /// </summary>
        public static StandardMeasurement FourthPrevCloseHHMM { get; } =
            CreateValue(1251, "4thPrevClose HHMM", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FourthPrevClose Event
        /// </summary>
        public static StandardMeasurement FourthPrevCloseEvent { get; } =
            CreateValue(1252, "4thPrevClose Event", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FourthPrevClose Ref Val
        /// </summary>
        public static StandardMeasurement FourthPrevCloseRefVal { get; } =
            CreateValue(1253, "4thPrevClose Ref Val", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FourthPrevClose Casing Psig
        /// </summary>
        public static StandardMeasurement FourthPrevCloseCasingPsig { get; } =
            CreateValue(1254, "4thPrevClose Casing Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the FourthPrevClose Tubing Psig
        /// </summary>
        public static StandardMeasurement FourthPrevCloseTubingPsig { get; } =
            CreateValue(1255, "4thPrevClose Tubing Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the FourthPrevClose Lowest Csg
        /// </summary>
        public static StandardMeasurement FourthPrevCloseLowestCsg { get; } =
            CreateValue(1256, "4thPrevClose Lowest Csg", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FourthPrevClose Flow Rate
        /// </summary>
        public static StandardMeasurement FourthPrevCloseFlowRate { get; } =
            CreateValue(1257, "4thPrevClose Flow Rate", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FourthPrevClose Critical Rate
        /// </summary>
        public static StandardMeasurement FourthPrevCloseCriticalRate { get; } =
            CreateValue(1258, "4thPrevClose Critical Rate", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FourthPrevClose AftFlw Mins
        /// </summary>
        public static StandardMeasurement FourthPrevCloseAftFlwMins { get; } =
            CreateValue(1259, "4thPrevClose AftFlw Mins", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FourthPrevClose Cycle Mcf
        /// </summary>
        public static StandardMeasurement FourthPrevCloseCycleMcf { get; } =
            CreateValue(1260, "4thPrevClose Cycle Mcf", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FourthPrevClose Line Psig
        /// </summary>
        public static StandardMeasurement FourthPrevCloseLinePsig { get; } =
            CreateValue(1261, "4thPrevClose Line Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the FifthPrevClose MMDD
        /// </summary>
        public static StandardMeasurement FifthPrevCloseMMDD { get; } =
            CreateValue(1262, "5thPrevClose MMDD", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FifthPrevClose HHMM
        /// </summary>
        public static StandardMeasurement FifthPrevCloseHHMM { get; } =
            CreateValue(1263, "5thPrevClose HHMM", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FifthPrevClose Event
        /// </summary>
        public static StandardMeasurement FifthPrevCloseEvent { get; } =
            CreateValue(1264, "5thPrevClose Event", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FifthPrevClose Ref Val
        /// </summary>
        public static StandardMeasurement FifthPrevCloseRefVal { get; } =
            CreateValue(1265, "5thPrevClose Ref Val", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FifthPrevClose Casing Psig
        /// </summary>
        public static StandardMeasurement FifthPrevCloseCasingPsig { get; } =
            CreateValue(1266, "5thPrevClose Casing Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the FifthPrevClose Tubing Psig
        /// </summary>
        public static StandardMeasurement FifthPrevCloseTubingPsig { get; } =
            CreateValue(1267, "5thPrevClose Tubing Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the FifthPrevClose Lowest Csg
        /// </summary>
        public static StandardMeasurement FifthPrevCloseLowestCsg { get; } =
            CreateValue(1268, "5thPrevClose Lowest Csg", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FifthPrevClose Flow Rate
        /// </summary>
        public static StandardMeasurement FifthPrevCloseFlowRate { get; } =
            CreateValue(1269, "5thPrevClose Flow Rate", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FifthPrevClose Critical Rate
        /// </summary>
        public static StandardMeasurement FifthPrevCloseCriticalRate { get; } =
            CreateValue(1270, "5thPrevClose Critical Rate", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FifthPrevClose AftFlw Mins
        /// </summary>
        public static StandardMeasurement FifthPrevCloseAftFlwMins { get; } =
            CreateValue(1271, "5thPrevClose AftFlw Mins", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FifthPrevClose Cycle Mcf
        /// </summary>
        public static StandardMeasurement FifthPrevCloseCycleMcf { get; } =
            CreateValue(1272, "5thPrevClose Cycle Mcf", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FifthPrevClose Line Psig
        /// </summary>
        public static StandardMeasurement FifthPrevCloseLinePsig { get; } =
            CreateValue(1273, "5thPrevClose Line Psig", UnitCategory.Pressure);

        /// <summary>
        /// Specifies that the parameter represents the PrevClose Meter DP
        /// </summary>
        public static StandardMeasurement PrevCloseMeterDP { get; } =
            CreateValue(1274, "PrevClose Meter DP", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the SecondPrevClose Meter DP
        /// </summary>
        public static StandardMeasurement SecondPrevCloseMeterDP { get; } =
            CreateValue(1275, "2ndPrevClose Meter DP", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the ThirdPrevClose Meter DP
        /// </summary>
        public static StandardMeasurement ThirdPrevCloseMeterDP { get; } =
            CreateValue(1276, "3ndPrevClose Meter DP", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FourthPrevClose Meter DP
        /// </summary>
        public static StandardMeasurement FourthPrevCloseMeterDP { get; } =
            CreateValue(1277, "4thPrevClose Meter DP", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the FifthPrevClose Meter DP
        /// </summary>
        public static StandardMeasurement FifthPrevCloseMeterDP { get; } =
            CreateValue(1282, "5thPrevClose Meter DP", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Cycles Replacement Date
        /// </summary>
        public static StandardMeasurement CyclesReplacementDate { get; } =
            CreateValue(1395, "Plunger Replacment Date", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Cycles Plunger Age Cycles
        /// </summary>
        public static StandardMeasurement CyclesPlungerAgeCycles { get; } =
            CreateValue(1396, "Plunger Age Cycles", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Cycles Plunger Age Days
        /// </summary>
        public static StandardMeasurement CyclesPlungerAgeDays { get; } =
            CreateValue(1397, "Plunger Age Days", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Location
        /// </summary>
        public static StandardMeasurement Location { get; } =
            CreateValue(1346, "Location", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Well ID
        /// </summary>
        public static StandardMeasurement WellID { get; } =
            CreateValue(1347, "Well ID", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Rtu Time
        /// </summary>
        public static StandardMeasurement RtuTime { get; } =
            CreateValue(1348, "Rtu Time", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Plunger Control Mode
        /// </summary>
        public static StandardMeasurement PlungerControlMode { get; } =
            CreateValue(1278, "Plunger Control Mode", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Current None
        /// </summary>
        public static StandardMeasurement CurrentNone { get; } =
            CreateValue(1279, "Current None", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Production Flow Time Today
        /// </summary>
        public static StandardMeasurement ProductionFlowTimeToday { get; } =
            CreateValue(1280, "Production Flow Time Today", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Production Flow Time Yesterday
        /// </summary>
        public static StandardMeasurement ProductionFlowTimeYesterday { get; } =
            CreateValue(1281, "Production Flow Time Yesterday", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Cycle Arrivals Today
        /// </summary>
        public static StandardMeasurement CycleArrivalsToday { get; } =
            CreateValue(1283, "Cycle Arrivals Today", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Cycle Arrivals Yesterday
        /// </summary>
        public static StandardMeasurement CycleArrivalsYesterday { get; } =
            CreateValue(1284, "Cycle Arrivals Yesterday", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Cycle Total Cycles
        /// </summary>
        public static StandardMeasurement CycleTotalCycles { get; } =
            CreateValue(1302, "Cycle Total Cycles", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Plunger Fall Time Setpoint
        /// </summary>
        public static StandardMeasurement PlungerFallTimeSetpoint { get; } =
            CreateValue(1304, "Plunger Fall Time Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the CurrentPositionCycleState
        /// </summary>
        public static StandardMeasurement CurrentPositionCycleState { get; } =
            CreateValue(1311, "CurrentPositionCycleState", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Manual Control Force Shut In
        /// </summary>
        public static StandardMeasurement ManualControlForceShutIn { get; } =
            CreateValue(1312, "Manual Control Force Shut In", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Manual Control Force Manual
        /// </summary>
        public static StandardMeasurement ManualControlForceManual { get; } =
            CreateValue(1313, "Manual Control Force Manual", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Timer Enable
        /// </summary>
        public static StandardMeasurement OpenTimerEnable { get; } =
            CreateValue(1349, "Open Timer Enable", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Timer Actual
        /// </summary>
        public static StandardMeasurement OpenTimerActual { get; } =
            CreateValue(1350, "Open Timer Actual", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Timer Setpoint
        /// </summary>
        public static StandardMeasurement OpenTimerSetpoint { get; } =
            CreateValue(1351, "Open Timer Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open TL Enable
        /// </summary>
        public static StandardMeasurement OpenTLEnable { get; } =
            CreateValue(1352, "Open TL Enable", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open TL Actual
        /// </summary>
        public static StandardMeasurement OpenTLActual { get; } =
            CreateValue(1353, "Open TL Actual", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open TL Setpoint
        /// </summary>
        public static StandardMeasurement OpenTLSetpoint { get; } =
            CreateValue(1354, "Open TL Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open CL Enable
        /// </summary>
        public static StandardMeasurement OpenCLEnable { get; } =
            CreateValue(1355, "Open CL Enable", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open CL Actual
        /// </summary>
        public static StandardMeasurement OpenCLActual { get; } =
            CreateValue(1356, "Open CL Actual", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open CL Setpoint
        /// </summary>
        public static StandardMeasurement OpenCLSetpoint { get; } =
            CreateValue(1357, "Open CL Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open CT Enable
        /// </summary>
        public static StandardMeasurement OpenCTEnable { get; } =
            CreateValue(1358, "Open CT Enable", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open CT Actual
        /// </summary>
        public static StandardMeasurement OpenCTActual { get; } =
            CreateValue(1359, "Open CT Actual", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open CT Setpoint
        /// </summary>
        public static StandardMeasurement OpenCTSetpoint { get; } =
            CreateValue(1360, "Open CT Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open CLTL Enable
        /// </summary>
        public static StandardMeasurement OpenCLTLEnable { get; } =
            CreateValue(1361, "Open CLTL Enable", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open CLTL Actual
        /// </summary>
        public static StandardMeasurement OpenCLTLActual { get; } =
            CreateValue(1362, "Open CLTL Actual", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open CLTL Setpoint
        /// </summary>
        public static StandardMeasurement OpenCLTLSetpoint { get; } =
            CreateValue(1363, "Open CLTL Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open CTTL Enable
        /// </summary>
        public static StandardMeasurement OpenCTTLEnable { get; } =
            CreateValue(1364, "Open CTTL Enable", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open CTTL Actual
        /// </summary>
        public static StandardMeasurement OpenCTTLActual { get; } =
            CreateValue(1365, "Open CTTL Actual", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open CTTL Setpoint
        /// </summary>
        public static StandardMeasurement OpenCTTLSetpoint { get; } =
            CreateValue(1366, "Open CTTL Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Load Ratio Enable
        /// </summary>
        public static StandardMeasurement OpenLoadRatioEnable { get; } =
            CreateValue(1367, "Open Load Ratio Enable", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Load Ratio Actual
        /// </summary>
        public static StandardMeasurement OpenLoadRatioActual { get; } =
            CreateValue(1368, "Open Load Ratio Actual", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Load Ratio Setpoint
        /// </summary>
        public static StandardMeasurement OpenLoadRatioSetpoint { get; } =
            CreateValue(1369, "Open Load Ratio Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open FG Enable
        /// </summary>
        public static StandardMeasurement OpenFGEnable { get; } =
            CreateValue(1299, "Open FG Enable", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open FG Actual
        /// </summary>
        public static StandardMeasurement OpenFGActual { get; } =
            CreateValue(1300, "Open FG Actual", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open FG Setpoint
        /// </summary>
        public static StandardMeasurement OpenFGSetpoint { get; } =
            CreateValue(1301, "Open FG Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Tubing Open Enable
        /// </summary>
        public static StandardMeasurement OpenTubingOpenEnable { get; } =
            CreateValue(1370, "Open Tubing Open Enable", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Tubing Open Actual
        /// </summary>
        public static StandardMeasurement OpenTubingOpenActual { get; } =
            CreateValue(1371, "Open Tubing Open Actual", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Tubing Open Setpoint
        /// </summary>
        public static StandardMeasurement OpenTubingOpenSetpoint { get; } =
            CreateValue(1372, "Open Tubing Open Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Casing Open Enable
        /// </summary>
        public static StandardMeasurement OpenCasingOpenEnable { get; } =
            CreateValue(1373, "Open Casing Open Enable", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Casing Open Actual
        /// </summary>
        public static StandardMeasurement OpenCasingOpenActual { get; } =
            CreateValue(1374, "Open Casing Open Actual", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Casing Open Setpoint
        /// </summary>
        public static StandardMeasurement OpenCasingOpenSetpoint { get; } =
            CreateValue(1375, "Open Casing Open Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Open1 Enable
        /// </summary>
        public static StandardMeasurement OpenOpen1Enable { get; } =
            CreateValue(1305, "Open Open1 Enable", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Open1 Actual
        /// </summary>
        public static StandardMeasurement OpenOpen1Actual { get; } =
            CreateValue(1306, "Open Open1 Actual", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Open1 Setpoint
        /// </summary>
        public static StandardMeasurement OpenOpen1Setpoint { get; } =
            CreateValue(1307, "Open Open1 Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Open2 Enable
        /// </summary>
        public static StandardMeasurement OpenOpen2Enable { get; } =
            CreateValue(1308, "Open Open2 Enable", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Open2 Actual
        /// </summary>
        public static StandardMeasurement OpenOpen2Actual { get; } =
            CreateValue(1309, "Open Open2 Actual", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Open Open2 Setpoint
        /// </summary>
        public static StandardMeasurement OpenOpen2Setpoint { get; } =
            CreateValue(1310, "Open Open2 Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Max Wait
        /// </summary>
        public static StandardMeasurement ArrivalMaxWait { get; } =
            CreateValue(1335, "Arrival Max Wait", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Slow Time
        /// </summary>
        public static StandardMeasurement ArrivalSlowTime { get; } =
            CreateValue(1336, "Arrival Slow Time", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Fast Time
        /// </summary>
        public static StandardMeasurement ArrivalFastTime { get; } =
            CreateValue(1337, "Arrival Fast Time", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Min Time
        /// </summary>
        public static StandardMeasurement ArrivalMinTime { get; } =
            CreateValue(1338, "Arrival Min Time", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Current Time
        /// </summary>
        public static StandardMeasurement ArrivalCurrentTime { get; } =
            CreateValue(1339, "Arrival Current Time", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Average Time
        /// </summary>
        public static StandardMeasurement ArrivalAverageTime { get; } =
            CreateValue(1340, "Arrival Average Time", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Max Count
        /// </summary>
        public static StandardMeasurement ArrivalMaxCount { get; } =
            CreateValue(1341, "Arrival Max Count", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Slow Count
        /// </summary>
        public static StandardMeasurement ArrivalSlowCount { get; } =
            CreateValue(1342, "Arrival Slow Count", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Normal Count
        /// </summary>
        public static StandardMeasurement ArrivalNormalCount { get; } =
            CreateValue(1343, "Arrival Normal Count", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Fast Count
        /// </summary>
        public static StandardMeasurement ArrivalFastCount { get; } =
            CreateValue(1344, "Arrival Fast Count", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Min Count
        /// </summary>
        public static StandardMeasurement ArrivalMinCount { get; } =
            CreateValue(1345, "Arrival Min Count", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Reset Counters
        /// </summary>
        public static StandardMeasurement ArrivalResetCounters { get; } =
            CreateValue(1314, "Arrival Reset Counters", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Timer Enable
        /// </summary>
        public static StandardMeasurement CloseTimerEnable { get; } =
            CreateValue(1285, "Close Timer Enable", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Timer Actual
        /// </summary>
        public static StandardMeasurement CloseTimerActual { get; } =
            CreateValue(1376, "Close Timer Actual", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Timer Setpoint
        /// </summary>
        public static StandardMeasurement CloseTimerSetpoint { get; } =
            CreateValue(1286, "Close Timer Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Turner Enable
        /// </summary>
        public static StandardMeasurement CloseTurnerEnable { get; } =
            CreateValue(1294, "Close Turner Enable", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Turner Actual
        /// </summary>
        public static StandardMeasurement CloseTurnerActual { get; } =
            CreateValue(1377, "Close Turner Actual", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Turner Setpoint
        /// </summary>
        public static StandardMeasurement CloseTurnerSetpoint { get; } =
            CreateValue(1293, "Close Turner Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Critical Turner Actual
        /// </summary>
        public static StandardMeasurement CloseCriticalTurnerActual { get; } =
            CreateValue(1392, "Close Critical Turner Actual", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Load Ratio Enable
        /// </summary>
        public static StandardMeasurement CloseLoadRatioEnable { get; } =
            CreateValue(1287, "Close Load Ratio Enable", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Load Ratio Actual
        /// </summary>
        public static StandardMeasurement CloseLoadRatioActual { get; } =
            CreateValue(1378, "Close Load Ratio Actual", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Load Ratio Setpoint
        /// </summary>
        public static StandardMeasurement CloseLoadRatioSetpoint { get; } =
            CreateValue(1288, "Close Load Ratio Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close DP Enable
        /// </summary>
        public static StandardMeasurement CloseDPEnable { get; } =
            CreateValue(1291, "Close DP Enable", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close DP Actual
        /// </summary>
        public static StandardMeasurement CloseDPActual { get; } =
            CreateValue(1379, "Close DP Actual", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close DP Setpoint
        /// </summary>
        public static StandardMeasurement CloseDPSetpoint { get; } =
            CreateValue(1292, "Close DP Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Flow Rate Enable
        /// </summary>
        public static StandardMeasurement CloseFlowRateEnable { get; } =
            CreateValue(1393, "Close Flow Rate Enable", UnitCategory.FluidRate);

        /// <summary>
        /// Specifies that the parameter represents the Close Flow Rate Setpoint
        /// </summary>
        public static StandardMeasurement CloseFlowRateSetpoint { get; } =
            CreateValue(1394, "Close Flow Rate Setpoint", UnitCategory.FluidRate);

        /// <summary>
        /// Specifies that the parameter represents the Close Casing Rise Enable
        /// </summary>
        public static StandardMeasurement CloseCasingRiseEnable { get; } =
            CreateValue(1290, "Close Casing Rise Enable", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Casing Rise Actual
        /// </summary>
        public static StandardMeasurement CloseCasingRiseActual { get; } =
            CreateValue(1380, "Close Casing Rise Actual", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Casing Rise Setpoint
        /// </summary>
        public static StandardMeasurement CloseCasingRiseSetpoint { get; } =
            CreateValue(1289, "Close Casing Rise Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Casing Slope Enable
        /// </summary>
        public static StandardMeasurement CloseCasingSlopeEnable { get; } =
            CreateValue(1295, "Close Casing Slope Enable", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Casing Slope Actual
        /// </summary>
        public static StandardMeasurement CloseCasingSlopeActual { get; } =
            CreateValue(1381, "Close Casing Slope Actual", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Casing Slope Setpoint
        /// </summary>
        public static StandardMeasurement CloseCasingSlopeSetpoint { get; } =
            CreateValue(1296, "Close Casing Slope Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Tubing Close Enable
        /// </summary>
        public static StandardMeasurement CloseTubingCloseEnable { get; } =
            CreateValue(1382, "Close Tubing Close Enable", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Tubing Close Actual
        /// </summary>
        public static StandardMeasurement CloseTubingCloseActual { get; } =
            CreateValue(1383, "Close Tubing Close Actual", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Tubing Close Setpoint
        /// </summary>
        public static StandardMeasurement CloseTubingCloseSetpoint { get; } =
            CreateValue(1384, "Close Tubing Close Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Casing Close Enable
        /// </summary>
        public static StandardMeasurement CloseCasingCloseEnable { get; } =
            CreateValue(1385, "Close Casing Close Enable", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Casing Close Actual
        /// </summary>
        public static StandardMeasurement CloseCasingCloseActual { get; } =
            CreateValue(1386, "Close Casing Close Actual", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Casing Close Setpoint
        /// </summary>
        public static StandardMeasurement CloseCasingCloseSetpoint { get; } =
            CreateValue(1387, "Close Casing Close Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Close1 Enable
        /// </summary>
        public static StandardMeasurement CloseClose1Enable { get; } =
            CreateValue(1297, "Close Close1 Enable", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Close1 Actual
        /// </summary>
        public static StandardMeasurement CloseClose1Actual { get; } =
            CreateValue(1388, "Close Close1 Actual", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Close1 Setpoint
        /// </summary>
        public static StandardMeasurement CloseClose1Setpoint { get; } =
            CreateValue(1298, "Close Close1 Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Close2 Enable
        /// </summary>
        public static StandardMeasurement CloseClose2Enable { get; } =
            CreateValue(1389, "Close Close2 Enable", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Close2 Actual
        /// </summary>
        public static StandardMeasurement CloseClose2Actual { get; } =
            CreateValue(1390, "Close Close2 Actual", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Close Close2 Setpoint
        /// </summary>
        public static StandardMeasurement CloseClose2Setpoint { get; } =
            CreateValue(1391, "Close Close2 Setpoint", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Status1
        /// </summary>
        public static StandardMeasurement ArrivalStatus1 { get; } =
            CreateValue(1315, "Arrival Status1", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Time1
        /// </summary>
        public static StandardMeasurement ArrivalTime1 { get; } =
            CreateValue(1325, "Arrival Time1", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Status2
        /// </summary>
        public static StandardMeasurement ArrivalStatus2 { get; } =
            CreateValue(1316, "Arrival Status2", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Time2
        /// </summary>
        public static StandardMeasurement ArrivalTime2 { get; } =
            CreateValue(1326, "Arrival Time2", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Status3
        /// </summary>
        public static StandardMeasurement ArrivalStatus3 { get; } =
            CreateValue(1317, "Arrival Status3", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Time3
        /// </summary>
        public static StandardMeasurement ArrivalTime3 { get; } =
            CreateValue(1327, "Arrival Time3", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Status4
        /// </summary>
        public static StandardMeasurement ArrivalStatus4 { get; } =
            CreateValue(1318, "Arrival Status4", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Time4
        /// </summary>
        public static StandardMeasurement ArrivalTime4 { get; } =
            CreateValue(1328, "Arrival Time4", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Status5
        /// </summary>
        public static StandardMeasurement ArrivalStatus5 { get; } =
            CreateValue(1319, "Arrival Status5", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Time5
        /// </summary>
        public static StandardMeasurement ArrivalTime5 { get; } =
            CreateValue(1329, "Arrival Time5", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Status6
        /// </summary>
        public static StandardMeasurement ArrivalStatus6 { get; } =
            CreateValue(1320, "Arrival Status6", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Time6
        /// </summary>
        public static StandardMeasurement ArrivalTime6 { get; } =
            CreateValue(1330, "Arrival Time6", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Status7
        /// </summary>
        public static StandardMeasurement ArrivalStatus7 { get; } =
            CreateValue(1321, "Arrival Status7", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Time7
        /// </summary>
        public static StandardMeasurement ArrivalTime7 { get; } =
            CreateValue(1331, "Arrival Time7", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Status8
        /// </summary>
        public static StandardMeasurement ArrivalStatus8 { get; } =
            CreateValue(1322, "Arrival Status8", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Time8
        /// </summary>
        public static StandardMeasurement ArrivalTime8 { get; } =
            CreateValue(1332, "Arrival Time8", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Status9
        /// </summary>
        public static StandardMeasurement ArrivalStatus9 { get; } =
            CreateValue(1323, "Arrival Status9", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Time9
        /// </summary>
        public static StandardMeasurement ArrivalTime9 { get; } =
            CreateValue(1333, "Arrival Time9", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Status10
        /// </summary>
        public static StandardMeasurement ArrivalStatus10 { get; } =
            CreateValue(1324, "Arrival Status10", UnitCategory.None);

        /// <summary>
        /// Specifies that the parameter represents the Arrival Time10
        /// </summary>
        public static StandardMeasurement ArrivalTime10 { get; } =
            CreateValue(1334, "Arrival Time10", UnitCategory.None);

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new StandardMeasurement with a specified key and name.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        public StandardMeasurement(int key, Text name)
            : this(key, name, UnitCategory.None)
        {

        }

        /// <summary>
        /// Initializes a new StandardMeasurement with a specified key, name, and unit category.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        /// <param name="unitCategory">The unit category associated with the measuremnt.</param>
        public StandardMeasurement(int key, Text name, UnitCategory unitCategory)
        {
            Key = key;
            Name = name;
            UnitCategory = unitCategory;
        }

        #endregion

        #region Static Methods

        private static StandardMeasurement CreateValue(int key, string name, UnitCategory unitCategory)
        {
            StandardMeasurement value = new StandardMeasurement(key, new Text(name), unitCategory);

            Register(value);

            return value;
        }

        /// <summary>
        /// Gets the value for the specified type and key.
        /// </summary>
        /// <param name="key">The key of the value.</param>
        /// <returns>
        /// The value found for the specified type and key; otherwise, a new value using UnsupportedName.
        /// </returns>
        /// <exception cref="MissingMethodException">
        /// The specified type does not contain a constructor that takes a key and a name.
        /// </exception>
        public static StandardMeasurement GetValue(int key)
        {
            StandardMeasurement result = null;
            IDictionary<int, StandardMeasurement> values = GetDictionary<StandardMeasurement>();

            if (values != null)
            {
                //synchronize access
                lock (values)
                {
                    if (values.TryGetValue(key, out var temp))
                    {
                        result = temp;
                    }
                }
            }

            return result ?? CreateInternal<StandardMeasurement>(key, new Text("Unsupported"), false);
        }

        /// <summary>
        /// Gets the EnhancedEnumBase.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>A dictionary of Id and EnhancedEnumBase.</returns>
        public static IDictionary<int, StandardMeasurement> GetDictionary<T>()
        {
            return GetDictionary(typeof(T));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Registers a value to be managed by EnhancedEnumBase.
        /// </summary>
        /// <param name="value">The value to register.</param>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="ArgumentException">
        /// A value with the same key already exists for the value's type.
        /// </exception>
        private static void Register(StandardMeasurement value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Type type = value.GetType();
            IDictionary<int, StandardMeasurement> values = GetDictionary(type);

            if (values == null)
            {
                values = new SortedDictionary<int, StandardMeasurement>(); //sort based on keys

                //synchronize access
                lock (_dictionary)
                {
                    _dictionary[type] = values;
                }
            }

            //synchronize access
            lock (values)
            {
                values.Add(value.Key, value);
            }
        }

        private static IDictionary<int, StandardMeasurement> GetDictionary(Type type)
        {
            IDictionary<int, StandardMeasurement> result = null;

            //synchronize access
            lock (_dictionary)
            {
                _dictionary.TryGetValue(type, out result);
            }

            if (result == null)
            {
                //Create an instance in case the static constructor wasn't called yet
                CreateInternal(type, 0, Text.Empty);

                //synchronize access
                lock (_dictionary)
                {
                    _dictionary.TryGetValue(type, out result);
                }
            }

            return result;
        }

        private static T CreateInternal<T>(int key, Text name, bool isSupported)
        {
            return (T)CreateInternal(typeof(T), key, name, isSupported);
        }

        private static object CreateInternal(Type type, int key, Text name)
        {
            return CreateInternal(type, key, name, true);
        }

        private static object CreateInternal(Type type, int key, Text name, bool isSupported)
        {
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            object value = Activator.CreateInstance(type, flags, null, new object[] { key, name }, null);
            if (value is StandardMeasurement standardMeasurement)
            {
                standardMeasurement.IsSupported = isSupported;
            }

            return value;
        }

        #endregion

    }
}
