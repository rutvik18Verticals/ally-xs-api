namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents a motor lead for an ESP well
    /// </summary>
    public class MotorLeadModel
    {
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the manufacturer
        /// </summary>
        public ESPManufacturerModel Manufacturer { get; set; }

        /// <summary>
        /// Gets or sets the model name
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the series
        /// </summary>
        public string Series { get; set; }

        /// <summary>
        /// Gets or sets the motor lead type
        /// </summary>
        public string MotorLeadType { get; set; }

        /// <summary>
        /// Gets or sets the motor lead description
        /// </summary>
        public string MotorLeadDescription { get; set; }

    }
}