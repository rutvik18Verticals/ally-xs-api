using System;
using System.Linq;
using Theta.XSPOC.Apex.Api.Contracts.Responses;
using Theta.XSPOC.Apex.Api.Contracts.Responses.Values;

namespace Theta.XSPOC.Apex.Api.Contracts.Mappers
{
    /// <summary>
    /// Maps Core.Models.Outputs.AvailableView to AvailableViewResponse object
    /// </summary>
    public static class AvailableViewDataMapper
    {

        /// <summary>
        /// Maps the <seealso cref="Core.Models.Outputs.AvailableViewOutput"/> core model to
        /// <seealso cref="NotificationResponse"/> object.
        /// </summary>
        /// <param name="correlationId">The correlation id.</param>
        /// <param name="coreModel">The <seealso cref="Core.Models.Outputs.AvailableViewOutput"/> object</param>
        /// <returns>The <seealso cref="NotificationResponse"/></returns>
        public static AvailableViewResponse Map(string correlationId, Core.Models.Outputs.AvailableViewOutput coreModel)
        {
            if (coreModel?.Values == null)
            {
                return null;
            }

            var values = coreModel.Values.Select(x => new AvailableViewData()
            {
                Id = x.ViewId,
                Name = x.ViewName,
                IsSelectedView = x.IsSelectedView
            }).ToList();

            return new AvailableViewResponse()
            {
                DateCreated = DateTime.UtcNow,
                Id = correlationId,
                Values = values
            };
        }

    }
}
