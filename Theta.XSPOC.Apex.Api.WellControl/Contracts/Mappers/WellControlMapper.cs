using System.Collections.Generic;
using Theta.XSPOC.Apex.Api.WellControl.Contracts.Responses;
using Theta.XSPOC.Apex.Api.WellControl.Contracts.Responses.Values;
using domain = Theta.XSPOC.Apex.Api.Core.Models.Outputs;

namespace Theta.XSPOC.Apex.Api.WellControl.Contracts.Mappers
{
    /// <summary>
    /// Maps well control data between domain and web api layer.
    /// </summary>
    public static class WellControlMapper
    {

        /// <summary>
        /// Maps data from a get well control action domain model to api response model.
        /// </summary>
        /// <param name="domainModel">The model from the domain layer.</param>
        /// <returns>A mapped <see cref="GetWellControlActionsResponse"/> for the api layer.</returns>
        public static GetWellControlActionsResponse Map(domain.GetWellControlActionsOutput domainModel)
        {
            var response = new GetWellControlActionsResponse()
            {
                WellControlActions = new List<WellControlAction>(),
                CanConfigWell = domainModel?.CanConfigWell ?? false,
            };

            foreach (var controlAction in domainModel?.WellControlActions)
            {
                response.WellControlActions.Add(new WellControlAction()
                {
                    Id = controlAction.Id,
                    Name = controlAction.Name,
                });
            }

            return response;
        }

    }
}
