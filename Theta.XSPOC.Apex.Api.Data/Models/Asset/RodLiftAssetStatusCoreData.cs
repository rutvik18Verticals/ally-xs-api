using System;
using Theta.XSPOC.Apex.Kernel.Quantity;

namespace Theta.XSPOC.Apex.Api.Data.Models.Asset
{
    /// <summary>
    /// This is the rod lift asset status core data that is pulled from the data store.
    /// </summary>
    public record RodLiftAssetStatusCoreData
    {

        /// <summary>
        /// Gets or sets the api designation.
        /// </summary>
        public string APIDesignation { get; set; }

        /// <summary>
        /// Gets or sets the api port.
        /// </summary>
        public int? ApiPort { get; set; }

        /// <summary>
        /// Gets or sets the port id.
        /// </summary>
        public short? PortId { get; set; } 

        /// <summary>
        /// Gets or sets the asset GUID.
        /// </summary>
        public Guid AssetGUID { get; set; }

        /// <summary>
        /// Gets or sets the calculated fluid level above the pump from esp result.
        /// </summary>
        public float? CalculatedFluidLevelAbovePump { get; set; }

        /// <summary>
        /// Gets or sets the casing pressure.
        /// </summary>
        public IValue CasingPressure { get; set; }

        /// <summary>
        /// Gets or sets the casing pressure unit string.
        /// </summary>
        public string CasingPressureUnitString { get; set; }

        /// <summary>
        /// Gets or sets the communication percentage for yesterday.
        /// </summary>
        public int? CommunicationPercentageYesterday { get; set; }

        /// <summary>
        /// Gets or sets the communication status.
        /// </summary>
        public string CommunicationStatus { get; set; }

        /// <summary>
        /// Gets or sets the ESP result test date.
        /// </summary>
        public DateTime? ESPResultTestDate { get; set; }

        /// <summary>
        /// Gets or sets the firmware version.
        /// </summary>
        public float? FirmwareVersion { get; set; }

        /// <summary>
        /// Gets or sets the fluid level.
        /// </summary>
        public IValue FluidLevel { get; set; }

        /// <summary>
        /// Gets or sets the fluid level unit string.
        /// </summary>
        public string FluidLevelUnitString { get; set; }

        /// <summary>
        /// Gets or sets the gas rate.
        /// </summary>
        public IValue GasRate { get; set; }

        /// <summary>
        /// Gets or sets the gear box load percentage.
        /// </summary>
        public short? GearBoxLoadPercentage { get; set; }

        /// <summary>
        /// Gets or sets the gross rate.
        /// </summary>
        public IValue GrossRate { get; set; }

        /// <summary>
        /// Gets or sets if the node is enabled.
        /// </summary>
        public bool IsNodeEnabled { get; set; }

        /// <summary>
        /// Gets or sets the last good scan.
        /// </summary>
        public DateTime? LastGoodScan { get; set; }

        /// <summary>
        /// The timezone offset of the node.
        /// </summary>
        public float TzOffset { get; set; }

        /// <summary>
        /// The honour daylight saving.
        /// </summary>
        public bool TzDaylight { get; set; }

        /// <summary>
        /// Gets or sets the last well test date.
        /// </summary>
        public DateTime? LastWellTestDate { get; set; }

        /// <summary>
        /// Gets or sets the max rod loading.
        /// </summary>
        public short? MaxRodLoading { get; set; }

        /// <summary>
        /// Gets or sets the motor kind name.
        /// </summary>
        public string MotorKindName { get; set; }

        /// <summary>
        /// Gets or sets the motor load.
        /// </summary>
        public short? MotorLoad { get; set; }

        /// <summary>
        /// Gets or sets the motor type id.
        /// </summary>
        public int? MotorTypeId { get; set; }

        /// <summary>
        /// Gets or sets the plunger diameter.
        /// </summary>
        public IValue PlungerDiameter { get; set; }

        /// <summary>
        /// Gets or sets the plunger diameter unit string.
        /// </summary>
        public string PlungerDiameterUnitString { get; set; }

