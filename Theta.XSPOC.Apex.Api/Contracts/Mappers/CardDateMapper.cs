using System;
using System.Linq;
using Theta.XSPOC.Apex.Api.Contracts.Responses;
using Theta.XSPOC.Apex.Api.Contracts.Responses.Values;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;

namespace Theta.XSPOC.Apex.Api.Contracts.Mappers
{
    /// <summary>
    /// Map card date response model.
    /// </summary>
    public static class CardDateMapper
    {

        /// <summary>
        /// Maps the core layer output to the card date response.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="coreModel">The core model.</param>
        /// <returns>A <seealso cref="CardDateResponse"/> representing the provided <paramref name="coreModel"/> 
        /// in the domain.</returns>
        public static CardDateResponse Map(string correlationId, CardDatesOutput coreModel)
        {
            if (coreModel?.Values != null)
            {
                var values = coreModel.Values.Select(x => new CardDateValue()
                {
                    CardTypeId = x.CardTypeId,
                    CardTypeName = x.CardTypeName,
                    Date = x.Date,
                }).ToList();

                return new CardDateResponse()
                {
                    DateCreated = DateTime.UtcNow,
                    Id = correlationId,
                    Values = values,
                };
            }

            return new CardDateResponse()
            {
                DateCreated = DateTime.UtcNow,
                Id = correlationId,
                Values = null,
            };
        }

    }
}
