using System;
using Theta.XSPOC.Apex.Api.Contracts.Responses;
using domain = Theta.XSPOC.Apex.Api.Core.Models.Outputs;

namespace Theta.XSPOC.Apex.Api.Contracts.Mappers
{
    /// <summary>
    /// Maps asset data between domain and api layer.
    /// </summary>
    public static class AppSettingsMapper
    {

        /// <summary>
        /// Maps data from AppplicationSettingsOutput to AppSettingsResponse.
        /// </summary>
        /// <param name="domainModel">The model from the domain layer.</param>
        /// <param name="correlationId">The correlation id.</param>
        /// <returns>A mapped <see cref="AppSettingsResponse"/> for the api layer.</returns>
        public static AppSettingsResponse Map(domain.AppplicationSettingsOutput domainModel, 
            string correlationId)
        {
            if (domainModel == null)
            {
                return null;
            }

            var response = new AppSettingsResponse();

            response.Values = domainModel.ApplicationSettings;
            response.DateCreated = DateTime.UtcNow;
            response.Id = correlationId;

            return response;
        }

    }
}
