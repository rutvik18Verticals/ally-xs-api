namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represents the source of a set of coefficients.
    /// </summary>
    public enum PumpCoefficientSource
    {

        /// <summary>
        /// Specifies that the coefficeints are for pressure.
        /// </summary>
        Pressure,

        /// <summary>
        /// Specifies that the coefficeints are for power.
        /// </summary>
        Power,

        /// <summary>
        /// Specifies that the coefficeints are for efficiency.
        /// </summary>
        Efficiency,

    }
}
