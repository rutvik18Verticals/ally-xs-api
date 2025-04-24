using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Contracts.Responses;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;

namespace Theta.XSPOC.Apex.Api.Contracts.Mappers
{
    /// <summary>
    /// Maps GLAnalysisCurveCoordinateDataOutput to GLAnalysisCurveCoordinateResponse object
    /// </summary>
    public static class GLAnalysisCurveCoordinateDataMapper
    {

        /// <summary>
        /// Maps the GLAnalysisCurveCoordinateResponse core model to GLAnalysisCurveCoordinateDataOutput object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="dataOutput">The data from core model.</param>
        /// <returns>The <seealso cref="GLAnalysisCurveCoordinateResponse"/>.</returns>
        public static GLAnalysisCurveCoordinateResponse Map(string correlationId, GLAnalysisCurveCoordinateDataOutput dataOutput)
        {
            if (dataOutput?.Values == null)
            {
                return null;
            }

            GLAnalysisCurveCoordinateResponse gLAnalysisCurveCoordinateResponse = new GLAnalysisCurveCoordinateResponse
            {
                Values = new List<GLAnalysisCurveCoordinateResponseValues>()
            };

            if (dataOutput.Values != null && dataOutput.Values.Count > 0)
            {
                gLAnalysisCurveCoordinateResponse.Values
                    = dataOutput.Values.Select(x => new GLAnalysisCurveCoordinateResponseValues
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Coordinates = x.CoordinatesOutput?.Select(x => new Coordinates
                        {
                            X = (float)x.X,
                            Y = (float)x.Y,
                        }).ToList(),
                        CurveTypeId = x.CurveTypeId,
                        DisplayName = x.DisplayName,
                    }).ToList();
            }
            gLAnalysisCurveCoordinateResponse.Id = correlationId;
            gLAnalysisCurveCoordinateResponse.DateCreated = DateTime.UtcNow;

            return gLAnalysisCurveCoordinateResponse;
        }

    }
}