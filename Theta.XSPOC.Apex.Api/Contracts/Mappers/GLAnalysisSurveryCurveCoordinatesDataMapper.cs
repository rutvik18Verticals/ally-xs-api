using System;
using System.Linq;
using Theta.XSPOC.Apex.Api.Contracts.Responses;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;

namespace Theta.XSPOC.Apex.Api.Contracts.Mappers
{
    /// <summary>
    /// Map curve coordinate response model.
    /// </summary>
    public class GLAnalysisSurveryCurveCoordinatesDataMapper
    {

        /// <summary>
        /// Map curve coordinates response.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="coreModel">gl analysis curve coordinate data output.</param>
        /// <returns>A <seealso cref="CurveCoordinatesResponse"/> representing the provided correction id and core model.
        /// in the domain.</returns>
        public static GLAnalysisSurveyCurveCoordinatesResponse Map(string correlationId,
            GLAnalysisCurveCoordinateDataOutput coreModel)
        {
            if (coreModel?.Values != null)
            {
                var values = coreModel.Values.Select(x => new GLAnalysisCurveCoordinateData()
                {
                    Id = x.Id,
                    CurveTypeId = x.CurveTypeId,
                    DisplayName = x.DisplayName,
                    Name = x.Name,
                    CoordinatesOutput = x.CoordinatesOutput.Select(y => new CoordinatesData<float>()
                    {
                        X = y.X,
                        Y = y.Y,
                    }).ToList(),
                }).ToList();

                return new GLAnalysisSurveyCurveCoordinatesResponse()
                {
                    Id = correlationId,
                    DateCreated = DateTime.UtcNow,
                    Values = values,
                };
            }

            return new GLAnalysisSurveyCurveCoordinatesResponse()
            {
                Id = correlationId,
                DateCreated = DateTime.UtcNow,
                Values = null,
            };
        }

    }
}
