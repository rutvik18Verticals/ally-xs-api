using System;
using System.Linq;
using Theta.XSPOC.Apex.Api.Contracts.Responses;
using Theta.XSPOC.Apex.Api.Core.Models.Outputs;

namespace Theta.XSPOC.Apex.Api.Contracts.Mappers
{
    /// <summary>
    /// Maps Core.Models.Outputs.NotificationTypesData to NotificationTypesResponse object.
    /// </summary>
    public static class NotificationTypesDataMapper
    {

        /// <summary>
        /// Maps the <seealso cref="NotificationTypesDataOutput"/> core model to
        /// <seealso cref="NotificationTypesResponse"/> object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="coreModel">The <seealso cref="NotificationTypesDataOutput"/> object</param>
        /// <returns>The <seealso cref="NotificationTypesResponse"/></returns>
        public static NotificationTypesResponse Map(string correlationId, NotificationTypesDataOutput coreModel)
        {
            if (coreModel?.Values == null)
            {
                return null;
            }

            var values = coreModel.Values.Select(x => new Responses.Values.NotificationTypesData()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();

            return new NotificationTypesResponse()
            {
                DateCreated = DateTime.UtcNow,
                Id = correlationId,
                Values = values,
            };
        }

    }
}
