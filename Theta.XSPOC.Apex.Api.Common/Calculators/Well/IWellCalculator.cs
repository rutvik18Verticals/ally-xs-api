using System;
using Theta.XSPOC.Apex.Kernel.Quantity;
using Theta.XSPOC.Apex.Kernel.Quantity.Measures;

namespace Theta.XSPOC.Apex.Api.Common.Calculators.Well
{
    /// <summary>
    /// Provides calculations related to wells.
    /// </summary>
    public interface IWellCalculator
    {

        /// <summary>
        /// Calculates the water cut.
        /// </summary>
        /// <param name="waterProduction">The water production.</param>
        /// <param name="oilProduction">The oil production.</param>
        /// <returns>The water cut.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="oilProduction"/> is <c>null</c>
        /// OR
        /// <paramref name="waterProduction"/> is <c>null</c>.
        /// </exception>
        Quantity<Fraction> CalculateWaterCut(Quantity<LiquidVolume> waterProduction,
            Quantity<LiquidVolume> oilProduction);

        /// <summary>
        /// Calculates the static gradient.
        /// </summary>
        /// <param name="waterRelativeDensity">The relative density of the water.</param>
        /// <param name="oilRelativeDensity">The relative density of the oil.</param>
        /// <param name="waterCut">The water cut.</param>
        /// <returns>The static gradient.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="waterRelativeDensity"/> is <c>null</c>
        /// OR
        /// <paramref name="oilRelativeDensity"/> is <c>null</c>
        /// OR
        /// <paramref name="waterCut"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="waterCut"/> is less than <c>0</c>
        /// OR
        /// <paramref name="waterCut"/> is greater than <c>100%</c>.
        /// </exception>
        double CalculateStaticGradient(Quantity<RelativeDensity> waterRelativeDensity,
                                       Quantity<OilRelativeDensity> oilRelativeDensity,
                                       Quantity<Fraction> waterCut);

        /// <summary>
        /// Calculates the fluid level from the surface.
        /// </summary>
        /// <param name="verticalPumpDepth">The vertical pump depth.</param>
        /// <param name="fluidLevelAbovePump">The fluid level above the pump.</param>
        /// <returns>The fluid level from the surface.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="verticalPumpDepth"/> is <c>null</c>
        /// OR
        /// <paramref name="fluidLevelAbovePump"/> is <c>null</c>.
        /// </exception>
        Quantity<Length> CalculateFluidLevelFromSurface(Quantity<Length> verticalPumpDepth,
            Quantity<Length> fluidLevelAbovePump);

    }
}
