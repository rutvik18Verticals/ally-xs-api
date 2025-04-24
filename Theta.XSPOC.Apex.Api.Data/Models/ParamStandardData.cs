using Theta.XSPOC.Apex.Kernel.Quantity;

namespace Theta.XSPOC.Apex.Api.Data.Models
{
    /// <summary>
    /// This is a record that represents the param standard data.
    /// </summary>
    public record ParamStandardData
    {

        /// <summary>
        /// Gets or sets the node id.
        /// </summary>
        public string NodeId { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        public int Address { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public IValue Value { get; set; }

        /// <summary>
        /// Gets or sets the param standard type.
        /// </summary>
        public int? ParamStandardType { get; set; }

        /// <summary>
        /// Gets or sets the decimals.
        /// </summary>
        public int? Decimals { get; set; }

        /// <summary>
        /// Gets or sets the string value.
        /// </summary>
        public string StringValue { get; set; }

        /// <summary>
        /// Gets or sets the data type.
        /// </summary>
        public int? DataType { get; set; }

        /// <summary>
        /// Gets or sets the unit type id.
        /// </summary>
        public int UnitTypeId { get; set; }

        /// <summary>
        /// Gets or sets the units string.
        /// </summary>
        public string Units { get; set; }

    }
}