using System;
using Theta.XSPOC.Apex.Kernel.Quantity;
using Theta.XSPOC.Apex.Kernel.Quantity.Measures;

namespace Theta.XSPOC.Apex.Api.Common.Calculators.ESP
{
    /// <summary>
    /// Provides ESP-related calculations that account for gas.
    /// </summary>
    public interface IGasAwareESPCalculator
    {

        /// <summary>
        /// Calculates the flowing bottomhole pressure gradient.
        /// </summary>
        /// <param name="compositeTubingRelativeDensity">The composite tubing relative density.</param>
        /// <returns>The flowing bottomhole pressure gradient.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="compositeTubingRelativeDensity"/> is <c>null</c>.
        /// </exception>
        double CalculateFlowingBHPGradient(Quantity<RelativeDensity> compositeTubingRelativeDensity);

    }
}
