using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Models;
using Theta.XSPOC.Apex.Api.Core.Models.JWTToken;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// The cnx token validation implementation.
    /// </summary>
    public class CNXTokenValidation : ITokenValidation
    {

        private readonly IHttpClientHelperService _httpClientHelperService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CNXTokenValidation"/>.
        /// </summary>
        /// <param name="httpClientHelperService">The http client helper service.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CNXTokenValidation(IHttpClientHelperService httpClientHelperService)
        {
            _httpClientHelperService = httpClientHelperService ?? throw new ArgumentNullException(nameof(httpClientHelperService));
        }

        /// <summary>
        /// The ref token to validate.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="refToken">The ref token.</param>
        /// <returns></returns>
        public async Task<string> ValidateToken(HttpResponse response, string refToken)
        {
            if (response == null)
            {
                return string.Empty;
            }

            var parameter = new Parameters()
            {
                GrantType = "External",
                RefreshToken = string.Empty,
                Reference = refToken,
            };

            var data = JsonConvert.SerializeObject(parameter);

            var token = await _httpClientHelperService.PostCNXAuthDataAsync("api/CNXAccount/GetJWTTokenFromRef", data);

            if (string.IsNullOrEmpty(token.Message) == true)
            {
                return string.Empty;
            }

            var jwtToken = JsonConvert.DeserializeObject<JWTAccessToken>(token.Message);

            var tokenValidation = await _httpClientHelperService.ValidateCNXAsync("api/CNXAccount/ValidateToken", jwtToken?.AccessToken);

            if (tokenValidation.IsSuccessStatusCode)
            {
                string jsonData = await tokenValidation.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<bool>(jsonData);
                if (result)
                {
                    response.Cookies.Append("CNX-Authorization", jwtToken.AccessToken, new CookieOptions() { HttpOnly = true, Secure = false });
                    return token.Message;
                }
            }

            return string.Empty;
        }

    }
}
