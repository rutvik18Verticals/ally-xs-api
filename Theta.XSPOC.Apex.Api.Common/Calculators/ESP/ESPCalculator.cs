using System;
using Theta.XSPOC.Apex.Kernel.Quantity;
using Theta.XSPOC.Apex.Kernel.Quantity.Measures;

namespace Theta.XSPOC.Apex.Api.Common.Calculators.ESP
{
    /// <summary>
    /// Provides various calculations related to ESPs.
    /// </summary>
    public class ESPCalculator : IGasAwareESPCalculator
    {

        #region Private Fields

        private readonly IUnsafeGasAwareESPCalculator _unsafeEspCalculator;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ESPCalculator"/> class.
        /// </summary>
        /// <param name="unsafeEspCalculator">The <seealso cref="IUnsafeGasAwareESPCalculator"/></param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="unsafeEspCalculator"/> is null.
        /// </exception>
        public ESPCalculator(IUnsafeGasAwareESPCalculator unsafeEspCalculator)
        {
            _unsafeEspCalculator = unsafeEspCalculator ?? throw new ArgumentNullException(nameof(unsafeEspCalculator));
        }

        #endregion

        #region IGasAwareESPCalculator Implementation

        /// <summary>
        /// Calculates a flowing bottomhole pressure gradient.
        /// </summary>
        /// <param name="compositeTubingRelativeDensity">
        /// The relative density from which the gradient is calculated.
        /// </param>
        /// <returns>The flowing bottomhole pressure gradient.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="compositeTubingRelativeDensity"/> is <c>null</c>.
        /// </exception>
        public double CalculateFlowingBHPGradient(Quantity<RelativeDensity> compositeTubingRelativeDensity)
        {
            if (compositeTubingRelativeDensity == null)
            {
                throw new ArgumentNullException(nameof(compositeTubingRelativeDensity));
            }

            var compositeTubingSpecificGravity = compositeTubingRelativeDensity.ConvertTo(RelativeDensity.SpecificGravity);

            return _unsafeEspCalculator.CalculateFlowingBHPGradient(compositeTubingSpecificGravity.Amount);
        }

        #endregion

    }
}
