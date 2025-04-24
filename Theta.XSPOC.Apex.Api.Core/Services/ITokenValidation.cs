using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Theta.XSPOC.Apex.Api.Core.Services
{
    /// <summary>
    /// Interface to define token validation methods.
    /// </summary>
    public interface ITokenValidation
    {

        /// <summary>
        /// Validates a token.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="refToken">The ref token.</param>
        /// <returns></returns>
        Task<string> ValidateToken(HttpResponse response, string refToken);

    }
}