        /// <summary>
        /// Gets or sets the poc type description.
        /// </summary>
        public string PocTypeDescription { get; set; }

        /// <summary>
        /// Gets or sets the prime motor type.
        /// </summary>
        public string PrimeMoverType { get; set; }

        /// <summary>
        /// Gets or sets the pump depth.
        /// </summary>
        public IValue PumpDepth { get; set; }

        /// <summary>
        /// Gets or sets the pump depth unit string.
        /// </summary>
        public string PumpDepthUnitString { get; set; }

        /// <summary>
        /// Gets or sets the pump efficiency.
        /// </summary>
        /// 
        public int? PumpEfficiency { get; set; }

        /// <summary>
        /// Gets or sets the pump efficiency percentage.
        /// </summary>
        public short? PumpEfficiencyPercentage { get; set; }

        /// <summary>
        /// Gets or sets the pump fillage.
        /// </summary>
        public int? PumpFillage { get; set; }

        /// <summary>
        /// Gets or sets pumping unit manufacturer.
        /// </summary>
        public string PumpingUnitManufacturer { get; set; }

        /// <summary>
        /// Gets or sets the pumping unit name.
        /// </summary>
        public string PumpingUnitName { get; set; }

        /// <summary>
        /// Gets or sets the node address that represents how to communicate to a device.
        /// </summary>
        public string NodeAddress { get; set; }

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the rated horse power.
        /// </summary>
        public float? RatedHorsePower { get; set; }

        /// <summary>
        /// Gets or sets the run status.
        /// </summary>
        public string RunStatus { get; set; }

        /// <summary>
        /// Gets or sets the stroke length.
        /// </summary>
        public IValue StrokeLength { get; set; }

        /// <summary>
        /// Get or sets the stroke length unit string.
        /// </summary>
        public string StrokeLengthUnitString { get; set; }

        /// <summary>
        /// Gets or sets the strokes per minute.
        /// </summary>
        public float? StrokesPerMinute { get; set; }

        /// <summary>
        /// Gets or sets the structural loading.
        /// </summary>
        public short? StructuralLoading { get; set; }

        /// <summary>
        /// Gets or sets the time in state.
        /// </summary>
        public int? TimeInState { get; set; }

        /// <summary>
        /// Gets or sets the today runtime percentage.
        /// </summary>
        public float? TodayRuntimePercentage { get; set; }

        /// <summary>
        /// Gets or sets the tubing pressure.
        /// </summary>
        public IValue TubingPressure { get; set; }

        /// <summary>
        /// Gets or sets the tubing pressure unit string.
        /// </summary>
        public string TubingPressureUnitString { get; set; }

        /// <summary>
        /// Gets or sets the Rate At Test.
        /// </summary>
        public IValue RateAtTest { get; set; }

        /// <summary>
        /// Gets or sets the Rate At Test string.
        /// </summary>
        public string RateAtTestString { get; set; }

        /// <summary>
        /// Gets or sets the water cut.
        /// </summary>
        public IValue WaterCut { get; set; }

        /// <summary>
        /// Gets or sets the yesterday runtime percentage.
        /// </summary>
        public float? YesterdayRuntimePercentage { get; set; }

        /// <summary>
        /// Gets or sets the company guid.
        /// </summary>
        public Guid? CustomerGUID { get; set; }

        /// <summary>
        /// Gets or sets the application id.
        /// </summary>
        public int? ApplicationId { get; set; }

        /// <summary>
        /// Gets or sets the pumping unit type id.
        /// </summary>
        public string PumpingUnitTypeId { get; set; }

        /// <summary>
        /// Gets or sets the poc type.
        /// </summary>
        public int PocType { get; set; }

        /// <summary>
        /// Gets or sets the pump type.
        /// </summary>
        public string PumpType { get; set; }

        /// <summary>
        /// Gets or sets the casing pressure.
        /// </summary>
        public short? LinePressure { get; set; }

    }
}
