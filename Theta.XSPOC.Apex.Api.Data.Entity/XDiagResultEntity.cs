using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity
{
    /// <summary>
    /// The xdiag result table.
    /// </summary>
    [Table("tblXDiagResults")]
    public class XDiagResultEntity
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        [Column("NodeID")]
        [MaxLength(50)]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the additional uplift.
        /// </summary>
        [Column("AdditionalUplift")]
        public float? AdditionalUplift { get; set; }

        /// <summary>
        /// Gets or sets the additional uplift gross.
        /// </summary>
        [Column("AdditionalUpliftGross")]
        public float? AdditionalUpliftGross { get; set; }

        /// <summary>
        /// Gets or sets the uplift calculation missing requirements.
        /// </summary>
        [Column("UpliftCalculationMissingRequirements")]
        public string UpliftCalculationMissingRequirements { get; set; }

        /// <summary>
        /// Gets or sets the pump intake pressure from XDiag result.
        /// </summary>
        [Column("PumpIntakePressure")]
        public int? PumpIntakePressure { get; set; }

        /// <summary>
        /// Gets or sets the pump size.
        /// </summary>
        [Column("PumpSize")]
        public float? PumpSize { get; set; }

        /// <summary>
        /// Gets or sets the buoyant rod weight.
        /// </summary>
        [Column("BouyRodWeight")]
        public int? BouyRodWeight { get; set; }

        /// <summary>
        /// Gets or sets the fluid load on pump.
        /// </summary>
        [Column("FluidLoadonPump")]
        public int? FluidLoadonPump { get; set; }

        /// <summary>
        /// Gets or sets the gross pump stroke.
        /// </summary>
        [Column("GrossPumpStroke")]
        public short? GrossPumpStroke { get; set; }

        /// <summary>
        /// Gets or sets the downhole analysis.
        /// </summary>
        [Column("DownholeAnalysis")]
        public string DownholeAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the rod analysis.
        /// </summary>
        [Column("RodAnalysis")]
        public string RodAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the surface analysis.
        /// </summary>
        [Column("SurfaceAnalysis")]
        public string SurfaceAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the input analysis.
        /// </summary>
        [Column("InputAnalysis")]
        public string InputAnalysis { get; set; }

        /// <summary>
        ///  Gets or sets the gross pump displacement.
        /// </summary>
        [Column("GrossPumpDisplacement")]
        public int? GrossPumpDisplacement { get; set; }

        /// <summary>
        /// Gets or sets the dry rod weight.
        /// </summary>
        [Column("DryRodWeight")]
        public int? DryRodWeight { get; set; }

        /// <summary>
        /// Gets or sets the pump friction.
        /// </summary>
        [Column("PumpFriction")]
        public int? PumpFriction { get; set; }

        /// <summary>
        /// Gets or sets the pump off fluid load.
        /// </summary>
        [Column("PofluidLoad")]
        public int? PofluidLoad { get; set; }

        /// <summary>
        /// Gets or sets the pump efficiency.
        /// </summary>
        [Column("PumpEfficiency")]
        public int? PumpEfficiency { get; set; }

        /// <summary>
        /// Gets or sets the downhole capacity.
        /// </summary>
        [Column("DownholeCapacity24")]
        public float? DownholeCapacity24 { get; set; }

        /// <summary>
        /// Gets or sets the downhole capacity runtime.
        /// </summary>
        [Column("DownholeCapacityRuntime")]
        public float? DownholeCapacityRuntime { get; set; }

        /// <summary>
        /// Gets or sets the downhole capacity runtime fillage.
        /// </summary>
        [Column("DownholeCapacityRuntimeFillage")]
        public float? DownholeCapacityRuntimeFillage { get; set; }

        /// <summary>
        /// Gets or sets the maximum spm.
        /// </summary>
        [Column("MaximumSpm")]
        public float? MaximumSpm { get; set; }

        /// <summary>
        /// Gets or sets the production at maximum spm.
        /// </summary>
        [Column("ProductionAtMaximumSpm")]
        public float? ProductionAtMaximumSpm { get; set; }

        /// <summary>
        /// Gets or sets the oil production at maximum spm.
        /// </summary>
        [Column("OilProductionAtMaximumSpm")]
        public float? OilProductionAtMaximumSpm { get; set; }

        /// <summary>
        /// Gets or sets the message describing the results of calculating additional uplift opportunity.
        /// </summary>
        [Column("MaximumSpmoverloadMessage")]
        public string MaximumSpmoverloadMessage { get; set; }

        /// <summary>
        /// Gets or sets the current elec bo.
        /// </summary>
        [Column("CurrentElecBO")]
        public float? CurrentElecBO { get; set; }

        /// <summary>
        /// Gets or sets the AddOilProduction.
        /// </summary>
        [Column("AddOilProduction")]
        public int? AddOilProduction { get; set; }

        /// <summary>
        /// Gets or sets the AvgDHDSLoad.
        /// </summary>
        [Column("AvgDHDSLoad")]
        public int? AvgDHDSLoad { get; set; }

        /// <summary>
        /// Gets or sets the AvgDHDSPOLoad.
        /// </summary>
        [Column("AvgDHDSPOLoad")]
        public int? AvgDHDSPOLoad { get; set; }

        /// <summary>
        /// Gets or sets the AvgDHUSLoad.
        /// </summary>
        [Column("AvgDHUSLoad")]
        public int? AvgDHUSLoad { get; set; }

        /// <summary>
        /// Gets or sets the AvgDHUSPOLoad.
        /// </summary>
        [Column("AvgDHUSPOLoad")]
        public int? AvgDHUSPOLoad { get; set; }

        /// <summary>
        /// Gets or sets the CasingPressure.
        /// </summary>
        [Column("CasingPressure")]
        public int? CasingPressure { get; set; }

        /// <summary>
        /// Gets or sets the CurrentCBE.
        /// </summary>
        [Column("CurrentCBE")]
        public int? CurrentCBE { get; set; }

        /// <summary>
        /// Gets or sets the CurrentCLF.
        /// </summary>
        [Column("CurrentCLF")]
        public float? CurrentCLF { get; set; }

        /// <summary>
        /// Gets or sets the CurrentElecBG.
        /// </summary>
        [Column("CurrentElecBG")]
        public float? CurrentElecBG { get; set; }

        /// <summary>
        /// Gets or sets the CurrentKWH.
        /// </summary>
        [Column("CurrentKWH")]
        public int? CurrentKWH { get; set; }

        /// <summary>
        /// Gets or sets the CurrentMCB.
        /// </summary>
        [Column("CurrentMCB")]
        public short? CurrentMCB { get; set; }

        /// <summary>
        /// Gets or sets the CurrentMonthlyElec.
        /// </summary>
        [Column("CurrentMonthlyElec")]
        public int? CurrentMonthlyElec { get; set; }

        /// <summary>
        /// Gets or sets the ElecCostMinTorquePerBO.
        /// </summary>
        [Column("ElecCostMinTorquePerBO", TypeName = "money")]
        public decimal? ElecCostMinTorquePerBO { get; set; }

        /// <summary>
        /// Gets or sets the ElecCostMonthly.
        /// </summary>
        [Column("ElecCostMonthly", TypeName = "money")]
        public decimal? ElecCostMonthly { get; set; }

        /// <summary>
        /// Gets or sets the ElecCostPerBG.
        /// </summary>
        [Column("ElecCostPerBG", TypeName = "money")]
        public decimal? ElecCostPerBG { get; set; }

        /// <summary>
        /// Gets or sets the ElecCostPerBO.
        /// </summary>
        [Column("ElecCostPerBO", TypeName = "money")]
        public decimal? ElecCostPerBO { get; set; }

        /// <summary>
        /// Gets or sets the ElectCostMinEnergyPerBO.
        /// </summary>
        [Column("ElectCostMinEnergyPerBO", TypeName = "money")]
        public decimal? ElectCostMinEnergyPerBO { get; set; }

        /// <summary>
        /// Gets or sets the FillagePct.
        /// </summary>
        [Column("FillagePct")]
        public short? FillagePct { get; set; }

        /// <summary>
        /// Gets or sets the FluidLevelXDiag.
        /// </summary>
        [Column("FluidLevelXDiag")]
        public int? FluidLevelXDiag { get; set; }

        /// <summary>
        /// Gets or sets the Friction.
        /// </summary>
        [Column("Friction")]
        public double? Friction { get; set; }

        /// <summary>
        /// Gets or sets the GearBoxLoadPct.
        /// </summary>
        [Column("GearBoxLoadPct")]
        public short? GearBoxLoadPct { get; set; }

        /// <summary>
        /// Gets or sets the GearboxTorqueRating.
        /// </summary>
        [Column("GearboxTorqueRating")]
        public int? GearboxTorqueRating { get; set; }

        /// <summary>
        /// Gets or sets the GrossProd.
        /// </summary>
        [Column("GrossProd")]
        public int? GrossProd { get; set; }

        /// <summary>
        /// Gets or sets the LoadShift.
        /// </summary>
        [Column("LoadShift")]
        public short? LoadShift { get; set; }

        /// <summary>
        /// Gets or sets the MaxRodLoad.
        /// </summary>
        [Column("MaxRodLoad")]
        public short? MaxRodLoad { get; set; }

        /// <summary>
        /// Gets or sets the MinElecCostPerBG.
        /// </summary>
        [Column("MinElecCostPerBG", TypeName = "money")]
        public decimal? MinElecCostPerBG { get; set; }

        /// <summary>
        /// Gets or sets the MinEnerGBTorque.
        /// </summary>
        [Column("MinEnerGBTorque")]
        public int? MinEnerGBTorque { get; set; }

        /// <summary>
        /// Gets or sets the MinEnergyCBE.
        /// </summary>
        [Column("MinEnergyCBE")]
        public int? MinEnergyCBE { get; set; }

        /// <summary>
        /// Gets or sets the MinEnergyCLF.
        /// </summary>
        [Column("MinEnergyCLF")]
        public float? MinEnergyCLF { get; set; }

        /// <summary>
        /// Gets or sets the MinEnergyElecBG.
        /// </summary>
        [Column("MinEnergyElecBG")]
        public float? MinEnergyElecBG { get; set; }

        /// <summary>
        /// Gets or sets the MinEnergyGBLoadPct.
        /// </summary>
        [Column("MinEnergyGBLoadPct")]
        public short? MinEnergyGBLoadPct { get; set; }

        /// <summary>
        /// Gets or sets the MinEnergyMCB.
        /// </summary>
        [Column("MinEnergyMCB")]
        public short? MinEnergyMCB { get; set; }

        /// <summary>
        /// Gets or sets the MinEnergyMonthlyElec.
        /// </summary>
        [Column("MinEnergyMonthlyElec")]
        public int? MinEnergyMonthlyElec { get; set; }

        /// <summary>
        /// Gets or sets the MinGBTorque.
        /// </summary>
        [Column("MinGBTorque")]
        public int? MinGBTorque { get; set; }

        /// <summary>
        /// Gets or sets the MinHP.
        /// </summary>
        [Column("MinHP")]
        public int? MinHP { get; set; }

        /// <summary>
        /// Gets or sets the MinMonthlyElecCost.
        /// </summary>
        [Column("MinMonthlyElecCost", TypeName = "money")]
        public decimal? MinMonthlyElecCost { get; set; }

        /// <summary>
        /// Gets or sets the MinTorqueCBE.
        /// </summary>
        [Column("MinTorqueCBE")]
        public int? MinTorqueCBE { get; set; }

        /// <summary>
        /// Gets or sets the MinTorqueCLF.
        /// </summary>
        [Column("MinTorqueCLF")]
        public float? MinTorqueCLF { get; set; }

        /// <summary>
        /// Gets or sets the MinTorqueElecBG.
        /// </summary>
        [Column("MinTorqueElecBG")]
        public float? MinTorqueElecBG { get; set; }

        /// <summary>
        /// Gets or sets the MinTorqueElecBO.
        /// </summary>
        [Column("MinTorqueElecBO")]
        public float? MinTorqueElecBO { get; set; }

        /// <summary>
        /// Gets or sets the MinTorqueGBLoadPct.
        /// </summary>
        [Column("MinTorqueGBLoadPct")]
        public short? MinTorqueGBLoadPct { get; set; }

        /// <summary>
        /// Gets or sets the MinTorqueKWH.
        /// </summary>
        [Column("MinTorqueKWH")]
        public int? MinTorqueKWH { get; set; }

        /// <summary>
        /// Gets or sets the MinTorqueMCB.
        /// </summary>
        [Column("MinTorqueMCB")]
        public short? MinTorqueMCB { get; set; }

        /// <summary>
        /// Gets or sets the MinTorqueMonthlyElec.
        /// </summary>
        [Column("MinTorqueMonthlyElec")]
        public int? MinTorqueMonthlyElec { get; set; }

        /// <summary>
        /// Gets or sets the MotorLoad.
        /// </summary>
        [Column("MotorLoad")]
        public short? MotorLoad { get; set; }

        /// <summary>
        /// Gets or sets the MPRL.
        /// </summary>
        [Column("MPRL")]
        public int? MPRL { get; set; }

        /// <summary>
        /// Gets or sets the NetProd.
        /// </summary>
        [Column("NetProd")]
        public short? NetProd { get; set; }

        /// <summary>
        /// Gets or sets the NetStrokeApparent.
        /// </summary>
        [Column("NetStrokeApparent")]
        public float? NetStrokeApparent { get; set; }

        /// <summary>
        /// Gets or sets the OilAPIGravity.
        /// </summary>
        [Column("OilAPIGravity")]
        public float? OilAPIGravity { get; set; }

        /// <summary>
        /// Gets or sets the PeakGBTorque.
        /// </summary>
        [Column("PeakGBTorque")]
        public int? PeakGBTorque { get; set; }

        /// <summary>
        /// Gets or sets the POFluidLoad.
        /// </summary>
        [Column("POFluidLoad")]
        public int? POFluidLoad { get; set; }

        /// <summary>
        /// Gets or sets the PolRodHP.
        /// </summary>
        [Column("PolRodHP")]
        public float? PolRodHP { get; set; }

        /// <summary>
        /// Gets or sets the PPRL value.
        /// </summary>
        [Column("PPRL")]
        public int? PPRL { get; set; }

        /// <summary>
        /// Gets or sets the ProductionAtMaximumSPM.
        /// </summary>
        [Column("ProductionAtMaximumSPM")]
        public float? ProductionAtMaximumSPM { get; set; }

        /// <summary>
        /// Gets or sets the PumpEffPct.
        /// </summary>
        [Column("PumpEffPct")]
        public short? PumpEffPct { get; set; }

        /// <summary>
        /// Gets or sets the SystemEffPct.
        /// </summary>
        [Column("SystemEffPct")]
        public short? SystemEffPct { get; set; }

        /// <summary>
        /// Gets or sets the TubingMovement.
        /// </summary>
        [Column("TubingMovement")]
        public int? TubingMovement { get; set; }

        /// <summary>
        /// Gets or sets the UnitStructLoad.
        /// </summary>
        [Column("UnitStructLoad")]
        public short? UnitStructLoad { get; set; }

        /// <summary>
        /// Gets or sets the Tubing Pressure.
        /// </summary>
        [Column("TubingPressure")]
        public int? TubingPressure { get; set; }

        /// <summary>
        /// Gets or sets the WaterCutPct.
        /// </summary>
        [Column("WaterCutPct")]
        public short? WaterCutPct { get; set; }

        /// <summary>
        /// Gets or sets the WaterSG.
        /// </summary>
        [Column("WaterSG")]
        public float? WaterSG { get; set; }

    }
}
