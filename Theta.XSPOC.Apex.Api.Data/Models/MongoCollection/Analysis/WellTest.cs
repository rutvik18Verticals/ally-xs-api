using System;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Analysis
{
    /// <summary>
    /// A class that represents a well test.
    /// </summary>
    public class WellTest : AssetDocumentBase
    {

        /// <summary>
        /// Gets or sets tubing pressure.
        /// </summary>
        public double? TubingPressure { get; set; }

        /// <summary>
        /// Gets or sets casing pressure.
        /// </summary>
        public double? CasingPressure { get; set; }

        /// <summary>
        /// Gets or sets the runtime.
        /// </summary>
        public double? Runtime { get; set; }

        /// <summary>
        /// Gets or sets the stroke length.
        /// </summary>
        public double? StrokeLength { get; set; }

        /// <summary>
        /// Gets or sets the fluid above pump.
        /// </summary>
        public double? FluidAbovePump { get; set; }

        /// <summary>
        /// Gets or sets the pump size.
        /// </summary>
        public double? Pumpsize { get; set; }

        /// <summary>
        /// Gets or sets the strokes per minute.
        /// </summary>
        public double? StrokesPerMinute { get; set; }

        /// <summary>
        /// Gets or sets the flowline temperature.
        /// </summary>
        public double? FlowlineTemperature { get; set; }

        /// <summary>
        /// Gets or sets the casing temperature.
        /// </summary>
        public double? CasingTemperature { get; set; }

        /// <summary>
        /// Gets or sets the water specific gravity.
        /// </summary>
        public double? WaterSpecificGravity { get; set; }

        /// <summary>
        /// Gets or sets the oil API gravity.
        /// </summary>
        public double? OilAPIGravity { get; set; }

        /// <summary>
        /// Gets or sets the co2.
        /// </summary>
        public double? CO2 { get; set; }

        /// <summary>
        /// Gets or sets the high pressure gas rate.
        /// </summary>
        public double? HighPressureGasRate { get; set; }

        /// <summary>
        /// Gets or sets teh casing gas rate.
        /// </summary>
        public double? CasingGasRate { get; set; }

        /// <summary>
        /// Gets or sets the gas lift gas rate.
        /// </summary>
        public double? GasLiftGasRate { get; set; }

        /// <summary>
        /// Gets or sets the flowline pressure.
        /// </summary>
        public double? FlowlinePressure { get; set; }

        /// <summary>
        /// Gets or sets the tubing temperature.
        /// </summary>
        public double? TubingTemperature { get; set; }

        /// <summary>
        /// Gets or sets the pump intake pressure.
        /// </summary>
        public double? PumpIntakePressure { get; set; }

        /// <summary>
        /// Gets or sets the pump discharge pressure.
        /// </summary>
        public double? PumpDischargePressure { get; set; }

        /// <summary>
        /// Gets or sets the pump intake temperature
        /// </summary>
        public double? PumpIntakeTemperature { get; set; }

        /// <summary>
        /// Gets or sets the motor temperature.
        /// </summary>
        public double? MotorTemperature { get; set; }

        /// <summary>
        /// Gets or sets the drive current.
        /// </summary>
        public double? DriveCurrent { get; set; }

        /// <summary>
        /// Gets or sets the drive speed.
        /// </summary>
        public double? DriveSpeed { get; set; }

        /// <summary>
        /// Gets or sets the rod RPM.
        /// </summary>
        public double? RodRPM { get; set; }

        /// <summary>
        /// Gets or sets the rod torque.
        /// </summary>
        public double? RodTorque { get; set; }

        /// <summary>
        /// Gets or sets the triplex discharge pressure.
        /// </summary>
        public double? TriplexDischargePressure { get; set; }

        /// <summary>
        /// Gets or sets the suction pressure.
        /// </summary>
        public double? SuctionPressure { get; set; }

        /// <summary>
        /// Gets or sets the line pressure to setting.
        /// </summary>
        public double? LinePressureToSetting { get; set; }
        /// <summary>
        /// Gets or sets whether the well test is approved.
        /// </summary>
        public bool? Approved { get; set; }

        /// <summary>
        /// Gets or sets the gas rate.
        /// </summary>
        public double? GasRate { get; set; }

        /// <summary>
        /// Gets or sets the well test date.
        /// </summary>
        public DateTime TestDate { get; set; }

        /// <summary>
        /// Gets or sets the oil rate.
        /// </summary>
        public double? OilRate { get; set; }

        /// <summary>
        /// Gets or sets the water rate.
        /// </summary>
        public double? WaterRate { get; set; }

        /// <summary>
        /// Gets or sets the water rate.
        /// </summary>
        public double? Duration { get; set; }

    }
}
