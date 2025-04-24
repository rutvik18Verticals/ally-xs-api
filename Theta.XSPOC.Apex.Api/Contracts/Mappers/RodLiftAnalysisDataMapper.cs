using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Contracts.Responses;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;

namespace Theta.XSPOC.Apex.Api.Contracts.Mappers
{
    /// <summary>
    /// Maps Core.Models.Outputs.RodLiftAnalysisData to RodLiftAnalysisResponse object
    /// </summary>
    public static class RodLiftAnalysisDataMapper
    {

        /// <summary>
        /// Maps the RodLiftAnalysisDataOutput core model to RodLiftAnalysisResponse object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="dataOutput">The data from core model.</param>
        /// <returns></returns>
        public static RodLiftAnalysisResponse Map(string correlationId, RodLiftAnalysisDataOutput dataOutput)
        {
            if (dataOutput == null || dataOutput.Values == null
                || dataOutput.Result.Status == false)
            {
                return null;
            }

            var rodLiftAnalysisResponse = new RodLiftAnalysisResponse
            {
                Values = new Responses.RodLiftAnalysisValues()
            };

            var rodLiftAnalysisValues = dataOutput.Values;

            List<Responses.Values.ValueItem> input = new List<Responses.Values.ValueItem>();
            foreach (var value in rodLiftAnalysisValues.Input)
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

            rodLiftAnalysisResponse.Values.Input = input;

            List<Responses.Values.ValueItem> output = new List<Responses.Values.ValueItem>();
            foreach (var value in rodLiftAnalysisValues.Output)
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

            rodLiftAnalysisResponse.Values.Output = output;
            rodLiftAnalysisResponse.Id = correlationId;
            rodLiftAnalysisResponse.DateCreated = DateTime.UtcNow;

            return rodLiftAnalysisResponse;
        }

    }
}
