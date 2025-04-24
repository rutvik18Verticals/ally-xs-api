using System;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the xdiag data model.
    /// </summary>
    public class XDiagResultsModel
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the additional uplift.
        /// </summary>
        public float? AdditionalUplift { get; set; }

        /// <summary>
        /// Gets or sets the additional uplift gross.
        /// </summary>
        public float? AdditionalUpliftGross { get; set; }

        /// <summary>
        /// Gets or sets the uplift calculation missing requirements.
        /// </summary>
        public string UpliftCalculationMissingRequirements { get; set; }

        /// <summary>
        /// Gets or sets the pump intake pressure from XDiag result.
        /// </summary>
        public int? PumpIntakePressure { get; set; }

        /// <summary>
        /// Gets or sets the pump size.
        /// </summary>
        public float? PumpSize { get; set; }

        /// <summary>
        /// Gets or sets the buoyant rod weight.
        /// </summary>
        public int? BouyRodWeight { get; set; }

        /// <summary>
        /// Gets or sets the fluid load on pump.
        /// </summary>
        public int? FluidLoadonPump { get; set; }

        /// <summary>
        /// Gets or sets the gross pump stroke.
        /// </summary>
        public short? GrossPumpStroke { get; set; }

        /// <summary>
        /// Gets or sets the downhole analysis.
        /// </summary>
        public string DownholeAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the rod analysis.
        /// </summary>
        public string RodAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the surface analysis.
        /// </summary>
        public string SurfaceAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the input analysis.
        /// </summary>
        public string InputAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the dry rod weight.
        /// </summary>
        public int? DryRodWeight { get; set; }

        /// <summary>
        /// Gets or sets the pump friction.
        /// </summary>
        public int? PumpFriction { get; set; }

        /// <summary>
        /// Gets or sets the pump off fluid load.
        /// </summary>
        public int? PofluidLoad { get; set; }

        /// <summary>
        /// Gets or sets the pump efficiency.
        /// </summary>
        public int? PumpEfficiency { get; set; }

        /// <summary>
        /// Gets or sets the downhole capacity.
        /// </summary>
        public float? DownholeCapacity24 { get; set; }

        /// <summary>
        /// Gets or sets the downhole capacity runtime.
        /// </summary>
        public float? DownholeCapacityRuntime { get; set; }

        /// <summary>
        /// Gets or sets the downhole capacity runtime fillage.
        /// </summary>
        public float? DownholeCapacityRuntimeFillage { get; set; }

        /// <summary>
        /// Gets or sets the maximum spm.
        /// </summary>
        public float? MaximumSpm { get; set; }

        /// <summary>
        /// Gets or sets the production at maximum spm.
        /// </summary>
        public float? ProductionAtMaximumSpm { get; set; }

        /// <summary>
        /// Gets or sets the oil production at maximum spm.
        /// </summary>
        public float? OilProductionAtMaximumSpm { get; set; }

        /// <summary>
        /// Gets or sets the message describing the results of calculating additional uplift opportunity.
        /// </summary>
        public string MaximumSpmoverloadMessage { get; set; }

        /// <summary>
        /// Gets or sets the current elec bo.
        /// </summary>
        public float? CurrentElecBO { get; set; }

        /// <summary>
        /// Gets or sets the additional oil production.
        /// </summary>
        public int? AddOilProduction { get; set; }

        /// <summary>
        /// Gets or sets the average dhds load.
        /// </summary>
        public int? AvgDHDSLoad { get; set; }

        /// <summary>
        /// Gets or sets the average dhdspo load.
        /// </summary>
        public int? AvgDHDSPOLoad { get; set; }

        /// <summary>
        /// gets or sets the average dhus load.
        /// </summary>
        public int? AvgDHUSLoad { get; set; }

        /// <summary>
        /// Gets or sets the average dhus po load.
        /// </summary>
        public int? AvgDHUSPOLoad { get; set; }

        /// <summary>
        /// Gets or sets the casing pressure.
        /// </summary>
        public int? CasingPressure { get; set; }

        /// <summary>
        /// Gets or sets the current cbe.
        /// </summary>
        public int? CurrentCBE { get; set; }

        /// <summary>
        /// Gets or sets the current clf.
        /// </summary>
        public float? CurrentCLF { get; set; }

        /// <summary>
        /// Gets or sets the current elec bg.
        /// </summary>
        public float? CurrentElecBG { get; set; }

        /// <summary>
        /// Gets or sets the current kwh.
        /// </summary>
        public int? CurrentKWH { get; set; }

        /// <summary>
        /// Gets or sets the current mcb.
        /// </summary>
        public short? CurrentMCB { get; set; }

        /// <summary>
        /// Gets or sets the current monthly electricity.
        /// </summary>
        public int? CurrentMonthlyElec { get; set; }

        /// <summary>
        /// Gets or sets the elec cost min torque per bo.
        /// </summary>
        public decimal? ElecCostMinTorquePerBO { get; set; }

        /// <summary>
        /// Gets or sets the elec cost monthly.
        /// </summary>
        public decimal? ElecCostMonthly { get; set; }

        /// <summary>
        /// Gets or sets the elec cost per bg.
        /// </summary>
        public decimal? ElecCostPerBG { get; set; }

        /// <summary>
        /// Gets or sets the elec cost per bo.
        /// </summary>
        public decimal? ElecCostPerBO { get; set; }

        /// <summary>
        /// Gets or sets the elect cost min energyperbo.
        /// </summary>
        public decimal? ElectCostMinEnergyPerBO { get; set; }

        /// <summary>
        /// Gets or sets the fillage %.
        /// </summary>
        public short? FillagePct { get; set; }

        /// <summary>
        /// Gets or sets the xdiag fluid level .
        /// </summary>
        public int? FluidLevelXDiag { get; set; }

        /// <summary>
        /// Gets or sets the friction.
        /// </summary>
        public double? Friction { get; set; }

        /// <summary>
        /// Gets or sets the gear box load %.
        /// </summary>
        public short? GearBoxLoadPct { get; set; }

        /// <summary>
        /// Gets or sets the gear box torque rating.
        /// </summary>
        public int? GearboxTorqueRating { get; set; }

        /// <summary>
        /// Gets or sets the gross prod.
        /// </summary>
        public int? GrossProd { get; set; }

        /// <summary>
        /// Gets or sets the load shift.
        /// </summary>
        public short? LoadShift { get; set; }

        /// <summary>
        /// Gets or sets the max rod load.
        /// </summary>
        public short? MaxRodLoad { get; set; }

        /// <summary>
        /// Gets or sets the min elec cost per bg.
        /// </summary>
        public decimal? MinElecCostPerBG { get; set; }

        /// <summary>
        /// Gets or sets the min ener gb torque.
        /// </summary>
        public int? MinEnerGBTorque { get; set; }

        /// <summary>
        /// Gets or sets the min energy cbe.
        /// </summary>
        public int? MinEnergyCBE { get; set; }

        /// <summary>
        /// Gets or sets the min energy clf.
        /// </summary>
        public float? MinEnergyCLF { get; set; }

        /// <summary>
        /// Gets or sets the MinEnergyElecBG.
        /// </summary>
        public float? MinEnergyElecBG { get; set; }

        /// <summary>
        /// Gets or sets the min energy gb load %.
        /// </summary>
        public short? MinEnergyGBLoadPct { get; set; }

        /// <summary>
        /// Gets or sets the MinEnergyMCB.
        /// </summary>
        public short? MinEnergyMCB { get; set; }

        /// <summary>
        /// Gets or sets the MinEnergyMonthlyElec.
        /// </summary>
        public int? MinEnergyMonthlyElec { get; set; }

        /// <summary>
        /// Gets or sets the MinGBTorque.
        /// </summary>
        public int? MinGBTorque { get; set; }

        /// <summary>
        /// Gets or sets the MinHP.
        /// </summary>
        public int? MinHP { get; set; }

        /// <summary>
        /// Gets or sets the MinMonthlyElecCost.
        /// </summary>
        public decimal? MinMonthlyElecCost { get; set; }

        /// <summary>
        /// Gets or sets the MinTorqueCBE.
        /// </summary>
        public int? MinTorqueCBE { get; set; }

        /// <summary>
        /// Gets or sets the MinTorqueCLF.
        /// </summary>
        public float? MinTorqueCLF { get; set; }

        /// <summary>
        /// Gets or sets the MinTorqueElecBG.
        /// </summary>
        public float? MinTorqueElecBG { get; set; }

        /// <summary>
        /// Gets or sets the MinTorqueElecBO.
        /// </summary>
        public float? MinTorqueElecBO { get; set; }

        /// <summary>
        /// Gets or sets the min torque gb load %.
        /// </summary>
        public short? MinTorqueGBLoadPct { get; set; }

        /// <summary>
        /// Gets or sets the min torque kwh.
        /// </summary>
        public int? MinTorqueKWH { get; set; }

        /// <summary>
        /// Gets or sets the min torque mcb.
        /// </summary>
        public short? MinTorqueMCB { get; set; }

        /// <summary>
        /// Gets or sets the min torque monthly elec.
        /// </summary>
        public int? MinTorqueMonthlyElec { get; set; }

        /// <summary>
        /// Gets or sets the motor load.
        /// </summary>
        public short? MotorLoad { get; set; }

        /// <summary>
        /// Gets or sets the MPRL.
        /// </summary>
        public int? MPRL { get; set; }

        /// <summary>
        /// Gets or sets the net prod.
        /// </summary>
        public short? NetProd { get; set; }

        /// <summary>
        /// Gets or sets the net stroke apparent.
        /// </summary>
        public float? NetStrokeApparent { get; set; }

        /// <summary>
        /// Gets or sets the oil api gravity.
        /// </summary>
        public float? OilAPIGravity { get; set; }

        /// <summary>
        /// Gets or sets the peak gb torque.
        /// </summary>
        public int? PeakGBTorque { get; set; }

        /// <summary>
        /// Gets or sets the po fluid load.
        /// </summary>
        public int? POFluidLoad { get; set; }

        /// <summary>
        /// Gets or sets the pol rod hp.
        /// </summary>
        public float? PolRodHP { get; set; }

        /// <summary>
        /// Gets or sets the pprl value.
        /// </summary>
        public int? PPRL { get; set; }

        /// <summary>
        /// Gets or sets the production atmaximum spm.
        /// </summary>
        public float? ProductionAtMaximumSPM { get; set; }

        /// <summary>
        /// Gets or sets the pump eff %.
        /// </summary>
        public short? PumpEffPct { get; set; }

        /// <summary>
        /// Gets or sets the system eff pct.
        /// </summary>
        public short? SystemEffPct { get; set; }

        /// <summary>
        /// Gets or sets the tubing movement.
        /// </summary>
        public int? TubingMovement { get; set; }

        /// <summary>
        /// Gets or sets the unit struct load.
        /// </summary>
        public short? UnitStructLoad { get; set; }

        /// <summary>
        /// Gets or sets the tubing pressure.
        /// </summary>
        public int? TubingPressure { get; set; }

        /// <summary>
        /// Gets or sets the water cut percent.
        /// </summary>
        public short? WaterCutPct { get; set; }

        /// <summary>
        /// Gets or sets the WaterSG.
        /// </summary>
        public float? WaterSG { get; set; }

    }
}
