using Theta.XSPOC.Apex.Kernel.Quantity.Measures;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Represents a seal for an ESP well
    /// </summary>
    public class SealModel
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
        /// Gets or sets the series name
        /// </summary>
        public string Series { get; set; }

        /// <summary>
        /// Gets or sets the seal model name
        /// </summary>
        public string SealModelName { get; set; }

        /// <summary>
        /// Gets or sets the diameter
        /// </summary>
        public Length Diameter { get; set; }
    }
}