using System;
using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Common.Converters;
using Theta.XSPOC.Apex.Api.Contracts.Responses;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;

namespace Theta.XSPOC.Apex.Api.Contracts.Mappers
{
    /// <summary>
    /// Maps Core.Models.Outputs.WellTestTestDataOutput to WellTestResponse object
    /// </summary>
    public static class WellTestValues
    {

        /// <summary>
        /// Maps the WellTestTestDataOutput core model to WellTestResponse object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="dataOutput">The data from core model.</param>
        /// <returns></returns>
        public static WellTestResponse Map(string correlationId, WellTestDataOutput dataOutput)
        {
            if (dataOutput?.Values == null)
            {
                return null;
            }

            var wellTestResponse = new WellTestResponse
            {
                Values = new List<Responses.Values.WellTestData>()
            };

            if (dataOutput.Values != null)
            {
                var wellTestValues = dataOutput.Values;

                foreach (var value in wellTestValues)
                {
                    wellTestResponse.Values.Add(new Responses.Values.WellTestData
                    {
                        Date = value.Date,
                        AnalysisTypeName = value.AnalysisTypeName == "Well Test Analysis" ? "Well test" : value.AnalysisTypeName,
                    });
                }
            }

            wellTestResponse.Id = correlationId;
            wellTestResponse.DateCreated = DateTime.UtcNow;

            return wellTestResponse;
        }

        /// <summary>
        /// Maps the AnalysisKeyDataOutput core model to AnalysisKeyResponse object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="dataOutput">The data from core model.</param>
        /// <returns>The <seealso cref="GLAnalysisWellTestResponse"/> object.</returns>
        public static GLAnalysisWellTestResponse MapGLAnalysis(string correlationId, GLAnalysisWellTestDataOutput dataOutput)
        {
            if (dataOutput?.Values == null)
            {
                return null;
            }

            var values = new List<Responses.Values.GLAnalysisWellTestData>();
            if (dataOutput.Values != null && dataOutput.Values.Count > 0)
            {
                foreach (var value in dataOutput.Values)
                {
                    values.Add(new Responses.Values.GLAnalysisWellTestData
                    {
                        Date = value.Date,
                        AnalysisResultId = value.AnalysisResultId,
                        AnalysisTypeId = value.AnalysisTypeId,
                        AnalysisTypeName = PhraseConverter.ConvertFirstToUpperRestToLower(value.AnalysisTypeName
                        == "Well Test Analysis" ? "Well test" : value.AnalysisTypeName),
                    });
                }
            }

            GLAnalysisWellTestResponse wellTestResponse = new GLAnalysisWellTestResponse
            {
                Values = values,
                Id = correlationId,
                DateCreated = DateTime.UtcNow,
            };

            return wellTestResponse;
        }

    }
}
