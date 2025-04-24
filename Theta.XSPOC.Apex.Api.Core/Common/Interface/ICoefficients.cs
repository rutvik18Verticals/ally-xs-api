namespace Theta.XSPOC.Apex.Api.Core.Common.Interface
{
    /// <summary>
    /// Represents a set of coefficients.
    /// </summary>
    public interface ICoefficients
    {

        /// <summary>
        /// Gets the intercept.
        /// </summary>
        double Intercept { get; }

        /// <summary>
        /// Gets the first-order coefficient.
        /// </summary>
        double FirstOrder { get; }

        /// <summary>
        /// Gets the second-order coefficient.
        /// </summary>
        double SecondOrder { get; }

        /// <summary>
        /// Gets the third-order coefficient.
        /// </summary>
        double ThirdOrder { get; }

        /// <summary>
        /// Gets the fourth-order coefficient.
        /// </summary>
        double FourthOrder { get; }

        /// <summary>
        /// Gets the fifth-order coefficient.
        /// </summary>
        double FifthOrder { get; }

        /// <summary>
        /// Gets the sixth-order coefficient.
        /// </summary>
        double? SixthOrder { get; }

        /// <summary>
        /// Gets the seventh-order coefficient.
        /// </summary>
        double? SeventhOrder { get; }

        /// <summary>
        /// Gets the eighth-order coefficient.
        /// </summary>
        double? EighthOrder { get; }

        /// <summary>
        /// Gets the ninth-order coefficient.
        /// </summary>
        double? NinthOrder { get; }

    }
}
