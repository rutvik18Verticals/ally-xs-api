namespace Theta.XSPOC.Apex.Api.Common.Calculators.ESP
{
    /// <summary>
    /// Provides ESP-related calculations that are not type-safe.
    /// </summary>
    /// <seealso cref="IUnsafeGasAwareESPCalculator"/>.
    public class UnsafeESPCalculator : IUnsafeGasAwareESPCalculator
    {

        #region IUnsafeGasAwareESPCalculator Implementation

        /// <summary>
        /// Calculates the flowing bottomhole pressure gradient.
        /// </summary>
        /// <param name="compositeTubingSpecificGravity">The composite tubing specific gravity.</param>
        /// <returns>
        /// The flowing bottomhole pressure gradient.
        /// </returns>
        public double CalculateFlowingBHPGradient(double compositeTubingSpecificGravity)
        {
            return compositeTubingSpecificGravity * Constants.PsiPerFoot;
        }

        #endregion

    }
}
