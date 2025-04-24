using Theta.XSPOC.Apex.Kernel.Quantity;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// Defines register data for the most recent received value.
    /// </summary>
    public record RegisterData
    {

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public int? Address { get; set; }

        /// <summary>
        /// Gets or sets the register description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the register data type.
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Gets or sets the register string value.
        /// </summary>
        public string StringValue { get; set; }

        /// <summary>
        /// Gets or sets the register value.
        /// </summary>
        public IValue Value { get; set; }

        /// <summary>
        /// Gets or sets the register format.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the register display order.
        /// </summary>
        public int? Order { get; set; }

        /// <summary>
        /// Gets or sets the register units.
        /// </summary>
        public string Units { get; set; }

        /// <summary>
        /// Gets or sets the register bit location, this is leverage for facility tags only.
        /// </summary>
        public string Bit { get; set; }

        /// <summary>
        /// Gets or sets the register unit type.
        /// </summary>
        public int? UnitType { get; set; }

        /// <summary>
        /// Gets or sets the register phrase id.
        /// </summary>
        public string PhraseId { get; set; }

        /// <summary>
        /// Gets or sets the register state id for state lookup.
        /// </summary>
        public string StateId { get; set; }

        /// <summary>
        /// Gets or sets the register decimal places.
        /// </summary>
        public int Decimals { get; set; }

    }
}