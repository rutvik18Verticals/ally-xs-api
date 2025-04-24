using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Analysis
{
    /// <summary>
    /// A class that represents an XDiag Result.
    /// </summary>
    public class XDiagResults : AnalysisDocumentBase
    {
        /// <summary>
        /// Gets or sets the Car ID.
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string CardId { get; set; }

        /// <summary>
        /// Gets or sets the pump condition 1.
        /// </summary>
        public string PumpCond1 { get; set; }

        /// <summary>
        /// Gets or sets the pump condition 2.
        /// </summary>
        public string PumpCond2 { get; set; }

        /// <summary>
        /// Gets or sets the fillage percentage.
        /// </summary>
        public int? FillagePct { get; set; }

        /// <summary>
        /// Gets or sets the net production.
        /// </summary>
        public int? NetProd { get; set; }

        /// <summary>
        /// Gets or sets the fluid level XDiag.
        /// </summary>
        public int? FluidLevelXDiag { get; set; }

        /// <summary>
        /// Gets or sets the electric cost per BO.
        /// </summary>
        public double? ElecCostPerBO { get; set; }

        /// <summary>
        /// Gets or sets the electric cost minimum torque per BO.
        /// </summary>
        public double? ElecCostMinTorquePerBO { get; set; }

        /// <summary>
        /// Gets or sets the electric cost minimum energy per BO.
        /// </summary>
        public double? ElecCostMinEnergyPerBO { get; set; }

        /// <summary>
        /// Gets or sets the pump efficiency percentage.
        /// </summary>
        public short? PumpEffPct { get; set; }

        /// <summary>
        /// Gets or sets the system efficiency percentage.
        /// </summary>
        public short? SystemEffPct { get; set; }

        /// <summary>
        /// Gets or sets the pump stroke.
        /// </summary>
        public short? PumpStroke { get; set; }

        /// <summary>
        /// Gets or sets the gearbox load percentage.
        /// </summary>
        public short? GearBoxLoadPct { get; set; }

        /// <summary>
        /// Gets or sets the minimum torque gearbox load percentage.
        /// </summary>
        public short? MinTorqueGBLoadPct { get; set; }

        /// <summary>
        /// Gets or sets the minimum energy gearbox load percentage.
        /// </summary>
        public short? MinEnergyGBLoadPct { get; set; }

        /// <summary>
        /// Gets or sets the balance unit.
        /// </summary>
        public bool BalanceUnit { get; set; }

        /// <summary>
        /// Gets or sets the tubing pressure.
        /// </summary>
        public int? TubingPressure { get; set; }

        /// <summary>
        /// Gets or sets the casing pressure.
        /// </summary>
        public int? CasingPressure { get; set; }

        /// <summary>
        /// Gets or sets the gross production.
        /// </summary>
        public int? GrossProd { get; set; }

        /// <summary>
        /// Gets or sets the water cut percentage.
        /// </summary>
        public short? WaterCutPct { get; set; }

        /// <summary>
        /// Gets or sets the minimum horsepower.
        /// </summary>
        public int? MinHP { get; set; }

        /// <summary>
        /// Gets or sets the peak gearbox torque.
        /// </summary>
        public int? PeakGBTorque { get; set; }

        /// <summary>
        /// Gets or sets the minimum gearbox torque.
        /// </summary>
        public int? MinGBTorque { get; set; }

        /// <summary>
        /// Gets or sets the minimum energy gearbox torque.
        /// </summary>
        public double? MinEnerGBTorque { get; set; }

        /// <summary>
        /// Gets or sets the monthly electric cost.
        /// </summary>
        public double? MonthlyElecCost { get; set; }

        /// <summary>
        /// Gets or sets the minimum monthly electric cost.
        /// </summary>
        public double? MinMonthlyElecCost { get; set; }

        /// <summary>
        /// Gets or sets the electric cost per BG.
        /// </summary>
        public double? ElecCostPerBG { get; set; }

        /// <summary>
        /// Gets or sets the minimum electric cost per BG.
        /// </summary>
        public double? MinElecCostPerBG { get; set; }

        /// <summary>
        /// Gets or sets the pump size.
        /// </summary>
        public double? PumpSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is a tubing leak.
        /// </summary>
        public bool TubingLeak { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is bad SG.
        /// </summary>
        public bool BadSG { get; set; }

        /// <summary>
        /// Gets or sets the additional oil production.
        /// </summary>
        public int? AddOilProduction { get; set; }

        /// <summary>
        /// Gets or sets the analysis date.
        /// </summary>
        public DateTime? AnalysisDate { get; set; }

        /// <summary>
        /// Gets or sets the PPRL.
        /// </summary>
        public int? PPRL { get; set; }

        /// <summary>
        /// Gets or sets the MPRL.
        /// </summary>
        public int? MPRL { get; set; }

        /// <summary>
        /// Gets or sets the buoy rod weight.
        /// </summary>
        public int? BouyRodWeight { get; set; }

        /// <summary>
        /// Gets or sets the polish rod horsepower.
        /// </summary>
        public double? PolRodHP { get; set; }

        /// <summary>
        /// Gets or sets the unit structure load.
        /// </summary>
        public short? UnitStructLoad { get; set; }

        /// <summary>
        /// Gets or sets the maximum rod load.
        /// </summary>
        public int? MaxRodLoad { get; set; }

        /// <summary>
        /// Gets or sets the minimum energy CLF.
        /// </summary>
        public double? MinEnergyCLF { get; set; }

        /// <summary>
        /// Gets or sets the minimum torque CLF.
        /// </summary>
        public double? MinTorqueCLF { get; set; }

        /// <summary>
        /// Gets or sets the current CLF.
        /// </summary>
        public double? CurrentCLF { get; set; }

        /// <summary>
        /// Gets or sets the minimum energy MCB.
        /// </summary>
        public int? MinEnergyMCB { get; set; }

        /// <summary>
        /// Gets or sets the minimum torque MCB.
        /// </summary>
        public int? MinTorqueMCB { get; set; }

        /// <summary>
        /// Gets or sets the current MCB.
        /// </summary>
        public int? CurrentMCB { get; set; }

        /// <summary>
        /// Gets or sets the minimum energy CBE.
        /// </summary>
        public int? MinEnergyCBE { get; set; }

        /// <summary>
        /// Gets or sets the minimum torque CBE.
        /// </summary>
        public double? MinTorqueCBE { get; set; }

        /// <summary>
        /// Gets or sets the current CBE.
        /// </summary>
        public int? CurrentCBE { get; set; }

        /// <summary>
        /// Gets or sets the minimum energy KWH.
        /// </summary>
        public int? MinEnergyKWH { get; set; }

        /// <summary>
        /// Gets or sets the minimum torque KWH.
        /// </summary>
        public double? MinTorqueKWH { get; set; }

        /// <summary>
        /// Gets or sets the current KWH.
        /// </summary>
        public int? CurrentKWH { get; set; }

        /// <summary>
        /// Gets or sets the minimum energy monthly electric.
        /// </summary>
        public int? MinEnergyMonthlyElec { get; set; }

        /// <summary>
        /// Gets or sets the minimum torque monthly electric.
        /// </summary>
        public int? MinTorqueMonthlyElec { get; set; }

        /// <summary>
        /// Gets or sets the current monthly electric.
        /// </summary>
        public int? CurrentMonthlyElec { get; set; }

        /// <summary>
        /// Gets or sets the minimum energy electric BG.
        /// </summary>
        public double? MinEnergyElecBG { get; set; }

        /// <summary>
        /// Gets or sets the minimum torque electric BG.
        /// </summary>
        public double? MinTorqueElecBG { get; set; }

        /// <summary>
        /// Gets or sets the current electric BG.
        /// </summary>
        public double? CurrentElecBG { get; set; }

        /// <summary>
        /// Gets or sets the minimum energy electric BO.
        /// </summary>
        public double? MinEnergyElecBO { get; set; }

        /// <summary>
        /// Gets or sets the minimum torque electric BO.
        /// </summary>
        public double? MinTorqueElecBO { get; set; }

        /// <summary>
        /// Gets or sets the current electric BO.
        /// </summary>
        public double? CurrentElecBO { get; set; }

        /// <summary>
        /// Gets or sets the tubing movement.
        /// </summary>
        public int? TubingMovement { get; set; }

        /// <summary>
        /// Gets or sets the fluid load on pump.
        /// </summary>
        public int? FluidLoadonPump { get; set; }

        /// <summary>
        /// Gets or sets the pump intake pressure.
        /// </summary>
        public int? PumpIntakePressure { get; set; }

        /// <summary>
        /// Gets or sets the load shift.
        /// </summary>
        public short? LoadShift { get; set; }

        /// <summary>
        /// Gets or sets the water SG.
        /// </summary>
        public double? WaterSG { get; set; }

        /// <summary>
        /// Gets or sets the oil API gravity.
        /// </summary>
        public double? OilAPIGravity { get; set; }

        /// <summary>
        /// Gets or sets the fluid SG.
        /// </summary>
        public double? FluidSG { get; set; }

        /// <summary>
        /// Gets or sets the Gross Pump Stroke.
        /// </summary>
        public int? GrossPumpStroke { get; set; }

        /// <summary>
        /// Gets or sets the downhole analysis.
        /// </summary>
        public int? DownholeAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the Rod Analysis.
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
        /// Gets or sets the Gross Pump Displacement.
        /// </summary>
        public int? GrossPumpDisplacement { get; set; }

        /// <summary>
        /// Gets or sets the Net Stroke Apparent.
        /// </summary>
        public double? NetStrokeApparent { get; set; }

        /// <summary>
        /// Gets or sets the Avg DHDS Load.
        /// </summary>
        public int? AvgDHDSLoad { get; set; }

        /// <summary>
        /// Gets or sets the Avg DHUS Load.
        /// </summary>
        public int? AvgDHUSLoad { get; set; }

        /// <summary>
        /// Gets or sets the Avg DHDSPO Load.
        /// </summary>
        public int? AvgDHDSPOLoad { get; set; }

        /// <summary>
        /// Gets or sets the Avg DHUSPO Load.
        /// </summary>
        public int? AvgDHUSPOLoad { get; set; }

        /// <summary>
        /// Gets or sets the rod weight.
        /// </summary>
        public int? DryRodWeight { get; set; }

        /// <summary>
        /// Gets or sets the pump friction.
        /// </summary>
        public int? PumpFriction { get; set; }

        /// <summary>
        /// Gets or sets the PO fluid load.
        /// </summary>
        public int? POFluidLoad { get; set; }

        /// <summary>
        /// Gets or sets the pump efficiency.
        /// </summary>
        public int? PumpEfficiency { get; set; }

        /// <summary>
        /// Gets or sets the card type.
        /// </summary>
        public string CardType { get; set; }

        /// <summary>
        /// Gets or sets the downhole analysis locale.
        /// </summary>
        public string DownholeAnalysisLocale { get; set; }

        /// <summary>
        /// Gets or sets the rod analysis locale.
        /// </summary>
        public string RodAnalysisLocale { get; set; }

        /// <summary>
        /// Gets or sets the surface analysis locale.
        /// </summary>
        public string SurfaceAnalysisLocale { get; set; }

        /// <summary>
        /// Gets or sets the input analysis locale.
        /// </summary>
        public string InputAnalysisLocale { get; set; }

        /// <summary>
        /// Gets or sets the pump condition.
        /// </summary>
        public string PumpCondition { get; set; }

        /// <summary>
        /// Gets or sets the electric cost monthly.
        /// </summary>
        public double? ElecCostMonthly { get; set; }

        /// <summary>
        /// Gets or sets the gearbox torque rating.
        /// </summary>
        public int? GearboxTorqueRating { get; set; }

        /// <summary>
        /// Gets or sets the friction.
        /// </summary>
        public float? Friction { get; set; }

        /// <summary>
        /// Gets or sets the motor load.
        /// </summary>
        public short? MotorLoad { get; set; }

        /// <summary>
        /// Gets or sets the downhole capacity 24.
        /// </summary>
        public int? DownholeCapacity24 { get; set; }

        /// <summary>
        /// Gets or sets the downhole capacity runtime.
        /// </summary>
        public int? DownholeCapacityRuntime { get; set; }

        /// <summary>
        /// Gets or sets the downhole capacity runtime fillage.
        /// </summary>
        public int? DownholeCapacityRuntimeFillage { get; set; }

        /// <summary>
        /// Gets or sets the additional uplift.
        /// </summary>
        public double? AdditionalUplift { get; set; }

        /// <summary>
        /// Gets or sets the uplift calculation missing requirements.
        /// </summary>
        public double? UpliftCalculationMissingRequirements { get; set; }

        /// <summary>
        /// Gets or sets the additional uplift gross.
        /// </summary>
        public double? AdditionalUpliftGross { get; set; }

        /// <summary>
        /// Gets or sets the maximum SPM.
        /// </summary>
        public double? MaximumSPM { get; set; }

        /// <summary>
        /// Gets or sets the production at maximum SPM.
        /// </summary>
        public double? ProductionAtMaximumSPM { get; set; }

        /// <summary>
        /// Gets or sets the oil production at maximum SPM.
        /// </summary>
        public double? OilProductionAtMaximumSPM { get; set; }

        /// <summary>
        /// Gets or sets the maximum SPM overload message.
        /// </summary>
        public string MaximumSPMOverloadMessage { get; set; }

        /// <summary>
        /// Gets or sets the XDiag rod results.
        /// </summary>
        public IList<XDiagRodResult> XDiagRodResults { get; set; }

        /// <summary>
        /// Gets or sets the XDiag score.
        /// </summary>
        public XDiagScores XDiagScore { get; set; }

        /// <summary>
        /// Gets or sets the XDiag flag.
        /// </summary>
        public XDiagFlag XDiagFlag { get; set; }
    }

}
