using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Defines the PID Data Model.
    /// </summary>
    public class PIDDataModel
    {

        /// <summary>
        /// Gets or sets the Primary Process Variable.
        /// </summary>
        public string PrimaryProcessVariable { get; set; }

        /// <summary>
        /// Gets or sets the Primary Setpoint.
        /// </summary>
        public string PrimarySetpoint { get; set; }

        /// <summary>
        /// Gets or sets the Primary Deadband.
        /// </summary>
        public string PrimaryDeadband { get; set; }

        /// <summary>
        /// Gets or sets the Primary Proportional Gain.
        /// </summary>
        public string PrimaryProportionalGain { get; set; }

        /// <summary>
        /// Gets or sets the Primary Integral.
        /// </summary>
        public string PrimaryIntegral { get; set; }

        /// <summary>
        /// Gets or sets the PrimaryDerivative.
        /// </summary>
        public string PrimaryDerivative { get; set; }

        /// <summary>
        /// Gets or sets the Primary Scale Factor.
        /// </summary>
        public string PrimaryScaleFactor { get; set; }

        /// <summary>
        /// Gets or sets the Primary Output.
        /// </summary>
        public string PrimaryOutput { get; set; }

        /// <summary>
        /// Gets or sets the Override Process Variable.
        /// </summary>
        public string OverrideProcessVariable { get; set; }

        /// <summary>
        /// Gets or sets the Override Setpoint.
        /// </summary>
        public string OverrideSetpoint { get; set; }

        /// <summary>
        /// Gets or sets the Override Deadband.
        /// </summary>
        public string OverrideDeadband { get; set; }

        /// <summary>
        /// Gets or sets the OverrideProportionalGain.
        /// </summary>
        public string OverrideProportionalGain { get; set; }

        /// <summary>
        /// Gets or sets the Override Integral.
        /// </summary>
        public string OverrideIntegral { get; set; }

        /// <summary>
        /// Gets or sets the Override Derivative.
        /// </summary>
        public string OverrideDerivative { get; set; }

        /// <summary>
        /// Gets or sets the Override Scale Factor.
        /// </summary>
        public string OverrideScaleFactor { get; set; }

        /// <summary>
        /// Gets or sets the Override Output.
        /// </summary>
        public string OverrideOutput { get; set; }

        /// <summary>
        /// Gets or sets the Controller Mode Value.
        /// </summary>
        public string ControllerModeValue { get; set; }

        /// <summary>
        /// Gets or Sets the List of PID tag Groups
        /// </summary>
        public List<PIDDataModel> TagPIDDataModel { get; set; } = new List<PIDDataModel>();

    }
}