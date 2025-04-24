using System.Net.Http;
using System.Threading.Tasks;
using Theta.XSPOC.Apex.Api.Core.Models;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Utility http methods for making calls to Connexia platform.
    /// </summary>
    public interface IHttpClientHelperService
    {

        /// <summary>
        /// Post Connexia auth data, asynchronously.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        Task<StatusHelperModel> PostCNXAuthDataAsync(string url, string json);

        /// <summary>
        /// Validates connexia connection.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> ValidateCNXAsync(string url, string token);
    }
}
