using BffTokenHandler.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text;
using System.Threading.Tasks;

namespace BffTokenHandler.Extensions
{
    public class TokenHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenService _tokenService;
        private readonly IOptionsMonitor<CookieAuthenticationOptions> _cookieAuthenticationOptions;

        public TokenHandlerMiddleware(RequestDelegate next, ITokenService tokenService, IOptionsMonitor<CookieAuthenticationOptions> cookieAuthenticationOptions)
        {
            _next = next;
            _tokenService = tokenService;
            _cookieAuthenticationOptions = cookieAuthenticationOptions;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var options = _cookieAuthenticationOptions.Get("Cookies");

            var cookie = options.CookieManager.GetRequestCookie(httpContext, options.Cookie.Name);
            if (cookie is not null)
            {
                //Обновления cookie если оно истекло
                var accessToken = await _tokenService.RefreshTokenAsync(options.TicketDataFormat.Unprotect(cookie));

                var bearerToken = new StringBuilder().Append("Bearer ").Append(accessToken).ToString();

                if (httpContext.Request.Headers.ContainsKey("Authorization"))
                {
                    httpContext.Request.Headers["Authorization"] = bearerToken;
                }
                else
                {
                    httpContext.Request.Headers.Add("Authorization", bearerToken);
                }
            }

            await _next.Invoke(httpContext);
        }

        public static AuthenticationTicket DecryptAuthCookie(HttpContext httpContext)
        {
            // ONE - grab the CookieAuthenticationOptions instance
            var options = httpContext.RequestServices
                .GetRequiredService<IOptionsMonitor<CookieAuthenticationOptions>>()
                .Get(CookieAuthenticationDefaults.AuthenticationScheme); //or use .Get("Cookies")

            // TWO - Get the encrypted cookie value
            var cookie = options.CookieManager.GetRequestCookie(httpContext, options.Cookie.Name);

            if (cookie is null)
            {
                return null;
            }
            // THREE - decrypt it
            return options.TicketDataFormat.Unprotect(cookie);
        }
    }
}
