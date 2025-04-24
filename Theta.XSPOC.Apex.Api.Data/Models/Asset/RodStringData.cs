using Theta.XSPOC.Apex.Kernel.Quantity;

namespace Theta.XSPOC.Apex.Api.Data.Models.Asset
{
    /// <summary>
    /// The rod string data.
    /// </summary>
    public record RodStringData
    {

        /// <summary>
        /// Gets or sets the rod string position number.
        /// </summary>
        public short? RodStringPositionNumber { get; set; }

        /// <summary>
        /// Gets or sets the rod string grade name.
        /// </summary>
        public string RodStringGradeName { get; set; }

        /// <summary>
        /// Gets or sets the diameter.
        /// </summary>
        public double? Diameter { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        public IValue Length { get; set; }

        /// <summary>
        /// Gets or sets the rod string size display name.
        /// </summary>
        public string RodStringSizeDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the rod string unit.
        /// </summary>
        public string UnitString { get; set; }

    }
}
