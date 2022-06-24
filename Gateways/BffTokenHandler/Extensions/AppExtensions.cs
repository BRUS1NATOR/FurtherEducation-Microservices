using Microsoft.AspNetCore.Builder;

namespace BffTokenHandler.Extensions
{
    public static class AppExtensions
    {
        public static IApplicationBuilder GetTokenFromCookie(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenHandlerMiddleware>();
        }
    }
}
