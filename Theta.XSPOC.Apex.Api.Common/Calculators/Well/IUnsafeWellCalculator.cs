using System;

namespace Theta.XSPOC.Apex.Api.Common.Calculators.Well
{
    /// <summary>
    /// Provides calculations related to wells that are not type-safe.
    /// </summary>
    public interface IUnsafeWellCalculator
    {

        /// <summary>
        /// Calculates the water cut.
        /// </summary>
        /// <param name="waterProductionBbl">The water production in barrels.</param>
        /// <param name="oilProductionBbl">The oil production in barrels.</param>
        /// <returns>The water cut in decimal.</returns>
        double CalculateWaterCut(double waterProductionBbl, double oilProductionBbl);

        /// <summary>
        /// Calculates the static gradient.
        /// </summary>
        /// <param name="waterSpecificGravity">The specific gravity of the water.</param>
        /// <param name="oilSpecificGravity">The specific gravity of the oil.</param>
        /// <param name="waterCutDecimal">The water cut in decimal.</param>
        /// <returns>The static gradient.</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="waterCutDecimal"/> is less than <c>0</c>
        /// OR
        /// <paramref name="waterCutDecimal"/> is greater than <c>1</c>.
        /// </exception>
        double CalculateStaticGradient(double waterSpecificGravity,
                                       double oilSpecificGravity,
                                       double waterCutDecimal);

        /// <summary>
        /// Calculates the fluid level from the surface.
        /// </summary>
        /// <param name="verticalPumpDepthFt">The vertical pump depth in feet.</param>
        /// <param name="fluidLevelAbovePumpFt">The fluid level above the pump in feet.</param>
        /// <returns>The fluid level from the surface in feet.</returns>
        double CalculateFluidLevelFromSurface(double verticalPumpDepthFt, double fluidLevelAbovePumpFt);

    }
}
