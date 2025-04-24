using Theta.XSPOC.Apex.Kernel.Quantity;
using Theta.XSPOC.Apex.Kernel.Quantity.Measures;

namespace Theta.XSPOC.Apex.Api.Core.Models.Outputs
{
    /// <summary>
    /// Represents the wellbore output model.
    /// </summary>
    public class GLAnalysisWellboreData
    {

        /// <summary>
        /// Gets or sets the well depth.
        /// </summary>
        public Quantity<Length> WellDepth { get; set; }

        /// <summary>
        /// Gets or sets the packer depth.
        /// </summary>
        public Quantity<Length> PackerDepth { get; set; }

        /// <summary>
        /// Gets or sets the gas injection depth.
        /// </summary>
        public Quantity<Length> GasInjectionDepth { get; set; }

        /// <summary>
        /// Gets or sets the has packer.
        /// </summary>
        public bool HasPacker { get; set; }

        /// <summary>
        /// Gets or sets the tubing depth.
        /// </summary>
        public Quantity<Length> TubingDepth { get; set; }

    }
}
