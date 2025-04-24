using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The well details table.
    /// </summary>
    [Table("tblWellDetails")]
    public class WellDetailsEntity
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        [Column("NodeID", TypeName = "nvarchar")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MaxLength(50)]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the run time.
        /// </summary>
        [Column("Runtime")]
        public float? Runtime { get; set; }

        /// <summary>
        /// Gets or sets the pump depth. Represents the measured pump depth.
        /// </summary>
        [Column("PumpDepth")]
        public short? PumpDepth { get; set; }

        /// <summary>
        /// Gets or sets the tubing pressure.
        /// </summary>
        [Column("TubingPressure")]
        public short? TubingPressure { get; set; }

        /// <summary>
        /// Gets or sets the casing pressure.
        /// </summary>
        [Column("CasingPressure")]
        public short? CasingPressure { get; set; }

        /// <summary>
        /// Gets or sets the water cut.
        /// </summary>
        [Column("WaterCut")]
        public float? WaterCut { get; set; }

        /// <summary>
        /// Gets or sets the water specific gravity.
        /// </summary>
        [Column("WaterSG")]
        public float? WaterSpecificGravity { get; set; }

        /// <summary>
        /// Gets or sets the oil specific gravity, measured in API.
        /// </summary>
        [Column("OilAPIGravity")]
        public float? OilAPI { get; set; }

        /// <summary>
        /// Gets or sets the fluid level.
        /// </summary>
        [Column("FluidLevel")]
        public short? FluidLevel { get; set; }

        /// <summary>
        /// Gets or sets the strokes per minute.
        /// </summary>
        [Column("SPM")]
        public float? StrokesPerMinute { get; set; }

        /// <summary>
        /// Gets or sets the gross rate.
        /// </summary>
        [Column("GrossRate")]
        public float? GrossRate { get; set; }

        /// <summary>
        /// Gets or sets the tubing outer diameter.
        /// </summary>
        [Column("TubingOD")]
        public float? TubingOuterDiameter { get; set; }

        /// <summary>
        /// Gets or sets the tubing anchor depth.
        /// </summary>
        [Column("TubingAnchorDepth")]
        public short? TubingAnchorDepth { get; set; }

        /// <summary>
        /// Gets or sets the type of pump.
        /// </summary>
        [Column("PumpType", TypeName = "nvarchar")]
        [MaxLength(1)]
        public string PumpType { get; set; }

        /// <summary>
        /// Gets or sets the plunger diameter.
        /// </summary>
        [Column("PlungerDiam")]
        public float? PlungerDiameter { get; set; }

        /// <summary>
        /// Gets or sets the number of rod sections.
        /// </summary>
        [Column("NumRodSections")]
        public short? NumberOfRodSections { get; set; }

        /// <summary>
        /// Gets or sets the total length of the rod string.
        /// </summary>
        [Column("TotalRodLength")]
        public int? TotalRodLength { get; set; }

        /// <summary>
        /// Gets or sets the rod length adjustment.
        /// </summary>
        [Column("RodLengthAdjust")]
        public int? RodLengthAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the rod tubing friction.
        /// </summary>
        [Column("RodTubingFriction")]
        public float? RodTubingFriction { get; set; }

        /// <summary>
        /// Gets or sets the service factor.
        /// </summary>
        [Column("ServiceFactor")]
        public float? ServiceFactor { get; set; }

        /// <summary>
        /// Gets or sets the pumping unit id.
        /// </summary>
        [Column("PumpingUnitID")]
        [MaxLength(6)]
        public string PumpingUnitId { get; set; }

        /// <summary>
        /// Gets or sets the maximum counter balance moment.
        /// </summary>
        [Column("MaxCBMoment")]
        public int? MaximumCounterBalanceMoment { get; set; }

        /// <summary>
        /// Gets or sets the rotation of a rod pumping unit.
        /// </summary>
        [Column("Rotation")]
        [MaxLength(3)]
        public string Rotation { get; set; }

        /// <summary>
        /// Gets or sets the current crankhole's number.
        /// </summary>
        [Column("HoleNumber")]
        public short? CrankholeNumber { get; set; }

        /// <summary>
        /// Gets or sets the type of the power meter.
        /// </summary>
        [Column("PowerMeterType")]
        public string PowerMeterType { get; set; }

        /// <summary>
        /// Gets or sets the type of the prime mover.
        /// </summary>
        [Column("PrimeMoverType")]
        [MaxLength(4)]
        public string PrimeMoverType { get; set; }

        /// <summary>
        /// Gets or sets the motor horse power.
        /// </summary>
        [Column("MotorHP")]
        public float? MotorHorsePower { get; set; }

        /// <summary>
        /// Gets or sets the torque mode.
        /// </summary>
        [Column("TorqueMode")]
        [MaxLength(2)]
        public string TorqueMode { get; set; }

        /// <summary>
        /// Gets or sets the stroke length.
        /// </summary>
        [Column("StrokeLength")]
        public float? StrokeLength { get; set; }

        /// <summary>
        /// Gets or sets whether the XDG ( XDIAG ) file is available. No longer populated from XSPOC.
        /// </summary>
        [Column("XDGFileAvailable")]
        public bool XDGFileAvailable { get; set; }

        ///<summary>
        ///Gets or sets the down reason code.
        ///</summary>
        [Column("DownReasonCode")]
        [MaxLength(10)]
        public string DownReasonCode { get; set; }

        /// <summary>
        /// Gets or sets the Gross rate as provided by the pump off controller.
        /// </summary>
        [Column("POCGrossRate")]
        public short? POCGrossRate { get; set; }

        /// <summary>
        /// Gets or sets the cycles.
        /// </summary>
        [Column("Cycles")]
        public float? Cycles { get; set; }

        /// <summary>
        /// Gets or sets the date of the newest well test from tblWellTests.
        /// </summary>
        [Column("LastTestDate")]
        public DateTime? LastWellTestDate { get; set; }

        /// <summary>
        /// Gets or sets the depth of the topmost perforation for the well.
        /// </summary>
        [Column("TopPerf")]
        public int? DepthOfTopmostPerforation { get; set; }

        /// <summary>
        /// Gets or sets the depth of the bottommost perforation for the well.
        /// </summary>
        [Column("BotPerf")]
        public int? DepthOfBottommostPerforation { get; set; }

        /// <summary>
        /// Gets or sets the fluid temperature.
        /// </summary>
        [Column("FluidTemp")]
        public int? FluidTemperature { get; set; }

        /// <summary>
        /// Gets or sets the cost of electricity.
        /// </summary>
        [Column("ElectricityCost")]
        public float? ElectricityCost { get; set; }

        /// <summary>
        /// Gets or sets whether the tubing is anchored.
        /// </summary>
        [Column("TubingAnchored")]
        public bool? IsTubingAnchored { get; set; }

        /// <summary>
        /// Gets or sets the tubing inner diameter.
        /// </summary>
        [Column("TubingID")]
        public float? TubingInnerDiameter { get; set; }

        /// <summary>
        /// Gets or sets the source of fluid level information.
        /// </summary>
        [Column("FluidLevelSource")]
        public int? FluidLevelSource { get; set; }

        /// <summary>
        /// Gets or sets the pump intake pressure.
        /// </summary>
        [Column("PumpIntakePressure")]
        public float? PumpIntakePressure { get; set; }

        /// <summary>
        /// Gets or sets the motor type id.
        /// </summary>
        [Column("MotorTypeID")]
        public int? MotorTypeId { get; set; }

        /// <summary>
        /// Gets or sets the motor size id.
        /// </summary>
        [Column("MotorSizeID")]
        public int? MotorSizeId { get; set; }

        /// <summary>
        /// Gets or sets the motor setting id.
        /// </summary>
        [Column("MotorSettingID")]
        public int? MotorSettingId { get; set; }

        /// <summary>
        /// Gets or sets the peak load.
        /// </summary>
        [Column("PeakLoad")]
        public int? PeakLoad { get; set; }

        /// <summary>
        /// Gets or sets the minimum load.
        /// </summary>
        [Column("MinLoad")]
        public int? MinimumLoad { get; set; }

        /// <summary>
        /// Gets or sets the type of the load sensor.
        /// </summary>
        [Column("LoadSensorType")]
        public int? LoadSensorType { get; set; }

        /// <summary>
        /// Gets or sets the idle time.
        /// </summary>
        [Column("IdleTime")]
        public float? IdleTime { get; set; }

        /// <summary>
        /// Gets or sets the value of the DynoCorrectKinematic flag that is passed to XDIAG for XDIAG runs.
        /// </summary>
        [Column("DynoCorrectKinematic")]
        public int? DynoCorrectKinematic { get; set; }

        /// <summary>
        /// Gets or sets the value of the DynoCorrectPhase flag to pass to XDIAG for runs on this well.
        /// </summary>
        [Column("DynoCorrectPhase")]
        public int? DynoCorrectPhase { get; set; }

        /// <summary>
        /// Gets or sets the value of the DynoPhaseAdjustment flag to pass to XDIAG for runs on this well.
        /// </summary>
        [Column("DynoPhaseAdjustment")]
        public float? DynoPhaseAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the value of the DynoTOSAdjustment ( top of stroke ) flag to pass to XDIAG for runs on this well.
        /// </summary>
        [Column("DynoTOSAdjustment")]
        public float? DynoTOSAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the value of the DynoLoadAdjustment flag to pass to XDIAG for runs on this well.
        /// </summary>
        [Column("DynoLoadAdjustment")]
        public int? DynoLoadAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the gas rate.
        /// </summary>
        [Column("GasRate")]
        public float? GasRate { get; set; }

        /// <summary>
        /// Gets or sets the length of the pump barrel.
        /// </summary>
        [Column("PumpBarrelLength")]
        public float? PumpBarrelLength { get; set; }

        /// <summary>
        /// Gets or sets the traveling valve valve check load.
        /// </summary>
        [Column("ValveCheckTVLoad")]
        public int? ValveCheckTravelingValveLoad { get; set; }

        /// <summary>
        /// Gets or sets the standing valve valve check load.
        /// </summary>
        [Column("ValveCheckSVLoad")]
        public int? ValveCheckStandingValveLoad { get; set; }

        /// <summary>
        /// Gets or sets the leakage from a valve check.
        /// </summary>
        [Column("ValveCheckLeakage")]
        public int? ValveCheckLeakage { get; set; }

        /// <summary>
        /// Gets or sets the counterbalance type.
        /// </summary>
        [Column("CBDataType")]
        public int? CounterBalanceDataType { get; set; }

        /// <summary>
        /// Gets or sets the counterbalance angle.
        /// </summary>
        [Column("CBCrankAngle")]
        public int? CounterBalanceCrankAngle { get; set; }

        /// <summary>
        /// Gets or sets the viscosity.
        /// </summary>
        [Column("Viscosity")]
        public float? Viscosity { get; set; }

        /// <summary>
        /// Gets or sets the clearance.
        /// </summary>
        [Column("Clearance")]
        public float? Clearance { get; set; }

        /// <summary>
        /// Gets or sets the date for the daily volume calculations.
        /// </summary>
        [Column("CalcDailyDate")]
        public DateTime? CalcDailyDate { get; set; }

        /// <summary>
        /// Gets or sets the water rate from the daily volume calculations.
        /// </summary>
        [Column("CalcDailyWater")]
        public float? CalcDailyWater { get; set; }

        /// <summary>
        /// Gets or sets the oil rate from the daily volume calculations.
        /// </summary>
        [Column("CalcDailyOil")]
        public float? CalcDailyOil { get; set; }

        /// <summary>
        /// Gets or sets the gas rate from the daily volume calculations.
        /// </summary>
        [Column("CalcDailyGas")]
        public float? CalcDailyGas { get; set; }

        /// <summary>
        /// Gets or sets the difference in water rate from the daily volume calculations vs well test.
        /// </summary>
        [Column("DiffDailyWater")]
        public float? DiffDailyWater { get; set; }

        /// <summary>
        /// Gets or sets the difference in oil rate from the daily volume calculations vs well test.
        /// </summary>
        [Column("DiffDailyOil")]
        public float? DiffDailyOil { get; set; }

        /// <summary>
        /// Gets or sets the difference in gas rate from the daily volume calculations vs well test.
        /// </summary>
        [Column("DiffDailyGas")]
        public float? DiffDailyGas { get; set; }

        /// <summary>
        /// Gets or sets the difference in gross rate from the daily volume calculations vs well test.
        /// </summary>
        [Column("DiffDailyGross")]
        public float? DiffDailyGross { get; set; }

        /// <summary>
        /// Gets or sets the static reservoir pressure.
        /// </summary>
        [Column("StaticReservoirPressure")]
        public float? StaticReservoirPressure { get; set; }

        /// <summary>
        /// Gets or sets the gross rate at test. 
        /// Used to calculate IPR when a well does not have time series IPR due to its lift type.
        /// </summary>
        [Column("RateAtTest")]
        public float? GrossRateAtTest { get; set; }

        /// <summary>
        /// Gets or sets the flowing bottomhole pressure at test. 
        /// Used to calculate IPR when a well does not have time series IPR due to its lift type.
        /// </summary>
        [Column("FlowPressureAtTest")]
        public float? FlowingBottomholePressureAtTest { get; set; }

        /// <summary>
        /// Gets or sets the tubing friction coefficient.
        /// </summary>
        [Column("TubingFrictionCoefficient")]
        public float? TubingFrictionCoefficient { get; set; }

        /// <summary>
        /// Gets or sets the vertical pump depth.
        /// </summary>
        [Column("PumpDepthVertical")]
        public int? PumpDepthVertical { get; set; }

        /// <summary>
        /// Gets or sets the fluid specific gravity.
        /// </summary>
        [Column("FluidSpecificGravity")]
        public float? FluidSpecificGravity { get; set; }

        /// <summary>
        /// Gets or sets the install date of the plunger.
        /// </summary>
        [Column("PlungerInstallDate")]
        public DateTime? PlungerInstallDate { get; set; }

        /// <summary>
        /// Gets or sets the model of the plunger.
        /// </summary>
        [Column("PlungerModel", TypeName = "nvarchar")]
        [MaxLength(20)]
        public string PlungerModel { get; set; }

        /// <summary>
        /// Gets or sets the well number ( used by PCS wells ).
        /// </summary>
        [Column("WellNumber")]
        public int? WellNumber { get; set; }

        /// <summary>
        /// Gets or sets the field name.
        /// </summary>
        [Column("FieldName", TypeName = "nvarchar")]
        [MaxLength(30)]
        public string FieldName { get; set; }

        /// <summary>
        /// Gets or sets the pressure target.
        /// </summary>
        [Column("PressureTarget")]
        public float? PressureTarget { get; set; }

        /// <summary>
        /// Gets or sets the serial number of the plunger.
        /// </summary>
        [Column("PlungerSerial", TypeName = "nvarchar")]
        [MaxLength(20)]
        public string PlungerSerial { get; set; }

        /// <summary>
        /// Gets or sets the well name.
        /// </summary>
        [Column("WellName", TypeName = "nvarchar")]
        [MaxLength(30)]
        public string WellName { get; set; }

        /// <summary>
        /// Gets or sets the tubing pipe quality factor.
        /// </summary>
        [Column("TubingPipeQualityFactor")]
        public int? TubingPipeQualityFactor { get; set; }

        /// <summary>
        /// Gets or sets the bubblepoint pressure.
        /// </summary>
        [Column("BubblepointPressure")]
        public float? BubblepointPressure { get; set; }

        /// <summary>
        /// Gets or sets whether the well has a pump intake pressure sensor installed.
        /// </summary>
        [Column("PIPSensorInstalled")]
        public bool PumpIntakePressureSensorInstalled { get; set; }

        ///<summary>
        /// Gets or sets the XBAL maximum counterbalance moment.
        /// </summary>
        [Column("XBALMaxCBMoment")]
        public short? XBALMaximumCounterBalanceMoment { get; set; }

        /// <summary>
        /// Gets or sets the air tank pressure at the bottom of stroke.
        /// </summary>
        [Column("AirTankPressureBottomOfStroke")]
        public float? AirTankPressureBottomOfStroke { get; set; }

        /// <summary>
        /// Gets or sets the gas specific gravity.
        /// </summary>
        [Column("SpecificGravityOfGas")]
        public float? GasSpecificGravity { get; set; }

        /// <summary>
        /// Gets or sets whether the casing valve is closed.
        /// </summary>
        [Column("CasingValveClosed")]
        public bool IsCasingValveClosed { get; set; }

        ///<summary>
        /// Gets or sets the bottomhole temperature.
        /// </summary>
        [Column("BottomholeTemperature")]
        public float? BottomholeTemperature { get; set; }

        /// <summary>
        /// Gets or sets the production depth.
        /// </summary>
        [Column("ProductionDepth")]
        public float? ProductionDepth { get; set; }

        /// <summary>
        /// Gets or sets the packer depth.
        /// </summary>
        [Column("PackerDepth")]
        public float? PackerDepth { get; set; }

        /// <summary>
        /// Gets or sets whether the well has a packer.
        /// </summary>
        [Column("HasPacker")]
        public bool HasPacker { get; set; }

        /// <summary>
        /// Gets or sets the id that describes the type of downhole separator.
        /// </summary>
        [Column("DownholeSeparatorID")]
        public int? DownholeSeparatorId { get; set; }

        /// <summary>
        /// Gets or sets the pumping unit installation date.
        /// </summary>
        [Column("PumpingUnitInstallDate")]
        public DateTime? PumpingUnitInstallDate { get; set; }

    }
}
