using Microsoft.AspNetCore.Authentication;
using System.Threading.Tasks;

namespace BffTokenHandler.Services
{
    public interface ITokenService
    {
        public Task<string> RefreshTokenAsync(AuthenticationTicket cookie);
        public Task<string> GetTokenAsync(AuthenticationTicket cookie);
    }
}