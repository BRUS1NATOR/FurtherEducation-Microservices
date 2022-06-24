using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Profile.Infrastracture.Context;

namespace Profile.Infrastracture
{
    public class ProfileMigrationService : IHostedService
    {
        private IServiceProvider services;
        public ProfileMigrationService(IServiceProvider services)
        {
            this.services = services;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<ProfileContext>();

            var db = context.Database;
            var migrated = db.GetAppliedMigrations();

            if (db.GetPendingMigrations().Any())
            {
                await db.MigrateAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}