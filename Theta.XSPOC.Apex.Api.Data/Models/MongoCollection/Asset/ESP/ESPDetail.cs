using System;
using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models.MongoCollection.Asset.ESP
{
    /// <summary>
    /// A class representing the details of an ESP asset.
    /// </summary>
    public class ESPDetail : WellDetailsBase
    {

        /// <summary>
        /// Gets or sets the derating factor.
        /// </summary>
        public int? DeratingFactor { get; set; }

        /// <summary>
        /// Gets or sets the number of stages.
        /// </summary>
        public int? NumberOfStages { get; set; }

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        public double? Frequency { get; set; }

        /// <summary>
        /// Gets or sets if the variable frequency is used.
        /// </summary>
        public bool UseVariableFrequency { get; set; }

        /// <summary>
        /// Gets or sets the date installed.
        /// </summary>
        public DateTime? DateInstalled { get; set; }

        /// <summary>
        /// Gets or sets if gas handling is enabled.
        /// </summary>
        public bool EnableGasHandling { get; set; }

        /// <summary>
        /// Gets or sets the gas separator efficiency.
        /// </summary>
        public double? GasSeparatorEfficiency { get; set; }

        /// <summary>
        /// Gets or sets if the switchboard frequency is used.
        /// </summary>
        public bool UseSwitchboardFrequency { get; set; }

        /// <summary>
        /// Gets or sets the discharge gage depth.
        /// </summary>
        public double? DischargeGageDepth { get; set; }

        /// <summary>
        /// Gets or sets if the discharge gage is used in analysis.
        /// </summary>
        public bool UseDischargeGageInAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the pump intake pressure sensor vertical depth.
        /// </summary>
        public double? PumpIntakePressureSensorVerticalDepth { get; set; }

        /// <summary>
        /// Gets or sets if the intake pressure sensor is used in analysis.
        /// </summary>
        public bool UseIntakePressureSensorInAnalysis { get; set; }

        /// <summary>
        /// Gets or sets the ESP pumps.
        /// </summary>
        public IList<ESPWellPumps> ESPPumps { get; set; }

        /// <summary>
        /// Gets or sets the ESP seals.
        /// </summary>
        public IList<ESPWellSeal> ESPSeals { get; set; }

        /// <summary>
        /// Gets or sets the ESP cables.
        /// </summary>
        public IList<ESPWellCable> ESPCables { get; set; }

        /// <summary>
        /// Gets or sets the ESP motors.
        /// </summary>
        public IList<ESPWellMotor> ESPMotors { get; set; }

        /// <summary>
        /// Gets or sets the ESP motor leads.
        /// </summary>
        public IList<ESPWellMotorLead> ESPMotorLeads { get; set; }

    }
}
