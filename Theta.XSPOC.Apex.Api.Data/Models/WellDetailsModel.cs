using System;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the well deatils data model.
    /// </summary>
    public class WellDetailsModel
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the run time.
        /// </summary>
        public float? Runtime { get; set; }

        /// <summary>
        /// Gets or sets the pump depth. Represents the measured pump depth.
        /// </summary>
        public short? PumpDepth { get; set; }

        /// <summary>
        /// Gets or sets the tubing pressure.
        /// </summary>
        public short? TubingPressure { get; set; }

        /// <summary>
        /// Gets or sets the casing pressure.
        /// </summary>
        public short? CasingPressure { get; set; }

        /// <summary>
        /// Gets or sets the water cut.
        /// </summary>
        public float? WaterCut { get; set; }

        /// <summary>
        /// Gets or sets the water specific gravity.
        /// </summary>
        public float? WaterSpecificGravity { get; set; }

        /// <summary>
        /// Gets or sets the oil specific gravity, measured in API.
        /// </summary>
        public float? OilAPI { get; set; }

        /// <summary>
        /// Gets or sets the fluid level.
        /// </summary>
        public short? FluidLevel { get; set; }

        /// <summary>
        /// Gets or sets the strokes per minute.
        /// </summary>
        public float? StrokesPerMinute { get; set; }

        /// <summary>
        /// Gets or sets the gross rate.
        /// </summary>
        public float? GrossRate { get; set; }

        /// <summary>
        /// Gets or sets the tubing outer diameter.
        /// </summary>
        public float? TubingOuterDiameter { get; set; }

        /// <summary>
        /// Gets or sets the tubing anchor depth.
        /// </summary>
        public short? TubingAnchorDepth { get; set; }

        /// <summary>
        /// Gets or sets the type of pump.
        /// </summary>
        public string PumpType { get; set; }

        /// <summary>
        /// Gets or sets the plunger diameter.
        /// </summary>
        public float? PlungerDiameter { get; set; }

        /// <summary>
        /// Gets or sets the number of rod sections.
        /// </summary>
        public short? NumberOfRodSections { get; set; }

        /// <summary>
        /// Gets or sets the total length of the rod string.
        /// </summary>
        public int? TotalRodLength { get; set; }

        /// <summary>
        /// Gets or sets the rod length adjustment.
        /// </summary>
        public int? RodLengthAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the rod tubing friction.
        /// </summary>
        public float? RodTubingFriction { get; set; }

        /// <summary>
        /// Gets or sets the service factor.
        /// </summary>
        public float? ServiceFactor { get; set; }

        /// <summary>
        /// Gets or sets the pumping unit id.
        /// </summary>
        public string PumpingUnitId { get; set; }

        /// <summary>
        /// Gets or sets the maximum counter balance moment.
        /// </summary>
        public int? MaximumCounterBalanceMoment { get; set; }

        /// <summary>
        /// Gets or sets the rotation of a rod pumping unit.
        /// </summary>
        public string Rotation { get; set; }

        /// <summary>
        /// Gets or sets the current crankhole's number.
        /// </summary>
        public short? CrankholeNumber { get; set; }

        /// <summary>
        /// Gets or sets the type of the power meter.
        /// </summary>
        public string PowerMeterType { get; set; }

        /// <summary>
        /// Gets or sets the type of the prime mover.
        /// </summary>
        public string PrimeMoverType { get; set; }

        /// <summary>
        /// Gets or sets the motor horse power.
        /// </summary>
        public float? MotorHorsePower { get; set; }

        /// <summary>
        /// Gets or sets the torque mode.
        /// </summary>
        public string TorqueMode { get; set; }

        /// <summary>
        /// Gets or sets the stroke length.
        /// </summary>
        public float? StrokeLength { get; set; }

        /// <summary>
        /// Gets or sets whether the XDG ( XDIAG ) file is available. No longer populated from XSPOC.
        /// </summary>
        public bool XDGFileAvailable { get; set; }

        ///<summary>
        ///Gets or sets the down reason code.
        ///</summary>
        public string DownReasonCode { get; set; }

        /// <summary>
        /// Gets or sets the Gross rate as provided by the pump off controller.
        /// </summary>
        public short? POCGrossRate { get; set; }

        /// <summary>
        /// Gets or sets the cycles.
        /// </summary>
        public float? Cycles { get; set; }

        /// <summary>
        /// Gets or sets the date of the newest well test from tblWellTests.
        /// </summary>
        public DateTime? LastWellTestDate { get; set; }

        /// <summary>
        /// Gets or sets the depth of the topmost perforation for the well.
        /// </summary>
        public int? DepthOfTopmostPerforation { get; set; }

        /// <summary>
        /// Gets or sets the depth of the bottommost perforation for the well.
        /// </summary>
        public int? DepthOfBottommostPerforation { get; set; }

        /// <summary>
        /// Gets or sets the fluid temperature.
        /// </summary>
        public int? FluidTemperature { get; set; }

        /// <summary>
        /// Gets or sets the cost of electricity.
        /// </summary>
        public float? ElectricityCost { get; set; }

        /// <summary>
        /// Gets or sets whether the tubing is anchored.
        /// </summary>
        public bool? IsTubingAnchored { get; set; }

        /// <summary>
        /// Gets or sets the tubing inner diameter.
        /// </summary>
        public float? TubingInnerDiameter { get; set; }

        /// <summary>
        /// Gets or sets the source of fluid level information.
        /// </summary>
        public int? FluidLevelSource { get; set; }

        /// <summary>
        /// Gets or sets the pump intake pressure.
        /// </summary>
        public float? PumpIntakePressure { get; set; }

        /// <summary>
        /// Gets or sets the motor type id.
        /// </summary>
        public int? MotorTypeId { get; set; }

        /// <summary>
        /// Gets or sets the motor size id.
        /// </summary>
        public int? MotorSizeId { get; set; }

        /// <summary>
        /// Gets or sets the motor setting id.
        /// </summary>
        public int? MotorSettingId { get; set; }

        /// <summary>
        /// Gets or sets the peak load.
        /// </summary>
        public int? PeakLoad { get; set; }

        /// <summary>
        /// Gets or sets the minimum load.
        /// </summary>
        public int? MinimumLoad { get; set; }

        /// <summary>
        /// Gets or sets the type of the load sensor.
        /// </summary>
        public int? LoadSensorType { get; set; }

        /// <summary>
        /// Gets or sets the idle time.
        /// </summary>
        public float? IdleTime { get; set; }

        /// <summary>
        /// Gets or sets the value of the DynoCorrectKinematic flag that is passed to XDIAG for XDIAG runs.
        /// </summary>
        public int? DynoCorrectKinematic { get; set; }

        /// <summary>
        /// Gets or sets the value of the DynoCorrectPhase flag to pass to XDIAG for runs on this well.
        /// </summary>
        public int? DynoCorrectPhase { get; set; }

        /// <summary>
        /// Gets or sets the value of the DynoPhaseAdjustment flag to pass to XDIAG for runs on this well.
        /// </summary>
        public float? DynoPhaseAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the value of the DynoTOSAdjustment ( top of stroke ) flag to pass to XDIAG for runs on this well.
        /// </summary>
        public float? DynoTOSAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the value of the DynoLoadAdjustment flag to pass to XDIAG for runs on this well.
        /// </summary>
        public int? DynoLoadAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the gas rate.
        /// </summary>
        public float? GasRate { get; set; }

        /// <summary>
        /// Gets or sets the length of the pump barrel.
        /// </summary>
        public float? PumpBarrelLength { get; set; }

        /// <summary>
        /// Gets or sets the traveling valve valve check load.
        /// </summary>
        public int? ValveCheckTravelingValveLoad { get; set; }

        /// <summary>
        /// Gets or sets the standing valve valve check load.
        /// </summary>
        public int? ValveCheckStandingValveLoad { get; set; }

        /// <summary>
        /// Gets or sets the leakage from a valve check.
        /// </summary>
        public int? ValveCheckLeakage { get; set; }

        /// <summary>
        /// Gets or sets the counterbalance type.
        /// </summary>
        public int? CounterBalanceDataType { get; set; }

        /// <summary>
        /// Gets or sets the counterbalance angle.
        /// </summary>
        public int? CounterBalanceCrankAngle { get; set; }

        /// <summary>
        /// Gets or sets the viscosity.
        /// </summary>
        public float? Viscosity { get; set; }

        /// <summary>
        /// Gets or sets the clearance.
        /// </summary>
        public float? Clearance { get; set; }

        /// <summary>
        /// Gets or sets the date for the daily volume calculations.
        /// </summary>
        public DateTime? CalcDailyDate { get; set; }

        /// <summary>
        /// Gets or sets the water rate from the daily volume calculations.
        /// </summary>
        public float? CalcDailyWater { get; set; }

        /// <summary>
        /// Gets or sets the oil rate from the daily volume calculations.
        /// </summary>
        public float? CalcDailyOil { get; set; }

        /// <summary>
        /// Gets or sets the gas rate from the daily volume calculations.
        /// </summary>
        public float? CalcDailyGas { get; set; }

        /// <summary>
        /// Gets or sets the difference in water rate from the daily volume calculations vs well test.
        /// </summary>
        public float? DiffDailyWater { get; set; }

        /// <summary>
        /// Gets or sets the difference in oil rate from the daily volume calculations vs well test.
        /// </summary>
        public float? DiffDailyOil { get; set; }

        /// <summary>
        /// Gets or sets the difference in gas rate from the daily volume calculations vs well test.
        /// </summary>
        public float? DiffDailyGas { get; set; }

        /// <summary>
        /// Gets or sets the difference in gross rate from the daily volume calculations vs well test.
        /// </summary>
        public float? DiffDailyGross { get; set; }

        /// <summary>
        /// Gets or sets the static reservoir pressure.
        /// </summary>
        public float? StaticReservoirPressure { get; set; }

        /// <summary>
        /// Gets or sets the gross rate at test. 
        /// Used to calculate IPR when a well does not have time series IPR due to its lift type.
        /// </summary>
        public float? GrossRateAtTest { get; set; }

        /// <summary>
        /// Gets or sets the flowing bottomhole pressure at test. 
        /// Used to calculate IPR when a well does not have time series IPR due to its lift type.
        /// </summary>
        public float? FlowingBottomholePressureAtTest { get; set; }

        /// <summary>
        /// Gets or sets the tubing friction coefficient.
        /// </summary>
        public float? TubingFrictionCoefficient { get; set; }

        /// <summary>
        /// Gets or sets the vertical pump depth.
        /// </summary>
        public int? PumpDepthVertical { get; set; }

        /// <summary>
        /// Gets or sets the fluid specific gravity.
        /// </summary>
        public float? FluidSpecificGravity { get; set; }

        /// <summary>
        /// Gets or sets the install date of the plunger.
        /// </summary>
        public DateTime? PlungerInstallDate { get; set; }

        /// <summary>
        /// Gets or sets the model of the plunger.
        /// </summary>
        public string PlungerModel { get; set; }

        /// <summary>
        /// Gets or sets the well number ( used by PCS wells ).
        /// </summary>
        public int? WellNumber { get; set; }

        /// <summary>
        /// Gets or sets the field name.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the pressure target.
        /// </summary>
        public float? PressureTarget { get; set; }

        /// <summary>
        /// Gets or sets the serial number of the plunger.
        /// </summary>
        public string PlungerSerial { get; set; }

        /// <summary>
        /// Gets or sets the well name.
        /// </summary>
        public string WellName { get; set; }

        /// <summary>
        /// Gets or sets the tubing pipe quality factor.
        /// </summary>
        public int? TubingPipeQualityFactor { get; set; }

        /// <summary>
        /// Gets or sets the bubblepoint pressure.
        /// </summary>
        public float? BubblepointPressure { get; set; }

        /// <summary>
        /// Gets or sets whether the well has a pump intake pressure sensor installed.
        /// </summary>
        public bool PumpIntakePressureSensorInstalled { get; set; }

        ///<summary>
        /// Gets or sets the XBAL maximum counterbalance moment.
        /// </summary>
        public short? XBALMaximumCounterBalanceMoment { get; set; }

        /// <summary>
        /// Gets or sets the air tank pressure at the bottom of stroke.
        /// </summary>
        public float? AirTankPressureBottomOfStroke { get; set; }

        /// <summary>
        /// Gets or sets the gas specific gravity.
        /// </summary>
        public float? GasSpecificGravity { get; set; }

        /// <summary>
        /// Gets or sets whether the casing valve is closed.
        /// </summary>
        public bool IsCasingValveClosed { get; set; }

        ///<summary>
        /// Gets or sets the bottomhole temperature.
        /// </summary>
        public float? BottomholeTemperature { get; set; }

        /// <summary>
        /// Gets or sets the production depth.
        /// </summary>
        public float? ProductionDepth { get; set; }

        /// <summary>
        /// Gets or sets the packer depth.
        /// </summary>
        public float? PackerDepth { get; set; }

        /// <summary>
        /// Gets or sets whether the well has a packer.
        /// </summary>
        public bool HasPacker { get; set; }

        /// <summary>
        /// Gets or sets the id that describes the type of downhole separator.
        /// </summary>
        public int? DownholeSeparatorId { get; set; }

        /// <summary>
        /// Gets or sets the pumping unit installation date.
        /// </summary>
        public DateTime? PumpingUnitInstallDate { get; set; }

    }
}
