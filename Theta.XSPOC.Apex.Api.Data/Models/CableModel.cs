using Theta.XSPOC.Apex.Kernel.Quantity.Measures;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents a cable for an ESP well
    /// </summary>
    public class CableModel
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
        /// Gets or sets the diameter
        /// </summary>
        public Length Diameter { get; set; }

        /// <summary>
        /// Gets or sets the series
        /// </summary>
        public string Series { get; set; }

        /// <summary>
        /// Gets or sets the cable type
        /// </summary>
        public string CableType { get; set; }

        /// <summary>
        /// Gets or sets the cable description
        /// </summary>
        public string CableDescription { get; set; }
    }
}