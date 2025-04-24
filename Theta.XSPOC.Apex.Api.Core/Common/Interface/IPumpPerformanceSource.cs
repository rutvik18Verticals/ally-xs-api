namespace Theta.XSPOC.Apex.Api.Core.Common.Interface
{
    /// <summary>
    /// Represents a pump performance source.
    /// </summary>
    public interface IPumpPerformanceSource
    {

        /// <summary>
        /// Returns a set of coefficients that can be used to calculate the Pump Curve
        /// </summary>
        /// <returns>A set of coefficients that can be used to calculate the Pump Curve</returns>
        Coefficients GetPressureCoefficients();

        /// <summary>
        /// Returns a set of coefficients that can be used to calculate the Power Curve
        /// </summary>
        /// <returns>A set of coefficients that can be used to calculate the Power Curve</returns>
        Coefficients GetPowerCoefficients();

        /// <summary>
        /// Returns a set of coefficients that can be used to calculate the Efficiency Curve
        /// </summary>
        /// <returns>A set of coefficients that can be used to calculate the Efficiency Curve</returns>
        Coefficients GetEfficiencyCoefficients();

    }
}
