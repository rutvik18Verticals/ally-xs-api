using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.Contracts.Responses;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;

namespace Theta.XSPOC.Apex.Api.Contracts.Mappers
{
    /// <summary>
    /// The mapper for the card coordinate.
    /// </summary>
    public static class CardCoordinateDataMapper
    {

        /// <summary>
        /// Map card date response.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="coreModel">The core model.</param>
        /// <returns>A <seealso cref="CardCoordinateResponse"/> representing the provided <paramref name="coreModel"/> 
        /// in the domain.</returns>
        public static CardCoordinateResponse Map(string correlationId, CardCoordinateDataOutput coreModel)
        {
            var cardCoordinateResponse = new CardCoordinateResponse();

            if (coreModel?.Values != null)
            {
                var cardResponseList = new List<CardResponseValues>();

                foreach (var parent in coreModel.Values)
                {
                    if (parent.CoordinatesOutput != null)
                    {
                        var coordinatesList = new List<Coordinates>();

                        foreach (var row in parent.CoordinatesOutput)
                        {
                            var coordinate = new Coordinates
                            {
                                X = row.X,
                                Y = row.Y,
                            };
                            coordinatesList.Add(coordinate);
                        }

                        var cardResponseValue = new CardResponseValues
                        {
                            Id = parent.Id,
                            Name = parent.Name,
                            Coordinates = coordinatesList,
                        };

                        cardResponseList.Add(cardResponseValue);
                    }

                    cardCoordinateResponse.Values = cardResponseList;
                    cardCoordinateResponse.Id = correlationId;
                    cardCoordinateResponse.DateCreated = coreModel.DateCreated;
                }
            }

            return cardCoordinateResponse;
        }

    }
}
