using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profile.Infrastracture.Context;

namespace Profile.Infrastracture.Extensions
{
    public static class Extensions
    {
        public static void AddProfileContext(this IServiceCollection services, IConfiguration configuration)
        {
            var cnn = configuration.GetConnectionString("Database");
            services.AddDbContext<ProfileContext>(opt => opt.UseNpgsql(cnn));
        }
    }
}
