using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Theta.XSPOC.Apex.Api.Data.Entity.XDIAG
{
    /// <summary>
    /// A base class for entities containing xdiag result data.
    /// </summary>
    public abstract class XDIAGResultBase
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        [Column("NodeID")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        [Column("Date")]
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
        /// Gets or sets the pump efficiency percentage.
        /// </summary>
        [Column("PumpEffPct")]
        public short? PumpEfficiencyPercentage { get; set; }

        /// <summary>
        /// Gets or sets the gear box load percentage.
        /// </summary>
        [Column("GearBoxLoadPct")]
        public short? GearBoxLoadPercentage { get; set; }

        /// <summary>
        /// Gets or sets unit structural load.
        /// </summary>
        [Column("UnitStructLoad")]
        public short? UnitStructuralLoad { get; set; }

        /// <summary>
        /// Gets or sets the max rod load.
        /// </summary>
        [Column("MaxRodLoad")]
        public short? MaxRodLoad { get; set; }

        /// <summary>
        /// Gets or sets the pump efficiency.
        /// </summary>
        [Column("PumpEfficiency")]
        public int? PumpEfficiency { get; set; }

        /// <summary>
        /// Gets or sets the fillage percentage.
        /// </summary>
        [Column("FillagePct")]
        public short? FillagePercentage { get; set; }

        /// <summary>
        /// Gets or sets the motor load.
        /// </summary>
        [Column("MotorLoad")]
        public short? MotorLoad { get; set; }

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

    }
}