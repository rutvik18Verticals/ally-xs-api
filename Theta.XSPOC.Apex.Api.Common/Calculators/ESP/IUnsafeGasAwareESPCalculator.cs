namespace Theta.XSPOC.Apex.Api.Common.Calculators.ESP
{
    /// <summary>
    /// Provides ESP-related calculations that account for gas and are not type-safe.
    /// </summary>
    public interface IUnsafeGasAwareESPCalculator
    {

        /// <summary>
        /// Calculates the flowing bottomhole pressure gradient.
        /// </summary>
        /// <param name="compositeTubingSpecificGravity">The composite tubing specific gravity.</param>
        /// <returns>
        /// The flowing bottomhole pressure gradient.
        /// </returns>
        double CalculateFlowingBHPGradient(double compositeTubingSpecificGravity);

    }
}
