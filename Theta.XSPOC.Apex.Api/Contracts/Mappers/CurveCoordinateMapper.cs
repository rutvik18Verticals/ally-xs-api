using System;
using System.Collections.Generic;
using System.Linq;
using Theta.XSPOC.Apex.Api.Contracts.Responses;
using Theta.XSPOC.Apex.Api.Contracts.Responses.Values;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;

namespace Theta.XSPOC.Apex.Api.Contracts.Mappers
{
    /// <summary>
    /// Map curve coordinate response model.
    /// </summary>
    public class CurveCoordinateMapper
    {

        /// <summary>
        /// Map curve coordinates response.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="coreModel">The list of curve coordinates core model.</param>
        /// <returns>A <seealso cref="CardDateResponse"/> representing the provided correction id and core model.
        /// in the domain.</returns>
        public static CurveCoordinatesResponse Map(string correlationId,
            IList<CurveCoordinateValues> coreModel)
        {
            if (coreModel != null)
            {
                var values = coreModel.Select(x => new CurveCoordinateValue()
                {
                    Id = x.Id,
                    CurveTypeId = x.CurveTypeId,
                    Name = x.Name,
                    DisplayName = x.DisplayName,
                    Coordinates = x.Coordinates.Select(y => new CoordinatesValue()
                    {
                        X = y.X,
                        Y = y.Y,
                    }).ToList(),
                }).ToList();

                return new CurveCoordinatesResponse()
                {
                    Id = correlationId,
                    DateCreated = DateTime.UtcNow,
                    Values = values,
                };
            }

            return new CurveCoordinatesResponse()
            {
                Id = correlationId,
                DateCreated = DateTime.UtcNow,
                Values = null,
            };
        }

    }
}
