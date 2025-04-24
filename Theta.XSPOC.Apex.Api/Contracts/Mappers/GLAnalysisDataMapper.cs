using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Contracts.Responses;
using Theta.XSPOC.Apex.Api.Core.Common;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;

namespace Theta.XSPOC.Apex.Api.Contracts.Mappers
{
    /// <summary>
    /// Maps Core.Models.Outputs.GLAnalysisDataOutput to GLAnalysisResponse object
    /// </summary>
    public static class GLAnalysisDataMapper
    {

        /// <summary>
        /// Maps the GLAnalysisDataOutput core model to GLAnalysisResponse object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="dataOutput">The data from core model.</param>
        /// <returns>The <seealso cref="GLAnalysisResponse"/>.</returns>
        public static GLAnalysisResponse Map(string correlationId, GLAnalysisDataOutput dataOutput)
        {
            var glAnalysisResponse = new GLAnalysisResponse
            {
                Values = new Responses.Values.GLAnalysisValues()
            };

            if (dataOutput?.Values == null)
            {
                return null;
            }

            var glAnalysisValues = dataOutput.Values;

            List<Responses.Values.ValueItem> input = new List<Responses.Values.ValueItem>();
            if (glAnalysisValues.Inputs != null)
            {
                foreach (var value in glAnalysisValues.Inputs)
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

                glAnalysisResponse.Values.Inputs = input;
            }

            List<Responses.Values.ValueItem> output = new List<Responses.Values.ValueItem>();
            if (glAnalysisValues.Inputs != null)
            {
                foreach (var value in glAnalysisValues.Outputs)
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

                glAnalysisResponse.Values.Outputs = output;
            }

            List<FlowControlDeviceAnalysisValues> flowControlDevices = new List<FlowControlDeviceAnalysisValues>();
            if (glAnalysisValues.Valves != null)
            {
                foreach (var value in glAnalysisValues.Valves)
                {
                    flowControlDevices.Add(new FlowControlDeviceAnalysisValues()
                    {
                        Position = value.Position,
                        Description = value.Description,
                        Depth = value.Depth,
                        OpeningPressureAtDepth = value.OpeningPressureAtDepth,
                        ClosingPressureAtDepth = value.ClosingPressureAtDepth,
                        InjectionPressureAtDepth = value.InjectionPressureAtDepth,
                        TubingCriticalVelocityAtDepth = value.TubingCriticalVelocityAtDepth,
                        Status = value.Status,
                        IsOpen = value.IsOpen,
                        IsInjecting = value.IsInjecting,
                    });
                }

                glAnalysisResponse.Values.Valves = flowControlDevices;
            }

            if (glAnalysisValues.WellboreViewData != null)
            {
                glAnalysisResponse.Values.WellboreViewData = new GLAnalysisWellboreViewData()
                {
                    WellDepth = glAnalysisValues.WellboreViewData.WellDepth,
                    InjectionDepth = glAnalysisValues.WellboreViewData.InjectionDepth,
                    PackerDepth = glAnalysisValues.WellboreViewData.PackerDepth,
                    HasPacker = glAnalysisValues.WellboreViewData.HasPacker,
                    TubingDepth = glAnalysisValues.WellboreViewData.TubingDepth,
                    TopFluidColumn = glAnalysisValues.WellboreViewData.TopFluidColumn,
                    BottomFluidColumn = glAnalysisValues.WellboreViewData.BottomFluidColumn,
                    CasingFluidColumn = glAnalysisValues.WellboreViewData.CasingFluidColumn,
                    ErrorMessage = glAnalysisValues.WellboreViewData.ErrorMessage,
                };
            }

            glAnalysisResponse.Id = correlationId;
            glAnalysisResponse.DateCreated = DateTime.UtcNow;

            return glAnalysisResponse;
        }

    }
}
