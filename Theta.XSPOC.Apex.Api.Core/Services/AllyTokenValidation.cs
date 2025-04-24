using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Theta.XSPOC.Apex.Api.Core.Models.JWTToken;
using Theta.XSPOC.Apex.Api.Core.Models.Configuration;
using Theta.XSPOC.Apex.Api.Core.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// The Ally token validation implementation.
    /// </summary>
    public class AllyTokenValidation : ITokenValidation
    {

        private readonly HttpClient _httpClient;
        private readonly AppSettings _appSettings;

        /// <summary>
        /// Initializes a new instance of <see cref="AllyTokenValidation"/>.
        /// </summary>
        /// <param name="httpClient">The http client.</param>
        /// <param name="appSettings">The appsettings.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public AllyTokenValidation(HttpClient httpClient, IOptionsSnapshot<AppSettings> appSettings)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
        }

        /// <summary>
        /// The ref token to validate.
        /// </summary>
        /// <param name="httpResponse">The http response.</param>
        /// <param name="refToken">The ref token.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<string> ValidateToken(HttpResponse httpResponse, string refToken)
        {
            var parameters = new Parameters()
            {
                GrantType = "reference",
                Reference = refToken,
                RefreshToken = string.Empty,
            };

            var postData = JsonConvert.SerializeObject(parameters);

            var content = new StringContent(postData, Encoding.UTF8, "application/json");

            var baseApiUrl = _appSettings.AllyConnectApiURL;
            var builder = new UriBuilder($"{baseApiUrl}/allyapi/AllyAccount/GetJWTTokenFromRef");
            var query = HttpUtility.ParseQueryString(builder.Query);
            builder.Query = query.ToString();
            var url = builder.ToString();
            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var jwtToken = JsonConvert.DeserializeObject<JWTAccessToken>(responseContent);

                builder = new UriBuilder($"{baseApiUrl}/allyapi/AllyAccount/ValidateToken");
                query = HttpUtility.ParseQueryString(builder.Query);
                builder.Query = query.ToString();
                url = builder.ToString();

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken.AccessToken);

                response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent2 = await response.Content.ReadAsStringAsync();
                    var isValid = JsonConvert.DeserializeObject<bool>(responseContent2);

                    if (isValid)
                    {
                        return responseContent;
                    }
                }
            }

            return string.Empty;
        }

    }
}
