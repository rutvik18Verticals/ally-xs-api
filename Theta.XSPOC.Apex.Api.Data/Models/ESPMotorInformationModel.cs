using System.Collections.Generic;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Class represents the esp manufacturer model.
    /// </summary>
    public class ESPMotorInformationModel
    {

        /// <summary>
        /// Gets or set the cable.
        /// </summary>
        public string Cable { get; set; }

        /// <summary>
        /// Gets or set the cable description.
        /// </summary>
        public string CableDescription { get; set; }

        /// <summary>
        /// Gets or set the cable type.
        /// </summary>
        public string CableType { get; set; }

        /// <summary>
        /// Gets or set the cable series.
        /// </summary>
        public string CableSeries { get; set; }

        /// <summary>
        /// Gets or set the motor lead.
        /// </summary>
        public string MotorLead { get; set; }

        /// <summary>
        /// Gets or set the seal.
        /// </summary>
        public string Seal { get; set; }

        /// <summary>
        /// Gets or set the seal series.
        /// </summary>
        public string SealSeries { get; set; }

        /// <summary>
        /// Gets or set the seal model.
        /// </summary>
        public string SealModel { get; set; }

        /// <summary>
        /// Gets or set the motor.
        /// </summary>
        public string Motor { get; set; }

        /// <summary>
        /// Gets or set the motor series.
        /// </summary>
        public string MotorSeries { get; set; }

        /// <summary>
        /// Gets or set the motor lead series.
        /// </summary>
        public string MotorLeadSeries { get; set; }

        /// <summary>
        /// Gets or set the motor lead type.
        /// </summary>
        public string MotorLeadType { get; set; }

        /// <summary>
        /// Gets or set the motor lead description.
        /// </summary>
        public string MotorLeadDescription { get; set; }

        /// <summary>
        /// Gets or set the motor model.
        /// </summary>
        public string MotorModel { get; set; }

        /// <summary>
        /// Gets or sets the pump configurations
        /// </summary>
        public IList<PumpConfigurationModel> PumpConfigurations { get; set; } = new List<PumpConfigurationModel>();

        /// <summary>
        /// Gets or sets the motors.
        /// </summary>
        public IList<MotorModel> Motors { get; set; } = new List<MotorModel>();

        /// <summary>
        /// Gets or sets the motor leads.
        /// </summary>
        public IList<MotorLeadModel> MotorLeads { get; set; } = new List<MotorLeadModel>();

        /// <summary>
        /// Gets or sets the cables.
        /// </summary>
        public IList<CableModel> Cables { get; set; } = new List<CableModel>();

        /// <summary>
        /// Gets or sets the seals.
        /// </summary>
        public IList<SealModel> Seals { get; set; } = new List<SealModel>();

    }
}
