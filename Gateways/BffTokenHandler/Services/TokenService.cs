using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BffTokenHandler.Services
{
    public class TokenService : ITokenService
    {
        IConfiguration _configuration { get; set; }
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> RefreshTokenAsync(AuthenticationTicket cookie)
        {
            var tokenExpiresAt = cookie.Properties.GetTokenValue("expires_at");
            var accessTokenExpiration = DateTimeOffset.Parse(tokenExpiresAt);
            var timeRemaining = accessTokenExpiration.Subtract(DateTimeOffset.UtcNow);

            var refreshThreshold = TimeSpan.FromMinutes(_configuration.GetValue<int>("OpenID:RefreshThresholdMinutes"));

            if (timeRemaining < refreshThreshold)
            {
                var refreshToken = cookie.Properties.GetTokenValue("refresh_token");

                var response = await new HttpClient().RequestRefreshTokenAsync(new RefreshTokenRequest
                {
                    Address = _configuration["OpenID:TokenEndpoint"],
                    ClientId = _configuration["OpenID:ClientID"],
                    ClientSecret = _configuration["OpenID:Secret"],
                    RefreshToken = refreshToken
                });

                if (response.IsError)
                {
                    return "";
                }

                var expiresInSeconds = response.ExpiresIn;
                var updatedExpiresAt = DateTimeOffset.UtcNow.AddSeconds(expiresInSeconds);
                cookie.Properties.UpdateTokenValue("expires_at", updatedExpiresAt.ToString());
                cookie.Properties.UpdateTokenValue("access_token", response.AccessToken);
                cookie.Properties.UpdateTokenValue("refresh_token", response.RefreshToken);

                return response.AccessToken;
            }
            return cookie.Properties.GetTokenValue("access_token");
        }

        public async Task<string> GetTokenAsync(AuthenticationTicket cookie)
        {
            return cookie.Properties.GetTokenValue("access_token");
        }
    }
}
