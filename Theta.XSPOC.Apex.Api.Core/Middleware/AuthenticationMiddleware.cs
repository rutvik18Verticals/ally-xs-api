using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Theta.XSPOC.Apex.Api.Core.Middleware
{
    /// <summary>
    /// Authentication middleware to use cooke to add to authorization header bearer token.
    /// </summary>
    public class AuthenticationMiddleware : IMiddleware
    {

        /// <summary>
        /// The constructor for authentication middleware.
        /// </summary>
        public AuthenticationMiddleware()
        {
        }

        /// <summary>
        /// Invoked on all http calls.
        /// </summary>
        /// <param name="context">The current http context.</param>
        /// <param name="next">The nex request delegate.</param>
        /// <returns>An awaitable task.</returns>
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (context?.Request?.Cookies == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
            if (next == null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
            var authCookie = context.Request.Cookies["Ally-Authorization"];
            if (!context.Request.Headers.ContainsKey("Authorization") && authCookie != null)
            {
                context.Request.Headers.TryAdd("Authorization", $"Bearer {authCookie}");
                await next.Invoke(context);
            }
            else
            {
                await next.Invoke(context);
            }
        }

    }
}