using System;
using Theta.XSPOC.Apex.Kernel.Quantity.Measures;
using Theta.XSPOC.Apex.Kernel.Quantity;

namespace Theta.XSPOC.Apex.Api.Common.Calculators.Well
{
    /// <summary>
    /// Provides various calculations for wells.
    /// </summary>
    public class WellCalculator : IGasAwareWellCalculator
    {

        #region Private Fields

        private readonly IUnsafeWellCalculator _unsafeWellCalculator;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WellCalculator"/> class.
        /// </summary>
        /// <param name="unsafeWellCalculator">The <seealso cref="IUnsafeWellCalculator"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="unsafeWellCalculator"/> is null.
        /// </exception>
        public WellCalculator(IUnsafeWellCalculator unsafeWellCalculator)
        {
            _unsafeWellCalculator =
                unsafeWellCalculator ?? throw new ArgumentNullException(nameof(unsafeWellCalculator));
        }

        #endregion

        #region IGasAwareWellCalculator Implementation

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
        public Quantity<Fraction> CalculateWaterCut(Quantity<LiquidVolume> waterProduction,
            Quantity<LiquidVolume> oilProduction)
        {
            if (waterProduction == null)
            {
                throw new ArgumentNullException(nameof(waterProduction));
            }

            if (oilProduction == null)
            {
                throw new ArgumentNullException(nameof(oilProduction));
            }

            var waterProductionBbl = waterProduction.ConvertTo(LiquidVolume.Barrel);
            var oilProductionBbl = oilProduction.ConvertTo(LiquidVolume.Barrel);

            var result = _unsafeWellCalculator.CalculateWaterCut(waterProductionBbl.Amount, oilProductionBbl.Amount);

            return Fraction.FromDecimal(result);
        }

        /// <summary>
        /// Calculates a static gradient.
        /// </summary>
        /// <param name="waterRelativeDensity">The relative density of water.</param>
        /// <param name="oilRelativeDensity">The relative density of oil.</param>
        /// <param name="waterCut">The water cut.</param>
        /// <returns>The static gradient.</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="waterCut"/> is less than <c>0</c>
        /// OR
        /// <paramref name="waterCut"/> is greater than <c>100%</c>.
        /// </exception>
        public double CalculateStaticGradient(Quantity<RelativeDensity> waterRelativeDensity,
                                              Quantity<OilRelativeDensity> oilRelativeDensity,
                                              Quantity<Fraction> waterCut)
        {
            if (waterRelativeDensity == null)
            {
                throw new ArgumentNullException(nameof(waterRelativeDensity));
            }
            if (oilRelativeDensity == null)
            {
                throw new ArgumentNullException(nameof(oilRelativeDensity));
            }
            if (waterCut == null)
            {
                throw new ArgumentNullException(nameof(waterCut));
            }

            var waterSpecificGravity = waterRelativeDensity.ConvertTo(RelativeDensity.SpecificGravity);
            var oilSpecificGravity = oilRelativeDensity.ConvertTo(RelativeDensity.SpecificGravity);
            var waterCutDecimal = waterCut.ConvertTo(Fraction.Decimal);

            return _unsafeWellCalculator.CalculateStaticGradient(waterSpecificGravity.Amount,
                                                             oilSpecificGravity.Amount,
                                                             waterCutDecimal.Amount);
        }

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
        public Quantity<Length> CalculateFluidLevelFromSurface(Quantity<Length> verticalPumpDepth,
            Quantity<Length> fluidLevelAbovePump)
        {
            if (verticalPumpDepth == null)
            {
                throw new ArgumentNullException(nameof(verticalPumpDepth));
            }

            if (fluidLevelAbovePump == null)
            {
                throw new ArgumentNullException(nameof(fluidLevelAbovePump));
            }

            var verticalPumpDepthFt = verticalPumpDepth.ConvertTo(Length.Foot);
            var fluidLevelAbovePumpFt = fluidLevelAbovePump.ConvertTo(Length.Foot);
            double result =
                _unsafeWellCalculator.CalculateFluidLevelFromSurface(verticalPumpDepthFt.Amount,
                    fluidLevelAbovePumpFt.Amount);

            return Length.FromFeet(result);
        }

        #endregion

    }
}
