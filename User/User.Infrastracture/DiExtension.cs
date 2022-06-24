using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace User.Infrastracture.Extensions
{
    public static class Extensions
    {
        public static void AddUserContext(this IServiceCollection services, IConfiguration configuration)
        {
            var cnn = configuration.GetConnectionString("KeycloakDatabase");
            services.AddDbContext<UserContext>(opt => opt.UseNpgsql(cnn));
        }
    }
}
