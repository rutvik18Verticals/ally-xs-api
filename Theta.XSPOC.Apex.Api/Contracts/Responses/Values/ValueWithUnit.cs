namespace Theta.XSPOC.Apex.Api.Contracts.Responses.Values
{
    /// <summary>
    /// Represents a value with unit symbol.
    /// </summary>
    public class ValueWithUnit<T>
    {

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        public string Unit { get; set; }

    }
}
