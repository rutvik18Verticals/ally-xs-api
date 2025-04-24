using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// The http client helper service.
    /// </summary>
    public class HttpClientHelperService : IHttpClientHelperService
    {
        private readonly HttpClient _httpClient;
        private readonly ConnexiaWebBaseUrlSettings _connexiaUrlSettings;

        /// <summary>
        /// The http client helper service.
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="connexiaUrlSettings"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public HttpClientHelperService(HttpClient httpClient,
            IOptions<ConnexiaWebBaseUrlSettings> connexiaUrlSettings)
        {
            this._httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            if (connexiaUrlSettings == null)
            {
                throw new ArgumentNullException(nameof(connexiaUrlSettings));
            }

            this._connexiaUrlSettings = connexiaUrlSettings.Value ?? throw new ArgumentNullException(nameof(connexiaUrlSettings));
        }

        /// <summary>
        /// Post Connexia auth data, asynchronously.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public async Task<StatusHelperModel> PostCNXAuthDataAsync(string url, string json)
        {
            var baseUrl = _connexiaUrlSettings.ConnexiaBaseURL;

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(baseUrl + url),
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            request.Headers.Add(_connexiaUrlSettings.ConnexiaAPIKey, _connexiaUrlSettings.ConnexiaAPIKeyValue);

            var response = await _httpClient.SendAsync(request);
            StatusHelperModel data = new StatusHelperModel();

            if (response.IsSuccessStatusCode)
            {
                string jsonData = await response.Content.ReadAsStringAsync();
                data.Message = jsonData;
                data.Status = response.StatusCode.ToString();
            }
            else
            {
                data.Id = 0;
                data.Message = "";
                data.Status = "";
            }

            return data;
        }

        /// <summary>
        /// Validates connexia connection.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> ValidateCNXAsync(string url, string token)
        {
            var baseUrl = _connexiaUrlSettings.ConnexiaBaseURL;

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(baseUrl + url)
            };

            request.Headers.Add(_connexiaUrlSettings.ConnexiaAPIKey, _connexiaUrlSettings.ConnexiaAPIKeyValue);
            request.Headers.Add("Authorization", "bearer " + token);

            var response = await _httpClient.SendAsync(request);

            return response;
        }

    }
}
