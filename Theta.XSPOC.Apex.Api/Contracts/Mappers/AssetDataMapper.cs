using Theta.XSPOC.Apex.Api.Contracts.Responses;
using domain = Theta.XSPOC.Apex.Api.Core.Models.Outputs;

namespace Theta.XSPOC.Apex.Api.Contracts.Mappers
{
    /// <summary>
    /// Maps asset data between domain and api layer.
    /// </summary>
    public static class AssetDataMapper
    {

        /// <summary>
        /// Maps data from a well enabled status output to an api response model.
        /// </summary>
        /// <param name="domainModel">The model from the domain layer.</param>
        /// <returns>A mapped <see cref="GetEnabledStatusResponse"/> for the api layer.</returns>
        public static GetEnabledStatusResponse Map(domain.WellEnabledStatusOutput domainModel)
        {
            if (domainModel == null)
            {
                return null;
            }

            var response = new GetEnabledStatusResponse()
            {
                Enabled = domainModel.Enabled,
            };

            return response;
        }

    }
}
