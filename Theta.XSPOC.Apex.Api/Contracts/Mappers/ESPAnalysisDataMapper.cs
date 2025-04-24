using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Contracts.Responses;
using Theta.XSPOC.Apex.Api.Contracts.Responses.Values;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;
using Theta.XSPOC.Apex.Kernel.Quantity;

namespace Theta.XSPOC.Apex.Api.Contracts.Mappers
{
    /// <summary>
    /// Maps Core.Models.Outputs.ESPAnalysisDataOutput to ESPAnalysisResponse object
    /// </summary>
    public static class ESPAnalysisDataMapper
    {

        /// <summary>
        /// Maps the ESPAnalysisDataOutput core model to ESPAnalysisResponse object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="dataOutput">The data from core model.</param>
        /// <returns>The <seealso cref="ESPAnalysisResponse"/>.</returns>
        public static ESPAnalysisResponse Map(string correlationId, ESPAnalysisDataOutput dataOutput)
        {
            var espAnalysisResponse = new ESPAnalysisResponse
            {
                Values = new Responses.ESPAnalysisValues()
            };

            if (dataOutput?.Values == null)
            {
                return null;
            }

            var espAnalysisValues = dataOutput.Values;

            List<Responses.Values.ValueItem> input = new List<Responses.Values.ValueItem>();
            foreach (var value in espAnalysisValues.Inputs)
            {
                input.Add(new Responses.Values.ValueItem()
                {
                    Id = value.Id,
                    Name = value.Name,
                    DisplayName = value.DisplayName,
                    Value = value.Value,
                    DataTypeId = value.DataTypeId,
                    DisplayValue = value.DisplayValue,
                    MeasurementAbbreviation = value.MeasurementAbbreviation,
                    SourceId = value.SourceId,
                });
            }

            espAnalysisResponse.Values.Input = input;

            List<Responses.Values.ValueItem> output = new List<Responses.Values.ValueItem>();
            foreach (var value in espAnalysisValues.Outputs)
            {
                output.Add(new Responses.Values.ValueItem()
                {
                    Id = value.Id,
                    Name = value.Name,
                    DisplayName = value.DisplayName,
                    Value = value.Value,
                    DataTypeId = value.DataTypeId,
                    DisplayValue = value.DisplayValue,
                    MeasurementAbbreviation = value.MeasurementAbbreviation,
                    SourceId = value.SourceId,
                });
            }

            espAnalysisResponse.Values.Output = output;
            if (espAnalysisValues.IsGasHandlingEnabled)
            {
                espAnalysisResponse.Values.IsGasHandlingEnabled = true;
                List<Responses.Values.ValueItem> gasHandlingInput = new List<Responses.Values.ValueItem>();
                foreach (var value in espAnalysisValues.GasHandlingInputs)
                {
                    gasHandlingInput.Add(new Responses.Values.ValueItem()
                    {
                        Id = value.Id,
                        Name = value.Name,
                        DisplayName = value.DisplayName,
                        Value = value.Value,
                        DataTypeId = value.DataTypeId,
                        DisplayValue = value.DisplayValue,
                        MeasurementAbbreviation = value.MeasurementAbbreviation,
                        SourceId = value.SourceId,
                    });
                }

                espAnalysisResponse.Values.GasHandlingInputs = gasHandlingInput;

                List<Responses.Values.ValueItem> gasHandlingOutput = new List<Responses.Values.ValueItem>();
                foreach (var value in espAnalysisValues.GasHandlingOutputs)
                {
                    gasHandlingOutput.Add(new Responses.Values.ValueItem()
                    {
                        Id = value.Id,
                        Name = value.Name,
                        DisplayName = value.DisplayName,
                        Value = value.Value,
                        DataTypeId = value.DataTypeId,
                        DisplayValue = value.DisplayValue,
                        MeasurementAbbreviation = value.MeasurementAbbreviation,
                        SourceId = value.SourceId,
                    });
                }

                espAnalysisResponse.Values.GasHandlingOutputs = gasHandlingOutput;
            }

            espAnalysisResponse.PressureProfile = MapPressureProfile(dataOutput.PressureProfile);

            espAnalysisResponse.Id = correlationId;
            espAnalysisResponse.DateCreated = DateTime.UtcNow;

            return espAnalysisResponse;
        }

        #region Private Methods

        private static ESPPressureProfile MapPressureProfile(ESPPressureProfileData pressureProfile)
        {
            if (pressureProfile == null)
            {
                return null;
            }

            var result = new ESPPressureProfile();

            result.IsValid = pressureProfile.IsValid;
            result.ErrorMessage = pressureProfile.ErrorMessage;

            result.CasingPressure = CreateValueWithUnit(pressureProfile.CasingPressure);
            result.TubingPressure = CreateValueWithUnit(pressureProfile.TubingPressure);
            result.PumpIntakePressure = CreateValueWithUnit(pressureProfile.PumpIntakePressure);

            result.UsedDischargeGaugeInAnalysis = pressureProfile.UseDischargeGaugeInAnalysis;
            result.PumpDischargePressure = CreateValueWithUnit(pressureProfile.PumpDischargePressure);

            result.PumpStaticPressure = CreateValueWithUnit(pressureProfile.PumpStaticPressure);
            result.FluidLevel = CreateValueWithUnit(pressureProfile.FluidLevel);
            result.VerticalPumpDepth = CreateValueWithUnit(pressureProfile.VerticalPumpDepth);
            result.PerforationDepth = CreateValueWithUnit(pressureProfile.PerforationDepth);
            result.PumpFrictionDelta = CreateValueWithUnit(pressureProfile.PumpFrictionDelta);
            result.PumpPressureDelta = CreateValueWithUnit(pressureProfile.PumpPressureDelta);
            result.FlowingBHP = CreateValueWithUnit(pressureProfile.FlowingBottomholePressure);
            result.FlowingBHPGradient = pressureProfile.FlowingBottomholePressureGradient;
            result.StaticGradient = pressureProfile.StaticGradient;
            result.PressureGradientUnits = pressureProfile.PressureGradientUnits;
            result.DischargeGaugePressure = CreateValueWithUnit(pressureProfile.DischargeGaugePressure);
            result.DischargeGaugeDepth = CreateValueWithUnit(pressureProfile.DischargeGaugeDepth);

            return result;
        }

        private static ValueWithUnit<double> CreateValueWithUnit(Quantity value)
        {
            if (value == null)
            {
                return null;
            }

            return new ValueWithUnit<double>()
            {
                Value = value.Amount,
                Unit = value.Unit.Symbol,
            };
        }

        #endregion

    }
}
