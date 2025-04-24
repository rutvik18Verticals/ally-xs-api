using System;

namespace Theta.XSPOC.Apex.Api.Core.Common
{
    /// <summary>
    /// Represent the class for interpolating a phrase containing esp analysis phrase placeholder.
    /// </summary>
    public static class ESPAnalysisPhraseInterpolator
    {

        /// <summary>
        /// Interpolates a phrase containing <see cref="ESPAnalysisPhrasePlaceholder"/>s
        /// with ESP analysis data.
        /// </summary>
        /// <param name="phrase">The phrase to be interpolated.</param>
        /// <param name="analysisInput">The analysis input</param>
        /// <param name="analysisOutput">The analysis output.</param>
        /// <returns>A phrase with interpolated values.</returns>
        /// <exception cref="ArgumentNullException">
        /// phrase is null
        /// or
        /// analysisInput is null
        /// or
        /// analysis output is null
        /// or
        /// unitConfiguration is null
        /// </exception>
        public static string Interpolate(
            string phrase,
            ESPAnalysisInput analysisInput,
            AnalysisOutput analysisOutput)
        {
            if (phrase == null)
            {
                var phraseParamName = nameof(phrase);
                throw new ArgumentNullException($"{phraseParamName} cannot be null");
            }

            if (analysisInput == null)
            {
                var analysisInputParamName = nameof(analysisInput);
                throw new ArgumentNullException($"{analysisInputParamName} cannot be null");
            }

            if (analysisOutput == null)
            {
                var analysisOutputParamName = nameof(analysisOutput);
                throw new ArgumentNullException($"{analysisOutputParamName} cannot be null");
            }

            var result = phrase;

            foreach (var placeholder in EnhancedEnumBase.GetValues<ESPAnalysisPhrasePlaceholder>())
            {
                if (placeholder == ESPAnalysisPhrasePlaceholder.CalculatedFluidLevelAbovePump)
                {
                    var value = analysisOutput.CalculatedFluidLevelAbovePump;
                    if (value != null)
                    {
                        var convertedValue = MathUtility.RoundToSignificantDigits(Convert.ToDouble(value), 4);
                        result = result.Replace(placeholder.Name, convertedValue.ToString());
                    } // value is not null
                } // placeholder is CalculatedFluidLevelAbovePump
                else if (placeholder == ESPAnalysisPhrasePlaceholder.OilRate)
                {
                    var value = analysisInput.OilRate;
                    if (value != null)
                    {
                        result = result.Replace(placeholder.Name, value.ToString());
                    } // value is not null
                } // placeholder is OilRate
                else if (placeholder == ESPAnalysisPhrasePlaceholder.MaxPotentialProductionRate)
                {
                    var value = analysisOutput.Diagnostics?.MaxPotentialProductionRate;
                    if (value != null)
                    {
                        result = result.Replace(placeholder.Name, Math.Round(value.Value, 0).ToString());
                    } // value is not null
                } // placeholder is MaxPotentialProductionRate
            } // end foreach

            return result;
        }

    }
}
